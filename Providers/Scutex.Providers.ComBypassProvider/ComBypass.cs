using System;
using System.Collections.Generic;
using WaveTech.Scutex.Model.Interfaces.Providers;

namespace WaveTech.Scutex.Providers.ComBypassProvider
{
	internal class ComBypass : IComBypassProvider
	{
		private readonly IHashingProvider _hashingProvider;

		public ComBypass(IHashingProvider hashingProvider)
		{
			_hashingProvider = hashingProvider;
		}

		public bool IsComBypassEnabled()
		{
			AppDomain currentDomain = AppDomain.CurrentDomain;
			object data = currentDomain.GetData(GetBucketName());

			if (data == null)
				return false;

			return true;
		}

		public void SetComBypass(string p1, string p2)
		{
			if (!IsComBypassEnabled())
			{
				AppDomain currentDomain = AppDomain.CurrentDomain;

				List<string> data = new List<string>();
				data.Add(p1);
				data.Add(p2);

				currentDomain.SetData(GetBucketName(), data);
			}
		}

		public List<string> GetComBypass()
		{
			if (IsComBypassEnabled())
			{
				AppDomain currentDomain = AppDomain.CurrentDomain;

				List<string> data = (List<string>)currentDomain.GetData(GetBucketName());

				return data;
			}

			return null;
		}

		public void RemoveComBypass()
		{
			AppDomain currentDomain = AppDomain.CurrentDomain;
			currentDomain.SetData(GetBucketName(), null);
		}

		private string GetBucketName()
		{
			string hashData = DateTime.Now.Month + Environment.MachineName + DateTime.Now.Year + DateTime.Now.Day;

			return _hashingProvider.ComputeHash(hashData, "MD5");
		}
	}
}
