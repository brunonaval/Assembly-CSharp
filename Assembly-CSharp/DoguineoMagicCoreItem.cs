using System;

// Token: 0x020001B7 RID: 439
public class DoguineoMagicCoreItem : ItemBase
{
	// Token: 0x06000503 RID: 1283 RVA: 0x0001BC5C File Offset: 0x00019E5C
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		PetModule petModule;
		itemBaseConfig.UserObject.TryGetComponent<PetModule>(out petModule);
		if (petModule.ActivePet != null)
		{
			petModule.ReleaseActivePet();
			return false;
		}
		base.StartCoroutine(itemBaseConfig, petModule.SummonPet(45, itemBaseConfig.Item.Id));
		return false;
	}
}
