using System;
using System.Collections.Generic;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x02000640 RID: 1600
	public class UISceneRegistry
	{
		// Token: 0x06002337 RID: 9015 RVA: 0x000ADFBF File Offset: 0x000AC1BF
		protected UISceneRegistry()
		{
			this.m_Scenes = new List<UIScene>();
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06002338 RID: 9016 RVA: 0x000ADFD2 File Offset: 0x000AC1D2
		public static UISceneRegistry instance
		{
			get
			{
				if (UISceneRegistry.m_Instance == null)
				{
					UISceneRegistry.m_Instance = new UISceneRegistry();
				}
				return UISceneRegistry.m_Instance;
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06002339 RID: 9017 RVA: 0x000ADFEA File Offset: 0x000AC1EA
		public UIScene[] scenes
		{
			get
			{
				return this.m_Scenes.ToArray();
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x0600233A RID: 9018 RVA: 0x000ADFF7 File Offset: 0x000AC1F7
		public UIScene lastScene
		{
			get
			{
				return this.m_LastScene;
			}
		}

		// Token: 0x0600233B RID: 9019 RVA: 0x000ADFFF File Offset: 0x000AC1FF
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

		// Token: 0x0600233C RID: 9020 RVA: 0x000AE039 File Offset: 0x000AC239
		public void UnregisterScene(UIScene scene)
		{
			if (this.m_Scenes != null)
			{
				this.m_Scenes.Remove(scene);
			}
		}

		// Token: 0x0600233D RID: 9021 RVA: 0x000AE050 File Offset: 0x000AC250
		public UIScene[] GetActiveScenes()
		{
			return this.m_Scenes.FindAll((UIScene x) => x.isActivated).ToArray();
		}

		// Token: 0x0600233E RID: 9022 RVA: 0x000AE084 File Offset: 0x000AC284
		public UIScene GetScene(int id)
		{
			if (this.m_Scenes == null || this.m_Scenes.Count == 0)
			{
				return null;
			}
			return this.m_Scenes.Find((UIScene x) => x.id == id);
		}

		// Token: 0x0600233F RID: 9023 RVA: 0x000AE0CC File Offset: 0x000AC2CC
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

		// Token: 0x06002340 RID: 9024 RVA: 0x000AE13C File Offset: 0x000AC33C
		public void TransitionToScene(UIScene scene)
		{
			foreach (UIScene uiscene in this.GetActiveScenes())
			{
				uiscene.TransitionOut();
				this.m_LastScene = uiscene;
			}
			scene.TransitionIn();
		}

		// Token: 0x04001C8F RID: 7311
		private static UISceneRegistry m_Instance;

		// Token: 0x04001C90 RID: 7312
		private List<UIScene> m_Scenes;

		// Token: 0x04001C91 RID: 7313
		private UIScene m_LastScene;
	}
}
