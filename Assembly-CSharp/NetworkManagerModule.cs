using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Mirror;
using UnityEngine;

// Token: 0x02000389 RID: 905
public class NetworkManagerModule : NetworkManager
{
	// Token: 0x06001281 RID: 4737 RVA: 0x0005899C File Offset: 0x00056B9C
	public override void OnClientSceneChanged()
	{
		if (!NetworkClient.ready && NetworkClient.Ready())
		{
			PlayerSpawnNetworkMessage message = new PlayerSpawnNetworkMessage
			{
				PlayerId = this.ClientPlayerId,
				AccountId = this.ClientAccountId,
				PackageType = this.ClientPackageType,
				Position = this.ClientPlayerStartPosition,
				AccountUniqueId = this.ClientAccountUniqueId,
				ConnectionVersion = this.ClientConnectionVersion
			};
			NetworkClient.connection.Send<PlayerSpawnNetworkMessage>(message, 0);
			if (this.uiSystemModule == null)
			{
				GameObject.FindGameObjectWithTag("UISystem").TryGetComponent<UISystemModule>(out this.uiSystemModule);
			}
			this.uiSystemModule.ActivateMenuBackgroundImage();
			this.uiSystemModule.ShowFeedbackText(LanguageManager.Instance.GetText("loading_label"));
		}
	}

	// Token: 0x06001282 RID: 4738 RVA: 0x00058A6A File Offset: 0x00056C6A
	public override void OnServerDisconnect(NetworkConnectionToClient conn)
	{
		this.DisconnectFromServerAsync(conn);
	}

