using System;
using UnityEngine;

// Token: 0x0200026D RID: 621
public class AbortQuestWindowManager : MonoBehaviour
{
	// Token: 0x06000970 RID: 2416 RVA: 0x0002C408 File Offset: 0x0002A608
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x06000971 RID: 2417 RVA: 0x0002C42C File Offset: 0x0002A62C
	public void SetQuest(PlayerQuest quest)
	{
		this.quest = quest;
	}

	// Token: 0x06000972 RID: 2418 RVA: 0x0002C435 File Offset: 0x0002A635
	public void OnYesButtonClicked()
	{
		this.uiSystemModule.QuestModule.CmdAbortQuest(this.quest.QuestId);
		this.uiSystemModule.QuestWindow.GetComponent<QuestWindowManager>().ClearSelectedQuest();
		this.HideWindow();
	}

	// Token: 0x06000973 RID: 2419 RVA: 0x0002C46D File Offset: 0x0002A66D
	public void OnNoButtonClicked()
	{
		this.HideWindow();
	}

	// Token: 0x06000974 RID: 2420 RVA: 0x000202BF File Offset: 0x0001E4BF
	private void HideWindow()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x04000B12 RID: 2834
	private PlayerQuest quest;

	// Token: 0x04000B13 RID: 2835
	private UISystemModule uiSystemModule;
}
