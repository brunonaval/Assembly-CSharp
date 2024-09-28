using System;
using UnityEngine;

// Token: 0x02000422 RID: 1058
public class SortingLayerModule : MonoBehaviour
{
	// Token: 0x06001710 RID: 5904 RVA: 0x00075B5F File Offset: 0x00073D5F
	private void Awake()
	{
		this.meshRenderer = base.GetComponent<MeshRenderer>();
		this.meshRenderer.sortingLayerName = this.SortingLayerName;
		this.meshRenderer.sortingOrder = this.SortingOrder;
	}

	// Token: 0x0400149E RID: 5278
	public string SortingLayerName = "Default";

	// Token: 0x0400149F RID: 5279
	public int SortingOrder;

	// Token: 0x040014A0 RID: 5280
	private MeshRenderer meshRenderer;
}
