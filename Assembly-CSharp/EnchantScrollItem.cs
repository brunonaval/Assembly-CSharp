using System;

// Token: 0x020001CA RID: 458
public class EnchantScrollItem : ItemBase
{
	// Token: 0x06000547 RID: 1351 RVA: 0x0001C9D8 File Offset: 0x0001ABD8
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		SkillModule skillModule;
		itemBaseConfig.UserObject.TryGetComponent<SkillModule>(out skillModule);
		skillModule.TargetShowItemBoostWindow(itemBaseConfig.Item);
		return false;
	}
}
