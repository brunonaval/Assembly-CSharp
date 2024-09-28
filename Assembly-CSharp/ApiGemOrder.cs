using System;

// Token: 0x02000034 RID: 52
[Serializable]
public class ApiGemOrder
{
	// Token: 0x040000AE RID: 174
	public int OrderId;

	// Token: 0x040000AF RID: 175
	public int OrderAmount;

	// Token: 0x040000B0 RID: 176
	public int OrderItemId;

	// Token: 0x040000B1 RID: 177
	public int OrderUnitValue;

	// Token: 0x040000B2 RID: 178
	public int OrderRequiredLevel;

	// Token: 0x040000B3 RID: 179
	public Rarity OrderRarity;

	// Token: 0x040000B4 RID: 180
	public ApiItem Item;

	// Token: 0x040000B5 RID: 181
	public int BlueprintRequiredProfessionLevel;

	// Token: 0x040000B6 RID: 182
	public PlayerProfession BlueprintRequiredProfession;
}
