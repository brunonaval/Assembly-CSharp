using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

// Token: 0x02000561 RID: 1377
public class TargetFinder
{
	// Token: 0x06001EA4 RID: 7844 RVA: 0x00098D18 File Offset: 0x00096F18
	public TargetFinder(CombatModule attackerCombatModule)
	{
		this.attackerCombatModule = attackerCombatModule;
	}

	// Token: 0x06001EA5 RID: 7845 RVA: 0x00098D28 File Offset: 0x00096F28
	public List<GameObject> GetTargets(int maxTargets, float range, bool appliesDamage, bool visibleOnly, bool validatePath, bool avoidInverseDirection, GameObject attacker, GameObject selectedTarget)
	{
		if (attacker == null || !attacker.activeInHierarchy)
		{
			return new List<GameObject>();
		}
		return this.GetTargetsFromPosition(maxTargets, range, appliesDamage, visibleOnly, validatePath, avoidInverseDirection, attacker.transform.position, attacker, selectedTarget);
	}

	// Token: 0x06001EA6 RID: 7846 RVA: 0x00098D70 File Offset: 0x00096F70
	public List<GameObject> GetOrderedTargets(int maxTargets, float range, bool appliesDamage, bool visibleOnly, bool validatePath, bool avoidInverseDirection, GameObject attacker, GameObject selectedTarget)
	{
		List<GameObject> list = this.GetTargets(maxTargets, range, appliesDamage, visibleOnly, validatePath, avoidInverseDirection, attacker, selectedTarget).ToList<GameObject>();
		list.Sort((GameObject a, GameObject b) => Vector2.Distance(attacker.transform.position, a.transform.position).CompareTo(Vector2.Distance(attacker.transform.position, b.transform.position)));
		return list;
	}

	// Token: 0x06001EA7 RID: 7847 RVA: 0x00098DBC File Offset: 0x00096FBC
	public List<GameObject> GetTargetsFromPosition(int maxTargets, float range, bool appliesDamage, bool visibleOnly, bool validatePath, bool avoidInverseDirection, Vector3 position, GameObject attacker, GameObject selectedTarget)
	{
		List<GameObject> list = new List<GameObject>();
		maxTargets = this.AddSingleTargetToTargets(maxTargets, range, true, appliesDamage, visibleOnly, validatePath, false, position, attacker, selectedTarget, list);
		if (maxTargets == 0)
		{
			return list;
		}
		this.AddTargets(maxTargets, range, appliesDamage, visibleOnly, validatePath, avoidInverseDirection, position, attacker, selectedTarget, list);
		return list;
	}

	// Token: 0x06001EA8 RID: 7848 RVA: 0x00098E04 File Offset: 0x00097004
	private int AddTargets(int maxTargets, float range, bool appliesDamage, bool visibleOnly, bool validatePath, bool avoidInverseDirection, Vector3 position, GameObject attacker, GameObject selectedTarget, List<GameObject> targets)
	{
		List<GameObject> nearbyTagets = this.attackerCombatModule.GetNearbyTagets();
		for (int i = 0; i < nearbyTagets.Count; i++)
		{
			if (!(nearbyTagets[i] == null) && nearbyTagets[i].activeInHierarchy && !(nearbyTagets[i] == selectedTarget))
			{
				if (this.IsTargetValid(range, appliesDamage, visibleOnly, validatePath, avoidInverseDirection, position, attacker, nearbyTagets[i]))
				{
					targets.Add(nearbyTagets[i]);
					maxTargets--;
				}
				if (maxTargets == 0)
				{
					break;
				}
			}
		}
		return maxTargets;
	}

	// Token: 0x06001EA9 RID: 7849 RVA: 0x00098E90 File Offset: 0x00097090
	public List<GameObject> GetFriendlyTargets(int maxTargets, float range, bool avoidSelf, bool visibleOnly, bool validatePath, bool avoidInverseDirection, GameObject friendlyAttacker, GameObject selectedTarget)
	{
		return this.GetFriendlyTargetsFromPosition(maxTargets, range, avoidSelf, visibleOnly, validatePath, avoidInverseDirection, friendlyAttacker.transform.position, friendlyAttacker, selectedTarget);
	}

