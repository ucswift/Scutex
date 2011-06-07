
using System;
using System.Collections.Generic;
using System.Linq;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Model.Interfaces.Repositories;
using WaveTech.Scutex.Model.Interfaces.Wcf;

namespace WaveTech.Scutex.WcfServices.ServicesLibrary.Services
{
	public class ActivationLogService : IActivationLogService
	{
		private readonly IActivationLogRepoistory _activationLogRepository;
		private readonly IHashingProvider _hashingProvider;

		public ActivationLogService(IActivationLogRepoistory activationLogRepository, IHashingProvider hashingProvider)
		{
			_activationLogRepository = activationLogRepository;
			_hashingProvider = hashingProvider;
		}

		public List<ActivationLog> GetAllActivationLogs()
		{
			return _activationLogRepository.GetAllActivationLogs().ToList();
		}

		public void LogActiviation(string licenseKey, ActivationResults activationResult, string ipAddress)
		{
			ActivationLog log = new ActivationLog();
			log.LicenseKey = _hashingProvider.ComputeHash(licenseKey, "SHA256");
			log.ActivationResult = activationResult;
			log.IpAddress = ipAddress;
			log.Timestamp = DateTime.Now;

			_activationLogRepository.SaveActivationLog(log);
		}
	}
}
