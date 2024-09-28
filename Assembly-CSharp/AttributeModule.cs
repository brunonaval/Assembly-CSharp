using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using CodeStage.AntiCheat.ObscuredTypes;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

// Token: 0x020002BD RID: 701
public class AttributeModule : NetworkBehaviour
{
	// Token: 0x17000146 RID: 326
	// (get) Token: 0x06000BB5 RID: 2997 RVA: 0x000348D8 File Offset: 0x00032AD8
	// (set) Token: 0x06000BB6 RID: 2998 RVA: 0x000348E0 File Offset: 0x00032AE0
	public long ExperiencePerMinute { get; private set; }

	// Token: 0x17000147 RID: 327
	// (get) Token: 0x06000BB7 RID: 2999 RVA: 0x000348E9 File Offset: 0x00032AE9
	// (set) Token: 0x06000BB8 RID: 3000 RVA: 0x000348F1 File Offset: 0x00032AF1
	public long AttributeExperiencePerMinute { get; private set; }

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x06000BB9 RID: 3001 RVA: 0x000348FC File Offset: 0x00032AFC
	// (remove) Token: 0x06000BBA RID: 3002 RVA: 0x00034934 File Offset: 0x00032B34
	public event AttributeModule.OnBaseLevelUpEventHandler OnBaseLevelUp;

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x06000BBB RID: 3003 RVA: 0x0003496C File Offset: 0x00032B6C
	// (remove) Token: 0x06000BBC RID: 3004 RVA: 0x000349A4 File Offset: 0x00032BA4
	public event AttributeModule.OnProfessionLevelUpEventHandler OnProfessionLevelUp;

