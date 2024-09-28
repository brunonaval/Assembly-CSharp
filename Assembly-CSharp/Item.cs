using System;
using System.Linq;
using System.Text;
using Mirror;
using UnityEngine;

// Token: 0x0200010F RID: 271
public struct Item
{
	// Token: 0x060002B4 RID: 692 RVA: 0x000126E4 File Offset: 0x000108E4
	public Item(int slotPosition)
	{
		this.Id = 0;
		this.Name = null;
		this._attack = 0;
		this._defense = 0;
		this._value = 0;
		this._rarity = Rarity.Undefined;
		this.Script = null;
		this.Category = ItemCategory.Undefined;
		this.SlotType = SlotType.Undefined;
		this.Type = ItemType.Undefined;
		this.Soulbind = false;
		this.Sellable = false;
		this.Stackable = false;
		this.TwoHanded = false;
		this.AllowDropByLevel = false;
		this.DropByLevelChance = 0f;
		this.AllowUseWhileDead = false;
		this.TeleportSpawnPoint = null;
		this.Description = null;
		this.BlueprintId = 0;
		this.Projectile = default(Projectile);
		this._requiredLevel = 0;
		this.Quality = ItemQuality.Poor;
		this.ConsumeProjectile = false;
		this.RequiredVocation = Vocation.Undefined;
		this.SkillId = 0;
		this.ProjectileShootSoundEffectName = null;
		this.ProjectileExplosionSoundEffectName = null;
		this.BlueprintRequiredProfession = PlayerProfession.None;
		this.BlueprintRequiredProfessionLevel = 0;
		this.WeaponSkinAnimationType = AnimationType.Undefined;
		this.BoostLevel = 0;
		this.OwnerId = 0;
		this.OwnerName = null;
		this.TextColorId = 2;
		this._icon = null;
		this._fullDescription = null;
		this._metaName = null;
		this._uiSystemModule = null;
		this.Amount = 0;
		this.SlotPosition = slotPosition;
		this.AllowDropFromChests = false;
		this.IgnoreQualityRestrictions = false;
	}

	// Token: 0x060002B5 RID: 693 RVA: 0x0001282C File Offset: 0x00010A2C
	public Item(ApiItem apiItem, PlayerProfession blueprintRequiredProfession, int blueprintRequiredProfessionLevel)
	{
		this.Id = apiItem.Id;
		this.Name = apiItem.Name;
		this._requiredLevel = apiItem.RequiredLevel;
		this.RequiredVocation = apiItem.RequiredVocation;
		this._attack = apiItem.BaseAttack;
		this._defense = apiItem.BaseDefense;
		this._value = apiItem.BaseValue;
		this._rarity = apiItem.Rarity;
		this.Script = apiItem.Script;
		this.Category = apiItem.Category;
		this.SlotType = apiItem.SlotType;
		this.Type = apiItem.Type;
		this.Soulbind = apiItem.Soulbind;
		this.Sellable = apiItem.Sellable;
		this.TwoHanded = apiItem.TwoHanded;
		this.Stackable = apiItem.Stackable;
		this.BoostLevel = apiItem.BoostLevel;
		this.BlueprintId = apiItem.BlueprintId;
		this.AllowDropByLevel = apiItem.AllowDropByLevel;
		this.DropByLevelChance = 0f;
		this.AllowUseWhileDead = apiItem.AllowUseWhileDead;
		this.Description = apiItem.Description;
		this.Projectile = apiItem.Projectile;
		this.ConsumeProjectile = apiItem.ConsumeProjectile;
		this.Quality = apiItem.Quality;
		this.SkillId = apiItem.SkillId;
		this.TeleportSpawnPoint = apiItem.TeleportSpawnPoint;
		this.WeaponSkinAnimationType = AnimationType.Undefined;
		this.ProjectileShootSoundEffectName = apiItem.ProjectileShootSoundEffectName;
		this.ProjectileExplosionSoundEffectName = apiItem.ProjectileExplosionSoundEffectName;
		this.BlueprintRequiredProfession = blueprintRequiredProfession;
		this.BlueprintRequiredProfessionLevel = blueprintRequiredProfessionLevel;
		this.OwnerId = 0;
		this.OwnerName = null;
		this.TextColorId = apiItem.TextColorId;
		this._icon = null;
		this._fullDescription = null;
		this._metaName = apiItem.MetaName;
		this._uiSystemModule = null;
		this.Amount = 1;
		this.SlotPosition = 0;
		this.AllowDropFromChests = apiItem.AllowDropFromChests;
		this.IgnoreQualityRestrictions = apiItem.IgnoreQualityRestrictions;
	}

