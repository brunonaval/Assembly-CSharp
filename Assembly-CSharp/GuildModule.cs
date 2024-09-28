using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

// Token: 0x02000337 RID: 823
public class GuildModule : NetworkBehaviour
{
	// Token: 0x170001CF RID: 463
	// (get) Token: 0x06001004 RID: 4100 RVA: 0x0004AE54 File Offset: 0x00049054
	public bool HasPendingInvitation
	{
		get
		{
			return this.currentInviter != null;
		}
	}

	// Token: 0x06001005 RID: 4101 RVA: 0x0004AE62 File Offset: 0x00049062
	private void Awake()
	{
		base.TryGetComponent<PlayerModule>(out this.playerModule);
		if (NetworkServer.active)
		{
			base.TryGetComponent<EffectModule>(out this.effectModule);
			base.TryGetComponent<InventoryModule>(out this.inventoryModule);
			base.TryGetComponent<CombatModule>(out this.combatModule);
		}
	}

	// Token: 0x06001006 RID: 4102 RVA: 0x0004AE9F File Offset: 0x0004909F
	public override void OnStartLocalPlayer()
	{
		GameObject.FindGameObjectWithTag("UISystem").TryGetComponent<UISystemModule>(out this.uiSystemModule);
	}

	// Token: 0x06001007 RID: 4103 RVA: 0x0004AEB7 File Offset: 0x000490B7
	public override void OnStartServer()
	{
		GameObject.FindGameObjectWithTag("GameEnvironment").TryGetComponent<GameEnvironmentModule>(out this.gameEnvironmentModule);
	}

