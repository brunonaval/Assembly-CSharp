using System;

// Token: 0x020001BC RID: 444
public class SlurgMagicCoreItem : ItemBase
{
	// Token: 0x0600050D RID: 1293 RVA: 0x0001BDEC File Offset: 0x00019FEC
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		PetModule petModule;
		itemBaseConfig.UserObject.TryGetComponent<PetModule>(out petModule);
		if (petModule.ActivePet != null)
		{
			petModule.ReleaseActivePet();
			return false;
		}
		base.StartCoroutine(itemBaseConfig, petModule.SummonPet(39, itemBaseConfig.Item.Id));
		return false;
	}
}
