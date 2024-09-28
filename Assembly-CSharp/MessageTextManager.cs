using System;
using UnityEngine;

// Token: 0x0200020B RID: 523
public class MessageTextManager : MonoBehaviour
{
	// Token: 0x06000700 RID: 1792 RVA: 0x0002260E File Offset: 0x0002080E
	public void Initialize(float duration)
	{
		base.gameObject.SetActive(true);
		base.Invoke("DisableObject", duration);
	}

	// Token: 0x06000701 RID: 1793 RVA: 0x000202BF File Offset: 0x0001E4BF
	private void DisableObject()
	{
		base.gameObject.SetActive(false);
	}
}
