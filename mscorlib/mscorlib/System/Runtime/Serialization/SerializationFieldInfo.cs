using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.Remoting.Metadata;
using System.Security;
using System.Threading;

namespace System.Runtime.Serialization
{
	// Token: 0x02000671 RID: 1649
	internal sealed class SerializationFieldInfo : FieldInfo
	{
		// Token: 0x1700096F RID: 2415
		// (get) Token: 0x06003D6B RID: 15723 RVA: 0x000D4A68 File Offset: 0x000D2C68
		public override Module Module
		{
			get
			{
				return this.m_field.Module;
			}
		}

		// Token: 0x17000970 RID: 2416
		// (get) Token: 0x06003D6C RID: 15724 RVA: 0x000D4A75 File Offset: 0x000D2C75
		public override int MetadataToken
		{
			get
			{
				return this.m_field.MetadataToken;
			}
		}

		// Token: 0x06003D6D RID: 15725 RVA: 0x000D4A82 File Offset: 0x000D2C82
		internal SerializationFieldInfo(RuntimeFieldInfo field, string namePrefix)
		{
			this.m_field = field;
			this.m_serializationName = namePrefix + "+" + this.m_field.Name;
		}

		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x06003D6E RID: 15726 RVA: 0x000D4AAD File Offset: 0x000D2CAD
		public override string Name
		{
			get
			{
				return this.m_serializationName;
			}
		}

		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x06003D6F RID: 15727 RVA: 0x000D4AB5 File Offset: 0x000D2CB5
		public override Type DeclaringType
		{
			get
			{
				return this.m_field.DeclaringType;
			}
		}

		// Token: 0x17000973 RID: 2419
		// (get) Token: 0x06003D70 RID: 15728 RVA: 0x000D4AC2 File Offset: 0x000D2CC2
		public override Type ReflectedType
		{
			get
			{
				return this.m_field.ReflectedType;
			}
		}

		// Token: 0x06003D71 RID: 15729 RVA: 0x000D4ACF File Offset: 0x000D2CCF
		public override object[] GetCustomAttributes(bool inherit)
		{
			return this.m_field.GetCustomAttributes(inherit);
		}

		// Token: 0x06003D72 RID: 15730 RVA: 0x000D4ADD File Offset: 0x000D2CDD
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return this.m_field.GetCustomAttributes(attributeType, inherit);
		}

		// Token: 0x06003D73 RID: 15731 RVA: 0x000D4AEC File Offset: 0x000D2CEC
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return this.m_field.IsDefined(attributeType, inherit);
		}

		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x06003D74 RID: 15732 RVA: 0x000D4AFB File Offset: 0x000D2CFB
		public override Type FieldType
		{
			get
			{
				return this.m_field.FieldType;
			}
		}

		// Token: 0x06003D75 RID: 15733 RVA: 0x000D4B08 File Offset: 0x000D2D08
		public override object GetValue(object obj)
		{
			return this.m_field.GetValue(obj);
		}

		// Token: 0x06003D76 RID: 15734 RVA: 0x000D4B18 File Offset: 0x000D2D18
		[SecurityCritical]
		internal object InternalGetValue(object obj)
		{
			RtFieldInfo field = this.m_field;
			if (field != null)
			{
				field.CheckConsistency(obj);
				return field.UnsafeGetValue(obj);
			}
			return this.m_field.GetValue(obj);
		}

		// Token: 0x06003D77 RID: 15735 RVA: 0x000D4B50 File Offset: 0x000D2D50
		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture)
		{
			this.m_field.SetValue(obj, value, invokeAttr, binder, culture);
		}

		// Token: 0x06003D78 RID: 15736 RVA: 0x000D4B64 File Offset: 0x000D2D64
		[SecurityCritical]
		internal void InternalSetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture)
		{
			RtFieldInfo field = this.m_field;
			if (field != null)
			{
				field.CheckConsistency(obj);
				field.UnsafeSetValue(obj, value, invokeAttr, binder, culture);
				return;
			}
			this.m_field.SetValue(obj, value, invokeAttr, binder, culture);
		}

		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x06003D79 RID: 15737 RVA: 0x000D4BA8 File Offset: 0x000D2DA8
		internal RuntimeFieldInfo FieldInfo
		{
			get
			{
				return this.m_field;
			}
		}

		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x06003D7A RID: 15738 RVA: 0x000D4BB0 File Offset: 0x000D2DB0
		public override RuntimeFieldHandle FieldHandle
		{
			get
			{
				return this.m_field.FieldHandle;
			}
		}

		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x06003D7B RID: 15739 RVA: 0x000D4BBD File Offset: 0x000D2DBD
		public override FieldAttributes Attributes
		{
			get
			{
				return this.m_field.Attributes;
			}
		}

		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x06003D7C RID: 15740 RVA: 0x000D4BCC File Offset: 0x000D2DCC
		internal RemotingFieldCachedData RemotingCache
		{
			get
			{
				RemotingFieldCachedData remotingFieldCachedData = this.m_cachedData;
				if (remotingFieldCachedData == null)
				{
					remotingFieldCachedData = new RemotingFieldCachedData(this);
					RemotingFieldCachedData remotingFieldCachedData2 = Interlocked.CompareExchange<RemotingFieldCachedData>(ref this.m_cachedData, remotingFieldCachedData, null);
					if (remotingFieldCachedData2 != null)
					{
						remotingFieldCachedData = remotingFieldCachedData2;
					}
				}
				return remotingFieldCachedData;
			}
		}

		// Token: 0x04002782 RID: 10114
		internal const string FakeNameSeparatorString = "+";

		// Token: 0x04002783 RID: 10115
		private RuntimeFieldInfo m_field;

		// Token: 0x04002784 RID: 10116
		private string m_serializationName;

		// Token: 0x04002785 RID: 10117
		private RemotingFieldCachedData m_cachedData;
	}
}
