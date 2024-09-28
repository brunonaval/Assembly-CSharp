using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x020001E3 RID: 483
public class ChatInputManager : MonoBehaviour, IDeselectHandler, IEventSystemHandler
{
	// Token: 0x060005D2 RID: 1490 RVA: 0x0001E5BC File Offset: 0x0001C7BC
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x060005D3 RID: 1491 RVA: 0x0001E5E0 File Offset: 0x0001C7E0
	public void OnDeselect(BaseEventData eventData)
	{
		if (eventData.selectedObject.name == "Input Field (Chat)")
		{
			this.uiSystemModule.PlayerModule.ChatFocused = false;
		}
	}

	// Token: 0x04000824 RID: 2084
	private UISystemModule uiSystemModule;
}
