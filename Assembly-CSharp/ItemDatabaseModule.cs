using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x0200035F RID: 863
public class ItemDatabaseModule : MonoBehaviour
{
	// Token: 0x170001DD RID: 477
	// (get) Token: 0x06001133 RID: 4403 RVA: 0x00051512 File Offset: 0x0004F712
	// (set) Token: 0x06001134 RID: 4404 RVA: 0x0005151A File Offset: 0x0004F71A
	public bool IsLoaded { get; private set; }

	// Token: 0x170001DE RID: 478
	// (get) Token: 0x06001135 RID: 4405 RVA: 0x00051523 File Offset: 0x0004F723
	// (set) Token: 0x06001136 RID: 4406 RVA: 0x0005152B File Offset: 0x0004F72B
	public Item[] Items { get; private set; }

	// Token: 0x170001DF RID: 479
	// (get) Token: 0x06001137 RID: 4407 RVA: 0x00051534 File Offset: 0x0004F734
	// (set) Token: 0x06001138 RID: 4408 RVA: 0x0005153C File Offset: 0x0004F73C
	public Item[] DroppableItems { get; private set; }

	// Token: 0x170001E0 RID: 480
	// (get) Token: 0x06001139 RID: 4409 RVA: 0x00051545 File Offset: 0x0004F745
	// (set) Token: 0x0600113A RID: 4410 RVA: 0x0005154D File Offset: 0x0004F74D
	public List<StoreItemConfig> StoreItems { get; private set; }

