using System;
using UnityEngine;

// Token: 0x020000FC RID: 252
public struct ChatContact
{
	// Token: 0x0600027C RID: 636 RVA: 0x00011D20 File Offset: 0x0000FF20
	public ChatContact(GameObject player, string name, int connectionId)
	{
		this.Player = player;
		this.Name = name;
		this.ConnectionId = connectionId;
	}

	// Token: 0x1700001E RID: 30
	// (get) Token: 0x0600027D RID: 637 RVA: 0x00011D37 File Offset: 0x0000FF37
	public bool IsDefined
	{
		get
		{
			return !string.IsNullOrEmpty(this.Name);
		}
	}

	// Token: 0x0400048D RID: 1165
	public string Name;

	// Token: 0x0400048E RID: 1166
	public int ConnectionId;

	// Token: 0x0400048F RID: 1167
	public GameObject Player;
}
