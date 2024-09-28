using System;

// Token: 0x020001BA RID: 442
public class GeminiMagicCoreItem : ItemBase
{
	// Token: 0x06000509 RID: 1289 RVA: 0x0001BD4C File Offset: 0x00019F4C
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		PetModule petModule;
		itemBaseConfig.UserObject.TryGetComponent<PetModule>(out petModule);
		if (petModule.ActivePet != null)
		{
			petModule.ReleaseActivePet();
			return false;
		}
		base.StartCoroutine(itemBaseConfig, petModule.SummonPet(35, itemBaseConfig.Item.Id));
		return false;
	}
}
