using System;
using System.Runtime.InteropServices;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

// Token: 0x020002A2 RID: 674
public class AnimationControllerModule : NetworkBehaviour
{
	// Token: 0x06000AA2 RID: 2722 RVA: 0x000305CD File Offset: 0x0002E7CD
	[ClientCallback]
	private void Awake()
	{
		if (!NetworkClient.active)
		{
			return;
		}
		this.skillModule = base.GetComponent<SkillModule>();
		this.creatureModule = base.GetComponent<CreatureModule>();
		this.conditionModule = base.GetComponent<ConditionModule>();
	}

	// Token: 0x06000AA3 RID: 2723 RVA: 0x000305FE File Offset: 0x0002E7FE
	[ClientCallback]
	private void Start()
	{
		if (!NetworkClient.active)
		{
			return;
		}
		if (base.CompareTag("Player"))
		{
			this.localWalkingThreshold = 1E-06f;
		}
	}

	// Token: 0x06000AA4 RID: 2724 RVA: 0x00030623 File Offset: 0x0002E823
	[ClientCallback]
	private void Update()
	{
		if (!NetworkClient.active)
		{
			return;
		}
		this.UpdateWalkingAnimation();
		this.UpdateStandAnimation();
	}

	// Token: 0x06000AA5 RID: 2725 RVA: 0x0003063C File Offset: 0x0002E83C
	private void UpdateWalkingAnimation()
	{
		if (!this.creatureModule.IsAlive)
		{
			return;
		}
		if ((this.lastPosition - base.transform.position).magnitude < this.localWalkingThreshold)
		{
			return;
		}
		this.lastPosition = base.transform.position;
		base.BroadcastMessage("AnimateWalk", new AnimationConfig(13f), SendMessageOptions.DontRequireReceiver);
		this.lastAnimationTime = Time.time;
	}

	// Token: 0x06000AA6 RID: 2726 RVA: 0x000306BA File Offset: 0x0002E8BA
	private void UpdateStandAnimation()
	{
		if (this.skillModule != null && this.skillModule.IsCasting)
		{
			return;
		}
		if (Time.time - this.lastAnimationTime <= this.stopAnimationInterval)
		{
			return;
		}
		base.BroadcastMessage("AnimateStand", SendMessageOptions.DontRequireReceiver);
	}

