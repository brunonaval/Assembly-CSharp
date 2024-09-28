using System;

namespace System.Reflection
{
	// Token: 0x020008D4 RID: 2260
	public static class TypeExtensions
	{
		// Token: 0x06004B4C RID: 19276 RVA: 0x000F0453 File Offset: 0x000EE653
		public static ConstructorInfo GetConstructor(Type type, Type[] types)
		{
			Requires.NotNull(type, "type");
			return type.GetConstructor(types);
		}

		// Token: 0x06004B4D RID: 19277 RVA: 0x000F0467 File Offset: 0x000EE667
		public static ConstructorInfo[] GetConstructors(Type type)
		{
			Requires.NotNull(type, "type");
			return type.GetConstructors();
		}

		// Token: 0x06004B4E RID: 19278 RVA: 0x000F047A File Offset: 0x000EE67A
		public static ConstructorInfo[] GetConstructors(Type type, BindingFlags bindingAttr)
		{
			Requires.NotNull(type, "type");
			return type.GetConstructors(bindingAttr);
		}

		// Token: 0x06004B4F RID: 19279 RVA: 0x000F048E File Offset: 0x000EE68E
		public static MemberInfo[] GetDefaultMembers(Type type)
		{
			Requires.NotNull(type, "type");
			return type.GetDefaultMembers();
		}

		// Token: 0x06004B50 RID: 19280 RVA: 0x000F04A1 File Offset: 0x000EE6A1
		public static EventInfo GetEvent(Type type, string name)
		{
			Requires.NotNull(type, "type");
			return type.GetEvent(name);
		}

		// Token: 0x06004B51 RID: 19281 RVA: 0x000F04B5 File Offset: 0x000EE6B5
		public static EventInfo GetEvent(Type type, string name, BindingFlags bindingAttr)
		{
			Requires.NotNull(type, "type");
			return type.GetEvent(name, bindingAttr);
		}

		// Token: 0x06004B52 RID: 19282 RVA: 0x000F04CA File Offset: 0x000EE6CA
		public static EventInfo[] GetEvents(Type type)
		{
			Requires.NotNull(type, "type");
			return type.GetEvents();
		}

		// Token: 0x06004B53 RID: 19283 RVA: 0x000F04DD File Offset: 0x000EE6DD
		public static EventInfo[] GetEvents(Type type, BindingFlags bindingAttr)
		{
			Requires.NotNull(type, "type");
			return type.GetEvents(bindingAttr);
		}

		// Token: 0x06004B54 RID: 19284 RVA: 0x000F04F1 File Offset: 0x000EE6F1
		public static FieldInfo GetField(Type type, string name)
		{
			Requires.NotNull(type, "type");
			return type.GetField(name);
		}

		// Token: 0x06004B55 RID: 19285 RVA: 0x000F0505 File Offset: 0x000EE705
		public static FieldInfo GetField(Type type, string name, BindingFlags bindingAttr)
		{
			Requires.NotNull(type, "type");
			return type.GetField(name, bindingAttr);
		}

		// Token: 0x06004B56 RID: 19286 RVA: 0x000F051A File Offset: 0x000EE71A
		public static FieldInfo[] GetFields(Type type)
		{
			Requires.NotNull(type, "type");
			return type.GetFields();
		}

		// Token: 0x06004B57 RID: 19287 RVA: 0x000F052D File Offset: 0x000EE72D
		public static FieldInfo[] GetFields(Type type, BindingFlags bindingAttr)
		{
			Requires.NotNull(type, "type");
			return type.GetFields(bindingAttr);
		}

		// Token: 0x06004B58 RID: 19288 RVA: 0x000F0541 File Offset: 0x000EE741
		public static Type[] GetGenericArguments(Type type)
		{
			Requires.NotNull(type, "type");
			return type.GetGenericArguments();
		}

		// Token: 0x06004B59 RID: 19289 RVA: 0x000F0554 File Offset: 0x000EE754
		public static Type[] GetInterfaces(Type type)
		{
			Requires.NotNull(type, "type");
			return type.GetInterfaces();
		}

		// Token: 0x06004B5A RID: 19290 RVA: 0x000F0567 File Offset: 0x000EE767
		public static MemberInfo[] GetMember(Type type, string name)
		{
			Requires.NotNull(type, "type");
			return type.GetMember(name);
		}

		// Token: 0x06004B5B RID: 19291 RVA: 0x000F057B File Offset: 0x000EE77B
		public static MemberInfo[] GetMember(Type type, string name, BindingFlags bindingAttr)
		{
			Requires.NotNull(type, "type");
			return type.GetMember(name, bindingAttr);
		}

		// Token: 0x06004B5C RID: 19292 RVA: 0x000F0590 File Offset: 0x000EE790
		public static MemberInfo[] GetMembers(Type type)
		{
			Requires.NotNull(type, "type");
			return type.GetMembers();
		}

