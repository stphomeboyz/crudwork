// crudwork
// Copyright 2004 by Steve T. Pham (http://www.crudwork.com)
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with This program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using crudwork.Utilities;

namespace crudwork.Network
{
	/// <summary>
	/// TCP/IP Utility
	/// </summary>
	public static class IP
	{
		private static IPv4Number loopBackAddress;
		private static IPv4Number localHost;
		private static IPv4Number broadcastAddress;
		private static IPv4Range[] privateRange;

		static IP()
		{
			loopBackAddress = new IPv4Number("127.0.0.0");
			localHost = new IPv4Number("127.0.0.1");
			broadcastAddress = new IPv4Number("255.255.255.255");
			//broadcastAddress = new IPv4Number("0.0.0.0");			// FreeBSD

			privateRange = new IPv4Range[] {
				new IPv4Range("Class A", new IPv4Number("10.0.0.0"),new IPv4Number("10.255.255.255")),
				new IPv4Range("Class B", new IPv4Number("172.16.0.0"),new IPv4Number("172.31.255.255")),
				new IPv4Range("Class C", new IPv4Number("192.168.0.0"),new IPv4Number("192.168.255.255")),
			};
		}

		/// <summary>
		/// Return true if the given IP number is private.
		/// </summary>
		/// <param name="ipNumber"></param>
		/// <returns></returns>
		public static bool IsPrivateNetwork(IPv4Number ipNumber)
		{

			foreach (IPv4Range ipRange in privateRange)
			{
				if (ipRange.Contains(ipNumber))
					return true;
			}
			return false;
		}

		/// <summary>
		/// return IP number(s) of given host name
		/// </summary>
		/// <param name="hostname"></param>
		/// <returns></returns>
		public static IPv4Number[] GetIPAddressList(string hostname)
		{
			List<IPv4Number> results = new List<IPv4Number>();

			IPHostEntry host = Dns.GetHostEntry(hostname);

			foreach (IPAddress ip in host.AddressList)
			{
				results.Add(new IPv4Number(ip));
			}

			return results.ToArray();
		}

		/// <summary>
		/// return the hostname of given ip number
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetHostName(string value)
		{
			return GetHostName(new IPv4Number(value).IPAddress);
		}

		/// <summary>
		/// return the hostname of given ip number
		/// </summary>
		/// <param name="ip"></param>
		/// <returns></returns>
		public static string GetHostName(IPAddress ip)
		{
			IPHostEntry host = Dns.GetHostEntry(ip);
			return host.HostName;
		}
	}

	/// <summary>
	/// define a range of IP number
	/// </summary>
	public class IPv4Range
	{
		private string className;
		private IPv4Number fromIP;
		private IPv4Number toIP;

		/// <summary>
		/// create a new object with given attributes
		/// </summary>
		/// <param name="className"></param>
		/// <param name="fromIP"></param>
		/// <param name="toIP"></param>
		public IPv4Range(string className, IPv4Number fromIP, IPv4Number toIP)
		{
			this.className = className;
			this.fromIP = fromIP;
			this.toIP = toIP;
		}

		#region Public Properties
		/// <summary>
		/// get or set the descriptive class name
		/// </summary>
		public string ClassName
		{
			get
			{
				return this.className;
			}
			set
			{
				this.className = value;
			}
		}

		/// <summary>
		/// get or set the starting range
		/// </summary>
		public IPv4Number FromIP
		{
			get
			{
				return this.fromIP;
			}
			set
			{
				this.fromIP = value;
			}
		}

		/// <summary>
		/// get or set the ending range
		/// </summary>
		public IPv4Number ToIP
		{
			get
			{
				return this.toIP;
			}
			set
			{
				this.toIP = value;
			}
		}
		#endregion

		/// <summary>
		/// return true if the given IP number is within this range
		/// </summary>
		/// <param name="ip"></param>
		/// <returns></returns>
		public bool Contains(IPv4Number ip)
		{
			return ip >= fromIP && ip <= toIP;
		}
	}

	/// <summary>
	/// IP version 4
	/// </summary>
	public class IPv4Number
	{
		private byte n1;
		private byte n2;
		private byte n3;
		private byte n4;

		#region Constructors
		/// <summary>
		/// create an empty object
		/// </summary>
		public IPv4Number()
		{
			N1 = 0;
			N2 = 0;
			N3 = 0;
			N4 = 0;
		}

		/// <summary>
		/// create a new object with given attribute
		/// </summary>
		/// <param name="value"></param>
		public IPv4Number(string value)
			: this()
		{
			Parse(value);
		}

		/// <summary>
		/// create a new object with given attributes
		/// </summary>
		/// <param name="n1"></param>
		/// <param name="n2"></param>
		/// <param name="n3"></param>
		/// <param name="n4"></param>
		public IPv4Number(byte n1, byte n2, byte n3, byte n4)
		{
			N1 = n1;
			N2 = n2;
			N3 = n3;
			N4 = n4;
		}

