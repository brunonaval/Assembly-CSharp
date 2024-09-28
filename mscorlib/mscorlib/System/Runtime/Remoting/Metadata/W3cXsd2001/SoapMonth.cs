﻿using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	/// <summary>Wraps an XSD <see langword="gMonth" /> type.</summary>
	// Token: 0x020005EF RID: 1519
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapMonth : ISoapXsd
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapMonth" /> class.</summary>
		// Token: 0x06003999 RID: 14745 RVA: 0x0000259F File Offset: 0x0000079F
		public SoapMonth()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapMonth" /> class with a specified <see cref="T:System.DateTime" /> object.</summary>
		/// <param name="value">A <see cref="T:System.DateTime" /> object to initialize the current instance.</param>
		// Token: 0x0600399A RID: 14746 RVA: 0x000CBC49 File Offset: 0x000C9E49
		public SoapMonth(DateTime value)
		{
			this._value = value;
		}

		/// <summary>Gets or sets the date and time of the current instance.</summary>
		/// <returns>The <see cref="T:System.DateTime" /> object that contains the date and time of the current instance.</returns>
		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x0600399B RID: 14747 RVA: 0x000CBC58 File Offset: 0x000C9E58
		// (set) Token: 0x0600399C RID: 14748 RVA: 0x000CBC60 File Offset: 0x000C9E60
		public DateTime Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		/// <summary>Gets the XML Schema definition language (XSD) of the current SOAP type.</summary>
		/// <returns>A <see cref="T:System.String" /> that indicates the XSD of the current SOAP type.</returns>
		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x0600399D RID: 14749 RVA: 0x000CBC69 File Offset: 0x000C9E69
		public static string XsdType
		{
			get
			{
				return "gMonth";
			}
		}

		/// <summary>Returns the XML Schema definition language (XSD) of the current SOAP type.</summary>
		/// <returns>A <see cref="T:System.String" /> that indicates the XSD of the current SOAP type.</returns>
		// Token: 0x0600399E RID: 14750 RVA: 0x000CBC70 File Offset: 0x000C9E70
		public string GetXsdType()
		{
			return SoapMonth.XsdType;
		}

		/// <summary>Converts the specified <see cref="T:System.String" /> into a <see cref="T:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapMonth" /> object.</summary>
		/// <param name="value">The <see langword="String" /> to convert.</param>
		/// <returns>A <see cref="T:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapDay" /> object that is obtained from <paramref name="value" />.</returns>
		/// <exception cref="T:System.Runtime.Remoting.RemotingException">
		///   <paramref name="value" /> does not contain a date and time that corresponds to any of the recognized format patterns.</exception>
		// Token: 0x0600399F RID: 14751 RVA: 0x000CBC77 File Offset: 0x000C9E77
		public static SoapMonth Parse(string value)
		{
			return new SoapMonth(DateTime.ParseExact(value, SoapMonth._datetimeFormats, null, DateTimeStyles.None));
		}

		/// <summary>Returns <see cref="P:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapMonth.Value" /> as a <see cref="T:System.String" />.</summary>
		/// <returns>A <see cref="T:System.String" /> that is obtained from <see cref="P:System.Runtime.Remoting.Metadata.W3cXsd2001.SoapMonth.Value" /> in the format "--MM--".</returns>
		// Token: 0x060039A0 RID: 14752 RVA: 0x000CBC8B File Offset: 0x000C9E8B
		public override string ToString()
		{
			return this._value.ToString("--MM--", CultureInfo.InvariantCulture);
		}

		// Token: 0x04002630 RID: 9776
		private static readonly string[] _datetimeFormats = new string[]
		{
			"--MM--",
			"--MM--zzz"
		};

		// Token: 0x04002631 RID: 9777
		private DateTime _value;
	}
}
