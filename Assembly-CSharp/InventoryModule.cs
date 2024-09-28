using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

// Token: 0x0200034B RID: 843
public class InventoryModule : NetworkBehaviour
{
	// Token: 0x14000010 RID: 16
	// (add) Token: 0x06001086 RID: 4230 RVA: 0x0004E700 File Offset: 0x0004C900
	// (remove) Token: 0x06001087 RID: 4231 RVA: 0x0004E738 File Offset: 0x0004C938
	public event InventoryModule.OnItemAddedEventHandler OnItemAdded;

	// Token: 0x14000011 RID: 17
	// (add) Token: 0x06001088 RID: 4232 RVA: 0x0004E770 File Offset: 0x0004C970
	// (remove) Token: 0x06001089 RID: 4233 RVA: 0x0004E7A8 File Offset: 0x0004C9A8
	public event InventoryModule.OnAmountChangedEventHandler OnAmountAdded;

	// Token: 0x14000012 RID: 18
	// (add) Token: 0x0600108A RID: 4234 RVA: 0x0004E7E0 File Offset: 0x0004C9E0
	// (remove) Token: 0x0600108B RID: 4235 RVA: 0x0004E818 File Offset: 0x0004CA18
	public event InventoryModule.OnItemPositionSwapedEventHandler OnItemPositionSwaped;

	// Token: 0x14000013 RID: 19
	// (add) Token: 0x0600108C RID: 4236 RVA: 0x0004E850 File Offset: 0x0004CA50
	// (remove) Token: 0x0600108D RID: 4237 RVA: 0x0004E888 File Offset: 0x0004CA88
	public event InventoryModule.OnSlotAddedEventHandler OnSlotAdded;

	// Token: 0x14000014 RID: 20
	// (add) Token: 0x0600108E RID: 4238 RVA: 0x0004E8C0 File Offset: 0x0004CAC0
	// (remove) Token: 0x0600108F RID: 4239 RVA: 0x0004E8F8 File Offset: 0x0004CAF8
	public event InventoryModule.OnItemAddedToItemBarEventHandler OnItemAddedToItemBar;

	// Token: 0x14000015 RID: 21
	// (add) Token: 0x06001090 RID: 4240 RVA: 0x0004E930 File Offset: 0x0004CB30
	// (remove) Token: 0x06001091 RID: 4241 RVA: 0x0004E968 File Offset: 0x0004CB68
	public event InventoryModule.OnItemRemovedFromItemBarEventHandler OnItemRemovedFromItemBar;

	// Token: 0x14000016 RID: 22
	// (add) Token: 0x06001092 RID: 4242 RVA: 0x0004E9A0 File Offset: 0x0004CBA0
	// (remove) Token: 0x06001093 RID: 4243 RVA: 0x0004E9D8 File Offset: 0x0004CBD8
	public event InventoryModule.OnItemSwapItemBarSlotEventHandler OnItemSwapItemBarSlot;

	// Token: 0x170001D6 RID: 470
	// (get) Token: 0x06001095 RID: 4245 RVA: 0x0004EA16 File Offset: 0x0004CC16
	// (set) Token: 0x06001094 RID: 4244 RVA: 0x0004EA0D File Offset: 0x0004CC0D
	public int TotalSlots { get; private set; }

	// Token: 0x170001D7 RID: 471
	// (get) Token: 0x06001097 RID: 4247 RVA: 0x0004EA27 File Offset: 0x0004CC27
	// (set) Token: 0x06001096 RID: 4246 RVA: 0x0004EA1E File Offset: 0x0004CC1E
	public float ItemsUpdateTime { get; private set; }

	// Token: 0x170001D8 RID: 472
	// (get) Token: 0x06001098 RID: 4248 RVA: 0x0004EA2F File Offset: 0x0004CC2F
	public int EmptySlots
	{
		get
		{
			return this.Items.Count((Item c) => !c.IsDefined);
		}
	}

