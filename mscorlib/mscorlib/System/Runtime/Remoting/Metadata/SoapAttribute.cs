﻿using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata
{
	/// <summary>Provides default functionality for all SOAP attributes.</summary>
	// Token: 0x020005D8 RID: 1496
	[ComVisible(true)]
	public class SoapAttribute : Attribute
	{
		/// <summary>Gets or sets a value indicating whether the type must be nested during SOAP serialization.</summary>
		/// <returns>
		///   <see langword="true" /> if the target object must be nested during SOAP serialization; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x060038F5 RID: 14581 RVA: 0x000CAF6A File Offset: 0x000C916A
		// (set) Token: 0x060038F6 RID: 14582 RVA: 0x000CAF72 File Offset: 0x000C9172
		public virtual bool Embedded
		{
			get
			{
				return this._nested;
			}
			set
			{
				this._nested = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the target of the current attribute will be serialized as an XML attribute instead of an XML field.</summary>
		/// <returns>
		///   <see langword="true" /> if the target object of the current attribute must be serialized as an XML attribute; <see langword="false" /> if the target object must be serialized as a subelement.</returns>
		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x060038F7 RID: 14583 RVA: 0x000CAF7B File Offset: 0x000C917B
		// (set) Token: 0x060038F8 RID: 14584 RVA: 0x000CAF83 File Offset: 0x000C9183
		public virtual bool UseAttribute
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

		/// <summary>Gets or sets the XML namespace name.</summary>
		/// <returns>The XML namespace name under which the target of the current attribute is serialized.</returns>
		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x060038F9 RID: 14585 RVA: 0x000CAF8C File Offset: 0x000C918C
		// (set) Token: 0x060038FA RID: 14586 RVA: 0x000CAF94 File Offset: 0x000C9194
		public virtual string XmlNamespace
		{
			get
			{
				return this.ProtXmlNamespace;
			}
			set
			{
				this.ProtXmlNamespace = value;
			}
		}

		// Token: 0x060038FB RID: 14587 RVA: 0x000CAF9D File Offset: 0x000C919D
		internal virtual void SetReflectionObject(object reflectionObject)
		{
			this.ReflectInfo = reflectionObject;
		}

		// Token: 0x040025FE RID: 9726
		private bool _nested;

		// Token: 0x040025FF RID: 9727
		private bool _useAttribute;

		/// <summary>The XML namespace to which the target of the current SOAP attribute is serialized.</summary>
		// Token: 0x04002600 RID: 9728
		protected string ProtXmlNamespace;

		/// <summary>A reflection object used by attribute classes derived from the <see cref="T:System.Runtime.Remoting.Metadata.SoapAttribute" /> class to set XML serialization information.</summary>
		// Token: 0x04002601 RID: 9729
		protected object ReflectInfo;
	}
}
