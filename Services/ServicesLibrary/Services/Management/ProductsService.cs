using System;
using System.Diagnostics;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Model.Interfaces.Wcf;
using WaveTech.Scutex.Model.Results;
using WaveTech.Scutex.Model.ServiceData;
using WaveTech.Scutex.WcfServices.ServicesLibrary.Interfaces.Management;

namespace WaveTech.Scutex.WcfServices.ServicesLibrary.Services.Management
{
	public class ProductsService : IProductsService
	{
		#region Private Readonly Members
		private readonly IObjectSerializationProvider _serializationProvider;
		private readonly IControlService _controlService;
		private readonly IProductManagementService _productManagementService;
		private readonly IKeyManagementService _keyManagementService;
		#endregion Private Readonly Members

		#region Constructors
		public ProductsService()
			: this(ObjectLocator.GetInstance<IObjectSerializationProvider>(), ObjectLocator.GetInstance<IControlService>(),
			ObjectLocator.GetInstance<IProductManagementService>(), ObjectLocator.GetInstance<IKeyManagementService>())
		{

		}

		public ProductsService(IObjectSerializationProvider serializationProvider, IControlService controlService,
			IProductManagementService productManagementService, IKeyManagementService keyManagementService)
		{
			_serializationProvider = serializationProvider;
			_controlService = controlService;
			_productManagementService = productManagementService;
			_keyManagementService = keyManagementService;
		}
		#endregion Constructors

		public string AddProduct(string token, string data)
		{
			AddProductResult result = new AddProductResult();

			try
			{
				if (!_controlService.ValidateManagementToken(token))
				{
					result.WasOperationSuccessful = false;
					return _controlService.EncryptSymmetricResponse(_serializationProvider.Serialize(result));
				}

				result.WasRequestValid = true;

				ServiceProduct product = _controlService.DeserializeAndDencryptMgmtInboundData<ServiceProduct>(data);

				_productManagementService.SaveServiceProduct(product);

				result.WasOperationSuccessful = true;
			}
			catch (Exception ex)
			{
				result.WasException = true;
				result.ExceptionMessage = ex.ToString();
			}

			return _controlService.SerializeAndEncryptMgmtOutboundData(result);
		}

		public string GetLicenseKeysForProduct(string token, string data)
		{
			GetLicenseKeysForProductResult result = new GetLicenseKeysForProductResult();

			try
			{
				if (!_controlService.ValidateManagementToken(token))
				{
					result.WasOperationSuccessful = false;
					return _controlService.EncryptSymmetricResponse(_serializationProvider.Serialize(result));
				}

				result.WasRequestValid = true;

				GetLicenseKeysForProductData productData = _controlService.DeserializeAndDencryptMgmtInboundData<GetLicenseKeysForProductData>(data);
				result.LicenseKeys = _keyManagementService.GetKeysForLicenseSet(productData.LicenseSetId);
				Debug.WriteLine(result.LicenseKeys.Count);

				result.WasOperationSuccessful = true;
			}
			catch (Exception ex)
			{
				result.WasException = true;
				result.ExceptionMessage = ex.Message;
			}

			return _controlService.SerializeAndEncryptMgmtOutboundData(result);
		}

		public string AddLicenseKeysForProduct(string token, string data)
		{
			AddLicenseKeysForProductResult result = new AddLicenseKeysForProductResult();

			try
			{
				if (!_controlService.ValidateManagementToken(token))
				{
					result.WasOperationSuccessful = false;
					return _controlService.EncryptSymmetricResponse(_serializationProvider.Serialize(result));
				}

				result.WasRequestValid = true;

				AddLicenseKeysForProductData productData = _controlService.DeserializeAndDencryptMgmtInboundData<AddLicenseKeysForProductData>(data);
				_keyManagementService.AddLicenseKeysForLicenseSet(productData.LicenseSetId, productData.Keys);

				result.WasOperationSuccessful = true;
			}
			catch (Exception ex)
			{
				result.WasException = true;
				result.ExceptionMessage = ex.Message;
			}

			return _controlService.SerializeAndEncryptMgmtOutboundData(result);
		}
	}
}