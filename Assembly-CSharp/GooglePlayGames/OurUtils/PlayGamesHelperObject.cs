using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GooglePlayGames.OurUtils
{
	// Token: 0x02000693 RID: 1683
	public class PlayGamesHelperObject : MonoBehaviour
	{
		// Token: 0x0600254F RID: 9551 RVA: 0x000B486C File Offset: 0x000B2A6C
		public static void CreateObject()
		{
			if (PlayGamesHelperObject.instance != null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				GameObject gameObject = new GameObject("PlayGames_QueueRunner");
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				PlayGamesHelperObject.instance = gameObject.AddComponent<PlayGamesHelperObject>();
				return;
			}
			PlayGamesHelperObject.instance = new PlayGamesHelperObject();
			PlayGamesHelperObject.sIsDummy = true;
		}

		// Token: 0x06002550 RID: 9552 RVA: 0x00072B51 File Offset: 0x00070D51
		public void Awake()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}

		// Token: 0x06002551 RID: 9553 RVA: 0x000B48B9 File Offset: 0x000B2AB9
		public void OnDisable()
		{
			if (PlayGamesHelperObject.instance == this)
			{
				PlayGamesHelperObject.instance = null;
			}
		}

		// Token: 0x06002552 RID: 9554 RVA: 0x000B48D0 File Offset: 0x000B2AD0
		public static void RunCoroutine(IEnumerator action)
		{
			if (PlayGamesHelperObject.instance != null)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					PlayGamesHelperObject.instance.StartCoroutine(action);
				});
			}
		}

		// Token: 0x06002553 RID: 9555 RVA: 0x000B4908 File Offset: 0x000B2B08
		public static void RunOnGameThread(Action action)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			if (PlayGamesHelperObject.sIsDummy)
			{
				return;
			}
			List<Action> obj = PlayGamesHelperObject.sQueue;
			lock (obj)
			{
				PlayGamesHelperObject.sQueue.Add(action);
				PlayGamesHelperObject.sQueueEmpty = false;
			}
		}

		// Token: 0x06002554 RID: 9556 RVA: 0x000B496C File Offset: 0x000B2B6C
		public void Update()
		{
			if (PlayGamesHelperObject.sIsDummy || PlayGamesHelperObject.sQueueEmpty)
			{
				return;
			}
			this.localQueue.Clear();
			List<Action> obj = PlayGamesHelperObject.sQueue;
			lock (obj)
			{
				this.localQueue.AddRange(PlayGamesHelperObject.sQueue);
				PlayGamesHelperObject.sQueue.Clear();
				PlayGamesHelperObject.sQueueEmpty = true;
			}
			for (int i = 0; i < this.localQueue.Count; i++)
			{
				this.localQueue[i]();
			}
		}

		// Token: 0x06002555 RID: 9557 RVA: 0x000B4A0C File Offset: 0x000B2C0C
		public void OnApplicationFocus(bool focused)
		{
			foreach (Action<bool> action in PlayGamesHelperObject.sFocusCallbackList)
			{
				try
				{
					action(focused);
				}
				catch (Exception ex)
				{
					Logger.e("Exception in OnApplicationFocus:" + ex.Message + "\n" + ex.StackTrace);
				}
			}
		}

		// Token: 0x06002556 RID: 9558 RVA: 0x000B4A90 File Offset: 0x000B2C90
		public void OnApplicationPause(bool paused)
		{
			foreach (Action<bool> action in PlayGamesHelperObject.sPauseCallbackList)
			{
				try
				{
					action(paused);
				}
				catch (Exception ex)
				{
					Logger.e("Exception in OnApplicationPause:" + ex.Message + "\n" + ex.StackTrace);
				}
			}
		}

		// Token: 0x06002557 RID: 9559 RVA: 0x000B4B14 File Offset: 0x000B2D14
		public static void AddFocusCallback(Action<bool> callback)
		{
			if (!PlayGamesHelperObject.sFocusCallbackList.Contains(callback))
			{
				PlayGamesHelperObject.sFocusCallbackList.Add(callback);
			}
		}

		// Token: 0x06002558 RID: 9560 RVA: 0x000B4B2E File Offset: 0x000B2D2E
		public static bool RemoveFocusCallback(Action<bool> callback)
		{
			return PlayGamesHelperObject.sFocusCallbackList.Remove(callback);
		}

		// Token: 0x06002559 RID: 9561 RVA: 0x000B4B3B File Offset: 0x000B2D3B
		public static void AddPauseCallback(Action<bool> callback)
		{
			if (!PlayGamesHelperObject.sPauseCallbackList.Contains(callback))
			{
				PlayGamesHelperObject.sPauseCallbackList.Add(callback);
			}
		}

		// Token: 0x0600255A RID: 9562 RVA: 0x000B4B55 File Offset: 0x000B2D55
		public static bool RemovePauseCallback(Action<bool> callback)
		{
			return PlayGamesHelperObject.sPauseCallbackList.Remove(callback);
		}

		// Token: 0x04001E3D RID: 7741
		private static PlayGamesHelperObject instance = null;

		// Token: 0x04001E3E RID: 7742
		private static bool sIsDummy = false;

		// Token: 0x04001E3F RID: 7743
		private static List<Action> sQueue = new List<Action>();

		// Token: 0x04001E40 RID: 7744
		private List<Action> localQueue = new List<Action>();

		// Token: 0x04001E41 RID: 7745
		private static volatile bool sQueueEmpty = true;

		// Token: 0x04001E42 RID: 7746
		private static List<Action<bool>> sPauseCallbackList = new List<Action<bool>>();

		// Token: 0x04001E43 RID: 7747
		private static List<Action<bool>> sFocusCallbackList = new List<Action<bool>>();
	}
}
