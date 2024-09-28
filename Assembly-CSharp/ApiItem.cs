using System;

// Token: 0x02000035 RID: 53
[Serializable]
public class ApiItem
{
	// Token: 0x040000B7 RID: 183
	public int Id;

	// Token: 0x040000B8 RID: 184
	public int SkillId;

	// Token: 0x040000B9 RID: 185
	public string Name;

	// Token: 0x040000BA RID: 186
	public int BaseValue;

	// Token: 0x040000BB RID: 187
	public string Script;

	// Token: 0x040000BC RID: 188
	public ItemType Type;

	// Token: 0x040000BD RID: 189
	public Rarity Rarity;

	// Token: 0x040000BE RID: 190
	public bool Soulbind;

	// Token: 0x040000BF RID: 191
	public bool Sellable;

	// Token: 0x040000C0 RID: 192
	public bool TwoHanded;

	// Token: 0x040000C1 RID: 193
	public int BoostLevel;

	// Token: 0x040000C2 RID: 194
	public bool Stackable;

	// Token: 0x040000C3 RID: 195
	public int BlueprintId;

	// Token: 0x040000C4 RID: 196
	public int TextColorId;

	// Token: 0x040000C5 RID: 197
	public string MetaName;

	// Token: 0x040000C6 RID: 198
	public string OwnerName;

	// Token: 0x040000C7 RID: 199
	public short BaseAttack;

	// Token: 0x040000C8 RID: 200
	public short BaseDefense;

	// Token: 0x040000C9 RID: 201
	public int RequiredLevel;

	// Token: 0x040000CA RID: 202
	public SlotType SlotType;

	// Token: 0x040000CB RID: 203
	public string Description;

	// Token: 0x040000CC RID: 204
	public ItemQuality Quality;

	// Token: 0x040000CD RID: 205
	public bool AllowDropByLevel;

	// Token: 0x040000CE RID: 206
	public Projectile Projectile;

	// Token: 0x040000CF RID: 207
	public ItemCategory Category;

	// Token: 0x040000D0 RID: 208
	public bool AllowUseWhileDead;

	// Token: 0x040000D1 RID: 209
	public bool ConsumeProjectile;

	// Token: 0x040000D2 RID: 210
	public string TeleportSpawnPoint;

	// Token: 0x040000D3 RID: 211
	public bool AllowDropFromChests;

	// Token: 0x040000D4 RID: 212
	public Vocation RequiredVocation;

	// Token: 0x040000D5 RID: 213
	public bool IgnoreQualityRestrictions;

	// Token: 0x040000D6 RID: 214
	public string ProjectileShootSoundEffectName;

	// Token: 0x040000D7 RID: 215
	public string ProjectileExplosionSoundEffectName;
}
