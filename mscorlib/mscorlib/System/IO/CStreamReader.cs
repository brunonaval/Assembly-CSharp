﻿using System;
using System.Runtime.InteropServices;
using System.Text;

namespace System.IO
{
	// Token: 0x02000B6C RID: 2924
	internal class CStreamReader : StreamReader
	{
		// Token: 0x06006A67 RID: 27239 RVA: 0x0016C430 File Offset: 0x0016A630
		public CStreamReader(Stream stream, Encoding encoding) : base(stream, encoding)
		{
			this.driver = (TermInfoDriver)ConsoleDriver.driver;
		}

		// Token: 0x06006A68 RID: 27240 RVA: 0x0016C44C File Offset: 0x0016A64C
		public override int Peek()
		{
			try
			{
				return base.Peek();
			}
			catch (IOException)
			{
			}
			return -1;
		}

		// Token: 0x06006A69 RID: 27241 RVA: 0x0016C478 File Offset: 0x0016A678
		public override int Read()
		{
			try
			{
				return (int)Console.ReadKey().KeyChar;
			}
			catch (IOException)
			{
			}
			return -1;
		}

		// Token: 0x06006A6A RID: 27242 RVA: 0x0016C4AC File Offset: 0x0016A6AC
		public override int Read([In] [Out] char[] dest, int index, int count)
		{
			if (dest == null)
			{
				throw new ArgumentNullException("dest");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "< 0");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "< 0");
			}
			if (index > dest.Length - count)
			{
				throw new ArgumentException("index + count > dest.Length");
			}
			try
			{
				return this.driver.Read(dest, index, count);
			}
			catch (IOException)
			{
			}
			return 0;
		}

		// Token: 0x06006A6B RID: 27243 RVA: 0x0016C52C File Offset: 0x0016A72C
		public override string ReadLine()
		{
			try
			{
				return this.driver.ReadLine();
			}
			catch (IOException)
			{
			}
			return null;
		}

		// Token: 0x06006A6C RID: 27244 RVA: 0x0016C560 File Offset: 0x0016A760
		public override string ReadToEnd()
		{
			try
			{
				return this.driver.ReadToEnd();
			}
			catch (IOException)
			{
			}
			return null;
		}

		// Token: 0x04003D8B RID: 15755
		private TermInfoDriver driver;
	}
}
