using System;
using System.IO;
using UnityEngine;

// Token: 0x02000411 RID: 1041
public static class ServerSettingsManager
{
	// Token: 0x06001683 RID: 5763 RVA: 0x00072E50 File Offset: 0x00071050
	public static void LoadServerSettings()
	{
		string text = Path.Combine(Environment.CurrentDirectory, "Assets");
		text = Path.Combine(text, "Miscellaneous");
		text = Path.Combine(text, "Settings");
		text = Path.Combine(text, "server.json");
		if (File.Exists(text))
		{
			ServerSettings serverSettings = JsonUtility.FromJson<ServerSettings>(File.ReadAllText(text));
			ServerSettingsManager.TelegramBotId = serverSettings.TelegramBotId;
			ServerSettingsManager.TelegramChatId = serverSettings.TelegramChatId;
			ServerSettingsManager.DiscordWebhook = serverSettings.DiscordWebhook;
			ServerSettingsManager.ServerIdentifier = serverSettings.ServerIdentifier;
			return;
		}
		Debug.LogError("[SettingsModule] Missing server.json file on path: " + text);
	}

	// Token: 0x04001445 RID: 5189
	public static string TelegramBotId;

	// Token: 0x04001446 RID: 5190
	public static string DiscordWebhook;

	// Token: 0x04001447 RID: 5191
	public static string TelegramChatId;

	// Token: 0x04001448 RID: 5192
	public static string ServerIdentifier;

	// Token: 0x04001449 RID: 5193
	public static int ServerId;

	// Token: 0x0400144A RID: 5194
	public static string ServerName;

	// Token: 0x0400144B RID: 5195
	public static ServerType ServerType;

	// Token: 0x0400144C RID: 5196
	public static float ExperienceModifier;

	// Token: 0x0400144D RID: 5197
	public static float CraftExperienceModifier;

	// Token: 0x0400144E RID: 5198
	public static string ServerConnectionVersion;

	// Token: 0x0400144F RID: 5199
	public static string DiscordEventsChannelWebhook;

	// Token: 0x04001450 RID: 5200
	public static string DiscordMessagesChannelWebhook;

	// Token: 0x04001451 RID: 5201
	public static string DiscordCommandsChannelWebhook;

	// Token: 0x04001452 RID: 5202
	public static string DiscordLoginHistoryChannelWebhook;

	// Token: 0x04001453 RID: 5203
	public static bool CopperEventActive;

	// Token: 0x04001454 RID: 5204
	public static bool SilverEventActive;

	// Token: 0x04001455 RID: 5205
	public static bool GoldEventActive;
}
