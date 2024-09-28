using System;
using System.Linq;

// Token: 0x02000124 RID: 292
public struct NpcDialog
{
	// Token: 0x06000320 RID: 800 RVA: 0x00014589 File Offset: 0x00012789
	public NpcDialog(string name, string display, params NpcChoice[] choices)
	{
		this.Name = name;
		this.Display = display;
		this.Choices = choices;
		this.StoreItems = new StoreItemConfig[0];
		this.StoreAction = StoreAction.None;
		this.Quest = default(Quest);
	}

	// Token: 0x06000321 RID: 801 RVA: 0x000145BF File Offset: 0x000127BF
	public NpcDialog(string name, Quest quest, params NpcChoice[] choices)
	{
		this.Name = name;
		this.Display = null;
		this.Choices = choices;
		this.Quest = quest;
		this.StoreItems = new StoreItemConfig[0];
		this.StoreAction = StoreAction.None;
	}

	// Token: 0x06000322 RID: 802 RVA: 0x000145F0 File Offset: 0x000127F0
	public NpcDialog(string name, string display, StoreItemConfig[] storeItems, params NpcChoice[] choices)
	{
		this.Name = name;
		this.Display = display;
		this.Choices = choices;
		this.StoreItems = storeItems;
		this.StoreAction = StoreAction.None;
		this.Quest = default(Quest);
	}

	// Token: 0x17000062 RID: 98
	// (get) Token: 0x06000323 RID: 803 RVA: 0x00014622 File Offset: 0x00012822
	public bool IsDefined
	{
		get
		{
			return !string.IsNullOrEmpty(this.Name) & !string.IsNullOrEmpty(this.Display) & (this.Choices != null && this.Choices.Length != 0);
		}
	}

	// Token: 0x06000324 RID: 804 RVA: 0x00014657 File Offset: 0x00012857
	public void AddChoice(NpcChoice choice)
	{
		this.Choices = this.Choices.Union(new NpcChoice[]
		{
			choice
		}).ToArray<NpcChoice>();
	}

	// Token: 0x040005E4 RID: 1508
	public string Name;

	// Token: 0x040005E5 RID: 1509
	public string Display;

	// Token: 0x040005E6 RID: 1510
	public NpcChoice[] Choices;

	// Token: 0x040005E7 RID: 1511
	public StoreItemConfig[] StoreItems;

	// Token: 0x040005E8 RID: 1512
	public Quest Quest;

	// Token: 0x040005E9 RID: 1513
	public StoreAction StoreAction;
}
