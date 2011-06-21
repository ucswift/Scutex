using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WaveTech.Scutex.UnitTests
{
	[TestClass]
	public class FixtureBase
	{
		[ClassInitialize]
		public void SetupContext()
		{
			Before_each_test();
		}

		[ClassCleanup]
		public void TearDownContext()
		{
			After_each_test();
		}

		[TestInitialize]
		public void FixtureSetup()
		{
			Before_all_tests();
		}

		[TestCleanup]
		public void FixtureTeardown()
		{
			After_all_tests();
		}

		protected virtual void Before_each_test()
		{
		}

		protected virtual void After_each_test()
		{
		}

		protected virtual void Before_all_tests()
		{
		}

		protected virtual void After_all_tests()
		{
		}
	}
}