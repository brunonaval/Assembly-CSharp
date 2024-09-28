using System;
using System.Collections;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x020005A7 RID: 1447
	public class Test_CastBar : MonoBehaviour
	{
		// Token: 0x06001FED RID: 8173 RVA: 0x000A0594 File Offset: 0x0009E794
		private void Start()
		{
			if (this.m_CastBar != null && UISpellDatabase.Instance != null)
			{
				this.spell1 = UISpellDatabase.Instance.Get(0);
				this.spell2 = UISpellDatabase.Instance.Get(2);
				base.StartCoroutine("StartTestRoutine");
			}
		}

		// Token: 0x06001FEE RID: 8174 RVA: 0x000A05EA File Offset: 0x0009E7EA
		private IEnumerator StartTestRoutine()
		{
			yield return new WaitForSeconds(1f);
			this.m_CastBar.StartCasting(this.spell1, this.spell1.CastTime, Time.time + this.spell1.CastTime);
			yield return new WaitForSeconds(1f + this.spell1.CastTime);
			this.m_CastBar.StartCasting(this.spell2, this.spell2.CastTime, Time.time + this.spell2.CastTime);
			yield return new WaitForSeconds(this.spell2.CastTime * 0.75f);
			this.m_CastBar.Interrupt();
			base.StartCoroutine("StartTestRoutine");
			yield break;
		}

		// Token: 0x040019C1 RID: 6593
		[SerializeField]
		private UICastBar m_CastBar;

		// Token: 0x040019C2 RID: 6594
		private UISpellInfo spell1;

		// Token: 0x040019C3 RID: 6595
		private UISpellInfo spell2;
	}
}
