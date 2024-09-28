using System;

// Token: 0x020004E1 RID: 1249
public class AsmoonEvolutionNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001C02 RID: 7170 RVA: 0x0008DB60 File Offset: 0x0008BD60
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		NonPlayerAttributeModule nonPlayerAttributeModule;
		skillBaseConfig.CasterObject.TryGetComponent<NonPlayerAttributeModule>(out nonPlayerAttributeModule);
		NonPlayerEquipmentModule nonPlayerEquipmentModule;
		skillBaseConfig.CasterObject.TryGetComponent<NonPlayerEquipmentModule>(out nonPlayerEquipmentModule);
		float num = (float)nonPlayerAttributeModule.CurrentHealth / (float)nonPlayerAttributeModule.MaxHealth;
		if (num <= 0.2f & nonPlayerAttributeModule.BaseLevel == 400)
		{
			this.ShowRedStarsEffects(skillBaseConfig);
			nonPlayerAttributeModule.ResetHealth();
			nonPlayerAttributeModule.SetBaseLevel(500);
			nonPlayerAttributeModule.AddBlessing(AttributeType.Power, 2f);
			nonPlayerAttributeModule.AddBlessing(AttributeType.Agility, 2f);
			nonPlayerAttributeModule.AddBlessing(AttributeType.Precision, 2f);
			nonPlayerAttributeModule.AddBlessing(AttributeType.Toughness, 2f);
			nonPlayerAttributeModule.AddBlessing(AttributeType.Vitality, 2f);
			nonPlayerEquipmentModule.EquipSkin("asmoon_mid_skin");
			nonPlayerAttributeModule.ResetHealth();
			return;
		}
		if (num <= 0.2f & nonPlayerAttributeModule.BaseLevel == 500)
		{
			this.ShowRedStarsEffects(skillBaseConfig);
			nonPlayerAttributeModule.ResetHealth();
			nonPlayerAttributeModule.SetBaseLevel(600);
			nonPlayerAttributeModule.AddBlessing(AttributeType.Power, 2.5f);
			nonPlayerAttributeModule.AddBlessing(AttributeType.Agility, 2.5f);
			nonPlayerAttributeModule.AddBlessing(AttributeType.Precision, 2.5f);
			nonPlayerAttributeModule.AddBlessing(AttributeType.Toughness, 2.5f);
			nonPlayerAttributeModule.AddBlessing(AttributeType.Vitality, 2.5f);
			nonPlayerEquipmentModule.EquipSkin("asmoon_final_skin");
			nonPlayerAttributeModule.ResetHealth();
			return;
		}
	}

	// Token: 0x06001C03 RID: 7171 RVA: 0x0008DC9C File Offset: 0x0008BE9C
	private void ShowRedStarsEffects(SkillBaseConfig skillBaseConfig)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "RedStarBlast",
			EffectScaleModifier = 2f,
			EffectSpeedModifier = 0.2f,
			SoundEffectName = "magic_explosion",
			Position = skillBaseConfig.CasterObject.transform.position
		};
		EffectModule effectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowEffectsRandomly(21, skillBaseConfig.Skill.Range, 0.05f, effectConfig);
	}
}
