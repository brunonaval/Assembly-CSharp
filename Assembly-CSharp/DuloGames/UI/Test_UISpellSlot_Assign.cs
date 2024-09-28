using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x020005B5 RID: 1461
	public class Test_UISpellSlot_Assign : MonoBehaviour
	{
		// Token: 0x0600201E RID: 8222 RVA: 0x000A1128 File Offset: 0x0009F328
		private void Awake()
		{
			if (this.slot == null)
			{
				this.slot = base.GetComponent<UISpellSlot>();
			}
		}

		// Token: 0x0600201F RID: 8223 RVA: 0x000A1144 File Offset: 0x0009F344
		private void Start()
		{
			if (this.slot == null || UISpellDatabase.Instance == null)
			{
				this.Destruct();
				return;
			}
			this.slot.Assign(UISpellDatabase.Instance.GetByID(this.assignSpell));
			this.Destruct();
		}

		// Token: 0x06002020 RID: 8224 RVA: 0x00003485 File Offset: 0x00001685
		private void Destruct()
		{
			UnityEngine.Object.DestroyImmediate(this);
		}

		// Token: 0x040019F3 RID: 6643
		[SerializeField]
		private UISpellSlot slot;

		// Token: 0x040019F4 RID: 6644
		[SerializeField]
		private int assignSpell;
	}
}
