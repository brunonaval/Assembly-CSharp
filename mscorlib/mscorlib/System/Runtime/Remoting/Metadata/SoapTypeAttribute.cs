using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata
{
	/// <summary>Customizes SOAP generation and processing for target types. This class cannot be inherited.</summary>
	// Token: 0x020005DD RID: 1501
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface)]
	[ComVisible(true)]
	public sealed class SoapTypeAttribute : SoapAttribute
	{
		/// <summary>Gets or sets a <see cref="T:System.Runtime.Remoting.Metadata.SoapOption" /> configuration value.</summary>
		/// <returns>A <see cref="T:System.Runtime.Remoting.Metadata.SoapOption" /> configuration value.</returns>
		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x06003913 RID: 14611 RVA: 0x000CB105 File Offset: 0x000C9305
		// (set) Token: 0x06003914 RID: 14612 RVA: 0x000CB10D File Offset: 0x000C930D
		public SoapOption SoapOptions
		{
			get
			{
				return this._soapOption;
			}
			set
			{
				this._soapOption = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the target of the current attribute will be serialized as an XML attribute instead of an XML field.</summary>
		/// <returns>The current implementation always returns <see langword="false" />.</returns>
		/// <exception cref="T:System.Runtime.Remoting.RemotingException">An attempt was made to set the current property.</exception>
		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x06003915 RID: 14613 RVA: 0x000CB116 File Offset: 0x000C9316
		// (set) Token: 0x06003916 RID: 14614 RVA: 0x000CB11E File Offset: 0x000C931E
		public override bool UseAttribute
		{
			get
			{
				return this._useAttribute;
			}
			set
			{
				this._useAttribute = value;
			}
		}

		/// <summary>Gets or sets the XML element name.</summary>
		/// <returns>The XML element name.</returns>
		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x06003917 RID: 14615 RVA: 0x000CB127 File Offset: 0x000C9327
		// (set) Token: 0x06003918 RID: 14616 RVA: 0x000CB12F File Offset: 0x000C932F
		public string XmlElementName
		{
			get
			{
				return this._xmlElementName;
			}
			set
			{
				this._isElement = (value != null);
				this._xmlElementName = value;
			}
		}

		/// <summary>Gets or sets the XML field order for the target object type.</summary>
		/// <returns>The XML field order for the target object type.</returns>
		// Token: 0x17000830 RID: 2096
		// (get) Token: 0x06003919 RID: 14617 RVA: 0x000CB142 File Offset: 0x000C9342
		// (set) Token: 0x0600391A RID: 14618 RVA: 0x000CB14A File Offset: 0x000C934A
		public XmlFieldOrderOption XmlFieldOrder
		{
			get
			{
				return this._xmlFieldOrder;
			}
			set
			{
				this._xmlFieldOrder = value;
			}
		}

		/// <summary>Gets or sets the XML namespace that is used during serialization of the target object type.</summary>
		/// <returns>The XML namespace that is used during serialization of the target object type.</returns>
		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x0600391B RID: 14619 RVA: 0x000CB153 File Offset: 0x000C9353
		// (set) Token: 0x0600391C RID: 14620 RVA: 0x000CB15B File Offset: 0x000C935B
		public override string XmlNamespace
		{
			get
			{
				return this._xmlNamespace;
			}
			set
			{
				this._isElement = (value != null);
				this._xmlNamespace = value;
			}
		}

		/// <summary>Gets or sets the XML type name for the target object type.</summary>
		/// <returns>The XML type name for the target object type.</returns>
		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x0600391D RID: 14621 RVA: 0x000CB16E File Offset: 0x000C936E
		// (set) Token: 0x0600391E RID: 14622 RVA: 0x000CB176 File Offset: 0x000C9376
		public string XmlTypeName
		{
			get
			{
				return this._xmlTypeName;
			}
			set
			{
				this._isType = (value != null);
				this._xmlTypeName = value;
			}
		}

		/// <summary>Gets or sets the XML type namespace for the current object type.</summary>
		/// <returns>The XML type namespace for the current object type.</returns>
		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x0600391F RID: 14623 RVA: 0x000CB189 File Offset: 0x000C9389
		// (set) Token: 0x06003920 RID: 14624 RVA: 0x000CB191 File Offset: 0x000C9391
		public string XmlTypeNamespace
		{
			get
			{
				return this._xmlTypeNamespace;
			}
			set
			{
				this._isType = (value != null);
				this._xmlTypeNamespace = value;
			}
		}

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x06003921 RID: 14625 RVA: 0x000CB1A4 File Offset: 0x000C93A4
		internal bool IsInteropXmlElement
		{
			get
			{
				return this._isElement;
			}
		}

		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x06003922 RID: 14626 RVA: 0x000CB1AC File Offset: 0x000C93AC
		internal bool IsInteropXmlType
		{
			get
			{
				return this._isType;
			}
		}

		// Token: 0x06003923 RID: 14627 RVA: 0x000CB1B4 File Offset: 0x000C93B4
		internal override void SetReflectionObject(object reflectionObject)
		{
			Type type = (Type)reflectionObject;
			if (this._xmlElementName == null)
			{
				this._xmlElementName = type.Name;
			}
			if (this._xmlTypeName == null)
			{
				this._xmlTypeName = type.Name;
			}
			if (this._xmlTypeNamespace == null)
			{
				string assemblyName;
				if (type.Assembly == typeof(object).Assembly)
				{
					assemblyName = string.Empty;
				}
				else
				{
					assemblyName = type.Assembly.GetName().Name;
				}
				this._xmlTypeNamespace = SoapServices.CodeXmlNamespaceForClrTypeNamespace(type.Namespace, assemblyName);
			}
			if (this._xmlNamespace == null)
			{
				this._xmlNamespace = this._xmlTypeNamespace;
			}
		}

		// Token: 0x04002612 RID: 9746
		private SoapOption _soapOption;

		// Token: 0x04002613 RID: 9747
		private bool _useAttribute;

		// Token: 0x04002614 RID: 9748
		private string _xmlElementName;

		// Token: 0x04002615 RID: 9749
		private XmlFieldOrderOption _xmlFieldOrder;

		// Token: 0x04002616 RID: 9750
		private string _xmlNamespace;

		// Token: 0x04002617 RID: 9751
		private string _xmlTypeName;

		// Token: 0x04002618 RID: 9752
		private string _xmlTypeNamespace;

		// Token: 0x04002619 RID: 9753
		private bool _isType;

		// Token: 0x0400261A RID: 9754
		private bool _isElement;
	}
}
