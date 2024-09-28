using System;

// Token: 0x02000502 RID: 1282
public class InvincibleNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001C8F RID: 7311 RVA: 0x0008FCB0 File Offset: 0x0008DEB0
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.RunCastingAnimation(skillBaseConfig, animationConfig, null);
		this.StartConditions(skillBaseConfig);
	}

	// Token: 0x06001C90 RID: 7312 RVA: 0x0008FCDC File Offset: 0x0008DEDC
	private void StartConditions(SkillBaseConfig skillBaseConfig)
	{
		Effect effect = new Effect("IceShield", 0.5f, 0.25f);
		Condition condition = new Condition(ConditionCategory.Invincibility, ConditionType.Invincible, skillBaseConfig.Skill.SkillPower, 1f, 0f, effect, 0, 0f, "");
		ConditionModule conditionModule;
		skillBaseConfig.CasterObject.TryGetComponent<ConditionModule>(out conditionModule);
		conditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
	}
}
