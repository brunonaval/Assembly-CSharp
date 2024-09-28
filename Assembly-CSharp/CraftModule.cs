using System;
using System.Linq;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

// Token: 0x0200030A RID: 778
public class CraftModule : NetworkBehaviour
{
	// Token: 0x06000E73 RID: 3699 RVA: 0x00044758 File Offset: 0x00042958
	private void Awake()
	{
		if (NetworkClient.active)
		{
			GameObject.FindGameObjectWithTag("UISystem").TryGetComponent<UISystemModule>(out this.uiSystemModule);
		}
		if (NetworkServer.active)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("DatabaseManager");
			gameObject.TryGetComponent<BlueprintDatabaseModule>(out this.blueprintDatabaseModule);
			gameObject.TryGetComponent<ItemDatabaseModule>(out this.itemDatabaseModule);
			base.TryGetComponent<AreaModule>(out this.areaModule);
			base.TryGetComponent<ChatModule>(out this.chatModule);
			base.TryGetComponent<EffectModule>(out this.effectModule);
			base.TryGetComponent<PlayerModule>(out this.playerModule);
			base.TryGetComponent<AttributeModule>(out this.attributeModule);
			base.TryGetComponent<InventoryModule>(out this.inventoryModule);
		}
	}

	// Token: 0x06000E74 RID: 3700 RVA: 0x000447FA File Offset: 0x000429FA
	public override void OnStartServer()
	{
		this.Blueprints.Callback += delegate(SyncList<PlayerBlueprint>.Operation op, int index, PlayerBlueprint oldItem, PlayerBlueprint newItem)
		{
			this.ShouldPersistData = true;
		};
	}

	// Token: 0x06000E75 RID: 3701 RVA: 0x00044814 File Offset: 0x00042A14
	public bool HasBlueprint(int blueprintId)
	{
		return this.Blueprints.Any((PlayerBlueprint b) => b.Blueprint.Id == blueprintId);
	}

	// Token: 0x06000E76 RID: 3702 RVA: 0x00044848 File Offset: 0x00042A48
	[Server]
	public bool LearnBlueprint(int blueprintId)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean CraftModule::LearnBlueprint(System.Int32)' called when server was not active");
			return default(bool);
		}
		Blueprint blueprint = this.blueprintDatabaseModule.GetBlueprint(blueprintId);
		if (!this.ValidateLearnBlueprint(blueprint))
		{
			return false;
		}
		PlayerBlueprint item = new PlayerBlueprint(this.playerModule.PlayerId, 0, blueprint);
		this.Blueprints.Add(item);
		return true;
	}

	// Token: 0x06000E77 RID: 3703 RVA: 0x000448B0 File Offset: 0x00042AB0
	[Server]
	private bool ValidateLearnBlueprint(Blueprint blueprint)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean CraftModule::ValidateLearnBlueprint(Blueprint)' called when server was not active");
			return default(bool);
		}
		if (this.HasBlueprint(blueprint.Id))
		{
			this.effectModule.ShowScreenMessage("craft_already_knows_blueprint_message", 0, 3.5f, Array.Empty<string>());
			return false;
		}
		if (this.attributeModule.Profession == PlayerProfession.None)
		{
			this.effectModule.ShowScreenMessage("craft_dont_have_profession_message", 0, 3.5f, Array.Empty<string>());
			return false;
		}
		if (this.attributeModule.Profession != blueprint.RequiredProfession)
		{
			this.effectModule.ShowScreenMessage("craft_profession_cant_use_blueprint_message", 0, 3.5f, Array.Empty<string>());
			return false;
		}
		if (this.attributeModule.ProfessionLevel < blueprint.RequiredProfessionLevel)
		{
			this.effectModule.ShowScreenMessage("craft_profession_level_too_low_message", 3, 3.5f, Array.Empty<string>());
			return false;
		}
		return true;
	}

	// Token: 0x06000E78 RID: 3704 RVA: 0x00044994 File Offset: 0x00042B94
	[Server]
	public void InitializeBlueprints(PlayerBlueprint[] blueprints)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void CraftModule::InitializeBlueprints(PlayerBlueprint[])' called when server was not active");
			return;
		}
		foreach (PlayerBlueprint item in blueprints)
		{
			this.Blueprints.Add(item);
		}
	}

	// Token: 0x06000E79 RID: 3705 RVA: 0x000449DC File Offset: 0x00042BDC
	[Command]
	public void CmdCraftItem(int blueprintId, int desiredItemLevel)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(blueprintId);
		writer.WriteInt(desiredItemLevel);
		base.SendCommandInternal("System.Void CraftModule::CmdCraftItem(System.Int32,System.Int32)", 1205380644, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000E7A RID: 3706 RVA: 0x00044A20 File Offset: 0x00042C20
	private void ConsumeMaterials(PlayerBlueprint playerBlueprint, Item precursor)
	{
		foreach (BlueprintMaterial blueprintMaterial in playerBlueprint.Blueprint.Materials)
		{
			if (blueprintMaterial.MaterialItem.Id != precursor.Id)
			{
				this.inventoryModule.ConsumeItem(blueprintMaterial.MaterialItem.Id, blueprintMaterial.MaterialAmount);
			}
		}
	}

	// Token: 0x06000E7B RID: 3707 RVA: 0x00044A80 File Offset: 0x00042C80
	private void ConsumePrecursor(PlayerBlueprint playerBlueprint, out Item precursor)
	{
		BlueprintMaterial blueprintMaterial = playerBlueprint.Blueprint.Materials.FirstOrDefault((BlueprintMaterial m) => m.MaterialItem.Type == playerBlueprint.Blueprint.ProducesItem.Type);
		precursor = default(Item);
		if (blueprintMaterial.MaterialItem.IsDefined)
		{
			precursor = this.inventoryModule.GetItemOrderedByQuality(blueprintMaterial.MaterialItem.Id);
			if (precursor.IsDefined)
			{
				this.inventoryModule.ConsumeItem(precursor, blueprintMaterial.MaterialAmount);
			}
		}
	}

	// Token: 0x06000E7C RID: 3708 RVA: 0x00044B0C File Offset: 0x00042D0C
	[TargetRpc]
	public void TargetShowCraftWindow(NetworkConnection target)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		this.SendTargetRPCInternal(target, "System.Void CraftModule::TargetShowCraftWindow(Mirror.NetworkConnection)", 349509054, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000E7D RID: 3709 RVA: 0x00044B3C File Offset: 0x00042D3C
	[Server]
	private bool ValidateBeforeCraft(PlayerBlueprint playerBlueprint, int desiredItemLevel)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean CraftModule::ValidateBeforeCraft(PlayerBlueprint,System.Int32)' called when server was not active");
			return default(bool);
		}
		if (!playerBlueprint.IsDefined)
		{
			this.effectModule.ShowScreenMessage("craft_not_learned_blueprint_message", 3, 3.5f, Array.Empty<string>());
			return false;
		}
		int minRequiredLevelForQuality = GlobalUtils.GetMinRequiredLevelForQuality(playerBlueprint.Blueprint.ProducesItem.Quality);
		if (desiredItemLevel < minRequiredLevelForQuality)
		{
			this.effectModule.ShowScreenMessage("desired_level_lower_than_quality_min_level", 3, 3.5f, new string[]
			{
				minRequiredLevelForQuality.ToString()
			});
			return false;
		}
		if (desiredItemLevel > this.attributeModule.ProfessionLevel)
		{
			this.effectModule.ShowScreenMessage("desired_level_higher_than_profession_message", 3, 3.5f, Array.Empty<string>());
			return false;
		}
		bool flag = true;
		foreach (BlueprintMaterial blueprintMaterial in playerBlueprint.Blueprint.Materials)
		{
			if (this.inventoryModule.GetAmount(blueprintMaterial.MaterialItem.Id) < blueprintMaterial.MaterialAmount)
			{
				flag = false;
				break;
			}
		}
		if (!flag)
		{
			this.effectModule.ShowScreenMessage("not_enough_blueprint_materials_message", 3, 3.5f, Array.Empty<string>());
			return false;
		}
		if (!this.inventoryModule.HasFreeSlots(playerBlueprint.Blueprint.ProducesItem, playerBlueprint.Blueprint.ProducesAmount))
		{
			this.effectModule.ShowScreenMessage("inventory_full_message", 3, 3.5f, Array.Empty<string>());
			return false;
		}
		return this.areaModule.AreaType == AreaType.ProtectedArea;
	}

	// Token: 0x06000E7E RID: 3710 RVA: 0x00044CB8 File Offset: 0x00042EB8
	private Item CreateItemFromBlueprint(Item precursor, PlayerBlueprint playerBlueprint, int desiredItemLevel)
	{
		Item producesItem = playerBlueprint.Blueprint.ProducesItem;
		if (producesItem.Equipable)
		{
			producesItem.RequiredLevel = desiredItemLevel;
			producesItem.Rarity = this.GetCraftItemRarity(precursor);
		}
		return producesItem;
	}

	// Token: 0x06000E7F RID: 3711 RVA: 0x00044CF4 File Offset: 0x00042EF4
	private int CalculateCraftExperience(Item producedItem, int blueprintExperience, int desiredItemLevel)
	{
		float num = 0.0133f;
		float num2 = (float)desiredItemLevel / num * 0.25f;
		float num3 = (float)blueprintExperience * this.GetExperienceRarityBonus(producedItem.Rarity);
		float num4 = (float)blueprintExperience * this.GetExperienceQualityBonus(producedItem.Quality);
		float num5 = ((float)blueprintExperience + num3 + num4 + num2) * ServerSettingsManager.CraftExperienceModifier;
		if (this.playerModule.PremiumDays > 0)
		{
			num5 += num5 * 0.5f;
		}
		return Mathf.CeilToInt(num5);
	}

	// Token: 0x06000E80 RID: 3712 RVA: 0x00044D65 File Offset: 0x00042F65
	private float GetExperienceQualityBonus(ItemQuality itemQuality)
	{
		switch (itemQuality)
		{
		case ItemQuality.Fine:
			return 0.1f;
		case ItemQuality.Masterwork:
			return 0.2f;
		case ItemQuality.Ascended:
			return 0.35f;
		case ItemQuality.Epic:
			return 0.5f;
		default:
			return 0f;
		}
	}

	// Token: 0x06000E81 RID: 3713 RVA: 0x00044D9E File Offset: 0x00042F9E
	private float GetExperienceRarityBonus(Rarity itemRarity)
	{
		switch (itemRarity)
		{
		case Rarity.Uncommon:
			return 0.1f;
		case Rarity.Rare:
			return 0.2f;
		case Rarity.Exotic:
			return 0.35f;
		default:
			return 0f;
		}
	}

	// Token: 0x06000E82 RID: 3714 RVA: 0x00044DD0 File Offset: 0x00042FD0
	private Rarity GetCraftItemRarity(Item precursor)
	{
		float num = UnityEngine.Random.Range(0f, 1f);
		Rarity rarity = Rarity.Common;
		if (num > 0.998f)
		{
			rarity = Rarity.Divine;
		}
		else if (num > 0.99f)
		{
			rarity = Rarity.Legendary;
		}
		else if (num > 0.95f)
		{
			rarity = Rarity.Exotic;
		}
		else if (num > 0.8f)
		{
			rarity = Rarity.Rare;
		}
		else if (num > 0.5f)
		{
			rarity = Rarity.Uncommon;
		}
		if (rarity < precursor.Rarity)
		{
			rarity = precursor.Rarity;
		}
		return rarity;
	}

	// Token: 0x06000E83 RID: 3715 RVA: 0x00044E3C File Offset: 0x0004303C
	[Server]
	private void UpdateBoostLevel(int inventorySlotPosition, int boostLevel)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void CraftModule::UpdateBoostLevel(System.Int32,System.Int32)' called when server was not active");
			return;
		}
		Item value = this.inventoryModule.Items[inventorySlotPosition];
		string uniqueId = value.UniqueId;
		value.BoostLevel = boostLevel;
		this.inventoryModule.Items[inventorySlotPosition] = value;
		for (int i = 0; i < this.inventoryModule.ItemBar.Count; i++)
		{
			if (this.inventoryModule.ItemBar[i].UniqueId == uniqueId)
			{
				Item value2 = this.inventoryModule.ItemBar[i];
				value2.BoostLevel = boostLevel;
				this.inventoryModule.ItemBar[i] = value2;
			}
		}
	}

	// Token: 0x06000E84 RID: 3716 RVA: 0x00044EFC File Offset: 0x000430FC
	[Server]
	private void UpdateRequiredLevel(int inventorySlotPosition, int requiredLevel)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void CraftModule::UpdateRequiredLevel(System.Int32,System.Int32)' called when server was not active");
			return;
		}
		Item value = this.inventoryModule.Items[inventorySlotPosition];
		string uniqueId = value.UniqueId;
		value.RequiredLevel = requiredLevel;
		this.inventoryModule.Items[inventorySlotPosition] = value;
		for (int i = 0; i < this.inventoryModule.ItemBar.Count; i++)
		{
			if (this.inventoryModule.ItemBar[i].UniqueId == uniqueId)
			{
				Item value2 = this.inventoryModule.ItemBar[i];
				value2.RequiredLevel = requiredLevel;
				this.inventoryModule.ItemBar[i] = value2;
			}
		}
	}

	// Token: 0x06000E85 RID: 3717 RVA: 0x00044FBC File Offset: 0x000431BC
	[Command]
	public void CmdUpgradeEquipment(string scrollUniqueId, string equipmentUniqueId)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(scrollUniqueId);
		writer.WriteString(equipmentUniqueId);
		base.SendCommandInternal("System.Void CraftModule::CmdUpgradeEquipment(System.String,System.String)", -1091072327, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000E86 RID: 3718 RVA: 0x00045000 File Offset: 0x00043200
	[Command]
	public void CmdDestructEquipment(string toolkitUniqueId, string equipmentUniqueId)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(toolkitUniqueId);
		writer.WriteString(equipmentUniqueId);
		base.SendCommandInternal("System.Void CraftModule::CmdDestructEquipment(System.String,System.String)", -2024530409, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000E87 RID: 3719 RVA: 0x00045044 File Offset: 0x00043244
	[Command]
	public void CmdBoostEquipment(string boosterUniqueId, string equipmentUniqueId, string reagentUniqueId)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(boosterUniqueId);
		writer.WriteString(equipmentUniqueId);
		writer.WriteString(reagentUniqueId);
		base.SendCommandInternal("System.Void CraftModule::CmdBoostEquipment(System.String,System.String,System.String)", 539768508, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000E88 RID: 3720 RVA: 0x00045094 File Offset: 0x00043294
	private void ProcessUpgradeSuccess(Item equipment)
	{
		int requiredLevel = equipment.RequiredLevel;
		equipment.RequiredLevel = requiredLevel + 1;
		if (equipment.RequiredLevel >= this.attributeModule.ProfessionLevel)
		{
			equipment.RequiredLevel = this.attributeModule.ProfessionLevel;
		}
		this.UpdateRequiredLevel(equipment.SlotPosition, equipment.RequiredLevel);
		this.effectModule.ShowScreenMessage("item_upgrade_success_message", 1, 3.5f, new string[]
		{
			equipment.Name,
			equipment.RequiredLevel.ToString()
		});
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "BlueStarBlast",
			EffectScaleModifier = 1f,
			EffectSpeedModifier = 0.2f,
			SoundEffectName = "blessing"
		};
		this.effectModule.ShowEffects(effectConfig);
	}

	// Token: 0x06000E89 RID: 3721 RVA: 0x00045168 File Offset: 0x00043368
	private void ProcessBoostSuccess(Item equipment)
	{
		equipment.BoostLevel++;
		this.UpdateBoostLevel(equipment.SlotPosition, equipment.BoostLevel);
		this.effectModule.ShowScreenMessage("item_boost_success_message", 1, 3.5f, new string[]
		{
			equipment.Name,
			equipment.BoostLevel.ToString()
		});
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "BlueStarBlast",
			EffectScaleModifier = 1f,
			EffectSpeedModifier = 0.2f,
			SoundEffectName = "blessing"
		};
		this.effectModule.ShowEffects(effectConfig);
	}

	// Token: 0x06000E8A RID: 3722 RVA: 0x00045210 File Offset: 0x00043410
	private void ProcessBoostFailure(Item booster, Item equipment, Item reagent)
	{
		if (booster.Type == ItemType.ItemBooster & reagent.IsDefined)
		{
			this.UpdateBoostLevel(equipment.SlotPosition, 0);
			this.effectModule.ShowScreenMessage("item_boost_sacred_failure_message", 2, 3.5f, new string[]
			{
				equipment.Name
			});
		}
		if (booster.Type == ItemType.ItemBooster & !reagent.IsDefined)
		{
			this.inventoryModule.RemoveItem(equipment.SlotPosition);
			Item item = this.itemDatabaseModule.GetItem(281);
			item.Amount = this.CalculateScrapsAmount(equipment.BoostLevel, equipment.Quality);
			this.inventoryModule.AddItem(item, true);
			this.effectModule.ShowScreenMessage("item_boost_failure_message", 3, 3.5f, new string[]
			{
				equipment.Name,
				item.Amount.ToString()
			});
		}
		if (booster.Type == ItemType.SacredItemBooster & reagent.IsDefined)
		{
			this.effectModule.ShowScreenMessage("item_boost_with_reagent_failure_message", 2, 3.5f, new string[]
			{
				equipment.Name
			});
		}
		if (booster.Type == ItemType.SacredItemBooster & !reagent.IsDefined)
		{
			this.UpdateBoostLevel(equipment.SlotPosition, 0);
			this.effectModule.ShowScreenMessage("item_boost_sacred_failure_message", 2, 3.5f, new string[]
			{
				equipment.Name
			});
		}
		this.ShowFailureEffects();
	}

	// Token: 0x06000E8B RID: 3723 RVA: 0x00045380 File Offset: 0x00043580
	private void ShowFailureEffects()
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "SmokeBomb",
			EffectScaleModifier = 1f,
			EffectSpeedModifier = 0.2f,
			SoundEffectName = "curse"
		};
		this.effectModule.ShowEffects(effectConfig);
	}

	// Token: 0x06000E8C RID: 3724 RVA: 0x000453D4 File Offset: 0x000435D4
	[TargetRpc]
	public void TargetShowItemBoostWindow(Item booster)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		Mirror.GeneratedNetworkCode._Write_Item(writer, booster);
		this.SendTargetRPCInternal(null, "System.Void CraftModule::TargetShowItemBoostWindow(Item)", -1410301256, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000E8D RID: 3725 RVA: 0x00045410 File Offset: 0x00043610
	[TargetRpc]
	public void TargetShowItemUpgradeWindow(Item scroll)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		Mirror.GeneratedNetworkCode._Write_Item(writer, scroll);
		this.SendTargetRPCInternal(null, "System.Void CraftModule::TargetShowItemUpgradeWindow(Item)", 903778801, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000E8E RID: 3726 RVA: 0x0004544C File Offset: 0x0004364C
	[TargetRpc]
	public void TargetShowItemDestructionWindow(Item toolkit)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		Mirror.GeneratedNetworkCode._Write_Item(writer, toolkit);
		this.SendTargetRPCInternal(null, "System.Void CraftModule::TargetShowItemDestructionWindow(Item)", 1965987847, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000E8F RID: 3727 RVA: 0x00045488 File Offset: 0x00043688
	private int CalculateScrapsAmount(int itemBoostLevel, ItemQuality itemQuality)
	{
		itemBoostLevel = Mathf.Max(itemBoostLevel, 1);
		switch (itemQuality)
		{
		case ItemQuality.Poor:
			return itemBoostLevel;
		case ItemQuality.Basic:
			return 2 * itemBoostLevel;
		case ItemQuality.Fine:
			return 4 * itemBoostLevel;
		case ItemQuality.Masterwork:
			return 6 * itemBoostLevel;
		case ItemQuality.Ascended:
			return 9 * itemBoostLevel;
		case ItemQuality.Epic:
			return 15 * itemBoostLevel;
		case ItemQuality.Perfect:
			return 30 * itemBoostLevel;
		case ItemQuality.Ancient:
			return 60 * itemBoostLevel;
		default:
			return itemBoostLevel;
		}
	}

	// Token: 0x06000E90 RID: 3728 RVA: 0x000454E9 File Offset: 0x000436E9
	public CraftModule()
	{
		base.InitSyncObject(this.Blueprints);
	}

	// Token: 0x06000E92 RID: 3730 RVA: 0x0002FEC9 File Offset: 0x0002E0C9
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x06000E93 RID: 3731 RVA: 0x00045514 File Offset: 0x00043714
	protected void UserCode_CmdCraftItem__Int32__Int32(int blueprintId, int desiredItemLevel)
	{
		PlayerBlueprint playerBlueprint = this.Blueprints.FirstOrDefault((PlayerBlueprint b) => b.Blueprint.Id == blueprintId);
		if (!this.ValidateBeforeCraft(playerBlueprint, desiredItemLevel))
		{
			return;
		}
		Item precursor;
		this.ConsumePrecursor(playerBlueprint, out precursor);
		this.ConsumeMaterials(playerBlueprint, precursor);
		Item item = this.CreateItemFromBlueprint(precursor, playerBlueprint, desiredItemLevel);
		this.inventoryModule.AddItem(item, -1, playerBlueprint.Blueprint.ProducesAmount, true);
		int num = this.CalculateCraftExperience(item, playerBlueprint.Blueprint.Experience, desiredItemLevel);
		this.attributeModule.AddProfessionExperience((long)num, true);
		this.chatModule.SendSystemTranslatedMessage("profession_experience_gain_message", string.Empty, false, new string[]
		{
			num.ToString()
		});
	}

	// Token: 0x06000E94 RID: 3732 RVA: 0x000455CF File Offset: 0x000437CF
	protected static void InvokeUserCode_CmdCraftItem__Int32__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdCraftItem called on client.");
			return;
		}
		((CraftModule)obj).UserCode_CmdCraftItem__Int32__Int32(reader.ReadInt(), reader.ReadInt());
	}

	// Token: 0x06000E95 RID: 3733 RVA: 0x000455FE File Offset: 0x000437FE
	protected void UserCode_TargetShowCraftWindow__NetworkConnection(NetworkConnection target)
	{
		this.uiSystemModule.ShowCraftWindow();
	}

	// Token: 0x06000E96 RID: 3734 RVA: 0x0004560B File Offset: 0x0004380B
	protected static void InvokeUserCode_TargetShowCraftWindow__NetworkConnection(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetShowCraftWindow called on server.");
			return;
		}
		((CraftModule)obj).UserCode_TargetShowCraftWindow__NetworkConnection(null);
	}

	// Token: 0x06000E97 RID: 3735 RVA: 0x00045630 File Offset: 0x00043830
	protected void UserCode_CmdUpgradeEquipment__String__String(string scrollUniqueId, string equipmentUniqueId)
	{
		Item item = this.inventoryModule.GetItem(scrollUniqueId);
		if (!item.IsDefined)
		{
			return;
		}
		if (!item.IsItemBooster)
		{
			return;
		}
		Item item2 = this.inventoryModule.GetItem(equipmentUniqueId);
		if (!item2.IsDefined)
		{
			return;
		}
		if (!item2.Equipable)
		{
			return;
		}
		if (item2.RequiredLevel >= this.attributeModule.ProfessionLevel)
		{
			return;
		}
		this.inventoryModule.ConsumeItem(item, 1);
		this.ProcessUpgradeSuccess(item2);
	}

	// Token: 0x06000E98 RID: 3736 RVA: 0x000456A8 File Offset: 0x000438A8
	protected static void InvokeUserCode_CmdUpgradeEquipment__String__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdUpgradeEquipment called on client.");
			return;
		}
		((CraftModule)obj).UserCode_CmdUpgradeEquipment__String__String(reader.ReadString(), reader.ReadString());
	}

	// Token: 0x06000E99 RID: 3737 RVA: 0x000456D8 File Offset: 0x000438D8
	protected void UserCode_CmdDestructEquipment__String__String(string toolkitUniqueId, string equipmentUniqueId)
	{
		Item item = this.inventoryModule.GetItem(toolkitUniqueId);
		if (!item.IsDefined)
		{
			return;
		}
		if (!item.IsToolkit)
		{
			return;
		}
		Item item2 = this.inventoryModule.GetItem(equipmentUniqueId);
		if (!item2.IsDefined)
		{
			return;
		}
		if (!item2.Equipable)
		{
			return;
		}
		Blueprint blueprintByProducesItemId = this.blueprintDatabaseModule.GetBlueprintByProducesItemId(item2.Id);
		if (blueprintByProducesItemId.Id == 0)
		{
			return;
		}
		if (item.Type != ItemType.AdvancedToolkit)
		{
			this.inventoryModule.ConsumeItem(item, 1);
		}
		this.inventoryModule.ConsumeItem(item2, 1);
		float num = (item.Type == ItemType.AdvancedToolkit) ? 0.75f : 0.3f;
		foreach (BlueprintMaterial blueprintMaterial in blueprintByProducesItemId.Materials)
		{
			Item materialItem = blueprintMaterial.MaterialItem;
			if (!materialItem.Equipable && UnityEngine.Random.Range(0f, 1f) <= num)
			{
				int maxExclusive = (item.Type == ItemType.AdvancedToolkit) ? Mathf.FloorToInt((float)blueprintMaterial.MaterialAmount * 0.5f) : Mathf.FloorToInt((float)blueprintMaterial.MaterialAmount * 0.25f);
				int num2 = (item.Type == ItemType.AdvancedToolkit) ? Mathf.FloorToInt((float)blueprintMaterial.MaterialAmount * 0.15f) : 1;
				num2 = Mathf.Max(1, num2);
				int num3 = UnityEngine.Random.Range(num2, maxExclusive);
				ChatModule chatModule = this.chatModule;
				string content = "item_received_from_destruct_message";
				string colorTag = "green";
				bool hideFromChatOverlay = false;
				string[] array = new string[3];
				array[0] = num3.ToString();
				int num4 = 1;
				string text;
				if (num3 != 1)
				{
					materialItem = blueprintMaterial.MaterialItem;
					text = materialItem.PluralName;
				}
				else
				{
					text = blueprintMaterial.MaterialItem.Name;
				}
				array[num4] = text;
				array[2] = item2.Name;
				chatModule.SendSystemTranslatedMessage(content, colorTag, hideFromChatOverlay, array);
				Item materialItem2 = blueprintMaterial.MaterialItem;
				materialItem2.RequiredLevel = 1;
				this.inventoryModule.AddItem(materialItem2, -1, num3, true);
			}
		}
		Item item3 = this.itemDatabaseModule.GetItem(281);
		int amount = this.CalculateScrapsAmount(item2.BoostLevel, item2.Quality);
		this.chatModule.SendSystemTranslatedMessage("item_received_from_destruct_message", "green", false, new string[]
		{
			amount.ToString(),
			item3.Name,
			item2.Name
		});
		this.inventoryModule.AddItem(item3, -1, amount, true);
	}

	// Token: 0x06000E9A RID: 3738 RVA: 0x00045925 File Offset: 0x00043B25
	protected static void InvokeUserCode_CmdDestructEquipment__String__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdDestructEquipment called on client.");
			return;
		}
		((CraftModule)obj).UserCode_CmdDestructEquipment__String__String(reader.ReadString(), reader.ReadString());
	}

	// Token: 0x06000E9B RID: 3739 RVA: 0x00045954 File Offset: 0x00043B54
	protected void UserCode_CmdBoostEquipment__String__String__String(string boosterUniqueId, string equipmentUniqueId, string reagentUniqueId)
	{
		Item item = this.inventoryModule.GetItem(boosterUniqueId);
		if (!item.IsDefined)
		{
			return;
		}
		if (!item.IsItemBooster)
		{
			return;
		}
		Item item2 = this.inventoryModule.GetItem(equipmentUniqueId);
		if (!item2.IsDefined)
		{
			return;
		}
		if (!item2.Equipable)
		{
			return;
		}
		if (item2.BoostLevel >= 21)
		{
			return;
		}
		Item item3 = this.inventoryModule.GetItem(reagentUniqueId);
		if (item2.Quality == ItemQuality.Ancient)
		{
			if (item.Quality != ItemQuality.Ancient)
			{
				this.effectModule.ShowScreenMessage("need_divine_booster_message", 3, 3.5f, Array.Empty<string>());
				this.ShowFailureEffects();
				return;
			}
			if (item3.IsDefined && item3.Quality != ItemQuality.Ancient)
			{
				this.effectModule.ShowScreenMessage("need_superior_reagent_message", 3, 3.5f, Array.Empty<string>());
				this.ShowFailureEffects();
				return;
			}
		}
		float num = GlobalUtils.GetItemBoostPercentSuccessChance(item2.BoostLevel + 1);
		if (item3.IsDefined)
		{
			if (item3.Type != ItemType.Reagent)
			{
				return;
			}
			num += num * 0.25f;
			num = Mathf.Min(1f, num);
			this.inventoryModule.ConsumeItem(item3, 1);
		}
		this.inventoryModule.ConsumeItem(item, 1);
		if (UnityEngine.Random.Range(0f, 1f) > num)
		{
			this.ProcessBoostFailure(item, item2, item3);
			return;
		}
		this.ProcessBoostSuccess(item2);
	}

	// Token: 0x06000E9C RID: 3740 RVA: 0x00045A98 File Offset: 0x00043C98
	protected static void InvokeUserCode_CmdBoostEquipment__String__String__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdBoostEquipment called on client.");
			return;
		}
		((CraftModule)obj).UserCode_CmdBoostEquipment__String__String__String(reader.ReadString(), reader.ReadString(), reader.ReadString());
	}

	// Token: 0x06000E9D RID: 3741 RVA: 0x00045ACD File Offset: 0x00043CCD
	protected void UserCode_TargetShowItemBoostWindow__Item(Item booster)
	{
		this.uiSystemModule.ShowItemBoostWindow(booster);
	}

	// Token: 0x06000E9E RID: 3742 RVA: 0x00045ADB File Offset: 0x00043CDB
	protected static void InvokeUserCode_TargetShowItemBoostWindow__Item(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetShowItemBoostWindow called on server.");
			return;
		}
		((CraftModule)obj).UserCode_TargetShowItemBoostWindow__Item(Mirror.GeneratedNetworkCode._Read_Item(reader));
	}

	// Token: 0x06000E9F RID: 3743 RVA: 0x00045B04 File Offset: 0x00043D04
	protected void UserCode_TargetShowItemUpgradeWindow__Item(Item scroll)
	{
		this.uiSystemModule.ShowItemUpgradeWindow(scroll);
	}

	// Token: 0x06000EA0 RID: 3744 RVA: 0x00045B12 File Offset: 0x00043D12
	protected static void InvokeUserCode_TargetShowItemUpgradeWindow__Item(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetShowItemUpgradeWindow called on server.");
			return;
		}
		((CraftModule)obj).UserCode_TargetShowItemUpgradeWindow__Item(Mirror.GeneratedNetworkCode._Read_Item(reader));
	}

	// Token: 0x06000EA1 RID: 3745 RVA: 0x00045B3B File Offset: 0x00043D3B
	protected void UserCode_TargetShowItemDestructionWindow__Item(Item toolkit)
	{
		this.uiSystemModule.ShowItemDestructionWindow(toolkit);
	}

	// Token: 0x06000EA2 RID: 3746 RVA: 0x00045B49 File Offset: 0x00043D49
	protected static void InvokeUserCode_TargetShowItemDestructionWindow__Item(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetShowItemDestructionWindow called on server.");
			return;
		}
		((CraftModule)obj).UserCode_TargetShowItemDestructionWindow__Item(Mirror.GeneratedNetworkCode._Read_Item(reader));
	}

	// Token: 0x06000EA3 RID: 3747 RVA: 0x00045B74 File Offset: 0x00043D74
	static CraftModule()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(CraftModule), "System.Void CraftModule::CmdCraftItem(System.Int32,System.Int32)", new RemoteCallDelegate(CraftModule.InvokeUserCode_CmdCraftItem__Int32__Int32), true);
		RemoteProcedureCalls.RegisterCommand(typeof(CraftModule), "System.Void CraftModule::CmdUpgradeEquipment(System.String,System.String)", new RemoteCallDelegate(CraftModule.InvokeUserCode_CmdUpgradeEquipment__String__String), true);
		RemoteProcedureCalls.RegisterCommand(typeof(CraftModule), "System.Void CraftModule::CmdDestructEquipment(System.String,System.String)", new RemoteCallDelegate(CraftModule.InvokeUserCode_CmdDestructEquipment__String__String), true);
		RemoteProcedureCalls.RegisterCommand(typeof(CraftModule), "System.Void CraftModule::CmdBoostEquipment(System.String,System.String,System.String)", new RemoteCallDelegate(CraftModule.InvokeUserCode_CmdBoostEquipment__String__String__String), true);
		RemoteProcedureCalls.RegisterRpc(typeof(CraftModule), "System.Void CraftModule::TargetShowCraftWindow(Mirror.NetworkConnection)", new RemoteCallDelegate(CraftModule.InvokeUserCode_TargetShowCraftWindow__NetworkConnection));
		RemoteProcedureCalls.RegisterRpc(typeof(CraftModule), "System.Void CraftModule::TargetShowItemBoostWindow(Item)", new RemoteCallDelegate(CraftModule.InvokeUserCode_TargetShowItemBoostWindow__Item));
		RemoteProcedureCalls.RegisterRpc(typeof(CraftModule), "System.Void CraftModule::TargetShowItemUpgradeWindow(Item)", new RemoteCallDelegate(CraftModule.InvokeUserCode_TargetShowItemUpgradeWindow__Item));
		RemoteProcedureCalls.RegisterRpc(typeof(CraftModule), "System.Void CraftModule::TargetShowItemDestructionWindow(Item)", new RemoteCallDelegate(CraftModule.InvokeUserCode_TargetShowItemDestructionWindow__Item));
	}

	// Token: 0x04000EB7 RID: 3767
	private AreaModule areaModule;

	// Token: 0x04000EB8 RID: 3768
	private ChatModule chatModule;

	// Token: 0x04000EB9 RID: 3769
	private EffectModule effectModule;

	// Token: 0x04000EBA RID: 3770
	private PlayerModule playerModule;

	// Token: 0x04000EBB RID: 3771
	private AttributeModule attributeModule;

	// Token: 0x04000EBC RID: 3772
	private InventoryModule inventoryModule;

	// Token: 0x04000EBD RID: 3773
	private UISystemModule uiSystemModule;

	// Token: 0x04000EBE RID: 3774
	private ItemDatabaseModule itemDatabaseModule;

	// Token: 0x04000EBF RID: 3775
	private BlueprintDatabaseModule blueprintDatabaseModule;

	// Token: 0x04000EC0 RID: 3776
	public readonly SyncListPlayerBlueprint Blueprints = new SyncListPlayerBlueprint();

	// Token: 0x04000EC1 RID: 3777
	public bool ShouldPersistData;
}
