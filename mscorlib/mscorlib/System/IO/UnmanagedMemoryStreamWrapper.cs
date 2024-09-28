using System;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO
{
	// Token: 0x02000B2A RID: 2858
	internal sealed class UnmanagedMemoryStreamWrapper : MemoryStream
	{
		// Token: 0x060066B9 RID: 26297 RVA: 0x0015EF98 File Offset: 0x0015D198
		internal UnmanagedMemoryStreamWrapper(UnmanagedMemoryStream stream)
		{
			this._unmanagedStream = stream;
		}

		// Token: 0x170011D9 RID: 4569
		// (get) Token: 0x060066BA RID: 26298 RVA: 0x0015EFA7 File Offset: 0x0015D1A7
		public override bool CanRead
		{
			get
			{
				return this._unmanagedStream.CanRead;
			}
		}

		// Token: 0x170011DA RID: 4570
		// (get) Token: 0x060066BB RID: 26299 RVA: 0x0015EFB4 File Offset: 0x0015D1B4
		public override bool CanSeek
		{
			get
			{
				return this._unmanagedStream.CanSeek;
			}
		}

		// Token: 0x170011DB RID: 4571
		// (get) Token: 0x060066BC RID: 26300 RVA: 0x0015EFC1 File Offset: 0x0015D1C1
		public override bool CanWrite
		{
			get
			{
				return this._unmanagedStream.CanWrite;
			}
		}

		// Token: 0x060066BD RID: 26301 RVA: 0x0015EFD0 File Offset: 0x0015D1D0
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					this._unmanagedStream.Dispose();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x060066BE RID: 26302 RVA: 0x0015F008 File Offset: 0x0015D208
		public override void Flush()
		{
			this._unmanagedStream.Flush();
		}

		// Token: 0x060066BF RID: 26303 RVA: 0x0015F015 File Offset: 0x0015D215
		public override byte[] GetBuffer()
		{
			throw new UnauthorizedAccessException("MemoryStream's internal buffer cannot be accessed.");
		}

		// Token: 0x060066C0 RID: 26304 RVA: 0x0015F021 File Offset: 0x0015D221
		public override bool TryGetBuffer(out ArraySegment<byte> buffer)
		{
			buffer = default(ArraySegment<byte>);
			return false;
		}

		// Token: 0x170011DC RID: 4572
		// (get) Token: 0x060066C1 RID: 26305 RVA: 0x0015F02B File Offset: 0x0015D22B
		// (set) Token: 0x060066C2 RID: 26306 RVA: 0x0015F039 File Offset: 0x0015D239
		public override int Capacity
		{
			get
			{
				return (int)this._unmanagedStream.Capacity;
			}
			set
			{
				throw new IOException("Unable to expand length of this stream beyond its capacity.");
			}
		}

		// Token: 0x170011DD RID: 4573
		// (get) Token: 0x060066C3 RID: 26307 RVA: 0x0015F045 File Offset: 0x0015D245
		public override long Length
		{
			get
			{
				return this._unmanagedStream.Length;
			}
		}

		// Token: 0x170011DE RID: 4574
		// (get) Token: 0x060066C4 RID: 26308 RVA: 0x0015F052 File Offset: 0x0015D252
		// (set) Token: 0x060066C5 RID: 26309 RVA: 0x0015F05F File Offset: 0x0015D25F
		public override long Position
		{
			get
			{
				return this._unmanagedStream.Position;
			}
			set
			{
				this._unmanagedStream.Position = value;
			}
		}

		// Token: 0x060066C6 RID: 26310 RVA: 0x0015F06D File Offset: 0x0015D26D
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this._unmanagedStream.Read(buffer, offset, count);
		}

		// Token: 0x060066C7 RID: 26311 RVA: 0x0015F07D File Offset: 0x0015D27D
		public override int Read(Span<byte> buffer)
		{
			return this._unmanagedStream.Read(buffer);
		}

		// Token: 0x060066C8 RID: 26312 RVA: 0x0015F08B File Offset: 0x0015D28B
		public override int ReadByte()
		{
			return this._unmanagedStream.ReadByte();
		}

		// Token: 0x060066C9 RID: 26313 RVA: 0x0015F098 File Offset: 0x0015D298
		public override long Seek(long offset, SeekOrigin loc)
		{
			return this._unmanagedStream.Seek(offset, loc);
		}

		// Token: 0x060066CA RID: 26314 RVA: 0x0015F0A8 File Offset: 0x0015D2A8
		public override byte[] ToArray()
		{
			byte[] array = new byte[this._unmanagedStream.Length];
			this._unmanagedStream.Read(array, 0, (int)this._unmanagedStream.Length);
			return array;
		}

		// Token: 0x060066CB RID: 26315 RVA: 0x0015F0E2 File Offset: 0x0015D2E2
		public override void Write(byte[] buffer, int offset, int count)
		{
			this._unmanagedStream.Write(buffer, offset, count);
		}

		// Token: 0x060066CC RID: 26316 RVA: 0x0015F0F2 File Offset: 0x0015D2F2
		public override void Write(ReadOnlySpan<byte> buffer)
		{
			this._unmanagedStream.Write(buffer);
		}

		// Token: 0x060066CD RID: 26317 RVA: 0x0015F100 File Offset: 0x0015D300
		public override void WriteByte(byte value)
		{
			this._unmanagedStream.WriteByte(value);
		}

		// Token: 0x060066CE RID: 26318 RVA: 0x0015F110 File Offset: 0x0015D310
		public override void WriteTo(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream", "Stream cannot be null.");
			}
			byte[] array = this.ToArray();
			stream.Write(array, 0, array.Length);
		}

		// Token: 0x060066CF RID: 26319 RVA: 0x0015F142 File Offset: 0x0015D342
		public override void SetLength(long value)
		{
			base.SetLength(value);
		}

		// Token: 0x060066D0 RID: 26320 RVA: 0x0015F14C File Offset: 0x0015D34C
		public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
		{
			if (destination == null)
			{
				throw new ArgumentNullException("destination");
			}
			if (bufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize", "Positive number required.");
			}
			if (!this.CanRead && !this.CanWrite)
			{
				throw new ObjectDisposedException(null, "Cannot access a closed Stream.");
			}
			if (!destination.CanRead && !destination.CanWrite)
			{
				throw new ObjectDisposedException("destination", "Cannot access a closed Stream.");
			}
			if (!this.CanRead)
			{
				throw new NotSupportedException("Stream does not support reading.");
			}
			if (!destination.CanWrite)
			{
				throw new NotSupportedException("Stream does not support writing.");
			}
			return this._unmanagedStream.CopyToAsync(destination, bufferSize, cancellationToken);
		}

		// Token: 0x060066D1 RID: 26321 RVA: 0x0015F1EB File Offset: 0x0015D3EB
		public override Task FlushAsync(CancellationToken cancellationToken)
		{
			return this._unmanagedStream.FlushAsync(cancellationToken);
		}

		// Token: 0x060066D2 RID: 26322 RVA: 0x0015F1F9 File Offset: 0x0015D3F9
		public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			return this._unmanagedStream.ReadAsync(buffer, offset, count, cancellationToken);
		}

		// Token: 0x060066D3 RID: 26323 RVA: 0x0015F20B File Offset: 0x0015D40B
		public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
		{
			return this._unmanagedStream.ReadAsync(buffer, cancellationToken);
		}

		// Token: 0x060066D4 RID: 26324 RVA: 0x0015F21A File Offset: 0x0015D41A
		public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			return this._unmanagedStream.WriteAsync(buffer, offset, count, cancellationToken);
		}

		// Token: 0x060066D5 RID: 26325 RVA: 0x0015F22C File Offset: 0x0015D42C
		public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
		{
			return this._unmanagedStream.WriteAsync(buffer, cancellationToken);
		}

		// Token: 0x04003C1D RID: 15389
		private UnmanagedMemoryStream _unmanagedStream;
	}
}
