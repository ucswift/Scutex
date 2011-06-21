using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WaveTech.Scutex.Model;

namespace WaveTech.Scutex.UnitTests.Model
{
	[TestClass]
	public class CharactersTests
	{
	  [TestMethod]
		public void VerifyCharacterMapsInSync()
		{
			Assert.AreEqual(CharacterMap.Map.Count, CharacterMap.ReverseMap.Count);

			foreach (var v in CharacterMap.Map)
			{
				Assert.AreEqual(v.Key, CharacterMap.ReverseMap[v.Value]);
			}
		}
	}
}