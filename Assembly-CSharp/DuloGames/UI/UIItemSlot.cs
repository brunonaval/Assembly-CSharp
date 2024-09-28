using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DuloGames.UI
{
	// Token: 0x020005FF RID: 1535
	[AddComponentMenu("UI/Icon Slots/Item Slot", 12)]
	public class UIItemSlot : UISlotBase, IUIItemSlot
	{
		// Token: 0x1700033B RID: 827
		// (get) Token: 0x060021C1 RID: 8641 RVA: 0x000A8866 File Offset: 0x000A6A66
		// (set) Token: 0x060021C2 RID: 8642 RVA: 0x000A886E File Offset: 0x000A6A6E
		public UIItemSlot_Group slotGroup
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

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x060021C3 RID: 8643 RVA: 0x000A8877 File Offset: 0x000A6A77
		// (set) Token: 0x060021C4 RID: 8644 RVA: 0x000A887F File Offset: 0x000A6A7F
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

		// Token: 0x060021C5 RID: 8645 RVA: 0x000A8888 File Offset: 0x000A6A88
		public UIItemInfo GetItemInfo()
		{
			return this.m_ItemInfo;
		}

		// Token: 0x060021C6 RID: 8646 RVA: 0x000A8890 File Offset: 0x000A6A90
		protected override void OnEnable()
		{
			base.OnEnable();
		}

		// Token: 0x060021C7 RID: 8647 RVA: 0x000A8898 File Offset: 0x000A6A98
		public override bool IsAssigned()
		{
			return this.m_ItemInfo != null;
		}

		// Token: 0x060021C8 RID: 8648 RVA: 0x000A88A4 File Offset: 0x000A6AA4
		public bool Assign(UIItemInfo itemInfo, UnityEngine.Object source)
		{
			if (itemInfo == null)
			{
				return false;
			}
			this.Unassign();
			base.Assign(itemInfo.Icon);
			this.m_ItemInfo = itemInfo;
			if (this.onAssign != null)
			{
				this.onAssign.Invoke(this);
			}
			if (this.onAssignWithSource != null)
			{
				this.onAssignWithSource.Invoke(this, source);
			}
			return true;
		}

		// Token: 0x060021C9 RID: 8649 RVA: 0x000A88FA File Offset: 0x000A6AFA
		public bool Assign(UIItemInfo itemInfo)
		{
			return this.Assign(itemInfo, null);
		}

		// Token: 0x060021CA RID: 8650 RVA: 0x000A8904 File Offset: 0x000A6B04
		public override bool Assign(UnityEngine.Object source)
		{
			if (source is IUIItemSlot)
			{
				IUIItemSlot iuiitemSlot = source as IUIItemSlot;
				if (iuiitemSlot != null)
				{
					return this.Assign(iuiitemSlot.GetItemInfo(), source);
				}
			}
			return false;
		}

		// Token: 0x060021CB RID: 8651 RVA: 0x000A8932 File Offset: 0x000A6B32
		public override void Unassign()
		{
			base.Unassign();
			this.m_ItemInfo = null;
			if (this.onUnassign != null)
			{
				this.onUnassign.Invoke(this);
			}
		}

		// Token: 0x060021CC RID: 8652 RVA: 0x000A8955 File Offset: 0x000A6B55
		public override bool CanSwapWith(UnityEngine.Object target)
		{
			return target is IUIItemSlot && (!(target is UIEquipSlot) || (target as UIEquipSlot).CheckEquipType(this.GetItemInfo()));
		}

		// Token: 0x060021CD RID: 8653 RVA: 0x000A897C File Offset: 0x000A6B7C
		public override bool PerformSlotSwap(UnityEngine.Object sourceObject)
		{
			IUIItemSlot iuiitemSlot = sourceObject as IUIItemSlot;
			UIItemInfo itemInfo = iuiitemSlot.GetItemInfo();
			bool flag = iuiitemSlot.Assign(this.GetItemInfo(), this);
			bool flag2 = this.Assign(itemInfo, sourceObject);
			return flag && flag2;
		}

		// Token: 0x060021CE RID: 8654 RVA: 0x000A89AD File Offset: 0x000A6BAD
		public override void OnTooltip(bool show)
		{
			if (this.m_ItemInfo == null)
			{
				return;
			}
			if (show)
			{
				UITooltip.InstantiateIfNecessary(base.gameObject);
				UIItemSlot.PrepareTooltip(this.m_ItemInfo);
				UITooltip.AnchorToRect(base.transform as RectTransform);
				UITooltip.Show();
				return;
			}
			UITooltip.Hide();
		}

		// Token: 0x060021CF RID: 8655 RVA: 0x000A89EC File Offset: 0x000A6BEC
		public override void OnPointerClick(PointerEventData eventData)
		{
			base.OnPointerClick(eventData);
			if (!this.IsAssigned())
			{
				return;
			}
			if (eventData.button == PointerEventData.InputButton.Left && eventData.clickCount == 2)
			{
				eventData.clickCount = 0;
				if (this.onDoubleClick != null)
				{
					this.onDoubleClick.Invoke(this);
				}
			}
			if (eventData.button == PointerEventData.InputButton.Right && this.onRightClick != null)
			{
				this.onRightClick.Invoke(this);
			}
		}

		// Token: 0x060021D0 RID: 8656 RVA: 0x000A8A54 File Offset: 0x000A6C54
		protected override void OnThrowAwayDenied()
		{
			if (!this.IsAssigned())
			{
				return;
			}
			if (UIModalBoxManager.Instance == null)
			{
				Debug.LogWarning("Could not load the modal box manager while creating a modal box.");
				return;
			}
			UIModalBox uimodalBox = UIModalBoxManager.Instance.Create(base.gameObject);
			if (uimodalBox != null)
			{
				uimodalBox.SetText1("Do you really want to destroy \"" + this.m_ItemInfo.Name + "\"?");
				uimodalBox.SetText2("You wont be able to reverse this operation and your item will be permamently removed.");
				uimodalBox.SetConfirmButtonText("DESTROY");
				uimodalBox.onConfirm.AddListener(new UnityAction(this.Unassign));
				uimodalBox.Show();
			}
		}

		// Token: 0x060021D1 RID: 8657 RVA: 0x000A8AF0 File Offset: 0x000A6CF0
		[ContextMenu("Auto Assign ID")]
		public void AutoAssignID()
		{
			List<UIItemSlot> slotsInGroup = UIItemSlot.GetSlotsInGroup(this.m_SlotGroup);
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

		// Token: 0x060021D2 RID: 8658 RVA: 0x000A8B3C File Offset: 0x000A6D3C
		public static List<UIItemSlot> GetSlots()
		{
			List<UIItemSlot> list = new List<UIItemSlot>();
			foreach (UIItemSlot uiitemSlot in Resources.FindObjectsOfTypeAll<UIItemSlot>())
			{
				if (uiitemSlot.gameObject.activeInHierarchy)
				{
					list.Add(uiitemSlot);
				}
			}
			return list;
		}

		// Token: 0x060021D3 RID: 8659 RVA: 0x000A8B7C File Offset: 0x000A6D7C
		public static List<UIItemSlot> GetSlotsWithID(int ID)
		{
			List<UIItemSlot> list = new List<UIItemSlot>();
			foreach (UIItemSlot uiitemSlot in Resources.FindObjectsOfTypeAll<UIItemSlot>())
			{
				if (uiitemSlot.gameObject.activeInHierarchy && uiitemSlot.ID == ID)
				{
					list.Add(uiitemSlot);
				}
			}
			return list;
		}

		// Token: 0x060021D4 RID: 8660 RVA: 0x000A8BC8 File Offset: 0x000A6DC8
		public static List<UIItemSlot> GetSlotsInGroup(UIItemSlot_Group group)
		{
			List<UIItemSlot> list = new List<UIItemSlot>();
			foreach (UIItemSlot uiitemSlot in Resources.FindObjectsOfTypeAll<UIItemSlot>())
			{
				if (uiitemSlot.gameObject.activeInHierarchy && uiitemSlot.slotGroup == group)
				{
					list.Add(uiitemSlot);
				}
			}
			list.Sort((UIItemSlot a, UIItemSlot b) => a.ID.CompareTo(b.ID));
			return list;
		}

		// Token: 0x060021D5 RID: 8661 RVA: 0x000A8C38 File Offset: 0x000A6E38
		public static UIItemSlot GetSlot(int ID, UIItemSlot_Group group)
		{
			foreach (UIItemSlot uiitemSlot in Resources.FindObjectsOfTypeAll<UIItemSlot>())
			{
				if (uiitemSlot.gameObject.activeInHierarchy && uiitemSlot.ID == ID && uiitemSlot.slotGroup == group)
				{
					return uiitemSlot;
				}
			}
			return null;
		}

		// Token: 0x060021D6 RID: 8662 RVA: 0x000A8C80 File Offset: 0x000A6E80
		public static void PrepareTooltip(UIItemInfo itemInfo)
		{
			if (itemInfo == null)
			{
				return;
			}
			if (UITooltipManager.Instance != null)
			{
				UITooltip.SetWidth((float)UITooltipManager.Instance.itemTooltipWidth);
			}
			UITooltip.AddTitle(string.Concat(new string[]
			{
				"<color=#",
				UIItemQualityColor.GetHexColor(itemInfo.Quality),
				">",
				itemInfo.Name,
				"</color>"
			}));
			UITooltip.AddSpacer();
			UITooltip.AddLineColumn(itemInfo.Type, "ItemAttribute");
			UITooltip.AddLineColumn(itemInfo.Subtype, "ItemAttribute");
			if (itemInfo.ItemType == 1)
			{
				UITooltip.AddLineColumn(itemInfo.Damage.ToString() + " Damage", "ItemAttribute");
				UITooltip.AddLineColumn(itemInfo.AttackSpeed.ToString("0.0") + " Attack speed", "ItemAttribute");
				UITooltip.AddLine("(" + ((float)itemInfo.Damage / itemInfo.AttackSpeed).ToString("0.0") + " damage per second)", "ItemAttribute");
			}
			else
			{
				UITooltip.AddLineColumn(itemInfo.Armor.ToString() + " Armor", "ItemAttribute");
				UITooltip.AddLineColumn(itemInfo.Block.ToString() + " Block", "ItemAttribute");
			}
			UITooltip.AddSpacer();
			UITooltip.AddLine("+" + itemInfo.Stamina.ToString() + " Stamina", "ItemStat");
			UITooltip.AddLine("+" + itemInfo.Strength.ToString() + " Strength", "ItemStat");
			UITooltip.AddSpacer();
			UITooltip.AddLine("Durability " + itemInfo.Durability.ToString() + "/" + itemInfo.Durability.ToString(), "ItemAttribute");
			if (itemInfo.RequiredLevel > 0)
			{
				UITooltip.AddLine("Requires Level " + itemInfo.RequiredLevel.ToString(), "ItemAttribute");
			}
			if (!string.IsNullOrEmpty(itemInfo.Description))
			{
				UITooltip.AddSpacer();
				UITooltip.AddLine(itemInfo.Description, "ItemDescription");
			}
		}

		// Token: 0x04001B8B RID: 7051
		[SerializeField]
		private UIItemSlot_Group m_SlotGroup;

		// Token: 0x04001B8C RID: 7052
		[SerializeField]
		private int m_ID;

		// Token: 0x04001B8D RID: 7053
		private UIItemInfo m_ItemInfo;

		// Token: 0x04001B8E RID: 7054
		public UIItemSlot.OnRightClickEvent onRightClick = new UIItemSlot.OnRightClickEvent();

		// Token: 0x04001B8F RID: 7055
		public UIItemSlot.OnDoubleClickEvent onDoubleClick = new UIItemSlot.OnDoubleClickEvent();

		// Token: 0x04001B90 RID: 7056
		public UIItemSlot.OnAssignEvent onAssign = new UIItemSlot.OnAssignEvent();

		// Token: 0x04001B91 RID: 7057
		public UIItemSlot.OnAssignWithSourceEvent onAssignWithSource = new UIItemSlot.OnAssignWithSourceEvent();

		// Token: 0x04001B92 RID: 7058
		public UIItemSlot.OnUnassignEvent onUnassign = new UIItemSlot.OnUnassignEvent();

		// Token: 0x02000600 RID: 1536
		[Serializable]
		public class OnRightClickEvent : UnityEvent<UIItemSlot>
		{
		}

		// Token: 0x02000601 RID: 1537
		[Serializable]
		public class OnDoubleClickEvent : UnityEvent<UIItemSlot>
		{
		}

		// Token: 0x02000602 RID: 1538
		[Serializable]
		public class OnAssignEvent : UnityEvent<UIItemSlot>
		{
		}

		// Token: 0x02000603 RID: 1539
		[Serializable]
		public class OnAssignWithSourceEvent : UnityEvent<UIItemSlot, UnityEngine.Object>
		{
		}

		// Token: 0x02000604 RID: 1540
		[Serializable]
		public class OnUnassignEvent : UnityEvent<UIItemSlot>
		{
		}
	}
}
