using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Telepathy
{
	// Token: 0x0200000D RID: 13
	public class Server : Common
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000027 RID: 39 RVA: 0x00002DB2 File Offset: 0x00000FB2
		public int ReceivePipeTotalCount
		{
			get
			{
				return this.receivePipe.TotalCount;
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002DC0 File Offset: 0x00000FC0
		public int NextConnectionId()
		{
			int num = Interlocked.Increment(ref this.counter);
			if (num == 2147483647)
			{
				throw new Exception("connection id limit reached: " + num.ToString());
			}
			return num;
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000029 RID: 41 RVA: 0x00002DF9 File Offset: 0x00000FF9
		public bool Active
		{
			get
			{
				return this.listenerThread != null && this.listenerThread.IsAlive;
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002E10 File Offset: 0x00001010
		public Server(int MaxMessageSize) : base(MaxMessageSize)
		{
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002E3C File Offset: 0x0000103C
		private void Listen(int port)
		{
			try
			{
				this.listener = TcpListener.Create(port);
				this.listener.Server.NoDelay = this.NoDelay;
				this.listener.Start();
				Log.Info(string.Format("[Telepathy] Starting server on port {0}", port));
				for (;;)
				{
					TcpClient client = this.listener.AcceptTcpClient();
					client.NoDelay = this.NoDelay;
					client.SendTimeout = this.SendTimeout;
					client.ReceiveTimeout = this.ReceiveTimeout;
					int connectionId = this.NextConnectionId();
					ConnectionState connection = new ConnectionState(client, this.MaxMessageSize);
					this.clients[connectionId] = connection;
					Thread sendThread = new Thread(delegate()
					{
						try
						{
							ThreadFunctions.SendLoop(connectionId, client, connection.sendPipe, connection.sendPending);
						}
						catch (ThreadAbortException)
						{
						}
						catch (Exception ex7)
						{
							Action<string> error2 = Log.Error;
							string str4 = "[Telepathy] Server send thread exception: ";
							Exception ex8 = ex7;
							error2(str4 + ((ex8 != null) ? ex8.ToString() : null));
						}
					});
					sendThread.IsBackground = true;
					sendThread.Start();
					new Thread(delegate()
					{
						try
						{
							ThreadFunctions.ReceiveLoop(connectionId, client, this.MaxMessageSize, this.receivePipe, this.ReceiveQueueLimit);
							sendThread.Interrupt();
						}
						catch (Exception ex7)
						{
							Action<string> error2 = Log.Error;
							string str4 = "[Telepathy] Server client thread exception: ";
							Exception ex8 = ex7;
							error2(str4 + ((ex8 != null) ? ex8.ToString() : null));
						}
					})
					{
						IsBackground = true
					}.Start();
				}
			}
			catch (ThreadAbortException ex)
			{
				Action<string> info = Log.Info;
				string str = "[Telepathy] Server thread aborted. That's okay. ";
				ThreadAbortException ex2 = ex;
				info(str + ((ex2 != null) ? ex2.ToString() : null));
			}
			catch (SocketException ex3)
			{
				Action<string> info2 = Log.Info;
				string str2 = "[Telepathy] Server Thread stopped. That's okay. ";
				SocketException ex4 = ex3;
				info2(str2 + ((ex4 != null) ? ex4.ToString() : null));
			}
			catch (Exception ex5)
			{
				Action<string> error = Log.Error;
				string str3 = "[Telepathy] Server Exception: ";
				Exception ex6 = ex5;
				error(str3 + ((ex6 != null) ? ex6.ToString() : null));
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00003024 File Offset: 0x00001224
		public bool Start(int port)
		{
			if (this.Active)
			{
				return false;
			}
			this.receivePipe = new MagnificentReceivePipe(this.MaxMessageSize);
			Log.Info(string.Format("[Telepathy] Starting server on port {0}", port));
			this.listenerThread = new Thread(delegate()
			{
				this.Listen(port);
			});
			this.listenerThread.IsBackground = true;
			this.listenerThread.Priority = ThreadPriority.BelowNormal;
			this.listenerThread.Start();
			return true;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000030BC File Offset: 0x000012BC
		public void Stop()
		{
			if (!this.Active)
			{
				return;
			}
			Log.Info("[Telepathy] Server: stopping...");
			TcpListener tcpListener = this.listener;
			if (tcpListener != null)
			{
				tcpListener.Stop();
			}
			Thread thread = this.listenerThread;
			if (thread != null)
			{
				thread.Interrupt();
			}
			this.listenerThread = null;
			foreach (KeyValuePair<int, ConnectionState> keyValuePair in this.clients)
			{
				TcpClient client = keyValuePair.Value.client;
				try
				{
					client.GetStream().Close();
				}
				catch
				{
				}
				client.Close();
			}
			this.clients.Clear();
			this.counter = 0;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00003184 File Offset: 0x00001384
		public bool Send(int connectionId, ArraySegment<byte> message)
		{
			if (message.Count > this.MaxMessageSize)
			{
				Log.Error("[Telepathy] Server.Send: message too big: " + message.Count.ToString() + ". Limit: " + this.MaxMessageSize.ToString());
				return false;
			}
			ConnectionState connectionState;
			if (!this.clients.TryGetValue(connectionId, out connectionState))
			{
				return false;
			}
			if (connectionState.sendPipe.Count < this.SendQueueLimit)
			{
				connectionState.sendPipe.Enqueue(message);
				connectionState.sendPending.Set();
				return true;
			}
			Log.Warning(string.Format("[Telepathy] Server.Send: sendPipe for connection {0} reached limit of {1}. This can happen if we call send faster than the network can process messages. Disconnecting this connection for load balancing.", connectionId, this.SendQueueLimit));
			connectionState.client.Close();
			return false;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00003248 File Offset: 0x00001448
		public string GetClientAddress(int connectionId)
		{
			ConnectionState connectionState;
			if (this.clients.TryGetValue(connectionId, out connectionState))
			{
				return ((IPEndPoint)connectionState.client.Client.RemoteEndPoint).Address.ToString();
			}
			return "";
		}

		// Token: 0x06000030 RID: 48 RVA: 0x0000328C File Offset: 0x0000148C
		public bool Disconnect(int connectionId)
		{
			ConnectionState connectionState;
			if (this.clients.TryGetValue(connectionId, out connectionState))
			{
				connectionState.client.Close();
				Log.Info("[Telepathy] Server.Disconnect connectionId:" + connectionId.ToString());
				return true;
			}
			return false;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000032D4 File Offset: 0x000014D4
		public int Tick(int processLimit, Func<bool> checkEnabled = null)
		{
			if (this.receivePipe == null)
			{
				return 0;
			}
			int num = 0;
			int num2;
			EventType eventType;
			ArraySegment<byte> arg;
			while (num < processLimit && (checkEnabled == null || checkEnabled()) && this.receivePipe.TryPeek(out num2, out eventType, out arg))
			{
				switch (eventType)
				{
				case EventType.Connected:
				{
					Action<int> onConnected = this.OnConnected;
					if (onConnected != null)
					{
						onConnected(num2);
					}
					break;
				}
				case EventType.Data:
				{
					Action<int, ArraySegment<byte>> onData = this.OnData;
					if (onData != null)
					{
						onData(num2, arg);
					}
					break;
				}
				case EventType.Disconnected:
				{
					Action<int> onDisconnected = this.OnDisconnected;
					if (onDisconnected != null)
					{
						onDisconnected(num2);
					}
					ConnectionState connectionState;
					this.clients.TryRemove(num2, out connectionState);
					break;
				}
				}
				this.receivePipe.TryDequeue();
				num++;
			}
			return this.receivePipe.TotalCount;
		}

		// Token: 0x0400001F RID: 31
		public Action<int> OnConnected;

		// Token: 0x04000020 RID: 32
		public Action<int, ArraySegment<byte>> OnData;

		// Token: 0x04000021 RID: 33
		public Action<int> OnDisconnected;

		// Token: 0x04000022 RID: 34
		public TcpListener listener;

		// Token: 0x04000023 RID: 35
		private Thread listenerThread;

		// Token: 0x04000024 RID: 36
		public int SendQueueLimit = 10000;

		// Token: 0x04000025 RID: 37
		public int ReceiveQueueLimit = 10000;

		// Token: 0x04000026 RID: 38
		protected MagnificentReceivePipe receivePipe;

		// Token: 0x04000027 RID: 39
		private readonly ConcurrentDictionary<int, ConnectionState> clients = new ConcurrentDictionary<int, ConnectionState>();

		// Token: 0x04000028 RID: 40
		private int counter;
	}
}
