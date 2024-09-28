using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x0200062D RID: 1581
	[RequireComponent(typeof(UIWindow))]
	[RequireComponent(typeof(UIAlwaysOnTop))]
	public class UIModalBox : MonoBehaviour
	{
		// Token: 0x17000354 RID: 852
		// (get) Token: 0x060022C2 RID: 8898 RVA: 0x000AC43B File Offset: 0x000AA63B
		public bool isActive
		{
			get
			{
				return this.m_IsActive;
			}
		}

		// Token: 0x060022C3 RID: 8899 RVA: 0x000AC444 File Offset: 0x000AA644
		protected void Awake()
		{
			if (this.m_Window == null)
			{
				this.m_Window = base.gameObject.GetComponent<UIWindow>();
			}
			this.m_Window.ID = UIWindowID.ModalBox;
			this.m_Window.escapeKeyAction = UIWindow.EscapeKeyAction.None;
			this.m_Window.onTransitionComplete.AddListener(new UnityAction<UIWindow, UIWindow.VisualState>(this.OnWindowTransitionEnd));
			base.gameObject.GetComponent<UIAlwaysOnTop>().order = 99996;
			if (this.m_ConfirmButton != null)
			{
				this.m_ConfirmButton.onClick.AddListener(new UnityAction(this.Confirm));
			}
			if (this.m_CancelButton != null)
			{
				this.m_CancelButton.onClick.AddListener(new UnityAction(this.Close));
			}
		}

		// Token: 0x060022C4 RID: 8900 RVA: 0x000AC510 File Offset: 0x000AA710
		protected void Update()
		{
			if (!string.IsNullOrEmpty(this.m_CancelInput) && Input.GetButtonDown(this.m_CancelInput))
			{
				this.Close();
			}
			if (!string.IsNullOrEmpty(this.m_ConfirmInput) && Input.GetButtonDown(this.m_ConfirmInput))
			{
				this.Confirm();
			}
		}

		// Token: 0x060022C5 RID: 8901 RVA: 0x000AC55D File Offset: 0x000AA75D
		public void SetText1(string text)
		{
			if (this.m_Text1 != null)
			{
				this.m_Text1.text = text;
				this.m_Text1.gameObject.SetActive(!string.IsNullOrEmpty(text));
			}
		}

		// Token: 0x060022C6 RID: 8902 RVA: 0x000AC592 File Offset: 0x000AA792
		public void SetText2(string text)
		{
			if (this.m_Text2 != null)
			{
				this.m_Text2.text = text;
				this.m_Text2.gameObject.SetActive(!string.IsNullOrEmpty(text));
			}
		}

		// Token: 0x060022C7 RID: 8903 RVA: 0x000AC5C7 File Offset: 0x000AA7C7
		public void SetConfirmButtonText(string text)
		{
			if (this.m_ConfirmButtonText != null)
			{
				this.m_ConfirmButtonText.text = text;
			}
		}

		// Token: 0x060022C8 RID: 8904 RVA: 0x000AC5E3 File Offset: 0x000AA7E3
		public void SetCancelButtonText(string text)
		{
			if (this.m_CancelButtonText != null)
			{
				this.m_CancelButtonText.text = text;
			}
		}

		// Token: 0x060022C9 RID: 8905 RVA: 0x000AC5FF File Offset: 0x000AA7FF
		public void Show()
		{
			this.m_IsActive = true;
			if (UIModalBoxManager.Instance != null)
			{
				UIModalBoxManager.Instance.RegisterActiveBox(this);
			}
			if (this.m_Window != null)
			{
				this.m_Window.Show();
			}
		}

		// Token: 0x060022CA RID: 8906 RVA: 0x000AC639 File Offset: 0x000AA839
		public void Close()
		{
			this._Hide();
			if (this.onCancel != null)
			{
				this.onCancel.Invoke();
			}
		}

		// Token: 0x060022CB RID: 8907 RVA: 0x000AC654 File Offset: 0x000AA854
		public void Confirm()
		{
			this._Hide();
			if (this.onConfirm != null)
			{
				this.onConfirm.Invoke();
			}
		}

		// Token: 0x060022CC RID: 8908 RVA: 0x000AC66F File Offset: 0x000AA86F
		private void _Hide()
		{
			this.m_IsActive = false;
			if (UIModalBoxManager.Instance != null)
			{
				UIModalBoxManager.Instance.UnregisterActiveBox(this);
			}
			if (this.m_Window != null)
			{
				this.m_Window.Hide();
			}
		}

		// Token: 0x060022CD RID: 8909 RVA: 0x000AC6A9 File Offset: 0x000AA8A9
		public void OnWindowTransitionEnd(UIWindow window, UIWindow.VisualState state)
		{
			if (state == UIWindow.VisualState.Hidden)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x04001C3E RID: 7230
		[Header("Texts")]
		[SerializeField]
		private Text m_Text1;

		// Token: 0x04001C3F RID: 7231
		[SerializeField]
		private Text m_Text2;

		// Token: 0x04001C40 RID: 7232
		[Header("Buttons")]
		[SerializeField]
		private Button m_ConfirmButton;

		// Token: 0x04001C41 RID: 7233
		[SerializeField]
		private Text m_ConfirmButtonText;

		// Token: 0x04001C42 RID: 7234
		[SerializeField]
		private Button m_CancelButton;

		// Token: 0x04001C43 RID: 7235
		[SerializeField]
		private Text m_CancelButtonText;

		// Token: 0x04001C44 RID: 7236
		[Header("Inputs")]
		[SerializeField]
		private string m_ConfirmInput = "Submit";

		// Token: 0x04001C45 RID: 7237
		[SerializeField]
		private string m_CancelInput = "Cancel";

		// Token: 0x04001C46 RID: 7238
		private UIWindow m_Window;

		// Token: 0x04001C47 RID: 7239
		private bool m_IsActive;

		// Token: 0x04001C48 RID: 7240
		[Header("Events")]
		public UnityEvent onConfirm = new UnityEvent();

		// Token: 0x04001C49 RID: 7241
		public UnityEvent onCancel = new UnityEvent();
	}
}
