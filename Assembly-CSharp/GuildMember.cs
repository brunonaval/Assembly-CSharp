using System;

// Token: 0x02000341 RID: 833
[Serializable]
public class GuildMember
{
	// Token: 0x04000FC2 RID: 4034
	public int MemberPlayerId;

	// Token: 0x04000FC3 RID: 4035
	public string Name;

	// Token: 0x04000FC4 RID: 4036
	public int BaseLevel;

	// Token: 0x04000FC5 RID: 4037
	public Vocation Vocation;

	// Token: 0x04000FC6 RID: 4038
	public GuildMemberRank MemberRank;

	// Token: 0x04000FC7 RID: 4039
	public int MemberSinceDays;

	// Token: 0x04000FC8 RID: 4040
	public int DaysSinceLastLogin;
}
