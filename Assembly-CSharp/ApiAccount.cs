using System;
using System.Collections.Generic;

// Token: 0x0200002D RID: 45
[Serializable]
public class ApiAccount
{
	// Token: 0x04000080 RID: 128
	public int AccountId;

	// Token: 0x04000081 RID: 129
	public int MaxCharacterSlots;

	// Token: 0x04000082 RID: 130
	public string AccountUniqueId;

	// Token: 0x04000083 RID: 131
	public PackageType PackageType;

	// Token: 0x04000084 RID: 132
	public AccountType AccountType;

	// Token: 0x04000085 RID: 133
	public int PremiumDays;

	// Token: 0x04000086 RID: 134
	public List<Player> Players;

	// Token: 0x04000087 RID: 135
	public List<ApiServer> Servers;
}
