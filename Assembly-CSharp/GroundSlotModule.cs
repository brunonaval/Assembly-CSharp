using System;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

// Token: 0x02000331 RID: 817
public class GroundSlotModule : NetworkBehaviour
{
	// Token: 0x06000FDC RID: 4060 RVA: 0x0004A4B0 File Offset: 0x000486B0
	[ClientCallback]
	private void Update()
	{
		if (!NetworkClient.active)
		{
			return;
		}
		if (!NetworkClient.active)
		{
			return;
		}
		if (Time.time - this.effectTime > 0.5f)
		{
			this.effectTime = Time.time;
			if (this.ItemRarity == Rarity.Divine)
			{
				this.effectModule.ShowVisualEffect("RedSparks", 0.65f, 1f, 0f, base.transform.position);
				return;
			}
			if (this.ItemRarity == Rarity.Legendary)
			{
				this.effectModule.ShowVisualEffect("PurpleSparks", 0.65f, 1f, 0f, base.transform.position);
				return;
			}
			if (this.ItemRarity == Rarity.Exotic)
			{
				this.effectModule.ShowVisualEffect("OrangeSparks", 0.65f, 1f, 0f, base.transform.position);
				return;
			}
			if (this.ItemRarity == Rarity.Rare)
			{
				this.effectModule.ShowVisualEffect("GreenSparks", 0.65f, 1f, 0f, base.transform.position);
				return;
			}
		}
	}

	// Token: 0x06000FDD RID: 4061 RVA: 0x0004A5C1 File Offset: 0x000487C1
	public override void OnStartClient()
	{
		base.GetComponent<SpriteRenderer>().sprite = (string.IsNullOrEmpty(this.ItemMetaName) ? AssetBundleManager.Instance.GetItemIconSprite("undefined_icon") : AssetBundleManager.Instance.GetItemIconSprite(this.ItemMetaName));
	}

	// Token: 0x06000FDE RID: 4062 RVA: 0x0004A5FC File Offset: 0x000487FC
	public override void OnStopServer()
	{
		this.ResetGroundSlot();
	}

	// Token: 0x06000FDF RID: 4063 RVA: 0x0004A604 File Offset: 0x00048804
	[Server]
	public void Initialize(Item item, GameObject owner, float duration)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void GroundSlotModule::Initialize(Item,UnityEngine.GameObject,System.Single)' called when server was not active");
			return;
		}
		if (this.Item.IsDefined)
		{
			return;
		}
		this.Item = item;
		this.Owner = owner;
		this.NetworkItemMetaName = item.MetaName;
		this.NetworkItemRarity = item.Rarity;
		base.Invoke("RemoveOwner", 10f);
		base.Invoke("RemoveGroundSlot", duration);
	}

	// Token: 0x06000FE0 RID: 4064 RVA: 0x0004A678 File Offset: 0x00048878
	[Server]
	private void ResetGroundSlot()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void GroundSlotModule::ResetGroundSlot()' called when server was not active");
			return;
		}
		base.CancelInvoke("RemoveOwner");
		base.CancelInvoke("RemoveGroundSlot");
		base.gameObject.SetActive(false);
		this.Item = default(Item);
		this.NetworkItemMetaName = null;
		this.NetworkItemRarity = Rarity.Undefined;
		this.Owner = null;
		this.Collected = false;
	}

	// Token: 0x06000FE1 RID: 4065 RVA: 0x0004A6E4 File Offset: 0x000488E4
	private void RemoveOwner()
	{
		this.Owner = null;
	}

	// Token: 0x06000FE2 RID: 4066 RVA: 0x0004A6ED File Offset: 0x000488ED
	[Server]
	public void RemoveGroundSlot()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void GroundSlotModule::RemoveGroundSlot()' called when server was not active");
			return;
		}
		NetworkServer.UnSpawn(base.gameObject);
	}

	// Token: 0x06000FE4 RID: 4068 RVA: 0x0002FEC9 File Offset: 0x0002E0C9
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x170001CB RID: 459
	// (get) Token: 0x06000FE5 RID: 4069 RVA: 0x0004A710 File Offset: 0x00048910
	// (set) Token: 0x06000FE6 RID: 4070 RVA: 0x0004A723 File Offset: 0x00048923
	public Rarity NetworkItemRarity
	{
		get
		{
			return this.ItemRarity;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<Rarity>(value, ref this.ItemRarity, 1UL, null);
		}
	}

	// Token: 0x170001CC RID: 460
	// (get) Token: 0x06000FE7 RID: 4071 RVA: 0x0004A740 File Offset: 0x00048940
	// (set) Token: 0x06000FE8 RID: 4072 RVA: 0x0004A753 File Offset: 0x00048953
	public string NetworkItemMetaName
	{
		get
		{
			return this.ItemMetaName;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<string>(value, ref this.ItemMetaName, 2UL, null);
		}
	}

	// Token: 0x06000FE9 RID: 4073 RVA: 0x0004A770 File Offset: 0x00048970
	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			Mirror.GeneratedNetworkCode._Write_Rarity(writer, this.ItemRarity);
			writer.WriteString(this.ItemMetaName);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1UL) != 0UL)
		{
			Mirror.GeneratedNetworkCode._Write_Rarity(writer, this.ItemRarity);
		}
		if ((base.syncVarDirtyBits & 2UL) != 0UL)
		{
			writer.WriteString(this.ItemMetaName);
		}
	}

	// Token: 0x06000FEA RID: 4074 RVA: 0x0004A7F8 File Offset: 0x000489F8
	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			base.GeneratedSyncVarDeserialize<Rarity>(ref this.ItemRarity, null, Mirror.GeneratedNetworkCode._Read_Rarity(reader));
			base.GeneratedSyncVarDeserialize<string>(ref this.ItemMetaName, null, reader.ReadString());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 1L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<Rarity>(ref this.ItemRarity, null, Mirror.GeneratedNetworkCode._Read_Rarity(reader));
		}
		if ((num & 2L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<string>(ref this.ItemMetaName, null, reader.ReadString());
		}
	}

	// Token: 0x04000F5B RID: 3931
	public Item Item;

	// Token: 0x04000F5C RID: 3932
	[SyncVar]
	private Rarity ItemRarity;

	// Token: 0x04000F5D RID: 3933
	[SyncVar]
	private string ItemMetaName;

	// Token: 0x04000F5E RID: 3934
	[SerializeField]
	private EffectModule effectModule;

	// Token: 0x04000F5F RID: 3935
	public bool Collected;

	// Token: 0x04000F60 RID: 3936
	public GameObject Owner;

	// Token: 0x04000F61 RID: 3937
	private float effectTime;
}
