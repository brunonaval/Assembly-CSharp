using System;
using System.Runtime.CompilerServices;

// Token: 0x02000194 RID: 404
public class ElitePackageUpgradeItem : ItemBase
{
	// Token: 0x06000491 RID: 1169 RVA: 0x000199EC File Offset: 0x00017BEC
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		PlayerModule component = itemBaseConfig.UserObject.GetComponent<PlayerModule>();
		EffectModule component2 = itemBaseConfig.UserObject.GetComponent<EffectModule>();
		InventoryModule component3 = itemBaseConfig.UserObject.GetComponent<InventoryModule>();
		if (component3.EmptySlots < 1)
		{
			component2.ShowScreenMessage("inventory_full_message", 3, 7f, Array.Empty<string>());
			return false;
		}
		this.UpgradePackageAsync(itemBaseConfig, component, component2, component3);
		this.ShowStarBlastEffects(component2);
		return true;
	}

	// Token: 0x06000492 RID: 1170 RVA: 0x00019A50 File Offset: 0x00017C50
	private void UpgradePackageAsync(ItemBaseConfig itemBaseConfig, PlayerModule playerModule, EffectModule effectModule, InventoryModule inventoryModule)
	{
		ElitePackageUpgradeItem.<UpgradePackageAsync>d__1 <UpgradePackageAsync>d__;
		<UpgradePackageAsync>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<UpgradePackageAsync>d__.itemBaseConfig = itemBaseConfig;
		<UpgradePackageAsync>d__.playerModule = playerModule;
		<UpgradePackageAsync>d__.inventoryModule = inventoryModule;
		<UpgradePackageAsync>d__.<>1__state = -1;
		<UpgradePackageAsync>d__.<>t__builder.Start<ElitePackageUpgradeItem.<UpgradePackageAsync>d__1>(ref <UpgradePackageAsync>d__);
	}

	// Token: 0x06000493 RID: 1171 RVA: 0x00019A98 File Offset: 0x00017C98
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
