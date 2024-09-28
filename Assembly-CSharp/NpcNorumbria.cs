using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

// Token: 0x02000473 RID: 1139
public class NpcNorumbria : MonoBehaviour
{
	// Token: 0x17000273 RID: 627
	// (get) Token: 0x06001999 RID: 6553 RVA: 0x00082B78 File Offset: 0x00080D78
	// (set) Token: 0x0600199A RID: 6554 RVA: 0x00082B7F File Offset: 0x00080D7F
	public static float TvtStartTime { get; private set; }

	// Token: 0x17000274 RID: 628
	// (get) Token: 0x0600199B RID: 6555 RVA: 0x00082B87 File Offset: 0x00080D87
	// (set) Token: 0x0600199C RID: 6556 RVA: 0x00082B8E File Offset: 0x00080D8E
	public static int BlueTeamScore { get; set; }

	// Token: 0x17000275 RID: 629
	// (get) Token: 0x0600199D RID: 6557 RVA: 0x00082B96 File Offset: 0x00080D96
	// (set) Token: 0x0600199E RID: 6558 RVA: 0x00082B9D File Offset: 0x00080D9D
	public static int BlueTeamAsssits { get; set; }

	// Token: 0x17000276 RID: 630
	// (get) Token: 0x0600199F RID: 6559 RVA: 0x00082BA5 File Offset: 0x00080DA5
	// (set) Token: 0x060019A0 RID: 6560 RVA: 0x00082BAC File Offset: 0x00080DAC
	public static int RedTeamScore { get; set; }

	// Token: 0x17000277 RID: 631
	// (get) Token: 0x060019A1 RID: 6561 RVA: 0x00082BB4 File Offset: 0x00080DB4
	// (set) Token: 0x060019A2 RID: 6562 RVA: 0x00082BBB File Offset: 0x00080DBB
	public static int RedTeamAssists { get; set; }

