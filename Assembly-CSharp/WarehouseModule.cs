using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

// Token: 0x0200043E RID: 1086
public class WarehouseModule : NetworkBehaviour
{
	// Token: 0x1700026E RID: 622
	// (get) Token: 0x06001858 RID: 6232 RVA: 0x0007AC83 File Offset: 0x00078E83
	// (set) Token: 0x06001857 RID: 6231 RVA: 0x0007AC7A File Offset: 0x00078E7A
	public int TotalSlots { get; private set; }

	// Token: 0x1400002A RID: 42
	// (add) Token: 0x06001859 RID: 6233 RVA: 0x0007AC8C File Offset: 0x00078E8C
	// (remove) Token: 0x0600185A RID: 6234 RVA: 0x0007ACC4 File Offset: 0x00078EC4
	public event WarehouseModule.OnItemAddedEventHandler OnItemAdded;

	// Token: 0x1400002B RID: 43
	// (add) Token: 0x0600185B RID: 6235 RVA: 0x0007ACFC File Offset: 0x00078EFC
	// (remove) Token: 0x0600185C RID: 6236 RVA: 0x0007AD34 File Offset: 0x00078F34
	public event WarehouseModule.OnAmountAddedEventHandler OnAmountAdded;

	// Token: 0x1400002C RID: 44
	// (add) Token: 0x0600185D RID: 6237 RVA: 0x0007AD6C File Offset: 0x00078F6C
	// (remove) Token: 0x0600185E RID: 6238 RVA: 0x0007ADA4 File Offset: 0x00078FA4
	public event WarehouseModule.OnPositionSwapedEventHandler OnPositionSwaped;

	// Token: 0x1400002D RID: 45
	// (add) Token: 0x0600185F RID: 6239 RVA: 0x0007ADDC File Offset: 0x00078FDC
	// (remove) Token: 0x06001860 RID: 6240 RVA: 0x0007AE14 File Offset: 0x00079014
	public event WarehouseModule.OnSlotAddedEventHandler OnSlotAdded;

	// Token: 0x1700026F RID: 623
	// (get) Token: 0x06001861 RID: 6241 RVA: 0x0007AE49 File Offset: 0x00079049
	public bool HasFreeSlots
	{
		get
		{
			return this.Items.Any((Item c) => !c.IsDefined);
		}
	}

	// Token: 0x17000270 RID: 624
	// (get) Token: 0x06001862 RID: 6242 RVA: 0x0007AE75 File Offset: 0x00079075
	public int FreeSlots
	{
		get
		{
			return this.Items.Count((Item c) => !c.IsDefined);
		}
	}

	// Token: 0x06001863 RID: 6243 RVA: 0x0007AEA4 File Offset: 0x000790A4
	private void Awake()
	{
		if (NetworkClient.active)
		{
			GameObject.FindGameObjectWithTag("UISystem").TryGetComponent<UISystemModule>(out this.uiSystemModule);
		}
		if (NetworkServer.active)
		{
			GameObject.FindGameObjectWithTag("DatabaseManager").TryGetComponent<PlayerDatabaseModule>(out this.playerDatabaseModule);
		}
		base.TryGetComponent<PetModule>(out this.petModule);
		base.TryGetComponent<AreaModule>(out this.areaModule);
		base.TryGetComponent<PlayerModule>(out this.playerModule);
		base.TryGetComponent<EffectModule>(out this.effectModule);
		base.TryGetComponent<WalletModule>(out this.walletModule);
		base.TryGetComponent<InventoryModule>(out this.inventoryModule);
	}

	// Token: 0x06001864 RID: 6244 RVA: 0x0007AF39 File Offset: 0x00079139
	public override void OnStartLocalPlayer()
	{
		this.Items.Callback += this.OnItemsUpdatedAtLocalPlayer;
		this.BuildSlots();
	}

	// Token: 0x06001865 RID: 6245 RVA: 0x0007AF58 File Offset: 0x00079158
	public override void OnStartServer()
	{
		this.Items.Callback += delegate(SyncList<Item>.Operation op, int index, Item oldItem, Item newItem)
		{
			this.ShouldPersistData = true;
		};
	}

