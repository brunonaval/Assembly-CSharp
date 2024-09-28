using System;
using System.Collections;

// Token: 0x0200031B RID: 795
public static class DiscordWebhookManager
{
	// Token: 0x06000F22 RID: 3874 RVA: 0x0004788A File Offset: 0x00045A8A
	public static IEnumerator SendInGameMessagesChannel(string message)
	{
		message = "**(" + ServerSettingsManager.ServerName + "):** " + message;
		yield return ApiManager.Post<object>(ServerSettingsManager.DiscordMessagesChannelWebhook, new DiscordMessage
		{
			content = message
		}, null);
		yield break;
	}

	// Token: 0x06000F23 RID: 3875 RVA: 0x00047899 File Offset: 0x00045A99
	public static IEnumerator SendInGameCommandsChannel(string message)
	{
		message = "**(" + ServerSettingsManager.ServerName + "):** " + message;
		yield return ApiManager.Post<object>(ServerSettingsManager.DiscordCommandsChannelWebhook, new DiscordMessage
		{
			content = message
		}, null);
		yield break;
	}

	// Token: 0x06000F24 RID: 3876 RVA: 0x000478A8 File Offset: 0x00045AA8
	public static IEnumerator SendInGameEventsChannel(string message)
	{
		message = "**(" + ServerSettingsManager.ServerName + "):** " + message;
		yield return ApiManager.Post<object>(ServerSettingsManager.DiscordEventsChannelWebhook, new DiscordMessage
		{
			content = message
		}, null);
		yield break;
	}

	// Token: 0x06000F25 RID: 3877 RVA: 0x000478B7 File Offset: 0x00045AB7
	public static IEnumerator SendInLoginHistoryChannel(string message)
	{
		message = "**(" + ServerSettingsManager.ServerName + "):** " + message;
		yield return ApiManager.Post<object>(ServerSettingsManager.DiscordLoginHistoryChannelWebhook, new DiscordMessage
		{
			content = message
		}, null);
		yield break;
	}
}
