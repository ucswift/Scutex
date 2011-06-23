using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Licensing.Gui;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Exceptions;
using WaveTech.Scutex.Model.Interfaces.Applications;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Model.Interfaces.Services;

namespace WaveTech.Scutex.Licensing
{
	/// <summary>
	/// The Licensing Manager is the primary interface and interaction
	/// point for a product to communicate with the Scutex Licensing
	/// System.
	/// </summary>
	public class LicensingManager : ILicensingManager
	{
		#region Private Members
		private string publicKey;
		private string dllCheck;
		private object instance;
		private LicensingManagerOptions _options;
		//private bool assemblyVerified;
		#endregion Private Members

		#region Private Readonly Members
		private readonly IHashingProvider hashingProvider;
		private readonly IEncodingService encodingService;
		private readonly ILicenseManagerService licenseManagerService;
		private readonly IClientLicenseService _clientLicenseService;
		private readonly IComBypassProvider _comBypassProvider;
		private readonly IRegisterService _registerService;
		#endregion Private Readonly Members

		#region Internal Properties
		internal static ScutexLicense FormLicense { get; set; }
		internal static ClientLicense ClientLicense { get; set; }
		internal static TrialInterfaceInteraction InterfaceInteraction { get; set; }
		#endregion Internal Properties

		#region Constructors
		/// <summary>
		/// Default instantiation of the Licensing Manager
		/// </summary>
		/// <remarks>
		/// In a normal client side .Net application this should
		/// be the constructor used. If your application is called
		/// via another application which is housing everything, 
		/// use the other constructor.
		/// </remarks>
		public LicensingManager()
			: this(null)
		{

		}

		/// <summary>
		/// Override constructor to instantiate the Licensing Manager and
		/// point it to an object that it's the running executable.
		/// </summary>
		/// <remarks>
		/// This constructor is useful when running an application and calling
		/// Scutex via a CCW (COM Callable Wrapper) and the executing assembly
		/// (i.e. Access) does not contain the security attribute.
		/// </remarks>
		/// <param name="instance">
		/// Reference to an object were the required attributes can be located
		/// </param>
		public LicensingManager(object instance)
		{
			this.instance = instance;

			Bootstrapper.Configure();

			InterfaceInteraction = new TrialInterfaceInteraction();

			hashingProvider = ObjectLocator.GetInstance<IHashingProvider>();
			encodingService = ObjectLocator.GetInstance<IEncodingService>();
			licenseManagerService = ObjectLocator.GetInstance<ILicenseManagerService>();
			_clientLicenseService = ObjectLocator.GetInstance<IClientLicenseService>();
			_comBypassProvider = ObjectLocator.GetInstance<IComBypassProvider>();
			_registerService = ObjectLocator.GetInstance<IRegisterService>();

			SetCriticalVerificationData();
			VerifyLicensingAssembly();
		}

		/// <summary>
		/// Override constructor to instantiate the Licensing Manager and
		/// set options for its use.
		/// </summary>
		/// <remarks>
		/// This constructor is useful when calling Scutex from an instance
		/// where there is no running application or objects that can be 
		/// used to run a attribute check. For example a .Net Component or
		/// Control.
		/// </remarks>
		/// <param name="options">
		/// Options to override the default Licensing Manager behavior
		/// </param>
		public LicensingManager(LicensingManagerOptions options)
		{
			_options = options;
		}

		#endregion Constructors

		#region Public Methods
		/// <summary>
		/// Validates the software trial/demo/license status. This method will
		/// determine the status and will either preset the user with that
		/// information or just log it.
		/// </summary>
		/// <remarks>
		/// If the state of Scutex cannot be verified, for example the 
		/// data file or Licensing assembly was tampered with, this method 
		/// will warn the user and kill the entire process.
		/// </remarks>
		/// <param name="interactionMode">
		/// Determines how, or if, Scutex will report critical information 
		/// to the user.
		/// </param>
		public ScutexLicense Validate(InteractionModes interactionMode)
		{
			ScutexLicense scutexLicense = licenseManagerService.GetScutexLicense();

			if (scutexLicense == null)
				throw new ScutexAuditException();

			if (scutexLicense.IsLicensed == false)
			{
				switch (interactionMode)
				{
					case InteractionModes.None:
						IEventLogInteractionService eventLogInteractionService = ObjectLocator.GetInstance<IEventLogInteractionService>();
						eventLogInteractionService.Validate(scutexLicense);

						break;
					case InteractionModes.Gui:
						ShowLicensingWindow(scutexLicense);

						if (FormLicense != null)
							scutexLicense = FormLicense;

						if (ClientLicense != null)
						{
							scutexLicense.IsLicensed = ClientLicense.IsLicensed;
							scutexLicense.IsActivated = ClientLicense.IsActivated;
							scutexLicense.ActivatedOn = ClientLicense.ActivatedOn;
						}

						break;
					case InteractionModes.Console:
						IConsoleInteractionService consoleInteractionService = ObjectLocator.GetInstance<IConsoleInteractionService>();
						consoleInteractionService.Validate(scutexLicense);

						break;
					case InteractionModes.Silent:
						break;
				}
			}

			scutexLicense.InterfaceInteraction = InterfaceInteraction;

			return scutexLicense;
		}

