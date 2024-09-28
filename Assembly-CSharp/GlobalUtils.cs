using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Mirror;
using UnityEngine;

// Token: 0x0200055F RID: 1375
public static class GlobalUtils
{
	// Token: 0x06001E48 RID: 7752 RVA: 0x000970DE File Offset: 0x000952DE
	public static float GetAnimationDelay(AnimationType animationType, float framesPerSecondDelta = 0f)
	{
		framesPerSecondDelta = ((framesPerSecondDelta > 0f) ? framesPerSecondDelta : 0.1f);
		if (animationType == AnimationType.ShootArrow)
		{
			return 7f * framesPerSecondDelta;
		}
		return 4f * framesPerSecondDelta;
	}

	// Token: 0x06001E49 RID: 7753 RVA: 0x00097105 File Offset: 0x00095305
	public static int GetMaxInventorySlots(PackageType packageType)
	{
		switch (packageType)
		{
		case PackageType.Free:
			return 25;
		case PackageType.Veteran:
			return 40;
		case PackageType.Elite:
			return 45;
		case PackageType.Lengendary:
			return 55;
		default:
			return 25;
		}
	}

	// Token: 0x06001E4A RID: 7754 RVA: 0x00097130 File Offset: 0x00095330
	public static Color RarityToFrameColor(Rarity rarity)
	{
		switch (rarity)
		{
		case Rarity.Uncommon:
			return new Color(0.68f, 0.85f, 0.9f, 1f);
		case Rarity.Rare:
			return new Color(0f, 1f, 0f, 1f);
		case Rarity.Exotic:
			return new Color(1f, 0.65f, 0f, 1f);
		case Rarity.Legendary:
			return new Color(0.5f, 0f, 0.5f, 1f);
		case Rarity.Divine:
			return new Color(1f, 0f, 0f, 1f);
		}
		return new Color(0.31f, 0.25f, 0.23f, 1f);
	}

	// Token: 0x06001E4B RID: 7755 RVA: 0x00097200 File Offset: 0x00095400
	public static string SecondsToCooldownText(double seconds)
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
		if (timeSpan.Days > 0)
		{
			return string.Format("{0} d, {1} h, {2} m, {3} s", new object[]
			{
				timeSpan.Days,
				timeSpan.Hours,
				timeSpan.Minutes,
				timeSpan.Seconds
			});
		}
		if (timeSpan.Hours > 0)
		{
			return string.Format("{0} h, {1} m, {2} s", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
		}
		if (timeSpan.Minutes > 0)
		{
			return string.Format("{0} m, {1} s", timeSpan.Minutes, timeSpan.Seconds);
		}
		if (timeSpan.Seconds > 0)
		{
			return string.Format("{0} s", timeSpan.Seconds);
		}
		return string.Format("{0:0.0} s", (float)timeSpan.Milliseconds / 1000f);
	}

	// Token: 0x06001E4C RID: 7756 RVA: 0x00097310 File Offset: 0x00095510
	public static Vector3 RandomCircle(Vector3 center, float radius)
	{
		float num = UnityEngine.Random.value * 360f;
		Vector3 result;
		result.x = center.x + radius * Mathf.Sin(num * 0.017453292f);
		result.y = center.y + radius * Mathf.Cos(num * 0.017453292f);
		result.z = center.z;
		return result;
	}

	// Token: 0x06001E4D RID: 7757 RVA: 0x0009736F File Offset: 0x0009556F
	public static void SpawnItemOnGround(Item item, GameObject owner, Vector3 position, float duration)
	{
		GameObject gameObject = GlobalUtils.InstatiateGroundSlot(position);
		gameObject.GetComponent<GroundSlotModule>().Initialize(item, owner, duration);
		NetworkServer.Spawn(gameObject, null);
	}

	// Token: 0x06001E4E RID: 7758 RVA: 0x0009738B File Offset: 0x0009558B
	private static GameObject InstatiateGroundSlot(Vector3 position)
	{
		return ObjectPoolModule.Instance.GetGroundSlotFromPool(position);
	}

	// Token: 0x06001E4F RID: 7759 RVA: 0x00097398 File Offset: 0x00095598
	public static Direction AngleToDirection(float angle)
	{
		if (angle >= 45f & angle < 135f)
		{
			return Direction.North;
		}
		if (angle >= -45f & angle < 45f)
		{
			return Direction.East;
		}
		if (angle >= -135f & angle < -45f)
		{
			return Direction.South;
		}
		if (angle >= 135f | angle < -135f)
		{
			return Direction.West;
		}
		return Direction.South;
	}

	// Token: 0x06001E50 RID: 7760 RVA: 0x00097408 File Offset: 0x00095608
	public static Direction FindTargetDirection(Vector3 casterPosition, Vector3 targetPosition)
	{
		Vector3 vector = targetPosition - casterPosition;
		vector.Normalize();
		return GlobalUtils.AngleToDirection(Mathf.Atan2(vector.y, vector.x) * 57.29578f);
	}

	// Token: 0x06001E51 RID: 7761 RVA: 0x00097440 File Offset: 0x00095640
	public static Quaternion DirectionToRotation(Direction direction, float angle = 0f)
	{
		switch (direction)
		{
		case Direction.North:
			return Quaternion.Euler(0f, 0f, 90f + angle);
		case Direction.West:
			return Quaternion.Euler(0f, 0f, 180f + angle);
		case Direction.South:
			return Quaternion.Euler(0f, 0f, 270f + angle);
		case Direction.East:
			return Quaternion.Euler(0f, 0f, 0f + angle);
		default:
			return Quaternion.Euler(0f, 0f, 0f + angle);
		}
	}

