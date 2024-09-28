using System;

// Token: 0x02000036 RID: 54
[Serializable]
public class ApiMarketOrder
{
	// Token: 0x040000D8 RID: 216
	public int OrderId;

	// Token: 0x040000D9 RID: 217
	public int OrderAmount;

	// Token: 0x040000DA RID: 218
	public int OrderItemId;

	// Token: 0x040000DB RID: 219
	public string OrderPlayerName;

	// Token: 0x040000DC RID: 220
	public int OrderUnitValue;

	// Token: 0x040000DD RID: 221
	public int OrderPlayerId;

	// Token: 0x040000DE RID: 222
	public int OrderRequiredLevel;

	// Token: 0x040000DF RID: 223
	public int OrderBoostLevel;

	// Token: 0x040000E0 RID: 224
	public Rarity OrderRarity;

	// Token: 0x040000E1 RID: 225
	public ApiItem Item;

	// Token: 0x040000E2 RID: 226
	public int BlueprintRequiredProfessionLevel;

	// Token: 0x040000E3 RID: 227
	public PlayerProfession BlueprintRequiredProfession;
}
