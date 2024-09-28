using System;
using System.Linq;
using Mirror;
using UnityEngine;

// Token: 0x02000494 RID: 1172
public class NpcSeraphiel : MonoBehaviour
{
	// Token: 0x06001A9F RID: 6815 RVA: 0x00088ABD File Offset: 0x00086CBD
	private void Awake()
	{
		base.TryGetComponent<NpcModule>(out this.npcModule);
		base.InvokeRepeating("ApplyBuff", 40f, 40f);
		if (NetworkServer.active)
		{
			this.npcModule.NetworkIsPet = true;
		}
	}

	// Token: 0x06001AA0 RID: 6816 RVA: 0x00088AF4 File Offset: 0x00086CF4
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
		Condition condition = new Condition(ConditionCategory.Blessing, ConditionType.Power, 30f, 1f, 0.17f, effect, 0, 0f, "");
		Effect effect2 = new Effect("IceBarrier", 0.5f, 0.25f);
		Condition condition2 = new Condition(ConditionCategory.Blessing, ConditionType.Toughness, 30f, 1f, 0.17f, effect2, 0, 0f, "");
		AttributeModule attributeModule;
		this.npcModule.Owner.TryGetComponent<AttributeModule>(out attributeModule);
		float power = (float)attributeModule.MaxHealth * 0.1f;
		Effect effect3 = new Effect("RecoveryStrike", 0.5f, 0.25f);
		Condition condition3 = new Condition(ConditionCategory.Regeneration, ConditionType.Healing, 10f, 1f, power, effect3, 0, 0f, "heal_strike");
		ConditionModule conditionModule;
		this.npcModule.Owner.TryGetComponent<ConditionModule>(out conditionModule);
		conditionModule.StartCondition(condition, base.gameObject, true);
		conditionModule.StartCondition(condition2, base.gameObject, true);
		conditionModule.StartCondition(condition3, base.gameObject, true);
		this.ResetRandomSkill();
	}

	// Token: 0x06001AA1 RID: 6817 RVA: 0x00088C88 File Offset: 0x00086E88
	private void ResetRandomSkill()
	{
		EffectModule effectModule;
		base.gameObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowEffects(new EffectConfig
		{
			EffectName = "FireCast",
			EffectScaleModifier = 0.35f,
			EffectSpeedModifier = 0.25f,
			SoundEffectName = "default_cast"
		});
		SkillModule skillModule;
		this.npcModule.Owner.TryGetComponent<SkillModule>(out skillModule);
		Skill[] array = (from s in skillModule.SkillBook
		where !s.IsDefaultSkill & s.CooldownTimer(NetworkTime.time) > 0.0
		select s).ToArray<Skill>();
		if (array.Length == 0)
		{
			return;
		}
		int num = UnityEngine.Random.Range(0, array.Length);
		Skill skill = array[num];
		skillModule.SetLastUseTime(skill.Id, 0.0);
		ChatModule chatModule;
		this.npcModule.Owner.TryGetComponent<ChatModule>(out chatModule);
		chatModule.SendSystemTranslatedMessage("skill_reset_message", "orange", false, new string[]
		{
			skill.Name
		});
	}

	// Token: 0x0400168E RID: 5774
	private NpcModule npcModule;
}
