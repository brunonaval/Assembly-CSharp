using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001CF RID: 463
public class ActionBarManager : MonoBehaviour
{
	// Token: 0x0600055D RID: 1373 RVA: 0x0001CDDC File Offset: 0x0001AFDC
	private void Update()
	{
		if (this.uiSystemModule.AttributeModule == null)
		{
			return;
		}
		int currentHealth = this.uiSystemModule.AttributeModule.CurrentHealth;
		int currentEndurance = this.uiSystemModule.AttributeModule.CurrentEndurance;
		this.healthGlobeText.text = currentHealth.ToString();
		this.enduranceGlobeText.text = currentEndurance.ToString();
	}

	// Token: 0x040007D4 RID: 2004
	[SerializeField]
	private Text healthGlobeText;

	// Token: 0x040007D5 RID: 2005
	[SerializeField]
	private Text enduranceGlobeText;

	// Token: 0x040007D6 RID: 2006
	[SerializeField]
	private UISystemModule uiSystemModule;
}
