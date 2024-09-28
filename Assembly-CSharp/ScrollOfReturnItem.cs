using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001CB RID: 459
public class ScrollOfReturnItem : ItemBase
{
	// Token: 0x06000549 RID: 1353 RVA: 0x0001CA00 File Offset: 0x0001AC00
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		EffectModule component = itemBaseConfig.UserObject.GetComponent<EffectModule>();
		ConditionModule component2 = itemBaseConfig.UserObject.GetComponent<ConditionModule>();
		if (!this.ValidateIfUserIsCasting(component2, component))
		{
			return false;
		}
		if (!this.ValidateIfUserIsInCombat(itemBaseConfig, component))
		{
			return false;
		}
		if (!this.ValidateIfUserIsPk(itemBaseConfig, component))
		{
			return false;
		}
		this.StartCastingCondition(itemBaseConfig, component2);
		base.StartCoroutine(itemBaseConfig, this.TeleportAfterDelay(itemBaseConfig, component));
		return true;
	}

	// Token: 0x0600054A RID: 1354 RVA: 0x0001CA64 File Offset: 0x0001AC64
	private void StartCastingCondition(ItemBaseConfig itemBaseConfig, ConditionModule conditionModule)
	{
		Condition condition = new Condition(ConditionCategory.Paralyzing, ConditionType.Cast, 7f, 1f, 0f, new Effect("TeleporterHit", 0.5f, 0.25f), 0, 0f, "default_cast");
		conditionModule.StartNeutralCondition(condition, itemBaseConfig.UserObject, true);
	}

	// Token: 0x0600054B RID: 1355 RVA: 0x0001CAB8 File Offset: 0x0001ACB8
	private bool ValidateIfUserIsPk(ItemBaseConfig itemBaseConfig, EffectModule effectModule)
	{
		PvpModule pvpModule;
		itemBaseConfig.UserObject.TryGetComponent<PvpModule>(out pvpModule);
		if (pvpModule.HasPlayerKillerLimitations())
		{
			effectModule.ShowScreenMessage("item_cant_teleport_while_pk_message", 0, 3.5f, Array.Empty<string>());
			return false;
		}
		return true;
	}

	// Token: 0x0600054C RID: 1356 RVA: 0x0001CAF4 File Offset: 0x0001ACF4
	private bool ValidateIfUserIsInCombat(ItemBaseConfig itemBaseConfig, EffectModule effectModule)
	{
		if (itemBaseConfig.UserObject.GetComponent<CombatModule>().InCombat)
		{
			effectModule.ShowScreenMessage("item_cant_teleport_right_after_combat_message", 0, 3.5f, Array.Empty<string>());
			return false;
		}
		return true;
	}

	// Token: 0x0600054D RID: 1357 RVA: 0x0001CB21 File Offset: 0x0001AD21
	private bool ValidateIfUserIsCasting(ConditionModule conditionModule, EffectModule effectModule)
	{
		if (conditionModule.HasActiveCondition(ConditionType.Cast))
		{
			effectModule.ShowScreenMessage("cant_use_scroll_of_return_right_now_message", 0, 3.5f, Array.Empty<string>());
			return false;
		}
		return true;
	}

	// Token: 0x0600054E RID: 1358 RVA: 0x0001CB46 File Offset: 0x0001AD46
	private IEnumerator TeleportAfterDelay(ItemBaseConfig itemBaseConfig, EffectModule effectModule)
	{
		CombatModule combatModule = itemBaseConfig.UserObject.GetComponent<CombatModule>();
		ConditionModule conditionModule = itemBaseConfig.UserObject.GetComponent<ConditionModule>();
		itemBaseConfig.UserObject.GetComponent<AnimationControllerModule>().RunAnimation(AnimationType.Cast);
		float elapsed = 0f;
		while (!combatModule.InCombat)
		{
			elapsed += 0.5f;
			yield return new WaitForSecondsRealtime(0.5f);
			if (elapsed > 7f)
			{
				this.TeleportUserToSpawnPoint(itemBaseConfig);
				ScrollOfReturnItem.ShowTeleportEffects(effectModule);
				yield break;
			}
		}
		effectModule.ShowScreenMessage("scroll_use_stopped_message", 3, 3.5f, Array.Empty<string>());
		conditionModule.RemoveCondition(ConditionType.Cast);
		yield break;
	}

	// Token: 0x0600054F RID: 1359 RVA: 0x0001CB64 File Offset: 0x0001AD64
	private static void ShowTeleportEffects(EffectModule effectModule)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			SoundEffectName = "teleporter_hit"
		};
		effectModule.ShowEffects(effectConfig);
		effectModule.ShowScreenMessage("item_teleported_message", 0, 3.5f, Array.Empty<string>());
	}

	// Token: 0x06000550 RID: 1360 RVA: 0x0001CBA4 File Offset: 0x0001ADA4
	private void TeleportUserToSpawnPoint(ItemBaseConfig itemBaseConfig)
	{
		MovementModule component = itemBaseConfig.UserObject.GetComponent<MovementModule>();
		Vector3 vector = GlobalUtils.GetLocationFromSpawnPoint(itemBaseConfig.Item.TeleportSpawnPoint);
		if (itemBaseConfig.Item.TeleportSpawnPoint != "wahmar_harbor" && !itemBaseConfig.UserObject.GetComponent<QuestModule>().HasQuestCompleted(52))
		{
			vector = GlobalUtils.GetLocationFromSpawnPoint("wahmar_harbor");
		}
		if (vector == Vector3.zero)
		{
			vector = component.SpawnPointLocation;
		}
		Effect teleportEffect = new Effect("TeleporterHit", 0.5f, 0.25f);
		component.TargetTeleport(component.connectionToClient, vector, teleportEffect);
	}

	// Token: 0x040007CB RID: 1995
	private const float castingTime = 7f;
}