		// Token: 0x06004B5D RID: 19293 RVA: 0x000F05A3 File Offset: 0x000EE7A3
		public static MemberInfo[] GetMembers(Type type, BindingFlags bindingAttr)
		{
			Requires.NotNull(type, "type");
			return type.GetMembers(bindingAttr);
		}

		// Token: 0x06004B5E RID: 19294 RVA: 0x000F05B7 File Offset: 0x000EE7B7
		public static MethodInfo GetMethod(Type type, string name)
		{
			Requires.NotNull(type, "type");
			return type.GetMethod(name);
		}

		// Token: 0x06004B5F RID: 19295 RVA: 0x000F05CB File Offset: 0x000EE7CB
		public static MethodInfo GetMethod(Type type, string name, BindingFlags bindingAttr)
		{
			Requires.NotNull(type, "type");
			return type.GetMethod(name, bindingAttr);
		}

		// Token: 0x06004B60 RID: 19296 RVA: 0x000F05E0 File Offset: 0x000EE7E0
		public static MethodInfo GetMethod(Type type, string name, Type[] types)
		{
			Requires.NotNull(type, "type");
			return type.GetMethod(name, types);
		}

		// Token: 0x06004B61 RID: 19297 RVA: 0x000F05F5 File Offset: 0x000EE7F5
		public static MethodInfo[] GetMethods(Type type)
		{
			Requires.NotNull(type, "type");
			return type.GetMethods();
		}

		// Token: 0x06004B62 RID: 19298 RVA: 0x000F0608 File Offset: 0x000EE808
		public static MethodInfo[] GetMethods(Type type, BindingFlags bindingAttr)
		{
			Requires.NotNull(type, "type");
			return type.GetMethods(bindingAttr);
		}

		// Token: 0x06004B63 RID: 19299 RVA: 0x000F061C File Offset: 0x000EE81C
		public static Type GetNestedType(Type type, string name, BindingFlags bindingAttr)
		{
			Requires.NotNull(type, "type");
			return type.GetNestedType(name, bindingAttr);
		}

		// Token: 0x06004B64 RID: 19300 RVA: 0x000F0631 File Offset: 0x000EE831
		public static Type[] GetNestedTypes(Type type, BindingFlags bindingAttr)
		{
			Requires.NotNull(type, "type");
			return type.GetNestedTypes(bindingAttr);
		}

		// Token: 0x06004B65 RID: 19301 RVA: 0x000F0645 File Offset: 0x000EE845
		public static PropertyInfo[] GetProperties(Type type)
		{
			Requires.NotNull(type, "type");
			return type.GetProperties();
		}

		// Token: 0x06004B66 RID: 19302 RVA: 0x000F0658 File Offset: 0x000EE858
		public static PropertyInfo[] GetProperties(Type type, BindingFlags bindingAttr)
		{
			Requires.NotNull(type, "type");
			return type.GetProperties(bindingAttr);
		}

		// Token: 0x06004B67 RID: 19303 RVA: 0x000F066C File Offset: 0x000EE86C
		public static PropertyInfo GetProperty(Type type, string name)
		{
			Requires.NotNull(type, "type");
			return type.GetProperty(name);
		}

		// Token: 0x06004B68 RID: 19304 RVA: 0x000F0680 File Offset: 0x000EE880
		public static PropertyInfo GetProperty(Type type, string name, BindingFlags bindingAttr)
		{
			Requires.NotNull(type, "type");
			return type.GetProperty(name, bindingAttr);
		}

		// Token: 0x06004B69 RID: 19305 RVA: 0x000F0695 File Offset: 0x000EE895
		public static PropertyInfo GetProperty(Type type, string name, Type returnType)
		{
			Requires.NotNull(type, "type");
			return type.GetProperty(name, returnType);
		}

		// Token: 0x06004B6A RID: 19306 RVA: 0x000F06AA File Offset: 0x000EE8AA
		public static PropertyInfo GetProperty(Type type, string name, Type returnType, Type[] types)
		{
			Requires.NotNull(type, "type");
			return type.GetProperty(name, returnType, types);
		}

		// Token: 0x06004B6B RID: 19307 RVA: 0x000F06C0 File Offset: 0x000EE8C0
		public static bool IsAssignableFrom(Type type, Type c)
		{
			Requires.NotNull(type, "type");
			return type.IsAssignableFrom(c);
		}

		// Token: 0x06004B6C RID: 19308 RVA: 0x000F06D4 File Offset: 0x000EE8D4
		public static bool IsInstanceOfType(Type type, object o)
		{
			Requires.NotNull(type, "type");
			return type.IsInstanceOfType(o);
		}
	}
}
