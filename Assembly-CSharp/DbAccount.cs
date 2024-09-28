using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

// Token: 0x02000048 RID: 72
public static class DbAccount
{
	// Token: 0x060000BB RID: 187 RVA: 0x00004AB4 File Offset: 0x00002CB4
	public static Task SaveAccountAndClearIncomingGemsAsync(int accountId, int gems, int maxWarehouseSlots)
	{
		DbAccount.<SaveAccountAndClearIncomingGemsAsync>d__0 <SaveAccountAndClearIncomingGemsAsync>d__;
		<SaveAccountAndClearIncomingGemsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SaveAccountAndClearIncomingGemsAsync>d__.accountId = accountId;
		<SaveAccountAndClearIncomingGemsAsync>d__.gems = gems;
		<SaveAccountAndClearIncomingGemsAsync>d__.maxWarehouseSlots = maxWarehouseSlots;
		<SaveAccountAndClearIncomingGemsAsync>d__.<>1__state = -1;
		<SaveAccountAndClearIncomingGemsAsync>d__.<>t__builder.Start<DbAccount.<SaveAccountAndClearIncomingGemsAsync>d__0>(ref <SaveAccountAndClearIncomingGemsAsync>d__);
		return <SaveAccountAndClearIncomingGemsAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060000BC RID: 188 RVA: 0x00004B08 File Offset: 0x00002D08
	public static Task<int> GetIncomingGemsAsync(int accountId)
	{
		DbAccount.<GetIncomingGemsAsync>d__1 <GetIncomingGemsAsync>d__;
		<GetIncomingGemsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<int>.Create();
		<GetIncomingGemsAsync>d__.accountId = accountId;
		<GetIncomingGemsAsync>d__.<>1__state = -1;
		<GetIncomingGemsAsync>d__.<>t__builder.Start<DbAccount.<GetIncomingGemsAsync>d__1>(ref <GetIncomingGemsAsync>d__);
		return <GetIncomingGemsAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060000BD RID: 189 RVA: 0x00004B4C File Offset: 0x00002D4C
	public static Task AddPremiumDays(int accountId, int premiumDays, bool setGotFreePremium = false)
	{
		DbAccount.<AddPremiumDays>d__2 <AddPremiumDays>d__;
		<AddPremiumDays>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<AddPremiumDays>d__.accountId = accountId;
		<AddPremiumDays>d__.premiumDays = premiumDays;
		<AddPremiumDays>d__.setGotFreePremium = setGotFreePremium;
		<AddPremiumDays>d__.<>1__state = -1;
		<AddPremiumDays>d__.<>t__builder.Start<DbAccount.<AddPremiumDays>d__2>(ref <AddPremiumDays>d__);
		return <AddPremiumDays>d__.<>t__builder.Task;
	}

	// Token: 0x060000BE RID: 190 RVA: 0x00004BA0 File Offset: 0x00002DA0
	public static Task<bool> HasAnyPlayerOnlineAsync(int accountId)
	{
		DbAccount.<HasAnyPlayerOnlineAsync>d__3 <HasAnyPlayerOnlineAsync>d__;
		<HasAnyPlayerOnlineAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<HasAnyPlayerOnlineAsync>d__.accountId = accountId;
		<HasAnyPlayerOnlineAsync>d__.<>1__state = -1;
		<HasAnyPlayerOnlineAsync>d__.<>t__builder.Start<DbAccount.<HasAnyPlayerOnlineAsync>d__3>(ref <HasAnyPlayerOnlineAsync>d__);
		return <HasAnyPlayerOnlineAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060000BF RID: 191 RVA: 0x00004BE4 File Offset: 0x00002DE4
	public static Task<bool> HasOtherPlayersOnlineAsync(int accountId, int currentPlayerId)
	{
		DbAccount.<HasOtherPlayersOnlineAsync>d__4 <HasOtherPlayersOnlineAsync>d__;
		<HasOtherPlayersOnlineAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<HasOtherPlayersOnlineAsync>d__.accountId = accountId;
		<HasOtherPlayersOnlineAsync>d__.currentPlayerId = currentPlayerId;
		<HasOtherPlayersOnlineAsync>d__.<>1__state = -1;
		<HasOtherPlayersOnlineAsync>d__.<>t__builder.Start<DbAccount.<HasOtherPlayersOnlineAsync>d__4>(ref <HasOtherPlayersOnlineAsync>d__);
		return <HasOtherPlayersOnlineAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x00004C30 File Offset: 0x00002E30
	public static Task<DataRow> GetAccountAsync(int accountId)
	{
		DbAccount.<GetAccountAsync>d__5 <GetAccountAsync>d__;
		<GetAccountAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow>.Create();
		<GetAccountAsync>d__.accountId = accountId;
		<GetAccountAsync>d__.<>1__state = -1;
		<GetAccountAsync>d__.<>t__builder.Start<DbAccount.<GetAccountAsync>d__5>(ref <GetAccountAsync>d__);
		return <GetAccountAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x00004C74 File Offset: 0x00002E74
	public static Task UpdateGameRatingAsync(int accountId, int totalStars)
	{
		DbAccount.<UpdateGameRatingAsync>d__6 <UpdateGameRatingAsync>d__;
		<UpdateGameRatingAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<UpdateGameRatingAsync>d__.accountId = accountId;
		<UpdateGameRatingAsync>d__.totalStars = totalStars;
		<UpdateGameRatingAsync>d__.<>1__state = -1;
		<UpdateGameRatingAsync>d__.<>t__builder.Start<DbAccount.<UpdateGameRatingAsync>d__6>(ref <UpdateGameRatingAsync>d__);
		return <UpdateGameRatingAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x00004CC0 File Offset: 0x00002EC0
	public static Task SetMuteEndDateAsync(int accountId, int muteMinutes)
	{
		DbAccount.<SetMuteEndDateAsync>d__7 <SetMuteEndDateAsync>d__;
		<SetMuteEndDateAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SetMuteEndDateAsync>d__.accountId = accountId;
		<SetMuteEndDateAsync>d__.muteMinutes = muteMinutes;
		<SetMuteEndDateAsync>d__.<>1__state = -1;
		<SetMuteEndDateAsync>d__.<>t__builder.Start<DbAccount.<SetMuteEndDateAsync>d__7>(ref <SetMuteEndDateAsync>d__);
		return <SetMuteEndDateAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x00004D0C File Offset: 0x00002F0C
	public static Task SetWarningMessageAsync(int accountId, string warningMessage)
	{
		DbAccount.<SetWarningMessageAsync>d__8 <SetWarningMessageAsync>d__;
		<SetWarningMessageAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SetWarningMessageAsync>d__.accountId = accountId;
		<SetWarningMessageAsync>d__.warningMessage = warningMessage;
		<SetWarningMessageAsync>d__.<>1__state = -1;
		<SetWarningMessageAsync>d__.<>t__builder.Start<DbAccount.<SetWarningMessageAsync>d__8>(ref <SetWarningMessageAsync>d__);
		return <SetWarningMessageAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x00004D58 File Offset: 0x00002F58
	public static Task SetBannedAsync(int accountId, bool bannedValue, int banDays, string banReason)
	{
		DbAccount.<SetBannedAsync>d__9 <SetBannedAsync>d__;
		<SetBannedAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SetBannedAsync>d__.accountId = accountId;
		<SetBannedAsync>d__.bannedValue = bannedValue;
		<SetBannedAsync>d__.banDays = banDays;
		<SetBannedAsync>d__.banReason = banReason;
		<SetBannedAsync>d__.<>1__state = -1;
		<SetBannedAsync>d__.<>t__builder.Start<DbAccount.<SetBannedAsync>d__9>(ref <SetBannedAsync>d__);
		return <SetBannedAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x00004DB4 File Offset: 0x00002FB4
	public static Task UpdateDailyRewardAsync(int accountId, int currentDailyRewardId)
	{
		DbAccount.<UpdateDailyRewardAsync>d__10 <UpdateDailyRewardAsync>d__;
		<UpdateDailyRewardAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<UpdateDailyRewardAsync>d__.accountId = accountId;
		<UpdateDailyRewardAsync>d__.currentDailyRewardId = currentDailyRewardId;
		<UpdateDailyRewardAsync>d__.<>1__state = -1;
		<UpdateDailyRewardAsync>d__.<>t__builder.Start<DbAccount.<UpdateDailyRewardAsync>d__10>(ref <UpdateDailyRewardAsync>d__);
		return <UpdateDailyRewardAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x00004E00 File Offset: 0x00003000
	public static Task IncrementMaxCharacterSlotsAsync(int accountId, int amount)
	{
		DbAccount.<IncrementMaxCharacterSlotsAsync>d__11 <IncrementMaxCharacterSlotsAsync>d__;
		<IncrementMaxCharacterSlotsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<IncrementMaxCharacterSlotsAsync>d__.accountId = accountId;
		<IncrementMaxCharacterSlotsAsync>d__.amount = amount;
		<IncrementMaxCharacterSlotsAsync>d__.<>1__state = -1;
		<IncrementMaxCharacterSlotsAsync>d__.<>t__builder.Start<DbAccount.<IncrementMaxCharacterSlotsAsync>d__11>(ref <IncrementMaxCharacterSlotsAsync>d__);
		return <IncrementMaxCharacterSlotsAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x00004E4C File Offset: 0x0000304C
	public static Task UpgradeToVeteranPackageAsync(int accountId, int connectedPlayerId)
	{
		DbAccount.<UpgradeToVeteranPackageAsync>d__12 <UpgradeToVeteranPackageAsync>d__;
		<UpgradeToVeteranPackageAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<UpgradeToVeteranPackageAsync>d__.accountId = accountId;
		<UpgradeToVeteranPackageAsync>d__.connectedPlayerId = connectedPlayerId;
		<UpgradeToVeteranPackageAsync>d__.<>1__state = -1;
		<UpgradeToVeteranPackageAsync>d__.<>t__builder.Start<DbAccount.<UpgradeToVeteranPackageAsync>d__12>(ref <UpgradeToVeteranPackageAsync>d__);
		return <UpgradeToVeteranPackageAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x00004E98 File Offset: 0x00003098
	public static Task UpgradeToElitePackageAsync(int accountId, int connectedPlayerId)
	{
		DbAccount.<UpgradeToElitePackageAsync>d__13 <UpgradeToElitePackageAsync>d__;
		<UpgradeToElitePackageAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<UpgradeToElitePackageAsync>d__.accountId = accountId;
		<UpgradeToElitePackageAsync>d__.connectedPlayerId = connectedPlayerId;
		<UpgradeToElitePackageAsync>d__.<>1__state = -1;
		<UpgradeToElitePackageAsync>d__.<>t__builder.Start<DbAccount.<UpgradeToElitePackageAsync>d__13>(ref <UpgradeToElitePackageAsync>d__);
		return <UpgradeToElitePackageAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x00004EE4 File Offset: 0x000030E4
	public static Task UpgradeToLegendaryPackageAsync(int accountId, int connectedPlayerId)
	{
		DbAccount.<UpgradeToLegendaryPackageAsync>d__14 <UpgradeToLegendaryPackageAsync>d__;
		<UpgradeToLegendaryPackageAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<UpgradeToLegendaryPackageAsync>d__.accountId = accountId;
		<UpgradeToLegendaryPackageAsync>d__.connectedPlayerId = connectedPlayerId;
		<UpgradeToLegendaryPackageAsync>d__.<>1__state = -1;
		<UpgradeToLegendaryPackageAsync>d__.<>t__builder.Start<DbAccount.<UpgradeToLegendaryPackageAsync>d__14>(ref <UpgradeToLegendaryPackageAsync>d__);
		return <UpgradeToLegendaryPackageAsync>d__.<>t__builder.Task;
	}
}
