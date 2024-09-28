using System;
using System.Diagnostics;
using System.Net.Sockets;

namespace kcp2k
{
	// Token: 0x0200000B RID: 11
	public class KcpPeer
	{
		// Token: 0x06000019 RID: 25 RVA: 0x0000275C File Offset: 0x0000095C
		private static int ReliableMaxMessageSize_Unconstrained(int mtu, uint rcv_wnd)
		{
			return (mtu - 24 - 5) * (int)(rcv_wnd - 1U) - 1;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000276A File Offset: 0x0000096A
		public static int ReliableMaxMessageSize(int mtu, uint rcv_wnd)
		{
			return KcpPeer.ReliableMaxMessageSize_Unconstrained(mtu, Math.Min(rcv_wnd, 255U));
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000277D File Offset: 0x0000097D
		public static int UnreliableMaxMessageSize(int mtu)
		{
			return mtu - 5;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002782 File Offset: 0x00000982
		public int SendQueueCount
		{
			get
			{
				return this.kcp.snd_queue.Count;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600001D RID: 29 RVA: 0x00002794 File Offset: 0x00000994
		public int ReceiveQueueCount
		{
			get
			{
				return this.kcp.rcv_queue.Count;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000027A6 File Offset: 0x000009A6
		public int SendBufferCount
		{
			get
			{
				return this.kcp.snd_buf.Count;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600001F RID: 31 RVA: 0x000027B8 File Offset: 0x000009B8
		public int ReceiveBufferCount
		{
			get
			{
				return this.kcp.rcv_buf.Count;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000020 RID: 32 RVA: 0x000027CA File Offset: 0x000009CA
		public uint MaxSendRate
		{
			get
			{
				return this.kcp.snd_wnd * this.kcp.mtu * 1000U / this.kcp.interval;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000021 RID: 33 RVA: 0x000027F5 File Offset: 0x000009F5
		public uint MaxReceiveRate
		{
			get
			{
				return this.kcp.rcv_wnd * this.kcp.mtu * 1000U / this.kcp.interval;
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002820 File Offset: 0x00000A20
		public KcpPeer(Action<ArraySegment<byte>> output, Action OnAuthenticated, Action<ArraySegment<byte>, KcpChannel> OnData, Action OnDisconnected, Action<ErrorCode, string> OnError, KcpConfig config, uint cookie)
		{
			this.OnAuthenticated = OnAuthenticated;
			this.OnData = OnData;
			this.OnDisconnected = OnDisconnected;
			this.OnError = OnError;
			this.RawSend = output;
			this.kcp = new Kcp(0U, new Action<byte[], int>(this.RawSendReliable));
			this.cookie = cookie;
			this.kcp.SetNoDelay(config.NoDelay ? 1U : 0U, config.Interval, config.FastResend, !config.CongestionWindow);
			this.kcp.SetWindowSize(config.SendWindowSize, config.ReceiveWindowSize);
			this.kcp.SetMtu((uint)(config.Mtu - 5));
			this.rawSendBuffer = new byte[config.Mtu];
			this.unreliableMax = KcpPeer.UnreliableMaxMessageSize(config.Mtu);
			this.reliableMax = KcpPeer.ReliableMaxMessageSize(config.Mtu, config.ReceiveWindowSize);
			this.kcp.dead_link = config.MaxRetransmits;
			this.kcpMessageBuffer = new byte[1 + this.reliableMax];
			this.kcpSendBuffer = new byte[1 + this.reliableMax];
			this.timeout = config.Timeout;
			this.watch.Start();
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002979 File Offset: 0x00000B79
		private void HandleTimeout(uint time)
		{
			if ((ulong)time >= (ulong)this.lastReceiveTime + (ulong)((long)this.timeout))
			{
				this.OnError(ErrorCode.Timeout, string.Format("KcpPeer: Connection timed out after not receiving any message for {0}ms. Disconnecting.", this.timeout));
				this.Disconnect();
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000029B5 File Offset: 0x00000BB5
		private void HandleDeadLink()
		{
			if (this.kcp.state == -1)
			{
				this.OnError(ErrorCode.Timeout, string.Format("KcpPeer: dead_link detected: a message was retransmitted {0} times without ack. Disconnecting.", this.kcp.dead_link));
				this.Disconnect();
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000029F1 File Offset: 0x00000BF1
		private void HandlePing(uint time)
		{
			if (time >= this.lastPingTime + 1000U)
			{
				this.SendPing();
				this.lastPingTime = time;
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002A10 File Offset: 0x00000C10
		private void HandleChoked()
		{
			int num = this.kcp.rcv_queue.Count + this.kcp.snd_queue.Count + this.kcp.rcv_buf.Count + this.kcp.snd_buf.Count;
			if (num >= 10000)
			{
				this.OnError(ErrorCode.Congestion, "KcpPeer: disconnecting connection because it can't process data fast enough.\n" + string.Format("Queue total {0}>{1}. rcv_queue={2} snd_queue={3} rcv_buf={4} snd_buf={5}\n", new object[]
				{
					num,
					10000,
					this.kcp.rcv_queue.Count,
					this.kcp.snd_queue.Count,
					this.kcp.rcv_buf.Count,
					this.kcp.snd_buf.Count
				}) + "* Try to Enable NoDelay, decrease INTERVAL, disable Congestion Window (= enable NOCWND!), increase SEND/RECV WINDOW or compress data.\n* Or perhaps the network is simply too slow on our end, or on the other end.");
				this.kcp.snd_queue.Clear();
				this.Disconnect();
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002B24 File Offset: 0x00000D24
		private bool ReceiveNextReliable(out KcpHeader header, out ArraySegment<byte> message)
		{
			message = default(ArraySegment<byte>);
			header = KcpHeader.Disconnect;
			int num = this.kcp.PeekSize();
			if (num <= 0)
			{
				return false;
			}
			if (num > this.kcpMessageBuffer.Length)
			{
				this.OnError(ErrorCode.InvalidReceive, string.Format("KcpPeer: possible allocation attack for msgSize {0} > buffer {1}. Disconnecting the connection.", num, this.kcpMessageBuffer.Length));
				this.Disconnect();
				return false;
			}
			int num2 = this.kcp.Receive(this.kcpMessageBuffer, num);
			if (num2 < 0)
			{
				this.OnError(ErrorCode.InvalidReceive, string.Format("KcpPeer: Receive failed with error={0}. closing connection.", num2));
				this.Disconnect();
				return false;
			}
			header = (KcpHeader)this.kcpMessageBuffer[0];
			message = new ArraySegment<byte>(this.kcpMessageBuffer, 1, num - 1);
			this.lastReceiveTime = (uint)this.watch.ElapsedMilliseconds;
			return true;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002BF8 File Offset: 0x00000DF8
		private void TickIncoming_Connected(uint time)
		{
			this.HandleTimeout(time);
			this.HandleDeadLink();
			this.HandlePing(time);
			this.HandleChoked();
			KcpHeader kcpHeader;
			ArraySegment<byte> arraySegment;
			if (this.ReceiveNextReliable(out kcpHeader, out arraySegment))
			{
				switch (kcpHeader)
				{
				case KcpHeader.Handshake:
				{
					if (arraySegment.Count != 4)
					{
						this.OnError(ErrorCode.InvalidReceive, string.Format("KcpPeer: received invalid handshake message with size {0} != 4. Disconnecting the connection.", arraySegment.Count));
						this.Disconnect();
						return;
					}
					Buffer.BlockCopy(arraySegment.Array, arraySegment.Offset, this.receivedCookie, 0, 4);
					uint num = BitConverter.ToUInt32(arraySegment.Array, arraySegment.Offset);
					Log.Info(string.Format("KcpPeer: received handshake with cookie={0}", num));
					this.state = KcpState.Authenticated;
					Action onAuthenticated = this.OnAuthenticated;
					if (onAuthenticated == null)
					{
						return;
					}
					onAuthenticated();
					return;
				}
				case KcpHeader.Ping:
					break;
				case KcpHeader.Data:
				case KcpHeader.Disconnect:
					this.OnError(ErrorCode.InvalidReceive, string.Format("KcpPeer: received invalid header {0} while Connected. Disconnecting the connection.", kcpHeader));
					this.Disconnect();
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002D00 File Offset: 0x00000F00
		private void TickIncoming_Authenticated(uint time)
		{
			this.HandleTimeout(time);
			this.HandleDeadLink();
			this.HandlePing(time);
			this.HandleChoked();
			KcpHeader kcpHeader;
			ArraySegment<byte> arg;
			while (this.ReceiveNextReliable(out kcpHeader, out arg))
			{
				switch (kcpHeader)
				{
				case KcpHeader.Handshake:
					Log.Warning(string.Format("KcpPeer: received invalid header {0} while Authenticated. Disconnecting the connection.", kcpHeader));
					this.Disconnect();
					break;
				case KcpHeader.Data:
					if (arg.Count > 0)
					{
						Action<ArraySegment<byte>, KcpChannel> onData = this.OnData;
						if (onData != null)
						{
							onData(arg, KcpChannel.Reliable);
						}
					}
					else
					{
						this.OnError(ErrorCode.InvalidReceive, "KcpPeer: received empty Data message while Authenticated. Disconnecting the connection.");
						this.Disconnect();
					}
					break;
				case KcpHeader.Disconnect:
					Log.Info("KcpPeer: received disconnect message");
					this.Disconnect();
					break;
				}
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002DC8 File Offset: 0x00000FC8
		public void TickIncoming()
		{
			uint time = (uint)this.watch.ElapsedMilliseconds;
			try
			{
				switch (this.state)
				{
				case KcpState.Connected:
					this.TickIncoming_Connected(time);
					break;
				case KcpState.Authenticated:
					this.TickIncoming_Authenticated(time);
					break;
				}
			}
			catch (SocketException arg)
			{
				this.OnError(ErrorCode.ConnectionClosed, string.Format("KcpPeer: Disconnecting because {0}. This is fine.", arg));
				this.Disconnect();
			}
			catch (ObjectDisposedException arg2)
			{
				this.OnError(ErrorCode.ConnectionClosed, string.Format("KcpPeer: Disconnecting because {0}. This is fine.", arg2));
				this.Disconnect();
			}
			catch (Exception arg3)
			{
				this.OnError(ErrorCode.Unexpected, string.Format("KcpPeer: unexpected Exception: {0}", arg3));
				this.Disconnect();
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002E9C File Offset: 0x0000109C
		public void TickOutgoing()
		{
			uint currentTimeMilliSeconds = (uint)this.watch.ElapsedMilliseconds;
			try
			{
				KcpState kcpState = this.state;
				if (kcpState > KcpState.Authenticated)
				{
					if (kcpState != KcpState.Disconnected)
					{
					}
				}
				else
				{
					this.kcp.Update(currentTimeMilliSeconds);
				}
			}
			catch (SocketException arg)
			{
				this.OnError(ErrorCode.ConnectionClosed, string.Format("KcpPeer: Disconnecting because {0}. This is fine.", arg));
				this.Disconnect();
			}
			catch (ObjectDisposedException arg2)
			{
				this.OnError(ErrorCode.ConnectionClosed, string.Format("KcpPeer: Disconnecting because {0}. This is fine.", arg2));
				this.Disconnect();
			}
			catch (Exception arg3)
			{
				this.OnError(ErrorCode.Unexpected, string.Format("KcpPeer: unexpected exception: {0}", arg3));
				this.Disconnect();
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002F60 File Offset: 0x00001160
		private void OnRawInputReliable(ArraySegment<byte> message)
		{
			int num = this.kcp.Input(message.Array, message.Offset, message.Count);
			if (num != 0)
			{
				Log.Warning(string.Format("KcpPeer: Input failed with error={0} for buffer with length={1}", num, message.Count - 1));
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002FB9 File Offset: 0x000011B9
		private void OnRawInputUnreliable(ArraySegment<byte> message)
		{
			if (this.state == KcpState.Authenticated)
			{
				Action<ArraySegment<byte>, KcpChannel> onData = this.OnData;
				if (onData != null)
				{
					onData(message, KcpChannel.Unreliable);
				}
				this.lastReceiveTime = (uint)this.watch.ElapsedMilliseconds;
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002FEC File Offset: 0x000011EC
		public void RawInput(ArraySegment<byte> segment)
		{
			if (segment.Count <= 5)
			{
				return;
			}
			byte b = segment.Array[segment.Offset];
			uint num = BitConverter.ToUInt32(segment.Array, segment.Offset + 1);
			if (this.state == KcpState.Authenticated && num != this.cookie)
			{
				Log.Warning(string.Format("KcpPeer: dropped message with invalid cookie: {0} expected: {1}.", num, this.cookie));
				return;
			}
			ArraySegment<byte> message = new ArraySegment<byte>(segment.Array, segment.Offset + 1 + 4, segment.Count - 1 - 4);
			if (b == 1)
			{
				this.OnRawInputReliable(message);
				return;
			}
			if (b != 2)
			{
				Log.Warning(string.Format("KcpPeer: invalid channel header: {0}, likely internet noise", b));
				return;
			}
			this.OnRawInputUnreliable(message);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000030BC File Offset: 0x000012BC
		private void RawSendReliable(byte[] data, int length)
		{
			this.rawSendBuffer[0] = 1;
			Buffer.BlockCopy(this.receivedCookie, 0, this.rawSendBuffer, 1, 4);
			Buffer.BlockCopy(data, 0, this.rawSendBuffer, 5, length);
			ArraySegment<byte> obj = new ArraySegment<byte>(this.rawSendBuffer, 0, length + 1 + 4);
			this.RawSend(obj);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00003114 File Offset: 0x00001314
		private void SendReliable(KcpHeader header, ArraySegment<byte> content)
		{
			if (1 + content.Count > this.kcpSendBuffer.Length)
			{
				this.OnError(ErrorCode.InvalidSend, string.Format("KcpPeer: Failed to send reliable message of size {0} because it's larger than ReliableMaxMessageSize={1}", content.Count, this.reliableMax));
				return;
			}
			this.kcpSendBuffer[0] = (byte)header;
			if (content.Count > 0)
			{
				Buffer.BlockCopy(content.Array, content.Offset, this.kcpSendBuffer, 1, content.Count);
			}
			int num = this.kcp.Send(this.kcpSendBuffer, 0, 1 + content.Count);
			if (num < 0)
			{
				this.OnError(ErrorCode.InvalidSend, string.Format("KcpPeer: Send failed with error={0} for content with length={1}", num, content.Count));
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000031E0 File Offset: 0x000013E0
		private void SendUnreliable(ArraySegment<byte> message)
		{
			if (message.Count > this.unreliableMax)
			{
				Log.Error(string.Format("KcpPeer: Failed to send unreliable message of size {0} because it's larger than UnreliableMaxMessageSize={1}", message.Count, this.unreliableMax));
				return;
			}
			this.rawSendBuffer[0] = 2;
			Buffer.BlockCopy(this.receivedCookie, 0, this.rawSendBuffer, 1, 4);
			Buffer.BlockCopy(message.Array, message.Offset, this.rawSendBuffer, 5, message.Count);
			ArraySegment<byte> obj = new ArraySegment<byte>(this.rawSendBuffer, 0, message.Count + 1 + 4);
			this.RawSend(obj);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000328C File Offset: 0x0000148C
		public void SendHandshake()
		{
			byte[] bytes = BitConverter.GetBytes(this.cookie);
			Log.Info(string.Format("KcpPeer: sending Handshake to other end with cookie={0}!", this.cookie));
			this.SendReliable(KcpHeader.Handshake, new ArraySegment<byte>(bytes));
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000032D1 File Offset: 0x000014D1
		public void SendData(ArraySegment<byte> data, KcpChannel channel)
		{
			if (data.Count == 0)
			{
				this.OnError(ErrorCode.InvalidSend, "KcpPeer: tried sending empty message. This should never happen. Disconnecting.");
				this.Disconnect();
				return;
			}
			if (channel == KcpChannel.Reliable)
			{
				this.SendReliable(KcpHeader.Data, data);
				return;
			}
			if (channel != KcpChannel.Unreliable)
			{
				return;
			}
			this.SendUnreliable(data);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003310 File Offset: 0x00001510
		private void SendPing()
		{
			this.SendReliable(KcpHeader.Ping, default(ArraySegment<byte>));
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003330 File Offset: 0x00001530
		private void SendDisconnect()
		{
			this.SendReliable(KcpHeader.Disconnect, default(ArraySegment<byte>));
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003350 File Offset: 0x00001550
		public void Disconnect()
		{
			if (this.state == KcpState.Disconnected)
			{
				return;
			}
			try
			{
				this.SendDisconnect();
				this.kcp.Flush();
			}
			catch (SocketException)
			{
			}
			catch (ObjectDisposedException)
			{
			}
			Log.Info("KcpPeer: Disconnected.");
			this.state = KcpState.Disconnected;
			Action onDisconnected = this.OnDisconnected;
			if (onDisconnected == null)
			{
				return;
			}
			onDisconnected();
		}

		// Token: 0x0400002D RID: 45
		internal Kcp kcp;

		// Token: 0x0400002E RID: 46
		private readonly uint cookie;

		// Token: 0x0400002F RID: 47
		internal readonly byte[] receivedCookie = new byte[4];

		// Token: 0x04000030 RID: 48
		private readonly Action<ArraySegment<byte>> RawSend;

		// Token: 0x04000031 RID: 49
		private KcpState state;

		// Token: 0x04000032 RID: 50
		private readonly Action OnAuthenticated;

		// Token: 0x04000033 RID: 51
		private readonly Action<ArraySegment<byte>, KcpChannel> OnData;

		// Token: 0x04000034 RID: 52
		private readonly Action OnDisconnected;

		// Token: 0x04000035 RID: 53
		private readonly Action<ErrorCode, string> OnError;

		// Token: 0x04000036 RID: 54
		public const int DEFAULT_TIMEOUT = 10000;

		// Token: 0x04000037 RID: 55
		public int timeout;

		// Token: 0x04000038 RID: 56
		private uint lastReceiveTime;

		// Token: 0x04000039 RID: 57
		private readonly Stopwatch watch = new Stopwatch();

		// Token: 0x0400003A RID: 58
		private const int CHANNEL_HEADER_SIZE = 1;

		// Token: 0x0400003B RID: 59
		private const int COOKIE_HEADER_SIZE = 4;

		// Token: 0x0400003C RID: 60
		private const int METADATA_SIZE = 5;

		// Token: 0x0400003D RID: 61
		private readonly byte[] kcpMessageBuffer;

		// Token: 0x0400003E RID: 62
		private readonly byte[] kcpSendBuffer;

		// Token: 0x0400003F RID: 63
		private readonly byte[] rawSendBuffer;

		// Token: 0x04000040 RID: 64
		public const int PING_INTERVAL = 1000;

		// Token: 0x04000041 RID: 65
		private uint lastPingTime;

		// Token: 0x04000042 RID: 66
		internal const int QueueDisconnectThreshold = 10000;

		// Token: 0x04000043 RID: 67
		public readonly int unreliableMax;

		// Token: 0x04000044 RID: 68
		public readonly int reliableMax;
	}
}
