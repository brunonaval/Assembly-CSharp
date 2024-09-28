using System;

// Token: 0x0200014C RID: 332
[Serializable]
public struct SkillConfig
{
	// Token: 0x0600038B RID: 907 RVA: 0x0001626D File Offset: 0x0001446D
	public SkillConfig(int skillId, float castChance, float castInterval, float skillPower)
	{
		this.SkillId = skillId;
		this.CastChance = castChance;
		this.SkillPower = skillPower;
		this.CastInterval = castInterval;
	}

	// Token: 0x040006D9 RID: 1753
	public int SkillId;

	// Token: 0x040006DA RID: 1754
	public float CastChance;

	// Token: 0x040006DB RID: 1755
	public float SkillPower;

	// Token: 0x040006DC RID: 1756
	public float CastInterval;
}
