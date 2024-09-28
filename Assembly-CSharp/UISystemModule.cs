using System;
using System.Collections;
using DuloGames.UI;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000439 RID: 1081
public class UISystemModule : MonoBehaviour
{
	// Token: 0x060017B6 RID: 6070 RVA: 0x00078488 File Offset: 0x00076688
	private void Awake()
	{
		if (NetworkServer.active)
		{
			UnityEngine.Object.Destroy(this.uiHud);
			UnityEngine.Object.Destroy(this.mobileHud);
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (!GlobalSettings.IsMobilePlatform)
		{
			UnityEngine.Object.Destroy(this.mobileHud);
			return;
		}
		if (GlobalSettings.IsMobilePlatform)
		{
			UnityEngine.Object.Destroy(this.uiHud);
			return;
		}
	}

	// Token: 0x060017B7 RID: 6071 RVA: 0x000784E4 File Offset: 0x000766E4
	private void Update()
	{
		if (NetworkServer.active)
		{
			return;
		}
		this.timeSinceSceneLoaded += Time.deltaTime;
		this.deltaTime += (Time.deltaTime - this.deltaTime) * 0.1f;
		float fps = 1f / this.deltaTime;
		if (Time.time - this.lastFpsUpdate > 1f)
		{
			this.UpdateFpsAndLatencyTexts(fps);
			UIIgnoreRaycast[] componentsInChildren = base.gameObject.GetComponentsInChildren<UIIgnoreRaycast>(true);
			if (componentsInChildren.Length > 1)
			{
				UIIgnoreRaycast[] array = componentsInChildren;
				for (int i = 0; i < array.Length; i++)
				{
					UnityEngine.Object.Destroy(array[i].gameObject);
				}
			}
		}
	}

	// Token: 0x060017B8 RID: 6072 RVA: 0x00078584 File Offset: 0x00076784
	private void UpdateFpsAndLatencyTexts(float fps)
	{
		if (GlobalSettings.IsMobilePlatform)
		{
			return;
		}
		this.lastFpsUpdate = Time.time;
		this.FpsText.text = string.Format("FPS: {0}", Mathf.Ceil(fps));
		double num = 0.0;
		if (this.PlayerModule != null && NetworkClient.isConnected)
		{
			num = NetworkTime.rtt * 1000.0;
		}
		string text = LanguageManager.Instance.GetText("connection_quality_label");
		if (this.timeSinceSceneLoaded <= 60f)
		{
			string text2 = LanguageManager.Instance.GetText("connection_quality_measuring");
			if (Application.platform == RuntimePlatform.OSXEditor)
			{
				text2 += string.Format(" ({0:0})", num);
			}
			this.LatencyText.text = text + ": " + text2;
			return;
		}
		string text3 = this.GetConnectionQualityDescription(num);
		if (Application.platform == RuntimePlatform.OSXEditor)
		{
			text3 += string.Format(" ({0:0})", num);
		}
		this.LatencyText.text = text + ": " + text3;
	}

	// Token: 0x060017B9 RID: 6073 RVA: 0x00078694 File Offset: 0x00076894
	private string GetConnectionQualityDescription(double ping)
	{
		if (ping < 300.0)
		{
			return LanguageManager.Instance.GetText("connection_quality_great");
		}
		if (ping >= 300.0 & ping < 400.0)
		{
			return LanguageManager.Instance.GetText("connection_quality_good");
		}
		if (ping >= 400.0 & ping < 500.0)
		{
			return LanguageManager.Instance.GetText("connection_quality_average");
		}
		if (ping >= 500.0 & ping < 600.0)
		{
			return LanguageManager.Instance.GetText("connection_quality_poor");
		}
		return LanguageManager.Instance.GetText("connection_quality_very_poor");
	}

	// Token: 0x17000263 RID: 611
	// (get) Token: 0x060017BA RID: 6074 RVA: 0x00078756 File Offset: 0x00076956
	public Image PlatformPvpButtonIcon
	{
		get
		{
			if (GlobalSettings.IsMobilePlatform)
			{
				return this.mobilePvpButtonIcon;
			}
			return this.pvpButtonIcon;
		}
	}

	// Token: 0x17000264 RID: 612
	// (get) Token: 0x060017BB RID: 6075 RVA: 0x0007876C File Offset: 0x0007696C
	public Text PlatformCurrentWorldAreaName
	{
		get
		{
			if (GlobalSettings.IsMobilePlatform)
			{
				return this.mobileCurrentWorldAreaName;
			}
			return this.currentWorldAreaName;
		}
	}

	// Token: 0x17000265 RID: 613
	// (get) Token: 0x060017BC RID: 6076 RVA: 0x00078782 File Offset: 0x00076982
	public GameObject PlatformTargetConditionBar
	{
		get
		{
			if (GlobalSettings.IsMobilePlatform)
			{
				return this.mobileTargetConditionBar;
			}
			return this.targetConditionBar;
		}
	}

	// Token: 0x17000266 RID: 614
	// (get) Token: 0x060017BD RID: 6077 RVA: 0x00078798 File Offset: 0x00076998
	public ChatHolderManager PlatformChatHolderManager
	{
		get
		{
			if (GlobalSettings.IsMobilePlatform)
			{
				return this.MobileChatHolderManager;
			}
			return this.ChatHolderManager;
		}
	}

	// Token: 0x17000267 RID: 615
	// (get) Token: 0x060017BE RID: 6078 RVA: 0x000787B0 File Offset: 0x000769B0
	public bool NeedKeyboard
	{
		get
		{
			return this.FriendListWindow.activeInHierarchy | this.TradingPostWindow.activeInHierarchy | this.ChangeNameWindow.activeInHierarchy | this.GuildCreationWindow.activeInHierarchy | this.ChangeGuildWindow.activeInHierarchy | this.OptionsWindow.activeInHierarchy;
		}
	}

	// Token: 0x17000268 RID: 616
	// (get) Token: 0x060017BF RID: 6079 RVA: 0x00078804 File Offset: 0x00076A04
	public bool NeedNumberInputAccess
	{
		get
		{
			return this.DialogWindow.activeInHierarchy | this.ItemSplitWindow.activeInHierarchy | this.AddGoldWindow.activeInHierarchy;
		}
	}

	// Token: 0x060017C0 RID: 6080 RVA: 0x00078829 File Offset: 0x00076A29
	public void ShowBossEventWindow(string bossName, int bossLevel, Vector2 bossLocation)
	{
		if (this.BossEventWindow.activeInHierarchy)
		{
			return;
		}
		this.BossEventWindow.SetActive(true);
		this.BossEventWindow.GetComponent<BossEventWindowManager>().Initialize(bossName, bossLevel, bossLocation);
	}

	// Token: 0x060017C1 RID: 6081 RVA: 0x00078858 File Offset: 0x00076A58
	public void ShowTvtEventWindow()
	{
		if (this.TvtEventWindow.activeInHierarchy)
		{
			return;
		}
		this.TvtEventWindow.SetActive(true);
	}

	// Token: 0x060017C2 RID: 6082 RVA: 0x00078874 File Offset: 0x00076A74
	public void ShowPromoPackagesWindow()
	{
		if (this.PromoPackagesWindow.activeInHierarchy)
		{
			return;
		}
		this.PromoPackagesWindow.SetActive(true);
	}

	// Token: 0x060017C3 RID: 6083 RVA: 0x00078890 File Offset: 0x00076A90
	public void ShowAllPackagesDescriptionWindow()
	{
		if (this.AllPackagesDescriptionWindow.activeInHierarchy)
		{
			return;
		}
		this.AllPackagesDescriptionWindow.SetActive(true);
	}

	// Token: 0x060017C4 RID: 6084 RVA: 0x000788AC File Offset: 0x00076AAC
	public void ShowRenewPremiumWindow()
	{
		if (this.RenewPremiumWindow.activeInHierarchy)
		{
			return;
		}
		this.RenewPremiumWindow.SetActive(true);
	}

	// Token: 0x060017C5 RID: 6085 RVA: 0x000788C8 File Offset: 0x00076AC8
	public void ShowFreePremiumWindow()
	{
		if (this.FreePremiumWindow.activeInHierarchy)
		{
			return;
		}
		this.FreePremiumWindow.SetActive(true);
	}

	// Token: 0x060017C6 RID: 6086 RVA: 0x000788E4 File Offset: 0x00076AE4
	public void ShowChangeNameWindow()
	{
		if (this.ChangeNameWindow.activeInHierarchy)
		{
			return;
		}
		this.ChangeNameWindow.SetActive(true);
	}

	// Token: 0x060017C7 RID: 6087 RVA: 0x00078900 File Offset: 0x00076B00
	public void ShowGuildCreationWindow()
	{
		if (this.GuildCreationWindow.activeInHierarchy)
		{
			return;
		}
		this.GuildCreationWindow.SetActive(true);
	}

	// Token: 0x060017C8 RID: 6088 RVA: 0x0007891C File Offset: 0x00076B1C
	public void ShowChangeGuildWindow()
	{
		if (this.ChangeGuildWindow.activeInHierarchy)
		{
			return;
		}
		this.ChangeGuildWindow.SetActive(true);
	}

	// Token: 0x060017C9 RID: 6089 RVA: 0x00078938 File Offset: 0x00076B38
	public void ShowRateGameWindow()
	{
		this.RateGameWindow.SetActive(true);
	}

	// Token: 0x060017CA RID: 6090 RVA: 0x00078946 File Offset: 0x00076B46
	public void ShowInventoryBoosterWindow()
	{
		if (this.InventoryBoosterWindow.activeInHierarchy)
		{
			return;
		}
		this.InventoryBoosterWindow.SetActive(true);
	}

	// Token: 0x060017CB RID: 6091 RVA: 0x00078962 File Offset: 0x00076B62
	public void ShowWarehouseBoosterWindow()
	{
		if (this.WarehouseBoosterWindow.activeInHierarchy)
		{
			return;
		}
		this.WarehouseBoosterWindow.SetActive(true);
	}

	// Token: 0x060017CC RID: 6092 RVA: 0x00078980 File Offset: 0x00076B80
	public void ShowItemSplitWindow(Item item, Action<int> callback)
	{
		if (!this.ItemSplitWindow.activeInHierarchy)
		{
			this.ItemSplitWindow.SetActive(true);
		}
		ItemSplitWindowManager itemSplitWindowManager;
		this.ItemSplitWindow.TryGetComponent<ItemSplitWindowManager>(out itemSplitWindowManager);
		itemSplitWindowManager.Initialize(item, callback);
	}

	// Token: 0x060017CD RID: 6093 RVA: 0x000789BC File Offset: 0x00076BBC
	public void ShowInformationWindow(string title, string message, Action callback)
	{
		if (!this.InformationWindow.activeInHierarchy)
		{
			this.InformationWindow.SetActive(true);
		}
		InformationWindowManager informationWindowManager;
		this.InformationWindow.TryGetComponent<InformationWindowManager>(out informationWindowManager);
		informationWindowManager.Initialize(title, message, callback);
	}

	// Token: 0x060017CE RID: 6094 RVA: 0x000789FC File Offset: 0x00076BFC
	public void ShowConfirmationWindow(Sprite icon, string text, Action callback)
	{
		if (!this.ConfirmationWindow.activeInHierarchy)
		{
			this.ConfirmationWindow.SetActive(true);
		}
		ConfirmationWindowManager confirmationWindowManager;
		this.ConfirmationWindow.TryGetComponent<ConfirmationWindowManager>(out confirmationWindowManager);
		confirmationWindowManager.Initialize(icon, text, callback);
	}

	// Token: 0x060017CF RID: 6095 RVA: 0x00078A3C File Offset: 0x00076C3C
	public void ShowDefaultConfirmationWindow(Action callback)
	{
		if (!this.ConfirmationWindow.activeInHierarchy)
		{
			this.ConfirmationWindow.SetActive(true);
		}
		ConfirmationWindowManager confirmationWindowManager;
		this.ConfirmationWindow.TryGetComponent<ConfirmationWindowManager>(out confirmationWindowManager);
		string text = LanguageManager.Instance.GetText("default_confirmation_message");
		confirmationWindowManager.Initialize(this.defaultConfirmationSprite, text, callback);
	}

	// Token: 0x060017D0 RID: 6096 RVA: 0x00078A90 File Offset: 0x00076C90
	public void ShowSkillEnchantWindow(Item enchant)
	{
		SkillEnchantWindowManager skillEnchantWindowManager;
		this.SkillEnchantWindow.TryGetComponent<SkillEnchantWindowManager>(out skillEnchantWindowManager);
		skillEnchantWindowManager.SetEnchant(enchant);
		if (!this.SkillEnchantWindow.activeInHierarchy)
		{
			this.SkillEnchantWindow.SetActive(true);
		}
	}

	// Token: 0x060017D1 RID: 6097 RVA: 0x00078ACC File Offset: 0x00076CCC
	public void ShowItemUpgradeWindow(Item scroll)
	{
		ItemUpgradeWindowManager itemUpgradeWindowManager;
		this.ItemUpgradeWindow.TryGetComponent<ItemUpgradeWindowManager>(out itemUpgradeWindowManager);
		itemUpgradeWindowManager.SetScroll(scroll);
		if (!this.ItemUpgradeWindow.activeInHierarchy)
		{
			this.ItemUpgradeWindow.SetActive(true);
		}
	}

	// Token: 0x060017D2 RID: 6098 RVA: 0x00078B08 File Offset: 0x00076D08
	public void ShowItemDestructionWindow(Item toolkit)
	{
		ItemDestructionWindowManager itemDestructionWindowManager;
		this.ItemDestructionWindow.TryGetComponent<ItemDestructionWindowManager>(out itemDestructionWindowManager);
		itemDestructionWindowManager.SetToolkit(toolkit);
		if (!this.ItemDestructionWindow.activeInHierarchy)
		{
			this.ItemDestructionWindow.SetActive(true);
		}
	}

	// Token: 0x060017D3 RID: 6099 RVA: 0x00078B44 File Offset: 0x00076D44
	public void ShowItemBoostWindow(Item booster)
	{
		ItemBoostWindowManager itemBoostWindowManager;
		this.ItemBoostWindow.TryGetComponent<ItemBoostWindowManager>(out itemBoostWindowManager);
		itemBoostWindowManager.SetBooster(booster);
		if (!this.ItemBoostWindow.activeInHierarchy)
		{
			this.ItemBoostWindow.SetActive(true);
		}
	}

	// Token: 0x060017D4 RID: 6100 RVA: 0x00078B7F File Offset: 0x00076D7F
	public void ShowBlackOverlay()
	{
		if (!this.blackOverlay.activeInHierarchy)
		{
			this.blackOverlay.SetActive(true);
		}
	}

	// Token: 0x060017D5 RID: 6101 RVA: 0x00078B9A File Offset: 0x00076D9A
	public void HideBlackOverlay()
	{
		if (this.blackOverlay.activeInHierarchy)
		{
			this.blackOverlay.SetActive(false);
		}
	}

	// Token: 0x060017D6 RID: 6102 RVA: 0x00078BB8 File Offset: 0x00076DB8
	public void UpdateUISize()
	{
		float uisizeModifier = GlobalUtils.GetUISizeModifier(SettingsManager.Instance.UiSize);
		if (!GlobalSettings.IsMobilePlatform)
		{
			this.uiHudScaler.referenceResolution = new Vector2(3840f * uisizeModifier, 2160f * uisizeModifier);
		}
		this.uiSystemScaler.referenceResolution = new Vector2(3840f * uisizeModifier, 2160f * uisizeModifier);
		this.uiTweaksScaler.referenceResolution = new Vector2(3840f * uisizeModifier, 2160f * uisizeModifier);
	}

	// Token: 0x060017D7 RID: 6103 RVA: 0x00078C38 File Offset: 0x00076E38
	public void UpdateMinimapDetails()
	{
		if (SettingsManager.Instance.MinimapDetails == MinimapDetails.High)
		{
			this.minimapCamera.cullingMask = (1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Minimap Point") | 1 << LayerMask.NameToLayer("Tile") | 1 << LayerMask.NameToLayer("Quest Icon"));
			return;
		}
		this.minimapCamera.cullingMask = (1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Minimap Point") | 1 << LayerMask.NameToLayer("Quest Icon"));
	}

	// Token: 0x060017D8 RID: 6104 RVA: 0x00078CD8 File Offset: 0x00076ED8
	public bool CloseFirstWindowOnStack()
	{
		foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Window"))
		{
			if (gameObject.activeInHierarchy)
			{
				gameObject.SetActive(false);
				return true;
			}
		}
		return false;
	}

	// Token: 0x060017D9 RID: 6105 RVA: 0x00078D14 File Offset: 0x00076F14
	public void InitializeModules(GameObject modulesOwner)
	{
		modulesOwner.TryGetComponent<PartyModule>(out this.PartyModule);
		modulesOwner.TryGetComponent<TradeModule>(out this.TradeModule);
		modulesOwner.TryGetComponent<SkillModule>(out this.SkillModule);
		modulesOwner.TryGetComponent<InventoryModule>(out this.InventoryModule);
		modulesOwner.TryGetComponent<EquipmentModule>(out this.EquipmentModule);
		modulesOwner.TryGetComponent<AttributeModule>(out this.AttributeModule);
		modulesOwner.TryGetComponent<CreatureModule>(out this.CreatureModule);
		modulesOwner.TryGetComponent<CombatModule>(out this.CombatModule);
		modulesOwner.TryGetComponent<WalletModule>(out this.WalletModule);
		modulesOwner.TryGetComponent<PlayerModule>(out this.PlayerModule);
		modulesOwner.TryGetComponent<ChatModule>(out this.ChatModule);
		modulesOwner.TryGetComponent<StoreModule>(out this.StoreModule);
		modulesOwner.TryGetComponent<ItemModule>(out this.ItemModule);
		modulesOwner.TryGetComponent<VocationModule>(out this.VocationModule);
		modulesOwner.TryGetComponent<QuestModule>(out this.QuestModule);
		modulesOwner.TryGetComponent<EffectModule>(out this.EffectModule);
		modulesOwner.TryGetComponent<PvpModule>(out this.PvpModule);
		modulesOwner.TryGetComponent<WarehouseModule>(out this.WarehouseModule);
		modulesOwner.TryGetComponent<AreaModule>(out this.AreaModule);
		modulesOwner.TryGetComponent<DailyRewardModule>(out this.DailyRewardModule);
		modulesOwner.TryGetComponent<ConditionModule>(out this.ConditionModule);
		modulesOwner.TryGetComponent<CraftModule>(out this.CraftModule);
		modulesOwner.TryGetComponent<TitleModule>(out this.TitleModule);
		modulesOwner.TryGetComponent<MarketModule>(out this.MarketModule);
		modulesOwner.TryGetComponent<MovementModule>(out this.MovementModule);
		modulesOwner.TryGetComponent<GuildModule>(out this.GuildModule);
	}

	// Token: 0x060017DA RID: 6106 RVA: 0x00078E73 File Offset: 0x00077073
	public void ShowFightingIcon()
	{
		if (GlobalSettings.IsMobilePlatform)
		{
			if (!this.MobileFightingIcon.activeInHierarchy)
			{
				this.MobileFightingIcon.SetActive(true);
			}
			return;
		}
		if (!this.FightingIcon.activeInHierarchy)
		{
			this.FightingIcon.SetActive(true);
		}
	}

	// Token: 0x060017DB RID: 6107 RVA: 0x00078EAF File Offset: 0x000770AF
	public void HideFightingIcon()
	{
		if (GlobalSettings.IsMobilePlatform)
		{
			if (this.MobileFightingIcon.activeInHierarchy)
			{
				this.MobileFightingIcon.SetActive(false);
			}
			return;
		}
		if (this.FightingIcon.activeInHierarchy)
		{
			this.FightingIcon.SetActive(false);
		}
	}

	// Token: 0x060017DC RID: 6108 RVA: 0x00078EEC File Offset: 0x000770EC
	public void ShowCombatIcon()
	{
		this.ShowFightingIcon();
		if (GlobalSettings.IsMobilePlatform)
		{
			if (!this.MobileCombatIcon.activeInHierarchy)
			{
				this.MobileCombatIcon.SetActive(true);
			}
			return;
		}
		if (!this.CombatIcon.activeInHierarchy)
		{
			this.CombatIcon.SetActive(true);
		}
	}

	// Token: 0x060017DD RID: 6109 RVA: 0x00078F3C File Offset: 0x0007713C
	public void HideCombatIcon()
	{
		this.HideFightingIcon();
		if (GlobalSettings.IsMobilePlatform)
		{
			if (this.MobileCombatIcon.activeInHierarchy)
			{
				this.MobileCombatIcon.SetActive(false);
			}
			return;
		}
		if (this.CombatIcon.activeInHierarchy)
		{
			this.CombatIcon.SetActive(false);
		}
	}

	// Token: 0x060017DE RID: 6110 RVA: 0x00078F89 File Offset: 0x00077189
	public void HideOptionsWindow()
	{
		if (this.OptionsWindow.activeInHierarchy)
		{
			this.OptionsWindow.SetActive(false);
		}
	}

	// Token: 0x060017DF RID: 6111 RVA: 0x00078FA4 File Offset: 0x000771A4
	public void ShowOptionsWindow()
	{
		if (!this.OptionsWindow.activeInHierarchy)
		{
			this.OptionsWindow.SetActive(true);
		}
	}

	// Token: 0x060017E0 RID: 6112 RVA: 0x00078FBF File Offset: 0x000771BF
	public void ShowCollectorConfigWindow()
	{
		if (!this.CollectorConfigWindow.activeInHierarchy)
		{
			this.CollectorConfigWindow.SetActive(true);
		}
	}

	// Token: 0x060017E1 RID: 6113 RVA: 0x00078FDA File Offset: 0x000771DA
	public void ShowFriendListWindow()
	{
		if (!this.FriendListWindow.activeInHierarchy)
		{
			this.FriendListWindow.SetActive(true);
		}
	}

	// Token: 0x060017E2 RID: 6114 RVA: 0x00078FF5 File Offset: 0x000771F5
	public void HideFriendListWindow()
	{
		if (this.FriendListWindow.activeInHierarchy)
		{
			this.FriendListWindow.SetActive(false);
		}
	}

	// Token: 0x060017E3 RID: 6115 RVA: 0x00079010 File Offset: 0x00077210
	public void ToggleFriendListWindow()
	{
		if (!this.FriendListWindow.activeInHierarchy)
		{
			this.FriendListWindow.SetActive(true);
			return;
		}
		this.FriendListWindow.SetActive(false);
	}

	// Token: 0x060017E4 RID: 6116 RVA: 0x00079038 File Offset: 0x00077238
	public void ShowQuestWindow()
	{
		if (!this.QuestWindow.activeInHierarchy)
		{
			this.QuestWindow.SetActive(true);
		}
	}

	// Token: 0x060017E5 RID: 6117 RVA: 0x00079053 File Offset: 0x00077253
	public void HideQuestWindow()
	{
		if (this.QuestWindow.activeInHierarchy)
		{
			this.QuestWindow.SetActive(false);
		}
	}

	// Token: 0x060017E6 RID: 6118 RVA: 0x0007906E File Offset: 0x0007726E
	public void ShowHelpWindow()
	{
		if (!this.HelpWindow.activeInHierarchy)
		{
			this.HelpWindow.SetActive(true);
		}
	}

	// Token: 0x060017E7 RID: 6119 RVA: 0x00079089 File Offset: 0x00077289
	public void ToggleHelpWindow()
	{
		if (this.HelpWindow.activeInHierarchy)
		{
			this.HelpWindow.SetActive(false);
			return;
		}
		this.HelpWindow.SetActive(true);
	}

	// Token: 0x060017E8 RID: 6120 RVA: 0x000790B1 File Offset: 0x000772B1
	public void ToggleQuestWindow()
	{
		if (this.QuestWindow.activeInHierarchy)
		{
			this.QuestWindow.SetActive(false);
			return;
		}
		this.QuestWindow.SetActive(true);
	}

	// Token: 0x060017E9 RID: 6121 RVA: 0x000790D9 File Offset: 0x000772D9
	public void ShowDialogWindow(NpcDialog npcDialog)
	{
		if (!this.DialogWindow.activeInHierarchy)
		{
			this.DialogWindow.SetActive(true);
		}
		this.DialogWindowManager.SetDialog(npcDialog);
	}

	// Token: 0x060017EA RID: 6122 RVA: 0x00079100 File Offset: 0x00077300
	public void ToggleGuildWindow()
	{
		if (!this.GuildWindow.activeInHierarchy)
		{
			this.GuildWindow.SetActive(true);
			return;
		}
		this.GuildWindow.SetActive(false);
	}

	// Token: 0x060017EB RID: 6123 RVA: 0x00079128 File Offset: 0x00077328
	public void HideGuildWindow()
	{
		if (this.GuildWindow.activeInHierarchy)
		{
			this.GuildWindow.SetActive(false);
		}
	}

	// Token: 0x060017EC RID: 6124 RVA: 0x00079143 File Offset: 0x00077343
	public void ToggleTradingPostWindow()
	{
		if (!this.TradingPostWindow.activeInHierarchy)
		{
			this.TradingPostWindow.SetActive(true);
			return;
		}
		this.TradingPostWindow.SetActive(false);
	}

	// Token: 0x060017ED RID: 6125 RVA: 0x0007916C File Offset: 0x0007736C
	public void ShowTradingPostSkinsWindow()
	{
		TradingPostManager tradingPostManager;
		this.TradingPostWindow.TryGetComponent<TradingPostManager>(out tradingPostManager);
		tradingPostManager.ChangeActiveTab("skin");
		if (!this.TradingPostWindow.activeInHierarchy)
		{
			this.TradingPostWindow.SetActive(true);
		}
	}

	// Token: 0x060017EE RID: 6126 RVA: 0x000791AC File Offset: 0x000773AC
	public void ShowTradingPostGemWindow()
	{
		TradingPostManager tradingPostManager;
		this.TradingPostWindow.TryGetComponent<TradingPostManager>(out tradingPostManager);
		tradingPostManager.ChangeActiveTab("gem");
		if (!this.TradingPostWindow.activeInHierarchy)
		{
			this.TradingPostWindow.SetActive(true);
		}
	}

	// Token: 0x060017EF RID: 6127 RVA: 0x000791EC File Offset: 0x000773EC
	public void ShowTradingPostMarketWindow()
	{
		TradingPostManager tradingPostManager;
		this.TradingPostWindow.TryGetComponent<TradingPostManager>(out tradingPostManager);
		tradingPostManager.ChangeActiveTab("market");
		if (!this.TradingPostWindow.activeInHierarchy)
		{
			this.TradingPostWindow.SetActive(true);
		}
	}

	// Token: 0x060017F0 RID: 6128 RVA: 0x0007922C File Offset: 0x0007742C
	public void ShowTradingPostStorageWindow()
	{
		TradingPostManager tradingPostManager;
		this.TradingPostWindow.TryGetComponent<TradingPostManager>(out tradingPostManager);
		tradingPostManager.ChangeActiveTab("storage");
		if (!this.TradingPostWindow.activeInHierarchy)
		{
			this.TradingPostWindow.SetActive(true);
		}
	}

	// Token: 0x060017F1 RID: 6129 RVA: 0x0007926C File Offset: 0x0007746C
	public void ShowTradingPostBuyGemWindow()
	{
		TradingPostManager tradingPostManager;
		this.TradingPostWindow.TryGetComponent<TradingPostManager>(out tradingPostManager);
		tradingPostManager.ChangeActiveTab("buygem");
		if (!this.TradingPostWindow.activeInHierarchy)
		{
			this.TradingPostWindow.SetActive(true);
		}
	}

	// Token: 0x060017F2 RID: 6130 RVA: 0x000792AB File Offset: 0x000774AB
	public void HideDialogWindow()
	{
		if (this.DialogWindow.activeInHierarchy)
		{
			this.DialogWindow.SetActive(false);
		}
	}

	// Token: 0x060017F3 RID: 6131 RVA: 0x000792C8 File Offset: 0x000774C8
	public void ShowChatHolder()
	{
		if (GlobalSettings.IsMobilePlatform)
		{
			if (!this.MobileChatHolder.activeInHierarchy)
			{
				this.MobileChatHolder.SetActive(true);
				this.MobileChatHolderManager.ActivateFirstTab();
				this.MobileChatHolderManager.ClearChatInput();
				this.MobileChatHolderManager.SetSendMessageInputFocus(true, null);
			}
			return;
		}
		if (!this.ChatHolder.activeInHierarchy)
		{
			this.showChatButton.SetActive(false);
			this.ChatHolder.SetActive(true);
			this.ChatHolderManager.ActivateFirstTab();
		}
	}

	// Token: 0x060017F4 RID: 6132 RVA: 0x0007934C File Offset: 0x0007754C
	public void HideChatHolder()
	{
		if (GlobalSettings.IsMobilePlatform)
		{
			if (this.MobileChatHolder.activeInHierarchy)
			{
				this.MobileChatHolder.SetActive(false);
				this.MobileChatHolderManager.ActivateFirstTab();
				this.MobileChatHolderManager.ClearChatInput();
				this.MobileChatHolderManager.SetSendMessageInputFocus(false, null);
			}
			return;
		}
		if (this.ChatHolder.activeInHierarchy)
		{
			this.showChatButton.SetActive(true);
			this.ChatHolder.SetActive(false);
			this.ChatHolderManager.ActivateFirstTab();
		}
	}

	// Token: 0x060017F5 RID: 6133 RVA: 0x000793CD File Offset: 0x000775CD
	public void OpenMobileChatHolder()
	{
		this.MobileChatHolder.SetActive(true);
		this.MobileChatHolderManager.ActivateFirstTab();
	}

	// Token: 0x060017F6 RID: 6134 RVA: 0x000793E6 File Offset: 0x000775E6
	public void ToggleMobileChatHolder()
	{
		if (this.MobileChatHolder.activeInHierarchy)
		{
			this.MobileChatHolder.SetActive(false);
			this.MobileChatHolderManager.ActivateFirstTab();
			return;
		}
		this.MobileChatHolder.SetActive(true);
		this.MobileChatHolderManager.ActivateFirstTab();
	}

	// Token: 0x060017F7 RID: 6135 RVA: 0x00079424 File Offset: 0x00077624
	public void ShowDailyRewardWindow()
	{
		if (!this.DailyRewardWindow.activeInHierarchy)
		{
			this.DailyRewardWindow.SetActive(true);
		}
	}

	// Token: 0x060017F8 RID: 6136 RVA: 0x0007943F File Offset: 0x0007763F
	public void ToggleDailyRewardWindow()
	{
		if (this.DailyRewardWindow.activeInHierarchy)
		{
			this.DailyRewardWindow.SetActive(false);
			return;
		}
		this.DailyRewardWindow.SetActive(true);
	}

	// Token: 0x060017F9 RID: 6137 RVA: 0x00079467 File Offset: 0x00077667
	public void ShowAttributeWindow()
	{
		if (!this.AttributeWindow.activeInHierarchy)
		{
			this.AttributeWindow.SetActive(true);
		}
	}

	// Token: 0x060017FA RID: 6138 RVA: 0x00079482 File Offset: 0x00077682
	public void ToggleAttributeWindow()
	{
		if (this.AttributeWindow.activeInHierarchy)
		{
			this.AttributeWindow.SetActive(false);
			return;
		}
		this.AttributeWindow.SetActive(true);
	}

	// Token: 0x060017FB RID: 6139 RVA: 0x000794AA File Offset: 0x000776AA
	public void ShowMinimap()
	{
		if (GlobalSettings.IsMobilePlatform)
		{
			if (!this.MobileMinimapHolder.activeInHierarchy)
			{
				this.MobileMinimapHolder.SetActive(true);
			}
			return;
		}
		if (!this.MinimapHolder.activeInHierarchy)
		{
			this.MinimapHolder.SetActive(true);
		}
	}

	// Token: 0x060017FC RID: 6140 RVA: 0x000794E6 File Offset: 0x000776E6
	public void HideMinimap()
	{
		if (GlobalSettings.IsMobilePlatform)
		{
			if (this.MobileMinimapHolder.activeInHierarchy)
			{
				this.MobileMinimapHolder.SetActive(false);
			}
			return;
		}
		if (this.MinimapHolder.activeInHierarchy)
		{
			this.MinimapHolder.SetActive(false);
		}
	}

	// Token: 0x060017FD RID: 6141 RVA: 0x00079524 File Offset: 0x00077724
	public void ShowTargetHud(GameObject target)
	{
		if (GlobalSettings.IsMobilePlatform)
		{
			if (!this.mobileTargetHudHolder.activeInHierarchy)
			{
				this.mobileTargetHudHolder.SetActive(true);
				TargetHudModule targetHudModule;
				this.mobileTargetHudHolder.TryGetComponent<TargetHudModule>(out targetHudModule);
				targetHudModule.SetTarget(target);
				this.ShowTargetConditionBar(target);
			}
			return;
		}
		if (!this.targetHudHolder.activeInHierarchy)
		{
			this.targetHudHolder.SetActive(true);
			this.targetHudHolder.GetComponent<TargetHudModule>().SetTarget(target);
			this.ShowTargetConditionBar(target);
		}
	}

	// Token: 0x060017FE RID: 6142 RVA: 0x000795A0 File Offset: 0x000777A0
	public void HideTargetHud()
	{
		if (GlobalSettings.IsMobilePlatform)
		{
			if (this.mobileTargetHudHolder.activeInHierarchy)
			{
				this.mobileTargetHudHolder.SetActive(false);
				TargetHudModule targetHudModule;
				this.mobileTargetHudHolder.TryGetComponent<TargetHudModule>(out targetHudModule);
				this.HideTargetConditionBar();
			}
			return;
		}
		if (this.targetHudHolder.activeInHierarchy)
		{
			this.targetHudHolder.SetActive(false);
			this.HideTargetConditionBar();
		}
	}

	// Token: 0x060017FF RID: 6143 RVA: 0x00079601 File Offset: 0x00077801
	public void ShowHud()
	{
		if (GlobalSettings.IsMobilePlatform)
		{
			if (!this.MobileHudHolder.activeInHierarchy)
			{
				this.MobileHudHolder.SetActive(true);
			}
			return;
		}
		if (!this.HudHolder.activeInHierarchy)
		{
			this.HudHolder.SetActive(true);
		}
	}

	// Token: 0x06001800 RID: 6144 RVA: 0x0007963D File Offset: 0x0007783D
	public void HideHud()
	{
		if (GlobalSettings.IsMobilePlatform)
		{
			if (this.MobileHudHolder.activeInHierarchy)
			{
				this.MobileHudHolder.SetActive(false);
			}
			return;
		}
		if (this.HudHolder.activeInHierarchy)
		{
			this.HudHolder.SetActive(false);
		}
	}

	// Token: 0x06001801 RID: 6145 RVA: 0x00079679 File Offset: 0x00077879
	public void ShowTimedFeedback(string text, float duration, bool isError = false)
	{
		base.StartCoroutine(this.ShowTimedFeedbackAsync(text, duration, isError));
	}

	// Token: 0x06001802 RID: 6146 RVA: 0x0007968B File Offset: 0x0007788B
	private IEnumerator ShowTimedFeedbackAsync(string text, float duration, bool isError)
	{
		if (string.IsNullOrEmpty(text))
		{
			yield break;
		}
		if (isError)
		{
			this.ShowFeedbackError(text);
		}
		else
		{
			this.ShowFeedbackText(text);
		}
		yield return new WaitForSeconds(duration);
		this.HideFeedbackPanel();
		yield break;
	}

	// Token: 0x06001803 RID: 6147 RVA: 0x000796B0 File Offset: 0x000778B0
	public void ShowFeedbackText(string text)
	{
		if (!this.FeedbackPanel.activeInHierarchy)
		{
			this.FeedbackPanel.SetActive(true);
		}
		this.FeedbackText.color = Color.white;
		this.FeedbackText.text = text;
		this.FeedbackPanel.transform.SetAsLastSibling();
	}

	// Token: 0x06001804 RID: 6148 RVA: 0x00079702 File Offset: 0x00077902
	public void ShowStoryline()
	{
		if (!this.StorylineWindow.activeInHierarchy)
		{
			this.StorylineWindow.SetActive(true);
		}
	}

	// Token: 0x06001805 RID: 6149 RVA: 0x00079720 File Offset: 0x00077920
	public void ShowFeedbackError(string text)
	{
		if (!this.FeedbackPanel.activeInHierarchy)
		{
			this.FeedbackPanel.SetActive(true);
		}
		this.FeedbackText.color = Color.red;
		this.FeedbackText.text = text;
		this.FeedbackPanel.transform.SetAsLastSibling();
	}

	// Token: 0x06001806 RID: 6150 RVA: 0x00079772 File Offset: 0x00077972
	public void HideFeedbackPanel()
	{
		if (this.FeedbackPanel.activeInHierarchy)
		{
			this.FeedbackPanel.SetActive(false);
		}
	}

	// Token: 0x06001807 RID: 6151 RVA: 0x0007978D File Offset: 0x0007798D
	public void ActivateMenuBackgroundImage()
	{
		if (!this.MenuBackgroundImage.activeInHierarchy)
		{
			this.MenuBackgroundImage.SetActive(true);
		}
		this.MenuBackgroundImage.transform.SetAsLastSibling();
	}

	// Token: 0x06001808 RID: 6152 RVA: 0x000797B8 File Offset: 0x000779B8
	public void DeActivateMenuBackgroundImage()
	{
		if (this.MenuBackgroundImage.activeInHierarchy)
		{
			this.MenuBackgroundImage.SetActive(false);
		}
	}

	// Token: 0x06001809 RID: 6153 RVA: 0x000797D3 File Offset: 0x000779D3
	public void HideHint()
	{
		if (this.HintWindow.activeInHierarchy)
		{
			this.HintWindow.SetActive(false);
		}
	}

	// Token: 0x0600180A RID: 6154 RVA: 0x000797F0 File Offset: 0x000779F0
	public void ShowHint(string text)
	{
		if (this.HasWindowsOpened)
		{
			return;
		}
		if (!this.HintWindow.activeInHierarchy)
		{
			text = LanguageManager.Instance.GetText(text);
			this.HintWindow.SetActive(true);
			this.HintWindow.GetComponentInChildren<Text>().text = text.ToUpper();
		}
	}

	// Token: 0x0600180B RID: 6155 RVA: 0x00079842 File Offset: 0x00077A42
	public void CloseDefaultTooltip()
	{
		if (this.DefaultTooltipWindow != null)
		{
			this.DefaultTooltipWindow.SetActive(false);
		}
	}

	// Token: 0x0600180C RID: 6156 RVA: 0x00079860 File Offset: 0x00077A60
	public void ShowDefaultTooltip(Vector3 position, Image icon, string title, string description)
	{
		Rect rect = (this.DefaultTooltipWindow.transform as RectTransform).rect;
		this.DefaultTooltipWindow.SetActive(true);
		float num = 5f;
		float num2 = -5f;
		float x;
		if ((float)Screen.width - position.x >= rect.width * this.UISystemCanvas.scaleFactor)
		{
			x = 0f;
		}
		else
		{
			x = 1f;
			num *= -1f;
		}
		float y;
		if ((float)Screen.height - position.y >= rect.height * this.UISystemCanvas.scaleFactor)
		{
			y = 0f;
			num2 *= -1f;
		}
		else
		{
			y = 1f;
		}
		(this.DefaultTooltipWindow.transform as RectTransform).pivot = new Vector2(x, y);
		this.DefaultTooltipWindow.transform.position = new Vector3(position.x + num, position.y + num2, 10f);
		this.DefaultTooltipWindow.transform.Find("NamePanel/IconBackground/IconMask/IconImage").GetComponent<Image>().sprite = icon.sprite;
		this.DefaultTooltipWindow.transform.Find("NamePanel/NameBackground/NameText").GetComponent<Text>().text = title.ToLower();
		this.DefaultTooltipWindow.transform.Find("DescriptionText").GetComponent<Text>().text = description;
	}

	// Token: 0x0600180D RID: 6157 RVA: 0x000799CC File Offset: 0x00077BCC
	public void CloseConditionTooltip()
	{
		if (this.SkillTooltipWindow != null)
		{
			this.SkillTooltipWindow.SetActive(false);
		}
	}

	// Token: 0x0600180E RID: 6158 RVA: 0x000799E8 File Offset: 0x00077BE8
	public void ShowConditionTooltip(Vector3 position, Condition condition)
	{
		Rect rect = (this.SkillTooltipWindow.transform as RectTransform).rect;
		this.SkillTooltipWindow.SetActive(true);
		float num = 5f;
		float num2 = -5f;
		float x;
		if ((float)Screen.width - position.x >= rect.width * this.UISystemCanvas.scaleFactor)
		{
			x = 0f;
		}
		else
		{
			x = 1f;
			num *= -1f;
		}
		float y;
		if ((float)Screen.height - position.y >= rect.height * this.UISystemCanvas.scaleFactor)
		{
			y = 0f;
			num2 *= -1f;
		}
		else
		{
			y = 1f;
		}
		(this.SkillTooltipWindow.transform as RectTransform).pivot = new Vector2(x, y);
		this.SkillTooltipWindow.transform.position = new Vector3(position.x + num, position.y + num2, 10f);
		this.SkillTooltipWindow.transform.Find("NamePanel/IconBackground/IconMask/IconImage").GetComponent<Image>().sprite = condition.Icon;
		this.SkillTooltipWindow.transform.Find("NamePanel/NameBackground/NameText").GetComponent<Text>().text = string.Concat(new string[]
		{
			"<color=",
			GlobalUtils.ConditionCategoryToColorString(condition.Category),
			">",
			condition.TranslatedName.ToLower(),
			"</color>".ToLower()
		});
		this.SkillTooltipWindow.transform.Find("DescriptionText").GetComponent<Text>().text = condition.FullDescription;
		double seconds = condition.CooldownTimer(NetworkTime.time);
		this.SkillTooltipWindow.transform.Find("CooldownPanel/CooldownText").GetComponent<Text>().text = GlobalUtils.SecondsToCooldownText(seconds);
	}

	// Token: 0x0600180F RID: 6159 RVA: 0x00079BCD File Offset: 0x00077DCD
	public void CloseSkillTooltip()
	{
		if (this.SkillTooltipWindow != null && this.SkillTooltipWindow.activeInHierarchy)
		{
			this.SkillTooltipWindow.SetActive(false);
		}
	}

	// Token: 0x06001810 RID: 6160 RVA: 0x00079BF8 File Offset: 0x00077DF8
	public void ShowSkillTooltip(Vector3 position, Skill skill)
	{
		Rect rect = (this.SkillTooltipWindow.transform as RectTransform).rect;
		this.SkillTooltipWindow.SetActive(true);
		float num = 5f;
		float num2 = -5f;
		float x;
		if ((float)Screen.width - position.x >= rect.width * this.UISystemCanvas.scaleFactor)
		{
			x = 0f;
		}
		else
		{
			x = 1f;
			num *= -1f;
		}
		float y;
		if ((float)Screen.height - position.y >= rect.height * this.UISystemCanvas.scaleFactor)
		{
			y = 0f;
			num2 *= -1f;
		}
		else
		{
			y = 1f;
		}
		(this.SkillTooltipWindow.transform as RectTransform).pivot = new Vector2(x, y);
		this.SkillTooltipWindow.transform.position = new Vector3(position.x + num, position.y + num2, 10f);
		this.SkillTooltipWindow.transform.Find("NamePanel/IconBackground/IconMask/IconImage").GetComponent<Image>().sprite = skill.Icon;
		this.SkillTooltipWindow.transform.Find("NamePanel/NameBackground/NameText").GetComponent<Text>().text = skill.DisplayName.ToLower();
		this.SkillTooltipWindow.transform.Find("DescriptionText").GetComponent<Text>().text = skill.FullDescription;
		this.SkillTooltipWindow.transform.Find("CooldownPanel/CooldownText").GetComponent<Text>().text = skill.Cooldown.ToString();
	}

	// Token: 0x06001811 RID: 6161 RVA: 0x00079D9F File Offset: 0x00077F9F
	public void CloseItemTooltip()
	{
		if (this.ItemTooltipWindow != null && this.ItemTooltipWindow.activeInHierarchy)
		{
			this.ItemTooltipWindow.SetActive(false);
		}
	}

	// Token: 0x06001812 RID: 6162 RVA: 0x00079DC8 File Offset: 0x00077FC8
	public void ShowItemTooltip(Vector3 position, Item item)
	{
		RectTransform rectTransform = this.ItemTooltipWindow.transform as RectTransform;
		Rect rect = rectTransform.rect;
		this.ItemTooltipWindow.SetActive(true);
		float num = 5f;
		float num2 = -5f;
		float x;
		if ((float)Screen.width - position.x >= rect.width * this.UISystemCanvas.scaleFactor * rectTransform.localScale.x)
		{
			x = 0f;
		}
		else
		{
			x = 1f;
			num *= -1f;
		}
		float y;
		if ((float)Screen.height - position.y >= rect.height * this.UISystemCanvas.scaleFactor * rectTransform.localScale.y)
		{
			y = 0f;
			num2 *= -1f;
		}
		else
		{
			y = 1f;
		}
		(this.ItemTooltipWindow.transform as RectTransform).pivot = new Vector2(x, y);
		this.ItemTooltipWindow.transform.position = new Vector3(position.x + num, position.y + num2, 10f);
		this.ItemTooltipWindow.transform.Find("NamePanel/IconBackground/IconImage").GetComponent<Image>().sprite = item.Icon;
		this.ItemTooltipWindow.transform.Find("NamePanel/NameBackground/NameText").GetComponent<Text>().text = item.DisplayName.ToLower();
		this.ItemTooltipWindow.transform.Find("DescriptionText").GetComponent<Text>().text = item.FullDescription;
		this.ItemTooltipWindow.transform.Find("ValuePanel/ValueText").GetComponent<Text>().text = item.DisplayValue;
	}

	// Token: 0x06001813 RID: 6163 RVA: 0x00079F78 File Offset: 0x00078178
	public void ChangeSkillBars()
	{
		if (this.SecondSkillBarWindow.activeInHierarchy)
		{
			this.SecondSkillBarWindow.SetActive(false);
		}
		else
		{
			this.SecondSkillBarWindow.SetActive(true);
		}
		if (!this.SkillBarWindow.activeInHierarchy)
		{
			this.SkillBarWindow.SetActive(true);
			return;
		}
		this.SkillBarWindow.SetActive(false);
	}

	// Token: 0x06001814 RID: 6164 RVA: 0x00079FD2 File Offset: 0x000781D2
	public void ShowSkillBarWindow()
	{
		if (this.SecondSkillBarWindow.activeInHierarchy)
		{
			this.SecondSkillBarWindow.SetActive(false);
		}
		if (!this.SkillBarWindow.activeInHierarchy)
		{
			this.SkillBarWindow.SetActive(true);
		}
	}

	// Token: 0x06001815 RID: 6165 RVA: 0x0007A006 File Offset: 0x00078206
	public void ShowSecondSkillBarWindow()
	{
		if (!this.SecondSkillBarWindow.activeInHierarchy)
		{
			this.SecondSkillBarWindow.SetActive(true);
		}
		if (this.SkillBarWindow.activeInHierarchy)
		{
			this.SkillBarWindow.SetActive(false);
		}
	}

	// Token: 0x06001816 RID: 6166 RVA: 0x0007A03A File Offset: 0x0007823A
	public void ShowItemBarWindow()
	{
		if (!this.ItemBarWindow.activeInHierarchy)
		{
			this.ItemBarWindow.SetActive(true);
		}
	}

	// Token: 0x06001817 RID: 6167 RVA: 0x0007A055 File Offset: 0x00078255
	public void HideConditionBarWindow()
	{
		if (GlobalSettings.IsMobilePlatform)
		{
			if (this.MobileConditionBar.activeInHierarchy)
			{
				this.MobileConditionBar.SetActive(false);
			}
			return;
		}
		if (this.ConditionBar.activeInHierarchy)
		{
			this.ConditionBar.SetActive(false);
		}
	}

	// Token: 0x06001818 RID: 6168 RVA: 0x0007A091 File Offset: 0x00078291
	public void ShowConditionBarWindow()
	{
		if (GlobalSettings.IsMobilePlatform)
		{
			if (!this.MobileConditionBar.activeInHierarchy)
			{
				this.MobileConditionBar.SetActive(true);
			}
			return;
		}
		if (!this.ConditionBar.activeInHierarchy)
		{
			this.ConditionBar.SetActive(true);
		}
	}

	// Token: 0x06001819 RID: 6169 RVA: 0x0007A0CD File Offset: 0x000782CD
	public void ShowTradeWindow()
	{
		if (this.TradeWindow.activeInHierarchy)
		{
			return;
		}
		this.TradeWindow.SetActive(true);
	}

	// Token: 0x0600181A RID: 6170 RVA: 0x0007A0E9 File Offset: 0x000782E9
	public void HideTradeWindow()
	{
		if (!this.TradeWindow.activeInHierarchy)
		{
			return;
		}
		this.TradeWindow.SetActive(false);
	}

	// Token: 0x0600181B RID: 6171 RVA: 0x0007A108 File Offset: 0x00078308
	public void ShowTradeHandshakeWindow(GameObject trader)
	{
		if (!this.TradeHandshakeWindow.activeInHierarchy)
		{
			this.TradeHandshakeWindow.SetActive(true);
			TradeHandshakeWindowManager tradeHandshakeWindowManager;
			this.TradeHandshakeWindow.TryGetComponent<TradeHandshakeWindowManager>(out tradeHandshakeWindowManager);
			tradeHandshakeWindowManager.SetTrader(trader);
		}
	}

	// Token: 0x0600181C RID: 6172 RVA: 0x0007A143 File Offset: 0x00078343
	public void HideTradeHandshakeWindow()
	{
		if (this.TradeHandshakeWindow.activeInHierarchy)
		{
			this.TradeHandshakeWindow.SetActive(false);
		}
	}

	// Token: 0x0600181D RID: 6173 RVA: 0x0007A160 File Offset: 0x00078360
	public void ShowGuildInvitationWindow(GameObject inviter)
	{
		if (!this.GuildInvitationWindow.activeInHierarchy)
		{
			this.GuildInvitationWindow.SetActive(true);
			GuildInvitationWindowModule guildInvitationWindowModule;
			this.GuildInvitationWindow.TryGetComponent<GuildInvitationWindowModule>(out guildInvitationWindowModule);
			guildInvitationWindowModule.SetInviter(inviter);
		}
	}

	// Token: 0x0600181E RID: 6174 RVA: 0x0007A19B File Offset: 0x0007839B
	public void HideGuildInvitationWindow()
	{
		if (this.GuildInvitationWindow.activeInHierarchy)
		{
			this.GuildInvitationWindow.SetActive(false);
		}
	}

	// Token: 0x0600181F RID: 6175 RVA: 0x0007A1B6 File Offset: 0x000783B6
	public void ShowPartyInvitationWindow(PartyMember inviter)
	{
		if (!this.PartyInvitationWindow.activeInHierarchy)
		{
			this.PartyInvitationWindow.SetActive(true);
			this.PartyInvitationWindowManager.SetInviter(inviter);
		}
	}

	// Token: 0x06001820 RID: 6176 RVA: 0x0007A1DD File Offset: 0x000783DD
	public void HidePartyInvitationWindow()
	{
		if (this.PartyInvitationWindow.activeInHierarchy)
		{
			this.PartyInvitationWindow.SetActive(false);
		}
	}

	// Token: 0x06001821 RID: 6177 RVA: 0x0007A1F8 File Offset: 0x000783F8
	public void ShowTargetConditionBar(GameObject target)
	{
		if (!this.PlatformTargetConditionBar.activeInHierarchy)
		{
			this.PlatformTargetConditionBar.SetActive(true);
			for (int i = 0; i < this.PlatformTargetConditionBar.transform.childCount; i++)
			{
				Transform child = this.PlatformTargetConditionBar.transform.GetChild(i);
				if (child != null)
				{
					UnityEngine.Object.Destroy(child.gameObject);
				}
			}
			if (target != null)
			{
				ConditionModule component = target.GetComponent<ConditionModule>();
				if (component != null)
				{
					for (int j = 0; j < component.Conditions.Count; j++)
					{
						Condition condition = component.Conditions[j];
						GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.conditionBarSlotPrefab);
						gameObject.transform.SetParent(this.PlatformTargetConditionBar.transform, false);
						gameObject.transform.position = Vector2.zero;
						if (gameObject != null)
						{
							gameObject.GetComponent<ConditionBarSlotManager>().SetCondition(true, condition);
							gameObject.name = component.NetName(condition.Type.ToString());
						}
					}
				}
			}
		}
	}

	// Token: 0x06001822 RID: 6178 RVA: 0x0007A31E File Offset: 0x0007851E
	public void HideTargetConditionBar()
	{
		if (this.PlatformTargetConditionBar.activeInHierarchy)
		{
			this.PlatformTargetConditionBar.SetActive(false);
		}
	}

	// Token: 0x06001823 RID: 6179 RVA: 0x0007A339 File Offset: 0x00078539
	public void ShowInventoryWindow()
	{
		if (!this.InventoryWindow.activeInHierarchy)
		{
			this.InventoryWindow.SetActive(true);
		}
	}

	// Token: 0x06001824 RID: 6180 RVA: 0x0007A354 File Offset: 0x00078554
	public void ToggleInventoryWindow()
	{
		if (this.InventoryWindow.activeInHierarchy)
		{
			this.InventoryWindow.SetActive(false);
			return;
		}
		this.InventoryWindow.SetActive(true);
	}

	// Token: 0x06001825 RID: 6181 RVA: 0x0007A37C File Offset: 0x0007857C
	public void HideWarehouseWindow()
	{
		if (this.WarehouseWindow.activeInHierarchy)
		{
			this.WarehouseWindow.SetActive(false);
		}
	}

	// Token: 0x06001826 RID: 6182 RVA: 0x0007A397 File Offset: 0x00078597
	public void ShowWarehouseWindow()
	{
		if (!this.WarehouseWindow.activeInHierarchy)
		{
			this.WarehouseWindow.SetActive(true);
		}
	}

	// Token: 0x06001827 RID: 6183 RVA: 0x0007A3B2 File Offset: 0x000785B2
	public void ToggleWarehouseWindow()
	{
		if (this.WarehouseWindow.activeInHierarchy)
		{
			this.WarehouseWindow.SetActive(false);
			return;
		}
		this.WarehouseWindow.SetActive(true);
	}

	// Token: 0x06001828 RID: 6184 RVA: 0x0007A3DA File Offset: 0x000785DA
	public void TogglePlayerWindow()
	{
		if (this.PlayerWindow.activeInHierarchy)
		{
			this.PlayerWindow.SetActive(false);
			return;
		}
		this.PlayerWindow.SetActive(true);
	}

	// Token: 0x06001829 RID: 6185 RVA: 0x0007A402 File Offset: 0x00078602
	public void ToggleSkillBookWindow()
	{
		if (this.SkillBookWindow.activeInHierarchy)
		{
			this.SkillBookWindow.SetActive(false);
			return;
		}
		this.SkillBookWindow.SetActive(true);
	}

	// Token: 0x0600182A RID: 6186 RVA: 0x0007A42A File Offset: 0x0007862A
	public void ToggleGameMenu()
	{
		if (this.GameMenu.activeInHierarchy)
		{
			this.GameMenu.SetActive(false);
			return;
		}
		this.GameMenu.SetActive(true);
	}

	// Token: 0x0600182B RID: 6187 RVA: 0x0007A452 File Offset: 0x00078652
	public void TogglePvpEnabled()
	{
		if (!this.PvpModule.PvpEnabled)
		{
			this.PvpModule.SetPvpEnabled(true);
			return;
		}
		this.PvpModule.SetPvpEnabled(false);
	}

	// Token: 0x0600182C RID: 6188 RVA: 0x0007A47A File Offset: 0x0007867A
	public void ShowMap()
	{
		if (this.PlayerModule != null)
		{
			this.PlayerModule.ShowMap();
		}
	}

	// Token: 0x0600182D RID: 6189 RVA: 0x0007A498 File Offset: 0x00078698
	public void ShowMessageText(string content, int colorId, float textDuration)
	{
		GameObject screenMessageTextFromPool = ObjectPoolModule.Instance.GetScreenMessageTextFromPool(Vector3.zero);
		screenMessageTextFromPool.transform.SetParent(this.MessagePanel.transform, false);
		if (screenMessageTextFromPool != null)
		{
			TextMeshProUGUI textMeshProUGUI;
			screenMessageTextFromPool.TryGetComponent<TextMeshProUGUI>(out textMeshProUGUI);
			textMeshProUGUI.text = content;
			textMeshProUGUI.color = GlobalSettings.Colors[colorId];
			MessageTextManager messageTextManager;
			screenMessageTextFromPool.TryGetComponent<MessageTextManager>(out messageTextManager);
			messageTextManager.Initialize(textDuration);
		}
	}

	// Token: 0x0600182E RID: 6190 RVA: 0x0007A506 File Offset: 0x00078706
	public void ShowMessageText(string content, int colorId)
	{
		this.ShowMessageText(content, colorId, 3.5f);
	}

	// Token: 0x0600182F RID: 6191 RVA: 0x0007A518 File Offset: 0x00078718
	public void ReloadPartyMembers()
	{
		if (GlobalSettings.IsMobilePlatform)
		{
			for (int i = 0; i < this.mobilePartyMembersHolder.transform.childCount; i++)
			{
				UnityEngine.Object.Destroy(this.mobilePartyMembersHolder.transform.GetChild(i).gameObject);
			}
			foreach (PartyMember partyMember in this.PartyModule.PartyMembers)
			{
				UnityEngine.Object.Instantiate<GameObject>(this.mobilePartyMemberPrefab, this.mobilePartyMembersHolder.transform, false).GetComponent<PartyMemberManager>().SetPartyMember(partyMember);
			}
			return;
		}
		for (int j = 0; j < this.partyMembersHolder.transform.childCount; j++)
		{
			UnityEngine.Object.Destroy(this.partyMembersHolder.transform.GetChild(j).gameObject);
		}
		foreach (PartyMember partyMember2 in this.PartyModule.PartyMembers)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.partyMemberPrefab, this.partyMembersHolder.transform, false).GetComponent<PartyMemberManager>().SetPartyMember(partyMember2);
		}
	}

