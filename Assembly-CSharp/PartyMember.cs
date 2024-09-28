using System;
using UnityEngine;

// Token: 0x02000128 RID: 296
public struct PartyMember
{
	// Token: 0x17000063 RID: 99
	// (get) Token: 0x06000326 RID: 806 RVA: 0x000146A2 File Offset: 0x000128A2
	public bool IsDefined
	{
		get
		{
			return !string.IsNullOrEmpty(this.Name) && this.Member != null;
		}
	}

	// Token: 0x06000327 RID: 807 RVA: 0x000146BF File Offset: 0x000128BF
	public PartyMember(string name, uint networkInstanceId, GameObject member)
	{
		this.Name = name;
		this.Member = member;
		this.NetworkInstanceId = networkInstanceId;
	}

	// Token: 0x040005F7 RID: 1527
	public string Name;

	// Token: 0x040005F8 RID: 1528
	public uint NetworkInstanceId;

	// Token: 0x040005F9 RID: 1529
	public GameObject Member;
}
