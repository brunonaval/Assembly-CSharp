using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using CodeStage.AntiCheat.ObscuredTypes;
using Mirror;
using UnityEngine;

// Token: 0x02000396 RID: 918
public class NonPlayerAttributeModule : NetworkBehaviour
{
	// Token: 0x060012BE RID: 4798 RVA: 0x0005C768 File Offset: 0x0005A968
	private void Awake()
	{
		base.TryGetComponent<CreatureModule>(out this.creatureModule);
		base.TryGetComponent<ConditionModule>(out this.conditionModule);
	}

	// Token: 0x060012BF RID: 4799 RVA: 0x0005C784 File Offset: 0x0005A984
	public override void OnStartServer()
	{
		this.Attributes.Callback += this.OnAttributesUpdated;
		if (this.attributesCache.Count != 0)
		{
			this.Attributes.Clear();
			this.Attributes.AddRange(this.attributesCache);
		}
		this.AddAttributeRankBlessings(this.creatureModule.Rank);
		this.NetworkMaxHealth = this.CalculateMaxHealth();
		this.ResetHealth();
	}

	// Token: 0x060012C0 RID: 4800 RVA: 0x0005C7F4 File Offset: 0x0005A9F4
	public override void OnStopServer()
	{
		this.Attributes.Callback -= this.OnAttributesUpdated;
		if (this.Attributes.Count != 0)
		{
			this.attributesCache = new List<global::Attribute>(this.Attributes);
		}
	}

