using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000236 RID: 566
public class QuestSlotManager : MonoBehaviour
{
	// Token: 0x060007F2 RID: 2034 RVA: 0x00026438 File Offset: 0x00024638
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x060007F3 RID: 2035 RVA: 0x0002645C File Offset: 0x0002465C
	private void Start()
	{
		this.questSlotToggle.group = base.GetComponentInParent<ToggleGroup>();
	}

	// Token: 0x060007F4 RID: 2036 RVA: 0x00026470 File Offset: 0x00024670
	public void SetPlayerQuest(PlayerQuest playerQuest)
	{
		this.playerQuest = playerQuest;
		if (this.playerQuest.IsDefined)
		{
			if (this.playerQuest.IsCompleted)
			{
				this.questNameText.color = GlobalSettings.Colors[1];
			}
			else if (this.uiSystemModule.QuestModule.CanCompleteQuest(this.playerQuest.QuestId))
			{
				this.questNameText.color = GlobalSettings.Colors[9];
			}
			this.questNameText.text = LanguageManager.Instance.GetText(playerQuest.Name);
		}
	}

	// Token: 0x060007F5 RID: 2037 RVA: 0x00026508 File Offset: 0x00024708
	public void OnToggleValueChanged()
	{
		QuestWindowManager component = this.uiSystemModule.QuestWindow.GetComponent<QuestWindowManager>();
		if (component != null)
		{
			component.SetSelectedQuest(this.playerQuest);
		}
	}

	// Token: 0x040009D0 RID: 2512
	private PlayerQuest playerQuest;

	// Token: 0x040009D1 RID: 2513
	[SerializeField]
	private Text questNameText;

	// Token: 0x040009D2 RID: 2514
	[SerializeField]
	private Toggle questSlotToggle;

	// Token: 0x040009D3 RID: 2515
	private UISystemModule uiSystemModule;
}
