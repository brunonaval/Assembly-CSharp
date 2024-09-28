using System;

namespace System.Reflection
{
	// Token: 0x020008DA RID: 2266
	public static class PropertyInfoExtensions
	{
		// Token: 0x06004B7C RID: 19324 RVA: 0x000F084C File Offset: 0x000EEA4C
		public static MethodInfo[] GetAccessors(PropertyInfo property)
		{
			Requires.NotNull(property, "property");
			return property.GetAccessors();
		}

		// Token: 0x06004B7D RID: 19325 RVA: 0x000F085F File Offset: 0x000EEA5F
		public static MethodInfo[] GetAccessors(PropertyInfo property, bool nonPublic)
		{
			Requires.NotNull(property, "property");
			return property.GetAccessors(nonPublic);
		}

		// Token: 0x06004B7E RID: 19326 RVA: 0x000F0873 File Offset: 0x000EEA73
		public static MethodInfo GetGetMethod(PropertyInfo property)
		{
			Requires.NotNull(property, "property");
			return property.GetGetMethod();
		}

		// Token: 0x06004B7F RID: 19327 RVA: 0x000F0886 File Offset: 0x000EEA86
		public static MethodInfo GetGetMethod(PropertyInfo property, bool nonPublic)
		{
			Requires.NotNull(property, "property");
			return property.GetGetMethod(nonPublic);
		}

		// Token: 0x06004B80 RID: 19328 RVA: 0x000F089A File Offset: 0x000EEA9A
		public static MethodInfo GetSetMethod(PropertyInfo property)
		{
			Requires.NotNull(property, "property");
			return property.GetSetMethod();
		}

		// Token: 0x06004B81 RID: 19329 RVA: 0x000F08AD File Offset: 0x000EEAAD
		public static MethodInfo GetSetMethod(PropertyInfo property, bool nonPublic)
		{
			Requires.NotNull(property, "property");
			return property.GetSetMethod(nonPublic);
		}
	}
}
