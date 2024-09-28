using System;
using UnityEngine;

// Token: 0x020004DF RID: 1247
public class RequiemSkill : SkillBase
{
	// Token: 0x06001BF5 RID: 7157 RVA: 0x0008D70C File Offset: 0x0008B90C
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		GameObject deadFriendlySelectedTarget = base.GetDeadFriendlySelectedTarget(skillBaseConfig);
		EffectModule component = skillBaseConfig.CasterObject.GetComponent<EffectModule>();
		if (deadFriendlySelectedTarget == null)
		{
			component.ShowScreenMessage("skill_dont_have_target_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		if (deadFriendlySelectedTarget == null)
		{
			return;
		}
		PartyModule partyModule;
		if (!skillBaseConfig.CasterObject.TryGetComponent<PartyModule>(out partyModule))
		{
			return;
		}
		GuildModule guildModule;
		if (!skillBaseConfig.CasterObject.TryGetComponent<GuildModule>(out guildModule))
		{
			return;
		}
		GuildModule guildModule2;
		if (!deadFriendlySelectedTarget.TryGetComponent<GuildModule>(out guildModule2))
		{
			return;
		}
		if (guildModule.ActiveGuildId == 0 && guildModule2.ActiveGuildId == 0 && !partyModule.IsMember(deadFriendlySelectedTarget))
		{
			component.ShowScreenMessage("skill_cant_resurrect_when_not_party_or_guild_member_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		if (guildModule.ActiveGuildId != guildModule2.ActiveGuildId && !partyModule.IsMember(deadFriendlySelectedTarget))
		{
			component.ShowScreenMessage("skill_cant_resurrect_when_not_party_or_guild_member_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		AttributeModule attributeModule;
		skillBaseConfig.CasterObject.TryGetComponent<AttributeModule>(out attributeModule);
		int num = Mathf.CeilToInt(skillBaseConfig.Skill.SkillPower * (float)attributeModule.MaxHealth);
		if (attributeModule.CurrentHealth < num)
		{
			this.ShowTooLowHealthMessage(skillBaseConfig);
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, deadFriendlySelectedTarget);
		this.RemoveHealthAndResurrectTarget(skillBaseConfig, num, attributeModule, deadFriendlySelectedTarget);
	}

	// Token: 0x06001BF6 RID: 7158 RVA: 0x0008D840 File Offset: 0x0008BA40
	private void RemoveHealthAndResurrectTarget(SkillBaseConfig skillBaseConfig, int neededHealth, AttributeModule casterAttributeModule, GameObject target)
	{
		this.RemoveHealthFromCaster(skillBaseConfig, neededHealth, casterAttributeModule);
		this.ShowTargetEffects(skillBaseConfig, target);
		this.ApplyInvincibilityToTarget(skillBaseConfig, target);
		this.ResurrectTarget(skillBaseConfig, target);
	}

	// Token: 0x06001BF7 RID: 7159 RVA: 0x0008D868 File Offset: 0x0008BA68
	private void ResurrectTarget(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		PlayerModule playerModule;
		target.TryGetComponent<PlayerModule>(out playerModule);
		if (playerModule == null)
		{
			return;
		}
		playerModule.Revive(skillBaseConfig.Skill.SkillPower);
	}

	// Token: 0x06001BF8 RID: 7160 RVA: 0x0008D89C File Offset: 0x0008BA9C
	private void ApplyInvincibilityToTarget(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		Effect effect = new Effect("EarthStrike", 0.5f, 0.25f);
		Condition condition = new Condition(ConditionCategory.Invincibility, ConditionType.Invincible, 3f, 0.5f, 0f, effect, 0, 0f, "");
		ConditionModule conditionModule;
		target.TryGetComponent<ConditionModule>(out conditionModule);
		conditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
	}

	// Token: 0x06001BF9 RID: 7161 RVA: 0x0008D8FC File Offset: 0x0008BAFC
	private void ShowTargetEffects(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = skillBaseConfig.Skill.TargetEffect.Name,
			EffectScaleModifier = skillBaseConfig.Skill.TargetEffect.ScaleModifier,
			EffectSpeedModifier = skillBaseConfig.Skill.TargetEffect.SpeedModifier,
			SoundEffectName = skillBaseConfig.Skill.TargetSoundEffectName
		};
		EffectModule effectModule;
		target.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowEffects(effectConfig);
	}

	// Token: 0x06001BFA RID: 7162 RVA: 0x0008D980 File Offset: 0x0008BB80
	private void RemoveHealthFromCaster(SkillBaseConfig skillBaseConfig, int neededHealth, AttributeModule casterAttributeModule)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "BloodStrike",
			EffectScaleModifier = 0.5f,
			EffectSpeedModifier = 0.25f,
			Text = string.Format("{0}", neededHealth),
			TextColorId = 3
		};
		EffectModule effectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowEffects(effectConfig);
		casterAttributeModule.AddHealth(skillBaseConfig.CasterObject, -neededHealth, true, default(EffectConfig));
	}

	// Token: 0x06001BFB RID: 7163 RVA: 0x0008DA08 File Offset: 0x0008BC08
	private void ShowTooLowHealthMessage(SkillBaseConfig skillBaseConfig)
	{
		EffectModule effectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowScreenMessage("health_too_low_to_use_skill_message", 3, 3.5f, Array.Empty<string>());
	}
}
