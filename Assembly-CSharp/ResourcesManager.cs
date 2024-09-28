using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200040D RID: 1037
public class ResourcesManager : MonoBehaviour
{
	// Token: 0x0600166A RID: 5738 RVA: 0x000729D2 File Offset: 0x00070BD2
	private void Awake()
	{
		if (ResourcesManager.Instance == null)
		{
			ResourcesManager.Instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x0600166B RID: 5739 RVA: 0x000729FF File Offset: 0x00070BFF
	private void Start()
	{
		this.LoadVocationIconsFromResources();
		this.LoadVocationSpritesFromResources();
	}

	// Token: 0x0600166C RID: 5740 RVA: 0x00072A10 File Offset: 0x00070C10
	private void LoadVocationIconsFromResources()
	{
		foreach (Sprite sprite in Resources.LoadAll<Sprite>("Icons/Vocations/"))
		{
			this.vocationIconSprites.Add(sprite.name, sprite);
		}
	}

	// Token: 0x0600166D RID: 5741 RVA: 0x00072A4C File Offset: 0x00070C4C
	private void LoadVocationSpritesFromResources()
	{
		foreach (Sprite sprite in Resources.LoadAll<Sprite>("Sprites/Vocations/"))
		{
			this.vocationSprites.Add(sprite.name, sprite);
		}
	}

	// Token: 0x0600166E RID: 5742 RVA: 0x00072A88 File Offset: 0x00070C88
	public Sprite GetVocationIconSprite(string name)
	{
		if (this.vocationIconSprites.ContainsKey(name))
		{
			return this.vocationIconSprites[name];
		}
		Sprite sprite = Resources.Load<Sprite>("Icons/Vocations/" + name);
		if (sprite != null)
		{
			return sprite;
		}
		return null;
	}

	// Token: 0x0600166F RID: 5743 RVA: 0x00072AD0 File Offset: 0x00070CD0
	public Sprite GetVocationSprite(string name)
	{
		if (this.vocationSprites.ContainsKey(name))
		{
			return this.vocationSprites[name];
		}
		Sprite sprite = Resources.Load<Sprite>("Sprites/Vocations/" + name);
		if (sprite != null)
		{
			return sprite;
		}
		return null;
	}

	// Token: 0x04001438 RID: 5176
	public static ResourcesManager Instance;

	// Token: 0x04001439 RID: 5177
	private readonly Dictionary<string, Sprite> vocationSprites = new Dictionary<string, Sprite>();

	// Token: 0x0400143A RID: 5178
	private readonly Dictionary<string, Sprite> vocationIconSprites = new Dictionary<string, Sprite>();
}