		/// <summary>
		/// Validates the software trial/demo/license status. This method will
		/// determine the status and will either preset the user with that
		/// information or just log it.
		/// 
		/// The Ex Validate method is an external integration point for programs
		/// that may not be able to consume complex .Net object's. This should only
		/// be used when you cannot use the returned model object of the normal 
		/// Validate method.
		/// </summary>
		/// <remarks>
		/// Unlike the non-Ex Validate method this one won't kill the program.
		/// The first index of the returned array is an ok check, if it's false
		/// the system failed required security checks and should be aborted.
		/// </remarks>
		/// <param name="interactionMode"></param>
		/// <returns></returns>
		public object[] ValidateEx(int interactionMode)
		{
			List<object> data = new List<object>();

			ScutexLicense sl = Validate((InteractionModes)interactionMode);

			data.Add(true);
			data.Add(sl.IsTrialExpired);
			data.Add(sl.IsTrialValid);
			data.Add(sl.TrialFaultReason);
			data.Add(sl.IsLicensed);
			data.Add(sl.IsActivated);

			return data.ToArray();
		}

		/// <summary>
		/// Attempts to register the software and turn the version into a full
		/// version. This method may call remote web services, or authenticate
		/// locally ('offline') depending on the settings.
		/// </summary>
		/// <remarks>
		/// If the state of Scutex cannot be verified, for example the 
		/// data file or Licensing assembly was tampered with, this method 
		/// will warn the user and kill the entire process.
		/// </remarks>
		/// <param name="licenseKey">
		/// Plain text license key to attempt to register
		/// </param>
		public ScutexLicense Register(string licenseKey)
		{
			if (String.IsNullOrEmpty(licenseKey))
				throw new ScutexAuditException();

			ScutexLicense scutexLicense = licenseManagerService.GetScutexLicense();

			RegisterResult result = _registerService.Register(licenseKey, _clientLicenseService.GetClientLicense(), scutexLicense);

			return result.ScutexLicense;
		}

		/// <summary>
		/// Attempts to register the software and turn the version into a full
		/// version. This method may call remote web services, or authenticate
		/// locally ('offline') depending on the settings.
		/// 
		/// The Ex register method is an external integration point for programs
		/// that may not be able to consume complex .Net object's. This should only
		/// be used when you cannot use the returned model object of the normal Register
		/// method.
		/// </summary>
		/// <remarks>
		/// Unlike the non-Ex register method this one won't kill the program.
		/// The first index of the returned array is an ok check, if it's false
		/// the system failed required security checks and should be aborted.
		/// </remarks>
		/// <param name="licenseKey">Plain text license key to attempt to register</param>
		/// <returns></returns>
		public object[] RegisterEx(string licenseKey)
		{
			List<object> data = new List<object>();

			ScutexLicense sl = Register(licenseKey);

			data.Add(true);
			data.Add(sl.IsLicensed);
			data.Add(sl.IsActivated);
			data.Add(sl.ActivatedOn);

			return data.ToArray();
		}
		#endregion Public Methods

		#region Private Methods
		private void ShowLicensingWindow(ScutexLicense scutexLicense)
		{
			ThreadRunner runner = new ThreadRunner();
			runner.RunInSTA(
				delegate
				{
					LicenseWindow window = new LicenseWindow(this, scutexLicense, _clientLicenseService.GetClientLicense());
					window.Show();
					System.Windows.Threading.Dispatcher.Run();
				});
		}

		private void SetCriticalVerificationData()
		{
			if (_comBypassProvider.IsComBypassEnabled())
				GetAttributeInfoFromTLS();
			else
				GetLicenseAttributeInfo();
		}

