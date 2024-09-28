using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

// Token: 0x02000432 RID: 1074
public class TitleModule : NetworkBehaviour
{
	// Token: 0x06001767 RID: 5991 RVA: 0x0007702C File Offset: 0x0007522C
	private void Awake()
	{
		this.creatureModule = base.GetComponent<CreatureModule>();
		if (NetworkServer.active)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("DatabaseManager");
			this.titleDatabaseModule = gameObject.GetComponent<TitleDatabaseModule>();
		}
	}

	// Token: 0x06001768 RID: 5992 RVA: 0x00077064 File Offset: 0x00075264
	public bool HasTitle(int titleId)
	{
		return this.Titles.Any((Title t) => t.Id == titleId);
	}

	// Token: 0x06001769 RID: 5993 RVA: 0x00077098 File Offset: 0x00075298
	[Command]
	public void CmdSetActiveTitle(Title title)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		Mirror.GeneratedNetworkCode._Write_Title(writer, title);
		base.SendCommandInternal("System.Void TitleModule::CmdSetActiveTitle(Title)", 2063146063, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600176A RID: 5994 RVA: 0x000770D4 File Offset: 0x000752D4
	[Server]
	public void SetActiveTitle(Title title)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void TitleModule::SetActiveTitle(Title)' called when server was not active");
			return;
		}
		this.ActiveTitle = title;
		if (this.creatureModule != null)
		{
			this.creatureModule.SetCreatureTitle(this.ActiveTitle.Name);
		}
	}

	// Token: 0x0600176B RID: 5995 RVA: 0x00077124 File Offset: 0x00075324
	[Command]
	public void CmdClearTitle()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		base.SendCommandInternal("System.Void TitleModule::CmdClearTitle()", -1101559002, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600176C RID: 5996 RVA: 0x00077154 File Offset: 0x00075354
	public Task LoadPlayerTitlesAsync(int playerId)
	{
		TitleModule.<LoadPlayerTitlesAsync>d__9 <LoadPlayerTitlesAsync>d__;
		<LoadPlayerTitlesAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<LoadPlayerTitlesAsync>d__.<>4__this = this;
		<LoadPlayerTitlesAsync>d__.playerId = playerId;
		<LoadPlayerTitlesAsync>d__.<>1__state = -1;
		<LoadPlayerTitlesAsync>d__.<>t__builder.Start<TitleModule.<LoadPlayerTitlesAsync>d__9>(ref <LoadPlayerTitlesAsync>d__);
		return <LoadPlayerTitlesAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600176D RID: 5997 RVA: 0x000771A0 File Offset: 0x000753A0
	[Server]
	public Task AddTitleAsync(int playerId, int titleId)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Threading.Tasks.Task TitleModule::AddTitleAsync(System.Int32,System.Int32)' called when server was not active");
			return null;
		}
		TitleModule.<AddTitleAsync>d__10 <AddTitleAsync>d__;
		<AddTitleAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<AddTitleAsync>d__.<>4__this = this;
		<AddTitleAsync>d__.playerId = playerId;
		<AddTitleAsync>d__.titleId = titleId;
		<AddTitleAsync>d__.<>1__state = -1;
		<AddTitleAsync>d__.<>t__builder.Start<TitleModule.<AddTitleAsync>d__10>(ref <AddTitleAsync>d__);
		return <AddTitleAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600176E RID: 5998 RVA: 0x00077214 File Offset: 0x00075414
	public TitleModule()
	{
		base.InitSyncObject(this.Titles);
	}

	// Token: 0x0600176F RID: 5999 RVA: 0x0002FEC9 File Offset: 0x0002E0C9
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x06001770 RID: 6000 RVA: 0x00077233 File Offset: 0x00075433
	protected void UserCode_CmdSetActiveTitle__Title(Title title)
	{
		this.SetActiveTitle(title);
	}

	// Token: 0x06001771 RID: 6001 RVA: 0x0007723C File Offset: 0x0007543C
	protected static void InvokeUserCode_CmdSetActiveTitle__Title(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSetActiveTitle called on client.");
			return;
		}
		((TitleModule)obj).UserCode_CmdSetActiveTitle__Title(Mirror.GeneratedNetworkCode._Read_Title(reader));
	}

	// Token: 0x06001772 RID: 6002 RVA: 0x00077265 File Offset: 0x00075465
	protected void UserCode_CmdClearTitle()
	{
		this.ActiveTitle = default(Title);
		if (this.creatureModule != null)
		{
			this.creatureModule.SetCreatureTitle(string.Empty);
		}
	}

	// Token: 0x06001773 RID: 6003 RVA: 0x00077291 File Offset: 0x00075491
	protected static void InvokeUserCode_CmdClearTitle(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdClearTitle called on client.");
			return;
		}
		((TitleModule)obj).UserCode_CmdClearTitle();
	}

	// Token: 0x06001774 RID: 6004 RVA: 0x000772B4 File Offset: 0x000754B4
	static TitleModule()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(TitleModule), "System.Void TitleModule::CmdSetActiveTitle(Title)", new RemoteCallDelegate(TitleModule.InvokeUserCode_CmdSetActiveTitle__Title), true);
		RemoteProcedureCalls.RegisterCommand(typeof(TitleModule), "System.Void TitleModule::CmdClearTitle()", new RemoteCallDelegate(TitleModule.InvokeUserCode_CmdClearTitle), true);
	}

	// Token: 0x040014D7 RID: 5335
	public Title ActiveTitle;

	// Token: 0x040014D8 RID: 5336
	public readonly SyncListTitle Titles = new SyncListTitle();

	// Token: 0x040014D9 RID: 5337
	private CreatureModule creatureModule;

	// Token: 0x040014DA RID: 5338
	private TitleDatabaseModule titleDatabaseModule;
}
