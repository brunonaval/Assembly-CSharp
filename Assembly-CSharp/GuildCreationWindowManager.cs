using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000333 RID: 819
public class GuildCreationWindowManager : MonoBehaviour
{
	// Token: 0x06000FEE RID: 4078 RVA: 0x0004A90D File Offset: 0x00048B0D
	private void Start()
	{
		this.newGuildNameInput.Select();
	}

	// Token: 0x06000FEF RID: 4079 RVA: 0x0004A91C File Offset: 0x00048B1C
	public void OnConfirmButtonClicked()
	{
		if (!GlobalUtils.IsValidGuildName(this.newGuildNameInput.text))
		{
			this.uiSystemModule.EffectModule.ShowScreenMessage("invalid_guild_name_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		this.uiSystemModule.GuildModule.CmdCreateGuild(this.newGuildNameInput.text);
		DragWindowModule dragWindowModule;
		base.TryGetComponent<DragWindowModule>(out dragWindowModule);
		dragWindowModule.CloseWindow();
	}

	// Token: 0x04000F65 RID: 3941
	[SerializeField]
	private InputField newGuildNameInput;

	// Token: 0x04000F66 RID: 3942
	[SerializeField]
	private UISystemModule uiSystemModule;
}
