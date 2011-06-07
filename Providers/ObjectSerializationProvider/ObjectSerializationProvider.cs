using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using WaveTech.Scutex.Model.Interfaces.Providers;

namespace WaveTech.Scutex.Providers.ObjectSerialization
{
	internal class ObjectSerializationProvider : IObjectSerializationProvider
	{
		public string Serialize(object o)
		{
			String XmlizedString = null;
			MemoryStream memoryStream = new MemoryStream();
			XmlSerializer xs = new XmlSerializer(o.GetType());
			XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

			xs.Serialize(xmlTextWriter, o);
			memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
			XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());

			return XmlizedString;
		}

		public T Deserialize<T>(string o)
		{
			XmlSerializer xs = new XmlSerializer(typeof(T));
			MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(o));

			return (T)xs.Deserialize(memoryStream);
		}

		/// <summary>
		/// To convert a Byte Array of Unicode values (UTF-8 encoded) to a complete String.
		/// </summary>
		/// <param name="characters">Unicode Byte Array to be converted to String</param>
		/// <returns>String converted from Unicode Byte Array</returns>
		private String UTF8ByteArrayToString(Byte[] characters)
		{
			UTF8Encoding encoding = new UTF8Encoding();
			String constructedString = encoding.GetString(characters);

			return (constructedString);
		}

		/// <summary>
		/// Converts the String to UTF8 Byte array and is used in De serialization
		/// </summary>
		/// <param name="pXmlString"></param>
		/// <returns></returns>
		private Byte[] StringToUTF8ByteArray(String pXmlString)
		{
			UTF8Encoding encoding = new UTF8Encoding();
			Byte[] byteArray = encoding.GetBytes(pXmlString);

			return byteArray;
		}
	}
}