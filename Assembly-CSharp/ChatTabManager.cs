using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001E4 RID: 484
public class ChatTabManager : MonoBehaviour
{
	// Token: 0x170000AC RID: 172
	// (get) Token: 0x060005D5 RID: 1493 RVA: 0x0001E60A File Offset: 0x0001C80A
	// (set) Token: 0x060005D6 RID: 1494 RVA: 0x0001E612 File Offset: 0x0001C812
	public ChatChannel Channel { get; private set; }

	// Token: 0x170000AD RID: 173
	// (get) Token: 0x060005D7 RID: 1495 RVA: 0x0001E61B File Offset: 0x0001C81B
	public bool IsChannelActive
	{
		get
		{
			return this.Channel.IsActive;
		}
	}

	// Token: 0x060005D8 RID: 1496 RVA: 0x0001E628 File Offset: 0x0001C828
	private void Update()
	{
		if (this.Channel.IsActive)
		{
			this.tabNameText.color = GlobalSettings.Colors[2];
			return;
		}
		bool flag = (int)Time.time % 2 == 0;
		ChatChannel channel = this.Channel;
		if (flag & (channel != null && channel.Unread))
		{
			this.tabNameText.color = GlobalSettings.Colors[3];
			return;
		}
		this.tabNameText.color = GlobalSettings.Colors[11];
	}

	// Token: 0x060005D9 RID: 1497 RVA: 0x0001E6A7 File Offset: 0x0001C8A7
	public void SetChatHolderManager(ChatHolderManager chatHolderManager)
	{
		this.chatHolderManager = chatHolderManager;
	}

	// Token: 0x060005DA RID: 1498 RVA: 0x0001E6B0 File Offset: 0x0001C8B0
	public void SetChannel(ChatChannel channel)
	{
		this.Channel = channel;
		this.tabNameText.text = LanguageManager.Instance.GetText(channel.Name);
		if (this.Channel.IsFixed)
		{
			this.closeButton.gameObject.SetActive(false);
		}
		if (this.Channel.IsActive)
		{
			this.ActivateTab();
		}
	}

	// Token: 0x060005DB RID: 1499 RVA: 0x0001E710 File Offset: 0x0001C910
	public void CloseTab(bool closeFixed)
	{
		if (closeFixed | !this.Channel.IsFixed)
		{
			this.Channel.IsActive = false;
			this.chatHolderManager.CloseChannel(this.Channel.Name);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060005DC RID: 1500 RVA: 0x0001E75C File Offset: 0x0001C95C
	public void ActivateTab()
	{
		this.chatHolderManager.DeActivateAllTabs();
		this.activeOverlay.SetActive(true);
		this.anchor.SetActive(true);
		this.Channel.IsActive = true;
		this.Channel.Unread = false;
		this.chatHolderManager.SetMessagesText(this.Channel.MessagesContent);
	}

	// Token: 0x060005DD RID: 1501 RVA: 0x0001E7BA File Offset: 0x0001C9BA
	public void DeActivateTab()
	{
		this.activeOverlay.SetActive(false);
		this.anchor.SetActive(false);
		this.Channel.IsActive = false;
		this.chatHolderManager.SetMessagesText(string.Empty);
	}

	// Token: 0x04000826 RID: 2086
	private ChatHolderManager chatHolderManager;

	// Token: 0x04000827 RID: 2087
	[SerializeField]
	private Text tabNameText;

	// Token: 0x04000828 RID: 2088
	[SerializeField]
	private Button closeButton;

	// Token: 0x04000829 RID: 2089
	[SerializeField]
	private GameObject activeOverlay;

	// Token: 0x0400082A RID: 2090
	[SerializeField]
	private GameObject anchor;
}