	// Token: 0x06001E52 RID: 7762 RVA: 0x000974D7 File Offset: 0x000956D7
	public static Vector3 DirectionToPosition(Direction direction)
	{
		switch (direction)
		{
		case Direction.North:
			return Vector3.up;
		case Direction.West:
			return Vector3.left;
		case Direction.South:
			return Vector3.down;
		case Direction.East:
			return Vector3.right;
		default:
			return Vector3.up;
		}
	}

	// Token: 0x06001E53 RID: 7763 RVA: 0x0009750E File Offset: 0x0009570E
	public static Direction InverseDirection(Direction direction)
	{
		switch (direction)
		{
		case Direction.North:
			return Direction.South;
		case Direction.West:
			return Direction.East;
		case Direction.South:
			return Direction.North;
		case Direction.East:
			return Direction.West;
		default:
			return direction;
		}
	}

	// Token: 0x06001E54 RID: 7764 RVA: 0x00097534 File Offset: 0x00095734
	public static string SlotTypeToString(SlotType slotType)
	{
		string text = Regex.Replace(Enum.GetName(typeof(SlotType), slotType), "(?<=[a-z])([A-Z])", " $1").ToLower();
		text = new string((from w in text
		where char.IsLetterOrDigit(w) | char.IsWhiteSpace(w)
		select w).ToArray<char>());
		text = text.Replace(" ", "_");
		text = "slot_type_" + text;
		return LanguageManager.Instance.GetText(text);
	}

	// Token: 0x06001E55 RID: 7765 RVA: 0x000975C4 File Offset: 0x000957C4
	public static string VocationToString(Vocation vocation)
	{
		string text = Regex.Replace(Enum.GetName(typeof(Vocation), vocation), "(?<=[a-z])([A-Z])", " $1").ToLower();
		text = new string((from w in text
		where char.IsLetterOrDigit(w) | char.IsWhiteSpace(w)
		select w).ToArray<char>());
		text = text.Replace(" ", "_");
		text = "vocation_" + text;
		return LanguageManager.Instance.GetText(text);
	}

	// Token: 0x06001E56 RID: 7766 RVA: 0x00097654 File Offset: 0x00095854
	public static string KeyCodeToString(KeyCode keyCode)
	{
		string text = Regex.Replace(Enum.GetName(typeof(KeyCode), keyCode), "(?<=[a-z])([A-Z])", " $1").ToLower();
		text = new string((from w in text
		where char.IsLetterOrDigit(w) | char.IsWhiteSpace(w)
		select w).ToArray<char>());
		text = text.Replace(" ", "_");
		text = "keycode_" + text;
		string text2 = LanguageManager.Instance.GetText(text);
		if (text2.StartsWith("keycode_"))
		{
			return keyCode.ToString();
		}
		return text2;
	}

	// Token: 0x06001E57 RID: 7767 RVA: 0x00097700 File Offset: 0x00095900
	public static string KeyMapNameToString(string mapName)
	{
		mapName = Regex.Replace(mapName, "(?<=[a-z])([A-Z])", " $1");
		string text = mapName.ToLower();
		text = new string((from w in text
		where char.IsLetterOrDigit(w) | char.IsWhiteSpace(w)
		select w).ToArray<char>());
		text = text.Replace(" ", "_");
		text = "keymap_" + text;
		return LanguageManager.Instance.GetText(text);
	}

	// Token: 0x06001E58 RID: 7768 RVA: 0x00097780 File Offset: 0x00095980
	public static string GuildMemberRankToString(GuildMemberRank memberRank)
	{
		string text = Regex.Replace(Enum.GetName(typeof(GuildMemberRank), memberRank), "(?<=[a-z])([A-Z])", " $1").ToLower();
		text = new string((from w in text
		where char.IsLetterOrDigit(w) | char.IsWhiteSpace(w)
		select w).ToArray<char>());
		text = text.Replace(" ", "_");
		text = "guild_member_rank_" + text;
		return LanguageManager.Instance.GetText(text);
	}

	// Token: 0x06001E59 RID: 7769 RVA: 0x0009780F File Offset: 0x00095A0F
	public static string ItemQualityToString(ItemQuality itemQuality)
	{
		return GlobalUtils.ItemQualityToMeta(Enum.GetName(typeof(ItemQuality), itemQuality));
	}

	// Token: 0x06001E5A RID: 7770 RVA: 0x0009782C File Offset: 0x00095A2C
	public static string ItemQualityToMeta(string qualityName)
	{
		string text = Regex.Replace(qualityName, "(?<=[a-z])([A-Z])", " $1").ToLower();
		text = new string((from w in text
		where char.IsLetterOrDigit(w) | char.IsWhiteSpace(w)
		select w).ToArray<char>());
		text = text.Replace(" ", "_");
		text = "item_quality_" + text;
		return LanguageManager.Instance.GetText(text);
	}

