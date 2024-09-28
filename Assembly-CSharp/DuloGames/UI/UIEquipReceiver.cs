using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x020005F9 RID: 1529
	[AddComponentMenu("UI/Icon Slots/Equip Receiver", 46)]
	[ExecuteInEditMode]
	public class UIEquipReceiver : UIBehaviour, IEventSystemHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x060021A4 RID: 8612 RVA: 0x000A81D8 File Offset: 0x000A63D8
		protected override void Start()
		{
			if (Application.isPlaying && this.m_SlotsContainer == null)
			{
				this.m_SlotsContainer = base.transform;
			}
			if (this.m_HintText != null)
			{
				this.m_HintText.canvasRenderer.SetAlpha((this.m_HintState == UIEquipReceiver.HintState.Hidden) ? 0f : 1f);
			}
		}

		// Token: 0x060021A5 RID: 8613 RVA: 0x000A823C File Offset: 0x000A643C
		public UIEquipSlot GetSlotByType(UIEquipmentType type)
		{
			foreach (UIEquipSlot uiequipSlot in this.m_SlotsContainer.GetComponentsInChildren<UIEquipSlot>())
			{
				if (uiequipSlot.enabled && uiequipSlot.gameObject.activeSelf && uiequipSlot.equipType == type)
				{
					return uiequipSlot;
				}
			}
			return null;
		}

		// Token: 0x060021A6 RID: 8614 RVA: 0x000A8288 File Offset: 0x000A6488
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.m_HintText == null)
			{
				return;
			}
			if (eventData.dragging && this.ExtractSlot(eventData) != null)
			{
				if (this.m_Fading)
				{
					this.m_HintText.CrossFadeAlpha(1f, this.m_FadeDuration, true);
				}
				else
				{
					this.m_HintText.canvasRenderer.SetAlpha(1f);
				}
				this.m_HintState = UIEquipReceiver.HintState.Shown;
			}
		}

		// Token: 0x060021A7 RID: 8615 RVA: 0x000A82F8 File Offset: 0x000A64F8
		private void HideHint()
		{
			if (this.m_HintState == UIEquipReceiver.HintState.Hidden)
			{
				return;
			}
			if (this.m_Fading)
			{
				this.m_HintText.CrossFadeAlpha(0f, this.m_FadeDuration, true);
			}
			else
			{
				this.m_HintText.canvasRenderer.SetAlpha(0f);
			}
			this.m_HintState = UIEquipReceiver.HintState.Hidden;
		}

		// Token: 0x060021A8 RID: 8616 RVA: 0x000A834C File Offset: 0x000A654C
		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.m_HintText == null)
			{
				return;
			}
			this.HideHint();
		}

		// Token: 0x060021A9 RID: 8617 RVA: 0x000A8364 File Offset: 0x000A6564
		public void OnDrop(PointerEventData eventData)
		{
			if (eventData.pointerPress == null)
			{
				return;
			}
			UISlotBase uislotBase = this.ExtractSlot(eventData);
			if (uislotBase == null)
			{
				return;
			}
			if (uislotBase is UIItemSlot)
			{
				UIItemSlot uiitemSlot = uislotBase as UIItemSlot;
				if (uiitemSlot != null && uiitemSlot.IsAssigned())
				{
					UIEquipSlot slotByType = this.GetSlotByType(uiitemSlot.GetItemInfo().EquipType);
					if (slotByType != null)
					{
						slotByType.OnDrop(eventData);
						this.HideHint();
						return;
					}
				}
			}
		}

		// Token: 0x060021AA RID: 8618 RVA: 0x000A83DC File Offset: 0x000A65DC
		private UISlotBase ExtractSlot(PointerEventData eventData)
		{
			if (eventData.pointerPress == null)
			{
				return null;
			}
			UISlotBase uislotBase = eventData.pointerPress.GetComponent<UISlotBase>();
			if (uislotBase == null)
			{
				uislotBase = eventData.pointerPress.GetComponentInChildren<UISlotBase>();
			}
			return uislotBase;
		}

		// Token: 0x04001B7E RID: 7038
		[SerializeField]
		private Transform m_SlotsContainer;

		// Token: 0x04001B7F RID: 7039
		[SerializeField]
		private Text m_HintText;

		// Token: 0x04001B80 RID: 7040
		[SerializeField]
		private bool m_Fading = true;

		// Token: 0x04001B81 RID: 7041
		[SerializeField]
		private float m_FadeDuration = 0.15f;

		// Token: 0x04001B82 RID: 7042
		[SerializeField]
		private UIEquipReceiver.HintState m_HintState = UIEquipReceiver.HintState.Hidden;

		// Token: 0x020005FA RID: 1530
		public enum HintState
		{
			// Token: 0x04001B84 RID: 7044
			Shown,
			// Token: 0x04001B85 RID: 7045
			Hidden
		}
	}
}
