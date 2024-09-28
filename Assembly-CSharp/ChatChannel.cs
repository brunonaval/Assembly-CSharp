using System;
using System.Collections.Generic;
using System.Text;

// Token: 0x020000FA RID: 250
public class ChatChannel
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x06000272 RID: 626 RVA: 0x00011B9C File Offset: 0x0000FD9C
	// (remove) Token: 0x06000273 RID: 627 RVA: 0x00011BD4 File Offset: 0x0000FDD4
	public event ChatChannel.OnMessageAddedEventHandler OnMessageAdded;

	// Token: 0x06000274 RID: 628 RVA: 0x00011C09 File Offset: 0x0000FE09
	public ChatChannel(string name, bool isActive, bool isFixed, bool isGlobal, bool isParty)
	{
		this.Name = name;
		this.IsFixed = isFixed;
		this.IsActive = isActive;
		this.IsGlobal = isGlobal;
		this.IsParty = isParty;
		this.Messages = new List<ChatMessage>();
	}

	// Token: 0x1700001C RID: 28
	// (get) Token: 0x06000275 RID: 629 RVA: 0x00011C44 File Offset: 0x0000FE44
	public string MessagesContent
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (ChatMessage chatMessage in this.Messages)
			{
				stringBuilder.Append(chatMessage.FormattedMessage);
			}
			return stringBuilder.ToString();
		}
	}

	// Token: 0x1700001D RID: 29
	// (get) Token: 0x06000276 RID: 630 RVA: 0x00011CAC File Offset: 0x0000FEAC
	public bool IsPrivate
	{
		get
		{
			return !this.IsFixed & !this.IsGlobal & !this.IsParty;
		}
	}

	// Token: 0x06000277 RID: 631 RVA: 0x00011CCC File Offset: 0x0000FECC
	public void AddMessage(ChatMessage message)
	{
		if (this.Messages.Count >= 50)
		{
			this.Messages.RemoveAt(0);
		}
		this.Messages.Add(message);
		if (!this.IsActive)
		{
			this.Unread = true;
		}
		ChatChannel.OnMessageAddedEventHandler onMessageAdded = this.OnMessageAdded;
		if (onMessageAdded == null)
		{
			return;
		}
		onMessageAdded(message);
	}

	// Token: 0x04000485 RID: 1157
	public string Name;

	// Token: 0x04000486 RID: 1158
	public bool Unread;

	// Token: 0x04000487 RID: 1159
	public bool IsFixed;

	// Token: 0x04000488 RID: 1160
	public bool IsActive;

	// Token: 0x04000489 RID: 1161
	public bool IsGlobal;

	// Token: 0x0400048A RID: 1162
	public bool IsParty;

	// Token: 0x0400048B RID: 1163
	public List<ChatMessage> Messages;

	// Token: 0x020000FB RID: 251
	// (Invoke) Token: 0x06000279 RID: 633
	public delegate void OnMessageAddedEventHandler(ChatMessage message);
}
