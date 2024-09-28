using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x02000643 RID: 1603
	[Serializable]
	public class AnimationTriggersExtended
	{
		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06002346 RID: 9030 RVA: 0x000AE191 File Offset: 0x000AC391
		// (set) Token: 0x06002347 RID: 9031 RVA: 0x000AE199 File Offset: 0x000AC399
		public string normalTrigger
		{
			get
			{
				return this.m_NormalTrigger;
			}
			set
			{
				this.m_NormalTrigger = value;
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06002348 RID: 9032 RVA: 0x000AE1A2 File Offset: 0x000AC3A2
		// (set) Token: 0x06002349 RID: 9033 RVA: 0x000AE1AA File Offset: 0x000AC3AA
		public string highlightedTrigger
		{
			get
			{
				return this.m_HighlightedTrigger;
			}
			set
			{
				this.m_HighlightedTrigger = value;
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x0600234A RID: 9034 RVA: 0x000AE1B3 File Offset: 0x000AC3B3
		// (set) Token: 0x0600234B RID: 9035 RVA: 0x000AE1BB File Offset: 0x000AC3BB
		public string pressedTrigger
		{
			get
			{
				return this.m_PressedTrigger;
			}
			set
			{
				this.m_PressedTrigger = value;
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x0600234C RID: 9036 RVA: 0x000AE1C4 File Offset: 0x000AC3C4
		// (set) Token: 0x0600234D RID: 9037 RVA: 0x000AE1CC File Offset: 0x000AC3CC
		public string activeTrigger
		{
			get
			{
				return this.m_ActiveTrigger;
			}
			set
			{
				this.m_ActiveTrigger = value;
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x0600234E RID: 9038 RVA: 0x000AE1D5 File Offset: 0x000AC3D5
		// (set) Token: 0x0600234F RID: 9039 RVA: 0x000AE1DD File Offset: 0x000AC3DD
		public string activeHighlightedTrigger
		{
			get
			{
				return this.m_ActiveHighlightedTrigger;
			}
			set
			{
				this.m_ActiveHighlightedTrigger = value;
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06002350 RID: 9040 RVA: 0x000AE1E6 File Offset: 0x000AC3E6
		// (set) Token: 0x06002351 RID: 9041 RVA: 0x000AE1EE File Offset: 0x000AC3EE
		public string activePressedTrigger
		{
			get
			{
				return this.m_ActivePressedTrigger;
			}
			set
			{
				this.m_ActivePressedTrigger = value;
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06002352 RID: 9042 RVA: 0x000AE1F7 File Offset: 0x000AC3F7
		// (set) Token: 0x06002353 RID: 9043 RVA: 0x000AE1FF File Offset: 0x000AC3FF
		public string disabledTrigger
		{
			get
			{
				return this.m_DisabledTrigger;
			}
			set
			{
				this.m_DisabledTrigger = value;
			}
		}

		// Token: 0x04001C95 RID: 7317
		[SerializeField]
		private string m_NormalTrigger = "Normal";

		// Token: 0x04001C96 RID: 7318
		[SerializeField]
		private string m_HighlightedTrigger = "Highlighted";

		// Token: 0x04001C97 RID: 7319
		[SerializeField]
		private string m_PressedTrigger = "Pressed";

		// Token: 0x04001C98 RID: 7320
		[SerializeField]
		private string m_ActiveTrigger = "Active";

		// Token: 0x04001C99 RID: 7321
		[SerializeField]
		private string m_ActiveHighlightedTrigger = "ActiveHighlighted";

		// Token: 0x04001C9A RID: 7322
		[SerializeField]
		private string m_ActivePressedTrigger = "ActivePressed";

		// Token: 0x04001C9B RID: 7323
		[SerializeField]
		private string m_DisabledTrigger = "Disabled";
	}
}
