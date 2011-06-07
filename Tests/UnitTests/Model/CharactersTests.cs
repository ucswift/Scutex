using System;
using NUnit.Framework;
using WaveTech.Scutex.Model;

namespace WaveTech.Scutex.UnitTests.Model
{
	[TestFixture]
	public class CharactersTests
	{
	  [Test]
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