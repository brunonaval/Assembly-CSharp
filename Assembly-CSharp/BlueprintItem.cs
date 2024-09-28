using System;

// Token: 0x02000190 RID: 400
public class BlueprintItem : ItemBase
{
	// Token: 0x06000486 RID: 1158 RVA: 0x00019778 File Offset: 0x00017978
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		CraftModule component = itemBaseConfig.UserObject.GetComponent<CraftModule>();
		int itemBlueprintId = ItemBase.ItemDatabaseModule.GetItemBlueprintId(itemBaseConfig.Item.Id);
		bool flag = component.LearnBlueprint(itemBlueprintId);
		if (flag)
		{
			BlueprintItem.ShowMagicEffects(itemBaseConfig.UserObject.GetComponent<EffectModule>());
		}
		return flag;
	}

	// Token: 0x06000487 RID: 1159 RVA: 0x000197C0 File Offset: 0x000179C0
	private static void ShowMagicEffects(EffectModule effectModule)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "Magic",
			EffectScaleModifier = 0.5f,
			EffectSpeedModifier = 0.25f,
			SoundEffectName = "spellbook"
		};
		effectModule.ShowEffects(effectConfig);
	}
}
