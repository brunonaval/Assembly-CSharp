using System;
using System.Linq;
using System.Text;
using UnityEngine;

// Token: 0x0200013B RID: 315
public struct Quest
{
	// Token: 0x17000070 RID: 112
	// (get) Token: 0x06000349 RID: 841 RVA: 0x00015395 File Offset: 0x00013595
	// (set) Token: 0x0600034A RID: 842 RVA: 0x0001539D File Offset: 0x0001359D
	public int NpcId { readonly get; set; }

	// Token: 0x17000071 RID: 113
	// (get) Token: 0x0600034B RID: 843 RVA: 0x000153A6 File Offset: 0x000135A6
	// (set) Token: 0x0600034C RID: 844 RVA: 0x000153AE File Offset: 0x000135AE
	public Rank Rank { readonly get; set; }

	// Token: 0x17000072 RID: 114
	// (get) Token: 0x0600034D RID: 845 RVA: 0x000153B7 File Offset: 0x000135B7
	// (set) Token: 0x0600034E RID: 846 RVA: 0x000153BF File Offset: 0x000135BF
	public bool IsDailyTask { readonly get; set; }

	// Token: 0x17000073 RID: 115
	// (get) Token: 0x0600034F RID: 847 RVA: 0x000153C8 File Offset: 0x000135C8
	// (set) Token: 0x06000350 RID: 848 RVA: 0x000153D0 File Offset: 0x000135D0
	public string PositiveChoice { readonly get; set; }

	// Token: 0x17000074 RID: 116
	// (get) Token: 0x06000351 RID: 849 RVA: 0x000153D9 File Offset: 0x000135D9
	// (set) Token: 0x06000352 RID: 850 RVA: 0x000153E1 File Offset: 0x000135E1
	public string NegativeChoice { readonly get; set; }

	// Token: 0x17000075 RID: 117
	// (get) Token: 0x06000353 RID: 851 RVA: 0x000153EA File Offset: 0x000135EA
	// (set) Token: 0x06000354 RID: 852 RVA: 0x000153F2 File Offset: 0x000135F2
	public string CompletedDialog { readonly get; set; }

	// Token: 0x17000076 RID: 118
	// (get) Token: 0x06000355 RID: 853 RVA: 0x000153FB File Offset: 0x000135FB
	// (set) Token: 0x06000356 RID: 854 RVA: 0x00015403 File Offset: 0x00013603
	public string CompletedChoice { readonly get; set; }

	// Token: 0x17000077 RID: 119
	// (get) Token: 0x06000357 RID: 855 RVA: 0x0001540C File Offset: 0x0001360C
	// (set) Token: 0x06000358 RID: 856 RVA: 0x00015414 File Offset: 0x00013614
	public string InProgressDialog { readonly get; set; }

	// Token: 0x17000078 RID: 120
	// (get) Token: 0x06000359 RID: 857 RVA: 0x0001541D File Offset: 0x0001361D
	// (set) Token: 0x0600035A RID: 858 RVA: 0x00015425 File Offset: 0x00013625
	public string InProgressChoice { readonly get; set; }

	// Token: 0x17000079 RID: 121
	// (get) Token: 0x0600035B RID: 859 RVA: 0x0001542E File Offset: 0x0001362E
	public bool IsDefined
	{
		get
		{
			return !string.IsNullOrEmpty(this.Name) | this.Id > 0;
		}
	}

