using System;

namespace Mirror
{
	// Token: 0x02000081 RID: 129
	public struct ExponentialMovingAverage
	{
		// Token: 0x060003CF RID: 975 RVA: 0x0000E280 File Offset: 0x0000C480
		public ExponentialMovingAverage(int n)
		{
			this.alpha = 2.0 / (double)(n + 1);
			this.initialized = false;
			this.Value = 0.0;
			this.Variance = 0.0;
			this.StandardDeviation = 0.0;
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0000E2D8 File Offset: 0x0000C4D8
		public void Add(double newValue)
		{
			if (this.initialized)
			{
				double num = newValue - this.Value;
				this.Value += this.alpha * num;
				this.Variance = (1.0 - this.alpha) * (this.Variance + this.alpha * num * num);
				this.StandardDeviation = Math.Sqrt(this.Variance);
				return;
			}
			this.Value = newValue;
			this.initialized = true;
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0000E353 File Offset: 0x0000C553
		public void Reset()
		{
			this.initialized = false;
			this.Value = 0.0;
			this.Variance = 0.0;
			this.StandardDeviation = 0.0;
		}

		// Token: 0x04000173 RID: 371
		private readonly double alpha;

		// Token: 0x04000174 RID: 372
		private bool initialized;

		// Token: 0x04000175 RID: 373
		public double Value;

		// Token: 0x04000176 RID: 374
		public double Variance;

		// Token: 0x04000177 RID: 375
		public double StandardDeviation;
	}
}
