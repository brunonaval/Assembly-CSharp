using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

// Token: 0x020000E4 RID: 228
public static class DbWarehouse
{
	// Token: 0x0600024A RID: 586 RVA: 0x00010FCC File Offset: 0x0000F1CC
	public static Task<DataRow[]> GetItemsAsync(int serverId, int accountId)
	{
		DbWarehouse.<GetItemsAsync>d__0 <GetItemsAsync>d__;
		<GetItemsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow[]>.Create();
		<GetItemsAsync>d__.serverId = serverId;
		<GetItemsAsync>d__.accountId = accountId;
		<GetItemsAsync>d__.<>1__state = -1;
		<GetItemsAsync>d__.<>t__builder.Start<DbWarehouse.<GetItemsAsync>d__0>(ref <GetItemsAsync>d__);
		return <GetItemsAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600024B RID: 587 RVA: 0x00011018 File Offset: 0x0000F218
	public static Task<int> UpdateItemOnSlotAsync(int serverId, int accountId, int? ownerId, int slotPosition, int newItemId, int amount, int requiredLevel, int boostLevel, Rarity rarity)
	{
		DbWarehouse.<UpdateItemOnSlotAsync>d__1 <UpdateItemOnSlotAsync>d__;
		<UpdateItemOnSlotAsync>d__.<>t__builder = AsyncTaskMethodBuilder<int>.Create();
		<UpdateItemOnSlotAsync>d__.serverId = serverId;
		<UpdateItemOnSlotAsync>d__.accountId = accountId;
		<UpdateItemOnSlotAsync>d__.ownerId = ownerId;
		<UpdateItemOnSlotAsync>d__.slotPosition = slotPosition;
		<UpdateItemOnSlotAsync>d__.newItemId = newItemId;
		<UpdateItemOnSlotAsync>d__.amount = amount;
		<UpdateItemOnSlotAsync>d__.requiredLevel = requiredLevel;
		<UpdateItemOnSlotAsync>d__.boostLevel = boostLevel;
		<UpdateItemOnSlotAsync>d__.rarity = rarity;
		<UpdateItemOnSlotAsync>d__.<>1__state = -1;
		<UpdateItemOnSlotAsync>d__.<>t__builder.Start<DbWarehouse.<UpdateItemOnSlotAsync>d__1>(ref <UpdateItemOnSlotAsync>d__);
		return <UpdateItemOnSlotAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600024C RID: 588 RVA: 0x000110A0 File Offset: 0x0000F2A0
	public static Task InsertItemAsync(int serverId, int accountId, int? ownerId, int itemId, int slotPosition, int amount, int requiredLevel, int boostLevel, Rarity rarity)
	{
		DbWarehouse.<InsertItemAsync>d__2 <InsertItemAsync>d__;
		<InsertItemAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<InsertItemAsync>d__.serverId = serverId;
		<InsertItemAsync>d__.accountId = accountId;
		<InsertItemAsync>d__.ownerId = ownerId;
		<InsertItemAsync>d__.itemId = itemId;
		<InsertItemAsync>d__.slotPosition = slotPosition;
		<InsertItemAsync>d__.amount = amount;
		<InsertItemAsync>d__.requiredLevel = requiredLevel;
		<InsertItemAsync>d__.boostLevel = boostLevel;
		<InsertItemAsync>d__.rarity = rarity;
		<InsertItemAsync>d__.<>1__state = -1;
		<InsertItemAsync>d__.<>t__builder.Start<DbWarehouse.<InsertItemAsync>d__2>(ref <InsertItemAsync>d__);
		return <InsertItemAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600024D RID: 589 RVA: 0x00011128 File Offset: 0x0000F328
	public static Task RemoveItemFromSlotAsync(int serverId, int accountId, int slotPosition)
	{
		DbWarehouse.<RemoveItemFromSlotAsync>d__3 <RemoveItemFromSlotAsync>d__;
		<RemoveItemFromSlotAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<RemoveItemFromSlotAsync>d__.serverId = serverId;
		<RemoveItemFromSlotAsync>d__.accountId = accountId;
		<RemoveItemFromSlotAsync>d__.slotPosition = slotPosition;
		<RemoveItemFromSlotAsync>d__.<>1__state = -1;
		<RemoveItemFromSlotAsync>d__.<>t__builder.Start<DbWarehouse.<RemoveItemFromSlotAsync>d__3>(ref <RemoveItemFromSlotAsync>d__);
		return <RemoveItemFromSlotAsync>d__.<>t__builder.Task;
	}
}