	// Token: 0x06001830 RID: 6192 RVA: 0x0007A668 File Offset: 0x00078868
	public void ShowRespawnWindow()
	{
		if (!this.RespawnWindow.activeInHierarchy)
		{
			this.RespawnWindow.SetActive(true);
		}
	}

	// Token: 0x06001831 RID: 6193 RVA: 0x0007A683 File Offset: 0x00078883
	public void HideRespawnWindow()
	{
		if (this.RespawnWindow.activeInHierarchy)
		{
			this.RespawnWindow.SetActive(false);
		}
	}

	// Token: 0x06001832 RID: 6194 RVA: 0x0007A69E File Offset: 0x0007889E
	public void ToggleCraftWindow()
	{
		if (this.CraftWindow.activeInHierarchy)
		{
			this.CraftWindow.SetActive(false);
			return;
		}
		this.CraftWindow.SetActive(true);
	}

	// Token: 0x06001833 RID: 6195 RVA: 0x0007A6C6 File Offset: 0x000788C6
	public void ShowCraftWindow()
	{
		if (!this.CraftWindow.activeInHierarchy)
		{
			this.CraftWindow.SetActive(true);
		}
	}

	// Token: 0x06001834 RID: 6196 RVA: 0x0007A6E1 File Offset: 0x000788E1
	public void HideCraftWindow()
	{
		if (this.CraftWindow.activeInHierarchy)
		{
			this.CraftWindow.SetActive(false);
		}
	}

