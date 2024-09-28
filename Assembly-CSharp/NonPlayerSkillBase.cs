using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

// Token: 0x0200052A RID: 1322
public abstract class NonPlayerSkillBase : CombatBase
{
	// Token: 0x06001D2D RID: 7469 RVA: 0x00092836 File Offset: 0x00090A36
	static NonPlayerSkillBase()
	{
		GameObject.FindGameObjectWithTag("DatabaseManager").TryGetComponent<ItemDatabaseModule>(out NonPlayerSkillBase.ItemDatabaseModule);
	}

	// Token: 0x06001D2E RID: 7470
	public abstract void Cast(SkillBaseConfig skillBaseConfig);

	// Token: 0x06001D2F RID: 7471 RVA: 0x00092850 File Offset: 0x00090A50
	protected int CalculateDamageOnTarget(SkillBaseConfig skillBaseConfig, out bool critical, out bool blocked, GameObject target)
	{
		CombatModule combatModule;
		target.TryGetComponent<CombatModule>(out combatModule);
		return combatModule.CalculateDamage(skillBaseConfig.Skill.SkillPower, 0f, out critical, out blocked, skillBaseConfig.CasterObject);
	}

	// Token: 0x06001D30 RID: 7472 RVA: 0x00092888 File Offset: 0x00090A88
	protected List<GameObject> GetFriendlyTargets(SkillBaseConfig skillBaseConfig, bool visibleOnly)
	{
		CombatModule combatModule;
		skillBaseConfig.CasterObject.TryGetComponent<CombatModule>(out combatModule);
		return combatModule.TargetFinder.GetFriendlyTargets(skillBaseConfig.Skill.MaxTargets, skillBaseConfig.Skill.Range, false, visibleOnly, true, false, skillBaseConfig.CasterObject, null);
	}

	// Token: 0x06001D31 RID: 7473 RVA: 0x000928D0 File Offset: 0x00090AD0
	protected GameObject GetFriendlyTarget(SkillBaseConfig skillBaseConfig, bool visibleOnly)
	{
		List<GameObject> friendlyTargets = this.GetFriendlyTargets(skillBaseConfig, visibleOnly);
		if (friendlyTargets.Count != 0)
		{
			return friendlyTargets[0];
		}
		return null;
	}

	// Token: 0x06001D32 RID: 7474 RVA: 0x000928F8 File Offset: 0x00090AF8
	protected GameObject GetTarget(SkillBaseConfig skillBaseConfig, bool visibleOnly)
	{
		List<GameObject> targets = this.GetTargets(skillBaseConfig, visibleOnly);
		if (targets.Count != 0)
		{
			return targets[0];
		}
		return null;
	}

	// Token: 0x06001D33 RID: 7475 RVA: 0x00092920 File Offset: 0x00090B20
	protected List<GameObject> GetTargets(SkillBaseConfig skillBaseConfig, bool visibleOnly)
	{
		GameObject selectedTarget = this.GetSelectedTarget(skillBaseConfig, visibleOnly);
		CombatModule combatModule;
		skillBaseConfig.CasterObject.TryGetComponent<CombatModule>(out combatModule);
		return combatModule.TargetFinder.GetTargets(skillBaseConfig.Skill.MaxTargets, skillBaseConfig.Skill.Range, skillBaseConfig.Skill.CanCauseDamage, visibleOnly, true, false, skillBaseConfig.CasterObject, selectedTarget);
	}

	// Token: 0x06001D34 RID: 7476 RVA: 0x0009297C File Offset: 0x00090B7C
	protected GameObject GetSelectedTarget(SkillBaseConfig skillBaseConfig, bool visibleOnly)
	{
		if (skillBaseConfig.CasterObject == null)
		{
			return null;
		}
		CombatModule combatModule;
		skillBaseConfig.CasterObject.TryGetComponent<CombatModule>(out combatModule);
		GameObject target = combatModule.Target;
		if (combatModule.TargetFinder.IsTargetValid(skillBaseConfig.Skill.Range, skillBaseConfig.Skill.CanCauseDamage, visibleOnly, true, false, skillBaseConfig.CasterObject.transform.position, skillBaseConfig.CasterObject, target))
		{
			return target;
		}
		return null;
	}

	// Token: 0x06001D35 RID: 7477 RVA: 0x000929EF File Offset: 0x00090BEF
	protected List<GameObject> GetTargetsFromPosition(SkillBaseConfig skillBaseConfig, Vector3 position, bool visibleOnly)
	{
		return this.GetTargetsFromPosition(skillBaseConfig, position, skillBaseConfig.Skill.Range, visibleOnly);
	}