	// Token: 0x06001EAA RID: 7850 RVA: 0x00098EBC File Offset: 0x000970BC
	public List<GameObject> GetFriendlyTargetsFromPosition(int maxTargets, float range, bool avoidSelf, bool visibleOnly, bool validatePath, bool avoidInverseDirection, Vector3 position, GameObject friendlyAttacker, GameObject selectedTarget)
	{
		List<GameObject> list = new List<GameObject>();
		maxTargets = this.AddSingleTargetToTargets(maxTargets, range, avoidSelf, false, visibleOnly, validatePath, false, position, friendlyAttacker, friendlyAttacker, list);
		maxTargets = this.AddSingleTargetToTargets(maxTargets, range, avoidSelf, false, visibleOnly, validatePath, false, position, friendlyAttacker, selectedTarget, list);
		if (maxTargets == 0)
		{
			return list;
		}
		maxTargets = this.AddPartyMembersToTargets(maxTargets, range, avoidSelf, visibleOnly, position, friendlyAttacker, selectedTarget, list);
		if (maxTargets == 0)
		{
			return list;
		}
		this.AddFriendlyNearbyTargets(maxTargets, range, avoidSelf, visibleOnly, position, friendlyAttacker, selectedTarget, list);
		return list;
	}

	// Token: 0x06001EAB RID: 7851 RVA: 0x00098F34 File Offset: 0x00097134
	private int AddFriendlyNearbyTargets(int maxTargets, float range, bool avoidSelf, bool visibleOnly, Vector3 position, GameObject friendlyAttacker, GameObject selectedTarget, List<GameObject> targets)
	{
		List<GameObject> nearbyTagets = this.attackerCombatModule.GetNearbyTagets();
		for (int i = 0; i < nearbyTagets.Count; i++)
		{
			if (!(nearbyTagets[i] == selectedTarget))
			{
				if (this.IsFriendlyTargetValid(range, false, avoidSelf, visibleOnly, position, friendlyAttacker, nearbyTagets[i]))
				{
					targets.Add(nearbyTagets[i]);
					maxTargets--;
				}
				if (maxTargets == 0)
				{
					break;
				}
			}
		}
		return maxTargets;
	}

	// Token: 0x06001EAC RID: 7852 RVA: 0x00098FA0 File Offset: 0x000971A0
	private int AddSingleTargetToTargets(int maxTargets, float range, bool avoidSelf, bool appliesDamage, bool visibleOnly, bool validatePath, bool avoidInverseDirection, Vector3 position, GameObject attacker, GameObject selectedTarget, List<GameObject> targets)
	{
		if (avoidSelf & attacker == selectedTarget)
		{
			return maxTargets;
		}
		if (selectedTarget != null && this.IsTargetValid(range, appliesDamage, visibleOnly, validatePath, avoidInverseDirection, position, attacker, selectedTarget))
		{
			targets.Add(selectedTarget);
			maxTargets--;
		}
		return maxTargets;
	}

	// Token: 0x06001EAD RID: 7853 RVA: 0x00098FEC File Offset: 0x000971EC
	private int AddPartyMembersToTargets(int maxTargets, float range, bool avoidSelf, bool visibleOnly, Vector3 position, GameObject friendlyAttacker, GameObject selectedTarget, List<GameObject> targets)
	{
		PartyModule partyModule;
		friendlyAttacker.TryGetComponent<PartyModule>(out partyModule);
		if (partyModule != null)
		{
			foreach (PartyMember partyMember in partyModule.PartyMembers)
			{
				if (!(partyMember.Member == selectedTarget) && this.IsFriendlyTargetValid(range, false, avoidSelf, visibleOnly, position, friendlyAttacker, partyMember.Member))
				{
					targets.Add(partyMember.Member);
					maxTargets--;
					if (maxTargets == 0)
					{
						break;
					}
				}
			}
		}
		return maxTargets;
	}

