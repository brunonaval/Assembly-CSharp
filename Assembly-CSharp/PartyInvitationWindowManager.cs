using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000286 RID: 646
public class PartyInvitationWindowManager : MonoBehaviour
{
	// Token: 0x060009FF RID: 2559 RVA: 0x0002E26C File Offset: 0x0002C46C
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x06000A00 RID: 2560 RVA: 0x0002E290 File Offset: 0x0002C490
	private IEnumerator CountdownTimer()
	{
		float counter = (float)this.duration;
		WaitForSeconds waitForCountdown = new WaitForSeconds(this.countdownSpeed);
		do
		{
			counter -= this.countdownSpeed;
			float fillAmount = counter * 100f / (float)this.duration * 0.01f;
			this.timerFillImage.fillAmount = fillAmount;
			string text = string.Format(LanguageManager.Instance.GetText("party_invitation_message"), this.inviter.Name);
			this.invitationMessage.text = text;
			yield return waitForCountdown;
		}
		while (counter > 0f);
		this.Reject();
		yield break;
	}

	// Token: 0x06000A01 RID: 2561 RVA: 0x0002E29F File Offset: 0x0002C49F
	public void Accept()
	{
		this.uiSystemModule.PartyModule.CmdJoinParty(this.inviter);
		this.inviter = default(PartyMember);
		this.uiSystemModule.HidePartyInvitationWindow();
	}

	// Token: 0x06000A02 RID: 2562 RVA: 0x0002E2CE File Offset: 0x0002C4CE
	public void Reject()
	{
		this.uiSystemModule.PartyModule.CmdRejectParty(this.inviter);
		this.inviter = default(PartyMember);
		this.uiSystemModule.HidePartyInvitationWindow();
	}

	// Token: 0x06000A03 RID: 2563 RVA: 0x0002E300 File Offset: 0x0002C500
	public void SetInviter(PartyMember inviter)
	{
		if (!inviter.IsDefined)
		{
			Debug.Log("Party invitation canceled. Inviter is not defined.");
			this.Reject();
		}
		this.inviter = inviter;
		string text = string.Format(LanguageManager.Instance.GetText("party_invitation_message"), inviter.Name);
		this.invitationMessage.text = text;
		base.StartCoroutine(this.CountdownTimer());
	}

	// Token: 0x04000B7D RID: 2941
	[SerializeField]
	private int duration = 15;

	// Token: 0x04000B7E RID: 2942
	[SerializeField]
	private float countdownSpeed = 0.1f;

	// Token: 0x04000B7F RID: 2943
	[SerializeField]
	private Image timerFillImage;

	// Token: 0x04000B80 RID: 2944
	[SerializeField]
	private Text invitationMessage;

	// Token: 0x04000B81 RID: 2945
	private PartyMember inviter;

	// Token: 0x04000B82 RID: 2946
	private UISystemModule uiSystemModule;
}
