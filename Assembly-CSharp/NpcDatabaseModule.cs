using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x0200039A RID: 922
public class NpcDatabaseModule : MonoBehaviour
{
	// Token: 0x1700020B RID: 523
	// (get) Token: 0x060012FE RID: 4862 RVA: 0x0005D492 File Offset: 0x0005B692
	// (set) Token: 0x060012FF RID: 4863 RVA: 0x0005D49A File Offset: 0x0005B69A
	public bool IsLoaded { get; private set; }

	// Token: 0x06001300 RID: 4864 RVA: 0x0005D4A3 File Offset: 0x0005B6A3
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	// Token: 0x06001301 RID: 4865 RVA: 0x0005D4AB File Offset: 0x0005B6AB
	private IEnumerator Start()
	{
		yield return this.ProcessNpcs().WaitAsCoroutine();
		this.IsLoaded = true;
		yield break;
	}

	// Token: 0x06001302 RID: 4866 RVA: 0x0005D4BC File Offset: 0x0005B6BC
	private Task ProcessNpcs()
	{
		NpcDatabaseModule.<ProcessNpcs>d__7 <ProcessNpcs>d__;
		<ProcessNpcs>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<ProcessNpcs>d__.<>4__this = this;
		<ProcessNpcs>d__.<>1__state = -1;
		<ProcessNpcs>d__.<>t__builder.Start<NpcDatabaseModule.<ProcessNpcs>d__7>(ref <ProcessNpcs>d__);
		return <ProcessNpcs>d__.<>t__builder.Task;
	}

	// Token: 0x06001303 RID: 4867 RVA: 0x0005D500 File Offset: 0x0005B700
	private Task<Npc> ProcessNpcSkills(Npc npc)
	{
		NpcDatabaseModule.<ProcessNpcSkills>d__8 <ProcessNpcSkills>d__;
		<ProcessNpcSkills>d__.<>t__builder = AsyncTaskMethodBuilder<Npc>.Create();
		<ProcessNpcSkills>d__.<>4__this = this;
		<ProcessNpcSkills>d__.npc = npc;
		<ProcessNpcSkills>d__.<>1__state = -1;
		<ProcessNpcSkills>d__.<>t__builder.Start<NpcDatabaseModule.<ProcessNpcSkills>d__8>(ref <ProcessNpcSkills>d__);
		return <ProcessNpcSkills>d__.<>t__builder.Task;
	}

	// Token: 0x06001304 RID: 4868 RVA: 0x0005D54C File Offset: 0x0005B74C
	private void AddSkillConfig(List<SkillConfig> skillConfigs, DataRow dbSkill)
	{
		skillConfigs.Add(new SkillConfig((dbSkill["SkillId"] as int?).GetValueOrDefault(), (dbSkill["CastChance"] as float?).GetValueOrDefault(), (dbSkill["CastInterval"] as float?) ?? 99f, (dbSkill["SkillPower"] as float?).GetValueOrDefault()));
	}

	// Token: 0x06001305 RID: 4869 RVA: 0x0005D5E8 File Offset: 0x0005B7E8
	private static Npc BuildNpc(DataRow dbNpc)
	{
		int valueOrDefault = (dbNpc["Id"] as int?).GetValueOrDefault();
		string name = dbNpc["Name"].ToString();
		object obj = dbNpc["Title"];
		string title = (obj != null) ? obj.ToString() : null;
		CreatureGender valueOrDefault2 = (CreatureGender)(dbNpc["Gender"] as int?).GetValueOrDefault();
		string script = dbNpc["Script"].ToString();
		int baseLevel = (dbNpc["BaseLevel"] as int?) ?? 1;
		int powerLevel = (dbNpc["PowerLevel"] as int?) ?? 1;
		int agilityLevel = (dbNpc["AgilityLevel"] as int?) ?? 1;
		int precisionLevel = (dbNpc["PrecisionLevel"] as int?) ?? 1;
		int toughnessLevel = (dbNpc["ToughnessLevel"] as int?) ?? 1;
		int vitalityLevel = (dbNpc["VitalityLevel"] as int?) ?? 1;
		int valueOrDefault3 = (dbNpc["Attack"] as int?).GetValueOrDefault();
		int valueOrDefault4 = (dbNpc["Defense"] as int?).GetValueOrDefault();
		object obj2 = dbNpc["SkinMetaName"];
		return new Npc(valueOrDefault, name, title, valueOrDefault2, script, baseLevel, powerLevel, agilityLevel, precisionLevel, toughnessLevel, vitalityLevel, valueOrDefault3, valueOrDefault4, (obj2 != null) ? obj2.ToString() : null, ((bool?)dbNpc["CanMove"]) ?? true, ((bool?)dbNpc["IsCombatant"]).GetValueOrDefault(), (dbNpc["WalkRange"] as float?) ?? 1.5f, (dbNpc["WalkInterval"] as float?) ?? 45f, (dbNpc["WalkChance"] as float?) ?? 0.02f);
	}

	// Token: 0x06001306 RID: 4870 RVA: 0x0005D878 File Offset: 0x0005BA78
	public void Add(Npc npc)
	{
		if (this.npcs.Any((Npc a) => a.Id == npc.Id))
		{
			Debug.LogErrorFormat("Can't add npc, id [{0}] already exists.", new object[]
			{
				npc.Id
			});
			return;
		}
		this.npcs.Add(npc);
	}

	// Token: 0x06001307 RID: 4871 RVA: 0x0005D8E0 File Offset: 0x0005BAE0
	public Npc GetNpc(int npcId)
	{
		return this.npcs.FirstOrDefault((Npc f) => f.Id == npcId);
	}

	// Token: 0x040011AC RID: 4524
	private List<Npc> npcs = new List<Npc>();
}
