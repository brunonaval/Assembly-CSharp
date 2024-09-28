using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x02000202 RID: 514
public class LanguageManager : MonoBehaviour
{
	// Token: 0x170000B9 RID: 185
	// (get) Token: 0x060006B6 RID: 1718 RVA: 0x00021762 File Offset: 0x0001F962
	// (set) Token: 0x060006B7 RID: 1719 RVA: 0x0002176A File Offset: 0x0001F96A
	public bool IsReady { get; private set; }

	// Token: 0x060006B8 RID: 1720 RVA: 0x00021773 File Offset: 0x0001F973
	private void Awake()
	{
		if (LanguageManager.Instance == null)
		{
			LanguageManager.Instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x060006B9 RID: 1721 RVA: 0x000217A0 File Offset: 0x0001F9A0
	public IEnumerator LoadItemsFromWebRequest(string fileName)
	{
		this.Items = new Dictionary<string, string>();
		string uri = Path.Combine(Application.streamingAssetsPath, fileName);
		UnityWebRequest request = UnityWebRequest.Get(uri);
		request.SendWebRequest();
		yield return new WaitUntil(() => request.isDone);
		try
		{
			foreach (LanguageItem languageItem in JsonUtility.FromJson<LanguagePack>(request.downloadHandler.text).Items)
			{
				try
				{
					if (this.Items.ContainsKey(languageItem.Key))
					{
						Debug.LogWarning("The key: " + languageItem.Key + " already exists on language pack.");
					}
					else
					{
						this.Items.Add(languageItem.Key, languageItem.Value);
					}
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
			yield break;
		}
		finally
		{
			this.IsReady = true;
		}
		yield break;
	}

	// Token: 0x060006BA RID: 1722 RVA: 0x000217B6 File Offset: 0x0001F9B6
	public void LoadItems(string fileName)
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			base.StartCoroutine(this.LoadItemsFromWebRequest(fileName));
			return;
		}
		this.LoadItemsFromDisk(fileName);
	}

	// Token: 0x060006BB RID: 1723 RVA: 0x000217D8 File Offset: 0x0001F9D8
	private void LoadItemsFromDisk(string fileName)
	{
		try
		{
			this.Items = new Dictionary<string, string>();
			string path = Path.Combine(Application.streamingAssetsPath, fileName);
			if (File.Exists(path))
			{
				LanguagePack languagePack = JsonUtility.FromJson<LanguagePack>(File.ReadAllText(path));
				for (int i = 0; i < languagePack.Items.Length; i++)
				{
					LanguageItem languageItem = languagePack.Items[i];
					try
					{
						if (this.Items.ContainsKey(languageItem.Key))
						{
							Debug.LogWarningFormat("The key: {0} already exists on language pack.", new object[]
							{
								languageItem.Key
							});
						}
						else
						{
							this.Items.Add(languageItem.Key, languageItem.Value);
						}
					}
					catch (Exception ex)
					{
						Debug.LogError(string.Format("At: {0} - Item: {1}/{2} - Error: {3}", new object[]
						{
							i,
							languageItem.Key,
							languageItem.Value,
							ex
						}));
					}
				}
				Debug.LogFormat("Loaded {0} language items.", new object[]
				{
					this.Items.Count
				});
			}
		}
		finally
		{
			this.IsReady = true;
		}
	}

	// Token: 0x060006BC RID: 1724 RVA: 0x00021900 File Offset: 0x0001FB00
	public string GetText(string key)
	{
		if (string.IsNullOrEmpty(key))
		{
			return key;
		}
		if (this.Items == null | this.Items.Count < 1)
		{
			return key;
		}
		if (this.Items.ContainsKey(key))
		{
			return this.Items[key];
		}
		return key;
	}

	// Token: 0x040008DB RID: 2267
	public static LanguageManager Instance;

	// Token: 0x040008DC RID: 2268
	private Dictionary<string, string> Items;
}
