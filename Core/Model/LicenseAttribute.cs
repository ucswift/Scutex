using System;

namespace WaveTech.Scutex.Model
{
	[AttributeUsage(AttributeTargets.Assembly)]
	public sealed class LicenseAttribute : Attribute
	{
		private string _key;
		private string _check;

		public LicenseAttribute(string p1, string p2)
		{
			_key = p1;
			_check = p2;
		}

		public string Key
		{
			get{ return _key;}
		}

		public string Check
		{
			get { return _check; }
		}
	}
}