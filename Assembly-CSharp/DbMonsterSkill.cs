using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

// Token: 0x02000095 RID: 149
public static class DbMonsterSkill
{
	// Token: 0x06000184 RID: 388 RVA: 0x0000B054 File Offset: 0x00009254
	public static Task<DataRow[]> GetSkillsAsync(int monsterId)
	{
		DbMonsterSkill.<GetSkillsAsync>d__0 <GetSkillsAsync>d__;
		<GetSkillsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow[]>.Create();
		<GetSkillsAsync>d__.monsterId = monsterId;
		<GetSkillsAsync>d__.<>1__state = -1;
		<GetSkillsAsync>d__.<>t__builder.Start<DbMonsterSkill.<GetSkillsAsync>d__0>(ref <GetSkillsAsync>d__);
		return <GetSkillsAsync>d__.<>t__builder.Task;
	}
}
