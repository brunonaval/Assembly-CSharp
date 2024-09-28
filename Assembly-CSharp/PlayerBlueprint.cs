using System;

// Token: 0x0200012C RID: 300
public struct PlayerBlueprint
{
	// Token: 0x0600032D RID: 813 RVA: 0x00014792 File Offset: 0x00012992
	public PlayerBlueprint(int playerId, int createdAmount, Blueprint blueprint)
	{
		this.PlayerId = playerId;
		this.CreatedAmount = createdAmount;
		this.Blueprint = blueprint;
	}

	// Token: 0x17000066 RID: 102
	// (get) Token: 0x0600032E RID: 814 RVA: 0x000147A9 File Offset: 0x000129A9
	public bool IsDefined
	{
		get
		{
			return this.Blueprint.Id != 0 & this.PlayerId != 0;
		}
	}

	// Token: 0x0400060E RID: 1550
	public int PlayerId;

	// Token: 0x0400060F RID: 1551
	public int CreatedAmount;

	// Token: 0x04000610 RID: 1552
	public Blueprint Blueprint;
}
