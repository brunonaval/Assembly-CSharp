using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using Mono;

namespace System.Reflection
{
	// Token: 0x02000901 RID: 2305
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	internal class RuntimePropertyInfo : PropertyInfo, ISerializable
	{
		// Token: 0x06004E0B RID: 19979
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void get_property_info(RuntimePropertyInfo prop, ref MonoPropertyInfo info, PInfo req_info);

		// Token: 0x06004E0C RID: 19980
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Type[] GetTypeModifiers(RuntimePropertyInfo prop, bool optional);

		// Token: 0x06004E0D RID: 19981
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern object get_default_value(RuntimePropertyInfo prop);

		// Token: 0x17000CBC RID: 3260
		// (get) Token: 0x06004E0E RID: 19982 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		internal BindingFlags BindingFlags
		{
			get
			{
				return BindingFlags.Default;
			}
		}

		// Token: 0x17000CBD RID: 3261
		// (get) Token: 0x06004E0F RID: 19983 RVA: 0x000F5717 File Offset: 0x000F3917
		public override Module Module
		{
			get
			{
				return this.GetRuntimeModule();
			}
		}

		// Token: 0x06004E10 RID: 19984 RVA: 0x000F39B2 File Offset: 0x000F1BB2
		internal RuntimeType GetDeclaringTypeInternal()
		{
			return (RuntimeType)this.DeclaringType;
		}

		// Token: 0x17000CBE RID: 3262
		// (get) Token: 0x06004E11 RID: 19985 RVA: 0x000F39BF File Offset: 0x000F1BBF
		private RuntimeType ReflectedTypeInternal
		{
			get
			{
				return (RuntimeType)this.ReflectedType;
			}
		}

		// Token: 0x06004E12 RID: 19986 RVA: 0x000F571F File Offset: 0x000F391F
		internal RuntimeModule GetRuntimeModule()
		{
			return this.GetDeclaringTypeInternal().GetRuntimeModule();
		}

		// Token: 0x06004E13 RID: 19987 RVA: 0x000F572C File Offset: 0x000F392C
		public override string ToString()
		{
			return this.FormatNameAndSig(false);
		}

		// Token: 0x06004E14 RID: 19988 RVA: 0x000F5738 File Offset: 0x000F3938
		private string FormatNameAndSig(bool serialization)
		{
			StringBuilder stringBuilder = new StringBuilder(this.PropertyType.FormatTypeName(serialization));
			stringBuilder.Append(" ");
			stringBuilder.Append(this.Name);
			ParameterInfo[] indexParameters = this.GetIndexParameters();
			if (indexParameters.Length != 0)
			{
				stringBuilder.Append(" [");
				RuntimeParameterInfo.FormatParameters(stringBuilder, indexParameters, (CallingConventions)0, serialization);
				stringBuilder.Append("]");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06004E15 RID: 19989 RVA: 0x000F57A2 File Offset: 0x000F39A2
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			MemberInfoSerializationHolder.GetSerializationInfo(info, this.Name, this.ReflectedTypeInternal, this.ToString(), this.SerializationToString(), MemberTypes.Property, null);
		}

		// Token: 0x06004E16 RID: 19990 RVA: 0x000F57D3 File Offset: 0x000F39D3
		internal string SerializationToString()
		{
			return this.FormatNameAndSig(true);
		}

		// Token: 0x06004E17 RID: 19991 RVA: 0x000F57DC File Offset: 0x000F39DC
		private void CachePropertyInfo(PInfo flags)
		{
			if ((this.cached & flags) != flags)
			{
				RuntimePropertyInfo.get_property_info(this, ref this.info, flags);
				this.cached |= flags;
			}
		}

		// Token: 0x17000CBF RID: 3263
		// (get) Token: 0x06004E18 RID: 19992 RVA: 0x000F5804 File Offset: 0x000F3A04
		public override PropertyAttributes Attributes
		{
			get
			{
				this.CachePropertyInfo(PInfo.Attributes);
				return this.info.attrs;
			}
		}

		// Token: 0x17000CC0 RID: 3264
		// (get) Token: 0x06004E19 RID: 19993 RVA: 0x000F5818 File Offset: 0x000F3A18
		public override bool CanRead
		{
			get
			{
				this.CachePropertyInfo(PInfo.GetMethod);
				return this.info.get_method != null;
			}
		}

		// Token: 0x17000CC1 RID: 3265
		// (get) Token: 0x06004E1A RID: 19994 RVA: 0x000F5832 File Offset: 0x000F3A32
		public override bool CanWrite
		{
			get
			{
				this.CachePropertyInfo(PInfo.SetMethod);
				return this.info.set_method != null;
			}
		}

		// Token: 0x17000CC2 RID: 3266
		// (get) Token: 0x06004E1B RID: 19995 RVA: 0x000F584C File Offset: 0x000F3A4C
		public override Type PropertyType
		{
			get
			{
				this.CachePropertyInfo(PInfo.GetMethod | PInfo.SetMethod);
				if (this.info.get_method != null)
				{
					return this.info.get_method.ReturnType;
				}
				ParameterInfo[] parametersInternal = this.info.set_method.GetParametersInternal();
				return parametersInternal[parametersInternal.Length - 1].ParameterType;
			}
		}

		// Token: 0x17000CC3 RID: 3267
		// (get) Token: 0x06004E1C RID: 19996 RVA: 0x000F589F File Offset: 0x000F3A9F
		public override Type ReflectedType
		{
			get
			{
				this.CachePropertyInfo(PInfo.ReflectedType);
				return this.info.parent;
			}
		}

		// Token: 0x17000CC4 RID: 3268
		// (get) Token: 0x06004E1D RID: 19997 RVA: 0x000F58B3 File Offset: 0x000F3AB3
		public override Type DeclaringType
		{
			get
			{
				this.CachePropertyInfo(PInfo.DeclaringType);
				return this.info.declaring_type;
			}
		}

		// Token: 0x17000CC5 RID: 3269
		// (get) Token: 0x06004E1E RID: 19998 RVA: 0x000F58C8 File Offset: 0x000F3AC8
		public override string Name
		{
			get
			{
				this.CachePropertyInfo(PInfo.Name);
				return this.info.name;
			}
		}

		// Token: 0x06004E1F RID: 19999 RVA: 0x000F58E0 File Offset: 0x000F3AE0
		public override MethodInfo[] GetAccessors(bool nonPublic)
		{
			int num = 0;
			int num2 = 0;
			this.CachePropertyInfo(PInfo.GetMethod | PInfo.SetMethod);
			if (this.info.set_method != null && (nonPublic || this.info.set_method.IsPublic))
			{
				num2 = 1;
			}
			if (this.info.get_method != null && (nonPublic || this.info.get_method.IsPublic))
			{
				num = 1;
			}
			MethodInfo[] array = new MethodInfo[num + num2];
			int num3 = 0;
			if (num2 != 0)
			{
				array[num3++] = this.info.set_method;
			}
			if (num != 0)
			{
				array[num3++] = this.info.get_method;
			}
			return array;
		}

		// Token: 0x06004E20 RID: 20000 RVA: 0x000F5982 File Offset: 0x000F3B82
		public override MethodInfo GetGetMethod(bool nonPublic)
		{
			this.CachePropertyInfo(PInfo.GetMethod);
			if (this.info.get_method != null && (nonPublic || this.info.get_method.IsPublic))
			{
				return this.info.get_method;
			}
			return null;
		}

		// Token: 0x06004E21 RID: 20001 RVA: 0x000F59C0 File Offset: 0x000F3BC0
		public override ParameterInfo[] GetIndexParameters()
		{
			this.CachePropertyInfo(PInfo.GetMethod | PInfo.SetMethod);
			ParameterInfo[] parametersInternal;
			int num;
			if (this.info.get_method != null)
			{
				parametersInternal = this.info.get_method.GetParametersInternal();
				num = parametersInternal.Length;
			}
			else
			{
				if (!(this.info.set_method != null))
				{
					return EmptyArray<ParameterInfo>.Value;
				}
				parametersInternal = this.info.set_method.GetParametersInternal();
				num = parametersInternal.Length - 1;
			}
			ParameterInfo[] array = new ParameterInfo[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = RuntimeParameterInfo.New(parametersInternal[i], this);
			}
			return array;
		}

		// Token: 0x06004E22 RID: 20002 RVA: 0x000F5A50 File Offset: 0x000F3C50
		public override MethodInfo GetSetMethod(bool nonPublic)
		{
			this.CachePropertyInfo(PInfo.SetMethod);
			if (this.info.set_method != null && (nonPublic || this.info.set_method.IsPublic))
			{
				return this.info.set_method;
			}
			return null;
		}

		// Token: 0x06004E23 RID: 20003 RVA: 0x000F5A8E File Offset: 0x000F3C8E
		public override object GetConstantValue()
		{
			return RuntimePropertyInfo.get_default_value(this);
		}

		// Token: 0x06004E24 RID: 20004 RVA: 0x000F5A8E File Offset: 0x000F3C8E
		public override object GetRawConstantValue()
		{
			return RuntimePropertyInfo.get_default_value(this);
		}

		// Token: 0x06004E25 RID: 20005 RVA: 0x000F5A96 File Offset: 0x000F3C96
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return MonoCustomAttrs.IsDefined(this, attributeType, false);
		}

		// Token: 0x06004E26 RID: 20006 RVA: 0x000F546C File Offset: 0x000F366C
		public override object[] GetCustomAttributes(bool inherit)
		{
			return MonoCustomAttrs.GetCustomAttributes(this, false);
		}

		// Token: 0x06004E27 RID: 20007 RVA: 0x000F5475 File Offset: 0x000F3675
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return MonoCustomAttrs.GetCustomAttributes(this, attributeType, false);
		}

