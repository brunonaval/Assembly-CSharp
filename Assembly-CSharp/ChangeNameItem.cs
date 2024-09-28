using System;

// Token: 0x02000193 RID: 403
public class ChangeNameItem : ItemBase
{
	// Token: 0x0600048F RID: 1167 RVA: 0x000199C8 File Offset: 0x00017BC8
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		CreatureModule creatureModule;
		itemBaseConfig.UserObject.TryGetComponent<CreatureModule>(out creatureModule);
		creatureModule.ShowChangeNameWindow();
		return false;
	}
}
