using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001B4 RID: 436
public class CelestialDeadRuneItem : ItemBase
{
	// Token: 0x060004FA RID: 1274 RVA: 0x0001B724 File Offset: 0x00019924
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
		if (inventoryModule.GetAmount(377) < 1)
		{
			effectModule.ShowScreenMessage("missing_energized_rune_message", 0, 3.5f, Array.Empty<string>());
			return false;
		}
		foreach (KeyValuePair<int, int> keyValuePair2 in this.neededEnergies)
		{
			inventoryModule.ConsumeItem(keyValuePair2.Key, keyValuePair2.Value);
		}
		inventoryModule.ConsumeItem(377, 1);
		inventoryModule.ConsumeItem(1367, 1);
		if (UnityEngine.Random.Range(1, 101) > 50)
		{
			Item item = ItemBase.ItemDatabaseModule.GetItem(1368);
			item.RequiredLevel = 1;
			inventoryModule.AddItem(item, -1, 1, true);
			this.ShowMagicEffect(effectModule);
		}
		else
		{
			this.ShowFailureEffects(effectModule);
		}
		return false;
	}

	// Token: 0x060004FB RID: 1275 RVA: 0x0001B894 File Offset: 0x00019A94
	private void ShowFailureEffects(EffectModule effectModule)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "SmokeBomb",
			EffectScaleModifier = 1f,
			EffectSpeedModifier = 0.2f,
			SoundEffectName = "curse"
		};
		effectModule.ShowEffects(effectConfig);
	}

	// Token: 0x060004FC RID: 1276 RVA: 0x0001B8E4 File Offset: 0x00019AE4
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

	// Token: 0x040007C9 RID: 1993
	private readonly Dictionary<int, int> neededEnergies = new Dictionary<int, int>
	{
		{
			366,
			15
		},
		{
			367,
			15
		},
		{
			368,
			15
		},
		{
			369,
			15
		},
		{
			370,
			15
		},
		{
			371,
			15
		},
		{
			372,
			10
		},
		{
			373,
			10
		},
		{
			374,
			5
		},
		{
			375,
			5
		},
		{
			1369,
			5
		},
		{
			1370,
			5
		}
	};
}
