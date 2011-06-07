using NUnit.Framework;

namespace WaveTech.Scutex.UnitTests
{
	public class FixtureBase
	{
		[SetUp]
		public void SetupContext()
		{
			Before_each_test();
		}

		[TearDown]
		public void TearDownContext()
		{
			After_each_test();
		}

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			Before_all_tests();
		}

		[TestFixtureTearDown]
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