	// Token: 0x17000148 RID: 328
	private global::Attribute this[AttributeType type]
	{
		get
		{
			for (int i = 0; i < this.Attributes.Count; i++)
			{
				if (this.Attributes[i].Type == type)
				{
					return this.Attributes[i];
				}
			}
			return default(global::Attribute);
		}
		[Server]
		set
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("[Server] function 'System.Void AttributeModule::set_Item(AttributeType,Attribute)' called when server was not active");
				return;
			}
			int i = 0;
			while (i < this.Attributes.Count)
			{
				if (this.Attributes[i].Type == type)
				{
					this.Attributes[i] = value;
					if (type == AttributeType.Vitality)
					{
						this.NetworkMaxHealth = this.CalculateMaxHealth();
					}
					if (type == AttributeType.Agility)
					{
						this.NetworkMaxEndurance = this.CalculateMaxEndurance();
						return;
					}
					break;
				}
				else
				{
					i++;
				}
			}
		}
	}

	// Token: 0x06000BBF RID: 3007 RVA: 0x00034AA8 File Offset: 0x00032CA8
	private void Awake()
	{
		if (NetworkClient.active)
		{
			this.experienceLog = new List<ExperienceData>();
			this.attributeExperienceLog = new List<ExperienceData>();
		}
		this.pvpModule = base.GetComponent<PvpModule>();
		this.combatModule = base.GetComponent<CombatModule>();
		this.effectModule = base.GetComponent<EffectModule>();
		this.creatureModule = base.GetComponent<CreatureModule>();
		this.vocationModule = base.GetComponent<VocationModule>();
	}

	// Token: 0x06000BC0 RID: 3008 RVA: 0x00034B0E File Offset: 0x00032D0E
	private void Start()
	{
		this.InitializeExperienceBonusTable();
	}

	// Token: 0x06000BC1 RID: 3009 RVA: 0x00034B16 File Offset: 0x00032D16
	public override void OnStartServer()
	{
		this.Attributes.Callback += this.OnAttributesUpdatedOnServer;
	}

	// Token: 0x06000BC2 RID: 3010 RVA: 0x00034B2F File Offset: 0x00032D2F
	public override void OnStartLocalPlayer()
	{
		this.Attributes.Callback += this.OnAttributesUpdatedOnLocalPlayer;
	}

	// Token: 0x06000BC3 RID: 3011 RVA: 0x00034B48 File Offset: 0x00032D48
	private void OnAttributesUpdatedOnLocalPlayer(SyncList<global::Attribute>.Operation op, int itemIndex, global::Attribute oldItem, global::Attribute newItem)
	{
		long experience = newItem.Experience - oldItem.Experience;
		if (oldItem.Experience == 0L)
		{
			return;
		}
		this.attributeExperienceLog.RemoveAll((ExperienceData el) => Time.time - el.GameTime > 180f);
		this.attributeExperienceLog.Add(new ExperienceData
		{
			Experience = experience,
			GameTime = Time.time
		});
		if (this.attributeExperienceLog.Count > 0)
		{
			long num = this.attributeExperienceLog.Sum((ExperienceData el) => el.Experience);
			this.AttributeExperiencePerMinute = num / 3L;
		}
	}

	// Token: 0x06000BC4 RID: 3012 RVA: 0x00034C00 File Offset: 0x00032E00
	private void OnAttributesUpdatedOnServer(SyncList<global::Attribute>.Operation op, int itemIndex, global::Attribute oldItem, global::Attribute newItem)
	{
		if (oldItem.Type == AttributeType.Vitality | newItem.Type == AttributeType.Vitality)
		{
			this.NetworkMaxHealth = this.CalculateMaxHealth();
		}
		if (oldItem.Type == AttributeType.Agility | newItem.Type == AttributeType.Agility)
		{
			this.NetworkMaxEndurance = this.CalculateMaxEndurance();
		}
	}

	// Token: 0x17000149 RID: 329
	// (get) Token: 0x06000BC5 RID: 3013 RVA: 0x00034C54 File Offset: 0x00032E54
	public float RegenerationIntervalModifier
	{
		get
		{
			switch (this.vocationModule.Vocation)
			{
			case Vocation.Sentinel:
				return 0.005f;
			case Vocation.Warrior:
				return 0.007f;
			case Vocation.Elementor:
				return 0.003f;
			case Vocation.Protector:
				return 0.009f;
			case Vocation.Lyrus:
				return 0.013f;
			}
			return 0.005f;
		}
	}

	// Token: 0x1700014A RID: 330
	// (get) Token: 0x06000BC6 RID: 3014 RVA: 0x00034CB0 File Offset: 0x00032EB0
	public float RegenerationAmountModifier
	{
		get
		{
			switch (this.vocationModule.Vocation)
			{
			case Vocation.Sentinel:
				return 0.017f;
			case Vocation.Warrior:
				return 0.025f;
			case Vocation.Elementor:
				return 0.012f;
			case Vocation.Protector:
				return 0.032f;
			case Vocation.Lyrus:
				return 0.046f;
			default:
				return 0.025f;
			}
		}
	}

	// Token: 0x1700014B RID: 331
	// (get) Token: 0x06000BC7 RID: 3015 RVA: 0x00034D0C File Offset: 0x00032F0C
	public float CriticalChanceModifier
	{
		get
		{
			switch (this.vocationModule.Vocation)
			{
			case Vocation.Sentinel:
				return 0.0018f;
			case Vocation.Warrior:
			case Vocation.Elementor:
				return 0.0009f;
			case Vocation.Protector:
			case Vocation.Lyrus:
				return 0.0005f;
			default:
				return 0.0007f;
			}
		}
	}

	// Token: 0x1700014C RID: 332
	// (get) Token: 0x06000BC8 RID: 3016 RVA: 0x00034D5C File Offset: 0x00032F5C
	public float CriticalDamageModifier
	{
		get
		{
			switch (this.vocationModule.Vocation)
			{
			case Vocation.Sentinel:
				return 0.007f;
			case Vocation.Warrior:
				return 0.004f;
			case Vocation.Elementor:
			case Vocation.Lyrus:
				return 0.005f;
			case Vocation.Protector:
				return 0.002f;
			default:
				return 0.002f;
			}
		}
	}

	// Token: 0x1700014D RID: 333
	// (get) Token: 0x06000BC9 RID: 3017 RVA: 0x00034DB0 File Offset: 0x00032FB0
	public float EnduranceModifier
	{
		get
		{
			switch (this.vocationModule.Vocation)
			{
			case Vocation.Sentinel:
				return 1.5f;
			case Vocation.Warrior:
			case Vocation.Protector:
				return 1.45f;
			case Vocation.Elementor:
			case Vocation.Lyrus:
				return 1.4f;
			default:
				return 1.2f;
			}
		}
	}

	// Token: 0x1700014E RID: 334
	// (get) Token: 0x06000BCA RID: 3018 RVA: 0x00034E00 File Offset: 0x00033000
	public float AttackModifier
	{
		get
		{
			switch (this.vocationModule.Vocation)
			{
			case Vocation.Sentinel:
			case Vocation.Warrior:
				return 1.7f;
			case Vocation.Elementor:
				return 1.72f;
			case Vocation.Protector:
				return 1.67f;
			case Vocation.Lyrus:
				return 1.72f;
			default:
				return 1.67f;
			}
		}
	}

	// Token: 0x1700014F RID: 335
	// (get) Token: 0x06000BCB RID: 3019 RVA: 0x00034E54 File Offset: 0x00033054
	public float DefenseModifier
	{
		get
		{
			switch (this.vocationModule.Vocation)
			{
			case Vocation.Sentinel:
				return 1.52f;
			case Vocation.Warrior:
				return 1.55f;
			case Vocation.Elementor:
			case Vocation.Lyrus:
				return 1.47f;
			case Vocation.Protector:
				return 1.57f;
			default:
				return 1.55f;
			}
		}
	}

	// Token: 0x17000150 RID: 336
	// (get) Token: 0x06000BCC RID: 3020 RVA: 0x00034EA8 File Offset: 0x000330A8
	public float ResistanceModifier
	{
		get
		{
			switch (this.vocationModule.Vocation)
			{
			case Vocation.Sentinel:
				return 1.65f;
			case Vocation.Warrior:
				return 1.75f;
			case Vocation.Elementor:
			case Vocation.Lyrus:
				return 1.59f;
			case Vocation.Protector:
				return 1.72f;
			default:
				return 1.65f;
			}
		}
	}

	// Token: 0x17000151 RID: 337
	// (get) Token: 0x06000BCD RID: 3021 RVA: 0x00034EFC File Offset: 0x000330FC
	public float SpeedModifier
	{
		get
		{
			switch (this.vocationModule.Vocation)
			{
			case Vocation.Sentinel:
			case Vocation.Lyrus:
				return 0.08f;
			case Vocation.Elementor:
				return 0.06f;
			}
			return 0.07f;
		}
	}

	// Token: 0x17000152 RID: 338
	// (get) Token: 0x06000BCE RID: 3022 RVA: 0x00034F44 File Offset: 0x00033144
	public float HealthModifier
	{
		get
		{
			switch (this.vocationModule.Vocation)
			{
			case Vocation.Sentinel:
				return 42f;
			case Vocation.Warrior:
				return 48f;
			case Vocation.Elementor:
				return 40f;
			case Vocation.Protector:
				return 48f;
			case Vocation.Lyrus:
				return 54f;
			default:
				return 40f;
			}
		}
	}

	// Token: 0x17000153 RID: 339
	// (get) Token: 0x06000BCF RID: 3023 RVA: 0x00034FA0 File Offset: 0x000331A0
	public ExperienceBonus ActiveExperienceBonus
	{
		get
		{
			int b = this.Attributes.Sum((global::Attribute att) => att.Level);
			int playerLevel = Mathf.Max(this.BaseLevel, b);
			return this.experienceBonusTable.FirstOrDefault((ExperienceBonus eb) => playerLevel > eb.MinLevel & playerLevel <= eb.MaxLevel);
		}
	}

	// Token: 0x17000154 RID: 340
	// (get) Token: 0x06000BD0 RID: 3024 RVA: 0x00035007 File Offset: 0x00033207
	public int CurrentEndurance
	{
		get
		{
			if (this.currentEndurance <= this.MaxEndurance)
			{
				return this.currentEndurance;
			}
			return this.MaxEndurance;
		}
	}

	// Token: 0x17000155 RID: 341
	// (get) Token: 0x06000BD1 RID: 3025 RVA: 0x00035024 File Offset: 0x00033224
	public int CurrentHealth
	{
		get
		{
			if (this.currentHealth <= this.MaxHealth)
			{
				return this.currentHealth;
			}
			return this.MaxHealth;
		}
	}

	// Token: 0x17000156 RID: 342
	// (get) Token: 0x06000BD2 RID: 3026 RVA: 0x00035041 File Offset: 0x00033241
	public int EnduranceRegenerationAmount
	{
		get
		{
			return Mathf.RoundToInt((float)this.MaxEndurance * this.EnduranceRegenerationAmountBase);
		}
	}

	// Token: 0x17000157 RID: 343
	// (get) Token: 0x06000BD3 RID: 3027 RVA: 0x0003505B File Offset: 0x0003325B
	public float EnduranceRegenerationInterval
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x17000158 RID: 344
	// (get) Token: 0x06000BD4 RID: 3028 RVA: 0x00035064 File Offset: 0x00033264
	public int RegenerationAmount
	{
		get
		{
			global::Attribute adjustedAttribute = this.GetAdjustedAttribute(AttributeType.Vitality);
			return Mathf.RoundToInt((float)this.MaxHealth * 0.01f + adjustedAttribute.AdjustedValue * this.RegenerationAmountModifier);
		}
	}

	// Token: 0x17000159 RID: 345
	// (get) Token: 0x06000BD5 RID: 3029 RVA: 0x0003509C File Offset: 0x0003329C
	public float RegenerationInterval
	{
		get
		{
			global::Attribute adjustedAttribute = this.GetAdjustedAttribute(AttributeType.Vitality);
			return Mathf.Clamp(this.RegenerationIntervalBase - adjustedAttribute.AdjustedValue * this.RegenerationIntervalModifier, 1.5f, this.RegenerationIntervalBase);
		}
	}

	// Token: 0x1700015A RID: 346
	// (get) Token: 0x06000BD6 RID: 3030 RVA: 0x000350E0 File Offset: 0x000332E0
	public float Attack
	{
		get
		{
			return this.GetAdjustedAttribute(AttributeType.Power).AdjustedValue * this.AttackModifier;
		}
	}

	// Token: 0x1700015B RID: 347
	// (get) Token: 0x06000BD7 RID: 3031 RVA: 0x00035104 File Offset: 0x00033304
	public float Defense
	{
		get
		{
			return this.GetAdjustedAttribute(AttributeType.Toughness).AdjustedValue * this.DefenseModifier;
		}
	}

	// Token: 0x1700015C RID: 348
	// (get) Token: 0x06000BD8 RID: 3032 RVA: 0x00035128 File Offset: 0x00033328
	public float Resistance
	{
		get
		{
			return this.GetAdjustedAttribute(AttributeType.Toughness).AdjustedValue * this.ResistanceModifier;
		}
	}

	// Token: 0x1700015D RID: 349
	// (get) Token: 0x06000BD9 RID: 3033 RVA: 0x0003514C File Offset: 0x0003334C
	public float Speed
	{
		get
		{
			global::Attribute adjustedAttribute = this.GetAdjustedAttribute(AttributeType.Agility);
			float num = this.SpeedBase - this.SpeedBase * adjustedAttribute.CurseModifier;
			num += num * adjustedAttribute.BlessingModifier;
			num = Mathf.Max(0f, num);
			float a = num + adjustedAttribute.AdjustedValue * this.SpeedModifier;
			ObscuredFloat value = this.MaxPlayerSpeed;
			if (this.AccessLevel >= AccessLevel.CommunityManager)
			{
				value = this.StaffMaxPlayerSpeed;
			}
			return Mathf.Min(a, value);
		}
	}

	// Token: 0x1700015E RID: 350
	// (get) Token: 0x06000BDA RID: 3034 RVA: 0x000351CC File Offset: 0x000333CC
	public float CriticalChance
	{
		get
		{
			return this.GetAdjustedAttribute(AttributeType.Precision).AdjustedValue * this.CriticalChanceModifier;
		}
	}

	// Token: 0x1700015F RID: 351
	// (get) Token: 0x06000BDB RID: 3035 RVA: 0x000351F0 File Offset: 0x000333F0
	public float CriticalDamage
	{
		get
		{
			return Mathf.Min(1.8f, this.GetAdjustedAttribute(AttributeType.Power).AdjustedValue * this.CriticalDamageModifier + this.CriticalDamageBase);
		}
	}

	// Token: 0x17000160 RID: 352
	// (get) Token: 0x06000BDC RID: 3036 RVA: 0x0003522C File Offset: 0x0003342C
	public long BaseExperienceToLevel
	{
		get
		{
			long num = 0L;
			for (int i = 1; i <= this.BaseLevel; i++)
			{
				num += (long)(50 * (i * i) - 150 * i + 200);
			}
			return num;
		}
	}

	// Token: 0x17000161 RID: 353
	// (get) Token: 0x06000BDD RID: 3037 RVA: 0x00035266 File Offset: 0x00033466
	public long ProfessionExperienceToLevel
	{
		get
		{
			return this.GetProfessionExperienceToTargetLevel(this.ProfessionLevel);
		}
	}

	// Token: 0x17000162 RID: 354
	// (get) Token: 0x06000BDE RID: 3038 RVA: 0x00035274 File Offset: 0x00033474
	public long BaseExperienceToCurrentLevel
	{
		get
		{
			long num = 0L;
			for (int i = 1; i < this.BaseLevel; i++)
			{
				num += (long)(50 * i * i - 150 * i + 200);
			}
			return num;
		}
	}

	// Token: 0x06000BDF RID: 3039 RVA: 0x000352B0 File Offset: 0x000334B0
	public global::Attribute GetAdjustedAttribute(AttributeType type)
	{
		global::Attribute result = this[type];
		int baseAttributeLevel = AttributeBase.GetBaseAttributeLevel(this.vocationModule.Vocation, type);
		result.BaseLevel += Mathf.RoundToInt((float)(baseAttributeLevel * (this.MasteryLevel + 1)) * 0.3f);
		return result;
	}

	// Token: 0x06000BE0 RID: 3040 RVA: 0x000352FC File Offset: 0x000334FC
	private void InitializeExperienceBonusTable()
	{
		this.experienceBonusTable.Clear();
		this.experienceBonusTable.Add(new ExperienceBonus(0, 10, 1f));
		this.experienceBonusTable.Add(new ExperienceBonus(10, 15, 0.9f));
		this.experienceBonusTable.Add(new ExperienceBonus(15, 20, 0.8f));
		this.experienceBonusTable.Add(new ExperienceBonus(20, 25, 0.7f));
		this.experienceBonusTable.Add(new ExperienceBonus(25, 30, 0.6f));
		this.experienceBonusTable.Add(new ExperienceBonus(30, 35, 0.5f));
		this.experienceBonusTable.Add(new ExperienceBonus(35, 40, 0.3f));
		this.experienceBonusTable.Add(new ExperienceBonus(40, 45, 0.1f));
		this.experienceBonusTable.Add(new ExperienceBonus(45, int.MaxValue, 0f));
	}

	// Token: 0x06000BE1 RID: 3041 RVA: 0x000353F8 File Offset: 0x000335F8
	[Server]
	public void InitializeAttributes(params global::Attribute[] attributes)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AttributeModule::InitializeAttributes(Attribute[])' called when server was not active");
			return;
		}
		int i;
		int j;
		for (i = 0; i < Enum.GetValues(typeof(AttributeType)).Length; i = j + 1)
		{
			global::Attribute attribute = attributes.FirstOrDefault((global::Attribute f) => f.Type == (AttributeType)i);
			int baseAttributeLevel = AttributeBase.GetBaseAttributeLevel(this.vocationModule.Vocation, attribute.Type);
			if (attribute.IsDefined)
			{
				attribute.BaseLevel = baseAttributeLevel;
				this.Attributes.Add(attribute);
			}
			else
			{
				attribute = new global::Attribute(0, 0L, (AttributeType)i)
				{
					BaseLevel = baseAttributeLevel
				};
				this.Attributes.Add(attribute);
			}
			j = i;
		}
		this.NetworkMaxHealth = this.CalculateMaxHealth();
		this.NetworkMaxEndurance = this.CalculateMaxEndurance();
	}

	// Token: 0x06000BE2 RID: 3042 RVA: 0x000354E4 File Offset: 0x000336E4
	[Server]
	public void SetAccessLevel(AccessLevel accessLevel)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AttributeModule::SetAccessLevel(AccessLevel)' called when server was not active");
			return;
		}
		this.NetworkAccessLevel = accessLevel;
	}

	// Token: 0x06000BE3 RID: 3043 RVA: 0x00035504 File Offset: 0x00033704
	[Server]
	private int CalculateMaxHealth()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Int32 AttributeModule::CalculateMaxHealth()' called when server was not active");
			return 0;
		}
		global::Attribute adjustedAttribute = this.GetAdjustedAttribute(AttributeType.Vitality);
		float num = (float)this.healthBase - (float)this.healthBase * adjustedAttribute.CurseModifier;
		num += num * adjustedAttribute.BlessingModifier;
		num = Mathf.Max(0f, num);
		return (int)(num + adjustedAttribute.AdjustedValue * this.HealthModifier);
	}

	// Token: 0x06000BE4 RID: 3044 RVA: 0x0003557C File Offset: 0x0003377C
	[Server]
	private int CalculateMaxEndurance()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Int32 AttributeModule::CalculateMaxEndurance()' called when server was not active");
			return 0;
		}
		if (this.pvpModule.TvtTeam != TvtTeam.None)
		{
			return 150;
		}
		global::Attribute adjustedAttribute = this.GetAdjustedAttribute(AttributeType.Agility);
		float num = this.EnduranceBase - this.EnduranceBase * adjustedAttribute.CurseModifier;
		num += num * adjustedAttribute.BlessingModifier;
		num = Mathf.Max(0f, num);
		return (int)(num + adjustedAttribute.AdjustedValue * this.EnduranceModifier);
	}

	// Token: 0x06000BE5 RID: 3045 RVA: 0x0003560C File Offset: 0x0003380C
	[Server]
	public void SetHealthBase(int healthBase)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AttributeModule::SetHealthBase(System.Int32)' called when server was not active");
			return;
		}
		this.healthBase = healthBase;
	}

	// Token: 0x06000BE6 RID: 3046 RVA: 0x0003562C File Offset: 0x0003382C
	[Server]
	public void SetHealth(int amount)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AttributeModule::SetHealth(System.Int32)' called when server was not active");
			return;
		}
		this.NetworkMaxHealth = this.CalculateMaxHealth();
		int num = (amount > this.MaxHealth) ? this.MaxHealth : amount;
		num = ((num < 0) ? 0 : num);
		this.NetworkcurrentHealth = num;
	}

	// Token: 0x06000BE7 RID: 3047 RVA: 0x0003567F File Offset: 0x0003387F
	[Server]
	public void SetHealthToMax()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AttributeModule::SetHealthToMax()' called when server was not active");
			return;
		}
		this.SetHealth(this.MaxHealth);
	}

	// Token: 0x06000BE8 RID: 3048 RVA: 0x000356A2 File Offset: 0x000338A2
	[Server]
	public void SetEnduranceToMax()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AttributeModule::SetEnduranceToMax()' called when server was not active");
			return;
		}
		this.SetEndurance(this.MaxEndurance);
	}

	// Token: 0x06000BE9 RID: 3049 RVA: 0x000356C8 File Offset: 0x000338C8
	[Server]
	public void AddHealth(GameObject healerObject, int amount, bool shouldStartCombatFlags, EffectConfig healEffect)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AttributeModule::AddHealth(UnityEngine.GameObject,System.Int32,System.Boolean,EffectConfig)' called when server was not active");
			return;
		}
		if (!this.creatureModule.IsAlive)
		{
			return;
		}
		this.NormalizeHealth();
		if (amount > 0)
		{
			if (this.pvpModule.PvpStatus != PvpStatus.Neutral)
			{
				amount = Mathf.RoundToInt((float)amount * 0.2f);
			}
			if (this.pvpModule.TvtTeam != TvtTeam.None)
			{
				int min = Mathf.RoundToInt((float)this.MaxHealth * 0.07f);
				int max = Mathf.RoundToInt((float)this.MaxHealth * 0.13f);
				amount = Mathf.Clamp(amount, min, max);
			}
			if (healEffect.IsDefined)
			{
				healEffect.Text = string.Format("+{0}", amount);
				this.effectModule.ShowEffects(healEffect);
			}
		}
		int num = this.currentHealth + amount;
		num = ((num > this.MaxHealth) ? this.MaxHealth : num);
		num = ((num < 0) ? 0 : num);
		this.NetworkcurrentHealth = num;
		if (shouldStartCombatFlags)
		{
			this.PutHealerInCombatAndStartPvpIfPlayer(healerObject);
			this.combatModule.StartCombatAndFightingFlags();
		}
	}

	// Token: 0x06000BEA RID: 3050 RVA: 0x000357CC File Offset: 0x000339CC
	[Server]
	private void PutHealerInCombatAndStartPvpIfPlayer(GameObject healerObject)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AttributeModule::PutHealerInCombatAndStartPvpIfPlayer(UnityEngine.GameObject)' called when server was not active");
			return;
		}
		if (healerObject == null)
		{
			return;
		}
		if (healerObject == base.gameObject)
		{
			return;
		}
		if (!healerObject.CompareTag("Player"))
		{
			return;
		}
		healerObject.GetComponent<CombatModule>().StartCombatAndFightingFlags();
		if (this.pvpModule.PvpStatus == PvpStatus.Neutral)
		{
			return;
		}
		PvpModule component = healerObject.GetComponent<PvpModule>();
		if (component.PvpStatus == PvpStatus.Neutral)
		{
			component.SetPvpStatusAsync(PvpStatus.InCombat, true);
		}
	}

	// Token: 0x06000BEB RID: 3051 RVA: 0x00035848 File Offset: 0x00033A48
	[Server]
	public void SetEndurance(int amount)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AttributeModule::SetEndurance(System.Int32)' called when server was not active");
			return;
		}
		this.NetworkMaxEndurance = this.CalculateMaxEndurance();
		int num = (amount > this.MaxEndurance) ? this.MaxEndurance : amount;
		num = ((num < 0) ? 0 : num);
		this.NetworkcurrentEndurance = num;
	}

	// Token: 0x06000BEC RID: 3052 RVA: 0x0003589C File Offset: 0x00033A9C
	[Server]
	public void AddEndurance(int amount)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AttributeModule::AddEndurance(System.Int32)' called when server was not active");
			return;
		}
		if (!this.creatureModule.IsAlive)
		{
			return;
		}
		this.NormalizeEndurance();
		int num = this.currentEndurance + amount;
		num = ((num > this.MaxEndurance) ? this.MaxEndurance : num);
		num = ((num < 0) ? 0 : num);
		this.NetworkcurrentEndurance = num;
	}

	// Token: 0x06000BED RID: 3053 RVA: 0x00035900 File Offset: 0x00033B00
	[Server]
	private void AddExperience(AttributeType type, long experience)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AttributeModule::AddExperience(AttributeType,System.Int64)' called when server was not active");
			return;
		}
		if (!this.creatureModule.IsAlive)
		{
			return;
		}
		global::Attribute attribute = this[type];
		attribute.Experience += experience;
		if (attribute.Level >= attribute.MaxLevel & attribute.Experience >= attribute.ExperienceToLevel)
		{
			attribute.Experience = attribute.ExperienceToLevel;
			return;
		}
		while (attribute.Experience >= attribute.ExperienceToLevel)
		{
			attribute.Level++;
			this.effectModule.ShowScreenMessage("attribute_level_up_message", 1, 3.5f, new string[]
			{
				"attribute_type_" + type.ToString().ToLower(),
				attribute.Level.ToString()
			});
		}
		this[type] = attribute;
	}

	// Token: 0x06000BEE RID: 3054 RVA: 0x000359E8 File Offset: 0x00033BE8
	[Server]
	public void AddExperienceToAllAttributes(long experienceToAdd)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AttributeModule::AddExperienceToAllAttributes(System.Int64)' called when server was not active");
			return;
		}
		global::Attribute[] array = (from a in this.Attributes
		where a.Level < a.MaxLevel & (a.Experience < a.ExperienceToLevel | a.ExperienceToLevel == 0L)
		select a).ToArray<global::Attribute>();
		AttributeType[] attributeTypes = (from a in array
		select a.Type).ToArray<AttributeType>();
		foreach (global::Attribute attribute in array)
		{
			float num = AttributeBase.CalculateBaseAttributeLevelRatio(this.vocationModule.Vocation, attribute.Type, attributeTypes);
			long experience = (long)Mathf.Ceil((float)experienceToAdd * num);
			this.AddExperience(attribute.Type, experience);
		}
	}

	// Token: 0x06000BEF RID: 3055 RVA: 0x00035AB0 File Offset: 0x00033CB0
	[Server]
	private long GetUnusedExperience(AttributeType attributeType, long experienceToAdd)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Int64 AttributeModule::GetUnusedExperience(AttributeType,System.Int64)' called when server was not active");
			return 0L;
		}
		global::Attribute attribute = this[attributeType];
		if (attribute.Level >= attribute.MaxLevel & attribute.Experience >= attribute.ExperienceToLevel)
		{
			return experienceToAdd;
		}
		return 0L;
	}

	// Token: 0x06000BF0 RID: 3056 RVA: 0x00035B14 File Offset: 0x00033D14
	[Server]
	public void RemoveExperience(AttributeType type, long experience)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AttributeModule::RemoveExperience(AttributeType,System.Int64)' called when server was not active");
			return;
		}
		global::Attribute attribute = this[type];
		experience = (long)Mathf.Min((float)experience, (float)attribute.Experience);
		if (experience <= 0L)
		{
			return;
		}
		attribute.Experience -= experience;
		attribute.Experience = ((attribute.Experience < 0L) ? 0L : attribute.Experience);
		bool flag = false;
		while (attribute.Experience < attribute.ExperienceToCurrentLevel)
		{
			attribute.Level--;
			attribute.Level = ((attribute.Level < 0) ? 0 : attribute.Level);
			flag = true;
		}
		this[type] = attribute;
		this.effectModule.ShowScreenMessage("player_attribute_axp_lost_message", 3, 3.5f, new string[]
		{
			experience.ToString(),
			"attribute_type_" + attribute.Type.ToString().ToLower()
		});
		if (flag)
		{
			this.effectModule.ShowScreenMessage("player_attribute_level_down_message", 3, 3.5f, new string[]
			{
				"attribute_type_" + attribute.Type.ToString().ToLower(),
				attribute.Level.ToString()
			});
		}
	}

	// Token: 0x06000BF1 RID: 3057 RVA: 0x00035C5C File Offset: 0x00033E5C
	[Server]
	public void AddLevelBlessing(AttributeType type, int amount, bool mustBeAlive)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AttributeModule::AddLevelBlessing(AttributeType,System.Int32,System.Boolean)' called when server was not active");
			return;
		}
		if (mustBeAlive && !this.creatureModule.IsAlive)
		{
			return;
		}
		global::Attribute attribute = this[type];
		attribute.LevelBlessingModifier += amount;
		attribute.LevelBlessingModifier = ((attribute.LevelBlessingModifier < 0) ? 0 : attribute.LevelBlessingModifier);
		this[type] = attribute;
	}

	// Token: 0x06000BF2 RID: 3058 RVA: 0x00035CC4 File Offset: 0x00033EC4
	[Server]
	public void AddLevelCurse(AttributeType type, int amount, bool mustBeAlive)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AttributeModule::AddLevelCurse(AttributeType,System.Int32,System.Boolean)' called when server was not active");
			return;
		}
		if (mustBeAlive && !this.creatureModule.IsAlive)
		{
			return;
		}
		global::Attribute attribute = this[type];
		attribute.LevelCurseModifier += amount;
		attribute.LevelCurseModifier = ((attribute.LevelCurseModifier < 0) ? 0 : attribute.LevelCurseModifier);
		this[type] = attribute;
	}

	// Token: 0x06000BF3 RID: 3059 RVA: 0x00035D2C File Offset: 0x00033F2C
	[Server]
	public void AddBlessing(AttributeType type, float amount, bool mustBeAlive)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AttributeModule::AddBlessing(AttributeType,System.Single,System.Boolean)' called when server was not active");
			return;
		}
		if (mustBeAlive && !this.creatureModule.IsAlive)
		{
			return;
		}
		global::Attribute attribute = this[type];
		attribute.BlessingModifier += amount;
		attribute.BlessingModifier = ((attribute.BlessingModifier < 0f) ? 0f : attribute.BlessingModifier);
		this[type] = attribute;
	}

	// Token: 0x06000BF4 RID: 3060 RVA: 0x00035D9C File Offset: 0x00033F9C
	[Server]
	public void AddCurse(AttributeType type, float amount, bool mustBeAlive)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AttributeModule::AddCurse(AttributeType,System.Single,System.Boolean)' called when server was not active");
			return;
		}
		if (mustBeAlive && !this.creatureModule.IsAlive)
		{
			return;
		}
		global::Attribute attribute = this[type];
		attribute.CurseModifier += amount;
		attribute.CurseModifier = ((attribute.CurseModifier < 0f) ? 0f : attribute.CurseModifier);
		this[type] = attribute;
	}

	// Token: 0x06000BF5 RID: 3061 RVA: 0x00035E0C File Offset: 0x0003400C
	[Server]
	public void SetLevel(AttributeType type, int level)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AttributeModule::SetLevel(AttributeType,System.Int32)' called when server was not active");
			return;
		}
		global::Attribute value = this[type];
		if (level < 0)
		{
			value.Level = 0;
			value.Experience = 0L;
		}
		else
		{
			value.Level = level;
		}
		this[type] = value;
	}

	// Token: 0x06000BF6 RID: 3062 RVA: 0x00035E60 File Offset: 0x00034060
	[Server]
	public void AddLevel(AttributeType type, int amount)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AttributeModule::AddLevel(AttributeType,System.Int32)' called when server was not active");
			return;
		}
		if (!this.creatureModule.IsAlive)
		{
			return;
		}
		global::Attribute attribute = this[type];
		if (attribute.Level + amount < 0)
		{
			attribute.Level = 0;
			attribute.Experience = 0L;
		}
		else
		{
			long num = attribute.Experience - attribute.ExperienceToCurrentLevel;
			attribute.Level += amount;
			attribute.Experience = attribute.ExperienceToCurrentLevel + num;
		}
		this[type] = attribute;
	}

	// Token: 0x06000BF7 RID: 3063 RVA: 0x00035EEC File Offset: 0x000340EC
	[Server]
	public void SetExperience(AttributeType type, long experience)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AttributeModule::SetExperience(AttributeType,System.Int64)' called when server was not active");
			return;
		}
		global::Attribute value = this[type];
		value.Experience = experience;
		this[type] = value;
	}

	// Token: 0x06000BF8 RID: 3064 RVA: 0x00035F26 File Offset: 0x00034126
	[Server]
	private void NormalizeEndurance()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AttributeModule::NormalizeEndurance()' called when server was not active");
			return;
		}
		this.NetworkcurrentEndurance = ((this.currentEndurance > this.MaxEndurance) ? this.MaxEndurance : this.currentEndurance);
	}

	// Token: 0x06000BF9 RID: 3065 RVA: 0x00035F5F File Offset: 0x0003415F
	[Server]
	private void NormalizeHealth()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AttributeModule::NormalizeHealth()' called when server was not active");
			return;
		}
		this.NetworkcurrentHealth = ((this.currentHealth > this.MaxHealth) ? this.MaxHealth : this.currentHealth);
	}

	// Token: 0x06000BFA RID: 3066 RVA: 0x00035F98 File Offset: 0x00034198
	[TargetRpc]
	private void TargetSendAttributeExperienceToLog(long experience)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteLong(experience);
		this.SendTargetRPCInternal(null, "System.Void AttributeModule::TargetSendAttributeExperienceToLog(System.Int64)", -1156063693, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000BFB RID: 3067 RVA: 0x00035FD4 File Offset: 0x000341D4
	[Client]
	private void OnBaseExperience(long oldValue, long newValue)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void AttributeModule::OnBaseExperience(System.Int64,System.Int64)' called when client was not active");
			return;
		}
		this.NetworkBaseExperience = newValue;
		long experience = newValue - oldValue;
		if (oldValue == 0L)
		{
			return;
		}
		this.experienceLog.RemoveAll((ExperienceData el) => Time.time - el.GameTime > 180f);
		this.experienceLog.Add(new ExperienceData
		{
			Experience = experience,
			GameTime = Time.time
		});
		if (this.experienceLog.Count > 0)
		{
			long num = this.experienceLog.Sum((ExperienceData el) => el.Experience);
			this.ExperiencePerMinute = num / 3L;
		}
	}

	// Token: 0x06000BFC RID: 3068 RVA: 0x00036095 File Offset: 0x00034295
	[Server]
	public void SetMasteryLevel(int level)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AttributeModule::SetMasteryLevel(System.Int32)' called when server was not active");
			return;
		}
		this.NetworkMasteryLevel = level;
	}

	// Token: 0x06000BFD RID: 3069 RVA: 0x000360B3 File Offset: 0x000342B3
	[Server]
	public void IncrementMasteryLevel(int amount)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AttributeModule::IncrementMasteryLevel(System.Int32)' called when server was not active");
			return;
		}
		this.NetworkMasteryLevel = this.MasteryLevel + amount;
	}

	// Token: 0x06000BFE RID: 3070 RVA: 0x000360D8 File Offset: 0x000342D8
	[Server]
	public void SetBaseLevel(int level)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AttributeModule::SetBaseLevel(System.Int32)' called when server was not active");
			return;
		}
		this.NetworkBaseLevel = level;
	}

	// Token: 0x06000BFF RID: 3071 RVA: 0x000360F8 File Offset: 0x000342F8
	[Server]
	public void RemoveBaseExperience(long experience, bool invokeEvents)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AttributeModule::RemoveBaseExperience(System.Int64,System.Boolean)' called when server was not active");
			return;
		}
		experience = (long)Mathf.Min((float)experience, (float)this.BaseExperience);
		if (experience <= 0L)
		{
			return;
		}
		this.NetworkBaseExperience = this.BaseExperience - experience;
		this.NetworkBaseExperience = ((this.BaseExperience < 0L) ? 0L : this.BaseExperience);
		bool flag = false;
		while (this.BaseExperience < this.BaseExperienceToCurrentLevel)
		{
			this.NetworkBaseLevel = this.BaseLevel - 1;
			this.NetworkBaseLevel = ((this.BaseLevel < 0) ? 0 : this.BaseLevel);
			flag = true;
		}
		this.effectModule.ShowScreenMessage("player_exp_lost_message", 3, 3.5f, new string[]
		{
			experience.ToString()
		});
		if (flag)
		{
			this.effectModule.ShowScreenMessage("player_level_down_message", 3, 3.5f, new string[]
			{
				this.BaseLevel.ToString()
			});
		}
	}

	// Token: 0x06000C00 RID: 3072 RVA: 0x000361E8 File Offset: 0x000343E8
	[Server]
	public void AddBaseExperience(long experience, bool invokeEvents)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AttributeModule::AddBaseExperience(System.Int64,System.Boolean)' called when server was not active");
			return;
		}
		bool flag = false;
		this.NetworkBaseExperience = this.BaseExperience + experience;
		if (this.BaseLevel >= 2147483647 & this.BaseExperience >= this.BaseExperienceToLevel)
		{
			this.NetworkBaseExperience = this.BaseExperienceToLevel;
			return;
		}
		int num = 0;
		while (this.BaseExperience >= this.BaseExperienceToLevel & this.BaseLevel < 2147483647)
		{
			this.NetworkBaseLevel = this.BaseLevel + 1;
			num++;
			flag = true;
		}
		if (flag && invokeEvents && this.OnBaseLevelUp != null)
		{
			this.OnBaseLevelUp(num);
		}
	}

	// Token: 0x06000C01 RID: 3073 RVA: 0x0003629D File Offset: 0x0003449D
	[Server]
	public void SetBaseExperience(long experience)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AttributeModule::SetBaseExperience(System.Int64)' called when server was not active");
			return;
		}
		this.NetworkBaseExperience = experience;
	}

	// Token: 0x06000C02 RID: 3074 RVA: 0x000362BC File Offset: 0x000344BC
	[Command]
	public void CmdSetTrainingMode(TrainingMode trainingMode)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		Mirror.GeneratedNetworkCode._Write_TrainingMode(writer, trainingMode);
		base.SendCommandInternal("System.Void AttributeModule::CmdSetTrainingMode(TrainingMode)", -626844563, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000C03 RID: 3075 RVA: 0x000362F6 File Offset: 0x000344F6
	[Server]
	public void SetTrainingMode(TrainingMode trainingMode)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AttributeModule::SetTrainingMode(TrainingMode)' called when server was not active");
			return;
		}
		this.NetworkTrainingMode = trainingMode;
	}

	// Token: 0x06000C04 RID: 3076 RVA: 0x00036314 File Offset: 0x00034514
	private void OnTrainingMode(TrainingMode oldValue, TrainingMode newValue)
	{
		this.NetworkTrainingMode = newValue;
		if (!base.isLocalPlayer)
		{
			return;
		}
		switch (this.TrainingMode)
		{
		case TrainingMode.Balanced:
			this.effectModule.ShowScreenMessage("training_mode_balanced_message", 0, 3.5f, Array.Empty<string>());
			return;
		case TrainingMode.AxpFocused:
			this.effectModule.ShowScreenMessage("training_mode_axp_focused_message", 0, 3.5f, Array.Empty<string>());
			return;
		case TrainingMode.ExpFocused:
			this.effectModule.ShowScreenMessage("training_mode_exp_focused_message", 0, 3.5f, Array.Empty<string>());
			return;
		default:
			return;
		}
	}

	// Token: 0x06000C05 RID: 3077 RVA: 0x000363A0 File Offset: 0x000345A0
	public long GetProfessionExperienceToTargetLevel(int targetLevel)
	{
		long num = 0L;
		for (int i = 1; i <= targetLevel; i++)
		{
			num += (long)(50 * i * i - 150 * i + 200);
		}
		return num;
	}

	// Token: 0x06000C06 RID: 3078 RVA: 0x000363D5 File Offset: 0x000345D5
	[Server]
	public void SetProfession(PlayerProfession profession)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AttributeModule::SetProfession(PlayerProfession)' called when server was not active");
			return;
		}
		this.NetworkProfession = profession;
	}

	// Token: 0x06000C07 RID: 3079 RVA: 0x000363F3 File Offset: 0x000345F3
	[Server]
	public void SetProfessionLevel(int level)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AttributeModule::SetProfessionLevel(System.Int32)' called when server was not active");
			return;
		}
		this.NetworkProfessionLevel = level;
	}

	// Token: 0x06000C08 RID: 3080 RVA: 0x00036411 File Offset: 0x00034611
	[Server]
	public void SetProfessionExperience(long experience)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AttributeModule::SetProfessionExperience(System.Int64)' called when server was not active");
			return;
		}
		this.NetworkProfessionExperience = experience;
	}

	// Token: 0x06000C09 RID: 3081 RVA: 0x00036430 File Offset: 0x00034630
	[Server]
	public void AddProfessionExperience(long experience, bool invokeEvents)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void AttributeModule::AddProfessionExperience(System.Int64,System.Boolean)' called when server was not active");
			return;
		}
		bool flag = false;
		this.NetworkProfessionExperience = this.ProfessionExperience + experience;
		if (this.ProfessionLevel >= 850 & this.ProfessionExperience >= this.ProfessionExperienceToLevel)
		{
			this.NetworkProfessionExperience = this.ProfessionExperienceToLevel;
			return;
		}
		int num = 0;
		while (this.ProfessionExperience >= this.ProfessionExperienceToLevel & this.ProfessionLevel < 850)
		{
			this.NetworkProfessionLevel = this.ProfessionLevel + 1;
			num++;
			flag = true;
		}
		if (flag && invokeEvents && this.OnProfessionLevelUp != null)
		{
			this.OnProfessionLevelUp(num);
		}
	}

	// Token: 0x06000C0A RID: 3082 RVA: 0x000364E8 File Offset: 0x000346E8
	public AttributeModule()
	{
		base.InitSyncObject(this.Attributes);
	}

	// Token: 0x06000C0B RID: 3083 RVA: 0x0002FEC9 File Offset: 0x0002E0C9
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x17000163 RID: 355
	// (get) Token: 0x06000C0C RID: 3084 RVA: 0x00036594 File Offset: 0x00034794
	// (set) Token: 0x06000C0D RID: 3085 RVA: 0x000365A7 File Offset: 0x000347A7
	public int NetworkcurrentEndurance
	{
		get
		{
			return this.currentEndurance;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.currentEndurance, 1UL, null);
		}
	}

	// Token: 0x17000164 RID: 356
	// (get) Token: 0x06000C0E RID: 3086 RVA: 0x000365C4 File Offset: 0x000347C4
	// (set) Token: 0x06000C0F RID: 3087 RVA: 0x000365D7 File Offset: 0x000347D7
	public int NetworkcurrentHealth
	{
		get
		{
			return this.currentHealth;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.currentHealth, 2UL, null);
		}
	}

	// Token: 0x17000165 RID: 357
	// (get) Token: 0x06000C10 RID: 3088 RVA: 0x000365F4 File Offset: 0x000347F4
	// (set) Token: 0x06000C11 RID: 3089 RVA: 0x00036607 File Offset: 0x00034807
	public int NetworkMasteryLevel
	{
		get
		{
			return this.MasteryLevel;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.MasteryLevel, 4UL, null);
		}
	}

	// Token: 0x17000166 RID: 358
	// (get) Token: 0x06000C12 RID: 3090 RVA: 0x00036624 File Offset: 0x00034824
	// (set) Token: 0x06000C13 RID: 3091 RVA: 0x00036637 File Offset: 0x00034837
	public int NetworkMaxHealth
	{
		get
		{
			return this.MaxHealth;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.MaxHealth, 8UL, null);
		}
	}

	// Token: 0x17000167 RID: 359
	// (get) Token: 0x06000C14 RID: 3092 RVA: 0x00036654 File Offset: 0x00034854
	// (set) Token: 0x06000C15 RID: 3093 RVA: 0x00036667 File Offset: 0x00034867
	public int NetworkMaxEndurance
	{
		get
		{
			return this.MaxEndurance;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.MaxEndurance, 16UL, null);
		}
	}

	// Token: 0x17000168 RID: 360
	// (get) Token: 0x06000C16 RID: 3094 RVA: 0x00036684 File Offset: 0x00034884
	// (set) Token: 0x06000C17 RID: 3095 RVA: 0x00036697 File Offset: 0x00034897
	public int NetworkBaseLevel
	{
		get
		{
			return this.BaseLevel;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.BaseLevel, 32UL, null);
		}
	}

	// Token: 0x17000169 RID: 361
	// (get) Token: 0x06000C18 RID: 3096 RVA: 0x000366B4 File Offset: 0x000348B4
	// (set) Token: 0x06000C19 RID: 3097 RVA: 0x000366C7 File Offset: 0x000348C7
	public long NetworkBaseExperience
	{
		get
		{
			return this.BaseExperience;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<long>(value, ref this.BaseExperience, 64UL, new Action<long, long>(this.OnBaseExperience));
		}
	}

	// Token: 0x1700016A RID: 362
	// (get) Token: 0x06000C1A RID: 3098 RVA: 0x000366EC File Offset: 0x000348EC
	// (set) Token: 0x06000C1B RID: 3099 RVA: 0x000366FF File Offset: 0x000348FF
	public PlayerProfession NetworkProfession
	{
		get
		{
			return this.Profession;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<PlayerProfession>(value, ref this.Profession, 128UL, null);
		}
	}

	// Token: 0x1700016B RID: 363
	// (get) Token: 0x06000C1C RID: 3100 RVA: 0x0003671C File Offset: 0x0003491C
	// (set) Token: 0x06000C1D RID: 3101 RVA: 0x0003672F File Offset: 0x0003492F
	public int NetworkProfessionLevel
	{
		get
		{
			return this.ProfessionLevel;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.ProfessionLevel, 256UL, null);
		}
	}

	// Token: 0x1700016C RID: 364
	// (get) Token: 0x06000C1E RID: 3102 RVA: 0x0003674C File Offset: 0x0003494C
	// (set) Token: 0x06000C1F RID: 3103 RVA: 0x0003675F File Offset: 0x0003495F
	public long NetworkProfessionExperience
	{
		get
		{
			return this.ProfessionExperience;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<long>(value, ref this.ProfessionExperience, 512UL, null);
		}
	}

	// Token: 0x1700016D RID: 365
	// (get) Token: 0x06000C20 RID: 3104 RVA: 0x0003677C File Offset: 0x0003497C
	// (set) Token: 0x06000C21 RID: 3105 RVA: 0x0003678F File Offset: 0x0003498F
	public AccessLevel NetworkAccessLevel
	{
		get
		{
			return this.AccessLevel;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<AccessLevel>(value, ref this.AccessLevel, 1024UL, null);
		}
	}

	// Token: 0x1700016E RID: 366
	// (get) Token: 0x06000C22 RID: 3106 RVA: 0x000367AC File Offset: 0x000349AC
	// (set) Token: 0x06000C23 RID: 3107 RVA: 0x000367BF File Offset: 0x000349BF
	public TrainingMode NetworkTrainingMode
	{
		get
		{
			return this.TrainingMode;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<TrainingMode>(value, ref this.TrainingMode, 2048UL, new Action<TrainingMode, TrainingMode>(this.OnTrainingMode));
		}
	}

	// Token: 0x06000C24 RID: 3108 RVA: 0x000367E4 File Offset: 0x000349E4
	protected void UserCode_TargetSendAttributeExperienceToLog__Int64(long experience)
	{
		if (experience == 0L)
		{
			return;
		}
		this.attributeExperienceLog.RemoveAll((ExperienceData el) => Time.time - el.GameTime > 180f);
		this.attributeExperienceLog.Add(new ExperienceData
		{
			Experience = experience,
			GameTime = Time.time
		});
		if (this.attributeExperienceLog.Count > 0)
		{
			long num = this.attributeExperienceLog.Sum((ExperienceData el) => el.Experience);
			this.AttributeExperiencePerMinute = num / 3L;
		}
	}

	// Token: 0x06000C25 RID: 3109 RVA: 0x00036885 File Offset: 0x00034A85
	protected static void InvokeUserCode_TargetSendAttributeExperienceToLog__Int64(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetSendAttributeExperienceToLog called on server.");
			return;
		}
		((AttributeModule)obj).UserCode_TargetSendAttributeExperienceToLog__Int64(reader.ReadLong());
	}

	// Token: 0x06000C26 RID: 3110 RVA: 0x000368AE File Offset: 0x00034AAE
	protected void UserCode_CmdSetTrainingMode__TrainingMode(TrainingMode trainingMode)
	{
		this.SetTrainingMode(trainingMode);
	}

	// Token: 0x06000C27 RID: 3111 RVA: 0x000368B7 File Offset: 0x00034AB7
	protected static void InvokeUserCode_CmdSetTrainingMode__TrainingMode(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSetTrainingMode called on client.");
			return;
		}
		((AttributeModule)obj).UserCode_CmdSetTrainingMode__TrainingMode(Mirror.GeneratedNetworkCode._Read_TrainingMode(reader));
	}

	// Token: 0x06000C28 RID: 3112 RVA: 0x000368E0 File Offset: 0x00034AE0
	static AttributeModule()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(AttributeModule), "System.Void AttributeModule::CmdSetTrainingMode(TrainingMode)", new RemoteCallDelegate(AttributeModule.InvokeUserCode_CmdSetTrainingMode__TrainingMode), true);
		RemoteProcedureCalls.RegisterRpc(typeof(AttributeModule), "System.Void AttributeModule::TargetSendAttributeExperienceToLog(System.Int64)", new RemoteCallDelegate(AttributeModule.InvokeUserCode_TargetSendAttributeExperienceToLog__Int64));
	}

	// Token: 0x06000C29 RID: 3113 RVA: 0x00036930 File Offset: 0x00034B30
	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteInt(this.currentEndurance);
			writer.WriteInt(this.currentHealth);
			writer.WriteInt(this.MasteryLevel);
			writer.WriteInt(this.MaxHealth);
			writer.WriteInt(this.MaxEndurance);
			writer.WriteInt(this.BaseLevel);
			writer.WriteLong(this.BaseExperience);
			Mirror.GeneratedNetworkCode._Write_PlayerProfession(writer, this.Profession);
			writer.WriteInt(this.ProfessionLevel);
			writer.WriteLong(this.ProfessionExperience);
			Mirror.GeneratedNetworkCode._Write_AccessLevel(writer, this.AccessLevel);
			Mirror.GeneratedNetworkCode._Write_TrainingMode(writer, this.TrainingMode);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1UL) != 0UL)
		{
			writer.WriteInt(this.currentEndurance);
		}
		if ((base.syncVarDirtyBits & 2UL) != 0UL)
		{
			writer.WriteInt(this.currentHealth);
		}
		if ((base.syncVarDirtyBits & 4UL) != 0UL)
		{
			writer.WriteInt(this.MasteryLevel);
		}
		if ((base.syncVarDirtyBits & 8UL) != 0UL)
		{
			writer.WriteInt(this.MaxHealth);
		}
		if ((base.syncVarDirtyBits & 16UL) != 0UL)
		{
			writer.WriteInt(this.MaxEndurance);
		}
		if ((base.syncVarDirtyBits & 32UL) != 0UL)
		{
			writer.WriteInt(this.BaseLevel);
		}
		if ((base.syncVarDirtyBits & 64UL) != 0UL)
		{
			writer.WriteLong(this.BaseExperience);
		}
		if ((base.syncVarDirtyBits & 128UL) != 0UL)
		{
			Mirror.GeneratedNetworkCode._Write_PlayerProfession(writer, this.Profession);
		}
		if ((base.syncVarDirtyBits & 256UL) != 0UL)
		{
			writer.WriteInt(this.ProfessionLevel);
		}
		if ((base.syncVarDirtyBits & 512UL) != 0UL)
		{
			writer.WriteLong(this.ProfessionExperience);
		}
		if ((base.syncVarDirtyBits & 1024UL) != 0UL)
		{
			Mirror.GeneratedNetworkCode._Write_AccessLevel(writer, this.AccessLevel);
		}
		if ((base.syncVarDirtyBits & 2048UL) != 0UL)
		{
			Mirror.GeneratedNetworkCode._Write_TrainingMode(writer, this.TrainingMode);
		}
	}

	// Token: 0x06000C2A RID: 3114 RVA: 0x00036B84 File Offset: 0x00034D84
	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.currentEndurance, null, reader.ReadInt());
			base.GeneratedSyncVarDeserialize<int>(ref this.currentHealth, null, reader.ReadInt());
			base.GeneratedSyncVarDeserialize<int>(ref this.MasteryLevel, null, reader.ReadInt());
			base.GeneratedSyncVarDeserialize<int>(ref this.MaxHealth, null, reader.ReadInt());
			base.GeneratedSyncVarDeserialize<int>(ref this.MaxEndurance, null, reader.ReadInt());
			base.GeneratedSyncVarDeserialize<int>(ref this.BaseLevel, null, reader.ReadInt());
			base.GeneratedSyncVarDeserialize<long>(ref this.BaseExperience, new Action<long, long>(this.OnBaseExperience), reader.ReadLong());
			base.GeneratedSyncVarDeserialize<PlayerProfession>(ref this.Profession, null, Mirror.GeneratedNetworkCode._Read_PlayerProfession(reader));
			base.GeneratedSyncVarDeserialize<int>(ref this.ProfessionLevel, null, reader.ReadInt());
			base.GeneratedSyncVarDeserialize<long>(ref this.ProfessionExperience, null, reader.ReadLong());
			base.GeneratedSyncVarDeserialize<AccessLevel>(ref this.AccessLevel, null, Mirror.GeneratedNetworkCode._Read_AccessLevel(reader));
			base.GeneratedSyncVarDeserialize<TrainingMode>(ref this.TrainingMode, new Action<TrainingMode, TrainingMode>(this.OnTrainingMode), Mirror.GeneratedNetworkCode._Read_TrainingMode(reader));
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 1L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.currentEndurance, null, reader.ReadInt());
		}
		if ((num & 2L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.currentHealth, null, reader.ReadInt());
		}
		if ((num & 4L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.MasteryLevel, null, reader.ReadInt());
		}
		if ((num & 8L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.MaxHealth, null, reader.ReadInt());
		}
		if ((num & 16L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.MaxEndurance, null, reader.ReadInt());
		}
		if ((num & 32L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.BaseLevel, null, reader.ReadInt());
		}
		if ((num & 64L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<long>(ref this.BaseExperience, new Action<long, long>(this.OnBaseExperience), reader.ReadLong());
		}
		if ((num & 128L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<PlayerProfession>(ref this.Profession, null, Mirror.GeneratedNetworkCode._Read_PlayerProfession(reader));
		}
		if ((num & 256L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.ProfessionLevel, null, reader.ReadInt());
		}
		if ((num & 512L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<long>(ref this.ProfessionExperience, null, reader.ReadLong());
		}
		if ((num & 1024L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<AccessLevel>(ref this.AccessLevel, null, Mirror.GeneratedNetworkCode._Read_AccessLevel(reader));
		}
		if ((num & 2048L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<TrainingMode>(ref this.TrainingMode, new Action<TrainingMode, TrainingMode>(this.OnTrainingMode), Mirror.GeneratedNetworkCode._Read_TrainingMode(reader));
		}
	}

	// Token: 0x04000CF0 RID: 3312
	public readonly ObscuredFloat MaxPlayerSpeed = 7f;

	// Token: 0x04000CF1 RID: 3313
	public readonly ObscuredFloat StaffMaxPlayerSpeed = 15f;

	// Token: 0x04000CF2 RID: 3314
	public readonly ObscuredFloat SpeedBase = 2.75f;

	// Token: 0x04000CF3 RID: 3315
	public readonly ObscuredFloat CriticalDamageBase = 1.1f;

	// Token: 0x04000CF4 RID: 3316
	public readonly ObscuredFloat EnduranceBase = 100f;

	// Token: 0x04000CF5 RID: 3317
	public readonly ObscuredFloat EnduranceRegenerationAmountBase = 0.05f;

	// Token: 0x04000CF6 RID: 3318
	public readonly ObscuredFloat RegenerationIntervalBase = 5f;

	// Token: 0x04000CF7 RID: 3319
	[SyncVar]
	private int currentEndurance;

	// Token: 0x04000CF8 RID: 3320
	[SyncVar]
	private int currentHealth;

	// Token: 0x04000CF9 RID: 3321
	[SyncVar]
	public int MasteryLevel;

	// Token: 0x04000CFA RID: 3322
	[SyncVar]
	public int MaxHealth;

	// Token: 0x04000CFB RID: 3323
	[SyncVar]
	public int MaxEndurance;

	// Token: 0x04000CFC RID: 3324
	[SyncVar]
	public int BaseLevel = 1;

	// Token: 0x04000CFD RID: 3325
	[SyncVar(hook = "OnBaseExperience")]
	public long BaseExperience;

	// Token: 0x04000D00 RID: 3328
	[SyncVar]
	public PlayerProfession Profession;

	// Token: 0x04000D01 RID: 3329
	[SyncVar]
	public int ProfessionLevel;

	// Token: 0x04000D02 RID: 3330
	[SyncVar]
	public long ProfessionExperience;

	// Token: 0x04000D03 RID: 3331
	[SyncVar]
	public AccessLevel AccessLevel;

	// Token: 0x04000D04 RID: 3332
	[SyncVar(hook = "OnTrainingMode")]
	public TrainingMode TrainingMode;

	// Token: 0x04000D05 RID: 3333
	private int healthBase;

	// Token: 0x04000D08 RID: 3336
	public List<ExperienceData> experienceLog;

	// Token: 0x04000D09 RID: 3337
	public List<ExperienceData> attributeExperienceLog;

	// Token: 0x04000D0A RID: 3338
	public List<ExperienceBonus> experienceBonusTable = new List<ExperienceBonus>();

	// Token: 0x04000D0B RID: 3339
	public readonly SyncListAttribute Attributes = new SyncListAttribute();

	// Token: 0x04000D0C RID: 3340
	private PvpModule pvpModule;

	// Token: 0x04000D0D RID: 3341
	private CombatModule combatModule;

	// Token: 0x04000D0E RID: 3342
	private EffectModule effectModule;

	// Token: 0x04000D0F RID: 3343
	private CreatureModule creatureModule;

	// Token: 0x04000D10 RID: 3344
	private VocationModule vocationModule;

	// Token: 0x020002BE RID: 702
	// (Invoke) Token: 0x06000C2C RID: 3116
	public delegate void OnBaseLevelUpEventHandler(int levelsGain);

	// Token: 0x020002BF RID: 703
	// (Invoke) Token: 0x06000C30 RID: 3120
	public delegate void OnBaseLevelDownEventHandler(int levelsLost);

	// Token: 0x020002C0 RID: 704
	// (Invoke) Token: 0x06000C34 RID: 3124
	public delegate void OnProfessionLevelUpEventHandler(int levelsGain);
}
