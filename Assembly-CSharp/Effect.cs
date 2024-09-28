using System;

// Token: 0x0200010A RID: 266
public struct Effect
{
	// Token: 0x060002AC RID: 684 RVA: 0x00012626 File Offset: 0x00010826
	public Effect(string name)
	{
		this.Name = name;
		this.ScaleModifier = 1f;
		this.SpeedModifier = 1f;
	}

	// Token: 0x060002AD RID: 685 RVA: 0x00012645 File Offset: 0x00010845
	public Effect(string name, float scaleModifier)
	{
		this.Name = name;
		this.ScaleModifier = scaleModifier;
		this.SpeedModifier = 1f;
	}

	// Token: 0x060002AE RID: 686 RVA: 0x00012660 File Offset: 0x00010860
	public Effect(string name, float scaleModifier, float speedModifier)
	{
		this.Name = name;
		this.ScaleModifier = scaleModifier;
		this.SpeedModifier = speedModifier;
	}

	// Token: 0x17000036 RID: 54
	// (get) Token: 0x060002AF RID: 687 RVA: 0x00012677 File Offset: 0x00010877
	public bool IsDefined
	{
		get
		{
			return !string.IsNullOrEmpty(this.Name);
		}
	}

	// Token: 0x0400050A RID: 1290
	public string Name;

	// Token: 0x0400050B RID: 1291
	public float ScaleModifier;

	// Token: 0x0400050C RID: 1292
	public float SpeedModifier;
}
