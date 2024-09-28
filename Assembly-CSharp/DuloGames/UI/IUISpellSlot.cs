using System;

namespace DuloGames.UI
{
	// Token: 0x020005F8 RID: 1528
	public interface IUISpellSlot
	{
		// Token: 0x060021A1 RID: 8609
		UISpellInfo GetSpellInfo();

		// Token: 0x060021A2 RID: 8610
		bool Assign(UISpellInfo spellInfo);

		// Token: 0x060021A3 RID: 8611
		void Unassign();
	}
}
