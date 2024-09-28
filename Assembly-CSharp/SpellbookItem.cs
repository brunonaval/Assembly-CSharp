using System;

// Token: 0x020001AE RID: 430
public class SpellbookItem : ItemBase
{
	// Token: 0x060004E8 RID: 1256 RVA: 0x0001B270 File Offset: 0x00019470
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		SkillModule component = itemBaseConfig.UserObject.GetComponent<SkillModule>();
		EffectModule component2 = itemBaseConfig.UserObject.GetComponent<EffectModule>();
		if (!this.UserCanLearnSkill(itemBaseConfig, component, component2))
		{
			return false;
		}
		if (this.UserAlreadyHaveSkill(itemBaseConfig, component, component2))
		{
			return false;
		}
		this.AddNewSkillToSkillBook(itemBaseConfig, component);
		SpellbookItem.ShowSpellbookEffects(component2);
		return true;
	}

	// Token: 0x060004E9 RID: 1257 RVA: 0x0001B2BE File Offset: 0x000194BE
	private void AddNewSkillToSkillBook(ItemBaseConfig itemBaseConfig, SkillModule skillModule)
	{
		skillModule.LearnSkill(itemBaseConfig.Item.SkillId, 0.0, 0, true, true);
	}

	// Token: 0x060004EA RID: 1258 RVA: 0x0001B2E0 File Offset: 0x000194E0
	private static void ShowSpellbookEffects(EffectModule effectModule)
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

	// Token: 0x060004EB RID: 1259 RVA: 0x0001B32E File Offset: 0x0001952E
	private bool UserCanLearnSkill(ItemBaseConfig itemBaseConfig, SkillModule skillModule, EffectModule effectModule)
	{
		if (!skillModule.CanLearnSkill(itemBaseConfig.Item.SkillId) | itemBaseConfig.Item.SkillId == 0)
		{
			effectModule.ShowScreenMessage("item_cant_learn_skill_message", 3, 3.5f, Array.Empty<string>());
			return false;
		}
		return true;
	}

	// Token: 0x060004EC RID: 1260 RVA: 0x0001B36E File Offset: 0x0001956E
	private bool UserAlreadyHaveSkill(ItemBaseConfig itemBaseConfig, SkillModule skillModule, EffectModule effectModule)
	{
		if (skillModule.HasSkill(itemBaseConfig.Item.SkillId))
		{
			effectModule.ShowScreenMessage("item_already_know_skill_message", 3, 3.5f, Array.Empty<string>());
			return true;
		}
		return false;
	}
}
