namespace WaveTech.Scutex.Model.Interfaces.Services
{
	public interface IPackingService
	{
		string PackToken(Token token);
		Token UnpackToken(string data);
	}
}