using System;
using System.Collections;
using DuloGames.UI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200023F RID: 575
public class StartupManager : MonoBehaviour
{
	// Token: 0x06000836 RID: 2102 RVA: 0x0002765B File Offset: 0x0002585B
	private IEnumerator Start()
	{
		if (!SettingsManager.Instance.SettingsExists())
		{
			this.languagePanel.SetActive(true);
			yield break;
		}
		yield return this.LoadGame(null);
		yield break;
	}

	// Token: 0x06000837 RID: 2103 RVA: 0x0002766A File Offset: 0x0002586A
	public void OnEnglishButtonClick()
	{
		base.StartCoroutine(this.LoadGame(new Language?(Language.English)));
	}

	// Token: 0x06000838 RID: 2104 RVA: 0x0002767F File Offset: 0x0002587F
	public void OnPortugueseButtonClick()
	{
		base.StartCoroutine(this.LoadGame(new Language?(Language.Portuguese)));
	}

	// Token: 0x06000839 RID: 2105 RVA: 0x00027694 File Offset: 0x00025894
	public void OnSpanishButtonClick()
	{
		base.StartCoroutine(this.LoadGame(new Language?(Language.Spanish)));
	}

	// Token: 0x0600083A RID: 2106 RVA: 0x000276A9 File Offset: 0x000258A9
	public void OnFrenchButtonClick()
	{
		base.StartCoroutine(this.LoadGame(new Language?(Language.French)));
	}

	// Token: 0x0600083B RID: 2107 RVA: 0x000276BE File Offset: 0x000258BE
	private IEnumerator LoadGame(Language? language)
	{
		Debug.Log(string.Format("Carregando idioma: {0}", language));
		this.feedbackText.text = "Loading...";
		this.languagePanel.SetActive(false);
		SettingsManager.Instance.LoadSettings(language);
		yield return new WaitUntil(() => SettingsManager.Instance.IsLoaded);
		QualitySettings.vSyncCount = 0;
		if (Application.platform == RuntimePlatform.OSXPlayer)
		{
			Application.targetFrameRate = 70;
		}
		else
		{
			Application.targetFrameRate = ((SettingsManager.Instance.TargetFrameRate > 0) ? SettingsManager.Instance.TargetFrameRate : 30);
		}
		if (SettingsManager.Instance.FullScreen)
		{
			Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, true);
		}
		else if (Screen.fullScreen)
		{
			Screen.SetResolution(Display.main.systemWidth / 2, Display.main.systemHeight / 2, false);
		}
		string languageFile = GlobalUtils.GetLanguageFile(language ?? SettingsManager.Instance.Language);
		LanguageManager.Instance.LoadItems(languageFile);
		yield return new WaitUntil(() => LanguageManager.Instance.IsReady);
		UILoadingOverlayManager.Instance.Create().LoadScene(1);
		yield break;
	}

	// Token: 0x04000A15 RID: 2581
	[SerializeField]
	private GameObject languagePanel;

	// Token: 0x04000A16 RID: 2582
	[SerializeField]
	private Text feedbackText;
}
