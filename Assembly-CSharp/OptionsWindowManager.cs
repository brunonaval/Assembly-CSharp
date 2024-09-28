using System;
using System.Collections.Generic;
using DuloGames.UI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000285 RID: 645
public class OptionsWindowManager : MonoBehaviour
{
	// Token: 0x060009EF RID: 2543 RVA: 0x0002DA98 File Offset: 0x0002BC98
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		if (gameObject != null)
		{
			this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
		}
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("GameEnvironment");
		if (gameObject2 != null)
		{
			gameObject2.TryGetComponent<GameEnvironmentModule>(out this.gameEnvironmentModule);
		}
		if (!GlobalSettings.IsMobilePlatform)
		{
			this.fixedJoystickToggle.interactable = false;
		}
		if (GlobalSettings.IsMobilePlatform)
		{
			UnityEngine.Object.Destroy(this.keyboardControlsTabObject);
		}
	}

	// Token: 0x060009F0 RID: 2544 RVA: 0x0002DB0C File Offset: 0x0002BD0C
	private void OnEnable()
	{
		this.loaded = false;
		SettingsManager.Instance.LoadSettings(null);
		this.muteToggle.isOn = SettingsManager.Instance.Mute;
		this.sfxVolumeSlider.value = SettingsManager.Instance.SfxVolume;
		this.bgmVolumeSlider.value = SettingsManager.Instance.BgmVolume;
		this.targetFrameRateSlider.value = (float)SettingsManager.Instance.TargetFrameRate;
		this.fullScreenToggle.isOn = SettingsManager.Instance.FullScreen;
		this.showAllNamesToggle.isOn = SettingsManager.Instance.ShowAllNames;
		this.fixedJoystickToggle.isOn = SettingsManager.Instance.FixedJoystick;
		this.showInfoOnChatToggle.isOn = SettingsManager.Instance.ShowInfoOnChat;
		this.chatWithReturnKeyToggle.isOn = SettingsManager.Instance.ToggleChatWithReturnKey;
		this.ambienceVolumeSlider.value = SettingsManager.Instance.AmbienceVolume;
		this.disableDropProtectionToggle.isOn = SettingsManager.Instance.DisableDropProtection;
		this.disableLightSystemToggle.isOn = SettingsManager.Instance.DisableLightSystem;
		this.disableVisualEffectsToggle.isOn = SettingsManager.Instance.DisableVisualEffects;
		this.AddCameraModeOptionsAndSelectValue();
		this.AddUiSizeOptionsAndSelectValue();
		this.AddLanguageOptionsAndSelectValue();
		this.AddMinimapDetailsOptionsAndSelectValue();
		this.loaded = true;
	}

	// Token: 0x060009F1 RID: 2545 RVA: 0x0002DC64 File Offset: 0x0002BE64
	private void OnDisable()
	{
		this.UpdateSettings();
		SettingsManager.Instance.SaveSettings();
	}

	// Token: 0x060009F2 RID: 2546 RVA: 0x0002DC78 File Offset: 0x0002BE78
	private void AddMinimapDetailsOptionsAndSelectValue()
	{
		List<string> list = new List<string>();
		foreach (string text in Enum.GetNames(typeof(MinimapDetails)))
		{
			list.Add(LanguageManager.Instance.GetText(text.ToLower()));
		}
		this.minimapDetailsSlider.options = list;
		this.minimapDetailsSlider.maxValue = (float)(list.Count - 1);
		this.minimapDetailsSlider.value = (float)SettingsManager.Instance.MinimapDetails;
	}

	// Token: 0x060009F3 RID: 2547 RVA: 0x0002DCFC File Offset: 0x0002BEFC
	private void AddCameraModeOptionsAndSelectValue()
	{
		List<string> list = new List<string>();
		foreach (string text in Enum.GetNames(typeof(CameraMode)))
		{
			list.Add(LanguageManager.Instance.GetText(text.ToLower()));
		}
		this.cameraModeSlider.options = list;
		this.uiSizeSlider.maxValue = (float)(list.Count - 1);
		this.cameraModeSlider.value = (float)SettingsManager.Instance.CameraMode;
	}

	// Token: 0x060009F4 RID: 2548 RVA: 0x0002DD80 File Offset: 0x0002BF80
	private void AddUiSizeOptionsAndSelectValue()
	{
		List<string> list = new List<string>();
		foreach (string uiSize in Enum.GetNames(typeof(UiSize)))
		{
			list.Add(GlobalUtils.GetUiSizeText(uiSize));
		}
		this.uiSizeSlider.options = list;
		this.uiSizeSlider.maxValue = (float)(list.Count - 1);
		this.uiSizeSlider.value = (float)SettingsManager.Instance.UiSize;
	}

	// Token: 0x060009F5 RID: 2549 RVA: 0x0002DDF8 File Offset: 0x0002BFF8
	private void AddLanguageOptionsAndSelectValue()
	{
		List<string> list = new List<string>();
		foreach (string text in Enum.GetNames(typeof(Language)))
		{
			list.Add(LanguageManager.Instance.GetText(text.ToLower()));
		}
		this.languageSlider.options = list;
		this.languageSlider.maxValue = (float)(list.Count - 1);
		this.languageSlider.value = (float)SettingsManager.Instance.Language;
	}

	// Token: 0x060009F6 RID: 2550 RVA: 0x0002DE79 File Offset: 0x0002C079
	public void UpdateSettings()
	{
		if (!this.loaded)
		{
			return;
		}
		this.UpdateSoundSettings();
		this.UpdateGameSettings();
		this.UpdateScreenAndCameraSettings();
		this.UpdateGraphicsSettings();
	}

	// Token: 0x060009F7 RID: 2551 RVA: 0x0002DE9C File Offset: 0x0002C09C
	public void UpdateGraphicsSettings()
	{
		if (!this.loaded)
		{
			return;
		}
		if (this.targetFrameRateSlider != null)
		{
			SettingsManager.Instance.TargetFrameRate = (int)this.targetFrameRateSlider.value;
			if (Application.platform == RuntimePlatform.OSXPlayer)
			{
				Application.targetFrameRate = 70;
				return;
			}
			Application.targetFrameRate = ((SettingsManager.Instance.TargetFrameRate > 0) ? SettingsManager.Instance.TargetFrameRate : 30);
		}
	}

	// Token: 0x060009F8 RID: 2552 RVA: 0x0002DF08 File Offset: 0x0002C108
	public void UpdateScreenAndCameraSettings()
	{
		if (!this.loaded)
		{
			return;
		}
		SettingsManager.Instance.FullScreen = this.fullScreenToggle.isOn;
		SettingsManager.Instance.UiSize = (UiSize)this.uiSizeSlider.value;
		SettingsManager.Instance.MinimapDetails = (MinimapDetails)this.minimapDetailsSlider.value;
		SettingsManager.Instance.CameraMode = (CameraMode)this.cameraModeSlider.value;
		SettingsManager.Instance.DisableLightSystem = this.disableLightSystemToggle.isOn;
		SettingsManager.Instance.DisableVisualEffects = this.disableVisualEffectsToggle.isOn;
		if (this.uiSystemModule != null & SettingsManager.Instance != null & Camera.main != null)
		{
			Camera.main.orthographicSize = GlobalUtils.GetMainCameraDistance(SettingsManager.Instance.CameraMode, false);
			this.uiSystemModule.UpdateUISize();
			this.uiSystemModule.UpdateMinimapDetails();
		}
		if (SettingsManager.Instance.DisableLightSystem)
		{
			this.gameEnvironmentModule.SetAmbientLightToMax();
		}
		else
		{
			this.gameEnvironmentModule.SetIgnoreAmbientLight(false);
		}
		if (Screen.fullScreen != SettingsManager.Instance.FullScreen)
		{
			if (SettingsManager.Instance.FullScreen)
			{
				Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, true);
				return;
			}
			Screen.SetResolution(Display.main.systemWidth / 2, Display.main.systemHeight / 2, false);
		}
	}

	// Token: 0x060009F9 RID: 2553 RVA: 0x0002E070 File Offset: 0x0002C270
	public void UpdateGameSettings()
	{
		if (!this.loaded)
		{
			return;
		}
		SettingsManager.Instance.Language = (Language)this.languageSlider.value;
		SettingsManager.Instance.ShowAllNames = this.showAllNamesToggle.isOn;
		SettingsManager.Instance.FixedJoystick = this.fixedJoystickToggle.isOn;
		SettingsManager.Instance.ShowInfoOnChat = this.showInfoOnChatToggle.isOn;
		SettingsManager.Instance.ToggleChatWithReturnKey = this.chatWithReturnKeyToggle.isOn;
		SettingsManager.Instance.DisableDropProtection = this.disableDropProtectionToggle.isOn;
		if (this.uiSystemModule != null)
		{
			this.uiSystemModule.ChatModule.CmdSetShowInfoOnChat(SettingsManager.Instance.ShowInfoOnChat);
			this.uiSystemModule.ShowAllNames = SettingsManager.Instance.ShowAllNames;
		}
		if (!this.chatWithReturnKeyToggle.isOn & !GlobalSettings.IsMobilePlatform)
		{
			this.uiSystemModule.ShowChatHolder();
		}
	}

	// Token: 0x060009FA RID: 2554 RVA: 0x0002E168 File Offset: 0x0002C368
	public void UpdateSoundSettings()
	{
		if (!this.loaded)
		{
			return;
		}
		SettingsManager.Instance.Mute = this.muteToggle.isOn;
		SettingsManager.Instance.SfxVolume = this.sfxVolumeSlider.value;
		SettingsManager.Instance.BgmVolume = this.bgmVolumeSlider.value;
		SettingsManager.Instance.AmbienceVolume = this.ambienceVolumeSlider.value;
		if (this.uiSystemModule != null)
		{
			this.uiSystemModule.AreaModule.UpdateVolumes();
		}
	}

	// Token: 0x060009FB RID: 2555 RVA: 0x0002E1F0 File Offset: 0x0002C3F0
	public void UpdateLanguages()
	{
		if (!this.loaded)
		{
			return;
		}
		this.ShowNeedRestartMessage();
	}

	// Token: 0x060009FC RID: 2556 RVA: 0x0002E204 File Offset: 0x0002C404
	private void ShowNeedRestartMessage()
	{
		string text = LanguageManager.Instance.GetText("settings_need_restart_message");
		if (this.uiSystemModule != null)
		{
			this.uiSystemModule.ShowTimedFeedback(text, 7f, false);
		}
	}

	// Token: 0x060009FD RID: 2557 RVA: 0x0002E241 File Offset: 0x0002C441
	public void SaveSettings()
	{
		this.UpdateSettings();
		SettingsManager.Instance.SaveSettings();
		if (this.uiSystemModule != null)
		{
			this.uiSystemModule.HideOptionsWindow();
		}
	}

	// Token: 0x04000B68 RID: 2920
	[SerializeField]
	private Toggle muteToggle;

	// Token: 0x04000B69 RID: 2921
	[SerializeField]
	private Slider sfxVolumeSlider;

	// Token: 0x04000B6A RID: 2922
	[SerializeField]
	private Slider bgmVolumeSlider;

	// Token: 0x04000B6B RID: 2923
	[SerializeField]
	private Slider ambienceVolumeSlider;

	// Token: 0x04000B6C RID: 2924
	[SerializeField]
	private Slider targetFrameRateSlider;

	// Token: 0x04000B6D RID: 2925
	[SerializeField]
	private UISliderExtended uiSizeSlider;

	// Token: 0x04000B6E RID: 2926
	[SerializeField]
	private UISliderExtended cameraModeSlider;

	// Token: 0x04000B6F RID: 2927
	[SerializeField]
	private UISliderExtended languageSlider;

	// Token: 0x04000B70 RID: 2928
	[SerializeField]
	private UISliderExtended minimapDetailsSlider;

	// Token: 0x04000B71 RID: 2929
	[SerializeField]
	private Toggle fullScreenToggle;

	// Token: 0x04000B72 RID: 2930
	[SerializeField]
	private Toggle showAllNamesToggle;

	// Token: 0x04000B73 RID: 2931
	[SerializeField]
	private Toggle showInfoOnChatToggle;

	// Token: 0x04000B74 RID: 2932
	[SerializeField]
	private Toggle chatWithReturnKeyToggle;

	// Token: 0x04000B75 RID: 2933
	[SerializeField]
	private Toggle disableDropProtectionToggle;

	// Token: 0x04000B76 RID: 2934
	[SerializeField]
	private Toggle fixedJoystickToggle;

	// Token: 0x04000B77 RID: 2935
	[SerializeField]
	private Toggle disableLightSystemToggle;

	// Token: 0x04000B78 RID: 2936
	[SerializeField]
	private Toggle disableVisualEffectsToggle;

	// Token: 0x04000B79 RID: 2937
	[SerializeField]
	private GameObject keyboardControlsTabObject;

	// Token: 0x04000B7A RID: 2938
	private UISystemModule uiSystemModule;

	// Token: 0x04000B7B RID: 2939
	private GameEnvironmentModule gameEnvironmentModule;

	// Token: 0x04000B7C RID: 2940
	private bool loaded;
}