	// Token: 0x060002B6 RID: 694 RVA: 0x00012A0C File Offset: 0x00010C0C
	public Item(int id, string name, int requiredLevel, Vocation requiredVocation, short attack, short defense, int value, ItemQuality quality, Rarity rarity, Projectile projectile, ItemCategory category, SlotType slotType, ItemType type, bool stackable, bool soulbind, bool sellable, bool twoHanded, bool allowDropByLevel, float dropByLevelChance, bool allowUseWhileDead, bool consumeProjectile, string description, string metaName, int skillId, string projectileShootSoundEffectName, string projectileExplosionSoundEffectName, int blueprintId, int textColorId, string script, string teleportSpawnPoint, bool allowDropFromChests, PlayerProfession blueprintRequiredProfession, int blueprintRequiredProfessionLevel, bool ignoreQualityRestrictions, AnimationType weaponSkinAnimationType)
	{
		this.Id = id;
		this.Name = name;
		this._requiredLevel = requiredLevel;
		this.RequiredVocation = requiredVocation;
		this._attack = attack;
		this._defense = defense;
		this._value = value;
		this._rarity = rarity;
		this.Script = script;
		this.Category = category;
		this.SlotType = slotType;
		this.Type = type;
		this.Soulbind = soulbind;
		this.Sellable = sellable;
		this.TwoHanded = twoHanded;
		this.Stackable = stackable;
		this.AllowDropByLevel = allowDropByLevel;
		this.DropByLevelChance = dropByLevelChance;
		this.AllowUseWhileDead = allowUseWhileDead;
		this.Description = description;
		this.Projectile = projectile;
		this.ConsumeProjectile = consumeProjectile;
		this.Quality = quality;
		this.SkillId = skillId;
		this.BlueprintId = blueprintId;
		this.TeleportSpawnPoint = teleportSpawnPoint;
		this.ProjectileShootSoundEffectName = projectileShootSoundEffectName;
		this.ProjectileExplosionSoundEffectName = projectileExplosionSoundEffectName;
		this.BlueprintRequiredProfession = blueprintRequiredProfession;
		this.BlueprintRequiredProfessionLevel = blueprintRequiredProfessionLevel;
		this.OwnerId = 0;
		this.BoostLevel = 0;
		this.OwnerName = null;
		this.TextColorId = textColorId;
		this._icon = null;
		this._fullDescription = null;
		this._metaName = metaName;
		this._uiSystemModule = null;
		this.Amount = 1;
		this.SlotPosition = 0;
		this.AllowDropFromChests = allowDropFromChests;
		this.IgnoreQualityRestrictions = ignoreQualityRestrictions;
		this.WeaponSkinAnimationType = weaponSkinAnimationType;
	}

	// Token: 0x17000038 RID: 56
	// (get) Token: 0x060002B7 RID: 695 RVA: 0x00012B66 File Offset: 0x00010D66
	// (set) Token: 0x060002B8 RID: 696 RVA: 0x00012B6E File Offset: 0x00010D6E
	public string Script { readonly get; set; }

	// Token: 0x17000039 RID: 57
	// (get) Token: 0x060002B9 RID: 697 RVA: 0x00012B77 File Offset: 0x00010D77
	// (set) Token: 0x060002BA RID: 698 RVA: 0x00012B7F File Offset: 0x00010D7F
	public int TextColorId { readonly get; set; }

	// Token: 0x1700003A RID: 58
	// (get) Token: 0x060002BB RID: 699 RVA: 0x00012B88 File Offset: 0x00010D88
	// (set) Token: 0x060002BC RID: 700 RVA: 0x00012B90 File Offset: 0x00010D90
	public bool AllowDropByLevel { readonly get; set; }

	// Token: 0x1700003B RID: 59
	// (get) Token: 0x060002BD RID: 701 RVA: 0x00012B99 File Offset: 0x00010D99
	// (set) Token: 0x060002BE RID: 702 RVA: 0x00012BA1 File Offset: 0x00010DA1
	public float DropByLevelChance { readonly get; set; }

	// Token: 0x1700003C RID: 60
	// (get) Token: 0x060002BF RID: 703 RVA: 0x00012BAA File Offset: 0x00010DAA
	// (set) Token: 0x060002C0 RID: 704 RVA: 0x00012BB2 File Offset: 0x00010DB2
	public Projectile Projectile { readonly get; set; }

	// Token: 0x1700003D RID: 61
	// (get) Token: 0x060002C1 RID: 705 RVA: 0x00012BBB File Offset: 0x00010DBB
	// (set) Token: 0x060002C2 RID: 706 RVA: 0x00012BC3 File Offset: 0x00010DC3
	public bool AllowUseWhileDead { readonly get; set; }

	// Token: 0x1700003E RID: 62
	// (get) Token: 0x060002C3 RID: 707 RVA: 0x00012BCC File Offset: 0x00010DCC
	// (set) Token: 0x060002C4 RID: 708 RVA: 0x00012BD4 File Offset: 0x00010DD4
	public bool ConsumeProjectile { readonly get; set; }

	// Token: 0x1700003F RID: 63
	// (get) Token: 0x060002C5 RID: 709 RVA: 0x00012BDD File Offset: 0x00010DDD
	// (set) Token: 0x060002C6 RID: 710 RVA: 0x00012BE5 File Offset: 0x00010DE5
	public bool AllowDropFromChests { readonly get; set; }

	// Token: 0x17000040 RID: 64
	// (get) Token: 0x060002C7 RID: 711 RVA: 0x00012BEE File Offset: 0x00010DEE
	// (set) Token: 0x060002C8 RID: 712 RVA: 0x00012BF6 File Offset: 0x00010DF6
	public string TeleportSpawnPoint { readonly get; set; }

	// Token: 0x17000041 RID: 65
	// (get) Token: 0x060002C9 RID: 713 RVA: 0x00012BFF File Offset: 0x00010DFF
	// (set) Token: 0x060002CA RID: 714 RVA: 0x00012C07 File Offset: 0x00010E07
	public AnimationType WeaponSkinAnimationType { readonly get; set; }

