namespace WaveTech.Scutex.Model.Interfaces.Services
{
	public interface IEncodingService
	{
		string Encode(string data);
		string Decode(string data);

		string Encode64(string data);
		string Decode64(string data);
	}
}