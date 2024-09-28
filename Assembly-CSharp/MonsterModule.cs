using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000381 RID: 897
public class MonsterModule : MonoBehaviour
{
	// Token: 0x170001EB RID: 491
	// (get) Token: 0x060011E3 RID: 4579 RVA: 0x000553FE File Offset: 0x000535FE
	// (set) Token: 0x060011E4 RID: 4580 RVA: 0x00055406 File Offset: 0x00053606
	public bool IsBoss { get; private set; }

	// Token: 0x170001EC RID: 492
	// (get) Token: 0x060011E5 RID: 4581 RVA: 0x0005540F File Offset: 0x0005360F
	// (set) Token: 0x060011E6 RID: 4582 RVA: 0x00055417 File Offset: 0x00053617
	public int Attack { get; private set; }

	// Token: 0x170001ED RID: 493
	// (get) Token: 0x060011E7 RID: 4583 RVA: 0x00055420 File Offset: 0x00053620
	// (set) Token: 0x060011E8 RID: 4584 RVA: 0x00055428 File Offset: 0x00053628
	public int Defense { get; private set; }

	// Token: 0x170001EE RID: 494
	// (get) Token: 0x060011E9 RID: 4585 RVA: 0x00055431 File Offset: 0x00053631
	// (set) Token: 0x060011EA RID: 4586 RVA: 0x00055439 File Offset: 0x00053639
	public float SpawnDelay { get; private set; }

	// Token: 0x170001EF RID: 495
	// (get) Token: 0x060011EB RID: 4587 RVA: 0x00055442 File Offset: 0x00053642
	// (set) Token: 0x060011EC RID: 4588 RVA: 0x0005544A File Offset: 0x0005364A
	public bool IsPassive { get; private set; }

	// Token: 0x060011ED RID: 4589 RVA: 0x00055454 File Offset: 0x00053654
	private void Awake()
	{
		base.TryGetComponent<CombatModule>(out this.combatModule);
		base.TryGetComponent<EffectModule>(out this.effectModule);
		base.TryGetComponent<CreatureModule>(out this.creatureModule);
		base.TryGetComponent<MovementModule>(out this.movementModule);
		base.TryGetComponent<ConditionModule>(out this.conditionModule);
		base.TryGetComponent<CircleCollider2D>(out this.circleCollider2D);
		base.TryGetComponent<NonPlayerAIModule>(out this.nonPlayerAIModule);
		base.TryGetComponent<NonPlayerSkillModule>(out this.nonPlayerSkillModule);
		base.TryGetComponent<NonPlayerAttributeModule>(out this.nonPlayerAttributeModule);
		base.TryGetComponent<NonPlayerEquipmentModule>(out this.nonPlayerEquipmentModule);
		if (NetworkServer.active)
		{
			UnityEngine.Object.Destroy(this.localHud);
			GameObject gameObject = GameObject.FindGameObjectWithTag("GameEnvironment");
			this.gameEnvironmentModule = gameObject.GetComponent<GameEnvironmentModule>();
			GameObject gameObject2 = GameObject.FindGameObjectWithTag("DatabaseManager");
			this.itemDatabaseModule = gameObject2.GetComponent<ItemDatabaseModule>();
			this.monsterDatabaseModule = gameObject2.GetComponent<MonsterDatabaseModule>();
			this.creatureModule.OnKilled += this.HandleOnKilled;
			this.combatModule.OnReceiveDamage += this.HandleOnReceiveDamage;
			base.InvokeRepeating("Regeneration", 0f, 3f);
			base.InvokeRepeating("CheckDistance", 0f, 5f);
			base.InvokeRepeating("FindTargets", 0f, 1f);
		}
		if (NetworkClient.active)
		{
			base.InvokeRepeating("HandleVisibilityHud", 0f, 0.1f);
		}
	}

	// Token: 0x060011EE RID: 4590 RVA: 0x000555BB File Offset: 0x000537BB
	private void Start()
	{
		this.lastChangeTargetTime = Time.time;
	}

