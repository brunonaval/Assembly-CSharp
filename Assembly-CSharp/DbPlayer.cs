using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

// Token: 0x0200009B RID: 155
public static class DbPlayer
{
	// Token: 0x0600018D RID: 397 RVA: 0x0000B3A0 File Offset: 0x000095A0
	public static Task<int> GetPlayerIdAsync(string playerName)
	{
		DbPlayer.<GetPlayerIdAsync>d__0 <GetPlayerIdAsync>d__;
		<GetPlayerIdAsync>d__.<>t__builder = AsyncTaskMethodBuilder<int>.Create();
		<GetPlayerIdAsync>d__.playerName = playerName;
		<GetPlayerIdAsync>d__.<>1__state = -1;
		<GetPlayerIdAsync>d__.<>t__builder.Start<DbPlayer.<GetPlayerIdAsync>d__0>(ref <GetPlayerIdAsync>d__);
		return <GetPlayerIdAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600018E RID: 398 RVA: 0x0000B3E4 File Offset: 0x000095E4
	public static Task<DataRow> GetPlayerAsync(string playerName)
	{
		DbPlayer.<GetPlayerAsync>d__1 <GetPlayerAsync>d__;
		<GetPlayerAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow>.Create();
		<GetPlayerAsync>d__.playerName = playerName;
		<GetPlayerAsync>d__.<>1__state = -1;
		<GetPlayerAsync>d__.<>t__builder.Start<DbPlayer.<GetPlayerAsync>d__1>(ref <GetPlayerAsync>d__);
		return <GetPlayerAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600018F RID: 399 RVA: 0x0000B428 File Offset: 0x00009628
	public static Task<DataRow> GetPlayerAsync(string accountUniqueId, int accountId, int playerId)
	{
		DbPlayer.<GetPlayerAsync>d__2 <GetPlayerAsync>d__;
		<GetPlayerAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow>.Create();
		<GetPlayerAsync>d__.accountUniqueId = accountUniqueId;
		<GetPlayerAsync>d__.accountId = accountId;
		<GetPlayerAsync>d__.playerId = playerId;
		<GetPlayerAsync>d__.<>1__state = -1;
		<GetPlayerAsync>d__.<>t__builder.Start<DbPlayer.<GetPlayerAsync>d__2>(ref <GetPlayerAsync>d__);
		return <GetPlayerAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000190 RID: 400 RVA: 0x0000B47C File Offset: 0x0000967C
	public static Task<DataRow> GetPlayerAsync(int playerId)
	{
		DbPlayer.<GetPlayerAsync>d__3 <GetPlayerAsync>d__;
		<GetPlayerAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow>.Create();
		<GetPlayerAsync>d__.playerId = playerId;
		<GetPlayerAsync>d__.<>1__state = -1;
		<GetPlayerAsync>d__.<>t__builder.Start<DbPlayer.<GetPlayerAsync>d__3>(ref <GetPlayerAsync>d__);
		return <GetPlayerAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000191 RID: 401 RVA: 0x0000B4C0 File Offset: 0x000096C0
	public static Task<DataRow[]> GetPlayersByServerAsync(int serverId)
	{
		DbPlayer.<GetPlayersByServerAsync>d__4 <GetPlayersByServerAsync>d__;
		<GetPlayersByServerAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow[]>.Create();
		<GetPlayersByServerAsync>d__.serverId = serverId;
		<GetPlayersByServerAsync>d__.<>1__state = -1;
		<GetPlayersByServerAsync>d__.<>t__builder.Start<DbPlayer.<GetPlayersByServerAsync>d__4>(ref <GetPlayersByServerAsync>d__);
		return <GetPlayersByServerAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000192 RID: 402 RVA: 0x0000B504 File Offset: 0x00009704
	public static Task MuteFriendAsync(int accountId, int friendPlayerId)
	{
		DbPlayer.<MuteFriendAsync>d__5 <MuteFriendAsync>d__;
		<MuteFriendAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<MuteFriendAsync>d__.accountId = accountId;
		<MuteFriendAsync>d__.friendPlayerId = friendPlayerId;
		<MuteFriendAsync>d__.<>1__state = -1;
		<MuteFriendAsync>d__.<>t__builder.Start<DbPlayer.<MuteFriendAsync>d__5>(ref <MuteFriendAsync>d__);
		return <MuteFriendAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000193 RID: 403 RVA: 0x0000B550 File Offset: 0x00009750
	public static Task UnMuteFriendAsync(int accountId, int friendPlayerId)
	{
		DbPlayer.<UnMuteFriendAsync>d__6 <UnMuteFriendAsync>d__;
		<UnMuteFriendAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<UnMuteFriendAsync>d__.accountId = accountId;
		<UnMuteFriendAsync>d__.friendPlayerId = friendPlayerId;
		<UnMuteFriendAsync>d__.<>1__state = -1;
		<UnMuteFriendAsync>d__.<>t__builder.Start<DbPlayer.<UnMuteFriendAsync>d__6>(ref <UnMuteFriendAsync>d__);
		return <UnMuteFriendAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000194 RID: 404 RVA: 0x0000B59C File Offset: 0x0000979C
	public static Task<DataRow[]> GetFriendListAsync(int accountId, int serverId)
	{
		DbPlayer.<GetFriendListAsync>d__7 <GetFriendListAsync>d__;
		<GetFriendListAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow[]>.Create();
		<GetFriendListAsync>d__.accountId = accountId;
		<GetFriendListAsync>d__.serverId = serverId;
		<GetFriendListAsync>d__.<>1__state = -1;
		<GetFriendListAsync>d__.<>t__builder.Start<DbPlayer.<GetFriendListAsync>d__7>(ref <GetFriendListAsync>d__);
		return <GetFriendListAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000195 RID: 405 RVA: 0x0000B5E8 File Offset: 0x000097E8
	public static Task ChangeNameAsync(int playerId, string newName)
	{
		DbPlayer.<ChangeNameAsync>d__8 <ChangeNameAsync>d__;
		<ChangeNameAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<ChangeNameAsync>d__.playerId = playerId;
		<ChangeNameAsync>d__.newName = newName;
		<ChangeNameAsync>d__.<>1__state = -1;
		<ChangeNameAsync>d__.<>t__builder.Start<DbPlayer.<ChangeNameAsync>d__8>(ref <ChangeNameAsync>d__);
		return <ChangeNameAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000196 RID: 406 RVA: 0x0000B634 File Offset: 0x00009834
	public static Task ChangeGenderAsync(int playerId, CreatureGender newGender)
	{
		DbPlayer.<ChangeGenderAsync>d__9 <ChangeGenderAsync>d__;
		<ChangeGenderAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<ChangeGenderAsync>d__.playerId = playerId;
		<ChangeGenderAsync>d__.newGender = newGender;
		<ChangeGenderAsync>d__.<>1__state = -1;
		<ChangeGenderAsync>d__.<>t__builder.Start<DbPlayer.<ChangeGenderAsync>d__9>(ref <ChangeGenderAsync>d__);
		return <ChangeGenderAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000197 RID: 407 RVA: 0x0000B680 File Offset: 0x00009880
	public static Task<bool> NameExistsAsync(string name)
	{
		DbPlayer.<NameExistsAsync>d__10 <NameExistsAsync>d__;
		<NameExistsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<NameExistsAsync>d__.name = name;
		<NameExistsAsync>d__.<>1__state = -1;
		<NameExistsAsync>d__.<>t__builder.Start<DbPlayer.<NameExistsAsync>d__10>(ref <NameExistsAsync>d__);
		return <NameExistsAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000198 RID: 408 RVA: 0x0000B6C4 File Offset: 0x000098C4
	public static Task SavePlayerAsync(int playerId, Vocation vocation, int currentHealth, int currentEndurance, int baseLevel, long baseExperience, int powerLevel, long powerExperience, int toughnessLevel, long toughnessExperience, int agilityLevel, long agilityExperience, int precisionLevel, long precisionExperience, int vitalityLevel, long vitalityExperience, float positionX, float positionY, float positionZ, long goldCoins, int combatScore, int killScore, int weekTvtScore, int karmaPoints, bool pvpEnabled, PvpStatus pvpStatus, int? titleId, bool isDead, bool isOnline, int maxInventorySlots, string spawnPoint, int professionLevel, long professionExperience, PlayerProfession profession, int masteryLevel, TrainingMode trainingMode)
	{
		DbPlayer.<SavePlayerAsync>d__11 <SavePlayerAsync>d__;
		<SavePlayerAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SavePlayerAsync>d__.playerId = playerId;
		<SavePlayerAsync>d__.vocation = vocation;
		<SavePlayerAsync>d__.currentHealth = currentHealth;
		<SavePlayerAsync>d__.currentEndurance = currentEndurance;
		<SavePlayerAsync>d__.baseLevel = baseLevel;
		<SavePlayerAsync>d__.baseExperience = baseExperience;
		<SavePlayerAsync>d__.powerLevel = powerLevel;
		<SavePlayerAsync>d__.powerExperience = powerExperience;
		<SavePlayerAsync>d__.toughnessLevel = toughnessLevel;
		<SavePlayerAsync>d__.toughnessExperience = toughnessExperience;
		<SavePlayerAsync>d__.agilityLevel = agilityLevel;
		<SavePlayerAsync>d__.agilityExperience = agilityExperience;
		<SavePlayerAsync>d__.precisionLevel = precisionLevel;
		<SavePlayerAsync>d__.precisionExperience = precisionExperience;
		<SavePlayerAsync>d__.vitalityLevel = vitalityLevel;
		<SavePlayerAsync>d__.vitalityExperience = vitalityExperience;
		<SavePlayerAsync>d__.positionX = positionX;
		<SavePlayerAsync>d__.positionY = positionY;
		<SavePlayerAsync>d__.positionZ = positionZ;
		<SavePlayerAsync>d__.goldCoins = goldCoins;
		<SavePlayerAsync>d__.combatScore = combatScore;
		<SavePlayerAsync>d__.killScore = killScore;
		<SavePlayerAsync>d__.weekTvtScore = weekTvtScore;
		<SavePlayerAsync>d__.karmaPoints = karmaPoints;
		<SavePlayerAsync>d__.pvpEnabled = pvpEnabled;
		<SavePlayerAsync>d__.pvpStatus = pvpStatus;
		<SavePlayerAsync>d__.titleId = titleId;
		<SavePlayerAsync>d__.isDead = isDead;
		<SavePlayerAsync>d__.isOnline = isOnline;
		<SavePlayerAsync>d__.maxInventorySlots = maxInventorySlots;
		<SavePlayerAsync>d__.spawnPoint = spawnPoint;
		<SavePlayerAsync>d__.professionLevel = professionLevel;
		<SavePlayerAsync>d__.professionExperience = professionExperience;
		<SavePlayerAsync>d__.profession = profession;
		<SavePlayerAsync>d__.masteryLevel = masteryLevel;
		<SavePlayerAsync>d__.trainingMode = trainingMode;
		<SavePlayerAsync>d__.<>1__state = -1;
		<SavePlayerAsync>d__.<>t__builder.Start<DbPlayer.<SavePlayerAsync>d__11>(ref <SavePlayerAsync>d__);
		return <SavePlayerAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000199 RID: 409 RVA: 0x0000B840 File Offset: 0x00009A40
	public static Task SetPvpStatusAsync(int playerId, PvpStatus pvpStatus)
	{
		DbPlayer.<SetPvpStatusAsync>d__12 <SetPvpStatusAsync>d__;
		<SetPvpStatusAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SetPvpStatusAsync>d__.playerId = playerId;
		<SetPvpStatusAsync>d__.pvpStatus = pvpStatus;
		<SetPvpStatusAsync>d__.<>1__state = -1;
		<SetPvpStatusAsync>d__.<>t__builder.Start<DbPlayer.<SetPvpStatusAsync>d__12>(ref <SetPvpStatusAsync>d__);
		return <SetPvpStatusAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600019A RID: 410 RVA: 0x0000B88C File Offset: 0x00009A8C
	public static Task SetPlatformAsync(int playerId, string platform, string address)
	{
		DbPlayer.<SetPlatformAsync>d__13 <SetPlatformAsync>d__;
		<SetPlatformAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SetPlatformAsync>d__.playerId = playerId;
		<SetPlatformAsync>d__.platform = platform;
		<SetPlatformAsync>d__.address = address;
		<SetPlatformAsync>d__.<>1__state = -1;
		<SetPlatformAsync>d__.<>t__builder.Start<DbPlayer.<SetPlatformAsync>d__13>(ref <SetPlatformAsync>d__);
		return <SetPlatformAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600019B RID: 411 RVA: 0x0000B8E0 File Offset: 0x00009AE0
	public static Task UpdateTvtHeroAndWeekScoreAsync(int serverId)
	{
		DbPlayer.<UpdateTvtHeroAndWeekScoreAsync>d__14 <UpdateTvtHeroAndWeekScoreAsync>d__;
		<UpdateTvtHeroAndWeekScoreAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<UpdateTvtHeroAndWeekScoreAsync>d__.serverId = serverId;
		<UpdateTvtHeroAndWeekScoreAsync>d__.<>1__state = -1;
		<UpdateTvtHeroAndWeekScoreAsync>d__.<>t__builder.Start<DbPlayer.<UpdateTvtHeroAndWeekScoreAsync>d__14>(ref <UpdateTvtHeroAndWeekScoreAsync>d__);
		return <UpdateTvtHeroAndWeekScoreAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600019C RID: 412 RVA: 0x0000B924 File Offset: 0x00009B24
	public static Task SetOnlineAsync(int playerId)
	{
		DbPlayer.<SetOnlineAsync>d__15 <SetOnlineAsync>d__;
		<SetOnlineAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SetOnlineAsync>d__.playerId = playerId;
		<SetOnlineAsync>d__.<>1__state = -1;
		<SetOnlineAsync>d__.<>t__builder.Start<DbPlayer.<SetOnlineAsync>d__15>(ref <SetOnlineAsync>d__);
		return <SetOnlineAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600019D RID: 413 RVA: 0x0000B968 File Offset: 0x00009B68
	public static Task SetOfflineAsync(int playerId)
	{
		DbPlayer.<SetOfflineAsync>d__16 <SetOfflineAsync>d__;
		<SetOfflineAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SetOfflineAsync>d__.playerId = playerId;
		<SetOfflineAsync>d__.<>1__state = -1;
		<SetOfflineAsync>d__.<>t__builder.Start<DbPlayer.<SetOfflineAsync>d__16>(ref <SetOfflineAsync>d__);
		return <SetOfflineAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600019E RID: 414 RVA: 0x0000B9AC File Offset: 0x00009BAC
	public static Task SetOfflineToAllAsync(int serverId)
	{
		DbPlayer.<SetOfflineToAllAsync>d__17 <SetOfflineToAllAsync>d__;
		<SetOfflineToAllAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SetOfflineToAllAsync>d__.serverId = serverId;
		<SetOfflineToAllAsync>d__.<>1__state = -1;
		<SetOfflineToAllAsync>d__.<>t__builder.Start<DbPlayer.<SetOfflineToAllAsync>d__17>(ref <SetOfflineToAllAsync>d__);
		return <SetOfflineToAllAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600019F RID: 415 RVA: 0x0000B9F0 File Offset: 0x00009BF0
	public static Task<int> GetAccountIdAsync(string playerName)
	{
		DbPlayer.<GetAccountIdAsync>d__18 <GetAccountIdAsync>d__;
		<GetAccountIdAsync>d__.<>t__builder = AsyncTaskMethodBuilder<int>.Create();
		<GetAccountIdAsync>d__.playerName = playerName;
		<GetAccountIdAsync>d__.<>1__state = -1;
		<GetAccountIdAsync>d__.<>t__builder.Start<DbPlayer.<GetAccountIdAsync>d__18>(ref <GetAccountIdAsync>d__);
		return <GetAccountIdAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x0000BA34 File Offset: 0x00009C34
	public static Task RemoveFriendAsync(int accountId, int friendPlayerId)
	{
		DbPlayer.<RemoveFriendAsync>d__19 <RemoveFriendAsync>d__;
		<RemoveFriendAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<RemoveFriendAsync>d__.accountId = accountId;
		<RemoveFriendAsync>d__.friendPlayerId = friendPlayerId;
		<RemoveFriendAsync>d__.<>1__state = -1;
		<RemoveFriendAsync>d__.<>t__builder.Start<DbPlayer.<RemoveFriendAsync>d__19>(ref <RemoveFriendAsync>d__);
		return <RemoveFriendAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060001A1 RID: 417 RVA: 0x0000BA80 File Offset: 0x00009C80
	public static Task AddFriendAsync(int accountId, int friendPlayerId)
	{
		DbPlayer.<AddFriendAsync>d__20 <AddFriendAsync>d__;
		<AddFriendAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<AddFriendAsync>d__.accountId = accountId;
		<AddFriendAsync>d__.friendPlayerId = friendPlayerId;
		<AddFriendAsync>d__.<>1__state = -1;
		<AddFriendAsync>d__.<>t__builder.Start<DbPlayer.<AddFriendAsync>d__20>(ref <AddFriendAsync>d__);
		return <AddFriendAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060001A2 RID: 418 RVA: 0x0000BACC File Offset: 0x00009CCC
	public static Task<long> GetGoldCoinsAsync(int playerId)
	{
		DbPlayer.<GetGoldCoinsAsync>d__21 <GetGoldCoinsAsync>d__;
		<GetGoldCoinsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<long>.Create();
		<GetGoldCoinsAsync>d__.playerId = playerId;
		<GetGoldCoinsAsync>d__.<>1__state = -1;
		<GetGoldCoinsAsync>d__.<>t__builder.Start<DbPlayer.<GetGoldCoinsAsync>d__21>(ref <GetGoldCoinsAsync>d__);
		return <GetGoldCoinsAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060001A3 RID: 419 RVA: 0x0000BB10 File Offset: 0x00009D10
	public static Task<bool> PlayerHasGuildAsync(int playerId)
	{
		DbPlayer.<PlayerHasGuildAsync>d__22 <PlayerHasGuildAsync>d__;
		<PlayerHasGuildAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<PlayerHasGuildAsync>d__.playerId = playerId;
		<PlayerHasGuildAsync>d__.<>1__state = -1;
		<PlayerHasGuildAsync>d__.<>t__builder.Start<DbPlayer.<PlayerHasGuildAsync>d__22>(ref <PlayerHasGuildAsync>d__);
		return <PlayerHasGuildAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060001A4 RID: 420 RVA: 0x0000BB54 File Offset: 0x00009D54
	public static Task AddDeathHistory(int playerId, string killerName, int killerBaseLevel, int baseLevel, long baseExperienceBeforeDeath, long baseExperienceAfterDeath)
	{
		DbPlayer.<AddDeathHistory>d__23 <AddDeathHistory>d__;
		<AddDeathHistory>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<AddDeathHistory>d__.playerId = playerId;
		<AddDeathHistory>d__.killerName = killerName;
		<AddDeathHistory>d__.killerBaseLevel = killerBaseLevel;
		<AddDeathHistory>d__.baseLevel = baseLevel;
		<AddDeathHistory>d__.baseExperienceBeforeDeath = baseExperienceBeforeDeath;
		<AddDeathHistory>d__.baseExperienceAfterDeath = baseExperienceAfterDeath;
		<AddDeathHistory>d__.<>1__state = -1;
		<AddDeathHistory>d__.<>t__builder.Start<DbPlayer.<AddDeathHistory>d__23>(ref <AddDeathHistory>d__);
		return <AddDeathHistory>d__.<>t__builder.Task;
	}
}
