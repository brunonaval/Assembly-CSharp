using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

// Token: 0x02000548 RID: 1352
public abstract class SkillBase : CombatBase
{
	// Token: 0x06001DC8 RID: 7624 RVA: 0x00095057 File Offset: 0x00093257
	static SkillBase()
	{
		GameObject.FindGameObjectWithTag("DatabaseManager").TryGetComponent<ItemDatabaseModule>(out SkillBase.ItemDatabaseModule);
	}

	// Token: 0x06001DC9 RID: 7625
	public abstract void Cast(SkillBaseConfig skillBaseConfig);

	// Token: 0x06001DCA RID: 7626 RVA: 0x00095070 File Offset: 0x00093270
	protected int CalculateDamageOnTarget(SkillBaseConfig skillBaseConfig, out bool critical, out bool blocked, GameObject target)
	{
		CombatModule combatModule;
		target.TryGetComponent<CombatModule>(out combatModule);
		int num = GlobalUtils.CalculateAndUseTvtDamageIfNeeded(combatModule.CalculateDamage(skillBaseConfig.Skill.SkillPower, 0f, out critical, out blocked, skillBaseConfig.CasterObject), skillBaseConfig.Skill.SkillPower, skillBaseConfig.CasterObject, target);
		if (skillBaseConfig.Skill.DamageModifier != 0f)
		{
			num = Mathf.RoundToInt((float)num * skillBaseConfig.Skill.DamageModifier);
		}
		return num;
	}

	// Token: 0x06001DCB RID: 7627 RVA: 0x000950E8 File Offset: 0x000932E8
	protected List<GameObject> GetTargets(SkillBaseConfig skillBaseConfig, bool visibleOnly)
	{
		GameObject selectedTarget = this.GetSelectedTarget(skillBaseConfig, visibleOnly);
		CombatModule combatModule;
		skillBaseConfig.CasterObject.TryGetComponent<CombatModule>(out combatModule);
		return combatModule.TargetFinder.GetTargets(skillBaseConfig.Skill.MaxTargets + combatModule.ExtraTargets, skillBaseConfig.Skill.Range, skillBaseConfig.Skill.CanCauseDamage, visibleOnly, true, false, skillBaseConfig.CasterObject, selectedTarget);
	}

	// Token: 0x06001DCC RID: 7628 RVA: 0x0009514C File Offset: 0x0009334C
	protected List<GameObject> GetTargetsAtSameDirection(SkillBaseConfig skillBaseConfig, bool visibleOnly)
	{
		GameObject target = this.GetTarget(skillBaseConfig, visibleOnly);
		CombatModule combatModule;
		skillBaseConfig.CasterObject.TryGetComponent<CombatModule>(out combatModule);
		return combatModule.TargetFinder.GetTargets(skillBaseConfig.Skill.MaxTargets + combatModule.ExtraTargets, skillBaseConfig.Skill.Range, skillBaseConfig.Skill.CanCauseDamage, visibleOnly, true, true, skillBaseConfig.CasterObject, target);
	}

	// Token: 0x06001DCD RID: 7629 RVA: 0x000951AE File Offset: 0x000933AE
	protected List<GameObject> GetTargetsFromPosition(SkillBaseConfig skillBaseConfig, Vector3 position, bool visibleOnly)
	{
		return this.GetTargetsFromPosition(skillBaseConfig, position, skillBaseConfig.Skill.Range, visibleOnly);
	}

	// Token: 0x06001DCE RID: 7630 RVA: 0x000951C4 File Offset: 0x000933C4
	protected List<GameObject> GetTargetsFromPosition(SkillBaseConfig skillBaseConfig, Vector3 position, float range, bool visibleOnly)
	{
		GameObject selectedTarget = this.GetSelectedTarget(skillBaseConfig, visibleOnly);
		CombatModule combatModule;
		skillBaseConfig.CasterObject.TryGetComponent<CombatModule>(out combatModule);
		return combatModule.TargetFinder.GetTargetsFromPosition(skillBaseConfig.Skill.MaxTargets + combatModule.ExtraTargets, range, skillBaseConfig.Skill.CanCauseDamage, visibleOnly, true, false, position, skillBaseConfig.CasterObject, selectedTarget);
	}

