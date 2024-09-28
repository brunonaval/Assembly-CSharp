using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001DA RID: 474
public class AutoAttackButtonManager : MonoBehaviour
{
	// Token: 0x06000593 RID: 1427 RVA: 0x0001DA54 File Offset: 0x0001BC54
	public void ToggleAutoAttack()
	{
		this.uiSystemModule.PlayerModule.AutoAttackEnabled = !this.uiSystemModule.PlayerModule.AutoAttackEnabled;
		if (this.uiSystemModule.PlayerModule.AutoAttackEnabled)
		{
			this.autoAttackIconImage.color = Color.green;
			this.uiSystemModule.EffectModule.ShowScreenMessage("auto_attack_enabled_message", 1, 3.5f, Array.Empty<string>());
			return;
		}
		this.autoAttackIconImage.color = Color.white;
		this.uiSystemModule.EffectModule.ShowScreenMessage("auto_attack_disabled_message", 3, 3.5f, Array.Empty<string>());
	}

	// Token: 0x04000801 RID: 2049
	[SerializeField]
	private UISystemModule uiSystemModule;

	// Token: 0x04000802 RID: 2050
	[SerializeField]
	private Image autoAttackIconImage;
}
