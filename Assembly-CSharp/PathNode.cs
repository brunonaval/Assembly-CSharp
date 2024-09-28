using System;
using UnityEngine;

// Token: 0x0200012A RID: 298
public class PathNode
{
	// Token: 0x06000329 RID: 809 RVA: 0x000146DE File Offset: 0x000128DE
	public PathNode(Vector3 position)
	{
		this.Position = position;
	}

	// Token: 0x17000064 RID: 100
	// (get) Token: 0x0600032A RID: 810 RVA: 0x000146ED File Offset: 0x000128ED
	public float FCost
	{
		get
		{
			return this.GCost + this.HCost;
		}
	}

	// Token: 0x040005FA RID: 1530
	public float GCost;

	// Token: 0x040005FB RID: 1531
	public float HCost;

	// Token: 0x040005FC RID: 1532
	public Vector3 Position;

	// Token: 0x040005FD RID: 1533
	public PathNode Parent;
}
