using System;
using System.Collections;
using DuloGames.UI;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Token: 0x02000227 RID: 551
public class CharacterCreationManager : MonoBehaviour
{
	// Token: 0x0600078C RID: 1932 RVA: 0x0002463D File Offset: 0x0002283D
	private void Start()
	{
		this.SetVocation(Vocation.Warrior);
		this.SetGender(CreatureGender.Male);
		this.UpdateVocationInfo();
	}

	// Token: 0x0600078D RID: 1933 RVA: 0x00024653 File Offset: 0x00022853
	public void SetVocation(Vocation vocation)
	{
		this.selectedVocation = vocation;
		this.UpdateVocationInfo();
	}

	// Token: 0x0600078E RID: 1934 RVA: 0x00024662 File Offset: 0x00022862
	public void SetGender(CreatureGender gender)
	{
		this.selectedGender = gender;
		this.UpdateVocationInfo();
	}

	// Token: 0x0600078F RID: 1935 RVA: 0x00024671 File Offset: 0x00022871
	private void UpdateVocationInfo()
	{
		this.UpdateCharacterSprite();
		this.UpdateAttributesGrid();
		this.UpdateVocationDescription();
		this.UpdateVocationName();
	}

	// Token: 0x06000790 RID: 1936 RVA: 0x0002468C File Offset: 0x0002288C
	private void UpdateCharacterSprite()
	{
		string name = this.selectedVocation.ToString().ToLower() + "_" + this.selectedGender.ToString().ToLower() + "_full";
		this.selectedCharacterImage.sprite = null;
		this.selectedCharacterImage.sprite = ResourcesManager.Instance.GetVocationSprite(name);
	}

	// Token: 0x06000791 RID: 1937 RVA: 0x000246F8 File Offset: 0x000228F8
	private void UpdateAttributesGrid()
	{
		this.UpdateAttributeGrid(this.powerGrid, AttributeBase.GetBaseAttributeLevel(this.selectedVocation, AttributeType.Power));
		this.UpdateAttributeGrid(this.toughnessGrid, AttributeBase.GetBaseAttributeLevel(this.selectedVocation, AttributeType.Toughness));
		this.UpdateAttributeGrid(this.agilityGrid, AttributeBase.GetBaseAttributeLevel(this.selectedVocation, AttributeType.Agility));
		this.UpdateAttributeGrid(this.precisionGrid, AttributeBase.GetBaseAttributeLevel(this.selectedVocation, AttributeType.Precision));
		this.UpdateAttributeGrid(this.vitalityGrid, AttributeBase.GetBaseAttributeLevel(this.selectedVocation, AttributeType.Vitality));
	}

	// Token: 0x06000792 RID: 1938 RVA: 0x00024780 File Offset: 0x00022980
	private void UpdateVocationDescription()
	{
		string key = "vocation_description_" + this.selectedVocation.ToString().ToLower();
		string text = LanguageManager.Instance.GetText(key);
		this.vocationDescriptionText.text = text;
	}

	// Token: 0x06000793 RID: 1939 RVA: 0x000247C8 File Offset: 0x000229C8
	private void UpdateVocationName()
	{
		string text = LanguageManager.Instance.GetText(this.selectedVocation.ToString().ToLower());
		this.vocationNameText.text = text;
	}

