using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO
{
	/// <summary>Implements a <see cref="T:System.IO.TextWriter" /> for writing characters to a stream in a particular encoding.</summary>
	// Token: 0x02000B18 RID: 2840
	[Serializable]
	public class StreamWriter : TextWriter
	{
		// Token: 0x0600657A RID: 25978 RVA: 0x0015A546 File Offset: 0x00158746
		private void CheckAsyncTaskInProgress()
		{
			if (!this._asyncWriteTask.IsCompleted)
			{
				StreamWriter.ThrowAsyncIOInProgress();
			}
		}

		// Token: 0x0600657B RID: 25979 RVA: 0x0015863B File Offset: 0x0015683B
		private static void ThrowAsyncIOInProgress()
		{
			throw new InvalidOperationException("The stream is currently in use by a previous operation on the stream.");
		}

		// Token: 0x170011BF RID: 4543
		// (get) Token: 0x0600657C RID: 25980 RVA: 0x0015A55A File Offset: 0x0015875A
		private static Encoding UTF8NoBOM
		{
			get
			{
				return EncodingHelper.UTF8Unmarked;
			}
		}

		// Token: 0x0600657D RID: 25981 RVA: 0x0015A561 File Offset: 0x00158761
		internal StreamWriter() : base(null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamWriter" /> class for the specified stream by using UTF-8 encoding and the default buffer size.</summary>
		/// <param name="stream">The stream to write to.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="stream" /> is not writable.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="stream" /> is <see langword="null" />.</exception>
		// Token: 0x0600657E RID: 25982 RVA: 0x0015A575 File Offset: 0x00158775
		public StreamWriter(Stream stream) : this(stream, StreamWriter.UTF8NoBOM, 1024, false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamWriter" /> class for the specified stream by using the specified encoding and the default buffer size.</summary>
		/// <param name="stream">The stream to write to.</param>
		/// <param name="encoding">The character encoding to use.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="stream" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="stream" /> is not writable.</exception>
		// Token: 0x0600657F RID: 25983 RVA: 0x0015A589 File Offset: 0x00158789
		public StreamWriter(Stream stream, Encoding encoding) : this(stream, encoding, 1024, false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamWriter" /> class for the specified stream by using the specified encoding and buffer size.</summary>
		/// <param name="stream">The stream to write to.</param>
		/// <param name="encoding">The character encoding to use.</param>
		/// <param name="bufferSize">The buffer size, in bytes.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="stream" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="bufferSize" /> is negative.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="stream" /> is not writable.</exception>
		// Token: 0x06006580 RID: 25984 RVA: 0x0015A599 File Offset: 0x00158799
		public StreamWriter(Stream stream, Encoding encoding, int bufferSize) : this(stream, encoding, bufferSize, false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamWriter" /> class for the specified stream by using the specified encoding and buffer size, and optionally leaves the stream open.</summary>
		/// <param name="stream">The stream to write to.</param>
		/// <param name="encoding">The character encoding to use.</param>
		/// <param name="bufferSize">The buffer size, in bytes.</param>
		/// <param name="leaveOpen">
		///   <see langword="true" /> to leave the stream open after the <see cref="T:System.IO.StreamWriter" /> object is disposed; otherwise, <see langword="false" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="stream" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="bufferSize" /> is negative.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="stream" /> is not writable.</exception>
		// Token: 0x06006581 RID: 25985 RVA: 0x0015A5A8 File Offset: 0x001587A8
		public StreamWriter(Stream stream, Encoding encoding, int bufferSize, bool leaveOpen) : base(null)
		{
			if (stream == null || encoding == null)
			{
				throw new ArgumentNullException((stream == null) ? "stream" : "encoding");
			}
			if (!stream.CanWrite)
			{
				throw new ArgumentException("Stream was not writable.");
			}
			if (bufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize", "Positive number required.");
			}
			this.Init(stream, encoding, bufferSize, leaveOpen);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamWriter" /> class for the specified file by using the default encoding and buffer size.</summary>
		/// <param name="path">The complete file path to write to. <paramref name="path" /> can be a file name.</param>
		/// <exception cref="T:System.UnauthorizedAccessException">Access is denied.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is an empty string ("").  
		/// -or-  
		/// <paramref name="path" /> contains the name of a system device (com1, com2, and so on).</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.IO.IOException">
		///   <paramref name="path" /> includes an incorrect or invalid syntax for file name, directory name, or volume label syntax.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		// Token: 0x06006582 RID: 25986 RVA: 0x0015A614 File Offset: 0x00158814
		public StreamWriter(string path) : this(path, false, StreamWriter.UTF8NoBOM, 1024)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamWriter" /> class for the specified file by using the default encoding and buffer size. If the file exists, it can be either overwritten or appended to. If the file does not exist, this constructor creates a new file.</summary>
		/// <param name="path">The complete file path to write to.</param>
		/// <param name="append">
		///   <see langword="true" /> to append data to the file; <see langword="false" /> to overwrite the file. If the specified file does not exist, this parameter has no effect, and the constructor creates a new file.</param>
		/// <exception cref="T:System.UnauthorizedAccessException">Access is denied.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is empty.  
		/// -or-  
		/// <paramref name="path" /> contains the name of a system device (com1, com2, and so on).</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="T:System.IO.IOException">
		///   <paramref name="path" /> includes an incorrect or invalid syntax for file name, directory name, or volume label syntax.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		// Token: 0x06006583 RID: 25987 RVA: 0x0015A628 File Offset: 0x00158828
		public StreamWriter(string path, bool append) : this(path, append, StreamWriter.UTF8NoBOM, 1024)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamWriter" /> class for the specified file by using the specified encoding and default buffer size. If the file exists, it can be either overwritten or appended to. If the file does not exist, this constructor creates a new file.</summary>
		/// <param name="path">The complete file path to write to.</param>
		/// <param name="append">
		///   <see langword="true" /> to append data to the file; <see langword="false" /> to overwrite the file. If the specified file does not exist, this parameter has no effect, and the constructor creates a new file.</param>
		/// <param name="encoding">The character encoding to use.</param>
		/// <exception cref="T:System.UnauthorizedAccessException">Access is denied.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is empty.  
		/// -or-  
		/// <paramref name="path" /> contains the name of a system device (com1, com2, and so on).</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="T:System.IO.IOException">
		///   <paramref name="path" /> includes an incorrect or invalid syntax for file name, directory name, or volume label syntax.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		// Token: 0x06006584 RID: 25988 RVA: 0x0015A63C File Offset: 0x0015883C
		public StreamWriter(string path, bool append, Encoding encoding) : this(path, append, encoding, 1024)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.StreamWriter" /> class for the specified file on the specified path, using the specified encoding and buffer size. If the file exists, it can be either overwritten or appended to. If the file does not exist, this constructor creates a new file.</summary>
		/// <param name="path">The complete file path to write to.</param>
		/// <param name="append">
		///   <see langword="true" /> to append data to the file; <see langword="false" /> to overwrite the file. If the specified file does not exist, this parameter has no effect, and the constructor creates a new file.</param>
		/// <param name="encoding">The character encoding to use.</param>
		/// <param name="bufferSize">The buffer size, in bytes.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="path" /> is an empty string ("").  
		/// -or-  
		/// <paramref name="path" /> contains the name of a system device (com1, com2, and so on).</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="path" /> or <paramref name="encoding" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="bufferSize" /> is negative.</exception>
		/// <exception cref="T:System.IO.IOException">
		///   <paramref name="path" /> includes an incorrect or invalid syntax for file name, directory name, or volume label syntax.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">Access is denied.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		// Token: 0x06006585 RID: 25989 RVA: 0x0015A64C File Offset: 0x0015884C
		public StreamWriter(string path, bool append, Encoding encoding, int bufferSize)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (encoding == null)
			{
				throw new ArgumentNullException("encoding");
			}
			if (path.Length == 0)
			{
				throw new ArgumentException("Empty path name is not legal.");
			}
			if (bufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize", "Positive number required.");
			}
			Stream streamArg = new FileStream(path, append ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.Read, 4096, FileOptions.SequentialScan);
			this.Init(streamArg, encoding, bufferSize, false);
		}

		// Token: 0x06006586 RID: 25990 RVA: 0x0015A6D4 File Offset: 0x001588D4
		private void Init(Stream streamArg, Encoding encodingArg, int bufferSize, bool shouldLeaveOpen)
		{
			this._stream = streamArg;
			this._encoding = encodingArg;
			this._encoder = this._encoding.GetEncoder();
			if (bufferSize < 128)
			{
				bufferSize = 128;
			}
			this._charBuffer = new char[bufferSize];
			this._byteBuffer = new byte[this._encoding.GetMaxByteCount(bufferSize)];
			this._charLen = bufferSize;
			if (this._stream.CanSeek && this._stream.Position > 0L)
			{
				this._haveWrittenPreamble = true;
			}
			this._closable = !shouldLeaveOpen;
		}

		/// <summary>Closes the current <see langword="StreamWriter" /> object and the underlying stream.</summary>
		/// <exception cref="T:System.Text.EncoderFallbackException">The current encoding does not support displaying half of a Unicode surrogate pair.</exception>
		// Token: 0x06006587 RID: 25991 RVA: 0x0015A767 File Offset: 0x00158967
		public override void Close()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.IO.StreamWriter" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">
		///   <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
		/// <exception cref="T:System.Text.EncoderFallbackException">The current encoding does not support displaying half of a Unicode surrogate pair.</exception>
		// Token: 0x06006588 RID: 25992 RVA: 0x0015A778 File Offset: 0x00158978
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (this._stream != null && disposing)
				{
					this.CheckAsyncTaskInProgress();
					this.Flush(true, true);
				}
			}
			finally
			{
				if (!this.LeaveOpen && this._stream != null)
				{
					try
					{
						if (disposing)
						{
							this._stream.Close();
						}
					}
					finally
					{
						this._stream = null;
						this._byteBuffer = null;
						this._charBuffer = null;
						this._encoding = null;
						this._encoder = null;
						this._charLen = 0;
						base.Dispose(disposing);
					}
				}
			}
		}

		// Token: 0x06006589 RID: 25993 RVA: 0x0015A810 File Offset: 0x00158A10
		public override ValueTask DisposeAsync()
		{
			if (!(base.GetType() != typeof(StreamWriter)))
			{
				return this.DisposeAsyncCore();
			}
			return base.DisposeAsync();
		}

		// Token: 0x0600658A RID: 25994 RVA: 0x0015A838 File Offset: 0x00158A38
		private ValueTask DisposeAsyncCore()
		{
			StreamWriter.<DisposeAsyncCore>d__33 <DisposeAsyncCore>d__;
			<DisposeAsyncCore>d__.<>4__this = this;
			<DisposeAsyncCore>d__.<>t__builder = AsyncValueTaskMethodBuilder.Create();
			<DisposeAsyncCore>d__.<>1__state = -1;
			<DisposeAsyncCore>d__.<>t__builder.Start<StreamWriter.<DisposeAsyncCore>d__33>(ref <DisposeAsyncCore>d__);
			return <DisposeAsyncCore>d__.<>t__builder.Task;
		}

		// Token: 0x0600658B RID: 25995 RVA: 0x0015A87C File Offset: 0x00158A7C
		private void CloseStreamFromDispose(bool disposing)
		{
			if (!this.LeaveOpen && this._stream != null)
			{
				try
				{
					if (disposing)
					{
						this._stream.Close();
					}
				}
				finally
				{
					this._stream = null;
					this._byteBuffer = null;
					this._charBuffer = null;
					this._encoding = null;
					this._encoder = null;
					this._charLen = 0;
					base.Dispose(disposing);
				}
			}
		}

		/// <summary>Clears all buffers for the current writer and causes any buffered data to be written to the underlying stream.</summary>
		/// <exception cref="T:System.ObjectDisposedException">The current writer is closed.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error has occurred.</exception>
		/// <exception cref="T:System.Text.EncoderFallbackException">The current encoding does not support displaying half of a Unicode surrogate pair.</exception>
		// Token: 0x0600658C RID: 25996 RVA: 0x0015A8EC File Offset: 0x00158AEC
		public override void Flush()
		{
			this.CheckAsyncTaskInProgress();
			this.Flush(true, true);
		}

		// Token: 0x0600658D RID: 25997 RVA: 0x0015A8FC File Offset: 0x00158AFC
		private void Flush(bool flushStream, bool flushEncoder)
		{
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Can not write to a closed TextWriter.");
			}
			if (this._charPos == 0 && !flushStream && !flushEncoder)
			{
				return;
			}
			if (!this._haveWrittenPreamble)
			{
				this._haveWrittenPreamble = true;
				ReadOnlySpan<byte> preamble = this._encoding.Preamble;
				if (preamble.Length > 0)
				{
					this._stream.Write(preamble);
				}
			}
			int bytes = this._encoder.GetBytes(this._charBuffer, 0, this._charPos, this._byteBuffer, 0, flushEncoder);
			this._charPos = 0;
			if (bytes > 0)
			{
				this._stream.Write(this._byteBuffer, 0, bytes);
			}
			if (flushStream)
			{
				this._stream.Flush();
			}
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.IO.StreamWriter" /> will flush its buffer to the underlying stream after every call to <see cref="M:System.IO.StreamWriter.Write(System.Char)" />.</summary>
		/// <returns>
		///   <see langword="true" /> to force <see cref="T:System.IO.StreamWriter" /> to flush its buffer; otherwise, <see langword="false" />.</returns>
		// Token: 0x170011C0 RID: 4544
		// (get) Token: 0x0600658E RID: 25998 RVA: 0x0015A9AA File Offset: 0x00158BAA
		// (set) Token: 0x0600658F RID: 25999 RVA: 0x0015A9B2 File Offset: 0x00158BB2
		public virtual bool AutoFlush
		{
			get
			{
				return this._autoFlush;
			}
			set
			{
				this.CheckAsyncTaskInProgress();
				this._autoFlush = value;
				if (value)
				{
					this.Flush(true, false);
				}
			}
		}

		/// <summary>Gets the underlying stream that interfaces with a backing store.</summary>
		/// <returns>The stream this <see langword="StreamWriter" /> is writing to.</returns>
		// Token: 0x170011C1 RID: 4545
		// (get) Token: 0x06006590 RID: 26000 RVA: 0x0015A9CC File Offset: 0x00158BCC
		public virtual Stream BaseStream
		{
			get
			{
				return this._stream;
			}
		}

		// Token: 0x170011C2 RID: 4546
		// (get) Token: 0x06006591 RID: 26001 RVA: 0x0015A9D4 File Offset: 0x00158BD4
		internal bool LeaveOpen
		{
			get
			{
				return !this._closable;
			}
		}

		// Token: 0x170011C3 RID: 4547
		// (set) Token: 0x06006592 RID: 26002 RVA: 0x0015A9DF File Offset: 0x00158BDF
		internal bool HaveWrittenPreamble
		{
			set
			{
				this._haveWrittenPreamble = value;
			}
		}

		/// <summary>Gets the <see cref="T:System.Text.Encoding" /> in which the output is written.</summary>
		/// <returns>The <see cref="T:System.Text.Encoding" /> specified in the constructor for the current instance, or <see cref="T:System.Text.UTF8Encoding" /> if an encoding was not specified.</returns>
		// Token: 0x170011C4 RID: 4548
		// (get) Token: 0x06006593 RID: 26003 RVA: 0x0015A9E8 File Offset: 0x00158BE8
		public override Encoding Encoding
		{
			get
			{
				return this._encoding;
			}
		}

		/// <summary>Writes a character to the stream.</summary>
		/// <param name="value">The character to write to the stream.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		/// <exception cref="T:System.ObjectDisposedException">
		///   <see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and current writer is closed.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and the contents of the buffer cannot be written to the underlying fixed size stream because the <see cref="T:System.IO.StreamWriter" /> is at the end the stream.</exception>
		// Token: 0x06006594 RID: 26004 RVA: 0x0015A9F0 File Offset: 0x00158BF0
		public override void Write(char value)
		{
			this.CheckAsyncTaskInProgress();
			if (this._charPos == this._charLen)
			{
				this.Flush(false, false);
			}
			this._charBuffer[this._charPos] = value;
			this._charPos++;
			if (this._autoFlush)
			{
				this.Flush(true, false);
			}
		}

		/// <summary>Writes a character array to the stream.</summary>
		/// <param name="buffer">A character array containing the data to write. If <paramref name="buffer" /> is <see langword="null" />, nothing is written.</param>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		/// <exception cref="T:System.ObjectDisposedException">
		///   <see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and current writer is closed.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and the contents of the buffer cannot be written to the underlying fixed size stream because the <see cref="T:System.IO.StreamWriter" /> is at the end the stream.</exception>
		// Token: 0x06006595 RID: 26005 RVA: 0x0015AA45 File Offset: 0x00158C45
		[MethodImpl(MethodImplOptions.NoInlining)]
		public override void Write(char[] buffer)
		{
			this.WriteSpan(buffer, false);
		}

		/// <summary>Writes a subarray of characters to the stream.</summary>
		/// <param name="buffer">A character array that contains the data to write.</param>
		/// <param name="index">The character position in the buffer at which to start reading data.</param>
		/// <param name="count">The maximum number of characters to write.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The buffer length minus <paramref name="index" /> is less than <paramref name="count" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		/// <exception cref="T:System.ObjectDisposedException">
		///   <see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and current writer is closed.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and the contents of the buffer cannot be written to the underlying fixed size stream because the <see cref="T:System.IO.StreamWriter" /> is at the end the stream.</exception>
		// Token: 0x06006596 RID: 26006 RVA: 0x0015AA54 File Offset: 0x00158C54
		[MethodImpl(MethodImplOptions.NoInlining)]
		public override void Write(char[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", "Buffer cannot be null.");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "Non-negative number required.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			this.WriteSpan(buffer.AsSpan(index, count), false);
		}

		// Token: 0x06006597 RID: 26007 RVA: 0x0015AAC3 File Offset: 0x00158CC3
		[MethodImpl(MethodImplOptions.NoInlining)]
		public override void Write(ReadOnlySpan<char> buffer)
		{
			if (base.GetType() == typeof(StreamWriter))
			{
				this.WriteSpan(buffer, false);
				return;
			}
			base.Write(buffer);
		}

		// Token: 0x06006598 RID: 26008 RVA: 0x0015AAEC File Offset: 0x00158CEC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe void WriteSpan(ReadOnlySpan<char> buffer, bool appendNewLine)
		{
			this.CheckAsyncTaskInProgress();
			if (buffer.Length <= 4 && buffer.Length <= this._charLen - this._charPos)
			{
				for (int i = 0; i < buffer.Length; i++)
				{
					char[] charBuffer = this._charBuffer;
					int charPos = this._charPos;
					this._charPos = charPos + 1;
					charBuffer[charPos] = *buffer[i];
				}
			}
			else
			{
				char[] charBuffer2 = this._charBuffer;
				if (charBuffer2 == null)
				{
					throw new ObjectDisposedException(null, "Can not write to a closed TextWriter.");
				}
				fixed (char* reference = MemoryMarshal.GetReference<char>(buffer))
				{
					char* ptr = reference;
					fixed (char* ptr2 = &charBuffer2[0])
					{
						char* ptr3 = ptr2;
						char* ptr4 = ptr;
						int j = buffer.Length;
						int num = this._charPos;
						while (j > 0)
						{
							if (num == charBuffer2.Length)
							{
								this.Flush(false, false);
								num = 0;
							}
							int num2 = Math.Min(charBuffer2.Length - num, j);
							int num3 = num2 * 2;
							Buffer.MemoryCopy((void*)ptr4, (void*)(ptr3 + num), (long)num3, (long)num3);
							this._charPos += num2;
							num += num2;
							ptr4 += num2;
							j -= num2;
						}
					}
				}
			}
			if (appendNewLine)
			{
				char[] coreNewLine = this.CoreNewLine;
				for (int k = 0; k < coreNewLine.Length; k++)
				{
					if (this._charPos == this._charLen)
					{
						this.Flush(false, false);
					}
					this._charBuffer[this._charPos] = coreNewLine[k];
					this._charPos++;
				}
			}
			if (this._autoFlush)
			{
				this.Flush(true, false);
			}
		}

		/// <summary>Writes a string to the stream.</summary>
		/// <param name="value">The string to write to the stream. If <paramref name="value" /> is null, nothing is written.</param>
		/// <exception cref="T:System.ObjectDisposedException">
		///   <see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and current writer is closed.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <see cref="P:System.IO.StreamWriter.AutoFlush" /> is true or the <see cref="T:System.IO.StreamWriter" /> buffer is full, and the contents of the buffer cannot be written to the underlying fixed size stream because the <see cref="T:System.IO.StreamWriter" /> is at the end the stream.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
		// Token: 0x06006599 RID: 26009 RVA: 0x0015AC6C File Offset: 0x00158E6C
		[MethodImpl(MethodImplOptions.NoInlining)]
		public override void Write(string value)
		{
			this.WriteSpan(value, false);
		}

		// Token: 0x0600659A RID: 26010 RVA: 0x0015AC7B File Offset: 0x00158E7B
		[MethodImpl(MethodImplOptions.NoInlining)]
		public override void WriteLine(string value)
		{
			this.CheckAsyncTaskInProgress();
			this.WriteSpan(value, true);
		}

		// Token: 0x0600659B RID: 26011 RVA: 0x0015AC90 File Offset: 0x00158E90
		[MethodImpl(MethodImplOptions.NoInlining)]
		public override void WriteLine(ReadOnlySpan<char> value)
		{
			if (base.GetType() == typeof(StreamWriter))
			{
				this.CheckAsyncTaskInProgress();
				this.WriteSpan(value, true);
				return;
			}
			base.WriteLine(value);
		}

		/// <summary>Writes a character to the stream asynchronously.</summary>
		/// <param name="value">The character to write to the stream.</param>
		/// <returns>A task that represents the asynchronous write operation.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The stream writer is disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The stream writer is currently in use by a previous write operation.</exception>
		// Token: 0x0600659C RID: 26012 RVA: 0x0015ACC0 File Offset: 0x00158EC0
		public override Task WriteAsync(char value)
		{
			if (base.GetType() != typeof(StreamWriter))
			{
				return base.WriteAsync(value);
			}
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Can not write to a closed TextWriter.");
			}
			this.CheckAsyncTaskInProgress();
			Task task = StreamWriter.WriteAsyncInternal(this, value, this._charBuffer, this._charPos, this._charLen, this.CoreNewLine, this._autoFlush, false);
			this._asyncWriteTask = task;
			return task;
		}

		// Token: 0x0600659D RID: 26013 RVA: 0x0015AD38 File Offset: 0x00158F38
		private static Task WriteAsyncInternal(StreamWriter _this, char value, char[] charBuffer, int charPos, int charLen, char[] coreNewLine, bool autoFlush, bool appendNewLine)
		{
			StreamWriter.<WriteAsyncInternal>d__57 <WriteAsyncInternal>d__;
			<WriteAsyncInternal>d__._this = _this;
			<WriteAsyncInternal>d__.value = value;
			<WriteAsyncInternal>d__.charBuffer = charBuffer;
			<WriteAsyncInternal>d__.charPos = charPos;
			<WriteAsyncInternal>d__.charLen = charLen;
			<WriteAsyncInternal>d__.coreNewLine = coreNewLine;
			<WriteAsyncInternal>d__.autoFlush = autoFlush;
			<WriteAsyncInternal>d__.appendNewLine = appendNewLine;
			<WriteAsyncInternal>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<WriteAsyncInternal>d__.<>1__state = -1;
			<WriteAsyncInternal>d__.<>t__builder.Start<StreamWriter.<WriteAsyncInternal>d__57>(ref <WriteAsyncInternal>d__);
			return <WriteAsyncInternal>d__.<>t__builder.Task;
		}

		/// <summary>Writes a string to the stream asynchronously.</summary>
		/// <param name="value">The string to write to the stream. If <paramref name="value" /> is <see langword="null" />, nothing is written.</param>
		/// <returns>A task that represents the asynchronous write operation.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The stream writer is disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The stream writer is currently in use by a previous write operation.</exception>
		// Token: 0x0600659E RID: 26014 RVA: 0x0015ADB8 File Offset: 0x00158FB8
		public override Task WriteAsync(string value)
		{
			if (base.GetType() != typeof(StreamWriter))
			{
				return base.WriteAsync(value);
			}
			if (value == null)
			{
				return Task.CompletedTask;
			}
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Can not write to a closed TextWriter.");
			}
			this.CheckAsyncTaskInProgress();
			Task task = StreamWriter.WriteAsyncInternal(this, value, this._charBuffer, this._charPos, this._charLen, this.CoreNewLine, this._autoFlush, false);
			this._asyncWriteTask = task;
			return task;
		}

		// Token: 0x0600659F RID: 26015 RVA: 0x0015AE38 File Offset: 0x00159038
		private static Task WriteAsyncInternal(StreamWriter _this, string value, char[] charBuffer, int charPos, int charLen, char[] coreNewLine, bool autoFlush, bool appendNewLine)
		{
			StreamWriter.<WriteAsyncInternal>d__59 <WriteAsyncInternal>d__;
			<WriteAsyncInternal>d__._this = _this;
			<WriteAsyncInternal>d__.value = value;
			<WriteAsyncInternal>d__.charBuffer = charBuffer;
			<WriteAsyncInternal>d__.charPos = charPos;
			<WriteAsyncInternal>d__.charLen = charLen;
			<WriteAsyncInternal>d__.coreNewLine = coreNewLine;
			<WriteAsyncInternal>d__.autoFlush = autoFlush;
			<WriteAsyncInternal>d__.appendNewLine = appendNewLine;
			<WriteAsyncInternal>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<WriteAsyncInternal>d__.<>1__state = -1;
			<WriteAsyncInternal>d__.<>t__builder.Start<StreamWriter.<WriteAsyncInternal>d__59>(ref <WriteAsyncInternal>d__);
			return <WriteAsyncInternal>d__.<>t__builder.Task;
		}

		/// <summary>Writes a subarray of characters to the stream asynchronously.</summary>
		/// <param name="buffer">A character array that contains the data to write.</param>
		/// <param name="index">The character position in the buffer at which to begin reading data.</param>
		/// <param name="count">The maximum number of characters to write.</param>
		/// <returns>A task that represents the asynchronous write operation.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="index" /> plus <paramref name="count" /> is greater than the buffer length.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream writer is disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The stream writer is currently in use by a previous write operation.</exception>
		// Token: 0x060065A0 RID: 26016 RVA: 0x0015AEB8 File Offset: 0x001590B8
		public override Task WriteAsync(char[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", "Buffer cannot be null.");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "Non-negative number required.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			if (base.GetType() != typeof(StreamWriter))
			{
				return base.WriteAsync(buffer, index, count);
			}
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Can not write to a closed TextWriter.");
			}
			this.CheckAsyncTaskInProgress();
			Task task = StreamWriter.WriteAsyncInternal(this, new ReadOnlyMemory<char>(buffer, index, count), this._charBuffer, this._charPos, this._charLen, this.CoreNewLine, this._autoFlush, false, default(CancellationToken));
			this._asyncWriteTask = task;
			return task;
		}

		// Token: 0x060065A1 RID: 26017 RVA: 0x0015AF90 File Offset: 0x00159190
		public override Task WriteAsync(ReadOnlyMemory<char> buffer, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (base.GetType() != typeof(StreamWriter))
			{
				return base.WriteAsync(buffer, cancellationToken);
			}
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Can not write to a closed TextWriter.");
			}
			this.CheckAsyncTaskInProgress();
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled(cancellationToken);
			}
			Task task = StreamWriter.WriteAsyncInternal(this, buffer, this._charBuffer, this._charPos, this._charLen, this.CoreNewLine, this._autoFlush, false, cancellationToken);
			this._asyncWriteTask = task;
			return task;
		}

		// Token: 0x060065A2 RID: 26018 RVA: 0x0015B018 File Offset: 0x00159218
		private static Task WriteAsyncInternal(StreamWriter _this, ReadOnlyMemory<char> source, char[] charBuffer, int charPos, int charLen, char[] coreNewLine, bool autoFlush, bool appendNewLine, CancellationToken cancellationToken)
		{
			StreamWriter.<WriteAsyncInternal>d__62 <WriteAsyncInternal>d__;
			<WriteAsyncInternal>d__._this = _this;
			<WriteAsyncInternal>d__.source = source;
			<WriteAsyncInternal>d__.charBuffer = charBuffer;
			<WriteAsyncInternal>d__.charPos = charPos;
			<WriteAsyncInternal>d__.charLen = charLen;
			<WriteAsyncInternal>d__.coreNewLine = coreNewLine;
			<WriteAsyncInternal>d__.autoFlush = autoFlush;
			<WriteAsyncInternal>d__.appendNewLine = appendNewLine;
			<WriteAsyncInternal>d__.cancellationToken = cancellationToken;
			<WriteAsyncInternal>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<WriteAsyncInternal>d__.<>1__state = -1;
			<WriteAsyncInternal>d__.<>t__builder.Start<StreamWriter.<WriteAsyncInternal>d__62>(ref <WriteAsyncInternal>d__);
			return <WriteAsyncInternal>d__.<>t__builder.Task;
		}

		/// <summary>Writes a line terminator asynchronously to the stream.</summary>
		/// <returns>A task that represents the asynchronous write operation.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The stream writer is disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The stream writer is currently in use by a previous write operation.</exception>
		// Token: 0x060065A3 RID: 26019 RVA: 0x0015B0A0 File Offset: 0x001592A0
		public override Task WriteLineAsync()
		{
			if (base.GetType() != typeof(StreamWriter))
			{
				return base.WriteLineAsync();
			}
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Can not write to a closed TextWriter.");
			}
			this.CheckAsyncTaskInProgress();
			Task task = StreamWriter.WriteAsyncInternal(this, ReadOnlyMemory<char>.Empty, this._charBuffer, this._charPos, this._charLen, this.CoreNewLine, this._autoFlush, true, default(CancellationToken));
			this._asyncWriteTask = task;
			return task;
		}

		/// <summary>Writes a character followed by a line terminator asynchronously to the stream.</summary>
		/// <param name="value">The character to write to the stream.</param>
		/// <returns>A task that represents the asynchronous write operation.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The stream writer is disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The stream writer is currently in use by a previous write operation.</exception>
		// Token: 0x060065A4 RID: 26020 RVA: 0x0015B124 File Offset: 0x00159324
		public override Task WriteLineAsync(char value)
		{
			if (base.GetType() != typeof(StreamWriter))
			{
				return base.WriteLineAsync(value);
			}
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Can not write to a closed TextWriter.");
			}
			this.CheckAsyncTaskInProgress();
			Task task = StreamWriter.WriteAsyncInternal(this, value, this._charBuffer, this._charPos, this._charLen, this.CoreNewLine, this._autoFlush, true);
			this._asyncWriteTask = task;
			return task;
		}

		/// <summary>Writes a string followed by a line terminator asynchronously to the stream.</summary>
		/// <param name="value">The string to write. If the value is <see langword="null" />, only a line terminator is written.</param>
		/// <returns>A task that represents the asynchronous write operation.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The stream writer is disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The stream writer is currently in use by a previous write operation.</exception>
		// Token: 0x060065A5 RID: 26021 RVA: 0x0015B19C File Offset: 0x0015939C
		public override Task WriteLineAsync(string value)
		{
			if (value == null)
			{
				return this.WriteLineAsync();
			}
			if (base.GetType() != typeof(StreamWriter))
			{
				return base.WriteLineAsync(value);
			}
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Can not write to a closed TextWriter.");
			}
			this.CheckAsyncTaskInProgress();
			Task task = StreamWriter.WriteAsyncInternal(this, value, this._charBuffer, this._charPos, this._charLen, this.CoreNewLine, this._autoFlush, true);
			this._asyncWriteTask = task;
			return task;
		}

		/// <summary>Writes a subarray of characters followed by a line terminator asynchronously to the stream.</summary>
		/// <param name="buffer">The character array to write data from.</param>
		/// <param name="index">The character position in the buffer at which to start reading data.</param>
		/// <param name="count">The maximum number of characters to write.</param>
		/// <returns>A task that represents the asynchronous write operation.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="index" /> plus <paramref name="count" /> is greater than the buffer length.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="index" /> or <paramref name="count" /> is negative.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream writer is disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The stream writer is currently in use by a previous write operation.</exception>
		// Token: 0x060065A6 RID: 26022 RVA: 0x0015B21C File Offset: 0x0015941C
		public override Task WriteLineAsync(char[] buffer, int index, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", "Buffer cannot be null.");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "Non-negative number required.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
			}
			if (buffer.Length - index < count)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			if (base.GetType() != typeof(StreamWriter))
			{
				return base.WriteLineAsync(buffer, index, count);
			}
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Can not write to a closed TextWriter.");
			}
			this.CheckAsyncTaskInProgress();
			Task task = StreamWriter.WriteAsyncInternal(this, new ReadOnlyMemory<char>(buffer, index, count), this._charBuffer, this._charPos, this._charLen, this.CoreNewLine, this._autoFlush, true, default(CancellationToken));
			this._asyncWriteTask = task;
			return task;
		}

		// Token: 0x060065A7 RID: 26023 RVA: 0x0015B2F4 File Offset: 0x001594F4
		public override Task WriteLineAsync(ReadOnlyMemory<char> buffer, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (base.GetType() != typeof(StreamWriter))
			{
				return base.WriteLineAsync(buffer, cancellationToken);
			}
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Can not write to a closed TextWriter.");
			}
			this.CheckAsyncTaskInProgress();
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled(cancellationToken);
			}
			Task task = StreamWriter.WriteAsyncInternal(this, buffer, this._charBuffer, this._charPos, this._charLen, this.CoreNewLine, this._autoFlush, true, cancellationToken);
			this._asyncWriteTask = task;
			return task;
		}

		/// <summary>Clears all buffers for this stream asynchronously and causes any buffered data to be written to the underlying device.</summary>
		/// <returns>A task that represents the asynchronous flush operation.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
		// Token: 0x060065A8 RID: 26024 RVA: 0x0015B37C File Offset: 0x0015957C
		public override Task FlushAsync()
		{
			if (base.GetType() != typeof(StreamWriter))
			{
				return base.FlushAsync();
			}
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Can not write to a closed TextWriter.");
			}
			this.CheckAsyncTaskInProgress();
			Task task = this.FlushAsyncInternal(true, true, this._charBuffer, this._charPos, default(CancellationToken));
			this._asyncWriteTask = task;
			return task;
		}

		// Token: 0x170011C5 RID: 4549
		// (set) Token: 0x060065A9 RID: 26025 RVA: 0x0015B3E7 File Offset: 0x001595E7
		private int CharPos_Prop
		{
			set
			{
				this._charPos = value;
			}
		}

		// Token: 0x170011C6 RID: 4550
		// (set) Token: 0x060065AA RID: 26026 RVA: 0x0015A9DF File Offset: 0x00158BDF
		private bool HaveWrittenPreamble_Prop
		{
			set
			{
				this._haveWrittenPreamble = value;
			}
		}

		// Token: 0x060065AB RID: 26027 RVA: 0x0015B3F0 File Offset: 0x001595F0
		private Task FlushAsyncInternal(bool flushStream, bool flushEncoder, char[] sCharBuffer, int sCharPos, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled(cancellationToken);
			}
			if (sCharPos == 0 && !flushStream && !flushEncoder)
			{
				return Task.CompletedTask;
			}
			Task result = StreamWriter.FlushAsyncInternal(this, flushStream, flushEncoder, sCharBuffer, sCharPos, this._haveWrittenPreamble, this._encoding, this._encoder, this._byteBuffer, this._stream, cancellationToken);
			this._charPos = 0;
			return result;
		}

		// Token: 0x060065AC RID: 26028 RVA: 0x0015B450 File Offset: 0x00159650
		private static Task FlushAsyncInternal(StreamWriter _this, bool flushStream, bool flushEncoder, char[] charBuffer, int charPos, bool haveWrittenPreamble, Encoding encoding, Encoder encoder, byte[] byteBuffer, Stream stream, CancellationToken cancellationToken)
		{
			StreamWriter.<FlushAsyncInternal>d__74 <FlushAsyncInternal>d__;
			<FlushAsyncInternal>d__._this = _this;
			<FlushAsyncInternal>d__.flushStream = flushStream;
			<FlushAsyncInternal>d__.flushEncoder = flushEncoder;
			<FlushAsyncInternal>d__.charBuffer = charBuffer;
			<FlushAsyncInternal>d__.charPos = charPos;
			<FlushAsyncInternal>d__.haveWrittenPreamble = haveWrittenPreamble;
			<FlushAsyncInternal>d__.encoding = encoding;
			<FlushAsyncInternal>d__.encoder = encoder;
			<FlushAsyncInternal>d__.byteBuffer = byteBuffer;
			<FlushAsyncInternal>d__.stream = stream;
			<FlushAsyncInternal>d__.cancellationToken = cancellationToken;
			<FlushAsyncInternal>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<FlushAsyncInternal>d__.<>1__state = -1;
			<FlushAsyncInternal>d__.<>t__builder.Start<StreamWriter.<FlushAsyncInternal>d__74>(ref <FlushAsyncInternal>d__);
			return <FlushAsyncInternal>d__.<>t__builder.Task;
		}

		// Token: 0x04003B9E RID: 15262
		internal const int DefaultBufferSize = 1024;

		// Token: 0x04003B9F RID: 15263
		private const int DefaultFileStreamBufferSize = 4096;

		// Token: 0x04003BA0 RID: 15264
		private const int MinBufferSize = 128;

		// Token: 0x04003BA1 RID: 15265
		private const int DontCopyOnWriteLineThreshold = 512;

		/// <summary>Provides a <see langword="StreamWriter" /> with no backing store that can be written to, but not read from.</summary>
		// Token: 0x04003BA2 RID: 15266
		public new static readonly StreamWriter Null = new StreamWriter(Stream.Null, StreamWriter.UTF8NoBOM, 128, true);

		// Token: 0x04003BA3 RID: 15267
		private Stream _stream;

		// Token: 0x04003BA4 RID: 15268
		private Encoding _encoding;

		// Token: 0x04003BA5 RID: 15269
		private Encoder _encoder;

		// Token: 0x04003BA6 RID: 15270
		private byte[] _byteBuffer;

		// Token: 0x04003BA7 RID: 15271
		private char[] _charBuffer;

		// Token: 0x04003BA8 RID: 15272
		private int _charPos;

		// Token: 0x04003BA9 RID: 15273
		private int _charLen;

		// Token: 0x04003BAA RID: 15274
		private bool _autoFlush;

		// Token: 0x04003BAB RID: 15275
		private bool _haveWrittenPreamble;

		// Token: 0x04003BAC RID: 15276
		private bool _closable;

		// Token: 0x04003BAD RID: 15277
		private Task _asyncWriteTask = Task.CompletedTask;
	}
}
