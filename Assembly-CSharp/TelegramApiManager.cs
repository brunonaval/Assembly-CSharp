using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000246 RID: 582
public class TelegramApiManager
{
	// Token: 0x06000862 RID: 2146 RVA: 0x00028297 File Offset: 0x00026497
	public static IEnumerator SendTelegramMessage(string botId, string chatId, string message)
	{
		if (Application.isEditor)
		{
			yield break;
		}
		yield return ApiManager.Post<object>("https://api.telegram.org/" + botId + "/sendMessage", new TelegramPayload
		{
			chat_id = chatId,
			text = "[" + ServerSettingsManager.ServerName + "]: " + message,
			disable_web_page_preview = true
		}, null);
		yield break;
	}
}
