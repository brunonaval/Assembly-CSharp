using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DuloGames.UI
{
	// Token: 0x02000610 RID: 1552
	[AddComponentMenu("UI/Icon Slots/Spell Slot", 12)]
	public class UISpellSlot : UISlotBase, IUISpellSlot, IUISlotHasCooldown
	{
		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06002242 RID: 8770 RVA: 0x000AA471 File Offset: 0x000A8671
		// (set) Token: 0x06002243 RID: 8771 RVA: 0x000AA479 File Offset: 0x000A8679
		public UISpellSlot_Group slotGroup
		{
			get
			{
				return this.m_SlotGroup;
			}
			set
			{
				this.m_SlotGroup = value;
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06002244 RID: 8772 RVA: 0x000AA482 File Offset: 0x000A8682
		// (set) Token: 0x06002245 RID: 8773 RVA: 0x000AA48A File Offset: 0x000A868A
		public int ID
		{
			get
			{
				return this.m_ID;
			}
			set
			{
				this.m_ID = value;
			}
		}

		// Token: 0x06002246 RID: 8774 RVA: 0x000AA493 File Offset: 0x000A8693
		public UISpellInfo GetSpellInfo()
		{
			return this.m_SpellInfo;
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06002247 RID: 8775 RVA: 0x000AA49B File Offset: 0x000A869B
		public UISlotCooldown cooldownComponent
		{
			get
			{
				return this.m_Cooldown;
			}
		}

		// Token: 0x06002248 RID: 8776 RVA: 0x000A8890 File Offset: 0x000A6A90
		protected override void OnEnable()
		{
			base.OnEnable();
		}

		// Token: 0x06002249 RID: 8777 RVA: 0x000AA4A3 File Offset: 0x000A86A3
		public override bool IsAssigned()
		{
			return this.m_SpellInfo != null;
		}

		// Token: 0x0600224A RID: 8778 RVA: 0x000AA4B0 File Offset: 0x000A86B0
		public bool Assign(UISpellInfo spellInfo)
		{
			if (spellInfo == null)
			{
				return false;
			}
			this.Unassign();
			base.Assign(spellInfo.Icon);
			this.m_SpellInfo = spellInfo;
			if (this.onAssign != null)
			{
				this.onAssign.Invoke(this);
			}
			if (this.m_Cooldown != null)
			{
				this.m_Cooldown.OnAssignSpell();
			}
			return true;
		}

		// Token: 0x0600224B RID: 8779 RVA: 0x000AA50C File Offset: 0x000A870C
		public override bool Assign(UnityEngine.Object source)
		{
			if (source is IUISpellSlot)
			{
				IUISpellSlot iuispellSlot = source as IUISpellSlot;
				if (iuispellSlot != null)
				{
					return this.Assign(iuispellSlot.GetSpellInfo());
				}
			}
			return false;
		}

		// Token: 0x0600224C RID: 8780 RVA: 0x000AA539 File Offset: 0x000A8739
		public override void Unassign()
		{
			base.Unassign();
			this.m_SpellInfo = null;
			if (this.onUnassign != null)
			{
				this.onUnassign.Invoke(this);
			}
			if (this.m_Cooldown != null)
			{
				this.m_Cooldown.OnUnassignSpell();
			}
		}

		// Token: 0x0600224D RID: 8781 RVA: 0x000AA575 File Offset: 0x000A8775
		public override bool CanSwapWith(UnityEngine.Object target)
		{
			return target is IUISpellSlot;
		}

		// Token: 0x0600224E RID: 8782 RVA: 0x000AA580 File Offset: 0x000A8780
		public override bool PerformSlotSwap(UnityEngine.Object sourceObject)
		{
			IUISpellSlot iuispellSlot = sourceObject as IUISpellSlot;
			UISpellInfo spellInfo = iuispellSlot.GetSpellInfo();
			bool flag = iuispellSlot.Assign(this.GetSpellInfo());
			bool flag2 = this.Assign(spellInfo);
			return flag && flag2;
		}

		// Token: 0x0600224F RID: 8783 RVA: 0x000AA5AF File Offset: 0x000A87AF
		public override void OnPointerClick(PointerEventData eventData)
		{
			base.OnPointerClick(eventData);
			if (!this.IsAssigned())
			{
				return;
			}
			if (this.onClick != null)
			{
				this.onClick.Invoke(this);
			}
		}

		// Token: 0x06002250 RID: 8784 RVA: 0x000AA5D5 File Offset: 0x000A87D5
		public override void OnTooltip(bool show)
		{
			if (this.m_SpellInfo == null)
			{
				return;
			}
			if (show)
			{
				UITooltip.InstantiateIfNecessary(base.gameObject);
				UISpellSlot.PrepareTooltip(this.m_SpellInfo);
				UITooltip.AnchorToRect(base.transform as RectTransform);
				UITooltip.Show();
				return;
			}
			UITooltip.Hide();
		}

		// Token: 0x06002251 RID: 8785 RVA: 0x000AA614 File Offset: 0x000A8814
		public void SetCooldownComponent(UISlotCooldown cooldown)
		{
			this.m_Cooldown = cooldown;
		}

		// Token: 0x06002252 RID: 8786 RVA: 0x000AA620 File Offset: 0x000A8820
		[ContextMenu("Auto Assign ID")]
		public void AutoAssignID()
		{
			List<UISpellSlot> slotsInGroup = UISpellSlot.GetSlotsInGroup(this.m_SlotGroup);
			if (slotsInGroup.Count > 0)
			{
				slotsInGroup.Reverse();
				this.m_ID = slotsInGroup[0].ID + 1;
			}
			else
			{
				this.m_ID = 1;
			}
			slotsInGroup.Clear();
		}

		// Token: 0x06002253 RID: 8787 RVA: 0x000AA66C File Offset: 0x000A886C
		public static void PrepareTooltip(UISpellInfo spellInfo)
		{
			if (spellInfo == null)
			{
				return;
			}
			if (UITooltipManager.Instance != null)
			{
				UITooltip.SetWidth((float)UITooltipManager.Instance.spellTooltipWidth);
			}
			UITooltip.AddLine(spellInfo.Name, "SpellTitle");
			UITooltip.AddSpacer();
			if (spellInfo.Flags.Has(UISpellInfo_Flags.Passive))
			{
				UITooltip.AddLine("Passive", "SpellAttribute");
			}
			else
			{
				if (spellInfo.PowerCost > 0f)
				{
					if (spellInfo.Flags.Has(UISpellInfo_Flags.PowerCostInPct))
					{
						UITooltip.AddLineColumn(spellInfo.PowerCost.ToString("0") + "% Energy", "SpellAttribute");
					}
					else
					{
						UITooltip.AddLineColumn(spellInfo.PowerCost.ToString("0") + " Energy", "SpellAttribute");
					}
				}
				if (spellInfo.Range > 0f)
				{
					if (spellInfo.Range == 1f)
					{
						UITooltip.AddLineColumn("Melee range", "SpellAttribute");
					}
					else
					{
						UITooltip.AddLineColumn(spellInfo.Range.ToString("0") + " yd range", "SpellAttribute");
					}
				}
				if (spellInfo.CastTime == 0f)
				{
					UITooltip.AddLineColumn("Instant", "SpellAttribute");
				}
				else
				{
					UITooltip.AddLineColumn(spellInfo.CastTime.ToString("0.0") + " sec cast", "SpellAttribute");
				}
				if (spellInfo.Cooldown > 0f)
				{
					UITooltip.AddLineColumn(spellInfo.Cooldown.ToString("0.0") + " sec cooldown", "SpellAttribute");
				}
			}
			if (!string.IsNullOrEmpty(spellInfo.Description))
			{
				UITooltip.AddSpacer();
				UITooltip.AddLine(spellInfo.Description, "SpellDescription");
			}
		}

		// Token: 0x06002254 RID: 8788 RVA: 0x000AA81C File Offset: 0x000A8A1C
		public static List<UISpellSlot> GetSlots()
		{
			List<UISpellSlot> list = new List<UISpellSlot>();
			foreach (UISpellSlot uispellSlot in Resources.FindObjectsOfTypeAll<UISpellSlot>())
			{
				if (uispellSlot.gameObject.activeInHierarchy)
				{
					list.Add(uispellSlot);
				}
			}
			return list;
		}

		// Token: 0x06002255 RID: 8789 RVA: 0x000AA85C File Offset: 0x000A8A5C
		public static List<UISpellSlot> GetSlotsWithID(int ID)
		{
			List<UISpellSlot> list = new List<UISpellSlot>();
			foreach (UISpellSlot uispellSlot in Resources.FindObjectsOfTypeAll<UISpellSlot>())
			{
				if (uispellSlot.gameObject.activeInHierarchy && uispellSlot.ID == ID)
				{
					list.Add(uispellSlot);
				}
			}
			return list;
		}

		// Token: 0x06002256 RID: 8790 RVA: 0x000AA8A8 File Offset: 0x000A8AA8
		public static List<UISpellSlot> GetSlotsInGroup(UISpellSlot_Group group)
		{
			List<UISpellSlot> list = new List<UISpellSlot>();
			foreach (UISpellSlot uispellSlot in Resources.FindObjectsOfTypeAll<UISpellSlot>())
			{
				if (uispellSlot.gameObject.activeInHierarchy && uispellSlot.slotGroup == group)
				{
					list.Add(uispellSlot);
				}
			}
			list.Sort((UISpellSlot a, UISpellSlot b) => a.ID.CompareTo(b.ID));
			return list;
		}

		// Token: 0x06002257 RID: 8791 RVA: 0x000AA918 File Offset: 0x000A8B18
		public static UISpellSlot GetSlot(int ID, UISpellSlot_Group group)
		{
			foreach (UISpellSlot uispellSlot in Resources.FindObjectsOfTypeAll<UISpellSlot>())
			{
				if (uispellSlot.gameObject.activeInHierarchy && uispellSlot.ID == ID && uispellSlot.slotGroup == group)
				{
					return uispellSlot;
				}
			}
			return null;
		}

		// Token: 0x04001BDE RID: 7134
		[SerializeField]
		[Tooltip("Placing the slot in a group will make the slot accessible via the static method GetSlot.")]
		private UISpellSlot_Group m_SlotGroup;

		// Token: 0x04001BDF RID: 7135
		[SerializeField]
		private int m_ID;

		// Token: 0x04001BE0 RID: 7136
		public UISpellSlot.OnAssignEvent onAssign = new UISpellSlot.OnAssignEvent();

		// Token: 0x04001BE1 RID: 7137
		public UISpellSlot.OnUnassignEvent onUnassign = new UISpellSlot.OnUnassignEvent();

		// Token: 0x04001BE2 RID: 7138
		public UISpellSlot.OnClickEvent onClick = new UISpellSlot.OnClickEvent();

		// Token: 0x04001BE3 RID: 7139
		private UISpellInfo m_SpellInfo;

		// Token: 0x04001BE4 RID: 7140
		private UISlotCooldown m_Cooldown;

		// Token: 0x02000611 RID: 1553
		[Serializable]
		public class OnAssignEvent : UnityEvent<UISpellSlot>
		{
		}

		// Token: 0x02000612 RID: 1554
		[Serializable]
		public class OnUnassignEvent : UnityEvent<UISpellSlot>
		{
		}

		// Token: 0x02000613 RID: 1555
		[Serializable]
		public class OnClickEvent : UnityEvent<UISpellSlot>
		{
		}
	}
}
