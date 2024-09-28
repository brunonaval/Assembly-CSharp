﻿using System;
using System.Runtime.InteropServices;

namespace System.IO
{
	// Token: 0x02000B0F RID: 2831
	internal sealed class PinnedBufferMemoryStream : UnmanagedMemoryStream
	{
		// Token: 0x06006531 RID: 25905 RVA: 0x001584E8 File Offset: 0x001566E8
		internal unsafe PinnedBufferMemoryStream(byte[] array)
		{
			this._array = array;
			this._pinningHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			int num = array.Length;
			fixed (byte* reference = MemoryMarshal.GetReference<byte>(array))
			{
				byte* pointer = reference;
				base.Initialize(pointer, (long)num, (long)num, FileAccess.Read);
			}
		}

		// Token: 0x06006532 RID: 25906 RVA: 0x00158531 File Offset: 0x00156731
		public override int Read(Span<byte> buffer)
		{
			return base.ReadCore(buffer);
		}

		// Token: 0x06006533 RID: 25907 RVA: 0x0015853A File Offset: 0x0015673A
		public override void Write(ReadOnlySpan<byte> buffer)
		{
			base.WriteCore(buffer);
		}

		// Token: 0x06006534 RID: 25908 RVA: 0x00158544 File Offset: 0x00156744
		~PinnedBufferMemoryStream()
		{
			this.Dispose(false);
		}

		// Token: 0x06006535 RID: 25909 RVA: 0x00158574 File Offset: 0x00156774
		protected override void Dispose(bool disposing)
		{
			if (this._pinningHandle.IsAllocated)
			{
				this._pinningHandle.Free();
			}
			base.Dispose(disposing);
		}

		// Token: 0x04003B67 RID: 15207
		private byte[] _array;

		// Token: 0x04003B68 RID: 15208
		private GCHandle _pinningHandle;
	}
}
