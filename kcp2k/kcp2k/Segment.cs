using System;
using System.IO;

namespace kcp2k
{
	// Token: 0x02000012 RID: 18
	internal class Segment
	{
		// Token: 0x06000068 RID: 104 RVA: 0x00004E48 File Offset: 0x00003048
		internal int Encode(byte[] ptr, int offset)
		{
			int num = offset;
			offset += Utils.Encode32U(ptr, offset, this.conv);
			offset += Utils.Encode8u(ptr, offset, (byte)this.cmd);
			offset += Utils.Encode8u(ptr, offset, (byte)this.frg);
			offset += Utils.Encode16U(ptr, offset, (ushort)this.wnd);
			offset += Utils.Encode32U(ptr, offset, this.ts);
			offset += Utils.Encode32U(ptr, offset, this.sn);
			offset += Utils.Encode32U(ptr, offset, this.una);
			offset += Utils.Encode32U(ptr, offset, (uint)this.data.Position);
			return offset - num;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00004EEC File Offset: 0x000030EC
		internal void Reset()
		{
			this.conv = 0U;
			this.cmd = 0U;
			this.frg = 0U;
			this.wnd = 0U;
			this.ts = 0U;
			this.sn = 0U;
			this.una = 0U;
			this.rto = 0;
			this.xmit = 0U;
			this.resendts = 0U;
			this.fastack = 0U;
			this.data.SetLength(0L);
		}

		// Token: 0x04000096 RID: 150
		internal uint conv;

		// Token: 0x04000097 RID: 151
		internal uint cmd;

		// Token: 0x04000098 RID: 152
		internal uint frg;

		// Token: 0x04000099 RID: 153
		internal uint wnd;

		// Token: 0x0400009A RID: 154
		internal uint ts;

		// Token: 0x0400009B RID: 155
		internal uint sn;

		// Token: 0x0400009C RID: 156
		internal uint una;

		// Token: 0x0400009D RID: 157
		internal uint resendts;

		// Token: 0x0400009E RID: 158
		internal int rto;

		// Token: 0x0400009F RID: 159
		internal uint fastack;

		// Token: 0x040000A0 RID: 160
		internal uint xmit;

		// Token: 0x040000A1 RID: 161
		internal MemoryStream data = new MemoryStream(1200);
	}
}