	// Token: 0x060011EF RID: 4591 RVA: 0x000555C8 File Offset: 0x000537C8
	private void OnDestroy()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		this.creatureModule.OnKilled -= this.HandleOnKilled;
		this.combatModule.OnReceiveDamage -= this.HandleOnReceiveDamage;
	}

	// Token: 0x060011F0 RID: 4592 RVA: 0x00055600 File Offset: 0x00053800
	private void HandleOnReceiveDamage(GameObject attacker, int damage)
	{
		if (attacker == null)
		{
			return;
		}
		this.totalDamageReceived += Mathf.Min(damage, this.nonPlayerAttributeModule.CurrentHealth);
		if (!this.IsPassive && this.combatModule.Target == null)
		{
			this.combatModule.SetTarget(attacker, false);
		}
		if (attacker.CompareTag("Player"))
		{
			attacker.GetComponent<ChatModule>().SendSystemApplyDamageMessage(this.creatureModule.CreatureName, this.creatureModule.AllowRankNamePrefix, this.creatureModule.Rank, damage);
		}
	}

	// Token: 0x060011F1 RID: 4593 RVA: 0x00055698 File Offset: 0x00053898
	private void HandleOnKilled(GameObject killer, List<Attacker> attackers)
	{
		float num = 15f;
		int num2 = this.totalDamageReceived / this.combatModule.Attackers.Count;
		int num3 = this.IsBoss ? Mathf.RoundToInt((float)num2 * 0.01f) : Mathf.RoundToInt((float)num2 * 0.1f);
		int num4 = 0;
		GameObject owner = null;
		HashSet<GameObject> hashSet = new HashSet<GameObject>();
		foreach (Attacker attacker in this.combatModule.Attackers)
		{
			if (attacker.IsDefined && attacker.AttackerObject.CompareTag("Player") && GlobalUtils.IsClose(base.transform.position, attacker.AttackerObject.transform.position, num))
			{
				CreatureModule creatureModule;
				attacker.AttackerObject.TryGetComponent<CreatureModule>(out creatureModule);
				if (creatureModule.IsAlive)
				{
					if (attacker.DamageDealt > num4)
					{
						owner = attacker.AttackerObject;
						num4 = attacker.DamageDealt;
					}
					PartyModule partyModule;
					attacker.AttackerObject.TryGetComponent<PartyModule>(out partyModule);
					VocationModule vocationModule;
					attacker.AttackerObject.TryGetComponent<VocationModule>(out vocationModule);
					List<PartyMember> partyMembersCloseToPosition = partyModule.GetPartyMembersCloseToPosition(base.transform.position, num);
					int highestMemberLevelCloseToPosition = partyModule.GetHighestMemberLevelCloseToPosition(base.transform.position, num);
					int num5 = attacker.DamageDealt;
					HashSet<Vocation> hashSet2 = new HashSet<Vocation>
					{
						vocationModule.Vocation
					};
					using (List<PartyMember>.Enumerator enumerator2 = partyMembersCloseToPosition.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							PartyMember partyMember = enumerator2.Current;
							VocationModule vocationModule2;
							partyMember.Member.TryGetComponent<VocationModule>(out vocationModule2);
							if (!hashSet2.Contains(vocationModule2.Vocation))
							{
								hashSet2.Add(vocationModule2.Vocation);
							}
							Attacker attacker2 = this.combatModule.Attackers.FirstOrDefault((Attacker a) => a.AttackerObject == partyMember.Member);
							if (attacker2 != null && attacker2.IsDefined)
							{
								num5 += attacker2.DamageDealt;
							}
						}
					}
					if (num5 >= num3)
					{
						int partySize = partyMembersCloseToPosition.Count + 1;
						int count = hashSet2.Count;
						if (!hashSet.Contains(attacker.AttackerObject))
						{
							hashSet.Add(attacker.AttackerObject);
							this.ProcessPlayerReward(num2, highestMemberLevelCloseToPosition, num5, partySize, count, attacker.AttackerObject);
						}
						foreach (PartyMember partyMember2 in partyMembersCloseToPosition)
						{
							if (!hashSet.Contains(partyMember2.Member))
							{
								hashSet.Add(partyMember2.Member);
								this.ProcessPlayerReward(num2, highestMemberLevelCloseToPosition, num5, partySize, count, partyMember2.Member);
							}
						}
					}
				}
			}
		}
		this.combatModule.Attackers.Clear();
		this.SpawnLoot(owner);
		if (this.Summoner != null)
		{
			MonsterModule monsterModule;
			this.Summoner.TryGetComponent<MonsterModule>(out monsterModule);
			if (monsterModule.Summons != null)
			{
				monsterModule.Summons.Remove(base.gameObject);
			}
		}
		if (this.Summons != null)
		{
			foreach (GameObject obj in this.Summons)
			{
				NetworkServer.Destroy(obj);
			}
			this.Summons = null;
		}
		if (!this.allowRespawn || this.Summoner != null)
		{
			NetworkServer.Destroy(base.gameObject);
			return;
		}
		base.StartCoroutine(this.MonsterRespawn());
	}

	// Token: 0x060011F2 RID: 4594 RVA: 0x00055AA4 File Offset: 0x00053CA4
	private IEnumerator MonsterRespawn()
	{
		if (!this.allowRespawn)
		{
			yield break;
		}
		base.tag = "DeadMonster";
		this.circleCollider2D.isTrigger = true;
		this.combatModule.RemoveTarget();
		this.nonPlayerAIModule.SetCanMove(false);
		this.nonPlayerAIModule.RemoveDestination();
		yield return this.ShowSpawnDelayEffects(this.SpawnDelay);
		this.ChangeMonsterRankRandomically();
		this.nonPlayerAttributeModule.ResetHealth();
		base.tag = "Monster";
		this.totalDamageReceived = 0;
		this.circleCollider2D.isTrigger = false;
		this.nonPlayerAIModule.SetCanMove(true);
		this.creatureModule.SetAlive(true);
		this.effectModule.ShowVisualEffect("TeleporterHit", 0.6f, 0.3f, 0f, this.movementModule.SpawnPointLocation);
		yield break;
	}

	// Token: 0x060011F3 RID: 4595 RVA: 0x00055AB3 File Offset: 0x00053CB3
	private void ChangeMonsterRankRandomically()
	{
		if (!this.hasFixedRank & !this.IsBoss)
		{
			this.creatureModule.SetRank(GlobalUtils.GenerateRandomRank());
			this.nonPlayerAttributeModule.AddAttributeRankBlessings(this.creatureModule.Rank);
		}
	}

	// Token: 0x060011F4 RID: 4596 RVA: 0x00055AF0 File Offset: 0x00053CF0
	private IEnumerator ShowSpawnDelayEffects(float delay)
	{
		int maxDelayWithEffect = 4;
		int delayWithFastEffect = 2;
		if (delay > (float)maxDelayWithEffect)
		{
			delay -= (float)maxDelayWithEffect;
			yield return new WaitForSecondsRealtime(delay);
			delay = (float)maxDelayWithEffect;
		}
		this.creatureModule.SetRespawning(true);
		this.movementModule.SetDirection(Direction.South);
		this.movementModule.Teleport(this.movementModule.SpawnPointLocation, default(Effect));
		int num;
		for (int i = (int)delay; i > 0; i = num - 1)
		{
			if (i <= delayWithFastEffect | (i > delayWithFastEffect & i <= maxDelayWithEffect & i % 2 == 0))
			{
				this.effectModule.ShowVisualEffect("AirBlast", 0.25f, 0.5f, 0f, this.movementModule.SpawnPointLocation);
			}
			yield return new WaitForSecondsRealtime(1f);
			num = i;
		}
		yield break;
	}

	// Token: 0x060011F5 RID: 4597 RVA: 0x00055B08 File Offset: 0x00053D08
	private void SpawnLoot(GameObject owner)
	{
		if (this.IsPassive & this.experience == 0 & !this.movementModule.Movable)
		{
			return;
		}
		if (owner != null)
		{
			int num = 0;
			List<PossibleLoot> list = MonsterDatabaseModule.PossibleLoots[this.MonsterId];
			this.SpawnEventTokens(owner);
			this.SpawnGlobalEventItems(owner);
			for (int i = 0; i < list.Count; i++)
			{
				try
				{
					PossibleLoot possibleLoot = list[i];
					if (UnityEngine.Random.Range(0f, 1f) <= possibleLoot.DropChance)
					{
						int num2 = UnityEngine.Random.Range(possibleLoot.MinAmount, possibleLoot.MaxAmount);
						if (num2 >= 1)
						{
							Item item = possibleLoot.Item;
							item.Amount = num2;
							Vector3 position = base.transform.position;
							if (num > 0)
							{
								float radius = UnityEngine.Random.Range(0.08f, 0.32f);
								position = GlobalUtils.RandomCircle(base.transform.position, radius);
							}
							GlobalUtils.SpawnItemOnGround(item, owner, position, 20f);
							num++;
						}
					}
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
		}
	}

	// Token: 0x060011F6 RID: 4598 RVA: 0x00055C2C File Offset: 0x00053E2C
	private void SpawnGlobalEventItems(GameObject owner)
	{
		int layerMask = 1 << LayerMask.NameToLayer("WorldArea");
		if (Physics2D.OverlapCircle(base.transform.position, 0.32f, layerMask).GetComponent<WorldAreaModule>().AreaType.ToLower() == "event")
		{
			return;
		}
		foreach (GlobalEventConfig globalEventConfig in GlobalEventModule.Singleton.ActiveGlobalEvents)
		{
			if (this.nonPlayerAttributeModule.BaseLevel >= globalEventConfig.MinLevel && UnityEngine.Random.Range(0f, 100f) <= globalEventConfig.DropChance)
			{
				Item item = this.itemDatabaseModule.GetItem(globalEventConfig.ItemId);
				item.Amount = 1;
				float radius = UnityEngine.Random.Range(0.08f, 0.32f);
				Vector3 position = GlobalUtils.RandomCircle(base.transform.position, radius);
				GlobalUtils.SpawnItemOnGround(item, owner, position, 20f);
			}
		}
	}

	// Token: 0x060011F7 RID: 4599 RVA: 0x00055D40 File Offset: 0x00053F40
	private void SpawnEventTokens(GameObject owner)
	{
		int layerMask = 1 << LayerMask.NameToLayer("WorldArea");
		if (Physics2D.OverlapCircle(base.transform.position, 0.32f, layerMask).GetComponent<WorldAreaModule>().AreaType.ToLower() == "event")
		{
			return;
		}
		if (ServerSettingsManager.CopperEventActive)
		{
			this.SpawnEventToken(owner, 452, 1f);
		}
		if (ServerSettingsManager.SilverEventActive)
		{
			this.SpawnEventToken(owner, 450, 0.5f);
		}
		if (ServerSettingsManager.GoldEventActive)
		{
			this.SpawnEventToken(owner, 451, 0.25f);
		}
	}

	// Token: 0x060011F8 RID: 4600 RVA: 0x00055DDC File Offset: 0x00053FDC
	private void SpawnEventToken(GameObject owner, int tokenItemId, float dropChanceMultiplier)
	{
		float num = 0.009f;
		num *= dropChanceMultiplier;
		if (UnityEngine.Random.Range(0f, 1f) <= num)
		{
			Item item = this.itemDatabaseModule.GetItem(tokenItemId);
			item.Amount = 1;
			float radius = UnityEngine.Random.Range(0.08f, 0.32f);
			Vector3 position = GlobalUtils.RandomCircle(base.transform.position, radius);
			GlobalUtils.SpawnItemOnGround(item, owner, position, 20f);
		}
	}

	// Token: 0x060011F9 RID: 4601 RVA: 0x00055E4C File Offset: 0x0005404C
	private void ProcessPlayerReward(int averageDamageDealt, int partyHighestMemberLevel, int totalDamageDealt, int partySize, int distinctPartySize, GameObject player)
	{
		AttributeModule attributeModule;
		player.TryGetComponent<AttributeModule>(out attributeModule);
		if (!GlobalUtils.CanShareRewards(partyHighestMemberLevel, attributeModule.BaseLevel, attributeModule.MasteryLevel))
		{
			return;
		}
		int num = this.experience;
		if (!this.IsBoss)
		{
			float b = (float)totalDamageDealt / (float)this.totalDamageReceived;
			num = Mathf.RoundToInt((float)this.experience * Mathf.Min(1f, b));
		}
		float num2 = (float)num;
		num2 = this.CalculateRankBonus(num2);
		num2 = this.CalculateBaseLevelBonus(num2, attributeModule);
		num2 = (float)this.CalculatePartyBonusOrPenalty(attributeModule, partySize, distinctPartySize, partyHighestMemberLevel, num2);
		this.ReducesKarmaPointsFromPlayerKiller(player, Mathf.CeilToInt(num2));
		this.AddExperienceToPlayer(player, Mathf.CeilToInt(num2));
		this.UpdatePlayerQuests(player, partyHighestMemberLevel);
		this.ProcessBossReward(player, partySize > 1, totalDamageDealt * 100 / averageDamageDealt);
	}

	// Token: 0x060011FA RID: 4602 RVA: 0x00055F08 File Offset: 0x00054108
	private void ProcessBossReward(GameObject player, bool isPartyDamage, int totalDamageDealtPercent)
	{
		if (!this.IsBoss)
		{
			return;
		}
		if (totalDamageDealtPercent >= 85)
		{
			if ((this.bossEnergyDropChance != 0f & this.bossEnergyItemId != 0) && UnityEngine.Random.Range(0f, 1f) <= this.bossEnergyDropChance)
			{
				this.AddBossRewardToAttacker(player, isPartyDamage, (double)totalDamageDealtPercent, this.bossEnergyItemId);
			}
			if (this.nonPlayerAttributeModule.BaseLevel >= 850)
			{
				this.AddBossRewardToAttacker(player, isPartyDamage, (double)totalDamageDealtPercent, 395);
				this.AddBossRewardToAttacker(player, isPartyDamage, (double)totalDamageDealtPercent, 396);
				this.AddBossRewardToAttacker(player, isPartyDamage, (double)totalDamageDealtPercent, 449);
				return;
			}
			if (this.nonPlayerAttributeModule.BaseLevel >= 400)
			{
				this.AddBossRewardToAttacker(player, isPartyDamage, (double)totalDamageDealtPercent, 395);
				this.AddBossRewardToAttacker(player, isPartyDamage, (double)totalDamageDealtPercent, 396);
				return;
			}
			this.AddBossRewardToAttacker(player, isPartyDamage, (double)totalDamageDealtPercent, 1262);
			return;
		}
		else if (totalDamageDealtPercent >= 75)
		{
			if (this.nonPlayerAttributeModule.BaseLevel >= 850)
			{
				this.AddBossRewardToAttacker(player, isPartyDamage, (double)totalDamageDealtPercent, 396);
				this.AddBossRewardToAttacker(player, isPartyDamage, (double)totalDamageDealtPercent, 395);
			}
			if (this.nonPlayerAttributeModule.BaseLevel >= 400)
			{
				this.AddBossRewardToAttacker(player, isPartyDamage, (double)totalDamageDealtPercent, 1262);
				return;
			}
			this.AddBossRewardToAttacker(player, isPartyDamage, (double)totalDamageDealtPercent, 1261);
			return;
		}
		else
		{
			if (totalDamageDealtPercent >= 50)
			{
				this.AddBossRewardToAttacker(player, isPartyDamage, (double)totalDamageDealtPercent, 1260);
				return;
			}
			if (totalDamageDealtPercent >= 25)
			{
				this.AddBossRewardToAttacker(player, isPartyDamage, (double)totalDamageDealtPercent, 1259);
				return;
			}
			if (totalDamageDealtPercent >= 1)
			{
				this.AddBossRewardToAttacker(player, isPartyDamage, (double)totalDamageDealtPercent, 1258);
				return;
			}
			return;
		}
	}

	// Token: 0x060011FB RID: 4603 RVA: 0x00056090 File Offset: 0x00054290
	private void ReducesKarmaPointsFromPlayerKiller(GameObject player, int totalExperienceReward)
	{
		if (this.IsBoss)
		{
			return;
		}
		AreaModule areaModule;
		if (!player.TryGetComponent<AreaModule>(out areaModule))
		{
			return;
		}
		if (areaModule.AreaType == AreaType.ProtectedArea | areaModule.AreaType == AreaType.EventArea)
		{
			return;
		}
		PvpModule pvpModule;
		if (!player.TryGetComponent<PvpModule>(out pvpModule))
		{
			return;
		}
		if (pvpModule.HasPlayerKillerLimitations())
		{
			int num = pvpModule.KarmaPoints;
			num -= totalExperienceReward;
			pvpModule.SetKarmaPoints(num);
		}
	}

	// Token: 0x060011FC RID: 4604 RVA: 0x000560EC File Offset: 0x000542EC
	private void UpdatePlayerQuests(GameObject player, int partyHighestMemberLevel)
	{
		AttributeModule attributeModule;
		player.TryGetComponent<AttributeModule>(out attributeModule);
		if (!GlobalUtils.CanShareRewards(partyHighestMemberLevel, attributeModule.BaseLevel, attributeModule.MasteryLevel))
		{
			return;
		}
		QuestModule questModule;
		player.TryGetComponent<QuestModule>(out questModule);
		questModule.UpdateQuest(this.MonsterId, 1, ObjectiveType.KillMonsters, this.creatureModule.Rank);
	}

	// Token: 0x060011FD RID: 4605 RVA: 0x0005613C File Offset: 0x0005433C
	private void AddExperienceToPlayer(GameObject player, int incomingExperienceReward)
	{
		AttributeModule component = player.GetComponent<AttributeModule>();
		int num = Mathf.RoundToInt((float)incomingExperienceReward * 0.5f);
		int num2 = Mathf.RoundToInt((float)incomingExperienceReward * 0.5f);
		if (component.TrainingMode == TrainingMode.AxpFocused)
		{
			num = incomingExperienceReward;
			num2 = 0;
		}
		if (component.TrainingMode == TrainingMode.ExpFocused)
		{
			num = 0;
			num2 = incomingExperienceReward;
		}
		int layerMask = 1 << LayerMask.NameToLayer("WorldArea");
		float num3 = (Physics2D.OverlapCircle(base.transform.position, 0.32f, layerMask).GetComponent<WorldAreaModule>().AreaType.ToLower() == "event") ? 1f : ServerSettingsManager.ExperienceModifier;
		if (num2 > 0)
		{
			num2 = Mathf.RoundToInt((float)this.CalculateExpBoostBonus(player, num2) * num3);
			component.AddBaseExperience((long)num2, true);
		}
		if (num > 0)
		{
			num = Mathf.RoundToInt((float)this.CalculateAxpBoostBonus(player, num) * num3);
			component.AddExperienceToAllAttributes((long)num);
		}
		player.GetComponent<ChatModule>().SendSystemTranslatedMessage("experience_and_attribute_experience_gain_message", string.Empty, true, new string[]
		{
			num2.ToString(),
			num.ToString()
		});
		string text = string.Format("{0} EXP / {1} AXP", num2, num);
		player.GetComponent<EffectModule>().ShowAnimatedText(text, 0, true, Vector3.zero);
	}

	// Token: 0x060011FE RID: 4606 RVA: 0x00056273 File Offset: 0x00054473
	private float CalculateBaseLevelBonus(float experienceReward, AttributeModule playerAttributeModule)
	{
		experienceReward += experienceReward * playerAttributeModule.ActiveExperienceBonus.Bonus;
		return experienceReward;
	}

	// Token: 0x060011FF RID: 4607 RVA: 0x00056287 File Offset: 0x00054487
	private float CalculateRankBonus(float experienceReward)
	{
		if (this.IsBoss)
		{
			return experienceReward;
		}
		experienceReward += (float)Mathf.CeilToInt(experienceReward * GlobalUtils.RankToPercentExperienceBonus(this.creatureModule.Rank));
		return experienceReward;
	}

	// Token: 0x06001200 RID: 4608 RVA: 0x000562B0 File Offset: 0x000544B0
	private int CalculateExpBoostBonus(GameObject player, int experienceReward)
	{
		PlayerModule playerModule;
		player.TryGetComponent<PlayerModule>(out playerModule);
		ConditionModule conditionModule;
		player.TryGetComponent<ConditionModule>(out conditionModule);
		IEnumerable<Condition> conditions = conditionModule.GetConditions(ConditionType.ExpBonus);
		float num = 0f;
		foreach (Condition condition in conditions)
		{
			if (condition.IsDefined)
			{
				float num2 = Mathf.Min(condition.Power, 5f);
				num2 = Mathf.Max(0f, num2);
				float b = (playerModule.PremiumDays > 0) ? 0.25f : 0.5f;
				num2 = Mathf.Min(num2, b);
				num += num2;
			}
		}
		if (num != 0f)
		{
			experienceReward += Mathf.Max(1, Mathf.RoundToInt((float)experienceReward * num));
		}
		if (playerModule.PremiumDays > 0)
		{
			experienceReward += Mathf.Max(1, Mathf.RoundToInt((float)experienceReward * 0.5f));
		}
		PvpModule pvpModule;
		player.TryGetComponent<PvpModule>(out pvpModule);
		return experienceReward;
	}

	// Token: 0x06001201 RID: 4609 RVA: 0x000563AC File Offset: 0x000545AC
	private int CalculateAxpBoostBonus(GameObject player, int experienceReward)
	{
		ConditionModule conditionModule;
		player.TryGetComponent<ConditionModule>(out conditionModule);
		IEnumerable<Condition> conditions = conditionModule.GetConditions(ConditionType.AxpBonus);
		float num = 0f;
		foreach (Condition condition in conditions)
		{
			if (condition.IsDefined)
			{
				float num2 = Mathf.Min(condition.Power, 5f);
				num2 = Mathf.Max(0f, num2);
				num += num2;
			}
		}
		if (num != 0f)
		{
			experienceReward += Mathf.Max(1, Mathf.RoundToInt((float)experienceReward * num));
		}
		PlayerModule playerModule;
		player.TryGetComponent<PlayerModule>(out playerModule);
		if (playerModule.PremiumDays > 0)
		{
			experienceReward += Mathf.Max(1, Mathf.RoundToInt((float)experienceReward * 0.5f));
		}
		return experienceReward;
	}

	// Token: 0x06001202 RID: 4610 RVA: 0x00056478 File Offset: 0x00054678
	private long CalculatePartyBonusOrPenalty(AttributeModule playerAttributeModule, int partySize, int distinctPartySize, int partyHighestMemberLevel, float experience)
	{
		if (partySize == 1)
		{
			return (long)Mathf.RoundToInt(experience);
		}
		if (!GlobalUtils.CanShareRewards(partyHighestMemberLevel, playerAttributeModule.BaseLevel, playerAttributeModule.MasteryLevel))
		{
			return 0L;
		}
		float num = (float)distinctPartySize * 0.05f;
		experience += experience * num;
		return (long)Mathf.RoundToInt(experience);
	}

	// Token: 0x06001203 RID: 4611 RVA: 0x000564C4 File Offset: 0x000546C4
	private void AddBossRewardToAttacker(GameObject attacker, bool isPartyDamage, double damageDealtPercent, int rewardItemId)
	{
		Item item = this.itemDatabaseModule.GetItem(rewardItemId);
		item.Amount = 1;
		if (item.IsDefined)
		{
			InventoryModule inventoryModule;
			attacker.TryGetComponent<InventoryModule>(out inventoryModule);
			inventoryModule.AddItem(item, true);
			EffectModule effectModule;
			attacker.TryGetComponent<EffectModule>(out effectModule);
			effectModule.ShowScreenMessage(isPartyDamage ? "boss_party_reward_message" : "boss_reward_message", 6, 3.5f, new string[]
			{
				Mathf.Min((float)damageDealtPercent, 100f).ToString(),
				this.creatureModule.CreatureName,
				item.Name
			});
		}
	}

	// Token: 0x06001204 RID: 4612 RVA: 0x0005655C File Offset: 0x0005475C
	public void LoadMonsterData()
	{
		Monster monster = this.monsterDatabaseModule.GetMonster(this.MonsterId);
		if (monster.IsDefined)
		{
			this.skillConfigs = monster.SkillConfigs;
			this.respawnMessage = monster.RespawnMessage;
			this.creatureModule.SetCreatureName(monster.Name);
			this.creatureModule.SetOriginalCreatureName(monster.Name);
			this.creatureModule.SetGender(CreatureGender.Male);
			this.movementModule.SetMovable(monster.CanMove);
			this.movementModule.SetDirection(Direction.South);
			this.creatureModule.SetAllowRankNamePrefix(monster.AllowRankNamePrefix);
			this.nonPlayerAttributeModule.SetBaseLevel(monster.BaseLevel);
			this.nonPlayerAttributeModule.InitializeAttributes(new global::Attribute[]
			{
				new global::Attribute(monster.PowerLevel, AttributeType.Power),
				new global::Attribute(monster.AgilityLevel, AttributeType.Agility),
				new global::Attribute(monster.PrecisionLevel, AttributeType.Precision),
				new global::Attribute(monster.ToughnessLevel, AttributeType.Toughness),
				new global::Attribute(monster.VitalityLevel, AttributeType.Vitality)
			});
			this.Attack = monster.Attack;
			this.Defense = monster.Defense;
			if (this.creatureModule.Rank != Rank.Normal)
			{
				this.hasFixedRank = true;
			}
			this.IsBoss = monster.IsBoss;
			this.bossEnergyItemId = monster.BossEnergyItemId;
			this.bossEnergyDropChance = monster.BossEnergyDropChance;
			this.experience = monster.Experience;
			this.SpawnDelay = monster.SpawnDelay;
			if (!this.IsBoss)
			{
				float num = monster.SpawnDelay * GlobalUtils.RankToRespawnPercentDelay(this.creatureModule.Rank);
				if (num > 0f)
				{
					this.SpawnDelay += num;
				}
			}
			this.aggroRange = monster.AggroRange;
			this.combatModule.NetworkIsPreemptive = monster.IsPreemptive;
			this.IsPassive = monster.IsPassive;
			this.spawnRange = monster.SpawnRange;
			this.findTargetInterval = monster.FindTargetInterval;
			this.changeTargetChance = monster.ChangeTargetChance;
			this.changeTargetInterval = 5f;
			this.nonPlayerEquipmentModule.EquipSkin(monster.SkinMetaName);
		}
		this.nonPlayerAttributeModule.ResetHealth();
		this.creatureModule.SetAlive(true);
		if (!string.IsNullOrEmpty(this.respawnMessage))
		{
			this.gameEnvironmentModule.BroadcastChatMessage(null, this.respawnMessage);
		}
		if (this.skillConfigs.Count != 0)
		{
			this.skillTimer.Clear();
			foreach (SkillConfig skillConfig in this.skillConfigs)
			{
				this.skillTimer.Add(skillConfig.SkillId, Time.time);
			}
			base.InvokeRepeating("CastSkillController", 0f, this.skillConfigs.Min((SkillConfig sc) => sc.CastInterval));
		}
	}

	// Token: 0x06001205 RID: 4613 RVA: 0x00056864 File Offset: 0x00054A64
	private void HandleSelectionObject(bool isSelected)
	{
		if (!this.uiSystemModule.ShowAllNames)
		{
			if (this.selectionObject.activeInHierarchy)
			{
				this.selectionObject.SetActive(false);
			}
			return;
		}
		if (isSelected && !this.selectionObject.activeInHierarchy)
		{
			this.selectionObject.SetActive(true);
			return;
		}
		if (!isSelected && this.selectionObject.activeInHierarchy)
		{
			this.selectionObject.SetActive(false);
			return;
		}
	}

	// Token: 0x06001206 RID: 4614 RVA: 0x000568D4 File Offset: 0x00054AD4
	private void HandleVisibilityHud()
	{
		if (this.uiSystemModule == null)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
			this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
		}
		bool flag = false;
		if (this.uiSystemModule.CombatModule != null && this.uiSystemModule.CombatModule.Target != null)
		{
			NetworkIdentity component = this.uiSystemModule.CombatModule.Target.GetComponent<NetworkIdentity>();
			if (component != null)
			{
				flag = (component.netId == this.combatModule.netId);
			}
		}
		if ((flag | this.uiSystemModule.ShowAllNames) && !this.conditionModule.IsInvisible && this.creatureModule.IsAlive)
		{
			this.HandleSelectionObject(flag);
			if (this.combatModule.IsPreemptive)
			{
				this.nameText.color = Color.red;
			}
			else
			{
				this.nameText.color = Color.white;
			}
			float num = (float)this.nonPlayerAttributeModule.CurrentHealth * 100f / (float)this.nonPlayerAttributeModule.MaxHealth;
			this.healthBar.fillAmount = num / 100f;
			this.localHudLevelText.text = this.nonPlayerAttributeModule.BaseLevel.ToString();
			string text = LanguageManager.Instance.GetText(GlobalUtils.RankToString(this.creatureModule.Rank));
			string text2 = LanguageManager.Instance.GetText(this.creatureModule.CreatureName);
			this.nameText.text = text2;
			if (this.creatureModule.AllowRankNamePrefix)
			{
				this.titleText.text = text;
				this.titleText.color = GlobalUtils.RankToColor(this.creatureModule.Rank);
			}
			if (!this.localHud.activeInHierarchy)
			{
				this.localHud.SetActive(true);
				return;
			}
		}
		else if (this.localHud.activeInHierarchy)
		{
			this.localHud.SetActive(false);
		}
	}

	// Token: 0x06001207 RID: 4615 RVA: 0x00056AC5 File Offset: 0x00054CC5
	public void SetAllowRespawn(bool allowRespawn)
	{
		this.allowRespawn = allowRespawn;
	}

	// Token: 0x06001208 RID: 4616 RVA: 0x00056ACE File Offset: 0x00054CCE
	public void SetMonsterId(int monsterId)
	{
		this.MonsterId = monsterId;
	}

	// Token: 0x06001209 RID: 4617 RVA: 0x00056AD8 File Offset: 0x00054CD8
	private void CheckDistance()
	{
		if (!base.enabled)
		{
			return;
		}
		if (!this.creatureModule.IsAlive)
		{
			return;
		}
		if (!GlobalUtils.IsClose(this.movementModule.SpawnPointLocation, base.transform.position, this.spawnRange))
		{
			EffectConfig effectConfig = new EffectConfig
			{
				EffectName = "Puff",
				EffectScaleModifier = 0.5f,
				EffectSpeedModifier = 0.5f
			};
			this.effectModule.ShowEffects(effectConfig);
			this.combatModule.RemoveTarget();
			this.nonPlayerAIModule.RemoveDestination();
			this.movementModule.Teleport(this.movementModule.SpawnPointLocation, new Effect("TeleporterHit", 0.6f, 0.3f));
			this.nonPlayerAttributeModule.ResetHealth();
		}
		if (this.combatModule.Target == null && this.nonPlayerAIModule.HasDestination)
		{
			this.nonPlayerAIModule.RemoveDestination();
		}
	}

	// Token: 0x0600120A RID: 4618 RVA: 0x00056BDC File Offset: 0x00054DDC
	private void FindTargets()
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.IsPassive)
		{
			return;
		}
		if (!this.creatureModule.IsAlive)
		{
			this.combatModule.RemoveTarget();
			return;
		}
		if (this.combatModule.IsPreemptive)
		{
			if (this.combatModule.Target != null)
			{
				ConditionModule conditionModule;
				this.combatModule.Target.TryGetComponent<ConditionModule>(out conditionModule);
				CreatureModule creatureModule;
				this.combatModule.Target.TryGetComponent<CreatureModule>(out creatureModule);
				int isAlive = creatureModule.IsAlive ? 1 : 0;
				bool flag = conditionModule.HasActiveCondition(ConditionCategory.Invisibility);
				bool flag2 = !GlobalUtils.IsClose(base.transform.position, this.combatModule.Target.transform.position, this.spawnRange);
				if (isAlive == 0 || flag || flag2)
				{
					this.combatModule.RemoveTarget();
				}
			}
			bool flag3 = this.conditionModule.HasActiveCondition(ConditionCategory.Taunt);
			if (this.combatModule.Target == null)
			{
				if (flag3)
				{
					return;
				}
				GameObject gameObject = this.combatModule.TargetFinder.GetTargets(1, this.aggroRange, true, true, true, false, base.gameObject, this.combatModule.Target).FirstOrDefault<GameObject>();
				if (gameObject != null)
				{
					this.combatModule.SetTarget(gameObject, false);
				}
				return;
			}
			else if (!flag3 & Time.time - this.lastChangeTargetTime > this.changeTargetInterval)
			{
				this.lastChangeTargetTime = Time.time;
				this.ChangeTargetRandomly();
			}
		}
	}

	// Token: 0x0600120B RID: 4619 RVA: 0x00056D5C File Offset: 0x00054F5C
	private void ChangeTargetRandomly()
	{
		this.changeTargetChance = ((this.changeTargetChance > 1f) ? (this.changeTargetChance / 100f) : this.changeTargetChance);
		if (UnityEngine.Random.Range(0f, 1f) <= this.changeTargetChance)
		{
			List<GameObject> targets = this.combatModule.TargetFinder.GetTargets(10, this.aggroRange, true, true, true, false, base.gameObject, this.combatModule.Target);
			if (targets.Count == 0)
			{
				return;
			}
			int index = UnityEngine.Random.Range(0, targets.Count);
			GameObject target = targets[index];
			this.combatModule.SetTarget(target, false);
		}
	}

	// Token: 0x0600120C RID: 4620 RVA: 0x00056E00 File Offset: 0x00055000
	private void Regeneration()
	{
		if (!base.enabled)
		{
			return;
		}
		if (!this.creatureModule.IsAlive)
		{
			return;
		}
		if (this.IsBoss)
		{
			return;
		}
		if (this.nonPlayerAttributeModule.CurrentHealth >= this.nonPlayerAttributeModule.MaxHealth)
		{
			return;
		}
		if (this.combatModule.netIdentity.observers.Count == 0 && this.combatModule.Target == null && !this.combatModule.InCombat && !this.conditionModule.HasAnyCondition)
		{
			float num = (float)this.nonPlayerAttributeModule.MaxHealth * 0.25f;
			this.nonPlayerAttributeModule.AddHealth((int)num);
			return;
		}
		int amount = Mathf.Min(7000, this.nonPlayerAttributeModule.RegenerationAmount);
		this.nonPlayerAttributeModule.AddHealth(amount);
	}

	// Token: 0x0600120D RID: 4621 RVA: 0x00056ED0 File Offset: 0x000550D0
	private void CastSkillController()
	{
		if (!base.enabled)
		{
			return;
		}
		if (!this.creatureModule.IsAlive)
		{
			return;
		}
		ConditionModule conditionModule;
		if (this.combatModule.Target == null || !this.combatModule.Target.TryGetComponent<ConditionModule>(out conditionModule))
		{
			return;
		}
		CreatureModule creatureModule;
		if (this.combatModule.Target == null || !this.combatModule.Target.TryGetComponent<CreatureModule>(out creatureModule))
		{
			return;
		}
		if (!creatureModule.IsAlive | conditionModule.HasActiveCondition(ConditionCategory.Invisibility))
		{
			return;
		}
		foreach (SkillConfig skillConfig in this.skillConfigs)
		{
			if (Time.time - this.skillTimer[skillConfig.SkillId] >= skillConfig.CastInterval)
			{
				float num = (skillConfig.CastChance > 1f) ? (skillConfig.CastChance / 100f) : skillConfig.CastChance;
				if (UnityEngine.Random.Range(0f, 1f) <= num)
				{
					this.skillTimer[skillConfig.SkillId] = Time.time;
					Skill skill = this.nonPlayerSkillModule.BuildSkill(skillConfig.SkillId, skillConfig.SkillPower);
					this.nonPlayerSkillModule.Cast(skill);
				}
			}
		}
	}

	// Token: 0x040010DD RID: 4317
	public int MonsterId;

	// Token: 0x040010DE RID: 4318
	[SerializeField]
	private GameObject selectionObject;

	// Token: 0x040010DF RID: 4319
	[SerializeField]
	private GameObject localHud;

	// Token: 0x040010E0 RID: 4320
	[SerializeField]
	private Image healthBar;

	// Token: 0x040010E1 RID: 4321
	[SerializeField]
	private Text localHudLevelText;

	// Token: 0x040010E2 RID: 4322
	[SerializeField]
	private TextMeshPro nameText;

	// Token: 0x040010E3 RID: 4323
	[SerializeField]
	private TextMeshPro titleText;

	// Token: 0x040010E4 RID: 4324
	private int experience;

	// Token: 0x040010E5 RID: 4325
	private float aggroRange;

	// Token: 0x040010E6 RID: 4326
	private float spawnRange;

	// Token: 0x040010E7 RID: 4327
	private bool allowRespawn;

	// Token: 0x040010E8 RID: 4328
	private bool hasFixedRank;

	// Token: 0x040010E9 RID: 4329
	private string respawnMessage;

	// Token: 0x040010EA RID: 4330
	private float findTargetInterval;

	// Token: 0x040010EB RID: 4331
	private float changeTargetChance;

	// Token: 0x040010EC RID: 4332
	private float changeTargetInterval;

	// Token: 0x040010ED RID: 4333
	private float lastChangeTargetTime;

	// Token: 0x040010EE RID: 4334
	private int bossEnergyItemId;

	// Token: 0x040010EF RID: 4335
	private float bossEnergyDropChance;

	// Token: 0x040010F0 RID: 4336
	private int totalDamageReceived;

	// Token: 0x040010F1 RID: 4337
	public GameObject Summoner;

	// Token: 0x040010F2 RID: 4338
	public List<GameObject> Summons;

	// Token: 0x040010F3 RID: 4339
	private List<SkillConfig> skillConfigs;

	// Token: 0x040010F4 RID: 4340
	private Dictionary<int, float> skillTimer = new Dictionary<int, float>();

	// Token: 0x040010F5 RID: 4341
	private CombatModule combatModule;

	// Token: 0x040010F6 RID: 4342
	private EffectModule effectModule;

	// Token: 0x040010F7 RID: 4343
	private CreatureModule creatureModule;

	// Token: 0x040010F8 RID: 4344
	private MovementModule movementModule;

	// Token: 0x040010F9 RID: 4345
	private UISystemModule uiSystemModule;

	// Token: 0x040010FA RID: 4346
	private ConditionModule conditionModule;

	// Token: 0x040010FB RID: 4347
	private CircleCollider2D circleCollider2D;

	// Token: 0x040010FC RID: 4348
	private NonPlayerAIModule nonPlayerAIModule;

	// Token: 0x040010FD RID: 4349
	private ItemDatabaseModule itemDatabaseModule;

	// Token: 0x040010FE RID: 4350
	private NonPlayerSkillModule nonPlayerSkillModule;

	// Token: 0x040010FF RID: 4351
	private GameEnvironmentModule gameEnvironmentModule;

	// Token: 0x04001100 RID: 4352
	private MonsterDatabaseModule monsterDatabaseModule;

	// Token: 0x04001101 RID: 4353
	private NonPlayerAttributeModule nonPlayerAttributeModule;

	// Token: 0x04001102 RID: 4354
	private NonPlayerEquipmentModule nonPlayerEquipmentModule;
}
