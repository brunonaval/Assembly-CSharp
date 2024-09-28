using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000237 RID: 567
public class QuestTrackerManager : MonoBehaviour
{
	// Token: 0x060007F7 RID: 2039 RVA: 0x0002653C File Offset: 0x0002473C
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x060007F8 RID: 2040 RVA: 0x00026560 File Offset: 0x00024760
	public void RefreshPlayerQuests()
	{
		if (this.uiSystemModule.QuestModule == null)
		{
			return;
		}
		List<PlayerQuest> list = (from pq in this.uiSystemModule.QuestModule.PlayerQuests
		where !pq.IsCompleted & pq.ShowOverlay
		select pq).ToList<PlayerQuest>();
		this.header.SetActive(list.Count != 0);
		this.content.SetActive(list.Count != 0);
		for (int i = 0; i < this.questGroup.transform.childCount; i++)
		{
			UnityEngine.Object.Destroy(this.questGroup.transform.GetChild(i).gameObject);
		}
		foreach (PlayerQuest quest in list)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.questPrefab);
			gameObject.transform.SetParent(this.questGroup.transform, false);
			gameObject.transform.position = Vector2.zero;
			gameObject.GetComponent<QuestManager>().SetQuest(quest);
		}
	}

	// Token: 0x040009D4 RID: 2516
	[SerializeField]
	private GameObject questGroup;

	// Token: 0x040009D5 RID: 2517
	[SerializeField]
	private GameObject questPrefab;

	// Token: 0x040009D6 RID: 2518
	[SerializeField]
	private GameObject header;

	// Token: 0x040009D7 RID: 2519
	[SerializeField]
	private GameObject content;

	// Token: 0x040009D8 RID: 2520
	private UISystemModule uiSystemModule;
}
