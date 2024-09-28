using System;

// Token: 0x020004CB RID: 1227
public class StoneShieldSkill : SkillBase
{
	// Token: 0x06001B7E RID: 7038 RVA: 0x0008BA74 File Offset: 0x00089C74
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		base.StartCastingAnimation(skillBaseConfig, default(AnimationConfig), null);
		Effect effect = new Effect("EarthBarrier", 0.5f, 0.25f);
		Condition condition = new Condition(ConditionCategory.Blessing, ConditionType.Toughness, 12f, 1f, 0.33f, effect, 0, 0f, "");
		ConditionModule conditionModule;
		skillBaseConfig.CasterObject.TryGetComponent<ConditionModule>(out conditionModule);
		conditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
	}
}
