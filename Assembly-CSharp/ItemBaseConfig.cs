using System;
using UnityEngine;

// Token: 0x02000112 RID: 274
public struct ItemBaseConfig
{
	// Token: 0x06000306 RID: 774 RVA: 0x0001402A File Offset: 0x0001222A
	public ItemBaseConfig(Item item, GameObject userObject)
	{
		this.Item = item;
		this.UserObject = userObject;
	}

	// Token: 0x0400054A RID: 1354
	public Item Item;

	// Token: 0x0400054B RID: 1355
	public GameObject UserObject;
}
