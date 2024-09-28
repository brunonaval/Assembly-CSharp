using System;
using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000290 RID: 656
public class TalentButtonManager : MonoBehaviour
{
	// Token: 0x06000A43 RID: 2627 RVA: 0x0002F3B8 File Offset: 0x0002D5B8
	private IEnumerator Start()
	{
		yield return new WaitUntil(() => NetworkClient.connection.identity != null);
		this._talentModule = NetworkClient.connection.identity.GetComponent<TalentModule>();
		yield break;
	}

	// Token: 0x06000A44 RID: 2628 RVA: 0x0002F3C7 File Offset: 0x0002D5C7
	private void Update()
	{
		this.notificationImage.enabled = (this._talentModule.AvailableTalentPoints != 0);
	}

	// Token: 0x04000BC5 RID: 3013
	[SerializeField]
	private Image notificationImage;

	// Token: 0x04000BC6 RID: 3014
	private TalentModule _talentModule;
}
