using System;

// Token: 0x0200010C RID: 268
public struct ExperienceBonus
{
	// Token: 0x060002B1 RID: 689 RVA: 0x000126B5 File Offset: 0x000108B5
	public ExperienceBonus(int minLevel, int maxLevel, float bonus)
	{
		this.MinLevel = minLevel;
		this.MaxLevel = maxLevel;
		this.Bonus = bonus;
	}

	// Token: 0x04000515 RID: 1301
	public int MinLevel;

	// Token: 0x04000516 RID: 1302
	public int MaxLevel;

	// Token: 0x04000517 RID: 1303
	public float Bonus;
}
