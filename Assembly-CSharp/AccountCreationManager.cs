using System;
using System.Collections;
using System.Text;
using DuloGames.UI;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Token: 0x02000225 RID: 549
public class AccountCreationManager : MonoBehaviour
{
	// Token: 0x0600077D RID: 1917 RVA: 0x00024303 File Offset: 0x00022503
	public void CreateAccount()
	{
		base.StartCoroutine(this.SendPostRequest());
	}

	// Token: 0x0600077E RID: 1918 RVA: 0x00024312 File Offset: 0x00022512
	private IEnumerator SendPostRequest()
	{
		if (!this.rulesToggle.isOn)
		{
			this.ShowFeedbackError(LanguageManager.Instance.GetText("must_accept_rules_and_terms_message"));
			yield break;
		}
		this.ShowFeedbackText(LanguageManager.Instance.GetText("account_creation_message"));
		this.inputHolder.SetActive(false);
		ApiCreateAccountResource apiCreateAccountResource = new ApiCreateAccountResource
		{
			CultureName = LanguageManager.Instance.GetText("api_culture"),
			Email = this.emailInput.text,
			Name = LanguageManager.Instance.GetText("player"),
			Password = this.passwordInput.text,
			ReceiveNewsletter = this.newsletterToggle.isOn,
			Username = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8)
		};
		string s = JsonUtility.ToJson(apiCreateAccountResource);
		string url = GlobalSettings.PublicApiBaseUrl + "account/register?culture=" + apiCreateAccountResource.CultureName;
		UnityWebRequest request = new UnityWebRequest(url, "POST");
		byte[] bytes = Encoding.UTF8.GetBytes(s);
		request.uploadHandler = new UploadHandlerRaw(bytes);
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");
		yield return request.SendWebRequest();
		if (request.result == UnityWebRequest.Result.ConnectionError)
		{
			this.inputHolder.SetActive(true);
			this.ShowFeedbackError(request.error);
			yield break;
		}
		ApiResponse<object> apiResponse = JsonUtility.FromJson<ApiResponse<object>>(request.downloadHandler.text);
		if (!string.IsNullOrEmpty((apiResponse != null) ? apiResponse.Message : null))
		{
			this.inputHolder.SetActive(true);
			this.ShowFeedbackError(apiResponse.Message);
			yield break;
		}
		this.ShowFeedbackText(LanguageManager.Instance.GetText("account_created_message"));
		yield return new WaitForSeconds(5f);
		UILoadingOverlayManager.Instance.Create().LoadScene(1);
		yield break;
	}

	// Token: 0x0600077F RID: 1919 RVA: 0x00024321 File Offset: 0x00022521
	public void ShowFeedbackText(string text)
	{
		this.feedbackPanel.SetActive(true);
		this.feedbackText.color = new Color(115f, 94f, 78f);
		this.feedbackText.text = text;
	}

	// Token: 0x06000780 RID: 1920 RVA: 0x0002435A File Offset: 0x0002255A
	public void ShowFeedbackError(string text)
	{
		this.feedbackPanel.SetActive(true);
		this.feedbackText.color = Color.red;
		this.feedbackText.text = text;
	}

	// Token: 0x06000781 RID: 1921 RVA: 0x00024384 File Offset: 0x00022584
	public void HideFeedbackPanel()
	{
		if (this.feedbackPanel.activeInHierarchy)
		{
			this.feedbackPanel.SetActive(false);
		}
	}

	// Token: 0x06000782 RID: 1922 RVA: 0x0002439F File Offset: 0x0002259F
	public void BackToLogin()
	{
		UILoadingOverlayManager.Instance.Create().LoadScene(1);
	}

	// Token: 0x06000783 RID: 1923 RVA: 0x000243B1 File Offset: 0x000225B1
	public void GoToRulesAndTerms()
	{
		Application.OpenURL(GlobalSettings.WebPage + "/home/usageterms");
	}

	// Token: 0x06000784 RID: 1924 RVA: 0x00021E7E File Offset: 0x0002007E
	public void GoToRecoveryPassword()
	{
		Application.OpenURL(GlobalSettings.WebPage + "/account/recovery");
	}

	// Token: 0x04000960 RID: 2400
	[SerializeField]
	private GameObject feedbackPanel;

	// Token: 0x04000961 RID: 2401
	[SerializeField]
	private Text feedbackText;

	// Token: 0x04000962 RID: 2402
	[SerializeField]
	private GameObject inputHolder;

	// Token: 0x04000963 RID: 2403
	[SerializeField]
	private InputField emailInput;

	// Token: 0x04000964 RID: 2404
	[SerializeField]
	private InputField passwordInput;

	// Token: 0x04000965 RID: 2405
	[SerializeField]
	private Toggle rulesToggle;

	// Token: 0x04000966 RID: 2406
	[SerializeField]
	private Toggle newsletterToggle;
}
