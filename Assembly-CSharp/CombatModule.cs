using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

// Token: 0x020002E7 RID: 743
public class CombatModule : NetworkBehaviour
{
	// Token: 0x17000176 RID: 374
	// (get) Token: 0x06000D2E RID: 3374 RVA: 0x0003E1D5 File Offset: 0x0003C3D5
	// (set) Token: 0x06000D2F RID: 3375 RVA: 0x0003E1DD File Offset: 0x0003C3DD
	public float LastCombatTime { get; private set; }

	// Token: 0x17000177 RID: 375
	// (get) Token: 0x06000D30 RID: 3376 RVA: 0x0003E1E6 File Offset: 0x0003C3E6
	// (set) Token: 0x06000D31 RID: 3377 RVA: 0x0003E1EE File Offset: 0x0003C3EE
	public TargetFinder TargetFinder { get; private set; }

	// Token: 0x17000178 RID: 376
	// (get) Token: 0x06000D32 RID: 3378 RVA: 0x0003E1F7 File Offset: 0x0003C3F7
	// (set) Token: 0x06000D33 RID: 3379 RVA: 0x0003E200 File Offset: 0x0003C400
	public GameObject Target
	{
		get
		{
			return this._target;
		}
		set
		{
			try
			{
				this.HasTarget = (value != null);
				if (NetworkClient.active)
				{
					this._target = value;
				}
				else if (this._target != null && value == null && this.OnLostTarget != null)
				{
					this.OnLostTarget(this._target);
				}
				else if (this._target != value && value != null && this.OnGotTarget != null)
				{
					this.OnGotTarget(value);
				}
			}
			finally
			{
				this._target = value;
			}
		}
	}

	// Token: 0x14000007 RID: 7
	// (add) Token: 0x06000D34 RID: 3380 RVA: 0x0003E2A4 File Offset: 0x0003C4A4
	// (remove) Token: 0x06000D35 RID: 3381 RVA: 0x0003E2DC File Offset: 0x0003C4DC
	public event CombatModule.OnGotTargetEventHandler OnGotTarget;

	// Token: 0x14000008 RID: 8
	// (add) Token: 0x06000D36 RID: 3382 RVA: 0x0003E314 File Offset: 0x0003C514
	// (remove) Token: 0x06000D37 RID: 3383 RVA: 0x0003E34C File Offset: 0x0003C54C
	public event CombatModule.OnLostTargetEventHandler OnLostTarget;

	// Token: 0x14000009 RID: 9
	// (add) Token: 0x06000D38 RID: 3384 RVA: 0x0003E384 File Offset: 0x0003C584
	// (remove) Token: 0x06000D39 RID: 3385 RVA: 0x0003E3BC File Offset: 0x0003C5BC
	public event CombatModule.OnReceiveDamageEventHandler OnReceiveDamage;

	// Token: 0x06000D3A RID: 3386 RVA: 0x0003E3F4 File Offset: 0x0003C5F4
	private void Awake()
	{
		if (NetworkClient.active)
		{
			GameObject.FindGameObjectWithTag("UISystem").TryGetComponent<UISystemModule>(out this.uiSystemModule);
		}
		if (!base.CompareTag("Player"))
		{
			this.Attackers = new List<Attacker>();
		}
		base.TryGetComponent<PvpModule>(out this.pvpModule);
		base.TryGetComponent<AreaModule>(out this.areaModule);
		base.TryGetComponent<EffectModule>(out this.effectModule);
		base.TryGetComponent<MonsterModule>(out this.monsterModule);
		base.TryGetComponent<CreatureModule>(out this.creatureModule);
		base.TryGetComponent<MovementModule>(out this.movementModule);
		base.TryGetComponent<VocationModule>(out this.vocationModule);
		base.TryGetComponent<AttributeModule>(out this.attributeModule);
		base.TryGetComponent<EquipmentModule>(out this.equipmentModule);
		base.TryGetComponent<ConditionModule>(out this.conditionModule);
		base.TryGetComponent<NonPlayerAttributeModule>(out this.nonPlayerAttributeModule);
		this.TargetFinder = new TargetFinder(this);
	}

