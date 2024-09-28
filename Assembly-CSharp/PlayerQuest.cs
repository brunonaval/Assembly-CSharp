using System;
using System.Linq;
using System.Text;
using UnityEngine;

// Token: 0x0200012E RID: 302
public struct PlayerQuest
{
	// Token: 0x17000067 RID: 103
	// (get) Token: 0x06000330 RID: 816 RVA: 0x000147CC File Offset: 0x000129CC
	public DateTime StartedAsDateTime
	{
		get
		{
			DateTime result;
			DateTime.TryParse(this.Started, out result);
			return result;
		}
	}

	// Token: 0x17000068 RID: 104
	// (get) Token: 0x06000331 RID: 817 RVA: 0x000147E8 File Offset: 0x000129E8
	public DateTime CompletedAsDateTime
	{
		get
		{
			DateTime result;
			DateTime.TryParse(this.Completed, out result);
			return result;
		}
	}

	// Token: 0x17000069 RID: 105
	// (get) Token: 0x06000332 RID: 818 RVA: 0x00014804 File Offset: 0x00012A04
	public bool IsCompleted
	{
		get
		{
			return !string.IsNullOrEmpty(this.Completed);
		}
	}

	// Token: 0x1700006A RID: 106
	// (get) Token: 0x06000333 RID: 819 RVA: 0x00014814 File Offset: 0x00012A14
	public bool IsDefined
	{
		get
		{
			return !string.IsNullOrEmpty(this.Name) | this.QuestId > 0;
		}
	}

