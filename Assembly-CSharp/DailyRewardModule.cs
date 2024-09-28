using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

// Token: 0x02000315 RID: 789
public class DailyRewardModule : NetworkBehaviour
{
	// Token: 0x06000EF1 RID: 3825 RVA: 0x00046D40 File Offset: 0x00044F40
	private void Awake()
	{
		if (NetworkServer.active)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("DatabaseManager");
			this.dailyRewardDatabaseModule = gameObject.GetComponent<DailyRewardDatabaseModule>();
		}
		this.effectModule = base.GetComponent<EffectModule>();
		this.playerModule = base.GetComponent<PlayerModule>();
	}

	// Token: 0x06000EF2 RID: 3826 RVA: 0x00046D83 File Offset: 0x00044F83
	public override void OnStartServer()
	{
		this.maxRewardId = this.dailyRewardDatabaseModule.GetMaxRewardId();
		base.StartCoroutine(this.UpdateDailyRewardAsync());
	}

	// Token: 0x06000EF3 RID: 3827 RVA: 0x00046DA4 File Offset: 0x00044FA4
	[Client]
	private void OnCanGetReward(bool oldValue, bool newValue)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void DailyRewardModule::OnCanGetReward(System.Boolean,System.Boolean)' called when client was not active");
			return;
		}
		this.NetworkCanGetReward = newValue;
		if ((!oldValue && newValue) & !this.playerModule.IsFirstConnection)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
			this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
			this.uiSystemModule.ShowDailyRewardWindow();
		}
	}

	// Token: 0x06000EF4 RID: 3828 RVA: 0x00046E08 File Offset: 0x00045008
	[Server]
	private IEnumerator UpdateDailyRewardAsync()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Collections.IEnumerator DailyRewardModule::UpdateDailyRewardAsync()' called when server was not active");
			return null;
		}
		DailyRewardModule.<UpdateDailyRewardAsync>d__12 <UpdateDailyRewardAsync>d__ = new DailyRewardModule.<UpdateDailyRewardAsync>d__12(0);
		<UpdateDailyRewardAsync>d__.<>4__this = this;
		return <UpdateDailyRewardAsync>d__;
	}

	// Token: 0x06000EF5 RID: 3829 RVA: 0x00046E44 File Offset: 0x00045044
	[Server]
	private Task SetCanGetRewardIfOver24Hours()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Threading.Tasks.Task DailyRewardModule::SetCanGetRewardIfOver24Hours()' called when server was not active");
			return null;
		}
		DailyRewardModule.<SetCanGetRewardIfOver24Hours>d__13 <SetCanGetRewardIfOver24Hours>d__;
		<SetCanGetRewardIfOver24Hours>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SetCanGetRewardIfOver24Hours>d__.<>4__this = this;
		<SetCanGetRewardIfOver24Hours>d__.<>1__state = -1;
		<SetCanGetRewardIfOver24Hours>d__.<>t__builder.Start<DailyRewardModule.<SetCanGetRewardIfOver24Hours>d__13>(ref <SetCanGetRewardIfOver24Hours>d__);
		return <SetCanGetRewardIfOver24Hours>d__.<>t__builder.Task;
	}

	// Token: 0x06000EF6 RID: 3830 RVA: 0x00046EA8 File Offset: 0x000450A8
	public void LoadRewards()
	{
		foreach (DailyReward item in this.dailyRewardDatabaseModule.GetDailyRewards())
		{
			this.Rewards.Add(item);
		}
	}

	// Token: 0x06000EF7 RID: 3831 RVA: 0x00046EE4 File Offset: 0x000450E4
	[Command]
	public void CmdTakeReward()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		base.SendCommandInternal("System.Void DailyRewardModule::CmdTakeReward()", 28148129, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000EF8 RID: 3832 RVA: 0x00046F14 File Offset: 0x00045114
	public DailyRewardModule()
	{
		base.InitSyncObject(this.Rewards);
	}

	// Token: 0x06000EFA RID: 3834 RVA: 0x0002FEC9 File Offset: 0x0002E0C9
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x170001B4 RID: 436
	// (get) Token: 0x06000EFB RID: 3835 RVA: 0x00046F48 File Offset: 0x00045148
	// (set) Token: 0x06000EFC RID: 3836 RVA: 0x00046F5B File Offset: 0x0004515B
	public int NetworkDailyRewardId
	{
		get
		{
			return this.DailyRewardId;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.DailyRewardId, 1UL, null);
		}
	}

	// Token: 0x170001B5 RID: 437
	// (get) Token: 0x06000EFD RID: 3837 RVA: 0x00046F78 File Offset: 0x00045178
	// (set) Token: 0x06000EFE RID: 3838 RVA: 0x00046F8B File Offset: 0x0004518B
	public bool NetworkCanGetReward
	{
		get
		{
			return this.CanGetReward;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<bool>(value, ref this.CanGetReward, 2UL, new Action<bool, bool>(this.OnCanGetReward));
		}
	}

	// Token: 0x06000EFF RID: 3839 RVA: 0x00046FB0 File Offset: 0x000451B0
	protected void UserCode_CmdTakeReward()
	{
		DailyRewardModule.<CmdTakeReward>d__15 <CmdTakeReward>d__;
		<CmdTakeReward>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CmdTakeReward>d__.<>4__this = this;
		<CmdTakeReward>d__.<>1__state = -1;
		<CmdTakeReward>d__.<>t__builder.Start<DailyRewardModule.<CmdTakeReward>d__15>(ref <CmdTakeReward>d__);
	}

	// Token: 0x06000F00 RID: 3840 RVA: 0x00046FE7 File Offset: 0x000451E7
	protected static void InvokeUserCode_CmdTakeReward(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdTakeReward called on client.");
			return;
		}
		((DailyRewardModule)obj).UserCode_CmdTakeReward();
	}

	// Token: 0x06000F01 RID: 3841 RVA: 0x0004700A File Offset: 0x0004520A
	static DailyRewardModule()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(DailyRewardModule), "System.Void DailyRewardModule::CmdTakeReward()", new RemoteCallDelegate(DailyRewardModule.InvokeUserCode_CmdTakeReward), true);
	}

	// Token: 0x06000F02 RID: 3842 RVA: 0x00047030 File Offset: 0x00045230
	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteInt(this.DailyRewardId);
			writer.WriteBool(this.CanGetReward);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1UL) != 0UL)
		{
			writer.WriteInt(this.DailyRewardId);
		}
		if ((base.syncVarDirtyBits & 2UL) != 0UL)
		{
			writer.WriteBool(this.CanGetReward);
		}
	}

	// Token: 0x06000F03 RID: 3843 RVA: 0x000470B8 File Offset: 0x000452B8
	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.DailyRewardId, null, reader.ReadInt());
			base.GeneratedSyncVarDeserialize<bool>(ref this.CanGetReward, new Action<bool, bool>(this.OnCanGetReward), reader.ReadBool());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 1L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.DailyRewardId, null, reader.ReadInt());
		}
		if ((num & 2L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<bool>(ref this.CanGetReward, new Action<bool, bool>(this.OnCanGetReward), reader.ReadBool());
		}
	}

	// Token: 0x04000EE7 RID: 3815
	[SyncVar]
	public int DailyRewardId;

	// Token: 0x04000EE8 RID: 3816
	[SyncVar(hook = "OnCanGetReward")]
	public bool CanGetReward;

	// Token: 0x04000EE9 RID: 3817
	private DateTime lastDailyRewardDate;

	// Token: 0x04000EEA RID: 3818
	public readonly SyncListDailyReward Rewards = new SyncListDailyReward();

	// Token: 0x04000EEB RID: 3819
	private int maxRewardId = 1;

	// Token: 0x04000EEC RID: 3820
	private EffectModule effectModule;

	// Token: 0x04000EED RID: 3821
	private PlayerModule playerModule;

	// Token: 0x04000EEE RID: 3822
	private DailyRewardDatabaseModule dailyRewardDatabaseModule;

	// Token: 0x04000EEF RID: 3823
	private UISystemModule uiSystemModule;
}
