using System;
using System.Linq;
using System.Runtime.InteropServices;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

// Token: 0x02000436 RID: 1078
public class TradeModule : NetworkBehaviour
{
	// Token: 0x1700025F RID: 607
	// (get) Token: 0x0600177B RID: 6011 RVA: 0x0007756A File Offset: 0x0007576A
	// (set) Token: 0x0600177C RID: 6012 RVA: 0x00077572 File Offset: 0x00075772
	public bool ItemsHasChanged { get; set; }

	// Token: 0x0600177D RID: 6013 RVA: 0x0007757C File Offset: 0x0007577C
	private void Awake()
	{
		if (NetworkClient.active)
		{
			GameObject.FindGameObjectWithTag("UISystem").TryGetComponent<UISystemModule>(out this.uiSystemModule);
		}
		base.TryGetComponent<CombatModule>(out this.combatModule);
		base.TryGetComponent<EffectModule>(out this.effectModule);
		base.TryGetComponent<WalletModule>(out this.walletModule);
		base.TryGetComponent<CreatureModule>(out this.creatureModule);
		base.TryGetComponent<InventoryModule>(out this.inventoryModule);
	}

	// Token: 0x0600177E RID: 6014 RVA: 0x000775E7 File Offset: 0x000757E7
	public override void OnStartClient()
	{
		this.TradingItems.Callback += delegate(SyncList<Item>.Operation op, int itemIndex, Item oldItem, Item newItem)
		{
			this.ItemsHasChanged = true;
		};
	}

	// Token: 0x0600177F RID: 6015 RVA: 0x00077600 File Offset: 0x00075800
	private void OnTradingGoldCoins(long oldValue, long newValue)
	{
		this.ItemsHasChanged = true;
		this.NetworkTradingGoldCoins = newValue;
	}

	// Token: 0x06001780 RID: 6016 RVA: 0x00077610 File Offset: 0x00075810
	private void OnTradeConfirmed(bool oldValue, bool newValue)
	{
		this.ItemsHasChanged = true;
		this.NetworkTradeConfirmed = newValue;
	}

