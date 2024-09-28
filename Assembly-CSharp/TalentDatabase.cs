using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004BD RID: 1213
[CreateAssetMenu(fileName = "talent_database", menuName = "Scriptables/Database/Talents", order = 0)]
public class TalentDatabase : ScriptableObject
{
	// Token: 0x040016B6 RID: 5814
	public List<Talent> Talents = new List<Talent>();
}
