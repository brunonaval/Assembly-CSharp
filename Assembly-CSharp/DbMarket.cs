using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

// Token: 0x0200007E RID: 126
public static class DbMarket
{
	// Token: 0x06000148 RID: 328 RVA: 0x00009218 File Offset: 0x00007418
	public static Task<DataRow> GetMarketOrderByIdAsync(int orderId)
	{
		DbMarket.<GetMarketOrderByIdAsync>d__1 <GetMarketOrderByIdAsync>d__;
		<GetMarketOrderByIdAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow>.Create();
		<GetMarketOrderByIdAsync>d__.orderId = orderId;
		<GetMarketOrderByIdAsync>d__.<>1__state = -1;
		<GetMarketOrderByIdAsync>d__.<>t__builder.Start<DbMarket.<GetMarketOrderByIdAsync>d__1>(ref <GetMarketOrderByIdAsync>d__);
		return <GetMarketOrderByIdAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000149 RID: 329 RVA: 0x0000925C File Offset: 0x0000745C
	public static Task<DataRow> GetGemOrderByIdAsync(int orderId)
	{
		DbMarket.<GetGemOrderByIdAsync>d__2 <GetGemOrderByIdAsync>d__;
		<GetGemOrderByIdAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow>.Create();
		<GetGemOrderByIdAsync>d__.orderId = orderId;
		<GetGemOrderByIdAsync>d__.<>1__state = -1;
		<GetGemOrderByIdAsync>d__.<>t__builder.Start<DbMarket.<GetGemOrderByIdAsync>d__2>(ref <GetGemOrderByIdAsync>d__);
		return <GetGemOrderByIdAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600014A RID: 330 RVA: 0x000092A0 File Offset: 0x000074A0
	public static Task<DataRow> GetItemFromStorageAsync(int storageId, int serverId)
	{
		DbMarket.<GetItemFromStorageAsync>d__3 <GetItemFromStorageAsync>d__;
		<GetItemFromStorageAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow>.Create();
		<GetItemFromStorageAsync>d__.storageId = storageId;
		<GetItemFromStorageAsync>d__.serverId = serverId;
		<GetItemFromStorageAsync>d__.<>1__state = -1;
		<GetItemFromStorageAsync>d__.<>t__builder.Start<DbMarket.<GetItemFromStorageAsync>d__3>(ref <GetItemFromStorageAsync>d__);
		return <GetItemFromStorageAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600014B RID: 331 RVA: 0x000092EC File Offset: 0x000074EC
	public static Task PlaceOrderAsync(int playerId, int itemId, int amount, int unitValue, int requiredLevel, int boostLevel, Rarity rarity)
	{
		DbMarket.<PlaceOrderAsync>d__4 <PlaceOrderAsync>d__;
		<PlaceOrderAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<PlaceOrderAsync>d__.playerId = playerId;
		<PlaceOrderAsync>d__.itemId = itemId;
		<PlaceOrderAsync>d__.amount = amount;
		<PlaceOrderAsync>d__.unitValue = unitValue;
		<PlaceOrderAsync>d__.requiredLevel = requiredLevel;
		<PlaceOrderAsync>d__.boostLevel = boostLevel;
		<PlaceOrderAsync>d__.rarity = rarity;
		<PlaceOrderAsync>d__.<>1__state = -1;
		<PlaceOrderAsync>d__.<>t__builder.Start<DbMarket.<PlaceOrderAsync>d__4>(ref <PlaceOrderAsync>d__);
		return <PlaceOrderAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600014C RID: 332 RVA: 0x00009364 File Offset: 0x00007564
	public static Task<int> CountItemsFromMarketAsync(int playerId)
	{
		DbMarket.<CountItemsFromMarketAsync>d__5 <CountItemsFromMarketAsync>d__;
		<CountItemsFromMarketAsync>d__.<>t__builder = AsyncTaskMethodBuilder<int>.Create();
		<CountItemsFromMarketAsync>d__.playerId = playerId;
		<CountItemsFromMarketAsync>d__.<>1__state = -1;
		<CountItemsFromMarketAsync>d__.<>t__builder.Start<DbMarket.<CountItemsFromMarketAsync>d__5>(ref <CountItemsFromMarketAsync>d__);
		return <CountItemsFromMarketAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600014D RID: 333 RVA: 0x000093A8 File Offset: 0x000075A8
	public static Task<int> CountItemsFromStorageAsync(int accountId, int serverId)
	{
		DbMarket.<CountItemsFromStorageAsync>d__6 <CountItemsFromStorageAsync>d__;
		<CountItemsFromStorageAsync>d__.<>t__builder = AsyncTaskMethodBuilder<int>.Create();
		<CountItemsFromStorageAsync>d__.accountId = accountId;
		<CountItemsFromStorageAsync>d__.serverId = serverId;
		<CountItemsFromStorageAsync>d__.<>1__state = -1;
		<CountItemsFromStorageAsync>d__.<>t__builder.Start<DbMarket.<CountItemsFromStorageAsync>d__6>(ref <CountItemsFromStorageAsync>d__);
		return <CountItemsFromStorageAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600014E RID: 334 RVA: 0x000093F4 File Offset: 0x000075F4
	public static Task UpdateOrderAmountAsync(int marketOrderId, int newAmount)
	{
		DbMarket.<UpdateOrderAmountAsync>d__7 <UpdateOrderAmountAsync>d__;
		<UpdateOrderAmountAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<UpdateOrderAmountAsync>d__.marketOrderId = marketOrderId;
		<UpdateOrderAmountAsync>d__.newAmount = newAmount;
		<UpdateOrderAmountAsync>d__.<>1__state = -1;
		<UpdateOrderAmountAsync>d__.<>t__builder.Start<DbMarket.<UpdateOrderAmountAsync>d__7>(ref <UpdateOrderAmountAsync>d__);
		return <UpdateOrderAmountAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600014F RID: 335 RVA: 0x00009440 File Offset: 0x00007640
	public static Task<int> RemoveOrderAsync(int marketOrderId)
	{
		DbMarket.<RemoveOrderAsync>d__8 <RemoveOrderAsync>d__;
		<RemoveOrderAsync>d__.<>t__builder = AsyncTaskMethodBuilder<int>.Create();
		<RemoveOrderAsync>d__.marketOrderId = marketOrderId;
		<RemoveOrderAsync>d__.<>1__state = -1;
		<RemoveOrderAsync>d__.<>t__builder.Start<DbMarket.<RemoveOrderAsync>d__8>(ref <RemoveOrderAsync>d__);
		return <RemoveOrderAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000150 RID: 336 RVA: 0x00009484 File Offset: 0x00007684
	public static Task<int> RemoveStorageAsync(int storageId)
	{
		DbMarket.<RemoveStorageAsync>d__9 <RemoveStorageAsync>d__;
		<RemoveStorageAsync>d__.<>t__builder = AsyncTaskMethodBuilder<int>.Create();
		<RemoveStorageAsync>d__.storageId = storageId;
		<RemoveStorageAsync>d__.<>1__state = -1;
		<RemoveStorageAsync>d__.<>t__builder.Start<DbMarket.<RemoveStorageAsync>d__9>(ref <RemoveStorageAsync>d__);
		return <RemoveStorageAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000151 RID: 337 RVA: 0x000094C8 File Offset: 0x000076C8
	public static Task<int> SubtractAmountFromStorageAsync(int storageId, int amount)
	{
		DbMarket.<SubtractAmountFromStorageAsync>d__10 <SubtractAmountFromStorageAsync>d__;
		<SubtractAmountFromStorageAsync>d__.<>t__builder = AsyncTaskMethodBuilder<int>.Create();
		<SubtractAmountFromStorageAsync>d__.storageId = storageId;
		<SubtractAmountFromStorageAsync>d__.amount = amount;
		<SubtractAmountFromStorageAsync>d__.<>1__state = -1;
		<SubtractAmountFromStorageAsync>d__.<>t__builder.Start<DbMarket.<SubtractAmountFromStorageAsync>d__10>(ref <SubtractAmountFromStorageAsync>d__);
		return <SubtractAmountFromStorageAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000152 RID: 338 RVA: 0x00009514 File Offset: 0x00007714
	public static Task BuyFromMarket(int sellerServerId, int buyerServerId, int sellerAccountId, int buyerAccountId, int marketOrderId, int itemId, int amount, int unitValue, int requiredLevel, int boostLevel, Rarity rarity, bool itemIsStackable)
	{
		DbMarket.<BuyFromMarket>d__11 <BuyFromMarket>d__;
		<BuyFromMarket>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<BuyFromMarket>d__.sellerServerId = sellerServerId;
		<BuyFromMarket>d__.buyerServerId = buyerServerId;
		<BuyFromMarket>d__.sellerAccountId = sellerAccountId;
		<BuyFromMarket>d__.buyerAccountId = buyerAccountId;
		<BuyFromMarket>d__.marketOrderId = marketOrderId;
		<BuyFromMarket>d__.itemId = itemId;
		<BuyFromMarket>d__.amount = amount;
		<BuyFromMarket>d__.unitValue = unitValue;
		<BuyFromMarket>d__.requiredLevel = requiredLevel;
		<BuyFromMarket>d__.boostLevel = boostLevel;
		<BuyFromMarket>d__.rarity = rarity;
		<BuyFromMarket>d__.itemIsStackable = itemIsStackable;
		<BuyFromMarket>d__.<>1__state = -1;
		<BuyFromMarket>d__.<>t__builder.Start<DbMarket.<BuyFromMarket>d__11>(ref <BuyFromMarket>d__);
		return <BuyFromMarket>d__.<>t__builder.Task;
	}

	// Token: 0x06000153 RID: 339 RVA: 0x000095B8 File Offset: 0x000077B8
	public static Task CancelOrderAsync(int serverId, int marketOrderId, int ownerAccountId, int itemId, int amount, int requiredLevel, int boostLevel, Rarity rarity, bool itemIsStackable)
	{
		DbMarket.<CancelOrderAsync>d__12 <CancelOrderAsync>d__;
		<CancelOrderAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<CancelOrderAsync>d__.serverId = serverId;
		<CancelOrderAsync>d__.marketOrderId = marketOrderId;
		<CancelOrderAsync>d__.ownerAccountId = ownerAccountId;
		<CancelOrderAsync>d__.itemId = itemId;
		<CancelOrderAsync>d__.amount = amount;
		<CancelOrderAsync>d__.requiredLevel = requiredLevel;
		<CancelOrderAsync>d__.boostLevel = boostLevel;
		<CancelOrderAsync>d__.rarity = rarity;
		<CancelOrderAsync>d__.itemIsStackable = itemIsStackable;
		<CancelOrderAsync>d__.<>1__state = -1;
		<CancelOrderAsync>d__.<>t__builder.Start<DbMarket.<CancelOrderAsync>d__12>(ref <CancelOrderAsync>d__);
		return <CancelOrderAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000154 RID: 340 RVA: 0x00009640 File Offset: 0x00007840
	private static Task<int> GetAmountAsync(int marketOrderId)
	{
		DbMarket.<GetAmountAsync>d__13 <GetAmountAsync>d__;
		<GetAmountAsync>d__.<>t__builder = AsyncTaskMethodBuilder<int>.Create();
		<GetAmountAsync>d__.marketOrderId = marketOrderId;
		<GetAmountAsync>d__.<>1__state = -1;
		<GetAmountAsync>d__.<>t__builder.Start<DbMarket.<GetAmountAsync>d__13>(ref <GetAmountAsync>d__);
		return <GetAmountAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000155 RID: 341 RVA: 0x00009684 File Offset: 0x00007884
	public static Task AddToStorageAsync(int serverId, int ownerAccountId, int itemId, int amount, int requiredLevel, int boostLevel, Rarity rarity, bool itemIsStackable)
	{
		DbMarket.<AddToStorageAsync>d__14 <AddToStorageAsync>d__;
		<AddToStorageAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<AddToStorageAsync>d__.serverId = serverId;
		<AddToStorageAsync>d__.ownerAccountId = ownerAccountId;
		<AddToStorageAsync>d__.itemId = itemId;
		<AddToStorageAsync>d__.amount = amount;
		<AddToStorageAsync>d__.requiredLevel = requiredLevel;
		<AddToStorageAsync>d__.boostLevel = boostLevel;
		<AddToStorageAsync>d__.rarity = rarity;
		<AddToStorageAsync>d__.itemIsStackable = itemIsStackable;
		<AddToStorageAsync>d__.<>1__state = -1;
		<AddToStorageAsync>d__.<>t__builder.Start<DbMarket.<AddToStorageAsync>d__14>(ref <AddToStorageAsync>d__);
		return <AddToStorageAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000156 RID: 342 RVA: 0x00009704 File Offset: 0x00007904
	public static Task IncrementTotalSoldAsync(int itemId, int amount)
	{
		DbMarket.<IncrementTotalSoldAsync>d__15 <IncrementTotalSoldAsync>d__;
		<IncrementTotalSoldAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<IncrementTotalSoldAsync>d__.itemId = itemId;
		<IncrementTotalSoldAsync>d__.amount = amount;
		<IncrementTotalSoldAsync>d__.<>1__state = -1;
		<IncrementTotalSoldAsync>d__.<>t__builder.Start<DbMarket.<IncrementTotalSoldAsync>d__15>(ref <IncrementTotalSoldAsync>d__);
		return <IncrementTotalSoldAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000157 RID: 343 RVA: 0x00009750 File Offset: 0x00007950
	public static Task IncrementTotalSoldAsync(int orderId)
	{
		DbMarket.<IncrementTotalSoldAsync>d__16 <IncrementTotalSoldAsync>d__;
		<IncrementTotalSoldAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<IncrementTotalSoldAsync>d__.orderId = orderId;
		<IncrementTotalSoldAsync>d__.<>1__state = -1;
		<IncrementTotalSoldAsync>d__.<>t__builder.Start<DbMarket.<IncrementTotalSoldAsync>d__16>(ref <IncrementTotalSoldAsync>d__);
		return <IncrementTotalSoldAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000158 RID: 344 RVA: 0x00009794 File Offset: 0x00007994
	public static Task AddToHistory(int buyerAccountId, int itemId, Rarity itemRarity, int itemBoostLevel, int amount, int unitValue)
	{
		DbMarket.<AddToHistory>d__17 <AddToHistory>d__;
		<AddToHistory>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<AddToHistory>d__.buyerAccountId = buyerAccountId;
		<AddToHistory>d__.itemId = itemId;
		<AddToHistory>d__.itemRarity = itemRarity;
		<AddToHistory>d__.itemBoostLevel = itemBoostLevel;
		<AddToHistory>d__.amount = amount;
		<AddToHistory>d__.unitValue = unitValue;
		<AddToHistory>d__.<>1__state = -1;
		<AddToHistory>d__.<>t__builder.Start<DbMarket.<AddToHistory>d__17>(ref <AddToHistory>d__);
		return <AddToHistory>d__.<>t__builder.Task;
	}

	// Token: 0x06000159 RID: 345 RVA: 0x00009804 File Offset: 0x00007A04
	public static Task<DataRow[]> GetExpiredOrdersAsync()
	{
		DbMarket.<GetExpiredOrdersAsync>d__18 <GetExpiredOrdersAsync>d__;
		<GetExpiredOrdersAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow[]>.Create();
		<GetExpiredOrdersAsync>d__.<>1__state = -1;
		<GetExpiredOrdersAsync>d__.<>t__builder.Start<DbMarket.<GetExpiredOrdersAsync>d__18>(ref <GetExpiredOrdersAsync>d__);
		return <GetExpiredOrdersAsync>d__.<>t__builder.Task;
	}

	// Token: 0x04000236 RID: 566
	private const int GoldCoinsItemId = 10;
}