	// Token: 0x170001FD RID: 509
	public global::Attribute this[AttributeType type]
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
				Debug.LogWarning("[Server] function 'System.Void NonPlayerAttributeModule::set_Item(AttributeType,Attribute)' called when server was not active");
				return;
			}
			for (int i = 0; i < this.Attributes.Count; i++)
			{
				if (this.Attributes[i].Type == type)
				{
					this.Attributes[i] = value;
					return;
				}
			}
		}
	}

	// Token: 0x170001FE RID: 510
	// (get) Token: 0x060012C3 RID: 4803 RVA: 0x0005C8D6 File Offset: 0x0005AAD6
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

	// Token: 0x170001FF RID: 511
	// (get) Token: 0x060012C4 RID: 4804 RVA: 0x0005C8F3 File Offset: 0x0005AAF3
	// (set) Token: 0x060012C5 RID: 4805 RVA: 0x0005C8FB File Offset: 0x0005AAFB
	public int RegenerationAmount { get; private set; }

	// Token: 0x17000200 RID: 512
	// (get) Token: 0x060012C6 RID: 4806 RVA: 0x0005C904 File Offset: 0x0005AB04
	// (set) Token: 0x060012C7 RID: 4807 RVA: 0x0005C90C File Offset: 0x0005AB0C
	public float RegenerationInterval { get; private set; }

	// Token: 0x17000201 RID: 513
	// (get) Token: 0x060012C8 RID: 4808 RVA: 0x0005C915 File Offset: 0x0005AB15
	// (set) Token: 0x060012C9 RID: 4809 RVA: 0x0005C91D File Offset: 0x0005AB1D
	public float Speed { get; private set; }

	// Token: 0x17000202 RID: 514
	// (get) Token: 0x060012CA RID: 4810 RVA: 0x0005C926 File Offset: 0x0005AB26
	// (set) Token: 0x060012CB RID: 4811 RVA: 0x0005C92E File Offset: 0x0005AB2E
	public float CriticalChance { get; private set; }

	// Token: 0x17000203 RID: 515
	// (get) Token: 0x060012CC RID: 4812 RVA: 0x0005C937 File Offset: 0x0005AB37
	// (set) Token: 0x060012CD RID: 4813 RVA: 0x0005C93F File Offset: 0x0005AB3F
	public float CriticalDamage { get; private set; }

	// Token: 0x17000204 RID: 516
	// (get) Token: 0x060012CE RID: 4814 RVA: 0x0005C948 File Offset: 0x0005AB48
	// (set) Token: 0x060012CF RID: 4815 RVA: 0x0005C950 File Offset: 0x0005AB50
	public float Attack { get; private set; }

	// Token: 0x17000205 RID: 517
	// (get) Token: 0x060012D0 RID: 4816 RVA: 0x0005C959 File Offset: 0x0005AB59
	// (set) Token: 0x060012D1 RID: 4817 RVA: 0x0005C961 File Offset: 0x0005AB61
	public float Defense { get; private set; }

	// Token: 0x17000206 RID: 518
	// (get) Token: 0x060012D2 RID: 4818 RVA: 0x0005C96A File Offset: 0x0005AB6A
	// (set) Token: 0x060012D3 RID: 4819 RVA: 0x0005C972 File Offset: 0x0005AB72
	public float Resistance { get; private set; }

	// Token: 0x060012D4 RID: 4820 RVA: 0x0005C97B File Offset: 0x0005AB7B
	private void OnAttributesUpdated(SyncList<global::Attribute>.Operation op, int itemIndex, global::Attribute oldItem, global::Attribute newItem)
	{
		if (oldItem.Type == AttributeType.Vitality | newItem.Type == AttributeType.Vitality)
		{
			this.NetworkMaxHealth = this.CalculateMaxHealth();
		}
		this.UpdateAttributeProperties();
	}

	// Token: 0x060012D5 RID: 4821 RVA: 0x0005C9A5 File Offset: 0x0005ABA5
	[Server]
	public void SetBaseLevel(int level)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void NonPlayerAttributeModule::SetBaseLevel(System.Int32)' called when server was not active");
			return;
		}
		this.NetworkBaseLevel = level;
	}

	// Token: 0x060012D6 RID: 4822 RVA: 0x0005C9C4 File Offset: 0x0005ABC4
	[Server]
	private int CalculateMaxHealth()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Int32 NonPlayerAttributeModule::CalculateMaxHealth()' called when server was not active");
			return 0;
		}
		return (int)(this[AttributeType.Vitality].AdjustedValue * 9f);
	}

	// Token: 0x060012D7 RID: 4823 RVA: 0x0005CA08 File Offset: 0x0005AC08
	[Server]
	public void AddHealth(int amount)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void NonPlayerAttributeModule::AddHealth(System.Int32)' called when server was not active");
			return;
		}
		int num = this.currentHealth + amount;
		num = ((num > this.MaxHealth) ? this.MaxHealth : num);
		num = ((num < 0) ? 0 : num);
		this.NetworkcurrentHealth = num;
	}

	// Token: 0x060012D8 RID: 4824 RVA: 0x0005CA56 File Offset: 0x0005AC56
	[Server]
	public void ResetHealth()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void NonPlayerAttributeModule::ResetHealth()' called when server was not active");
			return;
		}
		this.AddHealth(this.MaxHealth);
	}

	// Token: 0x060012D9 RID: 4825 RVA: 0x0005CA7C File Offset: 0x0005AC7C
	[Server]
	public void ResetAttributesModifiers()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void NonPlayerAttributeModule::ResetAttributesModifiers()' called when server was not active");
			return;
		}
		this.conditionModule.RemoveAllConditions();
		for (int i = 0; i < this.Attributes.Count; i++)
		{
			global::Attribute value = this.Attributes[i];
			value.BlessingModifier = 0f;
			value.LevelBlessingModifier = 0;
			value.CurseModifier = 0f;
			value.LevelCurseModifier = 0;
			this.Attributes[i] = value;
		}
	}

	// Token: 0x060012DA RID: 4826 RVA: 0x0005CB04 File Offset: 0x0005AD04
	[Server]
	public void AddAttributeRankBlessings(Rank rank)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void NonPlayerAttributeModule::AddAttributeRankBlessings(Rank)' called when server was not active");
			return;
		}
		this.ResetAttributesModifiers();
		float num = GlobalUtils.RankToAttributePercentBonus(rank);
		if (num > 0f)
		{
			this.AddBlessing(AttributeType.Power, num);
			this.AddBlessing(AttributeType.Precision, num);
			this.AddBlessing(AttributeType.Toughness, num);
			this.AddBlessing(AttributeType.Vitality, num);
		}
	}

	// Token: 0x060012DB RID: 4827 RVA: 0x0005CB5C File Offset: 0x0005AD5C
	[Server]
	public void AddBlessing(AttributeType type, float amount)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void NonPlayerAttributeModule::AddBlessing(AttributeType,System.Single)' called when server was not active");
			return;
		}
		global::Attribute attribute = this[type];
		attribute.BlessingModifier += amount;
		attribute.BlessingModifier = ((attribute.BlessingModifier < 0f) ? 0f : attribute.BlessingModifier);
		this[type] = attribute;
	}

	// Token: 0x060012DC RID: 4828 RVA: 0x0005CBBC File Offset: 0x0005ADBC
	[Server]
	public void AddCurse(AttributeType type, float amount)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void NonPlayerAttributeModule::AddCurse(AttributeType,System.Single)' called when server was not active");
			return;
		}
		global::Attribute attribute = this[type];
		attribute.CurseModifier += amount;
		attribute.CurseModifier = ((attribute.CurseModifier < 0f) ? 0f : attribute.CurseModifier);
		this[type] = attribute;
	}

	// Token: 0x060012DD RID: 4829 RVA: 0x0005CC1C File Offset: 0x0005AE1C
	[Server]
	public void AddLevelBlessing(AttributeType type, int amount)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void NonPlayerAttributeModule::AddLevelBlessing(AttributeType,System.Int32)' called when server was not active");
			return;
		}
		global::Attribute attribute = this[type];
		attribute.LevelBlessingModifier += amount;
		attribute.LevelBlessingModifier = ((attribute.LevelBlessingModifier < 0) ? 0 : attribute.LevelBlessingModifier);
		this[type] = attribute;
	}

	// Token: 0x060012DE RID: 4830 RVA: 0x0005CC74 File Offset: 0x0005AE74
	[Server]
	public void AddLevelCurse(AttributeType type, int amount)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void NonPlayerAttributeModule::AddLevelCurse(AttributeType,System.Int32)' called when server was not active");
			return;
		}
		global::Attribute attribute = this[type];
		attribute.LevelCurseModifier += amount;
		attribute.LevelCurseModifier = ((attribute.LevelCurseModifier < 0) ? 0 : attribute.LevelCurseModifier);
		this[type] = attribute;
	}

	// Token: 0x060012DF RID: 4831 RVA: 0x0005CCCC File Offset: 0x0005AECC
	[Server]
	public void InitializeAttributes(params global::Attribute[] attributes)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void NonPlayerAttributeModule::InitializeAttributes(Attribute[])' called when server was not active");
			return;
		}
		int i;
		int j;
		for (i = 0; i < Enum.GetValues(typeof(AttributeType)).Length; i = j + 1)
		{
			global::Attribute item = attributes.FirstOrDefault((global::Attribute f) => f.Type == (AttributeType)i);
			if (item.IsDefined)
			{
				this.Attributes.Add(item);
			}
			else
			{
				this.Attributes.Add(new global::Attribute(1, 0L, (AttributeType)i));
			}
			j = i;
		}
		this.NetworkMaxHealth = this.CalculateMaxHealth();
		this.UpdateAttributeProperties();
	}

	// Token: 0x060012E0 RID: 4832 RVA: 0x0005CD80 File Offset: 0x0005AF80
	[Server]
	private void UpdateAttributeProperties()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void NonPlayerAttributeModule::UpdateAttributeProperties()' called when server was not active");
			return;
		}
		float num = (float)this.MaxHealth * 0.0023f;
		this.RegenerationAmount = Mathf.RoundToInt(num + this[AttributeType.Vitality].AdjustedValue * 0.025f);
		float value = 5f - this[AttributeType.Vitality].AdjustedValue * 0.005f;
		this.RegenerationInterval = Mathf.Clamp(value, 1.5f, 5f);
		float num2 = 1.725f - 1.725f * this[AttributeType.Agility].CurseModifier;
		num2 += num2 * this[AttributeType.Agility].BlessingModifier;
		num2 = Mathf.Max(0f, num2);
		float a = num2 + this[AttributeType.Agility].AdjustedValue * NonPlayerAttributeModule.SpeedModifier;
		this.Speed = Mathf.Min(a, NonPlayerAttributeModule.MaxMonsterSpeed);
		float b = this[AttributeType.Precision].AdjustedValue * 0.0018f;
		this.CriticalChance = Mathf.Min(0.05f, b);
		this.CriticalDamage = Mathf.Min(1.8f, this[AttributeType.Power].AdjustedValue * 0.004f + 1.1f);
		this.Attack = this[AttributeType.Power].AdjustedValue * 1.61f;
		this.Defense = this[AttributeType.Toughness].AdjustedValue * 1.375f;
		this.Resistance = this[AttributeType.Toughness].AdjustedValue * 1.41f;
	}

	// Token: 0x060012E1 RID: 4833 RVA: 0x0005CF15 File Offset: 0x0005B115
	public NonPlayerAttributeModule()
	{
		base.InitSyncObject(this.Attributes);
	}

	// Token: 0x060012E3 RID: 4835 RVA: 0x0002FEC9 File Offset: 0x0002E0C9
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x17000207 RID: 519
	// (get) Token: 0x060012E4 RID: 4836 RVA: 0x0005CF68 File Offset: 0x0005B168
	// (set) Token: 0x060012E5 RID: 4837 RVA: 0x0005CF7B File Offset: 0x0005B17B
	public int NetworkcurrentHealth
	{
		get
		{
			return this.currentHealth;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.currentHealth, 1UL, null);
		}
	}

	// Token: 0x17000208 RID: 520
	// (get) Token: 0x060012E6 RID: 4838 RVA: 0x0005CF98 File Offset: 0x0005B198
	// (set) Token: 0x060012E7 RID: 4839 RVA: 0x0005CFAB File Offset: 0x0005B1AB
	public int NetworkMaxHealth
	{
		get
		{
			return this.MaxHealth;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.MaxHealth, 2UL, null);
		}
	}

	// Token: 0x17000209 RID: 521
	// (get) Token: 0x060012E8 RID: 4840 RVA: 0x0005CFC8 File Offset: 0x0005B1C8
	// (set) Token: 0x060012E9 RID: 4841 RVA: 0x0005CFDB File Offset: 0x0005B1DB
	public int NetworkBaseLevel
	{
		get
		{
			return this.BaseLevel;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.BaseLevel, 4UL, null);
		}
	}

	// Token: 0x060012EA RID: 4842 RVA: 0x0005CFF8 File Offset: 0x0005B1F8
	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteInt(this.currentHealth);
			writer.WriteInt(this.MaxHealth);
			writer.WriteInt(this.BaseLevel);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1UL) != 0UL)
		{
			writer.WriteInt(this.currentHealth);
		}
		if ((base.syncVarDirtyBits & 2UL) != 0UL)
		{
			writer.WriteInt(this.MaxHealth);
		}
		if ((base.syncVarDirtyBits & 4UL) != 0UL)
		{
			writer.WriteInt(this.BaseLevel);
		}
	}

	// Token: 0x060012EB RID: 4843 RVA: 0x0005D0AC File Offset: 0x0005B2AC
	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.currentHealth, null, reader.ReadInt());
			base.GeneratedSyncVarDeserialize<int>(ref this.MaxHealth, null, reader.ReadInt());
			base.GeneratedSyncVarDeserialize<int>(ref this.BaseLevel, null, reader.ReadInt());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 1L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.currentHealth, null, reader.ReadInt());
		}
		if ((num & 2L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.MaxHealth, null, reader.ReadInt());
		}
		if ((num & 4L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.BaseLevel, null, reader.ReadInt());
		}
	}

	// Token: 0x04001189 RID: 4489
	public static readonly ObscuredFloat MaxMonsterSpeed = 6f;

	// Token: 0x0400118A RID: 4490
	public static float SpeedModifier = NonPlayerAttributeModule.MaxMonsterSpeed / 100f;

	// Token: 0x0400118B RID: 4491
	public const float SpeedBase = 1.725f;

	// Token: 0x0400118C RID: 4492
	public const float CriticalDamageBase = 1.1f;

	// Token: 0x0400118D RID: 4493
	public const float RegenerationIntervalBase = 5f;

	// Token: 0x0400118E RID: 4494
	public const float RegenerationIntervalModifier = 0.005f;

	// Token: 0x0400118F RID: 4495
	public const float RegenerationAmountModifier = 0.025f;

	// Token: 0x04001190 RID: 4496
	public const float CriticalChanceModifier = 0.0018f;

	// Token: 0x04001191 RID: 4497
	public const float CriticalDamageModifier = 0.004f;

	// Token: 0x04001192 RID: 4498
	public const float AttackModifier = 1.61f;

	// Token: 0x04001193 RID: 4499
	public const float DefenseModifier = 1.375f;

	// Token: 0x04001194 RID: 4500
	public const float ResistanceModifier = 1.41f;

	// Token: 0x04001195 RID: 4501
	public const float HealthModifier = 9f;

	// Token: 0x04001196 RID: 4502
	[SyncVar]
	private int currentHealth;

	// Token: 0x04001197 RID: 4503
	[SyncVar]
	public int MaxHealth;

	// Token: 0x04001198 RID: 4504
	[SyncVar]
	public int BaseLevel;

	// Token: 0x04001199 RID: 4505
	private CreatureModule creatureModule;

	// Token: 0x0400119A RID: 4506
	private ConditionModule conditionModule;

	// Token: 0x0400119B RID: 4507
	public readonly SyncListAttribute Attributes = new SyncListAttribute();

	// Token: 0x0400119C RID: 4508
	private List<global::Attribute> attributesCache = new List<global::Attribute>();
}