	// Token: 0x06001E5B RID: 7771 RVA: 0x000978A8 File Offset: 0x00095AA8
	public static string AccessLevelToColorName(AccessLevel accessLevel, bool isWeekTvtHero)
	{
		switch (accessLevel)
		{
		case AccessLevel.Tutor:
			return "white";
		case AccessLevel.SeniorTutor:
			return "lightblue";
		case AccessLevel.GameMaster:
			return "green";
		case AccessLevel.CommunityManager:
			return "orange";
		case AccessLevel.Administrator:
			return "red";
		default:
			if (isWeekTvtHero)
			{
				return "yellow";
			}
			return string.Empty;
		}
	}

	// Token: 0x06001E5C RID: 7772 RVA: 0x00097900 File Offset: 0x00095B00
	public static string GetAccessLevelPrefix(AccessLevel accessLevel, bool isWeekTvtHero)
	{
		switch (accessLevel)
		{
		case AccessLevel.Tutor:
			return "<color=white>[TT] </color>";
		case AccessLevel.SeniorTutor:
			return "<color=lightblue>[ST] </color>";
		case AccessLevel.GameMaster:
			return "<color=green>[GM] </color>";
		case AccessLevel.CommunityManager:
			return "<color=orange>[CM] </color>";
		case AccessLevel.Administrator:
			return "<color=red>[ADM] </color>";
		default:
			if (isWeekTvtHero)
			{
				return "<color=yellow>[HERO] </color>";
			}
			return string.Empty;
		}
	}

	// Token: 0x06001E5D RID: 7773 RVA: 0x00097957 File Offset: 0x00095B57
	public static string ItemCategoryToString(ItemCategory itemCategory)
	{
		return GlobalUtils.ItemCategoryToMeta(Enum.GetName(typeof(ItemCategory), itemCategory));
	}

	// Token: 0x06001E5E RID: 7774 RVA: 0x00097974 File Offset: 0x00095B74
	public static string ItemCategoryToMeta(string itemCategoryName)
	{
		string text = Regex.Replace(itemCategoryName, "(?<=[a-z])([A-Z])", " $1").ToLower();
		text = new string((from w in text
		where char.IsLetterOrDigit(w) | char.IsWhiteSpace(w)
		select w).ToArray<char>());
		text = text.Replace(" ", "_");
		text = "item_category_" + text;
		return LanguageManager.Instance.GetText(text);
	}

	// Token: 0x06001E5F RID: 7775 RVA: 0x000979F0 File Offset: 0x00095BF0
	public static void CopyClass<T>(T source, T target)
	{
		Type typeFromHandle = typeof(T);
		foreach (PropertyInfo propertyInfo in typeFromHandle.GetProperties())
		{
			typeFromHandle.GetProperty(propertyInfo.Name).SetValue(target, propertyInfo.GetValue(source, null), null);
		}
		foreach (FieldInfo fieldInfo in typeFromHandle.GetFields())
		{
			typeFromHandle.GetField(fieldInfo.Name).SetValue(target, fieldInfo.GetValue(source));
		}
	}

	// Token: 0x06001E60 RID: 7776 RVA: 0x00097A88 File Offset: 0x00095C88
	public static string RarityToMeta(string rarityName)
	{
		string text = Regex.Replace(rarityName, "(?<=[a-z])([A-Z])", " $1").ToLower();
		text = new string((from w in text
		where char.IsLetterOrDigit(w) | char.IsWhiteSpace(w)
		select w).ToArray<char>());
		text = text.Replace(" ", "_");
		text = "rarity_" + text;
		return LanguageManager.Instance.GetText(text);
	}

	// Token: 0x06001E61 RID: 7777 RVA: 0x00097B03 File Offset: 0x00095D03
	public static string RarityToString(Rarity rarity)
	{
		return GlobalUtils.RarityToMeta(Enum.GetName(typeof(Rarity), rarity));
	}

	// Token: 0x06001E62 RID: 7778 RVA: 0x00097B1F File Offset: 0x00095D1F
	public static string ItemTypeToString(ItemType itemType)
	{
		return GlobalUtils.ItemTypeNameToMeta(Enum.GetName(typeof(ItemType), itemType));
	}

	// Token: 0x06001E63 RID: 7779 RVA: 0x00097B3C File Offset: 0x00095D3C
	public static string ItemTypeNameToMeta(string itemTypeName)
	{
		string text = Regex.Replace(itemTypeName, "(?<=[a-z])([A-Z])", " $1").ToLower();
		text = new string((from w in text
		where char.IsLetterOrDigit(w) | char.IsWhiteSpace(w)
		select w).ToArray<char>());
		text = text.Replace(" ", "_");
		text = "item_type_" + text;
		return LanguageManager.Instance.GetText(text);
	}

	// Token: 0x06001E64 RID: 7780 RVA: 0x00097BB8 File Offset: 0x00095DB8
	public static string SkillCategoryToString(SkillCategory skillCategory)
	{
		string text = Regex.Replace(Enum.GetName(typeof(SkillCategory), skillCategory), "(?<=[a-z])([A-Z])", " $1").ToLower();
		text = new string((from w in text
		where char.IsLetterOrDigit(w) | char.IsWhiteSpace(w)
		select w).ToArray<char>());
		text = text.Replace(" ", "_");
		text = "skill_category_" + text;
		return LanguageManager.Instance.GetText(text);
	}

	// Token: 0x06001E65 RID: 7781 RVA: 0x00097C48 File Offset: 0x00095E48
	public static string SkillTypeToString(SkillType skillType)
	{
		string text = Regex.Replace(Enum.GetName(typeof(SkillType), skillType), "(?<=[a-z])([A-Z])", " $1").ToLower();
		text = new string((from w in text
		where char.IsLetterOrDigit(w) | char.IsWhiteSpace(w)
		select w).ToArray<char>());
		text = text.Replace(" ", "_");
		text = "skill_type_" + text;
		return LanguageManager.Instance.GetText(text);
	}

