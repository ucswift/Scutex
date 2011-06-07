namespace WaveTech.Scutex.Model.Interfaces.Providers
{
	public interface IStringDataGeneratorProvider
	{
		string GenerateRandomString(int minLength, int maxLength, bool includeSpecialCharacters, bool includeNumbers);
	}
}