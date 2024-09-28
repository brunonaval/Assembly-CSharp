using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

// Token: 0x02000066 RID: 102
public static class DbGuild
{
	// Token: 0x06000109 RID: 265 RVA: 0x0000748C File Offset: 0x0000568C
	public static Task<bool> NameExistsAsync(string name)
	{
		DbGuild.<NameExistsAsync>d__0 <NameExistsAsync>d__;
		<NameExistsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<NameExistsAsync>d__.name = name;
		<NameExistsAsync>d__.<>1__state = -1;
		<NameExistsAsync>d__.<>t__builder.Start<DbGuild.<NameExistsAsync>d__0>(ref <NameExistsAsync>d__);
		return <NameExistsAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600010A RID: 266 RVA: 0x000074D0 File Offset: 0x000056D0
	public static Task<int> CreateGuildAsync(string guildName, int ownerPlayerId)
	{
		DbGuild.<CreateGuildAsync>d__1 <CreateGuildAsync>d__;
		<CreateGuildAsync>d__.<>t__builder = AsyncTaskMethodBuilder<int>.Create();
		<CreateGuildAsync>d__.guildName = guildName;
		<CreateGuildAsync>d__.ownerPlayerId = ownerPlayerId;
		<CreateGuildAsync>d__.<>1__state = -1;
		<CreateGuildAsync>d__.<>t__builder.Start<DbGuild.<CreateGuildAsync>d__1>(ref <CreateGuildAsync>d__);
		return <CreateGuildAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600010B RID: 267 RVA: 0x0000751C File Offset: 0x0000571C
	public static Task ChangeGuildAsync(int guildId, string newGuildName, int newLeaderPlayerId)
	{
		DbGuild.<ChangeGuildAsync>d__2 <ChangeGuildAsync>d__;
		<ChangeGuildAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<ChangeGuildAsync>d__.guildId = guildId;
		<ChangeGuildAsync>d__.newGuildName = newGuildName;
		<ChangeGuildAsync>d__.newLeaderPlayerId = newLeaderPlayerId;
		<ChangeGuildAsync>d__.<>1__state = -1;
		<ChangeGuildAsync>d__.<>t__builder.Start<DbGuild.<ChangeGuildAsync>d__2>(ref <ChangeGuildAsync>d__);
		return <ChangeGuildAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600010C RID: 268 RVA: 0x00007570 File Offset: 0x00005770
	public static Task<bool> PlayerIsGuildLeaderAsync(int playerId, int guildId)
	{
		DbGuild.<PlayerIsGuildLeaderAsync>d__3 <PlayerIsGuildLeaderAsync>d__;
		<PlayerIsGuildLeaderAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<PlayerIsGuildLeaderAsync>d__.playerId = playerId;
		<PlayerIsGuildLeaderAsync>d__.guildId = guildId;
		<PlayerIsGuildLeaderAsync>d__.<>1__state = -1;
		<PlayerIsGuildLeaderAsync>d__.<>t__builder.Start<DbGuild.<PlayerIsGuildLeaderAsync>d__3>(ref <PlayerIsGuildLeaderAsync>d__);
		return <PlayerIsGuildLeaderAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600010D RID: 269 RVA: 0x000075BC File Offset: 0x000057BC
	public static Task<bool> MemberExistsAsync(int guildId, int playerId)
	{
		DbGuild.<MemberExistsAsync>d__4 <MemberExistsAsync>d__;
		<MemberExistsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<MemberExistsAsync>d__.guildId = guildId;
		<MemberExistsAsync>d__.playerId = playerId;
		<MemberExistsAsync>d__.<>1__state = -1;
		<MemberExistsAsync>d__.<>t__builder.Start<DbGuild.<MemberExistsAsync>d__4>(ref <MemberExistsAsync>d__);
		return <MemberExistsAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600010E RID: 270 RVA: 0x00007608 File Offset: 0x00005808
	public static Task<bool> MemberExistsAsync(int guildId, int playerId, GuildMemberRank guildMemberRank)
	{
		DbGuild.<MemberExistsAsync>d__5 <MemberExistsAsync>d__;
		<MemberExistsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<MemberExistsAsync>d__.guildId = guildId;
		<MemberExistsAsync>d__.playerId = playerId;
		<MemberExistsAsync>d__.guildMemberRank = guildMemberRank;
		<MemberExistsAsync>d__.<>1__state = -1;
		<MemberExistsAsync>d__.<>t__builder.Start<DbGuild.<MemberExistsAsync>d__5>(ref <MemberExistsAsync>d__);
		return <MemberExistsAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600010F RID: 271 RVA: 0x0000765C File Offset: 0x0000585C
	public static Task UpdateMemberRankAsync(int guildId, int updatedMemberPlayerId, GuildMemberRank newGuildMemberRank)
	{
		DbGuild.<UpdateMemberRankAsync>d__6 <UpdateMemberRankAsync>d__;
		<UpdateMemberRankAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<UpdateMemberRankAsync>d__.guildId = guildId;
		<UpdateMemberRankAsync>d__.updatedMemberPlayerId = updatedMemberPlayerId;
		<UpdateMemberRankAsync>d__.newGuildMemberRank = newGuildMemberRank;
		<UpdateMemberRankAsync>d__.<>1__state = -1;
		<UpdateMemberRankAsync>d__.<>t__builder.Start<DbGuild.<UpdateMemberRankAsync>d__6>(ref <UpdateMemberRankAsync>d__);
		return <UpdateMemberRankAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000110 RID: 272 RVA: 0x000076B0 File Offset: 0x000058B0
	public static Task AddMemberToGuildAsync(int guildId, int memberPlayerId, GuildMemberRank memberRank)
	{
		DbGuild.<AddMemberToGuildAsync>d__7 <AddMemberToGuildAsync>d__;
		<AddMemberToGuildAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<AddMemberToGuildAsync>d__.guildId = guildId;
		<AddMemberToGuildAsync>d__.memberPlayerId = memberPlayerId;
		<AddMemberToGuildAsync>d__.memberRank = memberRank;
		<AddMemberToGuildAsync>d__.<>1__state = -1;
		<AddMemberToGuildAsync>d__.<>t__builder.Start<DbGuild.<AddMemberToGuildAsync>d__7>(ref <AddMemberToGuildAsync>d__);
		return <AddMemberToGuildAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000111 RID: 273 RVA: 0x00007704 File Offset: 0x00005904
	public static Task RemoveMemberFromGuildAsync(int guildId, int removedMemberPlayerId)
	{
		DbGuild.<RemoveMemberFromGuildAsync>d__8 <RemoveMemberFromGuildAsync>d__;
		<RemoveMemberFromGuildAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<RemoveMemberFromGuildAsync>d__.guildId = guildId;
		<RemoveMemberFromGuildAsync>d__.removedMemberPlayerId = removedMemberPlayerId;
		<RemoveMemberFromGuildAsync>d__.<>1__state = -1;
		<RemoveMemberFromGuildAsync>d__.<>t__builder.Start<DbGuild.<RemoveMemberFromGuildAsync>d__8>(ref <RemoveMemberFromGuildAsync>d__);
		return <RemoveMemberFromGuildAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000112 RID: 274 RVA: 0x00007750 File Offset: 0x00005950
	public static Task DeleteGuildAsync(int guildId)
	{
		DbGuild.<DeleteGuildAsync>d__9 <DeleteGuildAsync>d__;
		<DeleteGuildAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<DeleteGuildAsync>d__.guildId = guildId;
		<DeleteGuildAsync>d__.<>1__state = -1;
		<DeleteGuildAsync>d__.<>t__builder.Start<DbGuild.<DeleteGuildAsync>d__9>(ref <DeleteGuildAsync>d__);
		return <DeleteGuildAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000113 RID: 275 RVA: 0x00007794 File Offset: 0x00005994
	public static Task<DataRow[]> GetMembersAsync(int guildId)
	{
		DbGuild.<GetMembersAsync>d__10 <GetMembersAsync>d__;
		<GetMembersAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow[]>.Create();
		<GetMembersAsync>d__.guildId = guildId;
		<GetMembersAsync>d__.<>1__state = -1;
		<GetMembersAsync>d__.<>t__builder.Start<DbGuild.<GetMembersAsync>d__10>(ref <GetMembersAsync>d__);
		return <GetMembersAsync>d__.<>t__builder.Task;
	}
}
