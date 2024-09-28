using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x020005DC RID: 1500
	[AddComponentMenu("UI/Joystick", 36)]
	[RequireComponent(typeof(RectTransform))]
	public class UIJoystick : UIBehaviour, IBeginDragHandler, IEventSystemHandler, IEndDragHandler, IDragHandler, IPointerDownHandler, IPointerUpHandler
	{
		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06002103 RID: 8451 RVA: 0x000A5AB4 File Offset: 0x000A3CB4
		// (set) Token: 0x06002104 RID: 8452 RVA: 0x000A5ABC File Offset: 0x000A3CBC
		public RectTransform Handle
		{
			get
			{
				return this.m_Handle;
			}
			set
			{
				this.m_Handle = value;
				this.UpdateHandle();
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06002105 RID: 8453 RVA: 0x000A5ACB File Offset: 0x000A3CCB
		// (set) Token: 0x06002106 RID: 8454 RVA: 0x000A5AD3 File Offset: 0x000A3CD3
		public RectTransform HandlingArea
		{
			get
			{
				return this.m_HandlingArea;
			}
			set
			{
				this.m_HandlingArea = value;
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06002107 RID: 8455 RVA: 0x000A5ADC File Offset: 0x000A3CDC
		// (set) Token: 0x06002108 RID: 8456 RVA: 0x000A5AE4 File Offset: 0x000A3CE4
		public Image ActiveGraphic
		{
			get
			{
				return this.m_ActiveGraphic;
			}
			set
			{
				this.m_ActiveGraphic = value;
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06002109 RID: 8457 RVA: 0x000A5AED File Offset: 0x000A3CED
		// (set) Token: 0x0600210A RID: 8458 RVA: 0x000A5AF5 File Offset: 0x000A3CF5
		public float Spring
		{
			get
			{
				return this.m_Spring;
			}
			set
			{
				this.m_Spring = value;
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x0600210B RID: 8459 RVA: 0x000A5AFE File Offset: 0x000A3CFE
		// (set) Token: 0x0600210C RID: 8460 RVA: 0x000A5B06 File Offset: 0x000A3D06
		public float DeadZone
		{
			get
			{
				return this.m_DeadZone;
			}
			set
			{
				this.m_DeadZone = value;
			}
		}

		// Token: 0x0600210D RID: 8461 RVA: 0x000A5B10 File Offset: 0x000A3D10
		protected override void OnEnable()
		{
			base.OnEnable();
			this.CreateVirtualAxes();
			if (this.m_HandlingArea == null)
			{
				this.m_HandlingArea = (base.transform as RectTransform);
			}
			if (this.m_ActiveGraphic != null)
			{
				this.m_ActiveGraphic.canvasRenderer.SetAlpha(0f);
			}
		}

		// Token: 0x0600210E RID: 8462 RVA: 0x0000219A File Offset: 0x0000039A
		protected void CreateVirtualAxes()
		{
		}

		// Token: 0x0600210F RID: 8463 RVA: 0x000A5B6C File Offset: 0x000A3D6C
		protected void LateUpdate()
		{
			if (base.isActiveAndEnabled && !this.m_IsDragging && this.m_Axis != Vector2.zero)
			{
				Vector2 axis = this.m_Axis - this.m_Axis * Time.unscaledDeltaTime * this.m_Spring;
				if (axis.sqrMagnitude <= 0.0001f)
				{
					axis = Vector2.zero;
				}
				this.SetAxis(axis);
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06002110 RID: 8464 RVA: 0x000A5BE0 File Offset: 0x000A3DE0
		// (set) Token: 0x06002111 RID: 8465 RVA: 0x000A5C2A File Offset: 0x000A3E2A
		public Vector2 JoystickAxis
		{
			get
			{
				Vector2 a = (this.m_Axis.magnitude > this.m_DeadZone) ? this.m_Axis : Vector2.zero;
				float magnitude = a.magnitude;
				return a * this.outputCurve.Evaluate(magnitude);
			}
			set
			{
				this.SetAxis(value);
			}
		}

		// Token: 0x06002112 RID: 8466 RVA: 0x000A5C34 File Offset: 0x000A3E34
		public void SetAxis(Vector2 axis)
		{
			this.m_Axis = Vector2.ClampMagnitude(axis, 1f);
			Vector2 a = (this.m_Axis.magnitude > this.m_DeadZone) ? this.m_Axis : Vector2.zero;
			float magnitude = a.magnitude;
			a *= this.outputCurve.Evaluate(magnitude);
			this.UpdateHandle();
		}

		// Token: 0x06002113 RID: 8467 RVA: 0x000A5C94 File Offset: 0x000A3E94
		public void OnBeginDrag(PointerEventData eventData)
		{
			if (!this.IsActive() || this.m_HandlingArea == null)
			{
				return;
			}
			Vector2 axis = this.m_HandlingArea.InverseTransformPoint(eventData.position);
			axis.x /= this.m_HandlingArea.sizeDelta.x * 0.5f;
			axis.y /= this.m_HandlingArea.sizeDelta.y * 0.5f;
			this.SetAxis(axis);
			this.m_IsDragging = true;
		}

		// Token: 0x06002114 RID: 8468 RVA: 0x000A5D24 File Offset: 0x000A3F24
		public void OnEndDrag(PointerEventData eventData)
		{
			this.m_IsDragging = false;
		}

		// Token: 0x06002115 RID: 8469 RVA: 0x000A5D30 File Offset: 0x000A3F30
		public void OnDrag(PointerEventData eventData)
		{
			if (this.m_HandlingArea == null)
			{
				return;
			}
			Vector2 vector = Vector2.zero;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_HandlingArea, eventData.position, eventData.pressEventCamera, out vector);
			vector -= this.m_HandlingArea.rect.center;
			vector.x /= this.m_HandlingArea.sizeDelta.x * 0.5f;
			vector.y /= this.m_HandlingArea.sizeDelta.y * 0.5f;
			this.SetAxis(vector);
		}

		// Token: 0x06002116 RID: 8470 RVA: 0x000A5DD0 File Offset: 0x000A3FD0
		private void UpdateHandle()
		{
			if (this.m_Handle && this.m_HandlingArea)
			{
				this.m_Handle.anchoredPosition = new Vector2(this.m_Axis.x * this.m_HandlingArea.sizeDelta.x * 0.5f, this.m_Axis.y * this.m_HandlingArea.sizeDelta.y * 0.5f);
			}
		}

		// Token: 0x06002117 RID: 8471 RVA: 0x000A5E4B File Offset: 0x000A404B
		public void OnPointerDown(PointerEventData eventData)
		{
			if (this.m_ActiveGraphic != null)
			{
				this.m_ActiveGraphic.CrossFadeAlpha(1f, 0.2f, false);
			}
		}

		// Token: 0x06002118 RID: 8472 RVA: 0x000A5E71 File Offset: 0x000A4071
		public void OnPointerUp(PointerEventData eventData)
		{
			if (this.m_ActiveGraphic != null)
			{
				this.m_ActiveGraphic.CrossFadeAlpha(0f, 0.2f, false);
			}
		}

		// Token: 0x04001AEB RID: 6891
		[SerializeField]
		[Tooltip("The child graphic that will be moved around.")]
		private RectTransform m_Handle;

		// Token: 0x04001AEC RID: 6892
		[SerializeField]
		[Tooltip("The handling area that the handle is allowed to be moved in.")]
		private RectTransform m_HandlingArea;

		// Token: 0x04001AED RID: 6893
		[SerializeField]
		[Tooltip("The child graphic that will be shown when the joystick is active.")]
		private Image m_ActiveGraphic;

		// Token: 0x04001AEE RID: 6894
		[SerializeField]
		private Vector2 m_Axis;

		// Token: 0x04001AEF RID: 6895
		[SerializeField]
		[Tooltip("How fast the joystick will go back to the center")]
		private float m_Spring = 25f;

		// Token: 0x04001AF0 RID: 6896
		[SerializeField]
		[Tooltip("How close to the center that the axis will be output as 0")]
		private float m_DeadZone = 0.1f;

		// Token: 0x04001AF1 RID: 6897
		[Tooltip("Customize the output that is sent in OnValueChange")]
		public AnimationCurve outputCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f, 1f, 1f),
			new Keyframe(1f, 1f, 1f, 1f)
		});

		// Token: 0x04001AF2 RID: 6898
		private bool m_IsDragging;

		// Token: 0x020005DD RID: 1501
		public enum AxisOption
		{
			// Token: 0x04001AF4 RID: 6900
			Both,
			// Token: 0x04001AF5 RID: 6901
			OnlyHorizontal,
			// Token: 0x04001AF6 RID: 6902
			OnlyVertical
		}
	}
}
