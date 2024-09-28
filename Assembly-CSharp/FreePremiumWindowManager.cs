using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000278 RID: 632
public class FreePremiumWindowManager : MonoBehaviour
{
	// Token: 0x060009A8 RID: 2472 RVA: 0x0002D03F File Offset: 0x0002B23F
	private void Start()
	{
		this.freePremiumDescriptionText.text = string.Format(LanguageManager.Instance.GetText("free_premium_description"), 3);
	}

	// Token: 0x060009A9 RID: 2473 RVA: 0x0002D066 File Offset: 0x0002B266
	public void TakePremiumDays()
	{
		this.uiSystemModule.PlayerModule.CmdTakeFreePremium();
		this.dragWindowModule.CloseWindow();
	}

	// Token: 0x04000B3D RID: 2877
	[SerializeField]
	private UISystemModule uiSystemModule;

	// Token: 0x04000B3E RID: 2878
	[SerializeField]
	private DragWindowModule dragWindowModule;

	// Token: 0x04000B3F RID: 2879
	[SerializeField]
	private Text freePremiumDescriptionText;
}
