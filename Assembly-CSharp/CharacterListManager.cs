using System;
using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using DuloGames.UI;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000229 RID: 553
public class CharacterListManager : MonoBehaviour
{
	// Token: 0x060007A4 RID: 1956 RVA: 0x00024EC0 File Offset: 0x000230C0
	private void Start()
	{
		this.UpdatePremiumDaysText();
		this.networkManagerModule = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManagerModule>();
		this.BuildPlayerSlots();
	}

	// Token: 0x060007A5 RID: 1957 RVA: 0x00024EE4 File Offset: 0x000230E4
	private void UpdatePremiumDaysText()
	{
		if (SettingsManager.Instance.ApiAccount.PremiumDays == 0)
		{
			this.premiumDaysText.text = LanguageManager.Instance.GetText("no_premium_days_message").ToLower();
			return;
		}
		if (SettingsManager.Instance.ApiAccount.PremiumDays == 1)
		{
			this.premiumDaysText.text = LanguageManager.Instance.GetText("last_premium_day_message").ToLower();
			return;
		}
		if (SettingsManager.Instance.ApiAccount.PremiumDays <= 7)
		{
			this.premiumDaysText.text = string.Format(LanguageManager.Instance.GetText("few_premium_days_message"), SettingsManager.Instance.ApiAccount.PremiumDays).ToLower();
			return;
		}
		this.premiumDaysText.text = string.Format(LanguageManager.Instance.GetText("premium_days_message"), SettingsManager.Instance.ApiAccount.PremiumDays).ToLower();
	}

	// Token: 0x060007A6 RID: 1958 RVA: 0x00024FD7 File Offset: 0x000231D7
	public void SetSelectedPlayer(Player player)
	{
		this.selectedPlayer = player;
		this.SetCharacterSpriteLineart();
	}

	// Token: 0x060007A7 RID: 1959 RVA: 0x00024FE8 File Offset: 0x000231E8
	private void SetCharacterSpriteLineart()
	{
		this.playButton.interactable = true;
		string name = this.selectedPlayer.VocationName.ToLower() + "_" + this.selectedPlayer.GenderName.ToLower() + "_lineart";
		this.selectedCharacterImage.sprite = null;
		this.selectedCharacterImage.sprite = ResourcesManager.Instance.GetVocationSprite(name);
		this.selectedCharacterImage.preserveAspect = true;
		this.selectedCharacterImage.color = Color.white;
	}

	// Token: 0x060007A8 RID: 1960 RVA: 0x00025070 File Offset: 0x00023270
	private void SetCharacterSpriteFull()
	{
		this.playButton.interactable = false;
		string name = this.selectedPlayer.VocationName.ToLower() + "_" + this.selectedPlayer.GenderName.ToLower() + "_full";
		this.selectedCharacterImage.sprite = null;
		this.selectedCharacterImage.sprite = ResourcesManager.Instance.GetVocationSprite(name);
		this.selectedCharacterImage.preserveAspect = true;
	}

	// Token: 0x060007A9 RID: 1961 RVA: 0x000250E7 File Offset: 0x000232E7
	public void ShowDeleteWindow(Player playerToDelete)
	{
		this.deleteCharacterWindow.GetComponent<DeleteCharacterWindowManager>().SetPlayer(playerToDelete);
		this.deleteCharacterWindow.GetComponent<UIWindow>().Show();
	}

	// Token: 0x060007AA RID: 1962 RVA: 0x0002510A File Offset: 0x0002330A
	public void Play()
	{
		if (!this.selectedPlayer.IsDefined)
		{
			UILoadingOverlayManager.Instance.Create().LoadScene(3);
			return;
		}
		base.StartCoroutine(this.DoPlay());
	}

	// Token: 0x060007AB RID: 1963 RVA: 0x00025137 File Offset: 0x00023337
	public IEnumerator DoPlay()
	{
		SettingsManager.Instance.SelectedPlayer = this.selectedPlayer;
		if (this.selectedPlayer.ServerConnectionVersion != "2024.2.7")
		{
			base.StartCoroutine(this.ShowErrorMessage(LanguageManager.Instance.GetText("invalid_client_version_message"), 3f));
			yield break;
		}
		yield return AssetBundleManager.Instance.LoadClientBundles();
		this.SetCharacterSpriteFull();
		yield return new WaitForSeconds(0.5f);
		if (NetworkClient.isConnected)
		{
			this.networkManagerModule.StopClient();
		}
		this.networkManagerModule.networkAddress = this.selectedPlayer.ServerAddress;
		Debug.Log("Iniciando conexão via Telepathy Transport.");
		TelepathyTransport component = this.networkManagerModule.GetComponent<TelepathyTransport>();
		component.enabled = true;
		component.port = (ushort)this.selectedPlayer.ServerPort;
		Transport.active = component;
		yield return new WaitForSeconds(0.5f);
		NetworkClient.RegisterHandler<ConnectionFailureMessage>(delegate(ConnectionFailureMessage msg)
		{
			CharacterListManager.<<DoPlay>b__19_0>d <<DoPlay>b__19_0>d;
			<<DoPlay>b__19_0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
			<<DoPlay>b__19_0>d.<>4__this = this;
			<<DoPlay>b__19_0>d.msg = msg;
			<<DoPlay>b__19_0>d.<>1__state = -1;
			<<DoPlay>b__19_0>d.<>t__builder.Start<CharacterListManager.<<DoPlay>b__19_0>d>(ref <<DoPlay>b__19_0>d);
		}, false);
		this.networkManagerModule.maxConnections = 200;
		this.networkManagerModule.StartClient();
		NetworkClient.connection.isAuthenticated = true;
		ServerSettingsManager.ServerType = this.selectedPlayer.ServerType;
		ServerSettingsManager.ServerName = this.selectedPlayer.ServerName;
		this.networkManagerModule.ClientPlayerId = this.selectedPlayer.Id;
		this.networkManagerModule.ClientAccountId = this.selectedPlayer.AccountId;
		this.networkManagerModule.ClientConnectionVersion = "2024.2.7";
		this.networkManagerModule.ClientPackageType = SettingsManager.Instance.ApiAccount.PackageType;
		this.networkManagerModule.ClientAccountUniqueId = SettingsManager.Instance.ApiAccount.AccountUniqueId;
		this.networkManagerModule.ClientPlayerStartPosition = new Vector3(this.selectedPlayer.PositionX, this.selectedPlayer.PositionY);
		this.ShowFeedbackText(LanguageManager.Instance.GetText("connecting_message"));
		yield break;
	}

