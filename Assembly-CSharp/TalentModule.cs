using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

// Token: 0x02000429 RID: 1065
public class TalentModule : NetworkBehaviour
{
	// Token: 0x1700025A RID: 602
	// (get) Token: 0x06001737 RID: 5943 RVA: 0x00076388 File Offset: 0x00074588
	public int TotalTalentPoints
	{
		get
		{
			if (this._attributeModule.BaseLevel > 1000)
			{
				return this._attributeModule.BaseLevel - 1000;
			}
			return 0;
		}
	}

	// Token: 0x1700025B RID: 603
	// (get) Token: 0x06001738 RID: 5944 RVA: 0x000763B0 File Offset: 0x000745B0
	public int AvailableTalentPoints
	{
		get
		{
			if (this._attributeModule.BaseLevel <= 1000)
			{
				return 0;
			}
			if (this.Talents.Count == 0)
			{
				return this._attributeModule.BaseLevel - 1000;
			}
			int num = this.Talents.Sum((KeyValuePair<TalentIdentifier, int> x) => x.Value);
			return this._attributeModule.BaseLevel - 1000 - num;
		}
	}

	// Token: 0x06001739 RID: 5945 RVA: 0x0007642E File Offset: 0x0007462E
	private void Awake()
	{
		this._playerModule = base.GetComponent<PlayerModule>();
		this._attributeModule = base.GetComponent<AttributeModule>();
	}

	// Token: 0x0600173A RID: 5946 RVA: 0x00076448 File Offset: 0x00074648
	public override void OnStartServer()
	{
		foreach (KeyValuePair<TalentIdentifier, int> keyValuePair in this.Talents)
		{
			ScriptableDatabaseModule.Singleton.GetTalent(keyValuePair.Key).ActivateOnServer(base.gameObject, keyValuePair.Value);
		}
	}

	// Token: 0x0600173B RID: 5947 RVA: 0x000764B8 File Offset: 0x000746B8
	[Command]
	public void CmdIncreaseTalent(TalentIdentifier talentId)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		Mirror.GeneratedNetworkCode._Write_TalentIdentifier(writer, talentId);
		base.SendCommandInternal("System.Void TalentModule::CmdIncreaseTalent(TalentIdentifier)", 744199128, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600173C RID: 5948 RVA: 0x000764F2 File Offset: 0x000746F2
	public TalentModule()
	{
		base.InitSyncObject(this.Talents);
	}

	// Token: 0x0600173D RID: 5949 RVA: 0x0002FEC9 File Offset: 0x0002E0C9
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x0600173E RID: 5950 RVA: 0x00076514 File Offset: 0x00074714
	protected void UserCode_CmdIncreaseTalent__TalentIdentifier(TalentIdentifier talentId)
	{
		TalentModule.<CmdIncreaseTalent>d__9 <CmdIncreaseTalent>d__;
		<CmdIncreaseTalent>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CmdIncreaseTalent>d__.<>4__this = this;
		<CmdIncreaseTalent>d__.talentId = talentId;
		<CmdIncreaseTalent>d__.<>1__state = -1;
		<CmdIncreaseTalent>d__.<>t__builder.Start<TalentModule.<CmdIncreaseTalent>d__9>(ref <CmdIncreaseTalent>d__);
	}

	// Token: 0x0600173F RID: 5951 RVA: 0x00076553 File Offset: 0x00074753
	protected static void InvokeUserCode_CmdIncreaseTalent__TalentIdentifier(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdIncreaseTalent called on client.");
			return;
		}
		((TalentModule)obj).UserCode_CmdIncreaseTalent__TalentIdentifier(Mirror.GeneratedNetworkCode._Read_TalentIdentifier(reader));
	}

	// Token: 0x06001740 RID: 5952 RVA: 0x0007657C File Offset: 0x0007477C
	static TalentModule()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(TalentModule), "System.Void TalentModule::CmdIncreaseTalent(TalentIdentifier)", new RemoteCallDelegate(TalentModule.InvokeUserCode_CmdIncreaseTalent__TalentIdentifier), true);
	}

	// Token: 0x040014AD RID: 5293
	public readonly SyncDictionary<TalentIdentifier, int> Talents = new SyncDictionary<TalentIdentifier, int>();

	// Token: 0x040014AE RID: 5294
	private AttributeModule _attributeModule;

	// Token: 0x040014AF RID: 5295
	private PlayerModule _playerModule;
}