	// Token: 0x060019A3 RID: 6563 RVA: 0x00082BC4 File Offset: 0x00080DC4
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		GameObject.FindGameObjectWithTag("DatabaseManager").TryGetComponent<ItemDatabaseModule>(out this.itemDatabaseModule);
		GameObject.FindGameObjectWithTag("GameEnvironment").TryGetComponent<GameEnvironmentModule>(out this.gameEnvironmentModule);
		base.TryGetComponent<NpcModule>(out this.npcModule);
		NpcDialog handshakeDialog = new NpcDialog("npc_norumbria_name", "npc_norumbria_handshake", new NpcChoice[]
		{
			new NpcChoice("npc_norumbria_register_tvt", 1),
			new NpcChoice("npc_norumbria_unregister_tvt", 2),
			new NpcChoice("npc_norumbria_token_store", 3),
			new NpcChoice("npc_norumbria_no_thanks", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
		this.npcModule.AddAction(new NpcAction(1, new NpcAction.NpcTask(this.RegisterPlayer)));
		this.npcModule.AddAction(new NpcAction(2, new NpcAction.NpcTask(this.UnregisterPlayer)));
		this.npcModule.AddAction(new NpcAction(3, new NpcAction.NpcTask(this.OpenTokenStore)));
		this.npcModule.AddAction(new NpcAction(4, delegate(GameObject player)
		{
			this.ExchangeTokensForItem(player, 224, 10);
		}));
		this.npcModule.AddAction(new NpcAction(5, delegate(GameObject player)
		{
			this.ExchangeTokensForItem(player, 196, 10);
		}));
		this.npcModule.AddAction(new NpcAction(6, delegate(GameObject player)
		{
			this.ExchangeTokensForItem(player, 440, 100);
		}));
		base.StartCoroutine(this.AnnounceTvt());
		base.InvokeRepeating("TvtFunctionTimer", 30f, 60f);
	}

	// Token: 0x060019A4 RID: 6564 RVA: 0x00082D54 File Offset: 0x00080F54
	private void OpenTokenStore(GameObject player)
	{
		PlayerModule playerModule;
		player.TryGetComponent<PlayerModule>(out playerModule);
		NpcDialog npcDialog = new NpcDialog("npc_norumbria_name", "npc_norumbria_token_store_dialog", new NpcChoice[]
		{
			new NpcChoice("npc_norumbria_axp_potion", 4),
			new NpcChoice("npc_norumbria_exp_potion", 5),
			new NpcChoice("npc_norumbria_skin", 6),
			new NpcChoice("npc_norumbria_exit", 99999)
		});
		playerModule.RenderNpcDialog(npcDialog);
	}

	// Token: 0x060019A5 RID: 6565 RVA: 0x00082DD4 File Offset: 0x00080FD4
	private void ExchangeTokensForItem(GameObject player, int itemId, int tokensNeeded)
	{
		PlayerModule playerModule;
		player.TryGetComponent<PlayerModule>(out playerModule);
		InventoryModule inventoryModule;
		player.TryGetComponent<InventoryModule>(out inventoryModule);
		int itemId2 = 195;
		if (inventoryModule.GetAmount(itemId2) < tokensNeeded)
		{
			NpcDialog npcDialog = new NpcDialog("npc_norumbria_name", "npc_norumbria_insuficient_tokens", new NpcChoice[]
			{
				new NpcChoice("npc_norumbria_ok", 99999)
			});
			playerModule.RenderNpcDialog(npcDialog);
			return;
		}
		Item item = this.itemDatabaseModule.GetItem(itemId);
		if (!inventoryModule.HasFreeSlots(item, 1))
		{
			NpcDialog npcDialog2 = new NpcDialog("npc_norumbria_name", "npc_norumbria_not_enough_inventory_slots", new NpcChoice[]
			{
				new NpcChoice("npc_norumbria_ok", 99999)
			});
			playerModule.RenderNpcDialog(npcDialog2);
			return;
		}
		inventoryModule.ConsumeItem(itemId2, tokensNeeded);
		inventoryModule.AddItem(item, -1, 1, true);
	}

	// Token: 0x060019A6 RID: 6566 RVA: 0x00082E98 File Offset: 0x00081098
	private void RegisterPlayer(GameObject player)
	{
		if (player == null || !player.activeInHierarchy)
		{
			return;
		}
		PlayerModule playerModule;
		if (!player.TryGetComponent<PlayerModule>(out playerModule))
		{
			return;
		}
		InventoryModule inventoryModule;
		if (!player.TryGetComponent<InventoryModule>(out inventoryModule))
		{
			return;
		}
		if (!this.registrationOpen)
		{
			NpcDialog npcDialog = new NpcDialog("npc_norumbria_name", "npc_norumbria_registration_closed", new NpcChoice[]
			{
				new NpcChoice("ok", 99999)
			});
			playerModule.RenderNpcDialog(npcDialog);
			return;
		}
		if (this.registeredPlayers.Contains(player))
		{
			NpcDialog npcDialog2 = new NpcDialog("npc_norumbria_name", "npc_norumbria_already_registered", new NpcChoice[]
			{
				new NpcChoice("ok", 99999)
			});
			playerModule.RenderNpcDialog(npcDialog2);
			return;
		}
		if (inventoryModule.EmptySlots < 4)
		{
			NpcDialog npcDialog3 = new NpcDialog("npc_norumbria_name", "npc_norumbria_not_enough_inventory_slots_to_register", new NpcChoice[]
			{
				new NpcChoice("ok", 99999)
			});
			playerModule.RenderNpcDialog(npcDialog3);
			return;
		}
		foreach (GameObject gameObject in this.registeredPlayers)
		{
			if (!(gameObject == null) && gameObject.activeInHierarchy)
			{
				PlayerModule playerModule2;
				gameObject.TryGetComponent<PlayerModule>(out playerModule2);
				if (playerModule2.DevicePlatform == playerModule.DevicePlatform)
				{
					NpcDialog npcDialog4 = new NpcDialog("npc_norumbria_name", "npc_norumbria_already_registered", new NpcChoice[]
					{
						new NpcChoice("ok", 99999)
					});
					playerModule.RenderNpcDialog(npcDialog4);
					break;
				}
			}
		}
		this.registeredPlayers.Add(player);
		NpcDialog npcDialog5 = new NpcDialog("npc_norumbria_name", "npc_norumbria_player_registered", new NpcChoice[]
		{
			new NpcChoice("ok", 99999)
		});
		playerModule.RenderNpcDialog(npcDialog5);
	}

	// Token: 0x060019A7 RID: 6567 RVA: 0x00083078 File Offset: 0x00081278
	private void UnregisterPlayer(GameObject player)
	{
		if (player == null || !player.activeInHierarchy)
		{
			return;
		}
		PlayerModule playerModule;
		if (!player.TryGetComponent<PlayerModule>(out playerModule))
		{
			return;
		}
		if (!this.registeredPlayers.Contains(player))
		{
			NpcDialog npcDialog = new NpcDialog("npc_norumbria_name", "npc_norumbria_not_registered", new NpcChoice[]
			{
				new NpcChoice("ok", 99999)
			});
			playerModule.RenderNpcDialog(npcDialog);
			return;
		}
		this.registeredPlayers.Remove(player);
		NpcDialog npcDialog2 = new NpcDialog("npc_norumbria_name", "npc_norumbria_register_removed", new NpcChoice[]
		{
			new NpcChoice("ok", 99999)
		});
		playerModule.RenderNpcDialog(npcDialog2);
	}

	// Token: 0x060019A8 RID: 6568 RVA: 0x00083128 File Offset: 0x00081328
	private void TvtFunctionTimer()
	{
		if (this.registrationOpen | this.registeredPlayers.Count < 4)
		{
			return;
		}
		int num = NpcNorumbria.RedTeamScore + NpcNorumbria.RedTeamAssists / 3;
		int num2 = NpcNorumbria.BlueTeamScore + NpcNorumbria.BlueTeamAsssits / 3;
		int num3 = 0;
		int num4 = 0;
		foreach (GameObject gameObject in this.registeredPlayers)
		{
			PlayerModule playerModule;
			PvpModule pvpModule;
			EffectModule effectModule;
			if (!(gameObject == null) && gameObject.activeInHierarchy && gameObject.TryGetComponent<PlayerModule>(out playerModule) && gameObject.TryGetComponent<PvpModule>(out pvpModule) && gameObject.TryGetComponent<EffectModule>(out effectModule) && pvpModule.TvtTeam != TvtTeam.None)
			{
				if (pvpModule.TvtTeam == TvtTeam.RedTeam)
				{
					num3++;
				}
				if (pvpModule.TvtTeam == TvtTeam.BlueTeam)
				{
					num4++;
				}
				this.ValidateIdlePlayer(120f, 500, pvpModule, effectModule, playerModule);
				this.ValidateIdlePlayer(240f, 1000, pvpModule, effectModule, playerModule);
				this.ValidateIdlePlayer(480f, 2000, pvpModule, effectModule, playerModule);
				effectModule.ShowScreenMessage("tvt_score_broadcast", 2, 7f, new string[]
				{
					num2.ToString(),
					num.ToString()
				});
			}
		}
		this.totalRedTeamPlayers = num3;
		this.totalBlueTeamPlayers = num4;
	}

	// Token: 0x060019A9 RID: 6569 RVA: 0x0008329C File Offset: 0x0008149C
	private void ValidateIdlePlayer(float timeLimiter, int minDamage, PvpModule playerPvpModule, EffectModule playerEffectModule, PlayerModule playerModule)
	{
		if (Time.time - NpcNorumbria.TvtStartTime <= timeLimiter)
		{
			return;
		}
		if (playerPvpModule.TeamFightDamage >= minDamage)
		{
			return;
		}
		if (playerPvpModule.TeamFightScore >= 5 | playerPvpModule.TeamFightAssist >= 10)
		{
			return;
		}
		playerEffectModule.ShowScreenMessage("tvt_idle_removed_message", 3, 7f, Array.Empty<string>());
		playerModule.Respawn();
	}

	// Token: 0x060019AA RID: 6570 RVA: 0x000832FD File Offset: 0x000814FD
	private IEnumerator AnnounceTvt()
	{
		NpcNorumbria.TvtStartTime = 0f;
		NpcNorumbria.BlueTeamAsssits = 0;
		NpcNorumbria.BlueTeamScore = 0;
		NpcNorumbria.RedTeamAssists = 0;
		NpcNorumbria.RedTeamScore = 0;
		this.registrationOpen = true;
		string message = "A TVT EVENT WILL START IN 10 MINUTES\r\nUM EVENTO DE TVT IRÁ INICIAR EM 10 MINUTOS.";
		this.gameEnvironmentModule.BroadcastScreenMessage("tvt_announcement_broadcast_ten_minutes_message", 2, 7f, Array.Empty<string>());
		yield return DiscordWebhookManager.SendInGameEventsChannel(message);
		yield return new WaitForSecondsRealtime(300f);
		message = "A TVT EVENT WILL START IN 5 MINUTES\r\nUM EVENTO DE TVT IRÁ INICIAR EM 5 MINUTOS.";
		this.gameEnvironmentModule.BroadcastScreenMessage("tvt_announcement_broadcast_five_minutes_message", 2, 7f, Array.Empty<string>());
		yield return DiscordWebhookManager.SendInGameEventsChannel(message);
		yield return new WaitForSecondsRealtime(240f);
		message = "A TVT EVENT WILL START IN 1 MINUTE\r\nUM EVENTO DE TVT IRÁ INICIAR EM 1 MINUTO.";
		this.gameEnvironmentModule.BroadcastScreenMessage("tvt_announcement_broadcast_one_minute_message", 2, 7f, Array.Empty<string>());
		yield return DiscordWebhookManager.SendInGameEventsChannel(message);
		yield return new WaitForSecondsRealtime(60f);
		yield return this.StartTvt();
		yield break;
	}

	// Token: 0x060019AB RID: 6571 RVA: 0x0008330C File Offset: 0x0008150C
	private IEnumerator StartTvt()
	{
		this.registrationOpen = false;
		if ((from p in this.registeredPlayers
		where p != null && p.activeInHierarchy
		select p).Count<GameObject>() < 4)
		{
			yield return this.BroadcastTvtCanceled();
			NetworkServer.Destroy(base.gameObject);
			yield break;
		}
		this.registeredPlayers.RemoveAll((GameObject p) => p == null || !p.activeInHierarchy);
		bool flag = UnityEngine.Random.Range(0, 2) == 0;
		foreach (GameObject gameObject in from player in this.registeredPlayers
		orderby player.GetComponent<VocationModule>().Vocation, player.GetComponent<AttributeModule>().BaseLevel descending
		select player)
		{
			PvpModule pvpModule;
			AttributeModule attributeModule;
			PartyModule partyModule;
			if (!(gameObject == null) && gameObject.TryGetComponent<PvpModule>(out pvpModule) && gameObject.TryGetComponent<AttributeModule>(out attributeModule) && gameObject.TryGetComponent<PartyModule>(out partyModule))
			{
				partyModule.LeaveParty();
				MovementModule movementModule;
				gameObject.TryGetComponent<MovementModule>(out movementModule);
				Vector3 locationFromSpawnPoint = GlobalUtils.GetLocationFromSpawnPoint(flag ? "red_team_spawn_point" : "blue_team_spawn_point");
				movementModule.TargetTeleport(movementModule.connectionToClient, locationFromSpawnPoint, new Effect("TeleporterHit", 0.5f, 0.25f));
				if (pvpModule.PvpStatus == PvpStatus.Neutral)
				{
					pvpModule.SetPvpStatusAsync(PvpStatus.InCombat, false);
				}
				pvpModule.NetworkTvtTeam = (flag ? TvtTeam.RedTeam : TvtTeam.BlueTeam);
				pvpModule.NetworkPvpEnabled = true;
				attributeModule.SetEndurance(150);
				flag = !flag;
			}
		}
		NpcNorumbria.TvtStartTime = Time.time;
		WaitForSecondsRealtime delay = new WaitForSecondsRealtime(30f);
		for (;;)
		{
			if (Time.time - NpcNorumbria.TvtStartTime > 120f)
			{
				if (this.totalRedTeamPlayers == 0 & this.totalBlueTeamPlayers == 0)
				{
					break;
				}
				if (this.totalRedTeamPlayers == 0 & this.totalBlueTeamPlayers > 0)
				{
					goto Block_9;
				}
				if (this.totalBlueTeamPlayers == 0 & this.totalRedTeamPlayers > 0)
				{
					goto Block_10;
				}
			}
			yield return delay;
			if (Time.time - NpcNorumbria.TvtStartTime > 600f)
			{
				goto IL_309;
			}
		}
		yield return this.BroadcastTvtCanceled();
		yield break;
		Block_9:
		yield return this.BroadcastRedTeamWo();
		goto IL_309;
		Block_10:
		yield return this.BroadcastBlueTeamWo();
		IL_309:
		int redTeamScore = NpcNorumbria.RedTeamScore + NpcNorumbria.RedTeamAssists / 3;
		int blueTeamScore = NpcNorumbria.BlueTeamScore + NpcNorumbria.BlueTeamAsssits / 3;
		List<GameObject> redTeamPlayers = new List<GameObject>();
		List<GameObject> blueTeamPlayers = new List<GameObject>();
		foreach (GameObject gameObject2 in this.registeredPlayers)
		{
			PlayerModule playerModule;
			PvpModule pvpModule2;
			AttributeModule attributeModule2;
			if (!(gameObject2 == null) && gameObject2.activeInHierarchy && gameObject2.TryGetComponent<PlayerModule>(out playerModule) && gameObject2.TryGetComponent<PvpModule>(out pvpModule2) && gameObject2.TryGetComponent<AttributeModule>(out attributeModule2) && pvpModule2.TvtTeam != TvtTeam.None)
			{
				if (pvpModule2.TeamFightScore > 0 | pvpModule2.TeamFightAssist > 0)
				{
					if (pvpModule2.TvtTeam == TvtTeam.RedTeam)
					{
						redTeamPlayers.Add(gameObject2);
					}
					if (pvpModule2.TvtTeam == TvtTeam.BlueTeam)
					{
						blueTeamPlayers.Add(gameObject2);
					}
				}
				else
				{
					EffectModule effectModule;
					gameObject2.TryGetComponent<EffectModule>(out effectModule);
					effectModule.ShowScreenMessage("tvt_not_enough_points_message", 2, 7f, Array.Empty<string>());
				}
				pvpModule2.NetworkTvtTeam = TvtTeam.None;
				PvpModule pvpModule3 = pvpModule2;
				pvpModule3.NetworkWeekTvtScore = pvpModule3.WeekTvtScore + (pvpModule2.TeamFightScore + pvpModule2.TeamFightAssist / 3);
				pvpModule2.NetworkTeamFightScore = 0;
				pvpModule2.NetworkTeamFightAssist = 0;
				pvpModule2.NetworkPvpEnabled = false;
				if (pvpModule2.PvpStatus == PvpStatus.InCombat)
				{
					pvpModule2.SetPvpStatusAsync(PvpStatus.Neutral, false);
				}
				pvpModule2.TeamFightDamage = 0;
				pvpModule2.TeamFightDeaths.Clear();
				attributeModule2.SetEnduranceToMax();
				playerModule.Respawn();
			}
		}
		if (redTeamScore > blueTeamScore)
		{
			yield return this.BroadcastWinnerTeam(TvtTeam.RedTeam, blueTeamScore, redTeamScore);
			this.AddPrizesToWinners(redTeamPlayers);
		}
		if (blueTeamScore > redTeamScore)
		{
			yield return this.BroadcastWinnerTeam(TvtTeam.BlueTeam, blueTeamScore, redTeamScore);
			this.AddPrizesToWinners(blueTeamPlayers);
		}
		if (blueTeamScore == redTeamScore)
		{
			yield return this.BroadcastWinnerTeam(TvtTeam.None, blueTeamScore, redTeamScore);
		}
		this.registeredPlayers.Clear();
		yield return new WaitForSecondsRealtime(60f);
		NetworkServer.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x060019AC RID: 6572 RVA: 0x0008331C File Offset: 0x0008151C
	private void AddPrizesToWinners(List<GameObject> winnerPlayers)
	{
		foreach (GameObject gameObject in winnerPlayers)
		{
			AttributeModule attributeModule;
			InventoryModule inventoryModule;
			WalletModule walletModule;
			PvpModule pvpModule;
			if (!(gameObject == null) && gameObject.activeInHierarchy && gameObject.TryGetComponent<AttributeModule>(out attributeModule) && gameObject.TryGetComponent<InventoryModule>(out inventoryModule) && gameObject.TryGetComponent<WalletModule>(out walletModule) && gameObject.TryGetComponent<PvpModule>(out pvpModule))
			{
				int num = 1;
				if (attributeModule.BaseLevel < 40)
				{
					walletModule.AddGoldCoins(7500 * num);
					inventoryModule.AddItem(this.itemDatabaseModule.GetItem(195), -1, num, true);
					inventoryModule.AddItem(this.itemDatabaseModule.GetItem(13), -1, 10 * num, true);
					inventoryModule.AddItem(this.itemDatabaseModule.GetItem(279), -1, 2 * num, true);
				}
				else if (attributeModule.BaseLevel < 80)
				{
					walletModule.AddGoldCoins(15000 * num);
					inventoryModule.AddItem(this.itemDatabaseModule.GetItem(195), -1, num, true);
					inventoryModule.AddItem(this.itemDatabaseModule.GetItem(13), -1, 30 * num, true);
					inventoryModule.AddItem(this.itemDatabaseModule.GetItem(221), -1, 5 * num, true);
					inventoryModule.AddItem(this.itemDatabaseModule.GetItem(279), -1, 5 * num, true);
				}
				else if (attributeModule.BaseLevel < 125)
				{
					walletModule.AddGoldCoins(40000 * num);
					inventoryModule.AddItem(this.itemDatabaseModule.GetItem(195), -1, num, true);
					inventoryModule.AddItem(this.itemDatabaseModule.GetItem(221), -1, 15 * num, true);
					inventoryModule.AddItem(this.itemDatabaseModule.GetItem(222), -1, num, true);
					inventoryModule.AddItem(this.itemDatabaseModule.GetItem(280), -1, num, true);
				}
				else if (attributeModule.BaseLevel < 350)
				{
					walletModule.AddGoldCoins(75000 * num);
					inventoryModule.AddItem(this.itemDatabaseModule.GetItem(195), -1, num, true);
					inventoryModule.AddItem(this.itemDatabaseModule.GetItem(222), -1, 8 * num, true);
					inventoryModule.AddItem(this.itemDatabaseModule.GetItem(280), -1, num, true);
					inventoryModule.AddItem(this.itemDatabaseModule.GetItem(283), -1, num, true);
				}
				else
				{
					walletModule.AddGoldCoins(120000 * num);
					inventoryModule.AddItem(this.itemDatabaseModule.GetItem(195), -1, num, true);
					inventoryModule.AddItem(this.itemDatabaseModule.GetItem(223), -1, num, true);
					inventoryModule.AddItem(this.itemDatabaseModule.GetItem(280), -1, 2 * num, true);
					inventoryModule.AddItem(this.itemDatabaseModule.GetItem(283), -1, 2 * num, true);
				}
			}
		}
	}

	// Token: 0x060019AD RID: 6573 RVA: 0x0008364C File Offset: 0x0008184C
	private IEnumerator BroadcastWinnerTeam(TvtTeam winnerTeam, int blueTeamScore, int redTeamScore)
	{
		if (winnerTeam == TvtTeam.None)
		{
			string message = string.Format("THE TVT EVENT HAS ENDED AND THE RESULT WAS A DRAW. >>BLUE TEAM {0} X {1} RED TEAM<<\r\n", blueTeamScore, redTeamScore) + string.Format("O EVENTO DE TVT TERMINOU E O RESULTADO FOI EMPATE. >>TIME AZUL {0} X {1} TIME VERMELHO<<", blueTeamScore, redTeamScore);
			this.gameEnvironmentModule.BroadcastScreenMessage("draw_tvt_broadcast_message", 2, 7f, new string[]
			{
				blueTeamScore.ToString(),
				redTeamScore.ToString()
			});
			yield return DiscordWebhookManager.SendInGameEventsChannel(message);
			yield break;
		}
		if (winnerTeam == TvtTeam.RedTeam)
		{
			string message2 = string.Format("THE TVT EVENT HAS ENDED AND THE WINNER WAS: RED TEAM. >>BLUE TEAM {0} X {1} RED TEAM<<\r\n", blueTeamScore, redTeamScore) + string.Format("O EVENTO DE TVT TERMINOU E O VENCEDOR FOI: TIME VERMELHO. >>TIME AZUL {0} X {1} TIME VERMELHO<<", blueTeamScore, redTeamScore);
			this.gameEnvironmentModule.BroadcastScreenMessage("red_team_tvt_winner_broadcast_message", 2, 7f, new string[]
			{
				blueTeamScore.ToString(),
				redTeamScore.ToString()
			});
			yield return DiscordWebhookManager.SendInGameEventsChannel(message2);
			yield break;
		}
		if (winnerTeam == TvtTeam.BlueTeam)
		{
			string message3 = string.Format("THE TVT EVENT HAS ENDED AND THE WINNER WAS: BLUE TEAM. >>BLUE TEAM {0} X {1} RED TEAM\r\n", blueTeamScore, redTeamScore) + string.Format("O EVENTO DE TVT TERMINOU E O VENCEDOR FOI: TIME AZUL. >>TIME AZUL {0} X {1} TIME VERMELHO<<", blueTeamScore, redTeamScore);
			this.gameEnvironmentModule.BroadcastScreenMessage("blue_team_tvt_winner_broadcast_message", 2, 7f, new string[]
			{
				blueTeamScore.ToString(),
				redTeamScore.ToString()
			});
			yield return DiscordWebhookManager.SendInGameEventsChannel(message3);
			yield break;
		}
		yield break;
	}

	// Token: 0x060019AE RID: 6574 RVA: 0x00083670 File Offset: 0x00081870
	private IEnumerator BroadcastTvtCanceled()
	{
		string message = string.Format("THE TVT EVENT HAS BEEN CANCELED BECAUSE LESS THAN {0} PLAYERS WAS REGISTERED.\r\n", 4) + string.Format("O EVENTO DE TVT FOI CANCELADO, POIS MENOS DE {0} JOGADORES ESTAVAM REGISTRADOS.", 4);
		yield return DiscordWebhookManager.SendInGameEventsChannel(message);
		this.gameEnvironmentModule.BroadcastScreenMessage("tvt_canceled_message", 2, 7f, Array.Empty<string>());
		yield break;
	}

	// Token: 0x060019AF RID: 6575 RVA: 0x0008367F File Offset: 0x0008187F
	private IEnumerator BroadcastRedTeamWo()
	{
		string message = "THE TVT HAS ENDED AS ALL PARTICIPANTS OF THE RED TEAM HAVE LEFT/WITHDREW\r\nO TVT FOI FINALIZADO, POIS TODOS OS PARTICIPANTES DO TIME VERMELHO SAIRAM/DESISTIRAM.";
		yield return DiscordWebhookManager.SendInGameEventsChannel(message);
		this.gameEnvironmentModule.BroadcastScreenMessage("red_team_wo_broadcast_message", 2, 7f, Array.Empty<string>());
		yield break;
	}

	// Token: 0x060019B0 RID: 6576 RVA: 0x0008368E File Offset: 0x0008188E
	private IEnumerator BroadcastBlueTeamWo()
	{
		string message = "THE TVT HAS ENDED AS ALL PARTICIPANTS OF THE BLUE TEAM HAVE LEFT/WITHDREW\r\nO TVT FOI FINALIZADO, POIS TODOS OS PARTICIPANTES DO TIME AZUL SAIRAM/DESISTIRAM.";
		yield return DiscordWebhookManager.SendInGameEventsChannel(message);
		this.gameEnvironmentModule.BroadcastScreenMessage("blue_team_wo_broadcast_message", 2, 7f, Array.Empty<string>());
		yield break;
	}

	// Token: 0x0400162B RID: 5675
	private readonly List<GameObject> registeredPlayers = new List<GameObject>();

	// Token: 0x0400162C RID: 5676
	private bool registrationOpen;

	// Token: 0x0400162D RID: 5677
	private NpcModule npcModule;

	// Token: 0x0400162E RID: 5678
	private GameEnvironmentModule gameEnvironmentModule;

	// Token: 0x0400162F RID: 5679
	private ItemDatabaseModule itemDatabaseModule;

	// Token: 0x04001635 RID: 5685
	public const int AssistBaseValue = 3;

	// Token: 0x04001636 RID: 5686
	private const float FirstAnnouncementInterval = 300f;

	// Token: 0x04001637 RID: 5687
	private const float SecondAnnouncementInterval = 240f;

	// Token: 0x04001638 RID: 5688
	private const float LastAnnouncementInterval = 60f;

	// Token: 0x04001639 RID: 5689
	private const float TvtDuration = 600f;

	// Token: 0x0400163A RID: 5690
	private const int MinimumRequiredRegisteredPlayers = 4;

	// Token: 0x0400163B RID: 5691
	private int totalBlueTeamPlayers;

	// Token: 0x0400163C RID: 5692
	private int totalRedTeamPlayers;
}
