using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

// Token: 0x020000E0 RID: 224
public static class DbSkill
{
	// Token: 0x06000244 RID: 580 RVA: 0x00010DC4 File Offset: 0x0000EFC4
	public static Task<DataRow[]> GetAllSkillsAsync()
	{
		DbSkill.<GetAllSkillsAsync>d__0 <GetAllSkillsAsync>d__;
		<GetAllSkillsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow[]>.Create();
		<GetAllSkillsAsync>d__.<>1__state = -1;
		<GetAllSkillsAsync>d__.<>t__builder.Start<DbSkill.<GetAllSkillsAsync>d__0>(ref <GetAllSkillsAsync>d__);
		return <GetAllSkillsAsync>d__.<>t__builder.Task;
	}
}
