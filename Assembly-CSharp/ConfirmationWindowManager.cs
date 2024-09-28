using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001E7 RID: 487
public class ConfirmationWindowManager : MonoBehaviour
{
	// Token: 0x060005EA RID: 1514 RVA: 0x0001EB10 File Offset: 0x0001CD10
	public void OnConfirmButtonClicked()
	{
		Action action = this.confirmCallback;
		if (action != null)
		{
			action();
		}
		DragWindowModule dragWindowModule;
		base.TryGetComponent<DragWindowModule>(out dragWindowModule);
		dragWindowModule.CloseWindow();
	}

	// Token: 0x060005EB RID: 1515 RVA: 0x0001EB3D File Offset: 0x0001CD3D
	public void Initialize(Sprite icon, string text, Action callback)
	{
		this.icon.sprite = icon;
		this.message.text = LanguageManager.Instance.GetText(text);
		this.confirmCallback = callback;
	}

	// Token: 0x04000835 RID: 2101
	[SerializeField]
	private Image icon;

	// Token: 0x04000836 RID: 2102
	[SerializeField]
	private Text message;

	// Token: 0x04000837 RID: 2103
	private Action confirmCallback;
}
