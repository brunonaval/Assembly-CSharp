using System;

// Token: 0x020001B5 RID: 437
public class CelestialEnergizedRuneItem : ItemBase
{
	// Token: 0x060004FE RID: 1278 RVA: 0x0001B9EC File Offset: 0x00019BEC
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		PetModule petModule;
		itemBaseConfig.UserObject.TryGetComponent<PetModule>(out petModule);
		if (petModule.ActivePet != null)
		{
			petModule.ReleaseActivePet();
			return false;
		}
		base.StartCoroutine(itemBaseConfig, petModule.SummonPet(55, itemBaseConfig.Item.Id));
		return false;
	}
}
