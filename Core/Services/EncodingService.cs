using System;
using System.Text;
using WaveTech.Scutex.Model.Interfaces.Services;

namespace WaveTech.Scutex.Services
{
	internal class EncodingService : IEncodingService
	{
		public string Encode(string data)
		{
			string hex = "";

			for (int i = 0; i < data.Length; i++)
			{
				int t = data[i];

				if (i > 0)
					hex += "|" + String.Format("{0:x2}", Convert.ToUInt32(t.ToString()));
				else
					hex += String.Format("{0:x2}", Convert.ToUInt32(t.ToString()));
			}

			return hex;
		}

		public string Decode(string data)
		{
			string str = "";
			string[] array = data.Split(Char.Parse("|"));

			foreach (string s in array)
			{
				str += Convert.ToChar(Convert.ToUInt32(s.Substring(0, 2), 16)).ToString();
			}
			return str;
		}

		public string Encode64(string data)
		{
			byte[] toEncodeAsBytes = ASCIIEncoding.ASCII.GetBytes(data);
			string returnValue = Convert.ToBase64String(toEncodeAsBytes);

			return returnValue;
		}

		public string Decode64(string data)
		{
			byte[] encodedDataAsBytes = Convert.FromBase64String(data);
			string returnValue = ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);

			return returnValue;
		}
	}
}