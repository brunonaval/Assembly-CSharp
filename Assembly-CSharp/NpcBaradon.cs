using System;
using System.Linq;
using Mirror;
using UnityEngine;

// Token: 0x0200048C RID: 1164
public class NpcBaradon : MonoBehaviour
{
	// Token: 0x06001A59 RID: 6745 RVA: 0x00087214 File Offset: 0x00085414
	private void Awake()
	{
		base.TryGetComponent<NpcModule>(out this.npcModule);
		base.InvokeRepeating("ApplyBuff", 45f, 45f);
		if (NetworkServer.active)
		{
			this.npcModule.NetworkIsPet = true;
		}
	}

	// Token: 0x06001A5A RID: 6746 RVA: 0x0008724C File Offset: 0x0008544C
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

	// Token: 0x0400167D RID: 5757
	private NpcModule npcModule;
}
