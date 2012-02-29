using System;
using System.Collections.Generic;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Model.Interfaces.Wcf;
using WaveTech.Scutex.Model.Results;
using WaveTech.Scutex.WcfServices.ServicesLibrary.Interfaces.Management;

namespace WaveTech.Scutex.WcfServices.ServicesLibrary.Services.Management
{
	public class StatusService : IStatusService
	{
		#region Private Readonly Members
		private readonly IObjectSerializationProvider _serializationProvider;
		private readonly IControlService _controlService;
		private readonly IProductManagementService _productManagementService;
		private readonly IMasterService _masterService;
		#endregion Private Readonly Members

		#region Constructors
		public StatusService()
			: this(ObjectLocator.GetInstance<IObjectSerializationProvider>(), ObjectLocator.GetInstance<IControlService>(),
			ObjectLocator.GetInstance<IProductManagementService>(), ObjectLocator.GetInstance<IMasterService>())
		{

		}

		public StatusService(IObjectSerializationProvider serializationProvider, IControlService controlService,
			IProductManagementService productManagementService, IMasterService masterService)
		{
			_serializationProvider = serializationProvider;
			_controlService = controlService;
			_productManagementService = productManagementService;
			_masterService = masterService;
		}
		#endregion Constructors

		public string GetServiceStatus(string token)
		{
			StatusRequestResult result = new StatusRequestResult();

			if (!_controlService.ValidateManagementToken(token))
			{
				result.IsActive = false;
				result.IsInitialized = false;
				result.IsRequestValid = false;

				return _controlService.EncryptSymmetricResponse(_serializationProvider.Serialize(result));
			}

			MasterServiceData masterData = _masterService.GetMasterServiceData();
			if (masterData == null)
			{
				result.IsActive = false;
				result.IsInitialized = false;
				result.IsRequestValid = true;
			}

			return _controlService.EncryptSymmetricResponse(_serializationProvider.Serialize(result));
		}

		public string InitializeService(string token, string data)
		{
			InitializationResult result = new InitializationResult();

			try
			{
				if (!_controlService.ValidateManagementToken(token))
				{
					result.WasInitializionSucessful = false;
					return _controlService.EncryptSymmetricResponse(_serializationProvider.Serialize(result));
				}

				string decryptedData = _controlService.DecryptSymmetricResponse(data);
				MasterServiceData masterData = _serializationProvider.Deserialize<MasterServiceData>(decryptedData);

				MasterServiceData masterData1 = _masterService.SetMasterServiceData(masterData);

				if (masterData1 == null)
				{
					result.WasInitializionSucessful = false;
					return _controlService.EncryptSymmetricResponse(_serializationProvider.Serialize(result));
				}

				result.WasInitializionSucessful = true;
			}
			catch (Exception ex)
			{
				LoggingHelper.LogException(ex);
			}

			return _controlService.EncryptSymmetricResponse(_serializationProvider.Serialize(result));
		}

		public string SetupTestProduct(string token, string key)
		{
			SetupTestProductResult result = new SetupTestProductResult();

			if (!_controlService.ValidateManagementToken(token))
			{
				result.WasOperationSuccessful = false;
				result.WasRequestValid = false;

				return _controlService.EncryptSymmetricResponse(_serializationProvider.Serialize(result));
			}

			ServiceProduct sp = _productManagementService.CreateTestProduct(key);

			if (sp != null)
			{
				result.WasOperationSuccessful = true;
				result.WasRequestValid = true;
			}

			return _controlService.EncryptSymmetricResponse(_serializationProvider.Serialize(result));
		}

		public string CleanTestProductData(string token)
		{
			SetupTestProductResult result = new SetupTestProductResult();

			if (!_controlService.ValidateManagementToken(token))
			{
				result.WasOperationSuccessful = false;
				result.WasRequestValid = false;

				return _controlService.EncryptSymmetricResponse(_serializationProvider.Serialize(result));
			}

			result.WasRequestValid = true;

			_productManagementService.DeleteTestServiceProduct();

			result.WasOperationSuccessful = true;

			return _controlService.EncryptSymmetricResponse(_serializationProvider.Serialize(result));
		}

		public string QueryActiveProductsAndLiceseSets(string token)
		{
			QueryActiveServiceProductsResult result = new QueryActiveServiceProductsResult();

			try
			{
				if (!_controlService.ValidateManagementToken(token))
				{
					result.WasOperationSuccessful = false;
					result.WasRequestValid = false;

					return _controlService.EncryptSymmetricResponse(_serializationProvider.Serialize(result));
				}

				result.WasRequestValid = true;

				result.ProductsAndLicenseSets = new List<QueryActiveServiceProductsResultData>();
				List<ServiceProduct> products = _productManagementService.GetAllServicePorducts();

				foreach (ServiceProduct sp in products)
				{
					List<int> liceseSets = new List<int>();

					foreach (ServiceLicenseSet sls in sp.LicenseSets)
					{
						liceseSets.Add(sls.LicenseSetId);
					}

					QueryActiveServiceProductsResultData data = new QueryActiveServiceProductsResultData();
					data.Id = sp.LicenseId;
					data.SetIds = liceseSets;

					result.ProductsAndLicenseSets.Add(data);
				}

				result.WasOperationSuccessful = true;
			}
			catch (Exception ex)
			{
				result.WasException = true;
				result.ExceptionMessage = ex.Message; // TODO: Must be modified to hide important data before release
			}

			return _controlService.SerializeAndEncryptMgmtOutboundData(result);
		}

		public string BasicServiceTest()
		{
			return "Ok";
		}
	}
}