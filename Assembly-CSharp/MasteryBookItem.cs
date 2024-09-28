using System;

// Token: 0x020001A3 RID: 419
public class MasteryBookItem : ItemBase
{
	// Token: 0x060004C8 RID: 1224 RVA: 0x0001AA38 File Offset: 0x00018C38
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		AttributeModule attributeModule;
		itemBaseConfig.UserObject.TryGetComponent<AttributeModule>(out attributeModule);
		EffectModule effectModule;
		itemBaseConfig.UserObject.TryGetComponent<EffectModule>(out effectModule);
		if (attributeModule.MasteryLevel >= 20)
		{
			effectModule.ShowScreenMessage("mastery_level_at_max_message", 0, 3.5f, Array.Empty<string>());
			return false;
		}
		if (attributeModule.MasteryLevel < 10)
		{
			effectModule.ShowScreenMessage("mastery_level_too_low_for_book_message", 0, 3.5f, Array.Empty<string>());
			return false;
		}
		int num = (attributeModule.MasteryLevel + 1) * 10;
		if (attributeModule.BaseLevel < num)
		{
			effectModule.ShowScreenMessage("cant_increase_mastery_level_message", 0, 3.5f, Array.Empty<string>());
			return false;
		}
		attributeModule.IncrementMasteryLevel(1);
		effectModule.ShowScreenMessage("mastery_increased_message", 1, 3.5f, new string[]
		{
			attributeModule.MasteryLevel.ToString()
		});
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "StarBlast",
			EffectScaleModifier = 2f,
			EffectSpeedModifier = 0.2f,
			SoundEffectName = "magic_explosion"
		};
		effectModule.ShowEffects(effectConfig);
		return true;
	}
}