	// Token: 0x06001E66 RID: 7782 RVA: 0x00097CD8 File Offset: 0x00095ED8
	public static string ConditionCategoryToString(ConditionCategory conditionCategory)
	{
		string text = Regex.Replace(Enum.GetName(typeof(ConditionCategory), conditionCategory), "(?<=[a-z])([A-Z])", " $1");
		text = text.Replace(" ", "_");
		text = text.ToLower();
		text = "condition_category_" + text;
		return LanguageManager.Instance.GetText(text);
	}

	// Token: 0x06001E67 RID: 7783 RVA: 0x00097D3C File Offset: 0x00095F3C
	public static string RankToString(Rank rank)
	{
		switch (rank)
		{
		case Rank.None:
		case Rank.Normal:
			return string.Empty;
		case Rank.Veteran:
			return "rank_veteran";
		case Rank.Elite:
			return "rank_elite";
		case Rank.Champion:
			return "rank_champion";
		case Rank.Legendary:
			return "rank_legendary";
		case Rank.Epic:
			return "rank_epic";
		default:
			return string.Empty;
		}
	}

	// Token: 0x06001E68 RID: 7784 RVA: 0x00097D96 File Offset: 0x00095F96
	public static string ConditionTypeToString(ConditionType conditionType)
	{
		return Regex.Replace(Enum.GetName(typeof(ConditionType), conditionType), "(?<=[a-z])([A-Z])", " $1");
	}

	// Token: 0x06001E69 RID: 7785 RVA: 0x00097DBC File Offset: 0x00095FBC
	public static string ConditionCategoryToColorString(ConditionCategory conditionCategory)
	{
		switch (conditionCategory)
		{
		case ConditionCategory.Regeneration:
			return "lime";
		case ConditionCategory.Degeneration:
			return "purple";
		case ConditionCategory.Blessing:
			return "teal";
		case ConditionCategory.Curse:
		case ConditionCategory.Paralyzing:
		case ConditionCategory.Confusion:
			return "gray";
		default:
			return "white";
		}
	}

	// Token: 0x06001E6A RID: 7786 RVA: 0x00097DFB File Offset: 0x00095FFB
	public static string AttributeTypeToString(AttributeType attributeType)
	{
		return Regex.Replace(Enum.GetName(typeof(AttributeType), attributeType), "(?<=[a-z])([A-Z])", " $1");
	}

	// Token: 0x06001E6B RID: 7787 RVA: 0x00097E24 File Offset: 0x00096024
	public static Color SkillCategoryToFrameColor(SkillCategory category)
	{
		switch (category)
		{
		case SkillCategory.Attack:
		case SkillCategory.Curse:
			return new Color(255f, 0f, 0f, 255f);
		case SkillCategory.Healing:
		case SkillCategory.Blessing:
			return Color.green;
		default:
			return Color.white;
		}
	}

	// Token: 0x06001E6C RID: 7788 RVA: 0x00097E70 File Offset: 0x00096070
	public static string SkillCategoryToColorString(SkillCategory category)
	{
		switch (category)
		{
		case SkillCategory.Attack:
			return "red";
		case SkillCategory.Healing:
			return "lime";
		case SkillCategory.Blessing:
			return "teal";
		case SkillCategory.Curse:
			return "grey";
		default:
			return "white";
		}
	}

	// Token: 0x06001E6D RID: 7789 RVA: 0x00097EAC File Offset: 0x000960AC
	public static string ItemQualityToColorString(ItemQuality itemQuality)
	{
		switch (itemQuality)
		{
		case ItemQuality.Poor:
			return "silver";
		case ItemQuality.Basic:
			return "white";
		case ItemQuality.Fine:
			return "lightblue";
		case ItemQuality.Masterwork:
			return "lime";
		case ItemQuality.Ascended:
			return "orange";
		case ItemQuality.Epic:
			return "teal";
		case ItemQuality.Perfect:
			return "purple";
		case ItemQuality.Ancient:
			return "red";
		default:
			return "white";
		}
	}

	// Token: 0x06001E6E RID: 7790 RVA: 0x00097F18 File Offset: 0x00096118
	public static string RarityToColorString(Rarity rarity)
	{
		switch (rarity)
		{
		case Rarity.Common:
			return "white";
		case Rarity.Uncommon:
			return "lightblue";
		case Rarity.Rare:
			return "lime";
		case Rarity.Exotic:
			return "orange";
		case Rarity.Legendary:
			return "purple";
		case Rarity.Divine:
			return "red";
		default:
			return "white";
		}
	}

	// Token: 0x06001E6F RID: 7791 RVA: 0x00097F70 File Offset: 0x00096170
	public static float RankToPercentExperienceBonus(Rank rank)
	{
		switch (rank)
		{
		case Rank.Veteran:
			return 1f;
		case Rank.Elite:
			return 2f;
		case Rank.Champion:
			return 4f;
		case Rank.Legendary:
			return 6f;
		case Rank.Epic:
			return 10f;
		default:
			return 0f;
		}
	}