	// Token: 0x040014F7 RID: 5367
	[NonSerialized]
	public SkillModule SkillModule;

	// Token: 0x040014F8 RID: 5368
	[NonSerialized]
	public InventoryModule InventoryModule;

	// Token: 0x040014F9 RID: 5369
	[NonSerialized]
	public EquipmentModule EquipmentModule;

	// Token: 0x040014FA RID: 5370
	[NonSerialized]
	public AttributeModule AttributeModule;

	// Token: 0x040014FB RID: 5371
	[NonSerialized]
	public CreatureModule CreatureModule;

	// Token: 0x040014FC RID: 5372
	[NonSerialized]
	public CombatModule CombatModule;

	// Token: 0x040014FD RID: 5373
	[NonSerialized]
	public WalletModule WalletModule;

	// Token: 0x040014FE RID: 5374
	[NonSerialized]
	public PlayerModule PlayerModule;

	// Token: 0x040014FF RID: 5375
	[NonSerialized]
	public ChatModule ChatModule;

	// Token: 0x04001500 RID: 5376
	[NonSerialized]
	public StoreModule StoreModule;

	// Token: 0x04001501 RID: 5377
	[NonSerialized]
	public ItemModule ItemModule;

	// Token: 0x04001502 RID: 5378
	[NonSerialized]
	public VocationModule VocationModule;

