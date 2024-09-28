using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000009 RID: 9
public class DynamicJoystick : Joystick
{
	// Token: 0x1700000B RID: 11
	// (get) Token: 0x0600002C RID: 44 RVA: 0x000027E8 File Offset: 0x000009E8
	// (set) Token: 0x0600002D RID: 45 RVA: 0x000027F0 File Offset: 0x000009F0
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

	// Token: 0x0600002E RID: 46 RVA: 0x000027FE File Offset: 0x000009FE
	protected override void Start()
	{
		this.MoveThreshold = this.moveThreshold;
		base.Start();
		this.background.gameObject.SetActive(false);
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00002823 File Offset: 0x00000A23
	public override void OnPointerDown(PointerEventData eventData)
	{
		this.background.anchoredPosition = base.ScreenPointToAnchoredPosition(eventData.position);
		this.background.gameObject.SetActive(true);
		base.OnPointerDown(eventData);
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00002854 File Offset: 0x00000A54
	public override void OnPointerUp(PointerEventData eventData)
	{
		this.background.gameObject.SetActive(false);
		base.OnPointerUp(eventData);
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00002870 File Offset: 0x00000A70
	protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
	{
		if (magnitude > this.moveThreshold)
		{
			Vector2 b = normalised * (magnitude - this.moveThreshold) * radius;
			this.background.anchoredPosition += b;
		}
		base.HandleInput(magnitude, normalised, radius, cam);
	}

	// Token: 0x0400001A RID: 26
	[SerializeField]
	private float moveThreshold = 1f;
}