	// Token: 0x06000D3B RID: 3387 RVA: 0x0003E4D4 File Offset: 0x0003C6D4
	[ServerCallback]
	private void Start()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		if (NetworkServer.active)
		{
			if (!base.CompareTag("Player"))
			{
				base.InvokeRepeating("RemoveDeadAttackersTimer", 0f, 5f);
			}
			base.InvokeRepeating("RemoveInvalidTargetsTimer", 0f, 5f);
			base.InvokeRepeating("FightingFlagsUpdate", 0f, 2.5f);
		}
	}

	// Token: 0x06000D3C RID: 3388 RVA: 0x0003E540 File Offset: 0x0003C740
	[ServerCallback]
	private void FightingFlagsUpdate()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		float num = Time.time - this.LastCombatTime;
		if (this.InCombat && num > 15f)
		{
			this.NetworkInCombat = false;
		}
		if (this.IsFighting && num > 5f)
		{
			this.NetworkIsFighting = false;
		}
	}

	// Token: 0x06000D3D RID: 3389 RVA: 0x0003E593 File Offset: 0x0003C793
	public void AddNearbyTarget(GameObject target)
	{
		this.RemoveInvalidNearbyTargets(target);
		this.nearbyTargets.Add(target);
		this.SortNearbyTargets();
	}

	// Token: 0x06000D3E RID: 3390 RVA: 0x0003E5AE File Offset: 0x0003C7AE
	public void RemoveNearbyTarget(GameObject target)
	{
		this.RemoveInvalidNearbyTargets(target);
		this.SortNearbyTargets();
	}

	// Token: 0x06000D3F RID: 3391 RVA: 0x0003E5C0 File Offset: 0x0003C7C0
	private void SortNearbyTargets()
	{
		if (this.nearbyTargets.Count <= 1)
		{
			return;
		}
		Vector3 currentPosition = base.transform.position;
		this.nearbyTargets.Sort((GameObject a, GameObject b) => (currentPosition - a.transform.position).sqrMagnitude.CompareTo((currentPosition - b.transform.position).sqrMagnitude));
	}

	// Token: 0x06000D40 RID: 3392 RVA: 0x0003E60C File Offset: 0x0003C80C
	private void RemoveInvalidNearbyTargets(GameObject target)
	{
		for (int i = this.nearbyTargets.Count - 1; i >= 0; i--)
		{
			if (this.nearbyTargets[i] == null || !this.nearbyTargets[i].activeInHierarchy)
			{
				this.nearbyTargets.RemoveAt(i);
			}
			else if (this.nearbyTargets[i] == target)
			{
				this.nearbyTargets.RemoveAt(i);
			}
		}
	}

	// Token: 0x06000D41 RID: 3393 RVA: 0x0003E681 File Offset: 0x0003C881
	public List<GameObject> GetNearbyTagets()
	{
		return this.nearbyTargets;
	}

	// Token: 0x06000D42 RID: 3394 RVA: 0x0003E689 File Offset: 0x0003C889
	private void OnIsFighting(bool oldValue, bool newValue)
	{
		this.NetworkIsFighting = newValue;
		if (base.isLocalPlayer)
		{
			if (this.IsFighting)
			{
				this.LastCombatTime = Time.time;
				this.uiSystemModule.ShowFightingIcon();
				return;
			}
			this.uiSystemModule.HideFightingIcon();
		}
	}

	// Token: 0x06000D43 RID: 3395 RVA: 0x0003E6C4 File Offset: 0x0003C8C4
	private void OnInCombat(bool oldValue, bool newValue)
	{
		this.NetworkInCombat = newValue;
		if (base.isLocalPlayer)
		{
			if (this.InCombat)
			{
				this.LastCombatTime = Time.time;
				this.uiSystemModule.ShowCombatIcon();
				return;
			}
			this.uiSystemModule.HideCombatIcon();
		}
	}

	// Token: 0x06000D44 RID: 3396 RVA: 0x0003E700 File Offset: 0x0003C900
	[ServerCallback]
	private void RemoveDeadAttackersTimer()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		for (int i = this.Attackers.Count - 1; i >= 0; i--)
		{
			try
			{
				if (this.Attackers[i].AttackerObject == null)
				{
					this.Attackers.RemoveAt(i);
				}
				else
				{
					CreatureModule creatureModule;
					this.Attackers[i].AttackerObject.TryGetComponent<CreatureModule>(out creatureModule);
					if (!creatureModule.IsAlive)
					{
						this.Attackers.RemoveAt(i);
					}
				}
			}
			catch (Exception arg)
			{
				Debug.LogError(string.Format("{0} - {1}", this.creatureModule.CreatureName, arg));
			}
		}
	}

	// Token: 0x06000D45 RID: 3397 RVA: 0x0003E7B8 File Offset: 0x0003C9B8
	[ServerCallback]
	private void RemoveInvalidTargetsTimer()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		if (this.Target == null & this.HasTarget)
		{
			this.HasTarget = false;
		}
		float num = 10f;
		for (int i = this.nearbyTargets.Count - 1; i >= 0; i--)
		{
			if (this.nearbyTargets[i] == null || !this.nearbyTargets[i].activeInHierarchy)
			{
				this.nearbyTargets.RemoveAt(i);
			}
			else if (Vector2.Distance(base.transform.position, this.nearbyTargets[i].transform.position) > num)
			{
				this.nearbyTargets.RemoveAt(i);
			}
		}
		if (this.Target == null)
		{
			this.Target = null;
			return;
		}
		if (!base.CompareTag("Player") && !TargetFinder.HasValidWalkablePath(base.gameObject, this.Target))
		{
			this.RemoveTarget();
			return;
		}
		CreatureModule creatureModule;
		this.Target.TryGetComponent<CreatureModule>(out creatureModule);
		if (!creatureModule.IsAlive)
		{
			if (base.CompareTag("Player") && creatureModule.CompareTag("Player"))
			{
				return;
			}
			this.RemoveTarget();
			return;
		}
		else
		{
			if (Vector2.Distance(base.transform.position, this.Target.transform.position) <= num)
			{
				return;
			}
			this.RemoveTarget();
			return;
		}
	}

	// Token: 0x06000D46 RID: 3398 RVA: 0x0003E928 File Offset: 0x0003CB28
	[Client]
	public void ChangeTarget(float maxRange)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void CombatModule::ChangeTarget(System.Single)' called when client was not active");
			return;
		}
		List<GameObject> orderedTargets = this.TargetFinder.GetOrderedTargets(10, maxRange, true, true, true, false, base.gameObject, this.Target);
		this.CmdRemoveTarget();
		if (orderedTargets.Count == 0)
		{
			this.targetIndex = 0;
			return;
		}
		if (this.targetIndex >= orderedTargets.Count)
		{
			this.targetIndex = 0;
		}
		GameObject x = orderedTargets[this.targetIndex];
		if (x != null && x == this.Target)
		{
			this.targetIndex++;
		}
		this.CmdSetTarget(orderedTargets[this.targetIndex], true);
		this.targetIndex++;
	}

	// Token: 0x06000D47 RID: 3399 RVA: 0x0003E9E8 File Offset: 0x0003CBE8
	[Server]
	public void RemoveTarget()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void CombatModule::RemoveTarget()' called when server was not active");
			return;
		}
		this.TargetLocked = false;
		if (this.Target != null)
		{
			this.Target = null;
			if (base.CompareTag("Player"))
			{
				this.TargetRemoveTarget(base.connectionToClient);
			}
		}
	}

	// Token: 0x06000D48 RID: 3400 RVA: 0x0003EA40 File Offset: 0x0003CC40
	[Command]
	public void CmdRemoveTarget()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		base.SendCommandInternal("System.Void CombatModule::CmdRemoveTarget()", 519941676, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000D49 RID: 3401 RVA: 0x0003EA70 File Offset: 0x0003CC70
	[TargetRpc]
	public void TargetRemoveTarget(NetworkConnection target)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		this.SendTargetRPCInternal(target, "System.Void CombatModule::TargetRemoveTarget(Mirror.NetworkConnection)", 859855982, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000D4A RID: 3402 RVA: 0x0003EAA0 File Offset: 0x0003CCA0
	[Server]
	public bool CanSetTarget()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean CombatModule::CanSetTarget()' called when server was not active");
			return default(bool);
		}
		return !this.conditionModule.HasActiveCondition(ConditionCategory.Confusion) & !this.conditionModule.HasActiveCondition(ConditionCategory.Paralyzing);
	}

	// Token: 0x06000D4B RID: 3403 RVA: 0x0003EAF0 File Offset: 0x0003CCF0
	[Server]
	public void SetTarget(GameObject target, bool lockTarget)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void CombatModule::SetTarget(UnityEngine.GameObject,System.Boolean)' called when server was not active");
			return;
		}
		if (!this.CanSetTarget())
		{
			return;
		}
		bool flag = false;
		if (target != null)
		{
			ConditionModule component = target.GetComponent<ConditionModule>();
			flag = (component != null && component.HasActiveCondition(ConditionCategory.Invisibility));
		}
		if (!flag)
		{
			this.Target = target;
			this.TargetLocked = lockTarget;
			if (base.CompareTag("Player"))
			{
				if (!base.isServer)
				{
					return;
				}
				this.TargetSetTarget(base.connectionToClient, target, lockTarget);
			}
		}
	}

	// Token: 0x06000D4C RID: 3404 RVA: 0x0003EB78 File Offset: 0x0003CD78
	[Command]
	public void CmdSetTarget(GameObject target, bool lockTarget)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteGameObject(target);
		writer.WriteBool(lockTarget);
		base.SendCommandInternal("System.Void CombatModule::CmdSetTarget(UnityEngine.GameObject,System.Boolean)", 862762599, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000D4D RID: 3405 RVA: 0x0003EBBC File Offset: 0x0003CDBC
	[TargetRpc]
	private void TargetSetTarget(NetworkConnection target, GameObject targetObject, bool lockTarget)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteGameObject(targetObject);
		writer.WriteBool(lockTarget);
		this.SendTargetRPCInternal(target, "System.Void CombatModule::TargetSetTarget(Mirror.NetworkConnection,UnityEngine.GameObject,System.Boolean)", -2034350335, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000D4E RID: 3406 RVA: 0x0003EC00 File Offset: 0x0003CE00
	[Server]
	public void ApplyDamage(int damage, EffectConfig damageEffectConfig, GameObject attackerObject, bool isConditionDamage, params ConditionConfig[] conditionConfigs)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void CombatModule::ApplyDamage(System.Int32,EffectConfig,UnityEngine.GameObject,System.Boolean,ConditionConfig[])' called when server was not active");
			return;
		}
		if (this.IsPvpProtected(attackerObject))
		{
			return;
		}
		if (!this.creatureModule.IsAlive)
		{
			return;
		}
		if (this.Evaded())
		{
			return;
		}
		if (!isConditionDamage)
		{
			damage = this.CalculatePvpDamage(damage, attackerObject, ref damageEffectConfig);
		}
		damage = this.CalculateBossDamage(damage, ref damageEffectConfig);
		CombatModule.OnReceiveDamageEventHandler onReceiveDamage = this.OnReceiveDamage;
		if (onReceiveDamage != null)
		{
			onReceiveDamage(attackerObject, damage);
		}
		int damageDealt = Mathf.Abs(damage);
		bool flag = this.AddDamageToHealthAndReturnIfShouldKill(attackerObject, ref damageDealt);
		this.StartConditionsIfAlive(damage, attackerObject, conditionConfigs, flag);
		if (damage == 0 || this.conditionModule.HasActiveCondition(ConditionCategory.Invincibility))
		{
			this.ShowDamageEffects(false, damageEffectConfig);
			return;
		}
		this.ShowDamageEffects(flag, damageEffectConfig);
		this.AddDamageToAttackers(attackerObject, damageDealt);
		if (flag)
		{
			this.creatureModule.Kill(attackerObject, this.Attackers);
		}
	}

	// Token: 0x06000D4F RID: 3407 RVA: 0x0003ECCC File Offset: 0x0003CECC
	private void StartConditionsIfAlive(int damage, GameObject attackerObject, ConditionConfig[] conditionConfigs, bool shouldKill)
	{
		if (shouldKill)
		{
			return;
		}
		for (int i = 0; i < conditionConfigs.Length; i++)
		{
			Condition condition = conditionConfigs[i].Condition;
			if (condition.IsDefined && !(damage == 0 & condition.Category != ConditionCategory.Taunt))
			{
				if (condition.CanCauseDamage & condition.Power <= 1f)
				{
					condition.Power *= (float)damage;
				}
				this.StartConditionIfNeeded(conditionConfigs[i].Chance, condition, attackerObject);
			}
		}
	}

	// Token: 0x06000D50 RID: 3408 RVA: 0x0003ED51 File Offset: 0x0003CF51
	private void ShowDamageEffects(bool shouldKill, EffectConfig damageEffectConfig)
	{
		if (shouldKill)
		{
			damageEffectConfig.Position = base.transform.position;
		}
		this.effectModule.ShowEffects(damageEffectConfig);
	}

	// Token: 0x06000D51 RID: 3409 RVA: 0x0003ED74 File Offset: 0x0003CF74
	private void StartConditionIfNeeded(float conditionChance, Condition condition, GameObject attacker)
	{
		if (condition.IsDefined && UnityEngine.Random.Range(0f, 1f) <= conditionChance)
		{
			this.conditionModule.StartCondition(condition, attacker, true);
		}
	}

	// Token: 0x06000D52 RID: 3410 RVA: 0x0003EDA0 File Offset: 0x0003CFA0
	private bool AddDamageToHealthAndReturnIfShouldKill(GameObject attacker, ref int damageDealt)
	{
		bool result = false;
		if (damageDealt == 0)
		{
			return result;
		}
		if (this.conditionModule.HasActiveCondition(ConditionCategory.Invincibility))
		{
			return result;
		}
		if (this.attributeModule)
		{
			damageDealt = Mathf.Min(damageDealt, this.attributeModule.CurrentHealth);
			this.attributeModule.AddHealth(attacker, -damageDealt, true, default(EffectConfig));
			result = (this.attributeModule.CurrentHealth < 1);
		}
		else if (this.nonPlayerAttributeModule != null)
		{
			damageDealt = Mathf.Min(damageDealt, this.nonPlayerAttributeModule.CurrentHealth);
			this.nonPlayerAttributeModule.AddHealth(-damageDealt);
			result = (this.nonPlayerAttributeModule.CurrentHealth < 1);
		}
		return result;
	}

	// Token: 0x06000D53 RID: 3411 RVA: 0x0003EE50 File Offset: 0x0003D050
	private void InvokeCombatEvents(int damage, GameObject attackerObject)
	{
		CombatModule.OnReceiveDamageEventHandler onReceiveDamage = this.OnReceiveDamage;
		if (onReceiveDamage == null)
		{
			return;
		}
		onReceiveDamage(attackerObject, damage);
	}

	// Token: 0x06000D54 RID: 3412 RVA: 0x0003EE64 File Offset: 0x0003D064
	private void AddDamageToAttackers(GameObject attackerObject, int damageDealt)
	{
		if (attackerObject == null)
		{
			return;
		}
		if (!attackerObject.CompareTag("Player") && base.CompareTag("Player"))
		{
			return;
		}
		if (attackerObject == base.gameObject)
		{
			return;
		}
		Attacker attacker = this.Attackers.FirstOrDefault((Attacker a) => a.AttackerObject == attackerObject);
		if (attacker != null)
		{
			attacker.DamageDealt += damageDealt;
			return;
		}
		this.Attackers.Add(new Attacker(attackerObject, damageDealt));
	}

	// Token: 0x06000D55 RID: 3413 RVA: 0x0003EF04 File Offset: 0x0003D104
	public int CalculatePvpDamage(int incomingDamage, GameObject attackerObject, ref EffectConfig damageEffectConfig)
	{
		if (attackerObject == null)
		{
			return incomingDamage;
		}
		if (!base.CompareTag("Player") | !attackerObject.CompareTag("Player"))
		{
			return incomingDamage;
		}
		if (this.pvpModule.TvtTeam != TvtTeam.None)
		{
			return incomingDamage;
		}
		int result = Mathf.RoundToInt((float)incomingDamage * 0.05f);
		if (incomingDamage != 0)
		{
			damageEffectConfig.Text = result.ToString();
		}
		return result;
	}

	// Token: 0x06000D56 RID: 3414 RVA: 0x0003EF6C File Offset: 0x0003D16C
	public int CalculateBossDamage(int incomingDamage, ref EffectConfig damageEffectConfig)
	{
		if (this.monsterModule == null || !this.monsterModule.IsBoss)
		{
			return incomingDamage;
		}
		int b = Mathf.RoundToInt((float)this.nonPlayerAttributeModule.MaxHealth * UnityEngine.Random.Range(0.00125f, 0.00325f));
		int result = Mathf.Min(incomingDamage, b);
		if (incomingDamage != 0)
		{
			damageEffectConfig.Text = result.ToString();
		}
		return result;
	}

	// Token: 0x06000D57 RID: 3415 RVA: 0x0003EFD4 File Offset: 0x0003D1D4
	public bool Evaded()
	{
		if (!this.movementModule.IsDashing)
		{
			return false;
		}
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "Puff",
			EffectScaleModifier = 0.35f,
			EffectSpeedModifier = 0.75f,
			Text = "damage_evaded",
			TextColorId = 3
		};
		this.effectModule.ShowEffects(effectConfig);
		return true;
	}

	// Token: 0x06000D58 RID: 3416 RVA: 0x0003F040 File Offset: 0x0003D240
	public bool IsPvpProtected(GameObject attackerObject)
	{
		if (attackerObject == null)
		{
			return false;
		}
		if (attackerObject == base.gameObject)
		{
			return false;
		}
		if (!base.CompareTag("Player"))
		{
			return false;
		}
		if (!attackerObject.CompareTag("Player"))
		{
			return false;
		}
		if (this.areaModule != null && this.areaModule.AreaType == AreaType.EventArea)
		{
			return false;
		}
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "Puff",
			EffectScaleModifier = 0.35f,
			EffectSpeedModifier = 0.75f,
			Text = "damage_protected",
			TextColorId = 3
		};
		PvpModule pvpModule;
		attackerObject.TryGetComponent<PvpModule>(out pvpModule);
		if (this.pvpModule.PvpStatus == PvpStatus.Neutral && pvpModule.PvpStatus == PvpStatus.Outlaw)
		{
			this.effectModule.ShowEffects(effectConfig);
			return true;
		}
		if (this.areaModule != null)
		{
			if (this.areaModule.AreaType != AreaType.EventArea && ServerSettingsManager.ServerType == ServerType.PvE)
			{
				this.effectModule.ShowEffects(effectConfig);
				return true;
			}
			if (this.areaModule.AreaType == AreaType.ProtectedArea)
			{
				this.effectModule.ShowEffects(effectConfig);
				return true;
			}
			if (this.areaModule.AreaType == AreaType.PveArea && this.pvpModule.PvpStatus == PvpStatus.Neutral)
			{
				this.effectModule.ShowEffects(effectConfig);
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000D59 RID: 3417 RVA: 0x0003F18A File Offset: 0x0003D38A
	[Server]
	public void StartCombatAndFightingFlags()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void CombatModule::StartCombatAndFightingFlags()' called when server was not active");
			return;
		}
		this.LastCombatTime = Time.time;
		this.NetworkInCombat = true;
		this.NetworkIsFighting = true;
	}

	// Token: 0x06000D5A RID: 3418 RVA: 0x0003F1BC File Offset: 0x0003D3BC
	private void ActivateArcaneProtection()
	{
		float arcaneProtectionChance = this.GetArcaneProtectionChance();
		if (UnityEngine.Random.Range(0f, 1f) >= arcaneProtectionChance)
		{
			return;
		}
		this.conditionModule.StartCondition(new Condition(ConditionCategory.Blessing, ConditionType.Toughness, 2f, 0.5f, 1f, new Effect("IceShield", 0.5f, 0.25f), 0, 0f, ""), base.gameObject, true);
	}

	// Token: 0x06000D5B RID: 3419 RVA: 0x0003F22C File Offset: 0x0003D42C
	private bool BlockAttack()
	{
		float shieldBlockChance = this.GetShieldBlockChance();
		return UnityEngine.Random.Range(0f, 1f) < shieldBlockChance;
	}

	// Token: 0x06000D5C RID: 3420 RVA: 0x0003F258 File Offset: 0x0003D458
	[Server]
	public int CalculateDamage(float skillPower, float bonusAttack, out bool critical, out bool blocked, GameObject attacker)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Int32 CombatModule::CalculateDamage(System.Single,System.Single,System.Boolean&,System.Boolean&,UnityEngine.GameObject)' called when server was not active");
			critical = default(bool);
			blocked = default(bool);
			return 0;
		}
		blocked = false;
		critical = false;
		if (attacker == null)
		{
			return 0;
		}
		if (this.conditionModule.HasActiveCondition(ConditionCategory.Invincibility))
		{
			return 0;
		}
		if (this.BlockAttack())
		{
			if (this.vocationModule.Vocation == Vocation.Protector)
			{
				this.attributeModule.AddEndurance(this.attributeModule.EnduranceRegenerationAmount * 3);
			}
			blocked = true;
			return 0;
		}
		CombatModule component = attacker.GetComponent<CombatModule>();
		int attack = component.GetAttack(skillPower, bonusAttack);
		if (attack == 0)
		{
			return 0;
		}
		this.ReflectDamageIfNeeded(component, attack);
		int num = this.CalculateIncomingAttack(out critical, component, attack);
		if (num != 0)
		{
			this.ActivateArcaneProtection();
		}
		return num;
	}

	// Token: 0x06000D5D RID: 3421 RVA: 0x0003F338 File Offset: 0x0003D538
	private void ReflectDamageIfNeeded(CombatModule attackerCombatModule, int incomingAttack)
	{
		Condition condition = this.conditionModule.GetCondition(ConditionType.Reflect);
		if (!condition.IsDefined)
		{
			return;
		}
		if (base.CompareTag("Player") && attackerCombatModule.CompareTag("Player") && this.pvpModule.PvpStatus == PvpStatus.Neutral)
		{
			return;
		}
		bool flag;
		int num = attackerCombatModule.CalculateIncomingAttack(out flag, this, incomingAttack);
		num = GlobalUtils.CalculateAndUseTvtDamageIfNeeded(num, 100f, base.gameObject, attackerCombatModule.gameObject);
		if (num == 0)
		{
			return;
		}
		EffectConfig damageEffectConfig = new EffectConfig
		{
			EffectName = condition.Effect.Name,
			EffectScaleModifier = condition.Effect.ScaleModifier,
			EffectSpeedModifier = condition.Effect.SpeedModifier,
			SoundEffectName = condition.SoundEffectName,
			Text = num.ToString(),
			TextColorId = condition.TextColorId
		};
		attackerCombatModule.ApplyDamage(num, damageEffectConfig, base.gameObject, false, Array.Empty<ConditionConfig>());
	}

	// Token: 0x06000D5E RID: 3422 RVA: 0x0003F430 File Offset: 0x0003D630
	public int CalculateIncomingAttack(out bool critical, CombatModule attackerCombatModule, int incomingAttack)
	{
		int weaponReduction = this.GetWeaponReduction();
		int armorReduction = this.GetArmorReduction();
		float num = (float)Mathf.Max(incomingAttack - weaponReduction, 0);
		float num2 = 1f - Mathf.Min(0.8f, (float)armorReduction / (float)incomingAttack);
		int num3 = Mathf.CeilToInt(num * num2);
		return attackerCombatModule.GetCritical(out critical, (float)num3);
	}

	// Token: 0x06000D5F RID: 3423 RVA: 0x0003F480 File Offset: 0x0003D680
	private float GetShieldBlockChance()
	{
		if (this.equipmentModule == null)
		{
			return 0f;
		}
		if (this.equipmentModule.EquippedItems.Count == 0)
		{
			return 0f;
		}
		Item item = this.equipmentModule.EquippedItems[7];
		if (!item.IsDefined)
		{
			return 0f;
		}
		return item.BlockChance;
	}

	// Token: 0x06000D60 RID: 3424 RVA: 0x0003F4E4 File Offset: 0x0003D6E4
	private float GetArcaneProtectionChance()
	{
		if (this.equipmentModule == null)
		{
			return 0f;
		}
		if (this.equipmentModule.EquippedItems.Count == 0)
		{
			return 0f;
		}
		Item item = this.equipmentModule.EquippedItems[7];
		if (!item.IsDefined)
		{
			return 0f;
		}
		return item.ArcaneProtectionChance;
	}

	// Token: 0x06000D61 RID: 3425 RVA: 0x0003F548 File Offset: 0x0003D748
	[Server]
	public int GetWeaponReduction()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Int32 CombatModule::GetWeaponReduction()' called when server was not active");
			return 0;
		}
		float num = (float)this.GetWeaponDefense();
		float num2 = (this.monsterModule != null) ? ((float)this.monsterModule.Defense) : 0f;
		int num3 = 0;
		float num4 = 0f;
		if (this.attributeModule != null)
		{
			num3 = this.attributeModule.BaseLevel;
			num4 = this.attributeModule.Defense;
		}
		else if (this.nonPlayerAttributeModule != null)
		{
			num3 = this.nonPlayerAttributeModule.BaseLevel;
			num4 = this.nonPlayerAttributeModule.Defense;
		}
		float num5 = UnityEngine.Random.Range(0.9f, 1f);
		return Mathf.RoundToInt(0.01f * num5 * (num + num2) * (num4 + (float)(num3 / 5)));
	}

	// Token: 0x06000D62 RID: 3426 RVA: 0x0003F620 File Offset: 0x0003D820
	[Server]
	public int GetArmorReduction()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Int32 CombatModule::GetArmorReduction()' called when server was not active");
			return 0;
		}
		float num = (float)this.GetArmorDefense();
		float num2 = (this.monsterModule != null) ? ((float)this.monsterModule.Defense) : 0f;
		int num3 = 0;
		float num4 = 0f;
		if (this.attributeModule != null)
		{
			num3 = this.attributeModule.BaseLevel;
			num4 = this.attributeModule.Resistance;
		}
		else if (this.nonPlayerAttributeModule != null)
		{
			num3 = this.nonPlayerAttributeModule.BaseLevel;
			num4 = this.nonPlayerAttributeModule.Resistance;
		}
		float num5 = UnityEngine.Random.Range(0.95f, 1f);
		return Mathf.RoundToInt(0.01f * num5 * (num + num2 * 3f) * (num4 + (float)(num3 / 5)));
	}

	// Token: 0x06000D63 RID: 3427 RVA: 0x0003F700 File Offset: 0x0003D900
	[Server]
	private int GetAttack(float skillPower, float bonusAttack = 0f)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Int32 CombatModule::GetAttack(System.Single,System.Single)' called when server was not active");
			return 0;
		}
		int weaponAttack = this.GetWeaponAttack();
		float num = (this.monsterModule != null) ? ((float)this.monsterModule.Attack) : 0f;
		int num2 = 0;
		float num3 = 0f;
		if (this.attributeModule != null)
		{
			num3 = this.attributeModule.Attack;
			num2 = this.attributeModule.BaseLevel;
		}
		else if (this.nonPlayerAttributeModule != null)
		{
			num3 = this.nonPlayerAttributeModule.Attack;
			num2 = this.nonPlayerAttributeModule.BaseLevel;
		}
		float num4 = UnityEngine.Random.Range(0.95f, 1f);
		float num5 = 0.009f * num4 * ((float)weaponAttack + bonusAttack + skillPower + num) * (num3 + (float)(num2 / 5));
		float num6 = skillPower / 54f;
		return Mathf.RoundToInt(num5 * num6);
	}

	// Token: 0x06000D64 RID: 3428 RVA: 0x0003F7E8 File Offset: 0x0003D9E8
	[Server]
	private int GetCritical(out bool critical, float attack)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Int32 CombatModule::GetCritical(System.Boolean&,System.Single)' called when server was not active");
			critical = default(bool);
			return 0;
		}
		critical = false;
		float num = UnityEngine.Random.Range(0f, 1f);
		if (this.attributeModule != null)
		{
			critical = (num <= this.attributeModule.CriticalChance);
			attack = (critical ? (attack * this.attributeModule.CriticalDamage) : attack);
		}
		else if (this.nonPlayerAttributeModule != null)
		{
			critical = (num <= this.nonPlayerAttributeModule.CriticalChance);
			attack = (critical ? (attack * this.nonPlayerAttributeModule.CriticalDamage) : attack);
		}
		return Mathf.RoundToInt(attack);
	}

	// Token: 0x06000D65 RID: 3429 RVA: 0x0003F8B4 File Offset: 0x0003DAB4
	public int GetWeaponDefense()
	{
		int num = (this.nonPlayerAttributeModule != null) ? this.nonPlayerAttributeModule.BaseLevel : this.attributeModule.BaseLevel;
		if (this.equipmentModule == null)
		{
			return 0;
		}
		int num2 = 0;
		for (int i = 0; i < this.equipmentModule.EquippedItems.Count; i++)
		{
			if (this.equipmentModule.EquippedItems[i].RequiredLevel <= num && this.equipmentModule.EquippedItems[i].Category == ItemCategory.Weapon)
			{
				num2 += (int)this.equipmentModule.EquippedItems[i].Defense;
			}
		}
		return num2;
	}

	// Token: 0x06000D66 RID: 3430 RVA: 0x0003F968 File Offset: 0x0003DB68
	public int GetWeaponAttack()
	{
		int num = (this.nonPlayerAttributeModule != null) ? this.nonPlayerAttributeModule.BaseLevel : this.attributeModule.BaseLevel;
		if (this.equipmentModule == null)
		{
			return 0;
		}
		int num2 = 0;
		for (int i = 0; i < this.equipmentModule.EquippedItems.Count; i++)
		{
			if (this.equipmentModule.EquippedItems[i].RequiredLevel <= num && this.equipmentModule.EquippedItems[i].Category == ItemCategory.Weapon)
			{
				num2 += (int)this.equipmentModule.EquippedItems[i].Attack;
			}
		}
		return num2;
	}

	// Token: 0x06000D67 RID: 3431 RVA: 0x0003FA1C File Offset: 0x0003DC1C
	public int GetArmorDefense()
	{
		int num = (this.nonPlayerAttributeModule != null) ? this.nonPlayerAttributeModule.BaseLevel : this.attributeModule.BaseLevel;
		if (this.equipmentModule == null)
		{
			return 0;
		}
		int num2 = 0;
		for (int i = 0; i < this.equipmentModule.EquippedItems.Count; i++)
		{
			if (this.equipmentModule.EquippedItems[i].RequiredLevel <= num && this.equipmentModule.EquippedItems[i].Category == ItemCategory.Armor)
			{
				num2 += (int)this.equipmentModule.EquippedItems[i].Defense;
			}
		}
		return num2;
	}

	// Token: 0x06000D69 RID: 3433 RVA: 0x0002FEC9 File Offset: 0x0002E0C9
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x17000179 RID: 377
	// (get) Token: 0x06000D6A RID: 3434 RVA: 0x0003FAE4 File Offset: 0x0003DCE4
	// (set) Token: 0x06000D6B RID: 3435 RVA: 0x0003FAF7 File Offset: 0x0003DCF7
	public bool NetworkInCombat
	{
		get
		{
			return this.InCombat;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<bool>(value, ref this.InCombat, 1UL, new Action<bool, bool>(this.OnInCombat));
		}
	}

	// Token: 0x1700017A RID: 378
	// (get) Token: 0x06000D6C RID: 3436 RVA: 0x0003FB1C File Offset: 0x0003DD1C
	// (set) Token: 0x06000D6D RID: 3437 RVA: 0x0003FB2F File Offset: 0x0003DD2F
	public bool NetworkIsFighting
	{
		get
		{
			return this.IsFighting;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<bool>(value, ref this.IsFighting, 2UL, new Action<bool, bool>(this.OnIsFighting));
		}
	}

	// Token: 0x1700017B RID: 379
	// (get) Token: 0x06000D6E RID: 3438 RVA: 0x0003FB54 File Offset: 0x0003DD54
	// (set) Token: 0x06000D6F RID: 3439 RVA: 0x0003FB67 File Offset: 0x0003DD67
	public bool NetworkIsPreemptive
	{
		get
		{
			return this.IsPreemptive;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<bool>(value, ref this.IsPreemptive, 4UL, null);
		}
	}

	// Token: 0x1700017C RID: 380
	// (get) Token: 0x06000D70 RID: 3440 RVA: 0x0003FB84 File Offset: 0x0003DD84
	// (set) Token: 0x06000D71 RID: 3441 RVA: 0x0003FB97 File Offset: 0x0003DD97
	public int NetworkExtraTargets
	{
		get
		{
			return this.ExtraTargets;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.ExtraTargets, 8UL, null);
		}
	}

	// Token: 0x06000D72 RID: 3442 RVA: 0x0003FBB1 File Offset: 0x0003DDB1
	protected void UserCode_CmdRemoveTarget()
	{
		this.RemoveTarget();
	}

	// Token: 0x06000D73 RID: 3443 RVA: 0x0003FBB9 File Offset: 0x0003DDB9
	protected static void InvokeUserCode_CmdRemoveTarget(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdRemoveTarget called on client.");
			return;
		}
		((CombatModule)obj).UserCode_CmdRemoveTarget();
	}

	// Token: 0x06000D74 RID: 3444 RVA: 0x0003FBDC File Offset: 0x0003DDDC
	protected void UserCode_TargetRemoveTarget__NetworkConnection(NetworkConnection target)
	{
		this.TargetLocked = false;
		if (this.Target != null)
		{
			this.Target = null;
		}
	}

	// Token: 0x06000D75 RID: 3445 RVA: 0x0003FBFA File Offset: 0x0003DDFA
	protected static void InvokeUserCode_TargetRemoveTarget__NetworkConnection(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetRemoveTarget called on server.");
			return;
		}
		((CombatModule)obj).UserCode_TargetRemoveTarget__NetworkConnection(null);
	}

	// Token: 0x06000D76 RID: 3446 RVA: 0x0003FC1E File Offset: 0x0003DE1E
	protected void UserCode_CmdSetTarget__GameObject__Boolean(GameObject target, bool lockTarget)
	{
		this.SetTarget(target, lockTarget);
	}

	// Token: 0x06000D77 RID: 3447 RVA: 0x0003FC28 File Offset: 0x0003DE28
	protected static void InvokeUserCode_CmdSetTarget__GameObject__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSetTarget called on client.");
			return;
		}
		((CombatModule)obj).UserCode_CmdSetTarget__GameObject__Boolean(reader.ReadGameObject(), reader.ReadBool());
	}

	// Token: 0x06000D78 RID: 3448 RVA: 0x0003FC57 File Offset: 0x0003DE57
	protected void UserCode_TargetSetTarget__NetworkConnection__GameObject__Boolean(NetworkConnection target, GameObject targetObject, bool lockTarget)
	{
		if (targetObject != null)
		{
			this.Target = targetObject;
			this.TargetLocked = true;
		}
	}

	// Token: 0x06000D79 RID: 3449 RVA: 0x0003FC70 File Offset: 0x0003DE70
	protected static void InvokeUserCode_TargetSetTarget__NetworkConnection__GameObject__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetSetTarget called on server.");
			return;
		}
		((CombatModule)obj).UserCode_TargetSetTarget__NetworkConnection__GameObject__Boolean(null, reader.ReadGameObject(), reader.ReadBool());
	}

	// Token: 0x06000D7A RID: 3450 RVA: 0x0003FCA0 File Offset: 0x0003DEA0
	static CombatModule()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(CombatModule), "System.Void CombatModule::CmdRemoveTarget()", new RemoteCallDelegate(CombatModule.InvokeUserCode_CmdRemoveTarget), true);
		RemoteProcedureCalls.RegisterCommand(typeof(CombatModule), "System.Void CombatModule::CmdSetTarget(UnityEngine.GameObject,System.Boolean)", new RemoteCallDelegate(CombatModule.InvokeUserCode_CmdSetTarget__GameObject__Boolean), true);
		RemoteProcedureCalls.RegisterRpc(typeof(CombatModule), "System.Void CombatModule::TargetRemoveTarget(Mirror.NetworkConnection)", new RemoteCallDelegate(CombatModule.InvokeUserCode_TargetRemoveTarget__NetworkConnection));
		RemoteProcedureCalls.RegisterRpc(typeof(CombatModule), "System.Void CombatModule::TargetSetTarget(Mirror.NetworkConnection,UnityEngine.GameObject,System.Boolean)", new RemoteCallDelegate(CombatModule.InvokeUserCode_TargetSetTarget__NetworkConnection__GameObject__Boolean));
	}

	// Token: 0x06000D7B RID: 3451 RVA: 0x0003FD30 File Offset: 0x0003DF30
	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteBool(this.InCombat);
			writer.WriteBool(this.IsFighting);
			writer.WriteBool(this.IsPreemptive);
			writer.WriteInt(this.ExtraTargets);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1UL) != 0UL)
		{
			writer.WriteBool(this.InCombat);
		}
		if ((base.syncVarDirtyBits & 2UL) != 0UL)
		{
			writer.WriteBool(this.IsFighting);
		}
		if ((base.syncVarDirtyBits & 4UL) != 0UL)
		{
			writer.WriteBool(this.IsPreemptive);
		}
		if ((base.syncVarDirtyBits & 8UL) != 0UL)
		{
			writer.WriteInt(this.ExtraTargets);
		}
	}

	// Token: 0x06000D7C RID: 3452 RVA: 0x0003FE14 File Offset: 0x0003E014
	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			base.GeneratedSyncVarDeserialize<bool>(ref this.InCombat, new Action<bool, bool>(this.OnInCombat), reader.ReadBool());
			base.GeneratedSyncVarDeserialize<bool>(ref this.IsFighting, new Action<bool, bool>(this.OnIsFighting), reader.ReadBool());
			base.GeneratedSyncVarDeserialize<bool>(ref this.IsPreemptive, null, reader.ReadBool());
			base.GeneratedSyncVarDeserialize<int>(ref this.ExtraTargets, null, reader.ReadInt());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 1L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<bool>(ref this.InCombat, new Action<bool, bool>(this.OnInCombat), reader.ReadBool());
		}
		if ((num & 2L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<bool>(ref this.IsFighting, new Action<bool, bool>(this.OnIsFighting), reader.ReadBool());
		}
		if ((num & 4L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<bool>(ref this.IsPreemptive, null, reader.ReadBool());
		}
		if ((num & 8L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.ExtraTargets, null, reader.ReadInt());
		}
	}

	// Token: 0x04000DE6 RID: 3558
	[SyncVar(hook = "OnInCombat")]
	public bool InCombat;

	// Token: 0x04000DE7 RID: 3559
	[SyncVar(hook = "OnIsFighting")]
	public bool IsFighting;

	// Token: 0x04000DE8 RID: 3560
	[SyncVar]
	public bool IsPreemptive;

	// Token: 0x04000DE9 RID: 3561
	[SyncVar]
	public int ExtraTargets;

	// Token: 0x04000DEA RID: 3562
	public bool HasTarget;

	// Token: 0x04000DEC RID: 3564
	private int targetIndex;

	// Token: 0x04000DED RID: 3565
	private PvpModule pvpModule;

	// Token: 0x04000DEE RID: 3566
	private AreaModule areaModule;

	// Token: 0x04000DEF RID: 3567
	private EffectModule effectModule;

	// Token: 0x04000DF0 RID: 3568
	private MonsterModule monsterModule;

	// Token: 0x04000DF1 RID: 3569
	private CreatureModule creatureModule;

	// Token: 0x04000DF2 RID: 3570
	private MovementModule movementModule;

	// Token: 0x04000DF3 RID: 3571
	private VocationModule vocationModule;

	// Token: 0x04000DF4 RID: 3572
	private AttributeModule attributeModule;

	// Token: 0x04000DF5 RID: 3573
	private EquipmentModule equipmentModule;

	// Token: 0x04000DF6 RID: 3574
	private ConditionModule conditionModule;

	// Token: 0x04000DF7 RID: 3575
	private NonPlayerAttributeModule nonPlayerAttributeModule;

	// Token: 0x04000DF8 RID: 3576
	public bool TargetLocked;

	// Token: 0x04000DF9 RID: 3577
	public List<Attacker> Attackers;

	// Token: 0x04000DFA RID: 3578
	private UISystemModule uiSystemModule;

	// Token: 0x04000DFB RID: 3579
	private readonly List<GameObject> nearbyTargets = new List<GameObject>();

	// Token: 0x04000DFD RID: 3581
	private GameObject _target;

	// Token: 0x020002E8 RID: 744
	// (Invoke) Token: 0x06000D7E RID: 3454
	public delegate void OnGotTargetEventHandler(GameObject target);

	// Token: 0x020002E9 RID: 745
	// (Invoke) Token: 0x06000D82 RID: 3458
	public delegate void OnLostTargetEventHandler(GameObject target);

	// Token: 0x020002EA RID: 746
	// (Invoke) Token: 0x06000D86 RID: 3462
	public delegate void OnReceiveDamageEventHandler(GameObject attacker, int damage);
}
