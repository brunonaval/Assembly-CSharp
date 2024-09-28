using System;

// Token: 0x02000183 RID: 387
public class DefaultItem : ItemBase
{
	// Token: 0x06000453 RID: 1107 RVA: 0x00018F8C File Offset: 0x0001718C
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		EffectModule effectModule;
		itemBaseConfig.UserObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowScreenMessage("item_cant_use_message", 3, 3.5f, Array.Empty<string>());
		return false;
	}
}