	// Token: 0x060007AC RID: 1964 RVA: 0x00025148 File Offset: 0x00023348
	public void BuildPlayerSlots()
	{
		Player[] array = (from p in SettingsManager.Instance.ApiAccount.Players
		orderby p.BaseLevel descending, p.Name
		select p).ToArray<Player>();
		for (int i = 0; i < this.charactersList.transform.childCount; i++)
		{
			UnityEngine.Object.Destroy(this.charactersList.transform.GetChild(i).gameObject);
		}
		for (int j = 0; j < array.Length; j++)
		{
			GameObject gameObject = this.CreateCharacterSlot();
			gameObject.GetComponent<CharacterSlotManager>().SetPlayer(array[j]);
			Toggle component = gameObject.GetComponent<Toggle>();
			component.group = this.charactersToggleGroup;
			if (j == 0)
			{
				this.selectedPlayer = array[j];
				component.isOn = true;
			}
		}
		this.createCharacterButton.gameObject.SetActive(SettingsManager.Instance.ApiAccount.Players.Count < SettingsManager.Instance.ApiAccount.MaxCharacterSlots);
	}

	// Token: 0x060007AD RID: 1965 RVA: 0x0002526C File Offset: 0x0002346C
	public void RemovePlayerAndRebuildSlots(Player player)
	{
		SettingsManager.Instance.ApiAccount.Players.Remove(player);
		this.BuildPlayerSlots();
	}

	// Token: 0x060007AE RID: 1966 RVA: 0x0002528A File Offset: 0x0002348A
	public void ShowFeedbackText(string text)
	{
		this.feedbackPanel.SetActive(true);
		this.feedbackText.color = new Color(115f, 94f, 78f);
		this.feedbackText.text = text;
	}

	// Token: 0x060007AF RID: 1967 RVA: 0x000252C3 File Offset: 0x000234C3
	private IEnumerator ShowErrorMessage(string message, float interval)
	{
		this.feedbackPanel.SetActive(true);
		this.feedbackText.color = Color.red;
		this.feedbackText.text = message;
		yield return new WaitForSeconds(interval);
		this.feedbackPanel.SetActive(false);
		yield break;
	}

	// Token: 0x060007B0 RID: 1968 RVA: 0x000252E0 File Offset: 0x000234E0
	private void ShowErrorBox(string message)
	{
		UIModalBox uimodalBox = UIModalBoxManager.Instance.Create(base.gameObject);
		if (uimodalBox != null)
		{
			uimodalBox.SetText1(message);
			uimodalBox.SetText2(LanguageManager.Instance.GetText("something_gone_wrong_message"));
			uimodalBox.SetConfirmButtonText("ok");
			uimodalBox.Show();
		}
	}

	// Token: 0x060007B1 RID: 1969 RVA: 0x00025334 File Offset: 0x00023534
	public GameObject CreateCharacterSlot()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.characterSlotPrefab);
		gameObject.transform.SetParent(this.charactersList.transform, false);
		gameObject.transform.position = Vector2.zero;
		return gameObject;
	}

	// Token: 0x060007B2 RID: 1970 RVA: 0x00021F50 File Offset: 0x00020150
	public void ExitGame()
	{
		Application.Quit();
	}

	// Token: 0x04000985 RID: 2437
	[SerializeField]
	private GameObject feedbackPanel;

	// Token: 0x04000986 RID: 2438
	[SerializeField]
	private Text feedbackText;

	// Token: 0x04000987 RID: 2439
	[SerializeField]
	private GameObject characterSlotPrefab;

	// Token: 0x04000988 RID: 2440
	[SerializeField]
	private GameObject charactersList;

	// Token: 0x04000989 RID: 2441
	[SerializeField]
	private GameObject deleteCharacterWindow;

	// Token: 0x0400098A RID: 2442
	[SerializeField]
	private Image selectedCharacterImage;

	// Token: 0x0400098B RID: 2443
	[SerializeField]
	private Button createCharacterButton;

	// Token: 0x0400098C RID: 2444
	[SerializeField]
	private Button playButton;

	// Token: 0x0400098D RID: 2445
	[SerializeField]
	private Text premiumDaysText;

	// Token: 0x0400098E RID: 2446
	[SerializeField]
	private ToggleGroup charactersToggleGroup;

	// Token: 0x0400098F RID: 2447
	private NetworkManagerModule networkManagerModule;

	// Token: 0x04000990 RID: 2448
	private Player selectedPlayer;
}
