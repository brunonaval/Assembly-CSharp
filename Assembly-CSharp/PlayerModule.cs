using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Mirror;
using Mirror.RemoteCalls;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x020003B4 RID: 948
public class PlayerModule : NetworkBehaviour
{
	// Token: 0x060013C7 RID: 5063 RVA: 0x000616F8 File Offset: 0x0005F8F8
	private void Awake()
	{
		GameObject.FindGameObjectWithTag("GameEnvironment").TryGetComponent<GameEnvironmentModule>(out this.gameEnvironmentModule);
		if (NetworkServer.active)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("DatabaseManager");
			gameObject.TryGetComponent<ItemDatabaseModule>(out this.itemDatabaseModule);
			gameObject.TryGetComponent<QuestDatabaseModule>(out this.questDatabaseModule);
			gameObject.TryGetComponent<SkillDatabaseModule>(out this.skillDatabaseModule);
			gameObject.TryGetComponent<MonsterDatabaseModule>(out this.monsterDatabaseModule);
			gameObject.TryGetComponent<BlueprintDatabaseModule>(out this.blueprintDatabaseModule);
		}
		base.TryGetComponent<AIModule>(out this.aiModule);
		base.TryGetComponent<PvpModule>(out this.pvpModule);
		base.TryGetComponent<AreaModule>(out this.areaModule);
		base.TryGetComponent<ItemModule>(out this.itemModule);
		base.TryGetComponent<CraftModule>(out this.craftModule);
		base.TryGetComponent<GuildModule>(out this.guildModule);
		base.TryGetComponent<TitleModule>(out this.titleModule);
		base.TryGetComponent<MarketModule>(out this.marketModule);
		base.TryGetComponent<WarehouseModule>(out this.warehouseModule);
		Rigidbody2D component = base.GetComponent<Rigidbody2D>();
		component.useAutoMass = false;
		component.constraints = RigidbodyConstraints2D.FreezeRotation;
	}

	// Token: 0x060013C8 RID: 5064 RVA: 0x000617F4 File Offset: 0x0005F9F4
	private void FixedUpdate()
	{
		if (!base.isLocalPlayer)
		{
			return;
		}
		if (!this.IsSceneLoaded)
		{
			return;
		}
		this.HandleMovement();
		this.HandleAutoDefaultAttack();
	}

	// Token: 0x060013C9 RID: 5065 RVA: 0x00061814 File Offset: 0x0005FA14
	private void Update()
	{
		if (!NetworkClient.active)
		{
			return;
		}
		this.HandleLocalHudVisibility();
		this.HandleDesktopControllers();
		this.HandleMobileControllers();
	}

	// Token: 0x060013CA RID: 5066 RVA: 0x00061830 File Offset: 0x0005FA30
	public override void OnStartClient()
	{
		if (!base.isLocalPlayer)
		{
			this.playerCollider.isTrigger = true;
		}
		base.StartCoroutine(this.PvpAura());
	}

	// Token: 0x060013CB RID: 5067 RVA: 0x00061854 File Offset: 0x0005FA54
	public override void OnStartServer()
	{
		PlayerModule.<OnStartServer>d__78 <OnStartServer>d__;
		<OnStartServer>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<OnStartServer>d__.<>4__this = this;
		<OnStartServer>d__.<>1__state = -1;
		<OnStartServer>d__.<>t__builder.Start<PlayerModule.<OnStartServer>d__78>(ref <OnStartServer>d__);
	}

	// Token: 0x060013CC RID: 5068 RVA: 0x0006188C File Offset: 0x0005FA8C
	public override void OnStartLocalPlayer()
	{
		NetworkClient.RegisterHandler<TvtEventMessage>(delegate(TvtEventMessage _)
		{
			this.uiSystemModule.ShowTvtEventWindow();
		}, true);
		NetworkClient.RegisterHandler<BossEventMessage>(delegate(BossEventMessage message)
		{
			if (this.attributeModule.BaseLevel < message.BossLevel)
			{
				return;
			}
			this.uiSystemModule.ShowBossEventWindow(message.BossName, message.BossLevel, message.BossPosition);
		}, true);
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		gameObject.TryGetComponent<UISystemModule>(out this.uiSystemModule);
		gameObject.TryGetComponent<Canvas>(out this.uiSystemCanvas);
		this.uiSystemModule.InitializeModules(base.gameObject);
		GameObject gameObject2 = GameObject.FindGameObjectWithTag("MobileHUD");
		GameObject gameObject3 = GameObject.FindGameObjectWithTag("UIHud");
		if (GlobalSettings.IsMobilePlatform)
		{
			gameObject2.TryGetComponent<Canvas>(out this.uiHudCanvas);
		}
		else
		{
			gameObject3.TryGetComponent<Canvas>(out this.uiHudCanvas);
			GameInputModule.LoadMapping();
		}
		GameObject.FindGameObjectWithTag("UITweaks").TryGetComponent<Canvas>(out this.uiTweaksCanvas);
		this.storageManager = gameObject.GetComponentInChildren<StorageManager>(true);
		this.buyMarketManager = gameObject.GetComponentInChildren<BuyMarketManager>(true);
		this.sellMarketManager = gameObject.GetComponentInChildren<SellMarketManager>(true);
		this.myOffersManager = gameObject.GetComponentInChildren<MyOffersManager>(true);
		this.uiSystemModule.PlatformChatHolderManager.OpenChannel("chat_tab_default", true, true, false, false);
		this.uiSystemModule.PlatformChatHolderManager.OpenChannel("chat_tab_global", false, true, true, false);
		this.uiSystemModule.PlatformChatHolderManager.OpenChannel("chat_tab_help", false, true, true, false);
		this.uiSystemModule.PlatformChatHolderManager.OpenChannel("chat_tab_system", false, true, false, false);
		this.uiSystemModule.ShowAllNames = SettingsManager.Instance.ShowAllNames;
		this.mainCameraObject = GameObject.FindGameObjectWithTag("MainCamera");
		if (this.mainCameraObject != null)
		{
			this.mainCamera = this.mainCameraObject.GetComponent<Camera>();
			this.mainCameraLayerMask = this.mainCamera.cullingMask;
			this.uiSystemModule.UpdateUISize();
			this.uiSystemModule.UpdateMinimapDetails();
			Camera.main.orthographicSize = GlobalUtils.GetMainCameraDistance(SettingsManager.Instance.CameraMode, false);
			this.mainCameraObject.transform.parent = base.gameObject.transform;
			this.mainCameraObject.transform.localPosition = new Vector3(0f, 0f, -10f);
		}
		this.chatModule.CmdSetShowInfoOnChat(SettingsManager.Instance.ShowInfoOnChat);
		base.StartCoroutine(this.RefreshFriendList());
		this.CmdSetPlatform(string.Format("{0}|{1}|{2}|{3}", new object[]
		{
			Application.platform,
			SystemInfo.deviceName,
			SystemInfo.deviceModel,
			SystemInfo.deviceUniqueIdentifier
		}));
		this.uiSystemModule.DeActivateMenuBackgroundImage();
		if (this.uiSystemModule.FeedbackText.text.ToLower().Contains("loading"))
		{
			this.uiSystemModule.HideFeedbackPanel();
		}
		this.uiSystemModule.ShowConditionBarWindow();
		if (!GlobalSettings.IsMobilePlatform)
		{
			this.uiSystemModule.ShowSkillBarWindow();
			this.uiSystemModule.ShowItemBarWindow();
			this.uiSystemModule.ShowChatHolder();
		}
		this.uiSystemModule.ShowHud();
		this.uiSystemModule.ShowMinimap();
		base.Invoke("ShowRenewPremiumIfNeeded", 5f);
		base.Invoke("ShowPromoPackageWindowOnClient", 7f);
		base.StartCoroutine(this.ShowWarningMessage());
	}

	// Token: 0x060013CD RID: 5069 RVA: 0x00061BBC File Offset: 0x0005FDBC
	public override void OnStopServer()
	{
		PlayerModule.<OnStopServer>d__80 <OnStopServer>d__;
		<OnStopServer>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<OnStopServer>d__.<>4__this = this;
		<OnStopServer>d__.<>1__state = -1;
		<OnStopServer>d__.<>t__builder.Start<PlayerModule.<OnStopServer>d__80>(ref <OnStopServer>d__);
	}

	// Token: 0x060013CE RID: 5070 RVA: 0x00061BF3 File Offset: 0x0005FDF3
	private void OnDestroy()
	{
		if (NetworkServer.active)
		{
			this.UnsubscribeFromServerEvents();
		}
	}

	// Token: 0x060013CF RID: 5071 RVA: 0x00061C04 File Offset: 0x0005FE04
	[Server]
	private Task LoadPlayerAsync()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Threading.Tasks.Task PlayerModule::LoadPlayerAsync()' called when server was not active");
			return null;
		}
		PlayerModule.<LoadPlayerAsync>d__82 <LoadPlayerAsync>d__;
		<LoadPlayerAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<LoadPlayerAsync>d__.<>4__this = this;
		<LoadPlayerAsync>d__.<>1__state = -1;
		<LoadPlayerAsync>d__.<>t__builder.Start<PlayerModule.<LoadPlayerAsync>d__82>(ref <LoadPlayerAsync>d__);
		return <LoadPlayerAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060013D0 RID: 5072 RVA: 0x00061C68 File Offset: 0x0005FE68
	[Server]
	public void KeepSavingPlayerData()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::KeepSavingPlayerData()' called when server was not active");
			return;
		}
		PlayerModule.<KeepSavingPlayerData>d__83 <KeepSavingPlayerData>d__;
		<KeepSavingPlayerData>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<KeepSavingPlayerData>d__.<>4__this = this;
		<KeepSavingPlayerData>d__.<>1__state = -1;
		<KeepSavingPlayerData>d__.<>t__builder.Start<PlayerModule.<KeepSavingPlayerData>d__83>(ref <KeepSavingPlayerData>d__);
	}

	// Token: 0x060013D1 RID: 5073 RVA: 0x00061CB4 File Offset: 0x0005FEB4
	[Command]
	public void CmdSavePlayerData(bool isOnline)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteBool(isOnline);
		base.SendCommandInternal("System.Void PlayerModule::CmdSavePlayerData(System.Boolean)", 1430922793, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x060013D2 RID: 5074 RVA: 0x00061CF0 File Offset: 0x0005FEF0
	public Task SavePlayerAsync(bool isOnline)
	{
		PlayerModule.<SavePlayerAsync>d__85 <SavePlayerAsync>d__;
		<SavePlayerAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SavePlayerAsync>d__.<>4__this = this;
		<SavePlayerAsync>d__.isOnline = isOnline;
		<SavePlayerAsync>d__.<>1__state = -1;
		<SavePlayerAsync>d__.<>t__builder.Start<PlayerModule.<SavePlayerAsync>d__85>(ref <SavePlayerAsync>d__);
		return <SavePlayerAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060013D3 RID: 5075 RVA: 0x00061D3C File Offset: 0x0005FF3C
	private Task SaveAccountDataAsync()
	{
		PlayerModule.<SaveAccountDataAsync>d__86 <SaveAccountDataAsync>d__;
		<SaveAccountDataAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SaveAccountDataAsync>d__.<>4__this = this;
		<SaveAccountDataAsync>d__.<>1__state = -1;
		<SaveAccountDataAsync>d__.<>t__builder.Start<PlayerModule.<SaveAccountDataAsync>d__86>(ref <SaveAccountDataAsync>d__);
		return <SaveAccountDataAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060013D4 RID: 5076 RVA: 0x00061D80 File Offset: 0x0005FF80
	private Task SavePlayerDataAsync(bool isOnline)
	{
		PlayerModule.<SavePlayerDataAsync>d__87 <SavePlayerDataAsync>d__;
		<SavePlayerDataAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SavePlayerDataAsync>d__.<>4__this = this;
		<SavePlayerDataAsync>d__.isOnline = isOnline;
		<SavePlayerDataAsync>d__.<>1__state = -1;
		<SavePlayerDataAsync>d__.<>t__builder.Start<PlayerModule.<SavePlayerDataAsync>d__87>(ref <SavePlayerDataAsync>d__);
		return <SavePlayerDataAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060013D5 RID: 5077 RVA: 0x00061DCC File Offset: 0x0005FFCC
	private Task SavePlayerQuestAsync(List<PlayerQuest> cachedPlayerQuests)
	{
		PlayerModule.<SavePlayerQuestAsync>d__88 <SavePlayerQuestAsync>d__;
		<SavePlayerQuestAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SavePlayerQuestAsync>d__.<>4__this = this;
		<SavePlayerQuestAsync>d__.cachedPlayerQuests = cachedPlayerQuests;
		<SavePlayerQuestAsync>d__.<>1__state = -1;
		<SavePlayerQuestAsync>d__.<>t__builder.Start<PlayerModule.<SavePlayerQuestAsync>d__88>(ref <SavePlayerQuestAsync>d__);
		return <SavePlayerQuestAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060013D6 RID: 5078 RVA: 0x00061E18 File Offset: 0x00060018
	private Task SavePlayerEquipmentAsync(List<Item> cachedEquippedItems)
	{
		PlayerModule.<SavePlayerEquipmentAsync>d__89 <SavePlayerEquipmentAsync>d__;
		<SavePlayerEquipmentAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SavePlayerEquipmentAsync>d__.<>4__this = this;
		<SavePlayerEquipmentAsync>d__.cachedEquippedItems = cachedEquippedItems;
		<SavePlayerEquipmentAsync>d__.<>1__state = -1;
		<SavePlayerEquipmentAsync>d__.<>t__builder.Start<PlayerModule.<SavePlayerEquipmentAsync>d__89>(ref <SavePlayerEquipmentAsync>d__);
		return <SavePlayerEquipmentAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060013D7 RID: 5079 RVA: 0x00061E64 File Offset: 0x00060064
	private Task SavePlayerInventoryAsync(List<Item> cachedInventoryItems)
	{
		PlayerModule.<SavePlayerInventoryAsync>d__90 <SavePlayerInventoryAsync>d__;
		<SavePlayerInventoryAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SavePlayerInventoryAsync>d__.<>4__this = this;
		<SavePlayerInventoryAsync>d__.cachedInventoryItems = cachedInventoryItems;
		<SavePlayerInventoryAsync>d__.<>1__state = -1;
		<SavePlayerInventoryAsync>d__.<>t__builder.Start<PlayerModule.<SavePlayerInventoryAsync>d__90>(ref <SavePlayerInventoryAsync>d__);
		return <SavePlayerInventoryAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060013D8 RID: 5080 RVA: 0x00061EB0 File Offset: 0x000600B0
	private Task SaveAccountWarehouseAsync(List<Item> cachedWarehouseItems)
	{
		PlayerModule.<SaveAccountWarehouseAsync>d__91 <SaveAccountWarehouseAsync>d__;
		<SaveAccountWarehouseAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SaveAccountWarehouseAsync>d__.<>4__this = this;
		<SaveAccountWarehouseAsync>d__.cachedWarehouseItems = cachedWarehouseItems;
		<SaveAccountWarehouseAsync>d__.<>1__state = -1;
		<SaveAccountWarehouseAsync>d__.<>t__builder.Start<PlayerModule.<SaveAccountWarehouseAsync>d__91>(ref <SaveAccountWarehouseAsync>d__);
		return <SaveAccountWarehouseAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060013D9 RID: 5081 RVA: 0x00061EFC File Offset: 0x000600FC
	private Task SavePlayerSkillBookAsync(List<Skill> cachedSkillBook)
	{
		PlayerModule.<SavePlayerSkillBookAsync>d__92 <SavePlayerSkillBookAsync>d__;
		<SavePlayerSkillBookAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SavePlayerSkillBookAsync>d__.<>4__this = this;
		<SavePlayerSkillBookAsync>d__.cachedSkillBook = cachedSkillBook;
		<SavePlayerSkillBookAsync>d__.<>1__state = -1;
		<SavePlayerSkillBookAsync>d__.<>t__builder.Start<PlayerModule.<SavePlayerSkillBookAsync>d__92>(ref <SavePlayerSkillBookAsync>d__);
		return <SavePlayerSkillBookAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060013DA RID: 5082 RVA: 0x00061F48 File Offset: 0x00060148
	private Task SavePlayerSkillBarAsync(List<Skill> cachedSkillBar, List<Skill> cachedSecondSkillBar)
	{
		PlayerModule.<SavePlayerSkillBarAsync>d__93 <SavePlayerSkillBarAsync>d__;
		<SavePlayerSkillBarAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SavePlayerSkillBarAsync>d__.<>4__this = this;
		<SavePlayerSkillBarAsync>d__.cachedSkillBar = cachedSkillBar;
		<SavePlayerSkillBarAsync>d__.cachedSecondSkillBar = cachedSecondSkillBar;
		<SavePlayerSkillBarAsync>d__.<>1__state = -1;
		<SavePlayerSkillBarAsync>d__.<>t__builder.Start<PlayerModule.<SavePlayerSkillBarAsync>d__93>(ref <SavePlayerSkillBarAsync>d__);
		return <SavePlayerSkillBarAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060013DB RID: 5083 RVA: 0x00061F9C File Offset: 0x0006019C
	private Task SavePlayerItemBarAsync(List<Item> cachedItemBar)
	{
		PlayerModule.<SavePlayerItemBarAsync>d__94 <SavePlayerItemBarAsync>d__;
		<SavePlayerItemBarAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SavePlayerItemBarAsync>d__.<>4__this = this;
		<SavePlayerItemBarAsync>d__.cachedItemBar = cachedItemBar;
		<SavePlayerItemBarAsync>d__.<>1__state = -1;
		<SavePlayerItemBarAsync>d__.<>t__builder.Start<PlayerModule.<SavePlayerItemBarAsync>d__94>(ref <SavePlayerItemBarAsync>d__);
		return <SavePlayerItemBarAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060013DC RID: 5084 RVA: 0x00061FE8 File Offset: 0x000601E8
	private Task SavePlayerConditionsAsync(List<Condition> cachedPlayerConditions)
	{
		PlayerModule.<SavePlayerConditionsAsync>d__95 <SavePlayerConditionsAsync>d__;
		<SavePlayerConditionsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SavePlayerConditionsAsync>d__.<>4__this = this;
		<SavePlayerConditionsAsync>d__.cachedPlayerConditions = cachedPlayerConditions;
		<SavePlayerConditionsAsync>d__.<>1__state = -1;
		<SavePlayerConditionsAsync>d__.<>t__builder.Start<PlayerModule.<SavePlayerConditionsAsync>d__95>(ref <SavePlayerConditionsAsync>d__);
		return <SavePlayerConditionsAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060013DD RID: 5085 RVA: 0x00062034 File Offset: 0x00060234
	private Task SavePlayerBlueprintsAsync(List<PlayerBlueprint> cachedPlayerBlueprints)
	{
		PlayerModule.<SavePlayerBlueprintsAsync>d__96 <SavePlayerBlueprintsAsync>d__;
		<SavePlayerBlueprintsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SavePlayerBlueprintsAsync>d__.<>4__this = this;
		<SavePlayerBlueprintsAsync>d__.cachedPlayerBlueprints = cachedPlayerBlueprints;
		<SavePlayerBlueprintsAsync>d__.<>1__state = -1;
		<SavePlayerBlueprintsAsync>d__.<>t__builder.Start<PlayerModule.<SavePlayerBlueprintsAsync>d__96>(ref <SavePlayerBlueprintsAsync>d__);
		return <SavePlayerBlueprintsAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060013DE RID: 5086 RVA: 0x00062080 File Offset: 0x00060280
	public void SubscribeToServerEvents()
	{
		this.combatModule.OnGotTarget += this.HandleOnGotTarget;
		this.combatModule.OnLostTarget += this.HandleOnLostTarget;
		this.combatModule.OnReceiveDamage += this.HandleOnReceiveDamage;
		this.conditionModule.OnConditionCreated += this.HandleOnConditionCreated;
		this.conditionModule.OnConditionRemoved += this.HandleOnConditionRemoved;
		this.creatureModule.OnKilled += this.HandleOnKilled;
		this.attributeModule.OnProfessionLevelUp += this.HandleOnProfessionLevelUp;
		this.attributeModule.OnBaseLevelUp += this.HandleOnBaseLevelUp;
		this.inventoryModule.OnAmountAdded += this.HandleOnAmountAdded;
		this.inventoryModule.OnSlotAdded += this.HandleOnSlotAdded;
		this.inventoryModule.OnItemAdded += this.HandleOnItemAdded;
		this.equipmentModule.OnItemConsumed += this.HandleEquipmentOnItemConsumed;
		this.storeModule.OnItemSold += this.HandleOnItemSold;
		this.storeModule.OnItemBought += this.HandleOnItemBought;
		this.storeModule.OnItemRepurchased += this.HandleOnItemRepurchased;
		this.questModule.OnQuestAccepted += this.HandleOnQuestAccepted;
		this.questModule.OnQuestUpdated += this.HandleOnQuestUpdated;
		this.questModule.OnQuestAborted += this.HandleOnQuestAborted;
		this.questModule.OnQuestCompleted += this.HandleOnQuestCompleted;
		this.chatModule.OnFriendAdded += this.HandleOnFriendAdded;
		this.chatModule.OnFriendRemoved += this.HandleOnFriendRemoved;
		this.chatModule.OnFriendBecomesOnline += this.HandleOnFriendBecomesOnline;
		this.warehouseModule.OnSlotAdded += this.HandleWarehouseOnSlotAdded;
		this.marketModule.OnBuyFromMarket += this.HandleOnBuyFromMarket;
		this.marketModule.OnBuyFromGemMarket += this.HandleOnBuyFromGemMarket;
		this.marketModule.OnTakeFromStorage += this.HandleOnTakeItemFromStorage;
		this.marketModule.OnOrderPlaced += this.HandleOnOrderPlaced;
		this.marketModule.OnOrderCanceled += this.HandleOnOrderCanceled;
		this.walletModule.OnGemAdded += this.HandleOnGemAdded;
		this.movementModule.OnSetSpawnPoint += this.HandleOnSetSpawnPoint;
	}

	// Token: 0x060013DF RID: 5087 RVA: 0x00062340 File Offset: 0x00060540
	public void UnsubscribeFromServerEvents()
	{
		this.combatModule.OnGotTarget -= this.HandleOnGotTarget;
		this.combatModule.OnLostTarget -= this.HandleOnLostTarget;
		this.combatModule.OnReceiveDamage -= this.HandleOnReceiveDamage;
		this.conditionModule.OnConditionCreated -= this.HandleOnConditionCreated;
		this.conditionModule.OnConditionRemoved -= this.HandleOnConditionRemoved;
		this.creatureModule.OnKilled -= this.HandleOnKilled;
		this.attributeModule.OnProfessionLevelUp -= this.HandleOnProfessionLevelUp;
		this.attributeModule.OnBaseLevelUp -= this.HandleOnBaseLevelUp;
		this.inventoryModule.OnAmountAdded -= this.HandleOnAmountAdded;
		this.inventoryModule.OnSlotAdded -= this.HandleOnSlotAdded;
		this.inventoryModule.OnItemAdded -= this.HandleOnItemAdded;
		this.equipmentModule.OnItemConsumed -= this.HandleEquipmentOnItemConsumed;
		this.storeModule.OnItemSold -= this.HandleOnItemSold;
		this.storeModule.OnItemBought -= this.HandleOnItemBought;
		this.storeModule.OnItemRepurchased -= this.HandleOnItemRepurchased;
		this.questModule.OnQuestAccepted -= this.HandleOnQuestAccepted;
		this.questModule.OnQuestUpdated -= this.HandleOnQuestUpdated;
		this.questModule.OnQuestAborted -= this.HandleOnQuestAborted;
		this.questModule.OnQuestCompleted -= this.HandleOnQuestCompleted;
		this.chatModule.OnFriendAdded -= this.HandleOnFriendAdded;
		this.chatModule.OnFriendRemoved -= this.HandleOnFriendRemoved;
		this.chatModule.OnFriendBecomesOnline -= this.HandleOnFriendBecomesOnline;
		this.warehouseModule.OnSlotAdded -= this.HandleWarehouseOnSlotAdded;
		this.marketModule.OnBuyFromMarket -= this.HandleOnBuyFromMarket;
		this.marketModule.OnBuyFromGemMarket -= this.HandleOnBuyFromGemMarket;
		this.marketModule.OnTakeFromStorage -= this.HandleOnTakeItemFromStorage;
		this.marketModule.OnOrderPlaced -= this.HandleOnOrderPlaced;
		this.marketModule.OnOrderCanceled -= this.HandleOnOrderCanceled;
		this.walletModule.OnGemAdded -= this.HandleOnGemAdded;
		this.movementModule.OnSetSpawnPoint -= this.HandleOnSetSpawnPoint;
	}

	// Token: 0x060013E0 RID: 5088 RVA: 0x000625FF File Offset: 0x000607FF
	[Server]
	private void HandleOnSetSpawnPoint(Vector3 spawnPoint, string spawnPointName)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::HandleOnSetSpawnPoint(UnityEngine.Vector3,System.String)' called when server was not active");
			return;
		}
		this.effectModule.ShowScreenMessage("spawn_point_changed_message", 1, 3.5f, Array.Empty<string>());
	}

	// Token: 0x060013E1 RID: 5089 RVA: 0x00062634 File Offset: 0x00060834
	[Server]
	private void HandleOnGemAdded(int totalGems, int amount)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::HandleOnGemAdded(System.Int32,System.Int32)' called when server was not active");
			return;
		}
		if (totalGems < 1)
		{
			this.effectModule.ShowScreenMessage("player_no_gem_available_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		if (totalGems == 1)
		{
			this.effectModule.ShowScreenMessage("player_one_gem_available_message", 0, 3.5f, Array.Empty<string>());
			return;
		}
		this.effectModule.ShowScreenMessage("player_gem_amount_message", 1, 3.5f, new string[]
		{
			totalGems.ToString()
		});
	}

	// Token: 0x060013E2 RID: 5090 RVA: 0x000626BC File Offset: 0x000608BC
	[Server]
	private void HandleOnConditionCreated(Condition condition)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::HandleOnConditionCreated(Condition)' called when server was not active");
			return;
		}
		if (condition.Category == ConditionCategory.Invisibility)
		{
			this.animationControllerModule.TargetSetIsInvisible(base.connectionToClient, true);
		}
		if (condition.Type == ConditionType.Freeze)
		{
			this.animationControllerModule.TargetSetColor(base.connectionToClient, 6);
		}
	}

	// Token: 0x060013E3 RID: 5091 RVA: 0x00062718 File Offset: 0x00060918
	[Server]
	private void HandleOnConditionRemoved(Condition condition)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::HandleOnConditionRemoved(Condition)' called when server was not active");
			return;
		}
		if (condition.Category == ConditionCategory.Invisibility)
		{
			this.animationControllerModule.TargetSetIsInvisible(base.connectionToClient, false);
		}
		if (condition.Type == ConditionType.Freeze)
		{
			this.animationControllerModule.TargetSetColorToOriginal(base.connectionToClient);
		}
	}

	// Token: 0x060013E4 RID: 5092 RVA: 0x00062770 File Offset: 0x00060970
	[Server]
	private void HandleOnOrderCanceled(int canceledItemId, int amount)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::HandleOnOrderCanceled(System.Int32,System.Int32)' called when server was not active");
			return;
		}
		Item item = this.itemDatabaseModule.GetItem(canceledItemId);
		string text = (amount > 1) ? "player_take_them_word" : "player_take_it_word";
		string text2 = (amount > 1) ? item.PluralName : item.Name;
		this.effectModule.ShowScreenMessage("player_removed_item_market_message", 2, 3.5f, new string[]
		{
			amount.ToString(),
			text2,
			text
		});
	}

	// Token: 0x060013E5 RID: 5093 RVA: 0x000627F4 File Offset: 0x000609F4
	[Server]
	private void HandleOnOrderPlaced(MarketOrder order, int amount, int unitValue)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::HandleOnOrderPlaced(MarketOrder,System.Int32,System.Int32)' called when server was not active");
			return;
		}
		string text = (amount > 1) ? order.Item.PluralName : order.Item.Name;
		this.effectModule.ShowScreenMessage("player_placed_item_message", 2, 3.5f, new string[]
		{
			amount.ToString(),
			text
		});
	}

	// Token: 0x060013E6 RID: 5094 RVA: 0x00062860 File Offset: 0x00060A60
	[Server]
	private void HandleOnTakeItemFromStorage(Item item, int storageId)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::HandleOnTakeItemFromStorage(Item,System.Int32)' called when server was not active");
			return;
		}
		PlayerModule.<HandleOnTakeItemFromStorage>d__105 <HandleOnTakeItemFromStorage>d__;
		<HandleOnTakeItemFromStorage>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<HandleOnTakeItemFromStorage>d__.<>4__this = this;
		<HandleOnTakeItemFromStorage>d__.<>1__state = -1;
		<HandleOnTakeItemFromStorage>d__.<>t__builder.Start<PlayerModule.<HandleOnTakeItemFromStorage>d__105>(ref <HandleOnTakeItemFromStorage>d__);
	}

	// Token: 0x060013E7 RID: 5095 RVA: 0x000628AC File Offset: 0x00060AAC
	[Server]
	private void HandleOnBuyFromGemMarket(int boughtItemId, int amount, int unitValue)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::HandleOnBuyFromGemMarket(System.Int32,System.Int32,System.Int32)' called when server was not active");
			return;
		}
		Item item = this.itemDatabaseModule.GetItem(boughtItemId);
		string text = (amount > 1) ? "player_take_them_word" : "player_take_it_word";
		string text2 = (amount > 1) ? item.PluralName : item.Name;
		this.effectModule.ShowScreenMessage("player_bought_gem_market_message", 1, 3.5f, new string[]
		{
			amount.ToString(),
			text2,
			text
		});
	}

	// Token: 0x060013E8 RID: 5096 RVA: 0x00062930 File Offset: 0x00060B30
	[Server]
	private void HandleOnBuyFromMarket(int boughtItemId, int amount, int unitValue)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::HandleOnBuyFromMarket(System.Int32,System.Int32,System.Int32)' called when server was not active");
			return;
		}
		Item item = this.itemDatabaseModule.GetItem(boughtItemId);
		string text = (amount > 1) ? "player_take_them_word" : "player_take_it_word";
		string text2 = (amount > 1) ? item.PluralName : item.Name;
		this.effectModule.ShowScreenMessage("player_bought_market_message", 2, 3.5f, new string[]
		{
			amount.ToString(),
			text2,
			text
		});
	}

	// Token: 0x060013E9 RID: 5097 RVA: 0x000629B2 File Offset: 0x00060BB2
	[Server]
	private void HandleOnSlotAdded(int amount, int totalSlots)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::HandleOnSlotAdded(System.Int32,System.Int32)' called when server was not active");
			return;
		}
		this.effectModule.ShowScreenMessage("player_inventory_slots_message", 0, 3.5f, new string[]
		{
			totalSlots.ToString()
		});
	}

	// Token: 0x060013EA RID: 5098 RVA: 0x000629EF File Offset: 0x00060BEF
	[Server]
	private void HandleWarehouseOnSlotAdded(int amount, int totalSlots)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::HandleWarehouseOnSlotAdded(System.Int32,System.Int32)' called when server was not active");
			return;
		}
		this.effectModule.ShowScreenMessage("player_warehouse_slots_message", 0, 3.5f, new string[]
		{
			totalSlots.ToString()
		});
	}

	// Token: 0x060013EB RID: 5099 RVA: 0x00062A2C File Offset: 0x00060C2C
	[Server]
	private void HandleOnFriendRemoved(AccountFriend friend)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::HandleOnFriendRemoved(AccountFriend)' called when server was not active");
			return;
		}
		PlayerModule.<HandleOnFriendRemoved>d__110 <HandleOnFriendRemoved>d__;
		<HandleOnFriendRemoved>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<HandleOnFriendRemoved>d__.<>4__this = this;
		<HandleOnFriendRemoved>d__.friend = friend;
		<HandleOnFriendRemoved>d__.<>1__state = -1;
		<HandleOnFriendRemoved>d__.<>t__builder.Start<PlayerModule.<HandleOnFriendRemoved>d__110>(ref <HandleOnFriendRemoved>d__);
	}

	// Token: 0x060013EC RID: 5100 RVA: 0x00062A80 File Offset: 0x00060C80
	[Server]
	private void HandleOnFriendAdded(AccountFriend friend)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::HandleOnFriendAdded(AccountFriend)' called when server was not active");
			return;
		}
		PlayerModule.<HandleOnFriendAdded>d__111 <HandleOnFriendAdded>d__;
		<HandleOnFriendAdded>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<HandleOnFriendAdded>d__.<>4__this = this;
		<HandleOnFriendAdded>d__.friend = friend;
		<HandleOnFriendAdded>d__.<>1__state = -1;
		<HandleOnFriendAdded>d__.<>t__builder.Start<PlayerModule.<HandleOnFriendAdded>d__111>(ref <HandleOnFriendAdded>d__);
	}

	// Token: 0x060013ED RID: 5101 RVA: 0x00062AD4 File Offset: 0x00060CD4
	[Server]
	private void HandleOnFriendBecomesOnline(AccountFriend friend)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::HandleOnFriendBecomesOnline(AccountFriend)' called when server was not active");
			return;
		}
		this.effectModule.ShowScreenMessage("friend_logged_in_message", 1, 3.5f, new string[]
		{
			friend.FriendName
		});
	}

	// Token: 0x060013EE RID: 5102 RVA: 0x00062B10 File Offset: 0x00060D10
	[Server]
	private void HandleOnQuestCompleted(PlayerQuest playerQuest)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::HandleOnQuestCompleted(PlayerQuest)' called when server was not active");
			return;
		}
		PlayerModule.<HandleOnQuestCompleted>d__113 <HandleOnQuestCompleted>d__;
		<HandleOnQuestCompleted>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<HandleOnQuestCompleted>d__.<>4__this = this;
		<HandleOnQuestCompleted>d__.playerQuest = playerQuest;
		<HandleOnQuestCompleted>d__.<>1__state = -1;
		<HandleOnQuestCompleted>d__.<>t__builder.Start<PlayerModule.<HandleOnQuestCompleted>d__113>(ref <HandleOnQuestCompleted>d__);
	}

	// Token: 0x060013EF RID: 5103 RVA: 0x00062B64 File Offset: 0x00060D64
	private IEnumerator SearchForNewQuests()
	{
		yield return new WaitForSecondsRealtime(5f);
		this.questModule.FindNewQuest(false);
		yield break;
	}

	// Token: 0x060013F0 RID: 5104 RVA: 0x00062B74 File Offset: 0x00060D74
	[Server]
	private void HandleOnQuestAborted(PlayerQuest playerQuest)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::HandleOnQuestAborted(PlayerQuest)' called when server was not active");
			return;
		}
		PlayerModule.<HandleOnQuestAborted>d__115 <HandleOnQuestAborted>d__;
		<HandleOnQuestAborted>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<HandleOnQuestAborted>d__.<>4__this = this;
		<HandleOnQuestAborted>d__.playerQuest = playerQuest;
		<HandleOnQuestAborted>d__.<>1__state = -1;
		<HandleOnQuestAborted>d__.<>t__builder.Start<PlayerModule.<HandleOnQuestAborted>d__115>(ref <HandleOnQuestAborted>d__);
	}

	// Token: 0x060013F1 RID: 5105 RVA: 0x00062BC8 File Offset: 0x00060DC8
	[Server]
	private void HandleOnQuestUpdated(PlayerQuest playerQuest, PlayerQuestObjective playerQuestObjective, int updateAmount, ObjectiveType objectiveType)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::HandleOnQuestUpdated(PlayerQuest,PlayerQuestObjective,System.Int32,ObjectiveType)' called when server was not active");
			return;
		}
		if (objectiveType != ObjectiveType.CollectItems)
		{
			if (objectiveType == ObjectiveType.KillMonsters)
			{
				Monster monster = this.monsterDatabaseModule.GetMonster(playerQuestObjective.ObjectiveId);
				if (monster.IsDefined)
				{
					string creatureRankName = GlobalUtils.RankToString(playerQuestObjective.ObjectiveRank);
					int left = playerQuestObjective.ObjectiveAmount - playerQuestObjective.ObjectiveProgress;
					this.chatModule.TargetSendSystemQuestUpdateKillMessage(base.connectionToClient, playerQuest.Name, left, creatureRankName, monster.Name, monster.PluralName);
				}
			}
		}
		else
		{
			Item item = this.itemDatabaseModule.GetItem(playerQuestObjective.ObjectiveId);
			if (item.IsDefined)
			{
				int left2 = playerQuestObjective.ObjectiveAmount - playerQuestObjective.ObjectiveProgress;
				this.chatModule.TargetSendSystemQuestUpdateCollectMessage(base.connectionToClient, playerQuest.Name, left2, item.Name, item.PluralName);
			}
		}
		if (playerQuest.Objectives.Any((PlayerQuestObjective o) => o.ObjectiveProgress < o.ObjectiveAmount))
		{
			this.questModule.TrackQuestObjectives(playerQuest.QuestId);
			return;
		}
		if (this.questModule.CanCompleteQuest(playerQuest.QuestId))
		{
			this.questModule.DoCompleteQuest(this.PlayerId, playerQuest.QuestId);
		}
		this.chatModule.TargetSendSystemQuestReadyMessage(base.connectionToClient);
		if (playerQuest.QuestId == 39)
		{
			this.effectModule.ShowScreenMessage("quest_all_ready_message", 1, 7f, Array.Empty<string>());
		}
	}

	// Token: 0x060013F2 RID: 5106 RVA: 0x00062D44 File Offset: 0x00060F44
	[Server]
	private void HandleOnQuestAccepted(Quest quest)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::HandleOnQuestAccepted(Quest)' called when server was not active");
			return;
		}
		PlayerModule.<HandleOnQuestAccepted>d__117 <HandleOnQuestAccepted>d__;
		<HandleOnQuestAccepted>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<HandleOnQuestAccepted>d__.<>4__this = this;
		<HandleOnQuestAccepted>d__.quest = quest;
		<HandleOnQuestAccepted>d__.<>1__state = -1;
		<HandleOnQuestAccepted>d__.<>t__builder.Start<PlayerModule.<HandleOnQuestAccepted>d__117>(ref <HandleOnQuestAccepted>d__);
	}

	// Token: 0x060013F3 RID: 5107 RVA: 0x00062D98 File Offset: 0x00060F98
	[Server]
	private void HandleOnItemRepurchased(int itemId, int buyAmount, int unitValue)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::HandleOnItemRepurchased(System.Int32,System.Int32,System.Int32)' called when server was not active");
			return;
		}
		this.TargetReloadStoreItems(this.effectModule.connectionToClient);
		this.effectModule.TargetShowAnimatedText(string.Format("-${0}", buyAmount * unitValue), 3, true, base.transform.position);
	}

	// Token: 0x060013F4 RID: 5108 RVA: 0x00062DF8 File Offset: 0x00060FF8
	[Server]
	private void HandleOnItemBought(int itemId, int buyAmount, int unitValue)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::HandleOnItemBought(System.Int32,System.Int32,System.Int32)' called when server was not active");
			return;
		}
		this.effectModule.TargetShowAnimatedText(string.Format("-${0}", buyAmount * unitValue), 3, true, base.transform.position);
	}

	// Token: 0x060013F5 RID: 5109 RVA: 0x00062E44 File Offset: 0x00061044
	[Server]
	private void HandleOnItemSold(string itemUniqueId, int sellAmount, int unitValue)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::HandleOnItemSold(System.String,System.Int32,System.Int32)' called when server was not active");
			return;
		}
		this.TargetReloadStoreItems(this.effectModule.connectionToClient);
		this.effectModule.TargetShowAnimatedText(string.Format("+${0}", sellAmount * unitValue), 1, true, base.transform.position);
	}

	// Token: 0x060013F6 RID: 5110 RVA: 0x00062EA1 File Offset: 0x000610A1
	[Server]
	private void HandleOnLostTarget(GameObject target)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::HandleOnLostTarget(UnityEngine.GameObject)' called when server was not active");
			return;
		}
		this.TargetHideSelectedTargetHud(base.connectionToClient);
	}

	// Token: 0x060013F7 RID: 5111 RVA: 0x00062EC4 File Offset: 0x000610C4
	[Server]
	private void HandleOnGotTarget(GameObject target)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::HandleOnGotTarget(UnityEngine.GameObject)' called when server was not active");
			return;
		}
		if (!base.isServer)
		{
			return;
		}
		this.TargetShowSelectedTargetHud(base.connectionToClient, target);
	}

	// Token: 0x060013F8 RID: 5112 RVA: 0x00062EF4 File Offset: 0x000610F4
	[Server]
	private void HandleEquipmentOnItemConsumed(int amount, Item item)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::HandleEquipmentOnItemConsumed(System.Int32,Item)' called when server was not active");
			return;
		}
		if (item.SlotType == SlotType.Ammo)
		{
			Item item2 = this.equipmentModule.GetItem(item.SlotType);
			if (item2.ConsumeProjectile & item2.Amount <= 15)
			{
				string text = (item2.Amount == 1) ? item2.Name : item2.PluralName;
				this.effectModule.ShowScreenMessage("player_item_consumed_message", 3, 3.5f, new string[]
				{
					item2.Amount.ToString(),
					text
				});
			}
		}
	}

	// Token: 0x060013F9 RID: 5113 RVA: 0x00062F91 File Offset: 0x00061191
	[Server]
	private void HandleOnAmountAdded(int amount, int slotPosition, Item item)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::HandleOnAmountAdded(System.Int32,System.Int32,Item)' called when server was not active");
			return;
		}
		if (amount > 0)
		{
			this.questModule.UpdateQuest(item.Id, amount, ObjectiveType.CollectItems, Rank.None);
		}
	}

	// Token: 0x060013FA RID: 5114 RVA: 0x00062FC0 File Offset: 0x000611C0
	[Server]
	private void HandleOnItemAdded(int itemId, int slotPosition, int amount)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::HandleOnItemAdded(System.Int32,System.Int32,System.Int32)' called when server was not active");
			return;
		}
		this.questModule.UpdateQuest(itemId, amount, ObjectiveType.CollectItems, Rank.None);
	}

	// Token: 0x060013FB RID: 5115 RVA: 0x00062FE8 File Offset: 0x000611E8
	[Server]
	private void HandleOnProfessionLevelUp(int levelsGain)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::HandleOnProfessionLevelUp(System.Int32)' called when server was not active");
			return;
		}
		if (levelsGain == 0)
		{
			return;
		}
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "StarBlast",
			EffectScaleModifier = 2f,
			EffectSpeedModifier = 0.2f,
			SoundEffectName = "magic_explosion"
		};
		this.effectModule.ShowEffects(effectConfig);
		this.effectModule.ShowScreenMessage("player_profession_level_advanced_message", 1, 3.5f, new string[]
		{
			this.attributeModule.ProfessionLevel.ToString()
		});
	}

	// Token: 0x060013FC RID: 5116 RVA: 0x00063084 File Offset: 0x00061284
	[Server]
	private void HandleOnBaseLevelUp(int levelsGain)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::HandleOnBaseLevelUp(System.Int32)' called when server was not active");
			return;
		}
		PlayerModule.<HandleOnBaseLevelUp>d__127 <HandleOnBaseLevelUp>d__;
		<HandleOnBaseLevelUp>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<HandleOnBaseLevelUp>d__.<>4__this = this;
		<HandleOnBaseLevelUp>d__.levelsGain = levelsGain;
		<HandleOnBaseLevelUp>d__.<>1__state = -1;
		<HandleOnBaseLevelUp>d__.<>t__builder.Start<PlayerModule.<HandleOnBaseLevelUp>d__127>(ref <HandleOnBaseLevelUp>d__);
	}

	// Token: 0x060013FD RID: 5117 RVA: 0x000630D8 File Offset: 0x000612D8
	private void ShowRateGameWindowOnLevelUp()
	{
		if (this.attributeModule.BaseLevel < 10)
		{
			return;
		}
		if (this.attributeModule.BaseLevel % 25 != 0)
		{
			return;
		}
		this.TargetShowRateGameWindow();
	}

	// Token: 0x060013FE RID: 5118 RVA: 0x00063104 File Offset: 0x00061304
	[Command]
	public void CmdRateGame(int totalStars)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(totalStars);
		base.SendCommandInternal("System.Void PlayerModule::CmdRateGame(System.Int32)", 1449643309, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x060013FF RID: 5119 RVA: 0x0006313E File Offset: 0x0006133E
	[Server]
	private void ShowHelpMessagesOnLowLevels()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::ShowHelpMessagesOnLowLevels()' called when server was not active");
			return;
		}
		if (this.attributeModule.BaseLevel == 2)
		{
			this.effectModule.ShowScreenMessage("player_attribute_points_message", 1, 7f, Array.Empty<string>());
		}
	}

	// Token: 0x06001400 RID: 5120 RVA: 0x00063180 File Offset: 0x00061380
	[Server]
	private void HandleOnKilled(GameObject killer, List<Attacker> attackers)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::HandleOnKilled(UnityEngine.GameObject,System.Collections.Generic.List`1<Attacker>)' called when server was not active");
			return;
		}
		PlayerModule.<HandleOnKilled>d__131 <HandleOnKilled>d__;
		<HandleOnKilled>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<HandleOnKilled>d__.<>4__this = this;
		<HandleOnKilled>d__.killer = killer;
		<HandleOnKilled>d__.attackers = attackers;
		<HandleOnKilled>d__.<>1__state = -1;
		<HandleOnKilled>d__.<>t__builder.Start<PlayerModule.<HandleOnKilled>d__131>(ref <HandleOnKilled>d__);
	}

	// Token: 0x06001401 RID: 5121 RVA: 0x000631DC File Offset: 0x000613DC
	private void RestrictPlayerIfDiedTooMuchForSamePlayerOnTvT(GameObject killer)
	{
		if (this.pvpModule.TvtTeam == TvtTeam.None)
		{
			return;
		}
		if (this.pvpModule.TeamFightDeaths.Count == 0)
		{
			return;
		}
		this.pvpModule.TeamFightDeaths.Reverse();
		if ((from p in this.pvpModule.TeamFightDeaths.Take(5)
		group p by p.Key into p
		select new
		{
			PlayerId = p.Key,
			Deaths = p.Count<KeyValuePair<int, float>>()
		}).Max(p => p.Deaths) >= 3)
		{
			this.conditionModule.StartCondition(new Condition(ConditionCategory.Paralyzing, ConditionType.Restricted, 30f, 30f, 0f, new Effect("BlueStars", 0.5f, 0.5f), 0, 0f, ""), killer, false);
		}
	}

	// Token: 0x06001402 RID: 5122 RVA: 0x000632E0 File Offset: 0x000614E0
	private Task AddPlayerDeathHistoryAsync(GameObject killer, long baseExpLoss)
	{
		PlayerModule.<AddPlayerDeathHistoryAsync>d__133 <AddPlayerDeathHistoryAsync>d__;
		<AddPlayerDeathHistoryAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<AddPlayerDeathHistoryAsync>d__.<>4__this = this;
		<AddPlayerDeathHistoryAsync>d__.killer = killer;
		<AddPlayerDeathHistoryAsync>d__.baseExpLoss = baseExpLoss;
		<AddPlayerDeathHistoryAsync>d__.<>1__state = -1;
		<AddPlayerDeathHistoryAsync>d__.<>t__builder.Start<PlayerModule.<AddPlayerDeathHistoryAsync>d__133>(ref <AddPlayerDeathHistoryAsync>d__);
		return <AddPlayerDeathHistoryAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06001403 RID: 5123 RVA: 0x00063334 File Offset: 0x00061534
	private void CalculateAndDropItems()
	{
		for (int i = 0; i < this.equipmentModule.EquippedItems.Count; i++)
		{
			if (this.CanDropItemOnDeath(this.equipmentModule.EquippedItems[i]) && UnityEngine.Random.Range(0f, 1f) <= this.pvpModule.EquipmentDropChance)
			{
				this.equipmentModule.UnequipToGround(this.equipmentModule.EquippedItems[i].SlotType, base.transform.position, true, true);
				this.effectModule.ShowScreenMessage("player_drop_item_message", 3, 3.5f, new string[]
				{
					this.equipmentModule.EquippedItems[i].Name
				});
			}
		}
	}

	// Token: 0x06001404 RID: 5124 RVA: 0x000633FA File Offset: 0x000615FA
	private bool CanDropItemOnDeath(Item item)
	{
		return item.Category == ItemCategory.Armor || item.Category == ItemCategory.Weapon;
	}

	// Token: 0x06001405 RID: 5125 RVA: 0x00063410 File Offset: 0x00061610
	private void IncreaseKillerScoresAndSetPvpStatus(GameObject killer)
	{
		if (killer == null)
		{
			return;
		}
		if (killer.CompareTag("Player"))
		{
			if (this.pvpModule.PvpStatus == PvpStatus.Neutral)
			{
				PvpModule component = killer.GetComponent<PvpModule>();
				AttributeModule component2 = killer.GetComponent<AttributeModule>();
				if (component != null)
				{
					int num = component.KillScore;
					num++;
					PvpStatus pvpStatus = PvpStatus.PlayerKiller;
					if (num >= 15)
					{
						pvpStatus = PvpStatus.Outlaw;
					}
					if (pvpStatus > component.PvpStatus)
					{
						component.SetPvpStatusAsync(pvpStatus, true);
					}
					int num2 = Mathf.CeilToInt((float)(component2.BaseExperienceToLevel - component2.BaseExperienceToCurrentLevel) * 0.1f);
					num2 += Mathf.RoundToInt((float)(num2 * num) * 0.05f);
					int num3 = component.KarmaPoints;
					num3 += num2;
					component.SetKarmaPoints(num3);
					component.SetKillScore(num);
					return;
				}
			}
			else
			{
				PvpModule component3 = killer.GetComponent<PvpModule>();
				if (component3 != null)
				{
					int num4 = component3.CombatScore;
					num4++;
					component3.SetCombatScore(num4);
				}
			}
		}
	}

	// Token: 0x06001406 RID: 5126 RVA: 0x00063500 File Offset: 0x00061700
	private void CalculateAndRemoveExperience(out long baseExpLoss)
	{
		baseExpLoss = 0L;
		if (this.conditionModule.HasActiveCondition(ConditionType.ExpProtection))
		{
			this.conditionModule.RemoveCondition(ConditionType.ExpProtection);
			return;
		}
		long num = this.attributeModule.BaseExperienceToLevel - this.attributeModule.BaseExperienceToCurrentLevel;
		baseExpLoss = (long)Mathf.CeilToInt((float)num * 0.25f);
		this.attributeModule.RemoveBaseExperience(baseExpLoss, true);
		foreach (global::Attribute attribute in this.attributeModule.Attributes)
		{
			int num2 = Mathf.CeilToInt((float)(attribute.ExperienceToLevel - attribute.ExperienceToCurrentLevel) * 0.25f);
			this.attributeModule.RemoveExperience(attribute.Type, (long)num2);
		}
	}

	// Token: 0x06001407 RID: 5127 RVA: 0x000635D8 File Offset: 0x000617D8
	private void StopAutoRespawn()
	{
		if (this.autoRespawnOnDeath != null)
		{
			base.StopCoroutine(this.autoRespawnOnDeath);
			this.autoRespawnOnDeath = null;
		}
	}

	// Token: 0x06001408 RID: 5128 RVA: 0x000635F8 File Offset: 0x000617F8
	public void AddToTeamFightAssist(GameObject assistant, int amount)
	{
		PvpModule pvpModule;
		if (!assistant.TryGetComponent<PvpModule>(out pvpModule))
		{
			return;
		}
		if (amount > 0)
		{
			if (pvpModule.TvtTeam == TvtTeam.None)
			{
				return;
			}
			if (pvpModule.TvtTeam == this.pvpModule.TvtTeam)
			{
				return;
			}
			if (this.areaModule.CurrentAreaExtraParam == "tvt-base")
			{
				return;
			}
		}
		PvpModule pvpModule2 = pvpModule;
		pvpModule2.NetworkTeamFightAssist = pvpModule2.TeamFightAssist + amount;
		if (pvpModule.TvtTeam == TvtTeam.RedTeam)
		{
			NpcNorumbria.RedTeamAssists += amount;
		}
		if (pvpModule.TvtTeam == TvtTeam.BlueTeam)
		{
			NpcNorumbria.BlueTeamAsssits += amount;
		}
	}

	// Token: 0x06001409 RID: 5129 RVA: 0x00063684 File Offset: 0x00061884
	public void AddToTeamFightScore(GameObject killer, int amount)
	{
		PvpModule pvpModule;
		if (!killer.TryGetComponent<PvpModule>(out pvpModule))
		{
			return;
		}
		PlayerModule playerModule;
		if (!killer.TryGetComponent<PlayerModule>(out playerModule))
		{
			return;
		}
		if (amount > 0)
		{
			if (pvpModule.TvtTeam == TvtTeam.None)
			{
				return;
			}
			if (pvpModule.TvtTeam == this.pvpModule.TvtTeam)
			{
				return;
			}
			if (this.areaModule.CurrentAreaExtraParam == "tvt-base")
			{
				return;
			}
		}
		PvpModule pvpModule2 = pvpModule;
		pvpModule2.NetworkTeamFightScore = pvpModule2.TeamFightScore + amount;
		if (amount > 0)
		{
			this.pvpModule.TeamFightDeaths.Add(new KeyValuePair<int, float>(playerModule.PlayerId, Time.time));
		}
		if (pvpModule.TvtTeam == TvtTeam.RedTeam)
		{
			NpcNorumbria.RedTeamScore += amount;
		}
		if (pvpModule.TvtTeam == TvtTeam.BlueTeam)
		{
			NpcNorumbria.BlueTeamScore += amount;
		}
	}

	// Token: 0x0600140A RID: 5130 RVA: 0x0006373C File Offset: 0x0006193C
	[Server]
	private void HandleOnReceiveDamage(GameObject attacker, int damage)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::HandleOnReceiveDamage(UnityEngine.GameObject,System.Int32)' called when server was not active");
			return;
		}
		if (attacker == null)
		{
			return;
		}
		if (attacker.CompareTag("Player") && attacker != base.gameObject)
		{
			PvpModule component = attacker.GetComponent<PvpModule>();
			if (component != null && component.PvpStatus == PvpStatus.Neutral)
			{
				component.SetPvpStatusAsync(PvpStatus.InCombat, true);
			}
			else if (component != null && component.PvpStatus == PvpStatus.InCombat)
			{
				component.SetInCombatTime(Time.time);
			}
		}
		if (damage > 0)
		{
			CreatureModule component2 = attacker.GetComponent<CreatureModule>();
			ChatModule component3 = attacker.GetComponent<ChatModule>();
			if (component2 != null)
			{
				this.chatModule.SendSystemReceiveDamageMessage(component2.CreatureName, component2.AllowRankNamePrefix, component2.Rank, damage);
			}
			if (component3 != null)
			{
				component3.SendSystemApplyDamageMessage(this.creatureModule.CreatureName, this.creatureModule.AllowRankNamePrefix, this.creatureModule.Rank, damage);
			}
			if (component2.IsAlive & this.combatModule.Target == null)
			{
				this.combatModule.SetTarget(attacker, false);
			}
		}
	}

	// Token: 0x0600140B RID: 5131 RVA: 0x00063858 File Offset: 0x00061A58
	[Client]
	private void HandleAutoDefaultAttack()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void PlayerModule::HandleAutoDefaultAttack()' called when client was not active");
			return;
		}
		if (Time.fixedTime - this.lastAutoAttackTime < 0.03f)
		{
			return;
		}
		if (!this.AutoAttackEnabled)
		{
			return;
		}
		if (this.pvpModule.PvpEnabled)
		{
			return;
		}
		if (this.pvpModule.PvpStatus != PvpStatus.Neutral)
		{
			return;
		}
		if (this.areaModule.AreaType == AreaType.ProtectedArea)
		{
			return;
		}
		this.lastAutoAttackTime = Time.fixedTime;
		Skill skill = this.skillModule.SkillBook.FirstOrDefault((Skill s) => s.IsDefaultSkill);
		this.CastSkill(skill.Id);
	}

	// Token: 0x0600140C RID: 5132 RVA: 0x00063908 File Offset: 0x00061B08
	[Client]
	private void HandleDesktopControllers()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void PlayerModule::HandleDesktopControllers()' called when client was not active");
			return;
		}
		if (!this.IsSceneLoaded)
		{
			return;
		}
		if (!base.isLocalPlayer)
		{
			return;
		}
		if (GlobalSettings.IsMobilePlatform)
		{
			return;
		}
		if (!this.uiSystemModule.NeedNumberInputAccess)
		{
			this.HandleSkills();
		}
		this.HandleItemBar();
		this.HandleDash();
		this.HandleActions();
		this.HandleDesktopNearby();
		this.HandleClick();
	}

	// Token: 0x0600140D RID: 5133 RVA: 0x00063975 File Offset: 0x00061B75
	[Client]
	private void HandleMobileControllers()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void PlayerModule::HandleMobileControllers()' called when client was not active");
			return;
		}
		if (!base.isLocalPlayer)
		{
			return;
		}
		if (!this.IsSceneLoaded)
		{
			return;
		}
		if (!GlobalSettings.IsMobilePlatform)
		{
			return;
		}
		this.HandleTouch();
	}

	// Token: 0x0600140E RID: 5134 RVA: 0x000639AC File Offset: 0x00061BAC
	[Client]
	private void HandleMovement()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void PlayerModule::HandleMovement()' called when client was not active");
			return;
		}
		if (!this.IsSceneLoaded)
		{
			return;
		}
		if (!this.ChatFocused && !this.uiSystemModule.NeedKeyboard)
		{
			Vector2 axis = GameInputModule.GetAxis();
			if (axis == Vector2.zero & GlobalSettings.IsMobilePlatform)
			{
				axis = new Vector2(this.uiSystemModule.MovementJoystick.Horizontal, this.uiSystemModule.MovementJoystick.Vertical);
			}
			if (axis != Vector2.zero)
			{
				if (this.aiModule.HasDestination)
				{
					this.aiModule.RemoveDestination();
				}
				bool flag = true;
				if (axis.x > 0f)
				{
					flag &= (this.movementModule.Direction != Direction.East);
					this.movementModule.PhyMoveEast(flag);
					flag = false;
				}
				if (axis.x < 0f)
				{
					flag &= (this.movementModule.Direction != Direction.West);
					this.movementModule.PhyMoveWest(flag);
					flag = false;
				}
				if (axis.y > 0f)
				{
					flag &= (this.movementModule.Direction > Direction.North);
					this.movementModule.PhyMoveNorth(flag);
					flag = false;
				}
				if (axis.y < 0f)
				{
					flag &= (this.movementModule.Direction != Direction.South);
					this.movementModule.PhyMoveSouth(flag);
				}
			}
		}
	}

	// Token: 0x0600140F RID: 5135 RVA: 0x00063B14 File Offset: 0x00061D14
	[Client]
	private void HandleItemBar()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void PlayerModule::HandleItemBar()' called when client was not active");
			return;
		}
		if (GameInputModule.GetKeyDown("Item1"))
		{
			this.UseItemFromItemBar(0);
			return;
		}
		if (GameInputModule.GetKeyDown("Item2"))
		{
			this.UseItemFromItemBar(1);
			return;
		}
		if (GameInputModule.GetKeyDown("Item3"))
		{
			this.UseItemFromItemBar(2);
			return;
		}
		if (GameInputModule.GetKeyDown("Item4"))
		{
			this.UseItemFromItemBar(3);
			return;
		}
		if (GameInputModule.GetKeyDown("Item5"))
		{
			this.UseItemFromItemBar(4);
			return;
		}
		if (GameInputModule.GetKeyDown("Item6"))
		{
			this.UseItemFromItemBar(5);
			return;
		}
		if (GameInputModule.GetKeyDown("Item7"))
		{
			this.UseItemFromItemBar(6);
			return;
		}
		if (GameInputModule.GetKeyDown("Item8"))
		{
			this.UseItemFromItemBar(7);
			return;
		}
		if (GameInputModule.GetKeyDown("Item9"))
		{
			this.UseItemFromItemBar(8);
			return;
		}
		if (GameInputModule.GetKeyDown("Item10"))
		{
			this.UseItemFromItemBar(9);
			return;
		}
	}

	// Token: 0x06001410 RID: 5136 RVA: 0x00063C00 File Offset: 0x00061E00
	private bool IsKeypadPressed()
	{
		return Input.GetKey(KeyCode.Keypad0) | Input.GetKey(KeyCode.Keypad1) | Input.GetKey(KeyCode.Keypad2) | Input.GetKey(KeyCode.Keypad3) | Input.GetKey(KeyCode.Keypad4) | Input.GetKey(KeyCode.Keypad5) | Input.GetKey(KeyCode.Keypad6) | Input.GetKey(KeyCode.Keypad7) | Input.GetKey(KeyCode.Keypad8) | Input.GetKey(KeyCode.Keypad9);
	}

	// Token: 0x06001411 RID: 5137 RVA: 0x00063C7C File Offset: 0x00061E7C
	[Client]
	private void HandleSkills()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void PlayerModule::HandleSkills()' called when client was not active");
			return;
		}
		if (!this.numLockHasBeenPressed && Input.GetKeyDown(KeyCode.Numlock) && this.IsKeypadPressed())
		{
			this.effectModule.ShowScreenMessage("numlock_pressed_warning", 3, 7f, Array.Empty<string>());
			this.numLockHasBeenPressed = true;
		}
		if (this.numLockHasBeenPressed)
		{
			return;
		}
		if (!this.ChatFocused && !this.uiSystemModule.NeedKeyboard)
		{
			if (GameInputModule.GetKeyDown("Skill1"))
			{
				this.CastSkill(this.skillModule.SkillBar[0].Id);
			}
			if (GameInputModule.GetKeyDown("Skill2"))
			{
				this.CastSkill(this.skillModule.SkillBar[1].Id);
			}
			if (GameInputModule.GetKeyDown("Skill3"))
			{
				this.CastSkill(this.skillModule.SkillBar[2].Id);
			}
			if (GameInputModule.GetKeyDown("Skill4"))
			{
				this.CastSkill(this.skillModule.SkillBar[3].Id);
			}
			if (GameInputModule.GetKeyDown("Skill5"))
			{
				this.CastSkill(this.skillModule.SkillBar[4].Id);
			}
			if (GameInputModule.GetKeyDown("Skill6"))
			{
				this.CastSkill(this.skillModule.SkillBar[5].Id);
			}
			if (GameInputModule.GetKeyDown("Skill7"))
			{
				this.CastSkill(this.skillModule.SkillBar[6].Id);
			}
			if (GameInputModule.GetKeyDown("Skill8"))
			{
				this.CastSkill(this.skillModule.SkillBar[7].Id);
			}
			if (GameInputModule.GetKeyDown("Skill9"))
			{
				this.CastSkill(this.skillModule.SkillBar[8].Id);
			}
			if (GameInputModule.GetKeyDown("Skill0"))
			{
				this.CastSkill(this.skillModule.SkillBar[9].Id);
			}
			if (GameInputModule.GetKeyDown("Second Skill1"))
			{
				this.CastSkill(this.skillModule.SecondSkillBar[0].Id);
			}
			if (GameInputModule.GetKeyDown("Second Skill2"))
			{
				this.CastSkill(this.skillModule.SecondSkillBar[1].Id);
			}
			if (GameInputModule.GetKeyDown("Second Skill3"))
			{
				this.CastSkill(this.skillModule.SecondSkillBar[2].Id);
			}
			if (GameInputModule.GetKeyDown("Second Skill4"))
			{
				this.CastSkill(this.skillModule.SecondSkillBar[3].Id);
			}
			if (GameInputModule.GetKeyDown("Second Skill5"))
			{
				this.CastSkill(this.skillModule.SecondSkillBar[4].Id);
			}
			if (GameInputModule.GetKeyDown("Second Skill6"))
			{
				this.CastSkill(this.skillModule.SecondSkillBar[5].Id);
			}
			if (GameInputModule.GetKeyDown("Second Skill7"))
			{
				this.CastSkill(this.skillModule.SecondSkillBar[6].Id);
			}
			if (GameInputModule.GetKeyDown("Second Skill8"))
			{
				this.CastSkill(this.skillModule.SecondSkillBar[7].Id);
			}
			if (GameInputModule.GetKeyDown("Second Skill9"))
			{
				this.CastSkill(this.skillModule.SecondSkillBar[8].Id);
			}
			if (GameInputModule.GetKeyDown("Second Skill0"))
			{
				this.CastSkill(this.skillModule.SecondSkillBar[9].Id);
			}
		}
	}

	// Token: 0x06001412 RID: 5138 RVA: 0x00064024 File Offset: 0x00062224
	[Client]
	private void HandleDash()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void PlayerModule::HandleDash()' called when client was not active");
			return;
		}
		if (!this.IsSceneLoaded)
		{
			return;
		}
		if (!this.ChatFocused && !this.uiSystemModule.NeedKeyboard && !this.skillModule.IsCasting)
		{
			Vector2 axis = GameInputModule.GetAxis();
			if (GameInputModule.GetKeyDown("Dash"))
			{
				if (axis.magnitude > 0f)
				{
					this.Dash(this.movementModule.Direction);
					return;
				}
				this.Dash(GlobalUtils.InverseDirection(this.movementModule.Direction));
			}
		}
	}

	// Token: 0x06001413 RID: 5139 RVA: 0x000640BC File Offset: 0x000622BC
	[Client]
	private void HandleActions()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void PlayerModule::HandleActions()' called when client was not active");
			return;
		}
		if (GameInputModule.GetKeyDown("Cancel"))
		{
			if (!this.uiSystemCanvas.enabled)
			{
				this.HideMap();
				return;
			}
			if (this.ChatFocused)
			{
				this.ChatFocused = false;
				this.uiSystemModule.PlatformChatHolderManager.ClearChatInput();
				this.uiSystemModule.PlatformChatHolderManager.SetSendMessageInputFocus(false, null);
				return;
			}
			if (this.uiSystemModule.CloseFirstWindowOnStack())
			{
				return;
			}
			if (this.combatModule.Target != null | this.aiModule.HasDestination)
			{
				if (this.combatModule.Target != null)
				{
					this.combatModule.CmdRemoveTarget();
				}
				if (this.aiModule.HasDestination)
				{
					this.aiModule.RemoveDestination();
				}
				return;
			}
			this.uiSystemModule.ToggleGameMenu();
		}
		if (!this.ChatFocused && !this.uiSystemModule.NeedKeyboard)
		{
			if (GameInputModule.GetKeyDown("Change Target"))
			{
				this.combatModule.ChangeTarget(7f);
			}
			if (GameInputModule.GetKeyDown("Inventory Window"))
			{
				this.uiSystemModule.ToggleInventoryWindow();
			}
			if (GameInputModule.GetKeyDown("Player Window"))
			{
				this.uiSystemModule.TogglePlayerWindow();
			}
			if (GameInputModule.GetKeyDown("Skills Window"))
			{
				this.uiSystemModule.ToggleSkillBookWindow();
			}
			if (GameInputModule.GetKeyDown("Attributes Window"))
			{
				this.uiSystemModule.ToggleAttributeWindow();
			}
			if (GameInputModule.GetKeyDown("Help Window"))
			{
				this.uiSystemModule.ToggleHelpWindow();
			}
			if (GameInputModule.GetKeyDown("Quest Window"))
			{
				this.uiSystemModule.ToggleQuestWindow();
			}
			if (GameInputModule.GetKeyDown("FriendList Window"))
			{
				this.CmdLoadFriendList();
				this.uiSystemModule.ToggleFriendListWindow();
			}
			if (GameInputModule.GetKeyDown("Map Window"))
			{
				if (this.uiSystemCanvas.enabled)
				{
					this.ShowMap();
				}
				else
				{
					this.HideMap();
				}
			}
			if (GameInputModule.GetKeyDown("Submit"))
			{
				this.ChatFocused = true;
				this.uiSystemModule.PlatformChatHolderManager.SetSendMessageInputFocus(true, null);
			}
			if (GameInputModule.GetKeyDown("Storage Window"))
			{
				this.uiSystemModule.ShowTradingPostStorageWindow();
			}
			if (GameInputModule.GetKeyDown("Change SkillBars"))
			{
				this.uiSystemModule.ChangeSkillBars();
				return;
			}
		}
		else if (this.ChatFocused && GameInputModule.GetKeyDown("Submit"))
		{
			this.ChatFocused = false;
			this.uiSystemModule.PlatformChatHolderManager.SendMessage();
			this.uiSystemModule.PlatformChatHolderManager.SetSendMessageInputFocus(false, null);
		}
	}

	// Token: 0x06001414 RID: 5140 RVA: 0x00064338 File Offset: 0x00062538
	[Client]
	private void HandleDesktopNearby()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void PlayerModule::HandleDesktopNearby()' called when client was not active");
			return;
		}
		if (this.ChatFocused)
		{
			return;
		}
		if (!this.creatureModule.IsAlive)
		{
			return;
		}
		int layerMask = 1 << LayerMask.NameToLayer("Npc") | 1 << LayerMask.NameToLayer("Combatant");
		int layerMask2 = 1 << LayerMask.NameToLayer("Action Tile");
		int layerMask3 = 1 << LayerMask.NameToLayer("Ground Slot");
		int layerMask4 = 1 << LayerMask.NameToLayer("Pet");
		Collider2D collider2D = Physics2D.OverlapCircle(base.transform.position, 0.32f, layerMask2);
		if (collider2D != null && collider2D.CompareTag("ActionTile"))
		{
			this.uiSystemModule.ShowHint(collider2D.GetComponent<ActionBrushManager>().HintText);
			if (GameInputModule.GetKeyDown("Action"))
			{
				this.ExecuteMapAction();
				return;
			}
		}
		Collider2D collider2D2 = Physics2D.OverlapCircle(base.transform.position, 0.24f, layerMask3);
		if (collider2D2 != null && collider2D2.CompareTag("GroundSlot"))
		{
			this.uiSystemModule.ShowHint("collect_hint");
			if (GameInputModule.GetKeyDown("Collect") || GameInputModule.GetKeyDown("Action") || Input.GetMouseButton(2))
			{
				if (Time.time - this.collectTime < 0.3f)
				{
					return;
				}
				this.collectTime = Time.time;
				this.CollectItemFromGround(collider2D2.gameObject);
				return;
			}
		}
		Collider2D collider2D3 = Physics2D.OverlapCircle(base.transform.position, 0.96f, layerMask);
		if (collider2D3 != null)
		{
			if (collider2D3.CompareTag("Combatant"))
			{
				this.uiSystemModule.ShowHint("talk_hint");
				if (GameInputModule.GetKeyDown("Action"))
				{
					this.CmdNpcHandshake(collider2D3.gameObject);
					return;
				}
			}
			else if (collider2D3.CompareTag("Npc"))
			{
				this.uiSystemModule.ShowHint("talk_hint");
				if (GameInputModule.GetKeyDown("Action"))
				{
					this.CmdNpcHandshake(collider2D3.gameObject);
					return;
				}
			}
		}
		Collider2D collider2D4 = Physics2D.OverlapCircle(base.transform.position, 0.48f, layerMask4);
		if (collider2D4 != null && collider2D4.CompareTag("Npc"))
		{
			this.uiSystemModule.ShowHint("talk_hint");
			if (GameInputModule.GetKeyDown("Action"))
			{
				this.CmdNpcHandshake(collider2D4.gameObject);
				return;
			}
		}
		if (collider2D == null & collider2D2 == null & collider2D3 == null & collider2D4 == null)
		{
			this.uiSystemModule.HideHint();
		}
	}

	// Token: 0x06001415 RID: 5141 RVA: 0x000645D8 File Offset: 0x000627D8
	[Client]
	private void HandleClick()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void PlayerModule::HandleClick()' called when client was not active");
			return;
		}
		if (!this.creatureModule.IsAlive)
		{
			return;
		}
		Vector3 vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		if (Input.GetMouseButton(1) && !EventSystem.current.IsPointerOverGameObject())
		{
			Skill skill = this.skillModule.SkillBook.FirstOrDefault((Skill s) => s.IsDefaultSkill);
			this.CastSkill(skill.Id);
		}
		if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
		{
			if (Time.time - this.collectTime < 0.3f)
			{
				return;
			}
			this.collectTime = Time.time;
			int layerMask = 1 << LayerMask.NameToLayer("Ground Slot") | 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Monster") | 1 << LayerMask.NameToLayer("Npc") | 1 << LayerMask.NameToLayer("Combatant") | 1 << LayerMask.NameToLayer("Pet");
			Collider2D[] source = Physics2D.OverlapCircleAll(new Vector3(vector.x, vector.y, 10f), 0.24f, layerMask);
			if (source.Any((Collider2D a) => a.CompareTag("Monster")))
			{
				Collider2D collider2D = (from w in source
				where w.CompareTag("Monster")
				select w).FirstOrDefault<Collider2D>();
				this.combatModule.CmdRemoveTarget();
				this.combatModule.CmdSetTarget(collider2D.gameObject, true);
				return;
			}
			if (source.Any((Collider2D a) => a.CompareTag("Combatant")))
			{
				Collider2D collider2D2 = (from w in source
				where w.CompareTag("Combatant")
				select w).FirstOrDefault<Collider2D>();
				if (GlobalUtils.IsClose(collider2D2.transform.position, base.transform.position, 1.12f))
				{
					this.CmdNpcHandshake(collider2D2.gameObject);
					return;
				}
				this.effectModule.ShowScreenMessage("player_get_close_npc_message", 0, 3.5f, Array.Empty<string>());
				return;
			}
			else if (source.Any((Collider2D a) => a.CompareTag("Npc")))
			{
				Collider2D collider2D3 = (from w in source
				where w.CompareTag("Npc")
				select w).FirstOrDefault<Collider2D>();
				if (GlobalUtils.IsClose(collider2D3.transform.position, base.transform.position, 1.12f))
				{
					this.CmdNpcHandshake(collider2D3.gameObject);
					return;
				}
				this.effectModule.ShowScreenMessage("player_get_close_npc_message", 0, 3.5f, Array.Empty<string>());
				return;
			}
			else
			{
				if (source.Any((Collider2D a) => a.CompareTag("Player")))
				{
					Collider2D collider2D4 = (from w in source
					where w.CompareTag("Player")
					select w).FirstOrDefault<Collider2D>();
					this.combatModule.CmdRemoveTarget();
					this.combatModule.CmdSetTarget(collider2D4.gameObject, true);
					return;
				}
				if (source.Any((Collider2D a) => a.CompareTag("GroundSlot")))
				{
					Collider2D collider2D5 = Physics2D.OverlapPoint(vector, 1 << LayerMask.NameToLayer("Ground Slot"));
					if (!GlobalUtils.IsClose(vector, base.transform.position, 1.12f))
					{
						this.aiModule.SetDestination(vector);
						return;
					}
					if (collider2D5 == null)
					{
						return;
					}
					this.CollectItemFromGround(collider2D5.gameObject);
					return;
				}
				else
				{
					this.aiModule.SetDestination(vector);
				}
			}
		}
	}

	// Token: 0x06001416 RID: 5142 RVA: 0x00064A08 File Offset: 0x00062C08
	[Client]
	private void HandleTouch()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void PlayerModule::HandleTouch()' called when client was not active");
			return;
		}
		if (!this.creatureModule.IsAlive)
		{
			return;
		}
		if (Input.touchCount == 0)
		{
			return;
		}
		Touch touch = Input.GetTouch(0);
		if (touch.phase != TouchPhase.Began)
		{
			return;
		}
		if (this.isMapActive)
		{
			this.HideMap();
		}
		Vector3 vector = Camera.main.ScreenToWorldPoint(touch.position);
		if (Time.time - this.collectTime < 0.3f)
		{
			return;
		}
		this.collectTime = Time.time;
		int layerMask = 1 << LayerMask.NameToLayer("Ground Slot") | 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Monster") | 1 << LayerMask.NameToLayer("Npc") | 1 << LayerMask.NameToLayer("Combatant") | 1 << LayerMask.NameToLayer("Pet");
		Collider2D[] source = Physics2D.OverlapCircleAll(new Vector3(vector.x, vector.y, 10f), 0.24f, layerMask);
		if (source.Any((Collider2D a) => a.CompareTag("Monster")))
		{
			Collider2D collider2D = (from w in source
			where w.CompareTag("Monster")
			select w).FirstOrDefault<Collider2D>();
			this.combatModule.CmdRemoveTarget();
			this.combatModule.CmdSetTarget(collider2D.gameObject, true);
			return;
		}
		if (source.Any((Collider2D a) => a.CompareTag("Combatant")))
		{
			Collider2D collider2D2 = (from w in source
			where w.CompareTag("Combatant")
			select w).FirstOrDefault<Collider2D>();
			if (GlobalUtils.IsClose(collider2D2.transform.position, base.transform.position, 1.12f))
			{
				this.CmdNpcHandshake(collider2D2.gameObject);
				return;
			}
			this.effectModule.ShowScreenMessage("player_get_close_npc_message", 0, 3.5f, Array.Empty<string>());
			return;
		}
		else if (source.Any((Collider2D a) => a.CompareTag("Npc")))
		{
			Collider2D collider2D3 = (from w in source
			where w.CompareTag("Npc")
			select w).FirstOrDefault<Collider2D>();
			if (GlobalUtils.IsClose(collider2D3.transform.position, base.transform.position, 1.12f))
			{
				this.CmdNpcHandshake(collider2D3.gameObject);
				return;
			}
			this.effectModule.ShowScreenMessage("player_get_close_npc_message", 0, 3.5f, Array.Empty<string>());
			return;
		}
		else
		{
			if (source.Any((Collider2D a) => a.CompareTag("Player")))
			{
				Collider2D collider2D4 = (from w in source
				where w.CompareTag("Player")
				select w).FirstOrDefault<Collider2D>();
				this.combatModule.CmdRemoveTarget();
				this.combatModule.CmdSetTarget(collider2D4.gameObject, true);
				return;
			}
			if (source.Any((Collider2D a) => a.CompareTag("GroundSlot")))
			{
				Collider2D collider2D5 = Physics2D.OverlapPoint(vector, 1 << LayerMask.NameToLayer("Ground Slot"));
				if (GlobalUtils.IsClose(vector, base.transform.position, 1.12f))
				{
					if (collider2D5 == null)
					{
						return;
					}
					this.CollectItemFromGround(collider2D5.gameObject);
				}
				return;
			}
			return;
		}
	}

	// Token: 0x06001417 RID: 5143 RVA: 0x00064DD8 File Offset: 0x00062FD8
	[Client]
	public void UseItemFromItemBar(int itemBarPosition)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void PlayerModule::UseItemFromItemBar(System.Int32)' called when client was not active");
			return;
		}
		if (this.lastUsedItemBarPosition == itemBarPosition && Time.time - this.itemBarInputTime < 1f)
		{
			return;
		}
		this.itemBarInputTime = Time.time;
		this.lastUsedItemBarPosition = itemBarPosition;
		this.itemModule.CmdUseItemFromItemBar(this.inventoryModule.ItemBar[itemBarPosition]);
	}

	// Token: 0x06001418 RID: 5144 RVA: 0x00064E48 File Offset: 0x00063048
	[Client]
	public void CastSkill(int skillId)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void PlayerModule::CastSkill(System.Int32)' called when client was not active");
			return;
		}
		Skill fromSkillBook = this.skillModule.GetFromSkillBook(skillId);
		if (fromSkillBook.CooldownTimer(NetworkTime.time) > 0.0)
		{
			return;
		}
		if (fromSkillBook.IsDefaultSkill | fromSkillBook.CanCauseDamage)
		{
			List<GameObject> nearbyTagets = this.combatModule.GetNearbyTagets();
			if (nearbyTagets.Count == 0)
			{
				return;
			}
			bool flag = false;
			foreach (GameObject gameObject in nearbyTagets)
			{
				CreatureModule creatureModule;
				if (gameObject.TryGetComponent<CreatureModule>(out creatureModule) && creatureModule.IsAlive && this.combatModule.TargetFinder.IsTargetValid(fromSkillBook.Range, true, true, true, false, base.transform.position, base.gameObject, gameObject))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return;
			}
		}
		if (!this.skillModule.IsCasting)
		{
			this.skillModule.CmdCast(skillId);
		}
	}

	// Token: 0x06001419 RID: 5145 RVA: 0x00064F58 File Offset: 0x00063158
	private IEnumerator ShowWarningMessage()
	{
		string url = string.Concat(new string[]
		{
			GlobalSettings.ApiHost,
			"api/",
			SettingsManager.Instance.ApiAccount.AccountUniqueId,
			"/account",
			string.Format("/warning/{0}", SettingsManager.Instance.ApiAccount.AccountId)
		});
		yield return ApiManager.Get<string>(url, delegate(ApiResponse<string> response)
		{
			if (response.Success && !string.IsNullOrEmpty(response.ResponseObject))
			{
				this.uiSystemModule.ShowInformationWindow("warning_title", response.ResponseObject, null);
			}
		});
		yield break;
	}

	// Token: 0x0600141A RID: 5146 RVA: 0x00064F68 File Offset: 0x00063168
	[TargetRpc]
	public void TargetShowWarningMessage(string message)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(message);
		this.SendTargetRPCInternal(null, "System.Void PlayerModule::TargetShowWarningMessage(System.String)", 3316919, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600141B RID: 5147 RVA: 0x00064FA2 File Offset: 0x000631A2
	[Client]
	public void SetGuildName(string guildName)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void PlayerModule::SetGuildName(System.String)' called when client was not active");
			return;
		}
		this.guildNameText.text = guildName;
	}

	// Token: 0x0600141C RID: 5148 RVA: 0x00064FC8 File Offset: 0x000631C8
	[Client]
	public void ShowRenewPremiumIfNeeded()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void PlayerModule::ShowRenewPremiumIfNeeded()' called when client was not active");
			return;
		}
		if (this.PremiumDays > 1)
		{
			return;
		}
		if (this.attributeModule.BaseLevel < 8)
		{
			return;
		}
		if (this.areaModule.AreaType != AreaType.ProtectedArea)
		{
			return;
		}
		string @string = PlayerPrefs.GetString("LastRenewPremiumNotification");
		bool flag = false;
		DateTime value;
		if (string.IsNullOrEmpty(@string))
		{
			flag = true;
		}
		else if (DateTime.TryParse(@string, out value))
		{
			flag = (DateTime.Now.Subtract(value).TotalDays > 7.0);
		}
		if (flag)
		{
			this.uiSystemModule.ShowRenewPremiumWindow();
			PlayerPrefs.SetString("LastRenewPremiumNotification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
		}
	}

	// Token: 0x0600141D RID: 5149 RVA: 0x00065084 File Offset: 0x00063284
	[Client]
	public void ShowPromoPackageWindowOnClient()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void PlayerModule::ShowPromoPackageWindowOnClient()' called when client was not active");
			return;
		}
		if (this.attributeModule.BaseLevel < 4)
		{
			return;
		}
		if (this.DaysSinceLastOrder < 2)
		{
			return;
		}
		string @string = PlayerPrefs.GetString("LastPromoPackagesWindowNotification");
		bool flag = false;
		DateTime value;
		if (string.IsNullOrEmpty(@string))
		{
			flag = true;
		}
		else if (DateTime.TryParse(@string, out value))
		{
			flag = (DateTime.Now.Subtract(value).TotalDays > 7.0);
		}
		if (flag)
		{
			this.uiSystemModule.ShowPromoPackagesWindow();
			PlayerPrefs.SetString("LastPromoPackagesWindowNotification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
		}
	}

	// Token: 0x0600141E RID: 5150 RVA: 0x0006512F File Offset: 0x0006332F
	[Server]
	public void ShowFreePremiumWindowIfAllowed()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::ShowFreePremiumWindowIfAllowed()' called when server was not active");
			return;
		}
		if (this.GotFreePremium)
		{
			return;
		}
		if (this.attributeModule.BaseLevel < 2)
		{
			return;
		}
		this.TargetShowFreePremiumWindow();
	}

	// Token: 0x0600141F RID: 5151 RVA: 0x00065164 File Offset: 0x00063364
	[TargetRpc]
	public void TargetShowFreePremiumWindow()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		this.SendTargetRPCInternal(null, "System.Void PlayerModule::TargetShowFreePremiumWindow()", 846791389, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001420 RID: 5152 RVA: 0x00065194 File Offset: 0x00063394
	[Command]
	public void CmdTakeFreePremium()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		base.SendCommandInternal("System.Void PlayerModule::CmdTakeFreePremium()", -189727044, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001421 RID: 5153 RVA: 0x000651C4 File Offset: 0x000633C4
	[TargetRpc]
	private void TargetShowRateGameWindow()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		this.SendTargetRPCInternal(null, "System.Void PlayerModule::TargetShowRateGameWindow()", 457708834, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001422 RID: 5154 RVA: 0x000651F4 File Offset: 0x000633F4
	[Client]
	public void OpenPartyChatChannel(bool activateTab)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void PlayerModule::OpenPartyChatChannel(System.Boolean)' called when client was not active");
			return;
		}
		this.uiSystemModule.PlatformChatHolderManager.OpenChannel("chat_tab_party", false, true, false, true);
		if (activateTab)
		{
			this.uiSystemModule.PlatformChatHolderManager.ActivateTab("chat_tab_party");
		}
	}

	// Token: 0x06001423 RID: 5155 RVA: 0x00065248 File Offset: 0x00063448
	[Client]
	public void ClosePartyChatChannel()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void PlayerModule::ClosePartyChatChannel()' called when client was not active");
			return;
		}
		this.uiSystemModule.PlatformChatHolderManager.CloseChatTab("chat_tab_party");
	}

	// Token: 0x06001424 RID: 5156 RVA: 0x00065274 File Offset: 0x00063474
	[Client]
	public void ExecuteMapAction()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void PlayerModule::ExecuteMapAction()' called when client was not active");
			return;
		}
		if (!base.isLocalPlayer)
		{
			return;
		}
		if (Time.time - this.lastMapActionTime < 1f)
		{
			return;
		}
		this.lastMapActionTime = Time.time;
		this.CmdExecuteAction();
	}

	// Token: 0x06001425 RID: 5157 RVA: 0x000652C4 File Offset: 0x000634C4
	[Command]
	private void CmdExecuteAction()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		base.SendCommandInternal("System.Void PlayerModule::CmdExecuteAction()", 1657104771, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001426 RID: 5158 RVA: 0x000652F4 File Offset: 0x000634F4
	[Client]
	public void CollectItemFromGround(GameObject groundSlot)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void PlayerModule::CollectItemFromGround(UnityEngine.GameObject)' called when client was not active");
			return;
		}
		this.inventoryModule.CmdCollectFromGround(groundSlot);
	}

	// Token: 0x06001427 RID: 5159 RVA: 0x00065318 File Offset: 0x00063518
	[TargetRpc]
	public void TargetDisconnect(NetworkConnection target)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		this.SendTargetRPCInternal(target, "System.Void PlayerModule::TargetDisconnect(Mirror.NetworkConnection)", 1446295298, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001428 RID: 5160 RVA: 0x00065348 File Offset: 0x00063548
	[Client]
	private void DisconnectPlayer()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void PlayerModule::DisconnectPlayer()' called when client was not active");
			return;
		}
		GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManagerModule>().StopClient();
	}

	// Token: 0x06001429 RID: 5161 RVA: 0x00065374 File Offset: 0x00063574
	[Client]
	private void HandleSelectionObject(bool isSelected)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void PlayerModule::HandleSelectionObject(System.Boolean)' called when client was not active");
			return;
		}
		if (!this.uiSystemModule.ShowAllNames)
		{
			if (this.selectionObject.activeInHierarchy)
			{
				this.selectionObject.SetActive(false);
			}
			return;
		}
		if (isSelected && !this.selectionObject.activeInHierarchy)
		{
			this.selectionObject.SetActive(true);
			return;
		}
		if (!isSelected && this.selectionObject.activeInHierarchy)
		{
			this.selectionObject.SetActive(false);
			return;
		}
	}

	// Token: 0x0600142A RID: 5162 RVA: 0x000653F8 File Offset: 0x000635F8
	[Client]
	private void HandleLocalHudVisibility()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void PlayerModule::HandleLocalHudVisibility()' called when client was not active");
			return;
		}
		if (this.uiSystemModule == null)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
			this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
		}
		bool flag = false;
		if (this.uiSystemModule.CombatModule != null && this.uiSystemModule.CombatModule.Target != null)
		{
			NetworkIdentity component = this.uiSystemModule.CombatModule.Target.GetComponent<NetworkIdentity>();
			if (component != null)
			{
				flag = (component.netId == base.netId);
			}
		}
		bool flag2 = (flag | this.uiSystemModule.ShowAllNames) && !this.conditionModule.IsInvisible && this.creatureModule.IsAlive;
		if (this.uiSystemModule.ShowAllNames && base.isLocalPlayer)
		{
			flag2 = true;
		}
		if (flag2)
		{
			this.HandleSelectionObject(flag);
			this.UpdateNameText();
			this.UpdateTitleAndGuildTexts();
			float num = (float)this.attributeModule.CurrentHealth * 100f / (float)this.attributeModule.MaxHealth;
			this.healthBar.fillAmount = num / 100f;
			this.levelText.text = this.attributeModule.BaseLevel.ToString();
			if (!this.localHud.activeInHierarchy)
			{
				this.localHud.SetActive(true);
				return;
			}
		}
		else if (this.localHud.activeInHierarchy)
		{
			this.localHud.SetActive(false);
		}
	}

	// Token: 0x0600142B RID: 5163 RVA: 0x00065574 File Offset: 0x00063774
	[Client]
	private void UpdateNameText()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void PlayerModule::UpdateNameText()' called when client was not active");
			return;
		}
		if (this.pvpModule.PvpStatus == PvpStatus.InCombat)
		{
			this.nameText.color = GlobalSettings.Colors[2];
		}
		else if (this.pvpModule.HasPlayerKillerLimitations())
		{
			this.nameText.color = Color.red;
		}
		else if (this.pvpModule.IsWeekTvtHero)
		{
			this.nameText.color = Color.yellow;
		}
		else
		{
			this.nameText.color = Color.cyan;
		}
		this.nameText.text = this.creatureModule.CreatureName;
	}

	// Token: 0x0600142C RID: 5164 RVA: 0x00065620 File Offset: 0x00063820
	[Client]
	private void UpdateTitleAndGuildTexts()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void PlayerModule::UpdateTitleAndGuildTexts()' called when client was not active");
			return;
		}
		if (this.pvpModule.TvtTeam == TvtTeam.RedTeam)
		{
			this.titleText.text = string.Format(LanguageManager.Instance.GetText("red_team_title"), this.pvpModule.TeamFightScore, this.pvpModule.TeamFightAssist);
		}
		else if (this.pvpModule.TvtTeam == TvtTeam.BlueTeam)
		{
			this.titleText.text = string.Format(LanguageManager.Instance.GetText("blue_team_title"), this.pvpModule.TeamFightScore, this.pvpModule.TeamFightAssist);
		}
		else
		{
			this.titleText.text = LanguageManager.Instance.GetText(this.creatureModule.CreatureTitle);
		}
		if (string.IsNullOrEmpty(this.titleText.text))
		{
			this.guildNameText.rectTransform.anchoredPosition = new Vector3(0f, 0.93f, 0f);
			return;
		}
		this.guildNameText.rectTransform.anchoredPosition = new Vector3(0f, 1.2f, 0f);
	}

	// Token: 0x0600142D RID: 5165 RVA: 0x00065765 File Offset: 0x00063965
	[Client]
	public void SetFriendListDirty(bool value)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void PlayerModule::SetFriendListDirty(System.Boolean)' called when client was not active");
			return;
		}
		this.friendListDirty = value;
	}

	// Token: 0x0600142E RID: 5166 RVA: 0x00065784 File Offset: 0x00063984
	[TargetRpc]
	private void TargetShowRespawnWindow(NetworkConnection t)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		this.SendTargetRPCInternal(t, "System.Void PlayerModule::TargetShowRespawnWindow(Mirror.NetworkConnection)", 1653481117, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600142F RID: 5167 RVA: 0x000657B4 File Offset: 0x000639B4
	[TargetRpc]
	private void TargetShowHelpWindow(NetworkConnection target)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		this.SendTargetRPCInternal(target, "System.Void PlayerModule::TargetShowHelpWindow(Mirror.NetworkConnection)", -1282896812, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001430 RID: 5168 RVA: 0x000657E4 File Offset: 0x000639E4
	[Command]
	private void CmdSetPlatform(string platform)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(platform);
		base.SendCommandInternal("System.Void PlayerModule::CmdSetPlatform(System.String)", 1094186613, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001431 RID: 5169 RVA: 0x00065820 File Offset: 0x00063A20
	[TargetRpc]
	public void TargetOpenWarehouse(NetworkConnection target, Vector3 npcPosition)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteVector3(npcPosition);
		this.SendTargetRPCInternal(target, "System.Void PlayerModule::TargetOpenWarehouse(Mirror.NetworkConnection,UnityEngine.Vector3)", 1695409806, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001432 RID: 5170 RVA: 0x0006585C File Offset: 0x00063A5C
	[TargetRpc]
	public void TargetOpenInventory(NetworkConnection target)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		this.SendTargetRPCInternal(target, "System.Void PlayerModule::TargetOpenInventory(Mirror.NetworkConnection)", 1171975636, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001433 RID: 5171 RVA: 0x0006588C File Offset: 0x00063A8C
	[Client]
	private IEnumerator PvpAura()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Collections.IEnumerator PlayerModule::PvpAura()' called when client was not active");
			return null;
		}
		PlayerModule.<PvpAura>d__182 <PvpAura>d__ = new PlayerModule.<PvpAura>d__182(0);
		<PvpAura>d__.<>4__this = this;
		return <PvpAura>d__;
	}

	// Token: 0x06001434 RID: 5172 RVA: 0x000658C7 File Offset: 0x00063AC7
	public void Dash(Direction direction)
	{
		if (this.aiModule.HasDestination)
		{
			this.aiModule.RemoveDestination();
		}
		this.movementModule.CmdDash(direction);
	}

	// Token: 0x06001435 RID: 5173 RVA: 0x000658F0 File Offset: 0x00063AF0
	private void Regeneration()
	{
		if (this.attributeModule.CurrentEndurance < this.attributeModule.MaxEndurance)
		{
			this.attributeModule.AddEndurance(this.attributeModule.EnduranceRegenerationAmount);
		}
		if (Time.time - this.lastRegeneration >= this.attributeModule.RegenerationInterval)
		{
			this.lastRegeneration = Time.time;
			if (this.attributeModule.CurrentHealth < this.attributeModule.MaxHealth)
			{
				if (!this.combatModule.InCombat)
				{
					float num = (float)this.attributeModule.MaxHealth * 0.25f;
					this.attributeModule.AddHealth(base.gameObject, (int)num, false, default(EffectConfig));
					return;
				}
				this.attributeModule.AddHealth(base.gameObject, this.attributeModule.RegenerationAmount, false, default(EffectConfig));
			}
		}
	}

	// Token: 0x06001436 RID: 5174 RVA: 0x000659D0 File Offset: 0x00063BD0
	[Server]
	private IEnumerator AutoRespawnOnDeath()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Collections.IEnumerator PlayerModule::AutoRespawnOnDeath()' called when server was not active");
			return null;
		}
		PlayerModule.<AutoRespawnOnDeath>d__185 <AutoRespawnOnDeath>d__ = new PlayerModule.<AutoRespawnOnDeath>d__185(0);
		<AutoRespawnOnDeath>d__.<>4__this = this;
		return <AutoRespawnOnDeath>d__;
	}

	// Token: 0x06001437 RID: 5175 RVA: 0x00065A0C File Offset: 0x00063C0C
	[Server]
	public void Respawn()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::Respawn()' called when server was not active");
			return;
		}
		this.attributeModule.SetHealthToMax();
		Effect teleportEffect = new Effect("TeleporterHit", 0.5f, 0.25f);
		Vector3 position = this.movementModule.SpawnPointLocation;
		if (this.pvpModule.PvpStatus == PvpStatus.Outlaw | this.pvpModule.PvpStatus == PvpStatus.PlayerKiller)
		{
			position = GlobalUtils.GetLocationFromSpawnPoint("outlaw_camp");
		}
		this.movementModule.Teleport(position, default(Effect));
		this.movementModule.TargetTeleport(base.connectionToClient, position, teleportEffect);
		Condition condition = new Condition(ConditionCategory.Invincibility, ConditionType.Invincible, 5f, 0.5f, 0f, new Effect("EarthStrike", 0.5f, 0.25f), 0, 0f, "");
		this.creatureModule.SetAlive(true);
		this.conditionModule.StartCondition(condition, base.gameObject, true);
	}

	// Token: 0x06001438 RID: 5176 RVA: 0x00065B04 File Offset: 0x00063D04
	[Server]
	private IEnumerator Resurrect(float healthPercent = 50f, GameObject killer = null)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Collections.IEnumerator PlayerModule::Resurrect(System.Single,UnityEngine.GameObject)' called when server was not active");
			return null;
		}
		PlayerModule.<Resurrect>d__187 <Resurrect>d__ = new PlayerModule.<Resurrect>d__187(0);
		<Resurrect>d__.<>4__this = this;
		<Resurrect>d__.healthPercent = healthPercent;
		<Resurrect>d__.killer = killer;
		return <Resurrect>d__;
	}

	// Token: 0x06001439 RID: 5177 RVA: 0x00065B50 File Offset: 0x00063D50
	[Server]
	public void Revive(float healthPercent)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::Revive(System.Single)' called when server was not active");
			return;
		}
		this.StopAutoRespawn();
		int health = Mathf.CeilToInt((float)this.attributeModule.MaxHealth * healthPercent);
		this.attributeModule.SetHealth(health);
		this.creatureModule.SetAlive(true);
	}

	// Token: 0x0600143A RID: 5178 RVA: 0x00065BA4 File Offset: 0x00063DA4
	[Command]
	public void CmdRespawn()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		base.SendCommandInternal("System.Void PlayerModule::CmdRespawn()", 575775392, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600143B RID: 5179 RVA: 0x00065BD4 File Offset: 0x00063DD4
	[TargetRpc]
	private void TargetShowSelectedTargetHud(NetworkConnection target, GameObject selectedTarget)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteGameObject(selectedTarget);
		this.SendTargetRPCInternal(target, "System.Void PlayerModule::TargetShowSelectedTargetHud(Mirror.NetworkConnection,UnityEngine.GameObject)", -664687858, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600143C RID: 5180 RVA: 0x00065C10 File Offset: 0x00063E10
	[TargetRpc]
	private void TargetHideSelectedTargetHud(NetworkConnection target)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		this.SendTargetRPCInternal(target, "System.Void PlayerModule::TargetHideSelectedTargetHud(Mirror.NetworkConnection)", -1096555765, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600143D RID: 5181 RVA: 0x00065C40 File Offset: 0x00063E40
	private void OnPremiumDays(int oldValue, int newValue)
	{
		this.NetworkPremiumDays = newValue;
		int num = newValue - oldValue;
		if (num > 0)
		{
			this.effectModule.ShowScreenMessage("premium_days_added_message", 1, 7f, new string[]
			{
				num.ToString()
			});
			EffectConfig effectConfig = new EffectConfig
			{
				EffectName = "Magic",
				EffectScaleModifier = 0.5f,
				EffectSpeedModifier = 0.25f,
				SoundEffectName = "spellbook"
			};
			this.effectModule.ShowEffects(effectConfig);
		}
	}

	// Token: 0x0600143E RID: 5182 RVA: 0x00065CC8 File Offset: 0x00063EC8
	private void OnHasItemOnStorage(bool oldValue, bool newValue)
	{
		this.NetworkHasItemOnStorage = newValue;
		if (newValue & oldValue != newValue & base.isLocalPlayer)
		{
			this.effectModule.ShowScreenMessage("has_items_on_storage_message", 1, 7f, Array.Empty<string>());
		}
	}

	// Token: 0x0600143F RID: 5183 RVA: 0x00065D00 File Offset: 0x00063F00
	public void CheckForItemsOnStorage()
	{
		PlayerModule.<CheckForItemsOnStorage>d__194 <CheckForItemsOnStorage>d__;
		<CheckForItemsOnStorage>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CheckForItemsOnStorage>d__.<>4__this = this;
		<CheckForItemsOnStorage>d__.<>1__state = -1;
		<CheckForItemsOnStorage>d__.<>t__builder.Start<PlayerModule.<CheckForItemsOnStorage>d__194>(ref <CheckForItemsOnStorage>d__);
	}

	// Token: 0x06001440 RID: 5184 RVA: 0x00065D38 File Offset: 0x00063F38
	[Server]
	public Task UpdateHasItemOnStorageAsync()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Threading.Tasks.Task PlayerModule::UpdateHasItemOnStorageAsync()' called when server was not active");
			return null;
		}
		PlayerModule.<UpdateHasItemOnStorageAsync>d__195 <UpdateHasItemOnStorageAsync>d__;
		<UpdateHasItemOnStorageAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<UpdateHasItemOnStorageAsync>d__.<>4__this = this;
		<UpdateHasItemOnStorageAsync>d__.<>1__state = -1;
		<UpdateHasItemOnStorageAsync>d__.<>t__builder.Start<PlayerModule.<UpdateHasItemOnStorageAsync>d__195>(ref <UpdateHasItemOnStorageAsync>d__);
		return <UpdateHasItemOnStorageAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06001441 RID: 5185 RVA: 0x00065D9C File Offset: 0x00063F9C
	[Server]
	public void SetHasItemOnStorage(bool value)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::SetHasItemOnStorage(System.Boolean)' called when server was not active");
			return;
		}
		this.NetworkHasItemOnStorage = value;
	}

	// Token: 0x06001442 RID: 5186 RVA: 0x00065DBC File Offset: 0x00063FBC
	[Command]
	public void CmdCancelOrder(int orderId)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(orderId);
		base.SendCommandInternal("System.Void PlayerModule::CmdCancelOrder(System.Int32)", -1460824259, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001443 RID: 5187 RVA: 0x00065DF8 File Offset: 0x00063FF8
	[TargetRpc]
	public void TargetRefreshMarketStorage(NetworkConnection target)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		this.SendTargetRPCInternal(target, "System.Void PlayerModule::TargetRefreshMarketStorage(Mirror.NetworkConnection)", 1625494890, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001444 RID: 5188 RVA: 0x00065E28 File Offset: 0x00064028
	[TargetRpc]
	public void TargetRefreshBuyMarket(NetworkConnection target)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		this.SendTargetRPCInternal(target, "System.Void PlayerModule::TargetRefreshBuyMarket(Mirror.NetworkConnection)", -2085848339, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001445 RID: 5189 RVA: 0x00065E58 File Offset: 0x00064058
	[TargetRpc]
	public void TargetRefreshSellMarket(NetworkConnection target)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		this.SendTargetRPCInternal(target, "System.Void PlayerModule::TargetRefreshSellMarket(Mirror.NetworkConnection)", 302978379, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001446 RID: 5190 RVA: 0x00065E88 File Offset: 0x00064088
	[TargetRpc]
	public void TargetRefreshMyOffers(NetworkConnection target)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		this.SendTargetRPCInternal(target, "System.Void PlayerModule::TargetRefreshMyOffers(Mirror.NetworkConnection)", 1574516608, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001447 RID: 5191 RVA: 0x00065EB8 File Offset: 0x000640B8
	[Command]
	public void CmdPlaceOrder(MarketOrder order, int amount, int unitValue)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		Mirror.GeneratedNetworkCode._Write_MarketOrder(writer, order);
		writer.WriteInt(amount);
		writer.WriteInt(unitValue);
		base.SendCommandInternal("System.Void PlayerModule::CmdPlaceOrder(MarketOrder,System.Int32,System.Int32)", 638092783, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001448 RID: 5192 RVA: 0x00065F08 File Offset: 0x00064108
	[Command]
	public void CmdBuyFromGemMarket(int orderId, int amount)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(orderId);
		writer.WriteInt(amount);
		base.SendCommandInternal("System.Void PlayerModule::CmdBuyFromGemMarket(System.Int32,System.Int32)", -169198047, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001449 RID: 5193 RVA: 0x00065F4C File Offset: 0x0006414C
	[Command]
	public void CmdBuyFromMarket(int orderId, int amount)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(orderId);
		writer.WriteInt(amount);
		base.SendCommandInternal("System.Void PlayerModule::CmdBuyFromMarket(System.Int32,System.Int32)", 364720322, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600144A RID: 5194 RVA: 0x00065F90 File Offset: 0x00064190
	[Command]
	public void CmdTakeFromStorage(int storageId)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(storageId);
		base.SendCommandInternal("System.Void PlayerModule::CmdTakeFromStorage(System.Int32)", -260853581, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600144B RID: 5195 RVA: 0x00065FCC File Offset: 0x000641CC
	[TargetRpc]
	private void TargetReloadStoreItems(NetworkConnection target)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		this.SendTargetRPCInternal(target, "System.Void PlayerModule::TargetReloadStoreItems(Mirror.NetworkConnection)", 1512232894, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600144C RID: 5196 RVA: 0x00065FFC File Offset: 0x000641FC
	[TargetRpc]
	private void TargetRenderNpcDialog(NetworkConnection target, NpcDialog npcDialog)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		Mirror.GeneratedNetworkCode._Write_NpcDialog(writer, npcDialog);
		this.SendTargetRPCInternal(target, "System.Void PlayerModule::TargetRenderNpcDialog(Mirror.NetworkConnection,NpcDialog)", -1668438184, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600144D RID: 5197 RVA: 0x00066038 File Offset: 0x00064238
	[Server]
	public void RenderNpcDialog(NpcDialog npcDialog)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PlayerModule::RenderNpcDialog(NpcDialog)' called when server was not active");
			return;
		}
		if (npcDialog.StoreAction == StoreAction.Buy | npcDialog.StoreAction == StoreAction.Repurchase)
		{
			foreach (StoreItemConfig storeItem in npcDialog.StoreItems)
			{
				this.itemDatabaseModule.AddNpcSellableItem(storeItem);
			}
		}
		this.TargetRenderNpcDialog(base.connectionToClient, npcDialog);
	}

	// Token: 0x0600144E RID: 5198 RVA: 0x000660A8 File Offset: 0x000642A8
	[TargetRpc]
	public void TargetCloseDialogWindow(NetworkConnection target)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		this.SendTargetRPCInternal(target, "System.Void PlayerModule::TargetCloseDialogWindow(Mirror.NetworkConnection)", -1406544750, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600144F RID: 5199 RVA: 0x000660D8 File Offset: 0x000642D8
	[Command]
	public void CmdNpcHandshake(GameObject npc)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteGameObject(npc);
		base.SendCommandInternal("System.Void PlayerModule::CmdNpcHandshake(UnityEngine.GameObject)", 45890808, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001450 RID: 5200 RVA: 0x00066114 File Offset: 0x00064314
	[Command]
	public void CmdSendChoice(int actionId)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(actionId);
		base.SendCommandInternal("System.Void PlayerModule::CmdSendChoice(System.Int32)", 266930436, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001451 RID: 5201 RVA: 0x00066150 File Offset: 0x00064350
	[TargetRpc]
	public void TargetShowStoryline()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		this.SendTargetRPCInternal(null, "System.Void PlayerModule::TargetShowStoryline()", -83018581, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001452 RID: 5202 RVA: 0x00066180 File Offset: 0x00064380
	[Command]
	public void CmdOpenQuests()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		base.SendCommandInternal("System.Void PlayerModule::CmdOpenQuests()", -1858644187, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001453 RID: 5203 RVA: 0x000661B0 File Offset: 0x000643B0
	[TargetRpc]
	public void TargetOpenQuests(NetworkConnection target)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		this.SendTargetRPCInternal(target, "System.Void PlayerModule::TargetOpenQuests(Mirror.NetworkConnection)", -755721119, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001454 RID: 5204 RVA: 0x000661E0 File Offset: 0x000643E0
	public void ShowMap()
	{
		this.uiSystemCanvas.enabled = false;
		this.uiHudCanvas.enabled = false;
		this.uiTweaksCanvas.enabled = false;
		this.gameEnvironmentModule.SetIgnoreAmbientLight(true);
		this.mainCamera.cullingMask = (1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Minimap Point") | 1 << LayerMask.NameToLayer("Tile") | 1 << LayerMask.NameToLayer("Quest Icon"));
		Camera.main.orthographicSize = GlobalUtils.GetMainCameraDistance(SettingsManager.Instance.CameraMode, true);
		this.isMapActive = true;
	}

	// Token: 0x06001455 RID: 5205 RVA: 0x00066288 File Offset: 0x00064488
	private void HideMap()
	{
		Camera.main.orthographicSize = GlobalUtils.GetMainCameraDistance(SettingsManager.Instance.CameraMode, false);
		this.gameEnvironmentModule.SetIgnoreAmbientLight(false);
		this.mainCamera.cullingMask = this.mainCameraLayerMask;
		this.uiSystemCanvas.enabled = true;
		this.uiHudCanvas.enabled = true;
		this.uiTweaksCanvas.enabled = true;
		this.isMapActive = false;
	}

	// Token: 0x06001456 RID: 5206 RVA: 0x000662FC File Offset: 0x000644FC
	[TargetRpc]
	public void TargetChangePlayerSceneMap(string newMapScene)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(newMapScene);
		this.SendTargetRPCInternal(null, "System.Void PlayerModule::TargetChangePlayerSceneMap(System.String)", 2107395646, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001457 RID: 5207 RVA: 0x00066336 File Offset: 0x00064536
	private IEnumerator LoadMapScene(string sceneName)
	{
		this.IsSceneLoaded = false;
		this.aiModule.RemoveDestination();
		this.uiSystemModule.ActivateMenuBackgroundImage();
		this.uiSystemModule.ShowFeedbackText(LanguageManager.Instance.GetText("loading_label"));
		AssetBundleManager.Instance.HideContentHolders();
		WorldCreatorModule component = GameObject.FindGameObjectWithTag("GameManager").GetComponent<WorldCreatorModule>();
		string[] array = component.sceneMapNames;
		for (int i = 0; i < array.Length; i++)
		{
			string text = array[i];
			if (!(text == sceneName))
			{
				Scene sceneByName = SceneManager.GetSceneByName(text);
				if (sceneByName.isLoaded)
				{
					AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(sceneByName);
					yield return new WaitUntil(() => asyncOperation.isDone);
				}
			}
		}
		array = null;
		if (!SceneManager.GetSceneByName(sceneName).isLoaded)
		{
			AsyncOperation loadAsyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
			yield return new WaitUntil(() => loadAsyncOperation.isDone);
		}
		this.uiSystemModule.DeActivateMenuBackgroundImage();
		this.uiSystemModule.HideFeedbackPanel();
		this.IsSceneLoaded = true;
		if (NetworkServer.active)
		{
			this.conditionModule.RemoveCondition(ConditionType.Restricted);
		}
		yield break;
	}

	// Token: 0x06001458 RID: 5208 RVA: 0x0006634C File Offset: 0x0006454C
	[Command]
	private void CmdLoadFriendList()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		base.SendCommandInternal("System.Void PlayerModule::CmdLoadFriendList()", 1830035468, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001459 RID: 5209 RVA: 0x0006637C File Offset: 0x0006457C
	[Client]
	private IEnumerator RefreshFriendList()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Collections.IEnumerator PlayerModule::RefreshFriendList()' called when client was not active");
			return null;
		}
		PlayerModule.<RefreshFriendList>d__220 <RefreshFriendList>d__ = new PlayerModule.<RefreshFriendList>d__220(0);
		<RefreshFriendList>d__.<>4__this = this;
		return <RefreshFriendList>d__;
	}

	// Token: 0x0600145E RID: 5214 RVA: 0x0002FEC9 File Offset: 0x0002E0C9
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x1700021F RID: 543
	// (get) Token: 0x0600145F RID: 5215 RVA: 0x00066428 File Offset: 0x00064628
	// (set) Token: 0x06001460 RID: 5216 RVA: 0x0006643B File Offset: 0x0006463B
	public int NetworkServerId
	{
		get
		{
			return this.ServerId;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.ServerId, 1UL, null);
		}
	}

	// Token: 0x17000220 RID: 544
	// (get) Token: 0x06001461 RID: 5217 RVA: 0x00066458 File Offset: 0x00064658
	// (set) Token: 0x06001462 RID: 5218 RVA: 0x0006646B File Offset: 0x0006466B
	public int NetworkPlayerId
	{
		get
		{
			return this.PlayerId;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.PlayerId, 2UL, null);
		}
	}

	// Token: 0x17000221 RID: 545
	// (get) Token: 0x06001463 RID: 5219 RVA: 0x00066488 File Offset: 0x00064688
	// (set) Token: 0x06001464 RID: 5220 RVA: 0x0006649B File Offset: 0x0006469B
	public PackageType NetworkPackageType
	{
		get
		{
			return this.PackageType;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<PackageType>(value, ref this.PackageType, 4UL, null);
		}
	}

	// Token: 0x17000222 RID: 546
	// (get) Token: 0x06001465 RID: 5221 RVA: 0x000664B8 File Offset: 0x000646B8
	// (set) Token: 0x06001466 RID: 5222 RVA: 0x000664CB File Offset: 0x000646CB
	public int NetworkDaysSinceLastOrder
	{
		get
		{
			return this.DaysSinceLastOrder;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.DaysSinceLastOrder, 8UL, null);
		}
	}

	// Token: 0x17000223 RID: 547
	// (get) Token: 0x06001467 RID: 5223 RVA: 0x000664E8 File Offset: 0x000646E8
	// (set) Token: 0x06001468 RID: 5224 RVA: 0x000664FB File Offset: 0x000646FB
	public bool NetworkIsFirstConnection
	{
		get
		{
			return this.IsFirstConnection;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<bool>(value, ref this.IsFirstConnection, 16UL, null);
		}
	}

	// Token: 0x17000224 RID: 548
	// (get) Token: 0x06001469 RID: 5225 RVA: 0x00066518 File Offset: 0x00064718
	// (set) Token: 0x0600146A RID: 5226 RVA: 0x0006652B File Offset: 0x0006472B
	public int NetworkPremiumDays
	{
		get
		{
			return this.PremiumDays;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.PremiumDays, 32UL, new Action<int, int>(this.OnPremiumDays));
		}
	}

	// Token: 0x17000225 RID: 549
	// (get) Token: 0x0600146B RID: 5227 RVA: 0x00066550 File Offset: 0x00064750
	// (set) Token: 0x0600146C RID: 5228 RVA: 0x00066563 File Offset: 0x00064763
	public bool NetworkHasItemOnStorage
	{
		get
		{
			return this.HasItemOnStorage;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<bool>(value, ref this.HasItemOnStorage, 64UL, new Action<bool, bool>(this.OnHasItemOnStorage));
		}
	}

	// Token: 0x0600146D RID: 5229 RVA: 0x00066588 File Offset: 0x00064788
	protected void UserCode_CmdSavePlayerData__Boolean(bool isOnline)
	{
		PlayerModule.<CmdSavePlayerData>d__84 <CmdSavePlayerData>d__;
		<CmdSavePlayerData>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CmdSavePlayerData>d__.<>4__this = this;
		<CmdSavePlayerData>d__.isOnline = isOnline;
		<CmdSavePlayerData>d__.<>1__state = -1;
		<CmdSavePlayerData>d__.<>t__builder.Start<PlayerModule.<CmdSavePlayerData>d__84>(ref <CmdSavePlayerData>d__);
	}

	// Token: 0x0600146E RID: 5230 RVA: 0x000665C7 File Offset: 0x000647C7
	protected static void InvokeUserCode_CmdSavePlayerData__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSavePlayerData called on client.");
			return;
		}
		((PlayerModule)obj).UserCode_CmdSavePlayerData__Boolean(reader.ReadBool());
	}

	// Token: 0x0600146F RID: 5231 RVA: 0x000665F0 File Offset: 0x000647F0
	protected void UserCode_CmdRateGame__Int32(int totalStars)
	{
		PlayerModule.<CmdRateGame>d__129 <CmdRateGame>d__;
		<CmdRateGame>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CmdRateGame>d__.<>4__this = this;
		<CmdRateGame>d__.totalStars = totalStars;
		<CmdRateGame>d__.<>1__state = -1;
		<CmdRateGame>d__.<>t__builder.Start<PlayerModule.<CmdRateGame>d__129>(ref <CmdRateGame>d__);
	}

	// Token: 0x06001470 RID: 5232 RVA: 0x0006662F File Offset: 0x0006482F
	protected static void InvokeUserCode_CmdRateGame__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdRateGame called on client.");
			return;
		}
		((PlayerModule)obj).UserCode_CmdRateGame__Int32(reader.ReadInt());
	}

	// Token: 0x06001471 RID: 5233 RVA: 0x00066658 File Offset: 0x00064858
	protected void UserCode_TargetShowWarningMessage__String(string message)
	{
		this.uiSystemModule.ShowInformationWindow("warning_title", message, null);
	}

	// Token: 0x06001472 RID: 5234 RVA: 0x0006666C File Offset: 0x0006486C
	protected static void InvokeUserCode_TargetShowWarningMessage__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetShowWarningMessage called on server.");
			return;
		}
		((PlayerModule)obj).UserCode_TargetShowWarningMessage__String(reader.ReadString());
	}

	// Token: 0x06001473 RID: 5235 RVA: 0x00066695 File Offset: 0x00064895
	protected void UserCode_TargetShowFreePremiumWindow()
	{
		this.uiSystemModule.ShowFreePremiumWindow();
	}

	// Token: 0x06001474 RID: 5236 RVA: 0x000666A2 File Offset: 0x000648A2
	protected static void InvokeUserCode_TargetShowFreePremiumWindow(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetShowFreePremiumWindow called on server.");
			return;
		}
		((PlayerModule)obj).UserCode_TargetShowFreePremiumWindow();
	}

	// Token: 0x06001475 RID: 5237 RVA: 0x000666C8 File Offset: 0x000648C8
	protected void UserCode_CmdTakeFreePremium()
	{
		PlayerModule.<CmdTakeFreePremium>d__163 <CmdTakeFreePremium>d__;
		<CmdTakeFreePremium>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CmdTakeFreePremium>d__.<>4__this = this;
		<CmdTakeFreePremium>d__.<>1__state = -1;
		<CmdTakeFreePremium>d__.<>t__builder.Start<PlayerModule.<CmdTakeFreePremium>d__163>(ref <CmdTakeFreePremium>d__);
	}

	// Token: 0x06001476 RID: 5238 RVA: 0x000666FF File Offset: 0x000648FF
	protected static void InvokeUserCode_CmdTakeFreePremium(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdTakeFreePremium called on client.");
			return;
		}
		((PlayerModule)obj).UserCode_CmdTakeFreePremium();
	}

	// Token: 0x06001477 RID: 5239 RVA: 0x00066722 File Offset: 0x00064922
	protected void UserCode_TargetShowRateGameWindow()
	{
		if (PlayerPrefs.HasKey("GameRating") && PlayerPrefs.GetInt("GameRating") >= 4)
		{
			return;
		}
		this.uiSystemModule.ShowRateGameWindow();
	}

	// Token: 0x06001478 RID: 5240 RVA: 0x00066749 File Offset: 0x00064949
	protected static void InvokeUserCode_TargetShowRateGameWindow(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetShowRateGameWindow called on server.");
			return;
		}
		((PlayerModule)obj).UserCode_TargetShowRateGameWindow();
	}

	// Token: 0x06001479 RID: 5241 RVA: 0x0006676C File Offset: 0x0006496C
	protected void UserCode_CmdExecuteAction()
	{
		int layerMask = 1 << LayerMask.NameToLayer("Action Tile");
		Collider2D collider2D = Physics2D.OverlapCircle(base.transform.position, 0.32f, layerMask);
		if (collider2D == null)
		{
			return;
		}
		collider2D.GetComponent<ActionBrushManager>().ExecuteAction(base.gameObject);
	}

	// Token: 0x0600147A RID: 5242 RVA: 0x000667C0 File Offset: 0x000649C0
	protected static void InvokeUserCode_CmdExecuteAction(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdExecuteAction called on client.");
			return;
		}
		((PlayerModule)obj).UserCode_CmdExecuteAction();
	}

	// Token: 0x0600147B RID: 5243 RVA: 0x000667E3 File Offset: 0x000649E3
	protected void UserCode_TargetDisconnect__NetworkConnection(NetworkConnection target)
	{
		this.DisconnectPlayer();
	}

	// Token: 0x0600147C RID: 5244 RVA: 0x000667EB File Offset: 0x000649EB
	protected static void InvokeUserCode_TargetDisconnect__NetworkConnection(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetDisconnect called on server.");
			return;
		}
		((PlayerModule)obj).UserCode_TargetDisconnect__NetworkConnection(null);
	}

	// Token: 0x0600147D RID: 5245 RVA: 0x0006680F File Offset: 0x00064A0F
	protected void UserCode_TargetShowRespawnWindow__NetworkConnection(NetworkConnection t)
	{
		this.HideMap();
		this.uiSystemModule.ShowRespawnWindow();
	}

	// Token: 0x0600147E RID: 5246 RVA: 0x00066822 File Offset: 0x00064A22
	protected static void InvokeUserCode_TargetShowRespawnWindow__NetworkConnection(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetShowRespawnWindow called on server.");
			return;
		}
		((PlayerModule)obj).UserCode_TargetShowRespawnWindow__NetworkConnection(null);
	}

	// Token: 0x0600147F RID: 5247 RVA: 0x00066846 File Offset: 0x00064A46
	protected void UserCode_TargetShowHelpWindow__NetworkConnection(NetworkConnection target)
	{
		this.uiSystemModule.ShowHelpWindow();
	}

	// Token: 0x06001480 RID: 5248 RVA: 0x00066853 File Offset: 0x00064A53
	protected static void InvokeUserCode_TargetShowHelpWindow__NetworkConnection(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetShowHelpWindow called on server.");
			return;
		}
		((PlayerModule)obj).UserCode_TargetShowHelpWindow__NetworkConnection(null);
	}

	// Token: 0x06001481 RID: 5249 RVA: 0x00066878 File Offset: 0x00064A78
	protected void UserCode_CmdSetPlatform__String(string platform)
	{
		PlayerModule.<CmdSetPlatform>d__179 <CmdSetPlatform>d__;
		<CmdSetPlatform>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CmdSetPlatform>d__.<>4__this = this;
		<CmdSetPlatform>d__.platform = platform;
		<CmdSetPlatform>d__.<>1__state = -1;
		<CmdSetPlatform>d__.<>t__builder.Start<PlayerModule.<CmdSetPlatform>d__179>(ref <CmdSetPlatform>d__);
	}

	// Token: 0x06001482 RID: 5250 RVA: 0x000668B7 File Offset: 0x00064AB7
	protected static void InvokeUserCode_CmdSetPlatform__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSetPlatform called on client.");
			return;
		}
		((PlayerModule)obj).UserCode_CmdSetPlatform__String(reader.ReadString());
	}

	// Token: 0x06001483 RID: 5251 RVA: 0x000668E0 File Offset: 0x00064AE0
	protected void UserCode_TargetOpenWarehouse__NetworkConnection__Vector3(NetworkConnection target, Vector3 npcPosition)
	{
		this.uiSystemModule.ShowWarehouseWindow();
	}

	// Token: 0x06001484 RID: 5252 RVA: 0x000668ED File Offset: 0x00064AED
	protected static void InvokeUserCode_TargetOpenWarehouse__NetworkConnection__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetOpenWarehouse called on server.");
			return;
		}
		((PlayerModule)obj).UserCode_TargetOpenWarehouse__NetworkConnection__Vector3(null, reader.ReadVector3());
	}

	// Token: 0x06001485 RID: 5253 RVA: 0x00066917 File Offset: 0x00064B17
	protected void UserCode_TargetOpenInventory__NetworkConnection(NetworkConnection target)
	{
		this.uiSystemModule.ShowInventoryWindow();
	}

	// Token: 0x06001486 RID: 5254 RVA: 0x00066924 File Offset: 0x00064B24
	protected static void InvokeUserCode_TargetOpenInventory__NetworkConnection(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetOpenInventory called on server.");
			return;
		}
		((PlayerModule)obj).UserCode_TargetOpenInventory__NetworkConnection(null);
	}

	// Token: 0x06001487 RID: 5255 RVA: 0x00066948 File Offset: 0x00064B48
	protected void UserCode_CmdRespawn()
	{
		this.StopAutoRespawn();
		this.Respawn();
	}

	// Token: 0x06001488 RID: 5256 RVA: 0x00066956 File Offset: 0x00064B56
	protected static void InvokeUserCode_CmdRespawn(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdRespawn called on client.");
			return;
		}
		((PlayerModule)obj).UserCode_CmdRespawn();
	}

	// Token: 0x06001489 RID: 5257 RVA: 0x00066979 File Offset: 0x00064B79
	protected void UserCode_TargetShowSelectedTargetHud__NetworkConnection__GameObject(NetworkConnection target, GameObject selectedTarget)
	{
		this.combatModule.Target = selectedTarget;
		this.uiSystemModule.ShowTargetHud(selectedTarget);
	}

	// Token: 0x0600148A RID: 5258 RVA: 0x00066993 File Offset: 0x00064B93
	protected static void InvokeUserCode_TargetShowSelectedTargetHud__NetworkConnection__GameObject(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetShowSelectedTargetHud called on server.");
			return;
		}
		((PlayerModule)obj).UserCode_TargetShowSelectedTargetHud__NetworkConnection__GameObject(null, reader.ReadGameObject());
	}

	// Token: 0x0600148B RID: 5259 RVA: 0x000669BD File Offset: 0x00064BBD
	protected void UserCode_TargetHideSelectedTargetHud__NetworkConnection(NetworkConnection target)
	{
		this.uiSystemModule.HideTargetHud();
	}

	// Token: 0x0600148C RID: 5260 RVA: 0x000669CA File Offset: 0x00064BCA
	protected static void InvokeUserCode_TargetHideSelectedTargetHud__NetworkConnection(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetHideSelectedTargetHud called on server.");
			return;
		}
		((PlayerModule)obj).UserCode_TargetHideSelectedTargetHud__NetworkConnection(null);
	}

	// Token: 0x0600148D RID: 5261 RVA: 0x000669F0 File Offset: 0x00064BF0
	protected void UserCode_CmdCancelOrder__Int32(int orderId)
	{
		PlayerModule.<CmdCancelOrder>d__197 <CmdCancelOrder>d__;
		<CmdCancelOrder>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CmdCancelOrder>d__.<>4__this = this;
		<CmdCancelOrder>d__.orderId = orderId;
		<CmdCancelOrder>d__.<>1__state = -1;
		<CmdCancelOrder>d__.<>t__builder.Start<PlayerModule.<CmdCancelOrder>d__197>(ref <CmdCancelOrder>d__);
	}

	// Token: 0x0600148E RID: 5262 RVA: 0x00066A2F File Offset: 0x00064C2F
	protected static void InvokeUserCode_CmdCancelOrder__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdCancelOrder called on client.");
			return;
		}
		((PlayerModule)obj).UserCode_CmdCancelOrder__Int32(reader.ReadInt());
	}

	// Token: 0x0600148F RID: 5263 RVA: 0x00066A58 File Offset: 0x00064C58
	protected void UserCode_TargetRefreshMarketStorage__NetworkConnection(NetworkConnection target)
	{
		base.StartCoroutine(this.storageManager.LoadOrders());
	}

	// Token: 0x06001490 RID: 5264 RVA: 0x00066A6C File Offset: 0x00064C6C
	protected static void InvokeUserCode_TargetRefreshMarketStorage__NetworkConnection(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetRefreshMarketStorage called on server.");
			return;
		}
		((PlayerModule)obj).UserCode_TargetRefreshMarketStorage__NetworkConnection(null);
	}

	// Token: 0x06001491 RID: 5265 RVA: 0x00066A90 File Offset: 0x00064C90
	protected void UserCode_TargetRefreshBuyMarket__NetworkConnection(NetworkConnection target)
	{
		base.StartCoroutine(this.buyMarketManager.LoadOrders());
	}

	// Token: 0x06001492 RID: 5266 RVA: 0x00066AA4 File Offset: 0x00064CA4
	protected static void InvokeUserCode_TargetRefreshBuyMarket__NetworkConnection(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetRefreshBuyMarket called on server.");
			return;
		}
		((PlayerModule)obj).UserCode_TargetRefreshBuyMarket__NetworkConnection(null);
	}

	// Token: 0x06001493 RID: 5267 RVA: 0x00066AC8 File Offset: 0x00064CC8
	protected void UserCode_TargetRefreshSellMarket__NetworkConnection(NetworkConnection target)
	{
		base.StartCoroutine(this.sellMarketManager.LoadOrders());
	}

	// Token: 0x06001494 RID: 5268 RVA: 0x00066ADC File Offset: 0x00064CDC
	protected static void InvokeUserCode_TargetRefreshSellMarket__NetworkConnection(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetRefreshSellMarket called on server.");
			return;
		}
		((PlayerModule)obj).UserCode_TargetRefreshSellMarket__NetworkConnection(null);
	}

	// Token: 0x06001495 RID: 5269 RVA: 0x00066B00 File Offset: 0x00064D00
	protected void UserCode_TargetRefreshMyOffers__NetworkConnection(NetworkConnection target)
	{
		base.StartCoroutine(this.myOffersManager.LoadOrders());
	}

	// Token: 0x06001496 RID: 5270 RVA: 0x00066B14 File Offset: 0x00064D14
	protected static void InvokeUserCode_TargetRefreshMyOffers__NetworkConnection(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetRefreshMyOffers called on server.");
			return;
		}
		((PlayerModule)obj).UserCode_TargetRefreshMyOffers__NetworkConnection(null);
	}

	// Token: 0x06001497 RID: 5271 RVA: 0x00066B38 File Offset: 0x00064D38
	protected void UserCode_CmdPlaceOrder__MarketOrder__Int32__Int32(MarketOrder order, int amount, int unitValue)
	{
		PlayerModule.<CmdPlaceOrder>d__202 <CmdPlaceOrder>d__;
		<CmdPlaceOrder>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CmdPlaceOrder>d__.<>4__this = this;
		<CmdPlaceOrder>d__.order = order;
		<CmdPlaceOrder>d__.amount = amount;
		<CmdPlaceOrder>d__.unitValue = unitValue;
		<CmdPlaceOrder>d__.<>1__state = -1;
		<CmdPlaceOrder>d__.<>t__builder.Start<PlayerModule.<CmdPlaceOrder>d__202>(ref <CmdPlaceOrder>d__);
	}

	// Token: 0x06001498 RID: 5272 RVA: 0x00066B87 File Offset: 0x00064D87
	protected static void InvokeUserCode_CmdPlaceOrder__MarketOrder__Int32__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdPlaceOrder called on client.");
			return;
		}
		((PlayerModule)obj).UserCode_CmdPlaceOrder__MarketOrder__Int32__Int32(Mirror.GeneratedNetworkCode._Read_MarketOrder(reader), reader.ReadInt(), reader.ReadInt());
	}

	// Token: 0x06001499 RID: 5273 RVA: 0x00066BBC File Offset: 0x00064DBC
	protected void UserCode_CmdBuyFromGemMarket__Int32__Int32(int orderId, int amount)
	{
		PlayerModule.<CmdBuyFromGemMarket>d__203 <CmdBuyFromGemMarket>d__;
		<CmdBuyFromGemMarket>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CmdBuyFromGemMarket>d__.<>4__this = this;
		<CmdBuyFromGemMarket>d__.orderId = orderId;
		<CmdBuyFromGemMarket>d__.amount = amount;
		<CmdBuyFromGemMarket>d__.<>1__state = -1;
		<CmdBuyFromGemMarket>d__.<>t__builder.Start<PlayerModule.<CmdBuyFromGemMarket>d__203>(ref <CmdBuyFromGemMarket>d__);
	}

	// Token: 0x0600149A RID: 5274 RVA: 0x00066C03 File Offset: 0x00064E03
	protected static void InvokeUserCode_CmdBuyFromGemMarket__Int32__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdBuyFromGemMarket called on client.");
			return;
		}
		((PlayerModule)obj).UserCode_CmdBuyFromGemMarket__Int32__Int32(reader.ReadInt(), reader.ReadInt());
	}

	// Token: 0x0600149B RID: 5275 RVA: 0x00066C34 File Offset: 0x00064E34
	protected void UserCode_CmdBuyFromMarket__Int32__Int32(int orderId, int amount)
	{
		PlayerModule.<CmdBuyFromMarket>d__204 <CmdBuyFromMarket>d__;
		<CmdBuyFromMarket>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CmdBuyFromMarket>d__.<>4__this = this;
		<CmdBuyFromMarket>d__.orderId = orderId;
		<CmdBuyFromMarket>d__.amount = amount;
		<CmdBuyFromMarket>d__.<>1__state = -1;
		<CmdBuyFromMarket>d__.<>t__builder.Start<PlayerModule.<CmdBuyFromMarket>d__204>(ref <CmdBuyFromMarket>d__);
	}

	// Token: 0x0600149C RID: 5276 RVA: 0x00066C7B File Offset: 0x00064E7B
	protected static void InvokeUserCode_CmdBuyFromMarket__Int32__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdBuyFromMarket called on client.");
			return;
		}
		((PlayerModule)obj).UserCode_CmdBuyFromMarket__Int32__Int32(reader.ReadInt(), reader.ReadInt());
	}

	// Token: 0x0600149D RID: 5277 RVA: 0x00066CAC File Offset: 0x00064EAC
	protected void UserCode_CmdTakeFromStorage__Int32(int storageId)
	{
		PlayerModule.<CmdTakeFromStorage>d__205 <CmdTakeFromStorage>d__;
		<CmdTakeFromStorage>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CmdTakeFromStorage>d__.<>4__this = this;
		<CmdTakeFromStorage>d__.storageId = storageId;
		<CmdTakeFromStorage>d__.<>1__state = -1;
		<CmdTakeFromStorage>d__.<>t__builder.Start<PlayerModule.<CmdTakeFromStorage>d__205>(ref <CmdTakeFromStorage>d__);
	}

	// Token: 0x0600149E RID: 5278 RVA: 0x00066CEB File Offset: 0x00064EEB
	protected static void InvokeUserCode_CmdTakeFromStorage__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdTakeFromStorage called on client.");
			return;
		}
		((PlayerModule)obj).UserCode_CmdTakeFromStorage__Int32(reader.ReadInt());
	}

	// Token: 0x0600149F RID: 5279 RVA: 0x00066D14 File Offset: 0x00064F14
	protected void UserCode_TargetReloadStoreItems__NetworkConnection(NetworkConnection target)
	{
		this.uiSystemModule.DialogWindowManager.ReloadStoreItems();
	}

	// Token: 0x060014A0 RID: 5280 RVA: 0x00066D26 File Offset: 0x00064F26
	protected static void InvokeUserCode_TargetReloadStoreItems__NetworkConnection(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetReloadStoreItems called on server.");
			return;
		}
		((PlayerModule)obj).UserCode_TargetReloadStoreItems__NetworkConnection(null);
	}

	// Token: 0x060014A1 RID: 5281 RVA: 0x00066D4A File Offset: 0x00064F4A
	protected void UserCode_TargetRenderNpcDialog__NetworkConnection__NpcDialog(NetworkConnection target, NpcDialog npcDialog)
	{
		this.uiSystemModule.ShowDialogWindow(npcDialog);
	}

	// Token: 0x060014A2 RID: 5282 RVA: 0x00066D58 File Offset: 0x00064F58
	protected static void InvokeUserCode_TargetRenderNpcDialog__NetworkConnection__NpcDialog(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetRenderNpcDialog called on server.");
			return;
		}
		((PlayerModule)obj).UserCode_TargetRenderNpcDialog__NetworkConnection__NpcDialog(null, Mirror.GeneratedNetworkCode._Read_NpcDialog(reader));
	}

	// Token: 0x060014A3 RID: 5283 RVA: 0x00066D82 File Offset: 0x00064F82
	protected void UserCode_TargetCloseDialogWindow__NetworkConnection(NetworkConnection target)
	{
		this.uiSystemModule.HideDialogWindow();
	}

	// Token: 0x060014A4 RID: 5284 RVA: 0x00066D8F File Offset: 0x00064F8F
	protected static void InvokeUserCode_TargetCloseDialogWindow__NetworkConnection(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetCloseDialogWindow called on server.");
			return;
		}
		((PlayerModule)obj).UserCode_TargetCloseDialogWindow__NetworkConnection(null);
	}

	// Token: 0x060014A5 RID: 5285 RVA: 0x00066DB4 File Offset: 0x00064FB4
	protected void UserCode_CmdNpcHandshake__GameObject(GameObject npc)
	{
		NpcModule component = npc.GetComponent<NpcModule>();
		if (component == null)
		{
			return;
		}
		component.Handshake(base.gameObject);
		this.activeNpc = npc;
	}

	// Token: 0x060014A6 RID: 5286 RVA: 0x00066DE5 File Offset: 0x00064FE5
	protected static void InvokeUserCode_CmdNpcHandshake__GameObject(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdNpcHandshake called on client.");
			return;
		}
		((PlayerModule)obj).UserCode_CmdNpcHandshake__GameObject(reader.ReadGameObject());
	}

	// Token: 0x060014A7 RID: 5287 RVA: 0x00066E10 File Offset: 0x00065010
	protected void UserCode_CmdSendChoice__Int32(int actionId)
	{
		if (this.activeNpc == null)
		{
			this.TargetCloseDialogWindow(base.connectionToClient);
			return;
		}
		GameObject gameObject = this.activeNpc;
		NpcModule npcModule = (gameObject != null) ? gameObject.GetComponent<NpcModule>() : null;
		if (npcModule == null)
		{
			return;
		}
		npcModule.Choose(base.gameObject, actionId);
	}

	// Token: 0x060014A8 RID: 5288 RVA: 0x00066E62 File Offset: 0x00065062
	protected static void InvokeUserCode_CmdSendChoice__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSendChoice called on client.");
			return;
		}
		((PlayerModule)obj).UserCode_CmdSendChoice__Int32(reader.ReadInt());
	}

	// Token: 0x060014A9 RID: 5289 RVA: 0x00066E8B File Offset: 0x0006508B
	protected void UserCode_TargetShowStoryline()
	{
		this.uiSystemModule.ShowStoryline();
	}

	// Token: 0x060014AA RID: 5290 RVA: 0x00066E98 File Offset: 0x00065098
	protected static void InvokeUserCode_TargetShowStoryline(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetShowStoryline called on server.");
			return;
		}
		((PlayerModule)obj).UserCode_TargetShowStoryline();
	}

	// Token: 0x060014AB RID: 5291 RVA: 0x00066EBB File Offset: 0x000650BB
	protected void UserCode_CmdOpenQuests()
	{
		this.TargetOpenQuests(base.connectionToClient);
	}

	// Token: 0x060014AC RID: 5292 RVA: 0x00066EC9 File Offset: 0x000650C9
	protected static void InvokeUserCode_CmdOpenQuests(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdOpenQuests called on client.");
			return;
		}
		((PlayerModule)obj).UserCode_CmdOpenQuests();
	}

	// Token: 0x060014AD RID: 5293 RVA: 0x00066EEC File Offset: 0x000650EC
	protected void UserCode_TargetOpenQuests__NetworkConnection(NetworkConnection target)
	{
		this.uiSystemModule.ToggleQuestWindow();
	}

	// Token: 0x060014AE RID: 5294 RVA: 0x00066EF9 File Offset: 0x000650F9
	protected static void InvokeUserCode_TargetOpenQuests__NetworkConnection(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetOpenQuests called on server.");
			return;
		}
		((PlayerModule)obj).UserCode_TargetOpenQuests__NetworkConnection(null);
	}

	// Token: 0x060014AF RID: 5295 RVA: 0x00066F1D File Offset: 0x0006511D
	protected void UserCode_TargetChangePlayerSceneMap__String(string newMapScene)
	{
		base.StartCoroutine(this.LoadMapScene(newMapScene));
	}

	// Token: 0x060014B0 RID: 5296 RVA: 0x00066F2D File Offset: 0x0006512D
	protected static void InvokeUserCode_TargetChangePlayerSceneMap__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetChangePlayerSceneMap called on server.");
			return;
		}
		((PlayerModule)obj).UserCode_TargetChangePlayerSceneMap__String(reader.ReadString());
	}

	// Token: 0x060014B1 RID: 5297 RVA: 0x00066F58 File Offset: 0x00065158
	protected void UserCode_CmdLoadFriendList()
	{
		PlayerModule.<CmdLoadFriendList>d__219 <CmdLoadFriendList>d__;
		<CmdLoadFriendList>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CmdLoadFriendList>d__.<>4__this = this;
		<CmdLoadFriendList>d__.<>1__state = -1;
		<CmdLoadFriendList>d__.<>t__builder.Start<PlayerModule.<CmdLoadFriendList>d__219>(ref <CmdLoadFriendList>d__);
	}

	// Token: 0x060014B2 RID: 5298 RVA: 0x00066F8F File Offset: 0x0006518F
	protected static void InvokeUserCode_CmdLoadFriendList(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdLoadFriendList called on client.");
			return;
		}
		((PlayerModule)obj).UserCode_CmdLoadFriendList();
	}

	// Token: 0x060014B3 RID: 5299 RVA: 0x00066FB4 File Offset: 0x000651B4
	static PlayerModule()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(PlayerModule), "System.Void PlayerModule::CmdSavePlayerData(System.Boolean)", new RemoteCallDelegate(PlayerModule.InvokeUserCode_CmdSavePlayerData__Boolean), true);
		RemoteProcedureCalls.RegisterCommand(typeof(PlayerModule), "System.Void PlayerModule::CmdRateGame(System.Int32)", new RemoteCallDelegate(PlayerModule.InvokeUserCode_CmdRateGame__Int32), true);
		RemoteProcedureCalls.RegisterCommand(typeof(PlayerModule), "System.Void PlayerModule::CmdTakeFreePremium()", new RemoteCallDelegate(PlayerModule.InvokeUserCode_CmdTakeFreePremium), true);
		RemoteProcedureCalls.RegisterCommand(typeof(PlayerModule), "System.Void PlayerModule::CmdExecuteAction()", new RemoteCallDelegate(PlayerModule.InvokeUserCode_CmdExecuteAction), true);
		RemoteProcedureCalls.RegisterCommand(typeof(PlayerModule), "System.Void PlayerModule::CmdSetPlatform(System.String)", new RemoteCallDelegate(PlayerModule.InvokeUserCode_CmdSetPlatform__String), true);
		RemoteProcedureCalls.RegisterCommand(typeof(PlayerModule), "System.Void PlayerModule::CmdRespawn()", new RemoteCallDelegate(PlayerModule.InvokeUserCode_CmdRespawn), true);
		RemoteProcedureCalls.RegisterCommand(typeof(PlayerModule), "System.Void PlayerModule::CmdCancelOrder(System.Int32)", new RemoteCallDelegate(PlayerModule.InvokeUserCode_CmdCancelOrder__Int32), true);
		RemoteProcedureCalls.RegisterCommand(typeof(PlayerModule), "System.Void PlayerModule::CmdPlaceOrder(MarketOrder,System.Int32,System.Int32)", new RemoteCallDelegate(PlayerModule.InvokeUserCode_CmdPlaceOrder__MarketOrder__Int32__Int32), true);
		RemoteProcedureCalls.RegisterCommand(typeof(PlayerModule), "System.Void PlayerModule::CmdBuyFromGemMarket(System.Int32,System.Int32)", new RemoteCallDelegate(PlayerModule.InvokeUserCode_CmdBuyFromGemMarket__Int32__Int32), true);
		RemoteProcedureCalls.RegisterCommand(typeof(PlayerModule), "System.Void PlayerModule::CmdBuyFromMarket(System.Int32,System.Int32)", new RemoteCallDelegate(PlayerModule.InvokeUserCode_CmdBuyFromMarket__Int32__Int32), true);
		RemoteProcedureCalls.RegisterCommand(typeof(PlayerModule), "System.Void PlayerModule::CmdTakeFromStorage(System.Int32)", new RemoteCallDelegate(PlayerModule.InvokeUserCode_CmdTakeFromStorage__Int32), true);
		RemoteProcedureCalls.RegisterCommand(typeof(PlayerModule), "System.Void PlayerModule::CmdNpcHandshake(UnityEngine.GameObject)", new RemoteCallDelegate(PlayerModule.InvokeUserCode_CmdNpcHandshake__GameObject), true);
		RemoteProcedureCalls.RegisterCommand(typeof(PlayerModule), "System.Void PlayerModule::CmdSendChoice(System.Int32)", new RemoteCallDelegate(PlayerModule.InvokeUserCode_CmdSendChoice__Int32), true);
		RemoteProcedureCalls.RegisterCommand(typeof(PlayerModule), "System.Void PlayerModule::CmdOpenQuests()", new RemoteCallDelegate(PlayerModule.InvokeUserCode_CmdOpenQuests), true);
		RemoteProcedureCalls.RegisterCommand(typeof(PlayerModule), "System.Void PlayerModule::CmdLoadFriendList()", new RemoteCallDelegate(PlayerModule.InvokeUserCode_CmdLoadFriendList), true);
		RemoteProcedureCalls.RegisterRpc(typeof(PlayerModule), "System.Void PlayerModule::TargetShowWarningMessage(System.String)", new RemoteCallDelegate(PlayerModule.InvokeUserCode_TargetShowWarningMessage__String));
		RemoteProcedureCalls.RegisterRpc(typeof(PlayerModule), "System.Void PlayerModule::TargetShowFreePremiumWindow()", new RemoteCallDelegate(PlayerModule.InvokeUserCode_TargetShowFreePremiumWindow));
		RemoteProcedureCalls.RegisterRpc(typeof(PlayerModule), "System.Void PlayerModule::TargetShowRateGameWindow()", new RemoteCallDelegate(PlayerModule.InvokeUserCode_TargetShowRateGameWindow));
		RemoteProcedureCalls.RegisterRpc(typeof(PlayerModule), "System.Void PlayerModule::TargetDisconnect(Mirror.NetworkConnection)", new RemoteCallDelegate(PlayerModule.InvokeUserCode_TargetDisconnect__NetworkConnection));
		RemoteProcedureCalls.RegisterRpc(typeof(PlayerModule), "System.Void PlayerModule::TargetShowRespawnWindow(Mirror.NetworkConnection)", new RemoteCallDelegate(PlayerModule.InvokeUserCode_TargetShowRespawnWindow__NetworkConnection));
		RemoteProcedureCalls.RegisterRpc(typeof(PlayerModule), "System.Void PlayerModule::TargetShowHelpWindow(Mirror.NetworkConnection)", new RemoteCallDelegate(PlayerModule.InvokeUserCode_TargetShowHelpWindow__NetworkConnection));
		RemoteProcedureCalls.RegisterRpc(typeof(PlayerModule), "System.Void PlayerModule::TargetOpenWarehouse(Mirror.NetworkConnection,UnityEngine.Vector3)", new RemoteCallDelegate(PlayerModule.InvokeUserCode_TargetOpenWarehouse__NetworkConnection__Vector3));
		RemoteProcedureCalls.RegisterRpc(typeof(PlayerModule), "System.Void PlayerModule::TargetOpenInventory(Mirror.NetworkConnection)", new RemoteCallDelegate(PlayerModule.InvokeUserCode_TargetOpenInventory__NetworkConnection));
		RemoteProcedureCalls.RegisterRpc(typeof(PlayerModule), "System.Void PlayerModule::TargetShowSelectedTargetHud(Mirror.NetworkConnection,UnityEngine.GameObject)", new RemoteCallDelegate(PlayerModule.InvokeUserCode_TargetShowSelectedTargetHud__NetworkConnection__GameObject));
		RemoteProcedureCalls.RegisterRpc(typeof(PlayerModule), "System.Void PlayerModule::TargetHideSelectedTargetHud(Mirror.NetworkConnection)", new RemoteCallDelegate(PlayerModule.InvokeUserCode_TargetHideSelectedTargetHud__NetworkConnection));
		RemoteProcedureCalls.RegisterRpc(typeof(PlayerModule), "System.Void PlayerModule::TargetRefreshMarketStorage(Mirror.NetworkConnection)", new RemoteCallDelegate(PlayerModule.InvokeUserCode_TargetRefreshMarketStorage__NetworkConnection));
		RemoteProcedureCalls.RegisterRpc(typeof(PlayerModule), "System.Void PlayerModule::TargetRefreshBuyMarket(Mirror.NetworkConnection)", new RemoteCallDelegate(PlayerModule.InvokeUserCode_TargetRefreshBuyMarket__NetworkConnection));
		RemoteProcedureCalls.RegisterRpc(typeof(PlayerModule), "System.Void PlayerModule::TargetRefreshSellMarket(Mirror.NetworkConnection)", new RemoteCallDelegate(PlayerModule.InvokeUserCode_TargetRefreshSellMarket__NetworkConnection));
		RemoteProcedureCalls.RegisterRpc(typeof(PlayerModule), "System.Void PlayerModule::TargetRefreshMyOffers(Mirror.NetworkConnection)", new RemoteCallDelegate(PlayerModule.InvokeUserCode_TargetRefreshMyOffers__NetworkConnection));
		RemoteProcedureCalls.RegisterRpc(typeof(PlayerModule), "System.Void PlayerModule::TargetReloadStoreItems(Mirror.NetworkConnection)", new RemoteCallDelegate(PlayerModule.InvokeUserCode_TargetReloadStoreItems__NetworkConnection));
		RemoteProcedureCalls.RegisterRpc(typeof(PlayerModule), "System.Void PlayerModule::TargetRenderNpcDialog(Mirror.NetworkConnection,NpcDialog)", new RemoteCallDelegate(PlayerModule.InvokeUserCode_TargetRenderNpcDialog__NetworkConnection__NpcDialog));
		RemoteProcedureCalls.RegisterRpc(typeof(PlayerModule), "System.Void PlayerModule::TargetCloseDialogWindow(Mirror.NetworkConnection)", new RemoteCallDelegate(PlayerModule.InvokeUserCode_TargetCloseDialogWindow__NetworkConnection));
		RemoteProcedureCalls.RegisterRpc(typeof(PlayerModule), "System.Void PlayerModule::TargetShowStoryline()", new RemoteCallDelegate(PlayerModule.InvokeUserCode_TargetShowStoryline));
		RemoteProcedureCalls.RegisterRpc(typeof(PlayerModule), "System.Void PlayerModule::TargetOpenQuests(Mirror.NetworkConnection)", new RemoteCallDelegate(PlayerModule.InvokeUserCode_TargetOpenQuests__NetworkConnection));
		RemoteProcedureCalls.RegisterRpc(typeof(PlayerModule), "System.Void PlayerModule::TargetChangePlayerSceneMap(System.String)", new RemoteCallDelegate(PlayerModule.InvokeUserCode_TargetChangePlayerSceneMap__String));
	}

	// Token: 0x060014B4 RID: 5300 RVA: 0x00067430 File Offset: 0x00065630
	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteInt(this.ServerId);
			writer.WriteInt(this.PlayerId);
			Mirror.GeneratedNetworkCode._Write_PackageType(writer, this.PackageType);
			writer.WriteInt(this.DaysSinceLastOrder);
			writer.WriteBool(this.IsFirstConnection);
			writer.WriteInt(this.PremiumDays);
			writer.WriteBool(this.HasItemOnStorage);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1UL) != 0UL)
		{
			writer.WriteInt(this.ServerId);
		}
		if ((base.syncVarDirtyBits & 2UL) != 0UL)
		{
			writer.WriteInt(this.PlayerId);
		}
		if ((base.syncVarDirtyBits & 4UL) != 0UL)
		{
			Mirror.GeneratedNetworkCode._Write_PackageType(writer, this.PackageType);
		}
		if ((base.syncVarDirtyBits & 8UL) != 0UL)
		{
			writer.WriteInt(this.DaysSinceLastOrder);
		}
		if ((base.syncVarDirtyBits & 16UL) != 0UL)
		{
			writer.WriteBool(this.IsFirstConnection);
		}
		if ((base.syncVarDirtyBits & 32UL) != 0UL)
		{
			writer.WriteInt(this.PremiumDays);
		}
		if ((base.syncVarDirtyBits & 64UL) != 0UL)
		{
			writer.WriteBool(this.HasItemOnStorage);
		}
	}

	// Token: 0x060014B5 RID: 5301 RVA: 0x0006759C File Offset: 0x0006579C
	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.ServerId, null, reader.ReadInt());
			base.GeneratedSyncVarDeserialize<int>(ref this.PlayerId, null, reader.ReadInt());
			base.GeneratedSyncVarDeserialize<PackageType>(ref this.PackageType, null, Mirror.GeneratedNetworkCode._Read_PackageType(reader));
			base.GeneratedSyncVarDeserialize<int>(ref this.DaysSinceLastOrder, null, reader.ReadInt());
			base.GeneratedSyncVarDeserialize<bool>(ref this.IsFirstConnection, null, reader.ReadBool());
			base.GeneratedSyncVarDeserialize<int>(ref this.PremiumDays, new Action<int, int>(this.OnPremiumDays), reader.ReadInt());
			base.GeneratedSyncVarDeserialize<bool>(ref this.HasItemOnStorage, new Action<bool, bool>(this.OnHasItemOnStorage), reader.ReadBool());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 1L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.ServerId, null, reader.ReadInt());
		}
		if ((num & 2L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.PlayerId, null, reader.ReadInt());
		}
		if ((num & 4L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<PackageType>(ref this.PackageType, null, Mirror.GeneratedNetworkCode._Read_PackageType(reader));
		}
		if ((num & 8L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.DaysSinceLastOrder, null, reader.ReadInt());
		}
		if ((num & 16L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<bool>(ref this.IsFirstConnection, null, reader.ReadBool());
		}
		if ((num & 32L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.PremiumDays, new Action<int, int>(this.OnPremiumDays), reader.ReadInt());
		}
		if ((num & 64L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<bool>(ref this.HasItemOnStorage, new Action<bool, bool>(this.OnHasItemOnStorage), reader.ReadBool());
		}
	}

	// Token: 0x0400122E RID: 4654
	[SyncVar]
	public int ServerId;

	// Token: 0x0400122F RID: 4655
	[SyncVar]
	public int PlayerId;

	// Token: 0x04001230 RID: 4656
	[SyncVar]
	public PackageType PackageType;

	// Token: 0x04001231 RID: 4657
	[SyncVar]
	public int DaysSinceLastOrder;

	// Token: 0x04001232 RID: 4658
	[SyncVar]
	public bool IsFirstConnection;

	// Token: 0x04001233 RID: 4659
	[SyncVar(hook = "OnPremiumDays")]
	public int PremiumDays;

	// Token: 0x04001234 RID: 4660
	[SyncVar(hook = "OnHasItemOnStorage")]
	public bool HasItemOnStorage;

	// Token: 0x04001235 RID: 4661
	public int AccountId;

	// Token: 0x04001236 RID: 4662
	public bool GotFreePremium;

	// Token: 0x04001237 RID: 4663
	public bool PlayerIsLoaded;

	// Token: 0x04001238 RID: 4664
	public bool AutoAttackEnabled;

	// Token: 0x04001239 RID: 4665
	public string DevicePlatform;

	// Token: 0x0400123A RID: 4666
	private float itemBarInputTime;

	// Token: 0x0400123B RID: 4667
	private float lastMapActionTime;

	// Token: 0x0400123C RID: 4668
	private float lastAutoAttackTime;

	// Token: 0x0400123D RID: 4669
	private bool isMapActive;

	// Token: 0x0400123E RID: 4670
	private int lastUsedItemBarPosition;

	// Token: 0x0400123F RID: 4671
	private bool numLockHasBeenPressed;

	// Token: 0x04001240 RID: 4672
	[SerializeField]
	private ConditionModule conditionModule;

	// Token: 0x04001241 RID: 4673
	[SerializeField]
	private InventoryModule inventoryModule;

	// Token: 0x04001242 RID: 4674
	[SerializeField]
	private EquipmentModule equipmentModule;

	// Token: 0x04001243 RID: 4675
	[SerializeField]
	private WarehouseModule warehouseModule;

	// Token: 0x04001244 RID: 4676
	[SerializeField]
	private AttributeModule attributeModule;

	// Token: 0x04001245 RID: 4677
	[SerializeField]
	private MovementModule movementModule;

	// Token: 0x04001246 RID: 4678
	[SerializeField]
	private SkillModule skillModule;

	// Token: 0x04001247 RID: 4679
	[SerializeField]
	private MarketModule marketModule;

	// Token: 0x04001248 RID: 4680
	[SerializeField]
	private AnimationControllerModule animationControllerModule;

	// Token: 0x04001249 RID: 4681
	[SerializeField]
	private EffectModule effectModule;

	// Token: 0x0400124A RID: 4682
	[SerializeField]
	private CreatureModule creatureModule;

	// Token: 0x0400124B RID: 4683
	[SerializeField]
	private CombatModule combatModule;

	// Token: 0x0400124C RID: 4684
	[SerializeField]
	private VocationModule vocationModule;

	// Token: 0x0400124D RID: 4685
	[SerializeField]
	private WalletModule walletModule;

	// Token: 0x0400124E RID: 4686
	[SerializeField]
	private ChatModule chatModule;

	// Token: 0x0400124F RID: 4687
	[SerializeField]
	private StoreModule storeModule;

	// Token: 0x04001250 RID: 4688
	[SerializeField]
	private QuestModule questModule;

	// Token: 0x04001251 RID: 4689
	[SerializeField]
	private GameObject localHud;

	// Token: 0x04001252 RID: 4690
	[SerializeField]
	private Image healthBar;

	// Token: 0x04001253 RID: 4691
	[SerializeField]
	private TextMeshPro nameText;

	// Token: 0x04001254 RID: 4692
	[SerializeField]
	private TextMeshPro titleText;

	// Token: 0x04001255 RID: 4693
	[SerializeField]
	private TextMeshPro guildNameText;

	// Token: 0x04001256 RID: 4694
	[SerializeField]
	private Text levelText;

	// Token: 0x04001257 RID: 4695
	[SerializeField]
	private GameObject selectionObject;

	// Token: 0x04001258 RID: 4696
	[SerializeField]
	private Collider2D playerCollider;

	// Token: 0x04001259 RID: 4697
	private StorageManager storageManager;

	// Token: 0x0400125A RID: 4698
	private BuyMarketManager buyMarketManager;

	// Token: 0x0400125B RID: 4699
	private SellMarketManager sellMarketManager;

	// Token: 0x0400125C RID: 4700
	private MyOffersManager myOffersManager;

	// Token: 0x0400125D RID: 4701
	private Canvas uiHudCanvas;

	// Token: 0x0400125E RID: 4702
	private Canvas uiSystemCanvas;

	// Token: 0x0400125F RID: 4703
	private Canvas uiTweaksCanvas;

	// Token: 0x04001260 RID: 4704
	private AIModule aiModule;

	// Token: 0x04001261 RID: 4705
	private PvpModule pvpModule;

	// Token: 0x04001262 RID: 4706
	private AreaModule areaModule;

	// Token: 0x04001263 RID: 4707
	private ItemModule itemModule;

	// Token: 0x04001264 RID: 4708
	private CraftModule craftModule;

	// Token: 0x04001265 RID: 4709
	private GuildModule guildModule;

	// Token: 0x04001266 RID: 4710
	private TitleModule titleModule;

	// Token: 0x04001267 RID: 4711
	private UISystemModule uiSystemModule;

	// Token: 0x04001268 RID: 4712
	private ItemDatabaseModule itemDatabaseModule;

	// Token: 0x04001269 RID: 4713
	private QuestDatabaseModule questDatabaseModule;

	// Token: 0x0400126A RID: 4714
	private SkillDatabaseModule skillDatabaseModule;

	// Token: 0x0400126B RID: 4715
	private GameEnvironmentModule gameEnvironmentModule;

	// Token: 0x0400126C RID: 4716
	private MonsterDatabaseModule monsterDatabaseModule;

	// Token: 0x0400126D RID: 4717
	private BlueprintDatabaseModule blueprintDatabaseModule;

	// Token: 0x0400126E RID: 4718
	private Camera mainCamera;

	// Token: 0x0400126F RID: 4719
	private GameObject activeNpc;

	// Token: 0x04001270 RID: 4720
	private GameObject mainCameraObject;

	// Token: 0x04001271 RID: 4721
	private LayerMask mainCameraLayerMask;

	// Token: 0x04001272 RID: 4722
	private Coroutine autoRespawnOnDeath;

	// Token: 0x04001273 RID: 4723
	private float lastRegeneration;

	// Token: 0x04001274 RID: 4724
	private float collectTime;

	// Token: 0x04001275 RID: 4725
	private bool friendListDirty;

	// Token: 0x04001276 RID: 4726
	public bool ChatFocused;

	// Token: 0x04001277 RID: 4727
	public bool IsSceneLoaded;
}
