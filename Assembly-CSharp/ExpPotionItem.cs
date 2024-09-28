using System;
using UnityEngine;

// Token: 0x020001C3 RID: 451
public class ExpPotionItem : ItemBase
{
	// Token: 0x0600052B RID: 1323 RVA: 0x0001C45C File Offset: 0x0001A65C
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		ChatModule chatModule;
		itemBaseConfig.UserObject.TryGetComponent<ChatModule>(out chatModule);
		EffectModule effectModule;
		itemBaseConfig.UserObject.TryGetComponent<EffectModule>(out effectModule);
		AttributeModule attributeModule;
		itemBaseConfig.UserObject.TryGetComponent<AttributeModule>(out attributeModule);
		int num = Mathf.RoundToInt((float)(attributeModule.BaseExperienceToLevel - attributeModule.BaseExperienceToCurrentLevel) * 0.1f);
		attributeModule.AddBaseExperience((long)num, true);
		chatModule.SendSystemTranslatedMessage("experience_gain_message", string.Empty, false, new string[]
		{
			num.ToString()
		});
		ExpPotionItem.ShowEffects(effectModule);
		return true;
	}

	// Token: 0x0600052C RID: 1324 RVA: 0x0001C4E0 File Offset: 0x0001A6E0
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
