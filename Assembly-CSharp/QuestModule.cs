using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

// Token: 0x020003FB RID: 1019
public class QuestModule : NetworkBehaviour
{
	// Token: 0x1400001D RID: 29
	// (add) Token: 0x060015EF RID: 5615 RVA: 0x0007004C File Offset: 0x0006E24C
	// (remove) Token: 0x060015F0 RID: 5616 RVA: 0x00070084 File Offset: 0x0006E284
	public event QuestModule.OnQuestAcceptedEventHandler OnQuestAccepted;

	// Token: 0x1400001E RID: 30
	// (add) Token: 0x060015F1 RID: 5617 RVA: 0x000700BC File Offset: 0x0006E2BC
	// (remove) Token: 0x060015F2 RID: 5618 RVA: 0x000700F4 File Offset: 0x0006E2F4
	public event QuestModule.OnQuestUpdatedEventHandler OnQuestUpdated;

	// Token: 0x1400001F RID: 31
	// (add) Token: 0x060015F3 RID: 5619 RVA: 0x0007012C File Offset: 0x0006E32C
	// (remove) Token: 0x060015F4 RID: 5620 RVA: 0x00070164 File Offset: 0x0006E364
	public event QuestModule.OnQuestAbortedEventHandler OnQuestAborted;

	// Token: 0x14000020 RID: 32
	// (add) Token: 0x060015F5 RID: 5621 RVA: 0x0007019C File Offset: 0x0006E39C
	// (remove) Token: 0x060015F6 RID: 5622 RVA: 0x000701D4 File Offset: 0x0006E3D4
	public event QuestModule.OnQuestCompletedEventHandler OnQuestCompleted;

	// Token: 0x1700024D RID: 589
	// (get) Token: 0x060015F7 RID: 5623 RVA: 0x00070209 File Offset: 0x0006E409
	public GameObject ObjectToTrack
	{
		get
		{
			return this.objectToTrack;
		}
	}

	// Token: 0x1700024E RID: 590
	// (get) Token: 0x060015F8 RID: 5624 RVA: 0x00070211 File Offset: 0x0006E411
	public Vector3 DestinationToTrack
	{
		get
		{
			return this.destinationToTrack;
		}
	}

