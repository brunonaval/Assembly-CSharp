using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000234 RID: 564
public class QuestManager : MonoBehaviour
{
	// Token: 0x060007E9 RID: 2025 RVA: 0x00026204 File Offset: 0x00024404
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x060007EA RID: 2026 RVA: 0x00026228 File Offset: 0x00024428
	public void SetQuest(PlayerQuest quest)
	{
		this.UpdateQuestInfo(quest);
	}

	// Token: 0x060007EB RID: 2027 RVA: 0x00026234 File Offset: 0x00024434
	private void UpdateQuestInfo(PlayerQuest quest)
	{
		if (!quest.IsDefined)
		{
			return;
		}
		this.questNameText.text = LanguageManager.Instance.GetText(quest.Name);
		if (this.uiSystemModule.QuestModule.CanCompleteQuest(quest.QuestId))
		{
			this.questNameText.color = GlobalSettings.Colors[1];
		}
		foreach (PlayerQuestObjective objective in quest.Objectives)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.objetivePrefab);
			gameObject.transform.SetParent(this.objectivesGroup.transform, false);
			gameObject.transform.position = Vector2.zero;
			gameObject.GetComponent<QuestTrackerObjectiveManager>().SetObjective(objective);
		}
	}

	// Token: 0x040009C7 RID: 2503
	[SerializeField]
	private Text questNameText;

	// Token: 0x040009C8 RID: 2504
	[SerializeField]
	private GameObject objetivePrefab;

	// Token: 0x040009C9 RID: 2505
	[SerializeField]
	private GameObject objectivesGroup;

	// Token: 0x040009CA RID: 2506
	private UISystemModule uiSystemModule;
}
