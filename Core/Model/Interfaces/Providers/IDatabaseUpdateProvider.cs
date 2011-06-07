
namespace WaveTech.Scutex.Model.Interfaces.Providers
{
	public interface IDatabaseUpdateProvider
	{
		bool ApplyBaseDatabaseSchema(string connectionString);
		bool IsDatabaseBasePopulated(string connectionString);
		bool ApplyBaseDatabaseData(string connectionString);
		bool InitializeDatabase(string connectionString);
		bool IsDatabaseVersionCorrect(string connectionString);
	}
}