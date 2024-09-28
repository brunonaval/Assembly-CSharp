using System;
using Mirror;
using UnityEngine;

// Token: 0x02000496 RID: 1174
public class NpcSlurg : MonoBehaviour
{
	// Token: 0x06001AA6 RID: 6822 RVA: 0x00088D95 File Offset: 0x00086F95
	private void Awake()
	{
		base.TryGetComponent<NpcModule>(out this.npcModule);
		base.InvokeRepeating("ApplyBuff", 60f, 60f);
		if (NetworkServer.active)
		{
			this.npcModule.NetworkIsPet = true;
		}
	}

	// Token: 0x06001AA7 RID: 6823 RVA: 0x00088DCC File Offset: 0x00086FCC
	private void ApplyBuff()
	{
		if (this.npcModule.Owner == null)
		{
			NetworkServer.Destroy(base.gameObject);
			return;
		}
		EffectModule effectModule;
		base.gameObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowEffects(new EffectConfig
		{
			EffectName = "BloodCast",
			EffectScaleModifier = 0.35f,
			EffectSpeedModifier = 0.25f,
			SoundEffectName = "default_cast"
		});
		Effect effect = new Effect("BloodBarrier", 0.5f, 0.25f);
		Condition condition = new Condition(ConditionCategory.Blessing, ConditionType.Power, 30f, 1f, 0.07f, effect, 0, 0f, "");
		ConditionModule conditionModule;
		this.npcModule.Owner.TryGetComponent<ConditionModule>(out conditionModule);
		conditionModule.StartCondition(condition, base.gameObject, true);
	}

	// Token: 0x04001691 RID: 5777
	private NpcModule npcModule;
}
