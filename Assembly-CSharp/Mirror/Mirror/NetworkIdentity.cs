using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Mirror.RemoteCalls;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mirror
{
	// Token: 0x02000045 RID: 69
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(-1)]
	[AddComponentMenu("Network/Network Identity")]
	[HelpURL("https://mirror-networking.gitbook.io/docs/components/network-identity")]
	public sealed class NetworkIdentity : MonoBehaviour
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000148 RID: 328 RVA: 0x00006993 File Offset: 0x00004B93
		// (set) Token: 0x06000149 RID: 329 RVA: 0x0000699B File Offset: 0x00004B9B
		public bool isClient { get; internal set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600014A RID: 330 RVA: 0x000069A4 File Offset: 0x00004BA4
		// (set) Token: 0x0600014B RID: 331 RVA: 0x000069AC File Offset: 0x00004BAC
		public bool isServer { get; internal set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600014C RID: 332 RVA: 0x000069B5 File Offset: 0x00004BB5
		// (set) Token: 0x0600014D RID: 333 RVA: 0x000069BD File Offset: 0x00004BBD
		public bool isLocalPlayer { get; internal set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600014E RID: 334 RVA: 0x000069C6 File Offset: 0x00004BC6
		public bool isServerOnly
		{
			get
			{
				return this.isServer && !this.isClient;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600014F RID: 335 RVA: 0x000069DB File Offset: 0x00004BDB
		public bool isClientOnly
		{
			get
			{
				return this.isClient && !this.isServer;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000150 RID: 336 RVA: 0x000069F0 File Offset: 0x00004BF0
		// (set) Token: 0x06000151 RID: 337 RVA: 0x000069F8 File Offset: 0x00004BF8
		public bool isOwned { get; internal set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000152 RID: 338 RVA: 0x00006A01 File Offset: 0x00004C01
		[Obsolete(".hasAuthority was renamed to .isOwned. This is easier to understand and prepares for SyncDirection, where there is a difference betwen isOwned and authority.")]
		public bool hasAuthority
		{
			get
			{
				return this.isOwned;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000153 RID: 339 RVA: 0x00006A09 File Offset: 0x00004C09
		// (set) Token: 0x06000154 RID: 340 RVA: 0x00006A11 File Offset: 0x00004C11
		public uint netId { get; internal set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000155 RID: 341 RVA: 0x00006A1A File Offset: 0x00004C1A
		// (set) Token: 0x06000156 RID: 342 RVA: 0x00006A22 File Offset: 0x00004C22
		public uint assetId
		{
			get
			{
				return this._assetId;
			}
			internal set
			{
				if (value == 0U)
				{
					Debug.LogError(string.Format("Can not set AssetId to empty guid on NetworkIdentity '{0}', old assetId '{1}'", base.name, this._assetId));
					return;
				}
				this._assetId = value;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000157 RID: 343 RVA: 0x00006A4F File Offset: 0x00004C4F
		// (set) Token: 0x06000158 RID: 344 RVA: 0x00006A57 File Offset: 0x00004C57
		public NetworkConnection connectionToServer { get; internal set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000159 RID: 345 RVA: 0x00006A60 File Offset: 0x00004C60
		// (set) Token: 0x0600015A RID: 346 RVA: 0x00006A68 File Offset: 0x00004C68
		public NetworkConnectionToClient connectionToClient
		{
			get
			{
				return this._connectionToClient;
			}
			internal set
			{
				NetworkConnectionToClient connectionToClient = this._connectionToClient;
				if (connectionToClient != null)
				{
					connectionToClient.RemoveOwnedObject(this);
				}
				this._connectionToClient = value;
				NetworkConnectionToClient connectionToClient2 = this._connectionToClient;
				if (connectionToClient2 == null)
				{
					return;
				}
				connectionToClient2.AddOwnedObject(this);
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600015B RID: 347 RVA: 0x00006A94 File Offset: 0x00004C94
		// (set) Token: 0x0600015C RID: 348 RVA: 0x00006A9C File Offset: 0x00004C9C
		public NetworkBehaviour[] NetworkBehaviours { get; private set; }

		// Token: 0x0600015D RID: 349 RVA: 0x00006AA8 File Offset: 0x00004CA8
		internal void HandleRemoteCall(byte componentIndex, ushort functionHash, RemoteCallType remoteCallType, NetworkReader reader, NetworkConnectionToClient senderConnection = null)
		{
			if (this == null)
			{
				Debug.LogWarning(string.Format("{0} [{1}] received for deleted object [netId={2}]", remoteCallType, functionHash, this.netId));
				return;
			}
			if ((int)componentIndex >= this.NetworkBehaviours.Length)
			{
				Debug.LogWarning(string.Format("Component [{0}] not found for [netId={1}]", componentIndex, this.netId));
				return;
			}
			NetworkBehaviour component = this.NetworkBehaviours[(int)componentIndex];
			if (!RemoteProcedureCalls.Invoke(functionHash, remoteCallType, reader, component, senderConnection))
			{
				Debug.LogError(string.Format("Found no receiver for incoming {0} [{1}] on {2}, the server and client should have the same NetworkBehaviour instances [netId={3}].", new object[]
				{
					remoteCallType,
					functionHash,
					base.gameObject.name,
					this.netId
				}));
			}
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00006B6B File Offset: 0x00004D6B
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		internal static void ResetStatics()
		{
			NetworkIdentity.ResetClientStatics();
			NetworkIdentity.ResetServerStatics();
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00006B77 File Offset: 0x00004D77
		internal static void ResetClientStatics()
		{
			NetworkIdentity.previousLocalPlayer = null;
			NetworkIdentity.clientAuthorityCallback = null;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00006B85 File Offset: 0x00004D85
		internal static void ResetServerStatics()
		{
			NetworkIdentity.nextNetworkId = 1U;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00006B8D File Offset: 0x00004D8D
		public static NetworkIdentity GetSceneIdentity(ulong id)
		{
			return NetworkIdentity.sceneIds[id];
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00006B9A File Offset: 0x00004D9A
		internal static uint GetNextNetworkId()
		{
			return NetworkIdentity.nextNetworkId++;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00006B85 File Offset: 0x00004D85
		public static void ResetNextNetworkId()
		{
			NetworkIdentity.nextNetworkId = 1U;
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000164 RID: 356 RVA: 0x00006BAC File Offset: 0x00004DAC
		// (remove) Token: 0x06000165 RID: 357 RVA: 0x00006BE0 File Offset: 0x00004DE0
		public static event NetworkIdentity.ClientAuthorityCallback clientAuthorityCallback;

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000166 RID: 358 RVA: 0x00006C13 File Offset: 0x00004E13
		// (set) Token: 0x06000167 RID: 359 RVA: 0x00006C1B File Offset: 0x00004E1B
		public bool SpawnedFromInstantiate { get; private set; }

		// Token: 0x06000168 RID: 360 RVA: 0x00006C24 File Offset: 0x00004E24
		internal void InitializeNetworkBehaviours()
		{
			this.NetworkBehaviours = base.GetComponentsInChildren<NetworkBehaviour>(true);
			this.ValidateComponents();
			for (int i = 0; i < this.NetworkBehaviours.Length; i++)
			{
				NetworkBehaviour networkBehaviour = this.NetworkBehaviours[i];
				networkBehaviour.netIdentity = this;
				networkBehaviour.ComponentIndex = (byte)i;
			}
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00006C70 File Offset: 0x00004E70
		private void ValidateComponents()
		{
			if (this.NetworkBehaviours == null)
			{
				Debug.LogError("NetworkBehaviours array is null on " + base.gameObject.name + "!\nTypically this can happen when a networked object is a child of a non-networked parent that's disabled, preventing Awake on the networked object from being invoked, where the NetworkBehaviours array is initialized.", base.gameObject);
				return;
			}
			if (this.NetworkBehaviours.Length > 64)
			{
				Debug.LogError(string.Format("NetworkIdentity {0} has too many NetworkBehaviour components: only {1} NetworkBehaviour components are allowed in order to save bandwidth.", base.name, 64), this);
			}
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00006CD4 File Offset: 0x00004ED4
		internal void Awake()
		{
			this.InitializeNetworkBehaviours();
			if (this.hasSpawned)
			{
				Debug.LogError(base.name + " has already spawned. Don't call Instantiate for NetworkIdentities that were in the scene since the beginning (aka scene objects).  Otherwise the client won't know which object to use for a SpawnSceneObject message.");
				this.SpawnedFromInstantiate = true;
				UnityEngine.Object.Destroy(base.gameObject);
			}
			this.hasSpawned = true;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00006D12 File Offset: 0x00004F12
		private void OnValidate()
		{
			this.hasSpawned = false;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00006D1B File Offset: 0x00004F1B
		public static uint AssetGuidToUint(Guid guid)
		{
			return (uint)guid.GetHashCode();
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00006D2C File Offset: 0x00004F2C
		private void OnDestroy()
		{
			if (this.SpawnedFromInstantiate)
			{
				return;
			}
			if (this.isServer && !this.destroyCalled)
			{
				NetworkServer.Destroy(base.gameObject);
			}
			if (this.isLocalPlayer && NetworkClient.localPlayer == this)
			{
				NetworkClient.localPlayer = null;
			}
			if (this.isClient)
			{
				if (NetworkClient.connection != null)
				{
					NetworkClient.connection.owned.Remove(this);
				}
				NetworkClient.spawned.Remove(this.netId);
			}
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00006DAC File Offset: 0x00004FAC
		internal void OnStartServer()
		{
			foreach (NetworkBehaviour networkBehaviour in this.NetworkBehaviours)
			{
				try
				{
					networkBehaviour.OnStartServer();
				}
				catch (Exception exception)
				{
					Debug.LogException(exception, networkBehaviour);
				}
			}
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00006DF4 File Offset: 0x00004FF4
		internal void OnStopServer()
		{
			foreach (NetworkBehaviour networkBehaviour in this.NetworkBehaviours)
			{
				try
				{
					networkBehaviour.OnStopServer();
				}
				catch (Exception exception)
				{
					Debug.LogException(exception, networkBehaviour);
				}
			}
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00006E3C File Offset: 0x0000503C
		internal void OnStartClient()
		{
			if (this.clientStarted)
			{
				return;
			}
			this.clientStarted = true;
			foreach (NetworkBehaviour networkBehaviour in this.NetworkBehaviours)
			{
				try
				{
					networkBehaviour.OnStartClient();
				}
				catch (Exception exception)
				{
					Debug.LogException(exception, networkBehaviour);
				}
			}
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00006E94 File Offset: 0x00005094
		internal void OnStopClient()
		{
			if (!this.clientStarted)
			{
				return;
			}
			foreach (NetworkBehaviour networkBehaviour in this.NetworkBehaviours)
			{
				try
				{
					networkBehaviour.OnStopClient();
				}
				catch (Exception exception)
				{
					Debug.LogException(exception, networkBehaviour);
				}
			}
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00006EE4 File Offset: 0x000050E4
		internal void OnStartLocalPlayer()
		{
			if (NetworkIdentity.previousLocalPlayer == this)
			{
				return;
			}
			NetworkIdentity.previousLocalPlayer = this;
			foreach (NetworkBehaviour networkBehaviour in this.NetworkBehaviours)
			{
				try
				{
					networkBehaviour.OnStartLocalPlayer();
				}
				catch (Exception exception)
				{
					Debug.LogException(exception, networkBehaviour);
				}
			}
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00006F40 File Offset: 0x00005140
		internal void OnStopLocalPlayer()
		{
			foreach (NetworkBehaviour networkBehaviour in this.NetworkBehaviours)
			{
				try
				{
					networkBehaviour.OnStopLocalPlayer();
				}
				catch (Exception exception)
				{
					Debug.LogException(exception, networkBehaviour);
				}
			}
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00006F88 File Offset: 0x00005188
		private ValueTuple<ulong, ulong> ServerDirtyMasks(bool initialState)
		{
			ulong num = 0UL;
			ulong num2 = 0UL;
			NetworkBehaviour[] networkBehaviours = this.NetworkBehaviours;
			for (int i = 0; i < networkBehaviours.Length; i++)
			{
				NetworkBehaviour networkBehaviour = networkBehaviours[i];
				bool flag = networkBehaviour.IsDirty();
				ulong num3 = 1UL << (i & 31);
				if (initialState || (networkBehaviour.syncDirection == SyncDirection.ServerToClient && flag))
				{
					num |= num3;
				}
				if (networkBehaviour.syncMode == SyncMode.Observers && (initialState || flag))
				{
					num2 |= num3;
				}
			}
			return new ValueTuple<ulong, ulong>(num, num2);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00006FFC File Offset: 0x000051FC
		private ulong ClientDirtyMask()
		{
			ulong num = 0UL;
			NetworkBehaviour[] networkBehaviours = this.NetworkBehaviours;
			for (int i = 0; i < networkBehaviours.Length; i++)
			{
				NetworkBehaviour networkBehaviour = networkBehaviours[i];
				if (this.isOwned && networkBehaviour.syncDirection == SyncDirection.ClientToServer && networkBehaviour.IsDirty())
				{
					num |= 1UL << (i & 31);
				}
			}
			return num;
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0000704C File Offset: 0x0000524C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool IsDirty(ulong mask, int index)
		{
			ulong num = (ulong)(1L << (index & 31));
			return (mask & num) > 0UL;
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00007068 File Offset: 0x00005268
		internal void SerializeServer(bool initialState, NetworkWriter ownerWriter, NetworkWriter observersWriter)
		{
			this.ValidateComponents();
			NetworkBehaviour[] networkBehaviours = this.NetworkBehaviours;
			ValueTuple<ulong, ulong> valueTuple = this.ServerDirtyMasks(initialState);
			ulong item = valueTuple.Item1;
			ulong item2 = valueTuple.Item2;
			if (item != 0UL)
			{
				Compression.CompressVarUInt(ownerWriter, item);
			}
			if (item2 != 0UL)
			{
				Compression.CompressVarUInt(observersWriter, item2);
			}
			if ((item | item2) != 0UL)
			{
				for (int i = 0; i < networkBehaviours.Length; i++)
				{
					NetworkBehaviour networkBehaviour = networkBehaviours[i];
					bool flag = NetworkIdentity.IsDirty(item, i);
					bool flag2 = NetworkIdentity.IsDirty(item2, i);
					if (flag || flag2)
					{
						using (NetworkWriterPooled networkWriterPooled = NetworkWriterPool.Get())
						{
							networkBehaviour.Serialize(networkWriterPooled, initialState);
							ArraySegment<byte> arraySegment = networkWriterPooled.ToArraySegment();
							if (flag)
							{
								ownerWriter.WriteBytes(arraySegment.Array, arraySegment.Offset, arraySegment.Count);
							}
							if (flag2)
							{
								observersWriter.WriteBytes(arraySegment.Array, arraySegment.Offset, arraySegment.Count);
							}
						}
						if (!initialState)
						{
							networkBehaviour.ClearAllDirtyBits();
						}
					}
				}
			}
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00007164 File Offset: 0x00005364
		internal void SerializeClient(NetworkWriter writer)
		{
			this.ValidateComponents();
			NetworkBehaviour[] networkBehaviours = this.NetworkBehaviours;
			ulong num = this.ClientDirtyMask();
			if (num != 0UL)
			{
				Compression.CompressVarUInt(writer, num);
			}
			if (num != 0UL)
			{
				for (int i = 0; i < networkBehaviours.Length; i++)
				{
					NetworkBehaviour networkBehaviour = networkBehaviours[i];
					if (NetworkIdentity.IsDirty(num, i))
					{
						networkBehaviour.Serialize(writer, false);
						networkBehaviour.ClearAllDirtyBits();
					}
				}
			}
		}

		// Token: 0x06000179 RID: 377 RVA: 0x000071BC File Offset: 0x000053BC
		internal bool DeserializeServer(NetworkReader reader)
		{
			this.ValidateComponents();
			NetworkBehaviour[] networkBehaviours = this.NetworkBehaviours;
			ulong mask = Compression.DecompressVarUInt(reader);
			for (int i = 0; i < networkBehaviours.Length; i++)
			{
				if (NetworkIdentity.IsDirty(mask, i))
				{
					NetworkBehaviour networkBehaviour = networkBehaviours[i];
					if (networkBehaviour.syncDirection == SyncDirection.ClientToServer)
					{
						if (!networkBehaviour.Deserialize(reader, false))
						{
							return false;
						}
						networkBehaviour.SetDirty();
					}
				}
			}
			return true;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00007214 File Offset: 0x00005414
		internal void DeserializeClient(NetworkReader reader, bool initialState)
		{
			this.ValidateComponents();
			NetworkBehaviour[] networkBehaviours = this.NetworkBehaviours;
			ulong mask = Compression.DecompressVarUInt(reader);
			for (int i = 0; i < networkBehaviours.Length; i++)
			{
				if (NetworkIdentity.IsDirty(mask, i))
				{
					networkBehaviours[i].Deserialize(reader, initialState);
				}
			}
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00007258 File Offset: 0x00005458
		internal NetworkIdentitySerialization GetServerSerializationAtTick(int tick)
		{
			if (this.lastSerialization.tick != tick)
			{
				this.lastSerialization.ownerWriter.Position = 0;
				this.lastSerialization.observersWriter.Position = 0;
				this.SerializeServer(false, this.lastSerialization.ownerWriter, this.lastSerialization.observersWriter);
				this.lastSerialization.tick = tick;
			}
			return this.lastSerialization;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x000072C4 File Offset: 0x000054C4
		internal void AddObserver(NetworkConnectionToClient conn)
		{
			if (this.observers.ContainsKey(conn.connectionId))
			{
				return;
			}
			if (this.observers.Count == 0)
			{
				this.ClearAllComponentsDirtyBits();
			}
			this.observers[conn.connectionId] = conn;
			conn.AddToObserving(this);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00007314 File Offset: 0x00005514
		internal void ClearAllComponentsDirtyBits()
		{
			NetworkBehaviour[] networkBehaviours = this.NetworkBehaviours;
			for (int i = 0; i < networkBehaviours.Length; i++)
			{
				networkBehaviours[i].ClearAllDirtyBits();
			}
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0000733E File Offset: 0x0000553E
		internal void RemoveObserver(NetworkConnection conn)
		{
			this.observers.Remove(conn.connectionId);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00007354 File Offset: 0x00005554
		public bool AssignClientAuthority(NetworkConnectionToClient conn)
		{
			if (!this.isServer)
			{
				Debug.LogError("AssignClientAuthority can only be called on the server for spawned objects.");
				return false;
			}
			if (conn == null)
			{
				Debug.LogError(string.Format("AssignClientAuthority for {0} owner cannot be null. Use RemoveClientAuthority() instead.", base.gameObject));
				return false;
			}
			if (this.connectionToClient != null && conn != this.connectionToClient)
			{
				Debug.LogError(string.Format("AssignClientAuthority for {0} already has an owner. Use RemoveClientAuthority() first.", base.gameObject));
				return false;
			}
			this.SetClientOwner(conn);
			NetworkServer.SendChangeOwnerMessage(this, conn);
			NetworkIdentity.ClientAuthorityCallback clientAuthorityCallback = NetworkIdentity.clientAuthorityCallback;
			if (clientAuthorityCallback != null)
			{
				clientAuthorityCallback(conn, this, true);
			}
			return true;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x000073D9 File Offset: 0x000055D9
		internal void SetClientOwner(NetworkConnectionToClient conn)
		{
			if (this.connectionToClient != null && conn != this.connectionToClient)
			{
				Debug.LogError(string.Format("Object {0} netId={1} already has an owner. Use RemoveClientAuthority() first", this, this.netId), this);
				return;
			}
			this.connectionToClient = conn;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00007410 File Offset: 0x00005610
		public void RemoveClientAuthority()
		{
			if (!this.isServer)
			{
				Debug.LogError("RemoveClientAuthority can only be called on the server for spawned objects.");
				return;
			}
			NetworkConnectionToClient connectionToClient = this.connectionToClient;
			if (((connectionToClient != null) ? connectionToClient.identity : null) == this)
			{
				Debug.LogError("RemoveClientAuthority cannot remove authority for a player object");
				return;
			}
			if (this.connectionToClient != null)
			{
				NetworkIdentity.ClientAuthorityCallback clientAuthorityCallback = NetworkIdentity.clientAuthorityCallback;
				if (clientAuthorityCallback != null)
				{
					clientAuthorityCallback(this.connectionToClient, this, false);
				}
				NetworkConnectionToClient connectionToClient2 = this.connectionToClient;
				this.connectionToClient = null;
				NetworkServer.SendChangeOwnerMessage(this, connectionToClient2);
			}
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000748C File Offset: 0x0000568C
		internal void Reset()
		{
			this.hasSpawned = false;
			this.clientStarted = false;
			this.isClient = false;
			this.isServer = false;
			this.isOwned = false;
			this.NotifyAuthority();
			this.netId = 0U;
			this.connectionToServer = null;
			this.connectionToClient = null;
			this.ClearObservers();
			if (this.isLocalPlayer && NetworkClient.localPlayer == this)
			{
				NetworkClient.localPlayer = null;
			}
			NetworkIdentity.previousLocalPlayer = null;
			this.isLocalPlayer = false;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00007505 File Offset: 0x00005705
		internal void NotifyAuthority()
		{
			if (!this.hadAuthority && this.isOwned)
			{
				this.OnStartAuthority();
			}
			if (this.hadAuthority && !this.isOwned)
			{
				this.OnStopAuthority();
			}
			this.hadAuthority = this.isOwned;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00007540 File Offset: 0x00005740
		internal void OnStartAuthority()
		{
			foreach (NetworkBehaviour networkBehaviour in this.NetworkBehaviours)
			{
				try
				{
					networkBehaviour.OnStartAuthority();
				}
				catch (Exception exception)
				{
					Debug.LogException(exception, networkBehaviour);
				}
			}
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00007588 File Offset: 0x00005788
		internal void OnStopAuthority()
		{
			foreach (NetworkBehaviour networkBehaviour in this.NetworkBehaviours)
			{
				try
				{
					networkBehaviour.OnStopAuthority();
				}
				catch (Exception exception)
				{
					Debug.LogException(exception, networkBehaviour);
				}
			}
		}

		// Token: 0x06000186 RID: 390 RVA: 0x000075D0 File Offset: 0x000057D0
		internal void ClearObservers()
		{
			foreach (NetworkConnectionToClient networkConnectionToClient in this.observers.Values)
			{
				networkConnectionToClient.RemoveFromObserving(this, true);
			}
			this.observers.Clear();
		}

		// Token: 0x040000B7 RID: 183
		public readonly Dictionary<int, NetworkConnectionToClient> observers = new Dictionary<int, NetworkConnectionToClient>();

		// Token: 0x040000B9 RID: 185
		[FormerlySerializedAs("m_SceneId")]
		[HideInInspector]
		public ulong sceneId;

		// Token: 0x040000BA RID: 186
		[SerializeField]
		private uint _assetId;

		// Token: 0x040000BB RID: 187
		[FormerlySerializedAs("m_ServerOnly")]
		[Tooltip("Prevents this object from being spawned / enabled on clients")]
		public bool serverOnly;

		// Token: 0x040000BC RID: 188
		internal bool destroyCalled;

		// Token: 0x040000BE RID: 190
		private NetworkConnectionToClient _connectionToClient;

		// Token: 0x040000C0 RID: 192
		private const int MaxNetworkBehaviours = 64;

		// Token: 0x040000C1 RID: 193
		[Tooltip("Visibility can overwrite interest management. ForceHidden can be useful to hide monsters while they respawn. ForceShown can be useful for score NetworkIdentities that should always broadcast to everyone in the world.")]
		public Visibility visible;

		// Token: 0x040000C2 RID: 194
		private NetworkIdentitySerialization lastSerialization = new NetworkIdentitySerialization
		{
			ownerWriter = new NetworkWriter(),
			observersWriter = new NetworkWriter()
		};

		// Token: 0x040000C3 RID: 195
		private static readonly Dictionary<ulong, NetworkIdentity> sceneIds = new Dictionary<ulong, NetworkIdentity>();

		// Token: 0x040000C4 RID: 196
		private static uint nextNetworkId = 1U;

		// Token: 0x040000C6 RID: 198
		[SerializeField]
		[HideInInspector]
		private bool hasSpawned;

		// Token: 0x040000C8 RID: 200
		private bool clientStarted;

		// Token: 0x040000C9 RID: 201
		internal static NetworkIdentity previousLocalPlayer = null;

		// Token: 0x040000CA RID: 202
		private bool hadAuthority;

		// Token: 0x02000046 RID: 70
		// (Invoke) Token: 0x0600018A RID: 394
		public delegate void ClientAuthorityCallback(NetworkConnectionToClient conn, NetworkIdentity identity, bool authorityState);
	}
}
