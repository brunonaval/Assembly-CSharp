using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace kcp2k
{
	// Token: 0x0200000C RID: 12
	public class KcpServer
	{
		// Token: 0x06000037 RID: 55 RVA: 0x000033C4 File Offset: 0x000015C4
		public KcpServer(Action<int> OnConnected, Action<int, ArraySegment<byte>, KcpChannel> OnData, Action<int> OnDisconnected, Action<int, ErrorCode, string> OnError, KcpConfig config)
		{
			this.OnConnected = OnConnected;
			this.OnData = OnData;
			this.OnDisconnected = OnDisconnected;
			this.OnError = OnError;
			this.config = config;
			this.rawReceiveBuffer = new byte[config.Mtu];
			this.newClientEP = (config.DualMode ? new IPEndPoint(IPAddress.IPv6Any, 0) : new IPEndPoint(IPAddress.Any, 0));
		}

		// Token: 0x06000038 RID: 56 RVA: 0x0000344B File Offset: 0x0000164B
		public virtual bool IsActive()
		{
			return this.socket != null;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003458 File Offset: 0x00001658
		private static Socket CreateServerSocket(bool DualMode, ushort port)
		{
			if (DualMode)
			{
				Socket socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
				try
				{
					socket.DualMode = true;
				}
				catch (NotSupportedException arg)
				{
					Log.Warning(string.Format("Failed to set Dual Mode, continuing with IPv6 without Dual Mode. Error: {0}", arg));
				}
				socket.Bind(new IPEndPoint(IPAddress.IPv6Any, (int)port));
				return socket;
			}
			Socket socket2 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			socket2.Bind(new IPEndPoint(IPAddress.Any, (int)port));
			return socket2;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000034D4 File Offset: 0x000016D4
		public virtual void Start(ushort port)
		{
			if (this.socket != null)
			{
				Log.Warning("KcpServer: already started!");
				return;
			}
			this.socket = KcpServer.CreateServerSocket(this.config.DualMode, port);
			this.socket.Blocking = false;
			Common.ConfigureSocketBuffers(this.socket, this.config.RecvBufferSize, this.config.SendBufferSize);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003540 File Offset: 0x00001740
		public void Send(int connectionId, ArraySegment<byte> segment, KcpChannel channel)
		{
			KcpServerConnection kcpServerConnection;
			if (this.connections.TryGetValue(connectionId, out kcpServerConnection))
			{
				kcpServerConnection.peer.SendData(segment, channel);
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x0000356C File Offset: 0x0000176C
		public void Disconnect(int connectionId)
		{
			KcpServerConnection kcpServerConnection;
			if (this.connections.TryGetValue(connectionId, out kcpServerConnection))
			{
				kcpServerConnection.peer.Disconnect();
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003594 File Offset: 0x00001794
		public IPEndPoint GetClientEndPoint(int connectionId)
		{
			KcpServerConnection kcpServerConnection;
			if (this.connections.TryGetValue(connectionId, out kcpServerConnection))
			{
				return kcpServerConnection.remoteEndPoint as IPEndPoint;
			}
			return null;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000035C0 File Offset: 0x000017C0
		protected virtual bool RawReceiveFrom(out ArraySegment<byte> segment, out int connectionId)
		{
			segment = default(ArraySegment<byte>);
			connectionId = 0;
			if (this.socket == null)
			{
				return false;
			}
			try
			{
				if (this.socket.ReceiveFromNonBlocking(this.rawReceiveBuffer, out segment, ref this.newClientEP))
				{
					connectionId = Common.ConnectionHash(this.newClientEP);
					return true;
				}
			}
			catch (SocketException arg)
			{
				Log.Info(string.Format("KcpServer: ReceiveFrom failed: {0}", arg));
			}
			return false;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x0000363C File Offset: 0x0000183C
		protected virtual void RawSend(int connectionId, ArraySegment<byte> data)
		{
			KcpServerConnection kcpServerConnection;
			if (!this.connections.TryGetValue(connectionId, out kcpServerConnection))
			{
				Log.Warning(string.Format("KcpServer: RawSend invalid connectionId={0}", connectionId));
				return;
			}
			try
			{
				this.socket.SendToNonBlocking(data, kcpServerConnection.remoteEndPoint);
			}
			catch (SocketException arg)
			{
				Log.Error(string.Format("KcpServer: SendTo failed: {0}", arg));
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000036B4 File Offset: 0x000018B4
		protected virtual KcpServerConnection CreateConnection(int connectionId)
		{
			KcpServer.<>c__DisplayClass18_0 CS$<>8__locals1 = new KcpServer.<>c__DisplayClass18_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.connectionId = connectionId;
			Action<ArraySegment<byte>> output = delegate(ArraySegment<byte> data)
			{
				CS$<>8__locals1.<>4__this.RawSend(CS$<>8__locals1.connectionId, data);
			};
			CS$<>8__locals1.connection = new KcpServerConnection(this.newClientEP);
			uint cookie = Common.GenerateCookie();
			KcpPeer peer = new KcpPeer(output, new Action(CS$<>8__locals1.<CreateConnection>g__OnAuthenticatedWrap|1), new Action<ArraySegment<byte>, KcpChannel>(CS$<>8__locals1.<CreateConnection>g__OnDataWrap|2), new Action(CS$<>8__locals1.<CreateConnection>g__OnDisconnectedWrap|3), new Action<ErrorCode, string>(CS$<>8__locals1.<CreateConnection>g__OnErrorWrap|4), this.config, cookie);
			CS$<>8__locals1.connection.peer = peer;
			return CS$<>8__locals1.connection;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003748 File Offset: 0x00001948
		private void ProcessMessage(ArraySegment<byte> segment, int connectionId)
		{
			KcpServerConnection kcpServerConnection;
			if (!this.connections.TryGetValue(connectionId, out kcpServerConnection))
			{
				kcpServerConnection = this.CreateConnection(connectionId);
				kcpServerConnection.peer.RawInput(segment);
				kcpServerConnection.peer.TickIncoming();
				return;
			}
			kcpServerConnection.peer.RawInput(segment);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003794 File Offset: 0x00001994
		public virtual void TickIncoming()
		{
			ArraySegment<byte> segment;
			int connectionId;
			while (this.RawReceiveFrom(out segment, out connectionId))
			{
				this.ProcessMessage(segment, connectionId);
			}
			foreach (KcpServerConnection kcpServerConnection in this.connections.Values)
			{
				kcpServerConnection.peer.TickIncoming();
			}
			foreach (int key in this.connectionsToRemove)
			{
				this.connections.Remove(key);
			}
			this.connectionsToRemove.Clear();
		}

		// Token: 0x06000043 RID: 67 RVA: 0x0000385C File Offset: 0x00001A5C
		public virtual void TickOutgoing()
		{
			foreach (KcpServerConnection kcpServerConnection in this.connections.Values)
			{
				kcpServerConnection.peer.TickOutgoing();
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000038B8 File Offset: 0x00001AB8
		public virtual void Tick()
		{
			this.TickIncoming();
			this.TickOutgoing();
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000038C6 File Offset: 0x00001AC6
		public virtual void Stop()
		{
			this.connections.Clear();
			Socket socket = this.socket;
			if (socket != null)
			{
				socket.Close();
			}
			this.socket = null;
		}

		// Token: 0x04000045 RID: 69
		protected readonly Action<int> OnConnected;

		// Token: 0x04000046 RID: 70
		protected readonly Action<int, ArraySegment<byte>, KcpChannel> OnData;

		// Token: 0x04000047 RID: 71
		protected readonly Action<int> OnDisconnected;

		// Token: 0x04000048 RID: 72
		protected readonly Action<int, ErrorCode, string> OnError;

		// Token: 0x04000049 RID: 73
		protected readonly KcpConfig config;

		// Token: 0x0400004A RID: 74
		protected Socket socket;

		// Token: 0x0400004B RID: 75
		private EndPoint newClientEP;

		// Token: 0x0400004C RID: 76
		protected readonly byte[] rawReceiveBuffer;

		// Token: 0x0400004D RID: 77
		public Dictionary<int, KcpServerConnection> connections = new Dictionary<int, KcpServerConnection>();

		// Token: 0x0400004E RID: 78
		private readonly HashSet<int> connectionsToRemove = new HashSet<int>();
	}
}
