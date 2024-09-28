using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000271 RID: 625
public class CollectorConfigWindowManager : MonoBehaviour
{
	// Token: 0x06000984 RID: 2436 RVA: 0x0002C678 File Offset: 0x0002A878
	private void OnEnable()
	{
		this.LoadItemTypesIntoSelectField();
		this.LoadItemCategoriesIntoSelectField();
		this.LoadRaritiesIntoSelectField();
		this.LoadQualitiesIntoSelectField();
		this.typeFilter.value = (int)this.uiSystemModule.InventoryModule.CollectorTypeFilter;
		this.categoryFilter.value = (int)this.uiSystemModule.InventoryModule.CollectorCategoryFilter;
		this.minRarityFilter.value = (int)this.uiSystemModule.InventoryModule.CollectorMinRarityFilter;
		this.minQualityFilter.value = (int)this.uiSystemModule.InventoryModule.CollectorMinQualityFilter;
	}

	// Token: 0x06000985 RID: 2437 RVA: 0x0002C70C File Offset: 0x0002A90C
	public void StartCollecting()
	{
		this.uiSystemModule.InventoryModule.CmdConfigureCollector((Rarity)this.minRarityFilter.value, (ItemQuality)this.minQualityFilter.value, (ItemType)this.typeFilter.value, (ItemCategory)this.categoryFilter.value);
		base.GetComponent<DragWindowModule>().CloseWindow();
	}

	// Token: 0x06000986 RID: 2438 RVA: 0x0002C760 File Offset: 0x0002A960
	private void LoadItemCategoriesIntoSelectField()
	{
		this.categoryFilter.ClearOptions();
		List<string> list = new List<string>();
		foreach (string itemCategoryName in Enum.GetNames(typeof(ItemCategory)))
		{
			list.Add(GlobalUtils.ItemCategoryToMeta(itemCategoryName));
		}
		this.categoryFilter.AddOptions(list);
	}

	// Token: 0x06000987 RID: 2439 RVA: 0x0002C7B8 File Offset: 0x0002A9B8
	private void LoadItemTypesIntoSelectField()
	{
		this.typeFilter.ClearOptions();
		List<string> list = new List<string>();
		foreach (string itemTypeName in Enum.GetNames(typeof(ItemType)))
		{
			list.Add(GlobalUtils.ItemTypeNameToMeta(itemTypeName));
		}
		this.typeFilter.AddOptions(list);
	}

	// Token: 0x06000988 RID: 2440 RVA: 0x0002C810 File Offset: 0x0002AA10
	private void LoadRaritiesIntoSelectField()
	{
		this.minRarityFilter.ClearOptions();
		List<string> list = new List<string>();
		foreach (string rarityName in Enum.GetNames(typeof(Rarity)))
		{
			list.Add(GlobalUtils.RarityToMeta(rarityName));
		}
		this.minRarityFilter.AddOptions(list);
	}

	// Token: 0x06000989 RID: 2441 RVA: 0x0002C868 File Offset: 0x0002AA68
	private void LoadQualitiesIntoSelectField()
	{
		this.minQualityFilter.ClearOptions();
		List<string> list = new List<string>();
		foreach (string qualityName in Enum.GetNames(typeof(ItemQuality)))
		{
			list.Add(GlobalUtils.ItemQualityToMeta(qualityName));
		}
		this.minQualityFilter.AddOptions(list);
	}

	// Token: 0x04000B23 RID: 2851
	[SerializeField]
	private UISystemModule uiSystemModule;

	// Token: 0x04000B24 RID: 2852
	[SerializeField]
	private Dropdown typeFilter;

	// Token: 0x04000B25 RID: 2853
	[SerializeField]
	private Dropdown categoryFilter;

	// Token: 0x04000B26 RID: 2854
	[SerializeField]
	private Dropdown minRarityFilter;

	// Token: 0x04000B27 RID: 2855
	[SerializeField]
	private Dropdown minQualityFilter;
}
