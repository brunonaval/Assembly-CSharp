using System;

// Token: 0x02000151 RID: 337
public struct StoreItemConfig
{
	// Token: 0x0600038C RID: 908 RVA: 0x0001628C File Offset: 0x0001448C
	public StoreItemConfig(int npcId, Item item)
	{
		this.Item = item;
		this.NpcId = npcId;
		this.ItemValue = -1;
	}

	// Token: 0x0600038D RID: 909 RVA: 0x000162A3 File Offset: 0x000144A3
	public StoreItemConfig(int npcId, Item item, int itemValue)
	{
		this.Item = item;
		this.NpcId = npcId;
		this.ItemValue = itemValue;
	}

	// Token: 0x04000700 RID: 1792
	public Item Item;

	// Token: 0x04000701 RID: 1793
	public int ItemValue;

	// Token: 0x04000702 RID: 1794
	public int NpcId;
}
