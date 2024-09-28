using System;

// Token: 0x02000038 RID: 56
[Serializable]
public class ApiServer
{
	// Token: 0x040000E7 RID: 231
	public int Id;

	// Token: 0x040000E8 RID: 232
	public string Name;

	// Token: 0x040000E9 RID: 233
	public int CurrentPlayers;

	// Token: 0x040000EA RID: 234
	public int MaxPlayers;

	// Token: 0x040000EB RID: 235
	public string Address;

	// Token: 0x040000EC RID: 236
	public int Port;

	// Token: 0x040000ED RID: 237
	public DateTime Heartbeat;

	// Token: 0x040000EE RID: 238
	public string ConnectionVersion;

	// Token: 0x040000EF RID: 239
	public float CraftExperienceModifier;

	// Token: 0x040000F0 RID: 240
	public float ExperienceModifier;

	// Token: 0x040000F1 RID: 241
	public ServerType ServerType;
}
