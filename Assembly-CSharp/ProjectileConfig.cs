using System;
using UnityEngine;

// Token: 0x02000139 RID: 313
public class ProjectileConfig
{
	// Token: 0x04000652 RID: 1618
	public Item Ammo;

	// Token: 0x04000653 RID: 1619
	public Skill Skill;

	// Token: 0x04000654 RID: 1620
	public Vector2 Velocity;

	// Token: 0x04000655 RID: 1621
	public Vector3 ShootPivot;

	// Token: 0x04000656 RID: 1622
	public Vector2 StartPoint;

	// Token: 0x04000657 RID: 1623
	public Condition Condition;

	// Token: 0x04000658 RID: 1624
	public GameObject Attacker;

	// Token: 0x04000659 RID: 1625
	public GameObject Target;

	// Token: 0x0400065A RID: 1626
	public float ConditionChance;

	// Token: 0x0400065B RID: 1627
	public bool UseAttackerAsStartPoint;
}
