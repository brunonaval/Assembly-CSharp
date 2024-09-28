using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200022F RID: 559
public class ServerSlotManager : MonoBehaviour
{
	// Token: 0x060007CF RID: 1999 RVA: 0x00025A0A File Offset: 0x00023C0A
	public void OnServerButtonClick()
	{
		this.characterCreationManager.SetSelectedServer(this.server);
		base.GetComponentInParent<DragWindowModule>().CloseWindow();
	}

	// Token: 0x060007D0 RID: 2000 RVA: 0x00025A28 File Offset: 0x00023C28
	public void SetServer(CharacterCreationManager CharacterListManager, ApiServer server)
	{
		this.server = server;
		this.characterCreationManager = CharacterListManager;
		if (this.server.CurrentPlayers >= this.server.MaxPlayers)
		{
			this.serverNameLabel.text = string.Format("{0} - ({1})\r\n<size=28><color=orange>EXP/AXP: {2}% | Craft EXP: {3}%</color></size>", new object[]
			{
				server.Name,
				LanguageManager.Instance.GetText("full"),
				server.ExperienceModifier * 100f,
				server.CraftExperienceModifier * 100f
			}).ToLower();
			this.serverNameLabel.color = GlobalSettings.Colors[3];
			this.serverButton.interactable = false;
			return;
		}
		this.serverNameLabel.text = string.Format("{0} - ({1})\r\n<size=28><color=orange>EXP/AXP: {2}% | Craft EXP: {3}%</color></size>", new object[]
		{
			server.Name,
			this.GetServerPopulationLabel(),
			server.ExperienceModifier * 100f,
			server.CraftExperienceModifier * 100f
		}).ToLower();
		this.serverNameLabel.color = this.GetServerColor();
		this.serverButton.interactable = true;
	}

	// Token: 0x060007D1 RID: 2001 RVA: 0x00025B5C File Offset: 0x00023D5C
	private string GetServerPopulationLabel()
	{
		float num = (float)this.server.CurrentPlayers / (float)this.server.MaxPlayers;
		if ((double)num < 0.5)
		{
			return LanguageManager.Instance.GetText("empty");
		}
		if ((double)num < 0.9)
		{
			return LanguageManager.Instance.GetText("normal");
		}
		return LanguageManager.Instance.GetText("crowded");
	}

	// Token: 0x060007D2 RID: 2002 RVA: 0x00025BCC File Offset: 0x00023DCC
	private Color GetServerColor()
	{
		float num = (float)this.server.CurrentPlayers / (float)this.server.MaxPlayers;
		if ((double)num < 0.5)
		{
			return GlobalSettings.Colors[1];
		}
		if ((double)num < 0.9)
		{
			return GlobalSettings.Colors[2];
		}
		return GlobalSettings.Colors[3];
	}

	// Token: 0x040009AB RID: 2475
	[SerializeField]
	private Text serverNameLabel;

	// Token: 0x040009AC RID: 2476
	[SerializeField]
	private Button serverButton;

	// Token: 0x040009AD RID: 2477
	private ApiServer server;

	// Token: 0x040009AE RID: 2478
	private CharacterCreationManager characterCreationManager;
}
