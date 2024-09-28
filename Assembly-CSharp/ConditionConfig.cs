using System;

// Token: 0x02000103 RID: 259
public struct ConditionConfig
{
	// Token: 0x060002A1 RID: 673 RVA: 0x000125B6 File Offset: 0x000107B6
	public ConditionConfig(Condition condition)
	{
		this = new ConditionConfig(1f, condition);
	}

	// Token: 0x060002A2 RID: 674 RVA: 0x000125C4 File Offset: 0x000107C4
	public ConditionConfig(float chance, Condition condition)
	{
		this.Chance = chance;
		this.Condition = condition;
	}

	// Token: 0x040004D1 RID: 1233
	public float Chance;

	// Token: 0x040004D2 RID: 1234
	public Condition Condition;
}