	// Token: 0x04001503 RID: 5379
	[NonSerialized]
	public QuestModule QuestModule;

	// Token: 0x04001504 RID: 5380
	[NonSerialized]
	public EffectModule EffectModule;

	// Token: 0x04001505 RID: 5381
	[NonSerialized]
	public PvpModule PvpModule;

	// Token: 0x04001506 RID: 5382
	[NonSerialized]
	public WarehouseModule WarehouseModule;

	// Token: 0x04001507 RID: 5383
	[NonSerialized]
	public PartyModule PartyModule;

	// Token: 0x04001508 RID: 5384
	[NonSerialized]
	public TradeModule TradeModule;

	// Token: 0x04001509 RID: 5385
	[NonSerialized]
	public AreaModule AreaModule;

	// Token: 0x0400150A RID: 5386
	[NonSerialized]
	public DailyRewardModule DailyRewardModule;

	// Token: 0x0400150B RID: 5387
	[NonSerialized]
	public ConditionModule ConditionModule;

	// Token: 0x0400150C RID: 5388
	[NonSerialized]
	public TitleModule TitleModule;

	// Token: 0x0400150D RID: 5389
	[NonSerialized]
	public CraftModule CraftModule;

	// Token: 0x0400150E RID: 5390
	[NonSerialized]
	public MarketModule MarketModule;

