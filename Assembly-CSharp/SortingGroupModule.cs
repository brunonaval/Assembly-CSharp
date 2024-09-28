using System;
using Mirror;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000421 RID: 1057
public class SortingGroupModule : MonoBehaviour
{
	// Token: 0x0600170B RID: 5899 RVA: 0x000759ED File Offset: 0x00073BED
	private void Start()
	{
		this.defaultLayerName = this.sortingGroup.sortingLayerName;
	}

	// Token: 0x0600170C RID: 5900 RVA: 0x00075A00 File Offset: 0x00073C00
	private void Update()
	{
		if (!NetworkClient.active)
		{
			return;
		}
		if (this.sortingGroup == null)
		{
			return;
		}
		if (Time.time - this.lastUpdate >= 0.3f)
		{
			this.UpdateLayerForDeadCreature();
			this.UpdateLayerForLivingCreature();
			this.lastUpdate = Time.time;
		}
	}

	// Token: 0x0600170D RID: 5901 RVA: 0x00075A50 File Offset: 0x00073C50
	private void UpdateLayerForDeadCreature()
	{
		if (this.creatureModule == null)
		{
			return;
		}
		if (this.creatureModule.IsAlive)
		{
			return;
		}
		if (this.sortingGroup.sortingLayerName == "Terrain")
		{
			return;
		}
		if (this.sortingGroup.sortingOrder == 49)
		{
			return;
		}
		this.sortingGroup.sortingLayerName = "Terrain";
		this.sortingGroup.sortingOrder = 49;
	}

	// Token: 0x0600170E RID: 5902 RVA: 0x00075AC0 File Offset: 0x00073CC0
	private void UpdateLayerForLivingCreature()
	{
		if (this.creatureModule != null && !this.creatureModule.IsAlive)
		{
			return;
		}
		int num = (int)((double)base.transform.position.y * 3.2 * -1.0);
		num = Mathf.Min(num, 32767);
		if (this.defaultLayerName != this.sortingGroup.sortingLayerName)
		{
			this.sortingGroup.sortingLayerName = this.defaultLayerName;
		}
		if (num != this.sortingGroup.sortingOrder)
		{
			this.sortingGroup.sortingOrder = num;
		}
	}

	// Token: 0x0400149A RID: 5274
	[SerializeField]
	private SortingGroup sortingGroup;

	// Token: 0x0400149B RID: 5275
	[SerializeField]
	private CreatureModule creatureModule;

	// Token: 0x0400149C RID: 5276
	private float lastUpdate;

	// Token: 0x0400149D RID: 5277
	private string defaultLayerName;
}
