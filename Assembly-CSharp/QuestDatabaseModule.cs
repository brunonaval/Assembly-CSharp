using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x020003F4 RID: 1012
public class QuestDatabaseModule : MonoBehaviour
{
	// Token: 0x1700024A RID: 586
	// (get) Token: 0x060015D0 RID: 5584 RVA: 0x0006F60E File Offset: 0x0006D80E
	// (set) Token: 0x060015D1 RID: 5585 RVA: 0x0006F616 File Offset: 0x0006D816
	public bool IsLoaded { get; private set; }

	// Token: 0x060015D2 RID: 5586 RVA: 0x0006F61F File Offset: 0x0006D81F
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
		this.npcDatabaseModule = base.GetComponent<NpcDatabaseModule>();
		this.itemDatabaseModule = base.GetComponent<ItemDatabaseModule>();
		this.titleDatabaseModule = base.GetComponent<TitleDatabaseModule>();
		this.monsterDatabaseModule = base.GetComponent<MonsterDatabaseModule>();
	}

	// Token: 0x060015D3 RID: 5587 RVA: 0x0006F657 File Offset: 0x0006D857
	private IEnumerator Start()
	{
		yield return new WaitUntil(() => this.titleDatabaseModule.IsLoaded);
		yield return new WaitUntil(() => this.itemDatabaseModule.IsLoaded);
		yield return new WaitUntil(() => this.monsterDatabaseModule.IsLoaded);
		yield return this.ProcessQuestsAsync().WaitAsCoroutine();
		this.IsLoaded = true;
		yield break;
	}

	// Token: 0x060015D4 RID: 5588 RVA: 0x0006F668 File Offset: 0x0006D868
	private Task ProcessQuestsAsync()
	{
		QuestDatabaseModule.<ProcessQuestsAsync>d__11 <ProcessQuestsAsync>d__;
		<ProcessQuestsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<ProcessQuestsAsync>d__.<>4__this = this;
		<ProcessQuestsAsync>d__.<>1__state = -1;
		<ProcessQuestsAsync>d__.<>t__builder.Start<QuestDatabaseModule.<ProcessQuestsAsync>d__11>(ref <ProcessQuestsAsync>d__);
		return <ProcessQuestsAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060015D5 RID: 5589 RVA: 0x0006F6AC File Offset: 0x0006D8AC
	public void Add(Quest quest)
	{
		if (this.quests.Any((Quest a) => a.Id == quest.Id))
		{
			Debug.LogErrorFormat("Can't add quest, id [{0}] already exists.", new object[]
			{
				quest.Id
			});
			return;
		}
		this.quests.Add(quest);
	}

	// Token: 0x060015D6 RID: 5590 RVA: 0x0006F714 File Offset: 0x0006D914
	public Quest GetQuest(int questId)
	{
		for (int i = 0; i < this.quests.Count; i++)
		{
			if (this.quests[i].Id == questId)
			{
				return this.quests[i];
			}
		}
		return default(Quest);
	}

	// Token: 0x060015D7 RID: 5591 RVA: 0x0006F764 File Offset: 0x0006D964
	public Quest[] GetTasks(Rank taskRank)
	{
		return (from q in this.quests
		where q.Rank == taskRank & q.IsDailyTask
		select q).ToArray<Quest>();
	}

	// Token: 0x060015D8 RID: 5592 RVA: 0x0006F79C File Offset: 0x0006D99C
	public Quest[] GetQuests(int requiredLevel, bool matchLevel)
	{
		if (matchLevel)
		{
			return (from q in this.quests
			where q.RequiredLevel == requiredLevel & !q.IsDailyTask
			select q).ToArray<Quest>();
		}
		return (from q in this.quests
		where q.RequiredLevel <= requiredLevel & !q.IsDailyTask
		select q).ToArray<Quest>();
	}

	// Token: 0x060015D9 RID: 5593 RVA: 0x0006F7F4 File Offset: 0x0006D9F4
	public Quest[] GetQuestsByNpcId(int npcId)
	{
		return (from q in this.quests
		where q.NpcId == npcId & !q.IsDailyTask
		select q).ToArray<Quest>();
	}

	// Token: 0x040013DE RID: 5086
	private List<Quest> quests = new List<Quest>();

	// Token: 0x040013DF RID: 5087
	private NpcDatabaseModule npcDatabaseModule;

	// Token: 0x040013E0 RID: 5088
	private ItemDatabaseModule itemDatabaseModule;

	// Token: 0x040013E1 RID: 5089
	private TitleDatabaseModule titleDatabaseModule;

	// Token: 0x040013E2 RID: 5090
	private MonsterDatabaseModule monsterDatabaseModule;
}
