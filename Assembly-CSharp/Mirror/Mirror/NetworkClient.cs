using System;
using System.Collections.Generic;
using System.Linq;
using Mirror.RemoteCalls;
using UnityEngine;

namespace Mirror
{
	// Token: 0x02000037 RID: 55
	public static class NetworkClient
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x00004118 File Offset: 0x00002318
		public static int sendRate
		{
			get
			{
				return NetworkServer.sendRate;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x0000411F File Offset: 0x0000231F
		public static float sendInterval
		{
			get
			{
				if (NetworkClient.sendRate >= 2147483647)
				{
					return 0f;
				}
				return 1f / (float)NetworkClient.sendRate;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x0000413F File Offset: 0x0000233F
		// (set) Token: 0x060000B7 RID: 183 RVA: 0x00004146 File Offset: 0x00002346
		public static NetworkConnection connection { get; internal set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x0000414E File Offset: 0x0000234E
		// (set) Token: 0x060000B9 RID: 185 RVA: 0x00004155 File Offset: 0x00002355
		public static NetworkIdentity localPlayer { get; internal set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000BA RID: 186 RVA: 0x0000415D File Offset: 0x0000235D
		public static bool active
		{
			get
			{
				return NetworkClient.connectState == ConnectState.Connecting || NetworkClient.connectState == ConnectState.Connected;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00004171 File Offset: 0x00002371
		public static bool activeHost
		{
			get
			{
				return NetworkClient.connection is LocalConnectionToServer;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00004180 File Offset: 0x00002380
		public static bool isConnecting
		{
			get
			{
				return NetworkClient.connectState == ConnectState.Connecting;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000BD RID: 189 RVA: 0x0000418A File Offset: 0x0000238A
		public static bool isConnected
		{
			get
			{
				return NetworkClient.connectState == ConnectState.Connected;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00004194 File Offset: 0x00002394
		[Obsolete("NetworkClient.isHostClient was renamed to .activeHost to be more obvious")]
		public static bool isHostClient
		{
			get
			{
				return NetworkClient.activeHost;
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x0000419C File Offset: 0x0000239C
		private static void AddTransportHandlers()
		{
			NetworkClient.RemoveTransportHandlers();
			Transport active = Transport.active;
			active.OnClientConnected = (Action)Delegate.Combine(active.OnClientConnected, new Action(NetworkClient.OnTransportConnected));
			Transport active2 = Transport.active;
			active2.OnClientDataReceived = (Action<ArraySegment<byte>, int>)Delegate.Combine(active2.OnClientDataReceived, new Action<ArraySegment<byte>, int>(NetworkClient.OnTransportData));
			Transport active3 = Transport.active;
			active3.OnClientDisconnected = (Action)Delegate.Combine(active3.OnClientDisconnected, new Action(NetworkClient.OnTransportDisconnected));
			Transport active4 = Transport.active;
			active4.OnClientError = (Action<TransportError, string>)Delegate.Combine(active4.OnClientError, new Action<TransportError, string>(NetworkClient.OnTransportError));
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00004248 File Offset: 0x00002448
		private static void RemoveTransportHandlers()
		{
			Transport active = Transport.active;
			active.OnClientConnected = (Action)Delegate.Remove(active.OnClientConnected, new Action(NetworkClient.OnTransportConnected));
			Transport active2 = Transport.active;
			active2.OnClientDataReceived = (Action<ArraySegment<byte>, int>)Delegate.Remove(active2.OnClientDataReceived, new Action<ArraySegment<byte>, int>(NetworkClient.OnTransportData));
			Transport active3 = Transport.active;
			active3.OnClientDisconnected = (Action)Delegate.Remove(active3.OnClientDisconnected, new Action(NetworkClient.OnTransportDisconnected));
			Transport active4 = Transport.active;
			active4.OnClientError = (Action<TransportError, string>)Delegate.Remove(active4.OnClientError, new Action<TransportError, string>(NetworkClient.OnTransportError));
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x000042ED File Offset: 0x000024ED
		private static void Initialize(bool hostMode)
		{
			if (!WeaverFuse.Weaved())
			{
				throw new Exception("NetworkClient won't start because Weaving failed or didn't run.");
			}
			NetworkClient.unbatcher = new Unbatcher();
			NetworkClient.InitTimeInterpolation();
			NetworkClient.RegisterMessageHandlers(hostMode);
			Transport.active.enabled = true;
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00004321 File Offset: 0x00002521
		public static void Connect(string address)
		{
			NetworkClient.Initialize(false);
			NetworkClient.AddTransportHandlers();
			NetworkClient.connectState = ConnectState.Connecting;
			Transport.active.ClientConnect(address);
			NetworkClient.connection = new NetworkConnectionToServer();
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00004349 File Offset: 0x00002549
		public static void Connect(Uri uri)
		{
			NetworkClient.Initialize(false);
			NetworkClient.AddTransportHandlers();
			NetworkClient.connectState = ConnectState.Connecting;
			Transport.active.ClientConnect(uri);
			NetworkClient.connection = new NetworkConnectionToServer();
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00004371 File Offset: 0x00002571
		public static void ConnectHost()
		{
			NetworkClient.Initialize(true);
			NetworkClient.connectState = ConnectState.Connected;
			HostMode.SetupConnections();
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00004384 File Offset: 0x00002584
		[Obsolete("NetworkClient.ConnectLocalServer was moved to HostMode.InvokeOnConnected")]
		public static void ConnectLocalServer()
		{
			HostMode.InvokeOnConnected();
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x0000438B File Offset: 0x0000258B
		public static void Disconnect()
		{
			if (NetworkClient.connectState != ConnectState.Connecting && NetworkClient.connectState != ConnectState.Connected)
			{
				return;
			}
			NetworkClient.connectState = ConnectState.Disconnecting;
			NetworkClient.ready = false;
			NetworkConnection connection = NetworkClient.connection;
			if (connection == null)
			{
				return;
			}
			connection.Disconnect();
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000043B9 File Offset: 0x000025B9
		private static void OnTransportConnected()
		{
			if (NetworkClient.connection == null)
			{
				Debug.LogError("Skipped Connect message handling because connection is null.");
				return;
			}
			NetworkTime.ResetStatics();
			NetworkClient.connectState = ConnectState.Connected;
			NetworkTime.UpdateClient();
			Action onConnectedEvent = NetworkClient.OnConnectedEvent;
			if (onConnectedEvent == null)
			{
				return;
			}
			onConnectedEvent();
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000043EC File Offset: 0x000025EC
		private static bool UnpackAndInvoke(NetworkReader reader, int channelId)
		{
			ushort num;
			if (!NetworkMessages.UnpackId(reader, out num))
			{
				Debug.LogWarning("Invalid message header.");
				return false;
			}
			NetworkMessageDelegate networkMessageDelegate;
			if (NetworkClient.handlers.TryGetValue(num, out networkMessageDelegate))
			{
				networkMessageDelegate(NetworkClient.connection, reader, channelId);
				if (NetworkClient.connection != null)
				{
					NetworkClient.connection.lastMessageTime = Time.time;
				}
				return true;
			}
			Debug.LogWarning(string.Format("Unknown message id: {0}. This can happen if no handler was registered for this message.", num));
			return false;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x0000445C File Offset: 0x0000265C
		internal static void OnTransportData(ArraySegment<byte> data, int channelId)
		{
			if (NetworkClient.connection != null)
			{
				if (!NetworkClient.unbatcher.AddBatch(data))
				{
					Debug.LogWarning("NetworkClient: failed to add batch, disconnecting.");
					NetworkClient.connection.Disconnect();
					return;
				}
				ArraySegment<byte> segment;
				double remoteTimeStamp;
				while (!NetworkClient.isLoadingScene && NetworkClient.unbatcher.GetNextMessage(out segment, out remoteTimeStamp))
				{
					using (NetworkReaderPooled networkReaderPooled = NetworkReaderPool.Get(segment))
					{
						if (networkReaderPooled.Remaining < 2)
						{
							Debug.LogWarning("NetworkClient: received Message was too short (messages should start with message id)");
							NetworkClient.connection.Disconnect();
							return;
						}
						NetworkClient.connection.remoteTimeStamp = remoteTimeStamp;
						if (!NetworkClient.UnpackAndInvoke(networkReaderPooled, channelId))
						{
							Debug.LogWarning("NetworkClient: failed to unpack and invoke message. Disconnecting.");
							NetworkClient.connection.Disconnect();
							return;
						}
						continue;
					}
					break;
				}
				if (!NetworkClient.isLoadingScene && NetworkClient.unbatcher.BatchesCount > 0)
				{
					Debug.LogError(string.Format("Still had {0} batches remaining after processing, even though processing was not interrupted by a scene change. This should never happen, as it would cause ever growing batches.\nPossible reasons:\n* A message didn't deserialize as much as it serialized\n*There was no message handler for a message id, so the reader wasn't read until the end.", NetworkClient.unbatcher.BatchesCount));
					return;
				}
			}
			else
			{
				Debug.LogError("Skipped Data message handling because connection is null.");
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00004558 File Offset: 0x00002758
		internal static void OnTransportDisconnected()
		{
			if (NetworkClient.connectState == ConnectState.Disconnected)
			{
				return;
			}
			Action onDisconnectedEvent = NetworkClient.OnDisconnectedEvent;
			if (onDisconnectedEvent != null)
			{
				onDisconnectedEvent();
			}
			NetworkClient.connectState = ConnectState.Disconnected;
			NetworkClient.ready = false;
			NetworkClient.snapshots.Clear();
			NetworkClient.localTimeline = 0.0;
			NetworkClient.connection = null;
			NetworkClient.RemoveTransportHandlers();
		}

		// Token: 0x060000CB RID: 203 RVA: 0x000045AD File Offset: 0x000027AD
		private static void OnTransportError(TransportError error, string reason)
		{
			Debug.LogWarning(string.Format("Client Transport Error: {0}: {1}. This is fine.", error, reason));
			Action<TransportError, string> onErrorEvent = NetworkClient.OnErrorEvent;
			if (onErrorEvent == null)
			{
				return;
			}
			onErrorEvent(error, reason);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x000045D6 File Offset: 0x000027D6
		public static void Send<T>(T message, int channelId = 0) where T : struct, NetworkMessage
		{
			if (NetworkClient.connection == null)
			{
				Debug.LogError("NetworkClient Send with no connection");
				return;
			}
			if (NetworkClient.connectState == ConnectState.Connected)
			{
				NetworkClient.connection.Send<T>(message, channelId);
				return;
			}
			Debug.LogError("NetworkClient Send when not connected to a server");
		}

		// Token: 0x060000CD RID: 205 RVA: 0x0000460C File Offset: 0x0000280C
		internal static void RegisterMessageHandlers(bool hostMode)
		{
			if (hostMode)
			{
				NetworkClient.RegisterHandler<ObjectDestroyMessage>(new Action<ObjectDestroyMessage>(NetworkClient.OnHostClientObjectDestroy), true);
				NetworkClient.RegisterHandler<ObjectHideMessage>(new Action<ObjectHideMessage>(NetworkClient.OnHostClientObjectHide), true);
				NetworkClient.RegisterHandler<NetworkPongMessage>(delegate(NetworkPongMessage _)
				{
				}, false);
				NetworkClient.RegisterHandler<SpawnMessage>(new Action<SpawnMessage>(NetworkClient.OnHostClientSpawn), true);
				NetworkClient.RegisterHandler<ObjectSpawnStartedMessage>(delegate(ObjectSpawnStartedMessage _)
				{
				}, true);
				NetworkClient.RegisterHandler<ObjectSpawnFinishedMessage>(delegate(ObjectSpawnFinishedMessage _)
				{
				}, true);
				NetworkClient.RegisterHandler<EntityStateMessage>(delegate(EntityStateMessage _)
				{
				}, true);
			}
			else
			{
				NetworkClient.RegisterHandler<ObjectDestroyMessage>(new Action<ObjectDestroyMessage>(NetworkClient.OnObjectDestroy), true);
				NetworkClient.RegisterHandler<ObjectHideMessage>(new Action<ObjectHideMessage>(NetworkClient.OnObjectHide), true);
				NetworkClient.RegisterHandler<NetworkPongMessage>(new Action<NetworkPongMessage>(NetworkTime.OnClientPong), false);
				NetworkClient.RegisterHandler<NetworkPingMessage>(new Action<NetworkPingMessage>(NetworkTime.OnClientPing), false);
				NetworkClient.RegisterHandler<SpawnMessage>(new Action<SpawnMessage>(NetworkClient.OnSpawn), true);
				NetworkClient.RegisterHandler<ObjectSpawnStartedMessage>(new Action<ObjectSpawnStartedMessage>(NetworkClient.OnObjectSpawnStarted), true);
				NetworkClient.RegisterHandler<ObjectSpawnFinishedMessage>(new Action<ObjectSpawnFinishedMessage>(NetworkClient.OnObjectSpawnFinished), true);
				NetworkClient.RegisterHandler<EntityStateMessage>(new Action<EntityStateMessage>(NetworkClient.OnEntityStateMessage), true);
			}
			NetworkClient.RegisterHandler<TimeSnapshotMessage>(new Action<TimeSnapshotMessage>(NetworkClient.OnTimeSnapshotMessage), true);
			NetworkClient.RegisterHandler<ChangeOwnerMessage>(new Action<ChangeOwnerMessage>(NetworkClient.OnChangeOwner), true);
			NetworkClient.RegisterHandler<RpcMessage>(new Action<RpcMessage>(NetworkClient.OnRPCMessage), true);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000047B4 File Offset: 0x000029B4
		public static void RegisterHandler<T>(Action<T> handler, bool requireAuthentication = true) where T : struct, NetworkMessage
		{
			NetworkClient.<>c__DisplayClass54_0<T> CS$<>8__locals1 = new NetworkClient.<>c__DisplayClass54_0<T>();
			CS$<>8__locals1.handler = handler;
			ushort id = NetworkMessageId<T>.Id;
			if (NetworkClient.handlers.ContainsKey(id))
			{
				Debug.LogWarning(string.Format("NetworkClient.RegisterHandler replacing handler for {0}, id={1}. If replacement is intentional, use ReplaceHandler instead to avoid this warning.", typeof(T).FullName, id));
			}
			NetworkMessages.Lookup[id] = typeof(T);
			NetworkClient.handlers[id] = NetworkMessages.WrapHandler<T, NetworkConnection>(new Action<NetworkConnection, T>(CS$<>8__locals1.<RegisterHandler>g__HandlerWrapped|0), requireAuthentication);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00004838 File Offset: 0x00002A38
		public static void ReplaceHandler<T>(Action<NetworkConnection, T> handler, bool requireAuthentication = true) where T : struct, NetworkMessage
		{
			ushort id = NetworkMessageId<T>.Id;
			NetworkClient.handlers[id] = NetworkMessages.WrapHandler<T, NetworkConnection>(handler, requireAuthentication);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x0000485D File Offset: 0x00002A5D
		public static void ReplaceHandler<T>(Action<T> handler, bool requireAuthentication = true) where T : struct, NetworkMessage
		{
			NetworkClient.ReplaceHandler<T>(delegate(NetworkConnection _, T value)
			{
				handler(value);
			}, requireAuthentication);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0000487C File Offset: 0x00002A7C
		public static bool UnregisterHandler<T>() where T : struct, NetworkMessage
		{
			ushort id = NetworkMessageId<T>.Id;
			return NetworkClient.handlers.Remove(id);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x0000489A File Offset: 0x00002A9A
		public static bool GetPrefab(uint assetId, out GameObject prefab)
		{
			prefab = null;
			return assetId != 0U && NetworkClient.prefabs.TryGetValue(assetId, out prefab) && prefab != null;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000048BC File Offset: 0x00002ABC
		private static void RegisterPrefabIdentity(NetworkIdentity prefab)
		{
			if (prefab.assetId == 0U)
			{
				Debug.LogError("Can not Register '" + prefab.name + "' because it had empty assetid. If this is a scene Object use RegisterSpawnHandler instead");
				return;
			}
			if (prefab.sceneId != 0UL)
			{
				Debug.LogError("Can not Register '" + prefab.name + "' because it has a sceneId, make sure you are passing in the original prefab and not an instance in the scene.");
				return;
			}
			if (prefab.GetComponentsInChildren<NetworkIdentity>().Length > 1)
			{
				Debug.LogError("Prefab '" + prefab.name + "' has multiple NetworkIdentity components. There should only be one NetworkIdentity on a prefab, and it must be on the root object.");
			}
			if (NetworkClient.prefabs.ContainsKey(prefab.assetId))
			{
				GameObject gameObject = NetworkClient.prefabs[prefab.assetId];
				Debug.LogWarning(string.Format("Replacing existing prefab with assetId '{0}'. Old prefab '{1}', New prefab '{2}'", prefab.assetId, gameObject.name, prefab.name));
			}
			if (NetworkClient.spawnHandlers.ContainsKey(prefab.assetId) || NetworkClient.unspawnHandlers.ContainsKey(prefab.assetId))
			{
				Debug.LogWarning(string.Format("Adding prefab '{0}' with assetId '{1}' when spawnHandlers with same assetId already exists. If you want to use custom spawn handling, then remove the prefab from NetworkManager's registered prefabs first.", prefab.name, prefab.assetId));
			}
			NetworkClient.prefabs[prefab.assetId] = prefab.gameObject;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x000049D8 File Offset: 0x00002BD8
		public static void RegisterPrefab(GameObject prefab, uint newAssetId)
		{
			if (prefab == null)
			{
				Debug.LogError("Could not register prefab because it was null");
				return;
			}
			if (newAssetId == 0U)
			{
				Debug.LogError("Could not register '" + prefab.name + "' with new assetId because the new assetId was empty");
				return;
			}
			NetworkIdentity networkIdentity;
			if (!prefab.TryGetComponent<NetworkIdentity>(out networkIdentity))
			{
				Debug.LogError("Could not register '" + prefab.name + "' since it contains no NetworkIdentity component");
				return;
			}
			if (networkIdentity.assetId != 0U && networkIdentity.assetId != newAssetId)
			{
				Debug.LogError(string.Format("Could not register '{0}' to {1} because it already had an AssetId, Existing assetId {2}", prefab.name, newAssetId, networkIdentity.assetId));
				return;
			}
			networkIdentity.assetId = newAssetId;
			NetworkClient.RegisterPrefabIdentity(networkIdentity);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00004A84 File Offset: 0x00002C84
		public static void RegisterPrefab(GameObject prefab)
		{
			if (prefab == null)
			{
				Debug.LogError("Could not register prefab because it was null");
				return;
			}
			NetworkIdentity prefab2;
			if (!prefab.TryGetComponent<NetworkIdentity>(out prefab2))
			{
				Debug.LogError("Could not register '" + prefab.name + "' since it contains no NetworkIdentity component");
				return;
			}
			NetworkClient.RegisterPrefabIdentity(prefab2);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00004AD0 File Offset: 0x00002CD0
		public static void RegisterPrefab(GameObject prefab, uint newAssetId, SpawnDelegate spawnHandler, UnSpawnDelegate unspawnHandler)
		{
			if (spawnHandler == null)
			{
				Debug.LogError(string.Format("Can not Register null SpawnHandler for {0}", newAssetId));
				return;
			}
			NetworkClient.RegisterPrefab(prefab, newAssetId, (SpawnMessage msg) => spawnHandler(msg.position, msg.assetId), unspawnHandler);
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00004B1C File Offset: 0x00002D1C
		public static void RegisterPrefab(GameObject prefab, SpawnDelegate spawnHandler, UnSpawnDelegate unspawnHandler)
		{
			if (prefab == null)
			{
				Debug.LogError("Could not register handler for prefab because the prefab was null");
				return;
			}
			NetworkIdentity networkIdentity;
			if (!prefab.TryGetComponent<NetworkIdentity>(out networkIdentity))
			{
				Debug.LogError("Could not register handler for '" + prefab.name + "' since it contains no NetworkIdentity component");
				return;
			}
			if (networkIdentity.sceneId != 0UL)
			{
				Debug.LogError("Can not Register '" + prefab.name + "' because it has a sceneId, make sure you are passing in the original prefab and not an instance in the scene.");
				return;
			}
			if (networkIdentity.assetId == 0U)
			{
				Debug.LogError("Can not Register handler for '" + prefab.name + "' because it had empty assetid. If this is a scene Object use RegisterSpawnHandler instead");
				return;
			}
			if (spawnHandler == null)
			{
				Debug.LogError(string.Format("Can not Register null SpawnHandler for {0}", networkIdentity.assetId));
				return;
			}
			NetworkClient.RegisterPrefab(prefab, (SpawnMessage msg) => spawnHandler(msg.position, msg.assetId), unspawnHandler);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00004BEC File Offset: 0x00002DEC
		public static void RegisterPrefab(GameObject prefab, uint newAssetId, SpawnHandlerDelegate spawnHandler, UnSpawnDelegate unspawnHandler)
		{
			if (newAssetId == 0U)
			{
				Debug.LogError("Could not register handler for '" + prefab.name + "' with new assetId because the new assetId was empty");
				return;
			}
			if (prefab == null)
			{
				Debug.LogError("Could not register handler for prefab because the prefab was null");
				return;
			}
			NetworkIdentity networkIdentity;
			if (!prefab.TryGetComponent<NetworkIdentity>(out networkIdentity))
			{
				Debug.LogError("Could not register handler for '" + prefab.name + "' since it contains no NetworkIdentity component");
				return;
			}
			if (networkIdentity.assetId != 0U && networkIdentity.assetId != newAssetId)
			{
				Debug.LogError(string.Format("Could not register Handler for '{0}' to {1} because it already had an AssetId, Existing assetId {2}", prefab.name, newAssetId, networkIdentity.assetId));
				return;
			}
			if (networkIdentity.sceneId != 0UL)
			{
				Debug.LogError("Can not Register '" + prefab.name + "' because it has a sceneId, make sure you are passing in the original prefab and not an instance in the scene.");
				return;
			}
			networkIdentity.assetId = newAssetId;
			uint assetId = networkIdentity.assetId;
			if (spawnHandler == null)
			{
				Debug.LogError(string.Format("Can not Register null SpawnHandler for {0}", assetId));
				return;
			}
			if (unspawnHandler == null)
			{
				Debug.LogError(string.Format("Can not Register null UnSpawnHandler for {0}", assetId));
				return;
			}
			if (NetworkClient.spawnHandlers.ContainsKey(assetId) || NetworkClient.unspawnHandlers.ContainsKey(assetId))
			{
				Debug.LogWarning(string.Format("Replacing existing spawnHandlers for prefab '{0}' with assetId '{1}'", prefab.name, assetId));
			}
			if (NetworkClient.prefabs.ContainsKey(assetId))
			{
				Debug.LogError(string.Format("assetId '{0}' is already used by prefab '{1}', unregister the prefab first before trying to add handler", assetId, NetworkClient.prefabs[assetId].name));
			}
			if (prefab.GetComponentsInChildren<NetworkIdentity>().Length > 1)
			{
				Debug.LogError("Prefab '" + prefab.name + "' has multiple NetworkIdentity components. There should only be one NetworkIdentity on a prefab, and it must be on the root object.");
			}
			NetworkClient.spawnHandlers[assetId] = spawnHandler;
			NetworkClient.unspawnHandlers[assetId] = unspawnHandler;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00004D90 File Offset: 0x00002F90
		public static void RegisterPrefab(GameObject prefab, SpawnHandlerDelegate spawnHandler, UnSpawnDelegate unspawnHandler)
		{
			if (prefab == null)
			{
				Debug.LogError("Could not register handler for prefab because the prefab was null");
				return;
			}
			NetworkIdentity networkIdentity;
			if (!prefab.TryGetComponent<NetworkIdentity>(out networkIdentity))
			{
				Debug.LogError("Could not register handler for '" + prefab.name + "' since it contains no NetworkIdentity component");
				return;
			}
			if (networkIdentity.sceneId != 0UL)
			{
				Debug.LogError("Can not Register '" + prefab.name + "' because it has a sceneId, make sure you are passing in the original prefab and not an instance in the scene.");
				return;
			}
			uint assetId = networkIdentity.assetId;
			if (assetId == 0U)
			{
				Debug.LogError("Can not Register handler for '" + prefab.name + "' because it had empty assetid. If this is a scene Object use RegisterSpawnHandler instead");
				return;
			}
			if (spawnHandler == null)
			{
				Debug.LogError(string.Format("Can not Register null SpawnHandler for {0}", assetId));
				return;
			}
			if (unspawnHandler == null)
			{
				Debug.LogError(string.Format("Can not Register null UnSpawnHandler for {0}", assetId));
				return;
			}
			if (NetworkClient.spawnHandlers.ContainsKey(assetId) || NetworkClient.unspawnHandlers.ContainsKey(assetId))
			{
				Debug.LogWarning(string.Format("Replacing existing spawnHandlers for prefab '{0}' with assetId '{1}'", prefab.name, assetId));
			}
			if (NetworkClient.prefabs.ContainsKey(assetId))
			{
				Debug.LogError(string.Format("assetId '{0}' is already used by prefab '{1}', unregister the prefab first before trying to add handler", assetId, NetworkClient.prefabs[assetId].name));
			}
			if (prefab.GetComponentsInChildren<NetworkIdentity>().Length > 1)
			{
				Debug.LogError("Prefab '" + prefab.name + "' has multiple NetworkIdentity components. There should only be one NetworkIdentity on a prefab, and it must be on the root object.");
			}
			NetworkClient.spawnHandlers[assetId] = spawnHandler;
			NetworkClient.unspawnHandlers[assetId] = unspawnHandler;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00004EF4 File Offset: 0x000030F4
		public static void UnregisterPrefab(GameObject prefab)
		{
			if (prefab == null)
			{
				Debug.LogError("Could not unregister prefab because it was null");
				return;
			}
			NetworkIdentity networkIdentity;
			if (!prefab.TryGetComponent<NetworkIdentity>(out networkIdentity))
			{
				Debug.LogError("Could not unregister '" + prefab.name + "' since it contains no NetworkIdentity component");
				return;
			}
			uint assetId = networkIdentity.assetId;
			NetworkClient.prefabs.Remove(assetId);
			NetworkClient.spawnHandlers.Remove(assetId);
			NetworkClient.unspawnHandlers.Remove(assetId);
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00004F68 File Offset: 0x00003168
		public static void RegisterSpawnHandler(uint assetId, SpawnDelegate spawnHandler, UnSpawnDelegate unspawnHandler)
		{
			if (spawnHandler == null)
			{
				Debug.LogError(string.Format("Can not Register null SpawnHandler for {0}", assetId));
				return;
			}
			NetworkClient.RegisterSpawnHandler(assetId, (SpawnMessage msg) => spawnHandler(msg.position, msg.assetId), unspawnHandler);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00004FB4 File Offset: 0x000031B4
		public static void RegisterSpawnHandler(uint assetId, SpawnHandlerDelegate spawnHandler, UnSpawnDelegate unspawnHandler)
		{
			if (spawnHandler == null)
			{
				Debug.LogError(string.Format("Can not Register null SpawnHandler for {0}", assetId));
				return;
			}
			if (unspawnHandler == null)
			{
				Debug.LogError(string.Format("Can not Register null UnSpawnHandler for {0}", assetId));
				return;
			}
			if (assetId == 0U)
			{
				Debug.LogError("Can not Register SpawnHandler for empty assetId");
				return;
			}
			if (NetworkClient.spawnHandlers.ContainsKey(assetId) || NetworkClient.unspawnHandlers.ContainsKey(assetId))
			{
				Debug.LogWarning(string.Format("Replacing existing spawnHandlers for {0}", assetId));
			}
			if (NetworkClient.prefabs.ContainsKey(assetId))
			{
				Debug.LogError(string.Format("assetId '{0}' is already used by prefab '{1}'", assetId, NetworkClient.prefabs[assetId].name));
			}
			NetworkClient.spawnHandlers[assetId] = spawnHandler;
			NetworkClient.unspawnHandlers[assetId] = unspawnHandler;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000507A File Offset: 0x0000327A
		public static void UnregisterSpawnHandler(uint assetId)
		{
			NetworkClient.spawnHandlers.Remove(assetId);
			NetworkClient.unspawnHandlers.Remove(assetId);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00005094 File Offset: 0x00003294
		public static void ClearSpawners()
		{
			NetworkClient.prefabs.Clear();
			NetworkClient.spawnHandlers.Clear();
			NetworkClient.unspawnHandlers.Clear();
		}

		// Token: 0x060000DF RID: 223 RVA: 0x000050B4 File Offset: 0x000032B4
		internal static bool InvokeUnSpawnHandler(uint assetId, GameObject obj)
		{
			UnSpawnDelegate unSpawnDelegate;
			if (NetworkClient.unspawnHandlers.TryGetValue(assetId, out unSpawnDelegate) && unSpawnDelegate != null)
			{
				unSpawnDelegate(obj);
				return true;
			}
			return false;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x000050E0 File Offset: 0x000032E0
		public static bool Ready()
		{
			if (NetworkClient.ready)
			{
				Debug.LogError("NetworkClient is already ready. It shouldn't be called twice.");
				return false;
			}
			if (NetworkClient.connection == null)
			{
				Debug.LogError("Ready() called with invalid connection object: conn=null");
				return false;
			}
			NetworkClient.ready = true;
			NetworkClient.connection.isReady = true;
			NetworkClient.connection.Send<ReadyMessage>(default(ReadyMessage), 0);
			return true;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00005139 File Offset: 0x00003339
		internal static void InternalAddPlayer(NetworkIdentity identity)
		{
			NetworkClient.localPlayer = identity;
			if (NetworkClient.ready && NetworkClient.connection != null)
			{
				NetworkClient.connection.identity = identity;
				return;
			}
			Debug.LogWarning("NetworkClient can't AddPlayer before being ready. Please call NetworkClient.Ready() first. Clients are considered ready after joining the game world.");
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00005168 File Offset: 0x00003368
		public static bool AddPlayer()
		{
			if (NetworkClient.connection == null)
			{
				Debug.LogError("AddPlayer requires a valid NetworkClient.connection.");
				return false;
			}
			if (!NetworkClient.ready)
			{
				Debug.LogError("AddPlayer requires a ready NetworkClient.");
				return false;
			}
			if (NetworkClient.connection.identity != null)
			{
				Debug.LogError("NetworkClient.AddPlayer: a PlayerController was already added. Did you call AddPlayer twice?");
				return false;
			}
			NetworkClient.connection.Send<AddPlayerMessage>(default(AddPlayerMessage), 0);
			return true;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000051D0 File Offset: 0x000033D0
		internal static void ApplySpawnPayload(NetworkIdentity identity, SpawnMessage message)
		{
			if (message.assetId != 0U)
			{
				identity.assetId = message.assetId;
			}
			if (!identity.gameObject.activeSelf)
			{
				identity.gameObject.SetActive(true);
			}
			identity.transform.localPosition = message.position;
			identity.transform.localRotation = message.rotation;
			identity.transform.localScale = message.scale;
			identity.isOwned = message.isOwner;
			identity.netId = message.netId;
			if (message.isLocalPlayer)
			{
				NetworkClient.InternalAddPlayer(identity);
			}
			NetworkClient.InitializeIdentityFlags(identity);
			if (message.payload.Count > 0)
			{
				using (NetworkReaderPooled networkReaderPooled = NetworkReaderPool.Get(message.payload))
				{
					identity.DeserializeClient(networkReaderPooled, true);
				}
			}
			NetworkClient.spawned[message.netId] = identity;
			if (identity.isOwned)
			{
				NetworkConnection connection = NetworkClient.connection;
				if (connection != null)
				{
					connection.owned.Add(identity);
				}
			}
			if (NetworkClient.isSpawnFinished)
			{
				NetworkClient.InvokeIdentityCallbacks(identity);
			}
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x000052E8 File Offset: 0x000034E8
		internal static bool FindOrSpawnObject(SpawnMessage message, out NetworkIdentity identity)
		{
			identity = NetworkClient.GetExistingObject(message.netId);
			if (identity != null)
			{
				return true;
			}
			if (message.assetId == 0U && message.sceneId == 0UL)
			{
				Debug.LogError(string.Format("OnSpawn message with netId '{0}' has no AssetId or sceneId", message.netId));
				return false;
			}
			identity = ((message.sceneId == 0UL) ? NetworkClient.SpawnPrefab(message) : NetworkClient.SpawnSceneObject(message.sceneId));
			if (identity == null)
			{
				Debug.LogError(string.Format("Could not spawn assetId={0} scene={1:X} netId={2}", message.assetId, message.sceneId, message.netId));
				return false;
			}
			return true;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00005394 File Offset: 0x00003594
		private static NetworkIdentity GetExistingObject(uint netid)
		{
			NetworkIdentity result;
			NetworkClient.spawned.TryGetValue(netid, out result);
			return result;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x000053B0 File Offset: 0x000035B0
		private static NetworkIdentity SpawnPrefab(SpawnMessage message)
		{
			SpawnHandlerDelegate spawnHandlerDelegate;
			if (NetworkClient.spawnHandlers.TryGetValue(message.assetId, out spawnHandlerDelegate))
			{
				GameObject gameObject = spawnHandlerDelegate(message);
				if (gameObject == null)
				{
					Debug.LogError(string.Format("Spawn Handler returned null, Handler assetId '{0}'", message.assetId));
					return null;
				}
				NetworkIdentity result;
				if (!gameObject.TryGetComponent<NetworkIdentity>(out result))
				{
					Debug.LogError(string.Format("Object Spawned by handler did not have a NetworkIdentity, Handler assetId '{0}'", message.assetId));
					return null;
				}
				return result;
			}
			else
			{
				GameObject original;
				if (NetworkClient.GetPrefab(message.assetId, out original))
				{
					return UnityEngine.Object.Instantiate<GameObject>(original, message.position, message.rotation).GetComponent<NetworkIdentity>();
				}
				Debug.LogError(string.Format("Failed to spawn server object, did you forget to add it to the NetworkManager? assetId={0} netId={1}", message.assetId, message.netId));
				return null;
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00005473 File Offset: 0x00003673
		private static NetworkIdentity SpawnSceneObject(ulong sceneId)
		{
			NetworkIdentity andRemoveSceneObject = NetworkClient.GetAndRemoveSceneObject(sceneId);
			if (andRemoveSceneObject == null)
			{
				Debug.LogError(string.Format("Spawn scene object not found for {0:X}. Make sure that client and server use exactly the same project. This only happens if the hierarchy gets out of sync.", sceneId));
			}
			return andRemoveSceneObject;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000549C File Offset: 0x0000369C
		private static NetworkIdentity GetAndRemoveSceneObject(ulong sceneId)
		{
			NetworkIdentity result;
			if (NetworkClient.spawnableObjects.TryGetValue(sceneId, out result))
			{
				NetworkClient.spawnableObjects.Remove(sceneId);
				return result;
			}
			return null;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x000054C8 File Offset: 0x000036C8
		public static void PrepareToSpawnSceneObjects()
		{
			NetworkClient.spawnableObjects.Clear();
			foreach (NetworkIdentity networkIdentity in Resources.FindObjectsOfTypeAll<NetworkIdentity>())
			{
				if (Utils.IsSceneObject(networkIdentity) && networkIdentity.netId == 0U)
				{
					NetworkIdentity networkIdentity2;
					if (NetworkClient.spawnableObjects.TryGetValue(networkIdentity.sceneId, out networkIdentity2))
					{
						Debug.LogWarning(string.Format("NetworkClient: Duplicate sceneId {0} detected on {1} and {2}\n", networkIdentity.sceneId, networkIdentity.gameObject.name, networkIdentity2.gameObject.name) + "This can happen if a networked object is persisted in DontDestroyOnLoad through loading / changing to the scene where it originated,\n" + string.Format("otherwise you may need to open and re-save the {0} to reset scene id's.", networkIdentity.gameObject.scene), networkIdentity.gameObject);
					}
					else
					{
						NetworkClient.spawnableObjects.Add(networkIdentity.sceneId, networkIdentity);
					}
				}
			}
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00005594 File Offset: 0x00003794
		internal static void OnObjectSpawnStarted(ObjectSpawnStartedMessage _)
		{
			NetworkClient.PrepareToSpawnSceneObjects();
			NetworkClient.isSpawnFinished = false;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x000055A4 File Offset: 0x000037A4
		internal static void OnObjectSpawnFinished(ObjectSpawnFinishedMessage _)
		{
			foreach (NetworkIdentity networkIdentity in from uv in NetworkClient.spawned.Values
			orderby uv.netId
			select uv)
			{
				if (networkIdentity != null)
				{
					NetworkClient.BootstrapIdentity(networkIdentity);
				}
				else
				{
					Debug.LogWarning("Found null entry in NetworkClient.spawned. This is unexpected. Was the NetworkIdentity not destroyed properly?");
				}
			}
			NetworkClient.isSpawnFinished = true;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00005634 File Offset: 0x00003834
		private static void OnHostClientObjectDestroy(ObjectDestroyMessage message)
		{
			NetworkIdentity item;
			if (NetworkClient.spawned.TryGetValue(message.netId, out item))
			{
				NetworkClient.connection.owned.Remove(item);
			}
			NetworkClient.spawned.Remove(message.netId);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00005678 File Offset: 0x00003878
		private static void OnHostClientObjectHide(ObjectHideMessage message)
		{
			NetworkIdentity networkIdentity;
			if (NetworkClient.spawned.TryGetValue(message.netId, out networkIdentity) && networkIdentity != null && NetworkClient.aoi != null)
			{
				NetworkClient.aoi.SetHostVisibility(networkIdentity, false);
			}
		}

		// Token: 0x060000EE RID: 238 RVA: 0x000056BC File Offset: 0x000038BC
		internal static void OnHostClientSpawn(SpawnMessage message)
		{
			NetworkIdentity networkIdentity;
			if (NetworkServer.spawned.TryGetValue(message.netId, out networkIdentity) && networkIdentity != null)
			{
				NetworkClient.spawned[message.netId] = networkIdentity;
				if (message.isOwner)
				{
					NetworkClient.connection.owned.Add(networkIdentity);
				}
				if (message.isLocalPlayer)
				{
					NetworkClient.InternalAddPlayer(networkIdentity);
				}
				if (NetworkClient.aoi != null)
				{
					NetworkClient.aoi.SetHostVisibility(networkIdentity, true);
				}
				networkIdentity.isOwned = message.isOwner;
				NetworkClient.BootstrapIdentity(networkIdentity);
			}
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0000574C File Offset: 0x0000394C
		private static void OnEntityStateMessage(EntityStateMessage message)
		{
			NetworkIdentity networkIdentity;
			if (NetworkClient.spawned.TryGetValue(message.netId, out networkIdentity) && networkIdentity != null)
			{
				using (NetworkReaderPooled networkReaderPooled = NetworkReaderPool.Get(message.payload))
				{
					networkIdentity.DeserializeClient(networkReaderPooled, false);
					return;
				}
			}
			Debug.LogWarning(string.Format("Did not find target for sync message for {0}. Were all prefabs added to the NetworkManager's spawnable list?\nNote: this can be completely normal because UDP messages may arrive out of order, so this message might have arrived after a Destroy message.", message.netId));
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000057C0 File Offset: 0x000039C0
		private static void OnRPCMessage(RpcMessage message)
		{
			NetworkIdentity networkIdentity;
			if (NetworkClient.spawned.TryGetValue(message.netId, out networkIdentity))
			{
				using (NetworkReaderPooled networkReaderPooled = NetworkReaderPool.Get(message.payload))
				{
					networkIdentity.HandleRemoteCall(message.componentIndex, message.functionHash, RemoteCallType.ClientRpc, networkReaderPooled, null);
				}
			}
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00005820 File Offset: 0x00003A20
		private static void OnObjectHide(ObjectHideMessage message)
		{
			NetworkClient.DestroyObject(message.netId);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x0000582D File Offset: 0x00003A2D
		internal static void OnObjectDestroy(ObjectDestroyMessage message)
		{
			NetworkClient.DestroyObject(message.netId);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000583C File Offset: 0x00003A3C
		internal static void OnSpawn(SpawnMessage message)
		{
			NetworkIdentity identity;
			if (NetworkClient.FindOrSpawnObject(message, out identity))
			{
				NetworkClient.ApplySpawnPayload(identity, message);
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0000585C File Offset: 0x00003A5C
		internal static void OnChangeOwner(ChangeOwnerMessage message)
		{
			NetworkIdentity existingObject = NetworkClient.GetExistingObject(message.netId);
			if (existingObject != null)
			{
				NetworkClient.ChangeOwner(existingObject, message);
				return;
			}
			Debug.LogError(string.Format("OnChangeOwner: Could not find object with netId {0}", message.netId));
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x000058A0 File Offset: 0x00003AA0
		internal static void ChangeOwner(NetworkIdentity identity, ChangeOwnerMessage message)
		{
			if (identity.isLocalPlayer && !message.isLocalPlayer)
			{
				identity.OnStopLocalPlayer();
			}
			identity.isOwned = message.isOwner;
			if (identity.isOwned)
			{
				NetworkConnection connection = NetworkClient.connection;
				if (connection != null)
				{
					connection.owned.Add(identity);
				}
			}
			else
			{
				NetworkConnection connection2 = NetworkClient.connection;
				if (connection2 != null)
				{
					connection2.owned.Remove(identity);
				}
			}
			identity.NotifyAuthority();
			identity.isLocalPlayer = message.isLocalPlayer;
			if (identity.isLocalPlayer)
			{
				NetworkClient.localPlayer = identity;
				identity.connectionToServer = NetworkClient.connection;
				identity.OnStartLocalPlayer();
				return;
			}
			if (NetworkClient.localPlayer == identity)
			{
				NetworkClient.localPlayer = null;
			}
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x0000594C File Offset: 0x00003B4C
		private static void InitializeIdentityFlags(NetworkIdentity identity)
		{
			identity.isClient = true;
			identity.isLocalPlayer = (NetworkClient.localPlayer == identity);
			if (identity.isLocalPlayer)
			{
				identity.connectionToServer = NetworkClient.connection;
			}
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00005979 File Offset: 0x00003B79
		private static void InvokeIdentityCallbacks(NetworkIdentity identity)
		{
			identity.NotifyAuthority();
			identity.OnStartClient();
			if (identity.isLocalPlayer)
			{
				identity.OnStartLocalPlayer();
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00005995 File Offset: 0x00003B95
		private static void BootstrapIdentity(NetworkIdentity identity)
		{
			NetworkClient.InitializeIdentityFlags(identity);
			NetworkClient.InvokeIdentityCallbacks(identity);
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x000059A4 File Offset: 0x00003BA4
		private static void BroadcastTimeSnapshot()
		{
			NetworkClient.Send<TimeSnapshotMessage>(default(TimeSnapshotMessage), 1);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x000059C0 File Offset: 0x00003BC0
		private static void Broadcast()
		{
			if (!NetworkClient.connection.isReady)
			{
				return;
			}
			if (NetworkServer.active)
			{
				return;
			}
			NetworkClient.BroadcastTimeSnapshot();
			foreach (NetworkIdentity networkIdentity in NetworkClient.connection.owned)
			{
				if (networkIdentity != null)
				{
					using (NetworkWriterPooled networkWriterPooled = NetworkWriterPool.Get())
					{
						networkIdentity.SerializeClient(networkWriterPooled);
						if (networkWriterPooled.Position > 0)
						{
							NetworkClient.Send<EntityStateMessage>(new EntityStateMessage
							{
								netId = networkIdentity.netId,
								payload = networkWriterPooled.ToArraySegment()
							}, 0);
						}
						continue;
					}
				}
				Debug.LogWarning("Found 'null' entry in owned list for client. This is unexpected behaviour.");
			}
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00005A94 File Offset: 0x00003C94
		internal static void NetworkEarlyUpdate()
		{
			if (Transport.active != null)
			{
				Transport.active.ClientEarlyUpdate();
			}
			NetworkClient.UpdateTimeInterpolation();
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00005AB4 File Offset: 0x00003CB4
		internal static void NetworkLateUpdate()
		{
			if (NetworkClient.active && (!Application.isPlaying || AccurateInterval.Elapsed(NetworkTime.localTime, (double)NetworkClient.sendInterval, ref NetworkClient.lastSendTime)))
			{
				NetworkClient.Broadcast();
			}
			LocalConnectionToServer localConnectionToServer = NetworkClient.connection as LocalConnectionToServer;
			if (localConnectionToServer != null)
			{
				localConnectionToServer.Update();
			}
			else
			{
				NetworkConnectionToServer networkConnectionToServer = NetworkClient.connection as NetworkConnectionToServer;
				if (networkConnectionToServer != null && NetworkClient.active && NetworkClient.connectState == ConnectState.Connected)
				{
					NetworkTime.UpdateClient();
					networkConnectionToServer.Update();
				}
			}
			if (Transport.active != null)
			{
				Transport.active.ClientLateUpdate();
			}
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00005B40 File Offset: 0x00003D40
		public static void DestroyAllClientObjects()
		{
			try
			{
				foreach (NetworkIdentity networkIdentity in NetworkClient.spawned.Values)
				{
					if (networkIdentity != null && networkIdentity.gameObject != null)
					{
						if (networkIdentity.isLocalPlayer)
						{
							networkIdentity.OnStopLocalPlayer();
						}
						networkIdentity.OnStopClient();
						bool flag = networkIdentity.connectionToServer is LocalConnectionToServer;
						if (!networkIdentity.isServer || flag)
						{
							if (NetworkClient.InvokeUnSpawnHandler(networkIdentity.assetId, networkIdentity.gameObject))
							{
								networkIdentity.Reset();
							}
							else if (networkIdentity.sceneId != 0UL)
							{
								networkIdentity.Reset();
								networkIdentity.gameObject.SetActive(false);
							}
							else
							{
								UnityEngine.Object.Destroy(networkIdentity.gameObject);
							}
						}
					}
				}
				NetworkClient.spawned.Clear();
				NetworkConnection connection = NetworkClient.connection;
				if (connection != null)
				{
					connection.owned.Clear();
				}
			}
			catch (InvalidOperationException exception)
			{
				Debug.LogException(exception);
				Debug.LogError("Could not DestroyAllClientObjects because spawned list was modified during loop, make sure you are not modifying NetworkIdentity.spawned by calling NetworkServer.Destroy or NetworkServer.Spawn in OnDestroy or OnDisable.");
			}
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00005C60 File Offset: 0x00003E60
		private static void DestroyObject(uint netId)
		{
			NetworkIdentity networkIdentity;
			if (NetworkClient.spawned.TryGetValue(netId, out networkIdentity) && networkIdentity != null)
			{
				if (networkIdentity.isLocalPlayer)
				{
					networkIdentity.OnStopLocalPlayer();
				}
				networkIdentity.OnStopClient();
				if (NetworkClient.InvokeUnSpawnHandler(networkIdentity.assetId, networkIdentity.gameObject))
				{
					networkIdentity.Reset();
				}
				else if (networkIdentity.sceneId == 0UL)
				{
					UnityEngine.Object.Destroy(networkIdentity.gameObject);
				}
				else
				{
					networkIdentity.gameObject.SetActive(false);
					NetworkClient.spawnableObjects[networkIdentity.sceneId] = networkIdentity;
					networkIdentity.Reset();
				}
				NetworkClient.connection.owned.Remove(networkIdentity);
				NetworkClient.spawned.Remove(netId);
			}
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00005D10 File Offset: 0x00003F10
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void Shutdown()
		{
			NetworkClient.DestroyAllClientObjects();
			NetworkClient.ClearSpawners();
			NetworkClient.spawned.Clear();
			NetworkConnection connection = NetworkClient.connection;
			if (connection != null)
			{
				connection.owned.Clear();
			}
			NetworkClient.handlers.Clear();
			NetworkClient.spawnableObjects.Clear();
			NetworkIdentity.ResetClientStatics();
			if (Transport.active != null)
			{
				Transport.active.ClientDisconnect();
			}
			NetworkClient.connectState = ConnectState.None;
			NetworkClient.connection = null;
			NetworkClient.localPlayer = null;
			NetworkClient.ready = false;
			NetworkClient.isSpawnFinished = false;
			NetworkClient.isLoadingScene = false;
			NetworkClient.lastSendTime = 0.0;
			NetworkClient.unbatcher = new Unbatcher();
			NetworkClient.OnConnectedEvent = null;
			NetworkClient.OnDisconnectedEvent = null;
			NetworkClient.OnErrorEvent = null;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00005DC4 File Offset: 0x00003FC4
		public static void OnGUI()
		{
			if (!NetworkClient.ready)
			{
				return;
			}
			GUILayout.BeginArea(new Rect(10f, 5f, 800f, 50f));
			GUILayout.BeginHorizontal("Box", Array.Empty<GUILayoutOption>());
			GUILayout.Label("Snapshot Interp.:", Array.Empty<GUILayoutOption>());
			if (NetworkClient.localTimescale > 1.0)
			{
				GUI.color = Color.green;
			}
			else if (NetworkClient.localTimescale < 1.0)
			{
				GUI.color = Color.red;
			}
			else
			{
				GUI.color = Color.white;
			}
			GUILayout.Box(string.Format("timeline: {0:F2}", NetworkClient.localTimeline), Array.Empty<GUILayoutOption>());
			GUILayout.Box(string.Format("buffer: {0}", NetworkClient.snapshots.Count), Array.Empty<GUILayoutOption>());
			GUILayout.Box(string.Format("DriftEMA: {0:F2}", NetworkClient.driftEma.Value), Array.Empty<GUILayoutOption>());
			GUILayout.Box(string.Format("DelTimeEMA: {0:F2}", NetworkClient.deliveryTimeEma.Value), Array.Empty<GUILayoutOption>());
			GUILayout.Box(string.Format("timescale: {0:F2}", NetworkClient.localTimescale), Array.Empty<GUILayoutOption>());
			GUILayout.Box(string.Format("BTM: {0:F2}", NetworkClient.bufferTimeMultiplier), Array.Empty<GUILayoutOption>());
			GUILayout.Box(string.Format("RTT: {0:F0}ms", NetworkTime.rtt * 1000.0), Array.Empty<GUILayoutOption>());
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00005F51 File Offset: 0x00004151
		[Obsolete("NetworkClient snapshot interpolation settings were moved to NetworkClient.snapshotSettings.*")]
		public static float catchupNegativeThreshold
		{
			get
			{
				return NetworkClient.snapshotSettings.catchupNegativeThreshold;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000102 RID: 258 RVA: 0x00005F5D File Offset: 0x0000415D
		[Obsolete("NetworkClient snapshot interpolation settings were moved to NetworkClient.snapshotSettings.*")]
		public static float catchupPositiveThreshold
		{
			get
			{
				return NetworkClient.snapshotSettings.catchupPositiveThreshold;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00005F69 File Offset: 0x00004169
		[Obsolete("NetworkClient snapshot interpolation settings were moved to NetworkClient.snapshotSettings.*")]
		public static double catchupSpeed
		{
			get
			{
				return NetworkClient.snapshotSettings.catchupSpeed;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00005F75 File Offset: 0x00004175
		[Obsolete("NetworkClient snapshot interpolation settings were moved to NetworkClient.snapshotSettings.*")]
		public static double slowdownSpeed
		{
			get
			{
				return NetworkClient.snapshotSettings.slowdownSpeed;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00005F81 File Offset: 0x00004181
		[Obsolete("NetworkClient snapshot interpolation settings were moved to NetworkClient.snapshotSettings.*")]
		public static int driftEmaDuration
		{
			get
			{
				return NetworkClient.snapshotSettings.driftEmaDuration;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000106 RID: 262 RVA: 0x00005F8D File Offset: 0x0000418D
		public static double initialBufferTime
		{
			get
			{
				return (double)NetworkServer.sendInterval * NetworkClient.snapshotSettings.bufferTimeMultiplier;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000107 RID: 263 RVA: 0x00005FA0 File Offset: 0x000041A0
		public static double bufferTime
		{
			get
			{
				return (double)NetworkServer.sendInterval * NetworkClient.bufferTimeMultiplier;
			}
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00005FB0 File Offset: 0x000041B0
		private static void InitTimeInterpolation()
		{
			NetworkClient.bufferTimeMultiplier = NetworkClient.snapshotSettings.bufferTimeMultiplier;
			NetworkClient.localTimeline = 0.0;
			NetworkClient.localTimescale = 1.0;
			NetworkClient.snapshots.Clear();
			NetworkClient.driftEma = new ExponentialMovingAverage(NetworkServer.sendRate * NetworkClient.snapshotSettings.driftEmaDuration);
			NetworkClient.deliveryTimeEma = new ExponentialMovingAverage(NetworkServer.sendRate * NetworkClient.snapshotSettings.deliveryTimeEmaDuration);
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00006026 File Offset: 0x00004226
		private static void OnTimeSnapshotMessage(TimeSnapshotMessage _)
		{
			NetworkClient.OnTimeSnapshot(new TimeSnapshot(NetworkClient.connection.remoteTimeStamp, NetworkTime.localTime));
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00006044 File Offset: 0x00004244
		public static void OnTimeSnapshot(TimeSnapshot snap)
		{
			if (NetworkClient.snapshotSettings.dynamicAdjustment)
			{
				NetworkClient.bufferTimeMultiplier = SnapshotInterpolation.DynamicAdjustment((double)NetworkServer.sendInterval, NetworkClient.deliveryTimeEma.StandardDeviation, (double)NetworkClient.snapshotSettings.dynamicAdjustmentTolerance);
			}
			SnapshotInterpolation.InsertAndAdjust<TimeSnapshot>(NetworkClient.snapshots, NetworkClient.snapshotSettings.bufferLimit, snap, ref NetworkClient.localTimeline, ref NetworkClient.localTimescale, NetworkServer.sendInterval, NetworkClient.bufferTime, NetworkClient.snapshotSettings.catchupSpeed, NetworkClient.snapshotSettings.slowdownSpeed, ref NetworkClient.driftEma, NetworkClient.snapshotSettings.catchupNegativeThreshold, NetworkClient.snapshotSettings.catchupPositiveThreshold, ref NetworkClient.deliveryTimeEma);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x000060E0 File Offset: 0x000042E0
		private static void UpdateTimeInterpolation()
		{
			if (NetworkClient.snapshots.Count > 0)
			{
				SnapshotInterpolation.StepTime((double)Time.unscaledDeltaTime, ref NetworkClient.localTimeline, NetworkClient.localTimescale);
				TimeSnapshot timeSnapshot;
				TimeSnapshot timeSnapshot2;
				double num;
				SnapshotInterpolation.StepInterpolation<TimeSnapshot>(NetworkClient.snapshots, NetworkClient.localTimeline, out timeSnapshot, out timeSnapshot2, out num);
			}
		}

		// Token: 0x04000067 RID: 103
		private static double lastSendTime;

		// Token: 0x04000068 RID: 104
		internal static readonly Dictionary<ushort, NetworkMessageDelegate> handlers = new Dictionary<ushort, NetworkMessageDelegate>();

		// Token: 0x04000069 RID: 105
		public static readonly Dictionary<uint, NetworkIdentity> spawned = new Dictionary<uint, NetworkIdentity>();

		// Token: 0x0400006B RID: 107
		public static bool ready;

		// Token: 0x0400006D RID: 109
		internal static ConnectState connectState = ConnectState.None;

		// Token: 0x0400006E RID: 110
		public static Action OnConnectedEvent;

		// Token: 0x0400006F RID: 111
		public static Action OnDisconnectedEvent;

		// Token: 0x04000070 RID: 112
		public static Action<TransportError, string> OnErrorEvent;

		// Token: 0x04000071 RID: 113
		public static readonly Dictionary<uint, GameObject> prefabs = new Dictionary<uint, GameObject>();

		// Token: 0x04000072 RID: 114
		internal static readonly Dictionary<uint, SpawnHandlerDelegate> spawnHandlers = new Dictionary<uint, SpawnHandlerDelegate>();

		// Token: 0x04000073 RID: 115
		internal static readonly Dictionary<uint, UnSpawnDelegate> unspawnHandlers = new Dictionary<uint, UnSpawnDelegate>();

		// Token: 0x04000074 RID: 116
		internal static bool isSpawnFinished;

		// Token: 0x04000075 RID: 117
		internal static readonly Dictionary<ulong, NetworkIdentity> spawnableObjects = new Dictionary<ulong, NetworkIdentity>();

		// Token: 0x04000076 RID: 118
		internal static Unbatcher unbatcher = new Unbatcher();

		// Token: 0x04000077 RID: 119
		public static InterestManagementBase aoi;

		// Token: 0x04000078 RID: 120
		public static bool isLoadingScene;

		// Token: 0x04000079 RID: 121
		public static ConnectionQuality connectionQuality = ConnectionQuality.ESTIMATING;

		// Token: 0x0400007A RID: 122
		public static SnapshotInterpolationSettings snapshotSettings = new SnapshotInterpolationSettings();

		// Token: 0x0400007B RID: 123
		public static double bufferTimeMultiplier;

		// Token: 0x0400007C RID: 124
		public static SortedList<double, TimeSnapshot> snapshots = new SortedList<double, TimeSnapshot>();

		// Token: 0x0400007D RID: 125
		internal static double localTimeline;

		// Token: 0x0400007E RID: 126
		internal static double localTimescale = 1.0;

		// Token: 0x0400007F RID: 127
		private static ExponentialMovingAverage driftEma;

		// Token: 0x04000080 RID: 128
		[Header("Snapshot Interpolation: Dynamic Adjustment")]
		[Tooltip("Automatically adjust bufferTimeMultiplier for smooth results.\nSets a low multiplier on stable connections, and a high multiplier on jittery connections.")]
		public static bool dynamicAdjustment = true;

		// Token: 0x04000081 RID: 129
		[Tooltip("Safety buffer that is always added to the dynamic bufferTimeMultiplier adjustment.")]
		public static float dynamicAdjustmentTolerance = 1f;

		// Token: 0x04000082 RID: 130
		[Tooltip("Dynamic adjustment is computed over n-second exponential moving average standard deviation.")]
		public static int deliveryTimeEmaDuration = 2;

		// Token: 0x04000083 RID: 131
		private static ExponentialMovingAverage deliveryTimeEma;
	}
}
