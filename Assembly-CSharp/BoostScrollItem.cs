using System;

// Token: 0x020001C9 RID: 457
public class BoostScrollItem : ItemBase
{
	// Token: 0x06000545 RID: 1349 RVA: 0x0001C9B0 File Offset: 0x0001ABB0
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		CraftModule craftModule;
		itemBaseConfig.UserObject.TryGetComponent<CraftModule>(out craftModule);
		craftModule.TargetShowItemBoostWindow(itemBaseConfig.Item);
		return false;
	}
}
