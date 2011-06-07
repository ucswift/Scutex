using NUnit.Framework;
using ScutexLicensingCCW;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.UnitTests.Helpers;

namespace WaveTech.Scutex.UnitTests.Applications
{
	namespace ComWraperTests
	{
		public class with_the_com_callable_wrapper : FixtureBase
		{
			protected LicensingManager _licensingManager;
			protected LicenseHelper _licenseHelper;
			protected LicenseServiceTestManager _serviceTestManager;

			protected override void Before_each_test()
			{
				_licensingManager = new LicensingManager();

				_licenseHelper.BasicSetup();
			}

			protected override void Before_all_tests()
			{
				_licenseHelper = new LicenseHelper();
				_serviceTestManager = new LicenseServiceTestManager(_licenseHelper);

				_serviceTestManager.Start();
			}

			protected override void After_all_tests()
			{
				_serviceTestManager.Stop();

				_licenseHelper = null;
			}
		}

		[TestFixture]
		public class when_calling_the_prepare_method : with_the_com_callable_wrapper
		{
			[Test]
			public void shall_not_throw_an_exception()
			{
				_licensingManager.Prepare("", "");
			}

			[Test]
			public void shall_return_false_with_nulls()
			{
				bool returnValue = _licensingManager.Prepare(null, null);

				Assert.IsFalse(returnValue);
			}

			[Test]
			public void shall_return_false_with_empty_strings()
			{
				bool returnValue = _licensingManager.Prepare("", "");

				Assert.IsFalse(returnValue);
			}

			[Test]
			public void shall_return_false_with_incorrect_data()
			{
				bool returnValue = _licensingManager.Prepare("adad9824h1p984h2", "123123123123123123");

				Assert.IsFalse(returnValue);
			}

			[Test]
			public void shall_return_false_with_incorrect_data2()
			{
				bool returnValue = _licensingManager.Prepare("38|34|32|37|31|34|37|34|30|30|30|39|32|31|31|30|37|30|33|39|31|32|35|32|38|38|36|33|31|37|37|33|36|32|31|35|33|31|37|38|33|36|36|31|35|38|37|38|38|38|35|32|35|32|38|39|38|38|34|30|30|32|30|37|30|38|37|39|33|37|32|38|37|35|35|36|34|38|34|30|30|36|37|39|34|37|37|31|34|34|30|37|35|30|33|31|39|31|39|30|37|30|30|30|38|34|39|32|32|34|30|33|35|37|33|34|33|35|32|37|30|36|37|30|38|39|33|36|34|31|36|31|35|33|34|39|35|35|37|38|35|39|37|33|35|31|30|31|39|30|37|37|36|39|36|36|31|36|39|39|31|35|37|39|34|39|32|30|32|39|31|32|31|38|30|30|34|32|30|33|32|36|36|30|32|36|33|35|37|39|39|39|37|34|38|39|30|36|39|33|39|35|30|36|34|30|36|32|32|33|30|36|38|35|30|33|37|31|35|33|38|34|33|32|32|35|38|32|30|34|39|7c|36|35|35|33|37", "43|31|2d|46|45|2d|44|35|2d|42|46|2d|46|45|2d|41|42|2d|42|41|2d|37|33|2d|45|37|2d|37|42|2d|43|45|2d|44|36|2d|46|44|2d|33|30|2d|39|44|2d|39|36|2d|43|33|2d|42|37|2d|38|33|2d|37|35");

				Assert.IsFalse(returnValue);
			}

			[Test]
			public void shall_return_true_with_correct_data()
			{
				bool returnValue = _licensingManager.Prepare(_licenseHelper.PublicKey, _licenseHelper.DllHash);

				Assert.IsTrue(returnValue);
			}
		}

		[TestFixture]
		public class when_calling_the_validate_method : with_the_com_callable_wrapper
		{
			private void Prepare()
			{
				_licensingManager.Prepare(_licenseHelper.PublicKey, _licenseHelper.DllHash);
			}

			[Test]
			public void shall_not_throw_an_exception()
			{
				Prepare();

				_licensingManager.Validate((int)InteractionModes.Silent);
			}

			[Test]
			public void valid_trial_should_return_a_value_of_0()
			{
				Prepare();

				int retval = _licensingManager.Validate((int)InteractionModes.Silent);

				Assert.AreEqual(0, retval);
			}

