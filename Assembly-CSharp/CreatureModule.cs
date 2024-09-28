using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

// Token: 0x0200030E RID: 782
public class CreatureModule : NetworkBehaviour
{
	// Token: 0x1400000C RID: 12
	// (add) Token: 0x06000EAA RID: 3754 RVA: 0x00045CD4 File Offset: 0x00043ED4
	// (remove) Token: 0x06000EAB RID: 3755 RVA: 0x00045D0C File Offset: 0x00043F0C
	public event CreatureModule.OnKilledEventHandler OnKilled;

	// Token: 0x06000EAC RID: 3756 RVA: 0x00045D41 File Offset: 0x00043F41
	public override void OnStartClient()
	{
		if (base.isLocalPlayer)
		{
			GameObject.FindGameObjectWithTag("UISystem").TryGetComponent<UISystemModule>(out this.uiSystemModule);
		}
		base.TryGetComponent<CircleCollider2D>(out this.circleCollider2D);
		base.TryGetComponent<AnimationControllerModule>(out this.animationControllerModule);
		this.UpdateMonsterAliveState();
	}

	// Token: 0x06000EAD RID: 3757 RVA: 0x00045D81 File Offset: 0x00043F81
	private void OnGenderChanged(CreatureGender oldValue, CreatureGender newValue)
	{
		this.NetworkGender = newValue;
		if (this.animationControllerModule == null)
		{
			return;
		}
		this.animationControllerModule.ReloadGenderSkinSprites();
	}

	// Token: 0x06000EAE RID: 3758 RVA: 0x00045DA4 File Offset: 0x00043FA4
	private void OnIsAlive(bool oldValue, bool newValue)
	{
		this.NetworkIsAlive = newValue;
		this.UpdateMonsterAliveState();
	}

	// Token: 0x06000EAF RID: 3759 RVA: 0x00045DB4 File Offset: 0x00043FB4
	private void UpdateMonsterAliveState()
	{
		if (!base.CompareTag("Monster") & !base.CompareTag("DeadMonster"))
		{
			return;
		}
		if (!this.IsAlive)
		{
			base.tag = "DeadMonster";
			if (this.animationControllerModule != null)
			{
				this.animationControllerModule.BroadcastAnimations(AnimationType.Death, default(AnimationConfig));
			}
			if (this.circleCollider2D != null)
			{
				this.circleCollider2D.isTrigger = true;
			}
			if (this.minimapPoint != null)
			{
				this.minimapPoint.SetActive(false);
			}
		}
		if (this.IsAlive)
		{
			base.tag = "Monster";
			if (this.circleCollider2D != null)
			{
				this.circleCollider2D.isTrigger = false;
			}
			if (this.minimapPoint != null)
			{
				this.minimapPoint.SetActive(true);
			}
		}
	}

