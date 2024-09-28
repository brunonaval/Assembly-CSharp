using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000239 RID: 569
public class QuestTrackerObjectiveManager : MonoBehaviour
{
	// Token: 0x060007FD RID: 2045 RVA: 0x000266B8 File Offset: 0x000248B8
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x060007FE RID: 2046 RVA: 0x000266DC File Offset: 0x000248DC
	private void Update()
	{
		if (Time.time - this.updatedTime < 1f)
		{
			return;
		}
		this.updatedTime = Time.time;
		this.UpdateObjectiveInfo(this.objective);
	}

	// Token: 0x060007FF RID: 2047 RVA: 0x00026709 File Offset: 0x00024909
	public void SetObjective(PlayerQuestObjective objective)
	{
		this.objective = objective;
		this.UpdateObjectiveInfo(objective);
	}

	// Token: 0x06000800 RID: 2048 RVA: 0x0002671C File Offset: 0x0002491C
	private void UpdateObjectiveInfo(PlayerQuestObjective objective)
	{
		string text = LanguageManager.Instance.GetText(GlobalUtils.RankToString(objective.ObjectiveRank));
		text = (string.IsNullOrEmpty(text) ? "" : (" (" + text + ")"));
		switch (objective.ObjectiveType)
		{
		case ObjectiveType.CollectItems:
			this.UpdateCollectObjectiveText(objective, text);
			return;
		case ObjectiveType.KillMonsters:
			this.UpdateKillObjectiveText(objective, text);
			return;
		case ObjectiveType.TalkNpc:
			this.UpdateTalkObjectiveText(objective);
			return;
		default:
			return;
		}
	}

	// Token: 0x06000801 RID: 2049 RVA: 0x00026794 File Offset: 0x00024994
	private void UpdateTalkObjectiveText(PlayerQuestObjective objective)
	{
		bool flag = objective.ObjectiveProgress > 0;
		string objectiveName = objective.ObjectiveName;
		if (flag)
		{
			this.completedImageObject.SetActive(true);
			string arg = string.Format(LanguageManager.Instance.GetText("objective_talk_ready"), objectiveName);
			this.objectiveText.text = string.Format("<color=green>{0}</color>", arg);
			return;
		}
		this.objectiveText.text = string.Format(LanguageManager.Instance.GetText("objective_talk"), objectiveName);
	}

	// Token: 0x06000802 RID: 2050 RVA: 0x0002680C File Offset: 0x00024A0C
	private void UpdateCollectObjectiveText(PlayerQuestObjective objective, string rankName)
	{
		int amount = this.uiSystemModule.InventoryModule.GetAmount(objective.ObjectiveId);
		int num = objective.ObjectiveAmount - amount;
		if (num < 1)
		{
			this.completedImageObject.SetActive(true);
			string text = (objective.ObjectiveAmount > 1) ? objective.ObjectivePluralName : objective.ObjectiveName;
			text = LanguageManager.Instance.GetText(text);
			string arg = string.Format(LanguageManager.Instance.GetText("objective_collect_ready"), objective.ObjectiveAmount, text);
			this.objectiveText.text = string.Format("<color=green>{0}</color>", arg);
			return;
		}
		string text2 = (num > 1) ? objective.ObjectivePluralName : objective.ObjectiveName;
		text2 = LanguageManager.Instance.GetText(text2);
		this.objectiveText.text = string.Format(LanguageManager.Instance.GetText("objective_collect_left"), num, text2, objective.ObjectiveAmount);
	}

	// Token: 0x06000803 RID: 2051 RVA: 0x000268FC File Offset: 0x00024AFC
	private void UpdateKillObjectiveText(PlayerQuestObjective objective, string rankName)
	{
		int num = objective.ObjectiveAmount - objective.ObjectiveProgress;
		if (num < 1)
		{
			this.completedImageObject.SetActive(true);
			string text = (objective.ObjectiveAmount > 1) ? objective.ObjectivePluralName : objective.ObjectiveName;
			text = LanguageManager.Instance.GetText(text);
			string arg = string.Format(LanguageManager.Instance.GetText("objective_kill_ready"), objective.ObjectiveAmount, text, rankName);
			this.objectiveText.text = string.Format("<color=green>{0}</color>", arg);
			return;
		}
		string text2 = (num > 1) ? objective.ObjectivePluralName : objective.ObjectiveName;
		text2 = LanguageManager.Instance.GetText(text2);
		this.objectiveText.text = string.Format(LanguageManager.Instance.GetText("objective_kill_left"), new object[]
		{
			num,
			text2,
			rankName,
			objective.ObjectiveAmount
		});
	}

	// Token: 0x040009DB RID: 2523
	[SerializeField]
	private GameObject completedImageObject;

	// Token: 0x040009DC RID: 2524
	[SerializeField]
	private Text objectiveText;

	// Token: 0x040009DD RID: 2525
	private PlayerQuestObjective objective;

	// Token: 0x040009DE RID: 2526
	private float updatedTime;

	// Token: 0x040009DF RID: 2527
	private UISystemModule uiSystemModule;
}
