using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000334 RID: 820
public class GuildInvitationWindowModule : MonoBehaviour
{
	// Token: 0x06000FF1 RID: 4081 RVA: 0x0004A986 File Offset: 0x00048B86
	private void Awake()
	{
		GameObject.FindGameObjectWithTag("UISystem").TryGetComponent<UISystemModule>(out this.uiSystemModule);
	}

	// Token: 0x06000FF2 RID: 4082 RVA: 0x0004A99E File Offset: 0x00048B9E
	private IEnumerator CountdownTimer()
	{
		float counter = (float)this.duration;
		WaitForSeconds waitForCountdown = new WaitForSeconds(this.countdownSpeed);
		GuildModule inviterGuildModule;
		this.inviter.TryGetComponent<GuildModule>(out inviterGuildModule);
		CreatureModule inviterCreatureModule;
		this.inviter.TryGetComponent<CreatureModule>(out inviterCreatureModule);
		do
		{
			counter -= this.countdownSpeed;
			float fillAmount = counter * 100f / (float)this.duration * 0.01f;
			this.timerFillImage.fillAmount = fillAmount;
			string text = string.Format(LanguageManager.Instance.GetText("guild_invitation_message"), inviterCreatureModule.CreatureName, inviterGuildModule.ActiveGuildName);
			this.invitationMessageText.text = text;
			yield return waitForCountdown;
		}
		while (counter > 0f);
		this.Reject();
		yield break;
	}

	// Token: 0x06000FF3 RID: 4083 RVA: 0x0004A9AD File Offset: 0x00048BAD
	public void Accept()
	{
		this.uiSystemModule.GuildModule.CmdJoinGuild(this.inviter);
		this.inviter = null;
		this.uiSystemModule.HideGuildInvitationWindow();
	}

	// Token: 0x06000FF4 RID: 4084 RVA: 0x0004A9D7 File Offset: 0x00048BD7
	public void Reject()
	{
		this.uiSystemModule.GuildModule.CmdRejectGuild(this.inviter);
		this.inviter = null;
		this.uiSystemModule.HideGuildInvitationWindow();
	}

	// Token: 0x06000FF5 RID: 4085 RVA: 0x0004AA04 File Offset: 0x00048C04
	public void SetInviter(GameObject inviter)
	{
		if (inviter == null)
		{
			Debug.Log("Guild invitation was canceled. Inviter is not defined.");
			this.Reject();
		}
		this.inviter = inviter;
		CreatureModule creatureModule;
		inviter.TryGetComponent<CreatureModule>(out creatureModule);
		GuildModule guildModule;
		inviter.TryGetComponent<GuildModule>(out guildModule);
		string text = string.Format(LanguageManager.Instance.GetText("guild_invitation_message"), creatureModule.CreatureName, guildModule.ActiveGuildName);
		this.invitationMessageText.text = text;
		base.StartCoroutine(this.CountdownTimer());
	}

	// Token: 0x04000F67 RID: 3943
	[SerializeField]
	private int duration = 15;

	// Token: 0x04000F68 RID: 3944
	[SerializeField]
	private float countdownSpeed = 0.1f;

	// Token: 0x04000F69 RID: 3945
	[SerializeField]
	private Image timerFillImage;

	// Token: 0x04000F6A RID: 3946
	[SerializeField]
	private Text invitationMessageText;

	// Token: 0x04000F6B RID: 3947
	private GameObject inviter;

	// Token: 0x04000F6C RID: 3948
	private UISystemModule uiSystemModule;
}
