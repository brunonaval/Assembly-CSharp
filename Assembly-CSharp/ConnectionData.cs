using System;

// Token: 0x02000105 RID: 261
public class ConnectionData
{
	// Token: 0x17000033 RID: 51
	// (get) Token: 0x060002A3 RID: 675 RVA: 0x000125D4 File Offset: 0x000107D4
	// (set) Token: 0x060002A4 RID: 676 RVA: 0x000125DC File Offset: 0x000107DC
	public int ConnectionId { get; set; }

	// Token: 0x17000034 RID: 52
	// (get) Token: 0x060002A5 RID: 677 RVA: 0x000125E5 File Offset: 0x000107E5
	// (set) Token: 0x060002A6 RID: 678 RVA: 0x000125ED File Offset: 0x000107ED
	public float GameTime { get; set; }

	// Token: 0x17000035 RID: 53
	// (get) Token: 0x060002A7 RID: 679 RVA: 0x000125F6 File Offset: 0x000107F6
	// (set) Token: 0x060002A8 RID: 680 RVA: 0x000125FE File Offset: 0x000107FE
	public long DataSize { get; set; }
}