	// Token: 0x0600113B RID: 4411 RVA: 0x00051556 File Offset: 0x0004F756
	private void Awake()
	{
		this.StoreItems = new List<StoreItemConfig>();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	// Token: 0x0600113C RID: 4412 RVA: 0x00051569 File Offset: 0x0004F769
	private IEnumerator Start()
	{
		yield return this.ProcessItems().WaitAsCoroutine();
		this.PopulateDroppableItems();
		this.IsLoaded = true;
		yield break;
	}

	// Token: 0x0600113D RID: 4413 RVA: 0x00051578 File Offset: 0x0004F778
	private Task ProcessItems()
	{
		ItemDatabaseModule.<ProcessItems>d__18 <ProcessItems>d__;
		<ProcessItems>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<ProcessItems>d__.<>4__this = this;
		<ProcessItems>d__.<>1__state = -1;
		<ProcessItems>d__.<>t__builder.Start<ItemDatabaseModule.<ProcessItems>d__18>(ref <ProcessItems>d__);
		return <ProcessItems>d__.<>t__builder.Task;
	}

	// Token: 0x0600113E RID: 4414 RVA: 0x000515BC File Offset: 0x0004F7BC
	private void PopulateDroppableItems()
	{
		Item[] array = (from i in this.Items
		where i.AllowDropByLevel
		select i).ToArray<Item>();
		this.DroppableItems = new Item[array.Max((Item a) => a.Id) + 1];
		foreach (Item item in array)
		{
			this.DroppableItems[item.Id] = item;
		}
	}

	// Token: 0x0600113F RID: 4415 RVA: 0x00051658 File Offset: 0x0004F858
	private Item BuildItem(DataRow dbItem, Projectile projectile)
	{
		try
		{
			int valueOrDefault = (dbItem["BlueprintRequiredProfession"] as int?).GetValueOrDefault();
			int valueOrDefault2 = (dbItem["BlueprintRequiredProfessionLevel"] as int?).GetValueOrDefault();
			int valueOrDefault3 = (dbItem["Id"] as int?).GetValueOrDefault();
			string name = dbItem["Name"].ToString();
			int valueOrDefault4 = (dbItem["RequiredLevel"] as int?).GetValueOrDefault();
			Vocation valueOrDefault5 = (Vocation)(dbItem["RequiredVocation"] as int?).GetValueOrDefault();
			short valueOrDefault6 = (dbItem["BaseAttack"] as short?).GetValueOrDefault();
			short valueOrDefault7 = (dbItem["BaseDefense"] as short?).GetValueOrDefault();
			int valueOrDefault8 = (dbItem["BaseValue"] as int?).GetValueOrDefault();
			ItemQuality valueOrDefault9 = (ItemQuality)(dbItem["Quality"] as int?).GetValueOrDefault();
			Rarity valueOrDefault10 = (Rarity)(dbItem["Rarity"] as int?).GetValueOrDefault();
			ItemCategory valueOrDefault11 = (ItemCategory)(dbItem["Category"] as int?).GetValueOrDefault();
			SlotType valueOrDefault12 = (SlotType)(dbItem["SlotType"] as int?).GetValueOrDefault();
			ItemType valueOrDefault13 = (ItemType)(dbItem["Type"] as int?).GetValueOrDefault();
			bool valueOrDefault14 = ((bool?)dbItem["Stackable"]).GetValueOrDefault();
			bool valueOrDefault15 = ((bool?)dbItem["Soulbind"]).GetValueOrDefault();
			bool valueOrDefault16 = ((bool?)dbItem["Sellable"]).GetValueOrDefault();
			bool valueOrDefault17 = ((bool?)dbItem["TwoHanded"]).GetValueOrDefault();
			bool valueOrDefault18 = ((bool?)dbItem["AllowDropByLevel"]).GetValueOrDefault();
			float valueOrDefault19 = (dbItem["DropByLevelChance"] as float?).GetValueOrDefault();
			bool valueOrDefault20 = ((bool?)dbItem["AllowUseWhileDead"]).GetValueOrDefault();
			bool valueOrDefault21 = ((bool?)dbItem["ConsumeProjectile"]).GetValueOrDefault();
			string description = dbItem["Description"].ToString();
			string metaName = dbItem["MetaName"].ToString();
			int valueOrDefault22 = (dbItem["SkillId"] as int?).GetValueOrDefault();
			string projectileShootSoundEffectName = dbItem["ProjectileShootSoundEffectName"].ToString();
			string projectileExplosionSoundEffectName = dbItem["ProjectileExplosionSoundEffectName"].ToString();
			int valueOrDefault23 = (dbItem["BlueprintId"] as int?).GetValueOrDefault();
			int textColorId = (dbItem["TextColorId"] as int?) ?? 2;
			string script = dbItem["Script"].ToString();
			string teleportSpawnPoint = dbItem["TeleportSpawnPoint"].ToString();
			bool valueOrDefault24 = ((bool?)dbItem["AllowDropFromChests"]).GetValueOrDefault();
			bool valueOrDefault25 = ((bool?)dbItem["IgnoreQualityRestrictions"]).GetValueOrDefault();
			AnimationType valueOrDefault26 = (AnimationType)(dbItem["WeaponSkinAnimationType"] as int?).GetValueOrDefault();
			return new Item(valueOrDefault3, name, valueOrDefault4, valueOrDefault5, valueOrDefault6, valueOrDefault7, valueOrDefault8, valueOrDefault9, valueOrDefault10, projectile, valueOrDefault11, valueOrDefault12, valueOrDefault13, valueOrDefault14, valueOrDefault15, valueOrDefault16, valueOrDefault17, valueOrDefault18, valueOrDefault19, valueOrDefault20, valueOrDefault21, description, metaName, valueOrDefault22, projectileShootSoundEffectName, projectileExplosionSoundEffectName, valueOrDefault23, textColorId, script, teleportSpawnPoint, valueOrDefault24, (PlayerProfession)valueOrDefault, valueOrDefault2, valueOrDefault25, valueOrDefault26);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
		return default(Item);
	}

	// Token: 0x06001140 RID: 4416 RVA: 0x00051A94 File Offset: 0x0004FC94
	private Projectile BuildProjectile(DataRow dbItem, Effect explosionEffect, Effect hitEffect)
	{
		Projectile result = default(Projectile);
		string text = dbItem["ProjectilePrefabName"].ToString();
		if (!string.IsNullOrEmpty(text))
		{
			string script = dbItem["ProjectileScript"].ToString();
			float speed = (dbItem["ProjectileSpeed"] as float?) ?? 1f;
			int maxTargets = (dbItem["ProjectileMaxTargets"] as int?) ?? 1;
			float range = (dbItem["ProjectileRange"] as float?) ?? 1f;
			result = new Projectile(text, script, speed, maxTargets, range, explosionEffect, hitEffect, false, false);
		}
		return result;
	}

	// Token: 0x06001141 RID: 4417 RVA: 0x00051B74 File Offset: 0x0004FD74
	private Effect BuildHitEffect(DataRow dbItem)
	{
		Effect result = default(Effect);
		string text = dbItem["ProjectileHitEffectName"].ToString();
		if (!string.IsNullOrEmpty(text))
		{
			float scaleModifier = (dbItem["ProjectileHitEffectScaleModifier"] as float?) ?? 1f;
			float speedModifier = (dbItem["ProjectileHitEffectSpeedModifier"] as float?) ?? 1f;
			result = new Effect(text, scaleModifier, speedModifier);
		}
		return result;
	}

	// Token: 0x06001142 RID: 4418 RVA: 0x00051C0C File Offset: 0x0004FE0C
	private Effect BuildExplosionEffect(DataRow dbItem)
	{
		Effect result = default(Effect);
		string text = dbItem["ProjectileExplosionEffectName"].ToString();
		if (!string.IsNullOrEmpty(text))
		{
			float scaleModifier = (dbItem["ProjectileExplosionEffectScaleModifier"] as float?) ?? 1f;
			float speedModifier = (dbItem["ProjectileExplosionEffectSpeedModifier"] as float?) ?? 1f;
			result = new Effect(text, scaleModifier, speedModifier);
		}
		return result;
	}

	// Token: 0x06001143 RID: 4419 RVA: 0x00051CA4 File Offset: 0x0004FEA4
	public Item GetItem(int itemId)
	{
		if (itemId >= this.Items.Length)
		{
			return default(Item);
		}
		return this.Items[itemId];
	}

	// Token: 0x06001144 RID: 4420 RVA: 0x00051CD4 File Offset: 0x0004FED4
	public Item GetItem(string itemUniqueId)
	{
		return this.Items.FirstOrDefault((Item i) => i.UniqueId == itemUniqueId);
	}

	// Token: 0x06001145 RID: 4421 RVA: 0x00051D08 File Offset: 0x0004FF08
	public string GetItemScript(int itemId)
	{
		return this.GetItem(itemId).Script;
	}

	// Token: 0x06001146 RID: 4422 RVA: 0x00051D24 File Offset: 0x0004FF24
	public int GetItemBlueprintId(int itemId)
	{
		return this.GetItem(itemId).BlueprintId;
	}

	// Token: 0x06001147 RID: 4423 RVA: 0x00051D34 File Offset: 0x0004FF34
	public void AddNpcSellableItem(StoreItemConfig storeItem)
	{
		if (this.StoreItems.Any((StoreItemConfig si) => si.Item.UniqueId == storeItem.Item.UniqueId & si.NpcId == storeItem.NpcId))
		{
			return;
		}
		this.StoreItems.Add(storeItem);
	}

	// Token: 0x06001148 RID: 4424 RVA: 0x00051D7C File Offset: 0x0004FF7C
	public StoreItemConfig GetStoreItem(int npcId, string itemUniqueId)
	{
		return this.StoreItems.FirstOrDefault((StoreItemConfig si) => si.NpcId == npcId & si.Item.UniqueId == itemUniqueId);
	}
}
