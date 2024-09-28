using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

// Token: 0x020003F2 RID: 1010
public class PvpModule : NetworkBehaviour
{
	// Token: 0x060015A0 RID: 5536 RVA: 0x0006E7DB File Offset: 0x0006C9DB
	public bool HasPlayerKillerLimitations()
	{
		return this.PvpStatus == PvpStatus.PlayerKiller | this.PvpStatus == PvpStatus.Outlaw;
	}

	// Token: 0x1700023E RID: 574
	// (get) Token: 0x060015A1 RID: 5537 RVA: 0x0006E7F0 File Offset: 0x0006C9F0
	public float EquipmentDropChance
	{
		get
		{
			if (this.PvpStatus == PvpStatus.PlayerKiller)
			{
				return Mathf.Min(0.05f + 0.01f * (float)this.KillScore, 1f);
			}
			if (this.PvpStatus == PvpStatus.Outlaw)
			{
				return Mathf.Min(0.05f + 0.01f * (float)this.KillScore * 3f, 1f);
			}
			return 0f;
		}
	}

	// Token: 0x1700023F RID: 575
	// (get) Token: 0x060015A2 RID: 5538 RVA: 0x0006E856 File Offset: 0x0006CA56
	public string TvtSpawnPoint
	{
		get
		{
			if (this.TvtTeam != TvtTeam.BlueTeam)
			{
				return "red_team_spawn_point";
			}
			return "blue_team_spawn_point";
		}
	}

	// Token: 0x060015A3 RID: 5539 RVA: 0x0006E86C File Offset: 0x0006CA6C
	private void Awake()
	{
		base.TryGetComponent<ChatModule>(out this.chatModule);
		base.TryGetComponent<PlayerModule>(out this.playerModule);
		base.TryGetComponent<EffectModule>(out this.effectModule);
		base.TryGetComponent<AttributeModule>(out this.attributeModule);
		base.TryGetComponent<AreaModule>(out this.areaModule);
		base.TryGetComponent<CombatModule>(out this.combatModule);
		base.TryGetComponent<ConditionModule>(out this.conditionModule);
		if (NetworkClient.active)
		{
			GameObject.FindGameObjectWithTag("UISystem").TryGetComponent<UISystemModule>(out this.uiSystemModule);
		}
	}

	// Token: 0x060015A4 RID: 5540 RVA: 0x0006E8F4 File Offset: 0x0006CAF4
	public override void OnStartServer()
	{
		base.InvokeRepeating("InCombatTimer", 0f, 5f);
		base.InvokeRepeating("InTvTTimer", 0f, 1f);
		if (ServerSettingsManager.ServerType == ServerType.PvE)
		{
			base.InvokeRepeating("CheckPvpStatusInPVEServerTimer", 5f, 1f);
		}
	}

