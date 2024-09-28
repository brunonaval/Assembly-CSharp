using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000210 RID: 528
public class MobileAvatarManager : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	// Token: 0x06000721 RID: 1825 RVA: 0x00023102 File Offset: 0x00021302
	public void OnPointerDown(PointerEventData eventData)
	{
		this.pressed = true;
	}

	// Token: 0x06000722 RID: 1826 RVA: 0x0002310B File Offset: 0x0002130B
	public void OnPointerUp(PointerEventData eventData)
	{
		if (this.pressed)
		{
			this.pressed = false;
			this.uiSystemModule.TogglePlayerWindow();
		}
	}

	// Token: 0x04000921 RID: 2337
	[SerializeField]
	private UISystemModule uiSystemModule;

	// Token: 0x04000922 RID: 2338
	private bool pressed;
}