	// Token: 0x06000AA7 RID: 2727 RVA: 0x000306FC File Offset: 0x0002E8FC
	[TargetRpc]
	public void TargetSetIsInvisible(NetworkConnection target, bool value)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteBool(value);
		this.SendTargetRPCInternal(target, "System.Void AnimationControllerModule::TargetSetIsInvisible(Mirror.NetworkConnection,System.Boolean)", 1170836127, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000AA8 RID: 2728 RVA: 0x00030738 File Offset: 0x0002E938
	[TargetRpc]
	public void TargetSetColor(NetworkConnection target, int colorId)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(colorId);
		this.SendTargetRPCInternal(target, "System.Void AnimationControllerModule::TargetSetColor(Mirror.NetworkConnection,System.Int32)", -249124487, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000AA9 RID: 2729 RVA: 0x00030774 File Offset: 0x0002E974
	[TargetRpc]
	public void TargetSetColorToOriginal(NetworkConnection target)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		this.SendTargetRPCInternal(target, "System.Void AnimationControllerModule::TargetSetColorToOriginal(Mirror.NetworkConnection)", -544536022, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000AAA RID: 2730 RVA: 0x000307A4 File Offset: 0x0002E9A4
	[Client]
	public void ReloadGenderSkinSprites()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void AnimationControllerModule::ReloadGenderSkinSprites()' called when client was not active");
			return;
		}
		base.BroadcastMessage("LoadGenderSkinSprites", SendMessageOptions.DontRequireReceiver);
	}

	// Token: 0x06000AAB RID: 2731 RVA: 0x000307C8 File Offset: 0x0002E9C8
	[Server]
	public void RunAnimation(AnimationType animationType)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AnimationControllerModule::RunAnimation(AnimationType)' called when server was not active");
			return;
		}
		this.RpcRunAnimation(animationType, default(AnimationConfig));
	}

	// Token: 0x06000AAC RID: 2732 RVA: 0x000307FA File Offset: 0x0002E9FA
	[Server]
	public void RunAnimation(AnimationType animationType, AnimationConfig animationConfig, Direction direction)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AnimationControllerModule::RunAnimation(AnimationType,AnimationConfig,Direction)' called when server was not active");
			return;
		}
		animationConfig.Direction = direction;
		animationConfig.ForceDirection = true;
		this.RpcRunAnimation(animationType, animationConfig);
	}

	// Token: 0x06000AAD RID: 2733 RVA: 0x0003082C File Offset: 0x0002EA2C
	[ClientRpc(channel = 1)]
	private void RpcRunAnimation(AnimationType animationType, AnimationConfig animationConfig)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		Mirror.GeneratedNetworkCode._Write_AnimationType(writer, animationType);
		Mirror.GeneratedNetworkCode._Write_AnimationConfig(writer, animationConfig);
		this.SendRPCInternal("System.Void AnimationControllerModule::RpcRunAnimation(AnimationType,AnimationConfig)", -369061533, writer, 1, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000AAE RID: 2734 RVA: 0x00030870 File Offset: 0x0002EA70
	[Client]
	public void BroadcastAnimations(AnimationType animationType, AnimationConfig animationConfig)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void AnimationControllerModule::BroadcastAnimations(AnimationType,AnimationConfig)' called when client was not active");
			return;
		}
		switch (animationType)
		{
		case AnimationType.Cast:
			base.BroadcastMessage("AnimateCast", animationConfig, SendMessageOptions.DontRequireReceiver);
			return;
		case AnimationType.Slash:
			base.BroadcastMessage("AnimateSlash", animationConfig, SendMessageOptions.DontRequireReceiver);
			return;
		case AnimationType.Thrust:
			base.BroadcastMessage("AnimateThrust", animationConfig, SendMessageOptions.DontRequireReceiver);
			return;
		case AnimationType.ShootArrow:
			base.BroadcastMessage("AnimateShootArrow", animationConfig, SendMessageOptions.DontRequireReceiver);
			return;
		case AnimationType.Death:
			base.BroadcastMessage("AnimateDeath", SendMessageOptions.DontRequireReceiver);
			return;
		default:
			base.BroadcastMessage("AnimateCast", animationConfig, SendMessageOptions.DontRequireReceiver);
			return;
		}
	}

	// Token: 0x06000AB0 RID: 2736 RVA: 0x0002FEC9 File Offset: 0x0002E0C9
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x17000116 RID: 278
	// (get) Token: 0x06000AB1 RID: 2737 RVA: 0x0003093C File Offset: 0x0002EB3C
	// (set) Token: 0x06000AB2 RID: 2738 RVA: 0x0003094F File Offset: 0x0002EB4F
	public bool NetworkUsePremiumSkins
	{
		get
		{
			return this.UsePremiumSkins;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<bool>(value, ref this.UsePremiumSkins, 1UL, null);
		}
	}

	// Token: 0x06000AB3 RID: 2739 RVA: 0x0003096C File Offset: 0x0002EB6C
	protected void UserCode_TargetSetIsInvisible__NetworkConnection__Boolean(NetworkConnection target, bool value)
	{
		AnimationModule[] componentsInChildren = base.GetComponentsInChildren<AnimationModule>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].IsInvisible = value;
		}
	}

	// Token: 0x06000AB4 RID: 2740 RVA: 0x00030997 File Offset: 0x0002EB97
	protected static void InvokeUserCode_TargetSetIsInvisible__NetworkConnection__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetSetIsInvisible called on server.");
			return;
		}
		((AnimationControllerModule)obj).UserCode_TargetSetIsInvisible__NetworkConnection__Boolean(null, reader.ReadBool());
	}

	// Token: 0x06000AB5 RID: 2741 RVA: 0x000309C1 File Offset: 0x0002EBC1
	protected void UserCode_TargetSetColor__NetworkConnection__Int32(NetworkConnection target, int colorId)
	{
		base.BroadcastMessage("SetColor", colorId, SendMessageOptions.DontRequireReceiver);
	}

	// Token: 0x06000AB6 RID: 2742 RVA: 0x000309D5 File Offset: 0x0002EBD5
	protected static void InvokeUserCode_TargetSetColor__NetworkConnection__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetSetColor called on server.");
			return;
		}
		((AnimationControllerModule)obj).UserCode_TargetSetColor__NetworkConnection__Int32(null, reader.ReadInt());
	}

	// Token: 0x06000AB7 RID: 2743 RVA: 0x000309FF File Offset: 0x0002EBFF
	protected void UserCode_TargetSetColorToOriginal__NetworkConnection(NetworkConnection target)
	{
		base.BroadcastMessage("SetColorToOriginal", SendMessageOptions.DontRequireReceiver);
	}

	// Token: 0x06000AB8 RID: 2744 RVA: 0x00030A0D File Offset: 0x0002EC0D
	protected static void InvokeUserCode_TargetSetColorToOriginal__NetworkConnection(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetSetColorToOriginal called on server.");
			return;
		}
		((AnimationControllerModule)obj).UserCode_TargetSetColorToOriginal__NetworkConnection(null);
	}

	// Token: 0x06000AB9 RID: 2745 RVA: 0x00030A31 File Offset: 0x0002EC31
	protected void UserCode_RpcRunAnimation__AnimationType__AnimationConfig(AnimationType animationType, AnimationConfig animationConfig)
	{
		this.BroadcastAnimations(animationType, animationConfig);
	}

	// Token: 0x06000ABA RID: 2746 RVA: 0x00030A3B File Offset: 0x0002EC3B
	protected static void InvokeUserCode_RpcRunAnimation__AnimationType__AnimationConfig(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcRunAnimation called on server.");
			return;
		}
		((AnimationControllerModule)obj).UserCode_RpcRunAnimation__AnimationType__AnimationConfig(Mirror.GeneratedNetworkCode._Read_AnimationType(reader), Mirror.GeneratedNetworkCode._Read_AnimationConfig(reader));
	}

	// Token: 0x06000ABB RID: 2747 RVA: 0x00030A6C File Offset: 0x0002EC6C
	static AnimationControllerModule()
	{
		RemoteProcedureCalls.RegisterRpc(typeof(AnimationControllerModule), "System.Void AnimationControllerModule::RpcRunAnimation(AnimationType,AnimationConfig)", new RemoteCallDelegate(AnimationControllerModule.InvokeUserCode_RpcRunAnimation__AnimationType__AnimationConfig));
		RemoteProcedureCalls.RegisterRpc(typeof(AnimationControllerModule), "System.Void AnimationControllerModule::TargetSetIsInvisible(Mirror.NetworkConnection,System.Boolean)", new RemoteCallDelegate(AnimationControllerModule.InvokeUserCode_TargetSetIsInvisible__NetworkConnection__Boolean));
		RemoteProcedureCalls.RegisterRpc(typeof(AnimationControllerModule), "System.Void AnimationControllerModule::TargetSetColor(Mirror.NetworkConnection,System.Int32)", new RemoteCallDelegate(AnimationControllerModule.InvokeUserCode_TargetSetColor__NetworkConnection__Int32));
		RemoteProcedureCalls.RegisterRpc(typeof(AnimationControllerModule), "System.Void AnimationControllerModule::TargetSetColorToOriginal(Mirror.NetworkConnection)", new RemoteCallDelegate(AnimationControllerModule.InvokeUserCode_TargetSetColorToOriginal__NetworkConnection));
	}

	// Token: 0x06000ABC RID: 2748 RVA: 0x00030AFC File Offset: 0x0002ECFC
	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteBool(this.UsePremiumSkins);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1UL) != 0UL)
		{
			writer.WriteBool(this.UsePremiumSkins);
		}
	}

	// Token: 0x06000ABD RID: 2749 RVA: 0x00030B54 File Offset: 0x0002ED54
	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			base.GeneratedSyncVarDeserialize<bool>(ref this.UsePremiumSkins, null, reader.ReadBool());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 1L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<bool>(ref this.UsePremiumSkins, null, reader.ReadBool());
		}
	}

	// Token: 0x04000C15 RID: 3093
	[SyncVar]
	public bool UsePremiumSkins;

	// Token: 0x04000C16 RID: 3094
	[SerializeField]
	private float stopAnimationInterval = 0.5f;

	// Token: 0x04000C17 RID: 3095
	private float localWalkingThreshold = 0.01f;

	// Token: 0x04000C18 RID: 3096
	private ConditionModule conditionModule;

	// Token: 0x04000C19 RID: 3097
	private CreatureModule creatureModule;

	// Token: 0x04000C1A RID: 3098
	private SkillModule skillModule;

	// Token: 0x04000C1B RID: 3099
	private Vector3 lastPosition;

	// Token: 0x04000C1C RID: 3100
	private float lastAnimationTime;
}
