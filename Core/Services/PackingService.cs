using System;
using System.Text;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Model.Interfaces.Services;

namespace WaveTech.Scutex.Services
{
	internal class PackingService : IPackingService
	{
		private readonly INumberDataGeneratorProvider _numberDataGeneratorProvider;

		public PackingService(INumberDataGeneratorProvider numberDataGeneratorProvider)
		{
			_numberDataGeneratorProvider = numberDataGeneratorProvider;
		}

		public string PackToken(Token token)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(_numberDataGeneratorProvider.GenerateRandomNumber(0, 9));
			sb.Append(_numberDataGeneratorProvider.GenerateRandomNumber(0, 9));
			sb.Append(token.Timestamp.Day.ToString().PadLeft(2, char.Parse("0")));
			sb.Append(_numberDataGeneratorProvider.GenerateRandomNumber(0, 9));
			sb.Append(_numberDataGeneratorProvider.GenerateRandomNumber(0, 9));
			sb.Append(token.Timestamp.Month.ToString().PadLeft(2, char.Parse("0")));

			string year = token.Timestamp.Year.ToString();
			sb.Append(year.Substring(0, 2));

			sb.Append(token.Timestamp.Hour.ToString().PadLeft(2, char.Parse("0")));
			sb.Append(token.Data);
			sb.Append(token.Timestamp.Minute.ToString().PadLeft(2, char.Parse("0")));
			sb.Append(_numberDataGeneratorProvider.GenerateRandomNumber(0, 9));
			sb.Append(year.Substring(2, 2));

			sb.Append(_numberDataGeneratorProvider.GenerateRandomNumber(0, 9));
			sb.Append(_numberDataGeneratorProvider.GenerateRandomNumber(0, 9));
			sb.Append(_numberDataGeneratorProvider.GenerateRandomNumber(0, 9));

			return sb.ToString();
		}

		public Token UnpackToken(string data)
		{
			Token t = new Token();
			string decryptedToken = data;

			// Remove the Preamble and Tail
			decryptedToken = decryptedToken.Remove(0, 2);
			decryptedToken = decryptedToken.Remove(decryptedToken.Length - 3, 3);

			string day = decryptedToken.Substring(0, 2);
			decryptedToken = decryptedToken.Remove(0, 2);

			// Remove pad
			decryptedToken = decryptedToken.Remove(0, 2);


			string month = decryptedToken.Substring(0, 2);
			decryptedToken = decryptedToken.Remove(0, 2);

			string year = decryptedToken.Substring(0, 2);
			year = year + decryptedToken.Substring(decryptedToken.Length - 2, 2);
			decryptedToken = decryptedToken.Remove(0, 2);
			decryptedToken = decryptedToken.Remove(decryptedToken.Length - 2, 2);

			// Remove pad
			decryptedToken = decryptedToken.Remove(decryptedToken.Length - 1, 1);

			string hour = decryptedToken.Substring(0, 2);
			decryptedToken = decryptedToken.Remove(0, 2);

			string minute = decryptedToken.Substring(decryptedToken.Length - 2, 2);
			decryptedToken = decryptedToken.Remove(decryptedToken.Length - 2, 2);

			t.Timestamp = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day), int.Parse(hour), int.Parse(minute), 0);
			t.Data = decryptedToken;

			return t;
		}
	}
}
