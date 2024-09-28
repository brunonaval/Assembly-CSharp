using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200032D RID: 813
public class GlobalEventModule : MonoBehaviour
{
	// Token: 0x06000FD2 RID: 4050 RVA: 0x0004A3B7 File Offset: 0x000485B7
	private void Awake()
	{
		if (GlobalEventModule.Singleton == null)
		{
			GlobalEventModule.Singleton = this;
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06000FD3 RID: 4051 RVA: 0x0004A3D8 File Offset: 0x000485D8
	public void ClearEvents()
	{
		this.ActiveGlobalEvents.Clear();
	}

	// Token: 0x06000FD4 RID: 4052 RVA: 0x0004A3E8 File Offset: 0x000485E8
	public void AddEvent(int itemId, float dropChance, int minLevel)
	{
		if (this.ActiveGlobalEvents.Any((GlobalEventConfig x) => x.ItemId == itemId))
		{
			return;
		}
		this.ActiveGlobalEvents.Add(new GlobalEventConfig
		{
			ItemId = itemId,
			DropChance = dropChance,
			MinLevel = minLevel
		});
	}

	// Token: 0x06000FD5 RID: 4053 RVA: 0x0004A448 File Offset: 0x00048648
	public void RemoveEvent(int itemId)
	{
		this.ActiveGlobalEvents.RemoveAll((GlobalEventConfig x) => x.ItemId == itemId);
	}

	// Token: 0x04000F54 RID: 3924
	public readonly List<GlobalEventConfig> ActiveGlobalEvents = new List<GlobalEventConfig>();

	// Token: 0x04000F55 RID: 3925
	public static GlobalEventModule Singleton;
}
