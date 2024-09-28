using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000232 RID: 562
public class QuestButtonManager : MonoBehaviour
{
	// Token: 0x060007E3 RID: 2019 RVA: 0x00026083 File Offset: 0x00024283
	private void Start()
	{
		base.InvokeRepeating("CheckCompletedQuestsTimer", 0f, 2f);
	}

	// Token: 0x060007E4 RID: 2020 RVA: 0x0002609C File Offset: 0x0002429C
	private void CheckCompletedQuestsTimer()
	{
		QuestModule questModule = this.uiSystemModule.QuestModule;
		if (questModule != null)
		{
			bool enabled = false;
			for (int i = 0; i < questModule.PlayerQuests.Count; i++)
			{
				PlayerQuest playerQuest = questModule.PlayerQuests[i];
				if (questModule.CanCompleteQuest(playerQuest.QuestId))
				{
					enabled = true;
					break;
				}
			}
			this.redExclamationImage.enabled = enabled;
		}
	}

	// Token: 0x040009C0 RID: 2496
	[SerializeField]
	private Image redExclamationImage;

	// Token: 0x040009C1 RID: 2497
	[SerializeField]
	private UISystemModule uiSystemModule;
}
