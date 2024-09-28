using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x020005B0 RID: 1456
	public class Test_UIItemSlot_Assign : MonoBehaviour
	{
		// Token: 0x06002010 RID: 8208 RVA: 0x000A0DAD File Offset: 0x0009EFAD
		private void Awake()
		{
			if (this.slot == null)
			{
				this.slot = base.GetComponent<UIItemSlot>();
			}
		}

		// Token: 0x06002011 RID: 8209 RVA: 0x000A0DCC File Offset: 0x0009EFCC
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

		// Token: 0x06002012 RID: 8210 RVA: 0x00003485 File Offset: 0x00001685
		private void Destruct()
		{
			UnityEngine.Object.DestroyImmediate(this);
		}

		// Token: 0x040019E1 RID: 6625
		[SerializeField]
		private UIItemSlot slot;

		// Token: 0x040019E2 RID: 6626
		[SerializeField]
		private int assignItem;
	}
}