	// Token: 0x06001EAE RID: 7854 RVA: 0x0009908C File Offset: 0x0009728C
	public bool IsDeadFriendlyTargetValid(float range, Vector3 position, GameObject attacker, GameObject target)
	{
		if (target == null || !target.activeInHierarchy)
		{
			return false;
		}
		CreatureModule creatureModule;
		target.TryGetComponent<CreatureModule>(out creatureModule);
		return (!(creatureModule != null) || !creatureModule.IsAlive) && this.IsFriendlyTargetTagValid(attacker, target) && this.IsTargetCloseToPosition(range, position, target) && TargetFinder.HasValidPath(attacker, target);
	}

	// Token: 0x06001EAF RID: 7855 RVA: 0x000990F4 File Offset: 0x000972F4
	public bool IsFriendlyTargetValid(float range, bool appliesDamage, bool avoidSelf, bool visibleOnly, Vector3 position, GameObject attacker, GameObject target)
	{
		if (target == null || !target.activeInHierarchy)
		{
			return false;
		}
		if (!this.IsPvpEnabled(attacker) & this.IsPvpEnabled(target))
		{
			return false;
		}
		if (!this.IsFriendlyTargetTagValid(attacker, target))
		{
			return false;
		}
		if (!this.IsTargetCloseToPosition(range, position, target))
		{
			return false;
		}
		if (!this.CanCauseDamageOnTarget(appliesDamage, attacker, target))
		{
			return false;
		}
		if (avoidSelf & target == attacker)
		{
			return false;
		}
		if (visibleOnly & !this.IsTargetVisible(target))
		{
			return false;
		}
		PartyModule partyModule;
		attacker.TryGetComponent<PartyModule>(out partyModule);
		bool flag = false;
		for (int i = 0; i < partyModule.PartyMembers.Count; i++)
		{
			if (partyModule.PartyMembers[i].Member == target)
			{
				flag = true;
				break;
			}
		}
		if (attacker.CompareTag("Player") && target.CompareTag("Player"))
		{
			PvpModule pvpModule;
			attacker.TryGetComponent<PvpModule>(out pvpModule);
			PvpModule pvpModule2;
			target.TryGetComponent<PvpModule>(out pvpModule2);
			if (pvpModule.TvtTeam > TvtTeam.None & pvpModule2.TvtTeam > TvtTeam.None & pvpModule.TvtTeam == pvpModule2.TvtTeam)
			{
				flag = true;
			}
		}
		return !(this.PlayerIsPvpOrPk(attacker) & attacker != target & !flag) && !(this.PlayerIsPvpOrPk(target) & attacker != target & !flag) && (!(attacker != target) || TargetFinder.HasValidPath(attacker, target));
	}

	// Token: 0x06001EB0 RID: 7856 RVA: 0x00099264 File Offset: 0x00097464
	public bool IsTargetValid(float range, bool appliesDamage, bool visibleOnly, bool validatePath, bool avoidInverseDirection, Vector3 position, GameObject attacker, GameObject target)
	{
		return !(target == null) && target.activeInHierarchy && !(this.IsPvpEnabled(attacker) & !target.CompareTag("Player")) && this.IsTargetTagValid(attacker, target) && this.IsTargetCloseToPosition(range, position, target) && this.CanCauseDamageOnTarget(appliesDamage, attacker, target) && this.AttackerCanGetTargets(attacker) && !(visibleOnly & !this.IsTargetVisible(target)) && !(avoidInverseDirection & this.IsTargetInInverseDirection(attacker, target)) && !(validatePath & !TargetFinder.HasValidPath(attacker, target));
	}

	// Token: 0x06001EB1 RID: 7857 RVA: 0x00099310 File Offset: 0x00097510
	private bool IsPvpEnabled(GameObject attacker)
	{
		PvpModule pvpModule;
		return attacker.TryGetComponent<PvpModule>(out pvpModule) && pvpModule.PvpEnabled;
	}

