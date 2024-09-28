using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001E5 RID: 485
public class ChoiceButtonManager : MonoBehaviour
{
	// Token: 0x060005DF RID: 1503 RVA: 0x0001E7F0 File Offset: 0x0001C9F0
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x060005E0 RID: 1504 RVA: 0x0001E814 File Offset: 0x0001CA14
	public void SetChoice(NpcChoice npcChoice)
	{
		this.npcChoice = npcChoice;
		string text = LanguageManager.Instance.GetText(npcChoice.Display).ToLower();
		if (npcChoice.Display.StartsWith("teleport_") && this.uiSystemModule.PlayerModule.PremiumDays > 0)
		{
			text += LanguageManager.Instance.GetText("free").ToLower();
			this.displayText.text = text;
			return;
		}
		if (!string.IsNullOrEmpty(npcChoice.Param1))
		{
			text += LanguageManager.Instance.GetText(npcChoice.Param1).ToLower();
		}
		if (!string.IsNullOrEmpty(npcChoice.Param2))
		{
			text += LanguageManager.Instance.GetText(npcChoice.Param2).ToLower();
		}
		if (!string.IsNullOrEmpty(npcChoice.Param3))
		{
			text += LanguageManager.Instance.GetText(npcChoice.Param3).ToLower();
		}
		this.displayText.text = text;
	}

	// Token: 0x060005E1 RID: 1505 RVA: 0x0001E912 File Offset: 0x0001CB12
	public void SendChoice()
	{
		this.uiSystemModule.PlayerModule.CmdSendChoice(this.npcChoice.ActionId);
	}

	// Token: 0x0400082B RID: 2091
	private NpcChoice npcChoice;

	// Token: 0x0400082C RID: 2092
	[SerializeField]
	private Text displayText;

	// Token: 0x0400082D RID: 2093
	private UISystemModule uiSystemModule;
}