	// Token: 0x06001008 RID: 4104 RVA: 0x0004AED0 File Offset: 0x000490D0
	[Command]
	public void CmdLeaveGuild()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		base.SendCommandInternal("System.Void GuildModule::CmdLeaveGuild()", -865931684, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001009 RID: 4105 RVA: 0x0004AF00 File Offset: 0x00049100
	[Command]
	public void CmdCreateGuild(string guildName)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(guildName);
		base.SendCommandInternal("System.Void GuildModule::CmdCreateGuild(System.String)", 1606996233, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600100A RID: 4106 RVA: 0x0004AF3C File Offset: 0x0004913C
	[Command]
	public void CmdChangeGuild(string newGuildName, string newLeaderName)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(newGuildName);
		writer.WriteString(newLeaderName);
		base.SendCommandInternal("System.Void GuildModule::CmdChangeGuild(System.String,System.String)", 900905785, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600100B RID: 4107 RVA: 0x0004AF80 File Offset: 0x00049180
	[Command]
	public void CmdExpelMember(string expelledMemberPlayerName, int expelledMemberPlayerId)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(expelledMemberPlayerName);
		writer.WriteInt(expelledMemberPlayerId);
		base.SendCommandInternal("System.Void GuildModule::CmdExpelMember(System.String,System.Int32)", 825528533, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600100C RID: 4108 RVA: 0x0004AFC4 File Offset: 0x000491C4
	[Command]
	public void CmdPromoteMember(string promotedMemberPlayerName, int promotedMemberPlayerId)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(promotedMemberPlayerName);
		writer.WriteInt(promotedMemberPlayerId);
		base.SendCommandInternal("System.Void GuildModule::CmdPromoteMember(System.String,System.Int32)", -1461919279, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600100D RID: 4109 RVA: 0x0004B008 File Offset: 0x00049208
	[Command]
	public void CmdDeleteGuild()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		base.SendCommandInternal("System.Void GuildModule::CmdDeleteGuild()", -399419638, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600100E RID: 4110 RVA: 0x0004B038 File Offset: 0x00049238
	[TargetRpc]
	public void ShowGuildCreationWindow()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		this.SendTargetRPCInternal(null, "System.Void GuildModule::ShowGuildCreationWindow()", 810244171, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600100F RID: 4111 RVA: 0x0004B068 File Offset: 0x00049268
	[TargetRpc]
	public void ShowChangeGuildWindow()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		this.SendTargetRPCInternal(null, "System.Void GuildModule::ShowChangeGuildWindow()", 1971315580, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001010 RID: 4112 RVA: 0x0004B098 File Offset: 0x00049298
	private void OnActiveGuildName(string oldValue, string newValue)
	{
		this.NetworkActiveGuildName = newValue;
		if (newValue != oldValue)
		{
			this.playerModule.SetGuildName(this.ActiveGuildName);
		}
	}

	// Token: 0x06001011 RID: 4113 RVA: 0x0004B0BC File Offset: 0x000492BC
	[Command]
	public void CmdBeginInviteOnTarget()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		base.SendCommandInternal("System.Void GuildModule::CmdBeginInviteOnTarget()", 340435460, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001012 RID: 4114 RVA: 0x0004B0EC File Offset: 0x000492EC
	[Server]
	public void BeginInvite(GameObject invitee)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void GuildModule::BeginInvite(UnityEngine.GameObject)' called when server was not active");
			return;
		}
		if (invitee == null || !invitee.CompareTag("Player"))
		{
			return;
		}
		if (this.MemberRank == GuildMemberRank.Member)
		{
			this.effectModule.ShowScreenMessage("player_does_not_have_enough_guild_rank_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		GuildModule guildModule;
		invitee.TryGetComponent<GuildModule>(out guildModule);
		if (guildModule.ActiveGuildId != 0)
		{
			this.effectModule.ShowScreenMessage("guild_invitation_already_has_guild_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		if (guildModule.HasPendingInvitation)
		{
			this.effectModule.ShowScreenMessage("guild_invitation_target_already_has_an_invitation_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		if (!GlobalUtils.IsClose(invitee.transform.position, base.transform.position, 5f))
		{
			this.effectModule.ShowScreenMessage("guild_invitation_target_too_far_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		this.currentInviter = base.gameObject;
		guildModule.SendInvite(base.gameObject);
	}

	// Token: 0x06001013 RID: 4115 RVA: 0x0004B1FB File Offset: 0x000493FB
	[Server]
	public void SendInvite(GameObject inviter)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void GuildModule::SendInvite(UnityEngine.GameObject)' called when server was not active");
			return;
		}
		this.currentInviter = inviter;
		this.TargetSendInvite(inviter);
	}

	// Token: 0x06001014 RID: 4116 RVA: 0x0004B220 File Offset: 0x00049420
	[TargetRpc]
	public void TargetSendInvite(GameObject inviter)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteGameObject(inviter);
		this.SendTargetRPCInternal(null, "System.Void GuildModule::TargetSendInvite(UnityEngine.GameObject)", -213060106, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001015 RID: 4117 RVA: 0x0004B25C File Offset: 0x0004945C
	[Command]
	public void CmdJoinGuild(GameObject inviter)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteGameObject(inviter);
		base.SendCommandInternal("System.Void GuildModule::CmdJoinGuild(UnityEngine.GameObject)", -1084240861, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001016 RID: 4118 RVA: 0x0004B298 File Offset: 0x00049498
	[Command]
	public void CmdRejectGuild(GameObject inviter)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteGameObject(inviter);
		base.SendCommandInternal("System.Void GuildModule::CmdRejectGuild(UnityEngine.GameObject)", -279950962, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001017 RID: 4119 RVA: 0x0004B2D2 File Offset: 0x000494D2
	[Server]
	private void ClearCurrentInviter()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void GuildModule::ClearCurrentInviter()' called when server was not active");
			return;
		}
		this.currentInviter = null;
	}

	// Token: 0x06001019 RID: 4121 RVA: 0x0002FEC9 File Offset: 0x0002E0C9
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x170001D0 RID: 464
	// (get) Token: 0x0600101A RID: 4122 RVA: 0x0004B2F0 File Offset: 0x000494F0
	// (set) Token: 0x0600101B RID: 4123 RVA: 0x0004B303 File Offset: 0x00049503
	public int NetworkActiveGuildId
	{
		get
		{
			return this.ActiveGuildId;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.ActiveGuildId, 1UL, null);
		}
	}

	// Token: 0x170001D1 RID: 465
	// (get) Token: 0x0600101C RID: 4124 RVA: 0x0004B320 File Offset: 0x00049520
	// (set) Token: 0x0600101D RID: 4125 RVA: 0x0004B333 File Offset: 0x00049533
	public GuildMemberRank NetworkMemberRank
	{
		get
		{
			return this.MemberRank;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<GuildMemberRank>(value, ref this.MemberRank, 2UL, null);
		}
	}

	// Token: 0x170001D2 RID: 466
	// (get) Token: 0x0600101E RID: 4126 RVA: 0x0004B350 File Offset: 0x00049550
	// (set) Token: 0x0600101F RID: 4127 RVA: 0x0004B363 File Offset: 0x00049563
	public string NetworkActiveGuildName
	{
		get
		{
			return this.ActiveGuildName;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<string>(value, ref this.ActiveGuildName, 4UL, new Action<string, string>(this.OnActiveGuildName));
		}
	}

	// Token: 0x06001020 RID: 4128 RVA: 0x0004B388 File Offset: 0x00049588
	protected void UserCode_CmdLeaveGuild()
	{
		GuildModule.<CmdLeaveGuild>d__15 <CmdLeaveGuild>d__;
		<CmdLeaveGuild>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CmdLeaveGuild>d__.<>4__this = this;
		<CmdLeaveGuild>d__.<>1__state = -1;
		<CmdLeaveGuild>d__.<>t__builder.Start<GuildModule.<CmdLeaveGuild>d__15>(ref <CmdLeaveGuild>d__);
	}

	// Token: 0x06001021 RID: 4129 RVA: 0x0004B3BF File Offset: 0x000495BF
	protected static void InvokeUserCode_CmdLeaveGuild(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdLeaveGuild called on client.");
			return;
		}
		((GuildModule)obj).UserCode_CmdLeaveGuild();
	}

	// Token: 0x06001022 RID: 4130 RVA: 0x0004B3E4 File Offset: 0x000495E4
	protected void UserCode_CmdCreateGuild__String(string guildName)
	{
		GuildModule.<CmdCreateGuild>d__16 <CmdCreateGuild>d__;
		<CmdCreateGuild>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CmdCreateGuild>d__.<>4__this = this;
		<CmdCreateGuild>d__.guildName = guildName;
		<CmdCreateGuild>d__.<>1__state = -1;
		<CmdCreateGuild>d__.<>t__builder.Start<GuildModule.<CmdCreateGuild>d__16>(ref <CmdCreateGuild>d__);
	}

	// Token: 0x06001023 RID: 4131 RVA: 0x0004B423 File Offset: 0x00049623
	protected static void InvokeUserCode_CmdCreateGuild__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdCreateGuild called on client.");
			return;
		}
		((GuildModule)obj).UserCode_CmdCreateGuild__String(reader.ReadString());
	}

	// Token: 0x06001024 RID: 4132 RVA: 0x0004B44C File Offset: 0x0004964C
	protected void UserCode_CmdChangeGuild__String__String(string newGuildName, string newLeaderName)
	{
		GuildModule.<CmdChangeGuild>d__17 <CmdChangeGuild>d__;
		<CmdChangeGuild>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CmdChangeGuild>d__.<>4__this = this;
		<CmdChangeGuild>d__.newGuildName = newGuildName;
		<CmdChangeGuild>d__.newLeaderName = newLeaderName;
		<CmdChangeGuild>d__.<>1__state = -1;
		<CmdChangeGuild>d__.<>t__builder.Start<GuildModule.<CmdChangeGuild>d__17>(ref <CmdChangeGuild>d__);
	}

	// Token: 0x06001025 RID: 4133 RVA: 0x0004B493 File Offset: 0x00049693
	protected static void InvokeUserCode_CmdChangeGuild__String__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdChangeGuild called on client.");
			return;
		}
		((GuildModule)obj).UserCode_CmdChangeGuild__String__String(reader.ReadString(), reader.ReadString());
	}

	// Token: 0x06001026 RID: 4134 RVA: 0x0004B4C4 File Offset: 0x000496C4
	protected void UserCode_CmdExpelMember__String__Int32(string expelledMemberPlayerName, int expelledMemberPlayerId)
	{
		GuildModule.<CmdExpelMember>d__18 <CmdExpelMember>d__;
		<CmdExpelMember>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CmdExpelMember>d__.<>4__this = this;
		<CmdExpelMember>d__.expelledMemberPlayerName = expelledMemberPlayerName;
		<CmdExpelMember>d__.expelledMemberPlayerId = expelledMemberPlayerId;
		<CmdExpelMember>d__.<>1__state = -1;
		<CmdExpelMember>d__.<>t__builder.Start<GuildModule.<CmdExpelMember>d__18>(ref <CmdExpelMember>d__);
	}

	// Token: 0x06001027 RID: 4135 RVA: 0x0004B50B File Offset: 0x0004970B
	protected static void InvokeUserCode_CmdExpelMember__String__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdExpelMember called on client.");
			return;
		}
		((GuildModule)obj).UserCode_CmdExpelMember__String__Int32(reader.ReadString(), reader.ReadInt());
	}

	// Token: 0x06001028 RID: 4136 RVA: 0x0004B53C File Offset: 0x0004973C
	protected void UserCode_CmdPromoteMember__String__Int32(string promotedMemberPlayerName, int promotedMemberPlayerId)
	{
		GuildModule.<CmdPromoteMember>d__19 <CmdPromoteMember>d__;
		<CmdPromoteMember>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CmdPromoteMember>d__.<>4__this = this;
		<CmdPromoteMember>d__.promotedMemberPlayerName = promotedMemberPlayerName;
		<CmdPromoteMember>d__.promotedMemberPlayerId = promotedMemberPlayerId;
		<CmdPromoteMember>d__.<>1__state = -1;
		<CmdPromoteMember>d__.<>t__builder.Start<GuildModule.<CmdPromoteMember>d__19>(ref <CmdPromoteMember>d__);
	}

	// Token: 0x06001029 RID: 4137 RVA: 0x0004B583 File Offset: 0x00049783
	protected static void InvokeUserCode_CmdPromoteMember__String__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdPromoteMember called on client.");
			return;
		}
		((GuildModule)obj).UserCode_CmdPromoteMember__String__Int32(reader.ReadString(), reader.ReadInt());
	}

	// Token: 0x0600102A RID: 4138 RVA: 0x0004B5B4 File Offset: 0x000497B4
	protected void UserCode_CmdDeleteGuild()
	{
		GuildModule.<CmdDeleteGuild>d__20 <CmdDeleteGuild>d__;
		<CmdDeleteGuild>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CmdDeleteGuild>d__.<>4__this = this;
		<CmdDeleteGuild>d__.<>1__state = -1;
		<CmdDeleteGuild>d__.<>t__builder.Start<GuildModule.<CmdDeleteGuild>d__20>(ref <CmdDeleteGuild>d__);
	}

	// Token: 0x0600102B RID: 4139 RVA: 0x0004B5EB File Offset: 0x000497EB
	protected static void InvokeUserCode_CmdDeleteGuild(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdDeleteGuild called on client.");
			return;
		}
		((GuildModule)obj).UserCode_CmdDeleteGuild();
	}

	// Token: 0x0600102C RID: 4140 RVA: 0x0004B60E File Offset: 0x0004980E
	protected void UserCode_ShowGuildCreationWindow()
	{
		this.uiSystemModule.ShowGuildCreationWindow();
	}

	// Token: 0x0600102D RID: 4141 RVA: 0x0004B61B File Offset: 0x0004981B
	protected static void InvokeUserCode_ShowGuildCreationWindow(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC ShowGuildCreationWindow called on server.");
			return;
		}
		((GuildModule)obj).UserCode_ShowGuildCreationWindow();
	}

	// Token: 0x0600102E RID: 4142 RVA: 0x0004B63E File Offset: 0x0004983E
	protected void UserCode_ShowChangeGuildWindow()
	{
		this.uiSystemModule.ShowChangeGuildWindow();
	}

	// Token: 0x0600102F RID: 4143 RVA: 0x0004B64B File Offset: 0x0004984B
	protected static void InvokeUserCode_ShowChangeGuildWindow(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC ShowChangeGuildWindow called on server.");
			return;
		}
		((GuildModule)obj).UserCode_ShowChangeGuildWindow();
	}

	// Token: 0x06001030 RID: 4144 RVA: 0x0004B66E File Offset: 0x0004986E
	protected void UserCode_CmdBeginInviteOnTarget()
	{
		this.BeginInvite(this.combatModule.Target);
	}

	// Token: 0x06001031 RID: 4145 RVA: 0x0004B681 File Offset: 0x00049881
	protected static void InvokeUserCode_CmdBeginInviteOnTarget(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdBeginInviteOnTarget called on client.");
			return;
		}
		((GuildModule)obj).UserCode_CmdBeginInviteOnTarget();
	}

