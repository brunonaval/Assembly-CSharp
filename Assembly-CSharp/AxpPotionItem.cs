using System;
using UnityEngine;

// Token: 0x020001C0 RID: 448
public class AxpPotionItem : ItemBase
{
	// Token: 0x0600051E RID: 1310 RVA: 0x0001C174 File Offset: 0x0001A374
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		ChatModule chatModule;
		itemBaseConfig.UserObject.TryGetComponent<ChatModule>(out chatModule);
		EffectModule effectModule;
		itemBaseConfig.UserObject.TryGetComponent<EffectModule>(out effectModule);
		AttributeModule attributeModule;
		itemBaseConfig.UserObject.TryGetComponent<AttributeModule>(out attributeModule);
		int num = 0;
		foreach (global::Attribute attribute in attributeModule.Attributes)
		{
			long num2 = attribute.ExperienceToLevel - attribute.ExperienceToCurrentLevel;
			num += Mathf.RoundToInt((float)num2 * 0.1f);
		}
		attributeModule.AddExperienceToAllAttributes((long)num);
		chatModule.SendSystemTranslatedMessage("attribute_experience_gain_message", string.Empty, false, new string[]
		{
			num.ToString()
		});
		AxpPotionItem.ShowEffects(effectModule);
		return true;
	}

	// Token: 0x0600051F RID: 1311 RVA: 0x0001C240 File Offset: 0x0001A440
	private static void ShowEffects(EffectModule effectModule)
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