	// Token: 0x17000042 RID: 66
	// (get) Token: 0x060002CB RID: 715 RVA: 0x00012C10 File Offset: 0x00010E10
	// (set) Token: 0x060002CC RID: 716 RVA: 0x00012C18 File Offset: 0x00010E18
	public string ProjectileShootSoundEffectName { readonly get; set; }

	// Token: 0x17000043 RID: 67
	// (get) Token: 0x060002CD RID: 717 RVA: 0x00012C21 File Offset: 0x00010E21
	// (set) Token: 0x060002CE RID: 718 RVA: 0x00012C29 File Offset: 0x00010E29
	public string ProjectileExplosionSoundEffectName { readonly get; set; }

	// Token: 0x17000044 RID: 68
	// (get) Token: 0x060002CF RID: 719 RVA: 0x00012C34 File Offset: 0x00010E34
	public string UniqueId
	{
		get
		{
			return string.Format("{0}_{1}_{2}_{3}_{4}", new object[]
			{
				this.Id,
				this.RequiredLevel,
				this.BoostLevel,
				this.Rarity,
				this.Quality
			});
		}
	}

	// Token: 0x17000045 RID: 69
	// (get) Token: 0x060002D0 RID: 720 RVA: 0x00012C97 File Offset: 0x00010E97
	public bool IsDefined
	{
		get
		{
			return !string.IsNullOrEmpty(this.Name);
		}
	}

	// Token: 0x17000046 RID: 70
	// (get) Token: 0x060002D1 RID: 721 RVA: 0x00012CA7 File Offset: 0x00010EA7
	public float BoostLevelPercentBonus
	{
		get
		{
			return this.GetBoostPercentBonus(this.BoostLevel);
		}
	}

	// Token: 0x17000047 RID: 71
	// (get) Token: 0x060002D2 RID: 722 RVA: 0x00012CB5 File Offset: 0x00010EB5
	public float NextBoostLevelPercentBonus
	{
		get
		{
			return this.GetBoostPercentBonus(this.BoostLevel + 1);
		}
	}

	// Token: 0x17000048 RID: 72
	// (get) Token: 0x060002D3 RID: 723 RVA: 0x00012CC5 File Offset: 0x00010EC5
	public bool UseOnHands
	{
		get
		{
			return this.SlotType == SlotType.LeftHand | this.SlotType == SlotType.RightHand;
		}
	}

	// Token: 0x17000049 RID: 73
	// (get) Token: 0x060002D4 RID: 724 RVA: 0x00012CDA File Offset: 0x00010EDA
	public bool IsItemBooster
	{
		get
		{
			return this.Type == ItemType.SacredItemBooster | this.Type == ItemType.ItemBooster;
		}
	}

	// Token: 0x1700004A RID: 74
	// (get) Token: 0x060002D5 RID: 725 RVA: 0x00012CF1 File Offset: 0x00010EF1
	public bool IsToolkit
	{
		get
		{
			return this.Type == ItemType.Toolkit | this.Type == ItemType.AdvancedToolkit;
		}
	}

	// Token: 0x1700004B RID: 75
	// (get) Token: 0x060002D6 RID: 726 RVA: 0x00012D08 File Offset: 0x00010F08
	public bool Equipable
	{
		get
		{
			return this.Category == ItemCategory.Acessory | this.Category == ItemCategory.Armor | this.Category == ItemCategory.Projectile | this.Category == ItemCategory.Weapon;
		}
	}

	// Token: 0x1700004C RID: 76
	// (get) Token: 0x060002D7 RID: 727 RVA: 0x00012D34 File Offset: 0x00010F34
	public string PluralName
	{
		get
		{
			if (string.IsNullOrEmpty(this.Name))
			{
				return this.Name;
			}
			if (this.Name.ToLower().EndsWith("s") | this.Name.ToLower().EndsWith("ss") | this.Name.ToLower().EndsWith("ch") | this.Name.ToLower().EndsWith("sh") | this.Name.ToLower().EndsWith("x") | this.Name.ToLower().EndsWith("z") | this.Name.ToLower().EndsWith("o"))
			{
				return this.Name + "es";
			}
			if (this.Name.ToLower().EndsWith("y"))
			{
				return this.Name.Substring(0, this.Name.Length - 1) + "ies";
			}
			if (this.Name.ToLower().EndsWith("f"))
			{
				return this.Name.Substring(0, this.Name.Length - 1) + "ves";
			}
			if (this.Name.ToLower().EndsWith("fe"))
			{
				return this.Name.Substring(0, this.Name.Length - 2) + "ves";
			}
			return this.Name + "s";
		}
	}

	// Token: 0x1700004D RID: 77
	// (get) Token: 0x060002D8 RID: 728 RVA: 0x00012EC4 File Offset: 0x000110C4
	public string MetaName
	{
		get
		{
			if (!string.IsNullOrEmpty(this._metaName))
			{
				return this._metaName;
			}
			if (string.IsNullOrEmpty(this.Name))
			{
				return null;
			}
			string text = this.Name.ToLower();
			if (text.Contains("item_"))
			{
				return text.Replace("item_", "");
			}
			text = new string((from w in text
			where char.IsLetterOrDigit(w) | char.IsWhiteSpace(w)
			select w).ToArray<char>());
			return text.Replace(" ", "_");
		}
	}

