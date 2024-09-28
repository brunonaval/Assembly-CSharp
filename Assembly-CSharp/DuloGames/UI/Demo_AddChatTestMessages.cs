using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x0200059A RID: 1434
	public class Demo_AddChatTestMessages : MonoBehaviour
	{
		// Token: 0x06001FB6 RID: 8118 RVA: 0x0009F434 File Offset: 0x0009D634
		protected void Start()
		{
			this.AddMessages();
		}

		// Token: 0x06001FB7 RID: 8119 RVA: 0x0009F43C File Offset: 0x0009D63C
		[ContextMenu("Add Messages")]
		public void AddMessages()
		{
			if (this.m_Chat != null)
			{
				this.m_Chat.ReceiveChatMessage(1, "<b><color=#ce4627ff>Jeff</color></b> <color=#59524bff>said:</color> Eget vulputate justo, at molestie urna. Pellentesque eu nunc...");
				this.m_Chat.ReceiveChatMessage(1, "<b><color=#b59e8aff>Subzero</color></b> <color=#59524bff>said:</color> Phasellus eget vulputate justo, at molestie urna. Pellentesque zesu nunc gravida felis finibus maximus.");
				this.m_Chat.ReceiveChatMessage(1, "<b><color=#318fd0ff>Jossy</color></b> <color=#59524bff>said:</color> Shasellus eget lputate justo, at molestie urna. Pellentesque eu nunc gravida felis finibus amaximus  justo, at molestie urna.");
				this.m_Chat.ReceiveChatMessage(1, "<b><color=#ce4627ff>Jeff</color></b> <color=#59524bff>said:</color> Eget vulputate justo, at molestie urna. Pellentesque eu nunc...");
				this.m_Chat.ReceiveChatMessage(1, "<b><color=#b37b4aff>Gandalf</color></b> <color=#59524bff>said:</color> Phasellus eget vulputate justo, at molestie urna. Pellentesque eu nunc gravida felis finibus maximus.");
				this.m_Chat.ReceiveChatMessage(1, "<b><color=#318fd0ff>Jossy</color></b> <color=#59524bff>said:</color> Shasellus eget lputate justo, at molestie urna. Pellentesque eu nunc gravida felis finibus amaximus  justo, at molestie urna.");
				this.m_Chat.ReceiveChatMessage(1, "<b><color=#ce4627ff>Jeff</color></b> <color=#59524bff>said:</color> Eget vulputate justo, at molestie urna. Pellentesque eu nunc...");
				this.m_Chat.ReceiveChatMessage(1, "<b><color=#b59e8aff>Subzero</color></b> <color=#59524bff>said:</color> Phasellus eget vulputate justo, at molestie urna. Pellentesque zesu nunc gravida felis finibus maximus.");
			}
		}

		// Token: 0x04001995 RID: 6549
		[SerializeField]
		private Demo_Chat m_Chat;
	}
}
