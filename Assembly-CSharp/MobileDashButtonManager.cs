using System;
using UnityEngine;

// Token: 0x02000213 RID: 531
public class MobileDashButtonManager : MonoBehaviour
{
	// Token: 0x0600072A RID: 1834 RVA: 0x000231DF File Offset: 0x000213DF
	public void OnClick()
	{
		if (this.uiSystemModule.SkillModule.IsCasting)
		{
			return;
		}
		this.uiSystemModule.PlayerModule.Dash(this.uiSystemModule.MovementModule.Direction);
	}

	// Token: 0x04000927 RID: 2343
	[SerializeField]
	private UISystemModule uiSystemModule;
}
