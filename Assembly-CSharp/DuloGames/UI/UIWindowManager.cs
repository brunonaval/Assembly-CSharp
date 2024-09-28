using System;
using System.Collections.Generic;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x02000679 RID: 1657
	public class UIWindowManager : MonoBehaviour
	{
		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x060024CD RID: 9421 RVA: 0x000B35A7 File Offset: 0x000B17A7
		public static UIWindowManager Instance
		{
			get
			{
				return UIWindowManager.m_Instance;
			}
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x060024CE RID: 9422 RVA: 0x000B35AE File Offset: 0x000B17AE
		public string escapeInputName
		{
			get
			{
				return this.m_EscapeInputName;
			}
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x060024CF RID: 9423 RVA: 0x000B35B6 File Offset: 0x000B17B6
		public bool escapedUsed
		{
			get
			{
				return this.m_EscapeUsed;
			}
		}

		// Token: 0x060024D0 RID: 9424 RVA: 0x000B35BE File Offset: 0x000B17BE
		protected virtual void Awake()
		{
			if (UIWindowManager.m_Instance == null)
			{
				UIWindowManager.m_Instance = this;
				return;
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x060024D1 RID: 9425 RVA: 0x000B35DF File Offset: 0x000B17DF
		protected virtual void OnDestroy()
		{
			if (UIWindowManager.m_Instance.Equals(this))
			{
				UIWindowManager.m_Instance = null;
			}
		}

		// Token: 0x060024D2 RID: 9426 RVA: 0x000B35F4 File Offset: 0x000B17F4
		protected virtual void Update()
		{
			if (this.m_EscapeUsed)
			{
				this.m_EscapeUsed = false;
			}
			if (Input.GetButtonDown(this.m_EscapeInputName))
			{
				UIModalBox[] array = UnityEngine.Object.FindObjectsOfType<UIModalBox>();
				if (array.Length != 0)
				{
					foreach (UIModalBox uimodalBox in array)
					{
						if (uimodalBox.isActive && uimodalBox.isActiveAndEnabled && uimodalBox.gameObject.activeInHierarchy)
						{
							return;
						}
					}
				}
				List<UIWindow> windows = UIWindow.GetWindows();
				foreach (UIWindow uiwindow in windows)
				{
					if (uiwindow.escapeKeyAction != UIWindow.EscapeKeyAction.None && uiwindow.IsOpen && (uiwindow.escapeKeyAction == UIWindow.EscapeKeyAction.Hide || uiwindow.escapeKeyAction == UIWindow.EscapeKeyAction.Toggle || (uiwindow.escapeKeyAction == UIWindow.EscapeKeyAction.HideIfFocused && uiwindow.IsFocused)))
					{
						uiwindow.Hide();
						this.m_EscapeUsed = true;
					}
				}
				if (this.m_EscapeUsed)
				{
					return;
				}
				foreach (UIWindow uiwindow2 in windows)
				{
					if (!uiwindow2.IsOpen && uiwindow2.escapeKeyAction == UIWindow.EscapeKeyAction.Toggle)
					{
						uiwindow2.Show();
					}
				}
			}
		}

		// Token: 0x04001DE8 RID: 7656
		private static UIWindowManager m_Instance;

		// Token: 0x04001DE9 RID: 7657
		[SerializeField]
		private string m_EscapeInputName = "Cancel";

		// Token: 0x04001DEA RID: 7658
		private bool m_EscapeUsed;
	}
}
