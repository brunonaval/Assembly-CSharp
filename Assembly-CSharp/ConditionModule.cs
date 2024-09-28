using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

// Token: 0x020002EE RID: 750
public class ConditionModule : NetworkBehaviour
{
	// Token: 0x1700017D RID: 381
	// (get) Token: 0x06000D97 RID: 3479 RVA: 0x0004049B File Offset: 0x0003E69B
	public bool HasAnyCondition
	{
		get
		{
			return this.Conditions.Count > 0;
		}
	}

	// Token: 0x1400000A RID: 10
	// (add) Token: 0x06000D98 RID: 3480 RVA: 0x000404AC File Offset: 0x0003E6AC
	// (remove) Token: 0x06000D99 RID: 3481 RVA: 0x000404E4 File Offset: 0x0003E6E4
	public event ConditionModule.OnConditionCreatedEventHandler OnConditionCreated;

	// Token: 0x1400000B RID: 11
	// (add) Token: 0x06000D9A RID: 3482 RVA: 0x0004051C File Offset: 0x0003E71C
	// (remove) Token: 0x06000D9B RID: 3483 RVA: 0x00040554 File Offset: 0x0003E754
	public event ConditionModule.OnConditionRemovedEventHandler OnConditionRemoved;

	// Token: 0x06000D9C RID: 3484 RVA: 0x0004058C File Offset: 0x0003E78C
	private void Awake()
	{
		if (NetworkClient.active)
		{
			this.trailRenderer = base.GetComponentInChildren<TrailRenderer>();
			GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
			this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
		}
		base.TryGetComponent<PvpModule>(out this.pvpModule);
		base.TryGetComponent<CombatModule>(out this.combatModule);
		base.TryGetComponent<EffectModule>(out this.effectModule);
		base.TryGetComponent<CreatureModule>(out this.creatureModule);
		base.TryGetComponent<MovementModule>(out this.movementModule);
		base.TryGetComponent<AttributeModule>(out this.attributeModule);
		base.TryGetComponent<InventoryModule>(out this.inventoryModule);
		base.TryGetComponent<NonPlayerAttributeModule>(out this.nonPlayerAttributeModule);
	}

	// Token: 0x06000D9D RID: 3485 RVA: 0x0004062B File Offset: 0x0003E82B
	private void Start()
	{
		if (this.trailRenderer != null)
		{
			this.trailRenderer.enabled = false;
		}
	}

	// Token: 0x06000D9E RID: 3486 RVA: 0x00040647 File Offset: 0x0003E847
	public override void OnStartServer()
	{
		if (!base.CompareTag("Player"))
		{
			return;
		}
		this.Conditions.Callback += delegate(SyncList<Condition>.Operation op, int index, Condition oldItem, Condition newItem)
		{
			if (newItem.Duration >= 180f | oldItem.Duration >= 180f)
			{
				this.ShouldPersistData = true;
			}
		};
	}

	// Token: 0x06000D9F RID: 3487 RVA: 0x0004066E File Offset: 0x0003E86E
	public override void OnStartClient()
	{
		this.Conditions.Callback += this.OnConditionsChanged;
	}

	// Token: 0x06000DA0 RID: 3488 RVA: 0x00040687 File Offset: 0x0003E887
	public override void OnStartLocalPlayer()
	{
		this.BuildConditions();
	}

