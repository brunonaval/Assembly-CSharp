using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

// Token: 0x020003A6 RID: 934
public class PartyModule : NetworkBehaviour
{
	// Token: 0x17000215 RID: 533
	// (get) Token: 0x06001369 RID: 4969 RVA: 0x0006020B File Offset: 0x0005E40B
	public bool HasPartyMembers
	{
		get
		{
			return this.PartyMembers.Count > 0;
		}
	}

	// Token: 0x17000216 RID: 534
	// (get) Token: 0x0600136A RID: 4970 RVA: 0x0006021B File Offset: 0x0005E41B
	public bool HasPendingInvitation
	{
		get
		{
			return this.currentInviter.IsDefined;
		}
	}

	// Token: 0x17000217 RID: 535
	// (get) Token: 0x0600136B RID: 4971 RVA: 0x00060228 File Offset: 0x0005E428
	public int HighestMemberLevel
	{
		get
		{
			if (this.PartyMembers.Count == 0)
			{
				return this.attributeModule.BaseLevel;
			}
			return Mathf.Max((from pm in this.PartyMembers
			select pm.Member.GetComponent<AttributeModule>()).Max((AttributeModule am) => am.BaseLevel), this.attributeModule.BaseLevel);
		}
	}

	// Token: 0x0600136C RID: 4972 RVA: 0x000602AC File Offset: 0x0005E4AC
	private void Awake()
	{
		if (NetworkClient.active)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
			this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
		}
		base.TryGetComponent<PlayerModule>(out this.playerModule);
		base.TryGetComponent<EffectModule>(out this.effectModule);
		base.TryGetComponent<CombatModule>(out this.combatModule);
		base.TryGetComponent<CreatureModule>(out this.creatureModule);
		base.TryGetComponent<AttributeModule>(out this.attributeModule);
		base.TryGetComponent<PvpModule>(out this.pvpModule);
	}

	// Token: 0x0600136D RID: 4973 RVA: 0x00060325 File Offset: 0x0005E525
	public override void OnStartServer()
	{
		base.InvokeRepeating("RemoveDisconnectedMembersTimer", 0f, 10f);
	}

	// Token: 0x0600136E RID: 4974 RVA: 0x0006033C File Offset: 0x0005E53C
	public override void OnStopServer()
	{
		base.CancelInvoke("RemoveDisconnectedMembersTimer");
	}

	// Token: 0x0600136F RID: 4975 RVA: 0x00060349 File Offset: 0x0005E549
	public override void OnStartLocalPlayer()
	{
		this.PartyMembers.Callback += this.OnPartyMembersUpdated;
	}

	// Token: 0x06001370 RID: 4976 RVA: 0x00060364 File Offset: 0x0005E564
	public List<PartyMember> GetPartyMembersCloseToPosition(Vector3 position, float maxDistance)
	{
		return (from pm in this.PartyMembers
		where pm.IsDefined && Vector3.Distance(pm.Member.transform.position, position) <= maxDistance
		select pm).ToList<PartyMember>();
	}

	// Token: 0x06001371 RID: 4977 RVA: 0x000603A4 File Offset: 0x0005E5A4
	[Client]
	public int GetHighestMemberLevel()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Int32 PartyModule::GetHighestMemberLevel()' called when client was not active");
			return 0;
		}
		if (this.PartyMembers.Count == 0)
		{
			return this.attributeModule.BaseLevel;
		}
		int baseLevel = this.attributeModule.BaseLevel;
		foreach (PartyMember partyMember in this.PartyMembers)
		{
			NetworkIdentity networkIdentity;
			if (NetworkClient.spawned.TryGetValue(partyMember.NetworkInstanceId, out networkIdentity))
			{
				AttributeModule attributeModule;
				networkIdentity.TryGetComponent<AttributeModule>(out attributeModule);
				if (attributeModule.BaseLevel > baseLevel)
				{
					baseLevel = attributeModule.BaseLevel;
				}
			}
		}
		return baseLevel;
	}

	// Token: 0x06001372 RID: 4978 RVA: 0x00060468 File Offset: 0x0005E668
	public int GetHighestMemberLevelCloseToPosition(Vector3 position, float maxDistance)
	{
		if (this.PartyMembers.Count == 0)
		{
			return this.attributeModule.BaseLevel;
		}
		PartyMember[] array = (from pm in this.PartyMembers
		where pm.IsDefined && Vector3.Distance(pm.Member.transform.position, position) <= maxDistance
		select pm).ToArray<PartyMember>();
		if (array.Length == 0)
		{
			return this.attributeModule.BaseLevel;
		}
		return Mathf.Max((from pm in array
		where pm.IsDefined
		select pm.Member.GetComponent<AttributeModule>()).Max((AttributeModule am) => am.BaseLevel), this.attributeModule.BaseLevel);
	}

	// Token: 0x06001373 RID: 4979 RVA: 0x0006054C File Offset: 0x0005E74C
	private void OnPartyMembersUpdated(SyncList<PartyMember>.Operation op, int itemIndex, PartyMember oldItem, PartyMember newItem)
	{
		this.uiSystemModule.ReloadPartyMembers();
		if (this.PartyMembers.Count == 0)
		{
			this.playerModule.ClosePartyChatChannel();
			return;
		}
		this.playerModule.OpenPartyChatChannel(false);
	}

	// Token: 0x06001374 RID: 4980 RVA: 0x00060580 File Offset: 0x0005E780
	[Command]
	public void CmdBeginInviteOnTarget()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		base.SendCommandInternal("System.Void PartyModule::CmdBeginInviteOnTarget()", -1987054345, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001375 RID: 4981 RVA: 0x000605B0 File Offset: 0x0005E7B0
	[Server]
	public void BeginInvite(GameObject invitee)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PartyModule::BeginInvite(UnityEngine.GameObject)' called when server was not active");
			return;
		}
		if (invitee == null || !invitee.CompareTag("Player"))
		{
			this.effectModule.ShowScreenMessage("party_invalid_target_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		if (this.PartyMembers.Count + 1 >= this.maxPartyPlayers)
		{
			this.effectModule.ShowScreenMessage("party_full_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		if (invitee == base.gameObject)
		{
			this.effectModule.ShowScreenMessage("party_cant_invite_yourself_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		PartyModule component = invitee.GetComponent<PartyModule>();
		invitee.GetComponent<AttributeModule>();
		if (component.HasPartyMembers)
		{
			this.effectModule.ShowScreenMessage("party_target_already_has_party_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		if (component.HasPendingInvitation)
		{
			this.effectModule.ShowScreenMessage("party_target_already_has_party_invitation_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		if (!GlobalUtils.IsClose(invitee.transform.position, base.transform.position, 5f))
		{
			this.effectModule.ShowScreenMessage("party_target_too_far_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		PartyMember inviter = new PartyMember(this.creatureModule.CreatureName, this.creatureModule.netId, base.gameObject);
		this.currentInviter = inviter;
		component.SendInvite(inviter);
	}

	// Token: 0x06001376 RID: 4982 RVA: 0x0006072E File Offset: 0x0005E92E
	[Server]
	public void SendInvite(PartyMember inviter)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PartyModule::SendInvite(PartyMember)' called when server was not active");
			return;
		}
		this.currentInviter = inviter;
		this.TargetSendInvite(inviter);
	}

	// Token: 0x06001377 RID: 4983 RVA: 0x00060754 File Offset: 0x0005E954
	[TargetRpc]
	public void TargetSendInvite(PartyMember inviter)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		Mirror.GeneratedNetworkCode._Write_PartyMember(writer, inviter);
		this.SendTargetRPCInternal(null, "System.Void PartyModule::TargetSendInvite(PartyMember)", 894982177, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001378 RID: 4984 RVA: 0x00060790 File Offset: 0x0005E990
	[Command]
	public void CmdJoinParty(PartyMember inviter)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		Mirror.GeneratedNetworkCode._Write_PartyMember(writer, inviter);
		base.SendCommandInternal("System.Void PartyModule::CmdJoinParty(PartyMember)", 462708353, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001379 RID: 4985 RVA: 0x000607CA File Offset: 0x0005E9CA
	[Server]
	private void ClearCurrentInviter()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PartyModule::ClearCurrentInviter()' called when server was not active");
			return;
		}
		this.currentInviter = default(PartyMember);
	}

	// Token: 0x0600137A RID: 4986 RVA: 0x000607F0 File Offset: 0x0005E9F0
	[Command]
	public void CmdRejectParty(PartyMember inviter)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		Mirror.GeneratedNetworkCode._Write_PartyMember(writer, inviter);
		base.SendCommandInternal("System.Void PartyModule::CmdRejectParty(PartyMember)", -476647754, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600137B RID: 4987 RVA: 0x0006082C File Offset: 0x0005EA2C
	[Server]
	public void InviteToParty(GameObject invitee)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PartyModule::InviteToParty(UnityEngine.GameObject)' called when server was not active");
			return;
		}
		if (invitee == null || !invitee.CompareTag("Player"))
		{
			this.effectModule.ShowScreenMessage("party_invalid_target_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		if (invitee == base.gameObject)
		{
			this.effectModule.ShowScreenMessage("party_cant_invite_yourself_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		PartyModule component = invitee.GetComponent<PartyModule>();
		if (component.HasPartyMembers)
		{
			this.effectModule.ShowScreenMessage("party_target_already_has_party_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		CreatureModule component2 = invitee.GetComponent<CreatureModule>();
		PartyMember partyMember = new PartyMember(component2.CreatureName, component2.netId, invitee);
		foreach (PartyMember partyMember2 in this.PartyMembers)
		{
			GameObject member = partyMember2.Member;
			if (!(member == null))
			{
				PartyModule component3 = member.GetComponent<PartyModule>();
				component.AddPartyMember(partyMember2);
				component3.AddPartyMember(partyMember);
			}
		}
		this.AddPartyMember(partyMember);
		this.effectModule.ShowScreenMessage("party_invite_message", 3, 3.5f, new string[]
		{
			partyMember.Name
		});
		PartyMember partyMember3 = new PartyMember(this.creatureModule.CreatureName, this.creatureModule.netId, base.gameObject);
		component.AddPartyMember(partyMember3);
	}

	// Token: 0x0600137C RID: 4988 RVA: 0x000609B0 File Offset: 0x0005EBB0
	public bool IsMember(GameObject player)
	{
		return this.PartyMembers.Any((PartyMember pm) => pm.Member == player);
	}

	// Token: 0x0600137D RID: 4989 RVA: 0x000609E4 File Offset: 0x0005EBE4
	[Server]
	public void LeaveParty()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PartyModule::LeaveParty()' called when server was not active");
			return;
		}
		foreach (PartyMember partyMember in this.PartyMembers)
		{
			GameObject member = partyMember.Member;
			if (!(member == null) && member.activeInHierarchy)
			{
				member.GetComponent<PartyModule>().RemovePartyMember(this.creatureModule.CreatureName);
			}
		}
		this.ClearCurrentInviter();
		this.PartyMembers.Clear();
	}

	// Token: 0x0600137E RID: 4990 RVA: 0x00060A84 File Offset: 0x0005EC84
	[Server]
	public void KickFromParty(string memberName)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PartyModule::KickFromParty(System.String)' called when server was not active");
			return;
		}
		foreach (PartyMember partyMember in this.PartyMembers)
		{
			GameObject member = partyMember.Member;
			if (!(member == null) && member.activeInHierarchy)
			{
				PartyModule component = member.GetComponent<PartyModule>();
				if (partyMember.Name.Equals(memberName, StringComparison.InvariantCultureIgnoreCase))
				{
					component.LeaveParty();
					break;
				}
			}
		}
	}

	// Token: 0x0600137F RID: 4991 RVA: 0x00060B1C File Offset: 0x0005ED1C
	[Server]
	public void RemovePartyMember(string memberName)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PartyModule::RemovePartyMember(System.String)' called when server was not active");
			return;
		}
		for (int i = 0; i < this.PartyMembers.Count; i++)
		{
			if (this.PartyMembers[i].Name.Equals(memberName, StringComparison.InvariantCultureIgnoreCase))
			{
				this.PartyMembers.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x06001380 RID: 4992 RVA: 0x00060B7C File Offset: 0x0005ED7C
	[Server]
	public void AddPartyMember(PartyMember partyMember)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PartyModule::AddPartyMember(PartyMember)' called when server was not active");
			return;
		}
		if (!this.PartyMembers.Any((PartyMember a) => a.Name.Equals(partyMember.Name, StringComparison.InvariantCultureIgnoreCase)))
		{
			this.PartyMembers.Add(partyMember);
		}
	}

	// Token: 0x06001381 RID: 4993 RVA: 0x00060BD8 File Offset: 0x0005EDD8
	[Server]
	private void RemoveDisconnectedMembersTimer()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PartyModule::RemoveDisconnectedMembersTimer()' called when server was not active");
			return;
		}
		for (int i = this.PartyMembers.Count - 1; i >= 0; i--)
		{
			if (this.PartyMembers[i].Member == null || !this.PartyMembers[i].Member.activeInHierarchy)
			{
				this.PartyMembers.RemoveAt(i);
			}
		}
	}

	// Token: 0x06001382 RID: 4994 RVA: 0x00060C50 File Offset: 0x0005EE50
	[Command]
	public void CmdSetTargetFromMember(string memberName)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(memberName);
		base.SendCommandInternal("System.Void PartyModule::CmdSetTargetFromMember(System.String)", -651027850, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001383 RID: 4995 RVA: 0x00060C8A File Offset: 0x0005EE8A
	public PartyModule()
	{
		base.InitSyncObject(this.PartyMembers);
	}

	// Token: 0x06001384 RID: 4996 RVA: 0x0002FEC9 File Offset: 0x0002E0C9
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x06001385 RID: 4997 RVA: 0x00060CB1 File Offset: 0x0005EEB1
	protected void UserCode_CmdBeginInviteOnTarget()
	{
		this.BeginInvite(this.combatModule.Target);
	}

	// Token: 0x06001386 RID: 4998 RVA: 0x00060CC4 File Offset: 0x0005EEC4
	protected static void InvokeUserCode_CmdBeginInviteOnTarget(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdBeginInviteOnTarget called on client.");
			return;
		}
		((PartyModule)obj).UserCode_CmdBeginInviteOnTarget();
	}

	// Token: 0x06001387 RID: 4999 RVA: 0x00060CE7 File Offset: 0x0005EEE7
	protected void UserCode_TargetSendInvite__PartyMember(PartyMember inviter)
	{
		this.uiSystemModule.ShowPartyInvitationWindow(inviter);
	}

	// Token: 0x06001388 RID: 5000 RVA: 0x00060CF5 File Offset: 0x0005EEF5
	protected static void InvokeUserCode_TargetSendInvite__PartyMember(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetSendInvite called on server.");
			return;
		}
		((PartyModule)obj).UserCode_TargetSendInvite__PartyMember(Mirror.GeneratedNetworkCode._Read_PartyMember(reader));
	}

	// Token: 0x06001389 RID: 5001 RVA: 0x00060D20 File Offset: 0x0005EF20
	protected void UserCode_CmdJoinParty__PartyMember(PartyMember inviter)
	{
		this.ClearCurrentInviter();
		if (inviter.Member == null)
		{
			this.effectModule.ShowScreenMessage("party_target_too_far_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		PartyModule partyModule;
		inviter.Member.TryGetComponent<PartyModule>(out partyModule);
		PvpModule pvpModule;
		inviter.Member.TryGetComponent<PvpModule>(out pvpModule);
		partyModule.ClearCurrentInviter();
		if (this.pvpModule.TvtTeam > TvtTeam.None | pvpModule.TvtTeam > TvtTeam.None)
		{
			this.effectModule.ShowScreenMessage("party_cant_join_while_in_tvt_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		if (partyModule.PartyMembers.Count + 1 >= this.maxPartyPlayers)
		{
			this.effectModule.ShowScreenMessage("party_full_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		PartyMember partyMember = new PartyMember(this.creatureModule.CreatureName, this.creatureModule.netId, base.gameObject);
		foreach (PartyMember partyMember2 in partyModule.PartyMembers)
		{
			if (!(partyMember2.Member == null))
			{
				PartyModule partyModule2;
				partyMember2.Member.TryGetComponent<PartyModule>(out partyModule2);
				this.AddPartyMember(partyMember2);
				partyModule2.AddPartyMember(partyMember);
			}
		}
		this.AddPartyMember(inviter);
		partyModule.AddPartyMember(partyMember);
	}

	// Token: 0x0600138A RID: 5002 RVA: 0x00060E88 File Offset: 0x0005F088
	protected static void InvokeUserCode_CmdJoinParty__PartyMember(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdJoinParty called on client.");
			return;
		}
		((PartyModule)obj).UserCode_CmdJoinParty__PartyMember(Mirror.GeneratedNetworkCode._Read_PartyMember(reader));
	}

	// Token: 0x0600138B RID: 5003 RVA: 0x00060EB1 File Offset: 0x0005F0B1
	protected void UserCode_CmdRejectParty__PartyMember(PartyMember inviter)
	{
		if (inviter.IsDefined)
		{
			inviter.Member.GetComponent<PartyModule>().ClearCurrentInviter();
		}
		this.ClearCurrentInviter();
	}

	// Token: 0x0600138C RID: 5004 RVA: 0x00060ED2 File Offset: 0x0005F0D2
	protected static void InvokeUserCode_CmdRejectParty__PartyMember(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdRejectParty called on client.");
			return;
		}
		((PartyModule)obj).UserCode_CmdRejectParty__PartyMember(Mirror.GeneratedNetworkCode._Read_PartyMember(reader));
	}

	// Token: 0x0600138D RID: 5005 RVA: 0x00060EFC File Offset: 0x0005F0FC
	protected void UserCode_CmdSetTargetFromMember__String(string memberName)
	{
		for (int i = 0; i < this.PartyMembers.Count; i++)
		{
			if (this.PartyMembers[i].Name.Equals(memberName, StringComparison.InvariantCultureIgnoreCase))
			{
				CombatModule component = this.PartyMembers[i].Member.GetComponent<CombatModule>();
				this.combatModule.RemoveTarget();
				this.combatModule.SetTarget(component.Target, true);
				return;
			}
		}
	}

	// Token: 0x0600138E RID: 5006 RVA: 0x00060F6E File Offset: 0x0005F16E
	protected static void InvokeUserCode_CmdSetTargetFromMember__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSetTargetFromMember called on client.");
			return;
		}
		((PartyModule)obj).UserCode_CmdSetTargetFromMember__String(reader.ReadString());
	}

	// Token: 0x0600138F RID: 5007 RVA: 0x00060F98 File Offset: 0x0005F198
	static PartyModule()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(PartyModule), "System.Void PartyModule::CmdBeginInviteOnTarget()", new RemoteCallDelegate(PartyModule.InvokeUserCode_CmdBeginInviteOnTarget), true);
		RemoteProcedureCalls.RegisterCommand(typeof(PartyModule), "System.Void PartyModule::CmdJoinParty(PartyMember)", new RemoteCallDelegate(PartyModule.InvokeUserCode_CmdJoinParty__PartyMember), true);
		RemoteProcedureCalls.RegisterCommand(typeof(PartyModule), "System.Void PartyModule::CmdRejectParty(PartyMember)", new RemoteCallDelegate(PartyModule.InvokeUserCode_CmdRejectParty__PartyMember), true);
		RemoteProcedureCalls.RegisterCommand(typeof(PartyModule), "System.Void PartyModule::CmdSetTargetFromMember(System.String)", new RemoteCallDelegate(PartyModule.InvokeUserCode_CmdSetTargetFromMember__String), true);
		RemoteProcedureCalls.RegisterRpc(typeof(PartyModule), "System.Void PartyModule::TargetSendInvite(PartyMember)", new RemoteCallDelegate(PartyModule.InvokeUserCode_TargetSendInvite__PartyMember));
	}

	// Token: 0x040011FF RID: 4607
	private PvpModule pvpModule;

	// Token: 0x04001200 RID: 4608
	private EffectModule effectModule;

	// Token: 0x04001201 RID: 4609
	private CombatModule combatModule;

	// Token: 0x04001202 RID: 4610
	private CreatureModule creatureModule;

	// Token: 0x04001203 RID: 4611
	private AttributeModule attributeModule;

	// Token: 0x04001204 RID: 4612
	private UISystemModule uiSystemModule;

	// Token: 0x04001205 RID: 4613
	private PartyMember currentInviter;

	// Token: 0x04001206 RID: 4614
	private PlayerModule playerModule;

	// Token: 0x04001207 RID: 4615
	private readonly int maxPartyPlayers = 10;

	// Token: 0x04001208 RID: 4616
	public readonly SyncListPartyMember PartyMembers = new SyncListPartyMember();
}
