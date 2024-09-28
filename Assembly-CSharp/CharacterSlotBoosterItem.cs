using System;
using System.Runtime.CompilerServices;

// Token: 0x02000167 RID: 359
public class CharacterSlotBoosterItem : ItemBase
{
	// Token: 0x060003DD RID: 989 RVA: 0x000174A2 File Offset: 0x000156A2
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		this.AddSlotAsync(itemBaseConfig);
		return true;
	}

	// Token: 0x060003DE RID: 990 RVA: 0x000174AC File Offset: 0x000156AC
	private void AddSlotAsync(ItemBaseConfig itemBaseConfig)
	{
		CharacterSlotBoosterItem.<AddSlotAsync>d__1 <AddSlotAsync>d__;
		<AddSlotAsync>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<AddSlotAsync>d__.itemBaseConfig = itemBaseConfig;
		<AddSlotAsync>d__.<>1__state = -1;
		<AddSlotAsync>d__.<>t__builder.Start<CharacterSlotBoosterItem.<AddSlotAsync>d__1>(ref <AddSlotAsync>d__);
	}
}
