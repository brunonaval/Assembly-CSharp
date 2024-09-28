using System;

namespace System.Reflection
{
	// Token: 0x020008D5 RID: 2261
	public static class AssemblyExtensions
	{
		// Token: 0x06004B6D RID: 19309 RVA: 0x000F06E8 File Offset: 0x000EE8E8
		public static Type[] GetExportedTypes(Assembly assembly)
		{
			Requires.NotNull(assembly, "assembly");
			return assembly.GetExportedTypes();
		}

		// Token: 0x06004B6E RID: 19310 RVA: 0x000F06FB File Offset: 0x000EE8FB
		public static Module[] GetModules(Assembly assembly)
		{
			Requires.NotNull(assembly, "assembly");
			return assembly.GetModules();
		}

		// Token: 0x06004B6F RID: 19311 RVA: 0x000F070E File Offset: 0x000EE90E
		public static Type[] GetTypes(Assembly assembly)
		{
			Requires.NotNull(assembly, "assembly");
			return assembly.GetTypes();
		}
	}
}
