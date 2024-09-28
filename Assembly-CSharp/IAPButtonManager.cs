using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

// Token: 0x02000159 RID: 345
public class IAPButtonManager : MonoBehaviour
{
	// Token: 0x06000393 RID: 915 RVA: 0x00016370 File Offset: 0x00014570
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("GameManager");
		gameObject.TryGetComponent<IAPManager>(out this.iAPManager);
		gameObject.TryGetComponent<SteamIAPManager>(out this.steamIAPManager);
	}

	// Token: 0x06000394 RID: 916 RVA: 0x00016398 File Offset: 0x00014598
	private void Start()
	{
		if (this.totalValuePanel != null)
		{
			this.totalValuePanel.SetActive(GlobalSettings.IsMobilePlatform);
		}
		if (GlobalSettings.IsMobilePlatform & this.priceLabel != null)
		{
			Product product = this.iAPManager.StoreController.products.WithID(this.productId.ToLower());
			this.priceLabel.text = product.metadata.localizedPriceString;
		}
	}

	// Token: 0x06000395 RID: 917 RVA: 0x00016410 File Offset: 0x00014610
	public void Buy()
	{
		base.StartCoroutine(this.DeactivateButton());
		if (GlobalSettings.IsMobilePlatform)
		{
			this.iAPManager.BuyProductById(this.productId.ToLower());
			return;
		}
		if (SteamManager.Initialized)
		{
			base.StartCoroutine(this.steamIAPManager.BuyProductById(this.productId, this.productName));
			return;
		}
		Application.OpenURL(GlobalSettings.WebPage + "/account/webstore");
	}

	// Token: 0x06000396 RID: 918 RVA: 0x00016482 File Offset: 0x00014682
	private IEnumerator DeactivateButton()
	{
		this.buyButton.interactable = false;
		this.originalBuyButtonText = this.buyButtonLabel.text;
		this.buyButtonLabel.text = LanguageManager.Instance.GetText("wait").ToLower() + "...";
		yield return new WaitForSecondsRealtime(5f);
		this.buyButtonLabel.text = this.originalBuyButtonText;
		this.buyButton.interactable = true;
		yield break;
	}

	// Token: 0x0400071F RID: 1823
	[SerializeField]
	private GameObject totalValuePanel;

	// Token: 0x04000720 RID: 1824
	[SerializeField]
	private Text priceLabel;

	// Token: 0x04000721 RID: 1825
	[SerializeField]
	private string productId;

	// Token: 0x04000722 RID: 1826
	[SerializeField]
	private string productName;

	// Token: 0x04000723 RID: 1827
	[SerializeField]
	private Button buyButton;

	// Token: 0x04000724 RID: 1828
	[SerializeField]
	private Text buyButtonLabel;

	// Token: 0x04000725 RID: 1829
	private IAPManager iAPManager;

	// Token: 0x04000726 RID: 1830
	private SteamIAPManager steamIAPManager;

	// Token: 0x04000727 RID: 1831
	private string originalBuyButtonText;
}
