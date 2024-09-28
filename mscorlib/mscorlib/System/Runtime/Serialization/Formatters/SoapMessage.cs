﻿using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Serialization.Formatters
{
	/// <summary>Holds the names and types of parameters required during serialization of a SOAP RPC (Remote Procedure Call).</summary>
	// Token: 0x02000682 RID: 1666
	[ComVisible(true)]
	[Serializable]
	public class SoapMessage : ISoapMessage
	{
		/// <summary>Gets or sets the parameter names for the called method.</summary>
		/// <returns>The parameter names for the called method.</returns>
		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x06003DF7 RID: 15863 RVA: 0x000D5CA0 File Offset: 0x000D3EA0
		// (set) Token: 0x06003DF8 RID: 15864 RVA: 0x000D5CA8 File Offset: 0x000D3EA8
		public string[] ParamNames
		{
			get
			{
				return this.paramNames;
			}
			set
			{
				this.paramNames = value;
			}
		}

		/// <summary>Gets or sets the parameter values for the called method.</summary>
		/// <returns>Parameter values for the called method.</returns>
		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x06003DF9 RID: 15865 RVA: 0x000D5CB1 File Offset: 0x000D3EB1
		// (set) Token: 0x06003DFA RID: 15866 RVA: 0x000D5CB9 File Offset: 0x000D3EB9
		public object[] ParamValues
		{
			get
			{
				return this.paramValues;
			}
			set
			{
				this.paramValues = value;
			}
		}

		/// <summary>This property is reserved. Use the <see cref="P:System.Runtime.Serialization.Formatters.SoapMessage.ParamNames" /> and/or <see cref="P:System.Runtime.Serialization.Formatters.SoapMessage.ParamValues" /> properties instead.</summary>
		/// <returns>Parameter types for the called method.</returns>
		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x06003DFB RID: 15867 RVA: 0x000D5CC2 File Offset: 0x000D3EC2
		// (set) Token: 0x06003DFC RID: 15868 RVA: 0x000D5CCA File Offset: 0x000D3ECA
		public Type[] ParamTypes
		{
			get
			{
				return this.paramTypes;
			}
			set
			{
				this.paramTypes = value;
			}
		}

		/// <summary>Gets or sets the name of the called method.</summary>
		/// <returns>The name of the called method.</returns>
		// Token: 0x17000996 RID: 2454
		// (get) Token: 0x06003DFD RID: 15869 RVA: 0x000D5CD3 File Offset: 0x000D3ED3
		// (set) Token: 0x06003DFE RID: 15870 RVA: 0x000D5CDB File Offset: 0x000D3EDB
		public string MethodName
		{
			get
			{
				return this.methodName;
			}
			set
			{
				this.methodName = value;
			}
		}

		/// <summary>Gets or sets the XML namespace name where the object that contains the called method is located.</summary>
		/// <returns>The XML namespace name where the object that contains the called method is located.</returns>
		// Token: 0x17000997 RID: 2455
		// (get) Token: 0x06003DFF RID: 15871 RVA: 0x000D5CE4 File Offset: 0x000D3EE4
		// (set) Token: 0x06003E00 RID: 15872 RVA: 0x000D5CEC File Offset: 0x000D3EEC
		public string XmlNameSpace
		{
			get
			{
				return this.xmlNameSpace;
			}
			set
			{
				this.xmlNameSpace = value;
			}
		}

		/// <summary>Gets or sets the out-of-band data of the called method.</summary>
		/// <returns>The out-of-band data of the called method.</returns>
		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x06003E01 RID: 15873 RVA: 0x000D5CF5 File Offset: 0x000D3EF5
		// (set) Token: 0x06003E02 RID: 15874 RVA: 0x000D5CFD File Offset: 0x000D3EFD
		public Header[] Headers
		{
			get
			{
				return this.headers;
			}
			set
			{
				this.headers = value;
			}
		}

		// Token: 0x040027B7 RID: 10167
		internal string[] paramNames;

		// Token: 0x040027B8 RID: 10168
		internal object[] paramValues;

		// Token: 0x040027B9 RID: 10169
		internal Type[] paramTypes;

		// Token: 0x040027BA RID: 10170
		internal string methodName;

		// Token: 0x040027BB RID: 10171
		internal string xmlNameSpace;

		// Token: 0x040027BC RID: 10172
		internal Header[] headers;
	}
}
