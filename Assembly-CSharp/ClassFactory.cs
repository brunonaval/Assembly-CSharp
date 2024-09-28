using System;
using System.Collections.Generic;
using System.Reflection;

// Token: 0x0200003D RID: 61
public static class ClassFactory
{
	// Token: 0x0600009B RID: 155 RVA: 0x0000371C File Offset: 0x0000191C
	static ClassFactory()
	{
		ClassFactory.assemblyTypes = Assembly.GetExecutingAssembly().GetTypes();
	}

	// Token: 0x0600009C RID: 156 RVA: 0x00003744 File Offset: 0x00001944
	public static string CacheMonitorData()
	{
		string text = "[ClassFactory Monitor]\r\n" + string.Format("AssemblyTypes: {0} objects.\r\n", ClassFactory.assemblyTypes.Length) + string.Format("ClassCache: {0} objects.\r\n", ClassFactory.classCache.Count);
		foreach (KeyValuePair<string, Type> keyValuePair in ClassFactory.classCache)
		{
			text = text + " - ClassName: " + keyValuePair.Key + "\r\n";
		}
		text += string.Format("InstanceCache: {0} objects.", ClassFactory.instanceCache.Count);
		foreach (KeyValuePair<string, object> keyValuePair2 in ClassFactory.instanceCache)
		{
			text = text + " - Instance: " + keyValuePair2.Key + "\r\n";
		}
		return text;
	}

	// Token: 0x0600009D RID: 157 RVA: 0x00003858 File Offset: 0x00001A58
	private static Type CreateClassType(string className)
	{
		if (ClassFactory.classCache.ContainsKey(className))
		{
			return ClassFactory.classCache[className];
		}
		for (int i = 0; i < ClassFactory.assemblyTypes.Length; i++)
		{
			if (ClassFactory.assemblyTypes[i].Name == className)
			{
				ClassFactory.classCache.Add(className, ClassFactory.assemblyTypes[i]);
				return ClassFactory.assemblyTypes[i];
			}
		}
		return null;
	}

	// Token: 0x0600009E RID: 158 RVA: 0x000038BF File Offset: 0x00001ABF
	private static T CreateClassInstance<T>(string className, params object[] args) where T : class
	{
		return (T)((object)Activator.CreateInstance(ClassFactory.CreateClassType(className), args));
	}

	// Token: 0x0600009F RID: 159 RVA: 0x000038D4 File Offset: 0x00001AD4
	public static T GetFromCache<T>(string className, params object[] args) where T : class
	{
		if (ClassFactory.instanceCache.ContainsKey(className))
		{
			return (T)((object)ClassFactory.instanceCache[className]);
		}
		T t = ClassFactory.CreateClassInstance<T>(className, args);
		ClassFactory.instanceCache.Add(className, t);
		return t;
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x0000391C File Offset: 0x00001B1C
	public static void Clear()
	{
		foreach (KeyValuePair<string, Type> keyValuePair in ClassFactory.classCache)
		{
			Type value = keyValuePair.Value;
		}
		foreach (KeyValuePair<string, object> keyValuePair2 in ClassFactory.instanceCache)
		{
			object value2 = keyValuePair2.Value;
		}
		ClassFactory.classCache.Clear();
		ClassFactory.instanceCache.Clear();
	}

	// Token: 0x04000103 RID: 259
	private static readonly Type[] assemblyTypes;

	// Token: 0x04000104 RID: 260
	private static readonly Dictionary<string, Type> classCache = new Dictionary<string, Type>();

	// Token: 0x04000105 RID: 261
	private static readonly Dictionary<string, object> instanceCache = new Dictionary<string, object>();
}
