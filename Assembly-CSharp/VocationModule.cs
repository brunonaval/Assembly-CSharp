using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

// Token: 0x0200043B RID: 1083
public class VocationModule : NetworkBehaviour
{
	// Token: 0x0600183C RID: 6204 RVA: 0x0007A798 File Offset: 0x00078998
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("DatabaseManager");
		this.skillDatabaseModule = gameObject.GetComponent<SkillDatabaseModule>();
	}

	// Token: 0x0600183D RID: 6205 RVA: 0x0007A7C4 File Offset: 0x000789C4
	public ItemType[] AllowedItemTypes()
	{
		List<ItemType> list = new List<ItemType>
		{
			ItemType.BodySkin,
			ItemType.OutfitSkin,
			ItemType.Undefined
		};
		switch (this.Vocation)
		{
		case Vocation.Sentinel:
			list.Add(ItemType.MediumArmor);
			list.Add(ItemType.Arrow);
			list.Add(ItemType.Bow);
			list.Add(ItemType.BowSkin);
			break;
		case Vocation.Warrior:
			list.Add(ItemType.HeavyArmor);
			list.Add(ItemType.Spear);
			list.Add(ItemType.SpearSkin);
			break;
		case Vocation.Elementor:
			list.Add(ItemType.Grimoire);
			list.Add(ItemType.LightArmor);
			list.Add(ItemType.Staff);
			list.Add(ItemType.StaffSkin);
			break;
		case Vocation.Protector:
			list.Add(ItemType.HeavyArmor);
			list.Add(ItemType.Shield);
			list.Add(ItemType.Sword);
			list.Add(ItemType.SwordSkin);
			list.Add(ItemType.ShieldSkin);
			break;
		case Vocation.Lyrus:
			list.Add(ItemType.Grimoire);
			list.Add(ItemType.LightArmor);
			list.Add(ItemType.Staff);
			list.Add(ItemType.StaffSkin);
			break;
		}
		return list.ToArray();
	}

	// Token: 0x0600183E RID: 6206 RVA: 0x0007A8BA File Offset: 0x00078ABA
	[Server]
	public void SetVocation(Vocation vocation)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void VocationModule::SetVocation(Vocation)' called when server was not active");
			return;
		}
		this.NetworkVocation = vocation;
	}

	// Token: 0x06001840 RID: 6208 RVA: 0x0002FEC9 File Offset: 0x0002E0C9
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x1700026B RID: 619
	// (get) Token: 0x06001841 RID: 6209 RVA: 0x0007A8D8 File Offset: 0x00078AD8
	// (set) Token: 0x06001842 RID: 6210 RVA: 0x0007A8EB File Offset: 0x00078AEB
	public Vocation NetworkVocation
	{
		get
		{
			return this.Vocation;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<Vocation>(value, ref this.Vocation, 1UL, null);
		}
	}

	// Token: 0x06001843 RID: 6211 RVA: 0x0007A908 File Offset: 0x00078B08
	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			Mirror.GeneratedNetworkCode._Write_Vocation(writer, this.Vocation);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1UL) != 0UL)
		{
			Mirror.GeneratedNetworkCode._Write_Vocation(writer, this.Vocation);
		}
	}

	// Token: 0x06001844 RID: 6212 RVA: 0x0007A960 File Offset: 0x00078B60
	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			base.GeneratedSyncVarDeserialize<Vocation>(ref this.Vocation, null, Mirror.GeneratedNetworkCode._Read_Vocation(reader));
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 1L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<Vocation>(ref this.Vocation, null, Mirror.GeneratedNetworkCode._Read_Vocation(reader));
		}
	}

	// Token: 0x04001584 RID: 5508
	[SyncVar]
	public Vocation Vocation;

	// Token: 0x04001585 RID: 5509
	private SkillDatabaseModule skillDatabaseModule;
}