	// Token: 0x1700004E RID: 78
	// (get) Token: 0x060002D9 RID: 729 RVA: 0x00012F60 File Offset: 0x00011160
	public Sprite Icon
	{
		get
		{
			if (NetworkServer.active)
			{
				return this._icon;
			}
			if (this._icon == null && !string.IsNullOrEmpty(this.MetaName))
			{
				this._icon = AssetBundleManager.Instance.GetItemIconSprite(this.MetaName);
			}
			if (this._icon == null)
			{
				this._icon = AssetBundleManager.Instance.GetItemIconSprite("undefined_icon");
			}
			return this._icon;
		}
	}

	// Token: 0x1700004F RID: 79
	// (get) Token: 0x060002DA RID: 730 RVA: 0x00012FD8 File Offset: 0x000111D8
	// (set) Token: 0x060002DB RID: 731 RVA: 0x00013054 File Offset: 0x00011254
	public short Attack
	{
		get
		{
			if (this.Category != ItemCategory.Weapon & this.Type != ItemType.Arrow)
			{
				return this._attack;
			}
			float num = (float)this._attack + (float)this._attack * (0.0295f * (float)(this.RequiredLevel - 1)) + (float)this._attack * this.RarityPercentBonus + (float)this._attack * this.ItemTypePercentBonus;
			return (short)Mathf.CeilToInt(num + num * this.BoostLevelPercentBonus);
		}
		set
		{
			this._attack = value;
		}
	}

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x060002DC RID: 732 RVA: 0x00013060 File Offset: 0x00011260
	// (set) Token: 0x060002DD RID: 733 RVA: 0x000130E8 File Offset: 0x000112E8
	public short Defense
	{
		get
		{
			if (this.Category != ItemCategory.Acessory & this.Category != ItemCategory.Armor & this.Category != ItemCategory.Weapon)
			{
				return this._defense;
			}
			float num = (float)this._defense + (float)this._defense * (0.0295f * (float)(this.RequiredLevel - 1)) + (float)this._defense * this.RarityPercentBonus + (float)this._defense * this.ItemTypePercentBonus;
			return (short)Mathf.CeilToInt(num + num * this.BoostLevelPercentBonus);
		}
		set
		{
			this._defense = value;
		}
	}

	// Token: 0x17000051 RID: 81
	// (get) Token: 0x060002DE RID: 734 RVA: 0x000130F4 File Offset: 0x000112F4
	public float ArcaneProtectionChance
	{
		get
		{
			if (this.Type != ItemType.Grimoire)
			{
				return 0f;
			}
			switch (this.Quality)
			{
			case ItemQuality.Poor:
				return 0f;
			case ItemQuality.Basic:
				return 0.01f;
			case ItemQuality.Fine:
				return 0.02f;
			case ItemQuality.Masterwork:
				return 0.03f;
			case ItemQuality.Ascended:
				return 0.05f;
			case ItemQuality.Epic:
				return 0.07f;
			case ItemQuality.Perfect:
				return 0.1f;
			case ItemQuality.Ancient:
				return 0.2f;
			default:
				return 0f;
			}
		}
	}

	// Token: 0x17000052 RID: 82
	// (get) Token: 0x060002DF RID: 735 RVA: 0x00013178 File Offset: 0x00011378
	public float BlockChance
	{
		get
		{
			if (this.Type != ItemType.Shield)
			{
				return 0f;
			}
			switch (this.Quality)
			{
			case ItemQuality.Poor:
				return 0f;
			case ItemQuality.Basic:
				return 0.01f;
			case ItemQuality.Fine:
				return 0.02f;
			case ItemQuality.Masterwork:
				return 0.03f;
			case ItemQuality.Ascended:
				return 0.05f;
			case ItemQuality.Epic:
				return 0.07f;
			case ItemQuality.Perfect:
				return 0.1f;
			case ItemQuality.Ancient:
				return 0.2f;
			default:
				return 0f;
			}
		}
	}

	// Token: 0x17000053 RID: 83
	// (get) Token: 0x060002E0 RID: 736 RVA: 0x000131F8 File Offset: 0x000113F8
	// (set) Token: 0x060002E1 RID: 737 RVA: 0x0001322C File Offset: 0x0001142C
	public Rarity Rarity
	{
		get
		{
			if (this.IgnoreQualityRestrictions)
			{
				return this._rarity;
			}
			Rarity maxRarityForQuality = GlobalUtils.GetMaxRarityForQuality(this.Quality);
			return (Rarity)Mathf.Min((int)this._rarity, (int)maxRarityForQuality);
		}
		set
		{
			this._rarity = value;
		}
	}

	// Token: 0x17000054 RID: 84
	// (get) Token: 0x060002E2 RID: 738 RVA: 0x00013235 File Offset: 0x00011435
	// (set) Token: 0x060002E3 RID: 739 RVA: 0x0001325C File Offset: 0x0001145C
	public int RequiredLevel
	{
		get
		{
			if (this.IgnoreQualityRestrictions)
			{
				return this._requiredLevel;
			}
			return Mathf.Max(GlobalUtils.GetMinRequiredLevelForQuality(this.Quality), this._requiredLevel);
		}
		set
		{
			this._requiredLevel = value;
		}
	}

	// Token: 0x17000055 RID: 85
	// (get) Token: 0x060002E4 RID: 740 RVA: 0x00013268 File Offset: 0x00011468
	public float RarityPercentBonus
	{
		get
		{
			float result = 0f;
			switch (this.Rarity)
			{
			case Rarity.Uncommon:
				result = 0.25f;
				break;
			case Rarity.Rare:
				result = 0.35f;
				break;
			case Rarity.Exotic:
				result = 0.535f;
				break;
			case Rarity.Legendary:
				result = 0.785f;
				break;
			case Rarity.Divine:
				result = 1.1f;
				break;
			}
			return result;
		}
	}

