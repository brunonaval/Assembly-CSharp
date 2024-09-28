using System;
using Mirror;
using UnityEngine;

// Token: 0x02000399 RID: 921
public class NonPlayerSkillModule : MonoBehaviour
{
	// Token: 0x060012F9 RID: 4857 RVA: 0x0005D3AC File Offset: 0x0005B5AC
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("DatabaseManager");
		this.skillDatabaseModule = gameObject.GetComponent<SkillDatabaseModule>();
		this.creatureModule = base.GetComponent<CreatureModule>();
		this.conditionModule = base.GetComponent<ConditionModule>();
	}

	// Token: 0x060012FA RID: 4858 RVA: 0x0005D3F0 File Offset: 0x0005B5F0
	public Skill BuildSkill(int skillId, float skillPower)
	{
		Skill skill = this.skillDatabaseModule.CreateSkill(skillId);
		skill.SkillPower = ((skillPower != 0f) ? skillPower : skill.SkillPower);
		return skill;
	}

	// Token: 0x060012FB RID: 4859 RVA: 0x0005D424 File Offset: 0x0005B624
	public void Cast(Skill skill)
	{
		if (!this.ValidateCast(skill))
		{
			return;
		}
		NonPlayerSkillBase fromCache = ClassFactory.GetFromCache<NonPlayerSkillBase>(skill.Script, Array.Empty<object>());
		SkillBaseConfig skillBaseConfig = new SkillBaseConfig(skill, Vector3.zero, base.gameObject);
		fromCache.Cast(skillBaseConfig);
	}

	// Token: 0x060012FC RID: 4860 RVA: 0x0005D465 File Offset: 0x0005B665
	private bool ValidateCast(Skill skill)
	{
		return skill.IsDefined && this.creatureModule.IsAlive && !this.conditionModule.HasActiveCondition(ConditionCategory.Paralyzing);
	}

	// Token: 0x040011A9 RID: 4521
	private CreatureModule creatureModule;

	// Token: 0x040011AA RID: 4522
	private ConditionModule conditionModule;

	// Token: 0x040011AB RID: 4523
	private SkillDatabaseModule skillDatabaseModule;
}
