using System;

namespace WaveTech.Scutex.Model.Results
{
	public class ActivationResult : BaseServiceResult
	{
		public Guid ServiceId { get; set; }
		public Guid? ActivationToken { get; set; }
		public bool ActivationSuccessful { get; set; }
	}
}