	// Token: 0x17000056 RID: 86
	// (get) Token: 0x060002E5 RID: 741 RVA: 0x000132C7 File Offset: 0x000114C7
	// (set) Token: 0x060002E6 RID: 742 RVA: 0x00013302 File Offset: 0x00011502
	public int Value
	{
		get
		{
			if (!this.Equipable)
			{
				return this._value;
			}
			return Mathf.RoundToInt((float)this._value + (float)this._value * this.RarityPercentBonus + (float)this._value * this.ItemLevelPercentBonus);
		}
		set
		{
			this._value = value;
		}
	}

	// Token: 0x17000057 RID: 87
	// (get) Token: 0x060002E7 RID: 743 RVA: 0x0001330C File Offset: 0x0001150C
	public float ItemTypePercentBonus
	{
		get
		{
			ItemType type = this.Type;
			if (type == ItemType.MediumArmor)
			{
				return 0.14f;
			}
			if (type != ItemType.HeavyArmor)
			{
				return 0f;
			}
			return 0.245f;
		}
	}

	// Token: 0x17000058 RID: 88
	// (get) Token: 0x060002E8 RID: 744 RVA: 0x0001333C File Offset: 0x0001153C
	public float ItemLevelPercentBonus
	{
		get
		{
			int num = Mathf.Max(0, this.RequiredLevel - 1);
			return 0.04f * (float)num;
		}
	}

	// Token: 0x17000059 RID: 89
	// (get) Token: 0x060002E9 RID: 745 RVA: 0x00013360 File Offset: 0x00011560
	public string DisplayName
	{
		get
		{
			if (NetworkServer.active)
			{
				return this.Name;
			}
			string str = "<color=" + GlobalUtils.RarityToColorString(this.Rarity) + ">" + LanguageManager.Instance.GetText(this.Name);
			if (this.BoostLevel > 0)
			{
				str += string.Format(" (+{0})", this.BoostLevel);
			}
			return str + "</color>";
		}
	}

	// Token: 0x1700005A RID: 90
	// (get) Token: 0x060002EA RID: 746 RVA: 0x000133D8 File Offset: 0x000115D8
	public string DisplayValue
	{
		get
		{
			if (!this.Sellable & NetworkClient.active)
			{
				return LanguageManager.Instance.GetText("item_cant_sell");
			}
			return (this.Amount * this.Value).ToString();
		}
	}

	// Token: 0x060002EB RID: 747 RVA: 0x0001341C File Offset: 0x0001161C
	private float GetBoostPercentBonus(int boostLevel)
	{
		switch (boostLevel)
		{
		case 1:
			return 0.005f;
		case 2:
			return 0.01f;
		case 3:
			return 0.015f;
		case 4:
			return 0.025f;
		case 5:
			return 0.035f;
		case 6:
			return 0.045f;
		case 7:
			return 0.065f;
		case 8:
			return 0.085f;
		case 9:
			return 0.105f;
		case 10:
			return 0.14f;
		case 11:
			return 0.175f;
		case 12:
			return 0.21f;
		case 13:
			return 0.26f;
		case 14:
			return 0.31f;
		case 15:
			return 0.36f;
		case 16:
			return 0.43f;
		case 17:
			return 0.5f;
		case 18:
			return 0.57f;
		case 19:
			return 0.69f;
		case 20:
			return 0.81f;
		case 21:
			return 0.93f;
		default:
			return 0f;
		}
	}

	// Token: 0x060002EC RID: 748 RVA: 0x0001350C File Offset: 0x0001170C
	public bool IsEqual(Item otherItem)
	{
		return this.Id == otherItem.Id && this.RequiredLevel == otherItem.RequiredLevel && this.BoostLevel == otherItem.BoostLevel && this.Rarity == otherItem.Rarity && this.Quality == otherItem.Quality;
	}

	// Token: 0x1700005B RID: 91
	// (get) Token: 0x060002ED RID: 749 RVA: 0x00013564 File Offset: 0x00011764
	public string FullDescription
	{
		get
		{
			if (NetworkServer.active)
			{
				return this._fullDescription;
			}
			if (string.IsNullOrEmpty(this._fullDescription))
			{
				if (this._uiSystemModule == null)
				{
					GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
					this._uiSystemModule = gameObject.GetComponent<UISystemModule>();
				}
				StringBuilder stringBuilder = new StringBuilder();
				Item item = this._uiSystemModule.EquipmentModule.GetItem(this.SlotType);
				this.AppendItemIdDescription(stringBuilder);
				this.AppendAttackDescription(stringBuilder, item);
				if (this.Category == ItemCategory.Armor)
				{
					this.AppendResistanceDescription(stringBuilder, item);
				}
				else
				{
					this.AppendDefenseDescription(stringBuilder, item);
					this.AppendBlockChanceDescription(stringBuilder);
					this.AppendArcaneProtectionChanceDescription(stringBuilder);
				}
				stringBuilder.AppendLine();
				this.AppendRarityDescription(stringBuilder);
				this.AppendQualityDescription(stringBuilder);
				this.AppendTypeDescription(stringBuilder);
				this.AppendCategoryDescription(stringBuilder);
				this.AppendSlotTypeDescription(stringBuilder);
				this.AppendRequiredLevelDescription(stringBuilder);
				this.AppendBoostLevelDescription(stringBuilder);
				this.AppendRequiredVocationDescription(stringBuilder);
				stringBuilder.AppendLine();
				this.AppendConsumableDescription(stringBuilder);
				this.AppendDescription(stringBuilder);
				stringBuilder.AppendLine();
				this.AppendSoulbindDescription(stringBuilder);
				this.AppendEquipableDescription(stringBuilder);
				this.AppendSkillDescription(stringBuilder);
				this.AppendBlueprintDescription(stringBuilder);
				this._fullDescription = stringBuilder.ToString();
			}
			return this._fullDescription;
		}
	}