	// Token: 0x0400150F RID: 5391
	[NonSerialized]
	public MovementModule MovementModule;

	// Token: 0x04001510 RID: 5392
	[NonSerialized]
	public GuildModule GuildModule;

	// Token: 0x04001511 RID: 5393
	[Header("Game Windows")]
	public GameObject AttributeWindow;

	// Token: 0x04001512 RID: 5394
	public GameObject CraftWindow;

	// Token: 0x04001513 RID: 5395
	public GameObject DailyRewardWindow;

	// Token: 0x04001514 RID: 5396
	public GameObject DialogWindow;

	// Token: 0x04001515 RID: 5397
	public GameObject FriendListWindow;

	// Token: 0x04001516 RID: 5398
	public GameObject GameMenu;

	// Token: 0x04001517 RID: 5399
	public GameObject HelpWindow;

	// Token: 0x04001518 RID: 5400
	public GameObject InventoryWindow;

	// Token: 0x04001519 RID: 5401
	public GameObject OptionsWindow;

	// Token: 0x0400151A RID: 5402
	public GameObject PartyInvitationWindow;

	// Token: 0x0400151B RID: 5403
	public GameObject TradeHandshakeWindow;

	// Token: 0x0400151C RID: 5404
	public GameObject TradeWindow;

	// Token: 0x0400151D RID: 5405
	public GameObject PlayerWindow;

