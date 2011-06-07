
using System.Runtime.InteropServices;

namespace ScutexLicensingCCW
{
	[Guid("06D662FC-8977-42B2-ABFC-CD78715C2B81")]
	[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
	[ComVisible(true)]
	public interface _LicensingManager
	{
		bool Prepare(string p1, string p2);
		int Validate(int interactionMode);
		int Register(string key);
	}
}