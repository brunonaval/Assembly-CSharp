using System;
using UnityEngine;

// Token: 0x02000121 RID: 289
public struct NpcAction
{
	// Token: 0x06000317 RID: 791 RVA: 0x000144CA File Offset: 0x000126CA
	public NpcAction(int id, NpcAction.NpcTask task)
	{
		this.Id = id;
		this.Task = task;
	}

	// Token: 0x040005DD RID: 1501
	public int Id;

	// Token: 0x040005DE RID: 1502
	public NpcAction.NpcTask Task;

	// Token: 0x02000122 RID: 290
	// (Invoke) Token: 0x06000319 RID: 793
	public delegate void NpcTask(GameObject player);
}