	// Token: 0x06001DCF RID: 7631 RVA: 0x00095220 File Offset: 0x00093420
	protected GameObject GetFriendlyTarget(SkillBaseConfig skillBaseConfig, bool visibleOnly)
	{
		List<GameObject> friendlyTargets = this.GetFriendlyTargets(skillBaseConfig, visibleOnly);
		if (friendlyTargets.Count != 0)
		{
			return friendlyTargets[0];
		}
		return null;
	}

	// Token: 0x06001DD0 RID: 7632 RVA: 0x00095248 File Offset: 0x00093448
	protected GameObject GetFriendlyTargetAndAvoidCaster(SkillBaseConfig skillBaseConfig, bool visibleOnly)
	{
		List<GameObject> friendlyTargetsAndAvoidCaster = this.GetFriendlyTargetsAndAvoidCaster(skillBaseConfig, visibleOnly);
		if (friendlyTargetsAndAvoidCaster.Count != 0)
		{
			return friendlyTargetsAndAvoidCaster[0];
		}
		return null;
	}

	// Token: 0x06001DD1 RID: 7633 RVA: 0x00095270 File Offset: 0x00093470
	protected List<GameObject> GetFriendlyTargetsAndAvoidCaster(SkillBaseConfig skillBaseConfig, bool visibleOnly)
	{
		GameObject friendlySelectedTargetAndAvoidCaster = this.GetFriendlySelectedTargetAndAvoidCaster(skillBaseConfig, visibleOnly);
		CombatModule combatModule;
		skillBaseConfig.CasterObject.TryGetComponent<CombatModule>(out combatModule);
		return combatModule.TargetFinder.GetFriendlyTargets(skillBaseConfig.Skill.MaxTargets + combatModule.ExtraTargets, skillBaseConfig.Skill.Range, true, visibleOnly, true, false, skillBaseConfig.CasterObject, friendlySelectedTargetAndAvoidCaster);
	}

	// Token: 0x06001DD2 RID: 7634 RVA: 0x000952C8 File Offset: 0x000934C8
	protected List<GameObject> GetFriendlyTargets(SkillBaseConfig skillBaseConfig, bool visibleOnly)
	{
		GameObject friendlySelectedTarget = this.GetFriendlySelectedTarget(skillBaseConfig, visibleOnly);
		CombatModule combatModule;
		skillBaseConfig.CasterObject.TryGetComponent<CombatModule>(out combatModule);
		return combatModule.TargetFinder.GetFriendlyTargets(skillBaseConfig.Skill.MaxTargets + combatModule.ExtraTargets, skillBaseConfig.Skill.Range, false, visibleOnly, true, false, skillBaseConfig.CasterObject, friendlySelectedTarget);
	}

	// Token: 0x06001DD3 RID: 7635 RVA: 0x00095320 File Offset: 0x00093520
	protected List<GameObject> GetFriendlyTargetsFromPosition(SkillBaseConfig skillBaseConfig, Vector3 position, float range, bool visibleOnly)
	{
		GameObject friendlySelectedTarget = this.GetFriendlySelectedTarget(skillBaseConfig, visibleOnly);
		CombatModule combatModule;
		skillBaseConfig.CasterObject.TryGetComponent<CombatModule>(out combatModule);
		return combatModule.TargetFinder.GetFriendlyTargetsFromPosition(skillBaseConfig.Skill.MaxTargets + combatModule.ExtraTargets, range, false, visibleOnly, true, false, position, skillBaseConfig.CasterObject, friendlySelectedTarget);
	}

	// Token: 0x06001DD4 RID: 7636 RVA: 0x00095370 File Offset: 0x00093570
	protected GameObject GetTarget(SkillBaseConfig skillBaseConfig, bool visibleOnly)
	{
		List<GameObject> targets = this.GetTargets(skillBaseConfig, visibleOnly);
		if (targets.Count != 0)
		{
			return targets[0];
		}
		return null;
	}

