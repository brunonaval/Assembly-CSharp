using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200023A RID: 570
public class RateGameWindowManager : MonoBehaviour
{
	// Token: 0x06000805 RID: 2053 RVA: 0x000269E7 File Offset: 0x00024BE7
	private void Awake()
	{
		GameObject.FindGameObjectWithTag("UISystem").TryGetComponent<UISystemModule>(out this.uiSystemModule);
	}

	// Token: 0x06000806 RID: 2054 RVA: 0x00026A00 File Offset: 0x00024C00
	public void SelectOneStar()
	{
		this.totalStars = 1;
		this.rateButton.interactable = true;
		this.oneStarImage.enabled = true;
		this.twoStarImage.enabled = false;
		this.threeStarImage.enabled = false;
		this.fourStarImage.enabled = false;
		this.fiveStarImage.enabled = false;
	}

	// Token: 0x06000807 RID: 2055 RVA: 0x00026A5C File Offset: 0x00024C5C
	public void SelectTwoStars()
	{
		this.totalStars = 2;
		this.rateButton.interactable = true;
		this.oneStarImage.enabled = true;
		this.twoStarImage.enabled = true;
		this.threeStarImage.enabled = false;
		this.fourStarImage.enabled = false;
		this.fiveStarImage.enabled = false;
	}

	// Token: 0x06000808 RID: 2056 RVA: 0x00026AB8 File Offset: 0x00024CB8
	public void SelectThreeStars()
	{
		this.totalStars = 3;
		this.rateButton.interactable = true;
		this.oneStarImage.enabled = true;
		this.twoStarImage.enabled = true;
		this.threeStarImage.enabled = true;
		this.fourStarImage.enabled = false;
		this.fiveStarImage.enabled = false;
	}

	// Token: 0x06000809 RID: 2057 RVA: 0x00026B14 File Offset: 0x00024D14
	public void SelectFourStars()
	{
		this.totalStars = 4;
		this.rateButton.interactable = true;
		this.oneStarImage.enabled = true;
		this.twoStarImage.enabled = true;
		this.threeStarImage.enabled = true;
		this.fourStarImage.enabled = true;
		this.fiveStarImage.enabled = false;
	}

	// Token: 0x0600080A RID: 2058 RVA: 0x00026B70 File Offset: 0x00024D70
	public void SelectFiveStars()
	{
		this.totalStars = 5;
		this.rateButton.interactable = true;
		this.oneStarImage.enabled = true;
		this.twoStarImage.enabled = true;
		this.threeStarImage.enabled = true;
		this.fourStarImage.enabled = true;
		this.fiveStarImage.enabled = true;
	}

	// Token: 0x0600080B RID: 2059 RVA: 0x00026BCC File Offset: 0x00024DCC
	public void RateGame()
	{
		this.uiSystemModule.EffectModule.ShowScreenMessage("rate_game_window_thanks_message", 2, 3.5f, Array.Empty<string>());
		PlayerPrefs.SetInt("GameRating", this.totalStars);
		this.uiSystemModule.PlayerModule.CmdRateGame(this.totalStars);
		if (this.totalStars < 4)
		{
			base.GetComponentInParent<DragWindowModule>().CloseWindow();
			return;
		}
		if (Application.platform == RuntimePlatform.Android)
		{
			Application.OpenURL("market://details?id=com.xtreaming.eternalquest");
			base.GetComponentInParent<DragWindowModule>().CloseWindow();
			return;
		}
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Application.OpenURL("itms-apps://itunes.apple.com/app/id1556838458");
			base.GetComponentInParent<DragWindowModule>().CloseWindow();
			return;
		}
		base.GetComponentInParent<DragWindowModule>().CloseWindow();
	}

	// Token: 0x040009E0 RID: 2528
	[SerializeField]
	private Image oneStarImage;

	// Token: 0x040009E1 RID: 2529
	[SerializeField]
	private Image twoStarImage;

	// Token: 0x040009E2 RID: 2530
	[SerializeField]
	private Image threeStarImage;

	// Token: 0x040009E3 RID: 2531
	[SerializeField]
	private Image fourStarImage;

	// Token: 0x040009E4 RID: 2532
	[SerializeField]
	private Image fiveStarImage;

	// Token: 0x040009E5 RID: 2533
	[SerializeField]
	private Button rateButton;

	// Token: 0x040009E6 RID: 2534
	private int totalStars;

	// Token: 0x040009E7 RID: 2535
	private UISystemModule uiSystemModule;
}
