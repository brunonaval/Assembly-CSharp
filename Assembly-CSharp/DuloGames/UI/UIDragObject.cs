using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DuloGames.UI
{
	// Token: 0x02000667 RID: 1639
	[AddComponentMenu("UI/Drag Object", 82)]
	public class UIDragObject : UIBehaviour, IEventSystemHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06002462 RID: 9314 RVA: 0x000B1ED6 File Offset: 0x000B00D6
		// (set) Token: 0x06002463 RID: 9315 RVA: 0x000B1EDE File Offset: 0x000B00DE
		public RectTransform target
		{
			get
			{
				return this.m_Target;
			}
			set
			{
				this.m_Target = value;
			}
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06002464 RID: 9316 RVA: 0x000B1EE7 File Offset: 0x000B00E7
		// (set) Token: 0x06002465 RID: 9317 RVA: 0x000B1EEF File Offset: 0x000B00EF
		public bool horizontal
		{
			get
			{
				return this.m_Horizontal;
			}
			set
			{
				this.m_Horizontal = value;
			}
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06002466 RID: 9318 RVA: 0x000B1EF8 File Offset: 0x000B00F8
		// (set) Token: 0x06002467 RID: 9319 RVA: 0x000B1F00 File Offset: 0x000B0100
		public bool vertical
		{
			get
			{
				return this.m_Vertical;
			}
			set
			{
				this.m_Vertical = value;
			}
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06002468 RID: 9320 RVA: 0x000B1F09 File Offset: 0x000B0109
		// (set) Token: 0x06002469 RID: 9321 RVA: 0x000B1F11 File Offset: 0x000B0111
		public bool inertia
		{
			get
			{
				return this.m_Inertia;
			}
			set
			{
				this.m_Inertia = value;
			}
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x0600246A RID: 9322 RVA: 0x000B1F1A File Offset: 0x000B011A
		// (set) Token: 0x0600246B RID: 9323 RVA: 0x000B1F22 File Offset: 0x000B0122
		public float dampeningRate
		{
			get
			{
				return this.m_DampeningRate;
			}
			set
			{
				this.m_DampeningRate = value;
			}
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x0600246C RID: 9324 RVA: 0x000B1F2B File Offset: 0x000B012B
		// (set) Token: 0x0600246D RID: 9325 RVA: 0x000B1F33 File Offset: 0x000B0133
		public bool constrainWithinCanvas
		{
			get
			{
				return this.m_ConstrainWithinCanvas;
			}
			set
			{
				this.m_ConstrainWithinCanvas = value;
			}
		}

		// Token: 0x0600246E RID: 9326 RVA: 0x000B1F3C File Offset: 0x000B013C
		protected override void Awake()
		{
			base.Awake();
			this.m_Canvas = UIUtility.FindInParents<Canvas>((this.m_Target != null) ? this.m_Target.gameObject : base.gameObject);
			if (this.m_Canvas != null)
			{
				this.m_CanvasRectTransform = (this.m_Canvas.transform as RectTransform);
			}
		}

		// Token: 0x0600246F RID: 9327 RVA: 0x000B1FA0 File Offset: 0x000B01A0
		protected override void OnTransformParentChanged()
		{
			base.OnTransformParentChanged();
			this.m_Canvas = UIUtility.FindInParents<Canvas>((this.m_Target != null) ? this.m_Target.gameObject : base.gameObject);
			if (this.m_Canvas != null)
			{
				this.m_CanvasRectTransform = (this.m_Canvas.transform as RectTransform);
			}
		}

		// Token: 0x06002470 RID: 9328 RVA: 0x000B2003 File Offset: 0x000B0203
		public override bool IsActive()
		{
			return base.IsActive() && this.m_Target != null;
		}

		// Token: 0x06002471 RID: 9329 RVA: 0x000B201B File Offset: 0x000B021B
		public void StopMovement()
		{
			this.m_Velocity = Vector2.zero;
		}

		// Token: 0x06002472 RID: 9330 RVA: 0x000B2028 File Offset: 0x000B0228
		public void OnBeginDrag(PointerEventData data)
		{
			if (!this.IsActive())
			{
				return;
			}
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_CanvasRectTransform, data.position, data.pressEventCamera, out this.m_PointerStartPosition);
			this.m_TargetStartPosition = this.m_Target.anchoredPosition;
			this.m_Velocity = Vector2.zero;
			this.m_Dragging = true;
			if (this.onBeginDrag != null)
			{
				this.onBeginDrag.Invoke(data);
			}
		}

		// Token: 0x06002473 RID: 9331 RVA: 0x000B2093 File Offset: 0x000B0293
		public void OnEndDrag(PointerEventData data)
		{
			this.m_Dragging = false;
			if (!this.IsActive())
			{
				return;
			}
			if (this.onEndDrag != null)
			{
				this.onEndDrag.Invoke(data);
			}
		}

		// Token: 0x06002474 RID: 9332 RVA: 0x000B20BC File Offset: 0x000B02BC
		public void OnDrag(PointerEventData data)
		{
			if (!this.IsActive() || this.m_Canvas == null)
			{
				return;
			}
			Vector2 vector;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_CanvasRectTransform, data.position, data.pressEventCamera, out vector);
			if (this.m_ConstrainWithinCanvas && this.m_ConstrainDrag)
			{
				vector = this.ClampToCanvas(vector);
			}
			Vector2 anchoredPosition = this.m_TargetStartPosition + (vector - this.m_PointerStartPosition);
			if (!this.m_Horizontal)
			{
				anchoredPosition.x = this.m_Target.anchoredPosition.x;
			}
			if (!this.m_Vertical)
			{
				anchoredPosition.y = this.m_Target.anchoredPosition.y;
			}
			this.m_Target.anchoredPosition = anchoredPosition;
			if (this.onDrag != null)
			{
				this.onDrag.Invoke(data);
			}
		}

		// Token: 0x06002475 RID: 9333 RVA: 0x000B2188 File Offset: 0x000B0388
		protected virtual void LateUpdate()
		{
			if (!this.m_Target)
			{
				return;
			}
			if (this.m_Dragging && this.m_Inertia)
			{
				Vector3 b = (this.m_Target.anchoredPosition - this.m_LastPosition) / Time.unscaledDeltaTime;
				this.m_Velocity = Vector3.Lerp(this.m_Velocity, b, Time.unscaledDeltaTime * 10f);
			}
			this.m_LastPosition = this.m_Target.anchoredPosition;
			if (!this.m_Dragging && this.m_Velocity != Vector2.zero)
			{
				Vector2 anchoredPosition = this.m_Target.anchoredPosition;
				this.Dampen(ref this.m_Velocity, this.m_DampeningRate, Time.unscaledDeltaTime);
				for (int i = 0; i < 2; i++)
				{
					if (this.m_Inertia)
					{
						ref Vector2 ptr = ref anchoredPosition;
						int index = i;
						ptr[index] += this.m_Velocity[i] * Time.unscaledDeltaTime;
					}
					else
					{
						this.m_Velocity[i] = 0f;
					}
				}
				if (this.m_Velocity != Vector2.zero)
				{
					if (!this.m_Horizontal)
					{
						anchoredPosition.x = this.m_Target.anchoredPosition.x;
					}
					if (!this.m_Vertical)
					{
						anchoredPosition.y = this.m_Target.anchoredPosition.y;
					}
					if (this.m_ConstrainWithinCanvas && this.m_ConstrainInertia && this.m_CanvasRectTransform != null)
					{
						Vector3[] array = new Vector3[4];
						this.m_CanvasRectTransform.GetWorldCorners(array);
						Vector3[] array2 = new Vector3[4];
						this.m_Target.GetWorldCorners(array2);
						if (array2[0].x < array[0].x || array2[2].x > array[2].x)
						{
							anchoredPosition.x = this.m_Target.anchoredPosition.x;
						}
						if (array2[3].y < array[3].y || array2[1].y > array[1].y)
						{
							anchoredPosition.y = this.m_Target.anchoredPosition.y;
						}
					}
					if (anchoredPosition != this.m_Target.anchoredPosition)
					{
						UIDragObject.Rounding inertiaRounding = this.m_InertiaRounding;
						if (inertiaRounding != UIDragObject.Rounding.Soft && inertiaRounding == UIDragObject.Rounding.Hard)
						{
							this.m_Target.anchoredPosition = new Vector2(Mathf.Round(anchoredPosition.x / 2f) * 2f, Mathf.Round(anchoredPosition.y / 2f) * 2f);
							return;
						}
						this.m_Target.anchoredPosition = new Vector2(Mathf.Round(anchoredPosition.x), Mathf.Round(anchoredPosition.y));
					}
				}
			}
		}

		// Token: 0x06002476 RID: 9334 RVA: 0x000B2470 File Offset: 0x000B0670
		protected Vector3 Dampen(ref Vector2 velocity, float strength, float delta)
		{
			if (delta > 1f)
			{
				delta = 1f;
			}
			float f = 1f - strength * 0.001f;
			int num = Mathf.RoundToInt(delta * 1000f);
			float num2 = Mathf.Pow(f, (float)num);
			Vector2 a = velocity * ((num2 - 1f) / Mathf.Log(f));
			velocity *= num2;
			return a * 0.06f;
		}

		// Token: 0x06002477 RID: 9335 RVA: 0x000B24EC File Offset: 0x000B06EC
		protected Vector2 ClampToScreen(Vector2 position)
		{
			if (this.m_Canvas != null && (this.m_Canvas.renderMode == RenderMode.ScreenSpaceOverlay || this.m_Canvas.renderMode == RenderMode.ScreenSpaceCamera))
			{
				float x = Mathf.Clamp(position.x, 0f, (float)Screen.width);
				float y = Mathf.Clamp(position.y, 0f, (float)Screen.height);
				return new Vector2(x, y);
			}
			return position;
		}

		// Token: 0x06002478 RID: 9336 RVA: 0x000B2558 File Offset: 0x000B0758
		protected Vector2 ClampToCanvas(Vector2 position)
		{
			if (this.m_CanvasRectTransform != null)
			{
				Vector3[] array = new Vector3[4];
				this.m_CanvasRectTransform.GetLocalCorners(array);
				float x = Mathf.Clamp(position.x, array[0].x, array[2].x);
				float y = Mathf.Clamp(position.y, array[3].y, array[1].y);
				return new Vector2(x, y);
			}
			return position;
		}

		// Token: 0x04001D98 RID: 7576
		[SerializeField]
		private RectTransform m_Target;

		// Token: 0x04001D99 RID: 7577
		[SerializeField]
		private bool m_Horizontal = true;

		// Token: 0x04001D9A RID: 7578
		[SerializeField]
		private bool m_Vertical = true;

		// Token: 0x04001D9B RID: 7579
		[SerializeField]
		private bool m_Inertia = true;

		// Token: 0x04001D9C RID: 7580
		[SerializeField]
		private UIDragObject.Rounding m_InertiaRounding = UIDragObject.Rounding.Hard;

		// Token: 0x04001D9D RID: 7581
		[SerializeField]
		private float m_DampeningRate = 9f;

		// Token: 0x04001D9E RID: 7582
		[SerializeField]
		private bool m_ConstrainWithinCanvas;

		// Token: 0x04001D9F RID: 7583
		[SerializeField]
		private bool m_ConstrainDrag = true;

		// Token: 0x04001DA0 RID: 7584
		[SerializeField]
		private bool m_ConstrainInertia = true;

		// Token: 0x04001DA1 RID: 7585
		private Canvas m_Canvas;

		// Token: 0x04001DA2 RID: 7586
		private RectTransform m_CanvasRectTransform;

		// Token: 0x04001DA3 RID: 7587
		private Vector2 m_PointerStartPosition = Vector2.zero;

		// Token: 0x04001DA4 RID: 7588
		private Vector2 m_TargetStartPosition = Vector2.zero;

		// Token: 0x04001DA5 RID: 7589
		private Vector2 m_Velocity;

		// Token: 0x04001DA6 RID: 7590
		private bool m_Dragging;

		// Token: 0x04001DA7 RID: 7591
		private Vector2 m_LastPosition = Vector2.zero;

		// Token: 0x04001DA8 RID: 7592
		public UIDragObject.BeginDragEvent onBeginDrag = new UIDragObject.BeginDragEvent();

		// Token: 0x04001DA9 RID: 7593
		public UIDragObject.EndDragEvent onEndDrag = new UIDragObject.EndDragEvent();

		// Token: 0x04001DAA RID: 7594
		public UIDragObject.DragEvent onDrag = new UIDragObject.DragEvent();

		// Token: 0x02000668 RID: 1640
		public enum Rounding
		{
			// Token: 0x04001DAC RID: 7596
			Soft,
			// Token: 0x04001DAD RID: 7597
			Hard
		}

		// Token: 0x02000669 RID: 1641
		[Serializable]
		public class BeginDragEvent : UnityEvent<BaseEventData>
		{
		}

		// Token: 0x0200066A RID: 1642
		[Serializable]
		public class EndDragEvent : UnityEvent<BaseEventData>
		{
		}

		// Token: 0x0200066B RID: 1643
		[Serializable]
		public class DragEvent : UnityEvent<BaseEventData>
		{
		}
	}
}
