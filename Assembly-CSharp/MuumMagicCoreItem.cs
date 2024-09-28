using System;

// Token: 0x020001BB RID: 443
public class MuumMagicCoreItem : ItemBase
{
	// Token: 0x0600050B RID: 1291 RVA: 0x0001BD9C File Offset: 0x00019F9C
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		PetModule petModule;
		itemBaseConfig.UserObject.TryGetComponent<PetModule>(out petModule);
		if (petModule.ActivePet != null)
		{
			petModule.ReleaseActivePet();
			return false;
		}
		base.StartCoroutine(itemBaseConfig, petModule.SummonPet(40, itemBaseConfig.Item.Id));
		return false;
	}
}
