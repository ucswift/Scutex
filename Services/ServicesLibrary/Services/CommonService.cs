using System.IO;
using WaveTech.Scutex.Model.Interfaces.Wcf;

namespace WaveTech.Scutex.WcfServices.ServicesLibrary.Services
{
	public class CommonService : ICommonService
	{
		public string GetPath(string relativePath)
		{
			string appPath = GetPath();
			string fullPath = Path.Combine(appPath, relativePath);

			return fullPath;
		}

		public string GetPath()
		{
			return System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
		}
	}
}