	// Token: 0x06001D36 RID: 7478 RVA: 0x00092A08 File Offset: 0x00090C08
	protected List<GameObject> GetTargetsFromPosition(SkillBaseConfig skillBaseConfig, Vector3 position, float range, bool visibleOnly)
	{
		GameObject selectedTarget = this.GetSelectedTarget(skillBaseConfig, visibleOnly);
		CombatModule combatModule;
		skillBaseConfig.CasterObject.TryGetComponent<CombatModule>(out combatModule);
		return combatModule.TargetFinder.GetTargetsFromPosition(skillBaseConfig.Skill.MaxTargets, range, skillBaseConfig.Skill.CanCauseDamage, visibleOnly, true, false, position, skillBaseConfig.CasterObject, selectedTarget);
	}

	// Token: 0x06001D37 RID: 7479 RVA: 0x00092A5C File Offset: 0x00090C5C
	protected EffectConfig BuildDamageEffectConfig(SkillBaseConfig skillBaseConfig, bool critical, bool blocked, int damage, int textColorId)
	{
		return base.BuildDamageEffectConfig(critical, blocked, skillBaseConfig.Skill.TargetEffect.Name, skillBaseConfig.Skill.TargetEffect.ScaleModifier, skillBaseConfig.Skill.TargetEffect.SpeedModifier, skillBaseConfig.Skill.TargetSoundEffectName, damage, textColorId);
	}

	// Token: 0x06001D38 RID: 7480 RVA: 0x00092AB4 File Offset: 0x00090CB4
	protected void RunCastingAnimation(SkillBaseConfig skillBaseConfig, AnimationConfig animationConfig, GameObject target)
	{
		this.ShowCasterEffects(skillBaseConfig);
		AnimationControllerModule animationControllerModule;
		skillBaseConfig.CasterObject.TryGetComponent<AnimationControllerModule>(out animationControllerModule);
		MovementModule movementModule;
		skillBaseConfig.CasterObject.TryGetComponent<MovementModule>(out movementModule);
		animationControllerModule.RunAnimation(skillBaseConfig.Skill.AnimationType, animationConfig, movementModule.Direction);
	}

	// Token: 0x06001D39 RID: 7481 RVA: 0x00092B00 File Offset: 0x00090D00
	private void ShowCasterEffects(SkillBaseConfig skillBaseConfig)
	{
		if (!skillBaseConfig.Skill.CasterEffect.IsDefined && string.IsNullOrEmpty(skillBaseConfig.Skill.CasterSoundEffectName))
		{
			return;
		}
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = skillBaseConfig.Skill.CasterEffect.Name,
			EffectScaleModifier = skillBaseConfig.Skill.CasterEffect.ScaleModifier,
			EffectSpeedModifier = skillBaseConfig.Skill.CasterEffect.SpeedModifier,
			SoundEffectName = skillBaseConfig.Skill.CasterSoundEffectName
		};
		EffectModule effectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowEffects(effectConfig);
	}

	// Token: 0x06001D3A RID: 7482 RVA: 0x00092BB4 File Offset: 0x00090DB4
	protected void Shoot(SkillBaseConfig skillBaseConfig, ShootConfig shootConfig)
	{
		if (skillBaseConfig.CasterObject == null)
		{
			return;
		}
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
		MovementModule movementModule;
		skillBaseConfig.CasterObject.TryGetComponent<MovementModule>(out movementModule);
		Vector3 a = GlobalUtils.DirectionToPosition(movementModule.Direction);
		projectileConfig.Velocity = a * shootConfig.Ammo.Projectile.Speed;
		GameObject gameObject = NonPlayerSkillBase.InstantiateProjectile(false, shootConfig, projectileConfig, movementModule);
		if (gameObject == null)
		{
			return;
		}
		NetworkServer.Spawn(gameObject, null);
	}

	// Token: 0x06001D3B RID: 7483 RVA: 0x00092CD8 File Offset: 0x00090ED8
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

	// Token: 0x06001D3C RID: 7484 RVA: 0x00092D70 File Offset: 0x00090F70
	protected Coroutine StartCoroutine(SkillBaseConfig skillBaseConfig, IEnumerator routine)
	{
		NonPlayerSkillModule nonPlayerSkillModule;
		skillBaseConfig.CasterObject.TryGetComponent<NonPlayerSkillModule>(out nonPlayerSkillModule);
		return nonPlayerSkillModule.StartCoroutine(routine);
	}

	// Token: 0x04001774 RID: 6004
	protected static readonly ItemDatabaseModule ItemDatabaseModule;
}