		private void GetLicenseAttributeInfo()
		{
			object[] attibutes;
			bool foundAttibute = false;

			if (instance != null)
			{
				Assembly assembly = Assembly.GetAssembly(instance.GetType());
				attibutes = assembly.GetCustomAttributes(true);
			}
			else
			{
				attibutes = Assembly.GetEntryAssembly().GetCustomAttributes(true);
			}

			foreach (object o in attibutes)
			{
				if (o.GetType() == typeof(LicenseAttribute))
				{
					if (String.IsNullOrEmpty(((LicenseAttribute)o).Key) || String.IsNullOrEmpty(((LicenseAttribute)o).Check))
						GetLicenseAttributeInfoFailed();

					try
					{
						publicKey = encodingService.Decode(((LicenseAttribute)o).Key);
						dllCheck = encodingService.Decode(((LicenseAttribute)o).Check);
					}
					catch
					{
						GetLicenseAttributeInfoFailed();
					}

					foundAttibute = true;
					break;
				}
			}

			if (!foundAttibute)
				GetLicenseAttributeInfoFailed();
		}

		private void GetAttributeInfoFromTLS()
		{
			try
			{
				List<string> data = _comBypassProvider.GetComBypass();

				if (!BasicTLSPreDataChecks(data))
					throw new Exception();

				publicKey = encodingService.Decode(data[0]);
				dllCheck = encodingService.Decode(data[1]);

				if (!BasicTLSPostDataChecks(publicKey, dllCheck))
					throw new Exception();
			}
			catch
			{
				GetAttributeDataFromTLSFailed();
				throw new ScutexAuditException();
			}
		}

		private bool BasicTLSPreDataChecks(List<string> data)
		{
			bool isValid = true;

			if (data.Count == 2)
			{
				if (!data[0].Contains("|"))
					isValid = false;

				if (!data[1].Contains("|"))
					isValid = false;

				if (data[0].Length < 150)
					isValid = false;

				if (data[1].Length < 150)
					isValid = false;
			}
			else
				isValid = false;
			

			return isValid;
		}

		private bool BasicTLSPostDataChecks(string publicKey, string dllCheck)
		{
			bool isValid = true;

			if (String.IsNullOrEmpty(publicKey) || String.IsNullOrEmpty(dllCheck))
				isValid = false;
			else
			{
				if (publicKey.Length < 150)
					isValid = false;

				if (dllCheck.Length != 59)
					isValid = false;
			}

			return isValid;
		}

		/// <summary>
		/// This function needs to be one of the first things calls to ensure that
		/// the licensing assembly is valid
		/// </summary>
		private void VerifyLicensingAssembly()
		{
			string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
			path = path.Replace("file:\\", "");

			//string currentFileHash = hashingProvider.HashFile(Directory.GetCurrentDirectory() + "\\WaveTech.Scutex.Licensing.dll");
			string currentFileHash = hashingProvider.HashFile(path + "\\WaveTech.Scutex.Licensing.dll");

#if (!DEBUG)
			// This code is disabled during dev (in debug mode) because the assembly
			// hash will keep on changing, and this will kill the application and any tests.
			if (currentFileHash != dllCheck)
			{
				MessageBox.Show("Scutex Licensing\r\n\r\n" + "If you are getting this error message Scutex Licensing was unable to \r\n" + "verify the licensing DLL. This means that the DLL might have been\r\n" + "corrupted or might have been tampered with.\r\n\r\n" + "To fix this issue you need to either replace the DLL file with the correct file or regenerate the licening data file from the Scutex Licensing Manager.", "Scutex Licensing", MessageBoxButton.OK, MessageBoxImage.Error);
				//Environment.Exit(1001);

				throw new ScutexAuditException();
			}
#endif
			//assemblyVerified = true;
		}

		/// <summary>
		/// Used to display a message box to the user when the assemblies fail verification
		/// </summary>
		private void GetLicenseAttributeInfoFailed()
		{
			MessageBox.Show("Scutex Licensing\r\n\r\n" + "If you are getting this error message you might be missing the\r\n" + "LicensingAttribute in your protected application's AssemblyInfo file.\r\n\r\n" + "You should check to ensure that the LicensingAttribute exists and that\r\n" + "both parameters of the attribute are populated.", "Scutex Licensing", MessageBoxButton.OK, MessageBoxImage.Error);
			Environment.Exit(1002);
		}

		/// <summary>
		/// Used to display a message box to the user when the assemblies fail verification
		/// </summary>
		private void GetAttributeDataFromTLSFailed()
		{
			//MessageBox.Show("Scutex Licensing\r\n\r\n" + "If you are getting this error message your application didn't call into\r\n" + "Scutex with the information required to start the protection process. \r\n\r\n" + "You should check to ensure that the values and parameters are correct and try again.", "Scutex Licensing", MessageBoxButton.OK, MessageBoxImage.Error);
			//Environment.Exit(1003);
		}
		#endregion Private Methods
	}
}