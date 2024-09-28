using System;

// Token: 0x02000198 RID: 408
public class GoldCoinItem : ItemBase
{
	// Token: 0x0600049C RID: 1180 RVA: 0x00019E47 File Offset: 0x00018047
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		itemBaseConfig.UserObject.GetComponent<WalletModule>().AddGoldCoins(itemBaseConfig.Item.Amount);
		return true;
	}
}
