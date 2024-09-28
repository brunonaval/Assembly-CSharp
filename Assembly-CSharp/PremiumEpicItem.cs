using System;
using System.Runtime.CompilerServices;

// Token: 0x020001A8 RID: 424
public class PremiumEpicItem : ItemBase
{
	// Token: 0x060004D4 RID: 1236 RVA: 0x0001ADF2 File Offset: 0x00018FF2
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		this.AddPremiumDaysAsync(itemBaseConfig);
		return true;
	}

	// Token: 0x060004D5 RID: 1237 RVA: 0x0001ADFC File Offset: 0x00018FFC
	private void AddPremiumDaysAsync(ItemBaseConfig itemBaseConfig)
	{
		PremiumEpicItem.<AddPremiumDaysAsync>d__1 <AddPremiumDaysAsync>d__;
		<AddPremiumDaysAsync>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<AddPremiumDaysAsync>d__.itemBaseConfig = itemBaseConfig;
		<AddPremiumDaysAsync>d__.<>1__state = -1;
		<AddPremiumDaysAsync>d__.<>t__builder.Start<PremiumEpicItem.<AddPremiumDaysAsync>d__1>(ref <AddPremiumDaysAsync>d__);
	}
}
