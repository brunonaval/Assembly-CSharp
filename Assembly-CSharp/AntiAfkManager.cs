using System;
using System.Collections;
using Mirror;
using UnityEngine;

// Token: 0x020001D1 RID: 465
public class AntiAfkManager : MonoBehaviour
{
	// Token: 0x06000563 RID: 1379 RVA: 0x0001CF0B File Offset: 0x0001B10B
	private IEnumerator Start()
	{
		if (NetworkServer.active)
		{
			UnityEngine.Object.Destroy(this);
			yield break;
		}
		UnityEngine.Object.Destroy(this);
		yield return new WaitUntil(() => NetworkClient.connection.identity != null);
		this.lastVerificationPosition = this.uiSystemModule.PlayerModule.transform.position;
		base.InvokeRepeating("VerificationTimer", this.antiAfkVerificationInterval, this.antiAfkVerificationInterval);
		yield break;
	}

	// Token: 0x06000564 RID: 1380 RVA: 0x0001CF1C File Offset: 0x0001B11C
	private void VerificationTimer()
	{
		if (this.uiSystemModule.AreaModule.AreaType == AreaType.ProtectedArea)
		{
			return;
		}
		if (Time.time - this.uiSystemModule.AreaModule.LastAreaChangeTime <= 300f)
		{
			return;
		}
		if (Time.time - this.uiSystemModule.CombatModule.LastCombatTime >= 300f)
		{
			return;
		}
		if (GlobalUtils.IsClose(this.lastVerificationPosition, this.uiSystemModule.PlayerModule.transform.position, 1.3f))
		{
			return;
		}
		this.lastVerificationPosition = this.uiSystemModule.PlayerModule.transform.position;
		this.antiAfkWindow.SetActive(true);
	}

	// Token: 0x040007DA RID: 2010
	private readonly float antiAfkVerificationInterval = 3600f;

	// Token: 0x040007DB RID: 2011
	[SerializeField]
	private UISystemModule uiSystemModule;

	// Token: 0x040007DC RID: 2012
	[SerializeField]
	private GameObject antiAfkWindow;

	// Token: 0x040007DD RID: 2013
	private Vector3 lastVerificationPosition;
}
