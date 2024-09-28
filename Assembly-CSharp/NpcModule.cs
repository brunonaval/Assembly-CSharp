using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003A0 RID: 928
public class NpcModule : NetworkBehaviour
{
	// Token: 0x1700020E RID: 526
	// (get) Token: 0x06001317 RID: 4887 RVA: 0x0005DC64 File Offset: 0x0005BE64
	// (set) Token: 0x06001318 RID: 4888 RVA: 0x0005DC6C File Offset: 0x0005BE6C
	public bool IsLoaded { get; private set; }

	// Token: 0x1700020F RID: 527
	// (get) Token: 0x06001319 RID: 4889 RVA: 0x0005DC78 File Offset: 0x0005BE78
	public bool HasAvailableQuests
	{
		get
		{
			if (this.quests == null)
			{
				return false;
			}
			if (this.uiSystemModule == null)
			{
				return false;
			}
			if (this.uiSystemModule.AttributeModule == null)
			{
				return false;
			}
			for (int i = 0; i < this.quests.Count; i++)
			{
				Quest quest = this.quests[i];
				if (this.uiSystemModule.AttributeModule.BaseLevel >= quest.RequiredLevel && quest.IsDefined && !this.uiSystemModule.QuestModule.HasQuest(quest.Id))
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x0600131A RID: 4890 RVA: 0x0005DD18 File Offset: 0x0005BF18
	private void Awake()
	{
		this.combatModule = base.GetComponent<CombatModule>();
		this.effectModule = base.GetComponent<EffectModule>();
		this.movementModule = base.GetComponent<MovementModule>();
		this.creatureModule = base.GetComponent<CreatureModule>();
		this.nonPlayerAIModule = base.GetComponent<NonPlayerAIModule>();
		this.nonPlayerSkillModule = base.GetComponent<NonPlayerSkillModule>();
		this.nonPlayerEquipmentModule = base.GetComponent<NonPlayerEquipmentModule>();
		this.nonPlayerAttributeModule = base.GetComponent<NonPlayerAttributeModule>();
		this.animationControllerModule = base.GetComponent<AnimationControllerModule>();
		if (NetworkServer.active)
		{
			UnityEngine.Object.Destroy(this.localHud);
			GameObject gameObject = GameObject.FindGameObjectWithTag("DatabaseManager");
			this.npcDatabaseModule = gameObject.GetComponent<NpcDatabaseModule>();
			this.questDatabaseModule = gameObject.GetComponent<QuestDatabaseModule>();
			this.skillDatabaseModule = gameObject.GetComponent<SkillDatabaseModule>();
			this.combatModule.OnReceiveDamage += this.HandleOnReceiveDamage;
			this.creatureModule.OnKilled += this.HandleOnKilled;
			base.InvokeRepeating("ServerUpdateTimer", 0f, 60f);
		}
		if (NetworkClient.active)
		{
			base.InvokeRepeating("ClientUpdateTimer", 0f, 1f);
		}
	}

	// Token: 0x0600131B RID: 4891 RVA: 0x0005DE31 File Offset: 0x0005C031
	private void OnDestroy()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		this.creatureModule.OnKilled -= this.HandleOnKilled;
		this.combatModule.OnReceiveDamage -= this.HandleOnReceiveDamage;
	}

	// Token: 0x0600131C RID: 4892 RVA: 0x0005DE6C File Offset: 0x0005C06C
	private void ClientUpdateTimer()
	{
		if (!NetworkClient.active)
		{
			return;
		}
		if (Time.time - this.updateTime < this.updateInterval)
		{
			return;
		}
		this.updateTime = Time.time;
		if (this.uiSystemModule == null)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
			this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
		}
		if (this.creatureModule.IsAlive)
		{
			this.nameText.color = Color.white;
			float num = 1f;
			if (this.nonPlayerAttributeModule != null)
			{
				num = (float)this.nonPlayerAttributeModule.CurrentHealth * 100f / (float)this.nonPlayerAttributeModule.MaxHealth;
			}
			this.healthBar.fillAmount = num / 100f;
			this.nameText.text = LanguageManager.Instance.GetText(this.creatureModule.CreatureName);
			this.titleText.text = LanguageManager.Instance.GetText(this.creatureModule.CreatureTitle);
			if (!this.localHud.activeInHierarchy)
			{
				this.localHud.SetActive(true);
			}
			bool hasAvailableQuests = this.HasAvailableQuests;
			if (hasAvailableQuests && !this.questIcon.activeInHierarchy)
			{
				this.questIcon.SetActive(true);
				return;
			}
			if (!hasAvailableQuests && this.questIcon.activeInHierarchy)
			{
				this.questIcon.SetActive(false);
				return;
			}
		}
		else
		{
			if (this.questIcon.activeInHierarchy)
			{
				this.questIcon.SetActive(false);
			}
			if (this.localHud.activeInHierarchy)
			{
				this.localHud.SetActive(false);
			}
		}
	}

	// Token: 0x0600131D RID: 4893 RVA: 0x0005DFF8 File Offset: 0x0005C1F8
	private void ServerUpdateTimer()
	{
		if (this.walkInterval == 0f)
		{
			return;
		}
		if (this.nonPlayerAIModule.HasDestination)
		{
			return;
		}
		if (this.combatModule.Target != null)
		{
			return;
		}
		if (!this.movementModule.CanMove())
		{
			return;
		}
		if (this.Owner != null)
		{
			return;
		}
		if ((this.IsCombatant & this.combatModule.Target == null) && !GlobalUtils.IsClose(base.transform.position, this.movementModule.SpawnPointLocation, 0.16f))
		{
			this.nonPlayerAIModule.SetDestination(this.movementModule.SpawnPointLocation);
			return;
		}
		if ((!this.IsCombatant & this.walkChance == 0f) && !GlobalUtils.IsClose(base.transform.position, this.movementModule.SpawnPointLocation, 0.16f))
		{
			this.nonPlayerAIModule.SetDestination(this.movementModule.SpawnPointLocation);
			return;
		}
		this.walkChance = ((this.walkChance > 1f) ? (this.walkChance / 100f) : this.walkChance);
		if (UnityEngine.Random.Range(0f, 1f) <= this.walkChance)
		{
			Vector3 destination = this.FindRandomDestination();
			this.nonPlayerAIModule.SetDestination(destination);
		}
	}

	// Token: 0x0600131E RID: 4894 RVA: 0x0005E15C File Offset: 0x0005C35C
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			collision.rigidbody.velocity = Vector2.zero;
			base.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		}
	}

	// Token: 0x0600131F RID: 4895 RVA: 0x0005E190 File Offset: 0x0005C390
	public override void OnStartClient()
	{
		if (this.IsCombatant)
		{
			base.gameObject.layer = LayerMask.NameToLayer("Combatant");
			base.gameObject.tag = "Combatant";
		}
	}

	// Token: 0x06001320 RID: 4896 RVA: 0x0005E1BF File Offset: 0x0005C3BF
	public override void OnStartServer()
	{
		if (this.questsCache.Count != 0)
		{
			this.quests.Clear();
			this.quests.AddRange(this.questsCache);
		}
	}

	// Token: 0x06001321 RID: 4897 RVA: 0x0005E1EA File Offset: 0x0005C3EA
	public override void OnStopServer()
	{
		if (this.quests.Count != 0)
		{
			this.questsCache = new List<Quest>(this.quests);
		}
	}

	// Token: 0x06001322 RID: 4898 RVA: 0x0005E20C File Offset: 0x0005C40C
	[Server]
	public void SetOwner(GameObject owner)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void NpcModule::SetOwner(UnityEngine.GameObject)' called when server was not active");
			return;
		}
		this.Owner = owner;
		if (this.Owner == null)
		{
			return;
		}
		CreatureModule creatureModule;
		this.Owner.TryGetComponent<CreatureModule>(out creatureModule);
		this.creatureModule.SetCreatureTitle(creatureModule.CreatureName);
		base.CancelInvoke("UpdateOwnerDestination");
		base.InvokeRepeating("UpdateOwnerDestination", 0f, 0.5f);
	}

	// Token: 0x06001323 RID: 4899 RVA: 0x0005E284 File Offset: 0x0005C484
	[Server]
	private void UpdateOwnerDestination()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void NpcModule::UpdateOwnerDestination()' called when server was not active");
			return;
		}
		if (this.Owner == null)
		{
			NetworkServer.Destroy(base.gameObject);
			return;
		}
		if (GlobalUtils.IsClose(this.Owner.transform.position, base.transform.position, 0.64f))
		{
			return;
		}
		this.nonPlayerAIModule.SetDestination(this.Owner.transform.position);
	}

	// Token: 0x06001324 RID: 4900 RVA: 0x0005E310 File Offset: 0x0005C510
	[Client]
	private void OnIsCombatant(bool oldValue, bool newValue)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void NpcModule::OnIsCombatant(System.Boolean,System.Boolean)' called when client was not active");
			return;
		}
		this.NetworkIsCombatant = newValue;
		if (newValue)
		{
			base.gameObject.layer = LayerMask.NameToLayer("Combatant");
			base.gameObject.tag = "Combatant";
		}
	}

	// Token: 0x06001325 RID: 4901 RVA: 0x0005E364 File Offset: 0x0005C564
	[Client]
	private void OnIsPet(bool oldValue, bool newValue)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void NpcModule::OnIsPet(System.Boolean,System.Boolean)' called when client was not active");
			return;
		}
		this.NetworkIsPet = newValue;
		if (newValue)
		{
			base.gameObject.layer = LayerMask.NameToLayer("Pet");
			base.gameObject.tag = (this.HasHandshakeDialog ? "Npc" : "Pet");
		}
	}

	// Token: 0x06001326 RID: 4902 RVA: 0x0005E3C4 File Offset: 0x0005C5C4
	[Server]
	public void LoadNpcData()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void NpcModule::LoadNpcData()' called when server was not active");
			return;
		}
		Npc npc = this.npcDatabaseModule.GetNpc(this.NpcId);
		if (!npc.IsDefined)
		{
			return;
		}
		this.actions = new List<NpcAction>
		{
			new NpcAction(99998, new NpcAction.NpcTask(this.ShowSoldItems)),
			new NpcAction(88888, new NpcAction.NpcTask(this.ShowQuests)),
			new NpcAction(99999, new NpcAction.NpcTask(this.CloseDialog))
		};
		this.script = npc.Script;
		if (!string.IsNullOrEmpty(this.script))
		{
			Type type = Type.GetType(this.script);
			base.gameObject.AddComponent(type);
		}
		this.creatureModule.SetCreatureName(npc.Name);
		this.creatureModule.SetOriginalCreatureName(npc.Name);
		this.creatureModule.SetCreatureTitle(npc.Title);
		this.creatureModule.SetGender(npc.Gender);
		this.nonPlayerAttributeModule.SetBaseLevel(npc.BaseLevel);
		this.nonPlayerAttributeModule.InitializeAttributes(new global::Attribute[]
		{
			new global::Attribute(npc.PowerLevel, AttributeType.Power),
			new global::Attribute(npc.AgilityLevel, AttributeType.Agility),
			new global::Attribute(npc.PrecisionLevel, AttributeType.Precision),
			new global::Attribute(npc.ToughnessLevel, AttributeType.Toughness),
			new global::Attribute(npc.VitalityLevel, AttributeType.Vitality)
		});
		this.nonPlayerEquipmentModule.EquipSkin(npc.SkinMetaName);
		this.nonPlayerAttributeModule.ResetHealth();
		this.creatureModule.SetAlive(true);
		this.walkChance = npc.WalkChance;
		this.walkInterval = npc.WalkInterval;
		this.walkRange = npc.WalkRange;
		this.movementModule.SetMovable(npc.CanMove);
		this.movementModule.SetDirection(Direction.South);
		this.skillConfigs = npc.SkillConfigs.ToList<SkillConfig>();
		if (this.skillConfigs.Count != 0)
		{
			this.skillTimer.Clear();
			foreach (SkillConfig skillConfig in this.skillConfigs)
			{
				this.skillTimer.Add(skillConfig.SkillId, Time.time);
			}
			base.InvokeRepeating("CastSkillController", 0f, this.skillConfigs.Min((SkillConfig sc) => sc.CastInterval));
		}
		this.quests.Clear();
		foreach (Quest quest in this.questDatabaseModule.GetQuestsByNpcId(npc.Id))
		{
			this.AddQuest(quest.Id);
		}
		this.NetworkIsCombatant = npc.IsCombatant;
		if (this.IsCombatant)
		{
			base.gameObject.layer = LayerMask.NameToLayer("Combatant");
			base.gameObject.tag = "Combatant";
		}
		this.InitializeCoroutines();
		this.IsLoaded = true;
	}

	// Token: 0x06001327 RID: 4903 RVA: 0x0005E70C File Offset: 0x0005C90C
	[Server]
	public void InitializeCoroutines()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void NpcModule::InitializeCoroutines()' called when server was not active");
			return;
		}
		base.InvokeRepeating("CheckDistance", 0f, (this.Owner != null) ? 1f : 5f);
		base.InvokeRepeating("FindTargets", 0f, 1f);
		base.InvokeRepeating("Regeneration", 0f, 3f);
	}

	// Token: 0x06001328 RID: 4904 RVA: 0x0005E782 File Offset: 0x0005C982
	[Server]
	public void SetNpcId(int npcId)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void NpcModule::SetNpcId(System.Int32)' called when server was not active");
			return;
		}
		this.NpcId = npcId;
	}

	// Token: 0x06001329 RID: 4905 RVA: 0x0005E7A0 File Offset: 0x0005C9A0
	[Server]
	private Vector3 FindRandomDestination()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'UnityEngine.Vector3 NpcModule::FindRandomDestination()' called when server was not active");
			return default(Vector3);
		}
		return GlobalUtils.RandomCircle(base.transform.position, 1f);
	}

	// Token: 0x0600132A RID: 4906 RVA: 0x0005E7E3 File Offset: 0x0005C9E3
	private void CheckDistance()
	{
		if (!base.enabled)
		{
			return;
		}
		this.CheckDistanceForAggroRange();
		this.CheckDistanceForWalkRange();
	}

	// Token: 0x0600132B RID: 4907 RVA: 0x0005E7FC File Offset: 0x0005C9FC
	private void CheckDistanceForWalkRange()
	{
		if (this.Owner != null)
		{
			return;
		}
		if (this.combatModule.Target != null)
		{
			return;
		}
		if (!GlobalUtils.IsClose(this.movementModule.SpawnPointLocation, base.transform.position, this.walkRange))
		{
			this.nonPlayerAIModule.SetDestination(this.movementModule.SpawnPointLocation);
		}
	}

	// Token: 0x0600132C RID: 4908 RVA: 0x0005E870 File Offset: 0x0005CA70
	private void CheckDistanceForAggroRange()
	{
		Vector3 vector = (this.Owner != null) ? this.Owner.transform.position : this.movementModule.SpawnPointLocation;
		if (!GlobalUtils.IsClose(vector, base.transform.position, 5f))
		{
			EffectConfig effectConfig = new EffectConfig
			{
				EffectName = "Puff",
				EffectScaleModifier = 0.5f,
				EffectSpeedModifier = 0.5f,
				Position = vector
			};
			this.effectModule.ShowEffects(effectConfig);
			this.movementModule.Teleport(vector, new Effect("TeleporterHit", 0.6f, 0.3f));
			this.movementModule.SetDirection(Direction.South);
			this.combatModule.RemoveTarget();
			this.nonPlayerAIModule.RemoveDestination();
		}
	}

	// Token: 0x0600132D RID: 4909 RVA: 0x0005E952 File Offset: 0x0005CB52
	[Server]
	public void SetHandshakeDialog(NpcDialog handshakeDialog)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void NpcModule::SetHandshakeDialog(NpcDialog)' called when server was not active");
			return;
		}
		this.handshakeDialog = handshakeDialog;
		this.NetworkHasHandshakeDialog = handshakeDialog.IsDefined;
	}

	// Token: 0x0600132E RID: 4910 RVA: 0x0005E980 File Offset: 0x0005CB80
	[Server]
	public void Handshake(GameObject player)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void NpcModule::Handshake(UnityEngine.GameObject)' called when server was not active");
			return;
		}
		if (!this.handshakeDialog.IsDefined)
		{
			return;
		}
		PlayerModule component = player.GetComponent<PlayerModule>();
		QuestModule component2 = player.GetComponent<QuestModule>();
		component.RenderNpcDialog(this.handshakeDialog);
		component2.UpdateQuest(this.NpcId, 1, ObjectiveType.TalkNpc, Rank.None);
	}

	// Token: 0x0600132F RID: 4911 RVA: 0x0005E9D8 File Offset: 0x0005CBD8
	[Server]
	public void Choose(GameObject player, int actionId)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void NpcModule::Choose(UnityEngine.GameObject,System.Int32)' called when server was not active");
			return;
		}
		NpcAction.NpcTask task = this.actions.FirstOrDefault((NpcAction f) => f.Id == actionId).Task;
		if (task == null)
		{
			return;
		}
		task(player);
	}

	// Token: 0x06001330 RID: 4912 RVA: 0x0005EA2E File Offset: 0x0005CC2E
	[Server]
	public void CloseDialog(GameObject player)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void NpcModule::CloseDialog(UnityEngine.GameObject)' called when server was not active");
			return;
		}
		PlayerModule component = player.GetComponent<PlayerModule>();
		component.TargetCloseDialogWindow(component.connectionToClient);
	}

	// Token: 0x06001331 RID: 4913 RVA: 0x0005EA56 File Offset: 0x0005CC56
	[Server]
	public void AddAction(NpcAction action)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void NpcModule::AddAction(NpcAction)' called when server was not active");
			return;
		}
		this.actions.Add(action);
	}

	// Token: 0x06001332 RID: 4914 RVA: 0x0005EA7C File Offset: 0x0005CC7C
	[Server]
	private void AddQuest(int questId)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void NpcModule::AddQuest(System.Int32)' called when server was not active");
			return;
		}
		Quest quest = this.questDatabaseModule.GetQuest(questId);
		if (quest.IsDefined)
		{
			this.quests.Add(quest);
			this.actions.Add(new NpcAction(10000 + quest.Id, delegate(GameObject player)
			{
				this.AskForQuest(player, quest);
			}));
			this.actions.Add(new NpcAction(20000 + quest.Id, delegate(GameObject player)
			{
				this.AcceptQuest(player, quest);
			}));
		}
	}

	// Token: 0x06001333 RID: 4915 RVA: 0x0005EB38 File Offset: 0x0005CD38
	[Server]
	public void AskForQuest(GameObject player, Quest quest)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void NpcModule::AskForQuest(UnityEngine.GameObject,Quest)' called when server was not active");
			return;
		}
		PlayerModule component = player.GetComponent<PlayerModule>();
		if (quest.IsDefined)
		{
			QuestModule component2 = player.GetComponent<QuestModule>();
			PlayerQuest playerQuest = component2.GetPlayerQuest(quest.Id);
			if (!component2.HasQuest(quest.Id))
			{
				NpcDialog npcDialog = new NpcDialog(quest.Name, quest, new NpcChoice[]
				{
					new NpcChoice(quest.PositiveChoice, 20000 + quest.Id),
					new NpcChoice(quest.NegativeChoice, 99999)
				});
				component.RenderNpcDialog(npcDialog);
				return;
			}
			if (playerQuest.IsCompleted)
			{
				NpcDialog npcDialog2 = new NpcDialog(quest.Name, quest.CompletedDialog, new NpcChoice[]
				{
					new NpcChoice(quest.CompletedChoice, 99999)
				});
				component.RenderNpcDialog(npcDialog2);
				return;
			}
			NpcDialog npcDialog3 = new NpcDialog(quest.Name, quest.InProgressDialog, new NpcChoice[]
			{
				new NpcChoice(quest.InProgressChoice, 99999)
			});
			component.RenderNpcDialog(npcDialog3);
		}
	}

	// Token: 0x06001334 RID: 4916 RVA: 0x0005EC60 File Offset: 0x0005CE60
	[Server]
	public void AcceptQuest(GameObject player, Quest quest)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void NpcModule::AcceptQuest(UnityEngine.GameObject,Quest)' called when server was not active");
			return;
		}
		QuestModule component = player.GetComponent<QuestModule>();
		EffectModule component2 = player.GetComponent<EffectModule>();
		if (quest.IsDefined & component != null)
		{
			component.AcceptQuest(quest.Id);
			component2.ShowScreenMessage("new_quest_message", 0, 3.5f, Array.Empty<string>());
			this.CloseDialog(player);
		}
	}

	// Token: 0x06001335 RID: 4917 RVA: 0x0005ECCC File Offset: 0x0005CECC
	[Server]
	public void ShowQuests(GameObject player)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void NpcModule::ShowQuests(UnityEngine.GameObject)' called when server was not active");
			return;
		}
		PlayerModule component = player.GetComponent<PlayerModule>();
		AttributeModule component2 = player.GetComponent<AttributeModule>();
		QuestModule component3 = player.GetComponent<QuestModule>();
		List<NpcChoice> list = new List<NpcChoice>();
		for (int i = 0; i < this.quests.Count; i++)
		{
			Quest quest = this.quests[i];
			if (component2.BaseLevel >= quest.RequiredLevel && !component3.GetPlayerQuest(quest.Id).IsDefined)
			{
				list.Add(new NpcChoice(quest.Name, 10000 + quest.Id));
			}
		}
		if (list.Any<NpcChoice>())
		{
			list.Add(new NpcChoice("default_npc_bye_choice", 99999));
			NpcDialog npcDialog = new NpcDialog("default_npc_quests_choice", "default_npc_quests_dialog", list.ToArray());
			component.RenderNpcDialog(npcDialog);
			return;
		}
		list.Add(new NpcChoice("default_npc_no_quest_available_choice", 99999));
		NpcDialog npcDialog2 = new NpcDialog("default_npc_quests_choice", "default_npc_no_quest_available_dialog", list.ToArray());
		component.RenderNpcDialog(npcDialog2);
	}

	// Token: 0x06001336 RID: 4918 RVA: 0x0005EDEC File Offset: 0x0005CFEC
	[Server]
	public void ShowSoldItems(GameObject player)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void NpcModule::ShowSoldItems(UnityEngine.GameObject)' called when server was not active");
			return;
		}
		PlayerModule playerModule;
		player.TryGetComponent<PlayerModule>(out playerModule);
		StoreModule storeModule;
		player.TryGetComponent<StoreModule>(out storeModule);
		if (storeModule.SoldItems.Count == 0)
		{
			NpcDialog npcDialog = new NpcDialog("default_npc_sold_items", "default_npc_empty_sold_items_greet", new NpcChoice[]
			{
				new NpcChoice("default_npc_bye_choice", 99999)
			});
			playerModule.RenderNpcDialog(npcDialog);
			return;
		}
		List<StoreItemConfig> list = new List<StoreItemConfig>();
		foreach (Item item in storeModule.SoldItems)
		{
			list.Add(new StoreItemConfig(this.NpcId, item));
		}
		NpcDialog npcDialog2 = new NpcDialog("default_npc_sold_items", "default_npc_sold_items_greet", list.ToArray(), new NpcChoice[]
		{
			new NpcChoice("default_npc_bye_choice", 99999)
		})
		{
			StoreAction = StoreAction.Repurchase
		};
		playerModule.RenderNpcDialog(npcDialog2);
	}

	// Token: 0x06001337 RID: 4919 RVA: 0x0005EF04 File Offset: 0x0005D104
	private void FindTargets()
	{
		if (!base.enabled)
		{
			return;
		}
		if (!this.IsCombatant)
		{
			return;
		}
		if (this.combatModule.Target != null && (this.combatModule.Target.GetComponent<ConditionModule>().IsInvisible | this.combatModule.Target.CompareTag("DeadMonster")))
		{
			this.combatModule.RemoveTarget();
		}
		if (this.combatModule.Target == null)
		{
			List<GameObject> targetsFromPosition = this.combatModule.TargetFinder.GetTargetsFromPosition(10, this.walkRange, true, true, true, false, this.movementModule.SpawnPointLocation, base.gameObject, this.combatModule.Target);
			if (targetsFromPosition.Count > 0)
			{
				this.combatModule.SetTarget(targetsFromPosition[0], false);
			}
		}
	}

	// Token: 0x06001338 RID: 4920 RVA: 0x0005EFD8 File Offset: 0x0005D1D8
	private void Regeneration()
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.nonPlayerAttributeModule.CurrentHealth < this.nonPlayerAttributeModule.MaxHealth)
		{
			if (this.combatModule.Target == null && !this.combatModule.InCombat)
			{
				float num = (float)this.nonPlayerAttributeModule.MaxHealth * 0.25f;
				this.nonPlayerAttributeModule.AddHealth((int)num);
				return;
			}
			this.nonPlayerAttributeModule.AddHealth(this.nonPlayerAttributeModule.RegenerationAmount);
		}
	}

	// Token: 0x06001339 RID: 4921 RVA: 0x0005F060 File Offset: 0x0005D260
	private void CastSkillController()
	{
		if (!base.enabled)
		{
			return;
		}
		if (!this.creatureModule.IsAlive)
		{
			return;
		}
		if (this.combatModule.Target == null)
		{
			return;
		}
		foreach (SkillConfig skillConfig in this.skillConfigs)
		{
			if (Time.time - this.skillTimer[skillConfig.SkillId] >= skillConfig.CastInterval)
			{
				float num = (skillConfig.CastChance > 1f) ? (skillConfig.CastChance / 100f) : skillConfig.CastChance;
				if (UnityEngine.Random.Range(0f, 1f) <= num)
				{
					this.skillTimer[skillConfig.SkillId] = Time.time;
					Skill skill = this.nonPlayerSkillModule.BuildSkill(skillConfig.SkillId, skillConfig.SkillPower);
					this.nonPlayerSkillModule.Cast(skill);
				}
			}
		}
	}

	// Token: 0x0600133A RID: 4922 RVA: 0x0005F16C File Offset: 0x0005D36C
	private bool ValidateSkillTimer(int skillId, float castInterval)
	{
		return !this.skillTimer.ContainsKey(skillId) || Time.time - this.skillTimer[skillId] >= castInterval;
	}

	// Token: 0x0600133B RID: 4923 RVA: 0x0005F196 File Offset: 0x0005D396
	private void UpdateSkillTimer(int skillId)
	{
		if (this.skillTimer.ContainsKey(skillId))
		{
			this.skillTimer[skillId] = Time.time;
			return;
		}
		this.skillTimer.Add(skillId, Time.time);
	}

	// Token: 0x0600133C RID: 4924 RVA: 0x0005F1CC File Offset: 0x0005D3CC
	[Server]
	private IEnumerator Respawn()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Collections.IEnumerator NpcModule::Respawn()' called when server was not active");
			return null;
		}
		NpcModule.<Respawn>d__81 <Respawn>d__ = new NpcModule.<Respawn>d__81(0);
		<Respawn>d__.<>4__this = this;
		return <Respawn>d__;
	}

	// Token: 0x0600133D RID: 4925 RVA: 0x0005F208 File Offset: 0x0005D408
	[Server]
	private void HandleOnKilled(GameObject killer, List<Attacker> attackers)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void NpcModule::HandleOnKilled(UnityEngine.GameObject,System.Collections.Generic.List`1<Attacker>)' called when server was not active");
			return;
		}
		this.animationControllerModule.RunAnimation(AnimationType.Death);
		this.nonPlayerAIModule.RemoveDestination();
		this.combatModule.Attackers.Clear();
		foreach (Attacker attacker in this.combatModule.Attackers)
		{
			if (attacker.AttackerObject != null && !attacker.AttackerObject.activeInHierarchy)
			{
				CombatModule component = attacker.AttackerObject.GetComponent<CombatModule>();
				if (component != null)
				{
					component.RemoveTarget();
				}
			}
		}
		base.StartCoroutine(this.Respawn());
	}

	// Token: 0x0600133E RID: 4926 RVA: 0x0005F2D8 File Offset: 0x0005D4D8
	[Server]
	private void HandleOnReceiveDamage(GameObject attacker, int damage)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void NpcModule::HandleOnReceiveDamage(UnityEngine.GameObject,System.Int32)' called when server was not active");
			return;
		}
		if (attacker == null)
		{
			return;
		}
		if (this.combatModule.Target == null)
		{
			this.combatModule.SetTarget(attacker, false);
		}
	}

	// Token: 0x0600133F RID: 4927 RVA: 0x0005F324 File Offset: 0x0005D524
	public NpcModule()
	{
		base.InitSyncObject(this.quests);
	}

	// Token: 0x06001340 RID: 4928 RVA: 0x0002FEC9 File Offset: 0x0002E0C9
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x17000210 RID: 528
	// (get) Token: 0x06001341 RID: 4929 RVA: 0x0005F364 File Offset: 0x0005D564
	// (set) Token: 0x06001342 RID: 4930 RVA: 0x0005F377 File Offset: 0x0005D577
	public bool NetworkHasHandshakeDialog
	{
		get
		{
			return this.HasHandshakeDialog;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<bool>(value, ref this.HasHandshakeDialog, 1UL, null);
		}
	}

	// Token: 0x17000211 RID: 529
	// (get) Token: 0x06001343 RID: 4931 RVA: 0x0005F394 File Offset: 0x0005D594
	// (set) Token: 0x06001344 RID: 4932 RVA: 0x0005F3A7 File Offset: 0x0005D5A7
	public bool NetworkIsPet
	{
		get
		{
			return this.IsPet;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<bool>(value, ref this.IsPet, 2UL, new Action<bool, bool>(this.OnIsPet));
		}
	}

	// Token: 0x17000212 RID: 530
	// (get) Token: 0x06001345 RID: 4933 RVA: 0x0005F3CC File Offset: 0x0005D5CC
	// (set) Token: 0x06001346 RID: 4934 RVA: 0x0005F3DF File Offset: 0x0005D5DF
	public bool NetworkIsCombatant
	{
		get
		{
			return this.IsCombatant;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<bool>(value, ref this.IsCombatant, 4UL, new Action<bool, bool>(this.OnIsCombatant));
		}
	}

	// Token: 0x06001347 RID: 4935 RVA: 0x0005F404 File Offset: 0x0005D604
	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteBool(this.HasHandshakeDialog);
			writer.WriteBool(this.IsPet);
			writer.WriteBool(this.IsCombatant);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1UL) != 0UL)
		{
			writer.WriteBool(this.HasHandshakeDialog);
		}
		if ((base.syncVarDirtyBits & 2UL) != 0UL)
		{
			writer.WriteBool(this.IsPet);
		}
		if ((base.syncVarDirtyBits & 4UL) != 0UL)
		{
			writer.WriteBool(this.IsCombatant);
		}
	}

	// Token: 0x06001348 RID: 4936 RVA: 0x0005F4B8 File Offset: 0x0005D6B8
	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			base.GeneratedSyncVarDeserialize<bool>(ref this.HasHandshakeDialog, null, reader.ReadBool());
			base.GeneratedSyncVarDeserialize<bool>(ref this.IsPet, new Action<bool, bool>(this.OnIsPet), reader.ReadBool());
			base.GeneratedSyncVarDeserialize<bool>(ref this.IsCombatant, new Action<bool, bool>(this.OnIsCombatant), reader.ReadBool());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 1L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<bool>(ref this.HasHandshakeDialog, null, reader.ReadBool());
		}
		if ((num & 2L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<bool>(ref this.IsPet, new Action<bool, bool>(this.OnIsPet), reader.ReadBool());
		}
		if ((num & 4L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<bool>(ref this.IsCombatant, new Action<bool, bool>(this.OnIsCombatant), reader.ReadBool());
		}
	}

	// Token: 0x040011BF RID: 4543
	public int NpcId;

	// Token: 0x040011C0 RID: 4544
	[SyncVar]
	public bool HasHandshakeDialog;

	// Token: 0x040011C1 RID: 4545
	[SyncVar(hook = "OnIsPet")]
	public bool IsPet;

	// Token: 0x040011C2 RID: 4546
	[SyncVar(hook = "OnIsCombatant")]
	public bool IsCombatant;

	// Token: 0x040011C3 RID: 4547
	private readonly SyncListQuest quests = new SyncListQuest();

	// Token: 0x040011C4 RID: 4548
	private List<Quest> questsCache = new List<Quest>();

	// Token: 0x040011C5 RID: 4549
	private List<NpcAction> actions;

	// Token: 0x040011C6 RID: 4550
	private NpcDialog handshakeDialog;

	// Token: 0x040011C7 RID: 4551
	private List<SkillConfig> skillConfigs;

	// Token: 0x040011C8 RID: 4552
	private Dictionary<int, float> skillTimer = new Dictionary<int, float>();

	// Token: 0x040011C9 RID: 4553
	public const int AskForQuestActionId = 10000;

	// Token: 0x040011CA RID: 4554
	public const int AcceptQuestActionId = 20000;

	// Token: 0x040011CB RID: 4555
	public const int ShowQuestsActionId = 88888;

	// Token: 0x040011CC RID: 4556
	public const int ShowSoldItemsActionId = 99998;

	// Token: 0x040011CD RID: 4557
	public const int CloseDialogActionId = 99999;

	// Token: 0x040011CE RID: 4558
	[SerializeField]
	private GameObject localHud;

	// Token: 0x040011CF RID: 4559
	[SerializeField]
	private GameObject questIcon;

	// Token: 0x040011D0 RID: 4560
	[SerializeField]
	private Image healthBar;

	// Token: 0x040011D1 RID: 4561
	[SerializeField]
	private TextMeshPro nameText;

	// Token: 0x040011D2 RID: 4562
	[SerializeField]
	private TextMeshPro titleText;

	// Token: 0x040011D3 RID: 4563
	private const float AggroRange = 5f;

	// Token: 0x040011D4 RID: 4564
	public GameObject Owner;

	// Token: 0x040011D5 RID: 4565
	private string script;

	// Token: 0x040011D6 RID: 4566
	private float walkRange;

	// Token: 0x040011D7 RID: 4567
	private float walkInterval;

	// Token: 0x040011D8 RID: 4568
	private float walkChance;

	// Token: 0x040011D9 RID: 4569
	private float updateInterval = 0.5f;

	// Token: 0x040011DA RID: 4570
	private float updateTime;

	// Token: 0x040011DB RID: 4571
	private CombatModule combatModule;

	// Token: 0x040011DC RID: 4572
	private EffectModule effectModule;

	// Token: 0x040011DD RID: 4573
	private CreatureModule creatureModule;

	// Token: 0x040011DE RID: 4574
	private UISystemModule uiSystemModule;

	// Token: 0x040011DF RID: 4575
	private MovementModule movementModule;

	// Token: 0x040011E0 RID: 4576
	private NonPlayerAIModule nonPlayerAIModule;

	// Token: 0x040011E1 RID: 4577
	private NpcDatabaseModule npcDatabaseModule;

	// Token: 0x040011E2 RID: 4578
	private QuestDatabaseModule questDatabaseModule;

	// Token: 0x040011E3 RID: 4579
	private SkillDatabaseModule skillDatabaseModule;

	// Token: 0x040011E4 RID: 4580
	private NonPlayerSkillModule nonPlayerSkillModule;

	// Token: 0x040011E5 RID: 4581
	private NonPlayerAttributeModule nonPlayerAttributeModule;

	// Token: 0x040011E6 RID: 4582
	private NonPlayerEquipmentModule nonPlayerEquipmentModule;

	// Token: 0x040011E7 RID: 4583
	private AnimationControllerModule animationControllerModule;
}