	// Token: 0x060002EE RID: 750 RVA: 0x00013692 File Offset: 0x00011892
	private void AppendBlockChanceDescription(StringBuilder fullDescription)
	{
		if (this.BlockChance > 0f)
		{
			fullDescription.AppendLine(string.Format("{0} {1}%", LanguageManager.Instance.GetText("block_chance_label"), this.BlockChance * 100f));
		}
	}

	// Token: 0x060002EF RID: 751 RVA: 0x000136D2 File Offset: 0x000118D2
	private void AppendArcaneProtectionChanceDescription(StringBuilder fullDescription)
	{
		if (this.ArcaneProtectionChance > 0f)
		{
			fullDescription.AppendLine(string.Format("{0} {1}%", LanguageManager.Instance.GetText("arcane_protection_chance_label"), this.ArcaneProtectionChance * 100f));
		}
	}

	// Token: 0x060002F0 RID: 752 RVA: 0x00013714 File Offset: 0x00011914
	private void AppendItemIdDescription(StringBuilder fullDescription)
	{
		if (this._uiSystemModule.AttributeModule.AccessLevel != AccessLevel.Player)
		{
			fullDescription.AppendLine(string.Format("Item ID: {0}", this.Id));
			fullDescription.AppendLine("Item UID: " + this.UniqueId);
			fullDescription.AppendLine();
		}
	}

	// Token: 0x060002F1 RID: 753 RVA: 0x00013770 File Offset: 0x00011970
	private void AppendBlueprintDescription(StringBuilder fullDescription)
	{
		if (this.BlueprintId == 0)
		{
			return;
		}
		if (this._uiSystemModule.AttributeModule.Profession != this.BlueprintRequiredProfession)
		{
			fullDescription.AppendLine("<color=red>" + LanguageManager.Instance.GetText("item_wrong_required_profession_message") + "</color>");
			return;
		}
		if (this._uiSystemModule.AttributeModule.ProfessionLevel < this.BlueprintRequiredProfessionLevel)
		{
			string str = string.Format(LanguageManager.Instance.GetText("item_profession_level_too_low_message"), this.BlueprintRequiredProfessionLevel);
			fullDescription.AppendLine("<color=red>" + str + "</color>");
			return;
		}
		if (this._uiSystemModule.CraftModule.HasBlueprint(this.BlueprintId))
		{
			fullDescription.AppendLine("<color=red>" + LanguageManager.Instance.GetText("item_already_know_blueprint_message") + "</color>");
			return;
		}
		fullDescription.AppendLine("<color=green>" + LanguageManager.Instance.GetText("item_can_learn_blueprint_message") + "</color>");
	}

	// Token: 0x060002F2 RID: 754 RVA: 0x00013878 File Offset: 0x00011A78
	private void AppendSkillDescription(StringBuilder fullDescription)
	{
		if (this.SkillId > 0 && this._uiSystemModule.SkillModule.HasSkill(this.SkillId))
		{
			fullDescription.AppendLine("<color=red>" + LanguageManager.Instance.GetText("item_already_know_skill_message") + "</color>");
		}
	}

	// Token: 0x060002F3 RID: 755 RVA: 0x000138CC File Offset: 0x00011ACC
	private void AppendEquipableDescription(StringBuilder fullDescription)
	{
		if (this.Equipable && this.RequiredVocation == Vocation.Undefined && !this._uiSystemModule.VocationModule.AllowedItemTypes().Contains(this.Type))
		{
			fullDescription.AppendLine("<color=red>" + LanguageManager.Instance.GetText("item_cant_equip_message") + "</color>");
		}
	}

	// Token: 0x060002F4 RID: 756 RVA: 0x0001392C File Offset: 0x00011B2C
	private void AppendSoulbindDescription(StringBuilder fullDescription)
	{
		if (this.Soulbind)
		{
			if (this.OwnerId != 0 && !string.IsNullOrEmpty(this.OwnerName) && this._uiSystemModule.PlayerModule.PlayerId != this.OwnerId)
			{
				fullDescription.AppendLine(string.Format("<color=red>" + LanguageManager.Instance.GetText("item_soulbound_to") + "</color>", this.OwnerName));
				return;
			}
			fullDescription.AppendLine(LanguageManager.Instance.GetText("item_soulbound"));
		}
	}

	// Token: 0x060002F5 RID: 757 RVA: 0x000139B5 File Offset: 0x00011BB5
	private void AppendDescription(StringBuilder fullDescription)
	{
		if (!string.IsNullOrEmpty(this.Description))
		{
			fullDescription.AppendLine(LanguageManager.Instance.GetText(this.Description));
		}
	}

