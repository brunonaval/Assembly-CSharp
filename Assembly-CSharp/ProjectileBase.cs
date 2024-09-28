using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004B5 RID: 1205
public abstract class ProjectileBase : CombatBase
{
	// Token: 0x06001B13 RID: 6931
	public abstract void Explode(ProjectileBaseConfig projectileBaseConfig);

	// Token: 0x06001B14 RID: 6932 RVA: 0x0008A43C File Offset: 0x0008863C
	protected int CalculateDamageOnTarget(ProjectileBaseConfig projectileBaseConfig, out bool critical, out bool blocked, GameObject target)
	{
		int num = GlobalUtils.CalculateAndUseTvtDamageIfNeeded(target.GetComponent<CombatModule>().CalculateDamage(projectileBaseConfig.Skill.SkillPower, (float)projectileBaseConfig.Ammo.Attack, out critical, out blocked, projectileBaseConfig.AttackerObject), projectileBaseConfig.Skill.SkillPower, projectileBaseConfig.AttackerObject, target);
		if (projectileBaseConfig.Skill.DamageModifier != 0f)
		{
			num = Mathf.RoundToInt((float)num * projectileBaseConfig.Skill.DamageModifier);
		}
		return num;
	}

	// Token: 0x06001B15 RID: 6933 RVA: 0x0008A4B8 File Offset: 0x000886B8
	public void ExplodeAndDestroy(ProjectileBaseConfig projectileBaseConfig)
	{
		try
		{
			this.Explode(projectileBaseConfig);
		}
		finally
		{
			ProjectileModule projectileModule;
			projectileBaseConfig.ProjectileObject.TryGetComponent<ProjectileModule>(out projectileModule);
			projectileModule.UnSpawnAndResetProjectile();
		}
	}

	// Token: 0x06001B16 RID: 6934 RVA: 0x0008A4F4 File Offset: 0x000886F4
	protected List<GameObject> GetTargetsFromProjectile(ProjectileBaseConfig projectileBaseConfig)
	{
		Projectile projectile = projectileBaseConfig.Ammo.Projectile;
		CombatModule combatModule;
		projectileBaseConfig.AttackerObject.TryGetComponent<CombatModule>(out combatModule);
		GameObject selectedTarget = projectileBaseConfig.ProjectileTarget ?? combatModule.Target;
		return combatModule.TargetFinder.GetTargetsFromPosition(projectile.MaxTargets, projectile.Range, true, false, true, false, projectileBaseConfig.ProjectileObject.transform.position, projectileBaseConfig.AttackerObject, selectedTarget);
	}

	// Token: 0x06001B17 RID: 6935 RVA: 0x0008A560 File Offset: 0x00088760
	protected List<GameObject> GetTargetsFromSkill(ProjectileBaseConfig projectileBaseConfig)
	{
		Projectile projectile = projectileBaseConfig.Ammo.Projectile;
		CombatModule combatModule;
		projectileBaseConfig.AttackerObject.TryGetComponent<CombatModule>(out combatModule);
		GameObject selectedTarget = projectileBaseConfig.ProjectileTarget ?? combatModule.Target;
		return combatModule.TargetFinder.GetTargetsFromPosition(projectileBaseConfig.Skill.MaxTargets + combatModule.ExtraTargets, projectile.Range, true, false, true, false, projectileBaseConfig.ProjectileObject.transform.position, projectileBaseConfig.AttackerObject, selectedTarget);
	}

	// Token: 0x06001B18 RID: 6936 RVA: 0x0008A5D8 File Offset: 0x000887D8
	protected GameObject GetTargetFromSkill(ProjectileBaseConfig projectileBaseConfig)
	{
		List<GameObject> targetsFromSkill = this.GetTargetsFromSkill(projectileBaseConfig);
		if (targetsFromSkill.Count != 0)
		{
			return targetsFromSkill[0];
		}
		return null;
	}

	// Token: 0x06001B19 RID: 6937 RVA: 0x0008A600 File Offset: 0x00088800
	protected List<GameObject> GetFriendlyTargetsFromSkill(ProjectileBaseConfig projectileBaseConfig)
	{
		Projectile projectile = projectileBaseConfig.Ammo.Projectile;
		CombatModule combatModule;
		projectileBaseConfig.AttackerObject.TryGetComponent<CombatModule>(out combatModule);
		GameObject gameObject = projectileBaseConfig.ProjectileTarget ?? combatModule.Target;
		bool flag = combatModule.TargetFinder.IsFriendlyTargetValid(projectile.Range, false, false, false, projectileBaseConfig.ProjectileObject.transform.position, projectileBaseConfig.AttackerObject, gameObject);
		return combatModule.TargetFinder.GetFriendlyTargetsFromPosition(projectileBaseConfig.Skill.MaxTargets + combatModule.ExtraTargets, projectile.Range, false, false, true, false, projectileBaseConfig.ProjectileObject.transform.position, projectileBaseConfig.AttackerObject, flag ? gameObject : null);
	}

	// Token: 0x06001B1A RID: 6938 RVA: 0x0008A6AC File Offset: 0x000888AC
	protected GameObject GetFriendlyTargetFromSkill(ProjectileBaseConfig projectileBaseConfig)
	{
		List<GameObject> friendlyTargetsFromSkill = this.GetFriendlyTargetsFromSkill(projectileBaseConfig);
		if (friendlyTargetsFromSkill.Count != 0)
		{
			return friendlyTargetsFromSkill[0];
		}
		return null;
	}

