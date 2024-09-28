using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x02000607 RID: 1543
	[RequireComponent(typeof(UIItemSlot))]
	public class UIItemSlot_SetColorFromQuality : MonoBehaviour
	{
		// Token: 0x060021E0 RID: 8672 RVA: 0x000A8F19 File Offset: 0x000A7119
		protected void Awake()
		{
			this.m_Slot = base.gameObject.GetComponent<UIItemSlot>();
		}

		// Token: 0x060021E1 RID: 8673 RVA: 0x000A8F2C File Offset: 0x000A712C
		protected void OnEnable()
		{
			this.m_Slot.onAssign.AddListener(new UnityAction<UIItemSlot>(this.OnSlotAssign));
			this.m_Slot.onUnassign.AddListener(new UnityAction<UIItemSlot>(this.OnSlotUnassign));
		}

		// Token: 0x060021E2 RID: 8674 RVA: 0x000A8F66 File Offset: 0x000A7166
		protected void OnDisable()
		{
			this.m_Slot.onAssign.RemoveListener(new UnityAction<UIItemSlot>(this.OnSlotAssign));
			this.m_Slot.onUnassign.RemoveListener(new UnityAction<UIItemSlot>(this.OnSlotUnassign));
		}

		// Token: 0x060021E3 RID: 8675 RVA: 0x000A8FA0 File Offset: 0x000A71A0
		public void OnSlotAssign(UIItemSlot slot)
		{
			if (this.m_Target == null && slot.GetItemInfo() != null)
			{
				return;
			}
			this.m_Target.color = UIItemQualityColor.GetColor(slot.GetItemInfo().Quality);
		}

		// Token: 0x060021E4 RID: 8676 RVA: 0x000A8FD4 File Offset: 0x000A71D4
		public void OnSlotUnassign(UIItemSlot slot)
		{
			if (this.m_Target == null)
			{
				return;
			}
			this.m_Target.color = Color.clear;
		}

		// Token: 0x04001B9B RID: 7067
		[SerializeField]
		private Graphic m_Target;

		// Token: 0x04001B9C RID: 7068
		private UIItemSlot m_Slot;

		// Token: 0x02000608 RID: 1544
		[Serializable]
		public struct QualityToColor
		{
			// Token: 0x04001B9D RID: 7069
			public UIItemQuality quality;

			// Token: 0x04001B9E RID: 7070
			public Color color;
		}
	}
}
