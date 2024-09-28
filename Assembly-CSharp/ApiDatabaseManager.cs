using System;
using System.IO;
using UnityEngine;

// Token: 0x02000028 RID: 40
public static class ApiDatabaseManager
{
	// Token: 0x17000011 RID: 17
	// (get) Token: 0x0600007F RID: 127 RVA: 0x000035F4 File Offset: 0x000017F4
	public static ApiDbEndpointsConfig Endpoints
	{
		get
		{
			if (ApiDatabaseManager._endpoints == null)
			{
				ApiDatabaseManager.LoadEndpoints();
				return ApiDatabaseManager._endpoints;
			}
			return ApiDatabaseManager._endpoints;
		}
	}

	// Token: 0x06000080 RID: 128 RVA: 0x00003610 File Offset: 0x00001810
	private static void LoadEndpoints()
	{
		string text = Path.Combine(Environment.CurrentDirectory, "Assets");
		text = Path.Combine(text, "Miscellaneous");
		text = Path.Combine(text, "Settings");
		text = Path.Combine(text, "endpoints.json");
		if (File.Exists(text))
		{
			ApiDatabaseManager._endpoints = JsonUtility.FromJson<ApiDbEndpointsConfig>(File.ReadAllText(text));
			return;
		}
		Debug.LogError("[ApiDatabaseManager] Missing endpoints.json file on path: " + text);
	}

	// Token: 0x04000078 RID: 120
	private static ApiDbEndpointsConfig _endpoints;
}
