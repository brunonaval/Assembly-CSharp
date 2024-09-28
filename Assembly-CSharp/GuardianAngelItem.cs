using System;

// Token: 0x02000199 RID: 409
public class GuardianAngelItem : ItemBase
{
	// Token: 0x0600049E RID: 1182 RVA: 0x00019E68 File Offset: 0x00018068
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		EffectModule component = itemBaseConfig.UserObject.GetComponent<EffectModule>();
		if (!this.ValidateCreature(itemBaseConfig, component))
		{
			return false;
		}
		this.ShowResurrectEffects(component);
		this.ReviveUser(itemBaseConfig);
		this.StartInvincibleCondition(itemBaseConfig);
		return true;
	}

	// Token: 0x0600049F RID: 1183 RVA: 0x00019EA3 File Offset: 0x000180A3
	private bool ValidateCreature(ItemBaseConfig itemBaseConfig, EffectModule effectModule)
	{
		if (itemBaseConfig.UserObject.GetComponent<CreatureModule>().IsAlive)
		{
			effectModule.ShowScreenMessage("cant_ressurrect_while_alive_message", 0, 3.5f, Array.Empty<string>());
			return false;
		}
		return true;
	}

	// Token: 0x060004A0 RID: 1184 RVA: 0x00019ED0 File Offset: 0x000180D0
	private void ShowResurrectEffects(EffectModule effectModule)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "ResurrectAura",
			EffectScaleModifier = 3f,
			EffectSpeedModifier = 0.5f,
			SoundEffectName = "resurrect"
		};
		effectModule.ShowEffects(effectConfig);
	}

	// Token: 0x060004A1 RID: 1185 RVA: 0x00019F1E File Offset: 0x0001811E
	private void ReviveUser(ItemBaseConfig itemBaseConfig)
	{
		itemBaseConfig.UserObject.GetComponent<PlayerModule>().Revive(0.3f);
	}

	// Token: 0x060004A2 RID: 1186 RVA: 0x00019F38 File Offset: 0x00018138
	private void StartInvincibleCondition(ItemBaseConfig itemBaseConfig)
	{
		ConditionModule component = itemBaseConfig.UserObject.GetComponent<ConditionModule>();
		Condition condition = new Condition(ConditionCategory.Invincibility, ConditionType.Invincible, 3f, 0.5f, 0f, new Effect("EarthStrike", 0.5f, 0.25f), 0, 0f, "");
		component.StartCondition(condition, itemBaseConfig.UserObject, true);
	}
}
