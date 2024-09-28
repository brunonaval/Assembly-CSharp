using System;
using System.Collections;
using DuloGames.UI;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Token: 0x02000273 RID: 627
public class DeleteCharacterWindowManager : MonoBehaviour
{
	// Token: 0x0600098E RID: 2446 RVA: 0x0002C9C5 File Offset: 0x0002ABC5
	public void SetPlayer(Player player)
	{
		this.player = player;
	}

	// Token: 0x0600098F RID: 2447 RVA: 0x0002C9CE File Offset: 0x0002ABCE
	public void OnConfirmButtonClicked()
	{
		base.StartCoroutine(this.ConfirmCharacterDeletion());
	}

	// Token: 0x06000990 RID: 2448 RVA: 0x0002C9DD File Offset: 0x0002ABDD
	private IEnumerator ConfirmCharacterDeletion()
	{
		string name = this.player.Name;
		string a = (name != null) ? name.ToLower() : null;
		string text = this.CharacterNameInput.text;
		if (a != ((text != null) ? text.ToLower() : null))
		{
			this.ShowErrorBox(LanguageManager.Instance.GetText("invalid_name_delete_message"));
			yield break;
		}
		try
		{
			base.GetComponent<CanvasGroup>().interactable = false;
			string text2 = LanguageManager.Instance.GetText("api_culture");
			string str = GlobalSettings.BuildApiBaseUrl(SettingsManager.Instance.ApiAccount.AccountUniqueId) + "player";
			string str2 = "?PlayerId=" + this.player.Id.ToString() + "&culture=" + text2;
			UnityWebRequest webRequest = UnityWebRequest.Delete(str + str2);
			yield return webRequest.SendWebRequest();
			if (!string.IsNullOrEmpty(webRequest.error))
			{
				ResponseError responseError = JsonUtility.FromJson<ResponseError>(webRequest.downloadHandler.text);
				if (responseError != null)
				{
					this.ShowErrorBox(responseError.Message);
				}
				else
				{
					this.ShowErrorBox(webRequest.error);
				}
			}
			else
			{
				this.characterListManager.RemovePlayerAndRebuildSlots(this.player);
				base.GetComponent<UIWindow>().Hide();
			}
			webRequest = null;
		}
		finally
		{
			base.GetComponent<CanvasGroup>().interactable = true;
		}
		yield break;
		yield break;
	}

	// Token: 0x06000991 RID: 2449 RVA: 0x0002C9EC File Offset: 0x0002ABEC
	private void ShowErrorBox(string message)
	{
		UIModalBox uimodalBox = UIModalBoxManager.Instance.Create(base.gameObject);
		if (uimodalBox != null)
		{
			uimodalBox.SetText1(LanguageManager.Instance.GetText("something_gone_wrong_message"));
			uimodalBox.SetText2(message);
			uimodalBox.SetConfirmButtonText("ok");
			uimodalBox.Show();
		}
	}

	// Token: 0x04000B2B RID: 2859
	[SerializeField]
	private Text CharacterNameInput;

	// Token: 0x04000B2C RID: 2860
	[SerializeField]
	private CharacterListManager characterListManager;

	// Token: 0x04000B2D RID: 2861
	private Player player;
}
