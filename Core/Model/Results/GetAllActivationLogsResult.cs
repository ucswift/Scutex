using System.Collections.Generic;

namespace WaveTech.Scutex.Model.Results
{
	public class GetAllActivationLogsResult : BaseServiceResult
	{
		public List<ActivationLog> ActivationLogs { get; set; }
	}
}