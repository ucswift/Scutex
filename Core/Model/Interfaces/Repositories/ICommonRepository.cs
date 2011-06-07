namespace WaveTech.Scutex.Model.Interfaces.Repositories
{
	public interface ICommonRepository
	{
		MasterServiceData GetMasterServiceData();
		MasterServiceData SetMasterServiceData(MasterServiceData data);
	}
}