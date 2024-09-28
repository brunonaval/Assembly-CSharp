using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001DC RID: 476
public class ChatHolderManager : MonoBehaviour
{
	// Token: 0x06000597 RID: 1431 RVA: 0x0001DB74 File Offset: 0x0001BD74
	private void Start()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x06000598 RID: 1432 RVA: 0x0001DB98 File Offset: 0x0001BD98
	private void Update()
	{
		if (Time.time - this.buttonUpdateTime < 1f)
		{
			return;
		}
		this.buttonUpdateTime = Time.time;
		this.backButton.interactable = (this.tabHolder.transform.childCount > 3);
		this.forwardButton.interactable = (this.tabHolder.transform.childCount > 3);
	}

	// Token: 0x06000599 RID: 1433 RVA: 0x0001DC00 File Offset: 0x0001BE00
	public bool HasUnreadMessages()
	{
		return this.channels.Any((ChatChannel c) => (c.IsPrivate | c.IsParty) & c.Unread);
	}

	// Token: 0x0600059A RID: 1434 RVA: 0x0001DC2C File Offset: 0x0001BE2C
	public ChatChannel FindActiveChannel()
	{
		return this.channels.FirstOrDefault((ChatChannel f) => f.IsActive & (!f.IsFixed | f.IsGlobal | f.IsParty));
	}

	// Token: 0x0600059B RID: 1435 RVA: 0x0001DC58 File Offset: 0x0001BE58
	public ChatChannel FindChannel(string channelName)
	{
		return this.channels.FirstOrDefault((ChatChannel f) => string.Equals(f.Name, channelName, StringComparison.OrdinalIgnoreCase));
	}

	// Token: 0x0600059C RID: 1436 RVA: 0x0001DC8C File Offset: 0x0001BE8C
	public ChatChannel OpenChannel(string channelName, bool isActive, bool isFixed, bool isGlobal, bool isParty)
	{
		ChatChannel channel = this.FindChannel(channelName);
		if (channel == null)
		{
			channel = new ChatChannel(channelName, isActive, isFixed, isGlobal, isParty);
			channel.OnMessageAdded += delegate(ChatMessage message)
			{
				if (channel.IsActive)
				{
					this.AddMessageText(this.messagesText, message.FormattedMessage);
				}
				if (!message.HideFromChatOverlay && this.overlayChatText != null && this.overlayChatText.gameObject.activeInHierarchy)
				{
					this.AddMessageText(this.overlayChatText, message.FormattedMessage);
					this.overlayScrollRect.StartCoroutine(this.ScrollMessagesDownOverlayAsync());
				}
				this.ScrollMessagesDown();
			};
			this.AddChannel(channel);
		}
		return channel;
	}

	// Token: 0x0600059D RID: 1437 RVA: 0x0001DCF8 File Offset: 0x0001BEF8
	public void AddMessageToChannel(ChatMessage message, bool isActive)
	{
		ChatChannel chatChannel = this.OpenChannel(message.Channel, isActive, message.IsChannelFixed, message.IsChannelGlobal, message.IsChannelParty);
		message.Timestamp = DateTime.Now.ToOADate();
		chatChannel.AddMessage(message);
	}

	// Token: 0x0600059E RID: 1438 RVA: 0x0001DD40 File Offset: 0x0001BF40
	public void OnBackButtonClicked()
	{
		this.tabHolderTransform.SetPositionAndRotation(new Vector3(this.tabHolderTransform.position.x + 30f, this.tabHolderTransform.position.y, this.tabHolderTransform.position.z), Quaternion.identity);
	}

	// Token: 0x0600059F RID: 1439 RVA: 0x0001DD98 File Offset: 0x0001BF98
	public void OnForwardButtonClicked()
	{
		this.tabHolderTransform.SetPositionAndRotation(new Vector3(this.tabHolderTransform.position.x - 30f, this.tabHolderTransform.position.y, this.tabHolderTransform.position.z), Quaternion.identity);
	}

	// Token: 0x060005A0 RID: 1440 RVA: 0x0001DDF0 File Offset: 0x0001BFF0
	private void AddChannel(ChatChannel channel)
	{
		this.channels.Add(channel);
		ChatTabManager component = this.CreateTab().GetComponent<ChatTabManager>();
		component.SetChatHolderManager(this);
		component.SetChannel(channel);
		this.ScrollTabsForward();
	}