	// Token: 0x060015F9 RID: 5625 RVA: 0x0007021C File Offset: 0x0006E41C
	private void Awake()
	{
		if (NetworkClient.active)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
			this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
		}
		this.aiModule = base.GetComponent<AIModule>();
		this.effectModule = base.GetComponent<EffectModule>();
		this.attributeModule = base.GetComponent<AttributeModule>();
		this.inventoryModule = base.GetComponent<InventoryModule>();
		if (NetworkServer.active)
		{
			this.vocationModule = base.GetComponent<VocationModule>();
			GameObject gameObject2 = GameObject.FindGameObjectWithTag("DatabaseManager");
			this.npcDatabaseModule = gameObject2.GetComponent<NpcDatabaseModule>();
			this.itemDatabaseModule = gameObject2.GetComponent<ItemDatabaseModule>();
			this.titleDatabaseModule = gameObject2.GetComponent<TitleDatabaseModule>();
			this.questDatabaseModule = gameObject2.GetComponent<QuestDatabaseModule>();
			this.monsterDatabaseModule = gameObject2.GetComponent<MonsterDatabaseModule>();
			this.pvpModule = base.GetComponent<PvpModule>();
			this.titleModule = base.GetComponent<TitleModule>();
			this.playerModule = base.GetComponent<PlayerModule>();
			this.walletModule = base.GetComponent<WalletModule>();
		}
	}

	// Token: 0x060015FA RID: 5626 RVA: 0x00070304 File Offset: 0x0006E504
	public override void OnStartLocalPlayer()
	{
		this.PlayerQuests.Callback += this.OnPlayerQuestsUpdated;
		this.RefreshPlayerQuests();
	}

	// Token: 0x060015FB RID: 5627 RVA: 0x00070323 File Offset: 0x0006E523
	public override void OnStartServer()
	{
		this.PlayerQuests.Callback += delegate(SyncList<PlayerQuest>.Operation op, int index, PlayerQuest oldQuest, PlayerQuest newQuest)
		{
			this.ShouldPersistData = true;
		};
	}

	// Token: 0x060015FC RID: 5628 RVA: 0x0007033C File Offset: 0x0006E53C
	private void OnPlayerQuestsUpdated(SyncList<PlayerQuest>.Operation op, int itemIndex, PlayerQuest oldItem, PlayerQuest newItem)
	{
		this.RefreshPlayerQuests();
	}

	// Token: 0x060015FD RID: 5629 RVA: 0x00070344 File Offset: 0x0006E544
	[Client]
	private void RefreshPlayerQuests()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void QuestModule::RefreshPlayerQuests()' called when client was not active");
			return;
		}
		QuestWindowManager questWindowManager;
		if (this.uiSystemModule.QuestWindow.activeInHierarchy && this.uiSystemModule.QuestWindow.TryGetComponent<QuestWindowManager>(out questWindowManager))
		{
			questWindowManager.RefreshPlayerQuests();
		}
		QuestTrackerManager questTrackerManager;
		if (!GlobalSettings.IsMobilePlatform && this.uiSystemModule.QuestTracker.activeInHierarchy && this.uiSystemModule.QuestTracker.TryGetComponent<QuestTrackerManager>(out questTrackerManager))
		{
			questTrackerManager.RefreshPlayerQuests();
		}
	}

	// Token: 0x060015FE RID: 5630 RVA: 0x000703C8 File Offset: 0x0006E5C8
	[Server]
	public Task LoadPlayerQuestsAsync(int playerId)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Threading.Tasks.Task QuestModule::LoadPlayerQuestsAsync(System.Int32)' called when server was not active");
			return null;
		}
		QuestModule.<LoadPlayerQuestsAsync>d__46 <LoadPlayerQuestsAsync>d__;
		<LoadPlayerQuestsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<LoadPlayerQuestsAsync>d__.<>4__this = this;
		<LoadPlayerQuestsAsync>d__.playerId = playerId;
		<LoadPlayerQuestsAsync>d__.<>1__state = -1;
		<LoadPlayerQuestsAsync>d__.<>t__builder.Start<QuestModule.<LoadPlayerQuestsAsync>d__46>(ref <LoadPlayerQuestsAsync>d__);
		return <LoadPlayerQuestsAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060015FF RID: 5631 RVA: 0x00070434 File Offset: 0x0006E634
	[Server]
	public void AcceptQuest(int questId)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void QuestModule::AcceptQuest(System.Int32)' called when server was not active");
			return;
		}
		if (this.AcceptingQuest)
		{
			this.effectModule.ShowScreenMessage("already_accepting_quest_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		Quest quest = this.questDatabaseModule.GetQuest(questId);
		if (!quest.IsDefined)
		{
			return;
		}
		if (this.HasQuest(quest.Id))
		{
			return;
		}
		this.AcceptingQuest = true;
		QuestModule.OnQuestAcceptedEventHandler onQuestAccepted = this.OnQuestAccepted;
		if (onQuestAccepted != null)
		{
			onQuestAccepted(quest);
		}
		this.AcceptingQuest = false;
	}

	// Token: 0x06001600 RID: 5632 RVA: 0x000704C0 File Offset: 0x0006E6C0
	private PlayerQuest BuildNewPlayerQuestFromQuest(Quest quest)
	{
		return new PlayerQuest(this.playerModule.PlayerId, quest.Id, quest.Name, quest.Description, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"), null, quest.RequiredLevel, true, quest.IsDailyTask)
		{
			Rewards = quest.Rewards.Select(new Func<QuestReward, PlayerQuestReward>(this.MapNewPlayerQuestRewardFromQuestReward)).ToArray<PlayerQuestReward>(),
			Objectives = quest.Objectives.Select(new Func<QuestObjective, PlayerQuestObjective>(this.MapNewPlayerQuestObjectiveFromQuestObjective)).ToArray<PlayerQuestObjective>()
		};
	}

	// Token: 0x06001601 RID: 5633 RVA: 0x0007055C File Offset: 0x0006E75C
	private PlayerQuestReward MapNewPlayerQuestRewardFromQuestReward(QuestReward reward)
	{
		return new PlayerQuestReward
		{
			Amount = reward.Amount,
			ItemId = reward.ItemId,
			ItemName = reward.ItemName,
			Rarity = reward.Rarity,
			RequiredLevel = reward.RequiredLevel,
			RewardType = reward.RewardType,
			TitleId = reward.TitleId,
			TitleName = reward.TitleName,
			Vocation = reward.Vocation
		};
	}

	// Token: 0x06001602 RID: 5634 RVA: 0x000705E8 File Offset: 0x0006E7E8
	private PlayerQuestObjective MapNewPlayerQuestObjectiveFromQuestObjective(QuestObjective objective)
	{
		return new PlayerQuestObjective
		{
			ObjectiveAmount = objective.ObjectiveAmount,
			ObjectiveId = objective.ObjectiveId,
			ObjectiveName = objective.ObjectiveName,
			ObjectivePluralName = objective.ObjectivePluralName,
			ObjectiveProgress = 0,
			ObjectiveRank = objective.ObjectiveRank,
			ObjectiveType = objective.ObjectiveType
		};
	}

	// Token: 0x06001603 RID: 5635 RVA: 0x00070654 File Offset: 0x0006E854
	[Server]
	public void AddQuestToPlayerQuests(Quest quest)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void QuestModule::AddQuestToPlayerQuests(Quest)' called when server was not active");
			return;
		}
		PlayerQuest item = this.BuildNewPlayerQuestFromQuest(quest);
		this.PlayerQuests.Add(item);
	}

	// Token: 0x06001604 RID: 5636 RVA: 0x0007068C File Offset: 0x0006E88C
	[Server]
	public void AddTaskForPlayer(Quest task)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void QuestModule::AddTaskForPlayer(Quest)' called when server was not active");
			return;
		}
		if (this.AcceptingQuest)
		{
			this.effectModule.ShowScreenMessage("already_accepting_quest_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		this.AcceptingQuest = true;
		QuestModule.OnQuestAcceptedEventHandler onQuestAccepted = this.OnQuestAccepted;
		if (onQuestAccepted != null)
		{
			onQuestAccepted(task);
		}
		this.AcceptingQuest = false;
	}

	// Token: 0x06001605 RID: 5637 RVA: 0x000706F4 File Offset: 0x0006E8F4
	public bool HasQuest(int questId)
	{
		for (int i = 0; i < this.PlayerQuests.Count; i++)
		{
			if (this.PlayerQuests[i].QuestId == questId)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001606 RID: 5638 RVA: 0x00070730 File Offset: 0x0006E930
	public bool HasQuestInProgress(int questId)
	{
		for (int i = 0; i < this.PlayerQuests.Count; i++)
		{
			if (this.PlayerQuests[i].QuestId == questId & !this.PlayerQuests[i].IsCompleted)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001607 RID: 5639 RVA: 0x00070784 File Offset: 0x0006E984
	public bool HasQuestCompleted(int questId)
	{
		for (int i = 0; i < this.PlayerQuests.Count; i++)
		{
			if (this.PlayerQuests[i].QuestId == questId & this.PlayerQuests[i].IsCompleted)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001608 RID: 5640 RVA: 0x000707D8 File Offset: 0x0006E9D8
	public PlayerQuest GetPlayerQuest(int questId)
	{
		for (int i = 0; i < this.PlayerQuests.Count; i++)
		{
			if (this.PlayerQuests[i].QuestId == questId)
			{
				return this.PlayerQuests[i];
			}
		}
		return default(PlayerQuest);
	}

	// Token: 0x06001609 RID: 5641 RVA: 0x00070828 File Offset: 0x0006EA28
	[Command]
	public void CmdAbortQuest(int questId)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(questId);
		base.SendCommandInternal("System.Void QuestModule::CmdAbortQuest(System.Int32)", 770919412, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600160A RID: 5642 RVA: 0x00070864 File Offset: 0x0006EA64
	[Server]
	public void AbortQuest(int questId)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void QuestModule::AbortQuest(System.Int32)' called when server was not active");
			return;
		}
		int i = 0;
		while (i < this.PlayerQuests.Count)
		{
			PlayerQuest playerQuest = this.PlayerQuests[i];
			if (playerQuest.QuestId == questId & playerQuest.PlayerId == this.playerModule.PlayerId)
			{
				this.PlayerQuests.RemoveAt(i);
				QuestModule.OnQuestAbortedEventHandler onQuestAborted = this.OnQuestAborted;
				if (onQuestAborted == null)
				{
					return;
				}
				onQuestAborted(playerQuest);
				return;
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x0600160B RID: 5643 RVA: 0x000708E8 File Offset: 0x0006EAE8
	[Command]
	public void CmdSetShowOverlay(int playerId, int questId, bool showOverlay)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(playerId);
		writer.WriteInt(questId);
		writer.WriteBool(showOverlay);
		base.SendCommandInternal("System.Void QuestModule::CmdSetShowOverlay(System.Int32,System.Int32,System.Boolean)", -1274679751, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600160C RID: 5644 RVA: 0x00070936 File Offset: 0x0006EB36
	public void CompleteQuest(int playerId, int questId)
	{
		if (base.isLocalPlayer)
		{
			this.CmdCompleteQuest(playerId, questId);
			return;
		}
		if (base.isServer)
		{
			this.DoCompleteQuest(playerId, questId);
		}
	}

	// Token: 0x0600160D RID: 5645 RVA: 0x0007095C File Offset: 0x0006EB5C
	[Command]
	private void CmdCompleteQuest(int playerId, int questId)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(playerId);
		writer.WriteInt(questId);
		base.SendCommandInternal("System.Void QuestModule::CmdCompleteQuest(System.Int32,System.Int32)", 422103948, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600160E RID: 5646 RVA: 0x000709A0 File Offset: 0x0006EBA0
	[Server]
	public void DoCompleteQuest(int playerId, int questId)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void QuestModule::DoCompleteQuest(System.Int32,System.Int32)' called when server was not active");
			return;
		}
		for (int i = 0; i < this.PlayerQuests.Count; i++)
		{
			PlayerQuest playerQuest = this.PlayerQuests[i];
			if (playerQuest.QuestId == questId & playerQuest.PlayerId == playerId)
			{
				if (playerQuest.IsCompleted)
				{
					this.effectModule.ShowScreenMessage("quest_already_completed_message", 3, 3.5f, Array.Empty<string>());
				}
				else
				{
					int emptySlots = this.inventoryModule.EmptySlots;
					PlayerQuestReward[] array = (from w in playerQuest.Rewards
					where w.RewardType == RewardType.Item & (w.Vocation == Vocation.Undefined | w.Vocation == this.vocationModule.Vocation)
					select w).ToArray<PlayerQuestReward>();
					if (emptySlots < array.Length)
					{
						this.effectModule.ShowScreenMessage("cant_complete_quest_inventory_full_message", 3, 3.5f, Array.Empty<string>());
					}
					else if (this.CanCompleteQuest(questId))
					{
						foreach (PlayerQuestObjective playerQuestObjective in (from w in playerQuest.Objectives
						where w.ObjectiveType == ObjectiveType.CollectItems
						select w).ToArray<PlayerQuestObjective>())
						{
							this.inventoryModule.ConsumeItem(playerQuestObjective.ObjectiveId, playerQuestObjective.ObjectiveAmount);
						}
						playerQuest.Completed = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
						this.PlayerQuests.RemoveAt(i);
						this.PlayerQuests.Add(playerQuest);
						this.AddRewardsToPlayer(playerQuest);
						QuestModule.OnQuestCompletedEventHandler onQuestCompleted = this.OnQuestCompleted;
						if (onQuestCompleted != null)
						{
							onQuestCompleted(playerQuest);
						}
					}
					else
					{
						this.effectModule.ShowScreenMessage("quest_finish_objectives_message", 3, 3.5f, Array.Empty<string>());
					}
				}
			}
		}
	}

	// Token: 0x0600160F RID: 5647 RVA: 0x00070B54 File Offset: 0x0006ED54
	private void AddRewardsToPlayer(PlayerQuest playerQuest)
	{
		QuestModule.<AddRewardsToPlayer>d__63 <AddRewardsToPlayer>d__;
		<AddRewardsToPlayer>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<AddRewardsToPlayer>d__.<>4__this = this;
		<AddRewardsToPlayer>d__.playerQuest = playerQuest;
		<AddRewardsToPlayer>d__.<>1__state = -1;
		<AddRewardsToPlayer>d__.<>t__builder.Start<QuestModule.<AddRewardsToPlayer>d__63>(ref <AddRewardsToPlayer>d__);
	}

	// Token: 0x06001610 RID: 5648 RVA: 0x00070B94 File Offset: 0x0006ED94
	public bool IsQuestCompleted(int questId)
	{
		PlayerQuest playerQuest = this.PlayerQuests.FirstOrDefault((PlayerQuest f) => f.QuestId == questId);
		return playerQuest.IsDefined && playerQuest.IsCompleted;
	}

	// Token: 0x06001611 RID: 5649 RVA: 0x00070BD8 File Offset: 0x0006EDD8
	public bool CanCompleteQuest(int questId)
	{
		PlayerQuest playerQuest = this.PlayerQuests.FirstOrDefault((PlayerQuest f) => f.QuestId == questId);
		if (!playerQuest.IsDefined)
		{
			return false;
		}
		if (playerQuest.IsCompleted)
		{
			return false;
		}
		bool result = true;
		for (int i = 0; i < playerQuest.Objectives.Length; i++)
		{
			PlayerQuestObjective playerQuestObjective = playerQuest.Objectives[i];
			int num = playerQuestObjective.ObjectiveProgress;
			if (playerQuestObjective.ObjectiveType == ObjectiveType.CollectItems)
			{
				num = this.inventoryModule.GetAmount(playerQuestObjective.ObjectiveId);
			}
			if (num < playerQuestObjective.ObjectiveAmount)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	// Token: 0x06001612 RID: 5650 RVA: 0x00070C7C File Offset: 0x0006EE7C
	[Server]
	public void UpdateQuest(int objectiveId, int updateAmount, ObjectiveType objectiveType, Rank objectiveRank)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void QuestModule::UpdateQuest(System.Int32,System.Int32,ObjectiveType,Rank)' called when server was not active");
			return;
		}
		for (int i = this.PlayerQuests.Count - 1; i >= 0; i--)
		{
			PlayerQuest playerQuest = this.PlayerQuests[i];
			if (!playerQuest.IsCompleted)
			{
				switch (objectiveType)
				{
				case ObjectiveType.CollectItems:
					for (int j = 0; j < playerQuest.Objectives.Length; j++)
					{
						PlayerQuestObjective playerQuestObjective = playerQuest.Objectives[j];
						if (playerQuestObjective.ObjectiveType == objectiveType & playerQuestObjective.ObjectiveId == objectiveId)
						{
							int amount = this.inventoryModule.GetAmount(objectiveId);
							if (amount < playerQuestObjective.ObjectiveAmount | playerQuestObjective.ObjectiveProgress < playerQuestObjective.ObjectiveAmount)
							{
								playerQuestObjective.ObjectiveProgress = Math.Min(playerQuestObjective.ObjectiveAmount, amount);
								playerQuest.Objectives[j] = playerQuestObjective;
								this.PlayerQuests.RemoveAt(i);
								this.PlayerQuests.Add(playerQuest);
								QuestModule.OnQuestUpdatedEventHandler onQuestUpdated = this.OnQuestUpdated;
								if (onQuestUpdated != null)
								{
									onQuestUpdated(playerQuest, playerQuestObjective, updateAmount, objectiveType);
								}
							}
						}
					}
					break;
				case ObjectiveType.KillMonsters:
					for (int k = 0; k < playerQuest.Objectives.Length; k++)
					{
						PlayerQuestObjective playerQuestObjective2 = playerQuest.Objectives[k];
						bool flag = playerQuestObjective2.ObjectiveRank == Rank.None || playerQuestObjective2.ObjectiveRank == objectiveRank;
						bool flag2 = playerQuestObjective2.ObjectiveType == objectiveType;
						bool flag3 = playerQuestObjective2.ObjectiveId == objectiveId;
						bool flag4 = playerQuestObjective2.ObjectiveProgress < playerQuestObjective2.ObjectiveAmount;
						if (flag2 && flag3 && flag4 && flag)
						{
							playerQuestObjective2.ObjectiveProgress += updateAmount;
							playerQuestObjective2.ObjectiveProgress = Math.Min(playerQuestObjective2.ObjectiveAmount, playerQuestObjective2.ObjectiveProgress);
							playerQuest.Objectives[k] = playerQuestObjective2;
							this.PlayerQuests.RemoveAt(i);
							this.PlayerQuests.Add(playerQuest);
							QuestModule.OnQuestUpdatedEventHandler onQuestUpdated2 = this.OnQuestUpdated;
							if (onQuestUpdated2 != null)
							{
								onQuestUpdated2(playerQuest, playerQuestObjective2, updateAmount, objectiveType);
							}
						}
					}
					break;
				case ObjectiveType.TalkNpc:
					for (int l = 0; l < playerQuest.Objectives.Length; l++)
					{
						PlayerQuestObjective playerQuestObjective3 = playerQuest.Objectives[l];
						bool flag5 = playerQuestObjective3.ObjectiveType == objectiveType;
						bool flag6 = playerQuestObjective3.ObjectiveId == objectiveId;
						bool flag7 = playerQuestObjective3.ObjectiveProgress < playerQuestObjective3.ObjectiveAmount;
						if (flag5 && flag6 && flag7)
						{
							playerQuestObjective3.ObjectiveProgress += updateAmount;
							playerQuestObjective3.ObjectiveProgress = Math.Min(playerQuestObjective3.ObjectiveAmount, playerQuestObjective3.ObjectiveProgress);
							playerQuest.Objectives[l] = playerQuestObjective3;
							this.PlayerQuests.RemoveAt(i);
							this.PlayerQuests.Add(playerQuest);
							QuestModule.OnQuestUpdatedEventHandler onQuestUpdated3 = this.OnQuestUpdated;
							if (onQuestUpdated3 != null)
							{
								onQuestUpdated3(playerQuest, playerQuestObjective3, updateAmount, objectiveType);
							}
						}
					}
					break;
				}
			}
		}
	}

	// Token: 0x06001613 RID: 5651 RVA: 0x00070F44 File Offset: 0x0006F144
	[Command]
	public void CmdFindNewQuest(bool alertNoQuests)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteBool(alertNoQuests);
		base.SendCommandInternal("System.Void QuestModule::CmdFindNewQuest(System.Boolean)", 1162758595, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001614 RID: 5652 RVA: 0x00070F80 File Offset: 0x0006F180
	[Server]
	public void FindNewQuest(bool alertNoQuests)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void QuestModule::FindNewQuest(System.Boolean)' called when server was not active");
			return;
		}
		Quest[] availableQuests = this.GetAvailableQuests();
		int totalAvailableQuests = availableQuests.Length;
		Quest quest = availableQuests.FirstOrDefault<Quest>();
		if (!this.ValidateQuestFound(this.effectModule, quest))
		{
			return;
		}
		if (!GameEnvironmentModule.NpcSpawnPoints.Any((NpcSpawnPoint a) => a.NpcId == quest.NpcId))
		{
			return;
		}
		NpcSpawnPoint npcSpawnPoint = (from n in GameEnvironmentModule.NpcSpawnPoints
		orderby Vector3.Distance(this.transform.position, n.SpawnPointPosition)
		select n).FirstOrDefault((NpcSpawnPoint n) => n.NpcId == quest.NpcId);
		Vector3 spawnPointPosition = npcSpawnPoint.SpawnPointPosition;
		Collider2D collider2D = Physics2D.OverlapPoint(spawnPointPosition, 1 << LayerMask.NameToLayer("WorldArea"));
		string worldAreaName = string.Empty;
		if (collider2D != null)
		{
			worldAreaName = collider2D.GetComponent<WorldAreaModule>().AreaName;
		}
		if (npcSpawnPoint.NpcObject.GetComponent<NetworkIdentity>().isServer)
		{
			this.TargetShowFindNewQuestsResult(npcSpawnPoint.NpcObject, spawnPointPosition, npcSpawnPoint.NpcName, totalAvailableQuests, worldAreaName, alertNoQuests);
			return;
		}
		this.TargetShowFindNewQuestsResult(null, spawnPointPosition, npcSpawnPoint.NpcName, totalAvailableQuests, worldAreaName, alertNoQuests);
	}

	// Token: 0x06001615 RID: 5653 RVA: 0x000710A0 File Offset: 0x0006F2A0
	[TargetRpc]
	private void TargetShowFindNewQuestsResult(GameObject npcObject, Vector3 destinationToTrack, string npcName, int totalAvailableQuests, string worldAreaName, bool alertNoQuests)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteGameObject(npcObject);
		writer.WriteVector3(destinationToTrack);
		writer.WriteString(npcName);
		writer.WriteInt(totalAvailableQuests);
		writer.WriteString(worldAreaName);
		writer.WriteBool(alertNoQuests);
		this.SendTargetRPCInternal(null, "System.Void QuestModule::TargetShowFindNewQuestsResult(UnityEngine.GameObject,UnityEngine.Vector3,System.String,System.Int32,System.String,System.Boolean)", 1713746703, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001616 RID: 5654 RVA: 0x0007110C File Offset: 0x0006F30C
	[Client]
	private bool ShowSuccessAvailableQuestsMessage(int totalAvailableQuests, string npcName, string worldAreaName, bool alertNoQuests)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Boolean QuestModule::ShowSuccessAvailableQuestsMessage(System.Int32,System.String,System.String,System.Boolean)' called when client was not active");
			return default(bool);
		}
		if (totalAvailableQuests == 0)
		{
			if (alertNoQuests)
			{
				this.effectModule.ShowScreenMessage("quest_finder_no_quest_available_message", 0, 3.5f, Array.Empty<string>());
			}
			return false;
		}
		this.effectModule.ShowScreenMessage("quest_finder_quest_available_message", 0, 3.5f, new string[]
		{
			npcName,
			worldAreaName
		});
		return true;
	}

	// Token: 0x06001617 RID: 5655 RVA: 0x00071183 File Offset: 0x0006F383
	[Client]
	private void ShowNoPathAvailableQuestsMessage(string npcName)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void QuestModule::ShowNoPathAvailableQuestsMessage(System.String)' called when client was not active");
			return;
		}
		this.effectModule.ShowScreenMessage("quest_finder_single_quest_no_path_available_message", 0, 7f, new string[]
		{
			npcName
		});
	}

	// Token: 0x06001618 RID: 5656 RVA: 0x000711BC File Offset: 0x0006F3BC
	[Command]
	public void CmdTrackQuestObjectives(int questId)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(questId);
		base.SendCommandInternal("System.Void QuestModule::CmdTrackQuestObjectives(System.Int32)", 2016985651, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001619 RID: 5657 RVA: 0x000711F8 File Offset: 0x0006F3F8
	[Server]
	public void TrackQuestObjectives(int questId)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void QuestModule::TrackQuestObjectives(System.Int32)' called when server was not active");
			return;
		}
		int i;
		int num;
		for (i = 0; i < this.PlayerQuests.Count; i = num + 1)
		{
			if (this.PlayerQuests[i].QuestId == questId)
			{
				if (this.PlayerQuests[i].IsCompleted)
				{
					break;
				}
				bool flag = false;
				int j;
				for (j = 0; j < this.PlayerQuests[i].Objectives.Length; j = num + 1)
				{
					int objectiveProgress = this.PlayerQuests[i].Objectives[j].ObjectiveProgress;
					if (this.PlayerQuests[i].Objectives[j].ObjectiveType != ObjectiveType.CollectItems && objectiveProgress < this.PlayerQuests[i].Objectives[j].ObjectiveAmount)
					{
						if (this.PlayerQuests[i].Objectives[j].ObjectiveType == ObjectiveType.TalkNpc)
						{
							NpcSpawnPoint npcSpawnPoint = (from n in GameEnvironmentModule.NpcSpawnPoints
							orderby Vector3.Distance(base.transform.position, n.SpawnPointPosition)
							select n).FirstOrDefault((NpcSpawnPoint n) => n.NpcId == this.PlayerQuests[i].Objectives[j].ObjectiveId);
							if (npcSpawnPoint != null)
							{
								flag = true;
								if (npcSpawnPoint.NpcObject.GetComponent<NetworkIdentity>().isServer)
								{
									this.TargetShowTrackQuestObjectivesResult(npcSpawnPoint.NpcObject, npcSpawnPoint.SpawnPointPosition);
									break;
								}
								this.TargetShowTrackQuestObjectivesResult(null, npcSpawnPoint.SpawnPointPosition);
								break;
							}
						}
						else if (this.PlayerQuests[i].Objectives[j].ObjectiveType == ObjectiveType.KillMonsters)
						{
							Rank objectiveRank = this.PlayerQuests[i].Objectives[j].ObjectiveRank;
							IEnumerable<MonsterSpawnPoint> source = from m in GameEnvironmentModule.MonsterSpawnPoints
							where m.MonsterId == this.PlayerQuests[i].Objectives[j].ObjectiveId
							select m;
							if (objectiveRank != Rank.None)
							{
								source = from m in source
								where m.MonsterRank == objectiveRank
								select m;
							}
							source = from m in source
							where m.MonsterObject.GetComponent<CreatureModule>().IsAlive
							select m;
							MonsterSpawnPoint monsterSpawnPoint = (from m in source
							orderby Vector3.Distance(base.transform.position, m.SpawnPointPosition), m.MonsterObject.GetComponent<NetworkIdentity>().isServer
							select m).FirstOrDefault<MonsterSpawnPoint>();
							if (monsterSpawnPoint != null)
							{
								flag = true;
								if (monsterSpawnPoint.MonsterObject.GetComponent<NetworkIdentity>().isServer)
								{
									this.TargetShowTrackQuestObjectivesResult(monsterSpawnPoint.MonsterObject, monsterSpawnPoint.SpawnPointPosition);
									break;
								}
								this.TargetShowTrackQuestObjectivesResult(null, monsterSpawnPoint.SpawnPointPosition);
								break;
							}
						}
					}
					num = j;
				}
				if (!flag)
				{
					this.effectModule.ShowScreenMessage("quest_finder_no_trackable_objective_message", 3, 3.5f, Array.Empty<string>());
				}
			}
			num = i;
		}
	}

	// Token: 0x0600161A RID: 5658 RVA: 0x0007157C File Offset: 0x0006F77C
	[Server]
	private bool ValidateQuestFound(EffectModule effectModule, Quest quest)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean QuestModule::ValidateQuestFound(EffectModule,Quest)' called when server was not active");
			return default(bool);
		}
		if (!quest.IsDefined)
		{
			effectModule.ShowScreenMessage("quest_finder_no_quest_available_message", 0, 3.5f, Array.Empty<string>());
			return false;
		}
		return true;
	}

	// Token: 0x0600161B RID: 5659 RVA: 0x000715CC File Offset: 0x0006F7CC
	[Server]
	private Quest[] GetAvailableQuests()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'Quest[] QuestModule::GetAvailableQuests()' called when server was not active");
			return default(Quest[]);
		}
		IEnumerable<int> playerQuestIds = from s in this.PlayerQuests
		select s.QuestId;
		return (from q in this.questDatabaseModule.GetQuests(this.attributeModule.BaseLevel, false)
		where !playerQuestIds.Contains(q.Id)
		select q).OrderBy(delegate(Quest q)
		{
			NpcSpawnPoint npcSpawnPoint = GameEnvironmentModule.NpcSpawnPoints.FirstOrDefault((NpcSpawnPoint a) => a.NpcId == q.NpcId);
			if (npcSpawnPoint == null)
			{
				return float.MaxValue;
			}
			return Vector3.Distance(this.transform.position, npcSpawnPoint.SpawnPointPosition);
		}).ToArray<Quest>();
	}

	// Token: 0x0600161C RID: 5660 RVA: 0x00071678 File Offset: 0x0006F878
	[TargetRpc]
	public void TargetShowTrackQuestObjectivesResult(GameObject objectToTrack, Vector3 destinationToTrack)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteGameObject(objectToTrack);
		writer.WriteVector3(destinationToTrack);
		this.SendTargetRPCInternal(null, "System.Void QuestModule::TargetShowTrackQuestObjectivesResult(UnityEngine.GameObject,UnityEngine.Vector3)", 1300790066, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600161D RID: 5661 RVA: 0x000716BC File Offset: 0x0006F8BC
	[TargetRpc]
	public void TargetRemoveDestinationToTrack()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		this.SendTargetRPCInternal(null, "System.Void QuestModule::TargetRemoveDestinationToTrack()", 1344539458, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600161E RID: 5662 RVA: 0x000716EC File Offset: 0x0006F8EC
	[Client]
	public void RemoveDestitionToTrack()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void QuestModule::RemoveDestitionToTrack()' called when client was not active");
			return;
		}
		this.objectToTrack = null;
		this.destinationToTrack = Vector3.zero;
	}

	// Token: 0x0600161F RID: 5663 RVA: 0x00071718 File Offset: 0x0006F918
	[Server]
	public void AbortAllDailyTasks(int playerId)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void QuestModule::AbortAllDailyTasks(System.Int32)' called when server was not active");
			return;
		}
		for (int i = 0; i < this.PlayerQuests.Count; i++)
		{
			PlayerQuest playerQuest = this.PlayerQuests[i];
			if (playerQuest.IsDailyTask & playerQuest.PlayerId == playerId)
			{
				this.PlayerQuests.RemoveAt(i);
				QuestModule.OnQuestAbortedEventHandler onQuestAborted = this.OnQuestAborted;
				if (onQuestAborted != null)
				{
					onQuestAborted(playerQuest);
				}
			}
		}
	}

	// Token: 0x06001620 RID: 5664 RVA: 0x0007178D File Offset: 0x0006F98D
	public QuestModule()
	{
		base.InitSyncObject(this.PlayerQuests);
	}

	// Token: 0x06001626 RID: 5670 RVA: 0x0002FEC9 File Offset: 0x0002E0C9
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x06001627 RID: 5671 RVA: 0x00071843 File Offset: 0x0006FA43
	protected void UserCode_CmdAbortQuest__Int32(int questId)
	{
		this.AbortQuest(questId);
	}

	// Token: 0x06001628 RID: 5672 RVA: 0x0007184C File Offset: 0x0006FA4C
	protected static void InvokeUserCode_CmdAbortQuest__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdAbortQuest called on client.");
			return;
		}
		((QuestModule)obj).UserCode_CmdAbortQuest__Int32(reader.ReadInt());
	}

	// Token: 0x06001629 RID: 5673 RVA: 0x00071878 File Offset: 0x0006FA78
	protected void UserCode_CmdSetShowOverlay__Int32__Int32__Boolean(int playerId, int questId, bool showOverlay)
	{
		QuestModule.<CmdSetShowOverlay>d__59 <CmdSetShowOverlay>d__;
		<CmdSetShowOverlay>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CmdSetShowOverlay>d__.<>4__this = this;
		<CmdSetShowOverlay>d__.playerId = playerId;
		<CmdSetShowOverlay>d__.questId = questId;
		<CmdSetShowOverlay>d__.showOverlay = showOverlay;
		<CmdSetShowOverlay>d__.<>1__state = -1;
		<CmdSetShowOverlay>d__.<>t__builder.Start<QuestModule.<CmdSetShowOverlay>d__59>(ref <CmdSetShowOverlay>d__);
	}

	// Token: 0x0600162A RID: 5674 RVA: 0x000718C7 File Offset: 0x0006FAC7
	protected static void InvokeUserCode_CmdSetShowOverlay__Int32__Int32__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSetShowOverlay called on client.");
			return;
		}
		((QuestModule)obj).UserCode_CmdSetShowOverlay__Int32__Int32__Boolean(reader.ReadInt(), reader.ReadInt(), reader.ReadBool());
	}

	// Token: 0x0600162B RID: 5675 RVA: 0x000718FC File Offset: 0x0006FAFC
	protected void UserCode_CmdCompleteQuest__Int32__Int32(int playerId, int questId)
	{
		this.DoCompleteQuest(playerId, questId);
	}

	// Token: 0x0600162C RID: 5676 RVA: 0x00071906 File Offset: 0x0006FB06
	protected static void InvokeUserCode_CmdCompleteQuest__Int32__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdCompleteQuest called on client.");
			return;
		}
		((QuestModule)obj).UserCode_CmdCompleteQuest__Int32__Int32(reader.ReadInt(), reader.ReadInt());
	}

	// Token: 0x0600162D RID: 5677 RVA: 0x00071935 File Offset: 0x0006FB35
	protected void UserCode_CmdFindNewQuest__Boolean(bool alertNoQuests)
	{
		this.FindNewQuest(alertNoQuests);
	}

	// Token: 0x0600162E RID: 5678 RVA: 0x0007193E File Offset: 0x0006FB3E
	protected static void InvokeUserCode_CmdFindNewQuest__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdFindNewQuest called on client.");
			return;
		}
		((QuestModule)obj).UserCode_CmdFindNewQuest__Boolean(reader.ReadBool());
	}

	// Token: 0x0600162F RID: 5679 RVA: 0x00071967 File Offset: 0x0006FB67
	protected void UserCode_TargetShowFindNewQuestsResult__GameObject__Vector3__String__Int32__String__Boolean(GameObject npcObject, Vector3 destinationToTrack, string npcName, int totalAvailableQuests, string worldAreaName, bool alertNoQuests)
	{
		if (!this.ShowSuccessAvailableQuestsMessage(totalAvailableQuests, npcName, worldAreaName, alertNoQuests))
		{
			return;
		}
		this.destinationToTrack = destinationToTrack;
		this.objectToTrack = npcObject;
	}

	// Token: 0x06001630 RID: 5680 RVA: 0x00071988 File Offset: 0x0006FB88
	protected static void InvokeUserCode_TargetShowFindNewQuestsResult__GameObject__Vector3__String__Int32__String__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetShowFindNewQuestsResult called on server.");
			return;
		}
		((QuestModule)obj).UserCode_TargetShowFindNewQuestsResult__GameObject__Vector3__String__Int32__String__Boolean(reader.ReadGameObject(), reader.ReadVector3(), reader.ReadString(), reader.ReadInt(), reader.ReadString(), reader.ReadBool());
	}

	// Token: 0x06001631 RID: 5681 RVA: 0x000719DA File Offset: 0x0006FBDA
	protected void UserCode_CmdTrackQuestObjectives__Int32(int questId)
	{
		this.TrackQuestObjectives(questId);
	}

	// Token: 0x06001632 RID: 5682 RVA: 0x000719E3 File Offset: 0x0006FBE3
	protected static void InvokeUserCode_CmdTrackQuestObjectives__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdTrackQuestObjectives called on client.");
			return;
		}
		((QuestModule)obj).UserCode_CmdTrackQuestObjectives__Int32(reader.ReadInt());
	}

	// Token: 0x06001633 RID: 5683 RVA: 0x00071A0C File Offset: 0x0006FC0C
	protected void UserCode_TargetShowTrackQuestObjectivesResult__GameObject__Vector3(GameObject objectToTrack, Vector3 destinationToTrack)
	{
		if (objectToTrack == null && destinationToTrack == Vector3.zero)
		{
			return;
		}
		this.destinationToTrack = destinationToTrack;
		this.objectToTrack = objectToTrack;
	}

	// Token: 0x06001634 RID: 5684 RVA: 0x00071A33 File Offset: 0x0006FC33
	protected static void InvokeUserCode_TargetShowTrackQuestObjectivesResult__GameObject__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetShowTrackQuestObjectivesResult called on server.");
			return;
		}
		((QuestModule)obj).UserCode_TargetShowTrackQuestObjectivesResult__GameObject__Vector3(reader.ReadGameObject(), reader.ReadVector3());
	}

	// Token: 0x06001635 RID: 5685 RVA: 0x00071A62 File Offset: 0x0006FC62
	protected void UserCode_TargetRemoveDestinationToTrack()
	{
		this.RemoveDestitionToTrack();
	}

	// Token: 0x06001636 RID: 5686 RVA: 0x00071A6A File Offset: 0x0006FC6A
	protected static void InvokeUserCode_TargetRemoveDestinationToTrack(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetRemoveDestinationToTrack called on server.");
			return;
		}
		((QuestModule)obj).UserCode_TargetRemoveDestinationToTrack();
	}

	// Token: 0x06001637 RID: 5687 RVA: 0x00071A90 File Offset: 0x0006FC90
	static QuestModule()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(QuestModule), "System.Void QuestModule::CmdAbortQuest(System.Int32)", new RemoteCallDelegate(QuestModule.InvokeUserCode_CmdAbortQuest__Int32), true);
		RemoteProcedureCalls.RegisterCommand(typeof(QuestModule), "System.Void QuestModule::CmdSetShowOverlay(System.Int32,System.Int32,System.Boolean)", new RemoteCallDelegate(QuestModule.InvokeUserCode_CmdSetShowOverlay__Int32__Int32__Boolean), true);
		RemoteProcedureCalls.RegisterCommand(typeof(QuestModule), "System.Void QuestModule::CmdCompleteQuest(System.Int32,System.Int32)", new RemoteCallDelegate(QuestModule.InvokeUserCode_CmdCompleteQuest__Int32__Int32), true);
		RemoteProcedureCalls.RegisterCommand(typeof(QuestModule), "System.Void QuestModule::CmdFindNewQuest(System.Boolean)", new RemoteCallDelegate(QuestModule.InvokeUserCode_CmdFindNewQuest__Boolean), true);
		RemoteProcedureCalls.RegisterCommand(typeof(QuestModule), "System.Void QuestModule::CmdTrackQuestObjectives(System.Int32)", new RemoteCallDelegate(QuestModule.InvokeUserCode_CmdTrackQuestObjectives__Int32), true);
		RemoteProcedureCalls.RegisterRpc(typeof(QuestModule), "System.Void QuestModule::TargetShowFindNewQuestsResult(UnityEngine.GameObject,UnityEngine.Vector3,System.String,System.Int32,System.String,System.Boolean)", new RemoteCallDelegate(QuestModule.InvokeUserCode_TargetShowFindNewQuestsResult__GameObject__Vector3__String__Int32__String__Boolean));
		RemoteProcedureCalls.RegisterRpc(typeof(QuestModule), "System.Void QuestModule::TargetShowTrackQuestObjectivesResult(UnityEngine.GameObject,UnityEngine.Vector3)", new RemoteCallDelegate(QuestModule.InvokeUserCode_TargetShowTrackQuestObjectivesResult__GameObject__Vector3));
		RemoteProcedureCalls.RegisterRpc(typeof(QuestModule), "System.Void QuestModule::TargetRemoveDestinationToTrack()", new RemoteCallDelegate(QuestModule.InvokeUserCode_TargetRemoveDestinationToTrack));
	}

	// Token: 0x040013F3 RID: 5107
	public readonly SyncListPlayerQuest PlayerQuests = new SyncListPlayerQuest();

	// Token: 0x040013F8 RID: 5112
	private bool AcceptingQuest;

	// Token: 0x040013F9 RID: 5113
	private AIModule aiModule;

	// Token: 0x040013FA RID: 5114
	private PvpModule pvpModule;

	// Token: 0x040013FB RID: 5115
	private TitleModule titleModule;

	// Token: 0x040013FC RID: 5116
	private EffectModule effectModule;

	// Token: 0x040013FD RID: 5117
	private PlayerModule playerModule;

	// Token: 0x040013FE RID: 5118
	private WalletModule walletModule;

	// Token: 0x040013FF RID: 5119
	private UISystemModule uiSystemModule;

	// Token: 0x04001400 RID: 5120
	private VocationModule vocationModule;

	// Token: 0x04001401 RID: 5121
	private AttributeModule attributeModule;

	// Token: 0x04001402 RID: 5122
	private InventoryModule inventoryModule;

	// Token: 0x04001403 RID: 5123
	private NpcDatabaseModule npcDatabaseModule;

	// Token: 0x04001404 RID: 5124
	private ItemDatabaseModule itemDatabaseModule;

	// Token: 0x04001405 RID: 5125
	private TitleDatabaseModule titleDatabaseModule;

	// Token: 0x04001406 RID: 5126
	private QuestDatabaseModule questDatabaseModule;

	// Token: 0x04001407 RID: 5127
	private MonsterDatabaseModule monsterDatabaseModule;

	// Token: 0x04001408 RID: 5128
	public bool ShouldPersistData;

	// Token: 0x04001409 RID: 5129
	public DateTime LastAcceptedDailyTaskLocal = DateTime.MinValue;

	// Token: 0x0400140A RID: 5130
	private GameObject objectToTrack;

	// Token: 0x0400140B RID: 5131
	private Vector3 destinationToTrack = Vector3.zero;

	// Token: 0x020003FC RID: 1020
	// (Invoke) Token: 0x06001639 RID: 5689
	public delegate void OnQuestAcceptedEventHandler(Quest quest);

	// Token: 0x020003FD RID: 1021
	// (Invoke) Token: 0x0600163D RID: 5693
	public delegate void OnQuestUpdatedEventHandler(PlayerQuest playerQuest, PlayerQuestObjective playerQuestObjective, int updateAmount, ObjectiveType objectiveType);

	// Token: 0x020003FE RID: 1022
	// (Invoke) Token: 0x06001641 RID: 5697
	public delegate void OnQuestAbortedEventHandler(PlayerQuest playerQuest);

	// Token: 0x020003FF RID: 1023
	// (Invoke) Token: 0x06001645 RID: 5701
	public delegate void OnQuestCompletedEventHandler(PlayerQuest playerQuest);
}
