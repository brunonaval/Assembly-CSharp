using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x020002C6 RID: 710
public class BlueprintDatabaseModule : MonoBehaviour
{
	// Token: 0x1700016F RID: 367
	// (get) Token: 0x06000C54 RID: 3156 RVA: 0x000376C1 File Offset: 0x000358C1
	// (set) Token: 0x06000C55 RID: 3157 RVA: 0x000376C9 File Offset: 0x000358C9
	public bool IsLoaded { get; private set; }

	// Token: 0x06000C56 RID: 3158 RVA: 0x000376D2 File Offset: 0x000358D2
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
		this.itemDatabaseModule = base.GetComponent<ItemDatabaseModule>();
	}

	// Token: 0x06000C57 RID: 3159 RVA: 0x000376E6 File Offset: 0x000358E6
	private IEnumerator Start()
	{
		yield return new WaitUntil(() => this.itemDatabaseModule.IsLoaded);
		yield return this.ProcessBlueprints().WaitAsCoroutine();
		this.alchemistBlueprintChance = this.CalculateBlueprintDropRate(PlayerProfession.Alchemist);
		this.weaponsmithBlueprintChance = this.CalculateBlueprintDropRate(PlayerProfession.Weaponsmith);
		this.armorsmithBlueprintChance = this.CalculateBlueprintDropRate(PlayerProfession.Armorsmith);
		this.IsLoaded = true;
		yield break;
	}

	// Token: 0x06000C58 RID: 3160 RVA: 0x000376F8 File Offset: 0x000358F8
	private Task ProcessBlueprints()
	{
		BlueprintDatabaseModule.<ProcessBlueprints>d__11 <ProcessBlueprints>d__;
		<ProcessBlueprints>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<ProcessBlueprints>d__.<>4__this = this;
		<ProcessBlueprints>d__.<>1__state = -1;
		<ProcessBlueprints>d__.<>t__builder.Start<BlueprintDatabaseModule.<ProcessBlueprints>d__11>(ref <ProcessBlueprints>d__);
		return <ProcessBlueprints>d__.<>t__builder.Task;
	}

	// Token: 0x06000C59 RID: 3161 RVA: 0x0003773C File Offset: 0x0003593C
	public Blueprint GetBlueprint(int blueprintId)
	{
		if (blueprintId >= this.blueprints.Length)
		{
			return default(Blueprint);
		}
		return this.blueprints[blueprintId];
	}

	// Token: 0x06000C5A RID: 3162 RVA: 0x0003776C File Offset: 0x0003596C
	public Blueprint GetBlueprintByProducesItemId(int producesItemId)
	{
		foreach (Blueprint blueprint in this.blueprints)
		{
			if (blueprint.ProducesItem.Id == producesItemId)
			{
				return blueprint;
			}
		}
		return default(Blueprint);
	}

	// Token: 0x06000C5B RID: 3163 RVA: 0x000377B0 File Offset: 0x000359B0
	public List<Blueprint> GetBlueprintsByProfession(PlayerProfession profession)
	{
		return (from b in this.blueprints
		where b.RequiredProfession == profession
		select b).ToList<Blueprint>();
	}

	// Token: 0x06000C5C RID: 3164 RVA: 0x000377E8 File Offset: 0x000359E8
	private float CalculateBlueprintDropRate(PlayerProfession profession)
	{
		List<Blueprint> blueprintsByProfession = this.GetBlueprintsByProfession(PlayerProfession.Alchemist);
		List<Blueprint> blueprintsByProfession2 = this.GetBlueprintsByProfession(PlayerProfession.Armorsmith);
		List<Blueprint> blueprintsByProfession3 = this.GetBlueprintsByProfession(PlayerProfession.Weaponsmith);
		float num = Mathf.Max(new float[]
		{
			(float)blueprintsByProfession.Count,
			(float)blueprintsByProfession3.Count,
			(float)blueprintsByProfession2.Count
		});
		List<Blueprint> blueprintsByProfession4 = this.GetBlueprintsByProfession(profession);
		return num / (float)blueprintsByProfession4.Count;
	}

	// Token: 0x06000C5D RID: 3165 RVA: 0x00037848 File Offset: 0x00035A48
	public float GetBlueprintDropRate(int blueprintId)
	{
		Blueprint blueprint = this.GetBlueprint(blueprintId);
		if (blueprint.RequiredProfession == PlayerProfession.None)
		{
			return 0f;
		}
		switch (blueprint.RequiredProfession)
		{
		case PlayerProfession.Weaponsmith:
			return this.weaponsmithBlueprintChance;
		case PlayerProfession.Armorsmith:
			return this.armorsmithBlueprintChance;
		case PlayerProfession.Alchemist:
			return this.alchemistBlueprintChance;
		default:
			return 0f;
		}
	}

	// Token: 0x04000D37 RID: 3383
	private Blueprint[] blueprints;

	// Token: 0x04000D38 RID: 3384
	private ItemDatabaseModule itemDatabaseModule;

	// Token: 0x04000D39 RID: 3385
	private float alchemistBlueprintChance;

	// Token: 0x04000D3A RID: 3386
	private float weaponsmithBlueprintChance;

	// Token: 0x04000D3B RID: 3387
	private float armorsmithBlueprintChance;
}
