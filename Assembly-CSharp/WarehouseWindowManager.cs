using System;
using UnityEngine;

// Token: 0x0200029A RID: 666
public class WarehouseWindowManager : MonoBehaviour
{
	// Token: 0x06000A7A RID: 2682 RVA: 0x0002FC3F File Offset: 0x0002DE3F
	public void SortItems()
	{
		this.uiSystemModule.WarehouseModule.CmdSortItems();
	}

	// Token: 0x04000BF7 RID: 3063
	[SerializeField]
	private UISystemModule uiSystemModule;
}
