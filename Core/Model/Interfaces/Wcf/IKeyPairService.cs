namespace WaveTech.Scutex.Model.Interfaces.Wcf
{
	public interface IKeyPairService
	{
		KeyPair GetClientKeyPair();
		KeyPair GetManagementKeyPair();
		string GetInboundHalfFromFile();
		string GetOutboundHalfFromFile();
		string GetTokenFromFile();
	}
}
