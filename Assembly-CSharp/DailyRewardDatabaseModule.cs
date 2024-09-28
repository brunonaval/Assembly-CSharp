using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x02000311 RID: 785
public class DailyRewardDatabaseModule : MonoBehaviour
{
	// Token: 0x170001B1 RID: 433
	// (get) Token: 0x06000EDA RID: 3802 RVA: 0x00046976 File Offset: 0x00044B76
	// (set) Token: 0x06000EDB RID: 3803 RVA: 0x0004697E File Offset: 0x00044B7E
	public bool IsLoaded { get; private set; }

	// Token: 0x06000EDC RID: 3804 RVA: 0x00046987 File Offset: 0x00044B87
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
		this.itemDatabaseModule = base.GetComponent<ItemDatabaseModule>();
	}

	// Token: 0x06000EDD RID: 3805 RVA: 0x0004699B File Offset: 0x00044B9B
	private IEnumerator Start()
	{
		yield return new WaitUntil(() => this.itemDatabaseModule.IsLoaded);
		yield return this.ProcessDailyRewards().WaitAsCoroutine();
		this.IsLoaded = true;
		yield break;
	}

	// Token: 0x06000EDE RID: 3806 RVA: 0x000469AC File Offset: 0x00044BAC
	private Task ProcessDailyRewards()
	{
		DailyRewardDatabaseModule.<ProcessDailyRewards>d__8 <ProcessDailyRewards>d__;
		<ProcessDailyRewards>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<ProcessDailyRewards>d__.<>4__this = this;
		<ProcessDailyRewards>d__.<>1__state = -1;
		<ProcessDailyRewards>d__.<>t__builder.Start<DailyRewardDatabaseModule.<ProcessDailyRewards>d__8>(ref <ProcessDailyRewards>d__);
		return <ProcessDailyRewards>d__.<>t__builder.Task;
	}

	// Token: 0x06000EDF RID: 3807 RVA: 0x000469F0 File Offset: 0x00044BF0
	private DailyReward BuildDailyReward(DataRow dbDailyReward)
	{
		Item item = this.BuildDailyRewardItem(dbDailyReward);
		return new DailyReward((dbDailyReward["Id"] as int?).GetValueOrDefault(), (dbDailyReward["Amount"] as int?).GetValueOrDefault(), item);
	}

	// Token: 0x06000EE0 RID: 3808 RVA: 0x00046A48 File Offset: 0x00044C48
	private Item BuildDailyRewardItem(DataRow dbDailyReward)
	{
		Item item = this.itemDatabaseModule.GetItem((dbDailyReward["ItemId"] as int?).GetValueOrDefault());
		item.RequiredLevel = (dbDailyReward["RequiredLevel"] as int?).GetValueOrDefault();
		item.Rarity = (Rarity)((dbDailyReward["Rarity"] as int?) ?? 1);
		return item;
	}

	// Token: 0x06000EE1 RID: 3809 RVA: 0x00046AD3 File Offset: 0x00044CD3
	public DailyReward[] GetDailyRewards()
	{
		return this.dailyRewards.ToArray();
	}

	// Token: 0x06000EE2 RID: 3810 RVA: 0x00046AE0 File Offset: 0x00044CE0
	public DailyReward GetDailyReward(int dailyRewardId)
	{
		for (int i = 0; i < this.dailyRewards.Count; i++)
		{
			DailyReward dailyReward = this.dailyRewards[i];
			if (dailyReward.Id == dailyRewardId)
			{
				return dailyReward;
			}
		}
		return default(DailyReward);
	}

	// Token: 0x06000EE3 RID: 3811 RVA: 0x00046B24 File Offset: 0x00044D24
	public int GetMaxRewardId()
	{
		int result;
		try
		{
			result = this.dailyRewards.Max((DailyReward m) => m.Id);
		}
		catch
		{
			result = 1;
		}
		return result;
	}

	// Token: 0x04000EDC RID: 3804
	private ItemDatabaseModule itemDatabaseModule;

	// Token: 0x04000EDD RID: 3805
	private readonly List<DailyReward> dailyRewards = new List<DailyReward>();
}
