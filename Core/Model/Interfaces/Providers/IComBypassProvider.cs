
using System.Collections.Generic;

namespace WaveTech.Scutex.Model.Interfaces.Providers
{
	public interface IComBypassProvider
	{
		bool IsComBypassEnabled();
		void SetComBypass(string p1, string p2);
		List<string> GetComBypass();
		void RemoveComBypass();
	}
}