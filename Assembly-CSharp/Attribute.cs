using System;
using UnityEngine;

// Token: 0x020000F3 RID: 243
public struct Attribute
{
	// Token: 0x17000016 RID: 22
	// (get) Token: 0x0600025F RID: 607 RVA: 0x000117DE File Offset: 0x0000F9DE
	public bool IsDefined
	{
		get
		{
			return this.Level > 0 | this.Experience > 0L;
		}
	}

	// Token: 0x17000017 RID: 23
	// (get) Token: 0x06000260 RID: 608 RVA: 0x000117F4 File Offset: 0x0000F9F4
	public int AdjustedLevel
	{
		get
		{
			return this.BaseLevel + this.Level + this.LevelBlessingModifier - this.LevelCurseModifier;
		}
	}

	// Token: 0x17000018 RID: 24
	// (get) Token: 0x06000261 RID: 609 RVA: 0x00011814 File Offset: 0x0000FA14
	public float AdjustedValue
	{
		get
		{
			float num = (float)this.AdjustedLevel - (float)this.AdjustedLevel * this.CurseModifier;
			num += num * this.BlessingModifier;
			return Mathf.Max(0f, num);
		}
	}

	// Token: 0x17000019 RID: 25
	// (get) Token: 0x06000262 RID: 610 RVA: 0x00011850 File Offset: 0x0000FA50
	public long ExperienceToLevel
	{
		get
		{
			long num = 0L;
			for (int i = 1; i <= this.Level; i++)
			{
				num += (long)(50 * i * i - 150 * i + 200);
			}
			return num;
		}
	}

	// Token: 0x1700001A RID: 26
	// (get) Token: 0x06000263 RID: 611 RVA: 0x0001188C File Offset: 0x0000FA8C
	public long ExperienceToCurrentLevel
	{
		get
		{
			long num = 0L;
			for (int i = 1; i < this.Level; i++)
			{
				num += (long)(50 * (i * i) - 150 * i + 200);
			}
			return num;
		}
	}

	// Token: 0x1700001B RID: 27
	// (get) Token: 0x06000264 RID: 612 RVA: 0x000118C8 File Offset: 0x0000FAC8
	public int MaxLevel
	{
		get
		{
			AttributeType type = this.Type;
			if (type - AttributeType.Precision <= 1)
			{
				return 105;
			}
			return int.MaxValue;
		}
	}

	// Token: 0x06000265 RID: 613 RVA: 0x000118EC File Offset: 0x0000FAEC
	public Attribute(int level, AttributeType type)
	{
		this.Type = type;
		this.Level = level;
		this.BaseLevel = 0;
		this.Experience = 0L;
		this.BlessingModifier = 0f;
		this.CurseModifier = 0f;
		this.LevelBlessingModifier = 0;
		this.LevelCurseModifier = 0;
		this.Experience = this.ExperienceToCurrentLevel;
	}

	// Token: 0x06000266 RID: 614 RVA: 0x00011948 File Offset: 0x0000FB48
	public Attribute(int level, long experience, AttributeType type)
	{
		this.Type = type;
		this.Level = level;
		this.Experience = experience;
		this.BaseLevel = 0;
		this.LevelBlessingModifier = 0;
		this.LevelCurseModifier = 0;
		this.BlessingModifier = 0f;
		this.CurseModifier = 0f;
	}

	// Token: 0x04000469 RID: 1129
	public int Level;

	// Token: 0x0400046A RID: 1130
	public int BaseLevel;

	// Token: 0x0400046B RID: 1131
	public long Experience;

	// Token: 0x0400046C RID: 1132
	public AttributeType Type;

	// Token: 0x0400046D RID: 1133
	public int LevelBlessingModifier;

	// Token: 0x0400046E RID: 1134
	public int LevelCurseModifier;

	// Token: 0x0400046F RID: 1135
	public float CurseModifier;

	// Token: 0x04000470 RID: 1136
	public float BlessingModifier;
}
