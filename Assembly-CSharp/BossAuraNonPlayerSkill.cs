using System;

// Token: 0x020004E2 RID: 1250
public class BossAuraNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001C05 RID: 7173 RVA: 0x0008DD2C File Offset: 0x0008BF2C
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		float power = (float)base.GetTargets(skillBaseConfig, false).Count * 0.15f;
		Condition condition = new Condition(ConditionCategory.Blessing, ConditionType.Power, 7f, 7f, power, default(Effect), 0, 0f, "");
		Condition condition2 = new Condition(ConditionCategory.Blessing, ConditionType.Toughness, 7f, 7f, power, default(Effect), 0, 0f, "");
		Condition condition3 = new Condition(ConditionCategory.Blessing, ConditionType.Vitality, 7f, 7f, power, default(Effect), 0, 0f, "");
		ConditionModule conditionModule;
		skillBaseConfig.CasterObject.TryGetComponent<ConditionModule>(out conditionModule);
		conditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
		conditionModule.StartCondition(condition2, skillBaseConfig.CasterObject, true);
		conditionModule.StartCondition(condition3, skillBaseConfig.CasterObject, true);
	}
}
