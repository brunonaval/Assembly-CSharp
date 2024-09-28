using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x020005AF RID: 1455
	public class Test_UIEquipSlot_Assign : MonoBehaviour
	{
		// Token: 0x0600200C RID: 8204 RVA: 0x000A0D3F File Offset: 0x0009EF3F
		private void Awake()
		{
			if (this.slot == null)
			{
				this.slot = base.GetComponent<UIEquipSlot>();
			}
		}

		// Token: 0x0600200D RID: 8205 RVA: 0x000A0D5C File Offset: 0x0009EF5C
		private void Start()
		{
			if (this.slot == null || UIItemDatabase.Instance == null)
			{
				this.Destruct();
				return;
			}
			this.slot.Assign(UIItemDatabase.Instance.GetByID(this.assignItem));
			this.Destruct();
		}

		// Token: 0x0600200E RID: 8206 RVA: 0x00003485 File Offset: 0x00001685
		private void Destruct()
		{
			UnityEngine.Object.DestroyImmediate(this);
		}

		// Token: 0x040019DF RID: 6623
		[SerializeField]
		private UIEquipSlot slot;

		// Token: 0x040019E0 RID: 6624
		[SerializeField]
		private int assignItem;
	}
}
