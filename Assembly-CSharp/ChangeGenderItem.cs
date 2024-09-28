using System;
using System.Runtime.CompilerServices;

// Token: 0x02000191 RID: 401
public class ChangeGenderItem : ItemBase
{
	// Token: 0x06000489 RID: 1161 RVA: 0x00019810 File Offset: 0x00017A10
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		CreatureModule creatureModule;
		itemBaseConfig.UserObject.TryGetComponent<CreatureModule>(out creatureModule);
		creatureModule.SetGender((creatureModule.Gender == CreatureGender.Male) ? CreatureGender.Female : CreatureGender.Male);
		ChangeGenderItem.ShowMagicEffects(itemBaseConfig);
		this.UpdateGenderOnDatabaseAsync(itemBaseConfig, creatureModule);
		return true;
	}

	// Token: 0x0600048A RID: 1162 RVA: 0x0001984C File Offset: 0x00017A4C
	private void UpdateGenderOnDatabaseAsync(ItemBaseConfig itemBaseConfig, CreatureModule creatureModule)
	{
		ChangeGenderItem.<UpdateGenderOnDatabaseAsync>d__1 <UpdateGenderOnDatabaseAsync>d__;
		<UpdateGenderOnDatabaseAsync>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<UpdateGenderOnDatabaseAsync>d__.itemBaseConfig = itemBaseConfig;
		<UpdateGenderOnDatabaseAsync>d__.creatureModule = creatureModule;
		<UpdateGenderOnDatabaseAsync>d__.<>1__state = -1;
		<UpdateGenderOnDatabaseAsync>d__.<>t__builder.Start<ChangeGenderItem.<UpdateGenderOnDatabaseAsync>d__1>(ref <UpdateGenderOnDatabaseAsync>d__);
	}

	// Token: 0x0600048B RID: 1163 RVA: 0x0001988C File Offset: 0x00017A8C
	private static void ShowMagicEffects(ItemBaseConfig itemBaseConfig)
	{
		EffectModule effectModule;
		itemBaseConfig.UserObject.TryGetComponent<EffectModule>(out effectModule);
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "Magic",
			EffectScaleModifier = 0.5f,
			EffectSpeedModifier = 0.25f,
			SoundEffectName = "spellbook"
		};
		effectModule.ShowEffects(effectConfig);
	}
}
