using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000214 RID: 532
public class MobileExpBarManager : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	// Token: 0x170000C4 RID: 196
	// (get) Token: 0x0600072C RID: 1836 RVA: 0x00023214 File Offset: 0x00021414
	// (set) Token: 0x0600072D RID: 1837 RVA: 0x0002321B File Offset: 0x0002141B
	public static bool ShowExpPerMinute { get; private set; } = true;

	// Token: 0x0600072E RID: 1838 RVA: 0x00023223 File Offset: 0x00021423
	public void OnPointerDown(PointerEventData eventData)
	{
		this.pressed = true;
	}

	// Token: 0x0600072F RID: 1839 RVA: 0x0002322C File Offset: 0x0002142C
	public void OnPointerUp(PointerEventData eventData)
	{
		if (this.pressed)
		{
			this.pressed = false;
			MobileExpBarManager.ShowExpPerMinute = !MobileExpBarManager.ShowExpPerMinute;
		}
	}

	// Token: 0x04000928 RID: 2344
	private bool pressed;
}
