using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000275 RID: 629
public class DialogWindowManager : MonoBehaviour
{
	// Token: 0x0600099A RID: 2458 RVA: 0x0002CC74 File Offset: 0x0002AE74
	public void SetDialog(NpcDialog npcDialog)
	{
		if (npcDialog.Quest.IsDefined)
		{
			this.displayText.text = npcDialog.Quest.FullDescription;
		}
		else
		{
			this.displayText.text = LanguageManager.Instance.GetText(npcDialog.Display);
		}
		this.npcNameText.text = LanguageManager.Instance.GetText(npcDialog.Name).ToLower();
		for (int i = 0; i < this.dialogHolder.childCount; i++)
		{
			Transform child = this.dialogHolder.GetChild(i);
			if (child != null && (child.CompareTag("DialogChoice") || child.CompareTag("UIStoreItemSlot")))
			{
				UnityEngine.Object.Destroy(child.gameObject);
			}
		}
		if (npcDialog.StoreAction == StoreAction.Buy)
		{
			foreach (StoreItemConfig storeItem in npcDialog.StoreItems)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.storeItemPrefab);
				gameObject.transform.SetParent(this.dialogHolder.transform, false);
				gameObject.transform.position = Vector2.zero;
				gameObject.GetComponentInChildren<StoreItemSlotManager>().SetStoreItem(storeItem, npcDialog.StoreAction);
			}
		}
		if (npcDialog.StoreAction == StoreAction.Repurchase)
		{
			foreach (StoreItemConfig storeItem2 in npcDialog.StoreItems)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.storeItemPrefab);
				gameObject2.transform.SetParent(this.dialogHolder.transform, false);
				gameObject2.transform.position = Vector2.zero;
				gameObject2.GetComponentInChildren<StoreItemSlotManager>().SetStoreItem(storeItem2, npcDialog.StoreAction);
			}
		}
		if (npcDialog.StoreAction == StoreAction.Sell)
		{
			foreach (StoreItemConfig storeItem3 in npcDialog.StoreItems)
			{
				GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(this.storeItemPrefab);
				gameObject3.transform.SetParent(this.dialogHolder.transform, false);
				gameObject3.transform.position = Vector2.zero;
				gameObject3.GetComponentInChildren<StoreItemSlotManager>().SetStoreItem(storeItem3, npcDialog.StoreAction);
			}
		}
		foreach (NpcChoice choice in from o in npcDialog.Choices
		orderby o.ActionId
		select o)
		{
			GameObject gameObject4 = UnityEngine.Object.Instantiate<GameObject>(this.choiceButtonPrefab);
			gameObject4.transform.SetParent(this.dialogHolder.transform, false);
			gameObject4.transform.position = Vector2.zero;
			gameObject4.GetComponent<ChoiceButtonManager>().SetChoice(choice);
		}
		this.dialogHolder.GetComponent<RectTransform>().localPosition = Vector2.zero;
		this.ScrollUp();
	}

	// Token: 0x0600099B RID: 2459 RVA: 0x0002CF4C File Offset: 0x0002B14C
	private void ScrollUp()
	{
		base.StartCoroutine(this.ScrollUpAsync());
	}

	// Token: 0x0600099C RID: 2460 RVA: 0x0002CF5B File Offset: 0x0002B15B
	private IEnumerator ScrollUpAsync()
	{
		yield return new WaitForEndOfFrame();
		this.dialogScrollRect.verticalNormalizedPosition = 1f;
		yield break;
	}

	// Token: 0x0600099D RID: 2461 RVA: 0x0002CF6C File Offset: 0x0002B16C
	public void ReloadStoreItems()
	{
		for (int i = 0; i < this.dialogHolder.childCount; i++)
		{
			Transform child = this.dialogHolder.GetChild(i);
			if (child != null && child.CompareTag("UIStoreItemSlot"))
			{
				child.GetComponentInChildren<StoreItemSlotManager>().ReloadStoreItem();
			}
		}
	}

	// Token: 0x04000B32 RID: 2866
	[SerializeField]
	private GameObject choiceButtonPrefab;

	// Token: 0x04000B33 RID: 2867
	[SerializeField]
	private GameObject storeItemPrefab;

	// Token: 0x04000B34 RID: 2868
	[SerializeField]
	private Text displayText;

	// Token: 0x04000B35 RID: 2869
	[SerializeField]
	private Text npcNameText;

	// Token: 0x04000B36 RID: 2870
	[SerializeField]
	private Transform dialogHolder;

	// Token: 0x04000B37 RID: 2871
	[SerializeField]
	private ScrollRect dialogScrollRect;
}
