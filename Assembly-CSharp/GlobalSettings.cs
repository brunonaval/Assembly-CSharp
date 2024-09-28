using System;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

// Token: 0x0200055E RID: 1374
public static class GlobalSettings
{
	// Token: 0x06001E44 RID: 7748 RVA: 0x00096E48 File Offset: 0x00095048
	static GlobalSettings()
	{
		GlobalSettings.LoadColors();
		GlobalSettings.IsMobilePlatform = Application.isMobilePlatform;
	}

	// Token: 0x06001E45 RID: 7749 RVA: 0x00096F08 File Offset: 0x00095108
	private static void LoadColors()
	{
		GlobalSettings.Colors.Add(0, new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
		GlobalSettings.Colors.Add(1, new Color32(83, byte.MaxValue, 0, byte.MaxValue));
		GlobalSettings.Colors.Add(2, new Color32(byte.MaxValue, 153, 0, byte.MaxValue));
		GlobalSettings.Colors.Add(3, new Color32(202, 16, 16, byte.MaxValue));
		GlobalSettings.Colors.Add(5, new Color32(243, 183, 1, byte.MaxValue));
		GlobalSettings.Colors.Add(4, Color.blue);
		GlobalSettings.Colors.Add(6, new Color32(50, 121, 186, byte.MaxValue));
		GlobalSettings.Colors.Add(9, new Color32(104, 151, 187, byte.MaxValue));
		GlobalSettings.Colors.Add(7, new Color32(116, 71, 106, byte.MaxValue));
		GlobalSettings.Colors.Add(8, new Color32(125, 96, 75, byte.MaxValue));
		GlobalSettings.Colors.Add(10, new Color32(180, 180, 180, byte.MaxValue));
		GlobalSettings.Colors.Add(11, new Color32(178, 157, 141, byte.MaxValue));
	}

	// Token: 0x170002C7 RID: 711
	// (get) Token: 0x06001E46 RID: 7750 RVA: 0x000970BB File Offset: 0x000952BB
	public static string PublicApiBaseUrl
	{
		get
		{
			return GlobalSettings.BuildApiBaseUrl(GlobalSettings.PublicApiKey);
		}
	}

	// Token: 0x06001E47 RID: 7751 RVA: 0x000970C7 File Offset: 0x000952C7
	public static string BuildApiBaseUrl(string apiKey)
	{
		return GlobalSettings.ApiHost + "api/" + apiKey + "/";
	}

	// Token: 0x040017D5 RID: 6101
	public const string ConnectionVersion = "2024.2.7";

	// Token: 0x040017D6 RID: 6102
	public const int MaxIdleTime = 168000;

	// Token: 0x040017D7 RID: 6103
	public static readonly bool IsMobilePlatform;

	// Token: 0x040017D8 RID: 6104
	public static readonly Dictionary<int, Color> Colors = new Dictionary<int, Color>();

	// Token: 0x040017D9 RID: 6105
	public const int WhiteColorId = 0;

	// Token: 0x040017DA RID: 6106
	public const int GreenColorId = 1;

	// Token: 0x040017DB RID: 6107
	public const int OrangeColorId = 2;

	// Token: 0x040017DC RID: 6108
	public const int RedColorId = 3;

	// Token: 0x040017DD RID: 6109
	public const int BlueColorId = 4;

	// Token: 0x040017DE RID: 6110
	public const int YellowColorId = 5;

	// Token: 0x040017DF RID: 6111
	public const int LightBlueColorId = 6;

	// Token: 0x040017E0 RID: 6112
	public const int PurpleColorId = 7;

	// Token: 0x040017E1 RID: 6113
	public const int BrownColorId = 8;

	// Token: 0x040017E2 RID: 6114
	public const int PaleBlueColorId = 9;

	// Token: 0x040017E3 RID: 6115
	public const int GrayColorId = 10;

	// Token: 0x040017E4 RID: 6116
	public const int LightBrownColorId = 11;

	// Token: 0x040017E5 RID: 6117
	public static string WebPage = "https://eternal-quest.online";

	// Token: 0x040017E6 RID: 6118
	public static string ApiHost = "https://eternal-quest.online/";

	// Token: 0x040017E7 RID: 6119
	public static string DiscordServer = "https://discord.gg/9kdRAK9";

	// Token: 0x040017E8 RID: 6120
	public static string InstagramProfile = "https://instagram.com/eq_game";

	// Token: 0x040017E9 RID: 6121
	public static string FacebookPage = "http://facebook.com.br/eternalquestgame";

	// Token: 0x040017EA RID: 6122
	public static string YoutubeChannel = "https://www.youtube.com/channel/UCqHvEBw9HTmZIwgGOvMO4OA";

	// Token: 0x040017EB RID: 6123
	public static string TwitterProfile = "https://twitter.com/eq_game";

	// Token: 0x040017EC RID: 6124
	public static string WikiUrl = "https://discord.gg/MkHrTRaazT";

	// Token: 0x040017ED RID: 6125
	public static string PublicApiKey = "5C2EFE99-F51A-4E87-9A04-215AFE10BF26";

	// Token: 0x040017EE RID: 6126
	public static readonly ObscuredInt StoreItemPriceMultiplier = 8;

	// Token: 0x040017EF RID: 6127
	public const int RequiredChatLevel = 2;

	// Token: 0x040017F0 RID: 6128
	public const int FreePremiumDays = 3;

	// Token: 0x040017F1 RID: 6129
	public const int MaxProfessionLevel = 850;

	// Token: 0x040017F2 RID: 6130
	public const string PartyChatChannel = "chat_tab_party";

	// Token: 0x040017F3 RID: 6131
	public const string GlobalChatChannel = "chat_tab_global";

	// Token: 0x040017F4 RID: 6132
	public const string HelpChatChannel = "chat_tab_help";

	// Token: 0x040017F5 RID: 6133
	public const string DefaultChatChannel = "chat_tab_default";

	// Token: 0x040017F6 RID: 6134
	public const string SystemChatChannel = "chat_tab_system";

	// Token: 0x040017F7 RID: 6135
	public const int MinPvpLevel = 25;

	// Token: 0x040017F8 RID: 6136
	public const int PlayerHealthBase = 120;

	// Token: 0x040017F9 RID: 6137
	public const float ChatRange = 6f;

	// Token: 0x040017FA RID: 6138
	public const int MaxBaseLevel = 2147483647;

	// Token: 0x040017FB RID: 6139
	public const int DashEnduranceNeeded = 50;

	// Token: 0x040017FC RID: 6140
	public const float CombatTime = 15f;

	// Token: 0x040017FD RID: 6141
	public const float FightingTime = 5f;

	// Token: 0x040017FE RID: 6142
	public const float GroundLootDuration = 20f;

	// Token: 0x040017FF RID: 6143
	public const int MinimumKillScoreToOutlaw = 15;

	// Token: 0x04001800 RID: 6144
	public const float MinRegenerationInterval = 1.5f;

	// Token: 0x04001801 RID: 6145
	public const float EnduranceRegenerationInterval = 1f;

	// Token: 0x04001802 RID: 6146
	public const float BasicCollectorPower = 3f;

	// Token: 0x04001803 RID: 6147
	public const float FineCollectorPower = 7f;

	// Token: 0x04001804 RID: 6148
	public const float MasterworkCollectorPower = 15f;

	// Token: 0x04001805 RID: 6149
	public const float ResolutionWidthRef = 3840f;

	// Token: 0x04001806 RID: 6150
	public const float ResolutionHeightRef = 2160f;

	// Token: 0x04001807 RID: 6151
	public const float AttackGainPercentPerItemLevel = 0.0295f;

	// Token: 0x04001808 RID: 6152
	public const float DefenseGainPercentPerItemLevel = 0.0295f;

	// Token: 0x04001809 RID: 6153
	public const float AttackGainPercentPerItemQuality = 0.3f;

	// Token: 0x0400180A RID: 6154
	public const float DefenseGainPercentPerItemQuality = 0.3f;

	// Token: 0x0400180B RID: 6155
	public const float PlaceOrderFee = 0.04f;

	// Token: 0x0400180C RID: 6156
	public const int MaxItemStack = 750;

	// Token: 0x0400180D RID: 6157
	public const int MaxItemBoost = 21;

	// Token: 0x0400180E RID: 6158
	public const int MaxSkillEnchant = 10;

	// Token: 0x0400180F RID: 6159
	public const float ReagentPercentBonus = 0.25f;

	// Token: 0x04001810 RID: 6160
	public const int InventoryBoostGoldPrice = 2000;

	// Token: 0x04001811 RID: 6161
	public const int InventoryBoostGemPrice = 2;

	// Token: 0x04001812 RID: 6162
	public const int MaxTotalInventorySlots = 200;

	// Token: 0x04001813 RID: 6163
	public const int MaxBasicInventorySlots = 25;

	// Token: 0x04001814 RID: 6164
	public const int MaxVeteranInventorySlots = 40;

	// Token: 0x04001815 RID: 6165
	public const int MaxEliteInventorySlots = 45;

	// Token: 0x04001816 RID: 6166
	public const int MaxLegendaryInventorySlots = 55;

	// Token: 0x04001817 RID: 6167
	public const int InitialInventorySlots = 16;

	// Token: 0x04001818 RID: 6168
	public const int WarehouseBoostGoldPrice = 4000;

	// Token: 0x04001819 RID: 6169
	public const int WarehouseBoostGemPrice = 4;

	// Token: 0x0400181A RID: 6170
	public const int MaxTotalWarehouseSlots = 450;

	// Token: 0x0400181B RID: 6171
	public const int MaxBasicWarehouseSlots = 35;

	// Token: 0x0400181C RID: 6172
	public const int InitialWarehouseSlots = 20;

	// Token: 0x0400181D RID: 6173
	public const float PremiumExpPercentBonus = 0.5f;

	// Token: 0x0400181E RID: 6174
	public const float PremiumAxpPercentBonus = 0.5f;

	// Token: 0x0400181F RID: 6175
	public const float WeekTvtHeroExpPercentBonus = 0.25f;

	// Token: 0x04001820 RID: 6176
	public const float WeekTvtHeroAxpPercentBonus = 0.25f;

	// Token: 0x04001821 RID: 6177
	public const float WeekTvtHeroProfessionPercentBonus = 0.25f;

	// Token: 0x04001822 RID: 6178
	public const float PremiumProfessionExpPercentBonus = 0.5f;

	// Token: 0x04001823 RID: 6179
	public const float PremiumOrderFeePercentDiscount = 0.5f;

	// Token: 0x04001824 RID: 6180
	public const float PremiumEquipmentDropChancePercentProtection = 0.5f;

	// Token: 0x04001825 RID: 6181
	public const int PremiumDailyRewardBonus = 2;

	// Token: 0x04001826 RID: 6182
	public const int PremiumDailyTaskRewardBonus = 2;

	// Token: 0x04001827 RID: 6183
	public const float MaxDropDistance = 4f;

	// Token: 0x04001828 RID: 6184
	public const float FramesPerSecond = 13f;

	// Token: 0x04001829 RID: 6185
	public const float AnimatedTextSpeed = 0.02f;

	// Token: 0x0400182A RID: 6186
	public const float AnimatedTextGrowthSpeed = 0.001f;

	// Token: 0x0400182B RID: 6187
	public const float DefaultFrameSpeed = 0.1f;

	// Token: 0x0400182C RID: 6188
	public const float AnimatedTextDuration = 0.5f;

	// Token: 0x0400182D RID: 6189
	public const float MessageTextDuration = 3.5f;

	// Token: 0x0400182E RID: 6190
	public const float MessageTextLongDuration = 7f;

	// Token: 0x0400182F RID: 6191
	public const string MonsterPrefabName = "Prefabs/Monster";

	// Token: 0x04001830 RID: 6192
	public const string NpcPrefabName = "Prefabs/Npc";

	// Token: 0x04001831 RID: 6193
	public static readonly string[] BlockableTags = new string[]
	{
		"Player",
		"Monster",
		"BlockAll",
		"BlockProjectile",
		"Combatant"
	};

	// Token: 0x04001832 RID: 6194
	public const string RankIconSpritesPath = "Icons/Ranks/";

	// Token: 0x04001833 RID: 6195
	public const string VocationIconSpritesPath = "Icons/Vocations/";

	// Token: 0x04001834 RID: 6196
	public const string VocationSpritesPath = "Sprites/Vocations/";

	// Token: 0x04001835 RID: 6197
	public const string ConditionIconSpritesPath = "Icons/Conditions/";

	// Token: 0x04001836 RID: 6198
	public const string ItemIconSpritesPath = "Icons/Items/";

	// Token: 0x04001837 RID: 6199
	public const string SkillIconSpritesPath = "Icons/Skills/";

	// Token: 0x04001838 RID: 6200
	public const string SlotIconSpritesPath = "Icons/Slots/";

	// Token: 0x04001839 RID: 6201
	public const string MinimapIconSpritesPath = "Icons/Minimap/";

	// Token: 0x0400183A RID: 6202
	public const string AnimationPrefabsPath = "Prefabs/Animations/";

	// Token: 0x0400183B RID: 6203
	public const string MonsterAnimationPrefabsPath = "Prefabs/Animations/Monsters/";

	// Token: 0x0400183C RID: 6204
	public const string EffectSpritesPath = "Sprites/Effects/";

	// Token: 0x0400183D RID: 6205
	public const string ProjectilePrefabsPath = "Prefabs/Projectiles/";

	// Token: 0x0400183E RID: 6206
	public const string EffectsSoundPath = "Sounds/Effects/";

	// Token: 0x0400183F RID: 6207
	public const string BackgroundSoundPath = "Sounds/Background/";

	// Token: 0x04001840 RID: 6208
	public const string AmbiencesSoundPath = "Sounds/Ambiences/";

	// Token: 0x04001841 RID: 6209
	public const string DefaultIconName = "undefined_icon";

	// Token: 0x04001842 RID: 6210
	public const string DefaultSkillBarIconName = "default_skill_slot";

	// Token: 0x04001843 RID: 6211
	public const string DefaultItemBarIconName = "default_item_slot";

	// Token: 0x04001844 RID: 6212
	public const string DefaultWarriorSkin = "default_warrior_skin";

	// Token: 0x04001845 RID: 6213
	public const string DefaultSentinelSkin = "default_sentinel_skin";

	// Token: 0x04001846 RID: 6214
	public const string DefaultElementorSkin = "default_elementor_skin";

	// Token: 0x04001847 RID: 6215
	public const string DefaultLyrusSkin = "default_lyrus_skin";

	// Token: 0x04001848 RID: 6216
	public const string DefaultProtectorSkin = "default_protector_skin";

	// Token: 0x04001849 RID: 6217
	public const string EliteWarriorSkin = "elite_warrior_skin";

	// Token: 0x0400184A RID: 6218
	public const string EliteSentinelSkin = "elite_sentinel_skin";

	// Token: 0x0400184B RID: 6219
	public const string EliteElementorSkin = "elite_elementor_skin";

	// Token: 0x0400184C RID: 6220
	public const string EliteLyrusSkin = "elite_lyrus_skin";

	// Token: 0x0400184D RID: 6221
	public const string EliteProtectorSkin = "elite_protector_skin";

	// Token: 0x0400184E RID: 6222
	public const string WhiteBodySkin = "white_body_skin";

	// Token: 0x0400184F RID: 6223
	public const string PaleBlueBodySkin = "pale_blue_body_skin";

	// Token: 0x04001850 RID: 6224
	public const string BlackBodySkin = "black_body_skin";

	// Token: 0x04001851 RID: 6225
	public const string DefaultSpearSkin = "default_spear_skin";

	// Token: 0x04001852 RID: 6226
	public const string DefaultStaffSkin = "default_staff_skin";

	// Token: 0x04001853 RID: 6227
	public const string DefaultSwordSkin = "default_sword_skin";

	// Token: 0x04001854 RID: 6228
	public const string DefaultShieldSkin = "default_shield_skin";

	// Token: 0x04001855 RID: 6229
	public const string DefaultBowSkin = "default_bow_skin";

	// Token: 0x04001856 RID: 6230
	public const string WalkVerticalButton = "Vertical";

	// Token: 0x04001857 RID: 6231
	public const string WalkHorizontalButton = "Horizontal";

	// Token: 0x04001858 RID: 6232
	public const string WalkUpButton = "Walk Up";

	// Token: 0x04001859 RID: 6233
	public const string WalkLeftButton = "Walk Left";

	// Token: 0x0400185A RID: 6234
	public const string WalkDownButton = "Walk Down";

	// Token: 0x0400185B RID: 6235
	public const string WalkRightButton = "Walk Right";

	// Token: 0x0400185C RID: 6236
	public const string DashButton = "Dash";

	// Token: 0x0400185D RID: 6237
	public const string ActionButton = "Action";

	// Token: 0x0400185E RID: 6238
	public const string SubmitButton = "Submit";

	// Token: 0x0400185F RID: 6239
	public const string CancelButton = "Cancel";

	// Token: 0x04001860 RID: 6240
	public const string CollectButton = "Collect";

	// Token: 0x04001861 RID: 6241
	public const string PlayerWindowButton = "Player Window";

	// Token: 0x04001862 RID: 6242
	public const string SkillsWindowButton = "Skills Window";

	// Token: 0x04001863 RID: 6243
	public const string AttributesWindowButton = "Attributes Window";

	// Token: 0x04001864 RID: 6244
	public const string InventoryWindowButton = "Inventory Window";

	// Token: 0x04001865 RID: 6245
	public const string ChangeTargetButton = "Change Target";

	// Token: 0x04001866 RID: 6246
	public const string QuestWindowButton = "Quest Window";

	// Token: 0x04001867 RID: 6247
	public const string FriendListWindowButton = "FriendList Window";

	// Token: 0x04001868 RID: 6248
	public const string HelpWindowButton = "Help Window";

	// Token: 0x04001869 RID: 6249
	public const string MapWindowButton = "Map Window";

	// Token: 0x0400186A RID: 6250
	public const string StorageWindowButton = "Storage Window";

	// Token: 0x0400186B RID: 6251
	public const string ChangeSkillBars = "Change SkillBars";

	// Token: 0x0400186C RID: 6252
	public const string Item1Button = "Item1";

	// Token: 0x0400186D RID: 6253
	public const string Item2Button = "Item2";

	// Token: 0x0400186E RID: 6254
	public const string Item3Button = "Item3";

	// Token: 0x0400186F RID: 6255
	public const string Item4Button = "Item4";

	// Token: 0x04001870 RID: 6256
	public const string Item5Button = "Item5";

	// Token: 0x04001871 RID: 6257
	public const string Item6Button = "Item6";

	// Token: 0x04001872 RID: 6258
	public const string Item7Button = "Item7";

	// Token: 0x04001873 RID: 6259
	public const string Item8Button = "Item8";

	// Token: 0x04001874 RID: 6260
	public const string Item9Button = "Item9";

	// Token: 0x04001875 RID: 6261
	public const string Item10Button = "Item10";

	// Token: 0x04001876 RID: 6262
	public const string Skill1Button = "Skill1";

	// Token: 0x04001877 RID: 6263
	public const string Skill2Button = "Skill2";

	// Token: 0x04001878 RID: 6264
	public const string Skill3Button = "Skill3";

	// Token: 0x04001879 RID: 6265
	public const string Skill4Button = "Skill4";

	// Token: 0x0400187A RID: 6266
	public const string Skill5Button = "Skill5";

	// Token: 0x0400187B RID: 6267
	public const string Skill6Button = "Skill6";

	// Token: 0x0400187C RID: 6268
	public const string Skill7Button = "Skill7";

	// Token: 0x0400187D RID: 6269
	public const string Skill8Button = "Skill8";

	// Token: 0x0400187E RID: 6270
	public const string Skill9Button = "Skill9";

	// Token: 0x0400187F RID: 6271
	public const string Skill0Button = "Skill0";

	// Token: 0x04001880 RID: 6272
	public const string SecondSkill1Button = "Second Skill1";

	// Token: 0x04001881 RID: 6273
	public const string SecondSkill2Button = "Second Skill2";

	// Token: 0x04001882 RID: 6274
	public const string SecondSkill3Button = "Second Skill3";

	// Token: 0x04001883 RID: 6275
	public const string SecondSkill4Button = "Second Skill4";

	// Token: 0x04001884 RID: 6276
	public const string SecondSkill5Button = "Second Skill5";

	// Token: 0x04001885 RID: 6277
	public const string SecondSkill6Button = "Second Skill6";

	// Token: 0x04001886 RID: 6278
	public const string SecondSkill7Button = "Second Skill7";

	// Token: 0x04001887 RID: 6279
	public const string SecondSkill8Button = "Second Skill8";

	// Token: 0x04001888 RID: 6280
	public const string SecondSkill9Button = "Second Skill9";

	// Token: 0x04001889 RID: 6281
	public const string SecondSkill0Button = "Second Skill0";
}
