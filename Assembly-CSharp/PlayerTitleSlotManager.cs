using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000231 RID: 561
public class PlayerTitleSlotManager : MonoBehaviour
{
	// Token: 0x060007DF RID: 2015 RVA: 0x00025FFC File Offset: 0x000241FC
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x060007E0 RID: 2016 RVA: 0x00026020 File Offset: 0x00024220
	public void SetTitle(Title title)
	{
		this.title = title;
		if (title.IsDefined)
		{
			this.titleNameText.text = LanguageManager.Instance.GetText(title.Name);
		}
	}

	// Token: 0x060007E1 RID: 2017 RVA: 0x0002604D File Offset: 0x0002424D
	public void OnToggleValueChanged()
	{
		if (this.titleToggle.isOn)
		{
			this.uiSystemModule.TitleModule.CmdSetActiveTitle(this.title);
			return;
		}
		this.uiSystemModule.TitleModule.CmdClearTitle();
	}

	// Token: 0x040009BC RID: 2492
	private Title title;

	// Token: 0x040009BD RID: 2493
	[SerializeField]
	private Text titleNameText;

	// Token: 0x040009BE RID: 2494
	[SerializeField]
	private Toggle titleToggle;

	// Token: 0x040009BF RID: 2495
	private UISystemModule uiSystemModule;
}
