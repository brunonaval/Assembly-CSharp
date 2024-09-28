using System;
using System.Collections;
using System.Linq;
using Mirror;
using UnityEngine;

// Token: 0x02000497 RID: 1175
public class NpcSuperSeraphiel : MonoBehaviour
{
	// Token: 0x06001AA9 RID: 6825 RVA: 0x00088EA4 File Offset: 0x000870A4
	private void Awake()
	{
		base.TryGetComponent<NpcModule>(out this.npcModule);
		base.InvokeRepeating("ApplyBuff", 40f, 40f);
		base.InvokeRepeating("ApplyBlasusBuff", 40f, 40f);
		base.InvokeRepeating("ApplyDoguineoBuff", 40f, 40f);
		if (NetworkServer.active)
		{
			this.npcModule.NetworkIsPet = true;
		}
	}

	// Token: 0x06001AAA RID: 6826 RVA: 0x00088F10 File Offset: 0x00087110
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
		Condition condition = new Condition(ConditionCategory.Blessing, ConditionType.Power, 30f, 1f, 0.22f, effect, 0, 0f, "");
		Effect effect2 = new Effect("IceBarrier", 0.5f, 0.25f);
		Condition condition2 = new Condition(ConditionCategory.Blessing, ConditionType.Toughness, 30f, 1f, 0.22f, effect2, 0, 0f, "");
		AttributeModule attributeModule;
		this.npcModule.Owner.TryGetComponent<AttributeModule>(out attributeModule);
		float power = (float)attributeModule.MaxHealth * 0.15f;
		Effect effect3 = new Effect("RecoveryStrike", 0.5f, 0.25f);
		Condition condition3 = new Condition(ConditionCategory.Regeneration, ConditionType.Healing, 10f, 1f, power, effect3, 0, 0f, "heal_strike");
		ConditionModule conditionModule;
		this.npcModule.Owner.TryGetComponent<ConditionModule>(out conditionModule);
		conditionModule.StartCondition(condition, base.gameObject, true);
		conditionModule.StartCondition(condition2, base.gameObject, true);
		conditionModule.StartCondition(condition3, base.gameObject, true);
		this.ResetRandomSkill();
	}

	// Token: 0x06001AAB RID: 6827 RVA: 0x000890A4 File Offset: 0x000872A4
	private void ApplyBlasusBuff()
	{
		EffectModule effectModule;
		base.gameObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowEffects(new EffectConfig
		{
			EffectName = "WindCast",
			EffectScaleModifier = 0.35f,
			EffectSpeedModifier = 0.25f,
			SoundEffectName = "default_cast"
		});
		base.StartCoroutine(this.StartMaxEnduranceEffect());
	}

	// Token: 0x06001AAC RID: 6828 RVA: 0x0008910C File Offset: 0x0008730C
	private void ApplyDoguineoBuff()
	{
		ConditionModule conditionModule;
		this.npcModule.Owner.TryGetComponent<ConditionModule>(out conditionModule);
		AttributeModule attributeModule;
		this.npcModule.Owner.TryGetComponent<AttributeModule>(out attributeModule);
		float num = 0.05f;
		float duration = 30f;
		if (attributeModule.TrainingMode == TrainingMode.AxpFocused)
		{
			conditionModule.StartCondition(new Condition(ConditionCategory.Boost, ConditionType.AxpBonus, duration, 5f, num, default(Effect), 0, 0f, ""), this.npcModule.Owner, true);
			return;
		}
		if (attributeModule.TrainingMode == TrainingMode.ExpFocused)
		{
			conditionModule.StartCondition(new Condition(ConditionCategory.Boost, ConditionType.ExpBonus, duration, 5f, num, default(Effect), 0, 0f, ""), this.npcModule.Owner, true);
			return;
		}
		num *= 0.5f;
		conditionModule.StartCondition(new Condition(ConditionCategory.Boost, ConditionType.ExpBonus, duration, 5f, num, default(Effect), 0, 0f, ""), this.npcModule.Owner, true);
		conditionModule.StartCondition(new Condition(ConditionCategory.Boost, ConditionType.AxpBonus, duration, 5f, num, default(Effect), 0, 0f, ""), this.npcModule.Owner, true);
	}

	// Token: 0x06001AAD RID: 6829 RVA: 0x0008923F File Offset: 0x0008743F
	private IEnumerator StartMaxEnduranceEffect()
	{
		WaitForSecondsRealtime delay = new WaitForSecondsRealtime(1f);
		AttributeModule ownerAttributeModule;
		this.npcModule.Owner.TryGetComponent<AttributeModule>(out ownerAttributeModule);
		EffectModule ownerEffectModule;
		this.npcModule.Owner.TryGetComponent<EffectModule>(out ownerEffectModule);
		int num;
		for (int i = 0; i < 3; i = num + 1)
		{
			ownerEffectModule.ShowEffects(NpcSuperSeraphiel.CreateBlueStarEffect());
			ownerAttributeModule.AddEndurance(ownerAttributeModule.MaxEndurance);
			yield return delay;
			num = i;
		}
		yield break;
	}

	// Token: 0x06001AAE RID: 6830 RVA: 0x00089250 File Offset: 0x00087450
	private static EffectConfig CreateBlueStarEffect()
	{
		return new EffectConfig
		{
			EffectName = "BlueStarBlast",
			EffectScaleModifier = 0.5f,
			EffectSpeedModifier = 0.25f,
			SoundEffectName = "heal_strike",
			TextColorId = 4
		};
	}

	// Token: 0x06001AAF RID: 6831 RVA: 0x000892A0 File Offset: 0x000874A0
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

	// Token: 0x04001692 RID: 5778
	private NpcModule npcModule;
}
