using System;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

// Token: 0x02000398 RID: 920
public class NonPlayerEquipmentModule : NetworkBehaviour
{
	// Token: 0x060012EE RID: 4846 RVA: 0x0005D188 File Offset: 0x0005B388
	private void Awake()
	{
		if (NetworkServer.active)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("DatabaseManager");
			this.itemDatabaseModule = gameObject.GetComponent<ItemDatabaseModule>();
			UnityEngine.Object.Destroy(this.skinHolder);
			this.skinHolder = null;
		}
	}

	// Token: 0x060012EF RID: 4847 RVA: 0x0005D1C5 File Offset: 0x0005B3C5
	[Server]
	public void EquipSkin(string skinMetaName)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void NonPlayerEquipmentModule::EquipSkin(System.String)' called when server was not active");
			return;
		}
		if (string.IsNullOrEmpty(skinMetaName))
		{
			return;
		}
		this.NetworkequippedSkinMetaName = skinMetaName;
	}

	// Token: 0x060012F0 RID: 4848 RVA: 0x0005D1EC File Offset: 0x0005B3EC
	public void OnEquippedSkinMetaName(string oldValue, string newValue)
	{
		this.NetworkequippedSkinMetaName = newValue;
		if (string.IsNullOrEmpty(this.equippedSkinMetaName))
		{
			return;
		}
		this.AddSkinAnimation(this.equippedSkinMetaName);
	}

	// Token: 0x060012F1 RID: 4849 RVA: 0x0005D20F File Offset: 0x0005B40F
	[Client]
	private void AddSkinAnimation(Item skin)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void NonPlayerEquipmentModule::AddSkinAnimation(Item)' called when client was not active");
			return;
		}
		this.AddSkinAnimation(skin.MetaName);
	}

	// Token: 0x060012F2 RID: 4850 RVA: 0x0005D234 File Offset: 0x0005B434
	[Client]
	private void AddSkinAnimation(string skinMetaName)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void NonPlayerEquipmentModule::AddSkinAnimation(System.String)' called when client was not active");
			return;
		}
		GameObject animationPrefab = AssetBundleManager.Instance.GetAnimationPrefab(skinMetaName);
		if (animationPrefab == null)
		{
			return;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(animationPrefab);
		gameObject.transform.SetParent(this.skinHolder.transform, false);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.layer = LayerMask.NameToLayer("AnimationSlot");
	}

	// Token: 0x060012F4 RID: 4852 RVA: 0x0002FEC9 File Offset: 0x0002E0C9
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x1700020A RID: 522
	// (get) Token: 0x060012F5 RID: 4853 RVA: 0x0005D2A8 File Offset: 0x0005B4A8
	// (set) Token: 0x060012F6 RID: 4854 RVA: 0x0005D2BB File Offset: 0x0005B4BB
	public string NetworkequippedSkinMetaName
	{
		get
		{
			return this.equippedSkinMetaName;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<string>(value, ref this.equippedSkinMetaName, 1UL, new Action<string, string>(this.OnEquippedSkinMetaName));
		}
	}

	// Token: 0x060012F7 RID: 4855 RVA: 0x0005D2E0 File Offset: 0x0005B4E0
	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteString(this.equippedSkinMetaName);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1UL) != 0UL)
		{
			writer.WriteString(this.equippedSkinMetaName);
		}
	}

	// Token: 0x060012F8 RID: 4856 RVA: 0x0005D338 File Offset: 0x0005B538
	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			base.GeneratedSyncVarDeserialize<string>(ref this.equippedSkinMetaName, new Action<string, string>(this.OnEquippedSkinMetaName), reader.ReadString());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 1L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<string>(ref this.equippedSkinMetaName, new Action<string, string>(this.OnEquippedSkinMetaName), reader.ReadString());
		}
	}

	// Token: 0x040011A6 RID: 4518
	[SyncVar(hook = "OnEquippedSkinMetaName")]
	private string equippedSkinMetaName;

	// Token: 0x040011A7 RID: 4519
	[SerializeField]
	private GameObject skinHolder;

	// Token: 0x040011A8 RID: 4520
	private ItemDatabaseModule itemDatabaseModule;
}
