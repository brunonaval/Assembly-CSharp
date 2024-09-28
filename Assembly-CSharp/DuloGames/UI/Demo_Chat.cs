using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x0200059B RID: 1435
	public class Demo_Chat : MonoBehaviour
	{
		// Token: 0x06001FB9 RID: 8121 RVA: 0x0009F4E4 File Offset: 0x0009D6E4
		protected void Awake()
		{
			this.m_ActiveTabInfo = this.FindActiveTab();
			if (this.m_Tabs != null && this.m_Tabs.Count > 0)
			{
				foreach (Demo_Chat.TabInfo tabInfo in this.m_Tabs)
				{
					if (tabInfo.content != null)
					{
						foreach (object obj in tabInfo.content)
						{
							UnityEngine.Object.Destroy(((Transform)obj).gameObject);
						}
					}
				}
			}
		}

		// Token: 0x06001FBA RID: 8122 RVA: 0x0009F5B0 File Offset: 0x0009D7B0
		protected void OnEnable()
		{
			if (this.m_Submit != null)
			{
				this.m_Submit.onClick.AddListener(new UnityAction(this.OnSubmitClick));
			}
			if (this.m_ScrollUpButton != null)
			{
				this.m_ScrollUpButton.onClick.AddListener(new UnityAction(this.OnScrollUpClick));
			}
			if (this.m_ScrollDownButton != null)
			{
				this.m_ScrollDownButton.onClick.AddListener(new UnityAction(this.OnScrollDownClick));
			}
			if (this.m_ScrollTopButton != null)
			{
				this.m_ScrollTopButton.onClick.AddListener(new UnityAction(this.OnScrollToTopClick));
			}
			if (this.m_ScrollBottomButton != null)
			{
				this.m_ScrollBottomButton.onClick.AddListener(new UnityAction(this.OnScrollToBottomClick));
			}
			if (this.m_InputField != null)
			{
				this.m_InputField.onEndEdit.AddListener(new UnityAction<string>(this.OnInputEndEdit));
			}
			if (this.m_Tabs != null && this.m_Tabs.Count > 0)
			{
				foreach (Demo_Chat.TabInfo tabInfo in this.m_Tabs)
				{
					if (tabInfo.button != null)
					{
						tabInfo.button.onValueChanged.AddListener(new UnityAction<bool>(this.OnTabStateChange));
					}
				}
			}
		}

		// Token: 0x06001FBB RID: 8123 RVA: 0x0009F738 File Offset: 0x0009D938
		protected void OnDisable()
		{
			if (this.m_Submit != null)
			{
				this.m_Submit.onClick.RemoveListener(new UnityAction(this.OnSubmitClick));
			}
			if (this.m_ScrollUpButton != null)
			{
				this.m_ScrollUpButton.onClick.RemoveListener(new UnityAction(this.OnScrollUpClick));
			}
			if (this.m_ScrollDownButton != null)
			{
				this.m_ScrollDownButton.onClick.RemoveListener(new UnityAction(this.OnScrollDownClick));
			}
			if (this.m_ScrollTopButton != null)
			{
				this.m_ScrollTopButton.onClick.RemoveListener(new UnityAction(this.OnScrollToTopClick));
			}
			if (this.m_ScrollBottomButton != null)
			{
				this.m_ScrollBottomButton.onClick.RemoveListener(new UnityAction(this.OnScrollToBottomClick));
			}
			if (this.m_Tabs != null && this.m_Tabs.Count > 0)
			{
				foreach (Demo_Chat.TabInfo tabInfo in this.m_Tabs)
				{
					if (tabInfo.button != null)
					{
						tabInfo.button.onValueChanged.RemoveListener(new UnityAction<bool>(this.OnTabStateChange));
					}
				}
			}
		}

		// Token: 0x06001FBC RID: 8124 RVA: 0x0009F898 File Offset: 0x0009DA98
		public void OnSubmitClick()
		{
			if (this.m_InputField != null)
			{
				string text = this.m_InputField.text;
				if (!string.IsNullOrEmpty(text))
				{
					this.SendChatMessage(text);
				}
			}
		}

		// Token: 0x06001FBD RID: 8125 RVA: 0x0009F8D0 File Offset: 0x0009DAD0
		public void OnScrollUpClick()
		{
			if (this.m_ActiveTabInfo == null || this.m_ActiveTabInfo.scrollRect == null)
			{
				return;
			}
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
			pointerEventData.scrollDelta = new Vector2(0f, 1f);
			this.m_ActiveTabInfo.scrollRect.OnScroll(pointerEventData);
		}

		// Token: 0x06001FBE RID: 8126 RVA: 0x0009F92C File Offset: 0x0009DB2C
		public void OnScrollDownClick()
		{
			if (this.m_ActiveTabInfo == null || this.m_ActiveTabInfo.scrollRect == null)
			{
				return;
			}
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
			pointerEventData.scrollDelta = new Vector2(0f, -1f);
			this.m_ActiveTabInfo.scrollRect.OnScroll(pointerEventData);
		}

		// Token: 0x06001FBF RID: 8127 RVA: 0x0009F986 File Offset: 0x0009DB86
		public void OnScrollToTopClick()
		{
			if (this.m_ActiveTabInfo == null || this.m_ActiveTabInfo.scrollRect == null)
			{
				return;
			}
			this.m_ActiveTabInfo.scrollRect.verticalNormalizedPosition = 1f;
		}

		// Token: 0x06001FC0 RID: 8128 RVA: 0x0009F9B9 File Offset: 0x0009DBB9
		public void OnScrollToBottomClick()
		{
			if (this.m_ActiveTabInfo == null || this.m_ActiveTabInfo.scrollRect == null)
			{
				return;
			}
			this.m_ActiveTabInfo.scrollRect.verticalNormalizedPosition = 0f;
		}

		// Token: 0x06001FC1 RID: 8129 RVA: 0x0009F9EC File Offset: 0x0009DBEC
		public void OnInputEndEdit(string text)
		{
			if (!string.IsNullOrEmpty(text) && Input.GetKey(KeyCode.Return))
			{
				this.SendChatMessage(text);
			}
		}

		// Token: 0x06001FC2 RID: 8130 RVA: 0x0009FA06 File Offset: 0x0009DC06
		public void OnTabStateChange(bool state)
		{
			if (state)
			{
				this.m_ActiveTabInfo = this.FindActiveTab();
			}
		}

		// Token: 0x06001FC3 RID: 8131 RVA: 0x0009FA18 File Offset: 0x0009DC18
		private Demo_Chat.TabInfo FindActiveTab()
		{
			if (this.m_Tabs != null && this.m_Tabs.Count > 0)
			{
				foreach (Demo_Chat.TabInfo tabInfo in this.m_Tabs)
				{
					if (tabInfo.button != null && tabInfo.button.isOn)
					{
						return tabInfo;
					}
				}
			}
			return null;
		}

		// Token: 0x06001FC4 RID: 8132 RVA: 0x0009FA9C File Offset: 0x0009DC9C
		public Demo_Chat.TabInfo GetTabInfo(int tabId)
		{
			if (this.m_Tabs != null && this.m_Tabs.Count > 0)
			{
				foreach (Demo_Chat.TabInfo tabInfo in this.m_Tabs)
				{
					if (tabInfo.id == tabId)
					{
						return tabInfo;
					}
				}
			}
			return null;
		}

		// Token: 0x06001FC5 RID: 8133 RVA: 0x0009FB10 File Offset: 0x0009DD10
		private void SendChatMessage(string text)
		{
			int arg = (this.m_ActiveTabInfo != null) ? this.m_ActiveTabInfo.id : 0;
			if (this.onSendMessage != null)
			{
				this.onSendMessage.Invoke(arg, text);
			}
			if (this.m_InputField != null)
			{
				this.m_InputField.text = "";
			}
		}

		// Token: 0x06001FC6 RID: 8134 RVA: 0x0009FB68 File Offset: 0x0009DD68
		public void ReceiveChatMessage(int tabId, string text)
		{
			Demo_Chat.TabInfo tabInfo = this.GetTabInfo(tabId);
			if (tabInfo == null || tabInfo.content == null)
			{
				return;
			}
			GameObject gameObject = new GameObject("Text " + tabInfo.content.childCount.ToString(), new Type[]
			{
				typeof(RectTransform)
			});
			gameObject.layer = base.gameObject.layer;
			RectTransform rectTransform = gameObject.transform as RectTransform;
			rectTransform.localScale = new Vector3(1f, 1f, 1f);
			rectTransform.pivot = new Vector2(0f, 1f);
			rectTransform.anchorMin = new Vector2(0f, 1f);
			rectTransform.anchorMax = new Vector2(0f, 1f);
			rectTransform.SetParent(tabInfo.content, false);
			Text text2 = gameObject.AddComponent<Text>();
			text2.font = this.m_TextFont;
			text2.fontSize = this.m_TextFontSize;
			text2.lineSpacing = this.m_TextLineSpacing;
			text2.color = this.m_TextColor;
			text2.text = text;
			if (this.m_TextEffect != Demo_Chat.TextEffect.None)
			{
				Demo_Chat.TextEffect textEffect = this.m_TextEffect;
				if (textEffect != Demo_Chat.TextEffect.Shadow)
				{
					if (textEffect == Demo_Chat.TextEffect.Outline)
					{
						Outline outline = gameObject.AddComponent<Outline>();
						outline.effectColor = this.m_TextEffectColor;
						outline.effectDistance = this.m_TextEffectDistance;
					}
				}
				else
				{
					Shadow shadow = gameObject.AddComponent<Shadow>();
					shadow.effectColor = this.m_TextEffectColor;
					shadow.effectDistance = this.m_TextEffectDistance;
				}
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(tabInfo.content as RectTransform);
			this.OnScrollToBottomClick();
		}

		// Token: 0x04001996 RID: 6550
		[SerializeField]
		private InputField m_InputField;

		// Token: 0x04001997 RID: 6551
		[Header("Buttons")]
		[SerializeField]
		private Button m_Submit;

		// Token: 0x04001998 RID: 6552
		[SerializeField]
		private Button m_ScrollTopButton;

		// Token: 0x04001999 RID: 6553
		[SerializeField]
		private Button m_ScrollBottomButton;

		// Token: 0x0400199A RID: 6554
		[SerializeField]
		private Button m_ScrollUpButton;

		// Token: 0x0400199B RID: 6555
		[SerializeField]
		private Button m_ScrollDownButton;

		// Token: 0x0400199C RID: 6556
		[Header("Tab Properties")]
		[SerializeField]
		private List<Demo_Chat.TabInfo> m_Tabs = new List<Demo_Chat.TabInfo>();

		// Token: 0x0400199D RID: 6557
		[Header("Text Properties")]
		[SerializeField]
		private Font m_TextFont = FontData.defaultFontData.font;

		// Token: 0x0400199E RID: 6558
		[SerializeField]
		private int m_TextFontSize = FontData.defaultFontData.fontSize;

		// Token: 0x0400199F RID: 6559
		[SerializeField]
		private float m_TextLineSpacing = FontData.defaultFontData.lineSpacing;

		// Token: 0x040019A0 RID: 6560
		[SerializeField]
		private Color m_TextColor = Color.white;

		// Token: 0x040019A1 RID: 6561
		[SerializeField]
		private Demo_Chat.TextEffect m_TextEffect;

		// Token: 0x040019A2 RID: 6562
		[SerializeField]
		private Color m_TextEffectColor = Color.black;

		// Token: 0x040019A3 RID: 6563
		[SerializeField]
		private Vector2 m_TextEffectDistance = new Vector2(1f, -1f);

		// Token: 0x040019A4 RID: 6564
		[Header("Events")]
		public Demo_Chat.SendMessageEvent onSendMessage = new Demo_Chat.SendMessageEvent();

		// Token: 0x040019A5 RID: 6565
		private Demo_Chat.TabInfo m_ActiveTabInfo;

		// Token: 0x0200059C RID: 1436
		[Serializable]
		public enum TextEffect
		{
			// Token: 0x040019A7 RID: 6567
			None,
			// Token: 0x040019A8 RID: 6568
			Shadow,
			// Token: 0x040019A9 RID: 6569
			Outline
		}

		// Token: 0x0200059D RID: 1437
		[Serializable]
		public class SendMessageEvent : UnityEvent<int, string>
		{
		}

		// Token: 0x0200059E RID: 1438
		[Serializable]
		public class TabInfo
		{
			// Token: 0x040019AA RID: 6570
			public int id;

			// Token: 0x040019AB RID: 6571
			public UITab button;

			// Token: 0x040019AC RID: 6572
			public Transform content;

			// Token: 0x040019AD RID: 6573
			public ScrollRect scrollRect;
		}
	}
}