	// Token: 0x06001032 RID: 4146 RVA: 0x0004B6A4 File Offset: 0x000498A4
	protected void UserCode_TargetSendInvite__GameObject(GameObject inviter)
	{
		this.uiSystemModule.ShowGuildInvitationWindow(inviter);
	}

	// Token: 0x06001033 RID: 4147 RVA: 0x0004B6B2 File Offset: 0x000498B2
	protected static void InvokeUserCode_TargetSendInvite__GameObject(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetSendInvite called on server.");
			return;
		}
		((GuildModule)obj).UserCode_TargetSendInvite__GameObject(reader.ReadGameObject());
	}

	// Token: 0x06001034 RID: 4148 RVA: 0x0004B6DC File Offset: 0x000498DC
	protected void UserCode_CmdJoinGuild__GameObject(GameObject inviter)
	{
		GuildModule.<CmdJoinGuild>d__28 <CmdJoinGuild>d__;
		<CmdJoinGuild>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CmdJoinGuild>d__.<>4__this = this;
		<CmdJoinGuild>d__.inviter = inviter;
		<CmdJoinGuild>d__.<>1__state = -1;
		<CmdJoinGuild>d__.<>t__builder.Start<GuildModule.<CmdJoinGuild>d__28>(ref <CmdJoinGuild>d__);
	}

	// Token: 0x06001035 RID: 4149 RVA: 0x0004B71B File Offset: 0x0004991B
	protected static void InvokeUserCode_CmdJoinGuild__GameObject(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdJoinGuild called on client.");
			return;
		}
		((GuildModule)obj).UserCode_CmdJoinGuild__GameObject(reader.ReadGameObject());
	}

	// Token: 0x06001036 RID: 4150 RVA: 0x0004B744 File Offset: 0x00049944
	protected void UserCode_CmdRejectGuild__GameObject(GameObject inviter)
	{
		this.ClearCurrentInviter();
		if (inviter != null)
		{
			GuildModule guildModule;
			inviter.TryGetComponent<GuildModule>(out guildModule);
			guildModule.ClearCurrentInviter();
		}
	}

	// Token: 0x06001037 RID: 4151 RVA: 0x0004B76F File Offset: 0x0004996F
	protected static void InvokeUserCode_CmdRejectGuild__GameObject(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdRejectGuild called on client.");
			return;
		}
		((GuildModule)obj).UserCode_CmdRejectGuild__GameObject(reader.ReadGameObject());
	}

	// Token: 0x06001038 RID: 4152 RVA: 0x0004B798 File Offset: 0x00049998
	static GuildModule()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(GuildModule), "System.Void GuildModule::CmdLeaveGuild()", new RemoteCallDelegate(GuildModule.InvokeUserCode_CmdLeaveGuild), true);
		RemoteProcedureCalls.RegisterCommand(typeof(GuildModule), "System.Void GuildModule::CmdCreateGuild(System.String)", new RemoteCallDelegate(GuildModule.InvokeUserCode_CmdCreateGuild__String), true);
		RemoteProcedureCalls.RegisterCommand(typeof(GuildModule), "System.Void GuildModule::CmdChangeGuild(System.String,System.String)", new RemoteCallDelegate(GuildModule.InvokeUserCode_CmdChangeGuild__String__String), true);
		RemoteProcedureCalls.RegisterCommand(typeof(GuildModule), "System.Void GuildModule::CmdExpelMember(System.String,System.Int32)", new RemoteCallDelegate(GuildModule.InvokeUserCode_CmdExpelMember__String__Int32), true);
		RemoteProcedureCalls.RegisterCommand(typeof(GuildModule), "System.Void GuildModule::CmdPromoteMember(System.String,System.Int32)", new RemoteCallDelegate(GuildModule.InvokeUserCode_CmdPromoteMember__String__Int32), true);
		RemoteProcedureCalls.RegisterCommand(typeof(GuildModule), "System.Void GuildModule::CmdDeleteGuild()", new RemoteCallDelegate(GuildModule.InvokeUserCode_CmdDeleteGuild), true);
		RemoteProcedureCalls.RegisterCommand(typeof(GuildModule), "System.Void GuildModule::CmdBeginInviteOnTarget()", new RemoteCallDelegate(GuildModule.InvokeUserCode_CmdBeginInviteOnTarget), true);
		RemoteProcedureCalls.RegisterCommand(typeof(GuildModule), "System.Void GuildModule::CmdJoinGuild(UnityEngine.GameObject)", new RemoteCallDelegate(GuildModule.InvokeUserCode_CmdJoinGuild__GameObject), true);
		RemoteProcedureCalls.RegisterCommand(typeof(GuildModule), "System.Void GuildModule::CmdRejectGuild(UnityEngine.GameObject)", new RemoteCallDelegate(GuildModule.InvokeUserCode_CmdRejectGuild__GameObject), true);
		RemoteProcedureCalls.RegisterRpc(typeof(GuildModule), "System.Void GuildModule::ShowGuildCreationWindow()", new RemoteCallDelegate(GuildModule.InvokeUserCode_ShowGuildCreationWindow));
		RemoteProcedureCalls.RegisterRpc(typeof(GuildModule), "System.Void GuildModule::ShowChangeGuildWindow()", new RemoteCallDelegate(GuildModule.InvokeUserCode_ShowChangeGuildWindow));
		RemoteProcedureCalls.RegisterRpc(typeof(GuildModule), "System.Void GuildModule::TargetSendInvite(UnityEngine.GameObject)", new RemoteCallDelegate(GuildModule.InvokeUserCode_TargetSendInvite__GameObject));
	}

	// Token: 0x06001039 RID: 4153 RVA: 0x0004B930 File Offset: 0x00049B30
	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteInt(this.ActiveGuildId);
			Mirror.GeneratedNetworkCode._Write_GuildMemberRank(writer, this.MemberRank);
			writer.WriteString(this.ActiveGuildName);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1UL) != 0UL)
		{
			writer.WriteInt(this.ActiveGuildId);
		}
		if ((base.syncVarDirtyBits & 2UL) != 0UL)
		{
			Mirror.GeneratedNetworkCode._Write_GuildMemberRank(writer, this.MemberRank);
		}
		if ((base.syncVarDirtyBits & 4UL) != 0UL)
		{
			writer.WriteString(this.ActiveGuildName);
		}
	}

	// Token: 0x0600103A RID: 4154 RVA: 0x0004B9E4 File Offset: 0x00049BE4
	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.ActiveGuildId, null, reader.ReadInt());
			base.GeneratedSyncVarDeserialize<GuildMemberRank>(ref this.MemberRank, null, Mirror.GeneratedNetworkCode._Read_GuildMemberRank(reader));
			base.GeneratedSyncVarDeserialize<string>(ref this.ActiveGuildName, new Action<string, string>(this.OnActiveGuildName), reader.ReadString());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 1L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.ActiveGuildId, null, reader.ReadInt());
		}
		if ((num & 2L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<GuildMemberRank>(ref this.MemberRank, null, Mirror.GeneratedNetworkCode._Read_GuildMemberRank(reader));
		}
		if ((num & 4L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<string>(ref this.ActiveGuildName, new Action<string, string>(this.OnActiveGuildName), reader.ReadString());
		}
	}

	// Token: 0x04000F7F RID: 3967
	[SyncVar]
	public int ActiveGuildId;

	// Token: 0x04000F80 RID: 3968
	[SyncVar]
	public GuildMemberRank MemberRank;

	// Token: 0x04000F81 RID: 3969
	[SyncVar(hook = "OnActiveGuildName")]
	public string ActiveGuildName;

	// Token: 0x04000F82 RID: 3970
	private EffectModule effectModule;

	// Token: 0x04000F83 RID: 3971
	private PlayerModule playerModule;

	// Token: 0x04000F84 RID: 3972
	private CombatModule combatModule;

	// Token: 0x04000F85 RID: 3973
	private UISystemModule uiSystemModule;

	// Token: 0x04000F86 RID: 3974
	private InventoryModule inventoryModule;

	// Token: 0x04000F87 RID: 3975
	private GameEnvironmentModule gameEnvironmentModule;

	// Token: 0x04000F88 RID: 3976
	private GameObject currentInviter;
}
