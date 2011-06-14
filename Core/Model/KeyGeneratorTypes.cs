using System.ComponentModel;
using WaveTech.Scutex.Model.Attributes;
using WaveTech.Scutex.Model.Resources;

namespace WaveTech.Scutex.Model
{
	public enum KeyGeneratorTypes
	{
		//[Description("None")]
		[LocalizableDescription(@"KeyGeneratorTypes_None", typeof(Strings))]
		None					= 0,

		//[Description("Small Static Key (13 Chars)")]
		[LocalizableDescription(@"KeyGeneratorTypes_StaticSmall", typeof(Strings))]
		StaticSmall		= 1,

		//[Description("Large Static Key (25 Chars)")]
		[LocalizableDescription(@"KeyGeneratorTypes_StaticLarge", typeof(Strings))]
		StaticLarge   = 2
	}
}