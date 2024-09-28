using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000270 RID: 624
public class ChangeNameWindowManager : MonoBehaviour
{
	// Token: 0x06000981 RID: 2433 RVA: 0x0002C5FD File Offset: 0x0002A7FD
	private void Start()
	{
		this.newNameInput.Select();
	}

	// Token: 0x06000982 RID: 2434 RVA: 0x0002C60C File Offset: 0x0002A80C
	public void OnConfirmButtonClicked()
	{
		if (!GlobalUtils.IsValidPlayerName(this.newNameInput.text))
		{
			this.uiSystemModule.EffectModule.ShowScreenMessage("invalid_character_name_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		this.uiSystemModule.CreatureModule.CmdChangePlayerName(this.newNameInput.text);
		DragWindowModule dragWindowModule;
		base.TryGetComponent<DragWindowModule>(out dragWindowModule);
		dragWindowModule.CloseWindow();
	}

	// Token: 0x04000B21 RID: 2849
	[SerializeField]
	private InputField newNameInput;

	// Token: 0x04000B22 RID: 2850
	[SerializeField]
	private UISystemModule uiSystemModule;
}