	// Token: 0x06000DA1 RID: 3489 RVA: 0x00040690 File Offset: 0x0003E890
	[Server]
	private void UpdateElapsed(Condition conditionToUpdate, float elapsed)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ConditionModule::UpdateElapsed(Condition,System.Single)' called when server was not active");
			return;
		}
		for (int i = 0; i < this.Conditions.Count; i++)
		{
			try
			{
				if (this.Conditions[i].IsEqual(conditionToUpdate))
				{
					Condition value = this.Conditions[i];
					value.Elapsed = elapsed;
					if (elapsed == 0f)
					{
						value.LastUseTime = NetworkTime.time;
					}
					this.Conditions[i] = value;
					break;
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}
	}

	// Token: 0x06000DA2 RID: 3490 RVA: 0x00040734 File Offset: 0x0003E934
	public Condition GetCondition(ConditionType conditionType)
	{
		for (int i = 0; i < this.Conditions.Count; i++)
		{
			if (this.Conditions[i].Type == conditionType)
			{
				return this.Conditions[i];
			}
		}
		return default(Condition);
	}

	// Token: 0x06000DA3 RID: 3491 RVA: 0x00040781 File Offset: 0x0003E981
	public IEnumerable<Condition> GetConditions(ConditionType conditionType)
	{
		int num;
		for (int i = 0; i < this.Conditions.Count; i = num + 1)
		{
			if (this.Conditions[i].Type == conditionType)
			{
				yield return this.Conditions[i];
			}
			num = i;
		}
		yield break;
	}

	// Token: 0x06000DA4 RID: 3492 RVA: 0x00040798 File Offset: 0x0003E998
	public Condition GetCondition(Condition condition)
	{
		for (int i = 0; i < this.Conditions.Count; i++)
		{
			if (this.Conditions[i].IsEqual(condition))
			{
				return this.Conditions[i];
			}
		}
		return default(Condition);
	}

	// Token: 0x06000DA5 RID: 3493 RVA: 0x000407E8 File Offset: 0x0003E9E8
	[Server]
	public void StartNeutralCondition(Condition condition, GameObject caster, bool invokeEvents)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ConditionModule::StartNeutralCondition(Condition,UnityEngine.GameObject,System.Boolean)' called when server was not active");
			return;
		}
		this.UpdateOrCreateCondition(condition, caster, true, invokeEvents);
	}

	// Token: 0x06000DA6 RID: 3494 RVA: 0x0004080C File Offset: 0x0003EA0C
	[Server]
	public void StartCondition(Condition condition, GameObject caster, bool invokeEvents)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ConditionModule::StartCondition(Condition,UnityEngine.GameObject,System.Boolean)' called when server was not active");
			return;
		}
		if (condition.IsBadCondition && this.combatModule.IsPvpProtected(caster))
		{
			return;
		}
		this.UpdateOrCreateCondition(condition, caster, false, invokeEvents);
		if (condition.IsNeutralCondition)
		{
			return;
		}
		this.EnterCombatIfPlayer();
		this.PutCasterInCombatIfPlayer(caster);
		this.ChangeCasterPvpStatusIfNeeded(caster, condition);
	}

	// Token: 0x06000DA7 RID: 3495 RVA: 0x00040870 File Offset: 0x0003EA70
	[Server]
	public void StartConditionAndIgnorePersistence(Condition condition, GameObject caster, bool invokeEvents)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ConditionModule::StartConditionAndIgnorePersistence(Condition,UnityEngine.GameObject,System.Boolean)' called when server was not active");
			return;
		}
		this.RemoveCondition(condition.Category, condition.Type, condition.Duration, true);
		condition.IgnorePersistence = true;
		this.StartNeutralCondition(condition, caster, invokeEvents);
	}

	// Token: 0x06000DA8 RID: 3496 RVA: 0x000408BC File Offset: 0x0003EABC
	private void UpdateOrCreateCondition(Condition condition, GameObject caster, bool isNeutral, bool invokeEvents)
	{
		if (!isNeutral & !this.creatureModule.IsAlive)
		{
			return;
		}
		if (condition.CanUpdateElapsedTime && this.HasActiveCondition(condition))
		{
			this.UpdateElapsed(condition, 0f);
			return;
		}
		if (condition.Category == ConditionCategory.Telekinesis)
		{
			if (this.TelekinesisCoroutine != null)
			{
				base.StopCoroutine(this.TelekinesisCoroutine);
			}
			this.TelekinesisCoroutine = base.StartCoroutine(this.CreateCondition(condition, caster, invokeEvents));
			return;
		}
		base.StartCoroutine(this.CreateCondition(condition, caster, invokeEvents));
	}

	// Token: 0x06000DA9 RID: 3497 RVA: 0x00040944 File Offset: 0x0003EB44
	private void EnterCombatIfPlayer()
	{
		if (base.CompareTag("Player"))
		{
			this.combatModule.StartCombatAndFightingFlags();
		}
	}

	// Token: 0x06000DAA RID: 3498 RVA: 0x0004095E File Offset: 0x0003EB5E
	private void PutCasterInCombatIfPlayer(GameObject caster)
	{
		if (caster != null && caster.CompareTag("Player"))
		{
			caster.GetComponent<CombatModule>().StartCombatAndFightingFlags();
		}
	}

	// Token: 0x06000DAB RID: 3499 RVA: 0x00040984 File Offset: 0x0003EB84
	private void ChangeCasterPvpStatusIfNeeded(GameObject caster, Condition condition)
	{
		if (!base.CompareTag("Player"))
		{
			return;
		}
		if (!caster.CompareTag("Player"))
		{
			return;
		}
		if (base.gameObject == caster)
		{
			return;
		}
		if (!condition.IsBadCondition)
		{
			return;
		}
		PvpModule pvpModule;
		caster.TryGetComponent<PvpModule>(out pvpModule);
		if (pvpModule.PvpStatus != PvpStatus.Neutral)
		{
			return;
		}
		pvpModule.SetPvpStatusAsync(PvpStatus.InCombat, true);
	}

	// Token: 0x06000DAC RID: 3500 RVA: 0x000409E0 File Offset: 0x0003EBE0
	[Server]
	private IEnumerator CreateCondition(Condition condition, GameObject caster, bool invokeEvents)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Collections.IEnumerator ConditionModule::CreateCondition(Condition,UnityEngine.GameObject,System.Boolean)' called when server was not active");
			return null;
		}
		ConditionModule.<CreateCondition>d__43 <CreateCondition>d__ = new ConditionModule.<CreateCondition>d__43(0);
		<CreateCondition>d__.<>4__this = this;
		<CreateCondition>d__.condition = condition;
		<CreateCondition>d__.caster = caster;
		<CreateCondition>d__.invokeEvents = invokeEvents;
		return <CreateCondition>d__;
	}

	// Token: 0x06000DAD RID: 3501 RVA: 0x00040A30 File Offset: 0x0003EC30
	[Server]
	public void RemoveCondition(ConditionType conditionType)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ConditionModule::RemoveCondition(ConditionType)' called when server was not active");
			return;
		}
		foreach (Condition condition in from c in this.Conditions
		where c.Type == conditionType
		select c)
		{
			ConditionModule.OnConditionRemovedEventHandler onConditionRemoved = this.OnConditionRemoved;
			if (onConditionRemoved != null)
			{
				onConditionRemoved(condition);
			}
		}
		this.Conditions.RemoveAll((Condition c) => c.Type == conditionType);
	}

	// Token: 0x06000DAE RID: 3502 RVA: 0x00040AD4 File Offset: 0x0003ECD4
	[Server]
	public void RemoveCondition(ConditionCategory category, ConditionType conditionType, float duration, bool ignorePersistence)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ConditionModule::RemoveCondition(ConditionCategory,ConditionType,System.Single,System.Boolean)' called when server was not active");
			return;
		}
		foreach (Condition condition in from c in this.Conditions
		where c.Type == conditionType & c.Category == category & c.Duration == duration & c.IgnorePersistence == ignorePersistence
		select c)
		{
			ConditionModule.OnConditionRemovedEventHandler onConditionRemoved = this.OnConditionRemoved;
			if (onConditionRemoved != null)
			{
				onConditionRemoved(condition);
			}
		}
		this.Conditions.RemoveAll((Condition c) => c.Type == conditionType & c.Category == category & c.Duration == duration & c.IgnorePersistence == ignorePersistence);
	}

	// Token: 0x06000DAF RID: 3503 RVA: 0x00040B90 File Offset: 0x0003ED90
	[Server]
	public void RemoveCondition(Condition conditionToRemove)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ConditionModule::RemoveCondition(Condition)' called when server was not active");
			return;
		}
		for (int i = 0; i < this.Conditions.Count; i++)
		{
			try
			{
				if (this.Conditions[i].IsEqual(conditionToRemove))
				{
					Condition condition = this.Conditions[i];
					this.Conditions.Remove(condition);
					ConditionModule.OnConditionRemovedEventHandler onConditionRemoved = this.OnConditionRemoved;
					if (onConditionRemoved != null)
					{
						onConditionRemoved(condition);
					}
					break;
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
	}

	// Token: 0x06000DB0 RID: 3504 RVA: 0x00040C48 File Offset: 0x0003EE48
	[Server]
	public void RemoveAllConditions()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ConditionModule::RemoveAllConditions()' called when server was not active");
			return;
		}
		this.Conditions.Clear();
	}

	// Token: 0x06000DB1 RID: 3505 RVA: 0x00040C6C File Offset: 0x0003EE6C
	public bool HasActiveCondition(ConditionCategory conditionCategory)
	{
		for (int i = 0; i < this.Conditions.Count; i++)
		{
			try
			{
				if (this.Conditions[i].Category == conditionCategory & this.Conditions[i].CooldownTimerPercent() < 1.0)
				{
					return true;
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
		return false;
	}

	// Token: 0x06000DB2 RID: 3506 RVA: 0x00040D04 File Offset: 0x0003EF04
	public bool HasActiveCondition(ConditionType conditionType)
	{
		for (int i = 0; i < this.Conditions.Count; i++)
		{
			try
			{
				if (this.Conditions[i].Type == conditionType & this.Conditions[i].CooldownTimerPercent() < 1.0)
				{
					return true;
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
		return false;
	}

	// Token: 0x06000DB3 RID: 3507 RVA: 0x00040D9C File Offset: 0x0003EF9C
	public bool HasActiveCondition(Condition condition)
	{
		return this.HasCondition(condition, true);
	}

	// Token: 0x06000DB4 RID: 3508 RVA: 0x00040DA8 File Offset: 0x0003EFA8
	private bool HasCondition(Condition condition, bool mustBeActive)
	{
		for (int i = 0; i < this.Conditions.Count; i++)
		{
			try
			{
				if (this.Conditions[i].IsEqual(condition))
				{
					if (!mustBeActive)
					{
						return true;
					}
					if (mustBeActive & this.Conditions[i].CooldownTimerPercent() < 1.0)
					{
						return true;
					}
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
		return false;
	}

	// Token: 0x06000DB5 RID: 3509 RVA: 0x00040E4C File Offset: 0x0003F04C
	[Server]
	private IEnumerator Regeneration(Condition condition, GameObject caster)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Collections.IEnumerator ConditionModule::Regeneration(Condition,UnityEngine.GameObject)' called when server was not active");
			return null;
		}
		ConditionModule.<Regeneration>d__52 <Regeneration>d__ = new ConditionModule.<Regeneration>d__52(0);
		<Regeneration>d__.<>4__this = this;
		<Regeneration>d__.condition = condition;
		<Regeneration>d__.caster = caster;
		return <Regeneration>d__;
	}

	// Token: 0x06000DB6 RID: 3510 RVA: 0x00040E98 File Offset: 0x0003F098
	[Server]
	private IEnumerator Degeneration(Condition condition, GameObject caster)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Collections.IEnumerator ConditionModule::Degeneration(Condition,UnityEngine.GameObject)' called when server was not active");
			return null;
		}
		ConditionModule.<Degeneration>d__53 <Degeneration>d__ = new ConditionModule.<Degeneration>d__53(0);
		<Degeneration>d__.<>4__this = this;
		<Degeneration>d__.condition = condition;
		<Degeneration>d__.caster = caster;
		return <Degeneration>d__;
	}

	// Token: 0x06000DB7 RID: 3511 RVA: 0x00040EE4 File Offset: 0x0003F0E4
	[Server]
	private IEnumerator LevelBlessing(Condition condition)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Collections.IEnumerator ConditionModule::LevelBlessing(Condition)' called when server was not active");
			return null;
		}
		ConditionModule.<LevelBlessing>d__54 <LevelBlessing>d__ = new ConditionModule.<LevelBlessing>d__54(0);
		<LevelBlessing>d__.<>4__this = this;
		<LevelBlessing>d__.condition = condition;
		return <LevelBlessing>d__;
	}

	// Token: 0x06000DB8 RID: 3512 RVA: 0x00040F28 File Offset: 0x0003F128
	[Server]
	private IEnumerator LevelCurse(Condition condition)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Collections.IEnumerator ConditionModule::LevelCurse(Condition)' called when server was not active");
			return null;
		}
		ConditionModule.<LevelCurse>d__55 <LevelCurse>d__ = new ConditionModule.<LevelCurse>d__55(0);
		<LevelCurse>d__.<>4__this = this;
		<LevelCurse>d__.condition = condition;
		return <LevelCurse>d__;
	}

	// Token: 0x06000DB9 RID: 3513 RVA: 0x00040F6C File Offset: 0x0003F16C
	[Server]
	private IEnumerator Blessing(Condition condition)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Collections.IEnumerator ConditionModule::Blessing(Condition)' called when server was not active");
			return null;
		}
		ConditionModule.<Blessing>d__56 <Blessing>d__ = new ConditionModule.<Blessing>d__56(0);
		<Blessing>d__.<>4__this = this;
		<Blessing>d__.condition = condition;
		return <Blessing>d__;
	}

	// Token: 0x06000DBA RID: 3514 RVA: 0x00040FB0 File Offset: 0x0003F1B0
	[Server]
	private IEnumerator Curse(Condition condition)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Collections.IEnumerator ConditionModule::Curse(Condition)' called when server was not active");
			return null;
		}
		ConditionModule.<Curse>d__57 <Curse>d__ = new ConditionModule.<Curse>d__57(0);
		<Curse>d__.<>4__this = this;
		<Curse>d__.condition = condition;
		return <Curse>d__;
	}

	// Token: 0x06000DBB RID: 3515 RVA: 0x00040FF4 File Offset: 0x0003F1F4
	[Server]
	private IEnumerator Resurrection(Condition condition)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Collections.IEnumerator ConditionModule::Resurrection(Condition)' called when server was not active");
			return null;
		}
		ConditionModule.<Resurrection>d__58 <Resurrection>d__ = new ConditionModule.<Resurrection>d__58(0);
		<Resurrection>d__.<>4__this = this;
		<Resurrection>d__.condition = condition;
		return <Resurrection>d__;
	}

	// Token: 0x06000DBC RID: 3516 RVA: 0x00041038 File Offset: 0x0003F238
	[Server]
	private IEnumerator Reflecting(Condition condition)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Collections.IEnumerator ConditionModule::Reflecting(Condition)' called when server was not active");
			return null;
		}
		ConditionModule.<Reflecting>d__59 <Reflecting>d__ = new ConditionModule.<Reflecting>d__59(0);
		<Reflecting>d__.<>4__this = this;
		<Reflecting>d__.condition = condition;
		return <Reflecting>d__;
	}

	// Token: 0x06000DBD RID: 3517 RVA: 0x0004107C File Offset: 0x0003F27C
	[Server]
	private IEnumerator Paralizing(Condition condition)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Collections.IEnumerator ConditionModule::Paralizing(Condition)' called when server was not active");
			return null;
		}
		ConditionModule.<Paralizing>d__60 <Paralizing>d__ = new ConditionModule.<Paralizing>d__60(0);
		<Paralizing>d__.<>4__this = this;
		<Paralizing>d__.condition = condition;
		return <Paralizing>d__;
	}

	// Token: 0x06000DBE RID: 3518 RVA: 0x000410C0 File Offset: 0x0003F2C0
	[Server]
	private IEnumerator Confusion(Condition condition, GameObject caster)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Collections.IEnumerator ConditionModule::Confusion(Condition,UnityEngine.GameObject)' called when server was not active");
			return null;
		}
		ConditionModule.<Confusion>d__61 <Confusion>d__ = new ConditionModule.<Confusion>d__61(0);
		<Confusion>d__.<>4__this = this;
		<Confusion>d__.condition = condition;
		<Confusion>d__.caster = caster;
		return <Confusion>d__;
	}

	// Token: 0x06000DBF RID: 3519 RVA: 0x0004110C File Offset: 0x0003F30C
	[Server]
	private IEnumerator Invisibility(Condition condition)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Collections.IEnumerator ConditionModule::Invisibility(Condition)' called when server was not active");
			return null;
		}
		ConditionModule.<Invisibility>d__62 <Invisibility>d__ = new ConditionModule.<Invisibility>d__62(0);
		<Invisibility>d__.<>4__this = this;
		<Invisibility>d__.condition = condition;
		return <Invisibility>d__;
	}

	// Token: 0x06000DC0 RID: 3520 RVA: 0x00041150 File Offset: 0x0003F350
	[Server]
	private IEnumerator Invincibility(Condition condition)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Collections.IEnumerator ConditionModule::Invincibility(Condition)' called when server was not active");
			return null;
		}
		ConditionModule.<Invincibility>d__63 <Invincibility>d__ = new ConditionModule.<Invincibility>d__63(0);
		<Invincibility>d__.<>4__this = this;
		<Invincibility>d__.condition = condition;
		return <Invincibility>d__;
	}

	// Token: 0x06000DC1 RID: 3521 RVA: 0x00041194 File Offset: 0x0003F394
	[Server]
	private IEnumerator Boost(Condition condition)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Collections.IEnumerator ConditionModule::Boost(Condition)' called when server was not active");
			return null;
		}
		ConditionModule.<Boost>d__64 <Boost>d__ = new ConditionModule.<Boost>d__64(0);
		<Boost>d__.<>4__this = this;
		<Boost>d__.condition = condition;
		return <Boost>d__;
	}

	// Token: 0x06000DC2 RID: 3522 RVA: 0x000411D8 File Offset: 0x0003F3D8
	[Server]
	private IEnumerator Food(Condition condition)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Collections.IEnumerator ConditionModule::Food(Condition)' called when server was not active");
			return null;
		}
		ConditionModule.<Food>d__65 <Food>d__ = new ConditionModule.<Food>d__65(0);
		<Food>d__.<>4__this = this;
		<Food>d__.condition = condition;
		return <Food>d__;
	}

	// Token: 0x06000DC3 RID: 3523 RVA: 0x0004121C File Offset: 0x0003F41C
	[Server]
	private IEnumerator Transformation(Condition condition)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Collections.IEnumerator ConditionModule::Transformation(Condition)' called when server was not active");
			return null;
		}
		ConditionModule.<Transformation>d__66 <Transformation>d__ = new ConditionModule.<Transformation>d__66(0);
		<Transformation>d__.<>4__this = this;
		<Transformation>d__.condition = condition;
		return <Transformation>d__;
	}

	// Token: 0x06000DC4 RID: 3524 RVA: 0x00041260 File Offset: 0x0003F460
	[Server]
	private IEnumerator Taunt(Condition condition, GameObject caster)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Collections.IEnumerator ConditionModule::Taunt(Condition,UnityEngine.GameObject)' called when server was not active");
			return null;
		}
		ConditionModule.<Taunt>d__67 <Taunt>d__ = new ConditionModule.<Taunt>d__67(0);
		<Taunt>d__.<>4__this = this;
		<Taunt>d__.condition = condition;
		<Taunt>d__.caster = caster;
		return <Taunt>d__;
	}

	// Token: 0x06000DC5 RID: 3525 RVA: 0x000412AC File Offset: 0x0003F4AC
	[Server]
	private IEnumerator Wrath(Condition condition)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Collections.IEnumerator ConditionModule::Wrath(Condition)' called when server was not active");
			return null;
		}
		ConditionModule.<Wrath>d__68 <Wrath>d__ = new ConditionModule.<Wrath>d__68(0);
		<Wrath>d__.<>4__this = this;
		<Wrath>d__.condition = condition;
		return <Wrath>d__;
	}

	// Token: 0x06000DC6 RID: 3526 RVA: 0x000412F0 File Offset: 0x0003F4F0
	[Server]
	private IEnumerator Expertise(Condition condition)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Collections.IEnumerator ConditionModule::Expertise(Condition)' called when server was not active");
			return null;
		}
		ConditionModule.<Expertise>d__69 <Expertise>d__ = new ConditionModule.<Expertise>d__69(0);
		<Expertise>d__.<>4__this = this;
		<Expertise>d__.condition = condition;
		return <Expertise>d__;
	}

	// Token: 0x06000DC7 RID: 3527 RVA: 0x00041334 File Offset: 0x0003F534
	[Server]
	private IEnumerator Telekinesis(Condition condition)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Collections.IEnumerator ConditionModule::Telekinesis(Condition)' called when server was not active");
			return null;
		}
		ConditionModule.<Telekinesis>d__70 <Telekinesis>d__ = new ConditionModule.<Telekinesis>d__70(0);
		<Telekinesis>d__.<>4__this = this;
		<Telekinesis>d__.condition = condition;
		return <Telekinesis>d__;
	}

	// Token: 0x06000DC8 RID: 3528 RVA: 0x00041378 File Offset: 0x0003F578
	public string NetName(string index)
	{
		return base.netId.ToString() + "_" + index;
	}

	// Token: 0x06000DC9 RID: 3529 RVA: 0x000413A0 File Offset: 0x0003F5A0
	[Client]
	private void OnConditionsChanged(SyncList<Condition>.Operation op, int index, Condition oldItem, Condition newItem)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void ConditionModule::OnConditionsChanged(Mirror.SyncList`1/Operation<Condition>,System.Int32,Condition,Condition)' called when client was not active");
			return;
		}
		if (op == SyncList<Condition>.Operation.OP_ADD)
		{
			if (base.isLocalPlayer)
			{
				this.DefineConditionBarSlot(index);
				this.DefineTargetConditionBarSlot(index);
				return;
			}
			this.DefineTargetConditionBarSlot(index);
			return;
		}
		else
		{
			if (op != SyncList<Condition>.Operation.OP_SET)
			{
				if (op == SyncList<Condition>.Operation.OP_REMOVEAT)
				{
					if (base.isLocalPlayer)
					{
						this.RemoveConditionBarSlot();
						this.RemoveTargetConditionBarSlot();
						return;
					}
					this.RemoveTargetConditionBarSlot();
				}
				return;
			}
			if (base.isLocalPlayer)
			{
				this.UpdateConditionBarSlot(index);
				this.UpdateTargetConditionBarSlot(index);
				return;
			}
			this.UpdateTargetConditionBarSlot(index);
			return;
		}
	}

	// Token: 0x06000DCA RID: 3530 RVA: 0x00041428 File Offset: 0x0003F628
	[Client]
	private void BuildConditions()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void ConditionModule::BuildConditions()' called when client was not active");
			return;
		}
		for (int i = 0; i < this.Conditions.Count; i++)
		{
			this.DefineConditionBarSlot(i);
			this.DefineTargetConditionBarSlot(i);
		}
	}

	// Token: 0x06000DCB RID: 3531 RVA: 0x00041470 File Offset: 0x0003F670
	[Client]
	public void BuildTargetConditions()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void ConditionModule::BuildTargetConditions()' called when client was not active");
			return;
		}
		this.DestroyTargetConditionsBarSlots();
		for (int i = 0; i < this.Conditions.Count; i++)
		{
			this.DefineTargetConditionBarSlot(i);
		}
	}

	// Token: 0x06000DCC RID: 3532 RVA: 0x000414B8 File Offset: 0x0003F6B8
	[Client]
	private void DefineTargetConditionBarSlot(int index)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void ConditionModule::DefineTargetConditionBarSlot(System.Int32)' called when client was not active");
			return;
		}
		GameObject target = this.uiSystemModule.CombatModule.Target;
		if (target == null)
		{
			return;
		}
		if (target.GetComponent<ConditionModule>().netId == base.netId)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.conditionBarSlotPrefab);
			gameObject.transform.SetParent(this.uiSystemModule.PlatformTargetConditionBar.transform, false);
			gameObject.transform.position = Vector2.zero;
			if (gameObject != null)
			{
				Condition condition = this.Conditions[index];
				gameObject.GetComponent<ConditionBarSlotManager>().SetCondition(true, condition);
				gameObject.name = this.NetName(condition.UniqueId);
			}
		}
	}

	// Token: 0x06000DCD RID: 3533 RVA: 0x0004157C File Offset: 0x0003F77C
	[Client]
	private void DefineConditionBarSlot(int index)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void ConditionModule::DefineConditionBarSlot(System.Int32)' called when client was not active");
			return;
		}
		GameObject conditionBarFromUiSystem = this.GetConditionBarFromUiSystem();
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.conditionBarSlotPrefab);
		gameObject.transform.SetParent(conditionBarFromUiSystem.transform, false);
		gameObject.transform.position = Vector2.zero;
		if (gameObject != null)
		{
			Condition condition = this.Conditions[index];
			gameObject.GetComponent<ConditionBarSlotManager>().SetCondition(false, condition);
			gameObject.name = this.NetName(condition.UniqueId);
		}
	}

	// Token: 0x06000DCE RID: 3534 RVA: 0x00041610 File Offset: 0x0003F810
	[Client]
	private void RemoveConditionBarSlot()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void ConditionModule::RemoveConditionBarSlot()' called when client was not active");
			return;
		}
		GameObject conditionBarFromUiSystem = this.GetConditionBarFromUiSystem();
		int childCount = conditionBarFromUiSystem.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = conditionBarFromUiSystem.transform.GetChild(i);
			ConditionBarSlotManager component = child.GetComponent<ConditionBarSlotManager>();
			if (component != null && !this.HasCondition(component.Condition, false))
			{
				UnityEngine.Object.Destroy(child.gameObject);
			}
		}
	}

	// Token: 0x06000DCF RID: 3535 RVA: 0x0004168C File Offset: 0x0003F88C
	[Client]
	private void RemoveTargetConditionBarSlot()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void ConditionModule::RemoveTargetConditionBarSlot()' called when client was not active");
			return;
		}
		GameObject target = this.uiSystemModule.CombatModule.Target;
		if (target == null)
		{
			return;
		}
		if (target.GetComponent<ConditionModule>().netId == base.netId)
		{
			int childCount = this.uiSystemModule.PlatformTargetConditionBar.transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				Transform child = this.uiSystemModule.PlatformTargetConditionBar.transform.GetChild(i);
				ConditionBarSlotManager component = child.GetComponent<ConditionBarSlotManager>();
				if (component != null && !this.HasCondition(component.Condition, false))
				{
					UnityEngine.Object.Destroy(child.gameObject);
				}
			}
		}
	}

	// Token: 0x06000DD0 RID: 3536 RVA: 0x00041744 File Offset: 0x0003F944
	[Client]
	private void DestroyTargetConditionsBarSlots()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void ConditionModule::DestroyTargetConditionsBarSlots()' called when client was not active");
			return;
		}
		GameObject target = this.uiSystemModule.CombatModule.Target;
		if (target == null)
		{
			return;
		}
		if (target.GetComponent<ConditionModule>().netId == base.netId)
		{
			int childCount = this.uiSystemModule.PlatformTargetConditionBar.transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				UnityEngine.Object.Destroy(this.uiSystemModule.PlatformTargetConditionBar.transform.GetChild(i).gameObject);
			}
		}
	}

	// Token: 0x06000DD1 RID: 3537 RVA: 0x000417D8 File Offset: 0x0003F9D8
	[Client]
	private void UpdateConditionBarSlot(int index)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void ConditionModule::UpdateConditionBarSlot(System.Int32)' called when client was not active");
			return;
		}
		GameObject conditionBarFromUiSystem = this.GetConditionBarFromUiSystem();
		Condition condition = this.Conditions[index];
		if (condition.IsDefined)
		{
			int childCount = conditionBarFromUiSystem.transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				ConditionBarSlotManager component = conditionBarFromUiSystem.transform.GetChild(i).GetComponent<ConditionBarSlotManager>();
				if (component != null && component.Condition.UniqueId == condition.UniqueId)
				{
					component.SetCondition(false, condition);
				}
			}
		}
	}

	// Token: 0x06000DD2 RID: 3538 RVA: 0x00041870 File Offset: 0x0003FA70
	[Client]
	private void UpdateTargetConditionBarSlot(int index)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void ConditionModule::UpdateTargetConditionBarSlot(System.Int32)' called when client was not active");
			return;
		}
		if (this.uiSystemModule.CombatModule.Target != null && this.uiSystemModule.CombatModule.Target.GetComponent<ConditionModule>().netId == base.netId)
		{
			Condition condition = this.Conditions[index];
			if (condition.IsDefined)
			{
				int childCount = this.uiSystemModule.PlatformTargetConditionBar.transform.childCount;
				for (int i = 0; i < childCount; i++)
				{
					ConditionBarSlotManager component = this.uiSystemModule.PlatformTargetConditionBar.transform.GetChild(i).GetComponent<ConditionBarSlotManager>();
					if (component != null && component.Condition.UniqueId == condition.UniqueId)
					{
						component.SetCondition(true, condition);
					}
				}
			}
		}
	}

	// Token: 0x06000DD3 RID: 3539 RVA: 0x00041950 File Offset: 0x0003FB50
	private GameObject GetConditionBarFromUiSystem()
	{
		GameObject result = this.uiSystemModule.ConditionBar;
		if (GlobalSettings.IsMobilePlatform)
		{
			result = this.uiSystemModule.MobileConditionBar;
		}
		return result;
	}

	// Token: 0x06000DD4 RID: 3540 RVA: 0x00041980 File Offset: 0x0003FB80
	private void OnIsInvisible(bool oldValue, bool newValue)
	{
		this.NetworkIsInvisible = newValue;
		if (base.isLocalPlayer | this.trailRenderer == null)
		{
			return;
		}
		if (this.IsInvisible)
		{
			this.trailRenderer.enabled = false;
			this.trailRenderer.Clear();
			this.trailRenderer.time = 0f;
			GameObject[] array = GameObject.FindGameObjectsWithTag("Effect");
			for (int i = 0; i < array.Length; i++)
			{
				UnityEngine.Object.Destroy(array[i]);
			}
			return;
		}
		this.trailRenderer.enabled = true;
	}

	// Token: 0x06000DD5 RID: 3541 RVA: 0x00041A07 File Offset: 0x0003FC07
	public int GetGoodConditionsCount()
	{
		return (from c in this.Conditions
		where c.IsGoodCondition
		select c).Count<Condition>();
	}

	// Token: 0x06000DD6 RID: 3542 RVA: 0x00041A38 File Offset: 0x0003FC38
	public ConditionModule()
	{
		base.InitSyncObject(this.Conditions);
	}

	// Token: 0x06000DD8 RID: 3544 RVA: 0x0002FEC9 File Offset: 0x0002E0C9
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x1700017E RID: 382
	// (get) Token: 0x06000DD9 RID: 3545 RVA: 0x00041A94 File Offset: 0x0003FC94
	// (set) Token: 0x06000DDA RID: 3546 RVA: 0x00041AA7 File Offset: 0x0003FCA7
	public bool NetworkIsInvisible
	{
		get
		{
			return this.IsInvisible;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<bool>(value, ref this.IsInvisible, 1UL, new Action<bool, bool>(this.OnIsInvisible));
		}
	}

	// Token: 0x06000DDB RID: 3547 RVA: 0x00041ACC File Offset: 0x0003FCCC
	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteBool(this.IsInvisible);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1UL) != 0UL)
		{
			writer.WriteBool(this.IsInvisible);
		}
	}

	// Token: 0x06000DDC RID: 3548 RVA: 0x00041B24 File Offset: 0x0003FD24
	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			base.GeneratedSyncVarDeserialize<bool>(ref this.IsInvisible, new Action<bool, bool>(this.OnIsInvisible), reader.ReadBool());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 1L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<bool>(ref this.IsInvisible, new Action<bool, bool>(this.OnIsInvisible), reader.ReadBool());
		}
	}

	// Token: 0x04000E10 RID: 3600
	[SyncVar(hook = "OnIsInvisible")]
	public bool IsInvisible;

	// Token: 0x04000E11 RID: 3601
	[SerializeField]
	private GameObject conditionBarSlotPrefab;

	// Token: 0x04000E12 RID: 3602
	private Collider2D[] collectorColliders = new Collider2D[30];

	// Token: 0x04000E13 RID: 3603
	private Coroutine TelekinesisCoroutine;

	// Token: 0x04000E14 RID: 3604
	private PvpModule pvpModule;

	// Token: 0x04000E15 RID: 3605
	private EffectModule effectModule;

	// Token: 0x04000E16 RID: 3606
	private CombatModule combatModule;

	// Token: 0x04000E17 RID: 3607
	private TrailRenderer trailRenderer;

	// Token: 0x04000E18 RID: 3608
	private CreatureModule creatureModule;

	// Token: 0x04000E19 RID: 3609
	private MovementModule movementModule;

	// Token: 0x04000E1A RID: 3610
	private UISystemModule uiSystemModule;

	// Token: 0x04000E1B RID: 3611
	private AttributeModule attributeModule;

	// Token: 0x04000E1C RID: 3612
	private InventoryModule inventoryModule;

	// Token: 0x04000E1D RID: 3613
	private NonPlayerAttributeModule nonPlayerAttributeModule;

	// Token: 0x04000E1E RID: 3614
	public bool ShouldPersistData;

	// Token: 0x04000E1F RID: 3615
	public readonly SyncListCondition Conditions = new SyncListCondition();

	// Token: 0x020002EF RID: 751
	// (Invoke) Token: 0x06000DDE RID: 3550
	public delegate void OnConditionCreatedEventHandler(Condition condition);

	// Token: 0x020002F0 RID: 752
	// (Invoke) Token: 0x06000DE2 RID: 3554
	public delegate void OnConditionRemovedEventHandler(Condition condition);

	// Token: 0x020002F1 RID: 753
	// (Invoke) Token: 0x06000DE6 RID: 3558
	public delegate void OnConditionElapsedEventHandler(Condition condition);
}