	// Token: 0x06001E70 RID: 7792 RVA: 0x00097FC0 File Offset: 0x000961C0
	public static float RankToAttributePercentBonus(Rank rank)
	{
		switch (rank)
		{
		case Rank.Veteran:
			return 0.65f;
		case Rank.Elite:
			return 0.95f;
		case Rank.Champion:
			return 1.4f;
		case Rank.Legendary:
			return 1.7f;
		case Rank.Epic:
			return 2.2f;
		default:
			return 0f;
		}
	}

	// Token: 0x06001E71 RID: 7793 RVA: 0x00098010 File Offset: 0x00096210
	public static float RankToPercentScale(Rank rank)
	{
		switch (rank)
		{
		case Rank.Veteran:
			return 0.3f;
		case Rank.Elite:
			return 0.5f;
		case Rank.Champion:
			return 0.7f;
		case Rank.Legendary:
			return 0.9f;
		case Rank.Epic:
			return 1.1f;
		default:
			return 0f;
		}
	}

	// Token: 0x06001E72 RID: 7794 RVA: 0x00098060 File Offset: 0x00096260
	public static float RankToRespawnPercentDelay(Rank rank)
	{
		switch (rank)
		{
		case Rank.Veteran:
			return 0.65f;
		case Rank.Elite:
			return 0.95f;
		case Rank.Champion:
			return 1.4f;
		case Rank.Legendary:
			return 1.7f;
		case Rank.Epic:
			return 2.2f;
		default:
			return 0f;
		}
	}

	// Token: 0x06001E73 RID: 7795 RVA: 0x000980B0 File Offset: 0x000962B0
	public static Color32 RankToColor(Rank rank)
	{
		switch (rank)
		{
		case Rank.Veteran:
			return GlobalSettings.Colors[1];
		case Rank.Elite:
			return GlobalSettings.Colors[2];
		case Rank.Champion:
			return GlobalSettings.Colors[5];
		case Rank.Legendary:
			return GlobalSettings.Colors[6];
		case Rank.Epic:
			return GlobalSettings.Colors[7];
		default:
			return Color.white;
		}
	}

	// Token: 0x06001E74 RID: 7796 RVA: 0x0009813A File Offset: 0x0009633A
	public static float EquipmentQualityToPercentDropChance(ItemQuality quality)
	{
		switch (quality)
		{
		case ItemQuality.Basic:
			return 0.002f;
		case ItemQuality.Fine:
			return 0.00045f;
		case ItemQuality.Masterwork:
			return 4.6E-05f;
		case ItemQuality.Ascended:
			return 2.3E-05f;
		default:
			return 0f;
		}
	}

	// Token: 0x06001E75 RID: 7797 RVA: 0x00098173 File Offset: 0x00096373
	public static int GetMaxQualityDropLevel(ItemQuality quality)
	{
		switch (quality)
		{
		case ItemQuality.Basic:
			return 25;
		case ItemQuality.Fine:
			return 55;
		case ItemQuality.Masterwork:
			return 105;
		case ItemQuality.Ascended:
			return 300;
		default:
			return 0;
		}
	}

	// Token: 0x06001E76 RID: 7798 RVA: 0x000981A0 File Offset: 0x000963A0
	public static float ItemRarityToPercentDropChance(Rarity rarity)
	{
		switch (rarity)
		{
		case Rarity.Common:
			return 0.04f;
		case Rarity.Uncommon:
			return 0.009f;
		case Rarity.Rare:
			return 0.0009f;
		case Rarity.Exotic:
			return 0.00023f;
		case Rarity.Legendary:
			return 9E-05f;
		case Rarity.Divine:
			return 2.3E-05f;
		default:
			return 0f;
		}
	}

	// Token: 0x06001E77 RID: 7799 RVA: 0x000981F8 File Offset: 0x000963F8
	public static Rank GenerateRandomRank()
	{
		int num = UnityEngine.Random.Range(0, 100001);
		if (num >= 99900)
		{
			return Rank.Epic;
		}
		if (num >= 99500)
		{
			return Rank.Legendary;
		}
		if (num >= 99000)
		{
			return Rank.Champion;
		}
		if (num >= 98000)
		{
			return Rank.Elite;
		}
		if (num >= 96000)
		{
			return Rank.Veteran;
		}
		return Rank.Normal;
	}

	// Token: 0x06001E78 RID: 7800 RVA: 0x00098244 File Offset: 0x00096444
	public static float BlueprintPercentDropChance(ItemQuality blueprintQuality)
	{
		switch (blueprintQuality)
		{
		case ItemQuality.Fine:
			return 0.0015f;
		case ItemQuality.Masterwork:
			return 0.00045f;
		case ItemQuality.Ascended:
			return 4.6E-05f;
		default:
			return 0f;
		}
	}

	// Token: 0x06001E79 RID: 7801 RVA: 0x00098274 File Offset: 0x00096474
	public static float GetSkillEnchantPercentSuccessChance(int enchantLevel)
	{
		switch (enchantLevel)
		{
		case 1:
			return 0.95f;
		case 2:
			return 0.83f;
		case 3:
			return 0.69f;
		case 4:
			return 0.5f;
		case 5:
			return 0.31f;
		case 6:
			return 0.27f;
		case 7:
			return 0.22f;
		case 8:
			return 0.17f;
		case 9:
			return 0.13f;
		case 10:
			return 0.08f;
		default:
			return 0f;
		}
	}

