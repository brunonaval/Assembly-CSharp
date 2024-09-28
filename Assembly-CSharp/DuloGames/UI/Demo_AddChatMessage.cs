using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x02000599 RID: 1433
	public class Demo_AddChatMessage : MonoBehaviour
	{
		// Token: 0x06001FB4 RID: 8116 RVA: 0x0009F3AC File Offset: 0x0009D5AC
		public void OnSendMessage(int tabId, string text)
		{
			if (this.m_Chat != null)
			{
				this.m_Chat.ReceiveChatMessage(tabId, string.Concat(new string[]
				{
					"<color=#",
					CommonColorBuffer.ColorToString(this.m_PlayerColor),
					"><b>",
					this.m_PlayerName,
					"</b></color> <color=#59524bff>said: </color>",
					text
				}));
			}
		}

		// Token: 0x04001992 RID: 6546
		[SerializeField]
		private Demo_Chat m_Chat;

		// Token: 0x04001993 RID: 6547
		[SerializeField]
		private string m_PlayerName = "Player";

		// Token: 0x04001994 RID: 6548
		[SerializeField]
		private Color m_PlayerColor = Color.white;
	}
}
