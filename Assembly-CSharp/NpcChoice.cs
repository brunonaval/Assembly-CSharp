using System;

// Token: 0x02000123 RID: 291
public struct NpcChoice
{
	// Token: 0x0600031C RID: 796 RVA: 0x000144DA File Offset: 0x000126DA
	public NpcChoice(string display, int actionId)
	{
		this.Display = display;
		this.ActionId = actionId;
		this.Param1 = string.Empty;
		this.Param2 = string.Empty;
		this.Param3 = string.Empty;
	}

	// Token: 0x0600031D RID: 797 RVA: 0x0001450B File Offset: 0x0001270B
	public NpcChoice(string display, string param1, int actionId)
	{
		this.Display = display;
		this.ActionId = actionId;
		this.Param1 = param1;
		this.Param2 = string.Empty;
		this.Param3 = string.Empty;
	}

	// Token: 0x0600031E RID: 798 RVA: 0x00014538 File Offset: 0x00012738
	public NpcChoice(string display, string param1, string param2, int actionId)
	{
		this.Display = display;
		this.ActionId = actionId;
		this.Param1 = param1;
		this.Param2 = param2;
		this.Param3 = string.Empty;
	}

	// Token: 0x0600031F RID: 799 RVA: 0x00014562 File Offset: 0x00012762
	public NpcChoice(string display, string param1, string param2, string param3, int actionId)
	{
		this.Display = display;
		this.ActionId = actionId;
		this.Param1 = param1;
		this.Param2 = param2;
		this.Param3 = param3;
	}

	// Token: 0x040005DF RID: 1503
	public int ActionId;

	// Token: 0x040005E0 RID: 1504
	public string Display;

	// Token: 0x040005E1 RID: 1505
	public string Param1;

	// Token: 0x040005E2 RID: 1506
	public string Param2;

	// Token: 0x040005E3 RID: 1507
	public string Param3;
}