	// Token: 0x06001DD5 RID: 7637 RVA: 0x00095398 File Offset: 0x00093598
	protected GameObject GetSelectedTarget(SkillBaseConfig skillBaseConfig, bool visibleOnly)
	{
		CombatModule combatModule;
		skillBaseConfig.CasterObject.TryGetComponent<CombatModule>(out combatModule);
		GameObject target = combatModule.Target;
		if (combatModule.TargetFinder.IsTargetValid(skillBaseConfig.Skill.Range, skillBaseConfig.Skill.CanCauseDamage, visibleOnly, true, false, skillBaseConfig.CasterObject.transform.position, skillBaseConfig.CasterObject, target))
		{
			return target;
		}
		return null;
	}

	// Token: 0x06001DD6 RID: 7638 RVA: 0x000953FC File Offset: 0x000935FC
	protected GameObject GetDeadFriendlySelectedTarget(SkillBaseConfig skillBaseConfig)
	{
		CombatModule combatModule;
		skillBaseConfig.CasterObject.TryGetComponent<CombatModule>(out combatModule);
		GameObject target = combatModule.Target;
		if (combatModule.TargetFinder.IsDeadFriendlyTargetValid(skillBaseConfig.Skill.Range, skillBaseConfig.CasterObject.transform.position, skillBaseConfig.CasterObject, target))
		{
			return target;
		}
		return null;
	}

	// Token: 0x06001DD7 RID: 7639 RVA: 0x00095450 File Offset: 0x00093650
	protected GameObject GetFriendlySelectedTargetAndAvoidCaster(SkillBaseConfig skillBaseConfig, bool visibleOnly)
	{
		CombatModule combatModule;
		skillBaseConfig.CasterObject.TryGetComponent<CombatModule>(out combatModule);
		GameObject target = combatModule.Target;
		if (combatModule.TargetFinder.IsFriendlyTargetValid(skillBaseConfig.Skill.Range, false, true, visibleOnly, skillBaseConfig.CasterObject.transform.position, skillBaseConfig.CasterObject, target))
		{
			return target;
		}
		return null;
	}

	// Token: 0x06001DD8 RID: 7640 RVA: 0x000954A8 File Offset: 0x000936A8
	protected GameObject GetFriendlySelectedTarget(SkillBaseConfig skillBaseConfig, bool visibleOnly)
	{
		CombatModule combatModule;
		skillBaseConfig.CasterObject.TryGetComponent<CombatModule>(out combatModule);
		GameObject target = combatModule.Target;
		if (combatModule.TargetFinder.IsFriendlyTargetValid(skillBaseConfig.Skill.Range, false, false, visibleOnly, skillBaseConfig.CasterObject.transform.position, skillBaseConfig.CasterObject, target))
		{
			return target;
		}
		return null;
	}

	// Token: 0x06001DD9 RID: 7641 RVA: 0x000954FF File Offset: 0x000936FF
	protected void StartCastingAnimation(SkillBaseConfig skillBaseConfig, AnimationConfig animationConfig, GameObject target)
	{
		this.StartCastingAnimation(skillBaseConfig, true, animationConfig, target);
	}

	// Token: 0x06001DDA RID: 7642 RVA: 0x0009550C File Offset: 0x0009370C
	protected void StartCastingAnimation(SkillBaseConfig skillBaseConfig, bool setLastUseTime, AnimationConfig animationConfig, GameObject target)
	{
		this.SelectIncomingTarget(skillBaseConfig, target);
		Direction direction = this.FollowTargetDirection(skillBaseConfig, target);
		this.StartCastingCondition(skillBaseConfig);
		AnimationType animationType = this.GetAnimationType(skillBaseConfig);
		this.DefineShootPivot(skillBaseConfig, animationType);
		AnimationControllerModule animationControllerModule;
		skillBaseConfig.CasterObject.TryGetComponent<AnimationControllerModule>(out animationControllerModule);
		animationControllerModule.RunAnimation(animationType, animationConfig, direction);
		this.SetCasterDirectionIfNeeded(skillBaseConfig, direction);
		if (setLastUseTime)
		{
			SkillModule skillModule;
			skillBaseConfig.CasterObject.TryGetComponent<SkillModule>(out skillModule);
			skillModule.SetLastUseTime(skillBaseConfig.Skill.Id, NetworkTime.time);
		}
	}

