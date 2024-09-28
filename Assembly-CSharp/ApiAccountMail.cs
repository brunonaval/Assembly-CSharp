using System;

// Token: 0x0200002E RID: 46
[Serializable]
public class ApiAccountMail
{
	// Token: 0x04000088 RID: 136
	public Guid Id;

	// Token: 0x04000089 RID: 137
	public string Subject;

	// Token: 0x0400008A RID: 138
	public string Content;

	// Token: 0x0400008B RID: 139
	public string SenderName;

	// Token: 0x0400008C RID: 140
	public bool SentBySystem;

	// Token: 0x0400008D RID: 141
	public int ReceiverId;

	// Token: 0x0400008E RID: 142
	public int GoldCoins;

	// Token: 0x0400008F RID: 143
	public ApiItem Item1;

	// Token: 0x04000090 RID: 144
	public int Item1Amount;

	// Token: 0x04000091 RID: 145
	public ApiItem Item2;

	// Token: 0x04000092 RID: 146
	public int Item2Amount;

	// Token: 0x04000093 RID: 147
	public ApiItem Item3;

	// Token: 0x04000094 RID: 148
	public int Item3Amount;

	// Token: 0x04000095 RID: 149
	public ApiItem Item4;

	// Token: 0x04000096 RID: 150
	public int Item4Amount;
}
