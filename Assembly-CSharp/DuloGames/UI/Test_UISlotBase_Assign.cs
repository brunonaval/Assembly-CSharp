using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x020005B4 RID: 1460
	public class Test_UISlotBase_Assign : MonoBehaviour
	{
		// Token: 0x0600201C RID: 8220 RVA: 0x000A10CC File Offset: 0x0009F2CC
		private void Start()
		{
			if (this.slot != null)
			{
				if (this.texture != null)
				{
					this.slot.Assign(this.texture);
					return;
				}
				if (this.sprite != null)
				{
					this.slot.Assign(this.sprite);
				}
			}
		}

		// Token: 0x040019F0 RID: 6640
		[SerializeField]
		private UISlotBase slot;

		// Token: 0x040019F1 RID: 6641
		[SerializeField]
		private Texture texture;

		// Token: 0x040019F2 RID: 6642
		[SerializeField]
		private Sprite sprite;
	}
}
