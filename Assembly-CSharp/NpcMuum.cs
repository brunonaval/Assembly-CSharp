using System;
using Mirror;
using UnityEngine;

// Token: 0x02000493 RID: 1171
public class NpcMuum : MonoBehaviour
{
	// Token: 0x06001A9C RID: 6812 RVA: 0x000889B0 File Offset: 0x00086BB0
	private void Awake()
	{
		base.TryGetComponent<NpcModule>(out this.npcModule);
		base.InvokeRepeating("ApplyBuff", 60f, 60f);
		if (NetworkServer.active)
		{
			this.npcModule.NetworkIsPet = true;
		}
	}

	// Token: 0x06001A9D RID: 6813 RVA: 0x000889E8 File Offset: 0x00086BE8
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
			EffectName = "IceCast",
			EffectScaleModifier = 0.35f,
			EffectSpeedModifier = 0.25f,
			SoundEffectName = "default_cast"
		});
		Effect effect = new Effect("IceBarrier", 0.5f, 0.25f);
		Condition condition = new Condition(ConditionCategory.Blessing, ConditionType.Toughness, 30f, 1f, 0.13f, effect, 0, 0f, "");
		ConditionModule conditionModule;
		this.npcModule.Owner.TryGetComponent<ConditionModule>(out conditionModule);
		conditionModule.StartCondition(condition, base.gameObject, true);
	}

	// Token: 0x0400168D RID: 5773
	private NpcModule npcModule;
}