	// Token: 0x0400151E RID: 5406
	public GameObject QuestWindow;

	// Token: 0x0400151F RID: 5407
	public GameObject RespawnWindow;

	// Token: 0x04001520 RID: 5408
	public GameObject SkillBookWindow;

	// Token: 0x04001521 RID: 5409
	public GameObject StorylineWindow;

	// Token: 0x04001522 RID: 5410
	public GameObject TradingPostWindow;

	// Token: 0x04001523 RID: 5411
	public GameObject WarehouseWindow;

	// Token: 0x04001524 RID: 5412
	public GameObject ItemBoostWindow;

	// Token: 0x04001525 RID: 5413
	public GameObject ItemUpgradeWindow;

	// Token: 0x04001526 RID: 5414
	public GameObject ItemDestructionWindow;

	// Token: 0x04001527 RID: 5415
	public GameObject ItemSplitWindow;

	// Token: 0x04001528 RID: 5416
	public GameObject ConfirmationWindow;

	// Token: 0x04001529 RID: 5417
	public GameObject InformationWindow;

	// Token: 0x0400152A RID: 5418
	public GameObject SkillEnchantWindow;

	// Token: 0x0400152B RID: 5419
	public GameObject InventoryBoosterWindow;

	// Token: 0x0400152C RID: 5420
	public GameObject WarehouseBoosterWindow;

