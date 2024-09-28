using System;
using UnityEngine;
using UnityEngine.Events;

namespace DuloGames.UI
{
	// Token: 0x0200059F RID: 1439
	public class Demo_CastManager : MonoBehaviour
	{
		// Token: 0x06001FCA RID: 8138 RVA: 0x0009FD7C File Offset: 0x0009DF7C
		protected void Start()
		{
			if (this.m_SlotContainers != null && this.m_SlotContainers.Length != 0)
			{
				Transform[] slotContainers = this.m_SlotContainers;
				for (int i = 0; i < slotContainers.Length; i++)
				{
					UISpellSlot[] componentsInChildren = slotContainers[i].GetComponentsInChildren<UISpellSlot>();
					for (int j = 0; j < componentsInChildren.Length; j++)
					{
						componentsInChildren[j].onClick.AddListener(new UnityAction<UISpellSlot>(this.OnSpellClick));
					}
				}
			}
		}

		// Token: 0x06001FCB RID: 8139 RVA: 0x0009FDE0 File Offset: 0x0009DFE0
		public void OnSpellClick(UISpellSlot slot)
		{
			if (this.m_CastBar == null || !slot.IsAssigned())
			{
				return;
			}
			if (this.m_CastBar.IsCasting)
			{
				return;
			}
			UISpellInfo spellInfo = slot.GetSpellInfo();
			if (spellInfo == null)
			{
				return;
			}
			if (spellInfo.Cooldown > 0f && slot.cooldownComponent != null && slot.cooldownComponent.IsOnCooldown)
			{
				return;
			}
			if (!spellInfo.Flags.Has(UISpellInfo_Flags.InstantCast))
			{
				this.m_CastBar.StartCasting(spellInfo, spellInfo.CastTime, Time.time + spellInfo.CastTime);
			}
			if (slot.cooldownComponent != null && spellInfo.Cooldown > 0f)
			{
				foreach (UISpellSlot uispellSlot in UISpellSlot.GetSlots())
				{
					if (uispellSlot.IsAssigned() && uispellSlot.GetSpellInfo() != null && uispellSlot.cooldownComponent != null && uispellSlot.GetSpellInfo().ID == spellInfo.ID)
					{
						uispellSlot.cooldownComponent.StartCooldown(spellInfo.ID, spellInfo.Cooldown);
					}
				}
			}
		}

		// Token: 0x040019AE RID: 6574
		[SerializeField]
		private UICastBar m_CastBar;

		// Token: 0x040019AF RID: 6575
		[SerializeField]
		private Transform[] m_SlotContainers;
	}
}
