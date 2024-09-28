using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace kcp2k
{
	// Token: 0x02000007 RID: 7
	public class KcpClient
	{
		// Token: 0x0600000D RID: 13 RVA: 0x000023C0 File Offset: 0x000005C0
		public KcpClient(Action OnConnected, Action<ArraySegment<byte>, KcpChannel> OnData, Action OnDisconnected, Action<ErrorCode, string> OnError, KcpConfig config)
		{
			this.OnConnected = OnConnected;
			this.OnData = OnData;
			this.OnDisconnected = OnDisconnected;
			this.OnError = OnError;
			this.config = config;
			this.rawReceiveBuffer = new byte[config.Mtu];
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002400 File Offset: 0x00000600
		public void Connect(string address, ushort port)
		{
			if (this.connected)
			{
				Log.Warning("KcpClient: already connected!");
				return;
			}
			IPAddress[] array;
			if (!Common.ResolveHostname(address, out array))
			{
				this.OnError(ErrorCode.DnsResolve, "Failed to resolve host: " + address);
				this.OnDisconnected();
				return;
			}
			this.peer = new KcpPeer(new Action<ArraySegment<byte>>(this.RawSend), new Action(this.<Connect>g__OnAuthenticatedWrap|11_0), this.OnData, new Action(this.<Connect>g__OnDisconnectedWrap|11_1), this.OnError, this.config, 0U);
			Log.Info(string.Format("KcpClient: connect to {0}:{1}", address, port));
			this.remoteEndPoint = new IPEndPoint(array[0], (int)port);
			this.socket = new Socket(this.remoteEndPoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
			this.socket.Blocking = false;
			Common.ConfigureSocketBuffers(this.socket, this.config.RecvBufferSize, this.config.SendBufferSize);
			this.socket.Connect(this.remoteEndPoint);
			this.peer.SendHandshake();
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002524 File Offset: 0x00000724
		protected virtual bool RawReceive(out ArraySegment<byte> segment)
		{
			segment = default(ArraySegment<byte>);
			if (this.socket == null)
			{
				return false;
			}
			bool result;
			try
			{
				result = this.socket.ReceiveNonBlocking(this.rawReceiveBuffer, out segment);
			}
			catch (SocketException arg)
			{
				Log.Info(string.Format("KcpClient: looks like the other end has closed the connection. This is fine: {0}", arg));
				this.peer.Disconnect();
				result = false;
			}
			return result;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002590 File Offset: 0x00000790
		protected virtual void RawSend(ArraySegment<byte> data)
		{
			try
			{
				this.socket.SendNonBlocking(data);
			}
			catch (SocketException arg)
			{
				Log.Error(string.Format("KcpClient: Send failed: {0}", arg));
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000025D4 File Offset: 0x000007D4
		public void Send(ArraySegment<byte> segment, KcpChannel channel)
		{
			if (!this.connected)
			{
				Log.Warning("KcpClient: can't send because not connected!");
				return;
			}
			this.peer.SendData(segment, channel);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000025FB File Offset: 0x000007FB
		public void Disconnect()
		{
			if (!this.connected)
			{
				return;
			}
			KcpPeer kcpPeer = this.peer;
			if (kcpPeer == null)
			{
				return;
			}
			kcpPeer.Disconnect();
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002618 File Offset: 0x00000818
		public virtual void TickIncoming()
		{
			if (this.peer != null)
			{
				ArraySegment<byte> segment;
				while (this.RawReceive(out segment))
				{
					this.peer.RawInput(segment);
				}
			}
			KcpPeer kcpPeer = this.peer;
			if (kcpPeer == null)
			{
				return;
			}
			kcpPeer.TickIncoming();
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002655 File Offset: 0x00000855
		public virtual void TickOutgoing()
		{
			KcpPeer kcpPeer = this.peer;
			if (kcpPeer == null)
			{
				return;
			}
			kcpPeer.TickOutgoing();
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002667 File Offset: 0x00000867
		public virtual void Tick()
		{
			this.TickIncoming();
			this.TickOutgoing();
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002675 File Offset: 0x00000875
		[CompilerGenerated]
		private void <Connect>g__OnAuthenticatedWrap|11_0()
		{
			Log.Info("KcpClient: OnConnected");
			this.connected = true;
			this.OnConnected();
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002698 File Offset: 0x00000898
		[CompilerGenerated]
		private void <Connect>g__OnDisconnectedWrap|11_1()
		{
			Log.Info("KcpClient: OnDisconnected");
			this.connected = false;
			this.peer = null;
			Socket socket = this.socket;
			if (socket != null)
			{
				socket.Close();
			}
			this.socket = null;
			this.remoteEndPoint = null;
			this.OnDisconnected();
		}

		// Token: 0x0400000E RID: 14
		public KcpPeer peer;

		// Token: 0x0400000F RID: 15
		protected Socket socket;

		// Token: 0x04000010 RID: 16
		public EndPoint remoteEndPoint;

		// Token: 0x04000011 RID: 17
		protected readonly KcpConfig config;

		// Token: 0x04000012 RID: 18
		protected readonly byte[] rawReceiveBuffer;

		// Token: 0x04000013 RID: 19
		protected readonly Action OnConnected;

		// Token: 0x04000014 RID: 20
		protected readonly Action<ArraySegment<byte>, KcpChannel> OnData;

		// Token: 0x04000015 RID: 21
		protected readonly Action OnDisconnected;

		// Token: 0x04000016 RID: 22
		protected readonly Action<ErrorCode, string> OnError;

		// Token: 0x04000017 RID: 23
		public bool connected;
	}
}
