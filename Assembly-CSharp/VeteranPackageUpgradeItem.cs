using System;
using System.Runtime.CompilerServices;

// Token: 0x020001B0 RID: 432
public class VeteranPackageUpgradeItem : ItemBase
{
	// Token: 0x060004F0 RID: 1264 RVA: 0x0001B3C4 File Offset: 0x000195C4
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		EffectModule component = itemBaseConfig.UserObject.GetComponent<EffectModule>();
		PlayerModule component2 = itemBaseConfig.UserObject.GetComponent<PlayerModule>();
		InventoryModule component3 = itemBaseConfig.UserObject.GetComponent<InventoryModule>();
		if (component3.EmptySlots < 1)
		{
			component.ShowScreenMessage("inventory_full_message", 3, 7f, Array.Empty<string>());
			return false;
		}
		this.UpgradePackageAsync(itemBaseConfig, component2, component, component3);
		this.ShowStarBlastEffects(component);
		return true;
	}

	// Token: 0x060004F1 RID: 1265 RVA: 0x0001B428 File Offset: 0x00019628
	private void UpgradePackageAsync(ItemBaseConfig itemBaseConfig, PlayerModule playerModule, EffectModule effectModule, InventoryModule inventoryModule)
	{
		VeteranPackageUpgradeItem.<UpgradePackageAsync>d__1 <UpgradePackageAsync>d__;
		<UpgradePackageAsync>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<UpgradePackageAsync>d__.itemBaseConfig = itemBaseConfig;
		<UpgradePackageAsync>d__.playerModule = playerModule;
		<UpgradePackageAsync>d__.inventoryModule = inventoryModule;
		<UpgradePackageAsync>d__.<>1__state = -1;
		<UpgradePackageAsync>d__.<>t__builder.Start<VeteranPackageUpgradeItem.<UpgradePackageAsync>d__1>(ref <UpgradePackageAsync>d__);
	}

	// Token: 0x060004F2 RID: 1266 RVA: 0x0001B470 File Offset: 0x00019670
	private void ShowStarBlastEffects(EffectModule effectModule)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "StarBlast",
			EffectScaleModifier = 2f,
			EffectSpeedModifier = 0.2f,
			SoundEffectName = "magic_explosion"
		};
		effectModule.ShowEffects(effectConfig);
	}
}
