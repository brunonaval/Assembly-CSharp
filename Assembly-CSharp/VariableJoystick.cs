using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200000C RID: 12
public class VariableJoystick : Joystick
{
	// Token: 0x1700000C RID: 12
	// (get) Token: 0x06000038 RID: 56 RVA: 0x000028F1 File Offset: 0x00000AF1
	// (set) Token: 0x06000039 RID: 57 RVA: 0x000028F9 File Offset: 0x00000AF9
	public float MoveThreshold
	{
		get
		{
			return this.moveThreshold;
		}
		set
		{
			this.moveThreshold = Mathf.Abs(value);
		}
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00002907 File Offset: 0x00000B07
	public void SetMode(JoystickType joystickType)
	{
		this.joystickType = joystickType;
		if (joystickType == JoystickType.Fixed)
		{
			this.background.anchoredPosition = this.fixedPosition;
			this.background.gameObject.SetActive(true);
			return;
		}
		this.background.gameObject.SetActive(false);
	}

	// Token: 0x0600003B RID: 59 RVA: 0x00002947 File Offset: 0x00000B47
	protected override void Start()
	{
		base.Start();
		this.fixedPosition = this.background.anchoredPosition;
		this.SetMode(this.joystickType);
	}

	// Token: 0x0600003C RID: 60 RVA: 0x0000296C File Offset: 0x00000B6C
	public override void OnPointerDown(PointerEventData eventData)
	{
		if (this.joystickType != JoystickType.Fixed)
		{
			this.background.anchoredPosition = base.ScreenPointToAnchoredPosition(eventData.position);
			this.background.gameObject.SetActive(true);
		}
		base.OnPointerDown(eventData);
	}

	// Token: 0x0600003D RID: 61 RVA: 0x000029A5 File Offset: 0x00000BA5
	public override void OnPointerUp(PointerEventData eventData)
	{
		if (this.joystickType != JoystickType.Fixed)
		{
			this.background.gameObject.SetActive(false);
		}
		base.OnPointerUp(eventData);
	}

	// Token: 0x0600003E RID: 62 RVA: 0x000029C8 File Offset: 0x00000BC8
	protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
	{
		if (this.joystickType == JoystickType.Dynamic && magnitude > this.moveThreshold)
		{
			Vector2 b = normalised * (magnitude - this.moveThreshold) * radius;
			this.background.anchoredPosition += b;
		}
		base.HandleInput(magnitude, normalised, radius, cam);
	}

	// Token: 0x0400001B RID: 27
	[SerializeField]
	private float moveThreshold = 1f;

	// Token: 0x0400001C RID: 28
	[SerializeField]
	private JoystickType joystickType;

	// Token: 0x0400001D RID: 29
	private Vector2 fixedPosition = Vector2.zero;
}
