namespace WaveTech.Scutex.Model.Interfaces.Providers
{
	public interface IWmiDataProvider
	{
		string GetProcessorData();
		string GetMotherboardData();
		string GetBiosData();
	}
}