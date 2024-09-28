using System;
using System.Collections;
using Mirror;
using UnityEngine;

// Token: 0x020002AD RID: 685
public class AreaModule : MonoBehaviour
{
	// Token: 0x17000125 RID: 293
	// (get) Token: 0x06000B1C RID: 2844 RVA: 0x00032BEC File Offset: 0x00030DEC
	public AudioSource BgmAudioSource
	{
		get
		{
			if (!this.networkIdentity.isLocalPlayer)
			{
				return null;
			}
			if (this._bgmAudioSource == null)
			{
				GameObject gameObject = GameObject.FindGameObjectWithTag("BGM");
				if (gameObject == null)
				{
					return null;
				}
				this._bgmAudioSource = gameObject.GetComponent<AudioSource>();
			}
			return this._bgmAudioSource;
		}
	}

	// Token: 0x17000126 RID: 294
	// (get) Token: 0x06000B1D RID: 2845 RVA: 0x00032C40 File Offset: 0x00030E40
	public AudioSource AmbienceAudioSource
	{
		get
		{
			if (!this.networkIdentity.isLocalPlayer)
			{
				return null;
			}
			if (this._ambienceAudioSource == null)
			{
				GameObject gameObject = GameObject.FindGameObjectWithTag("Ambience");
				if (gameObject == null)
				{
					return null;
				}
				this._ambienceAudioSource = gameObject.GetComponent<AudioSource>();
			}
			return this._ambienceAudioSource;
		}
	}

	// Token: 0x17000127 RID: 295
	// (get) Token: 0x06000B1E RID: 2846 RVA: 0x00032C94 File Offset: 0x00030E94
	public AreaType AreaType
	{
		get
		{
			if (string.IsNullOrEmpty(this.currentAreaType))
			{
				return AreaType.PveArea;
			}
			string a = this.currentAreaType.ToLower();
			if (a == "protected")
			{
				return AreaType.ProtectedArea;
			}
			if (a == "pve")
			{
				return AreaType.PveArea;
			}
			if (!(a == "pvp"))
			{
				if (!(a == "event"))
				{
					return AreaType.PveArea;
				}
				return AreaType.EventArea;
			}
			else
			{
				if (ServerSettingsManager.ServerType != ServerType.PvE)
				{
					return AreaType.PvpArea;
				}
				return AreaType.PveArea;
			}
		}
	}

	// Token: 0x17000128 RID: 296
	// (get) Token: 0x06000B1F RID: 2847 RVA: 0x00032D05 File Offset: 0x00030F05
	// (set) Token: 0x06000B20 RID: 2848 RVA: 0x00032D0D File Offset: 0x00030F0D
	public float CurrentAmbientLight { get; private set; }

	// Token: 0x17000129 RID: 297
	// (get) Token: 0x06000B21 RID: 2849 RVA: 0x00032D16 File Offset: 0x00030F16
	// (set) Token: 0x06000B22 RID: 2850 RVA: 0x00032D1E File Offset: 0x00030F1E
	public string CurrentArea { get; private set; }

	// Token: 0x1700012A RID: 298
	// (get) Token: 0x06000B23 RID: 2851 RVA: 0x00032D27 File Offset: 0x00030F27
	// (set) Token: 0x06000B24 RID: 2852 RVA: 0x00032D2F File Offset: 0x00030F2F
	public string CurrentAreaExtraParam { get; private set; }

	// Token: 0x1700012B RID: 299
	// (get) Token: 0x06000B25 RID: 2853 RVA: 0x00032D38 File Offset: 0x00030F38
	// (set) Token: 0x06000B26 RID: 2854 RVA: 0x00032D40 File Offset: 0x00030F40
	public float LastAreaChangeTime { get; private set; }

	// Token: 0x06000B27 RID: 2855 RVA: 0x00032D49 File Offset: 0x00030F49
	private void Awake()
	{
		this.networkIdentity = base.GetComponent<NetworkIdentity>();
		this.movementModule = base.GetComponent<MovementModule>();
		if (NetworkServer.active)
		{
			this.pvpModule = base.GetComponent<PvpModule>();
		}
	}

