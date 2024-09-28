using System;
using Mirror;
using UnityEngine;

// Token: 0x02000345 RID: 837
public class IdleModule : MonoBehaviour
{
	// Token: 0x0600105F RID: 4191 RVA: 0x0004D140 File Offset: 0x0004B340
	private void Awake()
	{
		base.TryGetComponent<SkillModule>(out this.skillModule);
		base.TryGetComponent<EffectModule>(out this.effectModule);
		base.TryGetComponent<MovementModule>(out this.movementModule);
		base.TryGetComponent<NetworkIdentity>(out this.networkIdentity);
		base.TryGetComponent<AttributeModule>(out this.attributeModule);
	}

	// Token: 0x06001060 RID: 4192 RVA: 0x0004D18E File Offset: 0x0004B38E
	private void Start()
	{
		if (!NetworkClient.active)
		{
			return;
		}
		if (!this.networkIdentity.isLocalPlayer)
		{
			return;
		}
		base.InvokeRepeating("IdleCheck", 0f, 60f);
	}

	// Token: 0x06001061 RID: 4193 RVA: 0x0004D1BC File Offset: 0x0004B3BC
	private void IdleCheck()
	{
		if (this.attributeModule.AccessLevel == AccessLevel.Administrator)
		{
			return;
		}
		if (this.attributeModule.AccessLevel == AccessLevel.CommunityManager)
		{
			return;
		}
		if (Time.time - this.movementModule.LastMoveTime <= 168000f)
		{
			this.notified = false;
			return;
		}
		if (this.notified)
		{
			this.DisconnectPlayer();
			return;
		}
		this.notified = true;
		this.effectModule.ShowScreenMessage("player_idle_message", 3, 3.5f, Array.Empty<string>());
	}

	// Token: 0x06001062 RID: 4194 RVA: 0x0004D238 File Offset: 0x0004B438
	private void DisconnectPlayer()
	{
		try
		{
			NetworkManagerModule networkManagerModule;
			GameObject.FindGameObjectWithTag("NetworkManager").TryGetComponent<NetworkManagerModule>(out networkManagerModule);
			networkManagerModule.StopClient();
		}
		catch (Exception ex)
		{
			Debug.LogError("(DisconnectPlayer) Error: " + ex.Message + " Details: " + ex.StackTrace);
		}
	}

	// Token: 0x04000FE2 RID: 4066
	private bool notified;

	// Token: 0x04000FE3 RID: 4067
	private SkillModule skillModule;

	// Token: 0x04000FE4 RID: 4068
	private EffectModule effectModule;

	// Token: 0x04000FE5 RID: 4069
	private MovementModule movementModule;

	// Token: 0x04000FE6 RID: 4070
	private NetworkIdentity networkIdentity;

	// Token: 0x04000FE7 RID: 4071
	private AttributeModule attributeModule;
}
