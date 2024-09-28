using System;
using System.Collections.Generic;
using System.Linq;
using Mirror.RemoteCalls;
using UnityEngine;

namespace Mirror
{
	// Token: 0x0200005B RID: 91
	public static class NetworkServer
	{
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600025C RID: 604 RVA: 0x00009987 File Offset: 0x00007B87
		public static float tickInterval
		{
			get
			{
				if (NetworkServer.tickRate >= 2147483647)
				{
					return 0f;
				}
				return 1f / (float)NetworkServer.tickRate;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600025D RID: 605 RVA: 0x000099A7 File Offset: 0x00007BA7
		public static int sendRate
		{
			get
			{
				return NetworkServer.tickRate;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600025E RID: 606 RVA: 0x000099AE File Offset: 0x00007BAE
		public static float sendInterval
		{
			get
			{
				if (NetworkServer.sendRate >= 2147483647)
				{
					return 0f;
				}
				return 1f / (float)NetworkServer.sendRate;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600025F RID: 607 RVA: 0x000099CE File Offset: 0x00007BCE
		// (set) Token: 0x06000260 RID: 608 RVA: 0x000099D5 File Offset: 0x00007BD5
		public static LocalConnectionToClient localConnection { get; private set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000261 RID: 609 RVA: 0x000099DD File Offset: 0x00007BDD
		[Obsolete("NetworkServer.localClientActive was renamed to .activeHost to be more obvious")]
		public static bool localClientActive
		{
			get
			{
				return NetworkServer.activeHost;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000262 RID: 610 RVA: 0x000099E4 File Offset: 0x00007BE4
		// (set) Token: 0x06000263 RID: 611 RVA: 0x000099EB File Offset: 0x00007BEB
		public static bool active { get; internal set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000264 RID: 612 RVA: 0x000099F3 File Offset: 0x00007BF3
		public static bool activeHost
		{
			get
			{
				return NetworkServer.localConnection != null;
			}
		}

		// Token: 0x06000265 RID: 613 RVA: 0x000099FD File Offset: 0x00007BFD
		public static void Listen(int maxConns)
		{
			NetworkServer.Initialize();
			NetworkServer.maxConnections = maxConns;
			if (!NetworkServer.dontListen)
			{
				Transport.active.ServerStart();
			}
			NetworkServer.active = true;
			NetworkServer.RegisterMessageHandlers();
		}

		// Token: 0x06000266 RID: 614 RVA: 0x00009A28 File Offset: 0x00007C28
		private static void Initialize()
		{
			if (NetworkServer.initialized)
			{
				return;
			}
			if (!WeaverFuse.Weaved())
			{
				throw new Exception("NetworkServer won't start because Weaving failed or didn't run.");
			}
			NetworkServer.connections.Clear();
			if (NetworkServer.aoi != null)
			{
				NetworkServer.aoi.Reset();
			}
			NetworkTime.ResetStatics();
			NetworkServer.AddTransportHandlers();
			NetworkServer.initialized = true;
			NetworkServer.earlyUpdateDuration = new TimeSample(NetworkServer.sendRate);
			NetworkServer.lateUpdateDuration = new TimeSample(NetworkServer.sendRate);
			NetworkServer.fullUpdateDuration = new TimeSample(NetworkServer.sendRate);
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00009AB0 File Offset: 0x00007CB0
		private static void AddTransportHandlers()
		{
			Transport active = Transport.active;
			active.OnServerConnected = (Action<int>)Delegate.Combine(active.OnServerConnected, new Action<int>(NetworkServer.OnTransportConnected));
			Transport active2 = Transport.active;
			active2.OnServerDataReceived = (Action<int, ArraySegment<byte>, int>)Delegate.Combine(active2.OnServerDataReceived, new Action<int, ArraySegment<byte>, int>(NetworkServer.OnTransportData));
			Transport active3 = Transport.active;
			active3.OnServerDisconnected = (Action<int>)Delegate.Combine(active3.OnServerDisconnected, new Action<int>(NetworkServer.OnTransportDisconnected));
			Transport active4 = Transport.active;
			active4.OnServerError = (Action<int, TransportError, string>)Delegate.Combine(active4.OnServerError, new Action<int, TransportError, string>(NetworkServer.OnTransportError));
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00009B58 File Offset: 0x00007D58
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void Shutdown()
		{
			if (NetworkServer.initialized)
			{
				NetworkServer.DisconnectAll();
				Transport.active.ServerStop();
				NetworkServer.RemoveTransportHandlers();
				NetworkServer.initialized = false;
			}
			NetworkServer.dontListen = false;
			NetworkServer.isLoadingScene = false;
			NetworkServer.lastSendTime = 0.0;
			NetworkServer.actualTickRate = 0;
			NetworkServer.localConnection = null;
			NetworkServer.connections.Clear();
			NetworkServer.connectionsCopy.Clear();
			NetworkServer.handlers.Clear();
			NetworkServer.CleanupSpawned();
			NetworkServer.active = false;
			NetworkIdentity.ResetStatics();
			NetworkServer.OnConnectedEvent = null;
			NetworkServer.OnDisconnectedEvent = null;
			NetworkServer.OnErrorEvent = null;
			if (NetworkServer.aoi != null)
			{
				NetworkServer.aoi.Reset();
			}
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00009C04 File Offset: 0x00007E04
		private static void RemoveTransportHandlers()
		{
			Transport active = Transport.active;
			active.OnServerConnected = (Action<int>)Delegate.Remove(active.OnServerConnected, new Action<int>(NetworkServer.OnTransportConnected));
			Transport active2 = Transport.active;
			active2.OnServerDataReceived = (Action<int, ArraySegment<byte>, int>)Delegate.Remove(active2.OnServerDataReceived, new Action<int, ArraySegment<byte>, int>(NetworkServer.OnTransportData));
			Transport active3 = Transport.active;
			active3.OnServerDisconnected = (Action<int>)Delegate.Remove(active3.OnServerDisconnected, new Action<int>(NetworkServer.OnTransportDisconnected));
			Transport active4 = Transport.active;
			active4.OnServerError = (Action<int, TransportError, string>)Delegate.Remove(active4.OnServerError, new Action<int, TransportError, string>(NetworkServer.OnTransportError));
		}

		// Token: 0x0600026A RID: 618 RVA: 0x00009CAC File Offset: 0x00007EAC
		private static void CleanupSpawned()
		{
			foreach (NetworkIdentity networkIdentity in NetworkServer.spawned.Values.ToList<NetworkIdentity>())
			{
				if (networkIdentity != null)
				{
					if (networkIdentity.sceneId != 0UL)
					{
						NetworkServer.DestroyObject(networkIdentity, NetworkServer.DestroyMode.Reset);
						networkIdentity.gameObject.SetActive(false);
					}
					else
					{
						NetworkServer.DestroyObject(networkIdentity, NetworkServer.DestroyMode.Destroy);
					}
				}
			}
			NetworkServer.spawned.Clear();
		}

		// Token: 0x0600026B RID: 619 RVA: 0x00009D38 File Offset: 0x00007F38
		internal static void RegisterMessageHandlers()
		{
			NetworkServer.RegisterHandler<ReadyMessage>(new Action<NetworkConnectionToClient, ReadyMessage>(NetworkServer.OnClientReadyMessage), true);
			NetworkServer.RegisterHandler<CommandMessage>(new Action<NetworkConnectionToClient, CommandMessage, int>(NetworkServer.OnCommandMessage), true);
			NetworkServer.RegisterHandler<NetworkPingMessage>(new Action<NetworkConnectionToClient, NetworkPingMessage>(NetworkTime.OnServerPing), false);
			NetworkServer.RegisterHandler<NetworkPongMessage>(new Action<NetworkConnectionToClient, NetworkPongMessage>(NetworkTime.OnServerPong), false);
			NetworkServer.RegisterHandler<EntityStateMessage>(new Action<NetworkConnectionToClient, EntityStateMessage>(NetworkServer.OnEntityStateMessage), true);
			NetworkServer.RegisterHandler<TimeSnapshotMessage>(new Action<NetworkConnectionToClient, TimeSnapshotMessage>(NetworkServer.OnTimeSnapshotMessage), true);
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00009DB1 File Offset: 0x00007FB1
		private static void OnClientReadyMessage(NetworkConnectionToClient conn, ReadyMessage msg)
		{
			NetworkServer.SetClientReady(conn);
		}

		// Token: 0x0600026D RID: 621 RVA: 0x00009DBC File Offset: 0x00007FBC
		private static void OnCommandMessage(NetworkConnectionToClient conn, CommandMessage msg, int channelId)
		{
			if (!conn.isReady)
			{
				if (channelId == 0)
				{
					Debug.LogWarning("Command received while client is not ready.\nThis may be ignored if client intentionally set NotReady.");
				}
				return;
			}
			NetworkIdentity networkIdentity;
			if (!NetworkServer.spawned.TryGetValue(msg.netId, out networkIdentity))
			{
				if (channelId == 0)
				{
					Debug.LogWarning(string.Format("Spawned object not found when handling Command message [netId={0}]", msg.netId));
				}
				return;
			}
			if (RemoteProcedureCalls.CommandRequiresAuthority(msg.functionHash) && networkIdentity.connectionToClient != conn)
			{
				Debug.LogWarning(string.Format("Command for object without authority [netId={0}]", msg.netId));
				return;
			}
			using (NetworkReaderPooled networkReaderPooled = NetworkReaderPool.Get(msg.payload))
			{
				networkIdentity.HandleRemoteCall(msg.componentIndex, msg.functionHash, RemoteCallType.Command, networkReaderPooled, conn);
			}
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00009E80 File Offset: 0x00008080
		private static void OnEntityStateMessage(NetworkConnectionToClient connection, EntityStateMessage message)
		{
			NetworkIdentity networkIdentity;
			if (NetworkServer.spawned.TryGetValue(message.netId, out networkIdentity) && networkIdentity != null)
			{
				if (networkIdentity.connectionToClient == connection)
				{
					using (NetworkReaderPooled networkReaderPooled = NetworkReaderPool.Get(message.payload))
					{
						if (!networkIdentity.DeserializeServer(networkReaderPooled))
						{
							Debug.LogWarning(string.Format("Server failed to deserialize client state for {0} with netId={1}, Disconnecting.", networkIdentity.name, networkIdentity.netId));
							connection.Disconnect();
						}
						return;
					}
				}
				Debug.LogWarning(string.Format("EntityStateMessage from {0} for {1} without authority.", connection, networkIdentity));
			}
		}

		// Token: 0x0600026F RID: 623 RVA: 0x00009F1C File Offset: 0x0000811C
		private static void OnTimeSnapshotMessage(NetworkConnectionToClient connection, TimeSnapshotMessage _)
		{
			connection.OnTimeSnapshot(new TimeSnapshot(connection.remoteTimeStamp, NetworkTime.localTime));
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00009F34 File Offset: 0x00008134
		public static bool AddConnection(NetworkConnectionToClient conn)
		{
			if (!NetworkServer.connections.ContainsKey(conn.connectionId))
			{
				NetworkServer.connections[conn.connectionId] = conn;
				return true;
			}
			return false;
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00009F5C File Offset: 0x0000815C
		public static bool RemoveConnection(int connectionId)
		{
			return NetworkServer.connections.Remove(connectionId);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00009F69 File Offset: 0x00008169
		internal static void SetLocalConnection(LocalConnectionToClient conn)
		{
			if (NetworkServer.localConnection != null)
			{
				Debug.LogError("Local Connection already exists");
				return;
			}
			NetworkServer.localConnection = conn;
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00009F83 File Offset: 0x00008183
		internal static void RemoveLocalConnection()
		{
			if (NetworkServer.localConnection != null)
			{
				NetworkServer.localConnection.Disconnect();
				NetworkServer.localConnection = null;
			}
			NetworkServer.RemoveConnection(0);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00009FA3 File Offset: 0x000081A3
		public static bool HasExternalConnections()
		{
			return NetworkServer.connections.Count > 0 && (NetworkServer.connections.Count != 1 || NetworkServer.localConnection == null);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00009FCC File Offset: 0x000081CC
		public static void SendToAll<T>(T message, int channelId = 0, bool sendToReadyOnly = false) where T : struct, NetworkMessage
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("Can not send using NetworkServer.SendToAll<T>(T msg) because NetworkServer is not active");
				return;
			}
			using (NetworkWriterPooled networkWriterPooled = NetworkWriterPool.Get())
			{
				NetworkMessages.Pack<T>(message, networkWriterPooled);
				ArraySegment<byte> segment = networkWriterPooled.ToArraySegment();
				int num = 0;
				foreach (NetworkConnectionToClient networkConnectionToClient in NetworkServer.connections.Values)
				{
					if (!sendToReadyOnly || networkConnectionToClient.isReady)
					{
						num++;
						networkConnectionToClient.Send(segment, channelId);
					}
				}
				NetworkDiagnostics.OnSend<T>(message, channelId, segment.Count, num);
			}
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000A088 File Offset: 0x00008288
		public static void SendToReady<T>(T message, int channelId = 0) where T : struct, NetworkMessage
		{
			if (!NetworkServer.active)
			{
				Debug.LogWarning("Can not send using NetworkServer.SendToReady<T>(T msg) because NetworkServer is not active");
				return;
			}
			NetworkServer.SendToAll<T>(message, channelId, true);
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000A0A4 File Offset: 0x000082A4
		private static void SendToObservers<T>(NetworkIdentity identity, T message, int channelId = 0) where T : struct, NetworkMessage
		{
			if (identity == null || identity.observers.Count == 0)
			{
				return;
			}
			using (NetworkWriterPooled networkWriterPooled = NetworkWriterPool.Get())
			{
				NetworkMessages.Pack<T>(message, networkWriterPooled);
				ArraySegment<byte> segment = networkWriterPooled.ToArraySegment();
				foreach (NetworkConnectionToClient networkConnectionToClient in identity.observers.Values)
				{
					networkConnectionToClient.Send(segment, channelId);
				}
				NetworkDiagnostics.OnSend<T>(message, channelId, segment.Count, identity.observers.Count);
			}
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000A158 File Offset: 0x00008358
		public static void SendToReadyObservers<T>(NetworkIdentity identity, T message, bool includeOwner = true, int channelId = 0) where T : struct, NetworkMessage
		{
			if (identity == null || identity.observers.Count == 0)
			{
				return;
			}
			using (NetworkWriterPooled networkWriterPooled = NetworkWriterPool.Get())
			{
				NetworkMessages.Pack<T>(message, networkWriterPooled);
				ArraySegment<byte> segment = networkWriterPooled.ToArraySegment();
				int num = 0;
				foreach (NetworkConnectionToClient networkConnectionToClient in identity.observers.Values)
				{
					if ((networkConnectionToClient != identity.connectionToClient || includeOwner) && networkConnectionToClient.isReady)
					{
						num++;
						networkConnectionToClient.Send(segment, channelId);
					}
				}
				NetworkDiagnostics.OnSend<T>(message, channelId, segment.Count, num);
			}
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000A228 File Offset: 0x00008428
		public static void SendToReadyObservers<T>(NetworkIdentity identity, T message, int channelId) where T : struct, NetworkMessage
		{
			NetworkServer.SendToReadyObservers<T>(identity, message, true, channelId);
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000A234 File Offset: 0x00008434
		private static void OnTransportConnected(int connectionId)
		{
			if (connectionId == 0)
			{
				Debug.LogError(string.Format("Server.HandleConnect: invalid connectionId: {0} . Needs to be != 0, because 0 is reserved for local player.", connectionId));
				Transport.active.ServerDisconnect(connectionId);
				return;
			}
			if (NetworkServer.connections.ContainsKey(connectionId))
			{
				Transport.active.ServerDisconnect(connectionId);
				return;
			}
			if (NetworkServer.connections.Count < NetworkServer.maxConnections)
			{
				NetworkServer.OnConnected(new NetworkConnectionToClient(connectionId));
				return;
			}
			Transport.active.ServerDisconnect(connectionId);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000A2A6 File Offset: 0x000084A6
		internal static void OnConnected(NetworkConnectionToClient conn)
		{
			NetworkServer.AddConnection(conn);
			Action<NetworkConnectionToClient> onConnectedEvent = NetworkServer.OnConnectedEvent;
			if (onConnectedEvent == null)
			{
				return;
			}
			onConnectedEvent(conn);
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000A2C0 File Offset: 0x000084C0
		private static bool UnpackAndInvoke(NetworkConnectionToClient connection, NetworkReader reader, int channelId)
		{
			ushort num;
			if (!NetworkMessages.UnpackId(reader, out num))
			{
				Debug.LogWarning(string.Format("Invalid message header for connection: {0}.", connection));
				return false;
			}
			NetworkMessageDelegate networkMessageDelegate;
			if (NetworkServer.handlers.TryGetValue(num, out networkMessageDelegate))
			{
				networkMessageDelegate(connection, reader, channelId);
				connection.lastMessageTime = Time.time;
				return true;
			}
			Debug.LogWarning(string.Format("Unknown message id: {0} for connection: {1}. This can happen if no handler was registered for this message.", num, connection));
			return false;
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000A328 File Offset: 0x00008528
		internal static void OnTransportData(int connectionId, ArraySegment<byte> data, int channelId)
		{
			NetworkConnectionToClient networkConnectionToClient;
			if (NetworkServer.connections.TryGetValue(connectionId, out networkConnectionToClient))
			{
				if (!networkConnectionToClient.unbatcher.AddBatch(data))
				{
					Debug.LogWarning("NetworkServer: received Message was too short (messages should start with message id)");
					networkConnectionToClient.Disconnect();
					return;
				}
				ArraySegment<byte> segment;
				double remoteTimeStamp;
				while (!NetworkServer.isLoadingScene && networkConnectionToClient.unbatcher.GetNextMessage(out segment, out remoteTimeStamp))
				{
					using (NetworkReaderPooled networkReaderPooled = NetworkReaderPool.Get(segment))
					{
						if (networkReaderPooled.Remaining < 2)
						{
							Debug.LogWarning(string.Format("NetworkServer: received Message was too short (messages should start with message id). Disconnecting {0}", connectionId));
							networkConnectionToClient.Disconnect();
							return;
						}
						networkConnectionToClient.remoteTimeStamp = remoteTimeStamp;
						if (!NetworkServer.UnpackAndInvoke(networkConnectionToClient, networkReaderPooled, channelId))
						{
							Debug.LogWarning(string.Format("NetworkServer: failed to unpack and invoke message. Disconnecting {0}.", connectionId));
							networkConnectionToClient.Disconnect();
							return;
						}
						continue;
					}
					break;
				}
				if (!NetworkServer.isLoadingScene && networkConnectionToClient.unbatcher.BatchesCount > 0)
				{
					Debug.LogError(string.Format("Still had {0} batches remaining after processing, even though processing was not interrupted by a scene change. This should never happen, as it would cause ever growing batches.\nPossible reasons:\n* A message didn't deserialize as much as it serialized\n*There was no message handler for a message id, so the reader wasn't read until the end.", networkConnectionToClient.unbatcher.BatchesCount));
					return;
				}
			}
			else
			{
				Debug.LogError(string.Format("HandleData Unknown connectionId:{0}", connectionId));
			}
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000A440 File Offset: 0x00008640
		internal static void OnTransportDisconnected(int connectionId)
		{
			NetworkConnectionToClient networkConnectionToClient;
			if (NetworkServer.connections.TryGetValue(connectionId, out networkConnectionToClient))
			{
				NetworkServer.RemoveConnection(connectionId);
				if (NetworkServer.OnDisconnectedEvent != null)
				{
					NetworkServer.OnDisconnectedEvent(networkConnectionToClient);
					return;
				}
				NetworkServer.DestroyPlayerForConnection(networkConnectionToClient);
			}
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000A47C File Offset: 0x0000867C
		private static void OnTransportError(int connectionId, TransportError error, string reason)
		{
			Debug.LogWarning(string.Format("Server Transport Error for connId={0}: {1}: {2}. This is fine.", connectionId, error, reason));
			NetworkConnectionToClient arg;
			NetworkServer.connections.TryGetValue(connectionId, out arg);
			Action<NetworkConnectionToClient, TransportError, string> onErrorEvent = NetworkServer.OnErrorEvent;
			if (onErrorEvent == null)
			{
				return;
			}
			onErrorEvent(arg, error, reason);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000A4C5 File Offset: 0x000086C5
		public static void DestroyPlayerForConnection(NetworkConnectionToClient conn)
		{
			conn.DestroyOwnedObjects();
			conn.RemoveFromObservingsObservers();
			conn.identity = null;
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000A4DC File Offset: 0x000086DC
		public static void RegisterHandler<T>(Action<NetworkConnectionToClient, T> handler, bool requireAuthentication = true) where T : struct, NetworkMessage
		{
			ushort id = NetworkMessageId<T>.Id;
			if (NetworkServer.handlers.ContainsKey(id))
			{
				Debug.LogWarning(string.Format("NetworkServer.RegisterHandler replacing handler for {0}, id={1}. If replacement is intentional, use ReplaceHandler instead to avoid this warning.", typeof(T).FullName, id));
			}
			NetworkMessages.Lookup[id] = typeof(T);
			NetworkServer.handlers[id] = NetworkMessages.WrapHandler<T, NetworkConnectionToClient>(handler, requireAuthentication);
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000A548 File Offset: 0x00008748
		public static void RegisterHandler<T>(Action<NetworkConnectionToClient, T, int> handler, bool requireAuthentication = true) where T : struct, NetworkMessage
		{
			ushort id = NetworkMessageId<T>.Id;
			if (NetworkServer.handlers.ContainsKey(id))
			{
				Debug.LogWarning(string.Format("NetworkServer.RegisterHandler replacing handler for {0}, id={1}. If replacement is intentional, use ReplaceHandler instead to avoid this warning.", typeof(T).FullName, id));
			}
			NetworkMessages.Lookup[id] = typeof(T);
			NetworkServer.handlers[id] = NetworkMessages.WrapHandler<T, NetworkConnectionToClient>(handler, requireAuthentication);
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000A5B3 File Offset: 0x000087B3
		public static void ReplaceHandler<T>(Action<T> handler, bool requireAuthentication = true) where T : struct, NetworkMessage
		{
			NetworkServer.ReplaceHandler<T>(delegate(NetworkConnectionToClient _, T value)
			{
				handler(value);
			}, requireAuthentication);
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000A5D4 File Offset: 0x000087D4
		public static void ReplaceHandler<T>(Action<NetworkConnectionToClient, T> handler, bool requireAuthentication = true) where T : struct, NetworkMessage
		{
			ushort id = NetworkMessageId<T>.Id;
			NetworkServer.handlers[id] = NetworkMessages.WrapHandler<T, NetworkConnectionToClient>(handler, requireAuthentication);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000A5FC File Offset: 0x000087FC
		public static void UnregisterHandler<T>() where T : struct, NetworkMessage
		{
			ushort id = NetworkMessageId<T>.Id;
			NetworkServer.handlers.Remove(id);
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000A61B File Offset: 0x0000881B
		public static void ClearHandlers()
		{
			NetworkServer.handlers.Clear();
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000A627 File Offset: 0x00008827
		internal static bool GetNetworkIdentity(GameObject go, out NetworkIdentity identity)
		{
			if (!go.TryGetComponent<NetworkIdentity>(out identity))
			{
				Debug.LogError("GameObject " + go.name + " doesn't have NetworkIdentity.");
				return false;
			}
			return true;
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000A650 File Offset: 0x00008850
		public static void DisconnectAll()
		{
			foreach (NetworkConnectionToClient networkConnectionToClient in NetworkServer.connections.Values.ToList<NetworkConnectionToClient>())
			{
				networkConnectionToClient.Disconnect();
				if (networkConnectionToClient.connectionId != 0)
				{
					NetworkServer.OnTransportDisconnected(networkConnectionToClient.connectionId);
				}
			}
			NetworkServer.connections.Clear();
			NetworkServer.localConnection = null;
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000A6D0 File Offset: 0x000088D0
		public static bool AddPlayerForConnection(NetworkConnectionToClient conn, GameObject player, uint assetId)
		{
			NetworkIdentity networkIdentity;
			if (NetworkServer.GetNetworkIdentity(player, out networkIdentity))
			{
				networkIdentity.assetId = assetId;
			}
			return NetworkServer.AddPlayerForConnection(conn, player);
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000A6F8 File Offset: 0x000088F8
		public static bool AddPlayerForConnection(NetworkConnectionToClient conn, GameObject player)
		{
			NetworkIdentity networkIdentity;
			if (!player.TryGetComponent<NetworkIdentity>(out networkIdentity))
			{
				Debug.LogWarning(string.Format("AddPlayer: playerGameObject has no NetworkIdentity. Please add a NetworkIdentity to {0}", player));
				return false;
			}
			if (conn.identity != null)
			{
				Debug.Log("AddPlayer: player object already exists");
				return false;
			}
			conn.identity = networkIdentity;
			networkIdentity.SetClientOwner(conn);
			if (conn is LocalConnectionToClient)
			{
				networkIdentity.isOwned = true;
				NetworkClient.InternalAddPlayer(networkIdentity);
			}
			NetworkServer.SetClientReady(conn);
			NetworkServer.Respawn(networkIdentity);
			return true;
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000A76C File Offset: 0x0000896C
		public static bool ReplacePlayerForConnection(NetworkConnectionToClient conn, GameObject player, bool keepAuthority = false)
		{
			NetworkIdentity networkIdentity;
			if (!player.TryGetComponent<NetworkIdentity>(out networkIdentity))
			{
				Debug.LogError(string.Format("ReplacePlayer: playerGameObject has no NetworkIdentity. Please add a NetworkIdentity to {0}", player));
				return false;
			}
			if (networkIdentity.connectionToClient != null && networkIdentity.connectionToClient != conn)
			{
				Debug.LogError(string.Format("Cannot replace player for connection. New player is already owned by a different connection{0}", player));
				return false;
			}
			NetworkIdentity identity = conn.identity;
			conn.identity = networkIdentity;
			networkIdentity.SetClientOwner(conn);
			if (conn is LocalConnectionToClient)
			{
				networkIdentity.isOwned = true;
				NetworkClient.InternalAddPlayer(networkIdentity);
			}
			NetworkServer.SpawnObserversForConnection(conn);
			NetworkServer.Respawn(networkIdentity);
			if (keepAuthority)
			{
				NetworkServer.SendChangeOwnerMessage(identity, conn);
			}
			else
			{
				identity.RemoveClientAuthority();
			}
			return true;
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000A804 File Offset: 0x00008A04
		public static bool ReplacePlayerForConnection(NetworkConnectionToClient conn, GameObject player, uint assetId, bool keepAuthority = false)
		{
			NetworkIdentity networkIdentity;
			if (NetworkServer.GetNetworkIdentity(player, out networkIdentity))
			{
				networkIdentity.assetId = assetId;
			}
			return NetworkServer.ReplacePlayerForConnection(conn, player, keepAuthority);
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000A82A File Offset: 0x00008A2A
		public static void RemovePlayerForConnection(NetworkConnection conn, bool destroyServerObject)
		{
			if (conn.identity != null)
			{
				if (destroyServerObject)
				{
					NetworkServer.Destroy(conn.identity.gameObject);
				}
				else
				{
					NetworkServer.UnSpawn(conn.identity.gameObject);
				}
				conn.identity = null;
			}
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000A866 File Offset: 0x00008A66
		public static void SetClientReady(NetworkConnectionToClient conn)
		{
			conn.isReady = true;
			if (conn.identity != null)
			{
				NetworkServer.SpawnObserversForConnection(conn);
			}
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000A884 File Offset: 0x00008A84
		private static void SpawnObserversForConnection(NetworkConnectionToClient conn)
		{
			if (!conn.isReady)
			{
				return;
			}
			conn.Send<ObjectSpawnStartedMessage>(default(ObjectSpawnStartedMessage), 0);
			foreach (NetworkIdentity networkIdentity in NetworkServer.spawned.Values)
			{
				if (networkIdentity.gameObject.activeSelf)
				{
					if (networkIdentity.visible == Visibility.ForceShown)
					{
						networkIdentity.AddObserver(conn);
					}
					else if (networkIdentity.visible != Visibility.ForceHidden && networkIdentity.visible == Visibility.Default)
					{
						if (NetworkServer.aoi != null)
						{
							if (NetworkServer.aoi.OnCheckObserver(networkIdentity, conn))
							{
								networkIdentity.AddObserver(conn);
							}
						}
						else
						{
							networkIdentity.AddObserver(conn);
						}
					}
				}
			}
			conn.Send<ObjectSpawnFinishedMessage>(default(ObjectSpawnFinishedMessage), 0);
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000A958 File Offset: 0x00008B58
		public static void SetClientNotReady(NetworkConnectionToClient conn)
		{
			conn.isReady = false;
			conn.RemoveFromObservingsObservers();
			conn.Send<NotReadyMessage>(default(NotReadyMessage), 0);
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000A984 File Offset: 0x00008B84
		public static void SetAllClientsNotReady()
		{
			foreach (NetworkConnectionToClient clientNotReady in NetworkServer.connections.Values)
			{
				NetworkServer.SetClientNotReady(clientNotReady);
			}
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000A9D8 File Offset: 0x00008BD8
		internal static void ShowForConnection(NetworkIdentity identity, NetworkConnection conn)
		{
			if (conn.isReady)
			{
				NetworkServer.SendSpawnMessage(identity, conn);
			}
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000A9EC File Offset: 0x00008BEC
		internal static void HideForConnection(NetworkIdentity identity, NetworkConnection conn)
		{
			ObjectHideMessage message = new ObjectHideMessage
			{
				netId = identity.netId
			};
			conn.Send<ObjectHideMessage>(message, 0);
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000AA18 File Offset: 0x00008C18
		internal static void SendSpawnMessage(NetworkIdentity identity, NetworkConnection conn)
		{
			if (identity.serverOnly)
			{
				return;
			}
			using (NetworkWriterPooled networkWriterPooled = NetworkWriterPool.Get())
			{
				using (NetworkWriterPooled networkWriterPooled2 = NetworkWriterPool.Get())
				{
					bool isOwner = identity.connectionToClient == conn;
					ArraySegment<byte> payload = NetworkServer.CreateSpawnMessagePayload(isOwner, identity, networkWriterPooled, networkWriterPooled2);
					SpawnMessage message = new SpawnMessage
					{
						netId = identity.netId,
						isLocalPlayer = (conn.identity == identity),
						isOwner = isOwner,
						sceneId = identity.sceneId,
						assetId = identity.assetId,
						position = identity.transform.localPosition,
						rotation = identity.transform.localRotation,
						scale = identity.transform.localScale,
						payload = payload
					};
					conn.Send<SpawnMessage>(message, 0);
				}
			}
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000AB18 File Offset: 0x00008D18
		private static ArraySegment<byte> CreateSpawnMessagePayload(bool isOwner, NetworkIdentity identity, NetworkWriterPooled ownerWriter, NetworkWriterPooled observersWriter)
		{
			if (identity.NetworkBehaviours.Length == 0)
			{
				return default(ArraySegment<byte>);
			}
			identity.SerializeServer(true, ownerWriter, observersWriter);
			ArraySegment<byte> result = ownerWriter.ToArraySegment();
			ArraySegment<byte> result2 = observersWriter.ToArraySegment();
			if (!isOwner)
			{
				return result2;
			}
			return result;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000AB58 File Offset: 0x00008D58
		internal static void SendChangeOwnerMessage(NetworkIdentity identity, NetworkConnectionToClient conn)
		{
			if (identity.netId == 0U || identity.serverOnly)
			{
				return;
			}
			if (!conn.observing.Contains(identity))
			{
				return;
			}
			conn.Send<ChangeOwnerMessage>(new ChangeOwnerMessage
			{
				netId = identity.netId,
				isOwner = (identity.connectionToClient == conn),
				isLocalPlayer = (conn.identity == identity)
			}, 0);
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000ABC5 File Offset: 0x00008DC5
		private static bool ValidParent(NetworkIdentity identity)
		{
			return identity.transform.parent == null || identity.transform.parent.gameObject.activeInHierarchy;
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000ABF4 File Offset: 0x00008DF4
		public static bool SpawnObjects()
		{
			if (!NetworkServer.active)
			{
				return false;
			}
			NetworkIdentity[] array = Resources.FindObjectsOfTypeAll<NetworkIdentity>();
			foreach (NetworkIdentity networkIdentity in array)
			{
				if (Utils.IsSceneObject(networkIdentity) && networkIdentity.netId == 0U)
				{
					networkIdentity.gameObject.SetActive(true);
				}
			}
			foreach (NetworkIdentity networkIdentity2 in array)
			{
				if (Utils.IsSceneObject(networkIdentity2) && networkIdentity2.netId == 0U && NetworkServer.ValidParent(networkIdentity2))
				{
					NetworkServer.Spawn(networkIdentity2.gameObject, networkIdentity2.connectionToClient);
				}
			}
			return true;
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000AC84 File Offset: 0x00008E84
		public static void Spawn(GameObject obj, GameObject ownerPlayer)
		{
			NetworkIdentity networkIdentity;
			if (!ownerPlayer.TryGetComponent<NetworkIdentity>(out networkIdentity))
			{
				Debug.LogError("Player object has no NetworkIdentity");
				return;
			}
			if (networkIdentity.connectionToClient == null)
			{
				Debug.LogError("Player object is not a player.");
				return;
			}
			NetworkServer.Spawn(obj, networkIdentity.connectionToClient);
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000ACC5 File Offset: 0x00008EC5
		private static void Respawn(NetworkIdentity identity)
		{
			if (identity.netId == 0U)
			{
				NetworkServer.Spawn(identity.gameObject, identity.connectionToClient);
				return;
			}
			NetworkServer.SendSpawnMessage(identity, identity.connectionToClient);
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000ACED File Offset: 0x00008EED
		public static void Spawn(GameObject obj, NetworkConnection ownerConnection = null)
		{
			NetworkServer.SpawnObject(obj, ownerConnection);
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000ACF8 File Offset: 0x00008EF8
		public static void Spawn(GameObject obj, uint assetId, NetworkConnection ownerConnection = null)
		{
			NetworkIdentity networkIdentity;
			if (NetworkServer.GetNetworkIdentity(obj, out networkIdentity))
			{
				networkIdentity.assetId = assetId;
			}
			NetworkServer.SpawnObject(obj, ownerConnection);
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000AD20 File Offset: 0x00008F20
		private static void SpawnObject(GameObject obj, NetworkConnection ownerConnection)
		{
			if (Utils.IsPrefab(obj))
			{
				Debug.LogError("GameObject " + obj.name + " is a prefab, it can't be spawned. Instantiate it first.", obj);
				return;
			}
			if (!NetworkServer.active)
			{
				Debug.LogError(string.Format("SpawnObject for {0}, NetworkServer is not active. Cannot spawn objects without an active server.", obj), obj);
				return;
			}
			NetworkIdentity networkIdentity;
			if (!obj.TryGetComponent<NetworkIdentity>(out networkIdentity))
			{
				Debug.LogError(string.Format("SpawnObject {0} has no NetworkIdentity. Please add a NetworkIdentity to {1}", obj, obj), obj);
				return;
			}
			if (networkIdentity.SpawnedFromInstantiate)
			{
				return;
			}
			if (NetworkServer.spawned.ContainsKey(networkIdentity.netId))
			{
				Debug.LogWarning(string.Format("{0} with netId={1} was already spawned.", networkIdentity, networkIdentity.netId), networkIdentity.gameObject);
				return;
			}
			networkIdentity.connectionToClient = (NetworkConnectionToClient)ownerConnection;
			if (ownerConnection is LocalConnectionToClient)
			{
				networkIdentity.isOwned = true;
			}
			if (!networkIdentity.isServer && networkIdentity.netId == 0U)
			{
				networkIdentity.isLocalPlayer = (NetworkClient.localPlayer == networkIdentity);
				networkIdentity.isClient = NetworkClient.active;
				networkIdentity.isServer = true;
				networkIdentity.netId = NetworkIdentity.GetNextNetworkId();
				NetworkServer.spawned[networkIdentity.netId] = networkIdentity;
				networkIdentity.OnStartServer();
			}
			if (NetworkServer.aoi)
			{
				try
				{
					NetworkServer.aoi.OnSpawned(networkIdentity);
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
			NetworkServer.RebuildObservers(networkIdentity, true);
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000AE6C File Offset: 0x0000906C
		public static void UnSpawn(GameObject obj)
		{
			NetworkServer.DestroyObject(obj, NetworkServer.DestroyMode.Reset);
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000AE75 File Offset: 0x00009075
		public static void Destroy(GameObject obj)
		{
			NetworkServer.DestroyObject(obj, NetworkServer.DestroyMode.Destroy);
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000AE80 File Offset: 0x00009080
		private static void DestroyObject(GameObject obj, NetworkServer.DestroyMode mode)
		{
			if (obj == null)
			{
				Debug.Log("NetworkServer DestroyObject is null");
				return;
			}
			NetworkIdentity identity;
			if (NetworkServer.GetNetworkIdentity(obj, out identity))
			{
				NetworkServer.DestroyObject(identity, mode);
			}
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000AEB4 File Offset: 0x000090B4
		private static void DestroyObject(NetworkIdentity identity, NetworkServer.DestroyMode mode)
		{
			if (NetworkServer.active && NetworkServer.aoi)
			{
				try
				{
					NetworkServer.aoi.OnDestroyed(identity);
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
			NetworkServer.spawned.Remove(identity.netId);
			NetworkConnectionToClient connectionToClient = identity.connectionToClient;
			if (connectionToClient != null)
			{
				connectionToClient.RemoveOwnedObject(identity);
			}
			NetworkServer.SendToObservers<ObjectDestroyMessage>(identity, new ObjectDestroyMessage
			{
				netId = identity.netId
			}, 0);
			identity.ClearObservers();
			if (NetworkClient.active && NetworkServer.activeHost)
			{
				if (identity.isLocalPlayer)
				{
					identity.OnStopLocalPlayer();
				}
				identity.OnStopClient();
				identity.isOwned = false;
				identity.NotifyAuthority();
				NetworkClient.connection.owned.Remove(identity);
				NetworkClient.spawned.Remove(identity.netId);
			}
			identity.OnStopServer();
			if (mode != NetworkServer.DestroyMode.Destroy)
			{
				if (mode == NetworkServer.DestroyMode.Reset)
				{
					identity.Reset();
				}
				return;
			}
			identity.destroyCalled = true;
			if (Application.isPlaying)
			{
				UnityEngine.Object.Destroy(identity.gameObject);
				return;
			}
			UnityEngine.Object.DestroyImmediate(identity.gameObject);
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000AFC8 File Offset: 0x000091C8
		private static void RebuildObserversDefault(NetworkIdentity identity, bool initialize)
		{
			if (initialize && identity.visible != Visibility.ForceHidden)
			{
				NetworkServer.AddAllReadyServerConnectionsToObservers(identity);
			}
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000AFDC File Offset: 0x000091DC
		internal static void AddAllReadyServerConnectionsToObservers(NetworkIdentity identity)
		{
			foreach (NetworkConnectionToClient networkConnectionToClient in NetworkServer.connections.Values)
			{
				if (networkConnectionToClient.isReady)
				{
					identity.AddObserver(networkConnectionToClient);
				}
			}
			if (NetworkServer.localConnection != null && NetworkServer.localConnection.isReady)
			{
				identity.AddObserver(NetworkServer.localConnection);
			}
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000B05C File Offset: 0x0000925C
		public static void RebuildObservers(NetworkIdentity identity, bool initialize)
		{
			if (NetworkServer.aoi == null || identity.visible == Visibility.ForceShown)
			{
				NetworkServer.RebuildObserversDefault(identity, initialize);
				return;
			}
			NetworkServer.aoi.Rebuild(identity, initialize);
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000B088 File Offset: 0x00009288
		private static NetworkWriter SerializeForConnection(NetworkIdentity identity, NetworkConnectionToClient connection)
		{
			NetworkIdentitySerialization serverSerializationAtTick = identity.GetServerSerializationAtTick(Time.frameCount);
			if (identity.connectionToClient == connection)
			{
				if (serverSerializationAtTick.ownerWriter.Position > 0)
				{
					return serverSerializationAtTick.ownerWriter;
				}
			}
			else if (serverSerializationAtTick.observersWriter.Position > 0)
			{
				return serverSerializationAtTick.observersWriter;
			}
			return null;
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000B0D8 File Offset: 0x000092D8
		private static void BroadcastToConnection(NetworkConnectionToClient connection)
		{
			foreach (NetworkIdentity networkIdentity in connection.observing)
			{
				if (networkIdentity != null)
				{
					NetworkWriter networkWriter = NetworkServer.SerializeForConnection(networkIdentity, connection);
					if (networkWriter != null)
					{
						EntityStateMessage message = new EntityStateMessage
						{
							netId = networkIdentity.netId,
							payload = networkWriter.ToArraySegment()
						};
						connection.Send<EntityStateMessage>(message, 0);
					}
				}
				else
				{
					Debug.LogWarning(string.Format("Found 'null' entry in observing list for connectionId={0}. Please call NetworkServer.Destroy to destroy networked objects. Don't use GameObject.Destroy.", connection.connectionId));
				}
			}
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000B184 File Offset: 0x00009384
		private static bool DisconnectIfInactive(NetworkConnectionToClient connection)
		{
			if (NetworkServer.disconnectInactiveConnections && !connection.IsAlive(NetworkServer.disconnectInactiveTimeout))
			{
				Debug.LogWarning(string.Format("Disconnecting {0} for inactivity!", connection));
				connection.Disconnect();
				return true;
			}
			return false;
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000B1B4 File Offset: 0x000093B4
		private static void Broadcast()
		{
			NetworkServer.connectionsCopy.Clear();
			NetworkServer.connections.Values.CopyTo(NetworkServer.connectionsCopy);
			foreach (NetworkConnectionToClient networkConnectionToClient in NetworkServer.connectionsCopy)
			{
				if (!NetworkServer.DisconnectIfInactive(networkConnectionToClient))
				{
					if (networkConnectionToClient.isReady)
					{
						networkConnectionToClient.Send<TimeSnapshotMessage>(default(TimeSnapshotMessage), 1);
						NetworkServer.BroadcastToConnection(networkConnectionToClient);
					}
					networkConnectionToClient.Update();
				}
			}
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000B24C File Offset: 0x0000944C
		internal static void NetworkEarlyUpdate()
		{
			if (NetworkServer.active)
			{
				NetworkServer.earlyUpdateDuration.Begin();
				NetworkServer.fullUpdateDuration.Begin();
			}
			if (Transport.active != null)
			{
				Transport.active.ServerEarlyUpdate();
			}
			foreach (NetworkConnectionToClient networkConnectionToClient in NetworkServer.connections.Values)
			{
				networkConnectionToClient.UpdateTimeInterpolation();
			}
			if (NetworkServer.active)
			{
				NetworkServer.earlyUpdateDuration.End();
			}
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000B2E4 File Offset: 0x000094E4
		internal static void NetworkLateUpdate()
		{
			if (NetworkServer.active)
			{
				NetworkServer.lateUpdateDuration.Begin();
				if (!Application.isPlaying || AccurateInterval.Elapsed(NetworkTime.localTime, (double)NetworkServer.sendInterval, ref NetworkServer.lastSendTime))
				{
					NetworkServer.Broadcast();
				}
			}
			if (Transport.active != null)
			{
				Transport.active.ServerLateUpdate();
			}
			if (NetworkServer.active)
			{
				NetworkServer.actualTickRateCounter++;
				if (NetworkTime.localTime >= NetworkServer.actualTickRateStart + 1.0)
				{
					float num = (float)(NetworkTime.localTime - NetworkServer.actualTickRateStart);
					NetworkServer.actualTickRate = Mathf.RoundToInt((float)NetworkServer.actualTickRateCounter / num);
					NetworkServer.actualTickRateStart = NetworkTime.localTime;
					NetworkServer.actualTickRateCounter = 0;
				}
				NetworkServer.lateUpdateDuration.End();
				NetworkServer.fullUpdateDuration.End();
			}
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000B3A8 File Offset: 0x000095A8
		[Obsolete("NetworkServer.ActivateHostScene was moved to HostMode.ActivateHostScene")]
		public static void ActivateHostScene()
		{
			HostMode.ActivateHostScene();
		}

		// Token: 0x0400010F RID: 271
		private static bool initialized;

		// Token: 0x04000110 RID: 272
		public static int maxConnections;

		// Token: 0x04000111 RID: 273
		public static int tickRate = 60;

		// Token: 0x04000112 RID: 274
		private static double lastSendTime;

		// Token: 0x04000114 RID: 276
		public static Dictionary<int, NetworkConnectionToClient> connections = new Dictionary<int, NetworkConnectionToClient>();

		// Token: 0x04000115 RID: 277
		internal static Dictionary<ushort, NetworkMessageDelegate> handlers = new Dictionary<ushort, NetworkMessageDelegate>();

		// Token: 0x04000116 RID: 278
		public static readonly Dictionary<uint, NetworkIdentity> spawned = new Dictionary<uint, NetworkIdentity>();

		// Token: 0x04000117 RID: 279
		public static bool dontListen;

		// Token: 0x04000119 RID: 281
		public static bool isLoadingScene;

		// Token: 0x0400011A RID: 282
		public static InterestManagementBase aoi;

		// Token: 0x0400011B RID: 283
		public static bool disconnectInactiveConnections;

		// Token: 0x0400011C RID: 284
		public static float disconnectInactiveTimeout = 60f;

		// Token: 0x0400011D RID: 285
		public static Action<NetworkConnectionToClient> OnConnectedEvent;

		// Token: 0x0400011E RID: 286
		public static Action<NetworkConnectionToClient> OnDisconnectedEvent;

		// Token: 0x0400011F RID: 287
		public static Action<NetworkConnectionToClient, TransportError, string> OnErrorEvent;

		// Token: 0x04000120 RID: 288
		public static int actualTickRate;

		// Token: 0x04000121 RID: 289
		private static double actualTickRateStart;

		// Token: 0x04000122 RID: 290
		private static int actualTickRateCounter;

		// Token: 0x04000123 RID: 291
		public static TimeSample earlyUpdateDuration;

		// Token: 0x04000124 RID: 292
		public static TimeSample lateUpdateDuration;

		// Token: 0x04000125 RID: 293
		public static TimeSample fullUpdateDuration;

		// Token: 0x04000126 RID: 294
		internal static readonly List<NetworkConnectionToClient> connectionsCopy = new List<NetworkConnectionToClient>();

		// Token: 0x0200005C RID: 92
		private enum DestroyMode
		{
			// Token: 0x04000128 RID: 296
			Destroy,
			// Token: 0x04000129 RID: 297
			Reset
		}
	}
}
