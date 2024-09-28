using System;
using UnityEngine;

// Token: 0x0200000E RID: 14
public abstract class ActionBrushManager : MonoBehaviour
{
	// Token: 0x06000040 RID: 64
	public abstract void ExecuteAction(GameObject player);

	// Token: 0x06000041 RID: 65 RVA: 0x00002A3C File Offset: 0x00000C3C
	private void Awake()
	{
		base.GetComponent<SpriteRenderer>().enabled = false;
	}

	// Token: 0x04000022 RID: 34
	public string HintText;
}