	// Token: 0x06001099 RID: 4249 RVA: 0x0004EA5C File Offset: 0x0004CC5C
	private void Awake()
	{
		if (NetworkClient.active)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
			this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
		}
		this.effectModule = base.GetComponent<EffectModule>();
		this.walletModule = base.GetComponent<WalletModule>();
		this.playerModule = base.GetComponent<PlayerModule>();
		this.creatureModule = base.GetComponent<CreatureModule>();
		this.equipmentModule = base.GetComponent<EquipmentModule>();
		this.warehouseModule = base.GetComponent<WarehouseModule>();
	}

	// Token: 0x0600109A RID: 4250 RVA: 0x0004EAD0 File Offset: 0x0004CCD0
	public override void OnStartLocalPlayer()
	{
		this.ItemsUpdateTime = Time.time;
		this.Items.Callback += this.OnItemsUpdated;
		if (!GlobalSettings.IsMobilePlatform)
		{
			this.ItemBar.Callback += this.OnItemBarUpdated;
			this.BuildItemBarSlots();
		}
		this.BuildInventorySlots();
	}

	// Token: 0x0600109B RID: 4251 RVA: 0x0004EB29 File Offset: 0x0004CD29
	public override void OnStartServer()
	{
		this.NetworkCollectorMinRarityFilter = Rarity.Common;
		this.NetworkCollectorMinQualityFilter = ItemQuality.Poor;
		this.NetworkCollectorTypeFilter = ItemType.Undefined;
		this.NetworkCollectorCategoryFilter = ItemCategory.Undefined;
		this.Items.Callback += delegate(SyncList<Item>.Operation op, int index, Item oldItem, Item newItem)
		{
			this.ShouldPersistData = true;
		};
	}

	// Token: 0x0600109C RID: 4252 RVA: 0x0004EB60 File Offset: 0x0004CD60
	private void OnItemBarUpdated(SyncList<Item>.Operation op, int itemIndex, Item oldItem, Item newItem)
	{
		if (op == SyncList<Item>.Operation.OP_ADD)
		{
			if (this.ItemBarSlots.Count < this.ItemBar.Count)
			{
				this.CreateItemBarSlot().name = string.Format("ItemBar Slot ({0})", itemIndex);
			}
			ItemBarSlotManager component = this.ItemBarSlots[itemIndex].GetComponent<ItemBarSlotManager>();
			component.SlotPosition = itemIndex;
			component.Item = this.ItemBar[itemIndex];
			return;
		}
		if (op == SyncList<Item>.Operation.OP_SET)
		{
			ItemBarSlotManager component2 = this.ItemBarSlots[itemIndex].GetComponent<ItemBarSlotManager>();
			component2.SlotPosition = itemIndex;
			component2.Item = this.ItemBar[itemIndex];
		}
	}

	// Token: 0x0600109D RID: 4253 RVA: 0x0004EBFC File Offset: 0x0004CDFC
	private void OnItemsUpdated(SyncList<Item>.Operation op, int itemIndex, Item oldItem, Item newItem)
	{
		if (op == SyncList<Item>.Operation.OP_ADD)
		{
			if (this.Slots.Count < this.Items.Count)
			{
				this.CreateSlot().name = string.Format("Slot ({0})", itemIndex);
			}
			InventorySlotManager component = this.Slots[itemIndex].GetComponent<InventorySlotManager>();
			component.SlotPosition = itemIndex;
			component.Item = this.Items[itemIndex];
		}
		else if (op == SyncList<Item>.Operation.OP_SET)
		{
			this.Slots[itemIndex].GetComponent<InventorySlotManager>().Item = this.Items[itemIndex];
		}
		this.ItemsUpdateTime = Time.time;
		foreach (GameObject gameObject in this.ItemBarSlots)
		{
			ItemBarSlotManager itemBarSlotManager;
			gameObject.TryGetComponent<ItemBarSlotManager>(out itemBarSlotManager);
			if (itemBarSlotManager.Item.IsDefined)
			{
				itemBarSlotManager.NeedsUpdate = true;
			}
		}
	}

	// Token: 0x0600109E RID: 4254 RVA: 0x0004ECF8 File Offset: 0x0004CEF8
	private void BuildInventorySlots()
	{
		for (int i = 0; i < this.Items.Count; i++)
		{
			if (this.Slots.Count < this.Items.Count)
			{
				this.CreateSlot().name = string.Format("Slot ({0})", i);
			}
			InventorySlotManager component = this.Slots[i].GetComponent<InventorySlotManager>();
			component.SlotPosition = i;
			component.Item = this.Items[i];
		}
	}

	// Token: 0x0600109F RID: 4255 RVA: 0x0004ED78 File Offset: 0x0004CF78
	private void BuildItemBarSlots()
	{
		for (int i = 0; i < this.ItemBar.Count; i++)
		{
			if (this.ItemBarSlots.Count < this.ItemBar.Count)
			{
				this.CreateItemBarSlot().name = string.Format("ItemBar Slot ({0})", i);
			}
			ItemBarSlotManager component = this.ItemBarSlots[i].GetComponent<ItemBarSlotManager>();
			component.SlotPosition = i;
			component.Item = this.ItemBar[i];
		}
	}

	// Token: 0x060010A0 RID: 4256 RVA: 0x0004EDF8 File Offset: 0x0004CFF8
	[Command]
	public void CmdIncreaseInventory()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		base.SendCommandInternal("System.Void InventoryModule::CmdIncreaseInventory()", -1637590107, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x060010A1 RID: 4257 RVA: 0x0004EE28 File Offset: 0x0004D028
	private bool IncreaseInventoryBasicSlot()
	{
		int maxInventorySlots = GlobalUtils.GetMaxInventorySlots(this.playerModule.PackageType);
		if (this.TotalSlots >= maxInventorySlots)
		{
			return false;
		}
		if (this.walletModule.GoldCoins < 2000L)
		{
			this.effectModule.ShowScreenMessage("not_enough_money_message", 3, 3.5f, Array.Empty<string>());
			return false;
		}
		this.walletModule.AddGoldCoins(-2000);
		this.AddSlots(1, true);
		this.effectModule.ShowAnimatedText("+1 Slot", 0, true, base.transform.position);
		return true;
	}

	// Token: 0x060010A2 RID: 4258 RVA: 0x0004EEB8 File Offset: 0x0004D0B8
	private Task<bool> IncreaseInventoryGemSlotAsync()
	{
		InventoryModule.<IncreaseInventoryGemSlotAsync>d__65 <IncreaseInventoryGemSlotAsync>d__;
		<IncreaseInventoryGemSlotAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<IncreaseInventoryGemSlotAsync>d__.<>4__this = this;
		<IncreaseInventoryGemSlotAsync>d__.<>1__state = -1;
		<IncreaseInventoryGemSlotAsync>d__.<>t__builder.Start<InventoryModule.<IncreaseInventoryGemSlotAsync>d__65>(ref <IncreaseInventoryGemSlotAsync>d__);
		return <IncreaseInventoryGemSlotAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060010A3 RID: 4259 RVA: 0x0004EEFC File Offset: 0x0004D0FC
	[Server]
	public void InitializeSlots(int totalSlots, Item[] items, Item[] itemBar)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void InventoryModule::InitializeSlots(System.Int32,Item[],Item[])' called when server was not active");
			return;
		}
		this.TotalSlots = totalSlots;
		this.TotalSlots = Mathf.Min(this.TotalSlots, 200);
		int k;
		int j;
		for (k = 0; k < this.TotalSlots; k = j + 1)
		{
			Item item = items.FirstOrDefault((Item f) => f.SlotPosition == k);
			if (item.IsDefined)
			{
				this.Items.Add(item);
			}
			else
			{
				this.Items.Add(new Item(k));
			}
			j = k;
		}
		int i;
		for (i = 0; i < 10; i = j + 1)
		{
			Item item2 = itemBar.FirstOrDefault((Item f) => f.SlotPosition == i);
			if (item2.IsDefined)
			{
				this.ItemBar.Add(item2);
			}
			else
			{
				this.ItemBar.Add(new Item(i));
			}
			j = i;
		}
	}

	// Token: 0x060010A4 RID: 4260 RVA: 0x0004F014 File Offset: 0x0004D214
	[Server]
	public void AddSlots(int amount, bool invokeEvents)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void InventoryModule::AddSlots(System.Int32,System.Boolean)' called when server was not active");
			return;
		}
		amount = Mathf.Max(0, amount);
		int totalSlots = this.TotalSlots;
		this.TotalSlots += amount;
		this.TotalSlots = Mathf.Min(this.TotalSlots, 200);
		for (int i = totalSlots; i < this.TotalSlots; i++)
		{
			this.Items.Add(new Item(i));
		}
		if (invokeEvents && this.OnSlotAdded != null)
		{
			this.OnSlotAdded(amount, this.TotalSlots);
		}
	}

	// Token: 0x060010A5 RID: 4261 RVA: 0x0004F0A8 File Offset: 0x0004D2A8
	[Client]
	private GameObject CreateSlot()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'UnityEngine.GameObject InventoryModule::CreateSlot()' called when client was not active");
			return null;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.inventorySlotPrefab);
		gameObject.transform.SetParent(this.uiSystemModule.InventoryHolder.transform, false);
		gameObject.transform.position = Vector2.zero;
		this.Slots.Add(gameObject);
		return gameObject;
	}

	// Token: 0x060010A6 RID: 4262 RVA: 0x0004F120 File Offset: 0x0004D320
	[Client]
	private GameObject CreateItemBarSlot()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'UnityEngine.GameObject InventoryModule::CreateItemBarSlot()' called when client was not active");
			return null;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.itemBarSlotPrefab);
		gameObject.transform.SetParent(this.uiSystemModule.ItemBarWindow.transform, false);
		gameObject.transform.position = Vector2.zero;
		this.ItemBarSlots.Add(gameObject);
		return gameObject;
	}

	// Token: 0x060010A7 RID: 4263 RVA: 0x0004F198 File Offset: 0x0004D398
	public bool HasFreeSlots(Item item, int amount)
	{
		return item.Type == ItemType.Gold || this.GetTotalFreeSlots(item) >= amount;
	}

	// Token: 0x060010A8 RID: 4264 RVA: 0x0004F1B4 File Offset: 0x0004D3B4
	public int GetTotalFreeSlots(Item item)
	{
		int num = 0;
		for (int i = 0; i < this.Items.Count; i++)
		{
			if (!this.Items[i].IsDefined)
			{
				num += (item.Stackable ? 750 : 1);
			}
			else if (this.Items[i].IsDefined & this.Items[i].Stackable & this.Items[i].IsEqual(item))
			{
				num += 750 - this.Items[i].Amount;
			}
		}
		return num;
	}

	// Token: 0x060010A9 RID: 4265 RVA: 0x0004F264 File Offset: 0x0004D464
	public Item GetItemOrderedByQuality(int itemId)
	{
		return (from i in this.Items
		orderby i.Quality descending
		select i).FirstOrDefault((Item i) => i.Id == itemId);
	}

	// Token: 0x060010AA RID: 4266 RVA: 0x0004F2BC File Offset: 0x0004D4BC
	public Item GetItem(string itemUniqueId)
	{
		for (int i = 0; i < this.Items.Count; i++)
		{
			if (this.Items[i].UniqueId == itemUniqueId)
			{
				return this.Items[i];
			}
		}
		return default(Item);
	}

	// Token: 0x060010AB RID: 4267 RVA: 0x0004F314 File Offset: 0x0004D514
	public Item GetItem(Item itemToCompare)
	{
		for (int i = 0; i < this.Items.Count; i++)
		{
			if (this.Items[i].IsEqual(itemToCompare))
			{
				return this.Items[i];
			}
		}
		return default(Item);
	}

	// Token: 0x060010AC RID: 4268 RVA: 0x0004F364 File Offset: 0x0004D564
	[Server]
	public bool HasItemOnSlot(int slotPosition)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean InventoryModule::HasItemOnSlot(System.Int32)' called when server was not active");
			return default(bool);
		}
		if (slotPosition < 0)
		{
			return false;
		}
		Item item = this.Items[slotPosition];
		return item.IsDefined && item.Amount >= 1;
	}

	// Token: 0x060010AD RID: 4269 RVA: 0x0004F3BC File Offset: 0x0004D5BC
	[Server]
	public bool ContainsItem(string itemUniqueId, int amount)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean InventoryModule::ContainsItem(System.String,System.Int32)' called when server was not active");
			return default(bool);
		}
		if (amount < 1)
		{
			return false;
		}
		IEnumerable<Item> source = from w in this.Items
		where w.UniqueId == itemUniqueId
		select w;
		if (!source.Any<Item>())
		{
			return false;
		}
		return source.Sum((Item s) => s.Amount) >= amount;
	}

	// Token: 0x060010AE RID: 4270 RVA: 0x0004F44C File Offset: 0x0004D64C
	[Server]
	public Item GetAmmo(ItemType itemType)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'Item InventoryModule::GetAmmo(ItemType)' called when server was not active");
			return default(Item);
		}
		return (from i in this.Items
		where i.Type == itemType
		orderby i.Attack descending, i.Amount descending
		select i).FirstOrDefault<Item>();
	}

	// Token: 0x060010AF RID: 4271 RVA: 0x0004F4EC File Offset: 0x0004D6EC
	public int GetAmount(int itemId)
	{
		IEnumerable<Item> source = from w in this.Items
		where w.Id == itemId
		select w;
		if (source.Any<Item>())
		{
			return source.Sum((Item s) => s.Amount);
		}
		return 0;
	}

	// Token: 0x060010B0 RID: 4272 RVA: 0x0004F550 File Offset: 0x0004D750
	public int GetAmount(string itemUniqueId)
	{
		IEnumerable<Item> source = from w in this.Items
		where w.UniqueId == itemUniqueId
		select w;
		if (source.Any<Item>())
		{
			return source.Sum((Item s) => s.Amount);
		}
		return 0;
	}

	// Token: 0x060010B1 RID: 4273 RVA: 0x0004F5B4 File Offset: 0x0004D7B4
	[Server]
	public void ConsumeItem(int itemId, int amount)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void InventoryModule::ConsumeItem(System.Int32,System.Int32)' called when server was not active");
			return;
		}
		if (amount == 0)
		{
			return;
		}
		int amount2 = this.GetAmount(itemId);
		if (amount2 < amount)
		{
			throw new Exception(string.Format("PlayerId: {0} is trying to consume less items than available in inventory.", this.playerModule.PlayerId));
		}
		if (amount2 <= amount)
		{
			for (int i = 0; i < this.Items.Count; i++)
			{
				if (this.Items[i].Id == itemId)
				{
					this.RemoveItem(i);
				}
			}
			return;
		}
		int num = amount;
		for (int j = 0; j < this.Items.Count; j++)
		{
			Item item = this.Items[j];
			if (item.Id == itemId & item.Amount <= num)
			{
				num -= this.Items[j].Amount;
				this.RemoveItem(j);
			}
			else if (item.Id == itemId)
			{
				item.Amount -= num;
				this.Items[j] = item;
				return;
			}
		}
	}

	// Token: 0x060010B2 RID: 4274 RVA: 0x0004F6BC File Offset: 0x0004D8BC
	[Server]
	public void ConsumeItem(Item itemToConsume, int amount)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void InventoryModule::ConsumeItem(Item,System.Int32)' called when server was not active");
			return;
		}
		if (amount == 0)
		{
			return;
		}
		int amount2 = this.GetAmount(itemToConsume.UniqueId);
		if (amount2 < amount)
		{
			throw new Exception(string.Format("PlayerId: {0} is trying to consume less items than available in inventory.", this.playerModule.PlayerId));
		}
		if (amount2 <= amount)
		{
			for (int i = 0; i < this.Items.Count; i++)
			{
				if (this.Items[i].IsEqual(itemToConsume))
				{
					this.RemoveItem(i);
				}
			}
			return;
		}
		int num = amount;
		for (int j = 0; j < this.Items.Count; j++)
		{
			Item item = this.Items[j];
			if (item.IsEqual(itemToConsume) & item.Amount <= num)
			{
				num -= this.Items[j].Amount;
				this.RemoveItem(j);
			}
			else if (item.IsEqual(itemToConsume))
			{
				item.Amount -= num;
				this.Items[j] = item;
				return;
			}
		}
	}

	// Token: 0x060010B3 RID: 4275 RVA: 0x0004F7CD File Offset: 0x0004D9CD
	[Server]
	public void AddItem(Item item, bool invokeEvents)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void InventoryModule::AddItem(Item,System.Boolean)' called when server was not active");
			return;
		}
		this.AddItem(item, -1, item.Amount, invokeEvents);
	}

	// Token: 0x060010B4 RID: 4276 RVA: 0x0004F7F4 File Offset: 0x0004D9F4
	[Server]
	public void AddItem(Item item, int slotPosition, int amount, bool invokeEvents)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void InventoryModule::AddItem(Item,System.Int32,System.Int32,System.Boolean)' called when server was not active");
			return;
		}
		if (amount == 0)
		{
			Debug.LogWarning("Failed to add an item with amount 0 to the inventory");
			return;
		}
		if (!item.IsDefined)
		{
			Debug.LogWarning("Failed to add an undefined item to the inventory");
			return;
		}
		if (item.IsDefined && item.Type == ItemType.Gold && this.walletModule != null)
		{
			this.walletModule.AddGoldCoins(amount);
			return;
		}
		if (item.IsDefined && slotPosition != -1)
		{
			item.SlotPosition = slotPosition;
			item.Amount = amount;
			this.Items[slotPosition] = item;
			if (invokeEvents && this.OnItemAdded != null)
			{
				this.OnItemAdded(item.Id, slotPosition, amount);
				return;
			}
		}
		else if (item.IsDefined)
		{
			int num = amount;
			for (int i = 0; i < this.Items.Count; i++)
			{
				Item item2 = this.Items[i];
				if (item.Stackable & item2.IsEqual(item) & item2.Amount + num <= 750)
				{
					this.AddAmount(i, num, invokeEvents);
					num = 0;
					break;
				}
				if (item.Stackable & item2.IsEqual(item) & item2.Amount + num > 750)
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
							int num3 = num - 750;
							num3 = Mathf.Min(750, num3);
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
						this.OnItemAdded(item.Id, slotPosition, item.Amount);
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
				GlobalUtils.SpawnItemOnGround(item, null, base.transform.position, 60f);
			}
		}
	}

	// Token: 0x060010B5 RID: 4277 RVA: 0x0004FA24 File Offset: 0x0004DC24
	[Server]
	public void AddAmount(int slotPosition, int amount, bool invokeEvents)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void InventoryModule::AddAmount(System.Int32,System.Int32,System.Boolean)' called when server was not active");
			return;
		}
		if (amount == 0)
		{
			return;
		}
		if (amount < 0 & this.Items[slotPosition].Amount <= 1)
		{
			this.RemoveItem(slotPosition);
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
				this.RemoveItem(slotPosition);
			}
		}
		if (invokeEvents && this.OnAmountAdded != null)
		{
			this.OnAmountAdded(amount, slotPosition, this.Items[slotPosition]);
		}
	}

	// Token: 0x060010B6 RID: 4278 RVA: 0x0004FAEC File Offset: 0x0004DCEC
	[Server]
	public void RemoveItem(int position)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void InventoryModule::RemoveItem(System.Int32)' called when server was not active");
			return;
		}
		Item item = this.Items[position];
		if (item.IsDefined)
		{
			this.Items[position] = default(Item);
			if (item.Id == 301 || item.Id == 302 || item.Id == 303)
			{
				ConditionModule conditionModule;
				base.TryGetComponent<ConditionModule>(out conditionModule);
				conditionModule.RemoveCondition(ConditionType.Collect);
			}
		}
	}

	// Token: 0x060010B7 RID: 4279 RVA: 0x0004FB74 File Offset: 0x0004DD74
	[Command]
	public void CmdSwapItemPosition(int slotPosition, int draggingPosition, bool invokeEvents)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(slotPosition);
		writer.WriteInt(draggingPosition);
		writer.WriteBool(invokeEvents);
		base.SendCommandInternal("System.Void InventoryModule::CmdSwapItemPosition(System.Int32,System.Int32,System.Boolean)", -653416387, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x060010B8 RID: 4280 RVA: 0x0004FBC4 File Offset: 0x0004DDC4
	[Command]
	public void CmdSortItems()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		base.SendCommandInternal("System.Void InventoryModule::CmdSortItems()", -44215507, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x060010B9 RID: 4281 RVA: 0x0004FBF4 File Offset: 0x0004DDF4
	[Command]
	public void CmdAddAmountFromSlot(int originSlotPosition, int destinationSlotPosition, int amount, bool invokeEvents)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(originSlotPosition);
		writer.WriteInt(destinationSlotPosition);
		writer.WriteInt(amount);
		writer.WriteBool(invokeEvents);
		base.SendCommandInternal("System.Void InventoryModule::CmdAddAmountFromSlot(System.Int32,System.Int32,System.Int32,System.Boolean)", 2106290816, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x060010BA RID: 4282 RVA: 0x0004FC4C File Offset: 0x0004DE4C
	[Command]
	public void CmdDropItem(int slotPosition, Vector2 position)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(slotPosition);
		writer.WriteVector2(position);
		base.SendCommandInternal("System.Void InventoryModule::CmdDropItem(System.Int32,UnityEngine.Vector2)", 2141950014, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x060010BB RID: 4283 RVA: 0x0004FC90 File Offset: 0x0004DE90
	[Command]
	public void CmdCollectFromGround(GameObject groundSlot)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteGameObject(groundSlot);
		base.SendCommandInternal("System.Void InventoryModule::CmdCollectFromGround(UnityEngine.GameObject)", 450371518, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x060010BC RID: 4284 RVA: 0x0004FCCC File Offset: 0x0004DECC
	[Server]
	public bool AutomaticCollectFromGround(GameObject groundSlot)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean InventoryModule::AutomaticCollectFromGround(UnityEngine.GameObject)' called when server was not active");
			return default(bool);
		}
		if (!this.creatureModule.IsAlive)
		{
			return false;
		}
		if (groundSlot == null)
		{
			return false;
		}
		GroundSlotModule component = groundSlot.GetComponent<GroundSlotModule>();
		if (component.Owner == null)
		{
			return false;
		}
		if (!this.ValidateLootOwner(component.Owner))
		{
			return false;
		}
		Item item = component.Item;
		if (!this.HasFreeSlots(item, item.Amount))
		{
			return false;
		}
		if (component.Collected)
		{
			return false;
		}
		if (component.Item.Type != ItemType.Gold)
		{
			if (component.Item.Quality < this.CollectorMinQualityFilter)
			{
				return false;
			}
			if (component.Item.Rarity < this.CollectorMinRarityFilter)
			{
				return false;
			}
			if (this.CollectorCategoryFilter > ItemCategory.Undefined & component.Item.Category != this.CollectorCategoryFilter)
			{
				return false;
			}
			if (this.CollectorTypeFilter > ItemType.Undefined & component.Item.Type != this.CollectorTypeFilter)
			{
				return false;
			}
		}
		component.Collected = true;
		component.RemoveGroundSlot();
		this.AddItem(item, true);
		if (item.Rarity > Rarity.Rare)
		{
			base.StartCoroutine(DiscordWebhookManager.SendInGameCommandsChannel(string.Format("{0} Automatic Collected from Ground: {1}x {2}(+{3}) - {4}", new object[]
			{
				this.creatureModule.OriginalCreatureName,
				item.Amount,
				item.Name,
				item.BoostLevel,
				item.Rarity
			})));
		}
		return true;
	}

	// Token: 0x060010BD RID: 4285 RVA: 0x0004FE5C File Offset: 0x0004E05C
	private bool ValidateLootOwner(GameObject owner)
	{
		return !(owner != null) || !(owner != base.gameObject) || owner.GetComponent<PartyModule>().IsMember(base.gameObject);
	}

	// Token: 0x060010BE RID: 4286 RVA: 0x0004FE8C File Offset: 0x0004E08C
	public Item[] GetMarketSellableItems(string search)
	{
		IEnumerable<Item> source = from i in this.Items
		where i.Sellable & !i.Soulbind
		select i;
		if (!string.IsNullOrEmpty(search))
		{
			source = from i in source
			where LanguageManager.Instance.GetText(i.MetaName).Contains(search)
			select i;
		}
		return source.ToArray<Item>();
	}

	// Token: 0x060010BF RID: 4287 RVA: 0x0004FEF8 File Offset: 0x0004E0F8
	public Item[] GetUniqueSellableItems()
	{
		return (from i in this.Items
		where i.Sellable & !i.Soulbind & i.Value > 0
		select i into si
		select si.UniqueId).Distinct<string>().Select(new Func<string, Item>(this.GetItem)).ToArray<Item>();
	}

	// Token: 0x060010C0 RID: 4288 RVA: 0x0004FF70 File Offset: 0x0004E170
	[Command]
	public void CmdSendToWarehouse(int slotPosition, int warehouseSlotPosition)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(slotPosition);
		writer.WriteInt(warehouseSlotPosition);
		base.SendCommandInternal("System.Void InventoryModule::CmdSendToWarehouse(System.Int32,System.Int32)", -1051198405, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x060010C1 RID: 4289 RVA: 0x0004FFB4 File Offset: 0x0004E1B4
	[Command]
	public void CmdAddEquippedItemToItemBar(SlotType slotType, int itemBarSlotPosition, bool invokeEvents)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		Mirror.GeneratedNetworkCode._Write_SlotType(writer, slotType);
		writer.WriteInt(itemBarSlotPosition);
		writer.WriteBool(invokeEvents);
		base.SendCommandInternal("System.Void InventoryModule::CmdAddEquippedItemToItemBar(SlotType,System.Int32,System.Boolean)", -2082873193, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x060010C2 RID: 4290 RVA: 0x00050004 File Offset: 0x0004E204
	[Command]
	public void CmdAddToItemBar(int inventorySlotPosition, int itemBarSlotPosition, bool invokeEvents)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(inventorySlotPosition);
		writer.WriteInt(itemBarSlotPosition);
		writer.WriteBool(invokeEvents);
		base.SendCommandInternal("System.Void InventoryModule::CmdAddToItemBar(System.Int32,System.Int32,System.Boolean)", 518603730, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x060010C3 RID: 4291 RVA: 0x00050054 File Offset: 0x0004E254
	[Server]
	private void AddItemToItemBar(Item item, int itemBarSlotPosition, bool invokeEvents)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void InventoryModule::AddItemToItemBar(Item,System.Int32,System.Boolean)' called when server was not active");
			return;
		}
		for (int i = 0; i < this.ItemBar.Count; i++)
		{
			if (this.ItemBar[i].UniqueId == item.UniqueId)
			{
				this.ItemBar[i] = default(Item);
				InventoryModule.OnItemRemovedFromItemBarEventHandler onItemRemovedFromItemBar = this.OnItemRemovedFromItemBar;
				if (onItemRemovedFromItemBar != null)
				{
					onItemRemovedFromItemBar(itemBarSlotPosition);
				}
			}
		}
		bool isDefined = this.ItemBar[itemBarSlotPosition].IsDefined;
		item.Amount = this.GetAmount(item.UniqueId);
		item.SlotPosition = itemBarSlotPosition;
		this.ItemBar[itemBarSlotPosition] = item;
		if (invokeEvents && this.OnItemAddedToItemBar != null)
		{
			this.OnItemAddedToItemBar(itemBarSlotPosition, isDefined, this.ItemBar[itemBarSlotPosition]);
		}
	}

	// Token: 0x060010C4 RID: 4292 RVA: 0x00050138 File Offset: 0x0004E338
	[Command]
	public void CmdRemoveFromItemBar(int slotPosition, bool invokeEvents)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(slotPosition);
		writer.WriteBool(invokeEvents);
		base.SendCommandInternal("System.Void InventoryModule::CmdRemoveFromItemBar(System.Int32,System.Boolean)", -1877371473, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x060010C5 RID: 4293 RVA: 0x0005017C File Offset: 0x0004E37C
	[Command]
	public void CmdSwapItemBarSlot(int slotPosition, int draggingPosition, bool invokeEvents)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(slotPosition);
		writer.WriteInt(draggingPosition);
		writer.WriteBool(invokeEvents);
		base.SendCommandInternal("System.Void InventoryModule::CmdSwapItemBarSlot(System.Int32,System.Int32,System.Boolean)", -1554067251, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x060010C6 RID: 4294 RVA: 0x000501CC File Offset: 0x0004E3CC
	[Command]
	public void CmdConfigureCollector(Rarity minRarityFilter, ItemQuality minQualityFilter, ItemType typeFilter, ItemCategory categoryFilter)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		Mirror.GeneratedNetworkCode._Write_Rarity(writer, minRarityFilter);
		Mirror.GeneratedNetworkCode._Write_ItemQuality(writer, minQualityFilter);
		Mirror.GeneratedNetworkCode._Write_ItemType(writer, typeFilter);
		Mirror.GeneratedNetworkCode._Write_ItemCategory(writer, categoryFilter);
		base.SendCommandInternal("System.Void InventoryModule::CmdConfigureCollector(Rarity,ItemQuality,ItemType,ItemCategory)", -464220881, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x060010C7 RID: 4295 RVA: 0x00050224 File Offset: 0x0004E424
	[TargetRpc]
	public void TargetShowCollectorConfigWindow()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		this.SendTargetRPCInternal(null, "System.Void InventoryModule::TargetShowCollectorConfigWindow()", 1509179950, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x060010C8 RID: 4296 RVA: 0x00050254 File Offset: 0x0004E454
	public InventoryModule()
	{
		base.InitSyncObject(this.Items);
		base.InitSyncObject(this.ItemBar);
	}

	// Token: 0x060010CA RID: 4298 RVA: 0x0002FEC9 File Offset: 0x0002E0C9
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x170001D9 RID: 473
	// (get) Token: 0x060010CB RID: 4299 RVA: 0x000502B4 File Offset: 0x0004E4B4
	// (set) Token: 0x060010CC RID: 4300 RVA: 0x000502C7 File Offset: 0x0004E4C7
	public Rarity NetworkCollectorMinRarityFilter
	{
		get
		{
			return this.CollectorMinRarityFilter;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<Rarity>(value, ref this.CollectorMinRarityFilter, 1UL, null);
		}
	}

	// Token: 0x170001DA RID: 474
	// (get) Token: 0x060010CD RID: 4301 RVA: 0x000502E4 File Offset: 0x0004E4E4
	// (set) Token: 0x060010CE RID: 4302 RVA: 0x000502F7 File Offset: 0x0004E4F7
	public ItemQuality NetworkCollectorMinQualityFilter
	{
		get
		{
			return this.CollectorMinQualityFilter;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<ItemQuality>(value, ref this.CollectorMinQualityFilter, 2UL, null);
		}
	}

	// Token: 0x170001DB RID: 475
	// (get) Token: 0x060010CF RID: 4303 RVA: 0x00050314 File Offset: 0x0004E514
	// (set) Token: 0x060010D0 RID: 4304 RVA: 0x00050327 File Offset: 0x0004E527
	public ItemType NetworkCollectorTypeFilter
	{
		get
		{
			return this.CollectorTypeFilter;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<ItemType>(value, ref this.CollectorTypeFilter, 4UL, null);
		}
	}

	// Token: 0x170001DC RID: 476
	// (get) Token: 0x060010D1 RID: 4305 RVA: 0x00050344 File Offset: 0x0004E544
	// (set) Token: 0x060010D2 RID: 4306 RVA: 0x00050357 File Offset: 0x0004E557
	public ItemCategory NetworkCollectorCategoryFilter
	{
		get
		{
			return this.CollectorCategoryFilter;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<ItemCategory>(value, ref this.CollectorCategoryFilter, 8UL, null);
		}
	}

	// Token: 0x060010D3 RID: 4307 RVA: 0x00050374 File Offset: 0x0004E574
	protected void UserCode_CmdIncreaseInventory()
	{
		InventoryModule.<CmdIncreaseInventory>d__63 <CmdIncreaseInventory>d__;
		<CmdIncreaseInventory>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CmdIncreaseInventory>d__.<>4__this = this;
		<CmdIncreaseInventory>d__.<>1__state = -1;
		<CmdIncreaseInventory>d__.<>t__builder.Start<InventoryModule.<CmdIncreaseInventory>d__63>(ref <CmdIncreaseInventory>d__);
	}

	// Token: 0x060010D4 RID: 4308 RVA: 0x000503AB File Offset: 0x0004E5AB
	protected static void InvokeUserCode_CmdIncreaseInventory(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdIncreaseInventory called on client.");
			return;
		}
		((InventoryModule)obj).UserCode_CmdIncreaseInventory();
	}

	// Token: 0x060010D5 RID: 4309 RVA: 0x000503D0 File Offset: 0x0004E5D0
	protected void UserCode_CmdSwapItemPosition__Int32__Int32__Boolean(int slotPosition, int draggingPosition, bool invokeEvents)
	{
		Item value = this.Items[slotPosition];
		value.SlotPosition = draggingPosition;
		Item value2 = this.Items[draggingPosition];
		value2.SlotPosition = slotPosition;
		this.Items[slotPosition] = value2;
		this.Items[draggingPosition] = value;
		if (invokeEvents && this.OnItemPositionSwaped != null)
		{
			this.OnItemPositionSwaped(this.Items[slotPosition], this.Items[draggingPosition]);
		}
	}

	// Token: 0x060010D6 RID: 4310 RVA: 0x0005044F File Offset: 0x0004E64F
	protected static void InvokeUserCode_CmdSwapItemPosition__Int32__Int32__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSwapItemPosition called on client.");
			return;
		}
		((InventoryModule)obj).UserCode_CmdSwapItemPosition__Int32__Int32__Boolean(reader.ReadInt(), reader.ReadInt(), reader.ReadBool());
	}

	// Token: 0x060010D7 RID: 4311 RVA: 0x00050484 File Offset: 0x0004E684
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

	// Token: 0x060010D8 RID: 4312 RVA: 0x000505A1 File Offset: 0x0004E7A1
	protected static void InvokeUserCode_CmdSortItems(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSortItems called on client.");
			return;
		}
		((InventoryModule)obj).UserCode_CmdSortItems();
	}

	// Token: 0x060010D9 RID: 4313 RVA: 0x000505C4 File Offset: 0x0004E7C4
	protected void UserCode_CmdAddAmountFromSlot__Int32__Int32__Int32__Boolean(int originSlotPosition, int destinationSlotPosition, int amount, bool invokeEvents)
	{
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
		if (item.Amount < amount)
		{
			return;
		}
		if (item2.IsDefined)
		{
			if (!item.IsEqual(item2))
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
			if (!item2.IsDefined)
			{
				this.AddAmount(originSlotPosition, -amount, invokeEvents);
				this.AddItem(item, destinationSlotPosition, amount, invokeEvents);
				return;
			}
			return;
		}
	}

	// Token: 0x060010DA RID: 4314 RVA: 0x00050666 File Offset: 0x0004E866
	protected static void InvokeUserCode_CmdAddAmountFromSlot__Int32__Int32__Int32__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdAddAmountFromSlot called on client.");
			return;
		}
		((InventoryModule)obj).UserCode_CmdAddAmountFromSlot__Int32__Int32__Int32__Boolean(reader.ReadInt(), reader.ReadInt(), reader.ReadInt(), reader.ReadBool());
	}

	// Token: 0x060010DB RID: 4315 RVA: 0x000506A4 File Offset: 0x0004E8A4
	protected void UserCode_CmdDropItem__Int32__Vector2(int slotPosition, Vector2 position)
	{
		Item item = this.Items[slotPosition];
		if (!item.IsDefined)
		{
			return;
		}
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
		int layerMask = 1 << LayerMask.NameToLayer("Tile");
		if (Physics2D.OverlapPoint(position, layerMask) != null)
		{
			this.effectModule.ShowScreenMessage("equipment_cant_drop_items_there_message", 0, 3.5f, Array.Empty<string>());
			return;
		}
		this.RemoveItem(slotPosition);
		GlobalUtils.SpawnItemOnGround(item, null, position, 60f);
		base.StartCoroutine(DiscordWebhookManager.SendInGameCommandsChannel(string.Format("{0} Dropped from Inventory: {1}x {2}(+{3}) - {4}", new object[]
		{
			this.creatureModule.OriginalCreatureName,
			item.Amount,
			item.Name,
			item.BoostLevel,
			item.Rarity
		})));
	}

	// Token: 0x060010DC RID: 4316 RVA: 0x000507D7 File Offset: 0x0004E9D7
	protected static void InvokeUserCode_CmdDropItem__Int32__Vector2(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdDropItem called on client.");
			return;
		}
		((InventoryModule)obj).UserCode_CmdDropItem__Int32__Vector2(reader.ReadInt(), reader.ReadVector2());
	}

	// Token: 0x060010DD RID: 4317 RVA: 0x00050808 File Offset: 0x0004EA08
	protected void UserCode_CmdCollectFromGround__GameObject(GameObject groundSlot)
	{
		if (!this.creatureModule.IsAlive)
		{
			this.effectModule.ShowScreenMessage("skill_you_are_dead_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		if (groundSlot != null)
		{
			GroundSlotModule component = groundSlot.GetComponent<GroundSlotModule>();
			if (!this.ValidateLootOwner(component.Owner))
			{
				this.effectModule.ShowScreenMessage("loot_belongs_another_player_message", 3, 3.5f, Array.Empty<string>());
				return;
			}
			Item item = component.Item;
			if (!this.HasFreeSlots(item, item.Amount))
			{
				this.effectModule.ShowScreenMessage("inventory_full_message", 3, 3.5f, Array.Empty<string>());
				return;
			}
			if (component.Collected)
			{
				this.effectModule.ShowScreenMessage("someone_collecting_this_item_message", 3, 3.5f, Array.Empty<string>());
				return;
			}
			if (!GlobalUtils.IsClose(base.transform.position, groundSlot.transform.position, 1.12f))
			{
				return;
			}
			component.Collected = true;
			component.RemoveGroundSlot();
			this.AddItem(item, true);
			if (item.Rarity > Rarity.Uncommon)
			{
				base.StartCoroutine(DiscordWebhookManager.SendInGameCommandsChannel(string.Format("{0} Collected from Ground: {1}x {2}(+{3}) - {4}", new object[]
				{
					this.creatureModule.OriginalCreatureName,
					item.Amount,
					item.Name,
					item.BoostLevel,
					item.Rarity
				})));
			}
		}
	}

	// Token: 0x060010DE RID: 4318 RVA: 0x0005097B File Offset: 0x0004EB7B
	protected static void InvokeUserCode_CmdCollectFromGround__GameObject(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdCollectFromGround called on client.");
			return;
		}
		((InventoryModule)obj).UserCode_CmdCollectFromGround__GameObject(reader.ReadGameObject());
	}

	// Token: 0x060010DF RID: 4319 RVA: 0x000509A4 File Offset: 0x0004EBA4
	protected void UserCode_CmdSendToWarehouse__Int32__Int32(int slotPosition, int warehouseSlotPosition)
	{
		Item item = this.Items[slotPosition];
		if (item.Soulbind & item.OwnerId == 0)
		{
			item.OwnerId = this.playerModule.PlayerId;
		}
		if (!this.warehouseModule.HasFreeSlots)
		{
			this.effectModule.ShowScreenMessage("warehouse_full_message", 0, 3.5f, Array.Empty<string>());
			return;
		}
		if (item.IsDefined)
		{
			this.RemoveItem(slotPosition);
			if (this.warehouseModule.HasItemOnSlot(warehouseSlotPosition))
			{
				this.warehouseModule.AddItem(item, -1, item.Amount, true);
				return;
			}
			this.warehouseModule.AddItem(item, warehouseSlotPosition, item.Amount, true);
		}
	}

	// Token: 0x060010E0 RID: 4320 RVA: 0x00050A52 File Offset: 0x0004EC52
	protected static void InvokeUserCode_CmdSendToWarehouse__Int32__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSendToWarehouse called on client.");
			return;
		}
		((InventoryModule)obj).UserCode_CmdSendToWarehouse__Int32__Int32(reader.ReadInt(), reader.ReadInt());
	}

	// Token: 0x060010E1 RID: 4321 RVA: 0x00050A84 File Offset: 0x0004EC84
	protected void UserCode_CmdAddEquippedItemToItemBar__SlotType__Int32__Boolean(SlotType slotType, int itemBarSlotPosition, bool invokeEvents)
	{
		Item item = this.equipmentModule.GetItem(slotType);
		if (!item.IsDefined)
		{
			return;
		}
		this.AddItemToItemBar(item, itemBarSlotPosition, invokeEvents);
	}

	// Token: 0x060010E2 RID: 4322 RVA: 0x00050AB1 File Offset: 0x0004ECB1
	protected static void InvokeUserCode_CmdAddEquippedItemToItemBar__SlotType__Int32__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdAddEquippedItemToItemBar called on client.");
			return;
		}
		((InventoryModule)obj).UserCode_CmdAddEquippedItemToItemBar__SlotType__Int32__Boolean(Mirror.GeneratedNetworkCode._Read_SlotType(reader), reader.ReadInt(), reader.ReadBool());
	}

	// Token: 0x060010E3 RID: 4323 RVA: 0x00050AE8 File Offset: 0x0004ECE8
	protected void UserCode_CmdAddToItemBar__Int32__Int32__Boolean(int inventorySlotPosition, int itemBarSlotPosition, bool invokeEvents)
	{
		Item item = this.Items[inventorySlotPosition];
		if (!item.IsDefined)
		{
			return;
		}
		if (!this.Items.Any((Item a) => a.Id == item.Id))
		{
			return;
		}
		this.AddItemToItemBar(item, itemBarSlotPosition, invokeEvents);
	}

	// Token: 0x060010E4 RID: 4324 RVA: 0x00050B43 File Offset: 0x0004ED43
	protected static void InvokeUserCode_CmdAddToItemBar__Int32__Int32__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdAddToItemBar called on client.");
			return;
		}
		((InventoryModule)obj).UserCode_CmdAddToItemBar__Int32__Int32__Boolean(reader.ReadInt(), reader.ReadInt(), reader.ReadBool());
	}

	// Token: 0x060010E5 RID: 4325 RVA: 0x00050B78 File Offset: 0x0004ED78
	protected void UserCode_CmdRemoveFromItemBar__Int32__Boolean(int slotPosition, bool invokeEvents)
	{
		this.ItemBar[slotPosition] = default(Item);
		if (invokeEvents & this.OnItemRemovedFromItemBar != null)
		{
			this.OnItemRemovedFromItemBar(slotPosition);
		}
	}

	// Token: 0x060010E6 RID: 4326 RVA: 0x00050BB3 File Offset: 0x0004EDB3
	protected static void InvokeUserCode_CmdRemoveFromItemBar__Int32__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdRemoveFromItemBar called on client.");
			return;
		}
		((InventoryModule)obj).UserCode_CmdRemoveFromItemBar__Int32__Boolean(reader.ReadInt(), reader.ReadBool());
	}

	// Token: 0x060010E7 RID: 4327 RVA: 0x00050BE4 File Offset: 0x0004EDE4
	protected void UserCode_CmdSwapItemBarSlot__Int32__Int32__Boolean(int slotPosition, int draggingPosition, bool invokeEvents)
	{
		Item value = this.ItemBar[slotPosition];
		value.SlotPosition = draggingPosition;
		if (value.IsDefined)
		{
			value.Amount = this.GetAmount(value.UniqueId);
		}
		Item value2 = this.ItemBar[draggingPosition];
		value2.SlotPosition = slotPosition;
		if (value2.IsDefined)
		{
			value2.Amount = this.GetAmount(value2.UniqueId);
		}
		this.ItemBar[slotPosition] = value2;
		this.ItemBar[draggingPosition] = value;
		if (invokeEvents & this.OnItemSwapItemBarSlot != null)
		{
			this.OnItemSwapItemBarSlot(this.ItemBar[slotPosition], this.ItemBar[draggingPosition]);
		}
	}

	// Token: 0x060010E8 RID: 4328 RVA: 0x00050C9F File Offset: 0x0004EE9F
	protected static void InvokeUserCode_CmdSwapItemBarSlot__Int32__Int32__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSwapItemBarSlot called on client.");
			return;
		}
		((InventoryModule)obj).UserCode_CmdSwapItemBarSlot__Int32__Int32__Boolean(reader.ReadInt(), reader.ReadInt(), reader.ReadBool());
	}

	// Token: 0x060010E9 RID: 4329 RVA: 0x00050CD4 File Offset: 0x0004EED4
	protected void UserCode_CmdConfigureCollector__Rarity__ItemQuality__ItemType__ItemCategory(Rarity minRarityFilter, ItemQuality minQualityFilter, ItemType typeFilter, ItemCategory categoryFilter)
	{
		this.NetworkCollectorMinRarityFilter = minRarityFilter;
		this.NetworkCollectorMinQualityFilter = minQualityFilter;
		this.NetworkCollectorTypeFilter = typeFilter;
		this.NetworkCollectorCategoryFilter = categoryFilter;
	}

	// Token: 0x060010EA RID: 4330 RVA: 0x00050CF3 File Offset: 0x0004EEF3
	protected static void InvokeUserCode_CmdConfigureCollector__Rarity__ItemQuality__ItemType__ItemCategory(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdConfigureCollector called on client.");
			return;
		}
		((InventoryModule)obj).UserCode_CmdConfigureCollector__Rarity__ItemQuality__ItemType__ItemCategory(Mirror.GeneratedNetworkCode._Read_Rarity(reader), Mirror.GeneratedNetworkCode._Read_ItemQuality(reader), Mirror.GeneratedNetworkCode._Read_ItemType(reader), Mirror.GeneratedNetworkCode._Read_ItemCategory(reader));
	}

	// Token: 0x060010EB RID: 4331 RVA: 0x00050D2E File Offset: 0x0004EF2E
	protected void UserCode_TargetShowCollectorConfigWindow()
	{
		this.uiSystemModule.ShowCollectorConfigWindow();
	}

	// Token: 0x060010EC RID: 4332 RVA: 0x00050D3B File Offset: 0x0004EF3B
	protected static void InvokeUserCode_TargetShowCollectorConfigWindow(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetShowCollectorConfigWindow called on server.");
			return;
		}
		((InventoryModule)obj).UserCode_TargetShowCollectorConfigWindow();
	}

	// Token: 0x060010ED RID: 4333 RVA: 0x00050D60 File Offset: 0x0004EF60
	static InventoryModule()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(InventoryModule), "System.Void InventoryModule::CmdIncreaseInventory()", new RemoteCallDelegate(InventoryModule.InvokeUserCode_CmdIncreaseInventory), true);
		RemoteProcedureCalls.RegisterCommand(typeof(InventoryModule), "System.Void InventoryModule::CmdSwapItemPosition(System.Int32,System.Int32,System.Boolean)", new RemoteCallDelegate(InventoryModule.InvokeUserCode_CmdSwapItemPosition__Int32__Int32__Boolean), true);
		RemoteProcedureCalls.RegisterCommand(typeof(InventoryModule), "System.Void InventoryModule::CmdSortItems()", new RemoteCallDelegate(InventoryModule.InvokeUserCode_CmdSortItems), true);
		RemoteProcedureCalls.RegisterCommand(typeof(InventoryModule), "System.Void InventoryModule::CmdAddAmountFromSlot(System.Int32,System.Int32,System.Int32,System.Boolean)", new RemoteCallDelegate(InventoryModule.InvokeUserCode_CmdAddAmountFromSlot__Int32__Int32__Int32__Boolean), true);
		RemoteProcedureCalls.RegisterCommand(typeof(InventoryModule), "System.Void InventoryModule::CmdDropItem(System.Int32,UnityEngine.Vector2)", new RemoteCallDelegate(InventoryModule.InvokeUserCode_CmdDropItem__Int32__Vector2), true);
		RemoteProcedureCalls.RegisterCommand(typeof(InventoryModule), "System.Void InventoryModule::CmdCollectFromGround(UnityEngine.GameObject)", new RemoteCallDelegate(InventoryModule.InvokeUserCode_CmdCollectFromGround__GameObject), true);
		RemoteProcedureCalls.RegisterCommand(typeof(InventoryModule), "System.Void InventoryModule::CmdSendToWarehouse(System.Int32,System.Int32)", new RemoteCallDelegate(InventoryModule.InvokeUserCode_CmdSendToWarehouse__Int32__Int32), true);
		RemoteProcedureCalls.RegisterCommand(typeof(InventoryModule), "System.Void InventoryModule::CmdAddEquippedItemToItemBar(SlotType,System.Int32,System.Boolean)", new RemoteCallDelegate(InventoryModule.InvokeUserCode_CmdAddEquippedItemToItemBar__SlotType__Int32__Boolean), true);
		RemoteProcedureCalls.RegisterCommand(typeof(InventoryModule), "System.Void InventoryModule::CmdAddToItemBar(System.Int32,System.Int32,System.Boolean)", new RemoteCallDelegate(InventoryModule.InvokeUserCode_CmdAddToItemBar__Int32__Int32__Boolean), true);
		RemoteProcedureCalls.RegisterCommand(typeof(InventoryModule), "System.Void InventoryModule::CmdRemoveFromItemBar(System.Int32,System.Boolean)", new RemoteCallDelegate(InventoryModule.InvokeUserCode_CmdRemoveFromItemBar__Int32__Boolean), true);
		RemoteProcedureCalls.RegisterCommand(typeof(InventoryModule), "System.Void InventoryModule::CmdSwapItemBarSlot(System.Int32,System.Int32,System.Boolean)", new RemoteCallDelegate(InventoryModule.InvokeUserCode_CmdSwapItemBarSlot__Int32__Int32__Boolean), true);
		RemoteProcedureCalls.RegisterCommand(typeof(InventoryModule), "System.Void InventoryModule::CmdConfigureCollector(Rarity,ItemQuality,ItemType,ItemCategory)", new RemoteCallDelegate(InventoryModule.InvokeUserCode_CmdConfigureCollector__Rarity__ItemQuality__ItemType__ItemCategory), true);
		RemoteProcedureCalls.RegisterRpc(typeof(InventoryModule), "System.Void InventoryModule::TargetShowCollectorConfigWindow()", new RemoteCallDelegate(InventoryModule.InvokeUserCode_TargetShowCollectorConfigWindow));
	}

	// Token: 0x060010EE RID: 4334 RVA: 0x00050F1C File Offset: 0x0004F11C
	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			Mirror.GeneratedNetworkCode._Write_Rarity(writer, this.CollectorMinRarityFilter);
			Mirror.GeneratedNetworkCode._Write_ItemQuality(writer, this.CollectorMinQualityFilter);
			Mirror.GeneratedNetworkCode._Write_ItemType(writer, this.CollectorTypeFilter);
			Mirror.GeneratedNetworkCode._Write_ItemCategory(writer, this.CollectorCategoryFilter);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1UL) != 0UL)
		{
			Mirror.GeneratedNetworkCode._Write_Rarity(writer, this.CollectorMinRarityFilter);
		}
		if ((base.syncVarDirtyBits & 2UL) != 0UL)
		{
			Mirror.GeneratedNetworkCode._Write_ItemQuality(writer, this.CollectorMinQualityFilter);
		}
		if ((base.syncVarDirtyBits & 4UL) != 0UL)
		{
			Mirror.GeneratedNetworkCode._Write_ItemType(writer, this.CollectorTypeFilter);
		}
		if ((base.syncVarDirtyBits & 8UL) != 0UL)
		{
			Mirror.GeneratedNetworkCode._Write_ItemCategory(writer, this.CollectorCategoryFilter);
		}
	}

	// Token: 0x060010EF RID: 4335 RVA: 0x00051000 File Offset: 0x0004F200
	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			base.GeneratedSyncVarDeserialize<Rarity>(ref this.CollectorMinRarityFilter, null, Mirror.GeneratedNetworkCode._Read_Rarity(reader));
			base.GeneratedSyncVarDeserialize<ItemQuality>(ref this.CollectorMinQualityFilter, null, Mirror.GeneratedNetworkCode._Read_ItemQuality(reader));
			base.GeneratedSyncVarDeserialize<ItemType>(ref this.CollectorTypeFilter, null, Mirror.GeneratedNetworkCode._Read_ItemType(reader));
			base.GeneratedSyncVarDeserialize<ItemCategory>(ref this.CollectorCategoryFilter, null, Mirror.GeneratedNetworkCode._Read_ItemCategory(reader));
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 1L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<Rarity>(ref this.CollectorMinRarityFilter, null, Mirror.GeneratedNetworkCode._Read_Rarity(reader));
		}
		if ((num & 2L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<ItemQuality>(ref this.CollectorMinQualityFilter, null, Mirror.GeneratedNetworkCode._Read_ItemQuality(reader));
		}
		if ((num & 4L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<ItemType>(ref this.CollectorTypeFilter, null, Mirror.GeneratedNetworkCode._Read_ItemType(reader));
		}
		if ((num & 8L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<ItemCategory>(ref this.CollectorCategoryFilter, null, Mirror.GeneratedNetworkCode._Read_ItemCategory(reader));
		}
	}

	// Token: 0x04001006 RID: 4102
	[SyncVar]
	public Rarity CollectorMinRarityFilter;

	// Token: 0x04001007 RID: 4103
	[SyncVar]
	public ItemQuality CollectorMinQualityFilter;

	// Token: 0x04001008 RID: 4104
	[SyncVar]
	public ItemType CollectorTypeFilter;

	// Token: 0x04001009 RID: 4105
	[SyncVar]
	public ItemCategory CollectorCategoryFilter;

	// Token: 0x0400100A RID: 4106
	[SerializeField]
	private GameObject itemBarSlotPrefab;

	// Token: 0x0400100B RID: 4107
	[SerializeField]
	private GameObject inventorySlotPrefab;

	// Token: 0x0400100C RID: 4108
	public bool ShouldPersistData;

	// Token: 0x0400100D RID: 4109
	private WalletModule walletModule;

	// Token: 0x0400100E RID: 4110
	private EffectModule effectModule;

	// Token: 0x0400100F RID: 4111
	private PlayerModule playerModule;

	// Token: 0x04001010 RID: 4112
	private CreatureModule creatureModule;

	// Token: 0x04001011 RID: 4113
	private UISystemModule uiSystemModule;

	// Token: 0x04001012 RID: 4114
	private EquipmentModule equipmentModule;

	// Token: 0x04001013 RID: 4115
	private WarehouseModule warehouseModule;

	// Token: 0x04001014 RID: 4116
	public readonly SyncListItem Items = new SyncListItem();

	// Token: 0x04001015 RID: 4117
	public List<GameObject> Slots = new List<GameObject>();

	// Token: 0x04001016 RID: 4118
	public readonly SyncListItem ItemBar = new SyncListItem();

	// Token: 0x04001017 RID: 4119
	public List<GameObject> ItemBarSlots = new List<GameObject>();

	// Token: 0x0200034C RID: 844
	// (Invoke) Token: 0x060010F1 RID: 4337
	public delegate void OnItemAddedEventHandler(int itemId, int slotPosition, int amount);

	// Token: 0x0200034D RID: 845
	// (Invoke) Token: 0x060010F5 RID: 4341
	public delegate void OnAmountChangedEventHandler(int amount, int slotPosition, Item item);

	// Token: 0x0200034E RID: 846
	// (Invoke) Token: 0x060010F9 RID: 4345
	public delegate void OnItemPositionSwapedEventHandler(Item draggedItem, Item replacedItem);

	// Token: 0x0200034F RID: 847
	// (Invoke) Token: 0x060010FD RID: 4349
	public delegate void OnSlotAddedEventHandler(int amount, int totalSlots);

	// Token: 0x02000350 RID: 848
	// (Invoke) Token: 0x06001101 RID: 4353
	public delegate void OnItemRemovedFromItemBarEventHandler(int itemBarSlotPosition);

	// Token: 0x02000351 RID: 849
	// (Invoke) Token: 0x06001105 RID: 4357
	public delegate void OnItemAddedToItemBarEventHandler(int itemBarSlotPosition, bool overwrite, Item item);

	// Token: 0x02000352 RID: 850
	// (Invoke) Token: 0x06001109 RID: 4361
	public delegate void OnItemSwapItemBarSlotEventHandler(Item draggedItem, Item replacedItem);
}
