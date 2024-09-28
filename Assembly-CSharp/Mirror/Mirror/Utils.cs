using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mirror
{
	// Token: 0x0200008B RID: 139
	public static class Utils
	{
		// Token: 0x060003F2 RID: 1010 RVA: 0x0000E5A8 File Offset: 0x0000C7A8
		public static uint GetTrueRandomUInt()
		{
			uint result;
			using (RNGCryptoServiceProvider rngcryptoServiceProvider = new RNGCryptoServiceProvider())
			{
				byte[] array = new byte[4];
				rngcryptoServiceProvider.GetBytes(array);
				result = BitConverter.ToUInt32(array, 0);
			}
			return result;
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00003F64 File Offset: 0x00002164
		public static bool IsPrefab(GameObject obj)
		{
			return false;
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0000E5F0 File Offset: 0x0000C7F0
		public static bool IsSceneObject(NetworkIdentity identity)
		{
			return identity.gameObject.hideFlags != HideFlags.NotEditable && identity.gameObject.hideFlags != HideFlags.HideAndDontSave && identity.sceneId > 0UL;
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0000E61B File Offset: 0x0000C81B
		public static bool IsSceneObjectWithPrefabParent(GameObject gameObject, out GameObject prefab)
		{
			prefab = null;
			if (prefab == null)
			{
				Debug.LogError("Failed to find prefab parent for scene object [name:" + gameObject.name + "]");
				return false;
			}
			return true;
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0000E647 File Offset: 0x0000C847
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsPointInScreen(Vector2 point)
		{
			return 0f <= point.x && point.x < (float)Screen.width && 0f <= point.y && point.y < (float)Screen.height;
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0000E684 File Offset: 0x0000C884
		public static string PrettyBytes(long bytes)
		{
			if (bytes < 1024L)
			{
				return string.Format("{0} B", bytes);
			}
			if (bytes < 1048576L)
			{
				return string.Format("{0:F2} KB", (float)bytes / 1024f);
			}
			if (bytes < 1073741824L)
			{
				return string.Format("{0:F2} MB", (float)bytes / 1048576f);
			}
			return string.Format("{0:F2} GB", (float)bytes / 1.0737418E+09f);
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0000E704 File Offset: 0x0000C904
		public static string PrettySeconds(double seconds)
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
			string text = "";
			if (timeSpan.Days > 0)
			{
				text += string.Format("{0}d", timeSpan.Days);
			}
			if (timeSpan.Hours > 0)
			{
				text += string.Format("{0}{1}h", (text.Length > 0) ? " " : "", timeSpan.Hours);
			}
			if (timeSpan.Minutes > 0)
			{
				text += string.Format("{0}{1}m", (text.Length > 0) ? " " : "", timeSpan.Minutes);
			}
			if (timeSpan.Milliseconds > 0)
			{
				text += string.Format("{0}{1}.{2}s", (text.Length > 0) ? " " : "", timeSpan.Seconds, timeSpan.Milliseconds / 100);
			}
			else if (timeSpan.Seconds > 0)
			{
				text += string.Format("{0}{1}s", (text.Length > 0) ? " " : "", timeSpan.Seconds);
			}
			if (!(text != ""))
			{
				return "0s";
			}
			return text;
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0000E85C File Offset: 0x0000CA5C
		public static NetworkIdentity GetSpawnedInServerOrClient(uint netId)
		{
			if (NetworkServer.active)
			{
				NetworkIdentity result;
				NetworkServer.spawned.TryGetValue(netId, out result);
				return result;
			}
			if (NetworkClient.active)
			{
				NetworkIdentity result2;
				NetworkClient.spawned.TryGetValue(netId, out result2);
				return result2;
			}
			return null;
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0000E898 File Offset: 0x0000CA98
		public static Rect KeepInScreen(Rect rect)
		{
			rect.x = Math.Max(rect.x, 0f);
			rect.y = Math.Max(rect.y, 0f);
			rect.x = Math.Min(rect.x, (float)Screen.width - rect.width);
			rect.y = Math.Min(rect.y, (float)Screen.width - rect.height);
			return rect;
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0000E918 File Offset: 0x0000CB18
		public static void CreateLocalConnections(out LocalConnectionToClient connectionToClient, out LocalConnectionToServer connectionToServer)
		{
			connectionToServer = new LocalConnectionToServer();
			connectionToClient = new LocalConnectionToClient();
			connectionToServer.connectionToClient = connectionToClient;
			connectionToClient.connectionToServer = connectionToServer;
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0000E93C File Offset: 0x0000CB3C
		public static bool IsSceneActive(string scene)
		{
			Scene activeScene = SceneManager.GetActiveScene();
			return activeScene.path == scene || activeScene.name == scene;
		}
	}
}
