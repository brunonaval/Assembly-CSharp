using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Token: 0x020002B1 RID: 689
public class AssetBundleManager : MonoBehaviour
{
	// Token: 0x06000B50 RID: 2896 RVA: 0x00033A0E File Offset: 0x00031C0E
	private void Awake()
	{
		if (AssetBundleManager.Instance == null)
		{
			AssetBundleManager.Instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06000B51 RID: 2897 RVA: 0x00033A3B File Offset: 0x00031C3B
	private void Start()
	{
		this.LoadServerBundles();
	}

	// Token: 0x06000B52 RID: 2898 RVA: 0x00033A43 File Offset: 0x00031C43
	public IEnumerator LoadClientBundles()
	{
		if (this.isServer)
		{
			yield break;
		}
		if (this.isLoaded)
		{
			yield break;
		}
		this.progressHolder.SetActive(true);
		float num = 0f;
		foreach (AssetBundleConfig assetBundleConfig in this.AssetBundlesConfig)
		{
			if (!PlayerPrefs.HasKey(assetBundleConfig.Name))
			{
				this.canStartDownload = false;
				num += assetBundleConfig.Size;
			}
			else if (PlayerPrefs.GetString(assetBundleConfig.Name) != assetBundleConfig.Version.ToString())
			{
				this.canStartDownload = false;
				num += assetBundleConfig.Size;
			}
		}
		if (!this.canStartDownload)
		{
			this.alertText.text = string.Format(LanguageManager.Instance.GetText("asset_bundle_alert_message"), num.ToString());
			this.alertHolder.SetActive(true);
		}
		yield return new WaitUntil(() => this.canStartDownload);
		yield return this.LoadEffectSpritesFromAssetBundle();
		yield return this.LoadItemIconsFromAssetBundle();
		yield return this.LoadRankIconsFromAssetBundle();
		yield return this.LoadAnimationPrefabsFromAssetBundle();
		yield return this.LoadBackgroundMusicsCacheFromAssetBundle();
		yield return this.LoadSoundEffectsFromAssetBundle();
		yield return this.LoadAmbienceSoundsFromAssetBundle();
		this.progressFill.fillAmount = 1f;
		yield return this.UpdateProgress(LanguageManager.Instance.GetText("connecting_label"), false, null);
		this.CompleteLoading();
		this.isLoaded = true;
		yield break;
	}

	// Token: 0x06000B53 RID: 2899 RVA: 0x00033A52 File Offset: 0x00031C52
	public void HideContentHolders()
	{
		this.progressHolder.SetActive(false);
		this.alertHolder.SetActive(false);
	}

	// Token: 0x06000B54 RID: 2900 RVA: 0x00033A6C File Offset: 0x00031C6C
	private void CompleteLoading()
	{
		this.EffectPrefab = (Resources.Load("Prefabs/Effects/Effect") as GameObject);
		this.AnimatedTextPrefab = (Resources.Load("Prefabs/Effects/AnimatedText") as GameObject);
		Resources.UnloadUnusedAssets();
	}

	// Token: 0x06000B55 RID: 2901 RVA: 0x00033A9E File Offset: 0x00031C9E
	public void AcceptDownload()
	{
		this.canStartDownload = true;
		this.alertHolder.SetActive(false);
	}

	// Token: 0x06000B56 RID: 2902 RVA: 0x00033AB4 File Offset: 0x00031CB4
	private void LoadServerBundles()
	{
		if (!this.isServer)
		{
			return;
		}
		foreach (GameObject gameObject in Resources.LoadAll<GameObject>("Prefabs/Projectiles/"))
		{
			this.projectilePrefabs.Add(gameObject.name, gameObject);
		}
		this.NpcPrefab = (Resources.Load("Prefabs/Npc") as GameObject);
		this.MonsterPrefab = (Resources.Load("Prefabs/Monster") as GameObject);
		this.CompleteLoading();
	}

	// Token: 0x06000B57 RID: 2903 RVA: 0x00033B29 File Offset: 0x00031D29
	private IEnumerator LoadRankIconsFromAssetBundle()
	{
		yield return this.DownloadOrLoadCacheFromAssetBundle("rank_icons", "rank_icons_downloading_message", delegate(AssetBundle bundle)
		{
			this.rankIconSpritesBundle = bundle;
		});
		yield break;
	}

	// Token: 0x06000B58 RID: 2904 RVA: 0x00033B38 File Offset: 0x00031D38
	private IEnumerator LoadAmbienceSoundsFromAssetBundle()
	{
		yield return this.DownloadOrLoadCacheFromAssetBundle("ambience_sounds", "ambience_sounds_downloading_message", delegate(AssetBundle bundle)
		{
			this.ambienceAudioClipsBundle = bundle;
		});
		yield break;
	}

	// Token: 0x06000B59 RID: 2905 RVA: 0x00033B47 File Offset: 0x00031D47
	private IEnumerator LoadSoundEffectsFromAssetBundle()
	{
		yield return this.DownloadOrLoadCacheFromAssetBundle("sound_effects", "sound_effects_downloading_message", delegate(AssetBundle bundle)
		{
			this.effectAudioClipsBundle = bundle;
		});
		yield break;
	}

	// Token: 0x06000B5A RID: 2906 RVA: 0x00033B56 File Offset: 0x00031D56
	private IEnumerator LoadEffectSpritesFromAssetBundle()
	{
		yield return this.DownloadOrLoadCacheFromAssetBundle("effect_sprites", "effect_sprites_downloading_message", delegate(AssetBundle bundle)
		{
			this.effectSpritesBundle = bundle;
		});
		yield break;
	}

	// Token: 0x06000B5B RID: 2907 RVA: 0x00033B65 File Offset: 0x00031D65
	private IEnumerator LoadItemIconsFromAssetBundle()
	{
		yield return this.DownloadOrLoadCacheFromAssetBundle("item_icons", "item_icons_downloading_message", delegate(AssetBundle bundle)
		{
			this.itemIconsSpritesBundle = bundle;
		});
		yield break;
	}

	// Token: 0x06000B5C RID: 2908 RVA: 0x00033B74 File Offset: 0x00031D74
	private IEnumerator LoadAnimationPrefabsFromAssetBundle()
	{
		yield return this.DownloadOrLoadCacheFromAssetBundle("animation_prefabs", "animation_prefabs_downloading_message", delegate(AssetBundle bundle)
		{
			this.animationPrefabsBundle = bundle;
		});
		yield break;
	}

	// Token: 0x06000B5D RID: 2909 RVA: 0x00033B83 File Offset: 0x00031D83
	private IEnumerator LoadBackgroundMusicsCacheFromAssetBundle()
	{
		yield return this.DownloadOrLoadCacheFromAssetBundle("background_musics", "background_musics_downloading_message", delegate(AssetBundle bundle)
		{
			this.bgmAudioClipsBundle = bundle;
		});
		yield break;
	}

	// Token: 0x06000B5E RID: 2910 RVA: 0x00033B92 File Offset: 0x00031D92
	private IEnumerator DownloadOrLoadCacheFromAssetBundle(string bundleName, string downloadMessage, Action<AssetBundle> callback)
	{
		AssetBundleConfig bundleConfig = this.GetBundleConfig(bundleName);
		string text = this.ExtractBundlePlatformUrl(bundleConfig);
		if (string.IsNullOrEmpty(text))
		{
			yield return this.UpdateProgress("ERROR: INVALID BUNDLE DOWNLOAD URL FOR: " + bundleName, false, null);
			yield break;
		}
		using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(text, bundleConfig.Version, 0U))
		{
			Coroutine progressCoroutine = base.StartCoroutine(this.UpdateProgress(LanguageManager.Instance.GetText(downloadMessage), true, www));
			yield return www.SendWebRequest();
			base.StopCoroutine(progressCoroutine);
			if (www.result == UnityWebRequest.Result.ConnectionError)
			{
				yield return this.UpdateProgress("NETWORK ERROR", false, null);
				yield break;
			}
			AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
			if (bundle == null)
			{
				yield return this.UpdateProgress("ERROR: NOT A VALID BUNDLE", false, null);
				yield break;
			}
			yield return this.UpdateProgress(LanguageManager.Instance.GetText("extracting_bundle_message"), false, null);
			if (callback != null)
			{
				callback(bundle);
			}
			PlayerPrefs.SetString(bundleConfig.Name, bundleConfig.Version.ToString());
			www.Dispose();
			progressCoroutine = null;
			bundle = null;
		}
		UnityWebRequest www = null;
		yield break;
		yield break;
	}

	// Token: 0x06000B5F RID: 2911 RVA: 0x00033BB8 File Offset: 0x00031DB8
	private AssetBundleConfig GetBundleConfig(string bundleConfigName)
	{
		return this.AssetBundlesConfig.FirstOrDefault((AssetBundleConfig abc) => abc.Name == bundleConfigName);
	}

	// Token: 0x06000B60 RID: 2912 RVA: 0x00033BEC File Offset: 0x00031DEC
	private string ExtractBundlePlatformUrl(AssetBundleConfig bundleConfig)
	{
		if (bundleConfig == null)
		{
			return string.Empty;
		}
		if (bundleConfig.PlatformUrls.Length == 0)
		{
			return string.Empty;
		}
		AssetBundlePlatformUrl assetBundlePlatformUrl = bundleConfig.PlatformUrls.FirstOrDefault((AssetBundlePlatformUrl pu) => pu.Platform == this.GetRuntimePlatform());
		if (assetBundlePlatformUrl == null)
		{
			return bundleConfig.PlatformUrls[0].DownloadUrl;
		}
		return assetBundlePlatformUrl.DownloadUrl;
	}

	// Token: 0x06000B61 RID: 2913 RVA: 0x00033C40 File Offset: 0x00031E40
	private RuntimePlatform GetRuntimePlatform()
	{
		RuntimePlatform platform = Application.platform;
		if (platform == RuntimePlatform.OSXEditor)
		{
			return RuntimePlatform.OSXPlayer;
		}
		if (platform == RuntimePlatform.WindowsEditor)
		{
			return RuntimePlatform.WindowsPlayer;
		}
		if (platform != RuntimePlatform.LinuxEditor)
		{
			return Application.platform;
		}
		return RuntimePlatform.LinuxPlayer;
	}

	// Token: 0x06000B62 RID: 2914 RVA: 0x00033C6D File Offset: 0x00031E6D
	private IEnumerator UpdateProgress(string progressDisplay, bool isDownloading, UnityWebRequest request = null)
	{
		if (this.isServer)
		{
			yield break;
		}
		float totalBytes = 0f;
		while (request != null)
		{
			try
			{
				if (totalBytes == 0f)
				{
					float.TryParse(request.GetResponseHeader("content-length"), out totalBytes);
				}
				float num = Mathf.Clamp(request.downloadProgress * 100f, 0f, 100f);
				float num2 = totalBytes / 1024f / 1024f;
				float num3 = request.downloadedBytes / 1024f / 1024f;
				if (isDownloading & Application.platform != RuntimePlatform.IPhonePlayer)
				{
					this.progressText.text = string.Format("{0:0.00}% ({1:0.00}mb / {2:0.00}mb) - {3}", new object[]
					{
						num,
						num3,
						num2,
						progressDisplay
					});
				}
				else
				{
					this.progressText.text = string.Format("{0:0.00}% - {1}", num, progressDisplay);
				}
				this.progressFill.fillAmount = request.downloadProgress;
				if (request.downloadProgress >= 1f)
				{
					yield break;
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			yield return new WaitForSecondsRealtime(0.0001f);
		}
		this.progressText.text = progressDisplay;
		yield break;
		yield break;
	}

	// Token: 0x06000B63 RID: 2915 RVA: 0x00033C91 File Offset: 0x00031E91
	public Sprite GetRankIconSprite(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return null;
		}
		if (this.rankIconSpritesBundle == null)
		{
			return null;
		}
		return this.rankIconSpritesBundle.LoadAsset<Sprite>(name);
	}

	// Token: 0x06000B64 RID: 2916 RVA: 0x00033CB9 File Offset: 0x00031EB9
	public Sprite GetItemIconSprite(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return null;
		}
		if (this.itemIconsSpritesBundle == null)
		{
			return null;
		}
		return this.itemIconsSpritesBundle.LoadAsset<Sprite>(name);
	}

	// Token: 0x06000B65 RID: 2917 RVA: 0x00033CE1 File Offset: 0x00031EE1
	public GameObject GetProjectilePrefab(string name)
	{
		if (this.projectilePrefabs.ContainsKey(name))
		{
			return this.projectilePrefabs[name];
		}
		return null;
	}

	// Token: 0x06000B66 RID: 2918 RVA: 0x00033CFF File Offset: 0x00031EFF
	public string[] GetProjectilePrefabNames()
	{
		return this.projectilePrefabs.Keys.ToArray<string>();
	}

	// Token: 0x06000B67 RID: 2919 RVA: 0x00033D11 File Offset: 0x00031F11
	public GameObject GetAnimationPrefab(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return null;
		}
		if (this.animationPrefabsBundle == null)
		{
			return null;
		}
		return this.animationPrefabsBundle.LoadAsset<GameObject>(name);
	}

