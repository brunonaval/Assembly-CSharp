using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Mirror
{
	// Token: 0x0200004D RID: 77
	[DisallowMultipleComponent]
	[AddComponentMenu("Network/Network Manager")]
	[HelpURL("https://mirror-networking.gitbook.io/docs/components/network-manager")]
	public class NetworkManager : MonoBehaviour
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000197 RID: 407 RVA: 0x00007900 File Offset: 0x00005B00
		[Obsolete("NetworkManager.serverTickRate was renamed to sendRate because that's what it configures for both server & client now.")]
		public int serverTickRate
		{
			get
			{
				return this.sendRate;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000198 RID: 408 RVA: 0x00007908 File Offset: 0x00005B08
		[Obsolete("NetworkManager.serverTickInterval was moved to NetworkServer.tickInterval for consistency.")]
		public float serverTickInterval
		{
			get
			{
				return NetworkServer.tickInterval;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000199 RID: 409 RVA: 0x0000790F File Offset: 0x00005B0F
		// (set) Token: 0x0600019A RID: 410 RVA: 0x00007916 File Offset: 0x00005B16
		public static NetworkManager singleton { get; internal set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600019B RID: 411 RVA: 0x0000791E File Offset: 0x00005B1E
		public int numPlayers
		{
			get
			{
				return NetworkServer.connections.Count((KeyValuePair<int, NetworkConnectionToClient> kv) => kv.Value.identity != null);
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600019C RID: 412 RVA: 0x00007949 File Offset: 0x00005B49
		public bool isNetworkActive
		{
			get
			{
				return NetworkServer.active || NetworkClient.active;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600019D RID: 413 RVA: 0x00007959 File Offset: 0x00005B59
		// (set) Token: 0x0600019E RID: 414 RVA: 0x00007961 File Offset: 0x00005B61
		public NetworkManagerMode mode { get; private set; }

		// Token: 0x0600019F RID: 415 RVA: 0x0000796C File Offset: 0x00005B6C
		public virtual void OnValidate()
		{
			this.maxConnections = Mathf.Max(this.maxConnections, 0);
			NetworkIdentity networkIdentity;
			if (this.playerPrefab != null && !this.playerPrefab.TryGetComponent<NetworkIdentity>(out networkIdentity))
			{
				Debug.LogError("NetworkManager - Player Prefab must have a NetworkIdentity.");
				this.playerPrefab = null;
			}
			if (this.playerPrefab != null && this.spawnPrefabs.Contains(this.playerPrefab))
			{
				Debug.LogWarning("NetworkManager - Player Prefab should not be added to Registered Spawnable Prefabs list...removed it.");
				this.spawnPrefabs.Remove(this.playerPrefab);
			}
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x000079F8 File Offset: 0x00005BF8
		public virtual void Reset()
		{
			foreach (NetworkManager networkManager in base.transform.root.GetComponentsInChildren<NetworkManager>())
			{
				if (networkManager != this)
				{
					Debug.LogError(string.Format("{0} detected another component of type {1} in its hierarchy on {2}. There can only be one, please remove one of them.", base.name, typeof(NetworkManager), networkManager.name));
					return;
				}
			}
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00007A57 File Offset: 0x00005C57
		public virtual void Awake()
		{
			if (!this.InitializeSingleton())
			{
				return;
			}
			this.ApplyConfiguration();
			NetworkManager.networkSceneName = this.offlineScene;
			SceneManager.sceneLoaded += this.OnSceneLoaded;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual void Start()
		{
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00007A84 File Offset: 0x00005C84
		public virtual void Update()
		{
			this.ApplyConfiguration();
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00007A8C File Offset: 0x00005C8C
		public virtual void LateUpdate()
		{
			this.UpdateScene();
			this.UpdateConnectionQuality();
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00007A9C File Offset: 0x00005C9C
		private void UpdateConnectionQuality()
		{
			if (!NetworkClient.active)
			{
				return;
			}
			if (NetworkTime.time < this.lastConnectionQualityUpdate + (double)this.connectionQualityInterval)
			{
				return;
			}
			this.lastConnectionQualityUpdate = NetworkTime.time;
			this.CalculateConnectionQuality();
			if (NetworkClient.connectionQuality != this.lastConnectionQuality)
			{
				this.OnConnectionQualityChanged(this.lastConnectionQuality, NetworkClient.connectionQuality);
				this.lastConnectionQuality = NetworkClient.connectionQuality;
			}
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00007B01 File Offset: 0x00005D01
		protected virtual void CalculateConnectionQuality()
		{
			NetworkClient.connectionQuality = ConnectionQualityHeuristics.Simple(NetworkTime.rtt, NetworkTime.rttVariance);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00007B17 File Offset: 0x00005D17
		protected virtual void OnConnectionQualityChanged(ConnectionQuality previous, ConnectionQuality current)
		{
			Debug.Log(string.Format("[Mirror] Connection Quality changed from {0} to {1}", previous, current));
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00007B34 File Offset: 0x00005D34
		private bool IsServerOnlineSceneChangeNeeded()
		{
			return !string.IsNullOrWhiteSpace(this.onlineScene) && !Utils.IsSceneActive(this.onlineScene) && this.onlineScene != this.offlineScene;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00007B63 File Offset: 0x00005D63
		[Obsolete("NetworkManager.IsSceneActive moved to Utils.IsSceneActive")]
		public static bool IsSceneActive(string scene)
		{
			return Utils.IsSceneActive(scene);
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00007B6B File Offset: 0x00005D6B
		private void ApplyConfiguration()
		{
			NetworkServer.tickRate = this.sendRate;
			NetworkClient.snapshotSettings = this.snapshotSettings;
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00007B84 File Offset: 0x00005D84
		private void SetupServer()
		{
			this.InitializeSingleton();
			if (this.runInBackground)
			{
				Application.runInBackground = true;
			}
			if (this.authenticator != null)
			{
				this.authenticator.OnStartServer();
				this.authenticator.OnServerAuthenticated.AddListener(new UnityAction<NetworkConnectionToClient>(this.OnServerAuthenticated));
			}
			this.ConfigureHeadlessFrameRate();
			NetworkServer.disconnectInactiveConnections = this.disconnectInactiveConnections;
			NetworkServer.disconnectInactiveTimeout = this.disconnectInactiveTimeout;
			NetworkServer.Listen(this.maxConnections);
			this.RegisterServerMessages();
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00007C08 File Offset: 0x00005E08
		public void StartServer()
		{
			if (NetworkServer.active)
			{
				Debug.LogWarning("Server already started.");
				return;
			}
			this.mode = NetworkManagerMode.ServerOnly;
			this.SetupServer();
			this.OnStartServer();
			if (this.IsServerOnlineSceneChangeNeeded())
			{
				this.ServerChangeScene(this.onlineScene);
				return;
			}
			NetworkServer.SpawnObjects();
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00007C58 File Offset: 0x00005E58
		private void SetupClient()
		{
			this.InitializeSingleton();
			if (this.runInBackground)
			{
				Application.runInBackground = true;
			}
			if (this.authenticator != null)
			{
				this.authenticator.OnStartClient();
				this.authenticator.OnClientAuthenticated.AddListener(new UnityAction(this.OnClientAuthenticated));
			}
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00007CB0 File Offset: 0x00005EB0
		public void StartClient()
		{
			if (NetworkClient.active)
			{
				Debug.LogWarning("Client already started.");
				return;
			}
			this.mode = NetworkManagerMode.ClientOnly;
			this.SetupClient();
			this.ConfigureHeadlessFrameRate();
			this.RegisterClientMessages();
			if (string.IsNullOrWhiteSpace(this.networkAddress))
			{
				Debug.LogError("Must set the Network Address field in the manager");
				return;
			}
			NetworkClient.Connect(this.networkAddress);
			this.OnStartClient();
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00007D11 File Offset: 0x00005F11
		public void StartClient(Uri uri)
		{
			if (NetworkClient.active)
			{
				Debug.LogWarning("Client already started.");
				return;
			}
			this.mode = NetworkManagerMode.ClientOnly;
			this.SetupClient();
			this.RegisterClientMessages();
			this.networkAddress = uri.Host;
			NetworkClient.Connect(uri);
			this.OnStartClient();
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00007D50 File Offset: 0x00005F50
		public void StartHost()
		{
			if (NetworkServer.active || NetworkClient.active)
			{
				Debug.LogWarning("Server or Client already started.");
				return;
			}
			this.mode = NetworkManagerMode.Host;
			this.SetupServer();
			if (this.IsServerOnlineSceneChangeNeeded())
			{
				this.finishStartHostPending = true;
				this.ServerChangeScene(this.onlineScene);
				return;
			}
			this.FinishStartHost();
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00007DA5 File Offset: 0x00005FA5
		private void FinishStartHost()
		{
			NetworkClient.ConnectHost();
			this.OnStartServer();
			this.OnStartHost();
			NetworkServer.SpawnObjects();
			this.SetupClient();
			this.networkAddress = "localhost";
			this.RegisterClientMessages();
			HostMode.InvokeOnConnected();
			this.OnStartClient();
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00007DE0 File Offset: 0x00005FE0
		public void StopHost()
		{
			this.OnStopHost();
			this.StopClient();
			this.StopServer();
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00007DF4 File Offset: 0x00005FF4
		public void StopServer()
		{
			if (!NetworkServer.active)
			{
				return;
			}
			if (this.authenticator != null)
			{
				this.authenticator.OnServerAuthenticated.RemoveListener(new UnityAction<NetworkConnectionToClient>(this.OnServerAuthenticated));
				this.authenticator.OnStopServer();
			}
			if (base.gameObject != null && base.gameObject.scene.name == "DontDestroyOnLoad" && !string.IsNullOrWhiteSpace(this.offlineScene) && SceneManager.GetActiveScene().path != this.offlineScene)
			{
				SceneManager.MoveGameObjectToScene(base.gameObject, SceneManager.GetActiveScene());
			}
			this.OnStopServer();
			NetworkServer.Shutdown();
			this.mode = NetworkManagerMode.Offline;
			if (!string.IsNullOrWhiteSpace(this.offlineScene))
			{
				this.ServerChangeScene(this.offlineScene);
			}
			NetworkManager.startPositionIndex = 0;
			NetworkManager.networkSceneName = "";
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00007EDD File Offset: 0x000060DD
		public void StopClient()
		{
			if (this.mode == NetworkManagerMode.Offline)
			{
				return;
			}
			if (this.mode == NetworkManagerMode.Host)
			{
				this.OnServerDisconnect(NetworkServer.localConnection);
			}
			NetworkClient.Disconnect();
			this.OnClientDisconnectInternal();
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00007F07 File Offset: 0x00006107
		public virtual void OnApplicationQuit()
		{
			if (NetworkClient.isConnected)
			{
				this.StopClient();
			}
			if (NetworkServer.active)
			{
				this.StopServer();
			}
			NetworkManager.ResetStatics();
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual void ConfigureHeadlessFrameRate()
		{
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00007F28 File Offset: 0x00006128
		private bool InitializeSingleton()
		{
			if (NetworkManager.singleton != null && NetworkManager.singleton == this)
			{
				return true;
			}
			if (this.dontDestroyOnLoad)
			{
				if (NetworkManager.singleton != null)
				{
					Debug.LogWarning("Multiple NetworkManagers detected in the scene. Only one NetworkManager can exist at a time. The duplicate NetworkManager will be destroyed.");
					UnityEngine.Object.Destroy(base.gameObject);
					return false;
				}
				NetworkManager.singleton = this;
				if (Application.isPlaying)
				{
					base.transform.SetParent(null);
					UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
				}
			}
			else
			{
				NetworkManager.singleton = this;
			}
			if (this.transport == null)
			{
				Transport arg;
				if (!base.TryGetComponent<Transport>(out arg))
				{
					Debug.LogError("No Transport on Network Manager...add a transport and assign it.");
					return false;
				}
				Debug.LogWarning(string.Format("No Transport assigned to Network Manager - Using {0} found on same object.", arg));
				this.transport = arg;
			}
			Transport.active = this.transport;
			return true;
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00007FF4 File Offset: 0x000061F4
		private void RegisterServerMessages()
		{
			NetworkServer.OnConnectedEvent = new Action<NetworkConnectionToClient>(this.OnServerConnectInternal);
			NetworkServer.OnDisconnectedEvent = new Action<NetworkConnectionToClient>(this.OnServerDisconnect);
			NetworkServer.OnErrorEvent = new Action<NetworkConnectionToClient, TransportError, string>(this.OnServerError);
			NetworkServer.RegisterHandler<AddPlayerMessage>(new Action<NetworkConnectionToClient, AddPlayerMessage>(this.OnServerAddPlayerInternal), true);
			NetworkServer.ReplaceHandler<ReadyMessage>(new Action<NetworkConnectionToClient, ReadyMessage>(this.OnServerReadyMessageInternal), true);
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000805C File Offset: 0x0000625C
		private void RegisterClientMessages()
		{
			NetworkClient.OnConnectedEvent = new Action(this.OnClientConnectInternal);
			NetworkClient.OnDisconnectedEvent = new Action(this.OnClientDisconnectInternal);
			NetworkClient.OnErrorEvent = new Action<TransportError, string>(this.OnClientError);
			NetworkClient.RegisterHandler<NotReadyMessage>(new Action<NotReadyMessage>(this.OnClientNotReadyMessageInternal), true);
			NetworkClient.RegisterHandler<SceneMessage>(new Action<SceneMessage>(this.OnClientSceneInternal), false);
			if (this.playerPrefab != null)
			{
				NetworkClient.RegisterPrefab(this.playerPrefab);
			}
			foreach (GameObject prefab in from t in this.spawnPrefabs
			where t != null
			select t)
			{
				NetworkClient.RegisterPrefab(prefab);
			}
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000813C File Offset: 0x0000633C
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void ResetStatics()
		{
			if (NetworkManager.singleton)
			{
				NetworkManager.singleton.StopHost();
			}
			NetworkManager.startPositions.Clear();
			NetworkManager.startPositionIndex = 0;
			NetworkManager.clientReadyConnection = null;
			NetworkManager.loadingSceneAsync = null;
			NetworkManager.networkSceneName = string.Empty;
			NetworkManager.singleton = null;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual void OnDestroy()
		{
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060001BC RID: 444 RVA: 0x0000818B File Offset: 0x0000638B
		// (set) Token: 0x060001BD RID: 445 RVA: 0x00008192 File Offset: 0x00006392
		public static string networkSceneName { get; protected set; } = "";

		// Token: 0x060001BE RID: 446 RVA: 0x0000819C File Offset: 0x0000639C
		public virtual void ServerChangeScene(string newSceneName)
		{
			if (string.IsNullOrWhiteSpace(newSceneName))
			{
				Debug.LogError("ServerChangeScene empty scene name");
				return;
			}
			if (NetworkServer.isLoadingScene && newSceneName == NetworkManager.networkSceneName)
			{
				Debug.LogError("Scene change is already in progress for " + newSceneName);
				return;
			}
			NetworkServer.SetAllClientsNotReady();
			NetworkManager.networkSceneName = newSceneName;
			this.OnServerChangeScene(newSceneName);
			NetworkServer.isLoadingScene = true;
			NetworkManager.loadingSceneAsync = SceneManager.LoadSceneAsync(newSceneName);
			if (NetworkServer.active)
			{
				NetworkServer.SendToAll<SceneMessage>(new SceneMessage
				{
					sceneName = newSceneName
				}, 0, false);
			}
			NetworkManager.startPositionIndex = 0;
			NetworkManager.startPositions.Clear();
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00008234 File Offset: 0x00006434
		internal void ClientChangeScene(string newSceneName, SceneOperation sceneOperation = SceneOperation.Normal, bool customHandling = false)
		{
			if (string.IsNullOrWhiteSpace(newSceneName))
			{
				Debug.LogError("ClientChangeScene empty scene name");
				return;
			}
			this.OnClientChangeScene(newSceneName, sceneOperation, customHandling);
			if (NetworkServer.active)
			{
				return;
			}
			NetworkClient.isLoadingScene = true;
			this.clientSceneOperation = sceneOperation;
			if (customHandling)
			{
				return;
			}
			switch (sceneOperation)
			{
			case SceneOperation.Normal:
				NetworkManager.loadingSceneAsync = SceneManager.LoadSceneAsync(newSceneName);
				break;
			case SceneOperation.LoadAdditive:
				if (!SceneManager.GetSceneByName(newSceneName).IsValid() && !SceneManager.GetSceneByPath(newSceneName).IsValid())
				{
					NetworkManager.loadingSceneAsync = SceneManager.LoadSceneAsync(newSceneName, LoadSceneMode.Additive);
				}
				else
				{
					Debug.LogWarning("Scene " + newSceneName + " is already loaded");
					NetworkClient.isLoadingScene = false;
				}
				break;
			case SceneOperation.UnloadAdditive:
				if (SceneManager.GetSceneByName(newSceneName).IsValid() || SceneManager.GetSceneByPath(newSceneName).IsValid())
				{
					NetworkManager.loadingSceneAsync = SceneManager.UnloadSceneAsync(newSceneName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
				}
				else
				{
					Debug.LogWarning("Cannot unload " + newSceneName + " with UnloadAdditive operation");
					NetworkClient.isLoadingScene = false;
				}
				break;
			}
			if (sceneOperation == SceneOperation.Normal)
			{
				NetworkManager.networkSceneName = newSceneName;
			}
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000833A File Offset: 0x0000653A
		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			if (mode == LoadSceneMode.Additive)
			{
				if (NetworkServer.active)
				{
					NetworkServer.SpawnObjects();
				}
				if (NetworkClient.active)
				{
					NetworkClient.PrepareToSpawnSceneObjects();
				}
			}
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000835C File Offset: 0x0000655C
		private void UpdateScene()
		{
			if (NetworkManager.loadingSceneAsync != null && NetworkManager.loadingSceneAsync.isDone)
			{
				try
				{
					this.FinishLoadScene();
				}
				finally
				{
					NetworkManager.loadingSceneAsync.allowSceneActivation = true;
					NetworkManager.loadingSceneAsync = null;
				}
			}
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x000083A8 File Offset: 0x000065A8
		protected void FinishLoadScene()
		{
			NetworkServer.isLoadingScene = false;
			NetworkClient.isLoadingScene = false;
			if (this.mode == NetworkManagerMode.Host)
			{
				this.FinishLoadSceneHost();
				return;
			}
			if (this.mode == NetworkManagerMode.ServerOnly)
			{
				this.FinishLoadSceneServerOnly();
				return;
			}
			if (this.mode == NetworkManagerMode.ClientOnly)
			{
				this.FinishLoadSceneClientOnly();
			}
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x000083E8 File Offset: 0x000065E8
		private void FinishLoadSceneHost()
		{
			if (NetworkManager.clientReadyConnection != null)
			{
				this.clientLoadedScene = true;
				NetworkManager.clientReadyConnection = null;
			}
			if (this.finishStartHostPending)
			{
				this.finishStartHostPending = false;
				this.FinishStartHost();
				this.OnServerSceneChanged(NetworkManager.networkSceneName);
				return;
			}
			NetworkServer.SpawnObjects();
			this.OnServerSceneChanged(NetworkManager.networkSceneName);
			if (NetworkClient.isConnected)
			{
				this.OnClientSceneChanged();
			}
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00008448 File Offset: 0x00006648
		private void FinishLoadSceneServerOnly()
		{
			NetworkServer.SpawnObjects();
			this.OnServerSceneChanged(NetworkManager.networkSceneName);
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000845B File Offset: 0x0000665B
		private void FinishLoadSceneClientOnly()
		{
			if (NetworkManager.clientReadyConnection != null)
			{
				this.clientLoadedScene = true;
				NetworkManager.clientReadyConnection = null;
			}
			if (NetworkClient.isConnected)
			{
				this.OnClientSceneChanged();
			}
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000847E File Offset: 0x0000667E
		public static void RegisterStartPosition(Transform start)
		{
			NetworkManager.startPositions.Add(start);
			NetworkManager.startPositions = (from transform in NetworkManager.startPositions
			orderby transform.GetSiblingIndex()
			select transform).ToList<Transform>();
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x000084BE File Offset: 0x000066BE
		public static void UnRegisterStartPosition(Transform start)
		{
			NetworkManager.startPositions.Remove(start);
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x000084CC File Offset: 0x000066CC
		public virtual Transform GetStartPosition()
		{
			NetworkManager.startPositions.RemoveAll((Transform t) => t == null);
			if (NetworkManager.startPositions.Count == 0)
			{
				return null;
			}
			if (this.playerSpawnMethod == PlayerSpawnMethod.Random)
			{
				return NetworkManager.startPositions[UnityEngine.Random.Range(0, NetworkManager.startPositions.Count)];
			}
			Transform result = NetworkManager.startPositions[NetworkManager.startPositionIndex];
			NetworkManager.startPositionIndex = (NetworkManager.startPositionIndex + 1) % NetworkManager.startPositions.Count;
			return result;
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000855A File Offset: 0x0000675A
		private void OnServerConnectInternal(NetworkConnectionToClient conn)
		{
			if (this.authenticator != null)
			{
				this.authenticator.OnServerAuthenticate(conn);
				return;
			}
			this.OnServerAuthenticated(conn);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00008580 File Offset: 0x00006780
		private void OnServerAuthenticated(NetworkConnectionToClient conn)
		{
			conn.isAuthenticated = true;
			if (NetworkManager.networkSceneName != "" && NetworkManager.networkSceneName != this.offlineScene)
			{
				SceneMessage message = new SceneMessage
				{
					sceneName = NetworkManager.networkSceneName
				};
				conn.Send<SceneMessage>(message, 0);
			}
			this.OnServerConnect(conn);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x000085DC File Offset: 0x000067DC
		private void OnServerReadyMessageInternal(NetworkConnectionToClient conn, ReadyMessage msg)
		{
			this.OnServerReady(conn);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x000085E8 File Offset: 0x000067E8
		private void OnServerAddPlayerInternal(NetworkConnectionToClient conn, AddPlayerMessage msg)
		{
			if (this.autoCreatePlayer && this.playerPrefab == null)
			{
				Debug.LogError("The PlayerPrefab is empty on the NetworkManager. Please setup a PlayerPrefab object.");
				return;
			}
			NetworkIdentity networkIdentity;
			if (this.autoCreatePlayer && !this.playerPrefab.TryGetComponent<NetworkIdentity>(out networkIdentity))
			{
				Debug.LogError("The PlayerPrefab does not have a NetworkIdentity. Please add a NetworkIdentity to the player prefab.");
				return;
			}
			if (conn.identity != null)
			{
				Debug.LogError("There is already a player for this connection.");
				return;
			}
			this.OnServerAddPlayer(conn);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00008658 File Offset: 0x00006858
		private void OnClientConnectInternal()
		{
			if (this.authenticator != null)
			{
				this.authenticator.OnClientAuthenticate();
				return;
			}
			this.OnClientAuthenticated();
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000867C File Offset: 0x0000687C
		private void OnClientAuthenticated()
		{
			NetworkClient.connection.isAuthenticated = true;
			if (string.IsNullOrWhiteSpace(this.onlineScene) || this.onlineScene == this.offlineScene || Utils.IsSceneActive(this.onlineScene))
			{
				this.clientLoadedScene = false;
			}
			else
			{
				this.clientLoadedScene = true;
				NetworkManager.clientReadyConnection = NetworkClient.connection;
			}
			this.OnClientConnect();
		}

		// Token: 0x060001CF RID: 463 RVA: 0x000086E4 File Offset: 0x000068E4
		private void OnClientDisconnectInternal()
		{
			if (this.mode == NetworkManagerMode.ServerOnly || this.mode == NetworkManagerMode.Offline)
			{
				return;
			}
			this.OnClientDisconnect();
			if (this.authenticator != null)
			{
				this.authenticator.OnClientAuthenticated.RemoveListener(new UnityAction(this.OnClientAuthenticated));
				this.authenticator.OnStopClient();
			}
			if (this.mode == NetworkManagerMode.Host)
			{
				this.mode = NetworkManagerMode.ServerOnly;
			}
			else
			{
				this.mode = NetworkManagerMode.Offline;
			}
			this.OnStopClient();
			NetworkClient.Shutdown();
			if (this.mode == NetworkManagerMode.ServerOnly)
			{
				return;
			}
			if (base.gameObject != null && base.gameObject.scene.name == "DontDestroyOnLoad" && !string.IsNullOrWhiteSpace(this.offlineScene) && SceneManager.GetActiveScene().path != this.offlineScene)
			{
				SceneManager.MoveGameObjectToScene(base.gameObject, SceneManager.GetActiveScene());
			}
			if (!string.IsNullOrWhiteSpace(this.offlineScene) && !Utils.IsSceneActive(this.offlineScene) && NetworkManager.loadingSceneAsync == null && !NetworkServer.active)
			{
				this.ClientChangeScene(this.offlineScene, SceneOperation.Normal, false);
			}
			NetworkManager.networkSceneName = "";
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00008810 File Offset: 0x00006A10
		private void OnClientNotReadyMessageInternal(NotReadyMessage msg)
		{
			NetworkClient.ready = false;
			this.OnClientNotReady();
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000881E File Offset: 0x00006A1E
		private void OnClientSceneInternal(SceneMessage msg)
		{
			if (NetworkClient.isConnected)
			{
				this.ClientChangeScene(msg.sceneName, msg.sceneOperation, msg.customHandling);
			}
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual void OnServerConnect(NetworkConnectionToClient conn)
		{
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000883F File Offset: 0x00006A3F
		public virtual void OnServerDisconnect(NetworkConnectionToClient conn)
		{
			NetworkServer.DestroyPlayerForConnection(conn);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00008847 File Offset: 0x00006A47
		public virtual void OnServerReady(NetworkConnectionToClient conn)
		{
			conn.identity == null;
			NetworkServer.SetClientReady(conn);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000885C File Offset: 0x00006A5C
		public virtual void OnServerAddPlayer(NetworkConnectionToClient conn)
		{
			Transform startPosition = this.GetStartPosition();
			GameObject gameObject = (startPosition != null) ? UnityEngine.Object.Instantiate<GameObject>(this.playerPrefab, startPosition.position, startPosition.rotation) : UnityEngine.Object.Instantiate<GameObject>(this.playerPrefab);
			gameObject.name = string.Format("{0} [connId={1}]", this.playerPrefab.name, conn.connectionId);
			NetworkServer.AddPlayerForConnection(conn, gameObject);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual void OnServerError(NetworkConnectionToClient conn, TransportError error, string reason)
		{
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual void OnServerChangeScene(string newSceneName)
		{
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual void OnServerSceneChanged(string sceneName)
		{
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x000088CC File Offset: 0x00006ACC
		public virtual void OnClientConnect()
		{
			if (!this.clientLoadedScene)
			{
				if (!NetworkClient.ready)
				{
					NetworkClient.Ready();
				}
				if (this.autoCreatePlayer)
				{
					NetworkClient.AddPlayer();
				}
			}
		}

		// Token: 0x060001DA RID: 474 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual void OnClientDisconnect()
		{
		}

		// Token: 0x060001DB RID: 475 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual void OnClientError(TransportError error, string reason)
		{
		}

		// Token: 0x060001DC RID: 476 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual void OnClientNotReady()
		{
		}

		// Token: 0x060001DD RID: 477 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling)
		{
		}

		// Token: 0x060001DE RID: 478 RVA: 0x000088F4 File Offset: 0x00006AF4
		public virtual void OnClientSceneChanged()
		{
			if (NetworkClient.connection.isAuthenticated && !NetworkClient.ready)
			{
				NetworkClient.Ready();
			}
			if (NetworkClient.connection.isAuthenticated && this.clientSceneOperation == SceneOperation.Normal && this.autoCreatePlayer && NetworkClient.localPlayer == null)
			{
				NetworkClient.AddPlayer();
			}
		}

		// Token: 0x060001DF RID: 479 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual void OnStartHost()
		{
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual void OnStartServer()
		{
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual void OnStartClient()
		{
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual void OnStopServer()
		{
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual void OnStopClient()
		{
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual void OnStopHost()
		{
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00008949 File Offset: 0x00006B49
		private void OnGUI()
		{
			if (!this.timeInterpolationGui)
			{
				return;
			}
			NetworkClient.OnGUI();
		}

		// Token: 0x040000DA RID: 218
		[Header("Configuration")]
		[FormerlySerializedAs("m_DontDestroyOnLoad")]
		[Tooltip("Should the Network Manager object be persisted through scene changes?")]
		public bool dontDestroyOnLoad = true;

		// Token: 0x040000DB RID: 219
		[FormerlySerializedAs("m_RunInBackground")]
		[Tooltip("Multiplayer games should always run in the background so the network doesn't time out.")]
		public bool runInBackground = true;

		// Token: 0x040000DC RID: 220
		[Header("Headless Builds")]
		[Tooltip("Should the server auto-start when 'Server Build' is checked in build settings")]
		[FormerlySerializedAs("startOnHeadless")]
		public bool autoStartServerBuild = true;

		// Token: 0x040000DD RID: 221
		[Tooltip("Automatically connect the client in headless builds. Useful for CCU tests with bot clients.\n\nAddress may be passed as command line argument.\n\nMake sure that only 'autostartServer' or 'autoconnectClient' is enabled, not both!")]
		public bool autoConnectClientBuild;

		// Token: 0x040000DE RID: 222
		[Tooltip("Server & Client send rate per second. Use 60-100Hz for fast paced games like Counter-Strike to minimize latency. Use around 30Hz for games like WoW to minimize computations. Use around 1-10Hz for slow paced games like EVE.")]
		[FormerlySerializedAs("serverTickRate")]
		public int sendRate = 60;

		// Token: 0x040000DF RID: 223
		[Header("Scene Management")]
		[Scene]
		[FormerlySerializedAs("m_OfflineScene")]
		[Tooltip("Scene that Mirror will switch to when the client or server is stopped")]
		public string offlineScene = "";

		// Token: 0x040000E0 RID: 224
		[Scene]
		[FormerlySerializedAs("m_OnlineScene")]
		[Tooltip("Scene that Mirror will switch to when the server is started. Clients will recieve a Scene Message to load the server's current scene when they connect.")]
		public string onlineScene = "";

		// Token: 0x040000E1 RID: 225
		[Header("Network Info")]
		[Tooltip("Transport component attached to this object that server and client will use to connect")]
		public Transport transport;

		// Token: 0x040000E2 RID: 226
		[FormerlySerializedAs("m_NetworkAddress")]
		[Tooltip("Network Address where the client should connect to the server. Server does not use this for anything.")]
		public string networkAddress = "localhost";

		// Token: 0x040000E3 RID: 227
		[FormerlySerializedAs("m_MaxConnections")]
		[Tooltip("Maximum number of concurrent connections.")]
		public int maxConnections = 100;

		// Token: 0x040000E4 RID: 228
		[Tooltip("When enabled, the server automatically disconnects inactive connections after the configured timeout.")]
		public bool disconnectInactiveConnections;

		// Token: 0x040000E5 RID: 229
		[Tooltip("Timeout in seconds for server to automatically disconnect inactive connections if 'disconnectInactiveConnections' is enabled.")]
		public float disconnectInactiveTimeout = 60f;

		// Token: 0x040000E6 RID: 230
		[Header("Authentication")]
		[Tooltip("Authentication component attached to this object")]
		public NetworkAuthenticator authenticator;

		// Token: 0x040000E7 RID: 231
		[Header("Player Object")]
		[FormerlySerializedAs("m_PlayerPrefab")]
		[Tooltip("Prefab of the player object. Prefab must have a Network Identity component. May be an empty game object or a full avatar.")]
		public GameObject playerPrefab;

		// Token: 0x040000E8 RID: 232
		[FormerlySerializedAs("m_AutoCreatePlayer")]
		[Tooltip("Should Mirror automatically spawn the player after scene change?")]
		public bool autoCreatePlayer = true;

		// Token: 0x040000E9 RID: 233
		[FormerlySerializedAs("m_PlayerSpawnMethod")]
		[Tooltip("Round Robin or Random order of Start Position selection")]
		public PlayerSpawnMethod playerSpawnMethod;

		// Token: 0x040000EA RID: 234
		[FormerlySerializedAs("m_SpawnPrefabs")]
		[HideInInspector]
		public List<GameObject> spawnPrefabs = new List<GameObject>();

		// Token: 0x040000EB RID: 235
		public static List<Transform> startPositions = new List<Transform>();

		// Token: 0x040000EC RID: 236
		public static int startPositionIndex;

		// Token: 0x040000ED RID: 237
		[Header("Snapshot Interpolation")]
		public SnapshotInterpolationSettings snapshotSettings = new SnapshotInterpolationSettings();

		// Token: 0x040000EE RID: 238
		[Header("Connection Quality")]
		public float connectionQualityInterval = 3f;

		// Token: 0x040000EF RID: 239
		private double lastConnectionQualityUpdate;

		// Token: 0x040000F0 RID: 240
		private ConnectionQuality lastConnectionQuality = ConnectionQuality.ESTIMATING;

		// Token: 0x040000F1 RID: 241
		[Header("Debug")]
		public bool timeInterpolationGui;

		// Token: 0x040000F3 RID: 243
		internal static NetworkConnection clientReadyConnection;

		// Token: 0x040000F4 RID: 244
		protected bool clientLoadedScene;

		// Token: 0x040000F6 RID: 246
		private bool finishStartHostPending;

		// Token: 0x040000F8 RID: 248
		public static AsyncOperation loadingSceneAsync;

		// Token: 0x040000F9 RID: 249
		private SceneOperation clientSceneOperation;
	}
}
