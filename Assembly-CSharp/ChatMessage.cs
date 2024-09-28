using System;
using System.Text;

// Token: 0x020000FD RID: 253
public struct ChatMessage
{
	// Token: 0x1700001F RID: 31
	// (get) Token: 0x0600027E RID: 638 RVA: 0x00011D48 File Offset: 0x0000FF48
	public string FormattedMessage
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			DateTime dateTime = DateTime.FromOADate(this.Timestamp);
			string text = this.Content;
			if (!string.IsNullOrEmpty(this.SenderDisplay))
			{
				text = string.Format("{0:HH:mm} {1}: {2}", dateTime, this.SenderDisplay, text);
			}
			else
			{
				text = string.Format("{0:HH:mm} {1}", dateTime, text);
			}
			if (!string.IsNullOrEmpty(this.Color))
			{
				text = string.Format("<color={0}>{1}</color>", this.Color, text);
			}
			stringBuilder.AppendLine(text);
			return stringBuilder.ToString();
		}
	}

	// Token: 0x0600027F RID: 639 RVA: 0x00011DD4 File Offset: 0x0000FFD4
	public ChatMessage(string channel, string senderName, string senderDisplay, double timestamp, string content, bool isBroadcast, bool isChannelFixed, bool isChannelGlobal, bool isChannelParty, string color, bool hideFromChatOverlay)
	{
		this.Channel = channel;
		this.SenderName = senderName;
		this.SenderDisplay = senderDisplay;
		this.Timestamp = timestamp;
		this.Content = content;
		this.IsBroadcast = isBroadcast;
		this.IsChannelFixed = isChannelFixed;
		this.IsChannelGlobal = isChannelGlobal;
		this.IsChannelParty = isChannelParty;
		this.Color = color;
		this.HideFromChatOverlay = hideFromChatOverlay;
	}

	// Token: 0x17000020 RID: 32
	// (get) Token: 0x06000280 RID: 640 RVA: 0x00011E36 File Offset: 0x00010036
	public bool IsPrivate
	{
		get
		{
			return !this.IsBroadcast & !this.IsChannelFixed & !this.IsChannelGlobal & !this.IsChannelParty;
		}
	}

	// Token: 0x04000490 RID: 1168
	public string Color;

	// Token: 0x04000491 RID: 1169
	public string SenderName;

	// Token: 0x04000492 RID: 1170
	public string SenderDisplay;

	// Token: 0x04000493 RID: 1171
	public string Channel;

	// Token: 0x04000494 RID: 1172
	public string Content;

	// Token: 0x04000495 RID: 1173
	public double Timestamp;

	// Token: 0x04000496 RID: 1174
	public bool IsBroadcast;

	// Token: 0x04000497 RID: 1175
	public bool IsChannelFixed;

	// Token: 0x04000498 RID: 1176
	public bool IsChannelParty;

	// Token: 0x04000499 RID: 1177
	public bool IsChannelGlobal;

	// Token: 0x0400049A RID: 1178
	public bool HideFromChatOverlay;
}
