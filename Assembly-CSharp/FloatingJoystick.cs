using System;
using UnityEngine.EventSystems;

// Token: 0x0200000B RID: 11
public class FloatingJoystick : Joystick
{
	// Token: 0x06000034 RID: 52 RVA: 0x000028D8 File Offset: 0x00000AD8
	protected override void Start()
	{
		base.Start();
		this.background.gameObject.SetActive(false);
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00002823 File Offset: 0x00000A23
	public override void OnPointerDown(PointerEventData eventData)
	{
		this.background.anchoredPosition = base.ScreenPointToAnchoredPosition(eventData.position);
		this.background.gameObject.SetActive(true);
		base.OnPointerDown(eventData);
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00002854 File Offset: 0x00000A54
	public override void OnPointerUp(PointerEventData eventData)
	{
		this.background.gameObject.SetActive(false);
		base.OnPointerUp(eventData);
	}
}