	// Token: 0x06001E7A RID: 7802 RVA: 0x000982F4 File Offset: 0x000964F4
	public static float GetItemBoostPercentSuccessChance(int boostLevel)
	{
		switch (boostLevel)
		{
		case 1:
		case 2:
		case 3:
			return 0.95f;
		case 4:
		case 5:
		case 6:
			return 0.83f;
		case 7:
		case 8:
		case 9:
			return 0.69f;
		case 10:
		case 11:
		case 12:
			return 0.5f;
		case 13:
		case 14:
		case 15:
			return 0.31f;
		case 16:
		case 17:
		case 18:
			return 0.17f;
		case 19:
		case 20:
		case 21:
			return 0.05f;
		default:
			return 0f;
		}
	}

	// Token: 0x06001E7B RID: 7803 RVA: 0x0009838E File Offset: 0x0009658E
	public static bool IsClose(Vector2 fromPosition, Vector2 toPosition)
	{
		return GlobalUtils.IsClose(fromPosition, toPosition, 0.32f);
	}

	// Token: 0x06001E7C RID: 7804 RVA: 0x0009839C File Offset: 0x0009659C
	public static bool IsClose(Vector2 fromPosition, Vector2 toPosition, float closeDistance)
	{
		return Vector2.Distance(fromPosition, toPosition) < closeDistance;
	}

	// Token: 0x06001E7D RID: 7805 RVA: 0x000983A8 File Offset: 0x000965A8
	public static float CalculateMaxTvtDamage(Vocation targetVocation)
	{
		switch (targetVocation)
		{
		case Vocation.Warrior:
			return 0.035f;
		case Vocation.Protector:
			return 0.025f;
		case Vocation.Lyrus:
			return 0.04f;
		}
		return 0.05f;
	}

