using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x0200066C RID: 1644
	public class UIInputEvent : MonoBehaviour
	{
		// Token: 0x0600247D RID: 9341 RVA: 0x000B2662 File Offset: 0x000B0862
		protected void Awake()
		{
			this.m_Selectable = base.gameObject.GetComponent<Selectable>();
		}

		// Token: 0x0600247E RID: 9342 RVA: 0x000B2678 File Offset: 0x000B0878
		protected void Update()
		{
			if (!base.isActiveAndEnabled || !base.gameObject.activeInHierarchy || string.IsNullOrEmpty(this.m_InputName))
			{
				return;
			}
			if (EventSystem.current.currentSelectedGameObject != null)
			{
				Selectable component = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>();
				if ((this.m_Selectable == null && component != null) || (this.m_Selectable != null && component != null && !this.m_Selectable.Equals(component)))
				{
					return;
				}
			}
			if (UIWindowManager.Instance != null && UIWindowManager.Instance.escapeInputName == this.m_InputName && UIWindowManager.Instance.escapedUsed)
			{
				return;
			}
			try
			{
				if (Input.GetButton(this.m_InputName))
				{
					this.m_OnButton.Invoke();
				}
				if (Input.GetButtonDown(this.m_InputName))
				{
					this.m_OnButtonDown.Invoke();
				}
				if (Input.GetButtonUp(this.m_InputName))
				{
					this.m_OnButtonUp.Invoke();
				}
			}
			catch (ArgumentException)
			{
				base.enabled = false;
				Debug.LogWarning(string.Concat(new string[]
				{
					"Input \"",
					this.m_InputName,
					"\" used by game object \"",
					base.gameObject.name,
					"\" is not defined."
				}));
			}
		}

		// Token: 0x04001DAE RID: 7598
		[SerializeField]
		private string m_InputName;

		// Token: 0x04001DAF RID: 7599
		[SerializeField]
		private UnityEvent m_OnButton;

		// Token: 0x04001DB0 RID: 7600
		[SerializeField]
		private UnityEvent m_OnButtonDown;

		// Token: 0x04001DB1 RID: 7601
		[SerializeField]
		private UnityEvent m_OnButtonUp;

		// Token: 0x04001DB2 RID: 7602
		private Selectable m_Selectable;
	}
}
