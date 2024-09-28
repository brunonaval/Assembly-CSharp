using System;

// Token: 0x0200012B RID: 299
[Serializable]
public struct Player
{
	// Token: 0x17000065 RID: 101
	// (get) Token: 0x0600032B RID: 811 RVA: 0x000146FC File Offset: 0x000128FC
	public bool IsDefined
	{
		get
		{
			return !string.IsNullOrEmpty(this.Name);
		}
	}

	// Token: 0x0600032C RID: 812 RVA: 0x0001470C File Offset: 0x0001290C
	public Player(int id, int accountId, string name, int baseLevel)
	{
		this.Id = id;
		this.Name = name;
		this.AccountId = accountId;
		this.BaseLevel = baseLevel;
		this.PositionX = 0f;
		this.PositionY = 0f;
		this.ServerId = 0;
		this.ServerName = null;
		this.ServerPort = 0;
		this.ServerAddress = null;
		this.ServerConnectionVersion = null;
		this.VocationName = null;
		this.GenderName = null;
		this.ServerCurrentPlayers = 0;
		this.ServerMaxPlayers = 0;
		this.ServerType = ServerType.PvP;
	}

	// Token: 0x040005FE RID: 1534
	public int Id;

	// Token: 0x040005FF RID: 1535
	public string Name;

	// Token: 0x04000600 RID: 1536
	public int AccountId;

	// Token: 0x04000601 RID: 1537
	public int BaseLevel;

	// Token: 0x04000602 RID: 1538
	public float PositionX;

	// Token: 0x04000603 RID: 1539
	public float PositionY;

	// Token: 0x04000604 RID: 1540
	public int ServerId;

	// Token: 0x04000605 RID: 1541
	public string ServerName;

	// Token: 0x04000606 RID: 1542
	public int ServerPort;

	// Token: 0x04000607 RID: 1543
	public string ServerAddress;

	// Token: 0x04000608 RID: 1544
	public string ServerConnectionVersion;

	// Token: 0x04000609 RID: 1545
	public string VocationName;

	// Token: 0x0400060A RID: 1546
	public string GenderName;

	// Token: 0x0400060B RID: 1547
	public int ServerCurrentPlayers;

	// Token: 0x0400060C RID: 1548
	public int ServerMaxPlayers;

	// Token: 0x0400060D RID: 1549
	public ServerType ServerType;
}
