using System;
using System.Collections;
using Mirror;
using Steamworks;
using UnityEngine;

// Token: 0x0200015E RID: 350
public class SteamIAPManager : MonoBehaviour
{
	// Token: 0x060003B9 RID: 953 RVA: 0x00016DBE File Offset: 0x00014FBE
	private void Awake()
	{
		GameObject.FindGameObjectWithTag("UISystem").TryGetComponent<UISystemModule>(out this.uiSystemModule);
		this.m_MicroTxnAuthorizationResponse = Callback<MicroTxnAuthorizationResponse_t>.Create(new Callback<MicroTxnAuthorizationResponse_t>.DispatchDelegate(this.OnMicroTxnAuthorizationResponse));
	}

	// Token: 0x060003BA RID: 954 RVA: 0x000165A1 File Offset: 0x000147A1
	private void Start()
	{
		if (NetworkServer.active)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
	}

	// Token: 0x060003BB RID: 955 RVA: 0x00016DED File Offset: 0x00014FED
	private void OnMicroTxnAuthorizationResponse(MicroTxnAuthorizationResponse_t pCallback)
	{
		if (pCallback.m_bAuthorized == 1)
		{
			base.StartCoroutine(this.CompletePurchase(pCallback.m_ulOrderID.ToString()));
		}
	}

	// Token: 0x060003BC RID: 956 RVA: 0x00016E11 File Offset: 0x00015011
	public IEnumerator BuyProductById(string productId, string productName)
	{
		if (!SteamManager.Initialized)
		{
			yield break;
		}
		if (this.purchaseInProgress)
		{
			yield break;
		}
		StartSteamTransactionResource startSteamTransactionResource = new StartSteamTransactionResource
		{
			ItemDescription = LanguageManager.Instance.GetText(productName),
			PackageId = productId,
			PlayerId = this.uiSystemModule.PlayerModule.PlayerId,
			SteamUserId = SteamUser.GetSteamID().ToString()
		};
		Debug.Log(string.Format("PL: {0}|{1}|{2}|{3}", new object[]
		{
			startSteamTransactionResource.ItemDescription,
			startSteamTransactionResource.PackageId,
			startSteamTransactionResource.PlayerId,
			startSteamTransactionResource.SteamUserId
		}));
		yield return ApiManager.Post<object>("https://eternal-quest.online/api/payment/notify/startsteamtransaction", startSteamTransactionResource, delegate(ApiResponse<object> response)
		{
			this.purchaseInProgress = false;
			if (response.Success)
			{
				this.uiSystemModule.ShowTimedFeedback(LanguageManager.Instance.GetText("purchase_processing_message"), 3.5f, false);
				return;
			}
			this.uiSystemModule.ShowTimedFeedback(LanguageManager.Instance.GetText("purchase_failure_message"), 3.5f, true);
		});
		yield break;
	}

	// Token: 0x060003BD RID: 957 RVA: 0x00016E2E File Offset: 0x0001502E
	public IEnumerator CompletePurchase(string orderId)
	{
		CompleteSteamTransactionResource payload = new CompleteSteamTransactionResource
		{
			OrderId = orderId
		};
		yield return ApiManager.Post<object>("https://eternal-quest.online/api/payment/notify/completesteamtransaction", payload, delegate(ApiResponse<object> response)
		{
			this.purchaseInProgress = false;
			if (response.Success)
			{
				this.uiSystemModule.ShowTimedFeedback("<color=green>" + LanguageManager.Instance.GetText("purchase_success_message") + "</color>", 3.5f, false);
				this.uiSystemModule.PlayerModule.CmdSavePlayerData(true);
				return;
			}
			this.uiSystemModule.ShowTimedFeedback(LanguageManager.Instance.GetText("purchase_failure_message"), 3.5f, true);
		});
		yield break;
	}

	// Token: 0x04000740 RID: 1856
	private UISystemModule uiSystemModule;

	// Token: 0x04000741 RID: 1857
	private bool purchaseInProgress;

	// Token: 0x04000742 RID: 1858
	protected Callback<MicroTxnAuthorizationResponse_t> m_MicroTxnAuthorizationResponse;
}