	// Token: 0x06000EB0 RID: 3760 RVA: 0x00045E92 File Offset: 0x00044092
	[Server]
	public void SetCreatureName(string name)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void CreatureModule::SetCreatureName(System.String)' called when server was not active");
			return;
		}
		this.NetworkCreatureName = name;
	}

	// Token: 0x06000EB1 RID: 3761 RVA: 0x00045EB0 File Offset: 0x000440B0
	[Server]
	public void SetOriginalCreatureName(string name)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void CreatureModule::SetOriginalCreatureName(System.String)' called when server was not active");
			return;
		}
		this.NetworkOriginalCreatureName = name;
	}

	// Token: 0x06000EB2 RID: 3762 RVA: 0x00045ECE File Offset: 0x000440CE
	[Server]
	public void SetCreatureTitle(string title)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void CreatureModule::SetCreatureTitle(System.String)' called when server was not active");
			return;
		}
		this.NetworkCreatureTitle = title;
	}

	// Token: 0x06000EB3 RID: 3763 RVA: 0x00045EEC File Offset: 0x000440EC
	[Server]
	public void Kill(GameObject killer, List<Attacker> attackers)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void CreatureModule::Kill(UnityEngine.GameObject,System.Collections.Generic.List`1<Attacker>)' called when server was not active");
			return;
		}
		if (!this.IsAlive)
		{
			return;
		}
		this.SetAlive(false);
		CreatureModule.OnKilledEventHandler onKilled = this.OnKilled;
		if (onKilled == null)
		{
			return;
		}
		onKilled(killer, attackers);
	}

	// Token: 0x06000EB4 RID: 3764 RVA: 0x00045F25 File Offset: 0x00044125
	[Server]
	public void SetAlive(bool value)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void CreatureModule::SetAlive(System.Boolean)' called when server was not active");
			return;
		}
		this.NetworkIsAlive = value;
		if (this.IsAlive)
		{
			this.NetworkIsRespawning = false;
		}
	}

	// Token: 0x06000EB5 RID: 3765 RVA: 0x00045F52 File Offset: 0x00044152
	[Server]
	public void SetRespawning(bool value)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void CreatureModule::SetRespawning(System.Boolean)' called when server was not active");
			return;
		}
		this.NetworkIsRespawning = value;
	}

	// Token: 0x06000EB6 RID: 3766 RVA: 0x00045F70 File Offset: 0x00044170
	[Server]
	public void SetGender(CreatureGender gender)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void CreatureModule::SetGender(CreatureGender)' called when server was not active");
			return;
		}
		this.NetworkGender = gender;
	}

	// Token: 0x06000EB7 RID: 3767 RVA: 0x00045F8E File Offset: 0x0004418E
	[Server]
	public void SetRank(Rank rank)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void CreatureModule::SetRank(Rank)' called when server was not active");
			return;
		}
		this.NetworkRank = rank;
	}

	// Token: 0x06000EB8 RID: 3768 RVA: 0x00045FAC File Offset: 0x000441AC
	[Server]
	public void SetAllowRankNamePrefix(bool value)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void CreatureModule::SetAllowRankNamePrefix(System.Boolean)' called when server was not active");
			return;
		}
		this.NetworkAllowRankNamePrefix = value;
	}

	// Token: 0x06000EB9 RID: 3769 RVA: 0x00045FCC File Offset: 0x000441CC
	[TargetRpc]
	public void ShowChangeNameWindow()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		this.SendTargetRPCInternal(null, "System.Void CreatureModule::ShowChangeNameWindow()", -290292872, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000EBA RID: 3770 RVA: 0x00045FFC File Offset: 0x000441FC
	[Command]
	public void CmdChangePlayerName(string newName)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(newName);
		base.SendCommandInternal("System.Void CreatureModule::CmdChangePlayerName(System.String)", -308900518, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000EBC RID: 3772 RVA: 0x0002FEC9 File Offset: 0x0002E0C9
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x170001A9 RID: 425
	// (get) Token: 0x06000EBD RID: 3773 RVA: 0x00046040 File Offset: 0x00044240
	// (set) Token: 0x06000EBE RID: 3774 RVA: 0x00046053 File Offset: 0x00044253
	public string NetworkCreatureName
	{
		get
		{
			return this.CreatureName;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<string>(value, ref this.CreatureName, 1UL, null);
		}
	}

	// Token: 0x170001AA RID: 426
	// (get) Token: 0x06000EBF RID: 3775 RVA: 0x00046070 File Offset: 0x00044270
	// (set) Token: 0x06000EC0 RID: 3776 RVA: 0x00046083 File Offset: 0x00044283
	public string NetworkOriginalCreatureName
	{
		get
		{
			return this.OriginalCreatureName;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<string>(value, ref this.OriginalCreatureName, 2UL, null);
		}
	}

	// Token: 0x170001AB RID: 427
	// (get) Token: 0x06000EC1 RID: 3777 RVA: 0x000460A0 File Offset: 0x000442A0
	// (set) Token: 0x06000EC2 RID: 3778 RVA: 0x000460B3 File Offset: 0x000442B3
	public string NetworkCreatureTitle
	{
		get
		{
			return this.CreatureTitle;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<string>(value, ref this.CreatureTitle, 4UL, null);
		}
	}

	// Token: 0x170001AC RID: 428
	// (get) Token: 0x06000EC3 RID: 3779 RVA: 0x000460D0 File Offset: 0x000442D0
	// (set) Token: 0x06000EC4 RID: 3780 RVA: 0x000460E3 File Offset: 0x000442E3
	public CreatureGender NetworkGender
	{
		get
		{
			return this.Gender;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<CreatureGender>(value, ref this.Gender, 8UL, new Action<CreatureGender, CreatureGender>(this.OnGenderChanged));
		}
	}

	// Token: 0x170001AD RID: 429
	// (get) Token: 0x06000EC5 RID: 3781 RVA: 0x00046108 File Offset: 0x00044308
	// (set) Token: 0x06000EC6 RID: 3782 RVA: 0x0004611B File Offset: 0x0004431B
	public Rank NetworkRank
	{
		get
		{
			return this.Rank;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<Rank>(value, ref this.Rank, 16UL, null);
		}
	}

	// Token: 0x170001AE RID: 430
	// (get) Token: 0x06000EC7 RID: 3783 RVA: 0x00046138 File Offset: 0x00044338
	// (set) Token: 0x06000EC8 RID: 3784 RVA: 0x0004614B File Offset: 0x0004434B
	public bool NetworkAllowRankNamePrefix
	{
		get
		{
			return this.AllowRankNamePrefix;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<bool>(value, ref this.AllowRankNamePrefix, 32UL, null);
		}
	}

	// Token: 0x170001AF RID: 431
	// (get) Token: 0x06000EC9 RID: 3785 RVA: 0x00046168 File Offset: 0x00044368
	// (set) Token: 0x06000ECA RID: 3786 RVA: 0x0004617B File Offset: 0x0004437B
	public bool NetworkIsAlive
	{
		get
		{
			return this.IsAlive;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<bool>(value, ref this.IsAlive, 64UL, new Action<bool, bool>(this.OnIsAlive));
		}
	}

	// Token: 0x170001B0 RID: 432
	// (get) Token: 0x06000ECB RID: 3787 RVA: 0x000461A0 File Offset: 0x000443A0
	// (set) Token: 0x06000ECC RID: 3788 RVA: 0x000461B3 File Offset: 0x000443B3
	public bool NetworkIsRespawning
	{
		get
		{
			return this.IsRespawning;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<bool>(value, ref this.IsRespawning, 128UL, null);
		}
	}

	// Token: 0x06000ECD RID: 3789 RVA: 0x000461CD File Offset: 0x000443CD
	protected void UserCode_ShowChangeNameWindow()
	{
		this.uiSystemModule.ShowChangeNameWindow();
	}

	// Token: 0x06000ECE RID: 3790 RVA: 0x000461DA File Offset: 0x000443DA
	protected static void InvokeUserCode_ShowChangeNameWindow(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC ShowChangeNameWindow called on server.");
			return;
		}
		((CreatureModule)obj).UserCode_ShowChangeNameWindow();
	}

	// Token: 0x06000ECF RID: 3791 RVA: 0x00046200 File Offset: 0x00044400
	protected void UserCode_CmdChangePlayerName__String(string newName)
	{
		CreatureModule.<CmdChangePlayerName>d__30 <CmdChangePlayerName>d__;
		<CmdChangePlayerName>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CmdChangePlayerName>d__.<>4__this = this;
		<CmdChangePlayerName>d__.newName = newName;
		<CmdChangePlayerName>d__.<>1__state = -1;
		<CmdChangePlayerName>d__.<>t__builder.Start<CreatureModule.<CmdChangePlayerName>d__30>(ref <CmdChangePlayerName>d__);
	}

	// Token: 0x06000ED0 RID: 3792 RVA: 0x0004623F File Offset: 0x0004443F
	protected static void InvokeUserCode_CmdChangePlayerName__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdChangePlayerName called on client.");
			return;
		}
		((CreatureModule)obj).UserCode_CmdChangePlayerName__String(reader.ReadString());
	}

	// Token: 0x06000ED1 RID: 3793 RVA: 0x00046268 File Offset: 0x00044468
	static CreatureModule()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(CreatureModule), "System.Void CreatureModule::CmdChangePlayerName(System.String)", new RemoteCallDelegate(CreatureModule.InvokeUserCode_CmdChangePlayerName__String), true);
		RemoteProcedureCalls.RegisterRpc(typeof(CreatureModule), "System.Void CreatureModule::ShowChangeNameWindow()", new RemoteCallDelegate(CreatureModule.InvokeUserCode_ShowChangeNameWindow));
	}

	// Token: 0x06000ED2 RID: 3794 RVA: 0x000462B8 File Offset: 0x000444B8
	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteString(this.CreatureName);
			writer.WriteString(this.OriginalCreatureName);
			writer.WriteString(this.CreatureTitle);
			Mirror.GeneratedNetworkCode._Write_CreatureGender(writer, this.Gender);
			Mirror.GeneratedNetworkCode._Write_Rank(writer, this.Rank);
			writer.WriteBool(this.AllowRankNamePrefix);
			writer.WriteBool(this.IsAlive);
			writer.WriteBool(this.IsRespawning);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1UL) != 0UL)
		{
			writer.WriteString(this.CreatureName);
		}
		if ((base.syncVarDirtyBits & 2UL) != 0UL)
		{
			writer.WriteString(this.OriginalCreatureName);
		}
		if ((base.syncVarDirtyBits & 4UL) != 0UL)
		{
			writer.WriteString(this.CreatureTitle);
		}
		if ((base.syncVarDirtyBits & 8UL) != 0UL)
		{
			Mirror.GeneratedNetworkCode._Write_CreatureGender(writer, this.Gender);
		}
		if ((base.syncVarDirtyBits & 16UL) != 0UL)
		{
			Mirror.GeneratedNetworkCode._Write_Rank(writer, this.Rank);
		}
		if ((base.syncVarDirtyBits & 32UL) != 0UL)
		{
			writer.WriteBool(this.AllowRankNamePrefix);
		}
		if ((base.syncVarDirtyBits & 64UL) != 0UL)
		{
			writer.WriteBool(this.IsAlive);
		}
		if ((base.syncVarDirtyBits & 128UL) != 0UL)
		{
			writer.WriteBool(this.IsRespawning);
		}
	}

	// Token: 0x06000ED3 RID: 3795 RVA: 0x00046454 File Offset: 0x00044654
	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			base.GeneratedSyncVarDeserialize<string>(ref this.CreatureName, null, reader.ReadString());
			base.GeneratedSyncVarDeserialize<string>(ref this.OriginalCreatureName, null, reader.ReadString());
			base.GeneratedSyncVarDeserialize<string>(ref this.CreatureTitle, null, reader.ReadString());
			base.GeneratedSyncVarDeserialize<CreatureGender>(ref this.Gender, new Action<CreatureGender, CreatureGender>(this.OnGenderChanged), Mirror.GeneratedNetworkCode._Read_CreatureGender(reader));
			base.GeneratedSyncVarDeserialize<Rank>(ref this.Rank, null, Mirror.GeneratedNetworkCode._Read_Rank(reader));
			base.GeneratedSyncVarDeserialize<bool>(ref this.AllowRankNamePrefix, null, reader.ReadBool());
			base.GeneratedSyncVarDeserialize<bool>(ref this.IsAlive, new Action<bool, bool>(this.OnIsAlive), reader.ReadBool());
			base.GeneratedSyncVarDeserialize<bool>(ref this.IsRespawning, null, reader.ReadBool());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 1L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<string>(ref this.CreatureName, null, reader.ReadString());
		}
		if ((num & 2L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<string>(ref this.OriginalCreatureName, null, reader.ReadString());
		}
		if ((num & 4L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<string>(ref this.CreatureTitle, null, reader.ReadString());
		}
		if ((num & 8L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<CreatureGender>(ref this.Gender, new Action<CreatureGender, CreatureGender>(this.OnGenderChanged), Mirror.GeneratedNetworkCode._Read_CreatureGender(reader));
		}
		if ((num & 16L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<Rank>(ref this.Rank, null, Mirror.GeneratedNetworkCode._Read_Rank(reader));
		}
		if ((num & 32L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<bool>(ref this.AllowRankNamePrefix, null, reader.ReadBool());
		}
		if ((num & 64L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<bool>(ref this.IsAlive, new Action<bool, bool>(this.OnIsAlive), reader.ReadBool());
		}
		if ((num & 128L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<bool>(ref this.IsRespawning, null, reader.ReadBool());
		}
	}

	// Token: 0x04000EC5 RID: 3781
	[SyncVar]
	public string CreatureName;

	// Token: 0x04000EC6 RID: 3782
	[SyncVar]
	public string OriginalCreatureName;

	// Token: 0x04000EC7 RID: 3783
	[SyncVar]
	public string CreatureTitle;

	// Token: 0x04000EC8 RID: 3784
	[SyncVar(hook = "OnGenderChanged")]
	public CreatureGender Gender;

	// Token: 0x04000EC9 RID: 3785
	[SyncVar]
	public Rank Rank;

	// Token: 0x04000ECA RID: 3786
	[SyncVar]
	public bool AllowRankNamePrefix;

	// Token: 0x04000ECB RID: 3787
	[SyncVar(hook = "OnIsAlive")]
	public bool IsAlive;

	// Token: 0x04000ECC RID: 3788
	[SyncVar]
	public bool IsRespawning;

	// Token: 0x04000ECD RID: 3789
	[SerializeField]
	private GameObject minimapPoint;

	// Token: 0x04000ECE RID: 3790
	private UISystemModule uiSystemModule;

	// Token: 0x04000ECF RID: 3791
	private CircleCollider2D circleCollider2D;

	// Token: 0x04000ED0 RID: 3792
	private AnimationControllerModule animationControllerModule;

	// Token: 0x0200030F RID: 783
	// (Invoke) Token: 0x06000ED5 RID: 3797
	public delegate void OnKilledEventHandler(GameObject killer, List<Attacker> attackers);
}
