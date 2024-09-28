using System;
using System.Collections;
using Mirror;
using UnityEngine;

// Token: 0x0200019C RID: 412
public class MagicCoreItem : ItemBase
{
	// Token: 0x060004AA RID: 1194 RVA: 0x0001A3C0 File Offset: 0x000185C0
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		ConditionModule conditionModule;
		itemBaseConfig.UserObject.TryGetComponent<ConditionModule>(out conditionModule);
		EffectModule effectModule;
		itemBaseConfig.UserObject.TryGetComponent<EffectModule>(out effectModule);
		if (conditionModule.HasActiveCondition(ConditionType.MagicCore))
		{
			effectModule.ShowScreenMessage("magic_core_overload_message", 3, 3.5f, Array.Empty<string>());
			return false;
		}
		this.ShowThunderEffects(effectModule, itemBaseConfig.UserObject.transform.position);
		base.StartCoroutine(itemBaseConfig, this.SpawnGeminiAsync(itemBaseConfig));
		MagicCoreItem.StartMagicCoreCondition(conditionModule);
		return true;
	}

	// Token: 0x060004AB RID: 1195 RVA: 0x0001A438 File Offset: 0x00018638
	private static void StartMagicCoreCondition(ConditionModule conditionModule)
	{
		Effect effect = new Effect("Thunder", 0.6f, 0.25f);
		Condition condition = new Condition(ConditionCategory.Boost, ConditionType.MagicCore, 600f, 2f, 0f, effect, 0, 0f, "");
		conditionModule.StartCondition(condition, conditionModule.gameObject, true);
		PartyModule partyModule;
		conditionModule.gameObject.TryGetComponent<PartyModule>(out partyModule);
		foreach (PartyMember partyMember in partyModule.PartyMembers)
		{
			if (partyMember.IsDefined)
			{
				ConditionModule conditionModule2;
				partyMember.Member.TryGetComponent<ConditionModule>(out conditionModule2);
				conditionModule2.StartCondition(condition, conditionModule.gameObject, true);
			}
		}
	}

	// Token: 0x060004AC RID: 1196 RVA: 0x0001A504 File Offset: 0x00018704
	private IEnumerator SpawnGeminiAsync(ItemBaseConfig itemBaseConfig)
	{
		GameObject npcObject = GlobalUtils.SpawnNpc(AssetBundleManager.Instance.NpcPrefab, ItemBase.NpcDatabaseModule, 35, itemBaseConfig.UserObject.transform.position);
		NpcModule npcModule;
		npcObject.TryGetComponent<NpcModule>(out npcModule);
		yield return new WaitUntil(() => npcModule.IsLoaded);
		NetworkServer.Spawn(npcObject, null);
		MovementModule movementModule;
		itemBaseConfig.UserObject.TryGetComponent<MovementModule>(out movementModule);
		movementModule.TargetTeleport(movementModule.connectionToClient, GlobalUtils.RandomCircle(itemBaseConfig.UserObject.transform.position, 0.32f), default(Effect));
		yield break;
	}

	// Token: 0x060004AD RID: 1197 RVA: 0x0001A514 File Offset: 0x00018714
	private void ShowThunderEffects(EffectModule effectModule, Vector3 effectPosition)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "ThunderStrike",
			EffectScaleModifier = 1f,
			EffectSpeedModifier = 0.25f,
			SoundEffectName = "thunder",
			Position = effectPosition
		};
		effectModule.ShowEffects(effectConfig);
	}
}
