using System;
using System.Text;

// Token: 0x02000152 RID: 338
public struct Title
{
	// Token: 0x0600038E RID: 910 RVA: 0x000162BA File Offset: 0x000144BA
	public Title(int id, string name, string description)
	{
		this.Id = id;
		this.Name = name;
		this.Description = description;
		this._fullDescription = string.Empty;
	}

	// Token: 0x1700008E RID: 142
	// (get) Token: 0x0600038F RID: 911 RVA: 0x000162DC File Offset: 0x000144DC
	public bool IsDefined
	{
		get
		{
			return !string.IsNullOrEmpty(this.Name) | this.Id > 0;
		}
	}

	// Token: 0x1700008F RID: 143
	// (get) Token: 0x06000390 RID: 912 RVA: 0x000162F8 File Offset: 0x000144F8
	public string FullDescription
	{
		get
		{
			if (string.IsNullOrEmpty(this._fullDescription))
			{
				StringBuilder stringBuilder = new StringBuilder();
				string text = LanguageManager.Instance.GetText(this.Name);
				stringBuilder.AppendLine(text);
				stringBuilder.AppendLine();
				stringBuilder.AppendLine(LanguageManager.Instance.GetText(this.Description));
				stringBuilder.AppendLine();
				this._fullDescription = stringBuilder.ToString();
			}
			return this._fullDescription;
		}
	}

	// Token: 0x04000703 RID: 1795
	public int Id;

	// Token: 0x04000704 RID: 1796
	public string Name;

	// Token: 0x04000705 RID: 1797
	public string Description;

	// Token: 0x04000706 RID: 1798
	private string _fullDescription;
}
