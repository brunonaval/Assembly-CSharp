using System;

// Token: 0x020001B9 RID: 441
public class EnergizedRuneItem : ItemBase
{
	// Token: 0x06000507 RID: 1287 RVA: 0x0001BCFC File Offset: 0x00019EFC
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		PetModule petModule;
		itemBaseConfig.UserObject.TryGetComponent<PetModule>(out petModule);
		if (petModule.ActivePet != null)
		{
			petModule.ReleaseActivePet();
			return false;
		}
		base.StartCoroutine(itemBaseConfig, petModule.SummonPet(44, itemBaseConfig.Item.Id));
		return false;
	}
}
