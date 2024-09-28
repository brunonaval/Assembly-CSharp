using System;

// Token: 0x02000197 RID: 407
public class GemItem : ItemBase
{
	// Token: 0x0600049A RID: 1178 RVA: 0x00019E32 File Offset: 0x00018032
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		itemBaseConfig.UserObject.GetComponent<WalletModule>().AddGems(1, true);
		return true;
	}
}
