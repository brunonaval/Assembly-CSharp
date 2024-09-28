using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000235 RID: 565
public class QuestOverlaySlotManager : MonoBehaviour
{
	// Token: 0x060007ED RID: 2029 RVA: 0x000262F4 File Offset: 0x000244F4
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x060007EE RID: 2030 RVA: 0x00026318 File Offset: 0x00024518
	public void SetPlayerQuest(PlayerQuest playerQuest)
	{
		this.selectedQuest = playerQuest;
		if (!playerQuest.IsDefined)
		{
			return;
		}
		if (this.uiSystemModule.QuestModule.CanCompleteQuest(playerQuest.QuestId))
		{
			this.completeButton.interactable = true;
			this.questNameText.color = GlobalSettings.Colors[9];
		}
		this.questNameText.text = LanguageManager.Instance.GetText(playerQuest.Name);
		this.questDescriptionText.text = playerQuest.OverlayDescription;
	}

	// Token: 0x060007EF RID: 2031 RVA: 0x0002639E File Offset: 0x0002459E
	public void HideOverlay()
	{
		this.selectedQuest.ShowOverlay = false;
		this.uiSystemModule.QuestModule.CmdSetShowOverlay(this.selectedQuest.PlayerId, this.selectedQuest.QuestId, false);
	}

	// Token: 0x060007F0 RID: 2032 RVA: 0x000263D4 File Offset: 0x000245D4
	public void CompleteQuest()
	{
		if (!this.selectedQuest.IsCompleted)
		{
			this.uiSystemModule.QuestModule.CompleteQuest(this.selectedQuest.PlayerId, this.selectedQuest.QuestId);
			return;
		}
		this.uiSystemModule.EffectModule.ShowScreenMessage("quest_already_completed_message", 0, 3.5f, Array.Empty<string>());
	}

	// Token: 0x040009CB RID: 2507
	[SerializeField]
	private Text questNameText;

	// Token: 0x040009CC RID: 2508
	[SerializeField]
	private Text questDescriptionText;

	// Token: 0x040009CD RID: 2509
	[SerializeField]
	private Button completeButton;

	// Token: 0x040009CE RID: 2510
	private PlayerQuest selectedQuest;

	// Token: 0x040009CF RID: 2511
	private UISystemModule uiSystemModule;
}