	// Token: 0x1700007A RID: 122
	// (get) Token: 0x0600035C RID: 860 RVA: 0x00015448 File Offset: 0x00013648
	public string FullDescription
	{
		get
		{
			if (string.IsNullOrEmpty(this._fullDescription))
			{
				UISystemModule uiSystemModule = null;
				StringBuilder stringBuilder = new StringBuilder();
				string value = string.Format("{0} ({1}: {2})", LanguageManager.Instance.GetText(this.Name), LanguageManager.Instance.GetText("level"), this.RequiredLevel);
				stringBuilder.AppendLine(value);
				stringBuilder.AppendLine();
				stringBuilder.AppendLine(LanguageManager.Instance.GetText(this.Description));
				stringBuilder.AppendLine();
				if (this.Objectives.Length != 0)
				{
					stringBuilder.AppendLine("<color=red>" + LanguageManager.Instance.GetText("objectives") + ":</color>");
					foreach (QuestObjective questObjective in this.Objectives)
					{
						if (questObjective.ObjectiveType == ObjectiveType.KillMonsters)
						{
							string text = LanguageManager.Instance.GetText(GlobalUtils.RankToString(questObjective.ObjectiveRank));
							text = (string.IsNullOrEmpty(text) ? "" : (" (" + text + ")"));
							string text2 = (questObjective.ObjectiveAmount > 1) ? questObjective.ObjectivePluralName : questObjective.ObjectiveName;
							text2 = LanguageManager.Instance.GetText(text2);
							stringBuilder.AppendLine(string.Format(LanguageManager.Instance.GetText("objective_kill"), questObjective.ObjectiveAmount, text2, text));
						}
						else if (questObjective.ObjectiveType == ObjectiveType.CollectItems)
						{
							string text3 = (questObjective.ObjectiveAmount > 1) ? questObjective.ObjectivePluralName : questObjective.ObjectiveName;
							text3 = LanguageManager.Instance.GetText(text3);
							stringBuilder.AppendLine(string.Format(LanguageManager.Instance.GetText("objective_collect"), questObjective.ObjectiveAmount, text3));
						}
						else if (questObjective.ObjectiveType == ObjectiveType.TalkNpc)
						{
							string objectiveName = questObjective.ObjectiveName;
							stringBuilder.AppendLine(string.Format(LanguageManager.Instance.GetText("objective_talk"), objectiveName));
						}
					}
				}
				stringBuilder.AppendLine();
				if (uiSystemModule == null)
				{
					GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
					uiSystemModule = gameObject.GetComponent<UISystemModule>();
				}
				QuestReward[] array = (from w in this.Rewards
				where w.Vocation == Vocation.Undefined | w.Vocation == uiSystemModule.VocationModule.Vocation
				select w).ToArray<QuestReward>();
				if (array.Length != 0)
				{
					stringBuilder.AppendLine("<color=green>" + LanguageManager.Instance.GetText("rewards") + ":</color>");
					foreach (QuestReward questReward in array)
					{
						if (questReward.RewardType == RewardType.Experience)
						{
							stringBuilder.AppendLine(string.Format(LanguageManager.Instance.GetText("reward_experience"), questReward.Amount));
						}
						else if (questReward.RewardType == RewardType.GoldCoins)
						{
							stringBuilder.AppendLine(string.Format(LanguageManager.Instance.GetText("reward_gold_coins"), questReward.Amount));
						}
						else if (questReward.RewardType == RewardType.Item)
						{
							string key = (questReward.Amount == 1) ? questReward.ItemName : GlobalUtils.GetEnglishPlural(questReward.ItemName);
							stringBuilder.AppendLine(string.Format(LanguageManager.Instance.GetText("reward_item"), questReward.Amount, LanguageManager.Instance.GetText(key)));
						}
						else if (questReward.RewardType == RewardType.Title)
						{
							stringBuilder.AppendLine(string.Format(LanguageManager.Instance.GetText("reward_title"), LanguageManager.Instance.GetText(questReward.TitleName)));
						}
					}
				}
				this._fullDescription = stringBuilder.ToString();
			}
			return this._fullDescription;
		}
	}

	// Token: 0x0600035D RID: 861 RVA: 0x00015818 File Offset: 0x00013A18
	public Quest(int id, int npcId, Rank rank, string name, bool isDailyTask, int requiredLevel, string description, string positiveChoice, string negativeChoice, string completedDialog, string completedChoice, string inProgressDialog, string inProgressChoice)
	{
		this.Id = id;
		this.Name = name;
		this.Rank = rank;
		this.NpcId = npcId;
		this.Description = description;
		this.IsDailyTask = isDailyTask;
		this.RequiredLevel = requiredLevel;
		this.PositiveChoice = positiveChoice;
		this.NegativeChoice = negativeChoice;
		this.CompletedDialog = completedDialog;
		this.CompletedChoice = completedChoice;
		this.InProgressDialog = inProgressDialog;
		this.InProgressChoice = inProgressChoice;
		this.Rewards = new QuestReward[0];
		this.Objectives = new QuestObjective[0];
		this._fullDescription = string.Empty;
	}

	// Token: 0x04000661 RID: 1633
	public int Id;

	// Token: 0x04000662 RID: 1634
	public string Name;

	// Token: 0x04000663 RID: 1635
	private string _fullDescription;

	// Token: 0x04000664 RID: 1636
	public int RequiredLevel;

	// Token: 0x04000665 RID: 1637
	public string Description;

	// Token: 0x04000666 RID: 1638
	public QuestReward[] Rewards;

	// Token: 0x04000667 RID: 1639
	public QuestObjective[] Objectives;
}