	// Token: 0x1700006B RID: 107
	// (get) Token: 0x06000334 RID: 820 RVA: 0x00014830 File Offset: 0x00012A30
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
					foreach (PlayerQuestObjective playerQuestObjective in this.Objectives)
					{
						bool isCompleted = this.IsCompleted;
						if (playerQuestObjective.ObjectiveType == ObjectiveType.KillMonsters)
						{
							int num = isCompleted ? 0 : (playerQuestObjective.ObjectiveAmount - playerQuestObjective.ObjectiveProgress);
							string text = LanguageManager.Instance.GetText(GlobalUtils.RankToString(playerQuestObjective.ObjectiveRank));
							text = (string.IsNullOrEmpty(text) ? "" : (" (" + text + ")"));
							if (num < 1)
							{
								string text2 = (playerQuestObjective.ObjectiveAmount > 1) ? playerQuestObjective.ObjectivePluralName : playerQuestObjective.ObjectiveName;
								text2 = LanguageManager.Instance.GetText(text2);
								stringBuilder.AppendLine(string.Format(LanguageManager.Instance.GetText("objective_kill_ready"), playerQuestObjective.ObjectiveAmount, text2, text));
							}
							else
							{
								string text3 = (num > 1) ? playerQuestObjective.ObjectivePluralName : playerQuestObjective.ObjectiveName;
								text3 = LanguageManager.Instance.GetText(text3);
								stringBuilder.AppendLine(string.Format(LanguageManager.Instance.GetText("objective_kill_left"), new object[]
								{
									num,
									text3,
									text,
									playerQuestObjective.ObjectiveAmount
								}));
							}
						}
						else if (playerQuestObjective.ObjectiveType == ObjectiveType.CollectItems)
						{
							int num2 = 0;
							if (!isCompleted)
							{
								if (uiSystemModule == null)
								{
									GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
									uiSystemModule = gameObject.GetComponent<UISystemModule>();
								}
								int amount = uiSystemModule.InventoryModule.GetAmount(playerQuestObjective.ObjectiveId);
								num2 = playerQuestObjective.ObjectiveAmount - amount;
							}
							if (num2 < 1)
							{
								string text4 = (playerQuestObjective.ObjectiveAmount > 1) ? playerQuestObjective.ObjectivePluralName : playerQuestObjective.ObjectiveName;
								text4 = LanguageManager.Instance.GetText(text4);
								stringBuilder.AppendLine(string.Format(LanguageManager.Instance.GetText("objective_collect_ready"), playerQuestObjective.ObjectiveAmount, text4));
							}
							else
							{
								string text5 = (num2 > 1) ? playerQuestObjective.ObjectivePluralName : playerQuestObjective.ObjectiveName;
								text5 = LanguageManager.Instance.GetText(text5);
								stringBuilder.AppendLine(string.Format(LanguageManager.Instance.GetText("objective_collect_left"), num2, text5, playerQuestObjective.ObjectiveAmount));
							}
						}
						else if (playerQuestObjective.ObjectiveType == ObjectiveType.TalkNpc)
						{
							bool flag = playerQuestObjective.ObjectiveProgress > 0;
							string objectiveName = playerQuestObjective.ObjectiveName;
							if (flag)
							{
								stringBuilder.AppendLine(string.Format(LanguageManager.Instance.GetText("objective_talk_ready"), objectiveName));
							}
							else
							{
								stringBuilder.AppendLine(string.Format(LanguageManager.Instance.GetText("objective_talk"), objectiveName));
							}
						}
					}
				}
				stringBuilder.AppendLine();
				if (uiSystemModule == null)
				{
					GameObject gameObject2 = GameObject.FindGameObjectWithTag("UISystem");
					uiSystemModule = gameObject2.GetComponent<UISystemModule>();
				}
				PlayerQuestReward[] array = (from w in this.Rewards
				where w.Vocation == Vocation.Undefined | w.Vocation == uiSystemModule.VocationModule.Vocation
				select w).ToArray<PlayerQuestReward>();
				if (array.Length != 0)
				{
					stringBuilder.AppendLine("<color=green>" + LanguageManager.Instance.GetText("rewards") + ":</color>");
					foreach (PlayerQuestReward playerQuestReward in array)
					{
						if (playerQuestReward.RewardType == RewardType.Experience)
						{
							stringBuilder.AppendLine(string.Format(LanguageManager.Instance.GetText("reward_experience"), playerQuestReward.Amount));
						}
						else if (playerQuestReward.RewardType == RewardType.GoldCoins)
						{
							stringBuilder.AppendLine(string.Format(LanguageManager.Instance.GetText("reward_gold_coins"), playerQuestReward.Amount));
						}
						else if (playerQuestReward.RewardType == RewardType.Item)
						{
							string key = (playerQuestReward.Amount == 1) ? playerQuestReward.ItemName : GlobalUtils.GetEnglishPlural(playerQuestReward.ItemName);
							stringBuilder.AppendLine(string.Format(LanguageManager.Instance.GetText("reward_item"), playerQuestReward.Amount, LanguageManager.Instance.GetText(key)));
						}
						else if (playerQuestReward.RewardType == RewardType.Title)
						{
							stringBuilder.AppendLine(string.Format(LanguageManager.Instance.GetText("reward_title"), LanguageManager.Instance.GetText(playerQuestReward.TitleName)));
						}
						else if (playerQuestReward.RewardType == RewardType.MasteryLevel)
						{
							stringBuilder.AppendLine(LanguageManager.Instance.GetText("reward_mastery_level"));
						}
					}
				}
				this._fullDescription = stringBuilder.ToString();
			}
			return this._fullDescription;
		}
	}

	// Token: 0x1700006C RID: 108
	// (get) Token: 0x06000335 RID: 821 RVA: 0x00014D98 File Offset: 0x00012F98
	public string OverlayDescription
	{
		get
		{
			if (string.IsNullOrEmpty(this._overlayDescription))
			{
				UISystemModule uisystemModule = null;
				StringBuilder stringBuilder = new StringBuilder();
				if (this.Objectives.Length != 0)
				{
					foreach (PlayerQuestObjective playerQuestObjective in this.Objectives)
					{
						bool isCompleted = this.IsCompleted;
						if (playerQuestObjective.ObjectiveType == ObjectiveType.KillMonsters)
						{
							int num = isCompleted ? 0 : (playerQuestObjective.ObjectiveAmount - playerQuestObjective.ObjectiveProgress);
							string text = LanguageManager.Instance.GetText(GlobalUtils.RankToString(playerQuestObjective.ObjectiveRank));
							text = (string.IsNullOrEmpty(text) ? "" : (" (" + text + ")"));
							if (num < 1)
							{
								string text2 = (playerQuestObjective.ObjectiveAmount > 1) ? playerQuestObjective.ObjectivePluralName : playerQuestObjective.ObjectiveName;
								text2 = LanguageManager.Instance.GetText(text2);
								string arg = string.Format(LanguageManager.Instance.GetText("objective_kill_ready"), playerQuestObjective.ObjectiveAmount, text2, text);
								stringBuilder.AppendLine(string.Format("<color=green>{0}</color>", arg));
							}
							else
							{
								string text3 = (num > 1) ? playerQuestObjective.ObjectivePluralName : playerQuestObjective.ObjectiveName;
								text3 = LanguageManager.Instance.GetText(text3);
								stringBuilder.AppendLine(string.Format(LanguageManager.Instance.GetText("objective_kill_left"), new object[]
								{
									num,
									text3,
									text,
									playerQuestObjective.ObjectiveAmount
								}));
							}
						}
						else if (playerQuestObjective.ObjectiveType == ObjectiveType.CollectItems)
						{
							int num2 = 0;
							if (!isCompleted)
							{
								if (uisystemModule == null)
								{
									uisystemModule = GameObject.FindGameObjectWithTag("UISystem").GetComponent<UISystemModule>();
								}
								int amount = uisystemModule.InventoryModule.GetAmount(playerQuestObjective.ObjectiveId);
								num2 = playerQuestObjective.ObjectiveAmount - amount;
							}
							if (num2 < 1)
							{
								string text4 = (playerQuestObjective.ObjectiveAmount > 1) ? playerQuestObjective.ObjectivePluralName : playerQuestObjective.ObjectiveName;
								text4 = LanguageManager.Instance.GetText(text4);
								string arg2 = string.Format(LanguageManager.Instance.GetText("objective_collect_ready"), playerQuestObjective.ObjectiveAmount, text4);
								stringBuilder.AppendLine(string.Format("<color=green>{0}</color>", arg2));
							}
							else
							{
								string text5 = (num2 > 1) ? playerQuestObjective.ObjectivePluralName : playerQuestObjective.ObjectiveName;
								text5 = LanguageManager.Instance.GetText(text5);
								stringBuilder.AppendLine(string.Format(LanguageManager.Instance.GetText("objective_collect_left"), num2, text5, playerQuestObjective.ObjectiveAmount));
							}
						}
						else if (playerQuestObjective.ObjectiveType == ObjectiveType.TalkNpc)
						{
							bool flag = playerQuestObjective.ObjectiveProgress > 0;
							string objectiveName = playerQuestObjective.ObjectiveName;
							if (flag)
							{
								string arg3 = string.Format(LanguageManager.Instance.GetText("objective_talk_ready"), objectiveName);
								stringBuilder.AppendLine(string.Format("<color=green>{0}</color>", arg3));
							}
							else
							{
								stringBuilder.AppendLine(string.Format(LanguageManager.Instance.GetText("objective_talk"), objectiveName));
							}
						}
					}
				}
				this._overlayDescription = stringBuilder.ToString().TrimEnd(new char[]
				{
					'\r',
					'\n'
				});
			}
			return this._overlayDescription;
		}
	}

	// Token: 0x06000336 RID: 822 RVA: 0x000150D0 File Offset: 0x000132D0
	public PlayerQuest(int playerId, int questId, string name, string description, string started, string completed, int requiredLevel, bool showOverlay, bool isDailyTask)
	{
		this.PlayerId = playerId;
		this.QuestId = questId;
		this.Name = name;
		this.Description = description;
		this.Started = started;
		this.Completed = completed;
		this.RequiredLevel = requiredLevel;
		this.ShowOverlay = showOverlay;
		this.IsDailyTask = isDailyTask;
		this.Rewards = new PlayerQuestReward[0];
		this.Objectives = new PlayerQuestObjective[0];
		this._fullDescription = string.Empty;
		this._overlayDescription = string.Empty;
	}

	// Token: 0x04000611 RID: 1553
	public string Name;

	// Token: 0x04000612 RID: 1554
	public int QuestId;

	// Token: 0x04000613 RID: 1555
	public int PlayerId;

	// Token: 0x04000614 RID: 1556
	public string Started;

	// Token: 0x04000615 RID: 1557
	private string _fullDescription;

	// Token: 0x04000616 RID: 1558
	private string _overlayDescription;

	// Token: 0x04000617 RID: 1559
	public string Completed;

	// Token: 0x04000618 RID: 1560
	public int RequiredLevel;

	// Token: 0x04000619 RID: 1561
	public string Description;

	// Token: 0x0400061A RID: 1562
	public bool ShowOverlay;

	// Token: 0x0400061B RID: 1563
	public bool IsDailyTask;

	// Token: 0x0400061C RID: 1564
	public PlayerQuestReward[] Rewards;

	// Token: 0x0400061D RID: 1565
	public PlayerQuestObjective[] Objectives;
}
