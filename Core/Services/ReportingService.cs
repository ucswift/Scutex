
using System.Collections.Generic;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Model.Interfaces.Services;
using WaveTech.Scutex.Model.Results;
using WaveTech.Scutex.Services.Properties;

namespace WaveTech.Scutex.Services
{
	internal class ReportingService : IReportingService
	{
		private readonly IReportingProvider _reportingProvider;
		private readonly IPackingService _packingService;

		public ReportingService(IReportingProvider reportingProvider, IPackingService packingService)
		{
			_reportingProvider = reportingProvider;
			_packingService = packingService;
		}

		public List<ActivationLog> GetAllServiceActivationLogs(Service service)
		{
			string mgmtToken = _packingService.PackToken(service.GetManagementToken());

			GetAllActivationLogsResult result = _reportingProvider.GetAllServiceActivationLogs(service.ManagementUrl, mgmtToken,
																														 GetManagementStandardEncryptionInfo(service),
																														 service.GetManagementServiceKeyPair());

			if (IsResultValid(result))
				return result.ActivationLogs;

			return new List<ActivationLog>();
		}

		public List<LicenseActivation> GetAllServiceLicenseActivations(Service service)
		{
			string mgmtToken = _packingService.PackToken(service.GetManagementToken());

			GetAllLicenseActivationsResult result = _reportingProvider.GetAllServiceLicenseActivations(service.ManagementUrl, mgmtToken,
																														 GetManagementStandardEncryptionInfo(service),
																														 service.GetManagementServiceKeyPair());

			if (IsResultValid(result))
				return result.LicenseActivations;

			return new List<LicenseActivation>();
		}

		#region Private Methods
		private EncryptionInfo GetManagementStandardEncryptionInfo(Service service)
		{
			EncryptionInfo ei = new EncryptionInfo();
			ei.HashAlgorithm = "SHA1";
			ei.InitVector = Resources.ServicesIV;
			ei.Iterations = 2;
			ei.KeySize = 192;
			ei.PassPhrase = service.GetManagementOutboundKeyPart2();
			ei.SaltValue = service.GetManagementInboundKeyPart2();

			return ei;
		}

		private bool IsResultValid(BaseServiceResult result)
		{
			if (result.WasException)
				return false;

			if (result.WasRequestValid == false)
				return false;

			if (result.WasOperationSuccessful == false)
				return false;

			return true;
		}
		#endregion Private Methods
	}
}