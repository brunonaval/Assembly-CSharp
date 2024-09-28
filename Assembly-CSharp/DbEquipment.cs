using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

// Token: 0x02000061 RID: 97
public static class DbEquipment
{
	// Token: 0x060000FD RID: 253 RVA: 0x00006E34 File Offset: 0x00005034
	public static Task<DataRow[]> GetItemsAsync(int playerId)
	{
		DbEquipment.<GetItemsAsync>d__0 <GetItemsAsync>d__;
		<GetItemsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow[]>.Create();
		<GetItemsAsync>d__.playerId = playerId;
		<GetItemsAsync>d__.<>1__state = -1;
		<GetItemsAsync>d__.<>t__builder.Start<DbEquipment.<GetItemsAsync>d__0>(ref <GetItemsAsync>d__);
		return <GetItemsAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060000FE RID: 254 RVA: 0x00006E78 File Offset: 0x00005078
	public static Task<int> UpdateItemOnSlotAsync(int playerId, SlotType slotType, int itemId, int amount, int requiredLevel, int boostLevel, Rarity rarity)
	{
		DbEquipment.<UpdateItemOnSlotAsync>d__1 <UpdateItemOnSlotAsync>d__;
		<UpdateItemOnSlotAsync>d__.<>t__builder = AsyncTaskMethodBuilder<int>.Create();
		<UpdateItemOnSlotAsync>d__.playerId = playerId;
		<UpdateItemOnSlotAsync>d__.slotType = slotType;
		<UpdateItemOnSlotAsync>d__.itemId = itemId;
		<UpdateItemOnSlotAsync>d__.amount = amount;
		<UpdateItemOnSlotAsync>d__.requiredLevel = requiredLevel;
		<UpdateItemOnSlotAsync>d__.boostLevel = boostLevel;
		<UpdateItemOnSlotAsync>d__.rarity = rarity;
		<UpdateItemOnSlotAsync>d__.<>1__state = -1;
		<UpdateItemOnSlotAsync>d__.<>t__builder.Start<DbEquipment.<UpdateItemOnSlotAsync>d__1>(ref <UpdateItemOnSlotAsync>d__);
		return <UpdateItemOnSlotAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060000FF RID: 255 RVA: 0x00006EF0 File Offset: 0x000050F0
	public static Task InsertItemAsync(int playerId, SlotType slotType, int itemId, int amount, int requiredLevel, int boostLevel, Rarity rarity)
	{
		DbEquipment.<InsertItemAsync>d__2 <InsertItemAsync>d__;
		<InsertItemAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<InsertItemAsync>d__.playerId = playerId;
		<InsertItemAsync>d__.slotType = slotType;
		<InsertItemAsync>d__.itemId = itemId;
		<InsertItemAsync>d__.amount = amount;
		<InsertItemAsync>d__.requiredLevel = requiredLevel;
		<InsertItemAsync>d__.boostLevel = boostLevel;
		<InsertItemAsync>d__.rarity = rarity;
		<InsertItemAsync>d__.<>1__state = -1;
		<InsertItemAsync>d__.<>t__builder.Start<DbEquipment.<InsertItemAsync>d__2>(ref <InsertItemAsync>d__);
		return <InsertItemAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000100 RID: 256 RVA: 0x00006F68 File Offset: 0x00005168
	public static Task RemoveItemFromSlotAsync(int playerId, SlotType slotType)
	{
		DbEquipment.<RemoveItemFromSlotAsync>d__3 <RemoveItemFromSlotAsync>d__;
		<RemoveItemFromSlotAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<RemoveItemFromSlotAsync>d__.playerId = playerId;
		<RemoveItemFromSlotAsync>d__.slotType = slotType;
		<RemoveItemFromSlotAsync>d__.<>1__state = -1;
		<RemoveItemFromSlotAsync>d__.<>t__builder.Start<DbEquipment.<RemoveItemFromSlotAsync>d__3>(ref <RemoveItemFromSlotAsync>d__);
		return <RemoveItemFromSlotAsync>d__.<>t__builder.Task;
	}
}
