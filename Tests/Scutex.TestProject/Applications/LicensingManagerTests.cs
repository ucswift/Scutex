using Microsoft.VisualStudio.TestTools.UnitTesting;
using WaveTech.Scutex.Licensing;
using WaveTech.Scutex.Model;

namespace WaveTech.Scutex.UnitTests.Applications
{
	namespace LicensingManagerTests
	{
		[TestClass]
		public class with_the_licensing_manager
		{
			protected LicensingManager _licensingManager;
			protected LicenseHelper _licenseHelper;

			[TestInitialize]
			public void Before_each_test()
			{
				_licensingManager = new LicensingManager();
				_licenseHelper = new LicenseHelper();

				_licenseHelper.BasicSetup();
			}
		}

		[TestClass]
		public class when_newing_up_the_licensing_manager: with_the_licensing_manager
		{
			[TestMethod]
			public void default_constructor_should_not_throw_an_exception()
			{
				LicensingManager lm = new LicensingManager();
			}

			[TestMethod]
			public void instance_constructor_should_not_throw_an_exception()
			{
				LicensingManager lm = new LicensingManager(this);
			}

			[TestMethod]
			public void options_constructor_should_not_throw_an_exception()
			{
				LicensingManagerOptions options = new LicensingManagerOptions();
				options.DataFileLocation = @"C:\Temp\Scutex\sxu.dll";
				options.DllHash = "123";
				options.PublicKey = "123";

				LicensingManager lm = new LicensingManager(this, options);
			}
		}

		[TestClass]
		public class when_calling_the_validate_method : with_the_licensing_manager
		{
			[TestMethod]
			public void shall_not_throw_an_exception()
			{
				_licensingManager.Validate(InteractionModes.Silent);
			}
		}
	}
}