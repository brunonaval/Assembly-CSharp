using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000007 RID: 7
public class Joystick : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IDragHandler, IPointerUpHandler
{
	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000016 RID: 22 RVA: 0x000023CF File Offset: 0x000005CF
	public float Horizontal
	{
		get
		{
			if (!this.snapX)
			{
				return this.input.x;
			}
			return this.SnapFloat(this.input.x, AxisOptions.Horizontal);
		}
	}

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x06000017 RID: 23 RVA: 0x000023F7 File Offset: 0x000005F7
	public float Vertical
	{
		get
		{
			if (!this.snapY)
			{
				return this.input.y;
			}
			return this.SnapFloat(this.input.y, AxisOptions.Vertical);
		}
	}

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000018 RID: 24 RVA: 0x0000241F File Offset: 0x0000061F
	public Vector2 Direction
	{
		get
		{
			return new Vector2(this.Horizontal, this.Vertical);
		}
	}

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x06000019 RID: 25 RVA: 0x00002432 File Offset: 0x00000632
	// (set) Token: 0x0600001A RID: 26 RVA: 0x0000243A File Offset: 0x0000063A
	public float HandleRange
	{
		get
		{
			return this.handleRange;
		}
		set
		{
			this.handleRange = Mathf.Abs(value);
		}
	}

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x0600001B RID: 27 RVA: 0x00002448 File Offset: 0x00000648
	// (set) Token: 0x0600001C RID: 28 RVA: 0x00002450 File Offset: 0x00000650
	public float DeadZone
	{
		get
		{
			return this.deadZone;
		}
		set
		{
			this.deadZone = Mathf.Abs(value);
		}
	}

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x0600001D RID: 29 RVA: 0x0000245E File Offset: 0x0000065E
	// (set) Token: 0x0600001E RID: 30 RVA: 0x00002466 File Offset: 0x00000666
	public AxisOptions AxisOptions
	{
		get
		{
			return this.AxisOptions;
		}
		set
		{
			this.axisOptions = value;
		}
	}

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x0600001F RID: 31 RVA: 0x0000246F File Offset: 0x0000066F
	// (set) Token: 0x06000020 RID: 32 RVA: 0x00002477 File Offset: 0x00000677
	public bool SnapX
	{
		get
		{
			return this.snapX;
		}
		set
		{
			this.snapX = value;
		}
	}

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x06000021 RID: 33 RVA: 0x00002480 File Offset: 0x00000680
	// (set) Token: 0x06000022 RID: 34 RVA: 0x00002488 File Offset: 0x00000688
	public bool SnapY
	{
		get
		{
			return this.snapY;
		}
		set
		{
			this.snapY = value;
		}
	}

	// Token: 0x06000023 RID: 35 RVA: 0x00002494 File Offset: 0x00000694
	protected virtual void Start()
	{
		this.HandleRange = this.handleRange;
		this.DeadZone = this.deadZone;
		this.baseRect = base.GetComponent<RectTransform>();
		this.canvas = base.GetComponentInParent<Canvas>();
		if (this.canvas == null)
		{
			Debug.LogError("The Joystick is not placed inside a canvas");
		}
		Vector2 vector = new Vector2(0.5f, 0.5f);
		this.background.pivot = vector;
		this.handle.anchorMin = vector;
		this.handle.anchorMax = vector;
		this.handle.pivot = vector;
		this.handle.anchoredPosition = Vector2.zero;
	}

	// Token: 0x06000024 RID: 36 RVA: 0x0000253A File Offset: 0x0000073A
	public virtual void OnPointerDown(PointerEventData eventData)
	{
		this.OnDrag(eventData);
	}

	// Token: 0x06000025 RID: 37 RVA: 0x00002544 File Offset: 0x00000744
	public void OnDrag(PointerEventData eventData)
	{
		this.cam = null;
		if (this.canvas.renderMode == RenderMode.ScreenSpaceCamera)
		{
			this.cam = this.canvas.worldCamera;
		}
		Vector2 b = RectTransformUtility.WorldToScreenPoint(this.cam, this.background.position);
		Vector2 vector = this.background.sizeDelta / 2f;
		this.input = (eventData.position - b) / (vector * this.canvas.scaleFactor);
		this.FormatInput();
		this.HandleInput(this.input.magnitude, this.input.normalized, vector, this.cam);
		this.handle.anchoredPosition = this.input * vector * this.handleRange;
	}

	// Token: 0x06000026 RID: 38 RVA: 0x00002617 File Offset: 0x00000817
	protected virtual void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
	{
		if (magnitude > this.deadZone)
		{
			if (magnitude > 1f)
			{
				this.input = normalised;
				return;
			}
		}
		else
		{
			this.input = Vector2.zero;
		}
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00002640 File Offset: 0x00000840
	private void FormatInput()
	{
		if (this.axisOptions == AxisOptions.Horizontal)
		{
			this.input = new Vector2(this.input.x, 0f);
			return;
		}
		if (this.axisOptions == AxisOptions.Vertical)
		{
			this.input = new Vector2(0f, this.input.y);
		}
	}

	// Token: 0x06000028 RID: 40 RVA: 0x00002698 File Offset: 0x00000898
	private float SnapFloat(float value, AxisOptions snapAxis)
	{
		if (value == 0f)
		{
			return value;
		}
		if (this.axisOptions == AxisOptions.Both)
		{
			float num = Vector2.Angle(this.input, Vector2.up);
			if (snapAxis == AxisOptions.Horizontal)
			{
				if (num < 22.5f || num > 157.5f)
				{
					return 0f;
				}
				return (float)((value > 0f) ? 1 : -1);
			}
			else
			{
				if (snapAxis != AxisOptions.Vertical)
				{
					return value;
				}
				if (num > 67.5f && num < 112.5f)
				{
					return 0f;
				}
				return (float)((value > 0f) ? 1 : -1);
			}
		}
		else
		{
			if (value > 0f)
			{
				return 1f;
			}
			if (value < 0f)
			{
				return -1f;
			}
			return 0f;
		}
	}

	// Token: 0x06000029 RID: 41 RVA: 0x0000273B File Offset: 0x0000093B
	public virtual void OnPointerUp(PointerEventData eventData)
	{
		this.input = Vector2.zero;
		this.handle.anchoredPosition = Vector2.zero;
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00002758 File Offset: 0x00000958
	protected Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
	{
		Vector2 zero = Vector2.zero;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this.baseRect, screenPosition, this.cam, out zero))
		{
			Vector2 b = this.baseRect.pivot * this.baseRect.sizeDelta;
			return zero - this.background.anchorMax * this.baseRect.sizeDelta + b;
		}
		return Vector2.zero;
	}

	// Token: 0x0400000B RID: 11
	[SerializeField]
	private float handleRange = 1f;

	// Token: 0x0400000C RID: 12
	[SerializeField]
	private float deadZone;

	// Token: 0x0400000D RID: 13
	[SerializeField]
	private AxisOptions axisOptions;

	// Token: 0x0400000E RID: 14
	[SerializeField]
	private bool snapX;

	// Token: 0x0400000F RID: 15
	[SerializeField]
	private bool snapY;

	// Token: 0x04000010 RID: 16
	[SerializeField]
	protected RectTransform background;

	// Token: 0x04000011 RID: 17
	[SerializeField]
	private RectTransform handle;

	// Token: 0x04000012 RID: 18
	private RectTransform baseRect;

	// Token: 0x04000013 RID: 19
	private Canvas canvas;

	// Token: 0x04000014 RID: 20
	private Camera cam;

	// Token: 0x04000015 RID: 21
	private Vector2 input = Vector2.zero;
}
