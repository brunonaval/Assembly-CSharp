using System;
using UnityEngine;

// Token: 0x02000212 RID: 530
public class MobileChangeTargetButtonManager : MonoBehaviour
{
	// Token: 0x06000728 RID: 1832 RVA: 0x000231C8 File Offset: 0x000213C8
	public void OnClick()
	{
		this.uiSystemModule.CombatModule.ChangeTarget(7f);
	}

	// Token: 0x04000926 RID: 2342
	[SerializeField]
	private UISystemModule uiSystemModule;
}
