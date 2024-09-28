using System;
using System.Linq;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

// Token: 0x02000324 RID: 804
public class EquipmentModule : NetworkBehaviour
{
	// Token: 0x1400000D RID: 13
	// (add) Token: 0x06000F7C RID: 3964 RVA: 0x00048A50 File Offset: 0x00046C50
	// (remove) Token: 0x06000F7D RID: 3965 RVA: 0x00048A88 File Offset: 0x00046C88
	public event EquipmentModule.OnItemEquippedEventHandler OnItemEquipped;

	// Token: 0x1400000E RID: 14
	// (add) Token: 0x06000F7E RID: 3966 RVA: 0x00048AC0 File Offset: 0x00046CC0
	// (remove) Token: 0x06000F7F RID: 3967 RVA: 0x00048AF8 File Offset: 0x00046CF8
	public event EquipmentModule.OnItemUnequippedEventHandler OnItemUnequipped;

	// Token: 0x1400000F RID: 15
	// (add) Token: 0x06000F80 RID: 3968 RVA: 0x00048B30 File Offset: 0x00046D30
	// (remove) Token: 0x06000F81 RID: 3969 RVA: 0x00048B68 File Offset: 0x00046D68
	public event EquipmentModule.OnItemConsumedEventHandler OnItemConsumed;

	// Token: 0x170001C5 RID: 453
	// (get) Token: 0x06000F82 RID: 3970 RVA: 0x00048B9D File Offset: 0x00046D9D
	public bool HasItemOnHands
	{
		get
		{
			return this.IsEquipped(SlotType.RightHand) || this.IsEquipped(SlotType.LeftHand);
		}
	}

