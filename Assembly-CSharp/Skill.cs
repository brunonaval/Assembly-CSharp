using System;
using System.IO;
using System.Text;
using Mirror;
using UnityEngine;

// Token: 0x02000148 RID: 328
public struct Skill
{
	// Token: 0x1700007B RID: 123
	// (get) Token: 0x06000369 RID: 873 RVA: 0x00015A54 File Offset: 0x00013C54
	// (set) Token: 0x0600036A RID: 874 RVA: 0x00015A5C File Offset: 0x00013C5C
	public string Script { readonly get; set; }

	// Token: 0x1700007C RID: 124
	// (get) Token: 0x0600036B RID: 875 RVA: 0x00015A65 File Offset: 0x00013C65
	// (set) Token: 0x0600036C RID: 876 RVA: 0x00015A6D File Offset: 0x00013C6D
	public ItemType AmmoType { readonly get; set; }

	// Token: 0x1700007D RID: 125
	// (get) Token: 0x0600036D RID: 877 RVA: 0x00015A76 File Offset: 0x00013C76
	// (set) Token: 0x0600036E RID: 878 RVA: 0x00015A7E File Offset: 0x00013C7E
	public Effect CasterEffect { readonly get; set; }

	// Token: 0x1700007E RID: 126
	// (get) Token: 0x0600036F RID: 879 RVA: 0x00015A87 File Offset: 0x00013C87
	// (set) Token: 0x06000370 RID: 880 RVA: 0x00015A8F File Offset: 0x00013C8F
	public Effect TargetEffect { readonly get; set; }

	// Token: 0x1700007F RID: 127
	// (get) Token: 0x06000371 RID: 881 RVA: 0x00015A98 File Offset: 0x00013C98
	// (set) Token: 0x06000372 RID: 882 RVA: 0x00015AA0 File Offset: 0x00013CA0
	public Vocation CasterVocation { readonly get; set; }

	// Token: 0x17000080 RID: 128
	// (get) Token: 0x06000373 RID: 883 RVA: 0x00015AA9 File Offset: 0x00013CA9
	// (set) Token: 0x06000374 RID: 884 RVA: 0x00015AB1 File Offset: 0x00013CB1
	public AnimationType AnimationType { readonly get; set; }

	// Token: 0x17000081 RID: 129
	// (get) Token: 0x06000375 RID: 885 RVA: 0x00015ABA File Offset: 0x00013CBA
	// (set) Token: 0x06000376 RID: 886 RVA: 0x00015AC2 File Offset: 0x00013CC2
	public string CasterSoundEffectName { readonly get; set; }

	// Token: 0x17000082 RID: 130
	// (get) Token: 0x06000377 RID: 887 RVA: 0x00015ACB File Offset: 0x00013CCB
	// (set) Token: 0x06000378 RID: 888 RVA: 0x00015AD3 File Offset: 0x00013CD3
	public string TargetSoundEffectName { readonly get; set; }

	// Token: 0x17000083 RID: 131
	// (get) Token: 0x06000379 RID: 889 RVA: 0x00015ADC File Offset: 0x00013CDC
	// (set) Token: 0x0600037A RID: 890 RVA: 0x00015AE4 File Offset: 0x00013CE4
	public float DamageModifier { readonly get; set; }

	// Token: 0x17000084 RID: 132
	// (get) Token: 0x0600037B RID: 891 RVA: 0x00015AED File Offset: 0x00013CED
	public float EnchantLevelPercentBonus
	{
		get
		{
			return this.GetEnchantPercentBonus(this.EnchantLevel);
		}
	}

	// Token: 0x17000085 RID: 133
	// (get) Token: 0x0600037C RID: 892 RVA: 0x00015AFB File Offset: 0x00013CFB
	public float NextEnchantLevelPercentBonus
	{
		get
		{
			return this.GetEnchantPercentBonus(this.EnchantLevel + 1);
		}
	}

	// Token: 0x17000086 RID: 134
	// (get) Token: 0x0600037D RID: 893 RVA: 0x00015B0B File Offset: 0x00013D0B
	public bool IsRanged
	{
		get
		{
			return this.Type != SkillType.Melee & this.Type != SkillType.MeleeAoE;
		}
	}

	// Token: 0x17000087 RID: 135
	// (get) Token: 0x0600037E RID: 894 RVA: 0x00015B26 File Offset: 0x00013D26
	public bool CanCauseDamage
	{
		get
		{
			return this.Category == SkillCategory.Attack | this.Category == SkillCategory.Curse;
		}
	}

	// Token: 0x17000088 RID: 136
	// (get) Token: 0x0600037F RID: 895 RVA: 0x00015B3B File Offset: 0x00013D3B
	public float Cooldown
	{
		get
		{
			return this._cooldown - this._cooldown * this.EnchantLevelPercentBonus;
		}
	}

