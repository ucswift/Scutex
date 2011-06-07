
using System.Linq;

namespace WaveTech.Scutex.Model.Interfaces.Repositories
{
	public interface IActivationLogRepoistory
	{
		IQueryable<ActivationLog> GetAllActivationLogs();
		void SaveActivationLog(ActivationLog activationLog);
		void DeleteActivationLogByKey(string key);
	}
}