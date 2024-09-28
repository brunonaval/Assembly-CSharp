using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000342 RID: 834
public class GuildWindowModule : MonoBehaviour
{
	// Token: 0x0600104B RID: 4171 RVA: 0x0004CB70 File Offset: 0x0004AD70
	private void OnEnable()
	{
		this.searchInput.text = string.Empty;
		if (this.uiSystemModule.GuildModule.ActiveGuildId == 0)
		{
			this.noGuildOverlay.SetActive(true);
			this.DestroyMemberObjects();
			this.leaveGuildButton.interactable = false;
			this.deleteGuildButton.interactable = false;
			return;
		}
		base.StartCoroutine(this.LoadGuildInfo());
		this.leaveGuildButton.interactable = (this.uiSystemModule.GuildModule.MemberRank > GuildMemberRank.Leader);
		this.deleteGuildButton.interactable = (this.uiSystemModule.GuildModule.MemberRank == GuildMemberRank.Leader);
	}

	// Token: 0x0600104C RID: 4172 RVA: 0x0001D0D2 File Offset: 0x0001B2D2
	private void OnDisable()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x0600104D RID: 4173 RVA: 0x0004CC13 File Offset: 0x0004AE13
	public void OnSearchButtonClicked()
	{
		base.StartCoroutine(this.LoadGuildInfo());
	}

	// Token: 0x0600104E RID: 4174 RVA: 0x0004CC22 File Offset: 0x0004AE22
	public void OnLeaveGuildButtonClicked()
	{
		this.uiSystemModule.ShowDefaultConfirmationWindow(delegate
		{
			this.uiSystemModule.GuildModule.CmdLeaveGuild();
			this.uiSystemModule.HideGuildWindow();
		});
	}

	// Token: 0x0600104F RID: 4175 RVA: 0x0004CC3B File Offset: 0x0004AE3B
	public void OnDeleteGuildButtonClicked()
	{
		this.uiSystemModule.ShowDefaultConfirmationWindow(delegate
		{
			this.uiSystemModule.GuildModule.CmdDeleteGuild();
			this.uiSystemModule.HideGuildWindow();
		});
	}

	// Token: 0x06001050 RID: 4176 RVA: 0x0004CC54 File Offset: 0x0004AE54
	private IEnumerator LoadGuildInfo()
	{
		this.loadingOverlay.SetActive(true);
		string text = string.Concat(new string[]
		{
			GlobalSettings.ApiHost,
			"api/",
			SettingsManager.Instance.ApiAccount.AccountUniqueId,
			"/guild",
			string.Format("/{0}?accountId={1}", this.uiSystemModule.GuildModule.ActiveGuildId, SettingsManager.Instance.ApiAccount.AccountId)
		});
		if (!string.IsNullOrEmpty(this.searchInput.text))
		{
			text = text + "&memberPlayerName=" + this.searchInput.text;
		}
		yield return ApiManager.Get<Guild>(text, delegate(ApiResponse<Guild> response)
		{
			this.DestroyMemberObjects();
			this.guildNameText.text = response.ResponseObject.GuildName.ToLower();
			foreach (GuildMember member in response.ResponseObject.Members)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.memberPrefab);
				gameObject.transform.SetParent(this.guildMembersHolder.transform, false);
				gameObject.transform.position = Vector3.zero;
				GuildMemberModule guildMemberModule;
				gameObject.TryGetComponent<GuildMemberModule>(out guildMemberModule);
				guildMemberModule.SetMember(member);
			}
			this.loadingOverlay.SetActive(false);
		});
		yield break;
	}

	// Token: 0x06001051 RID: 4177 RVA: 0x0004CC64 File Offset: 0x0004AE64
	private void DestroyMemberObjects()
	{
		int childCount = this.guildMembersHolder.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = this.guildMembersHolder.transform.GetChild(i);
			if (child != null)
			{
				UnityEngine.Object.Destroy(child.gameObject);
			}
		}
	}

	// Token: 0x04000FC9 RID: 4041
	[SerializeField]
	private Text guildNameText;

	// Token: 0x04000FCA RID: 4042
	[SerializeField]
	private InputField searchInput;

	// Token: 0x04000FCB RID: 4043
	[SerializeField]
	private GameObject memberPrefab;

	// Token: 0x04000FCC RID: 4044
	[SerializeField]
	private GameObject guildMembersHolder;

	// Token: 0x04000FCD RID: 4045
	[SerializeField]
	private GameObject loadingOverlay;

	// Token: 0x04000FCE RID: 4046
	[SerializeField]
	private GameObject noGuildOverlay;

	// Token: 0x04000FCF RID: 4047
	[SerializeField]
	private Button leaveGuildButton;

	// Token: 0x04000FD0 RID: 4048
	[SerializeField]
	private Button deleteGuildButton;

	// Token: 0x04000FD1 RID: 4049
	[SerializeField]
	private UISystemModule uiSystemModule;
}