	// Token: 0x17000089 RID: 137
	// (get) Token: 0x06000380 RID: 896 RVA: 0x00015B54 File Offset: 0x00013D54
	public string FullDescription
	{
		get
		{
			if (string.IsNullOrEmpty(this._fullDescription))
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("<color=white>");
				if (this.SkillPower > 1f)
				{
					if (this.CastAmount > 1)
					{
						stringBuilder.AppendLine(string.Format(LanguageManager.Instance.GetText("skill_multi_power"), this.SkillPower, this.CastAmount));
					}
					else
					{
						stringBuilder.AppendLine(string.Format(LanguageManager.Instance.GetText("skill_single_power"), this.SkillPower));
					}
				}
				else if (this.SkillPower > 0f & this.SkillPower <= 1f)
				{
					if (this.CastAmount > 1)
					{
						stringBuilder.AppendLine(string.Format(LanguageManager.Instance.GetText("skill_percent_multi_power"), this.SkillPower * 100f, this.CastAmount));
					}
					else
					{
						stringBuilder.AppendLine(string.Format(LanguageManager.Instance.GetText("skill_percent_single_power"), this.SkillPower * 100f));
					}
				}
				stringBuilder.AppendLine(string.Format(LanguageManager.Instance.GetText("skill_range"), this.Range));
				stringBuilder.AppendLine(string.Format(LanguageManager.Instance.GetText("skill_targets"), this.MaxTargets));
				stringBuilder.AppendLine(string.Format(LanguageManager.Instance.GetText("skill_required_level"), this.RequiredLevel));
				if (this.EnchantLevel > 0)
				{
					stringBuilder.AppendLine(string.Format(LanguageManager.Instance.GetText("skill_enchant_level"), this.EnchantLevel));
				}
				if (this.WeaponType != ItemType.Undefined)
				{
					stringBuilder.AppendLine(string.Format(LanguageManager.Instance.GetText("skill_weapon_type"), GlobalUtils.ItemTypeToString(this.WeaponType)));
				}
				stringBuilder.AppendLine("</color>");
				stringBuilder.AppendLine(GlobalUtils.SkillCategoryToString(this.Category));
				stringBuilder.AppendLine(GlobalUtils.SkillTypeToString(this.Type));
				if (this.NeedTarget)
				{
					stringBuilder.AppendLine("<color=white>" + LanguageManager.Instance.GetText("skill_need_target") + "</color>");
				}
				if (!string.IsNullOrEmpty(this.Description))
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine(LanguageManager.Instance.GetText(this.Description));
				}
				this._fullDescription = stringBuilder.ToString();
			}
			return this._fullDescription;
		}
	}

	// Token: 0x06000381 RID: 897 RVA: 0x00015DF4 File Offset: 0x00013FF4
	public double CooldownTimer(double currentTime)
	{
		if (this.LastUseTime > currentTime)
		{
			return 0.0;
		}
		double num = (double)this.Cooldown - (currentTime - this.LastUseTime);
		num = ((num < 0.0) ? 0.0 : num);
		return (num > (double)this.Cooldown) ? ((double)this.Cooldown) : num;
	}

	// Token: 0x1700008A RID: 138
	// (get) Token: 0x06000382 RID: 898 RVA: 0x00015E54 File Offset: 0x00014054
	public bool IsDefined
	{
		get
		{
			return !string.IsNullOrEmpty(this.Name);
		}
	}

	// Token: 0x1700008B RID: 139
	// (get) Token: 0x06000383 RID: 899 RVA: 0x00015E64 File Offset: 0x00014064
	public string MetaName
	{
		get
		{
			if (string.IsNullOrEmpty(this.Name))
			{
				return null;
			}
			return this.Name.ToLower().Replace("skill_", "").Replace("_name", "").Replace(" ", "_");
		}
	}

	// Token: 0x1700008C RID: 140
	// (get) Token: 0x06000384 RID: 900 RVA: 0x00015EB8 File Offset: 0x000140B8
	public Sprite Icon
	{
		get
		{
			if (this._icon == null && !string.IsNullOrEmpty(this.MetaName))
			{
				string text = Path.Combine("Icons/Skills/", this.MetaName);
				text = text.Replace("\\", "/");
				this._icon = Resources.Load<Sprite>(text);
			}
			if (this._icon == null)
			{
				string text2 = Path.Combine("Icons/Skills/", "undefined_icon");
				text2 = text2.Replace("\\", "/");
				this._icon = Resources.Load<Sprite>(text2);
			}
			return this._icon;
		}
	}

	// Token: 0x1700008D RID: 141
	// (get) Token: 0x06000385 RID: 901 RVA: 0x00015F50 File Offset: 0x00014150
	public string DisplayName
	{
		get
		{
			if (NetworkServer.active)
			{
				return this.Name;
			}
			string str = "<color=" + GlobalUtils.SkillCategoryToColorString(this.Category) + ">" + LanguageManager.Instance.GetText(this.Name);
			if (this.EnchantLevel > 0)
			{
				str += string.Format(" (+{0})", this.EnchantLevel);
			}
			return str + "</color>";
		}
	}

	// Token: 0x06000386 RID: 902 RVA: 0x00015FC8 File Offset: 0x000141C8
	public float GetEnchantPercentBonus(int enchantLevel)
	{
		switch (enchantLevel)
		{
		case 1:
			return 0.02f;
		case 2:
			return 0.04f;
		case 3:
			return 0.06f;
		case 4:
			return 0.08f;
		case 5:
			return 0.1f;
		case 6:
			return 0.12f;
		case 7:
			return 0.14f;
		case 8:
			return 0.16f;
		case 9:
			return 0.18f;
		case 10:
			return 0.2f;
		default:
			return 0f;
		}
	}

	// Token: 0x06000387 RID: 903 RVA: 0x00016048 File Offset: 0x00014248
	public Skill(int slotPosition, int skillBarId)
	{
		this.Id = 0;
		this.Name = null;
		this.Range = 0f;
		this.Script = null;
		this.SkillPower = 0f;
		this.Type = SkillType.Undefined;
		this.CastAmount = 0;
		this.MaxTargets = 0;
		this.NeedTarget = false;
		this.EnchantLevel = 0;
		this.RequiredLevel = 0;
		this.CasterEffect = default(Effect);
		this.TargetEffect = default(Effect);
		this.Category = SkillCategory.Undefined;
		this.AnimationType = AnimationType.Cast;
		this.WeaponType = ItemType.Undefined;
		this.AmmoType = ItemType.Undefined;
		this.CasterVocation = Vocation.Undefined;
		this.IsDefaultSkill = false;
		this.CasterSoundEffectName = null;
		this.TargetSoundEffectName = null;
		this.DamageModifier = 0f;
		this._icon = null;
		this._cooldown = 0f;
		this._fullDescription = null;
		this.LastUseTime = 0.0;
		this.Description = null;
		this.SkillBarId = skillBarId;
		this.SlotPosition = slotPosition;
		this.Learned = false;
	}

	// Token: 0x06000388 RID: 904 RVA: 0x00016150 File Offset: 0x00014350
	public Skill(int id, string name, float range, string script, float cooldown, float skillPower, SkillType type, int maxTargets, bool needTarget, int requiredLevel, string description, Effect casterEffect, Effect targetEffect, SkillCategory category, AnimationType animationType, ItemType weaponType, ItemType ammoType, Vocation casterVocation, string casterSoundEffectName, string targetSoundEffectName, int castAmount, bool isDefaultSkill)
	{
		this.Id = id;
		this.Name = name;
		this.Range = range;
		this.Script = script;
		this.SkillPower = skillPower;
		this.Type = type;
		this.CastAmount = castAmount;
		this.MaxTargets = maxTargets;
		this.NeedTarget = needTarget;
		this.EnchantLevel = 0;
		this.RequiredLevel = requiredLevel;
		this.Description = description;
		this.CasterEffect = casterEffect;
		this.TargetEffect = targetEffect;
		this.Category = category;
		this.AnimationType = animationType;
		this.WeaponType = weaponType;
		this.AmmoType = ammoType;
		this.IsDefaultSkill = isDefaultSkill;
		this.CasterVocation = casterVocation;
		this.CasterSoundEffectName = casterSoundEffectName;
		this.TargetSoundEffectName = targetSoundEffectName;
		this.DamageModifier = 0f;
		this._icon = null;
		this._cooldown = cooldown;
		this._fullDescription = null;
		this.LastUseTime = 0.0;
		this.SlotPosition = 0;
		this.SkillBarId = 0;
		this.Learned = false;
	}

	// Token: 0x040006B0 RID: 1712
	private Sprite _icon;

	// Token: 0x040006B1 RID: 1713
	private string _fullDescription;

	// Token: 0x040006B2 RID: 1714
	public int Id;

	// Token: 0x040006B3 RID: 1715
	public string Name;

	// Token: 0x040006B4 RID: 1716
	public float Range;

	// Token: 0x040006B5 RID: 1717
	public bool Learned;

	// Token: 0x040006B6 RID: 1718
	public float _cooldown;

	// Token: 0x040006B7 RID: 1719
	public float SkillPower;

	// Token: 0x040006B8 RID: 1720
	public SkillType Type;

	// Token: 0x040006B9 RID: 1721
	public int CastAmount;

	// Token: 0x040006BA RID: 1722
	public int MaxTargets;

	// Token: 0x040006BB RID: 1723
	public bool NeedTarget;

	// Token: 0x040006BC RID: 1724
	public int EnchantLevel;

	// Token: 0x040006BD RID: 1725
	public int RequiredLevel;

	// Token: 0x040006BE RID: 1726
	public string Description;

	// Token: 0x040006BF RID: 1727
	public bool IsDefaultSkill;

	// Token: 0x040006C0 RID: 1728
	public ItemType WeaponType;

	// Token: 0x040006C1 RID: 1729
	public SkillCategory Category;

	// Token: 0x040006C2 RID: 1730
	public int SlotPosition;

	// Token: 0x040006C3 RID: 1731
	public int SkillBarId;

	// Token: 0x040006C4 RID: 1732
	public double LastUseTime;
}