	// Token: 0x06001781 RID: 6017 RVA: 0x00077620 File Offset: 0x00075820
	[Command]
	public void CmdBeginHandshake()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		base.SendCommandInternal("System.Void TradeModule::CmdBeginHandshake()", -1971789795, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001782 RID: 6018 RVA: 0x00077650 File Offset: 0x00075850
	[Server]
	public void BeginHandshake(GameObject target)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void TradeModule::BeginHandshake(UnityEngine.GameObject)' called when server was not active");
			return;
		}
		if (target == null || !target.CompareTag("Player"))
		{
			this.effectModule.ShowScreenMessage("trade_invalid_target_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		if (target == base.gameObject)
		{
			this.effectModule.ShowScreenMessage("trade_cant_invite_yourself_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		if (this.NetworkCurrentTrader != null)
		{
			this.effectModule.ShowScreenMessage("trade_already_trading_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		if (!GlobalUtils.IsClose(target.transform.position, base.transform.position, 5f))
		{
			this.effectModule.ShowScreenMessage("trade_target_too_far_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		TradeModule tradeModule;
		target.TryGetComponent<TradeModule>(out tradeModule);
		if (tradeModule.NetworkCurrentTrader != null)
		{
			this.effectModule.ShowScreenMessage("trade_target_already_trading_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		this.NetworkTradeConfirmed = false;
		this.NetworkCurrentTrader = target;
		this.TradingItems.Clear();
		this.NetworkTradingGoldCoins = 0L;
		tradeModule.SendHandshake(base.gameObject);
	}

	// Token: 0x06001783 RID: 6019 RVA: 0x000777A0 File Offset: 0x000759A0
	[Server]
	public void SendHandshake(GameObject trader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void TradeModule::SendHandshake(UnityEngine.GameObject)' called when server was not active");
			return;
		}
		this.NetworkCurrentTrader = trader;
		this.TargetSendHandshake(trader);
	}

	// Token: 0x06001784 RID: 6020 RVA: 0x000777C8 File Offset: 0x000759C8
	[TargetRpc]
	public void TargetSendHandshake(GameObject trader)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteGameObject(trader);
		this.SendTargetRPCInternal(null, "System.Void TradeModule::TargetSendHandshake(UnityEngine.GameObject)", 194298603, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001785 RID: 6021 RVA: 0x00077804 File Offset: 0x00075A04
	[Command]
	public void CmdAcceptTrade(GameObject trader)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteGameObject(trader);
		base.SendCommandInternal("System.Void TradeModule::CmdAcceptTrade(UnityEngine.GameObject)", 1962710759, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001786 RID: 6022 RVA: 0x00077840 File Offset: 0x00075A40
	[Command]
	public void CmdRejectTrade(GameObject trader)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteGameObject(trader);
		base.SendCommandInternal("System.Void TradeModule::CmdRejectTrade(UnityEngine.GameObject)", 615830448, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001787 RID: 6023 RVA: 0x0007787C File Offset: 0x00075A7C
	[Command]
	public void CmdAddItem(Item item)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		Mirror.GeneratedNetworkCode._Write_Item(writer, item);
		base.SendCommandInternal("System.Void TradeModule::CmdAddItem(Item)", -62276108, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001788 RID: 6024 RVA: 0x000778B8 File Offset: 0x00075AB8
	[Command]
	public void CmdAddGold(long amount)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteLong(amount);
		base.SendCommandInternal("System.Void TradeModule::CmdAddGold(System.Int64)", -1451209050, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001789 RID: 6025 RVA: 0x000778F4 File Offset: 0x00075AF4
	[TargetRpc]
	public void TargetOpenTradeWindow()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		this.SendTargetRPCInternal(null, "System.Void TradeModule::TargetOpenTradeWindow()", 1291543726, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600178A RID: 6026 RVA: 0x00077924 File Offset: 0x00075B24
	[TargetRpc]
	public void TargetCloseTradeWindow()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		this.SendTargetRPCInternal(null, "System.Void TradeModule::TargetCloseTradeWindow()", 469740378, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600178B RID: 6027 RVA: 0x00077954 File Offset: 0x00075B54
	[Command]
	public void CmdConfirmTrade()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		base.SendCommandInternal("System.Void TradeModule::CmdConfirmTrade()", 535947091, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600178C RID: 6028 RVA: 0x00077984 File Offset: 0x00075B84
	[Server]
	public bool CanExecuteTrade()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean TradeModule::CanExecuteTrade()' called when server was not active");
			return default(bool);
		}
		if (this.NetworkCurrentTrader == null)
		{
			return false;
		}
		if (this.walletModule.GoldCoins < this.TradingGoldCoins)
		{
			this.effectModule.ShowScreenMessage("not_enough_coins_to_trade", 3, 3.5f, Array.Empty<string>());
			this.EndTradeForBothPlayers();
			return false;
		}
		foreach (Item item in this.TradingItems)
		{
			if (!this.inventoryModule.ContainsItem(item.UniqueId, item.Amount))
			{
				this.effectModule.ShowScreenMessage("not_have_all_items_to_trade", 3, 3.5f, Array.Empty<string>());
				this.EndTradeForBothPlayers();
				return false;
			}
		}
		InventoryModule inventoryModule;
		this.NetworkCurrentTrader.TryGetComponent<InventoryModule>(out inventoryModule);
		if (inventoryModule.EmptySlots < this.TradingItems.Count)
		{
			this.effectModule.ShowScreenMessage("other_player_not_enough_empty_slots_to_trade", 3, 3.5f, Array.Empty<string>());
			this.EndTradeForBothPlayers();
			return false;
		}
		return true;
	}

	// Token: 0x0600178D RID: 6029 RVA: 0x00077AC0 File Offset: 0x00075CC0
	[Server]
	public void ExecuteTrade()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void TradeModule::ExecuteTrade()' called when server was not active");
			return;
		}
		if (this.NetworkCurrentTrader == null)
		{
			return;
		}
		WalletModule walletModule;
		this.NetworkCurrentTrader.TryGetComponent<WalletModule>(out walletModule);
		CreatureModule creatureModule;
		this.NetworkCurrentTrader.TryGetComponent<CreatureModule>(out creatureModule);
		string text = string.Concat(new string[]
		{
			"Trade: ",
			this.creatureModule.OriginalCreatureName,
			" >--> ",
			creatureModule.OriginalCreatureName,
			"\r\n",
			string.Format(" - Gold Coins: {0}\r\n", this.TradingGoldCoins)
		});
		this.walletModule.AddGoldCoins(-(int)this.TradingGoldCoins);
		walletModule.AddGoldCoins((int)this.TradingGoldCoins);
		InventoryModule inventoryModule;
		this.NetworkCurrentTrader.TryGetComponent<InventoryModule>(out inventoryModule);
		foreach (Item item in this.TradingItems)
		{
			text += string.Format(" - Item: {0}x {1}(+{2}) - {3}\r\n", new object[]
			{
				item.Amount,
				item.Name,
				item.BoostLevel,
				item.Rarity
			});
			this.inventoryModule.ConsumeItem(item, item.Amount);
			inventoryModule.AddItem(item, true);
		}
		this.EndTrade();
		base.StartCoroutine(DiscordWebhookManager.SendInGameCommandsChannel(text));
	}

	// Token: 0x0600178E RID: 6030 RVA: 0x00077C50 File Offset: 0x00075E50
	[Command]
	public void CmdCancelTrade()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		base.SendCommandInternal("System.Void TradeModule::CmdCancelTrade()", -556087555, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600178F RID: 6031 RVA: 0x00077C80 File Offset: 0x00075E80
	[Server]
	private void EndTradeForBothPlayers()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void TradeModule::EndTradeForBothPlayers()' called when server was not active");
			return;
		}
		if (this.NetworkCurrentTrader != null)
		{
			TradeModule tradeModule;
			this.NetworkCurrentTrader.TryGetComponent<TradeModule>(out tradeModule);
			tradeModule.EndTrade();
		}
		this.EndTrade();
	}

	// Token: 0x06001790 RID: 6032 RVA: 0x00077CCA File Offset: 0x00075ECA
	[Server]
	public void EndTrade()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void TradeModule::EndTrade()' called when server was not active");
			return;
		}
		this.NetworkCurrentTrader = null;
		this.NetworkTradeConfirmed = false;
		this.TradingItems.Clear();
		this.NetworkTradingGoldCoins = 0L;
		this.TargetCloseTradeWindow();
	}

	// Token: 0x06001791 RID: 6033 RVA: 0x00077D08 File Offset: 0x00075F08
	public TradeModule()
	{
		base.InitSyncObject(this.TradingItems);
	}

	// Token: 0x06001793 RID: 6035 RVA: 0x0002FEC9 File Offset: 0x0002E0C9
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x17000260 RID: 608
	// (get) Token: 0x06001794 RID: 6036 RVA: 0x00077D30 File Offset: 0x00075F30
	// (set) Token: 0x06001795 RID: 6037 RVA: 0x00077D4F File Offset: 0x00075F4F
	public GameObject NetworkCurrentTrader
	{
		get
		{
			return base.GetSyncVarGameObject(this.___CurrentTraderNetId, ref this.CurrentTrader);
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter_GameObject(value, ref this.CurrentTrader, 1UL, null, ref this.___CurrentTraderNetId);
		}
	}

	// Token: 0x17000261 RID: 609
	// (get) Token: 0x06001796 RID: 6038 RVA: 0x00077D70 File Offset: 0x00075F70
	// (set) Token: 0x06001797 RID: 6039 RVA: 0x00077D83 File Offset: 0x00075F83
	public long NetworkTradingGoldCoins
	{
		get
		{
			return this.TradingGoldCoins;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<long>(value, ref this.TradingGoldCoins, 2UL, new Action<long, long>(this.OnTradingGoldCoins));
		}
	}

	// Token: 0x17000262 RID: 610
	// (get) Token: 0x06001798 RID: 6040 RVA: 0x00077DA8 File Offset: 0x00075FA8
	// (set) Token: 0x06001799 RID: 6041 RVA: 0x00077DBB File Offset: 0x00075FBB
	public bool NetworkTradeConfirmed
	{
		get
		{
			return this.TradeConfirmed;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<bool>(value, ref this.TradeConfirmed, 4UL, new Action<bool, bool>(this.OnTradeConfirmed));
		}
	}

	// Token: 0x0600179A RID: 6042 RVA: 0x00077DE0 File Offset: 0x00075FE0
	protected void UserCode_CmdBeginHandshake()
	{
		this.BeginHandshake(this.combatModule.Target);
	}

	// Token: 0x0600179B RID: 6043 RVA: 0x00077DF3 File Offset: 0x00075FF3
	protected static void InvokeUserCode_CmdBeginHandshake(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdBeginHandshake called on client.");
			return;
		}
		((TradeModule)obj).UserCode_CmdBeginHandshake();
	}

	// Token: 0x0600179C RID: 6044 RVA: 0x00077E16 File Offset: 0x00076016
	protected void UserCode_TargetSendHandshake__GameObject(GameObject trader)
	{
		this.uiSystemModule.ShowTradeHandshakeWindow(trader);
	}

	// Token: 0x0600179D RID: 6045 RVA: 0x00077E24 File Offset: 0x00076024
	protected static void InvokeUserCode_TargetSendHandshake__GameObject(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetSendHandshake called on server.");
			return;
		}
		((TradeModule)obj).UserCode_TargetSendHandshake__GameObject(reader.ReadGameObject());
	}

	// Token: 0x0600179E RID: 6046 RVA: 0x00077E50 File Offset: 0x00076050
	protected void UserCode_CmdAcceptTrade__GameObject(GameObject trader)
	{
		this.NetworkTradeConfirmed = false;
		this.TradingItems.Clear();
		this.NetworkTradingGoldCoins = 0L;
		this.TargetOpenTradeWindow();
		if (trader == null)
		{
			return;
		}
		TradeModule tradeModule;
		if (!trader.TryGetComponent<TradeModule>(out tradeModule))
		{
			return;
		}
		tradeModule.TargetOpenTradeWindow();
	}

	// Token: 0x0600179F RID: 6047 RVA: 0x00077E98 File Offset: 0x00076098
	protected static void InvokeUserCode_CmdAcceptTrade__GameObject(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdAcceptTrade called on client.");
			return;
		}
		((TradeModule)obj).UserCode_CmdAcceptTrade__GameObject(reader.ReadGameObject());
	}

	// Token: 0x060017A0 RID: 6048 RVA: 0x00077EC4 File Offset: 0x000760C4
	protected void UserCode_CmdRejectTrade__GameObject(GameObject trader)
	{
		this.NetworkTradeConfirmed = false;
		this.TradingItems.Clear();
		this.NetworkTradingGoldCoins = 0L;
		this.NetworkCurrentTrader = null;
		if (trader == null)
		{
			return;
		}
		TradeModule tradeModule;
		if (trader.TryGetComponent<TradeModule>(out tradeModule))
		{
			tradeModule.NetworkCurrentTrader = null;
		}
	}

	// Token: 0x060017A1 RID: 6049 RVA: 0x00077F0D File Offset: 0x0007610D
	protected static void InvokeUserCode_CmdRejectTrade__GameObject(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdRejectTrade called on client.");
			return;
		}
		((TradeModule)obj).UserCode_CmdRejectTrade__GameObject(reader.ReadGameObject());
	}

	// Token: 0x060017A2 RID: 6050 RVA: 0x00077F38 File Offset: 0x00076138
	protected void UserCode_CmdAddItem__Item(Item item)
	{
		int num = (from i in this.TradingItems
		where i.UniqueId == item.UniqueId
		select i).Sum((Item i) => i.Amount);
		if (!this.inventoryModule.ContainsItem(item.UniqueId, item.Amount + num))
		{
			return;
		}
		if (item.Soulbind)
		{
			return;
		}
		if (!item.Sellable)
		{
			return;
		}
		this.TradingItems.Add(item);
	}

	// Token: 0x060017A3 RID: 6051 RVA: 0x00077FE1 File Offset: 0x000761E1
	protected static void InvokeUserCode_CmdAddItem__Item(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdAddItem called on client.");
			return;
		}
		((TradeModule)obj).UserCode_CmdAddItem__Item(Mirror.GeneratedNetworkCode._Read_Item(reader));
	}

	// Token: 0x060017A4 RID: 6052 RVA: 0x0007800A File Offset: 0x0007620A
	protected void UserCode_CmdAddGold__Int64(long amount)
	{
		if (amount > this.walletModule.GoldCoins)
		{
			return;
		}
		if (this.TradingGoldCoins != 0L)
		{
			return;
		}
		this.NetworkTradingGoldCoins = amount;
	}

	// Token: 0x060017A5 RID: 6053 RVA: 0x0007802B File Offset: 0x0007622B
	protected static void InvokeUserCode_CmdAddGold__Int64(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdAddGold called on client.");
			return;
		}
		((TradeModule)obj).UserCode_CmdAddGold__Int64(reader.ReadLong());
	}

	// Token: 0x060017A6 RID: 6054 RVA: 0x00078054 File Offset: 0x00076254
	protected void UserCode_TargetOpenTradeWindow()
	{
		this.ItemsHasChanged = true;
		this.uiSystemModule.ShowTradeWindow();
	}

	// Token: 0x060017A7 RID: 6055 RVA: 0x00078068 File Offset: 0x00076268
	protected static void InvokeUserCode_TargetOpenTradeWindow(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetOpenTradeWindow called on server.");
			return;
		}
		((TradeModule)obj).UserCode_TargetOpenTradeWindow();
	}

	// Token: 0x060017A8 RID: 6056 RVA: 0x0007808B File Offset: 0x0007628B
	protected void UserCode_TargetCloseTradeWindow()
	{
		this.uiSystemModule.HideTradeWindow();
	}

	// Token: 0x060017A9 RID: 6057 RVA: 0x00078098 File Offset: 0x00076298
	protected static void InvokeUserCode_TargetCloseTradeWindow(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetCloseTradeWindow called on server.");
			return;
		}
		((TradeModule)obj).UserCode_TargetCloseTradeWindow();
	}

	// Token: 0x060017AA RID: 6058 RVA: 0x000780BC File Offset: 0x000762BC
	protected void UserCode_CmdConfirmTrade()
	{
		if (this.NetworkCurrentTrader == null)
		{
			return;
		}
		TradeModule tradeModule;
		this.NetworkCurrentTrader.TryGetComponent<TradeModule>(out tradeModule);
		if (!tradeModule.TradeConfirmed)
		{
			this.NetworkTradeConfirmed = true;
			return;
		}
		if (!this.CanExecuteTrade() || !tradeModule.CanExecuteTrade())
		{
			return;
		}
		this.ExecuteTrade();
		tradeModule.ExecuteTrade();
	}

	// Token: 0x060017AB RID: 6059 RVA: 0x00078113 File Offset: 0x00076313
	protected static void InvokeUserCode_CmdConfirmTrade(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdConfirmTrade called on client.");
			return;
		}
		((TradeModule)obj).UserCode_CmdConfirmTrade();
	}

