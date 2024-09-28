using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

// Token: 0x020000C3 RID: 195
public static class DbPlayerSkill
{
	// Token: 0x060001FC RID: 508 RVA: 0x0000EF0C File Offset: 0x0000D10C
	public static Task<DataRow[]> GetSkillsFromSkillBookAsync(int playerId)
	{
		DbPlayerSkill.<GetSkillsFromSkillBookAsync>d__0 <GetSkillsFromSkillBookAsync>d__;
		<GetSkillsFromSkillBookAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow[]>.Create();
		<GetSkillsFromSkillBookAsync>d__.playerId = playerId;
		<GetSkillsFromSkillBookAsync>d__.<>1__state = -1;
		<GetSkillsFromSkillBookAsync>d__.<>t__builder.Start<DbPlayerSkill.<GetSkillsFromSkillBookAsync>d__0>(ref <GetSkillsFromSkillBookAsync>d__);
		return <GetSkillsFromSkillBookAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060001FD RID: 509 RVA: 0x0000EF50 File Offset: 0x0000D150
	public static Task<DataRow[]> GetSkillsFromSkillBarAsync(int playerId)
	{
		DbPlayerSkill.<GetSkillsFromSkillBarAsync>d__1 <GetSkillsFromSkillBarAsync>d__;
		<GetSkillsFromSkillBarAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow[]>.Create();
		<GetSkillsFromSkillBarAsync>d__.playerId = playerId;
		<GetSkillsFromSkillBarAsync>d__.<>1__state = -1;
		<GetSkillsFromSkillBarAsync>d__.<>t__builder.Start<DbPlayerSkill.<GetSkillsFromSkillBarAsync>d__1>(ref <GetSkillsFromSkillBarAsync>d__);
		return <GetSkillsFromSkillBarAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060001FE RID: 510 RVA: 0x0000EF94 File Offset: 0x0000D194
	public static Task InsertOnSkillBarAsync(int playerId, int skillId, int slotPosition, int skillBarId)
	{
		DbPlayerSkill.<InsertOnSkillBarAsync>d__2 <InsertOnSkillBarAsync>d__;
		<InsertOnSkillBarAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<InsertOnSkillBarAsync>d__.playerId = playerId;
		<InsertOnSkillBarAsync>d__.skillId = skillId;
		<InsertOnSkillBarAsync>d__.slotPosition = slotPosition;
		<InsertOnSkillBarAsync>d__.skillBarId = skillBarId;
		<InsertOnSkillBarAsync>d__.<>1__state = -1;
		<InsertOnSkillBarAsync>d__.<>t__builder.Start<DbPlayerSkill.<InsertOnSkillBarAsync>d__2>(ref <InsertOnSkillBarAsync>d__);
		return <InsertOnSkillBarAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060001FF RID: 511 RVA: 0x0000EFF0 File Offset: 0x0000D1F0
	public static Task InsertOnSkillBookAsync(int playerId, int skillId, int skillEnchantLevel)
	{
		DbPlayerSkill.<InsertOnSkillBookAsync>d__3 <InsertOnSkillBookAsync>d__;
		<InsertOnSkillBookAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<InsertOnSkillBookAsync>d__.playerId = playerId;
		<InsertOnSkillBookAsync>d__.skillId = skillId;
		<InsertOnSkillBookAsync>d__.skillEnchantLevel = skillEnchantLevel;
		<InsertOnSkillBookAsync>d__.<>1__state = -1;
		<InsertOnSkillBookAsync>d__.<>t__builder.Start<DbPlayerSkill.<InsertOnSkillBookAsync>d__3>(ref <InsertOnSkillBookAsync>d__);
		return <InsertOnSkillBookAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000200 RID: 512 RVA: 0x0000F044 File Offset: 0x0000D244
	public static Task RemoveFromSkillBarAsync(int playerId, int slotPosition, int skillBarId)
	{
		DbPlayerSkill.<RemoveFromSkillBarAsync>d__4 <RemoveFromSkillBarAsync>d__;
		<RemoveFromSkillBarAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<RemoveFromSkillBarAsync>d__.playerId = playerId;
		<RemoveFromSkillBarAsync>d__.slotPosition = slotPosition;
		<RemoveFromSkillBarAsync>d__.skillBarId = skillBarId;
		<RemoveFromSkillBarAsync>d__.<>1__state = -1;
		<RemoveFromSkillBarAsync>d__.<>t__builder.Start<DbPlayerSkill.<RemoveFromSkillBarAsync>d__4>(ref <RemoveFromSkillBarAsync>d__);
		return <RemoveFromSkillBarAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000201 RID: 513 RVA: 0x0000F098 File Offset: 0x0000D298
	public static Task ChangeSkillOnSkillBarSlotAsync(int playerId, int slotPosition, int skillBarId, int skillId)
	{
		DbPlayerSkill.<ChangeSkillOnSkillBarSlotAsync>d__5 <ChangeSkillOnSkillBarSlotAsync>d__;
		<ChangeSkillOnSkillBarSlotAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<ChangeSkillOnSkillBarSlotAsync>d__.playerId = playerId;
		<ChangeSkillOnSkillBarSlotAsync>d__.slotPosition = slotPosition;
		<ChangeSkillOnSkillBarSlotAsync>d__.skillBarId = skillBarId;
		<ChangeSkillOnSkillBarSlotAsync>d__.skillId = skillId;
		<ChangeSkillOnSkillBarSlotAsync>d__.<>1__state = -1;
		<ChangeSkillOnSkillBarSlotAsync>d__.<>t__builder.Start<DbPlayerSkill.<ChangeSkillOnSkillBarSlotAsync>d__5>(ref <ChangeSkillOnSkillBarSlotAsync>d__);
		return <ChangeSkillOnSkillBarSlotAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000202 RID: 514 RVA: 0x0000F0F4 File Offset: 0x0000D2F4
	public static Task UpdateSkillOnSkillBookAsync(int playerId, int skillId, double lastUseTime, int enchantLevel)
	{
		DbPlayerSkill.<UpdateSkillOnSkillBookAsync>d__6 <UpdateSkillOnSkillBookAsync>d__;
		<UpdateSkillOnSkillBookAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<UpdateSkillOnSkillBookAsync>d__.playerId = playerId;
		<UpdateSkillOnSkillBookAsync>d__.skillId = skillId;
		<UpdateSkillOnSkillBookAsync>d__.lastUseTime = lastUseTime;
		<UpdateSkillOnSkillBookAsync>d__.enchantLevel = enchantLevel;
		<UpdateSkillOnSkillBookAsync>d__.<>1__state = -1;
		<UpdateSkillOnSkillBookAsync>d__.<>t__builder.Start<DbPlayerSkill.<UpdateSkillOnSkillBookAsync>d__6>(ref <UpdateSkillOnSkillBookAsync>d__);
		return <UpdateSkillOnSkillBookAsync>d__.<>t__builder.Task;
	}
}
