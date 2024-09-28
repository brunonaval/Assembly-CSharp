using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Mirror;
using UnityEngine;

// Token: 0x02000369 RID: 873
public class MarketModule : MonoBehaviour
{
	// Token: 0x14000017 RID: 23
	// (add) Token: 0x06001174 RID: 4468 RVA: 0x0005269C File Offset: 0x0005089C
	// (remove) Token: 0x06001175 RID: 4469 RVA: 0x000526D4 File Offset: 0x000508D4
	public event MarketModule.OnOrderPlacedEventHandler OnOrderPlaced;

	// Token: 0x14000018 RID: 24
	// (add) Token: 0x06001176 RID: 4470 RVA: 0x0005270C File Offset: 0x0005090C
	// (remove) Token: 0x06001177 RID: 4471 RVA: 0x00052744 File Offset: 0x00050944
	public event MarketModule.OnBuyFromMarketEventHandler OnBuyFromMarket;

	// Token: 0x14000019 RID: 25
	// (add) Token: 0x06001178 RID: 4472 RVA: 0x0005277C File Offset: 0x0005097C
	// (remove) Token: 0x06001179 RID: 4473 RVA: 0x000527B4 File Offset: 0x000509B4
	public event MarketModule.OnOrderCanceledEventHandler OnOrderCanceled;

	// Token: 0x1400001A RID: 26
	// (add) Token: 0x0600117A RID: 4474 RVA: 0x000527EC File Offset: 0x000509EC
	// (remove) Token: 0x0600117B RID: 4475 RVA: 0x00052824 File Offset: 0x00050A24
	public event MarketModule.OnTakeFromStorageEventHandler OnTakeFromStorage;

	// Token: 0x1400001B RID: 27
	// (add) Token: 0x0600117C RID: 4476 RVA: 0x0005285C File Offset: 0x00050A5C
	// (remove) Token: 0x0600117D RID: 4477 RVA: 0x00052894 File Offset: 0x00050A94
	public event MarketModule.OnBuyFromGemMarketEventHandler OnBuyFromGemMarket;

