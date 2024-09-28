using System;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

// Token: 0x0200043C RID: 1084
public class WalletModule : NetworkBehaviour
{
	// Token: 0x14000029 RID: 41
	// (add) Token: 0x06001845 RID: 6213 RVA: 0x0007A9BC File Offset: 0x00078BBC
	// (remove) Token: 0x06001846 RID: 6214 RVA: 0x0007A9F4 File Offset: 0x00078BF4
	public event WalletModule.OnGemAddedEventHandler OnGemAdded;

	// Token: 0x06001847 RID: 6215 RVA: 0x0007AA2C File Offset: 0x00078C2C
	[ServerCallback]
	public void AddGems(int amount, bool invokeEvents)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		if (!NetworkServer.active)
		{
			return;
		}
		if (this.Gems + amount < 0)
		{
			this.NetworkGems = 0;
		}
		else
		{
			this.NetworkGems = this.Gems + amount;
		}
		if (invokeEvents)
		{
			WalletModule.OnGemAddedEventHandler onGemAdded = this.OnGemAdded;
			if (onGemAdded == null)
			{
				return;
			}
			onGemAdded(this.Gems, amount);
		}
	}

	// Token: 0x06001848 RID: 6216 RVA: 0x0007AA88 File Offset: 0x00078C88
	[Server]
	public void SetGems(int amount)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void WalletModule::SetGems(System.Int32)' called when server was not active");
			return;
		}
		this.NetworkGems = amount;
	}

	// Token: 0x06001849 RID: 6217 RVA: 0x0007AAA6 File Offset: 0x00078CA6
	[Server]
	public void AddGoldCoins(int amount)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void WalletModule::AddGoldCoins(System.Int32)' called when server was not active");
			return;
		}
		if (this.GoldCoins + (long)amount < 0L)
		{
			this.NetworkGoldCoins = 0L;
			return;
		}
		this.NetworkGoldCoins = this.GoldCoins + (long)amount;
	}

	// Token: 0x0600184A RID: 6218 RVA: 0x0007AAE2 File Offset: 0x00078CE2
	[Server]
	public void SetGoldCoins(long amount)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void WalletModule::SetGoldCoins(System.Int64)' called when server was not active");
			return;
		}
		this.NetworkGoldCoins = amount;
	}

	// Token: 0x0600184C RID: 6220 RVA: 0x0002FEC9 File Offset: 0x0002E0C9
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x1700026C RID: 620
	// (get) Token: 0x0600184D RID: 6221 RVA: 0x0007AB00 File Offset: 0x00078D00
	// (set) Token: 0x0600184E RID: 6222 RVA: 0x0007AB13 File Offset: 0x00078D13
	public long NetworkGoldCoins
	{
		get
		{
			return this.GoldCoins;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<long>(value, ref this.GoldCoins, 1UL, null);
		}
	}

	// Token: 0x1700026D RID: 621
	// (get) Token: 0x0600184F RID: 6223 RVA: 0x0007AB30 File Offset: 0x00078D30
	// (set) Token: 0x06001850 RID: 6224 RVA: 0x0007AB43 File Offset: 0x00078D43
	public int NetworkGems
	{
		get
		{
			return this.Gems;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.Gems, 2UL, null);
		}
	}

	// Token: 0x06001851 RID: 6225 RVA: 0x0007AB60 File Offset: 0x00078D60
	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteLong(this.GoldCoins);
			writer.WriteInt(this.Gems);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1UL) != 0UL)
		{
			writer.WriteLong(this.GoldCoins);
		}
		if ((base.syncVarDirtyBits & 2UL) != 0UL)
		{
			writer.WriteInt(this.Gems);
		}
	}

	// Token: 0x06001852 RID: 6226 RVA: 0x0007ABE8 File Offset: 0x00078DE8
	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			base.GeneratedSyncVarDeserialize<long>(ref this.GoldCoins, null, reader.ReadLong());
			base.GeneratedSyncVarDeserialize<int>(ref this.Gems, null, reader.ReadInt());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 1L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<long>(ref this.GoldCoins, null, reader.ReadLong());
		}
		if ((num & 2L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.Gems, null, reader.ReadInt());
		}
	}

	// Token: 0x04001586 RID: 5510
	[SyncVar]
	public long GoldCoins;

	// Token: 0x04001587 RID: 5511
	[SyncVar]
	public int Gems;

	// Token: 0x0200043D RID: 1085
	// (Invoke) Token: 0x06001854 RID: 6228
	public delegate void OnGemAddedEventHandler(int totalGems, int amount);
}
