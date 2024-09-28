using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x0200061B RID: 1563
	[AddComponentMenu("Miscellaneous/Load Scene")]
	public class UILoadScene : MonoBehaviour
	{
		// Token: 0x06002284 RID: 8836 RVA: 0x000AB226 File Offset: 0x000A9426
		protected void OnEnable()
		{
			if (this.m_HookToButton != null)
			{
				this.m_HookToButton.onClick.AddListener(new UnityAction(this.LoadScene));
			}
		}

		// Token: 0x06002285 RID: 8837 RVA: 0x000AB252 File Offset: 0x000A9452
		protected void OnDisable()
		{
			if (this.m_HookToButton != null)
			{
				this.m_HookToButton.onClick.RemoveListener(new UnityAction(this.LoadScene));
			}
		}

		// Token: 0x06002286 RID: 8838 RVA: 0x000AB280 File Offset: 0x000A9480
		public void LoadScene()
		{
			if (!string.IsNullOrEmpty(this.m_Scene))
			{
				int num;
				bool flag = int.TryParse(this.m_Scene, out num);
				if (this.m_UseLoadingOverlay && UILoadingOverlayManager.Instance != null)
				{
					UILoadingOverlay uiloadingOverlay = UILoadingOverlayManager.Instance.Create();
					if (!(uiloadingOverlay != null))
					{
						Debug.LogWarning("Failed to instantiate the loading overlay prefab, make sure it's assigned on the manager.");
						return;
					}
					if (flag)
					{
						uiloadingOverlay.LoadScene(num);
						return;
					}
					uiloadingOverlay.LoadScene(this.m_Scene);
					return;
				}
				else
				{
					if (flag)
					{
						SceneManager.LoadScene(num);
						return;
					}
					SceneManager.LoadScene(this.m_Scene);
				}
			}
		}

		// Token: 0x06002287 RID: 8839 RVA: 0x000AB30C File Offset: 0x000A950C
		protected void Update()
		{
			if (!base.isActiveAndEnabled || !base.gameObject.activeInHierarchy || this.m_InputKey == UILoadScene.InputKey.None)
			{
				return;
			}
			if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>() != null)
			{
				return;
			}
			if (this.m_InputKey == UILoadScene.InputKey.Cancel && UIWindowManager.Instance != null && UIWindowManager.Instance.escapeInputName == "Cancel" && UIWindowManager.Instance.escapedUsed)
			{
				return;
			}
			if (this.m_InputKey == UILoadScene.InputKey.Cancel && UIModalBoxManager.Instance != null && UIModalBoxManager.Instance.activeBoxes.Length != 0)
			{
				return;
			}
			string text = string.Empty;
			switch (this.m_InputKey)
			{
			case UILoadScene.InputKey.Submit:
				text = "Submit";
				break;
			case UILoadScene.InputKey.Cancel:
				text = "Cancel";
				break;
			case UILoadScene.InputKey.Jump:
				text = "Jump";
				break;
			}
			if (!string.IsNullOrEmpty(text) && Input.GetButtonDown(text))
			{
				this.LoadScene();
			}
		}

		// Token: 0x04001C06 RID: 7174
		[SerializeField]
		private string m_Scene;

		// Token: 0x04001C07 RID: 7175
		[SerializeField]
		private bool m_UseLoadingOverlay;

		// Token: 0x04001C08 RID: 7176
		[SerializeField]
		private UILoadScene.InputKey m_InputKey;

		// Token: 0x04001C09 RID: 7177
		[SerializeField]
		private Button m_HookToButton;

		// Token: 0x0200061C RID: 1564
		private enum InputKey
		{
			// Token: 0x04001C0B RID: 7179
			None,
			// Token: 0x04001C0C RID: 7180
			Submit,
			// Token: 0x04001C0D RID: 7181
			Cancel,
			// Token: 0x04001C0E RID: 7182
			Jump
		}
	}
}
