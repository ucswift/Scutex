using System;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Model.Interfaces.Wcf;
using WaveTech.Scutex.Model.Results;
using WaveTech.Scutex.WcfServices.ServicesLibrary.Interfaces.Management;

namespace WaveTech.Scutex.WcfServices.ServicesLibrary.Services.Management
{
	public class ReportingService : IReportingService
	{
		#region Private Readonly Members
		private readonly IObjectSerializationProvider _serializationProvider;
		private readonly IControlService _controlService;
		private readonly IProductManagementService _productManagementService;
		private readonly IMasterService _masterService;
		private readonly IActivationLogService _activationLogService;
		private readonly IKeyManagementService _keyManagementService;
		#endregion Private Readonly Members

		#region Constructors
		public ReportingService()
			: this(ObjectLocator.GetInstance<IObjectSerializationProvider>(), ObjectLocator.GetInstance<IControlService>(),
			ObjectLocator.GetInstance<IProductManagementService>(), ObjectLocator.GetInstance<IMasterService>(), ObjectLocator.GetInstance<IActivationLogService>(), ObjectLocator.GetInstance<IKeyManagementService>())
		{

		}

		public ReportingService(IObjectSerializationProvider serializationProvider, IControlService controlService,
			IProductManagementService productManagementService, IMasterService masterService, IActivationLogService activationLogService, IKeyManagementService keyManagementService)
		{
			_serializationProvider = serializationProvider;
			_controlService = controlService;
			_productManagementService = productManagementService;
			_masterService = masterService;
			_activationLogService = activationLogService;
			_keyManagementService = keyManagementService;
		}
		#endregion Constructors

		public string GetAllActivationLogs(string token)
		{
			GetAllActivationLogsResult result = new GetAllActivationLogsResult();

			try
			{
				if (!_controlService.ValidateManagementToken(token))
				{
					result.WasOperationSuccessful = false;
					result.WasRequestValid = false;

					return _controlService.EncryptSymmetricResponse(_serializationProvider.Serialize(result));
				}

				result.WasRequestValid = true;
				result.ActivationLogs = _activationLogService.GetAllActivationLogs();

				result.WasOperationSuccessful = true;
			}
			catch (Exception ex)
			{
				result.WasException = true;
				result.ExceptionMessage = ex.Message; // TODO: Must be modified to hide important data before release
			}

			return _controlService.SerializeAndEncryptMgmtOutboundData(result);
		}

		public string GetAllLicenseActivations(string token)
		{
			GetAllLicenseActivationsResult result = new GetAllLicenseActivationsResult();

			try
			{
				if (!_controlService.ValidateManagementToken(token))
				{
					result.WasOperationSuccessful = false;
					result.WasRequestValid = false;

					return _controlService.EncryptSymmetricResponse(_serializationProvider.Serialize(result));
				}

				result.WasRequestValid = true;
				result.LicenseActivations = _keyManagementService.GetAllLicenseActivations();

				result.WasOperationSuccessful = true;
			}
			catch (Exception ex)
			{
				result.WasException = true;
				result.ExceptionMessage = ex.Message; // TODO: Must be modified to hide important data before release
			}

			return _controlService.SerializeAndEncryptMgmtOutboundData(result);
		}
	}
}