	// Token: 0x06001B1B RID: 6939 RVA: 0x0008A6D4 File Offset: 0x000888D4
	protected void ApplyDamageAndIncomingConditions(ProjectileBaseConfig projectileBaseConfig, int damage, EffectConfig damageEffectConfig, GameObject target)
	{
		CombatModule component = target.GetComponent<CombatModule>();
		component.ApplyDamage(damage, damageEffectConfig, projectileBaseConfig.AttackerObject, false, new ConditionConfig[]
		{
			projectileBaseConfig.ProjectileCondition,
			projectileBaseConfig.SkillCondition
		});
		ProjectileBase.KnockbackTarget(projectileBaseConfig, target, component);
		ProjectileBase.PullTarget(projectileBaseConfig, target, component);
	}

	// Token: 0x06001B1C RID: 6940 RVA: 0x0008A72C File Offset: 0x0008892C
	private static void PullTarget(ProjectileBaseConfig projectileBaseConfig, GameObject target, CombatModule targetCombatModule)
	{
		if (target == null)
		{
			return;
		}
		if (projectileBaseConfig.ProjectileObject == null)
		{
			return;
		}
		if (!projectileBaseConfig.Ammo.Projectile.Pull)
		{
			return;
		}
		if (!GlobalUtils.IsClose(target.transform.position, projectileBaseConfig.ProjectileObject.transform.position, 0.16f))
		{
			return;
		}
		if (targetCombatModule.IsPvpProtected(projectileBaseConfig.AttackerObject))
		{
			return;
		}
		MovementModule component = target.GetComponent<MovementModule>();
		Vector3 position = projectileBaseConfig.ProjectileObject.transform.position - target.transform.position;
		component.KnockbackToPosition(position, 3f);
	}

	// Token: 0x06001B1D RID: 6941 RVA: 0x0008A7DC File Offset: 0x000889DC
	private static void KnockbackTarget(ProjectileBaseConfig projectileBaseConfig, GameObject target, CombatModule targetCombatModule)
	{
		if (!projectileBaseConfig.Ammo.Projectile.Knockback)
		{
			return;
		}
		if (targetCombatModule.IsPvpProtected(projectileBaseConfig.AttackerObject))
		{
			return;
		}
		Direction direction = GlobalUtils.FindTargetDirection(projectileBaseConfig.AttackerObject.transform.position, target.transform.position);
		target.GetComponent<MovementModule>().Knockback(direction, 3f);
	}

	// Token: 0x06001B1E RID: 6942 RVA: 0x0008A840 File Offset: 0x00088A40
	protected void ShowExplosionEffects(ProjectileBaseConfig projectileBaseConfig, GameObject target)
	{
		EffectConfig effectConfig = this.BuildExplosionEffectConfig(projectileBaseConfig, target);
		if (target != null)
		{
			EffectModule effectModule;
			target.TryGetComponent<EffectModule>(out effectModule);
			effectModule.ShowEffects(effectConfig);
			return;
		}
		if (projectileBaseConfig.AttackerObject != null)
		{
			EffectModule effectModule2;
			projectileBaseConfig.AttackerObject.TryGetComponent<EffectModule>(out effectModule2);
			effectModule2.ShowEffects(effectConfig);
			return;
		}
		if (projectileBaseConfig.ProjectileObject != null)
		{
			EffectModule effectModule3;
			projectileBaseConfig.ProjectileObject.TryGetComponent<EffectModule>(out effectModule3);
			effectModule3.ShowEffects(effectConfig);
			return;
		}
	}

	// Token: 0x06001B1F RID: 6943 RVA: 0x0008A8B8 File Offset: 0x00088AB8
	protected EffectConfig BuildExplosionEffectConfig(ProjectileBaseConfig projectileBaseConfig, GameObject target)
	{
		EffectConfig result = new EffectConfig
		{
			EffectName = projectileBaseConfig.Ammo.Projectile.ExplosionEffect.Name,
			EffectScaleModifier = projectileBaseConfig.Ammo.Projectile.ExplosionEffect.ScaleModifier,
			EffectSpeedModifier = projectileBaseConfig.Ammo.Projectile.ExplosionEffect.SpeedModifier,
			SoundEffectName = projectileBaseConfig.Ammo.ProjectileExplosionSoundEffectName
		};
		if (target != null)
		{
			result.Position = target.transform.position;
		}
		else if (projectileBaseConfig.ProjectileObject != null)
		{
			result.Position = projectileBaseConfig.ProjectileObject.transform.position;
		}
		return result;
	}

	// Token: 0x06001B20 RID: 6944 RVA: 0x0008A97C File Offset: 0x00088B7C
	protected EffectConfig BuildHitEffectConfig(ProjectileBaseConfig projectileBaseConfig, bool critical, bool blocked, int damage, int textColorId)
	{
		return base.BuildDamageEffectConfig(critical, blocked, projectileBaseConfig.Ammo.Projectile.HitEffect.Name, projectileBaseConfig.Ammo.Projectile.HitEffect.ScaleModifier, projectileBaseConfig.Ammo.Projectile.HitEffect.SpeedModifier, string.Empty, damage, textColorId);
	}
}
