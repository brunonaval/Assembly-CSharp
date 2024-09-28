using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001F4 RID: 500
public class FriendSlotManager : MonoBehaviour
{
	// Token: 0x06000645 RID: 1605 RVA: 0x0001FEFC File Offset: 0x0001E0FC
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x06000646 RID: 1606 RVA: 0x0001FF20 File Offset: 0x0001E120
	public void SetFriend(AccountFriend friend)
	{
		this.friend = friend;
		if (this.friend.IsDefined)
		{
			this.friendNameText.text = friend.FriendName.ToLower();
			this.friendNameText.color = new Color(1f, 0.8784314f, 0.7058824f, 1f);
			this.muteButtonObject.SetActive(true);
			this.unmuteButtonObject.SetActive(false);
			if (this.friend.Muted)
			{
				this.friendNameText.color = Color.red;
				this.muteButtonObject.SetActive(false);
				this.unmuteButtonObject.SetActive(true);
				return;
			}
			if (this.friend.IsOnline)
			{
				this.friendNameText.color = Color.green;
				return;
			}
		}
	}

	// Token: 0x06000647 RID: 1607 RVA: 0x0001FFEC File Offset: 0x0001E1EC
	public void OnMuteButtonClick()
	{
		if (!this.friend.IsDefined)
		{
			return;
		}
		this.friend.Muted = true;
		this.SetFriend(this.friend);
		this.uiSystemModule.ChatModule.CmdMuteFriend(this.friend.FriendName);
	}

	// Token: 0x06000648 RID: 1608 RVA: 0x0002003C File Offset: 0x0001E23C
	public void OnUnMuteButtonClick()
	{
		if (!this.friend.IsDefined)
		{
			return;
		}
		this.friend.Muted = false;
		this.SetFriend(this.friend);
		this.uiSystemModule.ChatModule.CmdUnMuteFriend(this.friend.FriendName);
	}

	// Token: 0x06000649 RID: 1609 RVA: 0x0002008A File Offset: 0x0001E28A
	public void OnRemoveButtonClick()
	{
		if (!this.friend.IsDefined)
		{
			return;
		}
		this.uiSystemModule.ChatModule.CmdRemoveFriend(this.friend.FriendName);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600064A RID: 1610 RVA: 0x000200C0 File Offset: 0x0001E2C0
	public void OnSendMessageClick()
	{
		if (!this.friend.IsDefined)
		{
			return;
		}
		if (this.friend.IsOnline)
		{
			this.uiSystemModule.PlayerModule.ChatFocused = true;
			this.uiSystemModule.PlatformChatHolderManager.OpenChannel(this.friend.FriendName, true, false, false, false);
			this.uiSystemModule.PlatformChatHolderManager.SetSendMessageInputFocus(true, null);
			this.uiSystemModule.HideFriendListWindow();
			return;
		}
		this.uiSystemModule.EffectModule.ShowScreenMessage("chat_player_not_online_message", 0, 3.5f, new string[]
		{
			this.friend.FriendName
		});
	}

	// Token: 0x04000888 RID: 2184
	[SerializeField]
	private Text friendNameText;

	// Token: 0x04000889 RID: 2185
	[SerializeField]
	private GameObject muteButtonObject;

	// Token: 0x0400088A RID: 2186
	[SerializeField]
	private GameObject unmuteButtonObject;

	// Token: 0x0400088B RID: 2187
	private AccountFriend friend;

	// Token: 0x0400088C RID: 2188
	private UISystemModule uiSystemModule;
}
