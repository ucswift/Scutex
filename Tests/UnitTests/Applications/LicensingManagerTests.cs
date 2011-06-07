using NUnit.Framework;
using WaveTech.Scutex.Licensing;
using WaveTech.Scutex.Model;

namespace WaveTech.Scutex.UnitTests.Applications
{
	namespace LicensingManagerTests
	{
		public class with_the_licensing_manager : FixtureBase
		{
			protected LicensingManager _licensingManager;
			protected LicenseHelper _licenseHelper;

			protected override void Before_each_test()
			{
				_licensingManager = new LicensingManager();
				_licenseHelper = new LicenseHelper();

				_licenseHelper.BasicSetup();
			}
		}

		[TestFixture]
		public class when_calling_the_validate_method : with_the_licensing_manager
		{
			[Test]
			public void shall_not_throw_an_exception()
			{
				_licensingManager.Validate(InteractionModes.Silent);
			}

		}
	}
}