using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x0200062E RID: 1582
	[AddComponentMenu("UI/Modal Box Create", 8)]
	[DisallowMultipleComponent]
	public class UIModalBoxCreate : MonoBehaviour
	{
		// Token: 0x060022CF RID: 8911 RVA: 0x000AC6EE File Offset: 0x000AA8EE
		protected void OnEnable()
		{
			if (this.m_HookToButton != null)
			{
				this.m_HookToButton.onClick.AddListener(new UnityAction(this.CreateAndShow));
			}
		}

		// Token: 0x060022D0 RID: 8912 RVA: 0x000AC71A File Offset: 0x000AA91A
		protected void OnDisable()
		{
			if (this.m_HookToButton != null)
			{
				this.m_HookToButton.onClick.RemoveListener(new UnityAction(this.CreateAndShow));
			}
		}

		// Token: 0x060022D1 RID: 8913 RVA: 0x000AC748 File Offset: 0x000AA948
		public void CreateAndShow()
		{
			if (UIModalBoxManager.Instance == null)
			{
				Debug.LogWarning("Could not load the modal box manager while creating a modal box.");
				return;
			}
			UIModalBox uimodalBox = UIModalBoxManager.Instance.Create(base.gameObject);
			if (uimodalBox != null)
			{
				uimodalBox.SetText1(this.m_Text1);
				uimodalBox.SetText2(this.m_Text2);
				uimodalBox.SetConfirmButtonText(this.m_ConfirmText);
				uimodalBox.SetCancelButtonText(this.m_CancelText);
				uimodalBox.onConfirm.AddListener(new UnityAction(this.OnConfirm));
				uimodalBox.onCancel.AddListener(new UnityAction(this.OnCancel));
				uimodalBox.Show();
			}
		}

		// Token: 0x060022D2 RID: 8914 RVA: 0x000AC7EB File Offset: 0x000AA9EB
		public void OnConfirm()
		{
			if (this.onConfirm != null)
			{
				this.onConfirm.Invoke();
			}
		}

		// Token: 0x060022D3 RID: 8915 RVA: 0x000AC800 File Offset: 0x000AAA00
		public void OnCancel()
		{
			if (this.onCancel != null)
			{
				this.onCancel.Invoke();
			}
		}

		// Token: 0x04001C4A RID: 7242
		[SerializeField]
		private string m_Text1;

		// Token: 0x04001C4B RID: 7243
		[SerializeField]
		[TextArea]
		private string m_Text2;

		// Token: 0x04001C4C RID: 7244
		[SerializeField]
		private string m_ConfirmText;

		// Token: 0x04001C4D RID: 7245
		[SerializeField]
		private string m_CancelText;

		// Token: 0x04001C4E RID: 7246
		[SerializeField]
		private Button m_HookToButton;

		// Token: 0x04001C4F RID: 7247
		[Header("Events")]
		public UnityEvent onConfirm = new UnityEvent();

		// Token: 0x04001C50 RID: 7248
		public UnityEvent onCancel = new UnityEvent();
	}
}
