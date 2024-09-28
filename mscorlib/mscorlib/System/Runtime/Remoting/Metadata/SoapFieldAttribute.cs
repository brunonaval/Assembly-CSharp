using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata
{
	/// <summary>Customizes SOAP generation and processing for a field. This class cannot be inherited.</summary>
	// Token: 0x020005D9 RID: 1497
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Field)]
	public sealed class SoapFieldAttribute : SoapAttribute
	{
		/// <summary>Gets or sets the order of the current field attribute.</summary>
		/// <returns>The order of the current field attribute.</returns>
		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x060038FD RID: 14589 RVA: 0x000CAFAE File Offset: 0x000C91AE
		// (set) Token: 0x060038FE RID: 14590 RVA: 0x000CAFB6 File Offset: 0x000C91B6
		public int Order
		{
			get
			{
				return this._order;
			}
			set
			{
				this._order = value;
			}
		}

		/// <summary>Gets or sets the XML element name of the field contained in the <see cref="T:System.Runtime.Remoting.Metadata.SoapFieldAttribute" /> attribute.</summary>
		/// <returns>The XML element name of the field contained in this attribute.</returns>
		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x060038FF RID: 14591 RVA: 0x000CAFBF File Offset: 0x000C91BF
		// (set) Token: 0x06003900 RID: 14592 RVA: 0x000CAFC7 File Offset: 0x000C91C7
		public string XmlElementName
		{
			get
			{
				return this._elementName;
			}
			set
			{
				this._isElement = (value != null);
				this._elementName = value;
			}
		}

		/// <summary>Returns a value indicating whether the current attribute contains interop XML element values.</summary>
		/// <returns>
		///   <see langword="true" /> if the current attribute contains interop XML element values; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003901 RID: 14593 RVA: 0x000CAFDA File Offset: 0x000C91DA
		public bool IsInteropXmlElement()
		{
			return this._isElement;
		}

		// Token: 0x06003902 RID: 14594 RVA: 0x000CAFE4 File Offset: 0x000C91E4
		internal override void SetReflectionObject(object reflectionObject)
		{
			FieldInfo fieldInfo = (FieldInfo)reflectionObject;
			if (this._elementName == null)
			{
				this._elementName = fieldInfo.Name;
			}
		}

		// Token: 0x04002602 RID: 9730
		private int _order;

		// Token: 0x04002603 RID: 9731
		private string _elementName;

		// Token: 0x04002604 RID: 9732
		private bool _isElement;
	}
}
