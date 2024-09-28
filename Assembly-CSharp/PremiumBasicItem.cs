using System;
using System.Runtime.CompilerServices;

// Token: 0x020001A6 RID: 422
public class PremiumBasicItem : ItemBase
{
	// Token: 0x060004CF RID: 1231 RVA: 0x0001AC9A File Offset: 0x00018E9A
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		this.AddPremiumDaysAsync(itemBaseConfig);
		return true;
	}

	// Token: 0x060004D0 RID: 1232 RVA: 0x0001ACA4 File Offset: 0x00018EA4
	private void AddPremiumDaysAsync(ItemBaseConfig itemBaseConfig)
	{
		PremiumBasicItem.<AddPremiumDaysAsync>d__1 <AddPremiumDaysAsync>d__;
		<AddPremiumDaysAsync>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<AddPremiumDaysAsync>d__.itemBaseConfig = itemBaseConfig;
		<AddPremiumDaysAsync>d__.<>1__state = -1;
		<AddPremiumDaysAsync>d__.<>t__builder.Start<PremiumBasicItem.<AddPremiumDaysAsync>d__1>(ref <AddPremiumDaysAsync>d__);
	}
}
