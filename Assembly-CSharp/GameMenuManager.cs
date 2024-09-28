using System;
using UnityEngine;

// Token: 0x020001F5 RID: 501
public class GameMenuManager : MonoBehaviour
{
	// Token: 0x0600064C RID: 1612 RVA: 0x00020166 File Offset: 0x0001E366
	private void OnEnable()
	{
		this.uiSystemModule.ShowBlackOverlay();
	}

	// Token: 0x0600064D RID: 1613 RVA: 0x00020173 File Offset: 0x0001E373
	private void OnDisable()
	{
		this.uiSystemModule.HideBlackOverlay();
	}

	// Token: 0x0600064E RID: 1614 RVA: 0x00020180 File Offset: 0x0001E380
	public void Resume()
	{
		this.Close();
	}

	// Token: 0x0600064F RID: 1615 RVA: 0x00020188 File Offset: 0x0001E388
	public void Disconnect()
	{
		if (!this.ValidateBeforeExitOrDisconnect())
		{
			return;
		}
		try
		{
			GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManagerModule>().StopClient();
		}
		catch (Exception ex)
		{
			Debug.LogError("Failed on disconnect: " + ex.Message + " Details: " + ex.StackTrace);
		}
	}

	// Token: 0x06000650 RID: 1616 RVA: 0x000201E8 File Offset: 0x0001E3E8
	public void ExitGame()
	{
		if (!this.ValidateBeforeExitOrDisconnect())
		{
			return;
		}
		Application.Quit();
	}

	// Token: 0x06000651 RID: 1617 RVA: 0x000201F8 File Offset: 0x0001E3F8
	private bool ValidateBeforeExitOrDisconnect()
	{
		if (this.uiSystemModule.CombatModule != null)
		{
			if (this.uiSystemModule.CombatModule.IsFighting)
			{
				this.uiSystemModule.EffectModule.ShowScreenMessage("cant_disconnect_while_fighting_message", 3, 3.5f, Array.Empty<string>());
				return false;
			}
			if (this.uiSystemModule.CombatModule.InCombat)
			{
				this.uiSystemModule.EffectModule.ShowScreenMessage("cant_disconnect_right_after_combat_message", 3, 3.5f, Array.Empty<string>());
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000652 RID: 1618 RVA: 0x00020281 File Offset: 0x0001E481
	public void OpenHelpWindow()
	{
		this.uiSystemModule.ShowHelpWindow();
		this.Close();
	}

	// Token: 0x06000653 RID: 1619 RVA: 0x00020294 File Offset: 0x0001E494
	public void OpenSettingsWindow()
	{
		this.uiSystemModule.ShowOptionsWindow();
		this.Close();
	}

	// Token: 0x06000654 RID: 1620 RVA: 0x000202A7 File Offset: 0x0001E4A7
	public void OpenDiscord()
	{
		Application.OpenURL(GlobalSettings.DiscordServer);
	}

	// Token: 0x06000655 RID: 1621 RVA: 0x000202B3 File Offset: 0x0001E4B3
	public void OpenWiki()
	{
		Application.OpenURL(GlobalSettings.WikiUrl);
	}

	// Token: 0x06000656 RID: 1622 RVA: 0x000202BF File Offset: 0x0001E4BF
	public void Close()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x0400088D RID: 2189
	[SerializeField]
	private UISystemModule uiSystemModule;
}