	// Token: 0x06001866 RID: 6246 RVA: 0x0007AF74 File Offset: 0x00079174
	private void OnItemsUpdatedAtLocalPlayer(SyncList<Item>.Operation op, int itemIndex, Item oldItem, Item newItem)
	{
		if (op == SyncList<Item>.Operation.OP_ADD)
		{
			if (this.Slots.Count < this.Items.Count)
			{
				this.CreateSlot().name = string.Format("Slot ({0})", itemIndex);
			}
			WarehouseSlotManager component = this.Slots[itemIndex].GetComponent<WarehouseSlotManager>();
			component.SlotPosition = itemIndex;
			component.Item = this.Items[itemIndex];
			return;
		}
		if (op == SyncList<Item>.Operation.OP_SET)
		{
			this.Slots[itemIndex].GetComponent<WarehouseSlotManager>().Item = this.Items[itemIndex];
		}
	}

	// Token: 0x06001867 RID: 6247 RVA: 0x0007B008 File Offset: 0x00079208
	private void BuildSlots()
	{
		for (int i = 0; i < this.Items.Count; i++)
		{
			if (this.Slots.Count < this.Items.Count)
			{
				this.CreateSlot().name = string.Format("Slot ({0})", i);
			}
			WarehouseSlotManager component = this.Slots[i].GetComponent<WarehouseSlotManager>();
			component.SlotPosition = i;
			component.Item = this.Items[i];
		}
	}

	// Token: 0x06001868 RID: 6248 RVA: 0x0007B088 File Offset: 0x00079288
	[Command]
	public void CmdIncreaseWarehouse()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		base.SendCommandInternal("System.Void WarehouseModule::CmdIncreaseWarehouse()", -1055346061, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001869 RID: 6249 RVA: 0x0007B0B8 File Offset: 0x000792B8
	private bool IncreaseWarehouseBasicSlot()
	{
		if (this.TotalSlots >= 35)
		{
			return false;
		}
		if (this.walletModule.GoldCoins < 4000L)
		{
			this.effectModule.ShowScreenMessage("not_enough_money_message", 3, 3.5f, Array.Empty<string>());
			return false;
		}
		this.walletModule.AddGoldCoins(-4000);
		this.AddSlots(1, true);
		this.effectModule.ShowAnimatedText("+1 Slot", 0, true, base.transform.position);
		return true;
	}

	// Token: 0x0600186A RID: 6250 RVA: 0x0007B138 File Offset: 0x00079338
	private Task<bool> IncreaseWarehouseGemSlotAsync()
	{
		WarehouseModule.<IncreaseWarehouseGemSlotAsync>d__43 <IncreaseWarehouseGemSlotAsync>d__;
		<IncreaseWarehouseGemSlotAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<IncreaseWarehouseGemSlotAsync>d__.<>4__this = this;
		<IncreaseWarehouseGemSlotAsync>d__.<>1__state = -1;
		<IncreaseWarehouseGemSlotAsync>d__.<>t__builder.Start<WarehouseModule.<IncreaseWarehouseGemSlotAsync>d__43>(ref <IncreaseWarehouseGemSlotAsync>d__);
		return <IncreaseWarehouseGemSlotAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600186B RID: 6251 RVA: 0x0007B17C File Offset: 0x0007937C
	[Server]
	public void InitializeSlots(int totalSlots, Item[] warehouseItems)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void WarehouseModule::InitializeSlots(System.Int32,Item[])' called when server was not active");
			return;
		}
		this.TotalSlots = totalSlots;
		this.TotalSlots = Mathf.Min(this.TotalSlots, 450);
		int i;
		int j;
		for (i = 0; i < this.TotalSlots; i = j + 1)
		{
			Item item = warehouseItems.FirstOrDefault((Item f) => f.SlotPosition == i);
			if (item.IsDefined)
			{
				this.Items.Add(item);
			}
			else
			{
				this.Items.Add(new Item(i));
			}
			j = i;
		}
	}

