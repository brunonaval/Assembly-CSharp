using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Token: 0x02000253 RID: 595
public class GemStoreManager : MonoBehaviour
{
	// Token: 0x060008C7 RID: 2247 RVA: 0x00029AFE File Offset: 0x00027CFE
	private void OnEnable()
	{
		this.searchInput.text = string.Empty;
		base.StartCoroutine(this.LoadOrders());
	}

	// Token: 0x060008C8 RID: 2248 RVA: 0x00029B20 File Offset: 0x00027D20
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x060008C9 RID: 2249 RVA: 0x00029B44 File Offset: 0x00027D44
	private void Update()
	{
		if (Time.time - this.lastUpdateTime > 0.5f)
		{
			this.lastUpdateTime = Time.time;
			this.UpdateGemText();
		}
	}

	// Token: 0x060008CA RID: 2250 RVA: 0x00029B6A File Offset: 0x00027D6A
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
			GemOrderManager componentInChildren = gameObject.GetComponentInChildren<GemOrderManager>();
			if (componentInChildren != null)
			{
				componentInChildren.SetMarketOrder(orders[i]);
				yield return null;
			}
			num = i;
		}
		this.UpdateGemText();
		yield break;
	}

	// Token: 0x060008CB RID: 2251 RVA: 0x00029B80 File Offset: 0x00027D80
	private void UpdateGemText()
	{
		this.gemText.text = this.uiSystemModule.WalletModule.Gems.ToString();
	}

	// Token: 0x060008CC RID: 2252 RVA: 0x00029BA2 File Offset: 0x00027DA2
	public void OnSearchButtonClick()
	{
		base.StartCoroutine(this.LoadOrders());
	}

	// Token: 0x060008CD RID: 2253 RVA: 0x00029BB1 File Offset: 0x00027DB1
	private IEnumerator LoadOrders()
	{
		try
		{
			this.loadingOverlay.SetActive(true);
			string uri = string.Concat(new string[]
			{
				GlobalSettings.ApiHost,
				"api/",
				SettingsManager.Instance.ApiAccount.AccountUniqueId,
				"/tradingpost/gemstore",
				string.Format("?accountId={0}", SettingsManager.Instance.ApiAccount.AccountId),
				string.Format("&itemType={0}", (int)this.itemTypeFilter),
				"&search=",
				this.searchInput.text
			});
			using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
			{
				webRequest.timeout = 15;
				yield return webRequest.SendWebRequest();
				string[] array = uri.Split('/', StringSplitOptions.None);
				int num = array.Length - 1;
				if (webRequest.result == UnityWebRequest.Result.ConnectionError)
				{
					Debug.LogError(array[num] + ": Error: " + webRequest.error);
				}
				else
				{
					MarketOrder[] orders = (from o in JsonUtility.FromJson<ApiResponse<ApiGemOrder[]>>(webRequest.downloadHandler.text).ResponseObject
					select new MarketOrder(o.OrderId, new Item(o.Item, o.BlueprintRequiredProfession, o.BlueprintRequiredProfessionLevel), o.OrderAmount, o.OrderUnitValue, o.Item.Name)).ToArray<MarketOrder>();
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

	// Token: 0x04000A87 RID: 2695
	[SerializeField]
	private GameObject loadingOverlay;

	// Token: 0x04000A88 RID: 2696
	[SerializeField]
	private GameObject orderPrefab;

	// Token: 0x04000A89 RID: 2697
	[SerializeField]
	private Transform orderHolder;

	// Token: 0x04000A8A RID: 2698
	[SerializeField]
	private InputField searchInput;

	// Token: 0x04000A8B RID: 2699
	[SerializeField]
	private Text gemText;

	// Token: 0x04000A8C RID: 2700
	[SerializeField]
	private GameObject buyGemsButton;

	// Token: 0x04000A8D RID: 2701
	[SerializeField]
	private ItemType itemTypeFilter;

	// Token: 0x04000A8E RID: 2702
	private UISystemModule uiSystemModule;

	// Token: 0x04000A8F RID: 2703
	private float lastUpdateTime;
}
