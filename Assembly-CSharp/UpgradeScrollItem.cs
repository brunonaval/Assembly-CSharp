using System;

// Token: 0x020001CD RID: 461
public class UpgradeScrollItem : ItemBase
{
	// Token: 0x06000558 RID: 1368 RVA: 0x0001CD64 File Offset: 0x0001AF64
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		CraftModule craftModule;
		itemBaseConfig.UserObject.TryGetComponent<CraftModule>(out craftModule);
		craftModule.TargetShowItemUpgradeWindow(itemBaseConfig.Item);
		return false;
	}
}
