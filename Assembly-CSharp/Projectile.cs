using System;

// Token: 0x02000137 RID: 311
public struct Projectile
{
	// Token: 0x1700006F RID: 111
	// (get) Token: 0x06000343 RID: 835 RVA: 0x0001529E File Offset: 0x0001349E
	public bool IsDefined
	{
		get
		{
			return !string.IsNullOrEmpty(this.PrefabName);
		}
	}

	// Token: 0x06000344 RID: 836 RVA: 0x000152B0 File Offset: 0x000134B0
	public Projectile(string prefabName, string script, float speed, int maxTargets, float range, Effect explosionEffect)
	{
		this = new Projectile(prefabName, script, speed, maxTargets, range, explosionEffect, default(Effect), false, false);
	}

	// Token: 0x06000345 RID: 837 RVA: 0x000152D8 File Offset: 0x000134D8
	public Projectile(string prefabName, string script, float speed, int maxTargets, float range)
	{
		this = new Projectile(prefabName, script, speed, maxTargets, range, default(Effect), default(Effect), false, false);
	}

	// Token: 0x06000346 RID: 838 RVA: 0x00015308 File Offset: 0x00013508
	public Projectile(string prefabName, string script, float speed, int maxTargets, float range, Effect explosionEffect, Effect hitEffect, bool pull, bool knockback)
	{
		this.Pull = pull;
		this.Script = script;
		this.Speed = speed;
		this.Range = range;
		this.Knockback = knockback;
		this.MaxTargets = maxTargets;
		this.PrefabName = prefabName;
		this.HitEffect = hitEffect;
		this.ExplosionEffect = explosionEffect;
	}

	// Token: 0x04000642 RID: 1602
	public bool Pull;

	// Token: 0x04000643 RID: 1603
	public float Speed;

	// Token: 0x04000644 RID: 1604
	public float Range;

	// Token: 0x04000645 RID: 1605
	public int MaxTargets;

	// Token: 0x04000646 RID: 1606
	public bool Knockback;

	// Token: 0x04000647 RID: 1607
	public string Script;

	// Token: 0x04000648 RID: 1608
	public string PrefabName;

	// Token: 0x04000649 RID: 1609
	public Effect HitEffect;

	// Token: 0x0400064A RID: 1610
	public Effect ExplosionEffect;
}
