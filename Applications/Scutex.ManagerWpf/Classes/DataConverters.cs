using System.Collections.Generic;
using System.ComponentModel;
using WaveTech.Scutex.Model;
using License = WaveTech.Scutex.Model.License;

namespace WaveTech.Scutex.Manager.Classes
{
	internal static class DataConverters
	{
		internal static BindingList<UploadProductDisplayData> ConvertAllLicensesSetsToDisplay(Dictionary<License, List<LicenseSet>> data)
		{
			BindingList<UploadProductDisplayData> returnData = new BindingList<UploadProductDisplayData>();

			foreach (var v in data)
			{
				foreach (var x in v.Value)
				{
					UploadProductDisplayData n = new UploadProductDisplayData();
					n.LicenseId = v.Key.LicenseId;
					n.LicenseName = v.Key.Name;
					n.ProductId = v.Key.Product.ProductId;
					n.ProductName = v.Key.Product.Name;
					n.LicenseSetId = x.LicenseSetId;
					n.LicenseSetName = x.Name;

					returnData.Add(n);
				}
			}

			return returnData;
		}
	}
}
