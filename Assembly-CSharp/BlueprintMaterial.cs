using System;

// Token: 0x020000F8 RID: 248
public struct BlueprintMaterial
{
	// Token: 0x06000271 RID: 625 RVA: 0x00011B8A File Offset: 0x0000FD8A
	public BlueprintMaterial(int materialAmount, Item materialItem)
	{
		this.MaterialAmount = materialAmount;
		this.MaterialItem = materialItem;
	}

	// Token: 0x0400047F RID: 1151
	public Item MaterialItem;

	// Token: 0x04000480 RID: 1152
	public int MaterialAmount;
}
