using System;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

// Token: 0x02000366 RID: 870
public class ItemModule : NetworkBehaviour
{
	// Token: 0x0600115C RID: 4444 RVA: 0x00052054 File Offset: 0x00050254
	private void Awake()
	{
		this.effectModule = base.GetComponent<EffectModule>();
		this.creatureModule = base.GetComponent<CreatureModule>();
		this.vocationModule = base.GetComponent<VocationModule>();
		this.attributeModule = base.GetComponent<AttributeModule>();
		this.equipmentModule = base.GetComponent<EquipmentModule>();
		this.inventoryModule = base.GetComponent<InventoryModule>();
		if (NetworkServer.active)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("DatabaseManager");
			this.itemDatabaseModule = gameObject.GetComponent<ItemDatabaseModule>();
		}
	}

	// Token: 0x0600115D RID: 4445 RVA: 0x000520C8 File Offset: 0x000502C8
	[Server]
	private void UseItem(int inventorySlotPosition)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ItemModule::UseItem(System.Int32)' called when server was not active");
			return;
		}
		Item item = this.inventoryModule.Items[inventorySlotPosition];
		if (!this.ValidateUsage(item))
		{
			return;
		}
		if (string.IsNullOrEmpty(item.Script))
		{
			item.Script = this.itemDatabaseModule.GetItemScript(item.Id);
			if (string.IsNullOrEmpty(item.Script))
			{
				return;
			}
		}
		ItemBase itemBase = this.CreateItemBehaviour(item.Script, item);
		ItemBaseConfig itemBaseConfig = new ItemBaseConfig(item, base.gameObject);
		itemBase.UseAndConsumeItem(itemBaseConfig);
	}

	// Token: 0x0600115E RID: 4446 RVA: 0x00052160 File Offset: 0x00050360
	[Server]
	private ItemBase CreateItemBehaviour(string className, Item item)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'ItemBase ItemModule::CreateItemBehaviour(System.String,Item)' called when server was not active");
			return null;
		}
		className = (string.IsNullOrEmpty(className) ? "DefaultItem" : className);
		return ClassFactory.GetFromCache<ItemBase>(className, Array.Empty<object>());
	}

	// Token: 0x0600115F RID: 4447 RVA: 0x000521AC File Offset: 0x000503AC
	[Server]
	private bool ValidateUsage(Item item)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean ItemModule::ValidateUsage(Item)' called when server was not active");
			return default(bool);
		}
		if (this.lastUsedItemId == item.Id && Time.time - this.lastUseTime < 1f)
		{
			return false;
		}
		this.lastUseTime = Time.time;
		this.lastUsedItemId = item.Id;
		if (!item.IsDefined)
		{
			return false;
		}
		if (!item.AllowUseWhileDead & !this.creatureModule.IsAlive)
		{
			this.effectModule.ShowScreenMessage("item_cant_use_while_dead_message", 3, 3.5f, Array.Empty<string>());
			return false;
		}
		if (item.RequiredVocation != Vocation.Undefined && item.RequiredVocation != this.vocationModule.Vocation)
		{
			this.effectModule.ShowScreenMessage("item_cant_use_message", 3, 3.5f, Array.Empty<string>());
			return false;
		}
		if (this.attributeModule.BaseLevel < item.RequiredLevel)
		{
			this.effectModule.ShowScreenMessage("item_low_level_message", 3, 3.5f, Array.Empty<string>());
			return false;
		}
		if (item.Equipable)
		{
			this.equipmentModule.EquipFromInventory(item.SlotPosition, true);
			return false;
		}
		return true;
	}

	// Token: 0x06001160 RID: 4448 RVA: 0x000522E0 File Offset: 0x000504E0
	[Command]
	public void CmdUseItemFromItemBar(Item itemOnItemBar)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		Mirror.GeneratedNetworkCode._Write_Item(writer, itemOnItemBar);
		base.SendCommandInternal("System.Void ItemModule::CmdUseItemFromItemBar(Item)", -945265471, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001161 RID: 4449 RVA: 0x0005231C File Offset: 0x0005051C
	[Command]
	public void CmdUseItem(int inventorySlotPosition)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(inventorySlotPosition);
		base.SendCommandInternal("System.Void ItemModule::CmdUseItem(System.Int32)", 2075149749, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001163 RID: 4451 RVA: 0x0002FEC9 File Offset: 0x0002E0C9
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x06001164 RID: 4452 RVA: 0x00052358 File Offset: 0x00050558
	protected void UserCode_CmdUseItemFromItemBar__Item(Item itemOnItemBar)
	{
		if (!itemOnItemBar.IsDefined)
		{
			return;
		}
		int num = -1;
		for (int i = 0; i < this.inventoryModule.Items.Count; i++)
		{
			if (this.inventoryModule.Items[i].IsEqual(itemOnItemBar))
			{
				num = this.inventoryModule.Items[i].SlotPosition;
			}
		}
		if (num != -1)
		{
			this.UseItem(num);
			return;
		}
		this.effectModule.ShowScreenMessage("you_dont_have_this_item_message", 3, 3.5f, Array.Empty<string>());
	}

	// Token: 0x06001165 RID: 4453 RVA: 0x000523E6 File Offset: 0x000505E6
	protected static void InvokeUserCode_CmdUseItemFromItemBar__Item(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdUseItemFromItemBar called on client.");
			return;
		}
		((ItemModule)obj).UserCode_CmdUseItemFromItemBar__Item(Mirror.GeneratedNetworkCode._Read_Item(reader));
	}

	// Token: 0x06001166 RID: 4454 RVA: 0x0005240F File Offset: 0x0005060F
	protected void UserCode_CmdUseItem__Int32(int inventorySlotPosition)
	{
		this.UseItem(inventorySlotPosition);
	}

	// Token: 0x06001167 RID: 4455 RVA: 0x00052418 File Offset: 0x00050618
	protected static void InvokeUserCode_CmdUseItem__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdUseItem called on client.");
			return;
		}
		((ItemModule)obj).UserCode_CmdUseItem__Int32(reader.ReadInt());
	}

	// Token: 0x06001168 RID: 4456 RVA: 0x00052444 File Offset: 0x00050644
	static ItemModule()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(ItemModule), "System.Void ItemModule::CmdUseItemFromItemBar(Item)", new RemoteCallDelegate(ItemModule.InvokeUserCode_CmdUseItemFromItemBar__Item), true);
		RemoteProcedureCalls.RegisterCommand(typeof(ItemModule), "System.Void ItemModule::CmdUseItem(System.Int32)", new RemoteCallDelegate(ItemModule.InvokeUserCode_CmdUseItem__Int32), true);
	}

	// Token: 0x04001055 RID: 4181
	private EffectModule effectModule;

	// Token: 0x04001056 RID: 4182
	private CreatureModule creatureModule;

	// Token: 0x04001057 RID: 4183
	private VocationModule vocationModule;

	// Token: 0x04001058 RID: 4184
	private AttributeModule attributeModule;

	// Token: 0x04001059 RID: 4185
	private EquipmentModule equipmentModule;

	// Token: 0x0400105A RID: 4186
	private InventoryModule inventoryModule;

	// Token: 0x0400105B RID: 4187
	private ItemDatabaseModule itemDatabaseModule;

	// Token: 0x0400105C RID: 4188
	private float lastUseTime;

	// Token: 0x0400105D RID: 4189
	private int lastUsedItemId;
}
