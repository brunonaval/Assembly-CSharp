using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x020005B7 RID: 1463
	public class Test_UITalentSlot_Assign : MonoBehaviour
	{
		// Token: 0x06002025 RID: 8229 RVA: 0x000A1218 File Offset: 0x0009F418
		private void Start()
		{
			if (this.slot == null)
			{
				this.slot = base.GetComponent<UITalentSlot>();
			}
			if (this.slot == null || UITalentDatabase.Instance == null || UISpellDatabase.Instance == null)
			{
				this.Destruct();
				return;
			}
			UITalentInfo byID = UITalentDatabase.Instance.GetByID(this.assignTalent);
			if (byID != null)
			{
				this.slot.Assign(byID, UISpellDatabase.Instance.GetByID(byID.spellEntry));
				this.slot.AddPoints(this.addPoints);
			}
			this.Destruct();
		}

		// Token: 0x06002026 RID: 8230 RVA: 0x00003485 File Offset: 0x00001685
		private void Destruct()
		{
			UnityEngine.Object.DestroyImmediate(this);
		}

		// Token: 0x040019F6 RID: 6646
		[SerializeField]
		private UITalentSlot slot;

		// Token: 0x040019F7 RID: 6647
		[SerializeField]
		private int assignTalent;

		// Token: 0x040019F8 RID: 6648
		[SerializeField]
		private int addPoints;
	}
}
