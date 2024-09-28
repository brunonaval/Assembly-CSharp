using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x020005FB RID: 1531
	[AddComponentMenu("UI/Icon Slots/Equip Slot", 12)]
	public class UIEquipSlot : UISlotBase, IUIItemSlot
	{
		// Token: 0x1700033A RID: 826
		// (get) Token: 0x060021AC RID: 8620 RVA: 0x000A843C File Offset: 0x000A663C
		// (set) Token: 0x060021AD RID: 8621 RVA: 0x000A8444 File Offset: 0x000A6644
		public UIEquipmentType equipType
		{
			get
			{
				return this.m_EquipType;
			}
			set
			{
				this.m_EquipType = value;
			}
		}

		// Token: 0x060021AE RID: 8622 RVA: 0x000A844D File Offset: 0x000A664D
		public UIItemInfo GetItemInfo()
		{
			return this.m_ItemInfo;
		}

		// Token: 0x060021AF RID: 8623 RVA: 0x000A8455 File Offset: 0x000A6655
		public override bool IsAssigned()
		{
			return this.m_ItemInfo != null;
		}

		// Token: 0x060021B0 RID: 8624 RVA: 0x000A8460 File Offset: 0x000A6660
		public bool Assign(UIItemInfo itemInfo, UnityEngine.Object source)
		{
			if (itemInfo == null)
			{
				return false;
			}
			if (!this.CheckEquipType(itemInfo))
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

		// Token: 0x060021B1 RID: 8625 RVA: 0x000A84C1 File Offset: 0x000A66C1
		public bool Assign(UIItemInfo itemInfo)
		{
			return this.Assign(itemInfo, null);
		}

		// Token: 0x060021B2 RID: 8626 RVA: 0x000A84CC File Offset: 0x000A66CC
		public override bool Assign(UnityEngine.Object source)
		{
			if (source is IUIItemSlot)
			{
				IUIItemSlot iuiitemSlot = source as IUIItemSlot;
				if (iuiitemSlot != null)
				{
					return this.CheckEquipType(iuiitemSlot.GetItemInfo()) && this.Assign(iuiitemSlot.GetItemInfo(), source);
				}
			}
			return false;
		}

		// Token: 0x060021B3 RID: 8627 RVA: 0x000A850A File Offset: 0x000A670A
		public virtual bool CheckEquipType(UIItemInfo info)
		{
			return info != null && info.EquipType == this.equipType;
		}

		// Token: 0x060021B4 RID: 8628 RVA: 0x000A8522 File Offset: 0x000A6722
		public override void Unassign()
		{
			base.Unassign();
			this.m_ItemInfo = null;
			if (this.onUnassign != null)
			{
				this.onUnassign.Invoke(this);
			}
		}

		// Token: 0x060021B5 RID: 8629 RVA: 0x000A8545 File Offset: 0x000A6745
		public override bool CanSwapWith(UnityEngine.Object target)
		{
			return target is IUIItemSlot && (!(target is UIEquipSlot) || (target as UIEquipSlot).CheckEquipType(this.GetItemInfo()));
		}

		// Token: 0x060021B6 RID: 8630 RVA: 0x000A856C File Offset: 0x000A676C
		public override bool PerformSlotSwap(UnityEngine.Object sourceObject)
		{
			IUIItemSlot iuiitemSlot = sourceObject as IUIItemSlot;
			UIItemInfo itemInfo = iuiitemSlot.GetItemInfo();
			bool flag = iuiitemSlot.Assign(this.GetItemInfo(), this);
			bool flag2 = this.Assign(itemInfo, sourceObject);
			return flag && flag2;
		}

		// Token: 0x060021B7 RID: 8631 RVA: 0x000A85A0 File Offset: 0x000A67A0
		public override void OnTooltip(bool show)
		{
			UITooltip.InstantiateIfNecessary(base.gameObject);
			if (!this.IsAssigned())
			{
				if (show)
				{
					UITooltip.AddTitle(UIEquipSlot.EquipTypeToString(this.m_EquipType));
					UITooltip.SetHorizontalFitMode(ContentSizeFitter.FitMode.PreferredSize);
					UITooltip.AnchorToRect(base.transform as RectTransform);
					UITooltip.Show();
					return;
				}
				UITooltip.Hide();
				return;
			}
			else
			{
				if (this.m_ItemInfo == null)
				{
					return;
				}
				if (show)
				{
					UIItemSlot.PrepareTooltip(this.m_ItemInfo);
					UITooltip.AnchorToRect(base.transform as RectTransform);
					UITooltip.Show();
					return;
				}
				UITooltip.Hide();
				return;
			}
		}

		// Token: 0x060021B8 RID: 8632 RVA: 0x000A8628 File Offset: 0x000A6828
		protected override void OnThrowAwayDenied()
		{
			if (!this.IsAssigned())
			{
				return;
			}
			List<UIItemSlot> slotsInGroup = UIItemSlot.GetSlotsInGroup(UIItemSlot_Group.Inventory);
			if (slotsInGroup.Count > 0)
			{
				foreach (UIItemSlot uiitemSlot in slotsInGroup)
				{
					if (!uiitemSlot.IsAssigned())
					{
						uiitemSlot.Assign(this);
						this.Unassign();
						break;
					}
				}
			}
		}

		// Token: 0x060021B9 RID: 8633 RVA: 0x000A86A0 File Offset: 0x000A68A0
		public static string EquipTypeToString(UIEquipmentType type)
		{
			string result = "Undefined";
			switch (type)
			{
			case UIEquipmentType.Weapon_MainHand:
				result = "Main Hand";
				break;
			case UIEquipmentType.Weapon_OffHand:
				result = "Off Hand";
				break;
			case UIEquipmentType.Head:
				result = "Head";
				break;
			case UIEquipmentType.Necklace:
				result = "Necklace";
				break;
			case UIEquipmentType.Shoulders:
				result = "Shoulders";
				break;
			case UIEquipmentType.Chest:
				result = "Chest";
				break;
			case UIEquipmentType.Back:
				result = "Back";
				break;
			case UIEquipmentType.Bracers:
				result = "Bracers";
				break;
			case UIEquipmentType.Gloves:
				result = "Gloves";
				break;
			case UIEquipmentType.Belt:
				result = "Belt";
				break;
			case UIEquipmentType.Pants:
				result = "Pants";
				break;
			case UIEquipmentType.Boots:
				result = "Boots";
				break;
			case UIEquipmentType.Finger:
				result = "Ring";
				break;
			case UIEquipmentType.Trinket:
				result = "Trinket";
				break;
			}
			return result;
		}

		// Token: 0x060021BA RID: 8634 RVA: 0x000A8764 File Offset: 0x000A6964
		public static List<UIEquipSlot> GetSlots()
		{
			List<UIEquipSlot> list = new List<UIEquipSlot>();
			foreach (UIEquipSlot uiequipSlot in Resources.FindObjectsOfTypeAll<UIEquipSlot>())
			{
				if (uiequipSlot.gameObject.activeInHierarchy)
				{
					list.Add(uiequipSlot);
				}
			}
			return list;
		}

		// Token: 0x060021BB RID: 8635 RVA: 0x000A87A4 File Offset: 0x000A69A4
		public static UIEquipSlot GetSlotWithType(UIEquipmentType type)
		{
			foreach (UIEquipSlot uiequipSlot in Resources.FindObjectsOfTypeAll<UIEquipSlot>())
			{
				if (uiequipSlot.gameObject.activeInHierarchy && uiequipSlot.equipType == type)
				{
					return uiequipSlot;
				}
			}
			return null;
		}

		// Token: 0x060021BC RID: 8636 RVA: 0x000A87E4 File Offset: 0x000A69E4
		public static List<UIEquipSlot> GetSlotsWithType(UIEquipmentType type)
		{
			List<UIEquipSlot> list = new List<UIEquipSlot>();
			foreach (UIEquipSlot uiequipSlot in Resources.FindObjectsOfTypeAll<UIEquipSlot>())
			{
				if (uiequipSlot.gameObject.activeInHierarchy && uiequipSlot.equipType == type)
				{
					list.Add(uiequipSlot);
				}
			}
			return list;
		}

		// Token: 0x04001B86 RID: 7046
		[SerializeField]
		private UIEquipmentType m_EquipType;

		// Token: 0x04001B87 RID: 7047
		private UIItemInfo m_ItemInfo;

		// Token: 0x04001B88 RID: 7048
		public UIEquipSlot.OnAssignEvent onAssign = new UIEquipSlot.OnAssignEvent();

		// Token: 0x04001B89 RID: 7049
		public UIEquipSlot.OnAssignWithSourceEvent onAssignWithSource = new UIEquipSlot.OnAssignWithSourceEvent();

		// Token: 0x04001B8A RID: 7050
		public UIEquipSlot.OnUnassignEvent onUnassign = new UIEquipSlot.OnUnassignEvent();

		// Token: 0x020005FC RID: 1532
		[Serializable]
		public class OnAssignEvent : UnityEvent<UIEquipSlot>
		{
		}

		// Token: 0x020005FD RID: 1533
		[Serializable]
		public class OnAssignWithSourceEvent : UnityEvent<UIEquipSlot, UnityEngine.Object>
		{
		}

		// Token: 0x020005FE RID: 1534
		[Serializable]
		public class OnUnassignEvent : UnityEvent<UIEquipSlot>
		{
		}
	}
}