	// Token: 0x060005A1 RID: 1441 RVA: 0x0001DE1C File Offset: 0x0001C01C
	private GameObject CreateTab()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.chatTabPrefab);
		gameObject.transform.SetParent(this.tabHolder.transform, false);
		gameObject.transform.position = Vector2.zero;
		return gameObject;
	}

	// Token: 0x060005A2 RID: 1442 RVA: 0x0001DE58 File Offset: 0x0001C058
	public void DeActivateAllTabs()
	{
		int childCount = this.tabHolder.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			this.tabHolder.transform.GetChild(i).GetComponent<ChatTabManager>().DeActivateTab();
		}
	}

	// Token: 0x060005A3 RID: 1443 RVA: 0x0001DEA0 File Offset: 0x0001C0A0
	public void ActivateFirstTab()
	{
		Transform child = this.tabHolder.transform.GetChild(0);
		if (child != null)
		{
			child.GetComponent<ChatTabManager>().ActivateTab();
		}
	}

	// Token: 0x060005A4 RID: 1444 RVA: 0x0001DED4 File Offset: 0x0001C0D4
	public void AddMessageText(Text textComponent, string content)
	{
		string text = textComponent.text;
		if (!string.IsNullOrEmpty(text))
		{
			string[] array = text.Split(new string[]
			{
				"\r",
				"\n",
				Environment.NewLine
			}, StringSplitOptions.RemoveEmptyEntries);
			int count = array.Length - 50;
			string[] array2 = array.Skip(count).ToArray<string>();
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string text2 in array2)
			{
				stringBuilder.AppendLine(text2.Trim());
			}
			textComponent.text = stringBuilder.ToString();
		}
		textComponent.text += content;
	}

	// Token: 0x060005A5 RID: 1445 RVA: 0x0001DF72 File Offset: 0x0001C172
	public void SetMessagesText(string content)
	{
		this.messagesText.text = content;
		this.ScrollMessagesDown();
	}

	// Token: 0x060005A6 RID: 1446 RVA: 0x0001DF86 File Offset: 0x0001C186
	private void ScrollTabsForward()
	{
		if (base.gameObject.activeInHierarchy)
		{
			base.StartCoroutine(this.ScrollTabsForwardAsync());
		}
	}

	// Token: 0x060005A7 RID: 1447 RVA: 0x0001DFA2 File Offset: 0x0001C1A2
	private IEnumerator ScrollTabsForwardAsync()
	{
		yield return new WaitForEndOfFrame();
		this.tabsScrollRect.horizontalNormalizedPosition = 1f;
		yield break;
	}

	// Token: 0x060005A8 RID: 1448 RVA: 0x0001DFB1 File Offset: 0x0001C1B1
	private void ScrollMessagesDown()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		base.StartCoroutine(this.ScrollMessagesDownAsync());
	}

	// Token: 0x060005A9 RID: 1449 RVA: 0x0001DFCE File Offset: 0x0001C1CE
	private IEnumerator ScrollMessagesDownAsync()
	{
		yield return new WaitForEndOfFrame();
		this.messagesScrollRect.verticalNormalizedPosition = 0f;
		yield break;
	}

	// Token: 0x060005AA RID: 1450 RVA: 0x0001DFDD File Offset: 0x0001C1DD
	private IEnumerator ScrollMessagesDownOverlayAsync()
	{
		yield return new WaitForEndOfFrame();
		if (this.overlayScrollRect == null)
		{
			yield break;
		}
		if (!this.overlayScrollRect.gameObject.activeInHierarchy)
		{
			yield break;
		}
		this.overlayScrollRect.verticalNormalizedPosition = 0f;
		yield break;
	}

	// Token: 0x060005AB RID: 1451 RVA: 0x0001DFEC File Offset: 0x0001C1EC
	public void CloseChannel(string channelName)
	{
		for (int i = 0; i < this.channels.Count; i++)
		{
			if (this.channels[i].Name == channelName)
			{
				this.channels.RemoveAt(i);
				break;
			}
		}
		this.ActivateFirstTab();
	}

	// Token: 0x060005AC RID: 1452 RVA: 0x0001E03C File Offset: 0x0001C23C
	public void CloseChatTab(string channelName)
	{
		int childCount = this.tabHolder.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			ChatTabManager component = this.tabHolder.transform.GetChild(i).GetComponent<ChatTabManager>();
			if (string.Equals(component.Channel.Name, channelName, StringComparison.OrdinalIgnoreCase))
			{
				component.CloseTab(true);
				return;
			}
		}
	}

	// Token: 0x060005AD RID: 1453 RVA: 0x0001E09C File Offset: 0x0001C29C
	public void ActivateTab(string channelName)
	{
		int childCount = this.tabHolder.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			ChatTabManager component = this.tabHolder.transform.GetChild(i).GetComponent<ChatTabManager>();
			if (string.Equals(component.Channel.Name, channelName, StringComparison.OrdinalIgnoreCase))
			{
				component.ActivateTab();
				return;
			}
		}
	}

	// Token: 0x060005AE RID: 1454 RVA: 0x0001E0F8 File Offset: 0x0001C2F8
	public void SendMessage()
	{
		string text = this.sendMessageInput.text;
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		if (text.Length > 128)
		{
			text = text.Substring(0, 128);
		}
		if (this.TrySendNewPrivateMessage(text))
		{
			return;
		}
		if (this.TrySendMessageToActiveChannel(text))
		{
			return;
		}
		this.SendLocalMessage(text);
	}

	// Token: 0x060005AF RID: 1455 RVA: 0x0001E14F File Offset: 0x0001C34F
	private void SendLocalMessage(string messageContent)
	{
		this.uiSystemModule.ChatModule.CmdSendChatMessage(string.Empty, false, false, messageContent);
		this.ClearChatInput();
	}

	// Token: 0x060005B0 RID: 1456 RVA: 0x0001E170 File Offset: 0x0001C370
	private bool TrySendMessageToActiveChannel(string messageContent)
	{
		ChatChannel chatChannel = this.FindActiveChannel();
		if (chatChannel != null)
		{
			this.uiSystemModule.ChatModule.CmdSendChatMessage(chatChannel.Name, chatChannel.IsGlobal, chatChannel.IsParty, messageContent);
			this.ClearChatInput();
			return true;
		}
		return false;
	}

	// Token: 0x060005B1 RID: 1457 RVA: 0x0001E1B4 File Offset: 0x0001C3B4
	private bool TrySendNewPrivateMessage(string messageContent)
	{
		if (messageContent.StartsWith("/("))
		{
			string value = Regex.Match(messageContent, "\\(([^)]*)\\)").Groups[1].Value;
			if (!string.IsNullOrEmpty(value))
			{
				int num = messageContent.IndexOf(")");
				string text = messageContent.Substring(num + 1) ?? string.Empty;
				this.uiSystemModule.ChatModule.CmdSendChatMessage(value, false, false, text.Trim());
				this.ClearChatInput();
				return true;
			}
		}
		return false;
	}

	// Token: 0x060005B2 RID: 1458 RVA: 0x0001E233 File Offset: 0x0001C433
	public void ClearChatInput()
	{
		this.sendMessageInput.text = string.Empty;
	}

	// Token: 0x060005B3 RID: 1459 RVA: 0x0001E245 File Offset: 0x0001C445
	public void OnHideChatButtonClicked()
	{
		this.uiSystemModule.HideChatHolder();
	}

	// Token: 0x060005B4 RID: 1460 RVA: 0x0001E252 File Offset: 0x0001C452
	public void OnShowChatButtonClicked()
	{
		this.uiSystemModule.ShowChatHolder();
	}

	// Token: 0x060005B5 RID: 1461 RVA: 0x0001E260 File Offset: 0x0001C460
	public void SetSendMessageInputFocus(bool focused, string content = null)
	{
		if (!focused)
		{
			this.DeactivateChat();
			return;
		}
		if (SettingsManager.Instance.ToggleChatWithReturnKey)
		{
			this.uiSystemModule.ShowChatHolder();
		}
		this.sendMessageInput.interactable = true;
		this.sendMessageInput.Select();
		this.sendMessageInput.ActivateInputField();
		this.inputPlaceholderText.text = string.Empty;
		if (!string.IsNullOrEmpty(content))
		{
			this.sendMessageInput.text = content;
		}
	}

	// Token: 0x060005B6 RID: 1462 RVA: 0x0001E2D4 File Offset: 0x0001C4D4
	public void DeactivateChat()
	{
		this.inputPlaceholderText.text = LanguageManager.Instance.GetText("chat_press_enter_message");
		if (!GlobalSettings.IsMobilePlatform)
		{
			this.sendMessageInput.interactable = false;
		}
		this.sendMessageInput.DeactivateInputField();
		if (SettingsManager.Instance.ToggleChatWithReturnKey)
		{
			this.uiSystemModule.HideChatHolder();
		}
	}

	// Token: 0x04000806 RID: 2054
	[SerializeField]
	private Button backButton;

	// Token: 0x04000807 RID: 2055
	[SerializeField]
	private Button forwardButton;

	// Token: 0x04000808 RID: 2056
	[SerializeField]
	private GameObject chatTabPrefab;

	// Token: 0x04000809 RID: 2057
	[SerializeField]
	private GameObject tabHolder;

	// Token: 0x0400080A RID: 2058
	[SerializeField]
	private RectTransform tabHolderTransform;

	// Token: 0x0400080B RID: 2059
	[SerializeField]
	private Text messagesText;

	// Token: 0x0400080C RID: 2060
	[SerializeField]
	private Text overlayChatText;

	// Token: 0x0400080D RID: 2061
	[SerializeField]
	private ScrollRect overlayScrollRect;

	// Token: 0x0400080E RID: 2062
	[SerializeField]
	private InputField sendMessageInput;

	// Token: 0x0400080F RID: 2063
	[SerializeField]
	private Text inputPlaceholderText;

	// Token: 0x04000810 RID: 2064
	[SerializeField]
	private ScrollRect messagesScrollRect;

	// Token: 0x04000811 RID: 2065
	[SerializeField]
	private ScrollRect tabsScrollRect;

	// Token: 0x04000812 RID: 2066
	private UISystemModule uiSystemModule;

	// Token: 0x04000813 RID: 2067
	private List<ChatChannel> channels = new List<ChatChannel>();

	// Token: 0x04000814 RID: 2068
	private float buttonUpdateTime;
}
