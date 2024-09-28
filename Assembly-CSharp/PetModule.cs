using System;
using System.Collections;
using Mirror;
using UnityEngine;

// Token: 0x020003AC RID: 940
public class PetModule : MonoBehaviour
{
	// Token: 0x17000218 RID: 536
	// (get) Token: 0x0600139F RID: 5023 RVA: 0x00061105 File Offset: 0x0005F305
	// (set) Token: 0x060013A0 RID: 5024 RVA: 0x0006110D File Offset: 0x0005F30D
	public GameObject ActivePet { get; private set; }

	// Token: 0x17000219 RID: 537
	// (get) Token: 0x060013A1 RID: 5025 RVA: 0x00061116 File Offset: 0x0005F316
	// (set) Token: 0x060013A2 RID: 5026 RVA: 0x0006111E File Offset: 0x0005F31E
	public int PetItemId { get; private set; }

	// Token: 0x060013A3 RID: 5027 RVA: 0x00061128 File Offset: 0x0005F328
	[ServerCallback]
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		GameObject.FindGameObjectWithTag("DatabaseManager").TryGetComponent<NpcDatabaseModule>(out this.npcDatabaseModule);
		base.TryGetComponent<PvpModule>(out this.pvpModule);
		base.TryGetComponent<EffectModule>(out this.effectModule);
		base.TryGetComponent<InventoryModule>(out this.inventoryModule);
		base.InvokeRepeating("CheckIfPetIsAllowed", 3f, 1f);
	}

	// Token: 0x060013A4 RID: 5028 RVA: 0x00061192 File Offset: 0x0005F392
	private void CheckIfPetIsAllowed()
	{
		if (this.pvpModule.PvpEnabled)
		{
			this.ReleaseActivePet();
			return;
		}
		if (this.PetItemId != 0 && this.inventoryModule.GetAmount(this.PetItemId) < 1)
		{
			this.ReleaseActivePet();
			return;
		}
	}

	// Token: 0x060013A5 RID: 5029 RVA: 0x000611CB File Offset: 0x0005F3CB
	public IEnumerator SummonPet(int petNpcId, int petItemId)
	{
		Vector3 spawnPos = GlobalUtils.RandomCircle(base.transform.position, 0.32f);
		this.ShowThunderEffects(spawnPos);
		GameObject petObject = GlobalUtils.SpawnNpc(AssetBundleManager.Instance.NpcPrefab, this.npcDatabaseModule, petNpcId, base.transform.position);
		this.SetActivePet(petObject, petItemId);
		NpcModule petNpcModule;
		petObject.TryGetComponent<NpcModule>(out petNpcModule);
		petNpcModule.SetOwner(base.gameObject);
		yield return new WaitUntil(() => petNpcModule.IsLoaded);
		MovementModule movementModule;
		petObject.TryGetComponent<MovementModule>(out movementModule);
		movementModule.Teleport(spawnPos, default(Effect));
		NetworkServer.Spawn(petObject, null);
		yield break;
	}

	// Token: 0x060013A6 RID: 5030 RVA: 0x000611E8 File Offset: 0x0005F3E8
	private void SetActivePet(GameObject petObject, int petItemId)
	{
		if (petObject == null)
		{
			return;
		}
		this.ReleaseActivePet();
		this.ActivePet = petObject;
		this.PetItemId = petItemId;
	}

	// Token: 0x060013A7 RID: 5031 RVA: 0x00061208 File Offset: 0x0005F408
	public void ReleaseActivePet()
	{
		if (this.ActivePet == null)
		{
			return;
		}
		this.ShowThunderEffects(this.ActivePet.transform.position);
		NetworkServer.Destroy(this.ActivePet);
		this.PetItemId = 0;
	}

	// Token: 0x060013A8 RID: 5032 RVA: 0x00061244 File Offset: 0x0005F444
	private void ShowThunderEffects(Vector3 effectPosition)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "ThunderStrike",
			EffectScaleModifier = 1f,
			EffectSpeedModifier = 0.25f,
			SoundEffectName = "thunder",
			Position = effectPosition
		};
		this.effectModule.ShowEffects(effectConfig);
	}

	// Token: 0x04001217 RID: 4631
	private PvpModule pvpModule;

	// Token: 0x04001218 RID: 4632
	private EffectModule effectModule;

	// Token: 0x04001219 RID: 4633
	private InventoryModule inventoryModule;

	// Token: 0x0400121A RID: 4634
	private NpcDatabaseModule npcDatabaseModule;
}
