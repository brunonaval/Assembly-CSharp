using System;

// Token: 0x020001B8 RID: 440
public class DropineMagicCoreItem : ItemBase
{
	// Token: 0x06000505 RID: 1285 RVA: 0x0001BCAC File Offset: 0x00019EAC
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		PetModule petModule;
		itemBaseConfig.UserObject.TryGetComponent<PetModule>(out petModule);
		if (petModule.ActivePet != null)
		{
			petModule.ReleaseActivePet();
			return false;
		}
		base.StartCoroutine(itemBaseConfig, petModule.SummonPet(41, itemBaseConfig.Item.Id));
		return false;
	}
}
