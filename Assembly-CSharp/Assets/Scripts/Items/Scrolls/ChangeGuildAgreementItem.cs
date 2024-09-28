using System;

namespace Assets.Scripts.Items.Scrolls
{
	// Token: 0x02000588 RID: 1416
	public class ChangeGuildAgreementItem : ItemBase
	{
		// Token: 0x06001F5C RID: 8028 RVA: 0x0009E050 File Offset: 0x0009C250
		public override bool UseItem(ItemBaseConfig itemBaseConfig)
		{
			GuildModule guildModule;
			itemBaseConfig.UserObject.TryGetComponent<GuildModule>(out guildModule);
			guildModule.ShowChangeGuildWindow();
			return false;
		}
	}
}
