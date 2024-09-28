using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using DuloGames.UI;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Token: 0x02000205 RID: 517
public class LoginManager : MonoBehaviour
{
	// Token: 0x060006C6 RID: 1734 RVA: 0x00021AC8 File Offset: 0x0001FCC8
	private void Start()
	{
		if (this.detailsText != null)
		{
			this.detailsText.text = "by Xtreaming - v2024.2.7";
		}
		SettingsManager.Instance.ApiAccount = null;
		if (AssetBundleManager.Instance != null)
		{
			AssetBundleManager.Instance.HideContentHolders();
		}
		this.loginInputHolder.SetActive(Application.platform != RuntimePlatform.Android);
		this.goBackToGoogleLoginButton.SetActive(Application.platform == RuntimePlatform.Android);
		this.googleLoginInputHolder.SetActive(Application.platform == RuntimePlatform.Android);
		this.LoadPlayerPrefs();
		this.UpdateVolumes();
	}

	// Token: 0x060006C7 RID: 1735 RVA: 0x00021B60 File Offset: 0x0001FD60
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return) | Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			this.Connect();
		}
	}

	// Token: 0x060006C8 RID: 1736 RVA: 0x00021B7C File Offset: 0x0001FD7C
	public static void TriggerDialog()
	{
		TcpClient tcpClient = new TcpClient();
		try
		{
			tcpClient.Connect(IPAddress.Parse("255.255.255.255"), 8052);
		}
		catch (Exception ex)
		{
			Debug.Log("IOSNetworkPermission exception occured");
			Debug.Log(ex.Message);
		}
		if (tcpClient != null)
		{
			tcpClient.Close();
		}
	}

	// Token: 0x060006C9 RID: 1737 RVA: 0x00021BD8 File Offset: 0x0001FDD8
	private void LoadPlayerPrefs()
	{
		if (PlayerPrefs.HasKey("Username"))
		{
			this.usernameInput.text = PlayerPrefs.GetString("Username");
		}
		if (PlayerPrefs.HasKey("Password"))
		{
			this.passwordInput.text = PlayerPrefs.GetString("Password");
		}
		if (PlayerPrefs.HasKey("RememberMe"))
		{
			int @int = PlayerPrefs.GetInt("RememberMe");
			this.rememberMeToggle.isOn = (@int == 1);
		}
	}

	// Token: 0x060006CA RID: 1738 RVA: 0x00021C50 File Offset: 0x0001FE50
	public void Connect()
	{
		try
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				LoginManager.TriggerDialog();
			}
			if (this.rememberMeToggle.isOn)
			{
				PlayerPrefs.SetString("Username", this.usernameInput.text);
				PlayerPrefs.SetString("Password", this.passwordInput.text);
				PlayerPrefs.SetInt("RememberMe", 1);
			}
			else
			{
				PlayerPrefs.DeleteKey("Username");
				PlayerPrefs.DeleteKey("Password");
				PlayerPrefs.SetInt("RememberMe", 0);
			}
		}
		finally
		{
			base.StartCoroutine(this.DoConnect(this.usernameInput.text, this.passwordInput.text));
		}
	}

	// Token: 0x060006CB RID: 1739 RVA: 0x00021D04 File Offset: 0x0001FF04
	public void ShowFeedbackText(string text)
	{
		this.feedbackPanel.SetActive(true);
		this.feedbackText.color = new Color(115f, 94f, 78f);
		this.feedbackText.text = text;
	}

	// Token: 0x060006CC RID: 1740 RVA: 0x00021D3D File Offset: 0x0001FF3D
	public void ShowFeedbackError(string text)
	{
		this.feedbackPanel.SetActive(true);
		this.feedbackText.color = Color.red;
		this.feedbackText.text = text;
	}

	// Token: 0x060006CD RID: 1741 RVA: 0x00021D67 File Offset: 0x0001FF67
	public void HideFeedbackPanel()
	{
		if (this.feedbackPanel.activeInHierarchy)
		{
			this.feedbackPanel.SetActive(false);
		}
	}

	// Token: 0x060006CE RID: 1742 RVA: 0x00021D82 File Offset: 0x0001FF82
	public void GoToEmailConnect()
	{
		this.loginInputHolder.SetActive(true);
		this.googleLoginInputHolder.SetActive(false);
		this.goBackToGoogleLoginButton.SetActive(true);
	}

	// Token: 0x060006CF RID: 1743 RVA: 0x00021DA8 File Offset: 0x0001FFA8
	public void GoBackToGoogleConnect()
	{
		this.loginInputHolder.SetActive(false);
		this.googleLoginInputHolder.SetActive(true);
		this.goBackToGoogleLoginButton.SetActive(true);
	}

	// Token: 0x060006D0 RID: 1744 RVA: 0x00021DCE File Offset: 0x0001FFCE
	private IEnumerator DoConnect(string username, string password)
	{
		this.DisableLoginInputs();
		this.ShowFeedbackText(LanguageManager.Instance.GetText("connecting_message"));
		yield return new WaitForSeconds(0.1f);
		string cultureName = LanguageManager.Instance.GetText("api_culture");
		password = GlobalUtils.GetMd5Hash(password);
		yield return this.DiscoverExternalAddress();
		username = UnityWebRequest.EscapeURL(username);
		string uri = string.Concat(new string[]
		{
			GlobalSettings.PublicApiBaseUrl,
			"account?username=",
			username,
			"&password=",
			password,
			"&culture=",
			cultureName,
			"&sourceAddress=",
			this.sourceAddress
		});
		using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
		{
			yield return webRequest.SendWebRequest();
			if (webRequest.result == UnityWebRequest.Result.ConnectionError)
			{
				this.EnableLoginInputs();
				this.ShowFeedbackError(webRequest.error);
				yield break;
			}
			ApiConnectResponse apiConnectResponse = JsonUtility.FromJson<ApiConnectResponse>(webRequest.downloadHandler.text);
			if (!string.IsNullOrEmpty((apiConnectResponse != null) ? apiConnectResponse.Message : null))
			{
				this.EnableLoginInputs();
				this.ShowFeedbackError(apiConnectResponse.Message);
				yield break;
			}
			SettingsManager.Instance.ApiAccount = apiConnectResponse.ResponseObject;
			this.HideFeedbackPanel();
			UILoadingOverlayManager.Instance.Create().LoadScene(2);
		}
		UnityWebRequest webRequest = null;
		yield break;
		yield break;
	}

	// Token: 0x060006D1 RID: 1745 RVA: 0x00021DEB File Offset: 0x0001FFEB
	private IEnumerator DiscoverExternalAddress()
	{
		this.sourceAddress = "";
		yield return this.DiscoverExternalIPFromHost("https://ipinfo.io/ip");
		if (!string.IsNullOrEmpty(this.sourceAddress))
		{
			yield break;
		}
		yield return this.DiscoverExternalIPFromHost("https://icanhazip.com");
		yield break;
	}

	// Token: 0x060006D2 RID: 1746 RVA: 0x00021DFA File Offset: 0x0001FFFA
	private IEnumerator DiscoverExternalIPFromHost(string host)
	{
		using (UnityWebRequest webRequest = UnityWebRequest.Get(host))
		{
			yield return webRequest.SendWebRequest();
			if (webRequest.result == UnityWebRequest.Result.ConnectionError)
			{
				this.EnableLoginInputs();
				this.ShowFeedbackError(webRequest.error);
				yield break;
			}
			string text = webRequest.downloadHandler.text;
			text = text.Trim();
			if (host.Contains("dyndns.org"))
			{
				try
				{
					string[] array = text.Split(new string[]
					{
						":"
					}, StringSplitOptions.None);
					this.sourceAddress = array[1].Trim().Split(new string[]
					{
						"<"
					}, StringSplitOptions.None)[0].Trim();
				}
				catch (Exception arg)
				{
					Debug.Log(string.Format("Raw: {0} | Err: {1}", text, arg));
				}
				yield break;
			}
			this.sourceAddress = text;
			if (!string.IsNullOrEmpty(this.sourceAddress) && (this.sourceAddress.StartsWith("<") || this.sourceAddress.ToLower().Contains("error")))
			{
				this.sourceAddress = string.Empty;
			}
		}
		UnityWebRequest webRequest = null;
		yield break;
		yield break;
	}

	// Token: 0x060006D3 RID: 1747 RVA: 0x00021E10 File Offset: 0x00020010
	public void LoadAccountCreationScene()
	{
		UILoadingOverlayManager.Instance.Create().LoadScene(4);
	}

	// Token: 0x060006D4 RID: 1748 RVA: 0x00021E22 File Offset: 0x00020022
	public void OnCreateAccountButtonClicked()
	{
		Application.OpenURL(GlobalSettings.WebPage + "/account/register");
	}

	// Token: 0x060006D5 RID: 1749 RVA: 0x000202A7 File Offset: 0x0001E4A7
	public void GoToDiscordServer()
	{
		Application.OpenURL(GlobalSettings.DiscordServer);
	}

	// Token: 0x060006D6 RID: 1750 RVA: 0x00021E38 File Offset: 0x00020038
	public void GoToInstagramProfile()
	{
		Application.OpenURL(GlobalSettings.InstagramProfile);
	}

	// Token: 0x060006D7 RID: 1751 RVA: 0x00021E44 File Offset: 0x00020044
	public void GoToFacebookPage()
	{
		Application.OpenURL(GlobalSettings.FacebookPage);
	}

	// Token: 0x060006D8 RID: 1752 RVA: 0x00021E50 File Offset: 0x00020050
	public void GoToYoutubeChannel()
	{
		Application.OpenURL(GlobalSettings.YoutubeChannel);
	}

	// Token: 0x060006D9 RID: 1753 RVA: 0x00021E5C File Offset: 0x0002005C
	public void GoToTwitterProfile()
	{
		Application.OpenURL(GlobalSettings.TwitterProfile);
	}

	// Token: 0x060006DA RID: 1754 RVA: 0x00021E68 File Offset: 0x00020068
	public void OnDeleteAccountButtonClicked()
	{
		Application.OpenURL(GlobalSettings.WebPage + "/post/detail/263");
	}

	// Token: 0x060006DB RID: 1755 RVA: 0x00021E7E File Offset: 0x0002007E
	public void OnForgotPasswordButtonClicked()
	{
		Application.OpenURL(GlobalSettings.WebPage + "/account/recovery");
	}

	// Token: 0x060006DC RID: 1756 RVA: 0x00021E94 File Offset: 0x00020094
	public void OnMyAccountButtonClicked()
	{
		Application.OpenURL(GlobalSettings.WebPage);
	}

	// Token: 0x060006DD RID: 1757 RVA: 0x00021EA0 File Offset: 0x000200A0
	public void DisableLoginInputs()
	{
		this.loginInputHolder.SetActive(false);
		this.usernameInput.interactable = false;
		this.passwordInput.interactable = false;
		this.connectButton.interactable = false;
		this.rememberMeToggle.interactable = false;
	}

	// Token: 0x060006DE RID: 1758 RVA: 0x00021EDE File Offset: 0x000200DE
	public void EnableLoginInputs()
	{
		this.loginInputHolder.SetActive(true);
		this.usernameInput.interactable = true;
		this.passwordInput.interactable = true;
		this.connectButton.interactable = true;
		this.rememberMeToggle.interactable = true;
	}

	// Token: 0x060006DF RID: 1759 RVA: 0x00021F1C File Offset: 0x0002011C
	private void UpdateVolumes()
	{
		if (SettingsManager.Instance.Mute)
		{
			this.audioSource.volume = 0f;
			return;
		}
		this.audioSource.volume = SettingsManager.Instance.BgmVolume;
	}

	// Token: 0x060006E0 RID: 1760 RVA: 0x00021F50 File Offset: 0x00020150
	public void ExitGame()
	{
		Application.Quit();
	}

	// Token: 0x040008E4 RID: 2276
	[SerializeField]
	private GameObject feedbackPanel;

	// Token: 0x040008E5 RID: 2277
	[SerializeField]
	private Text feedbackText;

	// Token: 0x040008E6 RID: 2278
	[SerializeField]
	private InputField usernameInput;

	// Token: 0x040008E7 RID: 2279
	[SerializeField]
	private InputField passwordInput;

	// Token: 0x040008E8 RID: 2280
	[SerializeField]
	private Toggle rememberMeToggle;

	// Token: 0x040008E9 RID: 2281
	[SerializeField]
	private Button connectButton;

	// Token: 0x040008EA RID: 2282
	[SerializeField]
	private AudioSource audioSource;

	// Token: 0x040008EB RID: 2283
	[SerializeField]
	private GameObject loginInputHolder;

	// Token: 0x040008EC RID: 2284
	[SerializeField]
	private GameObject googleLoginInputHolder;

	// Token: 0x040008ED RID: 2285
	[SerializeField]
	private GameObject goBackToGoogleLoginButton;

	// Token: 0x040008EE RID: 2286
	[SerializeField]
	private Text detailsText;

	// Token: 0x040008EF RID: 2287
	[SerializeField]
	private GooglePlayServicesManager googlePlayServicesManager;

	// Token: 0x040008F0 RID: 2288
	[SerializeField]
	private Button goToEmailLoginButton;

	// Token: 0x040008F1 RID: 2289
	[SerializeField]
	private Button googleLoginButton;

	// Token: 0x040008F2 RID: 2290
	private string sourceAddress;
}
