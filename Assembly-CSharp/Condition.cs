using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Mirror;
using UnityEngine;

// Token: 0x020000FF RID: 255
public struct Condition
{
	// Token: 0x06000282 RID: 642 RVA: 0x00011E60 File Offset: 0x00010060
	public Condition(ConditionCategory category, ConditionType type, float duration, float interval, float power, Effect effect, int textColorId = 0, float elapsed = 0f, string soundEffectName = "")
	{
		this.Type = type;
		this.Power = power;
		this.Effect = effect;
		this.LastUseTime = 0.0;
		this.Elapsed = elapsed;
		this.Category = category;
		this.Duration = duration;
		this.Interval = interval;
		this.TextColorId = textColorId;
		this.SoundEffectName = soundEffectName;
		this.IgnorePersistence = false;
		this._icon = null;
		this._fullDescription = string.Empty;
		this._metaName = string.Empty;
		this._uiSystemModule = null;
	}

	// Token: 0x17000021 RID: 33
	// (get) Token: 0x06000283 RID: 643 RVA: 0x00011EEC File Offset: 0x000100EC
	// (set) Token: 0x06000284 RID: 644 RVA: 0x00011EF4 File Offset: 0x000100F4
	public float Elapsed { readonly get; set; }

	// Token: 0x17000022 RID: 34
	// (get) Token: 0x06000285 RID: 645 RVA: 0x00011EFD File Offset: 0x000100FD
	// (set) Token: 0x06000286 RID: 646 RVA: 0x00011F05 File Offset: 0x00010105
	public Effect Effect { readonly get; set; }

	// Token: 0x17000023 RID: 35
	// (get) Token: 0x06000287 RID: 647 RVA: 0x00011F0E File Offset: 0x0001010E
	// (set) Token: 0x06000288 RID: 648 RVA: 0x00011F16 File Offset: 0x00010116
	public int TextColorId { readonly get; set; }

	// Token: 0x17000024 RID: 36
	// (get) Token: 0x06000289 RID: 649 RVA: 0x00011F1F File Offset: 0x0001011F
	// (set) Token: 0x0600028A RID: 650 RVA: 0x00011F27 File Offset: 0x00010127
	public string SoundEffectName { readonly get; set; }

	// Token: 0x17000025 RID: 37
	// (get) Token: 0x0600028B RID: 651 RVA: 0x00011F30 File Offset: 0x00010130
	// (set) Token: 0x0600028C RID: 652 RVA: 0x00011F38 File Offset: 0x00010138
	public bool IgnorePersistence { readonly get; set; }

	// Token: 0x17000026 RID: 38
	// (get) Token: 0x0600028D RID: 653 RVA: 0x00011F44 File Offset: 0x00010144
	public string UniqueId
	{
		get
		{
			return string.Format("{0}_{1}_{2}_{3}", new object[]
			{
				this.Category,
				this.Type,
				this.Duration,
				this.Power
			});
		}
	}

	// Token: 0x17000027 RID: 39
	// (get) Token: 0x0600028E RID: 654 RVA: 0x00011F99 File Offset: 0x00010199
	public string Name
	{
		get
		{
			return GlobalUtils.ConditionTypeToString(this.Type);
		}
	}

	// Token: 0x17000028 RID: 40
	// (get) Token: 0x0600028F RID: 655 RVA: 0x00011FA8 File Offset: 0x000101A8
	public string TranslatedName
	{
		get
		{
			string text = this.MetaName.ToLower();
			text = "condition_type_" + text;
			return LanguageManager.Instance.GetText(text);
		}
	}

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x06000290 RID: 656 RVA: 0x00011FD8 File Offset: 0x000101D8
	public int PowerAsInt
	{
		get
		{
			return Mathf.CeilToInt(this.Power);
		}
	}

	// Token: 0x1700002A RID: 42
	// (get) Token: 0x06000291 RID: 657 RVA: 0x00011FE5 File Offset: 0x000101E5
	public bool IsDefined
	{
		get
		{
			return this.Duration > 0f | this.Power > 0f | this.Interval > 0f;
		}
	}

