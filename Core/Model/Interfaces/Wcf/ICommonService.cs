namespace WaveTech.Scutex.Model.Interfaces.Wcf
{
	public interface ICommonService
	{
		string GetPath();
		string GetPath(string relativePath);
	}
}