using System;

// Token: 0x020001B2 RID: 434
public class BaradonMagicCoreItem : ItemBase
{
	// Token: 0x060004F6 RID: 1270 RVA: 0x0001B684 File Offset: 0x00019884
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		PetModule petModule;
		itemBaseConfig.UserObject.TryGetComponent<PetModule>(out petModule);
		if (petModule.ActivePet != null)
		{
			petModule.ReleaseActivePet();
			return false;
		}
		base.StartCoroutine(itemBaseConfig, petModule.SummonPet(42, itemBaseConfig.Item.Id));
		return false;
	}
}
