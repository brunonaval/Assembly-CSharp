using System;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace Mirror
{
	// Token: 0x02000047 RID: 71
	public static class NetworkLoop
	{
		// Token: 0x0600018D RID: 397 RVA: 0x00007691 File Offset: 0x00005891
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void ResetStatics()
		{
			NetworkLoop.OnEarlyUpdate = null;
			NetworkLoop.OnLateUpdate = null;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x000076A0 File Offset: 0x000058A0
		internal static int FindPlayerLoopEntryIndex(PlayerLoopSystem.UpdateFunction function, PlayerLoopSystem playerLoop, Type playerLoopSystemType)
		{
			if (playerLoop.type == playerLoopSystemType)
			{
				return Array.FindIndex<PlayerLoopSystem>(playerLoop.subSystemList, (PlayerLoopSystem elem) => elem.updateDelegate == function);
			}
			if (playerLoop.subSystemList != null)
			{
				for (int i = 0; i < playerLoop.subSystemList.Length; i++)
				{
					int num = NetworkLoop.FindPlayerLoopEntryIndex(function, playerLoop.subSystemList[i], playerLoopSystemType);
					if (num != -1)
					{
						return num;
					}
				}
			}
			return -1;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000771C File Offset: 0x0000591C
		internal static bool AddToPlayerLoop(PlayerLoopSystem.UpdateFunction function, Type ownerType, ref PlayerLoopSystem playerLoop, Type playerLoopSystemType, NetworkLoop.AddMode addMode)
		{
			if (!(playerLoop.type == playerLoopSystemType))
			{
				if (playerLoop.subSystemList != null)
				{
					for (int i = 0; i < playerLoop.subSystemList.Length; i++)
					{
						if (NetworkLoop.AddToPlayerLoop(function, ownerType, ref playerLoop.subSystemList[i], playerLoopSystemType, addMode))
						{
							return true;
						}
					}
				}
				return false;
			}
			if (Array.FindIndex<PlayerLoopSystem>(playerLoop.subSystemList, (PlayerLoopSystem s) => s.updateDelegate == function) != -1)
			{
				return true;
			}
			int num = (playerLoop.subSystemList != null) ? playerLoop.subSystemList.Length : 0;
			Array.Resize<PlayerLoopSystem>(ref playerLoop.subSystemList, num + 1);
			PlayerLoopSystem playerLoopSystem = new PlayerLoopSystem
			{
				type = ownerType,
				updateDelegate = function
			};
			if (addMode == NetworkLoop.AddMode.Beginning)
			{
				Array.Copy(playerLoop.subSystemList, 0, playerLoop.subSystemList, 1, playerLoop.subSystemList.Length - 1);
				playerLoop.subSystemList[0] = playerLoopSystem;
			}
			else if (addMode == NetworkLoop.AddMode.End)
			{
				playerLoop.subSystemList[num] = playerLoopSystem;
			}
			return true;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00007828 File Offset: 0x00005A28
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void RuntimeInitializeOnLoad()
		{
			PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
			NetworkLoop.AddToPlayerLoop(new PlayerLoopSystem.UpdateFunction(NetworkLoop.NetworkEarlyUpdate), typeof(NetworkLoop), ref currentPlayerLoop, typeof(EarlyUpdate), NetworkLoop.AddMode.End);
			NetworkLoop.AddToPlayerLoop(new PlayerLoopSystem.UpdateFunction(NetworkLoop.NetworkLateUpdate), typeof(NetworkLoop), ref currentPlayerLoop, typeof(PreLateUpdate), NetworkLoop.AddMode.End);
			PlayerLoop.SetPlayerLoop(currentPlayerLoop);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00007893 File Offset: 0x00005A93
		private static void NetworkEarlyUpdate()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			NetworkServer.NetworkEarlyUpdate();
			NetworkClient.NetworkEarlyUpdate();
			Action onEarlyUpdate = NetworkLoop.OnEarlyUpdate;
			if (onEarlyUpdate == null)
			{
				return;
			}
			onEarlyUpdate();
		}

		// Token: 0x06000192 RID: 402 RVA: 0x000078B6 File Offset: 0x00005AB6
		private static void NetworkLateUpdate()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			Action onLateUpdate = NetworkLoop.OnLateUpdate;
			if (onLateUpdate != null)
			{
				onLateUpdate();
			}
			NetworkServer.NetworkLateUpdate();
			NetworkClient.NetworkLateUpdate();
		}

		// Token: 0x040000CB RID: 203
		public static Action OnEarlyUpdate;

		// Token: 0x040000CC RID: 204
		public static Action OnLateUpdate;

		// Token: 0x02000048 RID: 72
		internal enum AddMode
		{
			// Token: 0x040000CE RID: 206
			Beginning,
			// Token: 0x040000CF RID: 207
			End
		}
	}
}