	// Token: 0x0400152D RID: 5421
	public GameObject RateGameWindow;

	// Token: 0x0400152E RID: 5422
	public GameObject ChangeNameWindow;

	// Token: 0x0400152F RID: 5423
	public GameObject AddGoldWindow;

	// Token: 0x04001530 RID: 5424
	public GameObject FreePremiumWindow;

	// Token: 0x04001531 RID: 5425
	public GameObject RenewPremiumWindow;

	// Token: 0x04001532 RID: 5426
	public GameObject AllPackagesDescriptionWindow;

	// Token: 0x04001533 RID: 5427
	public GameObject PromoPackagesWindow;

	// Token: 0x04001534 RID: 5428
	public GameObject GuildCreationWindow;

	// Token: 0x04001535 RID: 5429
	public GameObject ChangeGuildWindow;

	// Token: 0x04001536 RID: 5430
	public GameObject GuildWindow;

	// Token: 0x04001537 RID: 5431
	public GameObject GuildInvitationWindow;

	// Token: 0x04001538 RID: 5432
	public GameObject CollectorConfigWindow;

	// Token: 0x04001539 RID: 5433
	public GameObject TvtEventWindow;

	// Token: 0x0400153A RID: 5434
	public GameObject BossEventWindow;

	// Token: 0x0400153B RID: 5435
	[Header("Game Window Holders")]
	public GameObject EquipmentSlotsHolder;

