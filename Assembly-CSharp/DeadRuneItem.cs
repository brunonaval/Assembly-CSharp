using System;
using System.Collections.Generic;

// Token: 0x020001B6 RID: 438
public class DeadRuneItem : ItemBase
{
	// Token: 0x06000500 RID: 1280 RVA: 0x0001BA3C File Offset: 0x00019C3C
	public DeadRuneItem()
	{
		this.neededEnergies = new Dictionary<int, int>
		{
			{
				366,
				10
			},
			{
				367,
				10
			},
			{
				368,
				10
			},
			{
				369,
				10
			},
			{
				370,
				10
			},
			{
				371,
				10
			},
			{
				372,
				5
			},
			{
				373,
				5
			},
			{
				374,
				1
			},
			{
				375,
				1
			}
		};
	}

	// Token: 0x06000501 RID: 1281 RVA: 0x0001BAE4 File Offset: 0x00019CE4
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		InventoryModule inventoryModule;
		itemBaseConfig.UserObject.TryGetComponent<InventoryModule>(out inventoryModule);
		EffectModule effectModule;
		itemBaseConfig.UserObject.TryGetComponent<EffectModule>(out effectModule);
		foreach (KeyValuePair<int, int> keyValuePair in this.neededEnergies)
		{
			if (inventoryModule.GetAmount(keyValuePair.Key) < keyValuePair.Value)
			{
				effectModule.ShowScreenMessage("not_enough_energies_message", 0, 3.5f, Array.Empty<string>());
				return false;
			}
		}
		foreach (KeyValuePair<int, int> keyValuePair2 in this.neededEnergies)
		{
			inventoryModule.ConsumeItem(keyValuePair2.Key, keyValuePair2.Value);
		}
		inventoryModule.ConsumeItem(376, 1);
		Item item = ItemBase.ItemDatabaseModule.GetItem(377);
		item.RequiredLevel = 1;
		inventoryModule.AddItem(item, -1, 1, true);
		this.ShowMagicEffect(effectModule);
		return false;
	}

	// Token: 0x06000502 RID: 1282 RVA: 0x0001BC0C File Offset: 0x00019E0C
	private void ShowMagicEffect(EffectModule effectModule)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "Magic",
			EffectScaleModifier = 0.5f,
			EffectSpeedModifier = 0.25f,
			SoundEffectName = "spellbook"
		};
		effectModule.ShowEffects(effectConfig);
	}

	// Token: 0x040007CA RID: 1994
	private Dictionary<int, int> neededEnergies = new Dictionary<int, int>();
}
