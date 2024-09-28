using System;

// Token: 0x020004A3 RID: 1187
public class DefaultProjectile : ProjectileBase
{
	// Token: 0x06001AD9 RID: 6873 RVA: 0x00089A41 File Offset: 0x00087C41
	public override void Explode(ProjectileBaseConfig projectileBaseConfig)
	{
		projectileBaseConfig.ProjectileObject.GetComponent<EffectModule>().ShowAnimatedText("BOOOOOM!", 3, true, projectileBaseConfig.ProjectileObject.transform.position);
	}
}
