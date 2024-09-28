using System;
using Mirror;
using UnityEngine;

// Token: 0x02000490 RID: 1168
public class NpcDoguineo : MonoBehaviour
{
	// Token: 0x06001A6A RID: 6762 RVA: 0x000875A8 File Offset: 0x000857A8
	private void Awake()
	{
		base.TryGetComponent<NpcModule>(out this.npcModule);
		base.TryGetComponent<EffectModule>(out this.effectModule);
		base.InvokeRepeating("ApplyBuff", 35f, 35f);
		if (NetworkServer.active)
		{
			this.npcModule.NetworkIsPet = true;
		}
	}

	// Token: 0x06001A6B RID: 6763 RVA: 0x000875F8 File Offset: 0x000857F8
	private void ApplyBuff()
	{
		if (this.npcModule.Owner == null)
		{
			NetworkServer.Destroy(base.gameObject);
			return;
		}
		int num = UnityEngine.Random.Range(0, 3);
		if (UnityEngine.Random.Range(0f, 1f) <= 0.85f)
		{
			switch (num)
			{
			case 0:
				this.Bark();
				break;
			case 1:
				this.Howl();
				break;
			case 2:
				this.Growl();
				break;
			}
			this.CreateConditionBoost();
		}
	}

	// Token: 0x06001A6C RID: 6764 RVA: 0x00087674 File Offset: 0x00085874
	private void Bark()
	{
		int num = UnityEngine.Random.Range(1, 5);
		string name = string.Format("bark_0{0}", num);
		EffectModule effectModule;
		this.npcModule.Owner.TryGetComponent<EffectModule>(out effectModule);
		effectModule.PlaySoundEffect(name, 1f, 0f, this.npcModule.Owner.transform.position);
		this.effectModule.ShowAnimatedText("woooof! woof!", 2, true, base.transform.position);
	}

	// Token: 0x06001A6D RID: 6765 RVA: 0x000876F0 File Offset: 0x000858F0
	private void Howl()
	{
		EffectModule effectModule;
		this.npcModule.Owner.TryGetComponent<EffectModule>(out effectModule);
		effectModule.PlaySoundEffect("howl", 1f, 0f, this.npcModule.Owner.transform.position);
		this.effectModule.ShowAnimatedText("aaah-oooooooooh!", 2, true, base.transform.position);
	}

	// Token: 0x06001A6E RID: 6766 RVA: 0x00087758 File Offset: 0x00085958
	private void Growl()
	{
		EffectModule effectModule;
		this.npcModule.Owner.TryGetComponent<EffectModule>(out effectModule);
		effectModule.PlaySoundEffect("growl", 1f, 0f, this.npcModule.Owner.transform.position);
		this.effectModule.ShowAnimatedText("grrrrrrrr!", 2, true, base.transform.position);
	}

	// Token: 0x06001A6F RID: 6767 RVA: 0x000877C0 File Offset: 0x000859C0
	private void CreateConditionBoost()
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

	// Token: 0x04001688 RID: 5768
	private NpcModule npcModule;

	// Token: 0x04001689 RID: 5769
	private EffectModule effectModule;
}
