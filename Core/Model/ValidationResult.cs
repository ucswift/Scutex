using System.Collections.Generic;

namespace WaveTech.Scutex.Model
{
	public class ValidationResult
	{
		public bool IsValid { get; set; }
		public List<string> ValidationErrors { get; set; }

		public ValidationResult()
		{
			ValidationErrors = new List<string>();
		}
	}
}