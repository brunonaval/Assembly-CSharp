using System;
using System.Collections.Generic;
using DuloGames.UI.Tweens;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x0200063A RID: 1594
	[DisallowMultipleComponent]
	[ExecuteInEditMode]
	[AddComponentMenu("UI/UI Scene/Manager")]
	public class UISceneManager : MonoBehaviour
	{
		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06002322 RID: 8994 RVA: 0x000ADC12 File Offset: 0x000ABE12
		public static UISceneManager instance
		{
			get
			{
				return UISceneManager.m_Instance;
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06002323 RID: 8995 RVA: 0x000ADC19 File Offset: 0x000ABE19
		public UIScene[] scenes
		{
			get
			{
				return this.m_Scenes.ToArray();
			}
		}

		// Token: 0x06002324 RID: 8996 RVA: 0x000ADC28 File Offset: 0x000ABE28
		protected void Awake()
		{
			if (UISceneManager.m_Instance != null)
			{
				if (Application.isPlaying)
				{
					UnityEngine.Object.Destroy(this);
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(this);
				}
				Debug.LogWarning("Multiple UISceneManagers are not allowed, destroying.");
				return;
			}
			UISceneManager.m_Instance = this;
			if (this.m_Scenes == null)
			{
				this.m_Scenes = new List<UIScene>();
			}
			if (Application.isPlaying)
			{
				UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
		}

		// Token: 0x06002325 RID: 8997 RVA: 0x000ADC8D File Offset: 0x000ABE8D
		protected void OnDestroy()
		{
			UISceneManager.m_Instance = null;
		}

		// Token: 0x06002326 RID: 8998 RVA: 0x000ADC95 File Offset: 0x000ABE95
		public void RegisterScene(UIScene scene)
		{
			if (this.m_Scenes == null)
			{
				this.m_Scenes = new List<UIScene>();
			}
			if (this.m_Scenes.Contains(scene))
			{
				Debug.LogWarning("Trying to register a UIScene multiple times.");
				return;
			}
			this.m_Scenes.Add(scene);
		}

		// Token: 0x06002327 RID: 8999 RVA: 0x000ADCCF File Offset: 0x000ABECF
		public void UnregisterScene(UIScene scene)
		{
			if (this.m_Scenes != null)
			{
				this.m_Scenes.Remove(scene);
			}
		}

		// Token: 0x06002328 RID: 9000 RVA: 0x000ADCE6 File Offset: 0x000ABEE6
		public UIScene[] GetActiveScenes()
		{
			return this.m_Scenes.FindAll((UIScene x) => x.isActivated).ToArray();
		}

		// Token: 0x06002329 RID: 9001 RVA: 0x000ADD18 File Offset: 0x000ABF18
		public UIScene GetScene(int id)
		{
			if (this.m_Scenes == null || this.m_Scenes.Count == 0)
			{
				return null;
			}
			return this.m_Scenes.Find((UIScene x) => x.id == id);
		}

		// Token: 0x0600232A RID: 9002 RVA: 0x000ADD60 File Offset: 0x000ABF60
		public int GetAvailableSceneId()
		{
			if (this.m_Scenes.Count == 0)
			{
				return 0;
			}
			int num = 0;
			foreach (UIScene uiscene in this.m_Scenes)
			{
				if (uiscene.id > num)
				{
					num = uiscene.id;
				}
			}
			return num + 1;
		}

		// Token: 0x0600232B RID: 9003 RVA: 0x000ADDD0 File Offset: 0x000ABFD0
		public void TransitionToScene(UIScene scene)
		{
			UIScene.Transition transition = scene.transition;
			float transitionDuration = scene.transitionDuration;
			TweenEasing transitionEasing = scene.transitionEasing;
			UIScene[] activeScenes = this.GetActiveScenes();
			for (int i = 0; i < activeScenes.Length; i++)
			{
				activeScenes[i].TransitionOut(transition, transitionDuration, transitionEasing);
			}
			scene.TransitionIn(transition, transitionDuration, transitionEasing);
		}

		// Token: 0x04001C7E RID: 7294
		private static UISceneManager m_Instance;

		// Token: 0x04001C7F RID: 7295
		private List<UIScene> m_Scenes;
	}
}
