using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Token: 0x02000257 RID: 599
public class MyOffersManager : MonoBehaviour
{
	// Token: 0x060008E0 RID: 2272 RVA: 0x0002A004 File Offset: 0x00028204
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x060008E1 RID: 2273 RVA: 0x0002A028 File Offset: 0x00028228
	private void Update()
	{
		if (Time.time - this.lastUpdateTime > 0.5f)
		{
			this.lastUpdateTime = Time.time;
			this.UpdateGoldCoinsText();
		}
	}

	// Token: 0x060008E2 RID: 2274 RVA: 0x0002A04E File Offset: 0x0002824E
	private void OnEnable()
	{
		this.searchInput.text = string.Empty;
		base.StartCoroutine(this.LoadOrders());
	}

	// Token: 0x060008E3 RID: 2275 RVA: 0x0001D0D2 File Offset: 0x0001B2D2
	private void OnDisable()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x060008E4 RID: 2276 RVA: 0x0002A06D File Offset: 0x0002826D
	private void UpdateGoldCoinsText()
	{
		this.goldCoinsText.text = GlobalUtils.FormatLongNumber(this.uiSystemModule.WalletModule.GoldCoins, LanguageManager.Instance.GetText("api_culture"));
	}

	// Token: 0x060008E5 RID: 2277 RVA: 0x0002A09E File Offset: 0x0002829E
	public void OnSearchButtonClick()
	{
		base.StartCoroutine(this.LoadOrders());
	}

	// Token: 0x060008E6 RID: 2278 RVA: 0x0002A0AD File Offset: 0x000282AD
	private IEnumerator SetOrders(MarketOrder[] orders)
	{
		orders = (from o in orders
		orderby o.UnitValue
		select o).Take(30).ToArray<MarketOrder>();
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
			PlayerOrderManager componentInChildren = gameObject.GetComponentInChildren<PlayerOrderManager>();
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

	// Token: 0x060008E7 RID: 2279 RVA: 0x0002A0C3 File Offset: 0x000282C3
	public IEnumerator LoadOrders()
	{
		try
		{
			if (!base.isActiveAndEnabled)
			{
				yield break;
			}
			this.loadingOverlay.SetActive(true);
			string uri = string.Concat(new string[]
			{
				GlobalSettings.ApiHost,
				"api/",
				SettingsManager.Instance.ApiAccount.AccountUniqueId,
				"/tradingpost/playeroffers",
				string.Format("?accountId={0}", SettingsManager.Instance.ApiAccount.AccountId),
				string.Format("&playerId={0}", this.uiSystemModule.PlayerModule.PlayerId),
				"&search=",
				this.searchInput.text
			});
			using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
			{
				webRequest.timeout = 15;
				yield return webRequest.SendWebRequest();
				string[] array = uri.Split('/', StringSplitOptions.None);
				int num = array.Length - 1;
				if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
				{
					Debug.LogError(array[num] + ": Error: " + webRequest.error);
				}
				else
				{
					MarketOrder[] orders = JsonUtility.FromJson<ApiResponse<ApiMarketOrder[]>>(webRequest.downloadHandler.text).ResponseObject.Select(delegate(ApiMarketOrder o)
					{
						MarketOrder result = new MarketOrder(o.OrderId, new Item(o.Item, o.BlueprintRequiredProfession, o.BlueprintRequiredProfessionLevel), o.OrderAmount, o.OrderPlayerId, o.OrderUnitValue, o.Item.Name, o.OrderPlayerName);
						result.Item.RequiredLevel = o.OrderRequiredLevel;
						result.Item.BoostLevel = o.OrderBoostLevel;
						result.Item.Rarity = o.OrderRarity;
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

	// Token: 0x04000A9C RID: 2716
	[SerializeField]
	private GameObject loadingOverlay;

	// Token: 0x04000A9D RID: 2717
	[SerializeField]
	private GameObject orderPrefab;

	// Token: 0x04000A9E RID: 2718
	[SerializeField]
	private Transform orderHolder;

	// Token: 0x04000A9F RID: 2719
	[SerializeField]
	private InputField searchInput;

	// Token: 0x04000AA0 RID: 2720
	[SerializeField]
	private Text goldCoinsText;

	// Token: 0x04000AA1 RID: 2721
	private UISystemModule uiSystemModule;

	// Token: 0x04000AA2 RID: 2722
	private float lastUpdateTime;
}
