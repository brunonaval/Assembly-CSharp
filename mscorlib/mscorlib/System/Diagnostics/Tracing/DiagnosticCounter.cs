using System;

namespace System.Diagnostics.Tracing
{
	// Token: 0x020009FE RID: 2558
	public abstract class DiagnosticCounter : IDisposable
	{
		// Token: 0x06005B3C RID: 23356 RVA: 0x0000259F File Offset: 0x0000079F
		internal DiagnosticCounter(string name, EventSource eventSource)
		{
		}

		// Token: 0x06005B3D RID: 23357 RVA: 0x0000259F File Offset: 0x0000079F
		internal DiagnosticCounter()
		{
		}

		// Token: 0x17000FAB RID: 4011
		// (get) Token: 0x06005B3E RID: 23358 RVA: 0x001348B1 File Offset: 0x00132AB1
		// (set) Token: 0x06005B3F RID: 23359 RVA: 0x001348B9 File Offset: 0x00132AB9
		public string DisplayName { get; set; }

		// Token: 0x17000FAC RID: 4012
		// (get) Token: 0x06005B40 RID: 23360 RVA: 0x001348C2 File Offset: 0x00132AC2
		// (set) Token: 0x06005B41 RID: 23361 RVA: 0x001348CA File Offset: 0x00132ACA
		public string DisplayUnits { get; set; }

		// Token: 0x17000FAD RID: 4013
		// (get) Token: 0x06005B42 RID: 23362 RVA: 0x001348D3 File Offset: 0x00132AD3
		public EventSource EventSource { get; }

		// Token: 0x17000FAE RID: 4014
		// (get) Token: 0x06005B43 RID: 23363 RVA: 0x001348DB File Offset: 0x00132ADB
		public string Name { get; }

		// Token: 0x06005B44 RID: 23364 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public void AddMetadata(string key, string value)
		{
		}

		// Token: 0x06005B45 RID: 23365 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public void Dispose()
		{
		}
	}
}
