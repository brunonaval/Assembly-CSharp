using System;
using System.IO;
using UnityEngine;

// Token: 0x0200023B RID: 571
public class SettingsManager : MonoBehaviour
{
	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x0600080D RID: 2061 RVA: 0x00026C7C File Offset: 0x00024E7C
	// (set) Token: 0x0600080E RID: 2062 RVA: 0x00026C84 File Offset: 0x00024E84
	public bool IsLoaded { get; private set; }

	// Token: 0x0600080F RID: 2063 RVA: 0x00026C8D File Offset: 0x00024E8D
	private void Awake()
	{
		if (SettingsManager.Instance == null)
		{
			SettingsManager.Instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06000810 RID: 2064 RVA: 0x00026CBC File Offset: 0x00024EBC
	public void CreateSettingsFile(Language language)
	{
		string path = Path.Combine(Application.persistentDataPath, "client.json");
		string contents = JsonUtility.ToJson(new ClientSettings
		{
			Mute = (this.Mute = false),
			FullScreen = (this.FullScreen = GlobalSettings.IsMobilePlatform),
			ShowAllNames = (this.ShowAllNames = true),
			FixedJoystick = (this.FixedJoystick = true),
			ShowInfoOnChat = (this.ShowInfoOnChat = true),
			ToggleChatWithReturnKey = (this.ToggleChatWithReturnKey = false),
			DisableDropProtection = (this.DisableDropProtection = false),
			DisableLightSystem = (this.DisableLightSystem = false),
			DisableVisualEffects = (this.DisableVisualEffects = false),
			BgmVolume = (this.BgmVolume = 0.7f),
			SfxVolume = (this.SfxVolume = 1f),
			AmbienceVolume = (this.AmbienceVolume = 0.85f),
			TargetFrameRate = 30,
			UiSize = (GlobalSettings.IsMobilePlatform ? UiSize.Huge : UiSize.Medium),
			MinimapDetails = MinimapDetails.High,
			CameraMode = (GlobalSettings.IsMobilePlatform ? CameraMode.Near : CameraMode.Normal),
			Language = language
		});
		File.WriteAllText(path, contents);
	}

	// Token: 0x06000811 RID: 2065 RVA: 0x00026DF1 File Offset: 0x00024FF1
	public bool SettingsExists()
	{
		return File.Exists(Path.Combine(Application.persistentDataPath, "client.json"));
	}

	// Token: 0x06000812 RID: 2066 RVA: 0x00026E08 File Offset: 0x00025008
	public void LoadSettings(Language? language = null)
	{
		try
		{
			string path = Path.Combine(Application.persistentDataPath, "client.json");
			if (File.Exists(path))
			{
				ClientSettings clientSettings = JsonUtility.FromJson<ClientSettings>(File.ReadAllText(path));
				this.Mute = clientSettings.Mute;
				this.FullScreen = clientSettings.FullScreen;
				this.ShowAllNames = clientSettings.ShowAllNames;
				this.FixedJoystick = clientSettings.FixedJoystick;
				this.ShowInfoOnChat = clientSettings.ShowInfoOnChat;
				this.ToggleChatWithReturnKey = clientSettings.ToggleChatWithReturnKey;
				this.DisableDropProtection = clientSettings.DisableDropProtection;
				this.DisableLightSystem = clientSettings.DisableLightSystem;
				this.DisableVisualEffects = clientSettings.DisableVisualEffects;
				this.SfxVolume = clientSettings.SfxVolume;
				this.BgmVolume = clientSettings.BgmVolume;
				this.AmbienceVolume = clientSettings.AmbienceVolume;
				this.TargetFrameRate = clientSettings.TargetFrameRate;
				this.UiSize = clientSettings.UiSize;
				this.MinimapDetails = clientSettings.MinimapDetails;
				this.CameraMode = clientSettings.CameraMode;
				this.Language = clientSettings.Language;
			}
			else
			{
				this.CreateSettingsFile(language.GetValueOrDefault());
				this.LoadSettings(null);
			}
		}
		finally
		{
			this.IsLoaded = true;
		}
	}

	// Token: 0x06000813 RID: 2067 RVA: 0x00026F4C File Offset: 0x0002514C
	public void SaveSettings()
	{
		string path = Path.Combine(Application.persistentDataPath, "client.json");
		string contents = JsonUtility.ToJson(new ClientSettings
		{
			Mute = this.Mute,
			FullScreen = this.FullScreen,
			ShowAllNames = this.ShowAllNames,
			FixedJoystick = this.FixedJoystick,
			ShowInfoOnChat = this.ShowInfoOnChat,
			ToggleChatWithReturnKey = this.ToggleChatWithReturnKey,
			DisableDropProtection = this.DisableDropProtection,
			DisableLightSystem = this.DisableLightSystem,
			DisableVisualEffects = this.DisableVisualEffects,
			BgmVolume = this.BgmVolume,
			SfxVolume = this.SfxVolume,
			AmbienceVolume = this.AmbienceVolume,
			TargetFrameRate = this.TargetFrameRate,
			UiSize = this.UiSize,
			MinimapDetails = this.MinimapDetails,
			CameraMode = this.CameraMode,
			Language = this.Language
		});
		File.WriteAllText(path, contents);
	}

	// Token: 0x040009E8 RID: 2536
	public static SettingsManager Instance;

	// Token: 0x040009E9 RID: 2537
	public bool Mute;

	// Token: 0x040009EA RID: 2538
	public bool FullScreen;

	// Token: 0x040009EB RID: 2539
	public bool ShowAllNames;

	// Token: 0x040009EC RID: 2540
	public bool FixedJoystick;

	// Token: 0x040009ED RID: 2541
	public bool ShowInfoOnChat;

	// Token: 0x040009EE RID: 2542
	public bool ToggleChatWithReturnKey;

	// Token: 0x040009EF RID: 2543
	public bool DisableDropProtection;

	// Token: 0x040009F0 RID: 2544
	public bool DisableLightSystem;

	// Token: 0x040009F1 RID: 2545
	public bool DisableVisualEffects;

	// Token: 0x040009F2 RID: 2546
	public float SfxVolume;

	// Token: 0x040009F3 RID: 2547
	public float BgmVolume;

	// Token: 0x040009F4 RID: 2548
	public float AmbienceVolume;

	// Token: 0x040009F5 RID: 2549
	public int TargetFrameRate;

	// Token: 0x040009F6 RID: 2550
	public UiSize UiSize;

	// Token: 0x040009F7 RID: 2551
	public CameraMode CameraMode;

	// Token: 0x040009F8 RID: 2552
	public Language Language;

	// Token: 0x040009F9 RID: 2553
	public MinimapDetails MinimapDetails;

	// Token: 0x040009FA RID: 2554
	public ApiAccount ApiAccount;

	// Token: 0x040009FB RID: 2555
	public Player SelectedPlayer;
}
