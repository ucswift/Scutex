using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Repositories;
using WaveTech.Scutex.Model.Interfaces.Services;

namespace WaveTech.Scutex.Services
{
	internal class ClientLicenseService : IClientLicenseService
	{
		private readonly IClientLicenseRepository _clientLicenseRepository;

		public ClientLicenseService(IClientLicenseRepository clientLicenseRepository)
		{
			_clientLicenseRepository = clientLicenseRepository;
		}

		public ClientLicense GetClientLicense()
		{
			return _clientLicenseRepository.GetClientLicense();
		}

		public ClientLicense GetClientLicense(string filePath)
		{
			return _clientLicenseRepository.GetClientLicense(filePath);
		}

		public ClientLicense SaveClientLicense(ClientLicense clientLicense)
		{
			return _clientLicenseRepository.SaveClientLicense(clientLicense);
		}

		public ClientLicense SaveClientLicense(ClientLicense clientLicense, string filePath)
		{
			return _clientLicenseRepository.SaveClientLicense(clientLicense, filePath);
		}
	}
}