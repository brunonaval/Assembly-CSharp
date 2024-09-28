using System;
using System.Collections;
using Mirror;
using UnityEngine;

// Token: 0x0200048E RID: 1166
public class NpcBlasus : MonoBehaviour
{
	// Token: 0x06001A5F RID: 6751 RVA: 0x0008739B File Offset: 0x0008559B
	private void Awake()
	{
		base.TryGetComponent<NpcModule>(out this.npcModule);
		base.InvokeRepeating("ApplyBuff", 45f, 45f);
		if (NetworkServer.active)
		{
			this.npcModule.NetworkIsPet = true;
		}
	}

	// Token: 0x06001A60 RID: 6752 RVA: 0x000873D4 File Offset: 0x000855D4
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
			EffectName = "WindCast",
			EffectScaleModifier = 0.35f,
			EffectSpeedModifier = 0.25f,
			SoundEffectName = "default_cast"
		});
		base.StartCoroutine(this.StartMaxEnduranceEffect());
	}

	// Token: 0x06001A61 RID: 6753 RVA: 0x0008745A File Offset: 0x0008565A
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
			ownerEffectModule.ShowEffects(this.CreateBlueStarEffect());
			ownerAttributeModule.AddEndurance(ownerAttributeModule.MaxEndurance);
			yield return delay;
			num = i;
		}
		yield break;
	}

	// Token: 0x06001A62 RID: 6754 RVA: 0x0008746C File Offset: 0x0008566C
	private EffectConfig CreateBlueStarEffect()
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

	// Token: 0x04001680 RID: 5760
	private NpcModule npcModule;
}