	// Token: 0x0600117E RID: 4478 RVA: 0x000528CC File Offset: 0x00050ACC
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("DatabaseManager");
		this.itemDatabaseModule = gameObject.GetComponent<ItemDatabaseModule>();
		this.areaModule = base.GetComponent<AreaModule>();
		this.effectModule = base.GetComponent<EffectModule>();
		this.playerModule = base.GetComponent<PlayerModule>();
		this.walletModule = base.GetComponent<WalletModule>();
		this.inventoryModule = base.GetComponent<InventoryModule>();
	}

	// Token: 0x0600117F RID: 4479 RVA: 0x00052934 File Offset: 0x00050B34
	public Task TakeFromStorageAsync(int storageId)
	{
		MarketModule.<TakeFromStorageAsync>d__29 <TakeFromStorageAsync>d__;
		<TakeFromStorageAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<TakeFromStorageAsync>d__.<>4__this = this;
		<TakeFromStorageAsync>d__.storageId = storageId;
		<TakeFromStorageAsync>d__.<>1__state = -1;
		<TakeFromStorageAsync>d__.<>t__builder.Start<MarketModule.<TakeFromStorageAsync>d__29>(ref <TakeFromStorageAsync>d__);
		return <TakeFromStorageAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06001180 RID: 4480 RVA: 0x00052980 File Offset: 0x00050B80
	public Task PlaceOrderAsync(MarketOrder order, int amount, int unitValue, int playerId, PackageType packageType)
	{
		MarketModule.<PlaceOrderAsync>d__30 <PlaceOrderAsync>d__;
		<PlaceOrderAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<PlaceOrderAsync>d__.<>4__this = this;
		<PlaceOrderAsync>d__.order = order;
		<PlaceOrderAsync>d__.amount = amount;
		<PlaceOrderAsync>d__.unitValue = unitValue;
		<PlaceOrderAsync>d__.playerId = playerId;
		<PlaceOrderAsync>d__.packageType = packageType;
		<PlaceOrderAsync>d__.<>1__state = -1;
		<PlaceOrderAsync>d__.<>t__builder.Start<MarketModule.<PlaceOrderAsync>d__30>(ref <PlaceOrderAsync>d__);
		return <PlaceOrderAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06001181 RID: 4481 RVA: 0x000529F0 File Offset: 0x00050BF0
	private int CalculateFee(int totalValue)
	{
		int num = Mathf.CeilToInt((float)totalValue * 0.04f);
		if (this.playerModule.PremiumDays > 0)
		{
			num -= Mathf.RoundToInt((float)num * 0.5f);
		}
		return num;
	}

	// Token: 0x06001182 RID: 4482 RVA: 0x00052A2C File Offset: 0x00050C2C
	private Task<bool> ValidateOrderAsync(MarketOrder order, int amount, int unitValue, int fee, int playerId, PackageType packageType)
	{
		MarketModule.<ValidateOrderAsync>d__32 <ValidateOrderAsync>d__;
		<ValidateOrderAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<ValidateOrderAsync>d__.<>4__this = this;
		<ValidateOrderAsync>d__.order = order;
		<ValidateOrderAsync>d__.amount = amount;
		<ValidateOrderAsync>d__.unitValue = unitValue;
		<ValidateOrderAsync>d__.fee = fee;
		<ValidateOrderAsync>d__.playerId = playerId;
		<ValidateOrderAsync>d__.packageType = packageType;
		<ValidateOrderAsync>d__.<>1__state = -1;
		<ValidateOrderAsync>d__.<>t__builder.Start<MarketModule.<ValidateOrderAsync>d__32>(ref <ValidateOrderAsync>d__);
		return <ValidateOrderAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06001183 RID: 4483 RVA: 0x00052AA4 File Offset: 0x00050CA4
	public Task BuyFromGemMarketAsync(int orderId, int amount)
	{
		MarketModule.<BuyFromGemMarketAsync>d__33 <BuyFromGemMarketAsync>d__;
		<BuyFromGemMarketAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<BuyFromGemMarketAsync>d__.<>4__this = this;
		<BuyFromGemMarketAsync>d__.orderId = orderId;
		<BuyFromGemMarketAsync>d__.amount = amount;
		<BuyFromGemMarketAsync>d__.<>1__state = -1;
		<BuyFromGemMarketAsync>d__.<>t__builder.Start<MarketModule.<BuyFromGemMarketAsync>d__33>(ref <BuyFromGemMarketAsync>d__);
		return <BuyFromGemMarketAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06001184 RID: 4484 RVA: 0x00052AF8 File Offset: 0x00050CF8
	public Task BuyFromMarketAsync(int orderId, int amount)
	{
		MarketModule.<BuyFromMarketAsync>d__34 <BuyFromMarketAsync>d__;
		<BuyFromMarketAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<BuyFromMarketAsync>d__.<>4__this = this;
		<BuyFromMarketAsync>d__.orderId = orderId;
		<BuyFromMarketAsync>d__.amount = amount;
		<BuyFromMarketAsync>d__.<>1__state = -1;
		<BuyFromMarketAsync>d__.<>t__builder.Start<MarketModule.<BuyFromMarketAsync>d__34>(ref <BuyFromMarketAsync>d__);
		return <BuyFromMarketAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06001185 RID: 4485 RVA: 0x00052B4C File Offset: 0x00050D4C
	public Task CancelOrderAsync(int orderId)
	{
		MarketModule.<CancelOrderAsync>d__35 <CancelOrderAsync>d__;
		<CancelOrderAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<CancelOrderAsync>d__.<>4__this = this;
		<CancelOrderAsync>d__.orderId = orderId;
		<CancelOrderAsync>d__.<>1__state = -1;
		<CancelOrderAsync>d__.<>t__builder.Start<MarketModule.<CancelOrderAsync>d__35>(ref <CancelOrderAsync>d__);
		return <CancelOrderAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0400106C RID: 4204
	private int cancelingOrderId;

	// Token: 0x0400106D RID: 4205
	private int takingFromStorageId;

	// Token: 0x0400106E RID: 4206
	private AreaModule areaModule;

	// Token: 0x0400106F RID: 4207
	private EffectModule effectModule;

	// Token: 0x04001070 RID: 4208
	private PlayerModule playerModule;

	// Token: 0x04001071 RID: 4209
	private WalletModule walletModule;

	// Token: 0x04001072 RID: 4210
	private InventoryModule inventoryModule;

	// Token: 0x04001073 RID: 4211
	private ItemDatabaseModule itemDatabaseModule;

	// Token: 0x0200036A RID: 874
	// (Invoke) Token: 0x06001188 RID: 4488
	public delegate void OnOrderCanceledEventHandler(int canceledItemId, int amount);

	// Token: 0x0200036B RID: 875
	// (Invoke) Token: 0x0600118C RID: 4492
	public delegate void OnTakeFromStorageEventHandler(Item item, int storageId);

	// Token: 0x0200036C RID: 876
	// (Invoke) Token: 0x06001190 RID: 4496
	public delegate void OnOrderPlacedEventHandler(MarketOrder order, int amount, int unitValue);

	// Token: 0x0200036D RID: 877
	// (Invoke) Token: 0x06001194 RID: 4500
	public delegate void OnBuyFromMarketEventHandler(int boughtItemId, int amount, int unitValue);

	// Token: 0x0200036E RID: 878
	// (Invoke) Token: 0x06001198 RID: 4504
	public delegate void OnBuyFromGemMarketEventHandler(int boughtItemId, int amount, int unitValue);
}