		/// <summary>
		/// create a new object with given attributes
		/// </summary>
		/// <param name="address"></param>
		public IPv4Number(IPAddress address)
		{
			IPAddress = address;
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// get or set the first number value of the IP Number.
		/// </summary>
		public byte N1
		{
			get
			{
				return this.n1;
			}
			set
			{
				this.n1 = TestNumber(value);
			}
		}

		/// <summary>
		/// get or set the second number value of the IP Number.
		/// </summary>
		public byte N2
		{
			get
			{
				return this.n2;
			}
			set
			{
				this.n2 = TestNumber(value);
			}
		}

		/// <summary>
		/// get or set the third number value of the IP Number.
		/// </summary>
		public byte N3
		{
			get
			{
				return this.n3;
			}
			set
			{
				this.n3 = TestNumber(value);
			}
		}

		/// <summary>
		/// get or set the forth number value of the IP Number.
		/// </summary>
		public byte N4
		{
			get
			{
				return this.n4;
			}
			set
			{
				this.n4 = TestNumber(value);
			}
		}

		/// <summary>
		/// get or set the IPAddress object.
		/// </summary>
		public IPAddress IPAddress
		{
			get
			{
				return new IPAddress(new byte[] { N1, N2, N3, N4 });
			}
			set
			{
				if (value == null)
					throw new ArgumentNullException("address");
				byte[] tokens = value.GetAddressBytes();

				N1 = tokens[0];
				N2 = tokens[1];
				N3 = tokens[2];
				N4 = tokens[3];
			}
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// parses the ip number in dotted notation
		/// </summary>
		/// <param name="value"></param>
		public void Parse(string value)
		{
			try
			{
				string[] tokens = value.Split('.');
				if (tokens.Length != 4)
					throw new ArgumentException("Invalid value.  (Expected 4 values in '" + value + "'");

				for (int i = 0; i < tokens.Length; i++)
				{
					byte n = Convert.ToByte(tokens[i]);

					switch (i)
					{
						case 0:
							N1 = n;
							break;
						case 1:
							N2 = n;
							break;
						case 2:
							N3 = n;
							break;
						case 3:
							N4 = n;
							break;
						default:
							throw new ArgumentOutOfRangeException("i=" + i);
					}
				}
			}
			catch (Exception ex)
			{
				DebuggerTool.AddData(ex, "value", value);
				throw;
			}
		}
		#endregion

		#region Private Methods
		private byte TestNumber(byte value)
		{
			if (value < 0 || value > 255)
				throw new ArgumentOutOfRangeException("Numeric value must be is between 0 and 255");

			return value;
		}
		#endregion

		#region Override Methods
		/// <summary>
		/// return the string presentation of the object
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("{0}.{1}.{2}.{3}", N1, N2, N3, N4);
		}

		/// <summary>
		/// evaluate for equality
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			return this == obj as IPv4Number;
		}

		/// <summary>
		/// return the hash code
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}
		#endregion

		#region Operators
		/// <summary>
		/// evaluate equality
		/// </summary>
		/// <param name="l"></param>
		/// <param name="r"></param>
		/// <returns></returns>
		public static bool operator ==(IPv4Number l, IPv4Number r)
		{
			if ((object)l == null || (object)r == null)
				return false;
			return l.ToString() == r.ToString();
		}

		/// <summary>
		/// evaluate equality
		/// </summary>
		/// <param name="l"></param>
		/// <param name="r"></param>
		/// <returns></returns>
		public static bool operator <(IPv4Number l, IPv4Number r)
		{
			if ((object)l == null || (object)r == null)
				return false;
			return l.N1 < r.N1 || l.N2 < r.N2 || l.N3 < r.N3 || l.N4 < r.N4;
		}

		/// <summary>
		/// evaluate equality
		/// </summary>
		/// <param name="l"></param>
		/// <param name="r"></param>
		/// <returns></returns>
		public static bool operator >(IPv4Number l, IPv4Number r)
		{
			//return !(l == r) && !(l < r);
			if ((object)l == null || (object)r == null)
				return false;
			return l.N1 > r.N1 || l.N2 > r.N2 || l.N3 > r.N3 || l.N4 > r.N4;
		}

		/// <summary>
		/// evaluate equality
		/// </summary>
		/// <param name="l"></param>
		/// <param name="r"></param>
		/// <returns></returns>
		public static bool operator !=(IPv4Number l, IPv4Number r)
		{
			return !(l == r);
		}

		/// <summary>
		/// evaluate equality
		/// </summary>
		/// <param name="l"></param>
		/// <param name="r"></param>
		/// <returns></returns>
		public static bool operator <=(IPv4Number l, IPv4Number r)
		{
			return (l < r) || (l == r);
		}

		/// <summary>
		/// evaluate equality
		/// </summary>
		/// <param name="l"></param>
		/// <param name="r"></param>
		/// <returns></returns>
		public static bool operator >=(IPv4Number l, IPv4Number r)
		{
			return (l > r) || (l == r);
		}

		#endregion
	}
}
