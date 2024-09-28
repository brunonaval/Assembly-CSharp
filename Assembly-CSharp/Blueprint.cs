using System;

// Token: 0x020000F7 RID: 247
public struct Blueprint
{
	// Token: 0x06000270 RID: 624 RVA: 0x00011B3C File Offset: 0x0000FD3C
	public Blueprint(int id, string itemName, PlayerProfession requiredProfession, int requiredProfessionLevel, int experience, Item producesItem, int producesAmount)
	{
		this.Id = id;
		this.ItemName = itemName;
		this.RequiredProfession = requiredProfession;
		this.RequiredProfessionLevel = requiredProfessionLevel;
		this.Experience = experience;
		this.ProducesItem = producesItem;
		this.ProducesAmount = producesAmount;
		this.Materials = new BlueprintMaterial[0];
	}

	// Token: 0x04000477 RID: 1143
	public int Id;

	// Token: 0x04000478 RID: 1144
	public string ItemName;

	// Token: 0x04000479 RID: 1145
	public PlayerProfession RequiredProfession;

	// Token: 0x0400047A RID: 1146
	public int RequiredProfessionLevel;

	// Token: 0x0400047B RID: 1147
	public int Experience;

	// Token: 0x0400047C RID: 1148
	public Item ProducesItem;

	// Token: 0x0400047D RID: 1149
	public int ProducesAmount;

	// Token: 0x0400047E RID: 1150
	public BlueprintMaterial[] Materials;
}
