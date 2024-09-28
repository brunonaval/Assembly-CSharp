using System;

namespace DuloGames.UI
{
	// Token: 0x020005F7 RID: 1527
	public interface IUISlotHasCooldown
	{
		// Token: 0x0600219E RID: 8606
		UISpellInfo GetSpellInfo();

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x0600219F RID: 8607
		UISlotCooldown cooldownComponent { get; }

		// Token: 0x060021A0 RID: 8608
		void SetCooldownComponent(UISlotCooldown cooldown);
	}
}
