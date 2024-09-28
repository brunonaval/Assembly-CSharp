using System;
using UnityEngine;

// Token: 0x020000F2 RID: 242
[Serializable]
public class Attacker
{
	// Token: 0x0600025D RID: 605 RVA: 0x000117BA File Offset: 0x0000F9BA
	public Attacker(GameObject attackerObject, int damageDealt)
	{
		this.AttackerObject = attackerObject;
		this.DamageDealt = damageDealt;
	}

	// Token: 0x17000015 RID: 21
	// (get) Token: 0x0600025E RID: 606 RVA: 0x000117D0 File Offset: 0x0000F9D0
	public bool IsDefined
	{
		get
		{
			return this.AttackerObject != null;
		}
	}

	// Token: 0x04000467 RID: 1127
	public GameObject AttackerObject;

	// Token: 0x04000468 RID: 1128
	public int DamageDealt;
}