	// Token: 0x06001EB2 RID: 7858 RVA: 0x00099330 File Offset: 0x00097530
	private bool IsTargetInInverseDirection(GameObject attacker, GameObject target)
	{
		MovementModule movementModule;
		if (!attacker.TryGetComponent<MovementModule>(out movementModule))
		{
			return false;
		}
		Direction direction = GlobalUtils.FindTargetDirection(attacker.transform.position, target.transform.position);
		Direction direction2 = GlobalUtils.InverseDirection(movementModule.Direction);
		return direction == direction2;
	}

	// Token: 0x06001EB3 RID: 7859 RVA: 0x00099374 File Offset: 0x00097574
	private bool IsTargetVisible(GameObject target)
	{
		if (target == null)
		{
			return false;
		}
		ConditionModule conditionModule;
		target.TryGetComponent<ConditionModule>(out conditionModule);
		return !conditionModule.HasActiveCondition(ConditionCategory.Invisibility);
	}

	// Token: 0x06001EB4 RID: 7860 RVA: 0x000993A0 File Offset: 0x000975A0
	private bool PlayerIsPvpOrPk(GameObject player)
	{
		if (player == null || !player.activeInHierarchy)
		{
			return false;
		}
		if (!player.CompareTag("Player"))
		{
			return false;
		}
		PvpModule pvpModule;
		player.TryGetComponent<PvpModule>(out pvpModule);
		return pvpModule.PvpStatus > PvpStatus.Neutral;
	}

	// Token: 0x06001EB5 RID: 7861 RVA: 0x000993E4 File Offset: 0x000975E4
	private bool AttackerCanGetTargets(GameObject attacker)
	{
		if (attacker == null || !attacker.activeInHierarchy)
		{
			return false;
		}
		if (attacker.CompareTag("Player"))
		{
			return true;
		}
		ConditionModule conditionModule;
		attacker.TryGetComponent<ConditionModule>(out conditionModule);
		return !conditionModule.HasActiveCondition(ConditionType.Confusion);
	}

	// Token: 0x06001EB6 RID: 7862 RVA: 0x00099428 File Offset: 0x00097628
	private bool IsFriendlyTargetTagValid(GameObject attacker, GameObject target)
	{
		if (attacker.CompareTag("Player"))
		{
			return target.CompareTag("Player");
		}
		if (attacker.CompareTag("Monster"))
		{
			return target.CompareTag("Monster");
		}
		return !attacker.CompareTag("Combatant") || (target.CompareTag("Player") | target.CompareTag("Combatant"));
	}

	// Token: 0x06001EB7 RID: 7863 RVA: 0x00099490 File Offset: 0x00097690
	private bool IsTargetTagValid(GameObject attacker, GameObject target)
	{
		if (attacker == null || !attacker.activeInHierarchy)
		{
			return false;
		}
		if (target == null || !target.activeInHierarchy)
		{
			return false;
		}
		if (attacker.CompareTag("Player") | attacker.CompareTag("Combatant"))
		{
			return target.CompareTag("Player") | target.CompareTag("Monster");
		}
		return !attacker.CompareTag("Monster") || (target.CompareTag("Player") | target.CompareTag("Combatant"));
	}

	// Token: 0x06001EB8 RID: 7864 RVA: 0x0009951C File Offset: 0x0009771C
	public static bool HasValidPath(GameObject attacker, GameObject target)
	{
		Vector3 v = target.transform.position - attacker.transform.position;
		int layerMask = 1 << LayerMask.NameToLayer("Tile");
		float distance = Vector2.Distance(target.transform.position, attacker.transform.position);
		RaycastHit2D raycastHit2D = Physics2D.Raycast(attacker.transform.position, v, distance, layerMask);
		return !(raycastHit2D.collider != null) || !(raycastHit2D.collider.gameObject.CompareTag("BlockAll") | raycastHit2D.collider.gameObject.CompareTag("BlockProjectile"));
	}