	// Token: 0x060002F6 RID: 758 RVA: 0x000139DC File Offset: 0x00011BDC
	private void AppendConsumableDescription(StringBuilder fullDescription)
	{
		if (this.Category == ItemCategory.Consumable)
		{
			fullDescription.AppendLine(LanguageManager.Instance.GetText("item_double_click_consume"));
			return;
		}
		if (this.Category == ItemCategory.Usable)
		{
			fullDescription.AppendLine(LanguageManager.Instance.GetText("item_double_click_use"));
		}
	}

	// Token: 0x060002F7 RID: 759 RVA: 0x00013A28 File Offset: 0x00011C28
	private void AppendRequiredVocationDescription(StringBuilder fullDescription)
	{
		if (this.RequiredVocation != Vocation.Undefined)
		{
			if (this._uiSystemModule.VocationModule.Vocation != this.RequiredVocation)
			{
				fullDescription.AppendLine(string.Format("<color=red>" + LanguageManager.Instance.GetText("item_required_vocation") + "</color>", GlobalUtils.VocationToString(this.RequiredVocation)));
				return;
			}
			fullDescription.AppendLine(string.Format(LanguageManager.Instance.GetText("item_required_vocation"), GlobalUtils.VocationToString(this.RequiredVocation)));
		}
	}

	// Token: 0x060002F8 RID: 760 RVA: 0x00013AB4 File Offset: 0x00011CB4
	private void AppendRequiredLevelDescription(StringBuilder fullDescription)
	{
		if (this.RequiredLevel > 0)
		{
			if (this._uiSystemModule.AttributeModule.BaseLevel < this.RequiredLevel)
			{
				fullDescription.AppendLine(string.Format("<color=red>" + LanguageManager.Instance.GetText("item_required_level") + "</color>", this.RequiredLevel));
				return;
			}
			fullDescription.AppendLine(string.Format(LanguageManager.Instance.GetText("item_required_level"), this.RequiredLevel));
		}
	}

	// Token: 0x060002F9 RID: 761 RVA: 0x00013B3E File Offset: 0x00011D3E
	public void AppendBoostLevelDescription(StringBuilder fullDescription)
	{
		if (this.BoostLevel > 0)
		{
			fullDescription.AppendLine(string.Format(LanguageManager.Instance.GetText("item_boost_level"), this.BoostLevel));
		}
	}

	// Token: 0x060002FA RID: 762 RVA: 0x00013B70 File Offset: 0x00011D70
	private void AppendSlotTypeDescription(StringBuilder fullDescription)
	{
		if (this.SlotType > SlotType.Undefined & !this.TwoHanded)
		{
			fullDescription.AppendLine(string.Format("<color=white>({0})</color>", GlobalUtils.SlotTypeToString(this.SlotType)));
			return;
		}
		if (this.SlotType > SlotType.Undefined & this.TwoHanded)
		{
			fullDescription.AppendLine("<color=white>(" + LanguageManager.Instance.GetText("item_two_handed") + ")</color>");
		}
	}

	// Token: 0x060002FB RID: 763 RVA: 0x00013BE8 File Offset: 0x00011DE8
	private void AppendQualityDescription(StringBuilder fullDescription)
	{
		string text = LanguageManager.Instance.GetText("item_quality");
		string text2 = GlobalUtils.ItemQualityToColorString(this.Quality);
		string text3 = GlobalUtils.ItemQualityToString(this.Quality);
		fullDescription.AppendLine(string.Concat(new string[]
		{
			text,
			" <color=",
			text2,
			">",
			text3,
			"</color>"
		}));
	}

	// Token: 0x060002FC RID: 764 RVA: 0x00013C53 File Offset: 0x00011E53
	private void AppendCategoryDescription(StringBuilder fullDescription)
	{
		if (this.Category != ItemCategory.Undefined)
		{
			fullDescription.AppendLine(string.Format(LanguageManager.Instance.GetText("item_category"), GlobalUtils.ItemCategoryToString(this.Category)));
		}
	}

	// Token: 0x060002FD RID: 765 RVA: 0x00013C83 File Offset: 0x00011E83
	private void AppendTypeDescription(StringBuilder fullDescription)
	{
		if (this.Type != ItemType.Undefined)
		{
			fullDescription.AppendLine(string.Format(LanguageManager.Instance.GetText("item_type"), GlobalUtils.ItemTypeToString(this.Type)));
		}
	}

	// Token: 0x060002FE RID: 766 RVA: 0x00013CB4 File Offset: 0x00011EB4
	private void AppendRarityDescription(StringBuilder fullDescription)
	{
		if (this.Rarity != Rarity.Undefined)
		{
			string text = LanguageManager.Instance.GetText("item_rarity");
			string text2 = GlobalUtils.RarityToColorString(this.Rarity);
			string text3 = GlobalUtils.RarityToString(this.Rarity);
			fullDescription.AppendLine(string.Concat(new string[]
			{
				text,
				" <color=",
				text2,
				">",
				text3,
				"</color>"
			}));
		}
	}