	// Token: 0x06001DDB RID: 7643 RVA: 0x0009558C File Offset: 0x0009378C
	private void DefineShootPivot(SkillBaseConfig skillBaseConfig, AnimationType animationType)
	{
		switch (animationType)
		{
		case AnimationType.Cast:
		case AnimationType.Thrust:
			skillBaseConfig.ShootPivot = new Vector3(0f, 0.13f, 0f);
			return;
		case AnimationType.Slash:
			break;
		case AnimationType.ShootArrow:
			skillBaseConfig.ShootPivot = new Vector3(0f, 0.23f, 0f);
			break;
		default:
			return;
		}
	}

	// Token: 0x06001DDC RID: 7644 RVA: 0x000955EC File Offset: 0x000937EC
	private AnimationType GetAnimationType(SkillBaseConfig skillBaseConfig)
	{
		if (skillBaseConfig.Skill.AnimationType == AnimationType.Cast)
		{
			return AnimationType.Cast;
		}
		EquipmentModule equipmentModule;
		skillBaseConfig.CasterObject.TryGetComponent<EquipmentModule>(out equipmentModule);
		Item item = equipmentModule.GetItem(SlotType.RightHandSkin);
		if (item.IsDefined & item.WeaponSkinAnimationType > AnimationType.Undefined)
		{
			return item.WeaponSkinAnimationType;
		}
		Item item2 = equipmentModule.GetItem(SlotType.RightHandSkin);
		if (item2.IsDefined & item2.WeaponSkinAnimationType > AnimationType.Undefined)
		{
			return item2.WeaponSkinAnimationType;
		}
		return skillBaseConfig.Skill.AnimationType;
	}

	// Token: 0x06001DDD RID: 7645 RVA: 0x0009566C File Offset: 0x0009386C
	protected EffectConfig BuildDamageEffectConfig(SkillBaseConfig skillBaseConfig, bool critical, bool blocked, int damage, int textColorId)
	{
		return base.BuildDamageEffectConfig(critical, blocked, skillBaseConfig.Skill.TargetEffect.Name, skillBaseConfig.Skill.TargetEffect.ScaleModifier, skillBaseConfig.Skill.TargetEffect.SpeedModifier, skillBaseConfig.Skill.TargetSoundEffectName, damage, textColorId);
	}

	// Token: 0x06001DDE RID: 7646 RVA: 0x000956C4 File Offset: 0x000938C4
	protected void Shoot(SkillBaseConfig skillBaseConfig, ShootConfig shootConfig)
	{
		if (!shootConfig.Ammo.IsDefined)
		{
			return;
		}
		bool useAttackerAsStartPoint = false;
		if (shootConfig.FromPosition == Vector3.zero)
		{
			useAttackerAsStartPoint = true;
			shootConfig.FromPosition = skillBaseConfig.CasterObject.transform.position;
		}
		ProjectileConfig projectileConfig = new ProjectileConfig
		{
			Ammo = shootConfig.Ammo,
			Attacker = skillBaseConfig.CasterObject,
			Target = shootConfig.Target,
			Condition = shootConfig.Condition,
			ConditionChance = shootConfig.ConditionChance,
			Skill = shootConfig.Skill,
			StartPoint = shootConfig.FromPosition,
			ShootPivot = skillBaseConfig.ShootPivot,
			UseAttackerAsStartPoint = useAttackerAsStartPoint
		};
		MovementModule component = skillBaseConfig.CasterObject.GetComponent<MovementModule>();
		Vector3 a = GlobalUtils.DirectionToPosition(component.Direction);
		projectileConfig.Velocity = a * shootConfig.Ammo.Projectile.Speed;
		if (shootConfig.Target != null && GlobalUtils.IsClose(skillBaseConfig.CasterObject.transform.position, shootConfig.Target.transform.position, 0.64f))
		{
			GameObject gameObject = SkillBase.InstantiateProjectile(true, shootConfig, projectileConfig, component);
			if (gameObject == null)
			{
				return;
			}
			EffectModule effectModule;
			skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
			effectModule.PlaySoundEffect(projectileConfig.Ammo.ProjectileShootSoundEffectName, 1f, 0f, gameObject.transform.position);
			NetworkServer.Spawn(gameObject, null);
			return;
		}
		else
		{
			GameObject gameObject = SkillBase.InstantiateProjectile(false, shootConfig, projectileConfig, component);
			if (gameObject == null)
			{
				return;
			}
			NetworkServer.Spawn(gameObject, null);
			return;
		}
	}