	// Token: 0x060015A5 RID: 5541 RVA: 0x0006E948 File Offset: 0x0006CB48
	[Server]
	private void CheckPvpStatusInPVEServerTimer()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PvpModule::CheckPvpStatusInPVEServerTimer()' called when server was not active");
			return;
		}
		if (this.TvtTeam != TvtTeam.None)
		{
			return;
		}
		if (this.areaModule.AreaType == AreaType.EventArea)
		{
			return;
		}
		if (this.PvpStatus != PvpStatus.Neutral)
		{
			this.SetPvpStatusAsync(PvpStatus.Neutral, false);
		}
		if (this.PvpEnabled)
		{
			this.DoSetPvpEnabled(false);
		}
	}

	// Token: 0x060015A6 RID: 5542 RVA: 0x0006E9A4 File Offset: 0x0006CBA4
	[Server]
	private void InCombatTimer()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PvpModule::InCombatTimer()' called when server was not active");
			return;
		}
		if (this.PvpStatus == PvpStatus.InCombat && Time.time - this.inCombatTime > this.inCombatInterval)
		{
			this.SetPvpStatusAsync(PvpStatus.Neutral, false);
		}
		if (this.TvtTeam > TvtTeam.None & this.areaModule.AreaType != AreaType.EventArea & Time.time - NpcNorumbria.TvtStartTime >= 30f)
		{
			this.attributeModule.SetEnduranceToMax();
			this.NetworkTvtTeam = TvtTeam.None;
			this.NetworkTeamFightScore = 0;
			this.TeamFightDamage = 0;
			this.NetworkTeamFightAssist = 0;
			this.TeamFightDeaths.Clear();
			this.NetworkPvpEnabled = false;
			if (this.PvpStatus == PvpStatus.InCombat)
			{
				this.SetPvpStatusAsync(PvpStatus.Neutral, false);
			}
			this.combatModule.Attackers.Clear();
		}
	}

	// Token: 0x060015A7 RID: 5543 RVA: 0x0006EA78 File Offset: 0x0006CC78
	[Server]
	public void InTvTTimer()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PvpModule::InTvTTimer()' called when server was not active");
			return;
		}
		if (this.TvtTeam == TvtTeam.None)
		{
			return;
		}
		if (this.areaModule.CurrentAreaExtraParam != "tvt-base")
		{
			this.tvtBaseTimeCounter = 0;
			return;
		}
		if (this.areaModule.CurrentAreaExtraParam == "tvt-base")
		{
			if (!this.conditionModule.HasActiveCondition(ConditionCategory.Paralyzing))
			{
				this.tvtBaseTimeCounter++;
			}
			if (this.tvtBaseTimeCounter >= 35)
			{
				if (this.TeamFightScore > 0)
				{
					this.playerModule.AddToTeamFightScore(base.gameObject, -1);
					return;
				}
				if (this.TeamFightAssist > 0)
				{
					this.playerModule.AddToTeamFightAssist(base.gameObject, -1);
				}
			}
			return;
		}
	}

	// Token: 0x060015A8 RID: 5544 RVA: 0x0006EB37 File Offset: 0x0006CD37
	[Server]
	public void SetCombatScore(int combatScore)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PvpModule::SetCombatScore(System.Int32)' called when server was not active");
			return;
		}
		this.NetworkCombatScore = combatScore;
	}

	// Token: 0x060015A9 RID: 5545 RVA: 0x0006EB55 File Offset: 0x0006CD55
	[Server]
	public void SetKillScore(int killScore)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PvpModule::SetKillScore(System.Int32)' called when server was not active");
			return;
		}
		if (killScore < 0)
		{
			this.NetworkKillScore = 0;
			return;
		}
		this.NetworkKillScore = killScore;
	}

	// Token: 0x060015AA RID: 5546 RVA: 0x0006EB80 File Offset: 0x0006CD80
	[Server]
	public void SetKarmaPoints(int newKarmaPoints)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PvpModule::SetKarmaPoints(System.Int32)' called when server was not active");
			return;
		}
		newKarmaPoints = Mathf.Max(newKarmaPoints, 0);
		if (this.KarmaPoints != newKarmaPoints)
		{
			this.NetworkKarmaPoints = newKarmaPoints;
			if (newKarmaPoints < 1 & this.PvpStatus == PvpStatus.PlayerKiller)
			{
				this.SetPvpStatusAsync(PvpStatus.InCombat, false);
			}
			this.chatModule.SendSystemTranslatedMessage("pvp_karma_changed_message", "orange", false, new string[]
			{
				this.KarmaPoints.ToString()
			});
		}
	}

	// Token: 0x060015AB RID: 5547 RVA: 0x0006EBFD File Offset: 0x0006CDFD
	[Server]
	public void InitializeKarmaPoints(int karmaPoints)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PvpModule::InitializeKarmaPoints(System.Int32)' called when server was not active");
			return;
		}
		this.NetworkKarmaPoints = Mathf.Max(karmaPoints, 0);
	}

	// Token: 0x060015AC RID: 5548 RVA: 0x0006EC21 File Offset: 0x0006CE21
	public void SetPvpEnabled(bool pvpEnabled)
	{
		if (base.isLocalPlayer)
		{
			this.CmdSetPvpEnabled(pvpEnabled);
			return;
		}
		if (base.isServer)
		{
			this.DoSetPvpEnabled(pvpEnabled);
		}
	}

	// Token: 0x060015AD RID: 5549 RVA: 0x0006EC44 File Offset: 0x0006CE44
	[Command]
	private void CmdSetPvpEnabled(bool pvpEnabled)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteBool(pvpEnabled);
		base.SendCommandInternal("System.Void PvpModule::CmdSetPvpEnabled(System.Boolean)", 1238864297, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x060015AE RID: 5550 RVA: 0x0006EC80 File Offset: 0x0006CE80
	[Server]
	private void DoSetPvpEnabled(bool pvpEnabled)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PvpModule::DoSetPvpEnabled(System.Boolean)' called when server was not active");
			return;
		}
		if (pvpEnabled)
		{
			if (this.areaModule.AreaType != AreaType.EventArea && ServerSettingsManager.ServerType == ServerType.PvE)
			{
				this.effectModule.ShowScreenMessage("pvp_cant_enbable_on_pve_server_message", 3, 3.5f, new string[]
				{
					25.ToString()
				});
				return;
			}
			if (this.attributeModule.BaseLevel < 25)
			{
				this.effectModule.ShowScreenMessage("pvp_cant_enbable_low_level_message", 3, 3.5f, new string[]
				{
					25.ToString()
				});
				return;
			}
		}
		this.NetworkPvpEnabled = pvpEnabled;
	}

	// Token: 0x060015AF RID: 5551 RVA: 0x0006ED28 File Offset: 0x0006CF28
	private void OnPvpEnabled(bool oldValue, bool newValue)
	{
		if (base.isLocalPlayer)
		{
			this.uiSystemModule.PlatformPvpButtonIcon.sprite = (newValue ? this.uiSystemModule.PvpEnabledSprite : this.uiSystemModule.PvpDisabledSprite);
			if (newValue)
			{
				this.effectModule.ShowScreenMessage("pvp_enabled_message", 3, 3.5f, Array.Empty<string>());
			}
			else
			{
				this.effectModule.ShowScreenMessage("pvp_disabled_message", 1, 3.5f, Array.Empty<string>());
			}
		}
		this.NetworkPvpEnabled = newValue;
	}

	// Token: 0x060015B0 RID: 5552 RVA: 0x0006EDAA File Offset: 0x0006CFAA
	[Server]
	public void InitializePvpStatus(PvpStatus pvpStatus)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PvpModule::InitializePvpStatus(PvpStatus)' called when server was not active");
			return;
		}
		this.NetworkPvpStatus = pvpStatus;
	}

	// Token: 0x060015B1 RID: 5553 RVA: 0x0006EDC8 File Offset: 0x0006CFC8
	[Server]
	public void SetPvpStatusAsync(PvpStatus pvpStatus, bool shouldPersistData)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PvpModule::SetPvpStatusAsync(PvpStatus,System.Boolean)' called when server was not active");
			return;
		}
		PvpModule.<SetPvpStatusAsync>d__42 <SetPvpStatusAsync>d__;
		<SetPvpStatusAsync>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<SetPvpStatusAsync>d__.<>4__this = this;
		<SetPvpStatusAsync>d__.pvpStatus = pvpStatus;
		<SetPvpStatusAsync>d__.shouldPersistData = shouldPersistData;
		<SetPvpStatusAsync>d__.<>1__state = -1;
		<SetPvpStatusAsync>d__.<>t__builder.Start<PvpModule.<SetPvpStatusAsync>d__42>(ref <SetPvpStatusAsync>d__);
	}

	// Token: 0x060015B2 RID: 5554 RVA: 0x0006EE24 File Offset: 0x0006D024
	[Server]
	public void SetInCombatTime(float time)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PvpModule::SetInCombatTime(System.Single)' called when server was not active");
			return;
		}
		this.inCombatTime = time;
	}

	// Token: 0x060015B4 RID: 5556 RVA: 0x0002FEC9 File Offset: 0x0002E0C9
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x17000240 RID: 576
	// (get) Token: 0x060015B5 RID: 5557 RVA: 0x0006EE60 File Offset: 0x0006D060
	// (set) Token: 0x060015B6 RID: 5558 RVA: 0x0006EE73 File Offset: 0x0006D073
	public int NetworkCombatScore
	{
		get
		{
			return this.CombatScore;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.CombatScore, 1UL, null);
		}
	}

	// Token: 0x17000241 RID: 577
	// (get) Token: 0x060015B7 RID: 5559 RVA: 0x0006EE90 File Offset: 0x0006D090
	// (set) Token: 0x060015B8 RID: 5560 RVA: 0x0006EEA3 File Offset: 0x0006D0A3
	public int NetworkKillScore
	{
		get
		{
			return this.KillScore;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.KillScore, 2UL, null);
		}
	}

	// Token: 0x17000242 RID: 578
	// (get) Token: 0x060015B9 RID: 5561 RVA: 0x0006EEC0 File Offset: 0x0006D0C0
	// (set) Token: 0x060015BA RID: 5562 RVA: 0x0006EED3 File Offset: 0x0006D0D3
	public int NetworkKarmaPoints
	{
		get
		{
			return this.KarmaPoints;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.KarmaPoints, 4UL, null);
		}
	}

	// Token: 0x17000243 RID: 579
	// (get) Token: 0x060015BB RID: 5563 RVA: 0x0006EEF0 File Offset: 0x0006D0F0
	// (set) Token: 0x060015BC RID: 5564 RVA: 0x0006EF03 File Offset: 0x0006D103
	public int NetworkWeekTvtScore
	{
		get
		{
			return this.WeekTvtScore;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.WeekTvtScore, 8UL, null);
		}
	}

	// Token: 0x17000244 RID: 580
	// (get) Token: 0x060015BD RID: 5565 RVA: 0x0006EF20 File Offset: 0x0006D120
	// (set) Token: 0x060015BE RID: 5566 RVA: 0x0006EF33 File Offset: 0x0006D133
	public bool NetworkIsWeekTvtHero
	{
		get
		{
			return this.IsWeekTvtHero;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<bool>(value, ref this.IsWeekTvtHero, 16UL, null);
		}
	}

	// Token: 0x17000245 RID: 581
	// (get) Token: 0x060015BF RID: 5567 RVA: 0x0006EF50 File Offset: 0x0006D150
	// (set) Token: 0x060015C0 RID: 5568 RVA: 0x0006EF63 File Offset: 0x0006D163
	public PvpStatus NetworkPvpStatus
	{
		get
		{
			return this.PvpStatus;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<PvpStatus>(value, ref this.PvpStatus, 32UL, null);
		}
	}

	// Token: 0x17000246 RID: 582
	// (get) Token: 0x060015C1 RID: 5569 RVA: 0x0006EF80 File Offset: 0x0006D180
	// (set) Token: 0x060015C2 RID: 5570 RVA: 0x0006EF93 File Offset: 0x0006D193
	public TvtTeam NetworkTvtTeam
	{
		get
		{
			return this.TvtTeam;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<TvtTeam>(value, ref this.TvtTeam, 64UL, null);
		}
	}

	// Token: 0x17000247 RID: 583
	// (get) Token: 0x060015C3 RID: 5571 RVA: 0x0006EFB0 File Offset: 0x0006D1B0
	// (set) Token: 0x060015C4 RID: 5572 RVA: 0x0006EFC3 File Offset: 0x0006D1C3
	public int NetworkTeamFightScore
	{
		get
		{
			return this.TeamFightScore;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.TeamFightScore, 128UL, null);
		}
	}

	// Token: 0x17000248 RID: 584
	// (get) Token: 0x060015C5 RID: 5573 RVA: 0x0006EFE0 File Offset: 0x0006D1E0
	// (set) Token: 0x060015C6 RID: 5574 RVA: 0x0006EFF3 File Offset: 0x0006D1F3
	public int NetworkTeamFightAssist
	{
		get
		{
			return this.TeamFightAssist;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.TeamFightAssist, 256UL, null);
		}
	}

	// Token: 0x17000249 RID: 585
	// (get) Token: 0x060015C7 RID: 5575 RVA: 0x0006F010 File Offset: 0x0006D210
	// (set) Token: 0x060015C8 RID: 5576 RVA: 0x0006F023 File Offset: 0x0006D223
	public bool NetworkPvpEnabled
	{
		get
		{
			return this.PvpEnabled;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<bool>(value, ref this.PvpEnabled, 512UL, new Action<bool, bool>(this.OnPvpEnabled));
		}
	}

	// Token: 0x060015C9 RID: 5577 RVA: 0x0006F048 File Offset: 0x0006D248
	protected void UserCode_CmdSetPvpEnabled__Boolean(bool pvpEnabled)
	{
		this.DoSetPvpEnabled(pvpEnabled);
	}

	// Token: 0x060015CA RID: 5578 RVA: 0x0006F051 File Offset: 0x0006D251
	protected static void InvokeUserCode_CmdSetPvpEnabled__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSetPvpEnabled called on client.");
			return;
		}
		((PvpModule)obj).UserCode_CmdSetPvpEnabled__Boolean(reader.ReadBool());
	}

	// Token: 0x060015CB RID: 5579 RVA: 0x0006F07A File Offset: 0x0006D27A
	static PvpModule()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(PvpModule), "System.Void PvpModule::CmdSetPvpEnabled(System.Boolean)", new RemoteCallDelegate(PvpModule.InvokeUserCode_CmdSetPvpEnabled__Boolean), true);
	}

	// Token: 0x060015CC RID: 5580 RVA: 0x0006F0A0 File Offset: 0x0006D2A0
	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteInt(this.CombatScore);
			writer.WriteInt(this.KillScore);
			writer.WriteInt(this.KarmaPoints);
			writer.WriteInt(this.WeekTvtScore);
			writer.WriteBool(this.IsWeekTvtHero);
			Mirror.GeneratedNetworkCode._Write_PvpStatus(writer, this.PvpStatus);
			Mirror.GeneratedNetworkCode._Write_TvtTeam(writer, this.TvtTeam);
			writer.WriteInt(this.TeamFightScore);
			writer.WriteInt(this.TeamFightAssist);
			writer.WriteBool(this.PvpEnabled);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1UL) != 0UL)
		{
			writer.WriteInt(this.CombatScore);
		}
		if ((base.syncVarDirtyBits & 2UL) != 0UL)
		{
			writer.WriteInt(this.KillScore);
		}
		if ((base.syncVarDirtyBits & 4UL) != 0UL)
		{
			writer.WriteInt(this.KarmaPoints);
		}
		if ((base.syncVarDirtyBits & 8UL) != 0UL)
		{
			writer.WriteInt(this.WeekTvtScore);
		}
		if ((base.syncVarDirtyBits & 16UL) != 0UL)
		{
			writer.WriteBool(this.IsWeekTvtHero);
		}
		if ((base.syncVarDirtyBits & 32UL) != 0UL)
		{
			Mirror.GeneratedNetworkCode._Write_PvpStatus(writer, this.PvpStatus);
		}
		if ((base.syncVarDirtyBits & 64UL) != 0UL)
		{
			Mirror.GeneratedNetworkCode._Write_TvtTeam(writer, this.TvtTeam);
		}
		if ((base.syncVarDirtyBits & 128UL) != 0UL)
		{
			writer.WriteInt(this.TeamFightScore);
		}
		if ((base.syncVarDirtyBits & 256UL) != 0UL)
		{
			writer.WriteInt(this.TeamFightAssist);
		}
		if ((base.syncVarDirtyBits & 512UL) != 0UL)
		{
			writer.WriteBool(this.PvpEnabled);
		}
	}

	// Token: 0x060015CD RID: 5581 RVA: 0x0006F298 File Offset: 0x0006D498
	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.CombatScore, null, reader.ReadInt());
			base.GeneratedSyncVarDeserialize<int>(ref this.KillScore, null, reader.ReadInt());
			base.GeneratedSyncVarDeserialize<int>(ref this.KarmaPoints, null, reader.ReadInt());
			base.GeneratedSyncVarDeserialize<int>(ref this.WeekTvtScore, null, reader.ReadInt());
			base.GeneratedSyncVarDeserialize<bool>(ref this.IsWeekTvtHero, null, reader.ReadBool());
			base.GeneratedSyncVarDeserialize<PvpStatus>(ref this.PvpStatus, null, Mirror.GeneratedNetworkCode._Read_PvpStatus(reader));
			base.GeneratedSyncVarDeserialize<TvtTeam>(ref this.TvtTeam, null, Mirror.GeneratedNetworkCode._Read_TvtTeam(reader));
			base.GeneratedSyncVarDeserialize<int>(ref this.TeamFightScore, null, reader.ReadInt());
			base.GeneratedSyncVarDeserialize<int>(ref this.TeamFightAssist, null, reader.ReadInt());
			base.GeneratedSyncVarDeserialize<bool>(ref this.PvpEnabled, new Action<bool, bool>(this.OnPvpEnabled), reader.ReadBool());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 1L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.CombatScore, null, reader.ReadInt());
		}
		if ((num & 2L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.KillScore, null, reader.ReadInt());
		}
		if ((num & 4L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.KarmaPoints, null, reader.ReadInt());
		}
		if ((num & 8L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.WeekTvtScore, null, reader.ReadInt());
		}
		if ((num & 16L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<bool>(ref this.IsWeekTvtHero, null, reader.ReadBool());
		}
		if ((num & 32L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<PvpStatus>(ref this.PvpStatus, null, Mirror.GeneratedNetworkCode._Read_PvpStatus(reader));
		}
		if ((num & 64L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<TvtTeam>(ref this.TvtTeam, null, Mirror.GeneratedNetworkCode._Read_TvtTeam(reader));
		}
		if ((num & 128L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.TeamFightScore, null, reader.ReadInt());
		}
		if ((num & 256L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.TeamFightAssist, null, reader.ReadInt());
		}
		if ((num & 512L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<bool>(ref this.PvpEnabled, new Action<bool, bool>(this.OnPvpEnabled), reader.ReadBool());
		}
	}

	// Token: 0x040013C1 RID: 5057
	[SyncVar]
	public int CombatScore;

	// Token: 0x040013C2 RID: 5058
	[SyncVar]
	public int KillScore;

	// Token: 0x040013C3 RID: 5059
	[SyncVar]
	public int KarmaPoints;

	// Token: 0x040013C4 RID: 5060
	[SyncVar]
	public int WeekTvtScore;

	// Token: 0x040013C5 RID: 5061
	[SyncVar]
	public bool IsWeekTvtHero;

	// Token: 0x040013C6 RID: 5062
	[SyncVar]
	public PvpStatus PvpStatus;

	// Token: 0x040013C7 RID: 5063
	[SyncVar]
	public TvtTeam TvtTeam;

	// Token: 0x040013C8 RID: 5064
	[SyncVar]
	public int TeamFightScore;

	// Token: 0x040013C9 RID: 5065
	[SyncVar]
	public int TeamFightAssist;

	// Token: 0x040013CA RID: 5066
	[SyncVar(hook = "OnPvpEnabled")]
	public bool PvpEnabled;

	// Token: 0x040013CB RID: 5067
	public int TeamFightDamage;

	// Token: 0x040013CC RID: 5068
	public List<KeyValuePair<int, float>> TeamFightDeaths = new List<KeyValuePair<int, float>>();

	// Token: 0x040013CD RID: 5069
	private float inCombatTime;

	// Token: 0x040013CE RID: 5070
	private int tvtBaseTimeCounter;

	// Token: 0x040013CF RID: 5071
	private ChatModule chatModule;

	// Token: 0x040013D0 RID: 5072
	private ConditionModule conditionModule;

	// Token: 0x040013D1 RID: 5073
	private CombatModule combatModule;

	// Token: 0x040013D2 RID: 5074
	private PlayerModule playerModule;

	// Token: 0x040013D3 RID: 5075
	private EffectModule effectModule;

	// Token: 0x040013D4 RID: 5076
	private UISystemModule uiSystemModule;

	// Token: 0x040013D5 RID: 5077
	private AttributeModule attributeModule;

	// Token: 0x040013D6 RID: 5078
	private AreaModule areaModule;

	// Token: 0x040013D7 RID: 5079
	private readonly float inCombatInterval = 120f;
}
