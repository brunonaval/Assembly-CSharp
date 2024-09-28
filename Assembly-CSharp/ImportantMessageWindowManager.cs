using System;
using UnityEngine;

// Token: 0x0200027B RID: 635
public class ImportantMessageWindowManager : MonoBehaviour
{
	// Token: 0x060009B5 RID: 2485 RVA: 0x000202A7 File Offset: 0x0001E4A7
	public void OnReadMoreButtonClicked()
	{
		Application.OpenURL(GlobalSettings.DiscordServer);
	}
}
