using System;

namespace Assets.Scripts.Items.Scrolls
{
	// Token: 0x02000589 RID: 1417
	public class GuildCreationAgreementItem : ItemBase
	{
		// Token: 0x06001F5E RID: 8030 RVA: 0x0009E074 File Offset: 0x0009C274
		public override bool UseItem(ItemBaseConfig itemBaseConfig)
		{
			GuildModule guildModule;
			itemBaseConfig.UserObject.TryGetComponent<GuildModule>(out guildModule);
			guildModule.ShowGuildCreationWindow();
			return false;
		}
	}
}
