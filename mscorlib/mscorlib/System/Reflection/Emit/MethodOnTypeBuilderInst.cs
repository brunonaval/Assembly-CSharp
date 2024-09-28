﻿using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Reflection.Emit
{
	// Token: 0x02000935 RID: 2357
	[StructLayout(LayoutKind.Sequential)]
	internal class MethodOnTypeBuilderInst : MethodInfo
	{
		// Token: 0x06005149 RID: 20809 RVA: 0x000FE4F5 File Offset: 0x000FC6F5
		public MethodOnTypeBuilderInst(TypeBuilderInstantiation instantiation, MethodInfo base_method)
		{
			this.instantiation = instantiation;
			this.base_method = base_method;
		}

		// Token: 0x0600514A RID: 20810 RVA: 0x000FE50C File Offset: 0x000FC70C
		internal MethodOnTypeBuilderInst(MethodOnTypeBuilderInst gmd, Type[] typeArguments)
		{
			this.instantiation = gmd.instantiation;
			this.base_method = gmd.base_method;
			this.method_arguments = new Type[typeArguments.Length];
			typeArguments.CopyTo(this.method_arguments, 0);
			this.generic_method_definition = gmd;
		}

		// Token: 0x0600514B RID: 20811 RVA: 0x000FE55C File Offset: 0x000FC75C
		internal MethodOnTypeBuilderInst(MethodInfo method, Type[] typeArguments)
		{
			this.instantiation = method.DeclaringType;
			this.base_method = MethodOnTypeBuilderInst.ExtractBaseMethod(method);
			this.method_arguments = new Type[typeArguments.Length];
			typeArguments.CopyTo(this.method_arguments, 0);
			if (this.base_method != method)
			{
				this.generic_method_definition = method;
			}
		}

		// Token: 0x0600514C RID: 20812 RVA: 0x000FE5B8 File Offset: 0x000FC7B8
		private static MethodInfo ExtractBaseMethod(MethodInfo info)
		{
			if (info is MethodBuilder)
			{
				return info;
			}
			if (info is MethodOnTypeBuilderInst)
			{
				return ((MethodOnTypeBuilderInst)info).base_method;
			}
			if (info.IsGenericMethod)
			{
				info = info.GetGenericMethodDefinition();
			}
			Type declaringType = info.DeclaringType;
			if (!declaringType.IsGenericType || declaringType.IsGenericTypeDefinition)
			{
				return info;
			}
			return (MethodInfo)declaringType.Module.ResolveMethod(info.MetadataToken);
		}

		// Token: 0x0600514D RID: 20813 RVA: 0x000FE622 File Offset: 0x000FC822
		internal Type[] GetTypeArgs()
		{
			if (!this.instantiation.IsGenericType || this.instantiation.IsGenericParameter)
			{
				return null;
			}
			return this.instantiation.GetGenericArguments();
		}

		// Token: 0x0600514E RID: 20814 RVA: 0x000FE64C File Offset: 0x000FC84C
		internal MethodInfo RuntimeResolve()
		{
			MethodInfo methodInfo = this.instantiation.InternalResolve().GetMethod(this.base_method);
			if (this.method_arguments != null)
			{
				Type[] array = new Type[this.method_arguments.Length];
				for (int i = 0; i < this.method_arguments.Length; i++)
				{
					array[i] = this.method_arguments[i].InternalResolve();
				}
				methodInfo = methodInfo.MakeGenericMethod(array);
			}
			return methodInfo;
		}

		// Token: 0x17000D6D RID: 3437
		// (get) Token: 0x0600514F RID: 20815 RVA: 0x000FE6B2 File Offset: 0x000FC8B2
		public override Type DeclaringType
		{
			get
			{
				return this.instantiation;
			}
		}

		// Token: 0x17000D6E RID: 3438
		// (get) Token: 0x06005150 RID: 20816 RVA: 0x000FE6BA File Offset: 0x000FC8BA
		public override string Name
		{
			get
			{
				return this.base_method.Name;
			}
		}

		// Token: 0x17000D6F RID: 3439
		// (get) Token: 0x06005151 RID: 20817 RVA: 0x000FE6B2 File Offset: 0x000FC8B2
		public override Type ReflectedType
		{
			get
			{
				return this.instantiation;
			}
		}

		// Token: 0x17000D70 RID: 3440
		// (get) Token: 0x06005152 RID: 20818 RVA: 0x000FE6C7 File Offset: 0x000FC8C7
		public override Type ReturnType
		{
			get
			{
				return this.base_method.ReturnType;
			}
		}

		// Token: 0x17000D71 RID: 3441
		// (get) Token: 0x06005153 RID: 20819 RVA: 0x000FE6D4 File Offset: 0x000FC8D4
		public override Module Module
		{
			get
			{
				return this.base_method.Module;
			}
		}

		// Token: 0x06005154 RID: 20820 RVA: 0x000472CC File Offset: 0x000454CC
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06005155 RID: 20821 RVA: 0x000472CC File Offset: 0x000454CC
		public override object[] GetCustomAttributes(bool inherit)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06005156 RID: 20822 RVA: 0x000472CC File Offset: 0x000454CC
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06005157 RID: 20823 RVA: 0x000FE6E4 File Offset: 0x000FC8E4
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(this.ReturnType.ToString());
			stringBuilder.Append(" ");
			stringBuilder.Append(this.base_method.Name);
			stringBuilder.Append("(");
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		// Token: 0x06005158 RID: 20824 RVA: 0x000FE73C File Offset: 0x000FC93C
		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			return this.base_method.GetMethodImplementationFlags();
		}

		// Token: 0x06005159 RID: 20825 RVA: 0x000F18BE File Offset: 0x000EFABE
		public override ParameterInfo[] GetParameters()
		{
			return this.GetParametersInternal();
		}

		// Token: 0x0600515A RID: 20826 RVA: 0x000472CC File Offset: 0x000454CC
		internal override ParameterInfo[] GetParametersInternal()
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000D72 RID: 3442
		// (get) Token: 0x0600515B RID: 20827 RVA: 0x000F8762 File Offset: 0x000F6962
		public override int MetadataToken
		{
			get
			{
				return base.MetadataToken;
			}
		}

		// Token: 0x0600515C RID: 20828 RVA: 0x000FE749 File Offset: 0x000FC949
		internal override int GetParametersCount()
		{
			return this.base_method.GetParametersCount();
		}

		// Token: 0x0600515D RID: 20829 RVA: 0x000472CC File Offset: 0x000454CC
		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000D73 RID: 3443
		// (get) Token: 0x0600515E RID: 20830 RVA: 0x000472CC File Offset: 0x000454CC
		public override RuntimeMethodHandle MethodHandle
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000D74 RID: 3444
		// (get) Token: 0x0600515F RID: 20831 RVA: 0x000FE756 File Offset: 0x000FC956
		public override MethodAttributes Attributes
		{
			get
			{
				return this.base_method.Attributes;
			}
		}

		// Token: 0x17000D75 RID: 3445
		// (get) Token: 0x06005160 RID: 20832 RVA: 0x000FE763 File Offset: 0x000FC963
		public override CallingConventions CallingConvention
		{
			get
			{
				return this.base_method.CallingConvention;
			}
		}

		// Token: 0x06005161 RID: 20833 RVA: 0x000FE770 File Offset: 0x000FC970
		public override MethodInfo MakeGenericMethod(params Type[] methodInstantiation)
		{
			if (!this.base_method.IsGenericMethodDefinition || this.method_arguments != null)
			{
				throw new InvalidOperationException("Method is not a generic method definition");
			}
			if (methodInstantiation == null)
			{
				throw new ArgumentNullException("methodInstantiation");
			}
			if (this.base_method.GetGenericArguments().Length != methodInstantiation.Length)
			{
				throw new ArgumentException("Incorrect length", "methodInstantiation");
			}
			for (int i = 0; i < methodInstantiation.Length; i++)
			{
				if (methodInstantiation[i] == null)
				{
					throw new ArgumentNullException("methodInstantiation");
				}
			}
			return new MethodOnTypeBuilderInst(this, methodInstantiation);
		}

		// Token: 0x06005162 RID: 20834 RVA: 0x000FE7FC File Offset: 0x000FC9FC
		public override Type[] GetGenericArguments()
		{
			if (!this.base_method.IsGenericMethodDefinition)
			{
				return null;
			}
			Type[] array = this.method_arguments ?? this.base_method.GetGenericArguments();
			Type[] array2 = new Type[array.Length];
			array.CopyTo(array2, 0);
			return array2;
		}

		// Token: 0x06005163 RID: 20835 RVA: 0x000FE83E File Offset: 0x000FCA3E
		public override MethodInfo GetGenericMethodDefinition()
		{
			return this.generic_method_definition ?? this.base_method;
		}

		// Token: 0x17000D76 RID: 3446
		// (get) Token: 0x06005164 RID: 20836 RVA: 0x000FE850 File Offset: 0x000FCA50
		public override bool ContainsGenericParameters
		{
			get
			{
				if (this.base_method.ContainsGenericParameters)
				{
					return true;
				}
				if (!this.base_method.IsGenericMethodDefinition)
				{
					throw new NotSupportedException();
				}
				if (this.method_arguments == null)
				{
					return true;
				}
				Type[] array = this.method_arguments;
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].ContainsGenericParameters)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000D77 RID: 3447
		// (get) Token: 0x06005165 RID: 20837 RVA: 0x000FE8AB File Offset: 0x000FCAAB
		public override bool IsGenericMethodDefinition
		{
			get
			{
				return this.base_method.IsGenericMethodDefinition && this.method_arguments == null;
			}
		}

		// Token: 0x17000D78 RID: 3448
		// (get) Token: 0x06005166 RID: 20838 RVA: 0x000FE8C5 File Offset: 0x000FCAC5
		public override bool IsGenericMethod
		{
			get
			{
				return this.base_method.IsGenericMethodDefinition;
			}
		}

		// Token: 0x06005167 RID: 20839 RVA: 0x000472CC File Offset: 0x000454CC
		public override MethodInfo GetBaseDefinition()
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000D79 RID: 3449
		// (get) Token: 0x06005168 RID: 20840 RVA: 0x000472CC File Offset: 0x000454CC
		public override ParameterInfo ReturnParameter
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000D7A RID: 3450
		// (get) Token: 0x06005169 RID: 20841 RVA: 0x000472CC File Offset: 0x000454CC
		public override ICustomAttributeProvider ReturnTypeCustomAttributes
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x040031D2 RID: 12754
		private Type instantiation;

		// Token: 0x040031D3 RID: 12755
		private MethodInfo base_method;

		// Token: 0x040031D4 RID: 12756
		private Type[] method_arguments;

		// Token: 0x040031D5 RID: 12757
		private MethodInfo generic_method_definition;
	}
}
