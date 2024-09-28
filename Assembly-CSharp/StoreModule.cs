using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

// Token: 0x02000423 RID: 1059
public class StoreModule : NetworkBehaviour
{
	// Token: 0x14000026 RID: 38
	// (add) Token: 0x06001712 RID: 5906 RVA: 0x00075BA4 File Offset: 0x00073DA4
	// (remove) Token: 0x06001713 RID: 5907 RVA: 0x00075BDC File Offset: 0x00073DDC
	public event StoreModule.OnItemRepurchasedEventHandler OnItemRepurchased;

	// Token: 0x14000027 RID: 39
	// (add) Token: 0x06001714 RID: 5908 RVA: 0x00075C14 File Offset: 0x00073E14
	// (remove) Token: 0x06001715 RID: 5909 RVA: 0x00075C4C File Offset: 0x00073E4C
	public event StoreModule.OnItemBoughtEventHandler OnItemBought;

	// Token: 0x14000028 RID: 40
	// (add) Token: 0x06001716 RID: 5910 RVA: 0x00075C84 File Offset: 0x00073E84
	// (remove) Token: 0x06001717 RID: 5911 RVA: 0x00075CBC File Offset: 0x00073EBC
	public event StoreModule.OnItemSoldEventHandler OnItemSold;

	// Token: 0x06001718 RID: 5912 RVA: 0x00075CF4 File Offset: 0x00073EF4
	private void Awake()
	{
		this.creatureModule = base.GetComponent<CreatureModule>();
		this.effectModule = base.GetComponent<EffectModule>();
		this.inventoryModule = base.GetComponent<InventoryModule>();
		this.walletModule = base.GetComponent<WalletModule>();
		if (NetworkServer.active)
		{
			GameObject.FindGameObjectWithTag("DatabaseManager").TryGetComponent<ItemDatabaseModule>(out this.itemDatabaseModule);
		}
	}

