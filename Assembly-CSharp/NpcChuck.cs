using System;
using System.Runtime.CompilerServices;
using Mirror;
using UnityEngine;

// Token: 0x02000456 RID: 1110
public class NpcChuck : MonoBehaviour
{
	// Token: 0x060018E4 RID: 6372 RVA: 0x0007DD4C File Offset: 0x0007BF4C
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("DatabaseManager");
		gameObject.TryGetComponent<ItemDatabaseModule>(out this.itemDatabaseModule);
		gameObject.TryGetComponent<QuestDatabaseModule>(out this.questDatabaseModule);
		gameObject.TryGetComponent<MonsterDatabaseModule>(out this.monsterDatabaseModule);
		base.TryGetComponent<NpcModule>(out this.npcModule);
		NpcDialog handshakeDialog = new NpcDialog("npc_chuck_name", "npc_chuck_handshake", new NpcChoice[]
		{
			new NpcChoice("npc_chuck_mastery_books_choice", 1),
			new NpcChoice("npc_show_tasks_choice", 2),
			new NpcChoice("npc_chuck_choice_quests", 88888),
			new NpcChoice("npc_chuck_choice_bye", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
		this.npcModule.AddAction(new NpcAction(1, new NpcAction.NpcTask(this.OpenMasteryBookStore)));
		this.npcModule.AddAction(new NpcAction(2, new NpcAction.NpcTask(this.ShowTasksAsync)));
		this.npcModule.AddAction(new NpcAction(3, delegate(GameObject player)
		{
			this.ChooseTask(player, Rank.Veteran);
		}));
		this.npcModule.AddAction(new NpcAction(4, delegate(GameObject player)
		{
			this.ChooseTask(player, Rank.Elite);
		}));
		this.npcModule.AddAction(new NpcAction(5, delegate(GameObject player)
		{
			this.ChooseTask(player, Rank.Champion);
		}));
		this.npcModule.AddAction(new NpcAction(6, delegate(GameObject player)
		{
			this.ChooseTask(player, Rank.Legendary);
		}));
		this.npcModule.AddAction(new NpcAction(7, delegate(GameObject player)
		{
			this.ChooseTask(player, Rank.Epic);
		}));
		this.npcModule.AddAction(new NpcAction(8, new NpcAction.NpcTask(this.ChooseRedemptionTask)));
	}

	// Token: 0x060018E5 RID: 6373 RVA: 0x0007DEFC File Offset: 0x0007C0FC
	private void OpenMasteryBookStore(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		StoreItemConfig[] storeItems = new StoreItemConfig[]
		{
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(298), 300000)
		};
		PlayerModule playerModule;
		player.TryGetComponent<PlayerModule>(out playerModule);
		NpcDialog npcDialog = new NpcDialog("npc_chuck_name", "npc_chuck_mastery_book_store_greet", storeItems, new NpcChoice[]
		{
			new NpcChoice("npc_chuck_choice_bye", 99999)
		})
		{
			StoreAction = StoreAction.Buy
		};
		playerModule.RenderNpcDialog(npcDialog);
	}

	// Token: 0x060018E6 RID: 6374 RVA: 0x0007DF8C File Offset: 0x0007C18C
	private void ShowTasksAsync(GameObject player)
	{
		NpcChuck.<ShowTasksAsync>d__7 <ShowTasksAsync>d__;
		<ShowTasksAsync>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<ShowTasksAsync>d__.<>4__this = this;
		<ShowTasksAsync>d__.player = player;
		<ShowTasksAsync>d__.<>1__state = -1;
		<ShowTasksAsync>d__.<>t__builder.Start<NpcChuck.<ShowTasksAsync>d__7>(ref <ShowTasksAsync>d__);
	}

	// Token: 0x060018E7 RID: 6375 RVA: 0x0007DFCC File Offset: 0x0007C1CC
	private void ChooseRedemptionTask(GameObject player)
	{
		NpcChuck.<ChooseRedemptionTask>d__8 <ChooseRedemptionTask>d__;
		<ChooseRedemptionTask>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<ChooseRedemptionTask>d__.<>4__this = this;
		<ChooseRedemptionTask>d__.player = player;
		<ChooseRedemptionTask>d__.<>1__state = -1;
		<ChooseRedemptionTask>d__.<>t__builder.Start<NpcChuck.<ChooseRedemptionTask>d__8>(ref <ChooseRedemptionTask>d__);
	}

	// Token: 0x060018E8 RID: 6376 RVA: 0x0007E00C File Offset: 0x0007C20C
	private void ChooseTask(GameObject player, Rank taskRank)
	{
		NpcChuck.<ChooseTask>d__9 <ChooseTask>d__;
		<ChooseTask>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<ChooseTask>d__.<>4__this = this;
		<ChooseTask>d__.player = player;
		<ChooseTask>d__.taskRank = taskRank;
		<ChooseTask>d__.<>1__state = -1;
		<ChooseTask>d__.<>t__builder.Start<NpcChuck.<ChooseTask>d__9>(ref <ChooseTask>d__);
	}

	// Token: 0x060018E9 RID: 6377 RVA: 0x0007E054 File Offset: 0x0007C254
	private QuestObjective[] GetQuestObjectives(int taskId, Rank taskRank)
	{
		switch (taskRank)
		{
		case Rank.Veteran:
		{
			Monster[] nonPassiveMonsters = this.monsterDatabaseModule.GetNonPassiveMonsters(15, 40);
			int num = UnityEngine.Random.Range(0, nonPassiveMonsters.Length);
			return new QuestObjective[]
			{
				new QuestObjective(taskId, nonPassiveMonsters[num].Id, 150, nonPassiveMonsters[num].Name, nonPassiveMonsters[num].PluralName)
			};
		}
		case Rank.Elite:
		{
			Monster[] nonPassiveMonsters2 = this.monsterDatabaseModule.GetNonPassiveMonsters(50, 80);
			int num2 = UnityEngine.Random.Range(0, nonPassiveMonsters2.Length);
			return new QuestObjective[]
			{
				new QuestObjective(taskId, nonPassiveMonsters2[num2].Id, 300, nonPassiveMonsters2[num2].Name, nonPassiveMonsters2[num2].PluralName)
			};
		}
		case Rank.Champion:
		{
			Monster[] nonPassiveMonsters3 = this.monsterDatabaseModule.GetNonPassiveMonsters(90, 125);
			int num3 = UnityEngine.Random.Range(0, nonPassiveMonsters3.Length);
			return new QuestObjective[]
			{
				new QuestObjective(taskId, nonPassiveMonsters3[num3].Id, 500, nonPassiveMonsters3[num3].Name, nonPassiveMonsters3[num3].PluralName)
			};
		}
		case Rank.Legendary:
		{
			Monster[] nonPassiveMonsters4 = this.monsterDatabaseModule.GetNonPassiveMonsters(135, 350);
			int num4 = UnityEngine.Random.Range(0, nonPassiveMonsters4.Length);
			return new QuestObjective[]
			{
				new QuestObjective(taskId, nonPassiveMonsters4[num4].Id, 750, nonPassiveMonsters4[num4].Name, nonPassiveMonsters4[num4].PluralName)
			};
		}
		case Rank.Epic:
		{
			Monster[] nonPassiveMonsters5 = this.monsterDatabaseModule.GetNonPassiveMonsters(400, 600);
			int num5 = UnityEngine.Random.Range(0, nonPassiveMonsters5.Length);
			return new QuestObjective[]
			{
				new QuestObjective(taskId, nonPassiveMonsters5[num5].Id, 1000, nonPassiveMonsters5[num5].Name, nonPassiveMonsters5[num5].PluralName)
			};
		}
		default:
			return new QuestObjective[0];
		}
	}

	// Token: 0x040015CD RID: 5581
	private NpcModule npcModule;

	// Token: 0x040015CE RID: 5582
	private ItemDatabaseModule itemDatabaseModule;

	// Token: 0x040015CF RID: 5583
	private QuestDatabaseModule questDatabaseModule;

	// Token: 0x040015D0 RID: 5584
	private MonsterDatabaseModule monsterDatabaseModule;

	// Token: 0x040015D1 RID: 5585
	private DateTime lastAcceptedDailyTask;
}
