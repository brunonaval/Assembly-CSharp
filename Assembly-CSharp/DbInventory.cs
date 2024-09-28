using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

// Token: 0x02000072 RID: 114
public static class DbInventory
{
	// Token: 0x0600012A RID: 298 RVA: 0x000083A8 File Offset: 0x000065A8
	public static Task<DataRow[]> GetItemsAsync(int playerId)
	{
		DbInventory.<GetItemsAsync>d__0 <GetItemsAsync>d__;
		<GetItemsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow[]>.Create();
		<GetItemsAsync>d__.playerId = playerId;
		<GetItemsAsync>d__.<>1__state = -1;
		<GetItemsAsync>d__.<>t__builder.Start<DbInventory.<GetItemsAsync>d__0>(ref <GetItemsAsync>d__);
		return <GetItemsAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600012B RID: 299 RVA: 0x000083EC File Offset: 0x000065EC
	public static Task<int> UpdateItemOnSlotAsync(int playerId, int slotPosition, int newItemId, int amount, int requiredLevel, int boostLevel, Rarity rarity)
	{
		DbInventory.<UpdateItemOnSlotAsync>d__1 <UpdateItemOnSlotAsync>d__;
		<UpdateItemOnSlotAsync>d__.<>t__builder = AsyncTaskMethodBuilder<int>.Create();
		<UpdateItemOnSlotAsync>d__.playerId = playerId;
		<UpdateItemOnSlotAsync>d__.slotPosition = slotPosition;
		<UpdateItemOnSlotAsync>d__.newItemId = newItemId;
		<UpdateItemOnSlotAsync>d__.amount = amount;
		<UpdateItemOnSlotAsync>d__.requiredLevel = requiredLevel;
		<UpdateItemOnSlotAsync>d__.boostLevel = boostLevel;
		<UpdateItemOnSlotAsync>d__.rarity = rarity;
		<UpdateItemOnSlotAsync>d__.<>1__state = -1;
		<UpdateItemOnSlotAsync>d__.<>t__builder.Start<DbInventory.<UpdateItemOnSlotAsync>d__1>(ref <UpdateItemOnSlotAsync>d__);
		return <UpdateItemOnSlotAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600012C RID: 300 RVA: 0x00008464 File Offset: 0x00006664
	public static Task InsertItemAsync(int playerId, int itemId, int slotPosition, int amount, int requiredLevel, int boostLevel, Rarity rarity)
	{
		DbInventory.<InsertItemAsync>d__2 <InsertItemAsync>d__;
		<InsertItemAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<InsertItemAsync>d__.playerId = playerId;
		<InsertItemAsync>d__.itemId = itemId;
		<InsertItemAsync>d__.slotPosition = slotPosition;
		<InsertItemAsync>d__.amount = amount;
		<InsertItemAsync>d__.requiredLevel = requiredLevel;
		<InsertItemAsync>d__.boostLevel = boostLevel;
		<InsertItemAsync>d__.rarity = rarity;
		<InsertItemAsync>d__.<>1__state = -1;
		<InsertItemAsync>d__.<>t__builder.Start<DbInventory.<InsertItemAsync>d__2>(ref <InsertItemAsync>d__);
		return <InsertItemAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600012D RID: 301 RVA: 0x000084DC File Offset: 0x000066DC
	public static Task RemoveItemFromSlotAsync(int playerId, int slotPosition)
	{
		DbInventory.<RemoveItemFromSlotAsync>d__3 <RemoveItemFromSlotAsync>d__;
		<RemoveItemFromSlotAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<RemoveItemFromSlotAsync>d__.playerId = playerId;
		<RemoveItemFromSlotAsync>d__.slotPosition = slotPosition;
		<RemoveItemFromSlotAsync>d__.<>1__state = -1;
		<RemoveItemFromSlotAsync>d__.<>t__builder.Start<DbInventory.<RemoveItemFromSlotAsync>d__3>(ref <RemoveItemFromSlotAsync>d__);
		return <RemoveItemFromSlotAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600012E RID: 302 RVA: 0x00008528 File Offset: 0x00006728
	public static Task<DataRow[]> GetItemsFromItemBarAsync(int playerId)
	{
		DbInventory.<GetItemsFromItemBarAsync>d__4 <GetItemsFromItemBarAsync>d__;
		<GetItemsFromItemBarAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow[]>.Create();
		<GetItemsFromItemBarAsync>d__.playerId = playerId;
		<GetItemsFromItemBarAsync>d__.<>1__state = -1;
		<GetItemsFromItemBarAsync>d__.<>t__builder.Start<DbInventory.<GetItemsFromItemBarAsync>d__4>(ref <GetItemsFromItemBarAsync>d__);
		return <GetItemsFromItemBarAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600012F RID: 303 RVA: 0x0000856C File Offset: 0x0000676C
	public static Task ChangeItemOnItemBarSlotAsync(int playerId, int slotPosition, int itemId, int requiredLevel, int boostLevel, Rarity rarity)
	{
		DbInventory.<ChangeItemOnItemBarSlotAsync>d__5 <ChangeItemOnItemBarSlotAsync>d__;
		<ChangeItemOnItemBarSlotAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<ChangeItemOnItemBarSlotAsync>d__.playerId = playerId;
		<ChangeItemOnItemBarSlotAsync>d__.slotPosition = slotPosition;
		<ChangeItemOnItemBarSlotAsync>d__.itemId = itemId;
		<ChangeItemOnItemBarSlotAsync>d__.requiredLevel = requiredLevel;
		<ChangeItemOnItemBarSlotAsync>d__.boostLevel = boostLevel;
		<ChangeItemOnItemBarSlotAsync>d__.rarity = rarity;
		<ChangeItemOnItemBarSlotAsync>d__.<>1__state = -1;
		<ChangeItemOnItemBarSlotAsync>d__.<>t__builder.Start<DbInventory.<ChangeItemOnItemBarSlotAsync>d__5>(ref <ChangeItemOnItemBarSlotAsync>d__);
		return <ChangeItemOnItemBarSlotAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000130 RID: 304 RVA: 0x000085DC File Offset: 0x000067DC
	public static Task InsertOnItemBarAsync(int playerId, int itemId, int slotPosition, int requiredLevel, int boostLevel, Rarity rarity)
	{
		DbInventory.<InsertOnItemBarAsync>d__6 <InsertOnItemBarAsync>d__;
		<InsertOnItemBarAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<InsertOnItemBarAsync>d__.playerId = playerId;
		<InsertOnItemBarAsync>d__.itemId = itemId;
		<InsertOnItemBarAsync>d__.slotPosition = slotPosition;
		<InsertOnItemBarAsync>d__.requiredLevel = requiredLevel;
		<InsertOnItemBarAsync>d__.boostLevel = boostLevel;
		<InsertOnItemBarAsync>d__.rarity = rarity;
		<InsertOnItemBarAsync>d__.<>1__state = -1;
		<InsertOnItemBarAsync>d__.<>t__builder.Start<DbInventory.<InsertOnItemBarAsync>d__6>(ref <InsertOnItemBarAsync>d__);
		return <InsertOnItemBarAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000131 RID: 305 RVA: 0x0000864C File Offset: 0x0000684C
	public static Task RemoveFromItemBarAsync(int playerId, int slotPosition)
	{
		DbInventory.<RemoveFromItemBarAsync>d__7 <RemoveFromItemBarAsync>d__;
		<RemoveFromItemBarAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<RemoveFromItemBarAsync>d__.playerId = playerId;
		<RemoveFromItemBarAsync>d__.slotPosition = slotPosition;
		<RemoveFromItemBarAsync>d__.<>1__state = -1;
		<RemoveFromItemBarAsync>d__.<>t__builder.Start<DbInventory.<RemoveFromItemBarAsync>d__7>(ref <RemoveFromItemBarAsync>d__);
		return <RemoveFromItemBarAsync>d__.<>t__builder.Task;
	}
}
