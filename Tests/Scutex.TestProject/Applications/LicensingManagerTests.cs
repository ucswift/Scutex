using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WaveTech.Scutex.Licensing;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Exceptions;
using WaveTech.Scutex.UnitTests.Applications.ComWraperTests;
using WaveTech.Scutex.UnitTests.Helpers;

namespace WaveTech.Scutex.UnitTests.Applications
{
	namespace LicensingManagerTests
	{
		[TestClass]
		public class with_the_licensing_manager
		{
			protected LicensingManager _licensingManager;
			protected LicenseHelper _licenseHelper;
			protected LicenseServiceTestManager _serviceTestManager;

			[TestInitialize]
			public void Before_each_test()
			{
				_licensingManager = new LicensingManager(this);

				_licenseHelper = new LicenseHelper();
				_serviceTestManager = new LicenseServiceTestManager(_licenseHelper);

				_serviceTestManager.Start();

				_licenseHelper.BasicSetup();
			}

			[TestCleanup]
			public void After_all_tests()
			{
				_serviceTestManager.Stop();

				_licenseHelper = null;
			}
		}

		[TestClass]
		public class when_newing_up_the_licensing_manager: with_the_licensing_manager
		{
			[TestMethod]
			public void default_constructor_should_not_throw_an_exception()
			{
				LicensingManager lm = new LicensingManager();
			}

			[TestMethod]
			public void instance_constructor_should_not_throw_an_exception()
			{
				LicensingManager lm = new LicensingManager(this);
			}

			[TestMethod]
			public void options_constructor_should_not_throw_an_exception()
			{
				LicensingManagerOptions options = new LicensingManagerOptions();
				options.DataFileLocation = @"C:\Temp\Scutex\sxu.dll";
				options.DllHash = "123";
				options.PublicKey = "123";

				LicensingManager lm = new LicensingManager(this, options);
			}
		}

		[TestClass]
		public class when_calling_the_validate_method_with_valid_trial : with_the_licensing_manager
		{
			[TestMethod]
			public void shall_not_throw_an_exception()
			{
				_licensingManager.Validate(InteractionModes.Silent);
			}

			[TestMethod]
			public void should_be_valid()
			{
				var retval = _licensingManager.Validate(InteractionModes.Silent);

				Assert.IsTrue(retval.IsTrialValid);
			}

			//[TestMethod]
			//public void expired_trial_should_return_false()
			//{
			//  _licenseHelper.ExpiredTrialSetup();

			//  var retval = _licensingManager.Validate(InteractionModes.Silent);

			//  Assert.IsTrue(retval.IsTrialValid);
			//  Assert.IsTrue(retval.IsTrialExpired);
			//  //Assert.AreEqual(TrialFaultReasons.TimeFault, retval.TrialFaultReason);
			//}

			[TestMethod]
			public void timeinvalid_trial_should_return_time_fault()
			{
				_licenseHelper.InvalidTrialSetup();

				var retval = _licensingManager.Validate(InteractionModes.Silent);

				Assert.IsFalse(retval.IsTrialValid);
				Assert.IsFalse(retval.IsTrialExpired);
				Assert.AreEqual(TrialFaultReasons.TimeFault, retval.TrialFaultReason);
			}

			[TestMethod]
			[ExpectedException(typeof(Scutex.Model.Exceptions.ScutexAuditException))]
			public void invalid_trial_should_throw_exception()
			{
				_licenseHelper.DeleteFile();

				_licensingManager.Validate(InteractionModes.Silent);
			}
		}

		[TestClass]
		public class when_calling_the_validate_method_with_expired_trial : with_the_licensing_manager
		{
			[TestInitialize]
			public void Initialize()
			{
				_licenseHelper.ExpiredTrialSetup();
			}

			[TestMethod]
			public void expired_trial_should_return_false()
			{
				var retval = _licensingManager.Validate(InteractionModes.Silent);

				Assert.IsTrue(retval.IsTrialValid);
				Assert.IsTrue(retval.IsTrialExpired);
			}
		}

		[TestClass]
		public class when_calling_validate_and_register_with_hardware_locking : with_the_licensing_manager
		{
			[TestInitialize]
			public void Initialize()
			{
				_licenseHelper.HardwareUserSetup();
			}