	// Token: 0x06001719 RID: 5913 RVA: 0x00075D50 File Offset: 0x00073F50
	[Command]
	public void CmdRepurchaseItem(int npcId, string itemUniqueId, int buyAmount)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(npcId);
		writer.WriteString(itemUniqueId);
		writer.WriteInt(buyAmount);
		base.SendCommandInternal("System.Void StoreModule::CmdRepurchaseItem(System.Int32,System.String,System.Int32)", 428568497, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600171A RID: 5914 RVA: 0x00075DA0 File Offset: 0x00073FA0
	[Command]
	public void CmdBuyItem(int npcId, string itemUniqueId, int buyAmount)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(npcId);
		writer.WriteString(itemUniqueId);
		writer.WriteInt(buyAmount);
		base.SendCommandInternal("System.Void StoreModule::CmdBuyItem(System.Int32,System.String,System.Int32)", -1571078367, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600171B RID: 5915 RVA: 0x00075DF0 File Offset: 0x00073FF0
	[Command]
	public void CmdSellItem(string itemUniqueId, int sellAmount)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(itemUniqueId);
		writer.WriteInt(sellAmount);
		base.SendCommandInternal("System.Void StoreModule::CmdSellItem(System.String,System.Int32)", -668739572, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600171D RID: 5917 RVA: 0x0002FEC9 File Offset: 0x0002E0C9
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x0600171E RID: 5918 RVA: 0x00075E48 File Offset: 0x00074048
	protected void UserCode_CmdRepurchaseItem__Int32__String__Int32(int npcId, string itemUniqueId, int buyAmount)
	{
		if (buyAmount < 1)
		{
			return;
		}
		StoreItemConfig storeItem = this.itemDatabaseModule.GetStoreItem(npcId, itemUniqueId);
		if (!storeItem.Item.IsDefined)
		{
			return;
		}
		int num = (storeItem.ItemValue >= 0) ? storeItem.ItemValue : storeItem.Item.Value;
		int num2 = buyAmount * num;
		if (this.walletModule.GoldCoins < (long)num2)
		{
			this.effectModule.ShowScreenMessage("store_not_enough_gold_coins_message", 0, 3.5f, Array.Empty<string>());
			return;
		}
		if (!this.inventoryModule.HasFreeSlots(storeItem.Item, buyAmount))
		{
			this.effectModule.ShowScreenMessage("inventory_full_message", 0, 3.5f, Array.Empty<string>());
			return;
		}
		if (!this.SoldItems.Any((Item si) => si.IsEqual(storeItem.Item)))
		{
			return;
		}
		if ((from si in this.SoldItems
		where si.IsEqual(storeItem.Item)
		select si).Sum((Item si) => si.Amount) < buyAmount)
		{
			return;
		}
		for (int i = 0; i < this.SoldItems.Count; i++)
		{
			if (this.SoldItems[i].IsEqual(storeItem.Item))
			{
				if (this.SoldItems[i].Amount <= buyAmount)
				{
					this.SoldItems.RemoveAt(i);
					break;
				}
				Item value = this.SoldItems[i];
				value.Amount -= buyAmount;
				this.SoldItems[i] = value;
			}
		}
		this.walletModule.AddGoldCoins(-num2);
		this.inventoryModule.AddItem(storeItem.Item, -1, buyAmount, true);
		StoreModule.OnItemRepurchasedEventHandler onItemRepurchased = this.OnItemRepurchased;
		if (onItemRepurchased == null)
		{
			return;
		}
		onItemRepurchased(storeItem.Item.Id, buyAmount, num);
	}

	// Token: 0x0600171F RID: 5919 RVA: 0x00076039 File Offset: 0x00074239
	protected static void InvokeUserCode_CmdRepurchaseItem__Int32__String__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdRepurchaseItem called on client.");
			return;
		}
		((StoreModule)obj).UserCode_CmdRepurchaseItem__Int32__String__Int32(reader.ReadInt(), reader.ReadString(), reader.ReadInt());
	}

	// Token: 0x06001720 RID: 5920 RVA: 0x00076070 File Offset: 0x00074270
	protected void UserCode_CmdBuyItem__Int32__String__Int32(int npcId, string itemUniqueId, int buyAmount)
	{
		if (buyAmount < 1)
		{
			return;
		}
		StoreItemConfig storeItem = this.itemDatabaseModule.GetStoreItem(npcId, itemUniqueId);
		if (!storeItem.Item.IsDefined)
		{
			return;
		}
		int num = (storeItem.ItemValue >= 0) ? storeItem.ItemValue : (storeItem.Item.Value * GlobalSettings.StoreItemPriceMultiplier);
		int num2 = buyAmount * num;
		if (this.walletModule.GoldCoins < (long)num2)
		{
			this.effectModule.ShowScreenMessage("store_not_enough_gold_coins_message", 0, 3.5f, Array.Empty<string>());
			return;
		}
		if (!this.inventoryModule.HasFreeSlots(storeItem.Item, buyAmount))
		{
			this.effectModule.ShowScreenMessage("inventory_full_message", 0, 3.5f, Array.Empty<string>());
			return;
		}
		this.walletModule.AddGoldCoins(-num2);
		this.inventoryModule.AddItem(storeItem.Item, -1, buyAmount, true);
		StoreModule.OnItemBoughtEventHandler onItemBought = this.OnItemBought;
		if (onItemBought == null)
		{
			return;
		}
		onItemBought(storeItem.Item.Id, buyAmount, num);
	}

	// Token: 0x06001721 RID: 5921 RVA: 0x00076165 File Offset: 0x00074365
	protected static void InvokeUserCode_CmdBuyItem__Int32__String__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdBuyItem called on client.");
			return;
		}
		((StoreModule)obj).UserCode_CmdBuyItem__Int32__String__Int32(reader.ReadInt(), reader.ReadString(), reader.ReadInt());
	}

	// Token: 0x06001722 RID: 5922 RVA: 0x0007619C File Offset: 0x0007439C
	protected void UserCode_CmdSellItem__String__Int32(string itemUniqueId, int sellAmount)
	{
		if (sellAmount < 1)
		{
			return;
		}
		if (!this.inventoryModule.ContainsItem(itemUniqueId, sellAmount))
		{
			this.effectModule.ShowScreenMessage("store_not_enough_items_message", 0, 3.5f, Array.Empty<string>());
			return;
		}
		Item item = this.inventoryModule.GetItem(itemUniqueId);
		if (!item.IsDefined)
		{
			return;
		}
		int value = item.Value;
		int amount = sellAmount * item.Value;
		this.inventoryModule.ConsumeItem(item, sellAmount);
		this.walletModule.AddGoldCoins(amount);
		item.Amount = sellAmount;
		if (this.SoldItems.Count >= 9)
		{
			this.SoldItems.RemoveAt(0);
			this.SoldItems.Add(item);
		}
		else
		{
			this.SoldItems.Add(item);
		}
		base.StartCoroutine(DiscordWebhookManager.SendInGameCommandsChannel(string.Format("{0} Sold to NPC: {1}x {2}(+{3}) - {4}", new object[]
		{
			this.creatureModule.OriginalCreatureName,
			item.Amount,
			item.Name,
			item.BoostLevel,
			item.Rarity
		})));
		StoreModule.OnItemSoldEventHandler onItemSold = this.OnItemSold;
		if (onItemSold == null)
		{
			return;
		}
		onItemSold(itemUniqueId, sellAmount, value);
	}

	// Token: 0x06001723 RID: 5923 RVA: 0x000762C9 File Offset: 0x000744C9
	protected static void InvokeUserCode_CmdSellItem__String__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSellItem called on client.");
			return;
		}
		((StoreModule)obj).UserCode_CmdSellItem__String__Int32(reader.ReadString(), reader.ReadInt());
	}

	// Token: 0x06001724 RID: 5924 RVA: 0x000762F8 File Offset: 0x000744F8
	static StoreModule()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(StoreModule), "System.Void StoreModule::CmdRepurchaseItem(System.Int32,System.String,System.Int32)", new RemoteCallDelegate(StoreModule.InvokeUserCode_CmdRepurchaseItem__Int32__String__Int32), true);
		RemoteProcedureCalls.RegisterCommand(typeof(StoreModule), "System.Void StoreModule::CmdBuyItem(System.Int32,System.String,System.Int32)", new RemoteCallDelegate(StoreModule.InvokeUserCode_CmdBuyItem__Int32__String__Int32), true);
		RemoteProcedureCalls.RegisterCommand(typeof(StoreModule), "System.Void StoreModule::CmdSellItem(System.String,System.Int32)", new RemoteCallDelegate(StoreModule.InvokeUserCode_CmdSellItem__String__Int32), true);
	}

	// Token: 0x040014A1 RID: 5281
	private CreatureModule creatureModule;

	// Token: 0x040014A2 RID: 5282
	private EffectModule effectModule;

	// Token: 0x040014A3 RID: 5283
	private InventoryModule inventoryModule;

	// Token: 0x040014A4 RID: 5284
	private WalletModule walletModule;

	// Token: 0x040014A5 RID: 5285
	private ItemDatabaseModule itemDatabaseModule;

	// Token: 0x040014A6 RID: 5286
	public readonly List<Item> SoldItems = new List<Item>();

	// Token: 0x02000424 RID: 1060
	// (Invoke) Token: 0x06001726 RID: 5926
	public delegate void OnItemRepurchasedEventHandler(int itemId, int buyAmount, int unitValue);

	// Token: 0x02000425 RID: 1061
	// (Invoke) Token: 0x0600172A RID: 5930
	public delegate void OnItemBoughtEventHandler(int itemId, int buyAmount, int unitValue);

	// Token: 0x02000426 RID: 1062
	// (Invoke) Token: 0x0600172E RID: 5934
	public delegate void OnItemSoldEventHandler(string itemUniqueId, int sellAmount, int unitValue);
}
