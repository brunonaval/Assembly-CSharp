using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DuloGames.UI;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Token: 0x0200024C RID: 588
public class BuyMarketManager : MonoBehaviour
{
	// Token: 0x0600088C RID: 2188 RVA: 0x00028C38 File Offset: 0x00026E38
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x0600088D RID: 2189 RVA: 0x00028C5C File Offset: 0x00026E5C
	private void Update()
	{
		if (Time.time - this.lastUpdateTime > 0.5f)
		{
			this.lastUpdateTime = Time.time;
			this.UpdateGoldCoinsText();
		}
	}

	// Token: 0x0600088E RID: 2190 RVA: 0x00028C84 File Offset: 0x00026E84
	private void OnEnable()
	{
		this.searchInput.text = string.Empty;
		this.minLevelInput.text = string.Empty;
		this.maxLevelInput.text = string.Empty;
		this.itemTypeInput.gameObject.SetActive(GlobalSettings.IsMobilePlatform);
		this.itemTypeSelectField.gameObject.SetActive(!GlobalSettings.IsMobilePlatform);
		this.itemTypeSelectField.SelectOptionByIndex(0);
		base.StartCoroutine(this.LoadOrders());
		this.LoadItemTypesIntoSelectField();
	}

	// Token: 0x0600088F RID: 2191 RVA: 0x0001D0D2 File Offset: 0x0001B2D2
	private void OnDisable()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x06000890 RID: 2192 RVA: 0x00028D0D File Offset: 0x00026F0D
	private void UpdateGoldCoinsText()
	{
		this.goldCoinsText.text = GlobalUtils.FormatLongNumber(this.uiSystemModule.WalletModule.GoldCoins, LanguageManager.Instance.GetText("api_culture"));
	}

	// Token: 0x06000891 RID: 2193 RVA: 0x00028D40 File Offset: 0x00026F40
	private int ParseItemType()
	{
		BuyMarketManager.<>c__DisplayClass16_0 CS$<>8__locals1 = new BuyMarketManager.<>c__DisplayClass16_0();
		List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>();
		foreach (string text in Enum.GetNames(typeof(ItemType)))
		{
			ItemType value;
			if (Enum.TryParse<ItemType>(text, true, out value))
			{
				list.Add(new KeyValuePair<string, int>(LanguageManager.Instance.GetText(GlobalUtils.ItemTypeNameToMeta(text)), (int)value));
			}
		}
		BuyMarketManager.<>c__DisplayClass16_0 CS$<>8__locals2 = CS$<>8__locals1;
		string text2 = this.itemTypeInput.text;
		CS$<>8__locals2.itemTypeInputText = ((text2 != null) ? text2.ToLower() : null);
		return (from itn in list
		where itn.Key.ToLower().Contains(CS$<>8__locals1.itemTypeInputText)
		select itn into it
		select it.Value).FirstOrDefault<int>();
	}

	// Token: 0x06000892 RID: 2194 RVA: 0x00028DFE File Offset: 0x00026FFE
	private IEnumerator SetOrders(MarketOrder[] orders)
	{
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
			BuyOrderManager componentInChildren = gameObject.GetComponentInChildren<BuyOrderManager>();
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

	// Token: 0x06000893 RID: 2195 RVA: 0x00028E14 File Offset: 0x00027014
	public void OnSearchButtonClick()
	{
		base.StartCoroutine(this.LoadOrders());
	}

	// Token: 0x06000894 RID: 2196 RVA: 0x00028E24 File Offset: 0x00027024
	private void LoadItemTypesIntoSelectField()
	{
		this.itemTypeSelectField.ClearOptions();
		foreach (string itemTypeName in Enum.GetNames(typeof(ItemType)))
		{
			this.itemTypeSelectField.AddOption(LanguageManager.Instance.GetText(GlobalUtils.ItemTypeNameToMeta(itemTypeName)));
		}
	}

	// Token: 0x06000895 RID: 2197 RVA: 0x00028E79 File Offset: 0x00027079
	public IEnumerator LoadOrders()
	{
		try
		{
			this.loadingOverlay.SetActive(true);
			int num = GlobalSettings.IsMobilePlatform ? this.ParseItemType() : Mathf.Max(0, this.itemTypeSelectField.selectedOptionIndex);
			string uri = string.Concat(new string[]
			{
				GlobalSettings.ApiHost,
				"api/",
				SettingsManager.Instance.ApiAccount.AccountUniqueId,
				"/tradingpost/buymarket",
				string.Format("?accountId={0}", SettingsManager.Instance.ApiAccount.AccountId),
				string.Format("&serverId={0}", SettingsManager.Instance.SelectedPlayer.ServerId),
				"&minLevel=",
				string.IsNullOrEmpty(this.minLevelInput.text) ? "0" : this.minLevelInput.text,
				"&maxLevel=",
				string.IsNullOrEmpty(this.maxLevelInput.text) ? "9999" : this.maxLevelInput.text,
				string.Format("&itemType={0}", num),
				"&search=",
				this.searchInput.text
			});
			using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
			{
				webRequest.timeout = 15;
				yield return webRequest.SendWebRequest();
				string[] array = uri.Split('/', StringSplitOptions.None);
				int num2 = array.Length - 1;
				if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
				{
					Debug.LogError(array[num2] + ": Error: " + webRequest.error);
				}
				else
				{
					MarketOrder[] orders = JsonUtility.FromJson<ApiResponse<ApiMarketOrder[]>>(webRequest.downloadHandler.text).ResponseObject.Select(delegate(ApiMarketOrder o)
					{
						MarketOrder result = new MarketOrder(o.OrderId, new Item(o.Item, o.BlueprintRequiredProfession, o.BlueprintRequiredProfessionLevel), o.OrderAmount, o.OrderPlayerId, o.OrderUnitValue, o.Item.Name, o.OrderPlayerName);
						result.Item.RequiredLevel = o.OrderRequiredLevel;
						result.Item.Rarity = o.OrderRarity;
						result.Item.BoostLevel = o.OrderBoostLevel;
						return result;
					}).ToArray<MarketOrder>();
					yield return this.SetOrders(orders);
				}
			}
			UnityWebRequest webRequest = null;
			uri = null;
		}
		finally
		{
			this.loadingOverlay.SetActive(false);
		}
		yield break;
		yield break;
	}

	// Token: 0x04000A57 RID: 2647
	[SerializeField]
	private GameObject loadingOverlay;

	// Token: 0x04000A58 RID: 2648
	[SerializeField]
	private GameObject orderPrefab;

	// Token: 0x04000A59 RID: 2649
	[SerializeField]
	private Transform orderHolder;

	// Token: 0x04000A5A RID: 2650
	[SerializeField]
	private InputField searchInput;

	// Token: 0x04000A5B RID: 2651
	[SerializeField]
	private InputField minLevelInput;

	// Token: 0x04000A5C RID: 2652
	[SerializeField]
	private InputField maxLevelInput;

	// Token: 0x04000A5D RID: 2653
	[SerializeField]
	private UISelectField itemTypeSelectField;

	// Token: 0x04000A5E RID: 2654
	[SerializeField]
	private InputField itemTypeInput;

	// Token: 0x04000A5F RID: 2655
	[SerializeField]
	private Text goldCoinsText;

	// Token: 0x04000A60 RID: 2656
	private UISystemModule uiSystemModule;

	// Token: 0x04000A61 RID: 2657
	private float lastUpdateTime;
}
