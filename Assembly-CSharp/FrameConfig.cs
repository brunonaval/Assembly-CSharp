using System;

// Token: 0x0200010E RID: 270
[Serializable]
public struct FrameConfig
{
	// Token: 0x060002B3 RID: 691 RVA: 0x000126CC File Offset: 0x000108CC
	public FrameConfig(int index, int frames, int[] gaps)
	{
		this.Index = index;
		this.Frames = frames;
		this.Gaps = gaps;
	}

	// Token: 0x0400051A RID: 1306
	public int Index;

	// Token: 0x0400051B RID: 1307
	public int Frames;

	// Token: 0x0400051C RID: 1308
	public int[] Gaps;
}
