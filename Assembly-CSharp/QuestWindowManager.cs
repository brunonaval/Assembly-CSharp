using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200028B RID: 651
public class QuestWindowManager : MonoBehaviour
{
	// Token: 0x06000A20 RID: 2592 RVA: 0x0002EC44 File Offset: 0x0002CE44
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x06000A21 RID: 2593 RVA: 0x0002EC68 File Offset: 0x0002CE68
	private void OnEnable()
	{
		this.RefreshPlayerQuests();
	}

	// Token: 0x06000A22 RID: 2594 RVA: 0x0002EC70 File Offset: 0x0002CE70
	public void RefreshPlayerQuests()
	{
		if (this.uiSystemModule.QuestModule == null)
		{
			return;
		}
		for (int i = 0; i < this.questHolder.transform.childCount; i++)
		{
			Transform child = this.questHolder.transform.GetChild(i);
			if (child != null)
			{
				UnityEngine.Object.Destroy(child.gameObject);
			}
		}
		IEnumerable<PlayerQuest> enumerable = (from o in this.uiSystemModule.QuestModule.PlayerQuests
		orderby o.IsCompleted
		select o).ThenBy((PlayerQuest t) => t.RequiredLevel).ThenBy((PlayerQuest t) => t.Name);
		if (!this.showCompletedToggle.isOn)
		{
			enumerable = from q in enumerable
			where !q.IsCompleted
			select q;
		}
		foreach (PlayerQuest playerQuest in enumerable)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.questSlotPrefab);
			gameObject.transform.SetParent(this.questHolder.transform, false);
			gameObject.transform.position = Vector2.zero;
			gameObject.GetComponent<QuestSlotManager>().SetPlayerQuest(playerQuest);
		}
	}

	// Token: 0x06000A23 RID: 2595 RVA: 0x0002EDFC File Offset: 0x0002CFFC
	private void OnDisable()
	{
		this.selectedQuest = default(PlayerQuest);
		this.displayText.text = string.Empty;
	}

	// Token: 0x06000A24 RID: 2596 RVA: 0x0002EE1C File Offset: 0x0002D01C
	public void SetSelectedQuest(PlayerQuest quest)
	{
		this.selectedQuest = quest;
		if (this.selectedQuest.IsDefined & !this.selectedQuest.IsCompleted)
		{
			this.showOverlayButton.interactable = true;
			this.showOverlayText.text = (this.selectedQuest.ShowOverlay ? LanguageManager.Instance.GetText("hide").ToLower() : LanguageManager.Instance.GetText("show").ToLower());
			if (this.uiSystemModule.QuestModule.CanCompleteQuest(quest.QuestId))
			{
				this.completeButton.interactable = true;
			}
			else
			{
				this.completeButton.interactable = false;
			}
		}
		else
		{
			this.completeButton.interactable = false;
			this.showOverlayButton.interactable = false;
		}
		this.displayText.text = this.selectedQuest.FullDescription;
	}

	// Token: 0x06000A25 RID: 2597 RVA: 0x0002EF00 File Offset: 0x0002D100
	public void AbortQuest()
	{
		if (!this.selectedQuest.IsCompleted)
		{
			if (!this.abortQuestWindow.activeInHierarchy)
			{
				this.abortQuestWindow.SetActive(true);
			}
			this.abortQuestWindow.GetComponent<AbortQuestWindowManager>().SetQuest(this.selectedQuest);
			return;
		}
		this.uiSystemModule.EffectModule.ShowScreenMessage("quest_cant_abort_completed_message", 0, 3.5f, Array.Empty<string>());
	}

	// Token: 0x06000A26 RID: 2598 RVA: 0x0002EF6C File Offset: 0x0002D16C
	public void CompleteQuest()
	{
		if (!this.selectedQuest.IsCompleted)
		{
			this.uiSystemModule.QuestModule.CompleteQuest(this.selectedQuest.PlayerId, this.selectedQuest.QuestId);
			this.selectedQuest = default(PlayerQuest);
			this.displayText.text = string.Empty;
			this.uiSystemModule.HideQuestWindow();
			return;
		}
		this.uiSystemModule.EffectModule.ShowScreenMessage("quest_already_completed_message", 0, 3.5f, Array.Empty<string>());
	}

	// Token: 0x06000A27 RID: 2599 RVA: 0x0002EFF4 File Offset: 0x0002D1F4
	public void SetShowOverlay()
	{
		if (!this.selectedQuest.IsCompleted)
		{
			if (this.selectedQuest.ShowOverlay)
			{
				this.HideOverlay();
				return;
			}
			this.ShowOverlay();
		}
	}

	// Token: 0x06000A28 RID: 2600 RVA: 0x0002F020 File Offset: 0x0002D220
	public void HideOverlay()
	{
		this.selectedQuest.ShowOverlay = false;
		this.showOverlayText.text = LanguageManager.Instance.GetText("show").ToLower();
		this.uiSystemModule.QuestModule.CmdSetShowOverlay(this.selectedQuest.PlayerId, this.selectedQuest.QuestId, false);
	}

	// Token: 0x06000A29 RID: 2601 RVA: 0x0002F080 File Offset: 0x0002D280
	public void ShowOverlay()
	{
		this.selectedQuest.ShowOverlay = true;
		this.showOverlayText.text = LanguageManager.Instance.GetText("hide").ToLower();
		this.uiSystemModule.QuestModule.CmdSetShowOverlay(this.selectedQuest.PlayerId, this.selectedQuest.QuestId, true);
	}

	// Token: 0x06000A2A RID: 2602 RVA: 0x0002EDFC File Offset: 0x0002CFFC
	public void ClearSelectedQuest()
	{
		this.selectedQuest = default(PlayerQuest);
		this.displayText.text = string.Empty;
	}

	// Token: 0x06000A2B RID: 2603 RVA: 0x0002F0E0 File Offset: 0x0002D2E0
	public void FindNewQuest()
	{
		if (Time.time - this.findNewQuestTime < 10f)
		{
			this.findNewQuestTime = Time.time;
			this.uiSystemModule.EffectModule.ShowScreenMessage("cant_find_new_quest_right_now_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		this.uiSystemModule.QuestModule.CmdFindNewQuest(true);
		this.uiSystemModule.HideQuestWindow();
	}

	// Token: 0x06000A2C RID: 2604 RVA: 0x0002F148 File Offset: 0x0002D348
	public void TrackQuestObjectives()
	{
		if (Time.time - this.trackObjectivesTime < 10f)
		{
			this.trackObjectivesTime = Time.time;
			this.uiSystemModule.EffectModule.ShowScreenMessage("cant_track_objectives_right_now_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		this.uiSystemModule.QuestModule.CmdTrackQuestObjectives(this.selectedQuest.QuestId);
		this.uiSystemModule.HideQuestWindow();
	}

	// Token: 0x06000A2D RID: 2605 RVA: 0x0002F1BA File Offset: 0x0002D3BA
	public void StopTracking()
	{
		this.uiSystemModule.QuestModule.RemoveDestitionToTrack();
		this.uiSystemModule.HideQuestWindow();
	}

	// Token: 0x04000BA9 RID: 2985
	[SerializeField]
	private GameObject questSlotPrefab;

	// Token: 0x04000BAA RID: 2986
	[SerializeField]
	private GameObject questHolder;

	// Token: 0x04000BAB RID: 2987
	[SerializeField]
	private Button completeButton;

	// Token: 0x04000BAC RID: 2988
	[SerializeField]
	private Button showOverlayButton;

	// Token: 0x04000BAD RID: 2989
	[SerializeField]
	private Text showOverlayText;

	// Token: 0x04000BAE RID: 2990
	[SerializeField]
	private Text displayText;

	// Token: 0x04000BAF RID: 2991
	[SerializeField]
	private GameObject abortQuestWindow;

	// Token: 0x04000BB0 RID: 2992
	[SerializeField]
	private Toggle showCompletedToggle;

	// Token: 0x04000BB1 RID: 2993
	private float findNewQuestTime;

	// Token: 0x04000BB2 RID: 2994
	private float trackObjectivesTime;

	// Token: 0x04000BB3 RID: 2995
	private PlayerQuest selectedQuest;

	// Token: 0x04000BB4 RID: 2996
	private UISystemModule uiSystemModule;
}