	// Token: 0x06001EB9 RID: 7865 RVA: 0x000995DC File Offset: 0x000977DC
	public static bool HasValidWalkablePath(GameObject attacker, GameObject target)
	{
		Vector3 v = target.transform.position - attacker.transform.position;
		int layerMask = 1 << LayerMask.NameToLayer("Tile");
		float distance = Vector2.Distance(target.transform.position, attacker.transform.position);
		RaycastHit2D raycastHit2D = Physics2D.Raycast(attacker.transform.position, v, distance, layerMask);
		if (raycastHit2D.collider == null)
		{
			return true;
		}
		if (raycastHit2D.collider.gameObject == null)
		{
			return true;
		}
		GameObject gameObject = raycastHit2D.collider.gameObject;
		return !(gameObject.CompareTag("BlockAll") | gameObject.CompareTag("BlockProjectile") | gameObject.CompareTag("BlockCreature"));
	}

	// Token: 0x06001EBA RID: 7866 RVA: 0x000996BC File Offset: 0x000978BC
	private bool CanCauseDamageOnTarget(bool appliesDamage, GameObject attacker, GameObject target)
	{
		bool result = true;
		if (appliesDamage)
		{
			PvpModule pvpModule;
			attacker.TryGetComponent<PvpModule>(out pvpModule);
			if (pvpModule != null && !pvpModule.PvpEnabled)
			{
				result = !target.CompareTag(attacker.tag);
			}
			if (target == attacker)
			{
				result = false;
			}
			if (attacker.CompareTag("Player") && target.CompareTag("Player"))
			{
				PvpModule pvpModule2;
				target.TryGetComponent<PvpModule>(out pvpModule2);
				if (pvpModule.TvtTeam > TvtTeam.None & pvpModule2.TvtTeam == TvtTeam.None)
				{
					return false;
				}
				if (pvpModule.TvtTeam == TvtTeam.None & pvpModule2.TvtTeam > TvtTeam.None)
				{
					return false;
				}
				if ((pvpModule.TvtTeam > TvtTeam.None & pvpModule2.TvtTeam > TvtTeam.None) && pvpModule.TvtTeam == pvpModule2.TvtTeam)
				{
					return false;
				}
				if (pvpModule.TvtTeam == TvtTeam.None && pvpModule2.TvtTeam == TvtTeam.None)
				{
					PartyModule partyModule;
					attacker.TryGetComponent<PartyModule>(out partyModule);
					if (partyModule != null)
					{
						using (SyncList<PartyMember>.Enumerator enumerator = partyModule.PartyMembers.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (enumerator.Current.Member == target)
								{
									result = false;
									break;
								}
							}
						}
					}
					GuildModule guildModule;
					attacker.TryGetComponent<GuildModule>(out guildModule);
					GuildModule guildModule2;
					target.TryGetComponent<GuildModule>(out guildModule2);
					if (guildModule.ActiveGuildId != 0 & guildModule2.ActiveGuildId != 0 & guildModule.ActiveGuildId == guildModule2.ActiveGuildId)
					{
						return false;
					}
				}
			}
			if (attacker.CompareTag("Combatant"))
			{
				if (target.CompareTag("Player"))
				{
					PvpModule pvpModule3;
					target.TryGetComponent<PvpModule>(out pvpModule3);
					if (!pvpModule3.HasPlayerKillerLimitations())
					{
						result = false;
					}
				}
				if (target.CompareTag("Monster"))
				{
					MonsterModule monsterModule;
					target.TryGetComponent<MonsterModule>(out monsterModule);
					if (monsterModule.IsPassive)
					{
						result = false;
					}
				}
			}
		}
		CreatureModule creatureModule;
		target.TryGetComponent<CreatureModule>(out creatureModule);
		if (creatureModule != null && !creatureModule.IsAlive)
		{
			result = false;
		}
		return result;
	}

	// Token: 0x06001EBB RID: 7867 RVA: 0x000998A4 File Offset: 0x00097AA4
	private bool IsTargetCloseToPosition(float range, Vector3 position, GameObject target)
	{
		return GlobalUtils.IsClose(position, target.transform.position, range);
	}

	// Token: 0x04001898 RID: 6296
	private readonly CombatModule attackerCombatModule;
}