	// Token: 0x0400153C RID: 5436
	public GameObject InventoryHolder;

	// Token: 0x0400153D RID: 5437
	public GameObject SkillBookHolder;

	// Token: 0x0400153E RID: 5438
	public GameObject WarehouseHolder;

	// Token: 0x0400153F RID: 5439
	[Header("HUD Parts")]
	[SerializeField]
	private GameObject uiHud;

	// Token: 0x04001540 RID: 5440
	public GameObject ChatHolder;

	// Token: 0x04001541 RID: 5441
	public GameObject ConditionBar;

	// Token: 0x04001542 RID: 5442
	public GameObject HudHolder;

	// Token: 0x04001543 RID: 5443
	public GameObject ItemBarWindow;

	// Token: 0x04001544 RID: 5444
	public GameObject MinimapHolder;

	// Token: 0x04001545 RID: 5445
	public GameObject QuestTracker;

	// Token: 0x04001546 RID: 5446
	public GameObject SkillBarWindow;

	// Token: 0x04001547 RID: 5447
	public GameObject SecondSkillBarWindow;

	// Token: 0x04001548 RID: 5448
	[SerializeField]
	private Image pvpButtonIcon;

	// Token: 0x04001549 RID: 5449
	[SerializeField]
	private Text currentWorldAreaName;

	// Token: 0x0400154A RID: 5450
	[SerializeField]
	private GameObject targetHudHolder;

	// Token: 0x0400154B RID: 5451
	[SerializeField]
	private GameObject partyMembersHolder;

	// Token: 0x0400154C RID: 5452
	[SerializeField]
	private GameObject targetConditionBar;

	// Token: 0x0400154D RID: 5453
	[SerializeField]
	private Camera minimapCamera;

	// Token: 0x0400154E RID: 5454
	[Header("Mobile HUD Parts")]
	[SerializeField]
	private GameObject mobileHud;

	// Token: 0x0400154F RID: 5455
	public Joystick MovementJoystick;

	// Token: 0x04001550 RID: 5456
	public GameObject MobileChatHolder;

	// Token: 0x04001551 RID: 5457
	public GameObject MobileMinimapHolder;

	// Token: 0x04001552 RID: 5458
	public GameObject MobileHudHolder;

	// Token: 0x04001553 RID: 5459
	public GameObject MobileConditionBar;

	// Token: 0x04001554 RID: 5460
	public GameObject MobileCombatIcon;

	// Token: 0x04001555 RID: 5461
	public GameObject MobileFightingIcon;

	// Token: 0x04001556 RID: 5462
	[SerializeField]
	private Image mobilePvpButtonIcon;

	// Token: 0x04001557 RID: 5463
	[SerializeField]
	private Text mobileCurrentWorldAreaName;

	// Token: 0x04001558 RID: 5464
	[SerializeField]
	private GameObject mobileTargetHudHolder;

	// Token: 0x04001559 RID: 5465
	[SerializeField]
	private GameObject mobilePartyMembersHolder;

	// Token: 0x0400155A RID: 5466
	[SerializeField]
	private GameObject mobileTargetConditionBar;

	// Token: 0x0400155B RID: 5467
	[Header("Other GUI Parts")]
	public Text FpsText;

	// Token: 0x0400155C RID: 5468
	public Text LatencyText;

	// Token: 0x0400155D RID: 5469
	public Text FeedbackText;

	// Token: 0x0400155E RID: 5470
	public Canvas UISystemCanvas;

	// Token: 0x0400155F RID: 5471
	[SerializeField]
	private CanvasScaler uiHudScaler;

	// Token: 0x04001560 RID: 5472
	[SerializeField]
	private CanvasScaler uiSystemScaler;

	// Token: 0x04001561 RID: 5473
	[SerializeField]
	private CanvasScaler uiTweaksScaler;

	// Token: 0x04001562 RID: 5474
	public GameObject FeedbackPanel;

	// Token: 0x04001563 RID: 5475
	public GameObject HintWindow;

	// Token: 0x04001564 RID: 5476
	public GameObject ItemTooltipWindow;

	// Token: 0x04001565 RID: 5477
	public GameObject SkillTooltipWindow;

	// Token: 0x04001566 RID: 5478
	public GameObject DefaultTooltipWindow;

	// Token: 0x04001567 RID: 5479
	public GameObject MenuBackgroundImage;

	// Token: 0x04001568 RID: 5480
	public GameObject MessagePanel;

	// Token: 0x04001569 RID: 5481
	public GameObject CombatIcon;

	// Token: 0x0400156A RID: 5482
	public GameObject FightingIcon;

	// Token: 0x0400156B RID: 5483
	[SerializeField]
	private GameObject blackOverlay;

	// Token: 0x0400156C RID: 5484
	[SerializeField]
	private GameObject showChatButton;

	// Token: 0x0400156D RID: 5485
	[Header("GUI Objects")]
	public Sprite PvpEnabledSprite;

	// Token: 0x0400156E RID: 5486
	public Sprite PvpDisabledSprite;

	// Token: 0x0400156F RID: 5487
	[SerializeField]
	private GameObject conditionBarSlotPrefab;

	// Token: 0x04001570 RID: 5488
	[SerializeField]
	private GameObject partyMemberPrefab;

	// Token: 0x04001571 RID: 5489
	[SerializeField]
	private GameObject mobilePartyMemberPrefab;

	// Token: 0x04001572 RID: 5490
	[Header("GUI Managers")]
	[SerializeField]
	private ChatHolderManager ChatHolderManager;

	// Token: 0x04001573 RID: 5491
	public DialogWindowManager DialogWindowManager;

	// Token: 0x04001574 RID: 5492
	public GameEnvironmentModule GameEnvironmentModule;

	// Token: 0x04001575 RID: 5493
	public FriendListWindowManager FriendListWindowManager;

	// Token: 0x04001576 RID: 5494
	public PartyInvitationWindowManager PartyInvitationWindowManager;

	// Token: 0x04001577 RID: 5495
	[Header("Mobile GUI Managers")]
	[SerializeField]
	private ChatHolderManager MobileChatHolderManager;

	// Token: 0x04001578 RID: 5496
	[Header("Common Variables")]
	[SerializeField]
	private Sprite defaultConfirmationSprite;

	// Token: 0x04001579 RID: 5497
	[HideInInspector]
	public bool ShowAllNames;

	// Token: 0x0400157A RID: 5498
	[HideInInspector]
	public bool HasWindowsOpened;

	// Token: 0x0400157B RID: 5499
	private float lastFpsUpdate;

	// Token: 0x0400157C RID: 5500
	private float deltaTime;

	// Token: 0x0400157D RID: 5501
	private float timeSinceSceneLoaded;
}