	// Token: 0x06001283 RID: 4739 RVA: 0x00058A74 File Offset: 0x00056C74
	private void DisconnectFromServerAsync(NetworkConnectionToClient conn)
	{
		try
		{
			if (conn.identity != null)
			{
				CreatureModule component = conn.identity.GetComponent<CreatureModule>();
				base.StartCoroutine(DiscordWebhookManager.SendInLoginHistoryChannel("Jogador " + component.CreatureName + " se **desconectou** ao servidor."));
			}
			NetworkServer.DestroyPlayerForConnection(conn);
			this.RemoveConnectedPlayer(conn);
			base.StartCoroutine(DiscordWebhookManager.SendInLoginHistoryChannel(string.Format("{0} jogadores online.", NetworkManagerModule.ConnectedPlayers.Count)));
			GameObject gameObject = GameObject.FindGameObjectWithTag("GameEnvironment");
			if (gameObject != null)
			{
				gameObject.GetComponent<GameEnvironmentModule>().RemoveContact(conn.connectionId);
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("(DisconnectFromServerAsync) Error: " + ex.Message + " Details: " + ex.StackTrace);
		}
	}

	// Token: 0x06001284 RID: 4740 RVA: 0x00058B48 File Offset: 0x00056D48
	public override void OnStartClient()
	{
		Application.wantsToQuit += this.WantsToQuit;
		NetworkClient.RegisterPrefab(Resources.Load<GameObject>("Prefabs/Npc"));
		NetworkClient.RegisterPrefab(Resources.Load<GameObject>("Prefabs/Monster"));
		GameObject[] array = Resources.LoadAll<GameObject>("Prefabs/Projectiles/");
		for (int i = 0; i < array.Length; i++)
		{
			NetworkClient.RegisterPrefab(array[i]);
		}
	}

	// Token: 0x06001285 RID: 4741 RVA: 0x00058BA8 File Offset: 0x00056DA8
	public override void OnStartServer()
	{
		NetworkManagerModule.<OnStartServer>d__20 <OnStartServer>d__;
		<OnStartServer>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<OnStartServer>d__.<>4__this = this;
		<OnStartServer>d__.<>1__state = -1;
		<OnStartServer>d__.<>t__builder.Start<NetworkManagerModule.<OnStartServer>d__20>(ref <OnStartServer>d__);
	}

	// Token: 0x06001286 RID: 4742 RVA: 0x00058BE0 File Offset: 0x00056DE0
	private bool WantsToQuit()
	{
		if (NetworkClient.active)
		{
			if (this.uiSystemModule != null)
			{
				if (this.uiSystemModule.CombatModule.IsFighting)
				{
					this.uiSystemModule.EffectModule.ShowScreenMessage("cant_quit_while_fighting_message", 3, 3.5f, Array.Empty<string>());
					return false;
				}
				if (this.uiSystemModule.CombatModule.InCombat)
				{
					this.uiSystemModule.EffectModule.ShowScreenMessage("cant_quit_right_after_combat_message", 3, 3.5f, Array.Empty<string>());
					return false;
				}
			}
			base.StopClient();
			return true;
		}
		if (NetworkServer.active)
		{
			foreach (KeyValuePair<int, GameObject> keyValuePair in NetworkManagerModule.ConnectedPlayers)
			{
				try
				{
					if (keyValuePair.Value != null && keyValuePair.Value.activeInHierarchy)
					{
						PlayerModule component = keyValuePair.Value.GetComponent<PlayerModule>();
						component.connectionToClient.Disconnect();
						this.SavePlayerDataOnQuit(component);
						Debug.LogFormat("Player {0} was disconected and saved due server closing.", new object[]
						{
							component.PlayerId
						});
					}
				}
				catch (Exception ex)
				{
					Debug.LogErrorFormat("Error: {0}. Details: {1}", new object[]
					{
						ex.Message,
						ex.StackTrace
					});
				}
			}
			base.OnApplicationQuit();
			return true;
		}
		return true;
	}

	// Token: 0x06001287 RID: 4743 RVA: 0x00058D58 File Offset: 0x00056F58
	private void UpdateServerCurrentPlayers()
	{
		NetworkManagerModule.<UpdateServerCurrentPlayers>d__22 <UpdateServerCurrentPlayers>d__;
		<UpdateServerCurrentPlayers>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<UpdateServerCurrentPlayers>d__.<>1__state = -1;
		<UpdateServerCurrentPlayers>d__.<>t__builder.Start<NetworkManagerModule.<UpdateServerCurrentPlayers>d__22>(ref <UpdateServerCurrentPlayers>d__);
	}

	// Token: 0x06001288 RID: 4744 RVA: 0x00058D88 File Offset: 0x00056F88
	private void CreateServerHistoryTimer()
	{
		NetworkManagerModule.<CreateServerHistoryTimer>d__23 <CreateServerHistoryTimer>d__;
		<CreateServerHistoryTimer>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CreateServerHistoryTimer>d__.<>1__state = -1;
		<CreateServerHistoryTimer>d__.<>t__builder.Start<NetworkManagerModule.<CreateServerHistoryTimer>d__23>(ref <CreateServerHistoryTimer>d__);
	}

	// Token: 0x06001289 RID: 4745 RVA: 0x00058DB8 File Offset: 0x00056FB8
	public static void CreateCheckpointLog()
	{
		Debug.Log(string.Format("Checkpoint: {0:dd/MM/yyyy HH:mm:ss}", DateTime.UtcNow));
		Debug.Log(string.Format("CurrentSavingAccountIds: {0}", NetworkManagerModule.CurrentSavingAccountIds.Count));
		Debug.Log(string.Format("Connected Players: {0}", NetworkManagerModule.ConnectedPlayers.Count));
		Debug.Log(ObjectPoolModule.Instance.GetServerPoolMonitorData());
		Debug.Log(ClassFactory.CacheMonitorData());
	}

	// Token: 0x0600128A RID: 4746 RVA: 0x00058E33 File Offset: 0x00057033
	private void ValidateRefundedPurchasesTimer()
	{
		base.StartCoroutine(this.NotifyPurchaseRefunds());
	}

	// Token: 0x0600128B RID: 4747 RVA: 0x00058E42 File Offset: 0x00057042
	private IEnumerator NotifyPurchaseRefunds()
	{
		yield return ApiManager.Post<object>("https://eternal-quest.online/api/payment/notify/googleplayrefunds", new
		{

		}, null);
		yield break;
	}

	// Token: 0x0600128C RID: 4748 RVA: 0x00058E4C File Offset: 0x0005704C
	private void CancelExpiredMarketOrdersTimer()
	{
		NetworkManagerModule.<CancelExpiredMarketOrdersTimer>d__27 <CancelExpiredMarketOrdersTimer>d__;
		<CancelExpiredMarketOrdersTimer>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CancelExpiredMarketOrdersTimer>d__.<>4__this = this;
		<CancelExpiredMarketOrdersTimer>d__.<>1__state = -1;
		<CancelExpiredMarketOrdersTimer>d__.<>t__builder.Start<NetworkManagerModule.<CancelExpiredMarketOrdersTimer>d__27>(ref <CancelExpiredMarketOrdersTimer>d__);
	}

	// Token: 0x0600128D RID: 4749 RVA: 0x00058E84 File Offset: 0x00057084
	private Task DestroyInvalidPlayerAsync(GameObject invalidPlayer)
	{
		NetworkManagerModule.<DestroyInvalidPlayerAsync>d__28 <DestroyInvalidPlayerAsync>d__;
		<DestroyInvalidPlayerAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<DestroyInvalidPlayerAsync>d__.invalidPlayer = invalidPlayer;
		<DestroyInvalidPlayerAsync>d__.<>1__state = -1;
		<DestroyInvalidPlayerAsync>d__.<>t__builder.Start<NetworkManagerModule.<DestroyInvalidPlayerAsync>d__28>(ref <DestroyInvalidPlayerAsync>d__);
		return <DestroyInvalidPlayerAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600128E RID: 4750 RVA: 0x00058EC8 File Offset: 0x000570C8
	private void LoadPlayerDataAsync(NetworkConnectionToClient conn, PlayerSpawnNetworkMessage spawnNetworkMessage)
	{
		NetworkManagerModule.<LoadPlayerDataAsync>d__29 <LoadPlayerDataAsync>d__;
		<LoadPlayerDataAsync>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<LoadPlayerDataAsync>d__.<>4__this = this;
		<LoadPlayerDataAsync>d__.conn = conn;
		<LoadPlayerDataAsync>d__.spawnNetworkMessage = spawnNetworkMessage;
		<LoadPlayerDataAsync>d__.<>1__state = -1;
		<LoadPlayerDataAsync>d__.<>t__builder.Start<NetworkManagerModule.<LoadPlayerDataAsync>d__29>(ref <LoadPlayerDataAsync>d__);
	}

	// Token: 0x0600128F RID: 4751 RVA: 0x00058F10 File Offset: 0x00057110
	private static void AddPlayerForConnection(NetworkConnectionToClient conn, GameObject player)
	{
		List<uint> list = new List<uint>();
		foreach (KeyValuePair<uint, NetworkIdentity> keyValuePair in NetworkServer.spawned)
		{
			if (keyValuePair.Value == null || keyValuePair.Value.gameObject == null)
			{
				list.Add(keyValuePair.Key);
			}
		}
		foreach (uint key in list)
		{
			NetworkServer.spawned.Remove(key);
		}
		NetworkServer.AddPlayerForConnection(conn, player);
	}

	// Token: 0x06001290 RID: 4752 RVA: 0x00058FE0 File Offset: 0x000571E0
	private void RemoveConnectedPlayer(NetworkConnection conn)
	{
		try
		{
			if (NetworkManagerModule.ConnectedPlayers.ContainsKey(conn.connectionId))
			{
				NetworkManagerModule.ConnectedPlayers.Remove(conn.connectionId);
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}

	// Token: 0x06001291 RID: 4753 RVA: 0x0005902C File Offset: 0x0005722C
	private void SavePlayerDataOnQuit(PlayerModule playerModule)
	{
		NetworkManagerModule.<SavePlayerDataOnQuit>d__32 <SavePlayerDataOnQuit>d__;
		<SavePlayerDataOnQuit>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<SavePlayerDataOnQuit>d__.playerModule = playerModule;
		<SavePlayerDataOnQuit>d__.<>1__state = -1;
		<SavePlayerDataOnQuit>d__.<>t__builder.Start<NetworkManagerModule.<SavePlayerDataOnQuit>d__32>(ref <SavePlayerDataOnQuit>d__);
	}

	// Token: 0x06001292 RID: 4754 RVA: 0x00059063 File Offset: 0x00057263
	public static void AddAccountToCurrentSaving(int accountId)
	{
		if (NetworkManagerModule.CurrentSavingAccountIds.ContainsKey(accountId))
		{
			NetworkManagerModule.CurrentSavingAccountIds[accountId] = Time.time;
			return;
		}
		NetworkManagerModule.CurrentSavingAccountIds.Add(accountId, Time.time);
	}

	// Token: 0x06001293 RID: 4755 RVA: 0x00059093 File Offset: 0x00057293
	public static void RemoveAccountFromCurrentSaving(int accountId)
	{
		if (NetworkManagerModule.CurrentSavingAccountIds.ContainsKey(accountId))
		{
			NetworkManagerModule.CurrentSavingAccountIds.Remove(accountId);
		}
	}

	// Token: 0x04001128 RID: 4392
	public int ClientAccountId;

	// Token: 0x04001129 RID: 4393
	public int ClientPlayerId;

	// Token: 0x0400112A RID: 4394
	public PackageType ClientPackageType;

	// Token: 0x0400112B RID: 4395
	public string ClientAccountUniqueId;

	// Token: 0x0400112C RID: 4396
	public string ClientConnectionVersion;

	// Token: 0x0400112D RID: 4397
	public Vector3 ClientPlayerStartPosition = Vector3.zero;

	// Token: 0x0400112E RID: 4398
	public static Dictionary<int, GameObject> ConnectedPlayers = new Dictionary<int, GameObject>();

	// Token: 0x0400112F RID: 4399
	public static string ServerMachineName;

	// Token: 0x04001130 RID: 4400
	private int currentRecord;

	// Token: 0x04001131 RID: 4401
	private UISystemModule uiSystemModule;

	// Token: 0x04001132 RID: 4402
	private ItemDatabaseModule itemDatabaseModule;

	// Token: 0x04001133 RID: 4403
	private SkillDatabaseModule skillDatabaseModule;

	// Token: 0x04001134 RID: 4404
	private GameEnvironmentModule gameEnvironmentModule;

	// Token: 0x04001135 RID: 4405
	private BlueprintDatabaseModule blueprintDatabaseModule;

	// Token: 0x04001136 RID: 4406
	private static bool serverLoaded;

	// Token: 0x04001137 RID: 4407
	public static Dictionary<int, float> CurrentSavingAccountIds = new Dictionary<int, float>();
}
