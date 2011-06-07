using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using WaveTech.Scutex.Model.Interfaces.Providers;

namespace WaveTech.Scutex.Providers.NetworkTimeProvider
{
	internal class NtpProvider : INetworkTimeProvider
	{
		private static List<string> _ntpServers = new List<string>
		                                      	{
		                                      		"0.us.pool.ntp.org",
																							"1.us.pool.ntp.org",
																							"2.us.pool.ntp.org",
																							"3.us.pool.ntp.org",
		                                      	};

		/// <summary>
		/// Gets the current DateTime from time-a.nist.gov.
		/// </summary>
		/// <returns>A DateTime containing the current time.</returns>
		public DateTime GetNetworkTime()
		{
			List<DateTime> times = new List<DateTime>();
			List<double> averageDifferences = new List<double>();

			foreach (string s in _ntpServers)
				times.Add(GetNetworkTime(s));

			for (int i = 0; i < times.Count; i++)
			{
				double avg = times.Skip(i).Take(times.Count - 1).Average(t => t.Ticks);
				averageDifferences.Add(avg);
			}

			double min = averageDifferences.Min();
			DateTime accTime = DateTime.Now;

			for (int i = 0; i < times.Count; i++)
			{
				if (min == averageDifferences[i])
					accTime = times[i];
			}

			return accTime;
		}

		/// <summary>
		/// Gets the current DateTime from <paramref name="ntpServer"/>.
		/// </summary>
		/// <param name="ntpServer">The hostname of the NTP server.</param>
		/// <returns>A DateTime containing the current time.</returns>
		private DateTime GetNetworkTime(string ntpServer)
		{
			IPAddress[] address = Dns.GetHostEntry(ntpServer).AddressList;

			if (address == null || address.Length == 0)
				throw new ArgumentException("Could not resolve ip address from '" + ntpServer + "'.", "ntpServer");

			IPEndPoint ep = new IPEndPoint(address[0], 123);

			return GetNetworkTime(ep);
		}

		/// <summary>
		/// Gets the current DateTime form <paramref name="ep"/> IPEndPoint.
		/// </summary>
		/// <param name="ep">The IPEndPoint to connect to.</param>
		/// <returns>A DateTime containing the current time.</returns>
		private DateTime GetNetworkTime(IPEndPoint ep)
		{
			DateTime networkDateTime;

			try
			{
				Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
				s.ReceiveTimeout = 1000;

				s.Connect(ep);

				byte[] ntpData = new byte[48]; // RFC 2030
				ntpData[0] = 0x1B;
				for (int i = 1; i < 48; i++)
					ntpData[i] = 0;

				s.Send(ntpData);
				s.Receive(ntpData);

				byte offsetTransmitTime = 40;
				ulong intpart = 0;
				ulong fractpart = 0;

				for (int i = 0; i <= 3; i++)
					intpart = 256 * intpart + ntpData[offsetTransmitTime + i];

				for (int i = 4; i <= 7; i++)
					fractpart = 256 * fractpart + ntpData[offsetTransmitTime + i];

				ulong milliseconds = (intpart * 1000 + (fractpart * 1000) / 0x100000000L);
				s.Close();

				TimeSpan timeSpan = TimeSpan.FromTicks((long)milliseconds * TimeSpan.TicksPerMillisecond);

				DateTime dateTime = new DateTime(1900, 1, 1);
				dateTime += timeSpan;

				TimeSpan offsetAmount = TimeZone.CurrentTimeZone.GetUtcOffset(dateTime);
				networkDateTime = (dateTime + offsetAmount);
			}
			catch
			{
				networkDateTime = DateTime.Now;
			}

			return networkDateTime;
		}
	}
}
