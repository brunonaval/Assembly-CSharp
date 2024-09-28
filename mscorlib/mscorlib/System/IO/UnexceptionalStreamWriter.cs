﻿using System;
using System.Text;

namespace System.IO
{
	// Token: 0x02000B6B RID: 2923
	internal class UnexceptionalStreamWriter : StreamWriter
	{
		// Token: 0x06006A61 RID: 27233 RVA: 0x0016C348 File Offset: 0x0016A548
		public UnexceptionalStreamWriter(Stream stream, Encoding encoding) : base(stream, encoding, 1024, true)
		{
		}

		// Token: 0x06006A62 RID: 27234 RVA: 0x0016C358 File Offset: 0x0016A558
		public override void Flush()
		{
			try
			{
				base.Flush();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06006A63 RID: 27235 RVA: 0x0016C380 File Offset: 0x0016A580
		public override void Write(char[] buffer, int index, int count)
		{
			try
			{
				base.Write(buffer, index, count);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06006A64 RID: 27236 RVA: 0x0016C3AC File Offset: 0x0016A5AC
		public override void Write(char value)
		{
			try
			{
				base.Write(value);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06006A65 RID: 27237 RVA: 0x0016C3D8 File Offset: 0x0016A5D8
		public override void Write(char[] value)
		{
			try
			{
				base.Write(value);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06006A66 RID: 27238 RVA: 0x0016C404 File Offset: 0x0016A604
		public override void Write(string value)
		{
			try
			{
				base.Write(value);
			}
			catch (Exception)
			{
			}
		}
	}
}
