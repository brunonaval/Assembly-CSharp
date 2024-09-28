using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Mirror
{
	// Token: 0x02000033 RID: 51
	[AddComponentMenu("")]
	[HelpURL("https://mirror-networking.gitbook.io/docs/guides/networkbehaviour")]
	public abstract class NetworkBehaviour : MonoBehaviour
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00003000 File Offset: 0x00001200
		public bool isServer
		{
			get
			{
				return this.netIdentity.isServer;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000064 RID: 100 RVA: 0x0000300D File Offset: 0x0000120D
		public bool isClient
		{
			get
			{
				return this.netIdentity.isClient;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000065 RID: 101 RVA: 0x0000301A File Offset: 0x0000121A
		public bool isLocalPlayer
		{
			get
			{
				return this.netIdentity.isLocalPlayer;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00003027 File Offset: 0x00001227
		public bool isServerOnly
		{
			get
			{
				return this.netIdentity.isServerOnly;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00003034 File Offset: 0x00001234
		public bool isClientOnly
		{
			get
			{
				return this.netIdentity.isClientOnly;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00003041 File Offset: 0x00001241
		public bool isOwned
		{
			get
			{
				return this.netIdentity.isOwned;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000069 RID: 105 RVA: 0x0000304E File Offset: 0x0000124E
		[Obsolete(".hasAuthority was renamed to .isOwned. This is easier to understand and prepares for SyncDirection, where there is a difference betwen isOwned and authority.")]
		public bool hasAuthority
		{
			get
			{
				return this.isOwned;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600006A RID: 106 RVA: 0x00003056 File Offset: 0x00001256
		public bool authority
		{
			get
			{
				if (!this.isClient)
				{
					return this.syncDirection == SyncDirection.ServerToClient;
				}
				return this.syncDirection == SyncDirection.ClientToServer && this.isOwned;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600006B RID: 107 RVA: 0x0000307B File Offset: 0x0000127B
		public uint netId
		{
			get
			{
				return this.netIdentity.netId;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00003088 File Offset: 0x00001288
		public NetworkConnection connectionToServer
		{
			get
			{
				return this.netIdentity.connectionToServer;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00003095 File Offset: 0x00001295
		public NetworkConnectionToClient connectionToClient
		{
			get
			{
				return this.netIdentity.connectionToClient;
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000030A2 File Offset: 0x000012A2
		internal bool HasSyncObjects()
		{
			return this.syncObjects.Count > 0;
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600006F RID: 111 RVA: 0x000030B2 File Offset: 0x000012B2
		// (set) Token: 0x06000070 RID: 112 RVA: 0x000030BA File Offset: 0x000012BA
		public NetworkIdentity netIdentity { get; internal set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000071 RID: 113 RVA: 0x000030C3 File Offset: 0x000012C3
		// (set) Token: 0x06000072 RID: 114 RVA: 0x000030CB File Offset: 0x000012CB
		public byte ComponentIndex { get; internal set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000073 RID: 115 RVA: 0x000030D4 File Offset: 0x000012D4
		// (set) Token: 0x06000074 RID: 116 RVA: 0x000030DC File Offset: 0x000012DC
		private protected ulong syncVarDirtyBits { protected get; private set; }

		// Token: 0x06000075 RID: 117 RVA: 0x000030E5 File Offset: 0x000012E5
		protected bool GetSyncVarHookGuard(ulong dirtyBit)
		{
			return (this.syncVarHookGuard & dirtyBit) > 0UL;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000030F3 File Offset: 0x000012F3
		protected void SetSyncVarHookGuard(ulong dirtyBit, bool value)
		{
			if (value)
			{
				this.syncVarHookGuard |= dirtyBit;
				return;
			}
			this.syncVarHookGuard &= ~dirtyBit;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003116 File Offset: 0x00001316
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void SetSyncObjectDirtyBit(ulong dirtyBit)
		{
			this.syncObjectDirtyBits |= dirtyBit;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003126 File Offset: 0x00001326
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetSyncVarDirtyBit(ulong dirtyBit)
		{
			this.syncVarDirtyBits |= dirtyBit;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003136 File Offset: 0x00001336
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetDirty()
		{
			this.SetSyncVarDirtyBit(ulong.MaxValue);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003140 File Offset: 0x00001340
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsDirty()
		{
			return (this.syncVarDirtyBits | this.syncObjectDirtyBits) != 0UL && NetworkTime.localTime - this.lastSyncTime >= (double)this.syncInterval;
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0000316C File Offset: 0x0000136C
		public void ClearAllDirtyBits()
		{
			this.lastSyncTime = NetworkTime.localTime;
			this.syncVarDirtyBits = 0UL;
			this.syncObjectDirtyBits = 0UL;
			for (int i = 0; i < this.syncObjects.Count; i++)
			{
				this.syncObjects[i].ClearChanges();
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000031BC File Offset: 0x000013BC
		protected void InitSyncObject(SyncObject syncObject)
		{
			if (syncObject == null)
			{
				Debug.LogError("Uninitialized SyncObject. Manually call the constructor on your SyncList, SyncSet, SyncDictionary or SyncField<T>");
				return;
			}
			int count = this.syncObjects.Count;
			this.syncObjects.Add(syncObject);
			ulong nthBit = 1UL << count;
			syncObject.OnDirty = delegate()
			{
				this.SetSyncObjectDirtyBit(nthBit);
			};
			syncObject.IsWritable = delegate()
			{
				if (NetworkServer.active && NetworkClient.active)
				{
					return this.syncDirection == SyncDirection.ServerToClient || this.isOwned;
				}
				if (NetworkServer.active)
				{
					return this.syncDirection == SyncDirection.ServerToClient;
				}
				if (NetworkClient.active)
				{
					return this.netId == 0U || (this.syncDirection == SyncDirection.ClientToServer && this.isOwned);
				}
				throw new Exception("InitSyncObject: IsWritable: neither NetworkServer nor NetworkClient are active.");
			};
			syncObject.IsRecording = delegate()
			{
				if (this.isServer && this.isClient)
				{
					return this.netIdentity.observers.Count > 0;
				}
				if (this.isServer)
				{
					return this.netIdentity.observers.Count > 0;
				}
				return this.isClient && this.syncDirection == SyncDirection.ClientToServer && this.isOwned;
			};
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000024F9 File Offset: 0x000006F9
		protected virtual void OnValidate()
		{
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003240 File Offset: 0x00001440
		protected void SendCommandInternal(string functionFullName, int functionHashCode, NetworkWriter writer, int channelId, bool requiresAuthority = true)
		{
			if (!NetworkClient.active)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"Command ",
					functionFullName,
					" called on ",
					base.name,
					" without an active client."
				}), base.gameObject);
				return;
			}
			if (!NetworkClient.ready)
			{
				if (channelId == 0)
				{
					Debug.LogWarning(string.Concat(new string[]
					{
						"Command ",
						functionFullName,
						" called on ",
						base.name,
						" while NetworkClient is not ready.\nThis may be ignored if client intentionally set NotReady."
					}), base.gameObject);
				}
				return;
			}
			if (requiresAuthority && !this.isLocalPlayer && !this.isOwned)
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					"Command ",
					functionFullName,
					" called on ",
					base.name,
					" without authority."
				}), base.gameObject);
				return;
			}
			if (NetworkClient.connection == null)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"Command ",
					functionFullName,
					" called on ",
					base.name,
					" with no client running."
				}), base.gameObject);
				return;
			}
			CommandMessage message = new CommandMessage
			{
				netId = this.netId,
				componentIndex = this.ComponentIndex,
				functionHash = (ushort)functionHashCode,
				payload = writer.ToArraySegment()
			};
			NetworkClient.connection.Send<CommandMessage>(message, channelId);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000033B4 File Offset: 0x000015B4
		protected void SendRPCInternal(string functionFullName, int functionHashCode, NetworkWriter writer, int channelId, bool includeOwner)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError("RPC Function " + functionFullName + " called without an active server.", base.gameObject);
				return;
			}
			if (!this.isServer)
			{
				Debug.LogWarning("ClientRpc " + functionFullName + " called on un-spawned object: " + base.name, base.gameObject);
				return;
			}
			RpcMessage rpcMessage = new RpcMessage
			{
				netId = this.netId,
				componentIndex = this.ComponentIndex,
				functionHash = (ushort)functionHashCode,
				payload = writer.ToArraySegment()
			};
			if (this.netIdentity.observers == null || this.netIdentity.observers.Count == 0)
			{
				return;
			}
			using (NetworkWriterPooled networkWriterPooled = NetworkWriterPool.Get())
			{
				networkWriterPooled.Write<RpcMessage>(rpcMessage);
				foreach (NetworkConnectionToClient networkConnectionToClient in this.netIdentity.observers.Values)
				{
					if ((networkConnectionToClient != this.netIdentity.connectionToClient || includeOwner) && networkConnectionToClient.isReady)
					{
						networkConnectionToClient.Send<RpcMessage>(rpcMessage, channelId);
					}
				}
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003500 File Offset: 0x00001700
		protected void SendTargetRPCInternal(NetworkConnection conn, string functionFullName, int functionHashCode, NetworkWriter writer, int channelId)
		{
			if (!NetworkServer.active)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"TargetRPC ",
					functionFullName,
					" was called on ",
					base.name,
					" when server not active."
				}), base.gameObject);
				return;
			}
			if (!this.isServer)
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					"TargetRpc ",
					functionFullName,
					" called on ",
					base.name,
					" but that object has not been spawned or has been unspawned."
				}), base.gameObject);
				return;
			}
			if (conn == null)
			{
				conn = this.connectionToClient;
			}
			if (conn == null)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"TargetRPC ",
					functionFullName,
					" can't be sent because it was given a null connection. Make sure ",
					base.name,
					" is owned by a connection, or if you pass a connection manually then make sure it's not null. For example, TargetRpcs can be called on Player/Pet which are owned by a connection. However, they can not be called on Monsters/Npcs which don't have an owner connection."
				}), base.gameObject);
				return;
			}
			if (!(conn is NetworkConnectionToClient))
			{
				Debug.LogError(string.Concat(new string[]
				{
					"TargetRPC ",
					functionFullName,
					" called on ",
					base.name,
					" requires a NetworkConnectionToClient but was given ",
					conn.GetType().Name
				}), base.gameObject);
				return;
			}
			RpcMessage message = new RpcMessage
			{
				netId = this.netId,
				componentIndex = this.ComponentIndex,
				functionHash = (ushort)functionHashCode,
				payload = writer.ToArraySegment()
			};
			conn.Send<RpcMessage>(message, channelId);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003674 File Offset: 0x00001874
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GeneratedSyncVarSetter<T>(T value, ref T field, ulong dirtyBit, Action<T, T> OnChanged)
		{
			if (!NetworkBehaviour.SyncVarEqual<T>(value, ref field))
			{
				T arg = field;
				this.SetSyncVar<T>(value, ref field, dirtyBit);
				if (OnChanged != null && NetworkServer.activeHost && !this.GetSyncVarHookGuard(dirtyBit))
				{
					this.SetSyncVarHookGuard(dirtyBit, true);
					OnChanged(arg, value);
					this.SetSyncVarHookGuard(dirtyBit, false);
				}
			}
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000036C8 File Offset: 0x000018C8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GeneratedSyncVarSetter_GameObject(GameObject value, ref GameObject field, ulong dirtyBit, Action<GameObject, GameObject> OnChanged, ref uint netIdField)
		{
			if (!NetworkBehaviour.SyncVarGameObjectEqual(value, netIdField))
			{
				GameObject arg = field;
				this.SetSyncVarGameObject(value, ref field, dirtyBit, ref netIdField);
				if (OnChanged != null && NetworkServer.activeHost && !this.GetSyncVarHookGuard(dirtyBit))
				{
					this.SetSyncVarHookGuard(dirtyBit, true);
					OnChanged(arg, value);
					this.SetSyncVarHookGuard(dirtyBit, false);
				}
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x0000371C File Offset: 0x0000191C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GeneratedSyncVarSetter_NetworkIdentity(NetworkIdentity value, ref NetworkIdentity field, ulong dirtyBit, Action<NetworkIdentity, NetworkIdentity> OnChanged, ref uint netIdField)
		{
			if (!NetworkBehaviour.SyncVarNetworkIdentityEqual(value, netIdField))
			{
				NetworkIdentity arg = field;
				this.SetSyncVarNetworkIdentity(value, ref field, dirtyBit, ref netIdField);
				if (OnChanged != null && NetworkServer.activeHost && !this.GetSyncVarHookGuard(dirtyBit))
				{
					this.SetSyncVarHookGuard(dirtyBit, true);
					OnChanged(arg, value);
					this.SetSyncVarHookGuard(dirtyBit, false);
				}
			}
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003770 File Offset: 0x00001970
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GeneratedSyncVarSetter_NetworkBehaviour<T>(T value, ref T field, ulong dirtyBit, Action<T, T> OnChanged, ref NetworkBehaviourSyncVar netIdField) where T : NetworkBehaviour
		{
			if (!NetworkBehaviour.SyncVarNetworkBehaviourEqual<T>(value, netIdField))
			{
				T arg = field;
				this.SetSyncVarNetworkBehaviour<T>(value, ref field, dirtyBit, ref netIdField);
				if (OnChanged != null && NetworkServer.activeHost && !this.GetSyncVarHookGuard(dirtyBit))
				{
					this.SetSyncVarHookGuard(dirtyBit, true);
					OnChanged(arg, value);
					this.SetSyncVarHookGuard(dirtyBit, false);
				}
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000037CC File Offset: 0x000019CC
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool SyncVarGameObjectEqual(GameObject newGameObject, uint netIdField)
		{
			uint num = 0U;
			NetworkIdentity networkIdentity;
			if (newGameObject != null && newGameObject.TryGetComponent<NetworkIdentity>(out networkIdentity))
			{
				num = networkIdentity.netId;
				if (num == 0U)
				{
					Debug.LogWarning(string.Format("SetSyncVarGameObject GameObject {0} has a zero netId. Maybe it is not spawned yet?", newGameObject));
				}
			}
			return num == netIdField;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x0000380C File Offset: 0x00001A0C
		protected void SetSyncVarGameObject(GameObject newGameObject, ref GameObject gameObjectField, ulong dirtyBit, ref uint netIdField)
		{
			if (this.GetSyncVarHookGuard(dirtyBit))
			{
				return;
			}
			uint num = 0U;
			NetworkIdentity networkIdentity;
			if (newGameObject != null && newGameObject.TryGetComponent<NetworkIdentity>(out networkIdentity))
			{
				num = networkIdentity.netId;
				if (num == 0U)
				{
					Debug.LogWarning(string.Format("SetSyncVarGameObject GameObject {0} has a zero netId. Maybe it is not spawned yet?", newGameObject));
				}
			}
			this.SetSyncVarDirtyBit(dirtyBit);
			gameObjectField = newGameObject;
			netIdField = num;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003860 File Offset: 0x00001A60
		protected GameObject GetSyncVarGameObject(uint netId, ref GameObject gameObjectField)
		{
			if (this.isServer || !this.isClient)
			{
				return gameObjectField;
			}
			NetworkIdentity networkIdentity;
			if (NetworkClient.spawned.TryGetValue(netId, out networkIdentity) && networkIdentity != null)
			{
				GameObject gameObject;
				gameObjectField = (gameObject = networkIdentity.gameObject);
				return gameObject;
			}
			return null;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000038A8 File Offset: 0x00001AA8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool SyncVarNetworkIdentityEqual(NetworkIdentity newIdentity, uint netIdField)
		{
			uint num = 0U;
			if (newIdentity != null)
			{
				num = newIdentity.netId;
				if (num == 0U)
				{
					Debug.LogWarning(string.Format("SetSyncVarNetworkIdentity NetworkIdentity {0} has a zero netId. Maybe it is not spawned yet?", newIdentity));
				}
			}
			return num == netIdField;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000038E0 File Offset: 0x00001AE0
		public void GeneratedSyncVarDeserialize<T>(ref T field, Action<T, T> OnChanged, T value)
		{
			T t = field;
			field = value;
			if (OnChanged != null && !NetworkBehaviour.SyncVarEqual<T>(t, ref field))
			{
				OnChanged(t, field);
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003914 File Offset: 0x00001B14
		public void GeneratedSyncVarDeserialize_GameObject(ref GameObject field, Action<GameObject, GameObject> OnChanged, NetworkReader reader, ref uint netIdField)
		{
			uint value = netIdField;
			GameObject arg = field;
			netIdField = reader.ReadUInt();
			field = this.GetSyncVarGameObject(netIdField, ref field);
			if (OnChanged != null && !NetworkBehaviour.SyncVarEqual<uint>(value, ref netIdField))
			{
				OnChanged(arg, field);
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003954 File Offset: 0x00001B54
		public void GeneratedSyncVarDeserialize_NetworkIdentity(ref NetworkIdentity field, Action<NetworkIdentity, NetworkIdentity> OnChanged, NetworkReader reader, ref uint netIdField)
		{
			uint value = netIdField;
			NetworkIdentity arg = field;
			netIdField = reader.ReadUInt();
			field = this.GetSyncVarNetworkIdentity(netIdField, ref field);
			if (OnChanged != null && !NetworkBehaviour.SyncVarEqual<uint>(value, ref netIdField))
			{
				OnChanged(arg, field);
			}
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003994 File Offset: 0x00001B94
		public void GeneratedSyncVarDeserialize_NetworkBehaviour<T>(ref T field, Action<T, T> OnChanged, NetworkReader reader, ref NetworkBehaviourSyncVar netIdField) where T : NetworkBehaviour
		{
			NetworkBehaviourSyncVar value = netIdField;
			T arg = field;
			netIdField = reader.ReadNetworkBehaviourSyncVar();
			field = this.GetSyncVarNetworkBehaviour<T>(netIdField, ref field);
			if (OnChanged != null && !NetworkBehaviour.SyncVarEqual<NetworkBehaviourSyncVar>(value, ref netIdField))
			{
				OnChanged(arg, field);
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000039EC File Offset: 0x00001BEC
		protected void SetSyncVarNetworkIdentity(NetworkIdentity newIdentity, ref NetworkIdentity identityField, ulong dirtyBit, ref uint netIdField)
		{
			if (this.GetSyncVarHookGuard(dirtyBit))
			{
				return;
			}
			uint num = 0U;
			if (newIdentity != null)
			{
				num = newIdentity.netId;
				if (num == 0U)
				{
					Debug.LogWarning(string.Format("SetSyncVarNetworkIdentity NetworkIdentity {0} has a zero netId. Maybe it is not spawned yet?", newIdentity));
				}
			}
			this.SetSyncVarDirtyBit(dirtyBit);
			netIdField = num;
			identityField = newIdentity;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003A36 File Offset: 0x00001C36
		protected NetworkIdentity GetSyncVarNetworkIdentity(uint netId, ref NetworkIdentity identityField)
		{
			if (this.isServer || !this.isClient)
			{
				return identityField;
			}
			NetworkClient.spawned.TryGetValue(netId, out identityField);
			return identityField;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003A5C File Offset: 0x00001C5C
		protected static bool SyncVarNetworkBehaviourEqual<T>(T newBehaviour, NetworkBehaviourSyncVar syncField) where T : NetworkBehaviour
		{
			uint num = 0U;
			byte componentIndex = 0;
			if (newBehaviour != null)
			{
				num = newBehaviour.netId;
				componentIndex = newBehaviour.ComponentIndex;
				if (num == 0U)
				{
					Debug.LogWarning(string.Format("SetSyncVarNetworkIdentity NetworkIdentity {0} has a zero netId. Maybe it is not spawned yet?", newBehaviour));
				}
			}
			return syncField.Equals(num, (int)componentIndex);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003AB4 File Offset: 0x00001CB4
		protected void SetSyncVarNetworkBehaviour<T>(T newBehaviour, ref T behaviourField, ulong dirtyBit, ref NetworkBehaviourSyncVar syncField) where T : NetworkBehaviour
		{
			if (this.GetSyncVarHookGuard(dirtyBit))
			{
				return;
			}
			uint num = 0U;
			byte componentIndex = 0;
			if (newBehaviour != null)
			{
				num = newBehaviour.netId;
				componentIndex = newBehaviour.ComponentIndex;
				if (num == 0U)
				{
					Debug.LogWarning(string.Format("{0} NetworkIdentity {1} has a zero netId. Maybe it is not spawned yet?", "SetSyncVarNetworkBehaviour", newBehaviour));
				}
			}
			syncField = new NetworkBehaviourSyncVar(num, (int)componentIndex);
			this.SetSyncVarDirtyBit(dirtyBit);
			behaviourField = newBehaviour;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003B30 File Offset: 0x00001D30
		protected T GetSyncVarNetworkBehaviour<T>(NetworkBehaviourSyncVar syncNetBehaviour, ref T behaviourField) where T : NetworkBehaviour
		{
			if (this.isServer || !this.isClient)
			{
				return behaviourField;
			}
			NetworkIdentity networkIdentity;
			if (!NetworkClient.spawned.TryGetValue(syncNetBehaviour.netId, out networkIdentity))
			{
				return default(T);
			}
			behaviourField = (networkIdentity.NetworkBehaviours[(int)syncNetBehaviour.componentIndex] as T);
			return behaviourField;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003B95 File Offset: 0x00001D95
		protected static bool SyncVarEqual<T>(T value, ref T fieldValue)
		{
			return EqualityComparer<T>.Default.Equals(value, fieldValue);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003BA8 File Offset: 0x00001DA8
		protected void SetSyncVar<T>(T value, ref T fieldValue, ulong dirtyBit)
		{
			this.SetSyncVarDirtyBit(dirtyBit);
			fieldValue = value;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00003BB8 File Offset: 0x00001DB8
		public virtual void OnSerialize(NetworkWriter writer, bool initialState)
		{
			this.SerializeSyncObjects(writer, initialState);
			this.SerializeSyncVars(writer, initialState);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00003BCA File Offset: 0x00001DCA
		public virtual void OnDeserialize(NetworkReader reader, bool initialState)
		{
			this.DeserializeSyncObjects(reader, initialState);
			this.DeserializeSyncVars(reader, initialState);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00003BDC File Offset: 0x00001DDC
		private void SerializeSyncObjects(NetworkWriter writer, bool initialState)
		{
			if (initialState)
			{
				this.SerializeObjectsAll(writer);
				return;
			}
			this.SerializeObjectsDelta(writer);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003BF0 File Offset: 0x00001DF0
		private void DeserializeSyncObjects(NetworkReader reader, bool initialState)
		{
			if (initialState)
			{
				this.DeserializeObjectsAll(reader);
				return;
			}
			this.DeserializeObjectsDelta(reader);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000024F9 File Offset: 0x000006F9
		protected virtual void SerializeSyncVars(NetworkWriter writer, bool initialState)
		{
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000024F9 File Offset: 0x000006F9
		protected virtual void DeserializeSyncVars(NetworkReader reader, bool initialState)
		{
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003C04 File Offset: 0x00001E04
		public void SerializeObjectsAll(NetworkWriter writer)
		{
			for (int i = 0; i < this.syncObjects.Count; i++)
			{
				this.syncObjects[i].OnSerializeAll(writer);
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00003C3C File Offset: 0x00001E3C
		public void SerializeObjectsDelta(NetworkWriter writer)
		{
			writer.WriteULong(this.syncObjectDirtyBits);
			for (int i = 0; i < this.syncObjects.Count; i++)
			{
				SyncObject syncObject = this.syncObjects[i];
				if ((this.syncObjectDirtyBits & 1UL << i) != 0UL)
				{
					syncObject.OnSerializeDelta(writer);
				}
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00003C90 File Offset: 0x00001E90
		internal void DeserializeObjectsAll(NetworkReader reader)
		{
			for (int i = 0; i < this.syncObjects.Count; i++)
			{
				this.syncObjects[i].OnDeserializeAll(reader);
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00003CC8 File Offset: 0x00001EC8
		internal void DeserializeObjectsDelta(NetworkReader reader)
		{
			ulong num = reader.ReadULong();
			for (int i = 0; i < this.syncObjects.Count; i++)
			{
				SyncObject syncObject = this.syncObjects[i];
				if ((num & 1UL << i) != 0UL)
				{
					syncObject.OnDeserializeDelta(reader);
				}
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00003D14 File Offset: 0x00001F14
		internal void Serialize(NetworkWriter writer, bool initialState)
		{
			int position = writer.Position;
			writer.WriteByte(0);
			int position2 = writer.Position;
			try
			{
				this.OnSerialize(writer, initialState);
			}
			catch (Exception ex)
			{
				Debug.LogError(string.Format("OnSerialize failed for: object={0} component={1} sceneId={2:X}\n\n{3}", new object[]
				{
					base.name,
					base.GetType(),
					this.netIdentity.sceneId,
					ex
				}));
			}
			int position3 = writer.Position;
			writer.Position = position;
			byte value = (byte)(position3 - position2 & 255);
			writer.WriteByte(value);
			writer.Position = position3;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00003DBC File Offset: 0x00001FBC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static int ErrorCorrection(int size, byte safety)
		{
			return (size & -256) | (int)safety;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00003DC8 File Offset: 0x00001FC8
		internal bool Deserialize(NetworkReader reader, bool initialState)
		{
			bool result = true;
			byte b = reader.ReadByte();
			int position = reader.Position;
			try
			{
				this.OnDeserialize(reader, initialState);
			}
			catch (Exception ex)
			{
				Debug.LogError(string.Concat(new string[]
				{
					string.Format("OnDeserialize failed Exception={0} (see below) object={1} component={2} netId={3}. Possible Reasons:\n", new object[]
					{
						ex.GetType(),
						base.name,
						base.GetType(),
						this.netId
					}),
					string.Format("  * Do {0}'s OnSerialize and OnDeserialize calls write the same amount of data? \n", base.GetType()),
					string.Format("  * Was there an exception in {0}'s OnSerialize/OnDeserialize code?\n", base.GetType()),
					"  * Are the server and client the exact same project?\n  * Maybe this OnDeserialize call was meant for another GameObject? The sceneIds can easily get out of sync if the Hierarchy was modified only in the client OR the server. Try rebuilding both.\n\n",
					string.Format("Exception {0}", ex)
				}));
				result = false;
			}
			int num = reader.Position - position;
			byte b2 = (byte)(num & 255);
			if (b2 != b)
			{
				Debug.LogWarning(string.Format("{0} (netId={1}): {2} OnDeserialize size mismatch. It read {3} bytes, which caused a size hash mismatch of {4:X2} vs. {5:X2}. Make sure that OnSerialize and OnDeserialize write/read the same amount of data in all cases.", new object[]
				{
					base.name,
					this.netId,
					base.GetType(),
					num,
					b2,
					b
				}));
				int num2 = NetworkBehaviour.ErrorCorrection(num, b);
				reader.Position = position + num2;
				result = false;
			}
			return result;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00003F14 File Offset: 0x00002114
		internal void ResetSyncObjects()
		{
			foreach (SyncObject syncObject in this.syncObjects)
			{
				syncObject.Reset();
			}
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual void OnStartServer()
		{
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual void OnStopServer()
		{
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual void OnStartClient()
		{
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual void OnStopClient()
		{
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual void OnStartLocalPlayer()
		{
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual void OnStopLocalPlayer()
		{
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual void OnStartAuthority()
		{
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual void OnStopAuthority()
		{
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00003F64 File Offset: 0x00002164
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual bool Weaved()
		{
			return false;
		}

		// Token: 0x04000053 RID: 83
		[Tooltip("Server Authority calls OnSerialize on the server and syncs it to clients.\n\nClient Authority calls OnSerialize on the owning client, syncs it to server, which then broadcasts it to all other clients.\n\nUse server authority for cheat safety.")]
		[HideInInspector]
		public SyncDirection syncDirection;

		// Token: 0x04000054 RID: 84
		[Tooltip("By default synced data is sent from the server to all Observers of the object.\nChange this to Owner to only have the server update the client that has ownership authority for this object")]
		[HideInInspector]
		public SyncMode syncMode;

		// Token: 0x04000055 RID: 85
		[Tooltip("Time in seconds until next change is synchronized to the client. '0' means send immediately if changed. '0.5' means only send changes every 500ms.\n(This is for state synchronization like SyncVars, SyncLists, OnSerialize. Not for Cmds, Rpcs, etc.)")]
		[Range(0f, 2f)]
		[HideInInspector]
		public float syncInterval;

		// Token: 0x04000056 RID: 86
		internal double lastSyncTime;

		// Token: 0x04000057 RID: 87
		protected readonly List<SyncObject> syncObjects = new List<SyncObject>();

		// Token: 0x0400005B RID: 91
		internal ulong syncObjectDirtyBits;

		// Token: 0x0400005C RID: 92
		private ulong syncVarHookGuard;
	}
}