	// Token: 0x06000F83 RID: 3971 RVA: 0x00048BB4 File Offset: 0x00046DB4
	private void Awake()
	{
		if (NetworkClient.active)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
			this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
		}
		base.TryGetComponent<PvpModule>(out this.pvpModule);
		base.TryGetComponent<CombatModule>(out this.combatModule);
		base.TryGetComponent<EffectModule>(out this.effectModule);
		base.TryGetComponent<VocationModule>(out this.vocationModule);
		base.TryGetComponent<CreatureModule>(out this.creatureModule);
		base.TryGetComponent<AttributeModule>(out this.attributeModule);
		base.TryGetComponent<ConditionModule>(out this.conditionModule);
		base.TryGetComponent<InventoryModule>(out this.inventoryModule);
	}

	// Token: 0x06000F84 RID: 3972 RVA: 0x00048C47 File Offset: 0x00046E47
	public override void OnStartLocalPlayer()
	{
		this.EquippedItems.Callback += this.OnItemsChanged;
		this.BuildSlots();
	}

	// Token: 0x06000F85 RID: 3973 RVA: 0x00048C66 File Offset: 0x00046E66
	public override void OnStartServer()
	{
		this.EquippedItems.Callback += delegate(SyncList<Item>.Operation op, int index, Item oldItem, Item newItem)
		{
			this.ShouldPersistData = true;
		};
	}

	// Token: 0x06000F86 RID: 3974 RVA: 0x00048C80 File Offset: 0x00046E80
	public void InitializeSlots(Item[] items)
	{
		int i;
		int j;
		for (i = 0; i < Enum.GetValues(typeof(SlotType)).Length; i = j + 1)
		{
			Item item = items.FirstOrDefault((Item f) => f.SlotType == (SlotType)i);
			if (item.IsDefined)
			{
				this.EquippedItems.Add(item);
			}
			else
			{
				this.EquippedItems.Add(new Item(i));
			}
			j = i;
		}
	}

	// Token: 0x06000F87 RID: 3975 RVA: 0x00048D08 File Offset: 0x00046F08
	[Client]
	private void OnItemsChanged(SyncList<Item>.Operation op, int index, Item oldItem, Item newItem)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void EquipmentModule::OnItemsChanged(Mirror.SyncList`1/Operation<Item>,System.Int32,Item,Item)' called when client was not active");
			return;
		}
		if (op == SyncList<Item>.Operation.OP_ADD)
		{
			this.SetEquippedItemOnSlot((SlotType)index, newItem);
			return;
		}
		if (op != SyncList<Item>.Operation.OP_SET)
		{
			return;
		}
		this.SetEquippedItemOnSlot((SlotType)index, newItem);
	}

	// Token: 0x06000F88 RID: 3976 RVA: 0x00048D3C File Offset: 0x00046F3C
	[Client]
	private void BuildSlots()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void EquipmentModule::BuildSlots()' called when client was not active");
			return;
		}
		for (int i = 0; i < this.EquippedItems.Count; i++)
		{
			this.SetEquippedItemOnSlot((SlotType)i, this.EquippedItems[i]);
		}
	}

	// Token: 0x06000F89 RID: 3977 RVA: 0x00048D88 File Offset: 0x00046F88
	[Client]
	private void SetEquippedItemOnSlot(SlotType slotType, Item equippedItem)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void EquipmentModule::SetEquippedItemOnSlot(SlotType,Item)' called when client was not active");
			return;
		}
		EquipmentSlotManager equipmentSlotManager = this.uiSystemModule.EquipmentSlotsHolder.GetComponentsInChildren<EquipmentSlotManager>(true).FirstOrDefault((EquipmentSlotManager esm) => esm.SlotType == slotType);
		if (equipmentSlotManager != null)
		{
			equipmentSlotManager.SetItem(equippedItem);
		}
	}

	// Token: 0x06000F8A RID: 3978 RVA: 0x00048DEC File Offset: 0x00046FEC
	[Command]
	public void CmdEquipFromInventory(int inventoryPosition, bool invokeEvents)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(inventoryPosition);
		writer.WriteBool(invokeEvents);
		base.SendCommandInternal("System.Void EquipmentModule::CmdEquipFromInventory(System.Int32,System.Boolean)", -457145403, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000F8B RID: 3979 RVA: 0x00048E30 File Offset: 0x00047030
	[Server]
	public void EquipFromInventory(int inventoryPosition, bool invokeEvents)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void EquipmentModule::EquipFromInventory(System.Int32,System.Boolean)' called when server was not active");
			return;
		}
		Item item = this.inventoryModule.Items[inventoryPosition];
		if (item.SlotType == SlotType.Undefined)
		{
			return;
		}
		if (this.combatModule.IsFighting)
		{
			this.effectModule.ShowScreenMessage("equipment_cant_equip_while_fighting_message", 0, 3.5f, Array.Empty<string>());
			return;
		}
		if (this.vocationModule != null && !this.vocationModule.AllowedItemTypes().Contains(item.Type))
		{
			this.effectModule.ShowScreenMessage("equipment_vocation_cant_equip_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		if (item.RequiredLevel > this.attributeModule.BaseLevel)
		{
			this.effectModule.ShowScreenMessage("equipment_need_level_message", 3, 3.5f, new string[]
			{
				item.RequiredLevel.ToString()
			});
			return;
		}
		if (item.UseOnHands && item.TwoHanded && this.HasItemOnHands && !this.inventoryModule.HasFreeSlots(item, item.Amount))
		{
			this.effectModule.ShowScreenMessage("equipment_both_hands_message", 0, 3.5f, Array.Empty<string>());
			return;
		}
		Item item2 = this.EquippedItems[(int)item.SlotType];
		if (item2.IsDefined & this.pvpModule.HasPlayerKillerLimitations())
		{
			this.effectModule.ShowScreenMessage("equipment_cant_unequip_while_player_killer_message", 0, 3.5f, Array.Empty<string>());
			return;
		}
		if (item2.IsDefined & item2.Stackable & item2.UniqueId == item.UniqueId)
		{
			int amount = item2.Amount;
			int amount2 = item.Amount;
			int num = amount + amount2 - 750;
			if (num <= 0)
			{
				item.Amount = amount + amount2;
				this.inventoryModule.RemoveItem(inventoryPosition);
			}
			else
			{
				item.Amount = 750;
				int num2 = amount2 - num;
				this.inventoryModule.AddAmount(inventoryPosition, -num2, invokeEvents);
			}
		}
		else if (item2.IsDefined)
		{
			this.inventoryModule.RemoveItem(inventoryPosition);
			if (item.TwoHanded)
			{
				this.Unequip(SlotType.RightHand, invokeEvents);
				this.Unequip(SlotType.LeftHand, invokeEvents);
			}
			else
			{
				this.Unequip(item.SlotType, invokeEvents);
				if (item.UseOnHands)
				{
					this.CheckAndUnequipOtherHand(item, invokeEvents);
				}
			}
		}
		else
		{
			this.inventoryModule.RemoveItem(inventoryPosition);
			if (item.TwoHanded)
			{
				this.Unequip(SlotType.RightHand, invokeEvents);
				this.Unequip(SlotType.LeftHand, invokeEvents);
			}
			else if (item.UseOnHands)
			{
				this.CheckAndUnequipOtherHand(item, invokeEvents);
			}
		}
		this.EquippedItems[(int)item.SlotType] = item;
		if (invokeEvents && this.OnItemEquipped != null)
		{
			this.OnItemEquipped(item);
		}
	}

	// Token: 0x06000F8C RID: 3980 RVA: 0x000490E0 File Offset: 0x000472E0
	[Command]
	public void CmdUnequip(SlotType slotType, bool invokeEvents)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		Mirror.GeneratedNetworkCode._Write_SlotType(writer, slotType);
		writer.WriteBool(invokeEvents);
		base.SendCommandInternal("System.Void EquipmentModule::CmdUnequip(SlotType,System.Boolean)", -1804837279, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000F8D RID: 3981 RVA: 0x00049124 File Offset: 0x00047324
	[Server]
	private void Unequip(SlotType slotType, bool invokeEvents)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void EquipmentModule::Unequip(SlotType,System.Boolean)' called when server was not active");
			return;
		}
		Item item = this.EquippedItems[(int)slotType];
		if (!item.IsDefined)
		{
			return;
		}
		if (this.combatModule.IsFighting)
		{
			this.ShowCantUnequipeWhileFightingMessage();
			return;
		}
		if (this.conditionModule.HasActiveCondition(ConditionType.Paralize))
		{
			this.ShowCantUnequipeWhileFightingMessage();
			return;
		}
		if (this.pvpModule.HasPlayerKillerLimitations())
		{
			this.effectModule.ShowScreenMessage("equipment_cant_unequip_while_player_killer_message", 0, 3.5f, Array.Empty<string>());
			return;
		}
		this.EquippedItems[(int)slotType] = default(Item);
		this.inventoryModule.AddItem(item, invokeEvents);
		if (invokeEvents && this.OnItemUnequipped != null)
		{
			this.OnItemUnequipped(item.Id, slotType);
		}
	}

	// Token: 0x06000F8E RID: 3982 RVA: 0x000491F0 File Offset: 0x000473F0
	[Command]
	public void CmdUnequipToGround(SlotType slotType, Vector3 position, bool unequippingByDeath, bool invokeEvents)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		Mirror.GeneratedNetworkCode._Write_SlotType(writer, slotType);
		writer.WriteVector3(position);
		writer.WriteBool(unequippingByDeath);
		writer.WriteBool(invokeEvents);
		base.SendCommandInternal("System.Void EquipmentModule::CmdUnequipToGround(SlotType,UnityEngine.Vector3,System.Boolean,System.Boolean)", -439357103, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000F8F RID: 3983 RVA: 0x00049248 File Offset: 0x00047448
	[Server]
	public void UnequipToGround(SlotType slotType, Vector3 position, bool unequippingByDeath, bool invokeEvents)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void EquipmentModule::UnequipToGround(SlotType,UnityEngine.Vector3,System.Boolean,System.Boolean)' called when server was not active");
			return;
		}
		Item item = this.EquippedItems[(int)slotType];
		if (item.Soulbind & !unequippingByDeath)
		{
			this.effectModule.ShowScreenMessage("cant_drop_this_item_message", 0, 3.5f, Array.Empty<string>());
			return;
		}
		if (!GlobalUtils.IsClose(base.transform.position, position, 4f))
		{
			this.effectModule.ShowScreenMessage("equipment_cant_drop_items_that_far_message", 0, 3.5f, Array.Empty<string>());
			return;
		}
		if (!item.IsDefined)
		{
			return;
		}
		if (item.SlotType != SlotType.Ammo & this.combatModule.InCombat & !unequippingByDeath)
		{
			this.ShowCantUnequipeWhileFightingMessage();
			return;
		}
		if (this.conditionModule.HasActiveCondition(ConditionType.Paralize) & !unequippingByDeath)
		{
			this.ShowCantUnequipeWhileFightingMessage();
			return;
		}
		if (this.pvpModule.HasPlayerKillerLimitations() & !unequippingByDeath)
		{
			this.effectModule.ShowScreenMessage("equipment_cant_unequip_while_player_killer_message", 0, 3.5f, Array.Empty<string>());
			return;
		}
		int layerMask = 1 << LayerMask.NameToLayer("Tile");
		if (Physics2D.OverlapPoint(position, layerMask) != null & !unequippingByDeath)
		{
			this.effectModule.ShowScreenMessage("equipment_cant_drop_items_there_message", 0, 3.5f, Array.Empty<string>());
			return;
		}
		this.EquippedItems[(int)slotType] = default(Item);
		float duration = 300f;
		if (unequippingByDeath)
		{
			duration = 900f;
		}
		GlobalUtils.SpawnItemOnGround(item, null, position, duration);
		if (invokeEvents && this.OnItemUnequipped != null)
		{
			this.OnItemUnequipped(item.Id, slotType);
		}
		base.StartCoroutine(DiscordWebhookManager.SendInGameCommandsChannel(string.Format("{0} Unequipped to Ground: {1}x {2}(+{3}) - {4}", new object[]
		{
			this.creatureModule.OriginalCreatureName,
			item.Amount,
			item.Name,
			item.BoostLevel,
			item.Rarity
		})));
	}

	// Token: 0x06000F90 RID: 3984 RVA: 0x00049445 File Offset: 0x00047645
	private void ShowCantUnequipeWhileFightingMessage()
	{
		this.effectModule.ShowScreenMessage("equipment_cant_unequip_while_fighting_message", 0, 3.5f, Array.Empty<string>());
	}

	// Token: 0x06000F91 RID: 3985 RVA: 0x00049464 File Offset: 0x00047664
	[Server]
	private void CheckAndUnequipOtherHand(Item item, bool invokeEvents = true)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void EquipmentModule::CheckAndUnequipOtherHand(Item,System.Boolean)' called when server was not active");
			return;
		}
		if (item.SlotType == SlotType.RightHand)
		{
			Item item2 = this.GetItem(SlotType.LeftHand);
			if (item2.TwoHanded)
			{
				this.Unequip(item2.SlotType, invokeEvents);
			}
			return;
		}
		if (item.SlotType == SlotType.LeftHand)
		{
			Item item3 = this.GetItem(SlotType.RightHand);
			if (item3.TwoHanded)
			{
				this.Unequip(item3.SlotType, invokeEvents);
			}
			return;
		}
	}

	// Token: 0x06000F92 RID: 3986 RVA: 0x000494D4 File Offset: 0x000476D4
	public Item GetItem(SlotType slotType)
	{
		return this.EquippedItems[(int)slotType];
	}

	// Token: 0x06000F93 RID: 3987 RVA: 0x000494E4 File Offset: 0x000476E4
	private bool IsEquipped(SlotType slotType)
	{
		for (int i = 0; i < this.EquippedItems.Count; i++)
		{
			if (this.EquippedItems[i].SlotType == slotType)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000F94 RID: 3988 RVA: 0x00049520 File Offset: 0x00047720
	public bool IsEquipped(SlotType slotType, ItemType itemType)
	{
		for (int i = 0; i < this.EquippedItems.Count; i++)
		{
			if (this.EquippedItems[i].SlotType == slotType && this.EquippedItems[i].Type == itemType)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000F95 RID: 3989 RVA: 0x00049570 File Offset: 0x00047770
	[Server]
	public void ConsumeItem(SlotType slotType, ItemType itemType, int amount, bool invokeEvents)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void EquipmentModule::ConsumeItem(SlotType,ItemType,System.Int32,System.Boolean)' called when server was not active");
			return;
		}
		if (this.EquippedItems[(int)slotType].Type == itemType)
		{
			if (this.GetAmount(slotType, itemType) <= amount)
			{
				this.EquippedItems[(int)slotType] = default(Item);
			}
			else
			{
				Item item = this.GetItem(slotType);
				if (item.Amount <= amount)
				{
					this.EquippedItems[(int)slotType] = default(Item);
				}
				else
				{
					item.Amount -= amount;
					this.EquippedItems[(int)slotType] = item;
				}
			}
			if (invokeEvents && this.OnItemConsumed != null)
			{
				Item item2 = this.GetItem(slotType);
				item2.SlotType = slotType;
				this.OnItemConsumed(amount, item2);
			}
		}
	}

	// Token: 0x06000F96 RID: 3990 RVA: 0x00049638 File Offset: 0x00047838
	private int GetAmount(SlotType slotType, ItemType itemType)
	{
		Item item = this.GetItem(slotType);
		if (!item.IsDefined | item.Type != itemType)
		{
			return 0;
		}
		return item.Amount;
	}

	// Token: 0x06000F97 RID: 3991 RVA: 0x0004966E File Offset: 0x0004786E
	public EquipmentModule()
	{
		base.InitSyncObject(this.EquippedItems);
	}

	// Token: 0x06000F99 RID: 3993 RVA: 0x0002FEC9 File Offset: 0x0002E0C9
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x06000F9A RID: 3994 RVA: 0x00049696 File Offset: 0x00047896
	protected void UserCode_CmdEquipFromInventory__Int32__Boolean(int inventoryPosition, bool invokeEvents)
	{
		this.EquipFromInventory(inventoryPosition, invokeEvents);
	}

	// Token: 0x06000F9B RID: 3995 RVA: 0x000496A0 File Offset: 0x000478A0
	protected static void InvokeUserCode_CmdEquipFromInventory__Int32__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdEquipFromInventory called on client.");
			return;
		}
		((EquipmentModule)obj).UserCode_CmdEquipFromInventory__Int32__Boolean(reader.ReadInt(), reader.ReadBool());
	}

	// Token: 0x06000F9C RID: 3996 RVA: 0x000496CF File Offset: 0x000478CF
	protected void UserCode_CmdUnequip__SlotType__Boolean(SlotType slotType, bool invokeEvents)
	{
		this.Unequip(slotType, invokeEvents);
	}

	// Token: 0x06000F9D RID: 3997 RVA: 0x000496D9 File Offset: 0x000478D9
	protected static void InvokeUserCode_CmdUnequip__SlotType__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdUnequip called on client.");
			return;
		}
		((EquipmentModule)obj).UserCode_CmdUnequip__SlotType__Boolean(Mirror.GeneratedNetworkCode._Read_SlotType(reader), reader.ReadBool());
	}

	// Token: 0x06000F9E RID: 3998 RVA: 0x00049708 File Offset: 0x00047908
	protected void UserCode_CmdUnequipToGround__SlotType__Vector3__Boolean__Boolean(SlotType slotType, Vector3 position, bool unequippingByDeath, bool invokeEvents)
	{
		this.UnequipToGround(slotType, position, unequippingByDeath, invokeEvents);
	}

	// Token: 0x06000F9F RID: 3999 RVA: 0x00049715 File Offset: 0x00047915
	protected static void InvokeUserCode_CmdUnequipToGround__SlotType__Vector3__Boolean__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdUnequipToGround called on client.");
			return;
		}
		((EquipmentModule)obj).UserCode_CmdUnequipToGround__SlotType__Vector3__Boolean__Boolean(Mirror.GeneratedNetworkCode._Read_SlotType(reader), reader.ReadVector3(), reader.ReadBool(), reader.ReadBool());
	}

	// Token: 0x06000FA0 RID: 4000 RVA: 0x00049750 File Offset: 0x00047950
	static EquipmentModule()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(EquipmentModule), "System.Void EquipmentModule::CmdEquipFromInventory(System.Int32,System.Boolean)", new RemoteCallDelegate(EquipmentModule.InvokeUserCode_CmdEquipFromInventory__Int32__Boolean), true);
		RemoteProcedureCalls.RegisterCommand(typeof(EquipmentModule), "System.Void EquipmentModule::CmdUnequip(SlotType,System.Boolean)", new RemoteCallDelegate(EquipmentModule.InvokeUserCode_CmdUnequip__SlotType__Boolean), true);
		RemoteProcedureCalls.RegisterCommand(typeof(EquipmentModule), "System.Void EquipmentModule::CmdUnequipToGround(SlotType,UnityEngine.Vector3,System.Boolean,System.Boolean)", new RemoteCallDelegate(EquipmentModule.InvokeUserCode_CmdUnequipToGround__SlotType__Vector3__Boolean__Boolean), true);
	}

	// Token: 0x04000F2C RID: 3884
	[SerializeField]
	private GameObject animationSlotPrefab;

	// Token: 0x04000F2D RID: 3885
	private PvpModule pvpModule;

	// Token: 0x04000F2E RID: 3886
	private EffectModule effectModule;

	// Token: 0x04000F2F RID: 3887
	private CombatModule combatModule;

	// Token: 0x04000F30 RID: 3888
	private UISystemModule uiSystemModule;

	// Token: 0x04000F31 RID: 3889
	private InventoryModule inventoryModule;

	// Token: 0x04000F32 RID: 3890
	private VocationModule vocationModule;

	// Token: 0x04000F33 RID: 3891
	private AttributeModule attributeModule;

	// Token: 0x04000F34 RID: 3892
	private ConditionModule conditionModule;

	// Token: 0x04000F35 RID: 3893
	private CreatureModule creatureModule;

	// Token: 0x04000F36 RID: 3894
	public readonly SyncListItem EquippedItems = new SyncListItem();

	// Token: 0x04000F3A RID: 3898
	public bool ShouldPersistData;

	// Token: 0x02000325 RID: 805
	// (Invoke) Token: 0x06000FA2 RID: 4002
	public delegate void OnItemEquippedEventHandler(Item item);

	// Token: 0x02000326 RID: 806
	// (Invoke) Token: 0x06000FA6 RID: 4006
	public delegate void OnItemUnequippedEventHandler(int itemId, SlotType slotType);

	// Token: 0x02000327 RID: 807
	// (Invoke) Token: 0x06000FAA RID: 4010
	public delegate void OnItemConsumedEventHandler(int amount, Item item);
}
