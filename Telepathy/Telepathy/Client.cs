using System;
using System.Net.Sockets;
using System.Threading;

namespace Telepathy
{
	// Token: 0x02000004 RID: 4
	public class Client : Common
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000006 RID: 6 RVA: 0x0000213C File Offset: 0x0000033C
		public bool Connected
		{
			get
			{
				return this.state != null && this.state.Connected;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002153 File Offset: 0x00000353
		public bool Connecting
		{
			get
			{
				return this.state != null && this.state.Connecting;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000008 RID: 8 RVA: 0x0000216C File Offset: 0x0000036C
		public int ReceivePipeCount
		{
			get
			{
				if (this.state == null)
				{
					return 0;
				}
				return this.state.receivePipe.TotalCount;
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002188 File Offset: 0x00000388
		public Client(int MaxMessageSize) : base(MaxMessageSize)
		{
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021A8 File Offset: 0x000003A8
		private static void ReceiveThreadFunction(ClientConnectionState state, string ip, int port, int MaxMessageSize, bool NoDelay, int SendTimeout, int ReceiveTimeout, int ReceiveQueueLimit)
		{
			Thread thread = null;
			try
			{
				state.client.Connect(ip, port);
				state.Connecting = false;
				state.client.NoDelay = NoDelay;
				state.client.SendTimeout = SendTimeout;
				state.client.ReceiveTimeout = ReceiveTimeout;
				thread = new Thread(delegate()
				{
					ThreadFunctions.SendLoop(0, state.client, state.sendPipe, state.sendPending);
				});
				thread.IsBackground = true;
				thread.Start();
				ThreadFunctions.ReceiveLoop(0, state.client, MaxMessageSize, state.receivePipe, ReceiveQueueLimit);
			}
			catch (SocketException ex)
			{
				Action<string> info = Log.Info;
				string[] array = new string[6];
				array[0] = "[Telepathy] Client Recv: failed to connect to ip=";
				array[1] = ip;
				array[2] = " port=";
				array[3] = port.ToString();
				array[4] = " reason=";
				int num = 5;
				SocketException ex2 = ex;
				array[num] = ((ex2 != null) ? ex2.ToString() : null);
				info(string.Concat(array));
			}
			catch (ThreadInterruptedException)
			{
			}
			catch (ThreadAbortException)
			{
			}
			catch (ObjectDisposedException)
			{
			}
			catch (Exception ex3)
			{
				Action<string> error = Log.Error;
				string str = "[Telepathy] Client Recv Exception: ";
				Exception ex4 = ex3;
				error(str + ((ex4 != null) ? ex4.ToString() : null));
			}
			state.receivePipe.Enqueue(0, EventType.Disconnected, default(ArraySegment<byte>));
			if (thread != null)
			{
				thread.Interrupt();
			}
			state.Connecting = false;
			TcpClient client = state.client;
			if (client == null)
			{
				return;
			}
			client.Close();
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002360 File Offset: 0x00000560
		public void Connect(string ip, int port)
		{
			if (this.Connecting || this.Connected)
			{
				Log.Warning("[Telepathy] Client can not create connection because an existing connection is connecting or connected");
				return;
			}
			this.state = new ClientConnectionState(this.MaxMessageSize);
			this.state.Connecting = true;
			this.state.client.Client = null;
			this.state.receiveThread = new Thread(delegate()
			{
				Client.ReceiveThreadFunction(this.state, ip, port, this.MaxMessageSize, this.NoDelay, this.SendTimeout, this.ReceiveTimeout, this.ReceiveQueueLimit);
			});
			this.state.receiveThread.IsBackground = true;
			this.state.receiveThread.Start();
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002415 File Offset: 0x00000615
		public void Disconnect()
		{
			if (this.Connecting || this.Connected)
			{
				this.state.Dispose();
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002434 File Offset: 0x00000634
		public bool Send(ArraySegment<byte> message)
		{
			if (!this.Connected)
			{
				Log.Warning("[Telepathy] Client.Send: not connected!");
				return false;
			}
			if (message.Count > this.MaxMessageSize)
			{
				Log.Error("[Telepathy] Client.Send: message too big: " + message.Count.ToString() + ". Limit: " + this.MaxMessageSize.ToString());
				return false;
			}
			if (this.state.sendPipe.Count < this.SendQueueLimit)
			{
				this.state.sendPipe.Enqueue(message);
				this.state.sendPending.Set();
				return true;
			}
			Log.Warning(string.Format("[Telepathy] Client.Send: sendPipe reached limit of {0}. This can happen if we call send faster than the network can process messages. Disconnecting to avoid ever growing memory & latency.", this.SendQueueLimit));
			this.state.client.Close();
			return false;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002510 File Offset: 0x00000710
		public int Tick(int processLimit, Func<bool> checkEnabled = null)
		{
			if (this.state == null)
			{
				return 0;
			}
			int num = 0;
			int num2;
			EventType eventType;
			ArraySegment<byte> obj;
			while (num < processLimit && (checkEnabled == null || checkEnabled()) && this.state.receivePipe.TryPeek(out num2, out eventType, out obj))
			{
				switch (eventType)
				{
				case EventType.Connected:
				{
					Action onConnected = this.OnConnected;
					if (onConnected != null)
					{
						onConnected();
					}
					break;
				}
				case EventType.Data:
				{
					Action<ArraySegment<byte>> onData = this.OnData;
					if (onData != null)
					{
						onData(obj);
					}
					break;
				}
				case EventType.Disconnected:
				{
					Action onDisconnected = this.OnDisconnected;
					if (onDisconnected != null)
					{
						onDisconnected();
					}
					break;
				}
				}
				this.state.receivePipe.TryDequeue();
				num++;
			}
			return this.state.receivePipe.TotalCount;
		}

		// Token: 0x04000004 RID: 4
		public Action OnConnected;

		// Token: 0x04000005 RID: 5
		public Action<ArraySegment<byte>> OnData;

		// Token: 0x04000006 RID: 6
		public Action OnDisconnected;

		// Token: 0x04000007 RID: 7
		public int SendQueueLimit = 10000;

		// Token: 0x04000008 RID: 8
		public int ReceiveQueueLimit = 10000;

		// Token: 0x04000009 RID: 9
		private ClientConnectionState state;
	}
}