	// Token: 0x060002FF RID: 767 RVA: 0x00013D28 File Offset: 0x00011F28
	private void AppendDefenseDescription(StringBuilder fullDescription, Item equippedItem)
	{
		if (this.Defense <= 0)
		{
			return;
		}
		if (!equippedItem.IsDefined)
		{
			fullDescription.AppendLine(string.Format("{0} {1}", LanguageManager.Instance.GetText("defense_label"), this.Defense));
			return;
		}
		int num = (int)(this.Defense - equippedItem.Defense);
		if (num == 0)
		{
			fullDescription.AppendLine(string.Format("{0} {1} (+{2})", LanguageManager.Instance.GetText("defense_label"), this.Defense, num));
			return;
		}
		if (num > 0)
		{
			fullDescription.AppendLine(string.Format("{0} {1} (<color=green>+{2}</color>)", LanguageManager.Instance.GetText("defense_label"), this.Defense, num));
			return;
		}
		if (num < 0)
		{
			fullDescription.AppendLine(string.Format("{0} {1} (<color=red>{2}</color>)", LanguageManager.Instance.GetText("defense_label"), this.Defense, num));
			return;
		}
	}

	// Token: 0x06000300 RID: 768 RVA: 0x00013E24 File Offset: 0x00012024
	private void AppendResistanceDescription(StringBuilder fullDescription, Item equippedItem)
	{
		if (this.Defense <= 0)
		{
			return;
		}
		if (!equippedItem.IsDefined)
		{
			fullDescription.AppendLine(string.Format("{0} {1}", LanguageManager.Instance.GetText("resistance_label"), this.Defense));
			return;
		}
		int num = (int)(this.Defense - equippedItem.Defense);
		if (num == 0)
		{
			fullDescription.AppendLine(string.Format("{0} {1} (+{2})", LanguageManager.Instance.GetText("resistance_label"), this.Defense, num));
			return;
		}
		if (num > 0)
		{
			fullDescription.AppendLine(string.Format("{0} {1} (<color=green>+{2}</color>)", LanguageManager.Instance.GetText("resistance_label"), this.Defense, num));
			return;
		}
		if (num < 0)
		{
			fullDescription.AppendLine(string.Format("{0} {1} (<color=red>{2}</color>)", LanguageManager.Instance.GetText("resistance_label"), this.Defense, num));
			return;
		}
	}

	// Token: 0x06000301 RID: 769 RVA: 0x00013F20 File Offset: 0x00012120
	private void AppendAttackDescription(StringBuilder fullDescription, Item equippedItem)
	{
		if (this.Attack <= 0)
		{
			return;
		}
		if (!equippedItem.IsDefined)
		{
			fullDescription.AppendLine(string.Format("{0} {1}", LanguageManager.Instance.GetText("attack_label"), this.Attack));
			return;
		}
		int num = (int)(this.Attack - equippedItem.Attack);
		if (num == 0)
		{
			fullDescription.AppendLine(string.Format("{0} {1} (+0)", LanguageManager.Instance.GetText("attack_label"), this.Attack));
			return;
		}
		if (num > 0)
		{
			fullDescription.AppendLine(string.Format("{0} {1} (<color=green>+{2}</color>)", LanguageManager.Instance.GetText("attack_label"), this.Attack, num));
			return;
		}
		if (num < 1)
		{
			fullDescription.AppendLine(string.Format("{0} {1} (<color=red>{2}</color>)", LanguageManager.Instance.GetText("attack_label"), this.Attack, num));
			return;
		}
	}

	// Token: 0x0400051D RID: 1309
	private Sprite _icon;

	// Token: 0x0400051E RID: 1310
	private string _fullDescription;

	// Token: 0x0400051F RID: 1311
	private UISystemModule _uiSystemModule;

	// Token: 0x04000520 RID: 1312
	public int _value;

	// Token: 0x04000521 RID: 1313
	public short _attack;

	// Token: 0x04000522 RID: 1314
	public short _defense;

	// Token: 0x04000523 RID: 1315
	public string _metaName;

	// Token: 0x04000524 RID: 1316
	public Rarity _rarity;

	// Token: 0x04000525 RID: 1317
	public int _requiredLevel;

	// Token: 0x04000526 RID: 1318
	public int Id;

	// Token: 0x04000527 RID: 1319
	public int SkillId;

	// Token: 0x04000528 RID: 1320
	public string Name;

	// Token: 0x04000529 RID: 1321
	public ItemType Type;

	// Token: 0x0400052A RID: 1322
	public bool Soulbind;

	// Token: 0x0400052B RID: 1323
	public bool Sellable;

	// Token: 0x0400052C RID: 1324
	public bool TwoHanded;

	// Token: 0x0400052D RID: 1325
	public int BoostLevel;

	// Token: 0x0400052E RID: 1326
	public bool Stackable;

	// Token: 0x0400052F RID: 1327
	public int BlueprintId;

	// Token: 0x04000530 RID: 1328
	public string OwnerName;

	// Token: 0x04000531 RID: 1329
	public SlotType SlotType;

	// Token: 0x04000532 RID: 1330
	public string Description;

	// Token: 0x04000533 RID: 1331
	public ItemQuality Quality;

	// Token: 0x04000534 RID: 1332
	public ItemCategory Category;

	// Token: 0x04000535 RID: 1333
	public Vocation RequiredVocation;

	// Token: 0x04000536 RID: 1334
	public bool IgnoreQualityRestrictions;

	// Token: 0x04000537 RID: 1335
	public int BlueprintRequiredProfessionLevel;

	// Token: 0x04000538 RID: 1336
	public PlayerProfession BlueprintRequiredProfession;

	// Token: 0x04000539 RID: 1337
	public int Amount;

	// Token: 0x0400053A RID: 1338
	public int SlotPosition;

	// Token: 0x0400053B RID: 1339
	public int OwnerId;
}
