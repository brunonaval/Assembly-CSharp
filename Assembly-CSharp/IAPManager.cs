using System;
using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing.Security;

// Token: 0x0200015B RID: 347
public class IAPManager : MonoBehaviour, IDetailedStoreListener, IStoreListener
{
	// Token: 0x0600039E RID: 926 RVA: 0x00016558 File Offset: 0x00014758
	private void Awake()
	{
		if (!GlobalSettings.IsMobilePlatform)
		{
			return;
		}
		GameObject.FindGameObjectWithTag("UISystem").TryGetComponent<UISystemModule>(out this.uiSystemModule);
		ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(), Array.Empty<IPurchasingModule>());
		this.AddProductsToBuilder(builder);
		UnityPurchasing.Initialize(this, builder);
	}

	// Token: 0x0600039F RID: 927 RVA: 0x000165A1 File Offset: 0x000147A1
	private void Start()
	{
		if (NetworkServer.active)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
	}

	// Token: 0x060003A0 RID: 928 RVA: 0x000165B4 File Offset: 0x000147B4
	private void AddProductsToBuilder(ConfigurationBuilder builder)
	{
		builder.AddProduct(this.basicGemPack, ProductType.Consumable, new IDs
		{
			{
				this.basicGemPack,
				new string[]
				{
					"GooglePlay"
				}
			},
			{
				this.basicGemPack,
				new string[]
				{
					"AppleAppStore"
				}
			}
		});
		builder.AddProduct(this.masterworkGemPack, ProductType.Consumable, new IDs
		{
			{
				this.masterworkGemPack,
				new string[]
				{
					"GooglePlay"
				}
			},
			{
				this.masterworkGemPack,
				new string[]
				{
					"AppleAppStore"
				}
			}
		});
		builder.AddProduct(this.ascendedGemPack, ProductType.Consumable, new IDs
		{
			{
				this.ascendedGemPack,
				new string[]
				{
					"GooglePlay"
				}
			},
			{
				this.ascendedGemPack,
				new string[]
				{
					"AppleAppStore"
				}
			}
		});
		builder.AddProduct(this.epicGemPack, ProductType.Consumable, new IDs
		{
			{
				this.epicGemPack,
				new string[]
				{
					"GooglePlay"
				}
			},
			{
				this.epicGemPack,
				new string[]
				{
					"AppleAppStore"
				}
			}
		});
		builder.AddProduct(this.premium030, ProductType.Consumable, new IDs
		{
			{
				this.premium030,
				new string[]
				{
					"GooglePlay"
				}
			},
			{
				this.premium030,
				new string[]
				{
					"AppleAppStore"
				}
			}
		});
		builder.AddProduct(this.premium090, ProductType.Consumable, new IDs
		{
			{
				this.premium090,
				new string[]
				{
					"GooglePlay"
				}
			},
			{
				this.premium090,
				new string[]
				{
					"AppleAppStore"
				}
			}
		});
		builder.AddProduct(this.promoVetBas, ProductType.Consumable, new IDs
		{
			{
				this.promoVetBas,
				new string[]
				{
					"GooglePlay"
				}
			},
			{
				this.promoVetBas,
				new string[]
				{
					"AppleAppStore"
				}
			}
		});
		builder.AddProduct(this.promoEliBas, ProductType.Consumable, new IDs
		{
			{
				this.promoEliBas,
				new string[]
				{
					"GooglePlay"
				}
			},
			{
				this.promoEliBas,
				new string[]
				{
					"AppleAppStore"
				}
			}
		});
		builder.AddProduct(this.promoLegBas, ProductType.Consumable, new IDs
		{
			{
				this.promoLegBas,
				new string[]
				{
					"GooglePlay"
				}
			},
			{
				this.promoLegBas,
				new string[]
				{
					"AppleAppStore"
				}
			}
		});
	}

	// Token: 0x060003A1 RID: 929 RVA: 0x00016854 File Offset: 0x00014A54
	public void BuyProductById(string productId)
	{
		if (!this.isInitialized)
		{
			return;
		}
		if (this.purchaseInProgress)
		{
			return;
		}
		Product product = this.StoreController.products.WithID(productId);
		if (product == null)
		{
			return;
		}
		this.purchaseInProgress = true;
		Debug.Log("Purchasing product: " + product.definition.id);
		this.StoreController.InitiatePurchase(product);
	}

	// Token: 0x060003A2 RID: 930 RVA: 0x000168B6 File Offset: 0x00014AB6
	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		this.isInitialized = true;
		this.StoreController = controller;
		Debug.Log("IAP initialized.");
	}

	// Token: 0x060003A3 RID: 931 RVA: 0x000168D0 File Offset: 0x00014AD0
	public void OnInitializeFailed(InitializationFailureReason error)
	{
		Debug.Log(string.Format("[OnInitializeFailed]: {0}", error));
		this.uiSystemModule.ShowTimedFeedback(string.Format("[OnInitializeFailed]: {0}", error), 3.5f, true);
	}

	// Token: 0x060003A4 RID: 932 RVA: 0x00016908 File Offset: 0x00014B08
	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		this.purchaseInProgress = false;
		Debug.Log(string.Format("[OnPurchaseFailed]: {0} / {1}", product.definition.id, failureReason));
		this.uiSystemModule.ShowTimedFeedback(string.Format("[OnPurchaseFailed]: {0} / {1}", product.definition.id, failureReason), 3.5f, true);
	}

	// Token: 0x060003A5 RID: 933 RVA: 0x00016968 File Offset: 0x00014B68
	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
	{
		CrossPlatformValidator crossPlatformValidator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);
		try
		{
			crossPlatformValidator.Validate(purchaseEvent.purchasedProduct.receipt);
			if (Application.platform == RuntimePlatform.Android)
			{
				this.uiSystemModule.ShowTimedFeedback(LanguageManager.Instance.GetText("purchase_processing_message"), 3.5f, false);
				base.StartCoroutine(this.GooglePlayNotify(purchaseEvent.purchasedProduct));
			}
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				this.uiSystemModule.ShowTimedFeedback(LanguageManager.Instance.GetText("purchase_processing_message"), 3.5f, false);
				base.StartCoroutine(this.AppStoreNotify(purchaseEvent.purchasedProduct));
			}
		}
		catch (IAPSecurityException)
		{
			this.uiSystemModule.ShowTimedFeedback(LanguageManager.Instance.GetText("purchase_failure_message"), 3.5f, true);
		}
		return PurchaseProcessingResult.Complete;
	}

	// Token: 0x060003A6 RID: 934 RVA: 0x00016A4C File Offset: 0x00014C4C
	private IEnumerator AppStoreNotify(Product pendingPurchasedProduct)
	{
		AppStoreOrder payload = new AppStoreOrder
		{
			PlayerId = this.uiSystemModule.PlayerModule.PlayerId,
			Receipt = pendingPurchasedProduct.receipt
		};
		yield return ApiManager.Post<object>("https://eternal-quest.online/api/payment/notify/appstore", payload, delegate(ApiResponse<object> response)
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

	// Token: 0x060003A7 RID: 935 RVA: 0x00016A62 File Offset: 0x00014C62
	private IEnumerator GooglePlayNotify(Product pendingPurchasedProduct)
	{
		GooglePlayOrder payload = new GooglePlayOrder
		{
			PlayerId = this.uiSystemModule.PlayerModule.PlayerId,
			Receipt = pendingPurchasedProduct.receipt
		};
		yield return ApiManager.Post<object>("https://eternal-quest.online/api/payment/notify/googleplay", payload, delegate(ApiResponse<object> response)
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

	// Token: 0x060003A8 RID: 936 RVA: 0x00016A78 File Offset: 0x00014C78
	public void OnInitializeFailed(InitializationFailureReason error, string message)
	{
		Debug.Log(string.Format("[OnInitializeFailed]: {0}. Message: {1}", error, message));
		this.uiSystemModule.ShowTimedFeedback(string.Format("[OnInitializeFailed]: {0}. Message: {1}", error, message), 3.5f, true);
	}

	// Token: 0x060003A9 RID: 937 RVA: 0x00016AB4 File Offset: 0x00014CB4
	public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
	{
		this.purchaseInProgress = false;
		Debug.Log(string.Format("[OnPurchaseFailed]: {0} / {1}", product.definition.id, failureDescription));
		this.uiSystemModule.ShowTimedFeedback(string.Format("[OnPurchaseFailed]: {0} / {1}", product.definition.id, failureDescription), 3.5f, true);
	}

	// Token: 0x0400072B RID: 1835
	private readonly string basicGemPack = "gembas";

	// Token: 0x0400072C RID: 1836
	private readonly string masterworkGemPack = "gemmas";

	// Token: 0x0400072D RID: 1837
	private readonly string ascendedGemPack = "gemasc";

	// Token: 0x0400072E RID: 1838
	private readonly string epicGemPack = "gemepi";

	// Token: 0x0400072F RID: 1839
	private readonly string premium030 = "pre030";

	// Token: 0x04000730 RID: 1840
	private readonly string premium090 = "pre090";

	// Token: 0x04000731 RID: 1841
	private readonly string promoVetBas = "promovetbas";

	// Token: 0x04000732 RID: 1842
	private readonly string promoEliBas = "promoelibas";

	// Token: 0x04000733 RID: 1843
	private readonly string promoLegBas = "promolegbas";

	// Token: 0x04000734 RID: 1844
	private bool isInitialized;

	// Token: 0x04000735 RID: 1845
	private bool purchaseInProgress;

	// Token: 0x04000736 RID: 1846
	public IStoreController StoreController;

	// Token: 0x04000737 RID: 1847
	private UISystemModule uiSystemModule;
}
