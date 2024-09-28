using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

// Token: 0x020000B4 RID: 180
public static class DbPlayerCondition
{
	// Token: 0x060001D5 RID: 469 RVA: 0x0000D754 File Offset: 0x0000B954
	public static Task<DataRow[]> GetConditionsAsync(int playerId)
	{
		DbPlayerCondition.<GetConditionsAsync>d__0 <GetConditionsAsync>d__;
		<GetConditionsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow[]>.Create();
		<GetConditionsAsync>d__.playerId = playerId;
		<GetConditionsAsync>d__.<>1__state = -1;
		<GetConditionsAsync>d__.<>t__builder.Start<DbPlayerCondition.<GetConditionsAsync>d__0>(ref <GetConditionsAsync>d__);
		return <GetConditionsAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060001D6 RID: 470 RVA: 0x0000D798 File Offset: 0x0000B998
	public static Task<bool> HasConditionAsync(int playerId, ConditionType type)
	{
		DbPlayerCondition.<HasConditionAsync>d__1 <HasConditionAsync>d__;
		<HasConditionAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<HasConditionAsync>d__.playerId = playerId;
		<HasConditionAsync>d__.type = type;
		<HasConditionAsync>d__.<>1__state = -1;
		<HasConditionAsync>d__.<>t__builder.Start<DbPlayerCondition.<HasConditionAsync>d__1>(ref <HasConditionAsync>d__);
		return <HasConditionAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060001D7 RID: 471 RVA: 0x0000D7E4 File Offset: 0x0000B9E4
	public static Task InsertAsync(int playerId, ConditionType type, ConditionCategory category, float duration, string soundEffectName, string effectName, float effectScaleModifier, float effectSpeedModifier, float interval, float power, float elapsed, int textColorId, double lastUseTime)
	{
		DbPlayerCondition.<InsertAsync>d__2 <InsertAsync>d__;
		<InsertAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<InsertAsync>d__.playerId = playerId;
		<InsertAsync>d__.type = type;
		<InsertAsync>d__.category = category;
		<InsertAsync>d__.duration = duration;
		<InsertAsync>d__.soundEffectName = soundEffectName;
		<InsertAsync>d__.effectName = effectName;
		<InsertAsync>d__.effectScaleModifier = effectScaleModifier;
		<InsertAsync>d__.effectSpeedModifier = effectSpeedModifier;
		<InsertAsync>d__.interval = interval;
		<InsertAsync>d__.power = power;
		<InsertAsync>d__.elapsed = elapsed;
		<InsertAsync>d__.textColorId = textColorId;
		<InsertAsync>d__.lastUseTime = lastUseTime;
		<InsertAsync>d__.<>1__state = -1;
		<InsertAsync>d__.<>t__builder.Start<DbPlayerCondition.<InsertAsync>d__2>(ref <InsertAsync>d__);
		return <InsertAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060001D8 RID: 472 RVA: 0x0000D890 File Offset: 0x0000BA90
	public static Task RemoveAsync(int playerId, ConditionType type)
	{
		DbPlayerCondition.<RemoveAsync>d__3 <RemoveAsync>d__;
		<RemoveAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<RemoveAsync>d__.playerId = playerId;
		<RemoveAsync>d__.type = type;
		<RemoveAsync>d__.<>1__state = -1;
		<RemoveAsync>d__.<>t__builder.Start<DbPlayerCondition.<RemoveAsync>d__3>(ref <RemoveAsync>d__);
		return <RemoveAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060001D9 RID: 473 RVA: 0x0000D8DC File Offset: 0x0000BADC
	public static Task UpdateElapsedAsync(int playerId, ConditionType type, float elapsed)
	{
		DbPlayerCondition.<UpdateElapsedAsync>d__4 <UpdateElapsedAsync>d__;
		<UpdateElapsedAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<UpdateElapsedAsync>d__.playerId = playerId;
		<UpdateElapsedAsync>d__.type = type;
		<UpdateElapsedAsync>d__.elapsed = elapsed;
		<UpdateElapsedAsync>d__.<>1__state = -1;
		<UpdateElapsedAsync>d__.<>t__builder.Start<DbPlayerCondition.<UpdateElapsedAsync>d__4>(ref <UpdateElapsedAsync>d__);
		return <UpdateElapsedAsync>d__.<>t__builder.Task;
	}
}
