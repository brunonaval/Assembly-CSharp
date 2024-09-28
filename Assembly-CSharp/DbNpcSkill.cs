using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

// Token: 0x02000099 RID: 153
public static class DbNpcSkill
{
	// Token: 0x0600018A RID: 394 RVA: 0x0000B27C File Offset: 0x0000947C
	public static Task<DataRow[]> GetSkillsAsync(int npcId)
	{
		DbNpcSkill.<GetSkillsAsync>d__0 <GetSkillsAsync>d__;
		<GetSkillsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow[]>.Create();
		<GetSkillsAsync>d__.npcId = npcId;
		<GetSkillsAsync>d__.<>1__state = -1;
		<GetSkillsAsync>d__.<>t__builder.Start<DbNpcSkill.<GetSkillsAsync>d__0>(ref <GetSkillsAsync>d__);
		return <GetSkillsAsync>d__.<>t__builder.Task;
	}
}
