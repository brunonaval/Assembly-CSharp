using System;
using System.Runtime.CompilerServices;

// Token: 0x020001AA RID: 426
public class PremiumMasterworkItem : ItemBase
{
	// Token: 0x060004D9 RID: 1241 RVA: 0x0001AF4A File Offset: 0x0001914A
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		this.AddPremiumDaysAsync(itemBaseConfig);
		return true;
	}

	// Token: 0x060004DA RID: 1242 RVA: 0x0001AF54 File Offset: 0x00019154
	private void AddPremiumDaysAsync(ItemBaseConfig itemBaseConfig)
	{
		PremiumMasterworkItem.<AddPremiumDaysAsync>d__1 <AddPremiumDaysAsync>d__;
		<AddPremiumDaysAsync>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<AddPremiumDaysAsync>d__.itemBaseConfig = itemBaseConfig;
		<AddPremiumDaysAsync>d__.<>1__state = -1;
		<AddPremiumDaysAsync>d__.<>t__builder.Start<PremiumMasterworkItem.<AddPremiumDaysAsync>d__1>(ref <AddPremiumDaysAsync>d__);
	}
}