	// Token: 0x06000B68 RID: 2920 RVA: 0x00033D39 File Offset: 0x00031F39
	public AudioClip GetEffectAudioClip(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return null;
		}
		if (this.effectAudioClipsBundle == null)
		{
			return null;
		}
		return this.effectAudioClipsBundle.LoadAsset<AudioClip>(name);
	}

	// Token: 0x06000B69 RID: 2921 RVA: 0x00033D61 File Offset: 0x00031F61
	public AudioClip GetBgmAudioClip(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return null;
		}
		if (this.bgmAudioClipsBundle == null)
		{
			return null;
		}
		return this.bgmAudioClipsBundle.LoadAsset<AudioClip>(name);
	}

	// Token: 0x06000B6A RID: 2922 RVA: 0x00033D89 File Offset: 0x00031F89
	public AudioClip GetAmbienceAudioClip(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return null;
		}
		if (this.ambienceAudioClipsBundle == null)
		{
			return null;
		}
		return this.ambienceAudioClipsBundle.LoadAsset<AudioClip>(name);
	}

	// Token: 0x06000B6B RID: 2923 RVA: 0x00033DB1 File Offset: 0x00031FB1
	public Sprite[] GetEffectSprites(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return null;
		}
		if (this.effectSpritesBundle == null)
		{
			return Array.Empty<Sprite>();
		}
		return this.effectSpritesBundle.LoadAssetWithSubAssets<Sprite>(name);
	}

	// Token: 0x04000CAF RID: 3247
	public static AssetBundleManager Instance;

	// Token: 0x04000CB0 RID: 3248
	private readonly Dictionary<string, GameObject> projectilePrefabs = new Dictionary<string, GameObject>();

	// Token: 0x04000CB1 RID: 3249
	private AssetBundle bgmAudioClipsBundle;

	// Token: 0x04000CB2 RID: 3250
	private AssetBundle effectSpritesBundle;

	// Token: 0x04000CB3 RID: 3251
	private AssetBundle rankIconSpritesBundle;

	// Token: 0x04000CB4 RID: 3252
	private AssetBundle animationPrefabsBundle;

	// Token: 0x04000CB5 RID: 3253
	private AssetBundle effectAudioClipsBundle;

	// Token: 0x04000CB6 RID: 3254
	private AssetBundle itemIconsSpritesBundle;

	// Token: 0x04000CB7 RID: 3255
	private AssetBundle ambienceAudioClipsBundle;

	// Token: 0x04000CB8 RID: 3256
	[HideInInspector]
	public GameObject EffectPrefab;

	// Token: 0x04000CB9 RID: 3257
	[HideInInspector]
	public GameObject AnimatedTextPrefab;

	// Token: 0x04000CBA RID: 3258
	[HideInInspector]
	public GameObject MonsterPrefab;

	// Token: 0x04000CBB RID: 3259
	[HideInInspector]
	public GameObject NpcPrefab;

	// Token: 0x04000CBC RID: 3260
	[SerializeField]
	private GameObject progressHolder;

	// Token: 0x04000CBD RID: 3261
	[SerializeField]
	private GameObject alertHolder;

	// Token: 0x04000CBE RID: 3262
	[SerializeField]
	private Text alertText;

	// Token: 0x04000CBF RID: 3263
	[SerializeField]
	private GameObject mainContentHolder;

	// Token: 0x04000CC0 RID: 3264
	[SerializeField]
	private Image progressFill;

	// Token: 0x04000CC1 RID: 3265
	[SerializeField]
	private Text progressText;

	// Token: 0x04000CC2 RID: 3266
	[SerializeField]
	private AssetBundleConfig[] AssetBundlesConfig;

	// Token: 0x04000CC3 RID: 3267
	[SerializeField]
	private bool isServer;

	// Token: 0x04000CC4 RID: 3268
	private bool canStartDownload = true;

	// Token: 0x04000CC5 RID: 3269
	private bool isLoaded;
}
