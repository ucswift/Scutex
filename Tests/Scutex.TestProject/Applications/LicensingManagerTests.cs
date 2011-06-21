using Microsoft.VisualStudio.TestTools.UnitTesting;
using WaveTech.Scutex.Licensing;
using WaveTech.Scutex.Model;

namespace WaveTech.Scutex.UnitTests.Applications
{
	namespace LicensingManagerTests
	{
		[TestClass]
		public class with_the_licensing_manager
		{
			protected LicensingManager _licensingManager;
			protected LicenseHelper _licenseHelper;

			[TestInitialize]
			public void Before_each_test()
			{
				_licensingManager = new LicensingManager();
				_licenseHelper = new LicenseHelper();

				_licenseHelper.BasicSetup();
			}
		}

		[TestClass]
		public class when_calling_the_validate_method : with_the_licensing_manager
		{
			[TestMethod]
			public void shall_not_throw_an_exception()
			{
				_licensingManager.Validate(InteractionModes.Silent);
			}

		}
	}
}