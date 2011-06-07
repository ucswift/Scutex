namespace WaveTech.Scutex.Model.Interfaces.Providers
{
	public interface INumberDataGeneratorProvider
	{
		int GenerateRandomNumber();
		int GenerateRandomNumber(int min, int max);
	}
}