	// Token: 0x06000B28 RID: 2856 RVA: 0x00032D78 File Offset: 0x00030F78
	private void Start()
	{
		if (NetworkClient.active)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
			this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
			GameObject gameObject2 = GameObject.FindGameObjectWithTag("GameEnvironment");
			this.gameEnvironmentModule = gameObject2.GetComponent<GameEnvironmentModule>();
			this.DefineAreaIconsForMobilePlatform();
			this.DefineAreaIconsForDesktopPlatform();
		}
		base.InvokeRepeating("Stay2DTriggerTimer", 0f, 1f);
		base.InvokeRepeating("DetectWhenInsideWalls", 0f, 0.2f);
	}

	// Token: 0x06000B29 RID: 2857 RVA: 0x00032DF0 File Offset: 0x00030FF0
	private void Stay2DTriggerTimer()
	{
		int layerMask = 1 << LayerMask.NameToLayer("WorldArea");
		Collider2D collider2D = Physics2D.OverlapCircle(base.transform.position, 0.32f, layerMask);
		if (collider2D == null)
		{
			this.LeftServerArea();
			this.LeftClientArea();
			return;
		}
		this.ServerAreaDetected(collider2D);
		this.ServerDetectPkInEventArea();
		this.ClientAreaDetected(collider2D);
		this.LoadNewMapScene(collider2D);
	}

	// Token: 0x06000B2A RID: 2858 RVA: 0x00032E5C File Offset: 0x0003105C
	private void ServerDetectPkInEventArea()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		if (this.pvpModule.PvpStatus != PvpStatus.PlayerKiller & this.pvpModule.PvpStatus != PvpStatus.Outlaw)
		{
			return;
		}
		if (this.AreaType != AreaType.EventArea)
		{
			return;
		}
		Vector3 spawnPointLocation = this.movementModule.SpawnPointLocation;
		this.movementModule.TargetTeleport(this.movementModule.connectionToClient, spawnPointLocation, default(Effect));
	}

	// Token: 0x06000B2B RID: 2859 RVA: 0x00032ED0 File Offset: 0x000310D0
	private void ClientDetectWhenInsideWalls()
	{
		if (!NetworkClient.active)
		{
			return;
		}
		if (base.transform.position.Equals(this.lastValidPosition))
		{
			return;
		}
		int layerMask = 1 << LayerMask.NameToLayer("Tile");
		if (!(Physics2D.OverlapPoint(base.transform.position, layerMask) != null))
		{
			this.lastValidPosition = base.transform.position;
			return;
		}
		if (this.lastValidPosition == Vector3.zero)
		{
			this.movementModule.Teleport(this.movementModule.SpawnPointLocation, default(Effect));
			return;
		}
		this.movementModule.Teleport(this.lastValidPosition, default(Effect));
	}

	// Token: 0x06000B2C RID: 2860 RVA: 0x00032F90 File Offset: 0x00031190
	private void DetectWhenInsideWalls()
	{
		if (base.transform.position.Equals(this.lastValidPosition))
		{
			return;
		}
		int layerMask = 1 << LayerMask.NameToLayer("Tile");
		if (!(Physics2D.OverlapPoint(base.transform.position, layerMask) != null))
		{
			this.lastValidPosition = base.transform.position;
			return;
		}
		Vector3 spawnPointLocation = this.movementModule.SpawnPointLocation;
		if (this.lastValidPosition != Vector3.zero)
		{
			spawnPointLocation = this.lastValidPosition;
		}
		if (NetworkClient.active)
		{
			this.movementModule.Teleport(spawnPointLocation, default(Effect));
			return;
		}
		if (NetworkServer.active)
		{
			this.movementModule.TargetTeleport(this.movementModule.connectionToClient, spawnPointLocation, default(Effect));
		}
	}

	// Token: 0x06000B2D RID: 2861 RVA: 0x00033064 File Offset: 0x00031264
	private void LoadNewMapScene(Collider2D collision)
	{
		if (!this.networkIdentity.isServer)
		{
			return;
		}
		WorldAreaModule component = collision.GetComponent<WorldAreaModule>();
		if (component == null)
		{
			return;
		}
		string name = component.gameObject.scene.name;
		if (this.loadedScene == name)
		{
			return;
		}
		this.loadedScene = name;
		base.GetComponent<PlayerModule>().TargetChangePlayerSceneMap(component.gameObject.scene.name);
	}

	// Token: 0x06000B2E RID: 2862 RVA: 0x000330D8 File Offset: 0x000312D8
	public void LeftServerArea()
	{
		if (!this.networkIdentity.isServer)
		{
			return;
		}
		this.CurrentArea = string.Empty;
		this.currentAreaType = string.Empty;
		this.CurrentAreaExtraParam = string.Empty;
	}

	// Token: 0x06000B2F RID: 2863 RVA: 0x00033109 File Offset: 0x00031309
	public void LeftClientArea()
	{
		if (!this.networkIdentity.isLocalPlayer)
		{
			return;
		}
		if (this.leftClientAreaRoutine != null)
		{
			return;
		}
		this.leftClientAreaRoutine = base.StartCoroutine(this.LeftClientAreaAsync());
	}

	// Token: 0x06000B30 RID: 2864 RVA: 0x00033134 File Offset: 0x00031334
	private IEnumerator LeftClientAreaAsync()
	{
		yield return new WaitForSeconds(1f);
		this.StopBgm();
		this.StopAmbience();
		this.currentBgm = string.Empty;
		this.CurrentArea = string.Empty;
		this.CurrentAreaExtraParam = string.Empty;
		this.currentAreaType = string.Empty;
		this.currentAmbience = string.Empty;
		yield break;
	}

	// Token: 0x06000B31 RID: 2865 RVA: 0x00033144 File Offset: 0x00031344
	public void ServerAreaDetected(Collider2D collision)
	{
		if (!this.networkIdentity.isServer)
		{
			return;
		}
		WorldAreaModule component = collision.GetComponent<WorldAreaModule>();
		this.currentAreaType = component.AreaType;
		this.CurrentAreaExtraParam = component.ExtraParam;
		this.CurrentAmbientLight = component.AmbientLight;
		if (this.CurrentArea != component.AreaName)
		{
			this.CurrentArea = component.AreaName;
			this.LastAreaChangeTime = Time.time;
		}
	}

	// Token: 0x06000B32 RID: 2866 RVA: 0x000331B4 File Offset: 0x000313B4
	public void ClientAreaDetected(Collider2D collision)
	{
		if (!this.networkIdentity.isLocalPlayer)
		{
			return;
		}
		if (this.leftClientAreaRoutine != null)
		{
			base.StopCoroutine(this.leftClientAreaRoutine);
			this.leftClientAreaRoutine = null;
		}
		WorldAreaModule component = collision.GetComponent<WorldAreaModule>();
		if (this.currentBgm != component.BgmName)
		{
			base.StartCoroutine(this.StartBgm(component.BgmName));
		}
		string text = this.gameEnvironmentModule.IsNight ? component.NightAmbienceName : component.DayAmbienceName;
		if (this.currentAmbience != text)
		{
			base.StartCoroutine(this.StartAmbience(text));
		}
		this.currentAmbience = text;
		this.currentBgm = component.BgmName;
		this.currentAreaType = component.AreaType;
		this.CurrentAreaExtraParam = component.ExtraParam;
		this.CurrentAmbientLight = component.AmbientLight;
		if (this.CurrentArea != component.AreaName)
		{
			this.CurrentArea = component.AreaName;
			this.LastAreaChangeTime = Time.time;
			string text2 = LanguageManager.Instance.GetText(component.AreaName);
			string areaTypeName = this.GetAreaTypeName();
			string areaTypeColor = this.GetAreaTypeColor();
			this.uiSystemModule.ShowTimedFeedback(string.Concat(new string[]
			{
				text2,
				"\r\n<color=",
				areaTypeColor,
				"><size=60>-",
				areaTypeName,
				"-</size></color>"
			}), 5f, false);
			this.uiSystemModule.PlatformCurrentWorldAreaName.text = text2.ToLower();
		}
		this.UpdateAreaTypeIcon();
	}

	// Token: 0x06000B33 RID: 2867 RVA: 0x00033330 File Offset: 0x00031530
	private void UpdateAreaTypeIcon()
	{
		this.protectedAreaIcon.SetActive(false);
		this.pveAreaIcon.SetActive(false);
		this.pvpAreaIcon.SetActive(false);
		this.eventAreaIcon.SetActive(false);
		switch (this.AreaType)
		{
		case AreaType.ProtectedArea:
			this.protectedAreaIcon.SetActive(true);
			return;
		case AreaType.PveArea:
			this.pveAreaIcon.SetActive(true);
			return;
		case AreaType.PvpArea:
			this.pvpAreaIcon.SetActive(true);
			return;
		case AreaType.EventArea:
			this.eventAreaIcon.SetActive(true);
			return;
		default:
			this.pveAreaIcon.SetActive(true);
			return;
		}
	}

	// Token: 0x06000B34 RID: 2868 RVA: 0x000333CC File Offset: 0x000315CC
	private string GetAreaTypeColor()
	{
		switch (this.AreaType)
		{
		case AreaType.ProtectedArea:
			return "green";
		case AreaType.PveArea:
			return "orange";
		case AreaType.PvpArea:
			return "red";
		case AreaType.EventArea:
			return "purple";
		default:
			return "orange";
		}
	}

	// Token: 0x06000B35 RID: 2869 RVA: 0x00033418 File Offset: 0x00031618
	private string GetAreaTypeName()
	{
		switch (this.AreaType)
		{
		case AreaType.ProtectedArea:
			return LanguageManager.Instance.GetText("protected_area_type");
		case AreaType.PveArea:
			return LanguageManager.Instance.GetText("pve_area_type");
		case AreaType.PvpArea:
			return LanguageManager.Instance.GetText("pvp_area_type");
		case AreaType.EventArea:
			return LanguageManager.Instance.GetText("event_area_type");
		default:
			return LanguageManager.Instance.GetText("pve_area_type");
		}
	}

	// Token: 0x06000B36 RID: 2870 RVA: 0x00033493 File Offset: 0x00031693
	private IEnumerator StartBgm(string bgmName)
	{
		this.UpdateVolumes();
		WaitForSeconds delay = new WaitForSeconds(0.1f);
		float localVolume = this.BgmAudioSource.volume;
		while (localVolume > 0f)
		{
			localVolume -= 0.05f;
			this.BgmAudioSource.volume = localVolume;
			yield return delay;
		}
		this.BgmAudioSource.loop = true;
		this.BgmAudioSource.clip = AssetBundleManager.Instance.GetBgmAudioClip(bgmName);
		this.BgmAudioSource.Play();
		localVolume = this._bgmAudioSource.volume;
		while (localVolume < SettingsManager.Instance.BgmVolume & !SettingsManager.Instance.Mute)
		{
			localVolume += 0.05f;
			this.BgmAudioSource.volume = localVolume;
			yield return delay;
		}
		yield break;
	}

	// Token: 0x06000B37 RID: 2871 RVA: 0x000334A9 File Offset: 0x000316A9
	private void StopBgm()
	{
		this.BgmAudioSource.clip = null;
		this.BgmAudioSource.Stop();
	}

	// Token: 0x06000B38 RID: 2872 RVA: 0x000334C2 File Offset: 0x000316C2
	private IEnumerator StartAmbience(string ambienceName)
	{
		this.UpdateVolumes();
		WaitForSeconds delay = new WaitForSeconds(0.1f);
		float localVolume = this.AmbienceAudioSource.volume;
		while (localVolume > 0f)
		{
			localVolume -= 0.1f;
			this.AmbienceAudioSource.volume = localVolume;
			yield return delay;
		}
		this.AmbienceAudioSource.loop = true;
		this.AmbienceAudioSource.clip = AssetBundleManager.Instance.GetAmbienceAudioClip(ambienceName);
		this.AmbienceAudioSource.Play();
		localVolume = this.AmbienceAudioSource.volume;
		while (localVolume < SettingsManager.Instance.AmbienceVolume & !SettingsManager.Instance.Mute)
		{
			localVolume += 0.05f;
			this.AmbienceAudioSource.volume = localVolume;
			yield return delay;
		}
		yield break;
	}

	// Token: 0x06000B39 RID: 2873 RVA: 0x000334D8 File Offset: 0x000316D8
	private void StopAmbience()
	{
		this.AmbienceAudioSource.clip = null;
		this.AmbienceAudioSource.Stop();
	}

	// Token: 0x06000B3A RID: 2874 RVA: 0x000334F4 File Offset: 0x000316F4
	private void DefineAreaIconsForDesktopPlatform()
	{
		if (GlobalSettings.IsMobilePlatform)
		{
			return;
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("UIHud");
		this.protectedAreaIcon = gameObject.transform.Find("Minimap/Protected Area Icon").gameObject;
		this.pveAreaIcon = gameObject.transform.Find("Minimap/PVE Area Icon").gameObject;
		this.pvpAreaIcon = gameObject.transform.Find("Minimap/PVP Area Icon").gameObject;
		this.eventAreaIcon = gameObject.transform.Find("Minimap/Event Area Icon").gameObject;
	}

	// Token: 0x06000B3B RID: 2875 RVA: 0x00033580 File Offset: 0x00031780
	private void DefineAreaIconsForMobilePlatform()
	{
		if (!GlobalSettings.IsMobilePlatform)
		{
			return;
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("MobileHUD");
		this.protectedAreaIcon = gameObject.transform.Find("HudHolder/Minimap/Protected Area Icon").gameObject;
		this.pveAreaIcon = gameObject.transform.Find("HudHolder/Minimap/PVE Area Icon").gameObject;
		this.pvpAreaIcon = gameObject.transform.Find("HudHolder/Minimap/PVP Area Icon").gameObject;
		this.eventAreaIcon = gameObject.transform.Find("HudHolder/Minimap/Event Area Icon").gameObject;
	}

	// Token: 0x06000B3C RID: 2876 RVA: 0x0003360C File Offset: 0x0003180C
	public void UpdateVolumes()
	{
		if (this.BgmAudioSource == null)
		{
			return;
		}
		if (this.AmbienceAudioSource == null)
		{
			return;
		}
		if (SettingsManager.Instance.Mute)
		{
			this.BgmAudioSource.volume = 0f;
			this.AmbienceAudioSource.volume = 0f;
			return;
		}
		this.BgmAudioSource.volume = SettingsManager.Instance.BgmVolume;
		this.AmbienceAudioSource.volume = SettingsManager.Instance.AmbienceVolume;
	}

	// Token: 0x04000C8B RID: 3211
	private string currentBgm;

	// Token: 0x04000C8C RID: 3212
	private string loadedScene;

	// Token: 0x04000C8D RID: 3213
	private string currentAreaType;

	// Token: 0x04000C8E RID: 3214
	private string currentAmbience;

	// Token: 0x04000C8F RID: 3215
	private Vector3 lastValidPosition;

	// Token: 0x04000C90 RID: 3216
	private UISystemModule uiSystemModule;

	// Token: 0x04000C91 RID: 3217
	private NetworkIdentity networkIdentity;

	// Token: 0x04000C92 RID: 3218
	private Coroutine leftClientAreaRoutine;

	// Token: 0x04000C93 RID: 3219
	private GameEnvironmentModule gameEnvironmentModule;

	// Token: 0x04000C94 RID: 3220
	private PvpModule pvpModule;

	// Token: 0x04000C95 RID: 3221
	private MovementModule movementModule;

	// Token: 0x04000C96 RID: 3222
	private AudioSource _bgmAudioSource;

	// Token: 0x04000C97 RID: 3223
	private AudioSource _ambienceAudioSource;

	// Token: 0x04000C98 RID: 3224
	private GameObject protectedAreaIcon;

	// Token: 0x04000C99 RID: 3225
	private GameObject pveAreaIcon;

	// Token: 0x04000C9A RID: 3226
	private GameObject pvpAreaIcon;

	// Token: 0x04000C9B RID: 3227
	private GameObject eventAreaIcon;
}
