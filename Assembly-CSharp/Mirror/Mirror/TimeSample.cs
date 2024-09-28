using System;
using System.Diagnostics;
using System.Threading;

namespace Mirror
{
	// Token: 0x02000085 RID: 133
	public struct TimeSample
	{
		// Token: 0x060003DF RID: 991 RVA: 0x0000E4E4 File Offset: 0x0000C6E4
		public TimeSample(int n)
		{
			this.watch = new Stopwatch();
			this.watch.Start();
			this.ema = new ExponentialMovingAverage(n);
			this.beginTime = 0.0;
			this.average = 0.0;
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0000E534 File Offset: 0x0000C734
		public void Begin()
		{
			this.beginTime = this.watch.Elapsed.TotalSeconds;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0000E55C File Offset: 0x0000C75C
		public void End()
		{
			double newValue = this.watch.Elapsed.TotalSeconds - this.beginTime;
			this.ema.Add(newValue);
			Interlocked.Exchange(ref this.average, this.ema.Value);
		}

		// Token: 0x0400017A RID: 378
		private readonly Stopwatch watch;

		// Token: 0x0400017B RID: 379
		private double beginTime;

		// Token: 0x0400017C RID: 380
		private ExponentialMovingAverage ema;

		// Token: 0x0400017D RID: 381
		public double average;
	}
}
