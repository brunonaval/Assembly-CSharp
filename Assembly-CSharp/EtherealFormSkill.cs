using System;

// Token: 0x020004C3 RID: 1219
public class EtherealFormSkill : SkillBase
{
	// Token: 0x06001B56 RID: 6998 RVA: 0x0008B160 File Offset: 0x00089360
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		ConditionModule conditionModule;
		skillBaseConfig.CasterObject.TryGetComponent<ConditionModule>(out conditionModule);
		if (conditionModule.HasActiveCondition(ConditionCategory.Transformation))
		{
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, null);
		this.StartConditions(skillBaseConfig, conditionModule);
	}

	// Token: 0x06001B57 RID: 6999 RVA: 0x0008B1A4 File Offset: 0x000893A4
	private void StartConditions(SkillBaseConfig skillBaseConfig, ConditionModule casterConditionModule)
	{
		Effect effect = new Effect("SmokeStrike", 1f, 0.25f);
		Condition condition = new Condition(ConditionCategory.Transformation, ConditionType.Ethereal, 10f, 0.75f, 0f, effect, 0, 0f, "");
		casterConditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
		Condition condition2 = new Condition(ConditionCategory.Invincibility, ConditionType.Invincible, 10f, 10f, 0f, default(Effect), 0, 0f, "");
		casterConditionModule.StartCondition(condition2, skillBaseConfig.CasterObject, true);
	}
}
