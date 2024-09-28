using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200027C RID: 636
public class InformationWindowManager : MonoBehaviour
{
	// Token: 0x060009B7 RID: 2487 RVA: 0x0002D258 File Offset: 0x0002B458
	public void OnOkButtonClicked()
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

	// Token: 0x060009B8 RID: 2488 RVA: 0x0002D288 File Offset: 0x0002B488
	public void Initialize(string title, string message, Action callback = null)
	{
		if (!string.IsNullOrEmpty(title))
		{
			this.titleText.text = LanguageManager.Instance.GetText(title).ToLower();
		}
		if (!string.IsNullOrEmpty(message))
		{
			this.messageText.text = LanguageManager.Instance.GetText(message);
		}
		this.confirmCallback = callback;
	}

	// Token: 0x04000B47 RID: 2887
	[SerializeField]
	private Text titleText;

	// Token: 0x04000B48 RID: 2888
	[SerializeField]
	private Text messageText;

	// Token: 0x04000B49 RID: 2889
	private Action confirmCallback;
}
