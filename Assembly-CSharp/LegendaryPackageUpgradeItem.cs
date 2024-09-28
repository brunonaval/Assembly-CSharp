using System;
using System.Runtime.CompilerServices;

// Token: 0x0200019A RID: 410
public class LegendaryPackageUpgradeItem : ItemBase
{
	// Token: 0x060004A4 RID: 1188 RVA: 0x00019F98 File Offset: 0x00018198
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		PlayerModule component = itemBaseConfig.UserObject.GetComponent<PlayerModule>();
		EffectModule component2 = itemBaseConfig.UserObject.GetComponent<EffectModule>();
		InventoryModule component3 = itemBaseConfig.UserObject.GetComponent<InventoryModule>();
		if (component3.EmptySlots < 2)
		{
			component2.ShowScreenMessage("inventory_full_message", 3, 7f, Array.Empty<string>());
			return false;
		}
		this.UpgradePackageAsync(itemBaseConfig, component, component2, component3);
		this.ShowStarBlastEffects(component2);
		return true;
	}

	// Token: 0x060004A5 RID: 1189 RVA: 0x00019FFC File Offset: 0x000181FC
	private void UpgradePackageAsync(ItemBaseConfig itemBaseConfig, PlayerModule playerModule, EffectModule effectModule, InventoryModule inventoryModule)
	{
		LegendaryPackageUpgradeItem.<UpgradePackageAsync>d__1 <UpgradePackageAsync>d__;
		<UpgradePackageAsync>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<UpgradePackageAsync>d__.itemBaseConfig = itemBaseConfig;
		<UpgradePackageAsync>d__.playerModule = playerModule;
		<UpgradePackageAsync>d__.inventoryModule = inventoryModule;
		<UpgradePackageAsync>d__.<>1__state = -1;
		<UpgradePackageAsync>d__.<>t__builder.Start<LegendaryPackageUpgradeItem.<UpgradePackageAsync>d__1>(ref <UpgradePackageAsync>d__);
	}

	// Token: 0x060004A6 RID: 1190 RVA: 0x0001A044 File Offset: 0x00018244
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
