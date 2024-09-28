using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200021D RID: 541
public class MobileMinimapManager : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	// Token: 0x06000753 RID: 1875 RVA: 0x000239E6 File Offset: 0x00021BE6
	public void OnPointerDown(PointerEventData eventData)
	{
		this.pressed = true;
	}

	// Token: 0x06000754 RID: 1876 RVA: 0x000239EF File Offset: 0x00021BEF
	public void OnPointerUp(PointerEventData eventData)
	{
		if (this.pressed)
		{
			this.pressed = false;
			this.uiSystemModule.PlayerModule.ShowMap();
		}
	}

	// Token: 0x04000948 RID: 2376
	[SerializeField]
	private UISystemModule uiSystemModule;

	// Token: 0x04000949 RID: 2377
	private bool pressed;
}
