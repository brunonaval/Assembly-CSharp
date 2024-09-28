using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000336 RID: 822
public class GuildMemberModule : MonoBehaviour
{
	// Token: 0x06000FFD RID: 4093 RVA: 0x0004ABC4 File Offset: 0x00048DC4
	private void Awake()
	{
		GameObject.FindGameObjectWithTag("UISystem").TryGetComponent<UISystemModule>(out this.uiSystemModule);
	}

	// Token: 0x06000FFE RID: 4094 RVA: 0x0004ABDC File Offset: 0x00048DDC
	public void OnExpelButtonClick()
	{
		this.uiSystemModule.ShowDefaultConfirmationWindow(delegate
		{
			this.expelButton.interactable = false;
			this.uiSystemModule.GuildModule.CmdExpelMember(this.guildMember.Name, this.guildMember.MemberPlayerId);
			UnityEngine.Object.Destroy(this.guildMemberUIObject);
		});
	}

	// Token: 0x06000FFF RID: 4095 RVA: 0x0004ABF5 File Offset: 0x00048DF5
	public void OnPromoteButtonClick()
	{
		this.uiSystemModule.ShowDefaultConfirmationWindow(delegate
		{
			this.promoteButton.interactable = false;
			this.uiSystemModule.GuildModule.CmdPromoteMember(this.guildMember.Name, this.guildMember.MemberPlayerId);
			this.memberGuildRankText.text = GlobalUtils.GuildMemberRankToString(GuildMemberRank.Captain);
		});
	}

	// Token: 0x06001000 RID: 4096 RVA: 0x0004AC10 File Offset: 0x00048E10
	public void SetMember(GuildMember guildMember)
	{
		this.guildMember = guildMember;
		Sprite vocationIconSprite = ResourcesManager.Instance.GetVocationIconSprite(guildMember.Vocation.ToString().ToLower());
		this.vocationImage.sprite = vocationIconSprite;
		this.memberNameText.text = guildMember.Name;
		this.memberBaseLevelText.text = guildMember.BaseLevel.ToString();
		this.memberGuildRankText.text = GlobalUtils.GuildMemberRankToString(guildMember.MemberRank);
		if (guildMember.DaysSinceLastLogin == 1)
		{
			this.lastLoginText.text = string.Format(LanguageManager.Instance.GetText("guild_member_since_text"), guildMember.DaysSinceLastLogin);
		}
		else
		{
			this.lastLoginText.text = string.Format(LanguageManager.Instance.GetText("guild_member_since_plural_text"), guildMember.DaysSinceLastLogin);
		}
		if (guildMember.MemberSinceDays == 1)
		{
			this.memberSinceDaysText.text = string.Format(LanguageManager.Instance.GetText("guild_member_since_text"), guildMember.MemberSinceDays);
		}
		else
		{
			this.memberSinceDaysText.text = string.Format(LanguageManager.Instance.GetText("guild_member_since_plural_text"), guildMember.MemberSinceDays);
		}
		bool interactable = this.uiSystemModule.GuildModule.MemberRank != GuildMemberRank.Member;
		if (guildMember.MemberRank == GuildMemberRank.Leader)
		{
			this.expelButton.interactable = false;
			this.promoteButton.interactable = false;
		}
		if (guildMember.MemberRank == GuildMemberRank.Captain)
		{
			this.expelButton.interactable = interactable;
			this.promoteButton.interactable = false;
		}
		if (guildMember.MemberRank == GuildMemberRank.Member)
		{
			this.expelButton.interactable = interactable;
			this.promoteButton.interactable = interactable;
		}
	}

	// Token: 0x04000F74 RID: 3956
	[SerializeField]
	private Text memberNameText;

	// Token: 0x04000F75 RID: 3957
	[SerializeField]
	private Text memberBaseLevelText;

	// Token: 0x04000F76 RID: 3958
	[SerializeField]
	private Text memberGuildRankText;

	// Token: 0x04000F77 RID: 3959
	[SerializeField]
	private Text memberSinceDaysText;

	// Token: 0x04000F78 RID: 3960
	[SerializeField]
	private Text lastLoginText;

	// Token: 0x04000F79 RID: 3961
	[SerializeField]
	private Image vocationImage;

	// Token: 0x04000F7A RID: 3962
	[SerializeField]
	private Button expelButton;

	// Token: 0x04000F7B RID: 3963
	[SerializeField]
	private Button promoteButton;

	// Token: 0x04000F7C RID: 3964
	[SerializeField]
	private GameObject guildMemberUIObject;

	// Token: 0x04000F7D RID: 3965
	private GuildMember guildMember;

	// Token: 0x04000F7E RID: 3966
	private UISystemModule uiSystemModule;
}