	// Token: 0x06001DDF RID: 7647 RVA: 0x00095874 File Offset: 0x00093A74
	private static GameObject InstantiateProjectile(bool serverOnly, ShootConfig shootConfig, ProjectileConfig projectileConfig, MovementModule casterMovementModule)
	{
		if (string.IsNullOrEmpty(shootConfig.Ammo.Projectile.PrefabName))
		{
			return null;
		}
		GameObject gameObject;
		if (serverOnly)
		{
			gameObject = ObjectPoolModule.Instance.GetServerOnlyProjectileFromPool(shootConfig.FromPosition, shootConfig.Ammo.Projectile.PrefabName);
		}
		else
		{
			gameObject = ObjectPoolModule.Instance.GetProjectileFromPool(shootConfig.FromPosition, shootConfig.Ammo.Projectile.PrefabName);
		}
		gameObject.GetComponent<Rigidbody2D>().velocity = projectileConfig.Velocity;
		gameObject.GetComponent<ProjectileModule>().Config(projectileConfig, casterMovementModule.Direction);
		return gameObject;
	}

	// Token: 0x06001DE0 RID: 7648 RVA: 0x0009590A File Offset: 0x00093B0A
	protected Coroutine StartCoroutine(SkillBaseConfig skillBaseConfig, IEnumerator routine)
	{
		return skillBaseConfig.CasterObject.GetComponent<SkillModule>().StartCoroutine(routine);
	}

	// Token: 0x06001DE1 RID: 7649 RVA: 0x00095920 File Offset: 0x00093B20
	private void SetCasterDirectionIfNeeded(SkillBaseConfig skillBaseConfig, Direction animationDirection)
	{
		MovementModule movementModule;
		skillBaseConfig.CasterObject.TryGetComponent<MovementModule>(out movementModule);
		if (movementModule.Direction != animationDirection)
		{
			movementModule.SetDirection(animationDirection);
		}
	}

	// Token: 0x06001DE2 RID: 7650 RVA: 0x0009594C File Offset: 0x00093B4C
	private void SelectIncomingTarget(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		if (target == null)
		{
			return;
		}
		CombatModule combatModule;
		skillBaseConfig.CasterObject.TryGetComponent<CombatModule>(out combatModule);
		if (combatModule.Target != null)
		{
			CreatureModule creatureModule;
			combatModule.Target.TryGetComponent<CreatureModule>(out creatureModule);
			if (creatureModule.IsAlive)
			{
				return;
			}
		}
		combatModule.SetTarget(target, true);
	}

	// Token: 0x06001DE3 RID: 7651 RVA: 0x000959A0 File Offset: 0x00093BA0
	private void StartCastingCondition(SkillBaseConfig skillBaseConfig)
	{
		CombatModule combatModule;
		skillBaseConfig.CasterObject.TryGetComponent<CombatModule>(out combatModule);
		combatModule.StartCombatAndFightingFlags();
		EffectModule effectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowEffects(new EffectConfig
		{
			EffectName = skillBaseConfig.Skill.CasterEffect.Name,
			EffectScaleModifier = skillBaseConfig.Skill.CasterEffect.ScaleModifier,
			EffectSpeedModifier = skillBaseConfig.Skill.CasterEffect.SpeedModifier,
			SoundEffectName = skillBaseConfig.Skill.CasterSoundEffectName
		});
	}

	// Token: 0x06001DE4 RID: 7652 RVA: 0x00095A3C File Offset: 0x00093C3C
	private Direction FollowTargetDirection(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		MovementModule movementModule;
		skillBaseConfig.CasterObject.TryGetComponent<MovementModule>(out movementModule);
		Direction direction = movementModule.Direction;
		if (target == null || !movementModule.CanMove())
		{
			return direction;
		}
		direction = GlobalUtils.FindTargetDirection(skillBaseConfig.CasterObject.transform.position, target.transform.position);
		movementModule.SetDirection(direction);
		movementModule.TargetSetDirection(movementModule.connectionToClient, direction);
		return direction;
	}

	// Token: 0x040017A7 RID: 6055
	protected static readonly ItemDatabaseModule ItemDatabaseModule;
}
