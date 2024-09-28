using System;
using System.Diagnostics;

namespace System.Collections.Generic
{
	// Token: 0x02000AA8 RID: 2728
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	internal readonly struct CopyPosition
	{
		// Token: 0x060061A9 RID: 25001 RVA: 0x001467C0 File Offset: 0x001449C0
		internal CopyPosition(int row, int column)
		{
			this.Row = row;
			this.Column = column;
		}

		// Token: 0x1700115E RID: 4446
		// (get) Token: 0x060061AA RID: 25002 RVA: 0x001467D0 File Offset: 0x001449D0
		public static CopyPosition Start
		{
			get
			{
				return default(CopyPosition);
			}
		}

		// Token: 0x1700115F RID: 4447
		// (get) Token: 0x060061AB RID: 25003 RVA: 0x001467E6 File Offset: 0x001449E6
		internal int Row { get; }

		// Token: 0x17001160 RID: 4448
		// (get) Token: 0x060061AC RID: 25004 RVA: 0x001467EE File Offset: 0x001449EE
		internal int Column { get; }

		// Token: 0x060061AD RID: 25005 RVA: 0x001467F6 File Offset: 0x001449F6
		public CopyPosition Normalize(int endColumn)
		{
			if (this.Column != endColumn)
			{
				return this;
			}
			return new CopyPosition(this.Row + 1, 0);
		}

		// Token: 0x17001161 RID: 4449
		// (get) Token: 0x060061AE RID: 25006 RVA: 0x00146816 File Offset: 0x00144A16
		private string DebuggerDisplay
		{
			get
			{
				return string.Format("[{0}, {1}]", this.Row, this.Column);
			}
		}
	}
}
