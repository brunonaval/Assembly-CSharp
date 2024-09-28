using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000279 RID: 633
public class FriendListWindowManager : MonoBehaviour
{
	// Token: 0x060009AB RID: 2475 RVA: 0x0002D083 File Offset: 0x0002B283
	private void OnEnable()
	{
		this.RefreshFriendList();
	}

	// Token: 0x060009AC RID: 2476 RVA: 0x0002D08C File Offset: 0x0002B28C
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x060009AD RID: 2477 RVA: 0x0002D0B0 File Offset: 0x0002B2B0
	public void RefreshFriendList()
	{
		if (this.uiSystemModule == null)
		{
			return;
		}
		for (int i = 0; i < this.friendListHolder.transform.childCount; i++)
		{
			Transform child = this.friendListHolder.transform.GetChild(i);
			if (child != null)
			{
				UnityEngine.Object.Destroy(child.gameObject);
			}
		}
		foreach (AccountFriend friend in (from o in this.uiSystemModule.ChatModule.Friends
		orderby o.IsOnline descending
		select o).ThenBy((AccountFriend t) => t.FriendName))
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.friendSlotPrefab);
			gameObject.transform.SetParent(this.friendListHolder.transform, false);
			gameObject.transform.position = Vector2.zero;
			gameObject.GetComponent<FriendSlotManager>().SetFriend(friend);
		}
	}

	// Token: 0x060009AE RID: 2478 RVA: 0x0002D1DC File Offset: 0x0002B3DC
	public void OnAddButtonClick()
	{
		if (!string.IsNullOrEmpty(this.friendNameInput.text))
		{
			this.uiSystemModule.ChatModule.CmdAddFriend(this.friendNameInput.text);
		}
	}

	// Token: 0x060009AF RID: 2479 RVA: 0x0002D20B File Offset: 0x0002B40B
	public void OnRemoveButtonClick()
	{
		if (!string.IsNullOrEmpty(this.friendNameInput.text))
		{
			this.uiSystemModule.ChatModule.CmdRemoveFriend(this.friendNameInput.text);
		}
	}

	// Token: 0x04000B40 RID: 2880
	[SerializeField]
	private GameObject friendSlotPrefab;

	// Token: 0x04000B41 RID: 2881
	[SerializeField]
	private GameObject friendListHolder;

	// Token: 0x04000B42 RID: 2882
	[SerializeField]
	private InputField friendNameInput;

	// Token: 0x04000B43 RID: 2883
	private UISystemModule uiSystemModule;
}
