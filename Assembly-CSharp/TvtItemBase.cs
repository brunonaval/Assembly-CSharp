using System;

// Token: 0x020001CE RID: 462
public abstract class TvtItemBase : ItemBase
{
	// Token: 0x0600055A RID: 1370
	public abstract override bool UseItem(ItemBaseConfig itemBaseConfig);

	// Token: 0x0600055B RID: 1371 RVA: 0x0001CD8C File Offset: 0x0001AF8C
	public bool ValidateTvtItemUsage(ItemBaseConfig itemBaseConfig)
	{
		PvpModule component = itemBaseConfig.UserObject.GetComponent<PvpModule>();
		EffectModule component2 = itemBaseConfig.UserObject.GetComponent<EffectModule>();
		if (component == null || component.TvtTeam != TvtTeam.None)
		{
			component2.ShowScreenMessage("item_cant_use_on_tvt_message", 3, 3.5f, Array.Empty<string>());
			return false;
		}
		return true;
	}
}