	// Token: 0x0600186C RID: 6252 RVA: 0x0007B228 File Offset: 0x00079428
	[Client]
	private GameObject CreateSlot()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'UnityEngine.GameObject WarehouseModule::CreateSlot()' called when client was not active");
			return null;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.warehouseSlotPrefab);
		gameObject.transform.SetParent(this.uiSystemModule.WarehouseHolder.transform, false);
		gameObject.transform.position = Vector2.zero;
		this.Slots.Add(gameObject);
		return gameObject;
	}

	// Token: 0x0600186D RID: 6253 RVA: 0x0007B2A0 File Offset: 0x000794A0
	[Server]
	public void AddSlots(int amount, bool invokeEvents)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void WarehouseModule::AddSlots(System.Int32,System.Boolean)' called when server was not active");
			return;
		}
		amount = Mathf.Max(0, amount);
		int totalSlots = this.TotalSlots;
		this.TotalSlots += amount;
		this.TotalSlots = Mathf.Min(this.TotalSlots, 450);
		for (int i = totalSlots; i < this.TotalSlots; i++)
		{
			this.Items.Add(new Item(i));
		}
		if (invokeEvents && this.OnSlotAdded != null)
		{
			this.OnSlotAdded(amount, this.TotalSlots);
		}
	}

	// Token: 0x0600186E RID: 6254 RVA: 0x0007B333 File Offset: 0x00079533
	public bool HasItemOnSlot(int slotPosition)
	{
		return this.HasItemOnSlot(slotPosition, 1);
	}

	// Token: 0x0600186F RID: 6255 RVA: 0x0007B340 File Offset: 0x00079540
	public bool HasItemOnSlot(int slotPosition, int amount)
	{
		if (slotPosition < 0)
		{
			return false;
		}
		Item item = this.Items[slotPosition];
		return item.IsDefined && item.Amount >= amount;
	}

	// Token: 0x06001870 RID: 6256 RVA: 0x0007B378 File Offset: 0x00079578
	[Server]
	public void AddAmount(int slotPosition, int amount, bool invokeEvents)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void WarehouseModule::AddAmount(System.Int32,System.Int32,System.Boolean)' called when server was not active");
			return;
		}
		if (amount < 0 & this.Items[slotPosition].Amount <= 1)
		{
			this.Items[slotPosition] = default(Item);
		}
		else
		{
			Item item = this.Items[slotPosition];
			if (item.Amount + amount > 750)
			{
				item.Amount = 750;
			}
			else
			{
				item.Amount += amount;
			}
			if (item.Amount > 0)
			{
				this.Items[slotPosition] = item;
			}
			else
			{
				this.Items[slotPosition] = default(Item);
			}
		}
		if (invokeEvents && this.OnAmountAdded != null)
		{
			this.OnAmountAdded(amount, slotPosition, this.Items[slotPosition]);
		}
	}

	// Token: 0x06001871 RID: 6257 RVA: 0x0007B458 File Offset: 0x00079658
	[Command]
	public void CmdSortItems()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		base.SendCommandInternal("System.Void WarehouseModule::CmdSortItems()", 1546804564, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001872 RID: 6258 RVA: 0x0007B488 File Offset: 0x00079688
	[Command]
	public void CmdAddAmountFromSlot(int originSlotPosition, int destinationSlotPosition, int amount, bool invokeEvents)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(originSlotPosition);
		writer.WriteInt(destinationSlotPosition);
		writer.WriteInt(amount);
		writer.WriteBool(invokeEvents);
		base.SendCommandInternal("System.Void WarehouseModule::CmdAddAmountFromSlot(System.Int32,System.Int32,System.Int32,System.Boolean)", -994727495, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001873 RID: 6259 RVA: 0x0007B4E0 File Offset: 0x000796E0
	[Server]
	public void AddItem(Item item, int slotPosition, int amount, bool invokeEvents)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void WarehouseModule::AddItem(Item,System.Int32,System.Int32,System.Boolean)' called when server was not active");
			return;
		}
		if (item.Soulbind && item.OwnerId == 0)
		{
			Player player = this.playerDatabaseModule.GetPlayer(this.playerModule.PlayerId);
			if (player.IsDefined)
			{
				item.OwnerId = player.Id;
				item.OwnerName = player.Name;
			}
		}
		if (item.IsDefined && slotPosition != -1)
		{
			item.SlotPosition = slotPosition;
			item.Amount = amount;
			this.Items[slotPosition] = item;
			if (invokeEvents && this.OnItemAdded != null)
			{
				this.OnItemAdded(item, slotPosition);
				return;
			}
		}
		else if (item.IsDefined)
		{
			int num = amount;
			for (int i = 0; i < this.Items.Count; i++)
			{
				Item item2 = this.Items[i];
				if (item.Stackable & item2.OwnerId == item.OwnerId & item2.UniqueId == item.UniqueId & item2.Amount + num <= 750)
				{
					this.AddAmount(i, num, invokeEvents);
					num = 0;
					break;
				}
				if (item.Stackable & item2.OwnerId == item.OwnerId & item2.UniqueId == item.UniqueId & item2.Amount + num > 750)
				{
					int num2 = 750 - item2.Amount;
					num -= num2;
					this.AddAmount(i, num2, invokeEvents);
				}
				else if (!item2.IsDefined)
				{
					item.SlotPosition = i;
					slotPosition = i;
					if (item.Stackable)
					{
						if (num > 750)
						{
							int num3 = 750 - num;
							item.Amount = num3;
							num -= num3;
						}
						else
						{
							item.Amount = num;
							num = 0;
						}
					}
					else
					{
						item.Amount = 1;
						num--;
					}
					this.Items[i] = item;
					if (invokeEvents && this.OnItemAdded != null)
					{
						this.OnItemAdded(item, slotPosition);
					}
					if (num < 1)
					{
						break;
					}
				}
			}
			if (num > 0)
			{
				item.Amount = num;
				this.inventoryModule.AddItem(item, -1, item.Amount, invokeEvents);
			}
		}
	}

	// Token: 0x06001874 RID: 6260 RVA: 0x0007B718 File Offset: 0x00079918
	[Server]
	public void RemoveItem(int slotPosition)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void WarehouseModule::RemoveItem(System.Int32)' called when server was not active");
			return;
		}
		this.Items[slotPosition] = default(Item);
	}

	// Token: 0x06001875 RID: 6261 RVA: 0x0007B750 File Offset: 0x00079950
	[Command]
	public void CmdSwapPosition(int slotPosition, int draggingPosition)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(slotPosition);
		writer.WriteInt(draggingPosition);
		base.SendCommandInternal("System.Void WarehouseModule::CmdSwapPosition(System.Int32,System.Int32)", 260818042, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001876 RID: 6262 RVA: 0x0007B794 File Offset: 0x00079994
	[Server]
	public void SwapPosition(int slotPosition, int draggingPosition)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void WarehouseModule::SwapPosition(System.Int32,System.Int32)' called when server was not active");
			return;
		}
		Item value = this.Items[slotPosition];
		value.SlotPosition = draggingPosition;
		Item value2 = this.Items[draggingPosition];
		value2.SlotPosition = slotPosition;
		this.Items[slotPosition] = value2;
		this.Items[draggingPosition] = value;
		WarehouseModule.OnPositionSwapedEventHandler onPositionSwaped = this.OnPositionSwaped;
		if (onPositionSwaped == null)
		{
			return;
		}
		onPositionSwaped(this.Items[slotPosition], this.Items[draggingPosition]);
	}

	// Token: 0x06001877 RID: 6263 RVA: 0x0007B824 File Offset: 0x00079A24
	[Command]
	public void CmdSendToInventory(int slotPosition, int inventorySlotPosition)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(slotPosition);
		writer.WriteInt(inventorySlotPosition);
		base.SendCommandInternal("System.Void WarehouseModule::CmdSendToInventory(System.Int32,System.Int32)", 2020423419, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001878 RID: 6264 RVA: 0x0007B868 File Offset: 0x00079A68
	[Server]
	public void SendToInventory(int slotPosition, int inventorySlotPosition)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void WarehouseModule::SendToInventory(System.Int32,System.Int32)' called when server was not active");
			return;
		}
		Item item = this.Items[slotPosition];
		if (item.Soulbind && item.OwnerId != this.playerModule.PlayerId)
		{
			this.effectModule.ShowScreenMessage("warehouse_item_owner_message", 0, 3.5f, new string[]
			{
				item.OwnerName
			});
			return;
		}
		if (!this.inventoryModule.HasFreeSlots(item, item.Amount))
		{
			this.effectModule.ShowScreenMessage("inventory_full_message", 0, 3.5f, Array.Empty<string>());
			return;
		}
		if (item.IsDefined)
		{
			this.RemoveItem(slotPosition);
			if (this.inventoryModule.HasItemOnSlot(inventorySlotPosition))
			{
				this.inventoryModule.AddItem(item, -1, item.Amount, true);
				return;
			}
			this.inventoryModule.AddItem(item, inventorySlotPosition, item.Amount, true);
		}
	}

	// Token: 0x06001879 RID: 6265 RVA: 0x0007B950 File Offset: 0x00079B50
	[Command]
	public void CmdDropItem(int slotPosition, Vector2 position)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(slotPosition);
		writer.WriteVector2(position);
		base.SendCommandInternal("System.Void WarehouseModule::CmdDropItem(System.Int32,UnityEngine.Vector2)", 672071991, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600187A RID: 6266 RVA: 0x0007B994 File Offset: 0x00079B94
	public WarehouseModule()
	{
		base.InitSyncObject(this.Items);
	}

	// Token: 0x0600187C RID: 6268 RVA: 0x0002FEC9 File Offset: 0x0002E0C9
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x0600187D RID: 6269 RVA: 0x0007B9C8 File Offset: 0x00079BC8
	protected void UserCode_CmdIncreaseWarehouse()
	{
		WarehouseModule.<CmdIncreaseWarehouse>d__41 <CmdIncreaseWarehouse>d__;
		<CmdIncreaseWarehouse>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CmdIncreaseWarehouse>d__.<>4__this = this;
		<CmdIncreaseWarehouse>d__.<>1__state = -1;
		<CmdIncreaseWarehouse>d__.<>t__builder.Start<WarehouseModule.<CmdIncreaseWarehouse>d__41>(ref <CmdIncreaseWarehouse>d__);
	}

	// Token: 0x0600187E RID: 6270 RVA: 0x0007B9FF File Offset: 0x00079BFF
	protected static void InvokeUserCode_CmdIncreaseWarehouse(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdIncreaseWarehouse called on client.");
			return;
		}
		((WarehouseModule)obj).UserCode_CmdIncreaseWarehouse();
	}

	// Token: 0x0600187F RID: 6271 RVA: 0x0007BA24 File Offset: 0x00079C24
	protected void UserCode_CmdSortItems()
	{
		Item[] array = (from i in this.Items
		where i.IsDefined
		orderby i.Type, i.SlotType, i.Id, i.Amount descending
		select i).ToArray<Item>();
		for (int j = 0; j < this.Items.Count; j++)
		{
			if (j < array.Length)
			{
				Item value = array[j];
				value.SlotPosition = j;
				this.Items[j] = value;
			}
			else
			{
				this.Items[j] = default(Item);
			}
		}
	}

	// Token: 0x06001880 RID: 6272 RVA: 0x0007BB41 File Offset: 0x00079D41
	protected static void InvokeUserCode_CmdSortItems(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSortItems called on client.");
			return;
		}
		((WarehouseModule)obj).UserCode_CmdSortItems();
	}

	// Token: 0x06001881 RID: 6273 RVA: 0x0007BB64 File Offset: 0x00079D64
	protected void UserCode_CmdAddAmountFromSlot__Int32__Int32__Int32__Boolean(int originSlotPosition, int destinationSlotPosition, int amount, bool invokeEvents)
	{
		if (this.petModule.PetItemId != 22 && this.areaModule.AreaType != AreaType.ProtectedArea)
		{
			return;
		}
		if (amount < 1)
		{
			return;
		}
		if (originSlotPosition == destinationSlotPosition)
		{
			return;
		}
		Item item = this.Items[originSlotPosition];
		Item item2 = this.Items[destinationSlotPosition];
		if (item2.IsDefined & item.Amount >= amount)
		{
			if (item.UniqueId != item2.UniqueId)
			{
				return;
			}
			if (item2.Amount + amount > 750)
			{
				amount = 750 - item2.Amount;
			}
			this.AddAmount(originSlotPosition, -amount, invokeEvents);
			this.AddAmount(destinationSlotPosition, amount, invokeEvents);
			return;
		}
		else
		{
			if (!item2.IsDefined & item.Amount >= amount)
			{
				this.AddAmount(originSlotPosition, -amount, invokeEvents);
				this.AddItem(item, destinationSlotPosition, amount, invokeEvents);
				return;
			}
			return;
		}
	}

	// Token: 0x06001882 RID: 6274 RVA: 0x0007BC41 File Offset: 0x00079E41
	protected static void InvokeUserCode_CmdAddAmountFromSlot__Int32__Int32__Int32__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdAddAmountFromSlot called on client.");
			return;
		}
		((WarehouseModule)obj).UserCode_CmdAddAmountFromSlot__Int32__Int32__Int32__Boolean(reader.ReadInt(), reader.ReadInt(), reader.ReadInt(), reader.ReadBool());
	}

	// Token: 0x06001883 RID: 6275 RVA: 0x0007BC7C File Offset: 0x00079E7C
	protected void UserCode_CmdSwapPosition__Int32__Int32(int slotPosition, int draggingPosition)
	{
		if (this.petModule.PetItemId != 22 && this.areaModule.AreaType != AreaType.ProtectedArea)
		{
			return;
		}
		this.SwapPosition(slotPosition, draggingPosition);
	}

	// Token: 0x06001884 RID: 6276 RVA: 0x0007BCA3 File Offset: 0x00079EA3
	protected static void InvokeUserCode_CmdSwapPosition__Int32__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSwapPosition called on client.");
			return;
		}
		((WarehouseModule)obj).UserCode_CmdSwapPosition__Int32__Int32(reader.ReadInt(), reader.ReadInt());
	}

	// Token: 0x06001885 RID: 6277 RVA: 0x0007BCD2 File Offset: 0x00079ED2
	protected void UserCode_CmdSendToInventory__Int32__Int32(int slotPosition, int inventorySlotPosition)
	{
		if (this.petModule.PetItemId != 22 && this.areaModule.AreaType != AreaType.ProtectedArea)
		{
			return;
		}
		this.SendToInventory(slotPosition, inventorySlotPosition);
	}

	// Token: 0x06001886 RID: 6278 RVA: 0x0007BCF9 File Offset: 0x00079EF9
	protected static void InvokeUserCode_CmdSendToInventory__Int32__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSendToInventory called on client.");
			return;
		}
		((WarehouseModule)obj).UserCode_CmdSendToInventory__Int32__Int32(reader.ReadInt(), reader.ReadInt());
	}

	// Token: 0x06001887 RID: 6279 RVA: 0x0007BD28 File Offset: 0x00079F28
	protected void UserCode_CmdDropItem__Int32__Vector2(int slotPosition, Vector2 position)
	{
		if (this.petModule.PetItemId != 22 && this.areaModule.AreaType != AreaType.ProtectedArea)
		{
			return;
		}
		Item item = this.Items[slotPosition];
		if (item.Soulbind)
		{
			this.effectModule.ShowScreenMessage("cant_drop_this_item_message", 0, 3.5f, Array.Empty<string>());
			return;
		}
		if (!GlobalUtils.IsClose(base.transform.position, position, 4f))
		{
			this.effectModule.ShowScreenMessage("equipment_cant_drop_items_that_far_message", 0, 3.5f, Array.Empty<string>());
			return;
		}
		if (item.IsDefined)
		{
			int layerMask = 1 << LayerMask.NameToLayer("Tile");
			if (Physics2D.OverlapPoint(position, layerMask) != null)
			{
				this.effectModule.ShowScreenMessage("equipment_cant_drop_items_there_message", 0, 3.5f, Array.Empty<string>());
				return;
			}
			this.RemoveItem(slotPosition);
			GlobalUtils.SpawnItemOnGround(item, null, position, 60f);
		}
	}

	// Token: 0x06001888 RID: 6280 RVA: 0x0007BE19 File Offset: 0x0007A019
	protected static void InvokeUserCode_CmdDropItem__Int32__Vector2(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdDropItem called on client.");
			return;
		}
		((WarehouseModule)obj).UserCode_CmdDropItem__Int32__Vector2(reader.ReadInt(), reader.ReadVector2());
	}

	// Token: 0x06001889 RID: 6281 RVA: 0x0007BE48 File Offset: 0x0007A048
	static WarehouseModule()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(WarehouseModule), "System.Void WarehouseModule::CmdIncreaseWarehouse()", new RemoteCallDelegate(WarehouseModule.InvokeUserCode_CmdIncreaseWarehouse), true);
		RemoteProcedureCalls.RegisterCommand(typeof(WarehouseModule), "System.Void WarehouseModule::CmdSortItems()", new RemoteCallDelegate(WarehouseModule.InvokeUserCode_CmdSortItems), true);
		RemoteProcedureCalls.RegisterCommand(typeof(WarehouseModule), "System.Void WarehouseModule::CmdAddAmountFromSlot(System.Int32,System.Int32,System.Int32,System.Boolean)", new RemoteCallDelegate(WarehouseModule.InvokeUserCode_CmdAddAmountFromSlot__Int32__Int32__Int32__Boolean), true);
		RemoteProcedureCalls.RegisterCommand(typeof(WarehouseModule), "System.Void WarehouseModule::CmdSwapPosition(System.Int32,System.Int32)", new RemoteCallDelegate(WarehouseModule.InvokeUserCode_CmdSwapPosition__Int32__Int32), true);
		RemoteProcedureCalls.RegisterCommand(typeof(WarehouseModule), "System.Void WarehouseModule::CmdSendToInventory(System.Int32,System.Int32)", new RemoteCallDelegate(WarehouseModule.InvokeUserCode_CmdSendToInventory__Int32__Int32), true);
		RemoteProcedureCalls.RegisterCommand(typeof(WarehouseModule), "System.Void WarehouseModule::CmdDropItem(System.Int32,UnityEngine.Vector2)", new RemoteCallDelegate(WarehouseModule.InvokeUserCode_CmdDropItem__Int32__Vector2), true);
	}

	// Token: 0x0400158A RID: 5514
	[SerializeField]
	private GameObject warehouseSlotPrefab;

	// Token: 0x0400158B RID: 5515
	public readonly SyncListItem Items = new SyncListItem();

	// Token: 0x0400158C RID: 5516
	public List<GameObject> Slots = new List<GameObject>();

	// Token: 0x0400158D RID: 5517
	private PetModule petModule;

	// Token: 0x0400158E RID: 5518
	private AreaModule areaModule;

	// Token: 0x0400158F RID: 5519
	private EffectModule effectModule;

	// Token: 0x04001590 RID: 5520
	private PlayerModule playerModule;

	// Token: 0x04001591 RID: 5521
	private WalletModule walletModule;

	// Token: 0x04001592 RID: 5522
	private UISystemModule uiSystemModule;

	// Token: 0x04001593 RID: 5523
	private InventoryModule inventoryModule;

	// Token: 0x04001594 RID: 5524
	private PlayerDatabaseModule playerDatabaseModule;

	// Token: 0x04001595 RID: 5525
	public bool ShouldPersistData;

	// Token: 0x0200043F RID: 1087
	// (Invoke) Token: 0x0600188B RID: 6283
	public delegate void OnAmountAddedEventHandler(int amount, int slotPosition, Item item);

	// Token: 0x02000440 RID: 1088
	// (Invoke) Token: 0x0600188F RID: 6287
	public delegate void OnItemAddedEventHandler(Item item, int slotPosition);

	// Token: 0x02000441 RID: 1089
	// (Invoke) Token: 0x06001893 RID: 6291
	public delegate void OnPositionSwapedEventHandler(Item draggedItem, Item replacedItem);

	// Token: 0x02000442 RID: 1090
	// (Invoke) Token: 0x06001897 RID: 6295
	public delegate void OnSlotAddedEventHandler(int amount, int totalSlots);
}
