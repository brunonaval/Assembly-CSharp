using System;
using Mirror;
using UnityEngine;

// Token: 0x02000491 RID: 1169
public class NpcDropine : MonoBehaviour
{
	// Token: 0x06001A71 RID: 6769 RVA: 0x000878F3 File Offset: 0x00085AF3
	private void Awake()
	{
		base.TryGetComponent<NpcModule>(out this.npcModule);
		base.InvokeRepeating("ApplyBuff", 60f, 60f);
		if (NetworkServer.active)
		{
			this.npcModule.NetworkIsPet = true;
		}
	}

	// Token: 0x06001A72 RID: 6770 RVA: 0x0008792C File Offset: 0x00085B2C
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
		AttributeModule attributeModule;
		this.npcModule.Owner.TryGetComponent<AttributeModule>(out attributeModule);
		float power = (float)attributeModule.MaxHealth * 0.05f;
		Effect effect = new Effect("RecoveryStrike", 0.5f, 0.25f);
		Condition condition = new Condition(ConditionCategory.Regeneration, ConditionType.Healing, 20f, 1f, power, effect, 0, 0f, "heal_strike");
		ConditionModule conditionModule;
		this.npcModule.Owner.TryGetComponent<ConditionModule>(out conditionModule);
		conditionModule.StartCondition(condition, base.gameObject, true);
	}

	// Token: 0x0400168A RID: 5770
	private NpcModule npcModule;
}
