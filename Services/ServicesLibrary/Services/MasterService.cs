using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Repositories;
using WaveTech.Scutex.Model.Interfaces.Wcf;

namespace WaveTech.Scutex.WcfServices.ServicesLibrary.Services
{
	public class MasterService : IMasterService
	{
		private readonly ICommonRepository _commonRepository;

		public MasterService(ICommonRepository commonRepository)
		{
			_commonRepository = commonRepository;
		}

		public MasterServiceData GetMasterServiceData()
		{
			return _commonRepository.GetMasterServiceData();
		}

		public MasterServiceData SetMasterServiceData(MasterServiceData data)
		{
			return _commonRepository.SetMasterServiceData(data);
		}
	}
}