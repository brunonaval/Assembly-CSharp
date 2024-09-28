using System;
using UnityEngine;

// Token: 0x02000133 RID: 307
public class PlayerReward
{
	// Token: 0x0600033C RID: 828 RVA: 0x00015232 File Offset: 0x00013432
	public PlayerReward(int partySize, int partyHighestMemberLevel, int distinctPartySize, GameObject player)
	{
		this.Player = player;
		this.PartySize = partySize;
		this.DistinctPartySize = distinctPartySize;
		this.PartyHighestMemberLevel = partyHighestMemberLevel;
	}

	// Token: 0x04000633 RID: 1587
	public int PartySize;

	// Token: 0x04000634 RID: 1588
	public GameObject Player;

	// Token: 0x04000635 RID: 1589
	public int DistinctPartySize;

	// Token: 0x04000636 RID: 1590
	public int PartyHighestMemberLevel;
}
