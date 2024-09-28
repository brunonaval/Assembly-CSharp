using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Reflection.Emit
{
	// Token: 0x02000948 RID: 2376
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class TypeBuilderInstantiation : TypeInfo
	{
		// Token: 0x06005310 RID: 21264 RVA: 0x00105617 File Offset: 0x00103817
		internal TypeBuilderInstantiation()
		{
			throw new InvalidOperationException();
		}

		// Token: 0x06005311 RID: 21265 RVA: 0x00105624 File Offset: 0x00103824
		internal TypeBuilderInstantiation(Type tb, Type[] args)
		{
			this.generic_type = tb;
			this.type_arguments = args;
		}

		// Token: 0x06005312 RID: 21266 RVA: 0x0010563C File Offset: 0x0010383C
		internal override Type InternalResolve()
		{
			Type type = this.generic_type.InternalResolve();
			Type[] array = new Type[this.type_arguments.Length];
			for (int i = 0; i < this.type_arguments.Length; i++)
			{
				array[i] = this.type_arguments[i].InternalResolve();
			}
			return type.MakeGenericType(array);
		}

		// Token: 0x06005313 RID: 21267 RVA: 0x00105690 File Offset: 0x00103890
		internal override Type RuntimeResolve()
		{
			TypeBuilder typeBuilder = this.generic_type as TypeBuilder;
			if (typeBuilder != null && !typeBuilder.IsCreated())
			{
				AppDomain.CurrentDomain.DoTypeBuilderResolve(typeBuilder);
			}
			for (int i = 0; i < this.type_arguments.Length; i++)
			{
				TypeBuilder typeBuilder2 = this.type_arguments[i] as TypeBuilder;
				if (typeBuilder2 != null && !typeBuilder2.IsCreated())
				{
					AppDomain.CurrentDomain.DoTypeBuilderResolve(typeBuilder2);
				}
			}
			return this.InternalResolve();
		}

		// Token: 0x17000DC0 RID: 3520
		// (get) Token: 0x06005314 RID: 21268 RVA: 0x00105700 File Offset: 0x00103900
		internal bool IsCreated
		{
			get
			{
				TypeBuilder typeBuilder = this.generic_type as TypeBuilder;
				return !(typeBuilder != null) || typeBuilder.is_created;
			}
		}

		// Token: 0x06005315 RID: 21269 RVA: 0x0010572A File Offset: 0x0010392A
		private Type GetParentType()
		{
			return this.InflateType(this.generic_type.BaseType);
		}

		// Token: 0x06005316 RID: 21270 RVA: 0x0010573D File Offset: 0x0010393D
		internal Type InflateType(Type type)
		{
			return TypeBuilderInstantiation.InflateType(type, this.type_arguments, null);
		}

		// Token: 0x06005317 RID: 21271 RVA: 0x0010574C File Offset: 0x0010394C
		internal Type InflateType(Type type, Type[] method_args)
		{
			return TypeBuilderInstantiation.InflateType(type, this.type_arguments, method_args);
		}

		// Token: 0x06005318 RID: 21272 RVA: 0x0010575C File Offset: 0x0010395C
		internal static Type InflateType(Type type, Type[] type_args, Type[] method_args)
		{
			if (type == null)
			{
				return null;
			}
			if (!type.IsGenericParameter && !type.ContainsGenericParameters)
			{
				return type;
			}
			if (type.IsGenericParameter)
			{
				if (type.DeclaringMethod == null)
				{
					if (type_args != null)
					{
						return type_args[type.GenericParameterPosition];
					}
					return type;
				}
				else
				{
					if (method_args != null)
					{
						return method_args[type.GenericParameterPosition];
					}
					return type;
				}
			}
			else
			{
				if (type.IsPointer)
				{
					return TypeBuilderInstantiation.InflateType(type.GetElementType(), type_args, method_args).MakePointerType();
				}
				if (type.IsByRef)
				{
					return TypeBuilderInstantiation.InflateType(type.GetElementType(), type_args, method_args).MakeByRefType();
				}
				if (!type.IsArray)
				{
					Type[] genericArguments = type.GetGenericArguments();
					for (int i = 0; i < genericArguments.Length; i++)
					{
						genericArguments[i] = TypeBuilderInstantiation.InflateType(genericArguments[i], type_args, method_args);
					}
					return (type.IsGenericTypeDefinition ? type : type.GetGenericTypeDefinition()).MakeGenericType(genericArguments);
				}
				if (type.GetArrayRank() > 1)
				{
					return TypeBuilderInstantiation.InflateType(type.GetElementType(), type_args, method_args).MakeArrayType(type.GetArrayRank());
				}
				if (type.ToString().EndsWith("[*]", StringComparison.Ordinal))
				{
					return TypeBuilderInstantiation.InflateType(type.GetElementType(), type_args, method_args).MakeArrayType(1);
				}
				return TypeBuilderInstantiation.InflateType(type.GetElementType(), type_args, method_args).MakeArrayType();
			}
		}

		// Token: 0x17000DC1 RID: 3521
		// (get) Token: 0x06005319 RID: 21273 RVA: 0x0010588B File Offset: 0x00103A8B
		public override Type BaseType
		{
			get
			{
				return this.generic_type.BaseType;
			}
		}

		// Token: 0x0600531A RID: 21274 RVA: 0x000472CC File Offset: 0x000454CC
		public override Type[] GetInterfaces()
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600531B RID: 21275 RVA: 0x00105898 File Offset: 0x00103A98
		protected override bool IsValueTypeImpl()
		{
			return this.generic_type.IsValueType;
		}

		// Token: 0x0600531C RID: 21276 RVA: 0x001058A8 File Offset: 0x00103AA8
		internal override MethodInfo GetMethod(MethodInfo fromNoninstanciated)
		{
			if (this.methods == null)
			{
				this.methods = new Hashtable();
			}
			if (!this.methods.ContainsKey(fromNoninstanciated))
			{
				this.methods[fromNoninstanciated] = new MethodOnTypeBuilderInst(this, fromNoninstanciated);
			}
			return (MethodInfo)this.methods[fromNoninstanciated];
		}

		// Token: 0x0600531D RID: 21277 RVA: 0x001058FC File Offset: 0x00103AFC
		internal override ConstructorInfo GetConstructor(ConstructorInfo fromNoninstanciated)
		{
			if (this.ctors == null)
			{
				this.ctors = new Hashtable();
			}
			if (!this.ctors.ContainsKey(fromNoninstanciated))
			{
				this.ctors[fromNoninstanciated] = new ConstructorOnTypeBuilderInst(this, fromNoninstanciated);
			}
			return (ConstructorInfo)this.ctors[fromNoninstanciated];
		}

		// Token: 0x0600531E RID: 21278 RVA: 0x00105950 File Offset: 0x00103B50
		internal override FieldInfo GetField(FieldInfo fromNoninstanciated)
		{
			if (this.fields == null)
			{
				this.fields = new Hashtable();
			}
			if (!this.fields.ContainsKey(fromNoninstanciated))
			{
				this.fields[fromNoninstanciated] = new FieldOnTypeBuilderInst(this, fromNoninstanciated);
			}
			return (FieldInfo)this.fields[fromNoninstanciated];
		}

		// Token: 0x0600531F RID: 21279 RVA: 0x000472CC File Offset: 0x000454CC
		public override MethodInfo[] GetMethods(BindingFlags bf)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06005320 RID: 21280 RVA: 0x000472CC File Offset: 0x000454CC
		public override ConstructorInfo[] GetConstructors(BindingFlags bf)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06005321 RID: 21281 RVA: 0x000472CC File Offset: 0x000454CC
		public override FieldInfo[] GetFields(BindingFlags bf)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06005322 RID: 21282 RVA: 0x000472CC File Offset: 0x000454CC
		public override PropertyInfo[] GetProperties(BindingFlags bf)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06005323 RID: 21283 RVA: 0x000472CC File Offset: 0x000454CC
		public override EventInfo[] GetEvents(BindingFlags bf)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06005324 RID: 21284 RVA: 0x000472CC File Offset: 0x000454CC
		public override Type[] GetNestedTypes(BindingFlags bf)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06005325 RID: 21285 RVA: 0x000472CC File Offset: 0x000454CC
		public override bool IsAssignableFrom(Type c)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000DC2 RID: 3522
		// (get) Token: 0x06005326 RID: 21286 RVA: 0x0000270D File Offset: 0x0000090D
		public override Type UnderlyingSystemType
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000DC3 RID: 3523
		// (get) Token: 0x06005327 RID: 21287 RVA: 0x001059A2 File Offset: 0x00103BA2
		public override Assembly Assembly
		{
			get
			{
				return this.generic_type.Assembly;
			}
		}

		// Token: 0x17000DC4 RID: 3524
		// (get) Token: 0x06005328 RID: 21288 RVA: 0x001059AF File Offset: 0x00103BAF
		public override Module Module
		{
			get
			{
				return this.generic_type.Module;
			}
		}

		// Token: 0x17000DC5 RID: 3525
		// (get) Token: 0x06005329 RID: 21289 RVA: 0x001059BC File Offset: 0x00103BBC
		public override string Name
		{
			get
			{
				return this.generic_type.Name;
			}
		}

		// Token: 0x17000DC6 RID: 3526
		// (get) Token: 0x0600532A RID: 21290 RVA: 0x001059C9 File Offset: 0x00103BC9
		public override string Namespace
		{
			get
			{
				return this.generic_type.Namespace;
			}
		}

		// Token: 0x17000DC7 RID: 3527
		// (get) Token: 0x0600532B RID: 21291 RVA: 0x001059D6 File Offset: 0x00103BD6
		public override string FullName
		{
			get
			{
				return this.format_name(true, false);
			}
		}

		// Token: 0x17000DC8 RID: 3528
		// (get) Token: 0x0600532C RID: 21292 RVA: 0x001059E0 File Offset: 0x00103BE0
		public override string AssemblyQualifiedName
		{
			get
			{
				return this.format_name(true, true);
			}
		}

		// Token: 0x17000DC9 RID: 3529
		// (get) Token: 0x0600532D RID: 21293 RVA: 0x000472CC File Offset: 0x000454CC
		public override Guid GUID
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x0600532E RID: 21294 RVA: 0x001059EC File Offset: 0x00103BEC
		private string format_name(bool full_name, bool assembly_qualified)
		{
			StringBuilder stringBuilder = new StringBuilder(this.generic_type.FullName);
			stringBuilder.Append("[");
			for (int i = 0; i < this.type_arguments.Length; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append(",");
				}
				string text;
				if (full_name)
				{
					string fullName = this.type_arguments[i].Assembly.FullName;
					text = this.type_arguments[i].FullName;
					if (text != null && fullName != null)
					{
						text = text + ", " + fullName;
					}
				}
				else
				{
					text = this.type_arguments[i].ToString();
				}
				if (text == null)
				{
					return null;
				}
				if (full_name)
				{
					stringBuilder.Append("[");
				}
				stringBuilder.Append(text);
				if (full_name)
				{
					stringBuilder.Append("]");
				}
			}
			stringBuilder.Append("]");
			if (assembly_qualified)
			{
				stringBuilder.Append(", ");
				stringBuilder.Append(this.generic_type.Assembly.FullName);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600532F RID: 21295 RVA: 0x00105AE9 File Offset: 0x00103CE9
		public override string ToString()
		{
			return this.format_name(false, false);
		}

		// Token: 0x06005330 RID: 21296 RVA: 0x00105AF3 File Offset: 0x00103CF3
		public override Type GetGenericTypeDefinition()
		{
			return this.generic_type;
		}

		// Token: 0x06005331 RID: 21297 RVA: 0x00105AFC File Offset: 0x00103CFC
		public override Type[] GetGenericArguments()
		{
			Type[] array = new Type[this.type_arguments.Length];
			this.type_arguments.CopyTo(array, 0);
			return array;
		}

		// Token: 0x17000DCA RID: 3530
		// (get) Token: 0x06005332 RID: 21298 RVA: 0x00105B28 File Offset: 0x00103D28
		public override bool ContainsGenericParameters
		{
			get
			{
				Type[] array = this.type_arguments;
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

		// Token: 0x17000DCB RID: 3531
		// (get) Token: 0x06005333 RID: 21299 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public override bool IsGenericTypeDefinition
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000DCC RID: 3532
		// (get) Token: 0x06005334 RID: 21300 RVA: 0x000040F7 File Offset: 0x000022F7
		public override bool IsGenericType
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000DCD RID: 3533
		// (get) Token: 0x06005335 RID: 21301 RVA: 0x00105B57 File Offset: 0x00103D57
		public override Type DeclaringType
		{
			get
			{
				return this.generic_type.DeclaringType;
			}
		}

		// Token: 0x17000DCE RID: 3534
		// (get) Token: 0x06005336 RID: 21302 RVA: 0x000472CC File Offset: 0x000454CC
		public override RuntimeTypeHandle TypeHandle
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06005337 RID: 21303 RVA: 0x000F6505 File Offset: 0x000F4705
		public override Type MakeArrayType()
		{
			return new ArrayType(this, 0);
		}

		// Token: 0x06005338 RID: 21304 RVA: 0x000F650E File Offset: 0x000F470E
		public override Type MakeArrayType(int rank)
		{
			if (rank < 1)
			{
				throw new IndexOutOfRangeException();
			}
			return new ArrayType(this, rank);
		}

		// Token: 0x06005339 RID: 21305 RVA: 0x000F6521 File Offset: 0x000F4721
		public override Type MakeByRefType()
		{
			return new ByRefType(this);
		}

		// Token: 0x0600533A RID: 21306 RVA: 0x000F6529 File Offset: 0x000F4729
		public override Type MakePointerType()
		{
			return new PointerType(this);
		}

		// Token: 0x0600533B RID: 21307 RVA: 0x000472CC File Offset: 0x000454CC
		public override Type GetElementType()
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600533C RID: 21308 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		protected override bool HasElementTypeImpl()
		{
			return false;
		}

		// Token: 0x0600533D RID: 21309 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		protected override bool IsCOMObjectImpl()
		{
			return false;
		}

		// Token: 0x0600533E RID: 21310 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		protected override bool IsPrimitiveImpl()
		{
			return false;
		}

		// Token: 0x0600533F RID: 21311 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		protected override bool IsArrayImpl()
		{
			return false;
		}

		// Token: 0x06005340 RID: 21312 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		protected override bool IsByRefImpl()
		{
			return false;
		}

		// Token: 0x06005341 RID: 21313 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		protected override bool IsPointerImpl()
		{
			return false;
		}

		// Token: 0x06005342 RID: 21314 RVA: 0x00105B64 File Offset: 0x00103D64
		protected override TypeAttributes GetAttributeFlagsImpl()
		{
			return this.generic_type.Attributes;
		}

		// Token: 0x06005343 RID: 21315 RVA: 0x000472CC File Offset: 0x000454CC
		public override Type GetInterface(string name, bool ignoreCase)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06005344 RID: 21316 RVA: 0x000472CC File Offset: 0x000454CC
		public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06005345 RID: 21317 RVA: 0x000472CC File Offset: 0x000454CC
		public override FieldInfo GetField(string name, BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06005346 RID: 21318 RVA: 0x000472CC File Offset: 0x000454CC
		public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06005347 RID: 21319 RVA: 0x000472CC File Offset: 0x000454CC
		public override Type GetNestedType(string name, BindingFlags bindingAttr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06005348 RID: 21320 RVA: 0x000472CC File Offset: 0x000454CC
		public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06005349 RID: 21321 RVA: 0x000472CC File Offset: 0x000454CC
		protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600534A RID: 21322 RVA: 0x000472CC File Offset: 0x000454CC
		protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600534B RID: 21323 RVA: 0x000472CC File Offset: 0x000454CC
		protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600534C RID: 21324 RVA: 0x000472CC File Offset: 0x000454CC
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600534D RID: 21325 RVA: 0x00105B71 File Offset: 0x00103D71
		public override object[] GetCustomAttributes(bool inherit)
		{
			if (this.IsCreated)
			{
				return this.generic_type.GetCustomAttributes(inherit);
			}
			throw new NotSupportedException();
		}

		// Token: 0x0600534E RID: 21326 RVA: 0x00105B8D File Offset: 0x00103D8D
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			if (this.IsCreated)
			{
				return this.generic_type.GetCustomAttributes(attributeType, inherit);
			}
			throw new NotSupportedException();
		}

		// Token: 0x17000DCF RID: 3535
		// (get) Token: 0x0600534F RID: 21327 RVA: 0x00105BAC File Offset: 0x00103DAC
		internal override bool IsUserType
		{
			get
			{
				Type[] array = this.type_arguments;
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].IsUserType)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06005350 RID: 21328 RVA: 0x00105BDB File Offset: 0x00103DDB
		internal static Type MakeGenericType(Type type, Type[] typeArguments)
		{
			return new TypeBuilderInstantiation(type, typeArguments);
		}

		// Token: 0x17000DD0 RID: 3536
		// (get) Token: 0x06005351 RID: 21329 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public override bool IsTypeDefinition
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000DD1 RID: 3537
		// (get) Token: 0x06005352 RID: 21330 RVA: 0x000040F7 File Offset: 0x000022F7
		public override bool IsConstructedGenericType
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04003337 RID: 13111
		internal Type generic_type;

		// Token: 0x04003338 RID: 13112
		private Type[] type_arguments;

		// Token: 0x04003339 RID: 13113
		private Hashtable fields;

		// Token: 0x0400333A RID: 13114
		private Hashtable ctors;

		// Token: 0x0400333B RID: 13115
		private Hashtable methods;

		// Token: 0x0400333C RID: 13116
		private const BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
	}
}
