using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200022E RID: 558
public class CharacterSlotManager : MonoBehaviour
{
	// Token: 0x060007C7 RID: 1991 RVA: 0x000257EC File Offset: 0x000239EC
	public void SetPlayer(Player player)
	{
		this.player = player;
		this.nameText.text = player.Name.ToLower();
		this.levelText.text = player.BaseLevel.ToString();
		this.vocationText.text = LanguageManager.Instance.GetText(player.VocationName.ToLower());
		this.serverText.text = string.Format("<color=orange>{0}</color> - <color={1}>{2}</color>", player.ServerName, this.GetServerTypeColor(player.ServerType), player.ServerType);
		this.populationText.text = string.Format(LanguageManager.Instance.GetText("server_population_label"), string.Concat(new string[]
		{
			"<color=",
			this.GetServerPopulationColor(player),
			">",
			this.GetServerPopulationLabel(player),
			"</color>"
		}));
		this.SetVocationAvatar(player.VocationName);
	}

	// Token: 0x060007C8 RID: 1992 RVA: 0x000258E4 File Offset: 0x00023AE4
	private string GetServerPopulationLabel(Player player)
	{
		float num = (float)player.ServerCurrentPlayers / (float)player.ServerMaxPlayers;
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

	// Token: 0x060007C9 RID: 1993 RVA: 0x0002594C File Offset: 0x00023B4C
	private string GetServerPopulationColor(Player player)
	{
		float num = (float)player.ServerCurrentPlayers / (float)player.ServerMaxPlayers;
		if ((double)num < 0.5)
		{
			return "green";
		}
		if ((double)num < 0.9)
		{
			return "orange";
		}
		return "red";
	}

	// Token: 0x060007CA RID: 1994 RVA: 0x00025994 File Offset: 0x00023B94
	private string GetServerTypeColor(ServerType serverType)
	{
		if (serverType == ServerType.PvP)
		{
			return "red";
		}
		if (serverType != ServerType.PvE)
		{
			return "orange";
		}
		return "green";
	}

	// Token: 0x060007CB RID: 1995 RVA: 0x000259B0 File Offset: 0x00023BB0
	public void SetVocationAvatar(string vocationName)
	{
		this.avatar.sprite = ResourcesManager.Instance.GetVocationIconSprite(vocationName.ToLower());
	}

	// Token: 0x060007CC RID: 1996 RVA: 0x000259CD File Offset: 0x00023BCD
	public void OnDeleteCharacterClicked()
	{
		CharacterListManager componentInParent = base.GetComponentInParent<CharacterListManager>();
		if (componentInParent == null)
		{
			return;
		}
		componentInParent.ShowDeleteWindow(this.player);
	}

	// Token: 0x060007CD RID: 1997 RVA: 0x000259E5 File Offset: 0x00023BE5
	public void OnCharacterSelected()
	{
		if (this.selectionToggle.isOn)
		{
			CharacterListManager componentInParent = base.GetComponentInParent<CharacterListManager>();
			if (componentInParent == null)
			{
				return;
			}
			componentInParent.SetSelectedPlayer(this.player);
		}
	}

	// Token: 0x040009A2 RID: 2466
	[SerializeField]
	private Image avatar;

	// Token: 0x040009A3 RID: 2467
	[SerializeField]
	private Text nameText;

	// Token: 0x040009A4 RID: 2468
	[SerializeField]
	private Text levelText;

	// Token: 0x040009A5 RID: 2469
	[SerializeField]
	private Text vocationText;

	// Token: 0x040009A6 RID: 2470
	[SerializeField]
	private Toggle selectionToggle;

	// Token: 0x040009A7 RID: 2471
	[SerializeField]
	private Text serverText;

	// Token: 0x040009A8 RID: 2472
	[SerializeField]
	private Text populationText;

	// Token: 0x040009A9 RID: 2473
	private Player player;

	// Token: 0x040009AA RID: 2474
	private CharacterListManager characterListManager;
}