	// Token: 0x06001E7E RID: 7806 RVA: 0x000983DC File Offset: 0x000965DC
	public static GameObject SpawnMonster(GameObject monsterPrefab, int monsterId, Vector3 position, Rank rank, bool allowRespawn, bool addToSpawnPoints)
	{
		GameObject result;
		try
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(monsterPrefab, position, Quaternion.identity);
			MovementModule component = gameObject.GetComponent<MovementModule>();
			if (component != null)
			{
				component.SetSpawnPointLocation(position, false);
			}
			if (rank != Rank.None)
			{
				CreatureModule component2 = gameObject.GetComponent<CreatureModule>();
				if (component2 != null)
				{
					component2.SetRank(rank);
				}
			}
			MonsterModule component3 = gameObject.GetComponent<MonsterModule>();
			if (component3 != null)
			{
				component3.SetMonsterId(monsterId);
				component3.SetAllowRespawn(allowRespawn);
				component3.LoadMonsterData();
				if (addToSpawnPoints)
				{
					GameEnvironmentModule.MonsterSpawnPoints.Add(new MonsterSpawnPoint(monsterId, rank, position, gameObject));
				}
			}
			NetworkServer.Spawn(gameObject, null);
			result = gameObject;
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.ToString());
			result = null;
		}
		return result;
	}

	// Token: 0x06001E7F RID: 7807 RVA: 0x00098494 File Offset: 0x00096694
	public static GameObject SpawnNpc(GameObject npcPrefab, NpcDatabaseModule npcDatabaseModule, int npcId, Vector3 position)
	{
		GameObject result;
		try
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(npcPrefab, position, Quaternion.identity);
			MovementModule component = gameObject.GetComponent<MovementModule>();
			if (component != null)
			{
				component.SetSpawnPointLocation(position, false);
			}
			NpcModule component2 = gameObject.GetComponent<NpcModule>();
			if (component2 != null)
			{
				component2.SetNpcId(npcId);
				component2.LoadNpcData();
				Npc npc = npcDatabaseModule.GetNpc(npcId);
				gameObject.GetComponent<CreatureModule>();
				GameEnvironmentModule.NpcSpawnPoints.Add(new NpcSpawnPoint(npcId, npc.Name, position, gameObject));
			}
			result = gameObject;
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
			result = null;
		}
		return result;
	}

	// Token: 0x06001E80 RID: 7808 RVA: 0x00098530 File Offset: 0x00096730
	public static string GetMd5Hash(string input)
	{
		string result;
		using (MD5 md = MD5.Create())
		{
			byte[] array = md.ComputeHash(Encoding.UTF8.GetBytes(input));
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("x2"));
			}
			result = stringBuilder.ToString();
		}
		return result;
	}

	// Token: 0x06001E81 RID: 7809 RVA: 0x000985A8 File Offset: 0x000967A8
	public static string GetLanguageFile(Language language)
	{
		switch (language)
		{
		case Language.English:
			return "english.json";
		case Language.Portuguese:
			return "portuguese.json";
		case Language.Spanish:
			return "spanish.json";
		case Language.French:
			return "french.json";
		default:
			return "english.json";
		}
	}

	// Token: 0x06001E82 RID: 7810 RVA: 0x000985DF File Offset: 0x000967DF
	public static float GetMainCameraDistance(CameraMode mode, bool mapMode)
	{
		if (mapMode)
		{
			return 20f;
		}
		switch (mode)
		{
		case CameraMode.Near:
			return 2.88f;
		case CameraMode.Normal:
			return 3.84f;
		case CameraMode.Far:
			return 5.3f;
		default:
			return 3.84f;
		}
	}

	// Token: 0x06001E83 RID: 7811 RVA: 0x00098618 File Offset: 0x00096818
	public static string GetEnglishPlural(string word)
	{
		if (string.IsNullOrEmpty(word))
		{
			return word;
		}
		if (word.ToLower().EndsWith("s") | word.ToLower().EndsWith("ss") | word.ToLower().EndsWith("ch") | word.ToLower().EndsWith("sh") | word.ToLower().EndsWith("x") | word.ToLower().EndsWith("z") | word.ToLower().EndsWith("o"))
		{
			return word + "es";
		}
		if (word.ToLower().EndsWith("y"))
		{
			return word.Substring(0, word.Length - 1) + "ies";
		}
		if (word.ToLower().EndsWith("f"))
		{
			return word.Substring(0, word.Length - 1) + "ves";
		}
		if (word.ToLower().EndsWith("fe"))
		{
			return word.Substring(0, word.Length - 2) + "ves";
		}
		return word + "s";
	}

	// Token: 0x06001E84 RID: 7812 RVA: 0x00098744 File Offset: 0x00096944
	public static string SpliceWords(string text, int lineLength)
	{
		string text2 = string.Empty;
		string[] array = text.Split(new string[]
		{
			" "
		}, StringSplitOptions.RemoveEmptyEntries);
		int num = 0;
		foreach (string text3 in array)
		{
			text2 = text2 + text3 + " ";
			num += text3.Length;
			if (num >= lineLength)
			{
				text2 += Environment.NewLine;
				num = 0;
			}
		}
		return text2.Trim();
	}

	// Token: 0x06001E85 RID: 7813 RVA: 0x000987B4 File Offset: 0x000969B4
	public static float GetUISizeModifier(UiSize uiSize)
	{
		switch (uiSize)
		{
		case UiSize.Tiny:
			return 1.3f;
		case UiSize.VerySmall:
			return 1.2f;
		case UiSize.Small:
			return 1.1f;
		case UiSize.Medium:
			return 1f;
		case UiSize.Big:
			return 0.9f;
		case UiSize.VeryBig:
			return 0.8f;
		case UiSize.Huge:
			return 0.7f;
		default:
			return 1f;
		}
	}

	// Token: 0x06001E86 RID: 7814 RVA: 0x00098814 File Offset: 0x00096A14
	public static string GetUiSizeText(string uiSize)
	{
		uint num = <PrivateImplementationDetails>.ComputeStringHash(uiSize);
		if (num <= 2323542581U)
		{
			if (num != 163471254U)
			{
				if (num != 1216374316U)
				{
					if (num == 2323542581U)
					{
						if (uiSize == "VeryBig")
						{
							return "120%";
						}
					}
				}
				else if (uiSize == "Small")
				{
					return "90%";
				}
			}
			else if (uiSize == "Medium")
			{
				return "100%";
			}
		}
		else if (num <= 3283330104U)
		{
			if (num != 3100729689U)
			{
				if (num == 3283330104U)
				{
					if (uiSize == "VerySmall")
					{
						return "80%";
					}
				}
			}
			else if (uiSize == "Big")
			{
				return "110%";
			}
		}
		else if (num != 3822885771U)
		{
			if (num == 3887788930U)
			{
				if (uiSize == "Huge")
				{
					return "130%";
				}
			}
		}
		else if (uiSize == "Tiny")
		{
			return "70%";
		}
		return "100%";
	}

	// Token: 0x06001E87 RID: 7815 RVA: 0x00098918 File Offset: 0x00096B18
	public static string ColorIdToColorTag(int colorId)
	{
		switch (colorId)
		{
		case 1:
			return "green";
		case 2:
			return "orange";
		case 3:
			return "red";
		case 4:
			return "blue";
		case 6:
			return "lightblue";
		}
		return string.Empty;
	}

	// Token: 0x06001E88 RID: 7816 RVA: 0x0009896A File Offset: 0x00096B6A
	public static bool PackageHasEliteAccess(PackageType packageType)
	{
		return packageType > PackageType.Veteran && packageType - PackageType.Elite <= 1;
	}

	// Token: 0x06001E89 RID: 7817 RVA: 0x0009897D File Offset: 0x00096B7D
	public static bool PackageHasVeteranAccess(PackageType packageType)
	{
		return packageType != PackageType.Free && packageType - PackageType.Veteran <= 2;
	}

	// Token: 0x06001E8A RID: 7818 RVA: 0x00098990 File Offset: 0x00096B90
	public static Vector3 GetLocationFromSpawnPoint(string spawnPoint)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("SpawnPoint");
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].name == spawnPoint)
			{
				return array[i].transform.position;
			}
		}
		return Vector3.zero;
	}

	// Token: 0x06001E8B RID: 7819 RVA: 0x000989DC File Offset: 0x00096BDC
	public static int CalculateAndUseTvtDamageIfNeeded(int originalDamage, float skillPower, GameObject attacker, GameObject target)
	{
		if (attacker == null)
		{
			return originalDamage;
		}
		if (!attacker.CompareTag("Player"))
		{
			return originalDamage;
		}
		if (target == null)
		{
			return originalDamage;
		}
		if (!target.CompareTag("Player"))
		{
			return originalDamage;
		}
		PvpModule pvpModule;
		if (!attacker.TryGetComponent<PvpModule>(out pvpModule))
		{
			return originalDamage;
		}
		if (pvpModule.TvtTeam == TvtTeam.None)
		{
			return originalDamage;
		}
		PvpModule pvpModule2;
		if (!target.TryGetComponent<PvpModule>(out pvpModule2))
		{
			return 0;
		}
		if (pvpModule2.TvtTeam == TvtTeam.None)
		{
			return 0;
		}
		ConditionModule conditionModule;
		target.TryGetComponent<ConditionModule>(out conditionModule);
		if (conditionModule.HasActiveCondition(ConditionCategory.Invincibility))
		{
			return 0;
		}
		float num = skillPower * 0.001f;
		VocationModule vocationModule;
		target.TryGetComponent<VocationModule>(out vocationModule);
		num = Mathf.Min(GlobalUtils.CalculateMaxTvtDamage(vocationModule.Vocation), num);
		AttributeModule attributeModule;
		target.TryGetComponent<AttributeModule>(out attributeModule);
		int num2 = Mathf.RoundToInt((float)attributeModule.MaxHealth * num);
		pvpModule.TeamFightDamage += num2;
		return num2;
	}

	// Token: 0x06001E8C RID: 7820 RVA: 0x00098AAC File Offset: 0x00096CAC
	public static int GetMinRequiredLevelForQuality(ItemQuality quality)
	{
		switch (quality)
		{
		case ItemQuality.Poor:
		case ItemQuality.Basic:
			return 1;
		case ItemQuality.Fine:
			return 8;
		case ItemQuality.Masterwork:
			return 25;
		case ItemQuality.Ascended:
			return 55;
		case ItemQuality.Epic:
			return 105;
		case ItemQuality.Perfect:
			return 200;
		case ItemQuality.Ancient:
			return 400;
		default:
			return 1;
		}
	}

	// Token: 0x06001E8D RID: 7821 RVA: 0x00098AFB File Offset: 0x00096CFB
	public static Rarity GetMaxRarityForQuality(ItemQuality quality)
	{
		switch (quality)
		{
		case ItemQuality.Poor:
			return Rarity.Common;
		case ItemQuality.Basic:
			return Rarity.Uncommon;
		case ItemQuality.Fine:
		case ItemQuality.Masterwork:
			return Rarity.Rare;
		case ItemQuality.Ascended:
		case ItemQuality.Epic:
			return Rarity.Exotic;
		case ItemQuality.Perfect:
			return Rarity.Legendary;
		case ItemQuality.Ancient:
			return Rarity.Divine;
		default:
			return Rarity.Common;
		}
	}

	// Token: 0x06001E8E RID: 7822 RVA: 0x00098B34 File Offset: 0x00096D34
	public static bool IsValidPlayerName(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return false;
		}
		if (string.IsNullOrWhiteSpace(name))
		{
			return false;
		}
		if (name.Contains("  "))
		{
			return false;
		}
		return (from n in name
		where n == ' '
		select n).Count<char>() <= 4 && new Regex("^[a-zA-Z0-9\\s_-]*$").IsMatch(name);
	}

	// Token: 0x06001E8F RID: 7823 RVA: 0x00098BA4 File Offset: 0x00096DA4
	public static bool IsValidGuildName(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return false;
		}
		if (string.IsNullOrWhiteSpace(name))
		{
			return false;
		}
		if (name.Contains("  "))
		{
			return false;
		}
		return (from n in name
		where n == ' '
		select n).Count<char>() <= 4 && name.Length <= 32 && new Regex("^[a-zA-Z0-9\\s_-]*$").IsMatch(name);
	}

	// Token: 0x06001E90 RID: 7824 RVA: 0x00098C20 File Offset: 0x00096E20
	public static bool CanShareRewards(int creature1BaseLevel, int creature2BaseLevel, int creature2MasteryLevel)
	{
		int num = Mathf.Abs(creature1BaseLevel - creature2BaseLevel);
		int num2 = 30 + creature2MasteryLevel * 6;
		return num <= num2;
	}

	// Token: 0x06001E91 RID: 7825 RVA: 0x00098C42 File Offset: 0x00096E42
	public static bool CanStartPvpCombat(int creature1BaseLevel, int creature2BaseLevel, int creature2MasteryLevel)
	{
		return (creature1BaseLevel >= 200 & creature2BaseLevel >= 200) || GlobalUtils.CanShareRewards(creature1BaseLevel, creature2BaseLevel, creature2MasteryLevel);
	}

	// Token: 0x06001E92 RID: 7826 RVA: 0x00098C67 File Offset: 0x00096E67
	public static float GetDistinctPartySizeBonus(int distinctPartySize)
	{
		if (distinctPartySize < 3)
		{
			return 0.7f;
		}
		if (distinctPartySize == 3)
		{
			return 0.8f;
		}
		if (distinctPartySize == 4)
		{
			return 0.9f;
		}
		if (distinctPartySize >= 5)
		{
			return 1f;
		}
		return 0.7f;
	}

	// Token: 0x06001E93 RID: 7827 RVA: 0x00098C98 File Offset: 0x00096E98
	public static void Shuffle<T>(this List<T> list)
	{
		System.Random random = new System.Random();
		int i = list.Count;
		while (i > 1)
		{
			i--;
			int index = random.Next(i + 1);
			T value = list[index];
			list[index] = list[i];
			list[i] = value;
		}
	}

	// Token: 0x06001E94 RID: 7828 RVA: 0x00098CE4 File Offset: 0x00096EE4
	public static string FormatLongNumber(long input, string cultureName)
	{
		CultureInfo provider = new CultureInfo(cultureName);
		return input.ToString("N0", provider);
	}
}