	// Token: 0x06000292 RID: 658 RVA: 0x00012010 File Offset: 0x00010210
	public bool IsEqual(Condition otherCondition)
	{
		return this.Category == otherCondition.Category && this.Type == otherCondition.Type && this.Duration == otherCondition.Duration && this.Power == otherCondition.Power;
	}

	// Token: 0x1700002B RID: 43
	// (get) Token: 0x06000293 RID: 659 RVA: 0x0001204C File Offset: 0x0001024C
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
				StringBuilder stringBuilder = new StringBuilder();
				if (this.Power > 0f)
				{
					float num = this.Power;
					string arg = string.Empty;
					if (this.Type == ConditionType.ExpBonus | this.Type == ConditionType.AxpBonus)
					{
						if (this._uiSystemModule == null)
						{
							GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
							this._uiSystemModule = gameObject.GetComponent<UISystemModule>();
						}
						float b = (this._uiSystemModule.PlayerModule.PremiumDays > 0) ? 0.25f : 0.5f;
						num = Mathf.Min(num, b);
						num = this.Power * 100f;
						arg = "%";
					}
					else if (this.Category == ConditionCategory.Wrath | this.Category == ConditionCategory.Expertise | this.Category == ConditionCategory.Blessing | this.Category == ConditionCategory.Food | this.Category == ConditionCategory.Curse | this.Category == ConditionCategory.Boost)
					{
						num = this.Power * 100f;
						arg = "%";
					}
					else if (this.Category == ConditionCategory.Invincibility | this.Category == ConditionCategory.Confusion | this.Category == ConditionCategory.Invisibility | this.Category == ConditionCategory.Paralyzing)
					{
						arg = "s";
					}
					stringBuilder.AppendLine(string.Format(LanguageManager.Instance.GetText("condition_power"), num, arg));
				}
				if (this.Interval > 0f)
				{
					stringBuilder.AppendLine(string.Format(LanguageManager.Instance.GetText("condition_interval"), this.Interval));
				}
				stringBuilder.AppendLine(string.Format("<color=white>" + LanguageManager.Instance.GetText("condition_category") + "</color>", GlobalUtils.ConditionCategoryToString(this.Category)));
				this._fullDescription = stringBuilder.ToString();
			}
			return this._fullDescription;
		}
	}

	// Token: 0x06000294 RID: 660 RVA: 0x00012238 File Offset: 0x00010438
	public double CooldownTimer(double currentTime)
	{
		double num = (double)this.Duration - (currentTime - this.LastUseTime);
		num = ((num < 0.0) ? 0.0 : num);
		return (double)((float)num);
	}

	// Token: 0x06000295 RID: 661 RVA: 0x00012274 File Offset: 0x00010474
	public double CooldownTimerPercent()
	{
		double num = this.CooldownTimer(NetworkTime.time);
		return 1.0 - num * 100.0 / (double)this.Duration / 100.0;
	}

	// Token: 0x1700002C RID: 44
	// (get) Token: 0x06000296 RID: 662 RVA: 0x000122B4 File Offset: 0x000104B4
	public string MetaName
	{
		get
		{
			if (NetworkServer.active)
			{
				return null;
			}
			if (string.IsNullOrEmpty(this.Name))
			{
				return null;
			}
			if (!string.IsNullOrEmpty(this._metaName))
			{
				return this._metaName;
			}
			this._metaName = Regex.Replace(this.Name, "(?<=[a-z])([A-Z])", "_$1").ToLower();
			this._metaName = new string((from w in this._metaName
			where char.IsLetterOrDigit(w) | char.IsWhiteSpace(w)
			select w).ToArray<char>());
			this._metaName = this._metaName.Replace(" ", "_");
			if (this.Category == ConditionCategory.Blessing | this.Category == ConditionCategory.LevelBlessing)
			{
				this._metaName += "_blessing";
			}
			if (this.Category == ConditionCategory.Curse | this.Category == ConditionCategory.LevelCurse)
			{
				this._metaName += "_curse";
			}
			return this._metaName;
		}
	}

	// Token: 0x1700002D RID: 45
	// (get) Token: 0x06000297 RID: 663 RVA: 0x000123C0 File Offset: 0x000105C0
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
				string text = Path.Combine("Icons/Conditions/", this.MetaName);
				text = text.Replace("\\", "/");
				this._icon = Resources.Load<Sprite>(text);
			}
			if (this._icon == null && !string.IsNullOrEmpty(this.MetaName))
			{
				string text2 = Path.Combine("Icons/Skills/", this.MetaName);
				text2 = text2.Replace("\\", "/");
				this._icon = Resources.Load<Sprite>(text2);
			}
			if (this._icon == null)
			{
				string text3 = Path.Combine("Icons/Conditions/", "undefined_icon");
				text3 = text3.Replace("\\", "/");
				this._icon = Resources.Load<Sprite>(text3);
			}
			return this._icon;
		}
	}

	// Token: 0x1700002E RID: 46
	// (get) Token: 0x06000298 RID: 664 RVA: 0x000124AE File Offset: 0x000106AE
	public bool CanCauseDamage
	{
		get
		{
			return this.Category == ConditionCategory.Confusion | this.Category == ConditionCategory.Degeneration | this.Category == ConditionCategory.Reflecting;
		}
	}

	// Token: 0x1700002F RID: 47
	// (get) Token: 0x06000299 RID: 665 RVA: 0x000124CE File Offset: 0x000106CE
	public bool IsBadCondition
	{
		get
		{
			return this.Category == ConditionCategory.Confusion | this.Category == ConditionCategory.Degeneration | this.Category == ConditionCategory.Reflecting | this.Category == ConditionCategory.Curse | this.Category == ConditionCategory.Taunt | this.Category == ConditionCategory.Paralyzing;
		}
	}

	// Token: 0x17000030 RID: 48
	// (get) Token: 0x0600029A RID: 666 RVA: 0x0001250D File Offset: 0x0001070D
	public bool IsNeutralCondition
	{
		get
		{
			return this.Category == ConditionCategory.Boost | this.Category == ConditionCategory.Transformation | this.Type == ConditionType.Dash;
		}
	}

	// Token: 0x17000031 RID: 49
	// (get) Token: 0x0600029B RID: 667 RVA: 0x0001252E File Offset: 0x0001072E
	public bool IsGoodCondition
	{
		get
		{
			return !this.IsBadCondition & !this.IsNeutralCondition;
		}
	}

	// Token: 0x17000032 RID: 50
	// (get) Token: 0x0600029C RID: 668 RVA: 0x00012544 File Offset: 0x00010744
	public bool CanUpdateElapsedTime
	{
		get
		{
			return this.Category != ConditionCategory.Confusion & this.Category != ConditionCategory.Paralyzing & this.Category != ConditionCategory.Invisibility & this.Type != ConditionType.ExpBonus & this.Type != ConditionType.AxpBonus;
		}
	}

	// Token: 0x040004AC RID: 1196
	private Sprite _icon;

	// Token: 0x040004AD RID: 1197
	private string _fullDescription;

	// Token: 0x040004AE RID: 1198
	private string _metaName;

	// Token: 0x040004AF RID: 1199
	private UISystemModule _uiSystemModule;

	// Token: 0x040004B0 RID: 1200
	public float Power;

	// Token: 0x040004B1 RID: 1201
	public float Interval;

	// Token: 0x040004B2 RID: 1202
	public float Duration;

	// Token: 0x040004B3 RID: 1203
	public double LastUseTime;

	// Token: 0x040004B4 RID: 1204
	public ConditionType Type;

	// Token: 0x040004B5 RID: 1205
	public ConditionCategory Category;
}
