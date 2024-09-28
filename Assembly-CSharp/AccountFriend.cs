using System;

// Token: 0x020000EA RID: 234
public struct AccountFriend
{
	// Token: 0x17000014 RID: 20
	// (get) Token: 0x06000256 RID: 598 RVA: 0x00011732 File Offset: 0x0000F932
	public bool IsDefined
	{
		get
		{
			return !string.IsNullOrEmpty(this.FriendName);
		}
	}

	// Token: 0x06000257 RID: 599 RVA: 0x00011742 File Offset: 0x0000F942
	public AccountFriend(int friendPlayerId, string friendName, bool muted)
	{
		this.FriendPlayerId = friendPlayerId;
		this.FriendName = friendName;
		this.Muted = muted;
		this.IsOnline = false;
	}

	// Token: 0x04000448 RID: 1096
	public bool IsOnline;

	// Token: 0x04000449 RID: 1097
	public string FriendName;

	// Token: 0x0400044A RID: 1098
	public int FriendPlayerId;

	// Token: 0x0400044B RID: 1099
	public bool Muted;
}
