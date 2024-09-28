using System;
using System.Linq;
using UnityEngine;

// Token: 0x020004BB RID: 1211
public class ScriptableDatabaseModule : MonoBehaviour
{
	// Token: 0x1700028C RID: 652
	// (get) Token: 0x06001B43 RID: 6979 RVA: 0x0008AF98 File Offset: 0x00089198
	// (set) Token: 0x06001B44 RID: 6980 RVA: 0x0008AF9F File Offset: 0x0008919F
	public static ScriptableDatabaseModule Singleton { get; private set; }

	// Token: 0x06001B45 RID: 6981 RVA: 0x0008AFA7 File Offset: 0x000891A7
	private void Awake()
	{
		if (ScriptableDatabaseModule.Singleton == null)
		{
			ScriptableDatabaseModule.Singleton = this;
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06001B46 RID: 6982 RVA: 0x0008AFC8 File Offset: 0x000891C8
	public Talent GetTalent(TalentIdentifier talentId)
	{
		return this.talentDatabase.Talents.FirstOrDefault((Talent t) => t.TalentId == talentId);
	}

	// Token: 0x06001B47 RID: 6983 RVA: 0x0008AFFE File Offset: 0x000891FE
	public Talent[] GetAllTalents()
	{
		return this.talentDatabase.Talents.ToArray();
	}

	// Token: 0x040016B4 RID: 5812
	[SerializeField]
	private TalentDatabase talentDatabase;
}
