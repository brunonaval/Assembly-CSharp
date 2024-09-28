using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000282 RID: 642
public class LoginMessageWindowManager : MonoBehaviour
{
	// Token: 0x060009DF RID: 2527 RVA: 0x0002D91C File Offset: 0x0002BB1C
	private IEnumerator Start()
	{
		yield return this.LoadMessage();
		yield break;
	}

	// Token: 0x060009E0 RID: 2528 RVA: 0x0002D92B File Offset: 0x0002BB2B
	private IEnumerator LoadMessage()
	{
		string url = GlobalSettings.PublicApiBaseUrl + "post/" + LanguageManager.Instance.GetText("api_culture");
		yield return ApiManager.Get<LoginMessage>(url, delegate(ApiResponse<LoginMessage> response)
		{
			if (response.Success)
			{
				if (!string.IsNullOrEmpty(response.ResponseObject.Title))
				{
					this.titleText.text = response.ResponseObject.Title;
				}
				if (!string.IsNullOrEmpty(response.ResponseObject.Content))
				{
					this.messageText.text = response.ResponseObject.Content;
					this.loginMessageWindow.SetActive(true);
				}
			}
		});
		yield break;
	}

	// Token: 0x04000B5F RID: 2911
	[SerializeField]
	private Text titleText;

	// Token: 0x04000B60 RID: 2912
	[SerializeField]
	private Text messageText;

	// Token: 0x04000B61 RID: 2913
	[SerializeField]
	private GameObject loginMessageWindow;
}
