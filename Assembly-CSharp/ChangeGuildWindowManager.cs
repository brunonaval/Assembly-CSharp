using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000332 RID: 818
public class ChangeGuildWindowManager : MonoBehaviour
{
	// Token: 0x06000FEB RID: 4075 RVA: 0x0004A88A File Offset: 0x00048A8A
	private void Start()
	{
		this.newGuildNameInput.Select();
	}

	// Token: 0x06000FEC RID: 4076 RVA: 0x0004A898 File Offset: 0x00048A98
	public void OnConfirmButtonClicked()
	{
		if (!GlobalUtils.IsValidGuildName(this.newGuildNameInput.text))
		{
			this.uiSystemModule.EffectModule.ShowScreenMessage("invalid_guild_name_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		this.uiSystemModule.GuildModule.CmdChangeGuild(this.newGuildNameInput.text, this.newLeaderNameInput.text);
		DragWindowModule dragWindowModule;
		base.TryGetComponent<DragWindowModule>(out dragWindowModule);
		dragWindowModule.CloseWindow();
	}

	// Token: 0x04000F62 RID: 3938
	[SerializeField]
	private InputField newGuildNameInput;

	// Token: 0x04000F63 RID: 3939
	[SerializeField]
	private InputField newLeaderNameInput;

	// Token: 0x04000F64 RID: 3940
	[SerializeField]
	private UISystemModule uiSystemModule;
}