		// Token: 0x06004E28 RID: 20008 RVA: 0x000F5AA0 File Offset: 0x000F3CA0
		private static object GetterAdapterFrame<T, R>(RuntimePropertyInfo.Getter<T, R> getter, object obj)
		{
			return getter((T)((object)obj));
		}

		// Token: 0x06004E29 RID: 20009 RVA: 0x000F5AB3 File Offset: 0x000F3CB3
		private static object StaticGetterAdapterFrame<R>(RuntimePropertyInfo.StaticGetter<R> getter, object obj)
		{
			return getter();
		}

		// Token: 0x06004E2A RID: 20010 RVA: 0x000F5AC0 File Offset: 0x000F3CC0
		private static RuntimePropertyInfo.GetterAdapter CreateGetterDelegate(MethodInfo method)
		{
			Type[] typeArguments;
			Type typeFromHandle;
			string name;
			if (method.IsStatic)
			{
				typeArguments = new Type[]
				{
					method.ReturnType
				};
				typeFromHandle = typeof(RuntimePropertyInfo.StaticGetter<>);
				name = "StaticGetterAdapterFrame";
			}
			else
			{
				typeArguments = new Type[]
				{
					method.DeclaringType,
					method.ReturnType
				};
				typeFromHandle = typeof(RuntimePropertyInfo.Getter<, >);
				name = "GetterAdapterFrame";
			}
			object firstArgument = Delegate.CreateDelegate(typeFromHandle.MakeGenericType(typeArguments), method);
			MethodInfo methodInfo = typeof(RuntimePropertyInfo).GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic);
			methodInfo = methodInfo.MakeGenericMethod(typeArguments);
			return (RuntimePropertyInfo.GetterAdapter)Delegate.CreateDelegate(typeof(RuntimePropertyInfo.GetterAdapter), firstArgument, methodInfo, true);
		}

		// Token: 0x06004E2B RID: 20011 RVA: 0x000F5B68 File Offset: 0x000F3D68
		public override object GetValue(object obj, object[] index)
		{
			if (index == null || index.Length == 0)
			{
				if (this.cached_getter == null)
				{
					MethodInfo getMethod = this.GetGetMethod(true);
					if (getMethod == null)
					{
						throw new ArgumentException("Get Method not found for '" + this.Name + "'");
					}
					if (this.DeclaringType.IsValueType || this.PropertyType.IsByRef || getMethod.ContainsGenericParameters)
					{
						goto IL_97;
					}
					this.cached_getter = RuntimePropertyInfo.CreateGetterDelegate(getMethod);
					try
					{
						return this.cached_getter(obj);
					}
					catch (Exception inner)
					{
						throw new TargetInvocationException(inner);
					}
				}
				try
				{
					return this.cached_getter(obj);
				}
				catch (Exception inner2)
				{
					throw new TargetInvocationException(inner2);
				}
			}
			IL_97:
			return this.GetValue(obj, BindingFlags.Default, null, index, null);
		}

		// Token: 0x06004E2C RID: 20012 RVA: 0x000F5C38 File Offset: 0x000F3E38
		public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
		{
			object result = null;
			MethodInfo getMethod = this.GetGetMethod(true);
			if (getMethod == null)
			{
				throw new ArgumentException("Get Method not found for '" + this.Name + "'");
			}
			try
			{
				if (index == null || index.Length == 0)
				{
					result = getMethod.Invoke(obj, invokeAttr, binder, null, culture);
				}
				else
				{
					result = getMethod.Invoke(obj, invokeAttr, binder, index, culture);
				}
			}
			catch (SecurityException inner)
			{
				throw new TargetInvocationException(inner);
			}
			return result;
		}

		// Token: 0x06004E2D RID: 20013 RVA: 0x000F5CB4 File Offset: 0x000F3EB4
		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
		{
			MethodInfo setMethod = this.GetSetMethod(true);
			if (setMethod == null)
			{
				throw new ArgumentException("Set Method not found for '" + this.Name + "'");
			}
			object[] array;
			if (index == null || index.Length == 0)
			{
				array = new object[]
				{
					value
				};
			}
			else
			{
				int num = index.Length;
				array = new object[num + 1];
				index.CopyTo(array, 0);
				array[num] = value;
			}
			setMethod.Invoke(obj, invokeAttr, binder, array, culture);
		}

		// Token: 0x06004E2E RID: 20014 RVA: 0x000F5D2C File Offset: 0x000F3F2C
		public override Type[] GetOptionalCustomModifiers()
		{
			return this.GetCustomModifiers(true);
		}

		// Token: 0x06004E2F RID: 20015 RVA: 0x000F5D35 File Offset: 0x000F3F35
		public override Type[] GetRequiredCustomModifiers()
		{
			return this.GetCustomModifiers(false);
		}

		// Token: 0x06004E30 RID: 20016 RVA: 0x000F5D3E File Offset: 0x000F3F3E
		private Type[] GetCustomModifiers(bool optional)
		{
			return RuntimePropertyInfo.GetTypeModifiers(this, optional) ?? Type.EmptyTypes;
		}

		// Token: 0x06004E31 RID: 20017 RVA: 0x000F3C18 File Offset: 0x000F1E18
		public override IList<CustomAttributeData> GetCustomAttributesData()
		{
			return CustomAttributeData.GetCustomAttributes(this);
		}

		// Token: 0x06004E32 RID: 20018 RVA: 0x000F5D50 File Offset: 0x000F3F50
		public sealed override bool HasSameMetadataDefinitionAs(MemberInfo other)
		{
			return base.HasSameMetadataDefinitionAsCore<RuntimePropertyInfo>(other);
		}

		// Token: 0x17000CC6 RID: 3270
		// (get) Token: 0x06004E33 RID: 20019 RVA: 0x000F5D59 File Offset: 0x000F3F59
		public override int MetadataToken
		{
			get
			{
				return RuntimePropertyInfo.get_metadata_token(this);
			}
		}

		// Token: 0x06004E34 RID: 20020
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int get_metadata_token(RuntimePropertyInfo monoProperty);

		// Token: 0x06004E35 RID: 20021
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern PropertyInfo internal_from_handle_type(IntPtr event_handle, IntPtr type_handle);

		// Token: 0x06004E36 RID: 20022 RVA: 0x000F5D64 File Offset: 0x000F3F64
		internal static PropertyInfo GetPropertyFromHandle(RuntimePropertyHandle handle, RuntimeTypeHandle reflectedType)
		{
			if (handle.Value == IntPtr.Zero)
			{
				throw new ArgumentException("The handle is invalid.");
			}
			PropertyInfo propertyInfo = RuntimePropertyInfo.internal_from_handle_type(handle.Value, reflectedType.Value);
			if (propertyInfo == null)
			{
				throw new ArgumentException("The property handle and the type handle are incompatible.");
			}
			return propertyInfo;
		}

		// Token: 0x04003079 RID: 12409
		internal IntPtr klass;

		// Token: 0x0400307A RID: 12410
		internal IntPtr prop;

		// Token: 0x0400307B RID: 12411
		private MonoPropertyInfo info;

		// Token: 0x0400307C RID: 12412
		private PInfo cached;

		// Token: 0x0400307D RID: 12413
		private RuntimePropertyInfo.GetterAdapter cached_getter;

		// Token: 0x02000902 RID: 2306
		// (Invoke) Token: 0x06004E39 RID: 20025
		private delegate object GetterAdapter(object _this);

		// Token: 0x02000903 RID: 2307
		// (Invoke) Token: 0x06004E3D RID: 20029
		private delegate R Getter<T, R>(T _this);

		// Token: 0x02000904 RID: 2308
		// (Invoke) Token: 0x06004E41 RID: 20033
		private delegate R StaticGetter<R>();
	}
}
