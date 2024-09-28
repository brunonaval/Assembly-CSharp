using System;
using Mirror;
using UnityEngine;

// Token: 0x02000245 RID: 581
public class TargetTrackerManager : MonoBehaviour
{
	// Token: 0x0600085E RID: 2142 RVA: 0x00028140 File Offset: 0x00026340
	private void Awake()
	{
		if (NetworkServer.active)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x0600085F RID: 2143 RVA: 0x00028177 File Offset: 0x00026377
	private void Start()
	{
		if (!base.GetComponentInParent<PlayerModule>().isLocalPlayer)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
	}

	// Token: 0x06000860 RID: 2144 RVA: 0x00028194 File Offset: 0x00026394
	private void Update()
	{
		Vector3 vector = Vector3.zero;
		if (this.uiSystemModule.QuestModule.ObjectToTrack != null)
		{
			vector = this.uiSystemModule.QuestModule.ObjectToTrack.transform.position;
		}
		else
		{
			vector = this.uiSystemModule.QuestModule.DestinationToTrack;
		}
		if (vector == Vector3.zero)
		{
			this.trackerSprite.enabled = false;
			return;
		}
		if (GlobalUtils.IsClose(vector, this.uiSystemModule.PlayerModule.transform.position))
		{
			this.uiSystemModule.QuestModule.RemoveDestitionToTrack();
			this.trackerSprite.enabled = false;
			return;
		}
		this.trackerSprite.enabled = true;
		Vector3 vector2 = vector - base.transform.position;
		float angle = Mathf.Atan2(vector2.y, vector2.x) * 57.29578f;
		base.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	// Token: 0x04000A31 RID: 2609
	private UISystemModule uiSystemModule;

	// Token: 0x04000A32 RID: 2610
	[SerializeField]
	private SpriteRenderer trackerSprite;
}
