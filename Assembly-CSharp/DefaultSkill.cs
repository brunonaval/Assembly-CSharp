using System;

// Token: 0x020004C2 RID: 1218
public class DefaultSkill : SkillBase
{
	// Token: 0x06001B54 RID: 6996 RVA: 0x0008B120 File Offset: 0x00089320
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		EffectModule effectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowAnimatedText("Default Skill (Undefined)", 3, true, skillBaseConfig.CasterObject.transform.position);
	}
}
