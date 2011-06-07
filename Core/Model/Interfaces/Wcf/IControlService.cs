namespace WaveTech.Scutex.Model.Interfaces.Wcf
{
	public interface IControlService
	{
		EncryptionInfo GetStandardEncryptionInfo();
		bool ValidateClientToken(string token);
		bool ValidateManagementToken(string token);
		string EncryptSymmetricResponse(string data);
		string DecryptSymmetricResponse(string data);
		string SerializeAndEncryptMgmtOutboundData(object o);
		T DeserializeAndDencryptMgmtInboundData<T>(string data);
		string SerializeAndEncryptClientOutboundData(object o);
		T DeserializeAndDencryptClientInboundData<T>(string data);
	}
}