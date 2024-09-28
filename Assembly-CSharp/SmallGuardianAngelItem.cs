using System;

// Token: 0x020001AD RID: 429
public class SmallGuardianAngelItem : ItemBase
{
	// Token: 0x060004E2 RID: 1250 RVA: 0x0001B16C File Offset: 0x0001936C
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

	// Token: 0x060004E3 RID: 1251 RVA: 0x00019EA3 File Offset: 0x000180A3
	private bool ValidateCreature(ItemBaseConfig itemBaseConfig, EffectModule effectModule)
	{
		if (itemBaseConfig.UserObject.GetComponent<CreatureModule>().IsAlive)
		{
			effectModule.ShowScreenMessage("cant_ressurrect_while_alive_message", 0, 3.5f, Array.Empty<string>());
			return false;
		}
		return true;
	}

	// Token: 0x060004E4 RID: 1252 RVA: 0x0001B1A8 File Offset: 0x000193A8
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

	// Token: 0x060004E5 RID: 1253 RVA: 0x0001B1F6 File Offset: 0x000193F6
	private void ReviveUser(ItemBaseConfig itemBaseConfig)
	{
		itemBaseConfig.UserObject.GetComponent<PlayerModule>().Revive(0.2f);
	}

	// Token: 0x060004E6 RID: 1254 RVA: 0x0001B210 File Offset: 0x00019410
	private void StartInvincibleCondition(ItemBaseConfig itemBaseConfig)
	{
		ConditionModule component = itemBaseConfig.UserObject.GetComponent<ConditionModule>();
		Condition condition = new Condition(ConditionCategory.Invincibility, ConditionType.Invincible, 3f, 0.5f, 0f, new Effect("EarthStrike", 0.5f, 0.25f), 0, 0f, "");
		component.StartCondition(condition, itemBaseConfig.UserObject, true);
	}
}
