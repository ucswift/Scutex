
using System.Collections.Generic;

namespace WaveTech.Scutex.Model.Results
{
	public class QueryActiveServiceProductsResult : BaseServiceResult
	{
		public List<QueryActiveServiceProductsResultData> ProductsAndLicenseSets { get; set; }
	}
}