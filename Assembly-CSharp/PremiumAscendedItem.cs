using System;
using System.Runtime.CompilerServices;

// Token: 0x020001A4 RID: 420
public class PremiumAscendedItem : ItemBase
{
	// Token: 0x060004CA RID: 1226 RVA: 0x0001AB41 File Offset: 0x00018D41
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		this.AddPremiumDaysAsync(itemBaseConfig);
		return true;
	}

	// Token: 0x060004CB RID: 1227 RVA: 0x0001AB4C File Offset: 0x00018D4C
	private void AddPremiumDaysAsync(ItemBaseConfig itemBaseConfig)
	{
		PremiumAscendedItem.<AddPremiumDaysAsync>d__1 <AddPremiumDaysAsync>d__;
		<AddPremiumDaysAsync>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<AddPremiumDaysAsync>d__.itemBaseConfig = itemBaseConfig;
		<AddPremiumDaysAsync>d__.<>1__state = -1;
		<AddPremiumDaysAsync>d__.<>t__builder.Start<PremiumAscendedItem.<AddPremiumDaysAsync>d__1>(ref <AddPremiumDaysAsync>d__);
	}
}