			[TestMethod]
			public void valid_trial_should_return_true()
			{
				var retval = _licensingManager.Validate(InteractionModes.Silent);

				Assert.IsTrue(retval.IsTrialValid);
			}

			[TestMethod]
			public void trial_should_be_valid_after_registering_with_valid_hardware_key()
			{
				string validHardwareKey = _licenseHelper.GenerateLicenseKey(LicenseKeyTypes.HardwareLock);

				var retval = _licensingManager.Register(validHardwareKey);
			}
		}

		[TestClass]
		public class when_calling_the_register_method : with_the_licensing_manager
		{
			[TestMethod]
			[ExpectedException(typeof(ScutexAuditException))]
			public void shall_throw_an_exception()
			{
				_licensingManager.Register(null);
			}

			[TestMethod]
			[ExpectedException(typeof(ScutexAuditException))]
			public void shall_throw_an_exception_with_empty_string()
			{
				_licensingManager.Register("");
			}

			[TestMethod]
			public void shall_not_throw_an_exception_with_invalid_string()
			{
				_licensingManager.Register("asd*()@yL@I$78");
			}

			[TestMethod]
			[ExpectedException(typeof(ScutexAuditException))]
			public void shall_return_unlicensed_with_null()
			{
				var retval = _licensingManager.Register(null);

				Assert.IsFalse(retval.IsLicensed);
			}

			[TestMethod]
			[ExpectedException(typeof(ScutexAuditException))]
			public void shall_return_unlicensed_with_empty_string()
			{
				var retval = _licensingManager.Register("");

				Assert.IsFalse(retval.IsLicensed);
			}

			[TestMethod]
			public void shall_return_unlicensed_with_invalid_string()
			{

				var retval = _licensingManager.Register("asd*()@yL@I$78");

				Assert.IsFalse(retval.IsLicensed);
			}

			[TestMethod]
			public void shall_return_licensed_and_unactivated_when_registering_valid_enterprise_key()
			{
				string validEnterpriseKey = _licenseHelper.GenerateLicenseKey(LicenseKeyTypes.Enterprise);

				var retval = _licensingManager.Register(validEnterpriseKey);

				Assert.IsTrue(retval.IsLicensed);
				Assert.IsFalse(retval.IsActivated);
			}

			[TestMethod]
			public void trial_should_be_valid_after_registering_with_valid_enterprise_key()
			{
				string validEnterpriseKey = _licenseHelper.GenerateLicenseKey(LicenseKeyTypes.Enterprise);

				var retval1 = _licensingManager.Register(validEnterpriseKey);
				Assert.IsTrue(retval1.IsLicensed);
				Assert.IsFalse(retval1.IsActivated);

				var retval2 = _licensingManager.Validate(InteractionModes.Silent);
				Assert.IsTrue(retval2.IsLicensed);
				Assert.IsFalse(retval2.IsActivated);
			}

			[TestMethod]
			public void register_should_fail_with_non_enterprise_key()
			{
				string validEnterpriseKey = _licenseHelper.GenerateLicenseKey(LicenseKeyTypes.SingleUser);

				var retval1 = _licensingManager.Register(validEnterpriseKey);
				Assert.IsFalse(retval1.IsLicensed);
			}

			[TestMethod]
			public void register_should_fail_with_non_enterprise_key_and_no_service()
			{
				_licenseHelper.SingleUserSetup();

				string validEnterpriseKey = _licenseHelper.GenerateLicenseKey(LicenseKeyTypes.SingleUser);

				var retval1 = _licensingManager.Register(validEnterpriseKey);
				Assert.IsFalse(retval1.IsLicensed);
			}

			[TestMethod]
			public void register_should_work_with_valid_key_and_service()
			{
				_licenseHelper.SingleUserSetup();

				string validSingleUserKey = _licenseHelper.GenerateLicenseKey(LicenseKeyTypes.SingleUser);
				_licenseHelper.PushKeyToService(validSingleUserKey);

				var retval1 = _licensingManager.Register(validSingleUserKey);
				Assert.IsTrue(retval1.IsLicensed);
				Assert.IsTrue(retval1.IsActivated);
			}
		}
	}
}