			[Test]
			public void expired_trial_should_return_a_value_of_100()
			{
				Prepare();
				_licenseHelper.ExpiredTrialSetup();

				int retval = _licensingManager.Validate((int)InteractionModes.Silent);

				Assert.AreEqual(100, retval);
			}

			[Test]
			public void timeinvalid_trial_should_return_a_value_of_201()
			{
				Prepare();
				_licenseHelper.InvalidTrialSetup();

				int retval = _licensingManager.Validate((int)InteractionModes.Silent);

				Assert.AreEqual(201, retval);
			}

			[Test]
			public void invalid_trial_should_return_a_value_of_42()
			{
				Prepare();
				_licenseHelper.DeleteFile();

				int retval = _licensingManager.Validate((int)InteractionModes.Silent);

				Assert.AreEqual(42, retval);
			}
		}

		[TestFixture]
		public class when_calling_the_register_method : with_the_com_callable_wrapper
		{
			private void Prepare()
			{
				_licensingManager.Prepare(_licenseHelper.PublicKey, _licenseHelper.DllHash);
			}

			[Test]
			public void shall_not_throw_an_exception()
			{
				Prepare();

				_licensingManager.Register(null);
			}

			[Test]
			public void shall_not_throw_an_exception_with_empty_string()
			{
				Prepare();

				_licensingManager.Register("");
			}

			[Test]
			public void shall_not_throw_an_exception_with_invalid_string()
			{
				Prepare();

				_licensingManager.Register("asd*()@yL@I$78");
			}

			[Test]
			public void shall_return_42_with_null()
			{
				Prepare();

				int retval = _licensingManager.Register(null);

				Assert.AreEqual(42, retval);
			}

			[Test]
			public void shall_return_42_with_empty_string()
			{
				Prepare();

				int retval = _licensingManager.Register("");

				Assert.AreEqual(42, retval);
			}

			[Test]
			public void shall_return_42_with_invalid_string()
			{
				Prepare();

				int retval = _licensingManager.Register("asd*()@yL@I$78");

				Assert.AreEqual(42, retval);
			}

			[Test]
			public void shall_return_102_when_registering_valid_enterprise_key()
			{
				Prepare();

				string validEnterpriseKey = _licenseHelper.GenerateLicenseKey(LicenseKeyTypes.Enterprise);

				int retval = _licensingManager.Register(validEnterpriseKey);

				Assert.AreEqual(102, retval);
			}

			[Test]
			public void trial_should_be_valid_after_registering_with_valid_enterprise_key()
			{
				Prepare();

				string validEnterpriseKey = _licenseHelper.GenerateLicenseKey(LicenseKeyTypes.Enterprise);

				int retval1 = _licensingManager.Register(validEnterpriseKey);
				Assert.AreEqual(102, retval1);

				int retval2 = _licensingManager.Validate((int)InteractionModes.Silent);
				Assert.AreEqual(102, retval2);
			}

			[Test]
			public void register_should_fail_with_non_enterprise_key()
			{
				Prepare();

				string validEnterpriseKey = _licenseHelper.GenerateLicenseKey(LicenseKeyTypes.SingleUser);

				int retval1 = _licensingManager.Register(validEnterpriseKey);
				Assert.AreEqual(42, retval1);
			}

			[Test]
			public void register_should_fail_with_non_enterprise_key_and_no_service()
			{
				Prepare();
				_licenseHelper.SingleUserSetup();

				string validEnterpriseKey = _licenseHelper.GenerateLicenseKey(LicenseKeyTypes.SingleUser);

				int retval1 = _licensingManager.Register(validEnterpriseKey);
				Assert.AreEqual(42, retval1);
			}

			[Test]
			public void register_should_work_with_valid_key_and_service()
			{
				Prepare();
				_licenseHelper.SingleUserSetup();

				string validSingleUserKey = _licenseHelper.GenerateLicenseKey(LicenseKeyTypes.SingleUser);
				_licenseHelper.PushKeyToService(validSingleUserKey);

				int retval1 = _licensingManager.Register(validSingleUserKey);
				Assert.AreEqual(101, retval1);
			}
		}
	}
}