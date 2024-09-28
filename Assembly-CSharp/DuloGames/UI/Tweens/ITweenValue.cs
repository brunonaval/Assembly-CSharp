using System;

namespace DuloGames.UI.Tweens
{
	// Token: 0x02000681 RID: 1665
	internal interface ITweenValue
	{
		// Token: 0x060024FC RID: 9468
		void TweenValue(float floatPercentage);

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x060024FD RID: 9469
		bool ignoreTimeScale { get; }

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x060024FE RID: 9470
		float duration { get; }

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x060024FF RID: 9471
		TweenEasing easing { get; }

		// Token: 0x06002500 RID: 9472
		bool ValidTarget();

		// Token: 0x06002501 RID: 9473
		void Finished();
	}
}