	// Token: 0x06000794 RID: 1940 RVA: 0x00024804 File Offset: 0x00022A04
	private void UpdateAttributeGrid(GameObject grid, int attributePoints)
	{
		for (int i = 0; i < grid.transform.childCount; i++)
		{
			Transform child = grid.transform.GetChild(i);
			if (child.CompareTag("UIAttribute"))
			{
				UnityEngine.Object.Destroy(child.gameObject);
			}
		}
		for (int j = 0; j < attributePoints; j++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.attributePointPrefab);
			gameObject.transform.SetParent(grid.transform, false);
			gameObject.transform.position = Vector2.zero;
		}
	}

	// Token: 0x06000795 RID: 1941 RVA: 0x0002488C File Offset: 0x00022A8C
	public void SetSelectedServer(ApiServer server)
	{
		this.selectedServer = server;
		this.serverNameText.text = string.Format(LanguageManager.Instance.GetText("server_name_label"), "<color=orange>" + server.Name + "</color>").ToLower();
		this.serverTypeText.text = string.Format(LanguageManager.Instance.GetText("server_type_label"), string.Format("<color=red>{0}</color>", server.ServerType)).ToLower();
		this.serverPopulationText.text = string.Format(LanguageManager.Instance.GetText("server_population_label"), string.Concat(new string[]
		{
			"<color=",
			this.GetServerColor(),
			">",
			this.GetServerPopulationLabel(),
			"</color>"
		})).ToLower();
		this.changeServerButton.SetActive(true);
		this.createButtonText.text = LanguageManager.Instance.GetText("create").ToLower();
	}

	// Token: 0x06000796 RID: 1942 RVA: 0x00024998 File Offset: 0x00022B98
	private string GetServerPopulationLabel()
	{
		float num = (float)this.selectedServer.CurrentPlayers / (float)this.selectedServer.MaxPlayers;
		if ((double)num < 0.5)
		{
			return LanguageManager.Instance.GetText("empty");
		}
		if ((double)num < 0.9)
		{
			return LanguageManager.Instance.GetText("normal");
		}
		return LanguageManager.Instance.GetText("crowded");
	}

	// Token: 0x06000797 RID: 1943 RVA: 0x00024A08 File Offset: 0x00022C08
	private string GetServerColor()
	{
		float num = (float)this.selectedServer.CurrentPlayers / (float)this.selectedServer.MaxPlayers;
		if ((double)num < 0.5)
		{
			return "green";
		}
		if ((double)num < 0.9)
		{
			return "orange";
		}
		return "red";
	}

	// Token: 0x06000798 RID: 1944 RVA: 0x00024A5A File Offset: 0x00022C5A
	public void OnChangeServerClick()
	{
		this.ShowServerListWindow();
	}

	// Token: 0x06000799 RID: 1945 RVA: 0x00024A62 File Offset: 0x00022C62
	public void OnCreateButtonClick()
	{
		if (this.selectedServer == null || this.selectedServer.Id == 0)
		{
			this.ShowServerListWindow();
			return;
		}
		base.StartCoroutine(this.DoCreateCharacter());
	}

	// Token: 0x0600079A RID: 1946 RVA: 0x00024A8D File Offset: 0x00022C8D
	private IEnumerator DoCreateCharacter()
	{
		this.createButton.interactable = false;
		this.createButtonText.text = LanguageManager.Instance.GetText("create_button_wait").ToLower();
		string text = LanguageManager.Instance.GetText("api_culture");
		string text2 = GlobalSettings.BuildApiBaseUrl(SettingsManager.Instance.ApiAccount.AccountUniqueId) + "player";
		string text3 = "?AccountId=" + SettingsManager.Instance.ApiAccount.AccountId.ToString();
		text3 = text3 + "&ServerId=" + this.selectedServer.Id.ToString();
		text3 = text3 + "&Name=" + UnityWebRequest.EscapeURL(this.characterNameText.text);
		string str = text3;
		string str2 = "&Gender=";
		int num = (int)this.selectedGender;
		text3 = str + str2 + num.ToString();
		string str3 = text3;
		string str4 = "&Vocation=";
		num = (int)this.selectedVocation;
		text3 = str3 + str4 + num.ToString();
		text3 = text3 + "&culture=" + text;
		if (!GlobalUtils.IsValidPlayerName(this.characterNameText.text))
		{
			this.ShowErrorBox(LanguageManager.Instance.GetText("invalid_character_name_message"));
			yield break;
		}
		text2 += text3;
		UnityWebRequest webRequest = UnityWebRequest.PostWwwForm(text2, "no-data");
		yield return webRequest.SendWebRequest();
		if (!string.IsNullOrEmpty(webRequest.error) | webRequest.responseCode != 200L)
		{
			try
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
			catch
			{
				Debug.LogError(webRequest.downloadHandler.text);
			}
			this.createButton.interactable = true;
			this.createButtonText.text = LanguageManager.Instance.GetText("create").ToLower();
			yield break;
		}
		ApiCreateCharacterResponse apiCreateCharacterResponse = JsonUtility.FromJson<ApiCreateCharacterResponse>(webRequest.downloadHandler.text);
		SettingsManager.Instance.ApiAccount.Players.Add(apiCreateCharacterResponse.ResponseObject);
		UILoadingOverlayManager.Instance.Create().LoadScene(2);
		yield break;
	}

	// Token: 0x0600079B RID: 1947 RVA: 0x00024A9C File Offset: 0x00022C9C
	private void ShowErrorBox(string message)
	{
		UIModalBox uimodalBox = UIModalBoxManager.Instance.Create(base.gameObject);
		if (uimodalBox != null)
		{
			uimodalBox.SetText1(LanguageManager.Instance.GetText(message));
			uimodalBox.SetText2(LanguageManager.Instance.GetText("something_gone_wrong_message"));
			uimodalBox.SetConfirmButtonText("ok");
			uimodalBox.Show();
			this.createButton.interactable = true;
			this.createButtonText.text = LanguageManager.Instance.GetText("create");
		}
	}

	// Token: 0x0600079C RID: 1948 RVA: 0x00024B20 File Offset: 0x00022D20
	private void ShowServerListWindow()
	{
		this.serverListWindow.SetActive(true);
		for (int i = 0; i < this.serverListHolder.transform.childCount; i++)
		{
			UnityEngine.Object.Destroy(this.serverListHolder.transform.GetChild(i).gameObject);
		}
		foreach (ApiServer apiServer in SettingsManager.Instance.ApiAccount.Servers)
		{
			if (!(apiServer.ConnectionVersion != "2024.2.7"))
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.serverListPrefab);
				gameObject.transform.SetParent(this.serverListHolder.transform, false);
				gameObject.transform.position = Vector2.zero;
				ServerSlotManager serverSlotManager;
				gameObject.TryGetComponent<ServerSlotManager>(out serverSlotManager);
				serverSlotManager.SetServer(this, apiServer);
			}
		}
	}

	// Token: 0x0400096B RID: 2411
	private Vocation selectedVocation;

	// Token: 0x0400096C RID: 2412
	private CreatureGender selectedGender;

	// Token: 0x0400096D RID: 2413
	private ApiServer selectedServer;

	// Token: 0x0400096E RID: 2414
	[SerializeField]
	private Text characterNameText;

	// Token: 0x0400096F RID: 2415
	[SerializeField]
	private Text vocationDescriptionText;

	// Token: 0x04000970 RID: 2416
	[SerializeField]
	private Text vocationNameText;

	// Token: 0x04000971 RID: 2417
	[SerializeField]
	private Text createButtonText;

	// Token: 0x04000972 RID: 2418
	[SerializeField]
	private Text serverNameText;

	// Token: 0x04000973 RID: 2419
	[SerializeField]
	private Text serverTypeText;

	// Token: 0x04000974 RID: 2420
	[SerializeField]
	private Text serverPopulationText;

	// Token: 0x04000975 RID: 2421
	[SerializeField]
	private Image selectedCharacterImage;

	// Token: 0x04000976 RID: 2422
	[SerializeField]
	private GameObject attributePointPrefab;

	// Token: 0x04000977 RID: 2423
	[SerializeField]
	private GameObject powerGrid;

	// Token: 0x04000978 RID: 2424
	[SerializeField]
	private GameObject toughnessGrid;

	// Token: 0x04000979 RID: 2425
	[SerializeField]
	private GameObject agilityGrid;

	// Token: 0x0400097A RID: 2426
	[SerializeField]
	private GameObject precisionGrid;

	// Token: 0x0400097B RID: 2427
	[SerializeField]
	private GameObject vitalityGrid;

	// Token: 0x0400097C RID: 2428
	[SerializeField]
	private GameObject serverListWindow;

	// Token: 0x0400097D RID: 2429
	[SerializeField]
	private GameObject serverListHolder;

	// Token: 0x0400097E RID: 2430
	[SerializeField]
	private GameObject serverListPrefab;

	// Token: 0x0400097F RID: 2431
	[SerializeField]
	private GameObject changeServerButton;

	// Token: 0x04000980 RID: 2432
	[SerializeField]
	private Button createButton;
}
