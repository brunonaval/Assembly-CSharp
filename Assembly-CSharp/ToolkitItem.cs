using System;

// Token: 0x020001AF RID: 431
public class ToolkitItem : ItemBase
{
	// Token: 0x060004EE RID: 1262 RVA: 0x0001B39C File Offset: 0x0001959C
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		CraftModule craftModule;
		itemBaseConfig.UserObject.TryGetComponent<CraftModule>(out craftModule);
		craftModule.TargetShowItemDestructionWindow(itemBaseConfig.Item);
		return false;
	}
}
