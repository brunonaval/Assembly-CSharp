using System;
using UnityEngine;

// Token: 0x020004C1 RID: 1217
public abstract class Talent : ScriptableObject
{
	// Token: 0x1700028D RID: 653
	// (get) Token: 0x06001B50 RID: 6992 RVA: 0x0008B0F1 File Offset: 0x000892F1
	public string TranslatedName
	{
		get
		{
			return LanguageManager.Instance.GetText(this.Name);
		}
	}

	// Token: 0x1700028E RID: 654
	// (get) Token: 0x06001B51 RID: 6993 RVA: 0x0008B103 File Offset: 0x00089303
	public string TranslatedDescription
	{
		get
		{
			return LanguageManager.Instance.GetText(this.Description);
		}
	}

	// Token: 0x06001B52 RID: 6994
	public abstract void ActivateOnServer(GameObject player, int talentLevel);

	// Token: 0x040016BA RID: 5818
	public TalentIdentifier TalentId;

	// Token: 0x040016BB RID: 5819
	public Sprite Icon;

	// Token: 0x040016BC RID: 5820
	public string Name;

	// Token: 0x040016BD RID: 5821
	public int MaxLevel;

	// Token: 0x040016BE RID: 5822
	public string Description;

	// Token: 0x040016BF RID: 5823
	public Vocation Vocation;

	// Token: 0x040016C0 RID: 5824
	public float PowerPerLevel;
}
