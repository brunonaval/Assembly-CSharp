using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200025C RID: 604
public class SellMarketManager : MonoBehaviour
{
	// Token: 0x06000905 RID: 2309 RVA: 0x0002A840 File Offset: 0x00028A40
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x06000906 RID: 2310 RVA: 0x0002A864 File Offset: 0x00028A64
	private void Update()
	{
		if (Time.time - this.lastUpdateTime > 0.5f)
		{
			this.lastUpdateTime = Time.time;
			this.UpdateGoldCoinsText();
		}
	}

	// Token: 0x06000907 RID: 2311 RVA: 0x0002A88A File Offset: 0x00028A8A
	private void OnEnable()
	{
		this.searchInput.text = string.Empty;
		base.StartCoroutine(this.LoadOrders());
	}

	// Token: 0x06000908 RID: 2312 RVA: 0x0001D0D2 File Offset: 0x0001B2D2
	private void OnDisable()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x06000909 RID: 2313 RVA: 0x0002A8A9 File Offset: 0x00028AA9
	private void UpdateGoldCoinsText()
	{
		this.goldCoinsText.text = GlobalUtils.FormatLongNumber(this.uiSystemModule.WalletModule.GoldCoins, LanguageManager.Instance.GetText("api_culture"));
	}

	// Token: 0x0600090A RID: 2314 RVA: 0x0002A8DA File Offset: 0x00028ADA
	private IEnumerator SetOrders(MarketOrder[] orders)
	{
		orders = (from o in orders
		orderby o.UnitValue
		select o).Take(50).ToArray<MarketOrder>();
		int childCount = this.orderHolder.transform.childCount;
		for (int j = 0; j < childCount; j++)
		{
			Transform child = this.orderHolder.transform.GetChild(j);
			if (child != null)
			{
				UnityEngine.Object.Destroy(child.gameObject);
			}
		}
		int num;
		for (int i = 0; i < orders.Length; i = num + 1)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.orderPrefab);
			gameObject.transform.SetParent(this.orderHolder.transform, false);
			gameObject.transform.position = Vector2.zero;
			SellOrderManager componentInChildren = gameObject.GetComponentInChildren<SellOrderManager>();
			if (componentInChildren != null)
			{
				componentInChildren.SetOrder(orders[i]);
				yield return null;
			}
			num = i;
		}
		this.UpdateGoldCoinsText();
		yield break;
	}

	// Token: 0x0600090B RID: 2315 RVA: 0x0002A8F0 File Offset: 0x00028AF0
	public void OnSearchButtonClick()
	{
		base.StartCoroutine(this.LoadOrders());
	}

	// Token: 0x0600090C RID: 2316 RVA: 0x0002A8FF File Offset: 0x00028AFF
	public IEnumerator LoadOrders()
	{
		try
		{
			this.loadingOverlay.SetActive(true);
			List<MarketOrder> orders = new List<MarketOrder>();
			Item[] sellableItems = this.uiSystemModule.InventoryModule.GetMarketSellableItems(this.searchInput.text);
			int num;
			for (int i = 0; i < sellableItems.Length; i = num + 1)
			{
				Item item = sellableItems[i];
				if (item.Sellable & !item.Soulbind & item.Amount > 0)
				{
					MarketOrder item2 = new MarketOrder(0, item, item.Amount, this.uiSystemModule.PlayerModule.PlayerId, item.Value, item.Name, string.Empty);
					orders.Add(item2);
					yield return null;
				}
				num = i;
			}
			yield return this.SetOrders(orders.ToArray());
			orders = null;
			sellableItems = null;
		}
		finally
		{
			this.loadingOverlay.SetActive(false);
		}
		yield break;
		yield break;
	}

	// Token: 0x04000ABB RID: 2747
	[SerializeField]
	private GameObject loadingOverlay;

	// Token: 0x04000ABC RID: 2748
	[SerializeField]
	private GameObject orderPrefab;

	// Token: 0x04000ABD RID: 2749
	[SerializeField]
	private Transform orderHolder;

	// Token: 0x04000ABE RID: 2750
	[SerializeField]
	private Text goldCoinsText;

	// Token: 0x04000ABF RID: 2751
	[SerializeField]
	private InputField searchInput;

	// Token: 0x04000AC0 RID: 2752
	private UISystemModule uiSystemModule;

	// Token: 0x04000AC1 RID: 2753
	private float lastUpdateTime;
}
