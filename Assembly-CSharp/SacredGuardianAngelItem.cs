using System;

// Token: 0x020001AC RID: 428
public class SacredGuardianAngelItem : ItemBase
{
	// Token: 0x060004DE RID: 1246 RVA: 0x0001B0A2 File Offset: 0x000192A2
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		this.ShowResurrectEffects(itemBaseConfig);
		this.StartProtectionCondition(itemBaseConfig);
		return true;
	}

	// Token: 0x060004DF RID: 1247 RVA: 0x0001B0B4 File Offset: 0x000192B4
	private void ShowResurrectEffects(ItemBaseConfig itemBaseConfig)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "ResurrectAura",
			EffectScaleModifier = 3f,
			EffectSpeedModifier = 0.5f,
			SoundEffectName = "resurrect"
		};
		EffectModule effectModule;
		itemBaseConfig.UserObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowEffects(effectConfig);
	}

	// Token: 0x060004E0 RID: 1248 RVA: 0x0001B110 File Offset: 0x00019310
	private void StartProtectionCondition(ItemBaseConfig itemBaseConfig)
	{
		ConditionModule component = itemBaseConfig.UserObject.GetComponent<ConditionModule>();
		component.RemoveCondition(ConditionType.ExpProtection);
		Condition condition = new Condition(ConditionCategory.Boost, ConditionType.ExpProtection, 3600f, 2f, 0f, default(Effect), 0, 0f, "");
		component.StartCondition(condition, itemBaseConfig.UserObject, true);
	}
}
