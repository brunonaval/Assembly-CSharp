using System;

// Token: 0x020001B3 RID: 435
public class BlasusMagicCoreItem : ItemBase
{
	// Token: 0x060004F8 RID: 1272 RVA: 0x0001B6D4 File Offset: 0x000198D4
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		PetModule petModule;
		itemBaseConfig.UserObject.TryGetComponent<PetModule>(out petModule);
		if (petModule.ActivePet != null)
		{
			petModule.ReleaseActivePet();
			return false;
		}
		base.StartCoroutine(itemBaseConfig, petModule.SummonPet(43, itemBaseConfig.Item.Id));
		return false;
	}
}