	// Token: 0x060017AC RID: 6060 RVA: 0x00078136 File Offset: 0x00076336
	protected void UserCode_CmdCancelTrade()
	{
		this.EndTradeForBothPlayers();
	}

	// Token: 0x060017AD RID: 6061 RVA: 0x0007813E File Offset: 0x0007633E
	protected static void InvokeUserCode_CmdCancelTrade(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdCancelTrade called on client.");
			return;
		}
		((TradeModule)obj).UserCode_CmdCancelTrade();
	}

	// Token: 0x060017AE RID: 6062 RVA: 0x00078164 File Offset: 0x00076364
	static TradeModule()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(TradeModule), "System.Void TradeModule::CmdBeginHandshake()", new RemoteCallDelegate(TradeModule.InvokeUserCode_CmdBeginHandshake), true);
		RemoteProcedureCalls.RegisterCommand(typeof(TradeModule), "System.Void TradeModule::CmdAcceptTrade(UnityEngine.GameObject)", new RemoteCallDelegate(TradeModule.InvokeUserCode_CmdAcceptTrade__GameObject), true);
		RemoteProcedureCalls.RegisterCommand(typeof(TradeModule), "System.Void TradeModule::CmdRejectTrade(UnityEngine.GameObject)", new RemoteCallDelegate(TradeModule.InvokeUserCode_CmdRejectTrade__GameObject), true);
		RemoteProcedureCalls.RegisterCommand(typeof(TradeModule), "System.Void TradeModule::CmdAddItem(Item)", new RemoteCallDelegate(TradeModule.InvokeUserCode_CmdAddItem__Item), true);
		RemoteProcedureCalls.RegisterCommand(typeof(TradeModule), "System.Void TradeModule::CmdAddGold(System.Int64)", new RemoteCallDelegate(TradeModule.InvokeUserCode_CmdAddGold__Int64), true);
		RemoteProcedureCalls.RegisterCommand(typeof(TradeModule), "System.Void TradeModule::CmdConfirmTrade()", new RemoteCallDelegate(TradeModule.InvokeUserCode_CmdConfirmTrade), true);
		RemoteProcedureCalls.RegisterCommand(typeof(TradeModule), "System.Void TradeModule::CmdCancelTrade()", new RemoteCallDelegate(TradeModule.InvokeUserCode_CmdCancelTrade), true);
		RemoteProcedureCalls.RegisterRpc(typeof(TradeModule), "System.Void TradeModule::TargetSendHandshake(UnityEngine.GameObject)", new RemoteCallDelegate(TradeModule.InvokeUserCode_TargetSendHandshake__GameObject));
		RemoteProcedureCalls.RegisterRpc(typeof(TradeModule), "System.Void TradeModule::TargetOpenTradeWindow()", new RemoteCallDelegate(TradeModule.InvokeUserCode_TargetOpenTradeWindow));
		RemoteProcedureCalls.RegisterRpc(typeof(TradeModule), "System.Void TradeModule::TargetCloseTradeWindow()", new RemoteCallDelegate(TradeModule.InvokeUserCode_TargetCloseTradeWindow));
	}

	// Token: 0x060017AF RID: 6063 RVA: 0x000782B8 File Offset: 0x000764B8
	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteGameObject(this.NetworkCurrentTrader);
			writer.WriteLong(this.TradingGoldCoins);
			writer.WriteBool(this.TradeConfirmed);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1UL) != 0UL)
		{
			writer.WriteGameObject(this.NetworkCurrentTrader);
		}
		if ((base.syncVarDirtyBits & 2UL) != 0UL)
		{
			writer.WriteLong(this.TradingGoldCoins);
		}
		if ((base.syncVarDirtyBits & 4UL) != 0UL)
		{
			writer.WriteBool(this.TradeConfirmed);
		}
	}

	// Token: 0x060017B0 RID: 6064 RVA: 0x0007836C File Offset: 0x0007656C
	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			base.GeneratedSyncVarDeserialize_GameObject(ref this.CurrentTrader, null, reader, ref this.___CurrentTraderNetId);
			base.GeneratedSyncVarDeserialize<long>(ref this.TradingGoldCoins, new Action<long, long>(this.OnTradingGoldCoins), reader.ReadLong());
			base.GeneratedSyncVarDeserialize<bool>(ref this.TradeConfirmed, new Action<bool, bool>(this.OnTradeConfirmed), reader.ReadBool());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 1L) != 0L)
		{
			base.GeneratedSyncVarDeserialize_GameObject(ref this.CurrentTrader, null, reader, ref this.___CurrentTraderNetId);
		}
		if ((num & 2L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<long>(ref this.TradingGoldCoins, new Action<long, long>(this.OnTradingGoldCoins), reader.ReadLong());
		}
		if ((num & 4L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<bool>(ref this.TradeConfirmed, new Action<bool, bool>(this.OnTradeConfirmed), reader.ReadBool());
		}
	}

	// Token: 0x040014E8 RID: 5352
	[SyncVar]
	public GameObject CurrentTrader;

	// Token: 0x040014E9 RID: 5353
	[SyncVar(hook = "OnTradingGoldCoins")]
	public long TradingGoldCoins;

	// Token: 0x040014EA RID: 5354
	[SyncVar(hook = "OnTradeConfirmed")]
	public bool TradeConfirmed;

	// Token: 0x040014EB RID: 5355
	public readonly SyncListItem TradingItems = new SyncListItem();

	// Token: 0x040014EC RID: 5356
	private CombatModule combatModule;

	// Token: 0x040014ED RID: 5357
	private EffectModule effectModule;

	// Token: 0x040014EE RID: 5358
	private WalletModule walletModule;

	// Token: 0x040014EF RID: 5359
	private CreatureModule creatureModule;

	// Token: 0x040014F0 RID: 5360
	private UISystemModule uiSystemModule;

	// Token: 0x040014F1 RID: 5361
	private InventoryModule inventoryModule;

	// Token: 0x040014F3 RID: 5363
	protected uint ___CurrentTraderNetId;
}
