using System;

// Token: 0x02000164 RID: 356
public class BasicCollectorItem : ItemBase
{
	// Token: 0x060003D3 RID: 979 RVA: 0x000171E4 File Offset: 0x000153E4
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		ConditionModule conditionModule;
		itemBaseConfig.UserObject.TryGetComponent<ConditionModule>(out conditionModule);
		EffectModule effectModule;
		itemBaseConfig.UserObject.TryGetComponent<EffectModule>(out effectModule);
		if (conditionModule.HasActiveCondition(ConditionCategory.Telekinesis))
		{
			conditionModule.RemoveCondition(ConditionType.Collect);
			effectModule.ShowEffects(new EffectConfig
			{
				EffectName = "RedStarBlast",
				EffectScaleModifier = 1.5f,
				EffectSpeedModifier = 0.2f,
				SoundEffectName = "telekinesis_off"
			});
			return false;
		}
		effectModule.ShowEffects(new EffectConfig
		{
			EffectName = "StarBlast",
			EffectScaleModifier = 1.5f,
			EffectSpeedModifier = 0.2f,
			SoundEffectName = "telekinesis_on"
		});
		Condition condition = new Condition(ConditionCategory.Telekinesis, ConditionType.Collect, 86400f, 2.5f, 3f, default(Effect), 0, 0f, "");
		conditionModule.StartCondition(condition, itemBaseConfig.UserObject, true);
		return false;
	}
}
