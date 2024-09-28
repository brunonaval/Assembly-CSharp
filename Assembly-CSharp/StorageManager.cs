using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Token: 0x02000262 RID: 610
public class StorageManager : MonoBehaviour
{
	// Token: 0x0600093B RID: 2363 RVA: 0x0002B449 File Offset: 0x00029649
	private void OnEnable()
	{
		this.searchInput.text = string.Empty;
		base.StartCoroutine(this.LoadOrders());
	}

	// Token: 0x0600093C RID: 2364 RVA: 0x0002B468 File Offset: 0x00029668
	public void OnSearchButtonClick()
	{
		base.StartCoroutine(this.LoadOrders());
	}

	// Token: 0x0600093D RID: 2365 RVA: 0x0002B477 File Offset: 0x00029677
	private IEnumerator SetOrders(MarketStorage[] storageItems)
	{
		int childCount = this.storageItemHolder.transform.childCount;
		for (int j = 0; j < childCount; j++)
		{
			Transform child = this.storageItemHolder.transform.GetChild(j);
			if (child != null)
			{
				UnityEngine.Object.Destroy(child.gameObject);
			}
		}
		int num;
		for (int i = 0; i < storageItems.Length; i = num + 1)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.storageItemPrefab);
			gameObject.transform.SetParent(this.storageItemHolder.transform, false);
			gameObject.transform.position = Vector2.zero;
			StorageItemManager componentInChildren = gameObject.GetComponentInChildren<StorageItemManager>();
			if (componentInChildren != null)
			{
				componentInChildren.SetMarketOrder(storageItems[i]);
				yield return null;
			}
			num = i;
		}
		yield break;
	}

	// Token: 0x0600093E RID: 2366 RVA: 0x0002B48D File Offset: 0x0002968D
	public IEnumerator LoadOrders()
	{
		try
		{
			this.loadingOverlay.SetActive(true);
			string uri = string.Concat(new string[]
			{
				GlobalSettings.ApiHost,
				"api/",
				SettingsManager.Instance.ApiAccount.AccountUniqueId,
				"/tradingpost/storage",
				string.Format("?accountId={0}", SettingsManager.Instance.ApiAccount.AccountId),
				string.Format("&serverId={0}", this.uiSystemModule.PlayerModule.ServerId),
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
					MarketStorage[] orders = JsonUtility.FromJson<ApiResponse<ApiStorageItem[]>>(webRequest.downloadHandler.text).ResponseObject.Select(delegate(ApiStorageItem s)
					{
						MarketStorage result = new MarketStorage(s.StorageId, s.StorageAccountId, new Item(s.StorageItem, s.BlueprintRequiredProfession, s.BlueprintRequiredProfessionLevel), s.StorageAmount);
						result.Item.RequiredLevel = s.StorageRequiredLevel;
						result.Item.BoostLevel = s.StorageBoostLevel;
						result.Item.Rarity = s.StorageRarity;
						return result;
					}).ToArray<MarketStorage>();
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

	// Token: 0x04000AE6 RID: 2790
	[SerializeField]
	private GameObject loadingOverlay;

	// Token: 0x04000AE7 RID: 2791
	[SerializeField]
	private GameObject storageItemPrefab;

	// Token: 0x04000AE8 RID: 2792
	[SerializeField]
	private Transform storageItemHolder;

	// Token: 0x04000AE9 RID: 2793
	[SerializeField]
	private InputField searchInput;

	// Token: 0x04000AEA RID: 2794
	[SerializeField]
	private UISystemModule uiSystemModule;
}
