using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000289 RID: 649
public class QuestOverlayWindowManager : MonoBehaviour
{
	// Token: 0x06000A19 RID: 2585 RVA: 0x0002EAF0 File Offset: 0x0002CCF0
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x06000A1A RID: 2586 RVA: 0x0002EB14 File Offset: 0x0002CD14
	public void RefreshPlayerQuests()
	{
		if (this.uiSystemModule.QuestModule == null)
		{
			return;
		}
		List<PlayerQuest> list = (from pq in this.uiSystemModule.QuestModule.PlayerQuests
		where !pq.IsCompleted & pq.ShowOverlay
		select pq).ToList<PlayerQuest>();
		for (int i = 0; i < this.questHolder.transform.childCount; i++)
		{
			UnityEngine.Object.Destroy(this.questHolder.transform.GetChild(i).gameObject);
		}
		foreach (PlayerQuest playerQuest in list)
		{
			this.CreateQuestOverlaySlot(playerQuest);
		}
	}

	// Token: 0x06000A1B RID: 2587 RVA: 0x0002EBE8 File Offset: 0x0002CDE8
	private void CreateQuestOverlaySlot(PlayerQuest playerQuest)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.questOverlaySlotPrefab);
		gameObject.transform.SetParent(this.questHolder.transform, false);
		gameObject.transform.position = Vector2.zero;
		gameObject.GetComponent<QuestOverlaySlotManager>().SetPlayerQuest(playerQuest);
	}

	// Token: 0x04000BA4 RID: 2980
	[SerializeField]
	private GameObject questHolder;

	// Token: 0x04000BA5 RID: 2981
	[SerializeField]
	private GameObject questOverlaySlotPrefab;

	// Token: 0x04000BA6 RID: 2982
	private UISystemModule uiSystemModule;
}
