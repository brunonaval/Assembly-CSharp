using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x0200063D RID: 1597
	[AddComponentMenu("UI/UI Scene/Open")]
	public class UISceneOpen : MonoBehaviour
	{
		// Token: 0x06002332 RID: 9010 RVA: 0x000ADE44 File Offset: 0x000AC044
		protected void OnEnable()
		{
			if (this.m_HookToButton != null)
			{
				this.m_HookToButton.onClick.AddListener(new UnityAction(this.Open));
			}
		}

		// Token: 0x06002333 RID: 9011 RVA: 0x000ADE70 File Offset: 0x000AC070
		protected void OnDisable()
		{
			if (this.m_HookToButton != null)
			{
				this.m_HookToButton.onClick.RemoveListener(new UnityAction(this.Open));
			}
		}

		// Token: 0x06002334 RID: 9012 RVA: 0x000ADE9C File Offset: 0x000AC09C
		public void Open()
		{
			UIScene uiscene = null;
			UISceneOpen.ActionType actionType = this.m_ActionType;
			if (actionType != UISceneOpen.ActionType.SpecificID)
			{
				if (actionType == UISceneOpen.ActionType.LastScene)
				{
					uiscene = UISceneRegistry.instance.lastScene;
				}
			}
			else
			{
				uiscene = UISceneRegistry.instance.GetScene(this.m_SceneId);
			}
			if (uiscene != null)
			{
				uiscene.TransitionTo();
			}
		}

		// Token: 0x06002335 RID: 9013 RVA: 0x000ADEE8 File Offset: 0x000AC0E8
		protected void Update()
		{
			if (!base.isActiveAndEnabled || !base.gameObject.activeInHierarchy || this.m_InputKey == UISceneOpen.InputKey.None)
			{
				return;
			}
			if (this.m_InputKey == UISceneOpen.InputKey.Cancel && UIWindowManager.Instance != null && UIWindowManager.Instance.escapeInputName == "Cancel" && UIWindowManager.Instance.escapedUsed)
			{
				return;
			}
			if (this.m_InputKey == UISceneOpen.InputKey.Cancel && UIModalBoxManager.Instance != null && UIModalBoxManager.Instance.activeBoxes.Length != 0)
			{
				return;
			}
			string text = string.Empty;
			switch (this.m_InputKey)
			{
			case UISceneOpen.InputKey.Submit:
				text = "Submit";
				break;
			case UISceneOpen.InputKey.Cancel:
				text = "Cancel";
				break;
			case UISceneOpen.InputKey.Jump:
				text = "Jump";
				break;
			}
			if (!string.IsNullOrEmpty(text) && Input.GetButtonDown(text))
			{
				this.Open();
			}
		}

		// Token: 0x04001C83 RID: 7299
		[SerializeField]
		private UISceneOpen.ActionType m_ActionType;

		// Token: 0x04001C84 RID: 7300
		[SerializeField]
		private int m_SceneId;

		// Token: 0x04001C85 RID: 7301
		[SerializeField]
		private UISceneOpen.InputKey m_InputKey;

		// Token: 0x04001C86 RID: 7302
		[SerializeField]
		private Button m_HookToButton;

		// Token: 0x0200063E RID: 1598
		private enum ActionType
		{
			// Token: 0x04001C88 RID: 7304
			SpecificID,
			// Token: 0x04001C89 RID: 7305
			LastScene
		}

		// Token: 0x0200063F RID: 1599
		private enum InputKey
		{
			// Token: 0x04001C8B RID: 7307
			None,
			// Token: 0x04001C8C RID: 7308
			Submit,
			// Token: 0x04001C8D RID: 7309
			Cancel,
			// Token: 0x04001C8E RID: 7310
			Jump
		}
	}
}
