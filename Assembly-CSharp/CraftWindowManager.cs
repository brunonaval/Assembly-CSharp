using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001EA RID: 490
public class CraftWindowManager : MonoBehaviour
{
	// Token: 0x060005F7 RID: 1527 RVA: 0x0001ED7C File Offset: 0x0001CF7C
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x060005F8 RID: 1528 RVA: 0x0001EDA0 File Offset: 0x0001CFA0
	private void Update()
	{
		if (Time.time - this.lastUpdateUITime < 0.5f)
		{
			return;
		}
		this.UpdateUI();
	}

	// Token: 0x060005F9 RID: 1529 RVA: 0x0001EDBC File Offset: 0x0001CFBC
	private void OnEnable()
	{
		this.RefreshPlayerBlueprints();
		this.ClearMaterials();
		this.levelInput.text = this.uiSystemModule.AttributeModule.ProfessionLevel.ToString();
		this.progressFill.fillAmount = 0f;
		this.craftButton.interactable = true;
	}

	// Token: 0x060005FA RID: 1530 RVA: 0x0001EE11 File Offset: 0x0001D011
	private void OnDisable()
	{
		this.selectedBlueprint = default(PlayerBlueprint);
	}

	// Token: 0x060005FB RID: 1531 RVA: 0x0001EE20 File Offset: 0x0001D020
	private void UpdateUI()
	{
		this.lastUpdateUITime = Time.time;
		int professionLevel = this.uiSystemModule.AttributeModule.ProfessionLevel;
		this.professionLevelText.text = professionLevel.ToString();
		long professionExperienceToTargetLevel = this.uiSystemModule.AttributeModule.GetProfessionExperienceToTargetLevel(professionLevel - 1);
		long professionExperienceToTargetLevel2 = this.uiSystemModule.AttributeModule.GetProfessionExperienceToTargetLevel(professionLevel);
		long num = professionExperienceToTargetLevel2 - professionExperienceToTargetLevel;
		long num2 = num - (professionExperienceToTargetLevel2 - this.uiSystemModule.AttributeModule.ProfessionExperience);
		this.professionExperienceText.text = string.Format("{0}/{1}", num2, num);
		long num3 = num2 * 100L / num;
		this.professionExperienceFill.fillAmount = (float)num3 * 0.01f;
	}

	// Token: 0x060005FC RID: 1532 RVA: 0x0001EEDC File Offset: 0x0001D0DC
	public void RefreshPlayerBlueprints()
	{
		if (this.uiSystemModule.CraftModule == null)
		{
			return;
		}
		for (int i = 0; i < this.blueprintsHolder.transform.childCount; i++)
		{
			Transform child = this.blueprintsHolder.transform.GetChild(i);
			if (child != null)
			{
				UnityEngine.Object.Destroy(child.gameObject);
			}
		}
		foreach (PlayerBlueprint playerBlueprint in from b in this.uiSystemModule.CraftModule.Blueprints
		orderby b.Blueprint.RequiredProfessionLevel, b.Blueprint.ItemName
		select b)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.blueprintSlotPrefab);
			gameObject.transform.SetParent(this.blueprintsHolder.transform, false);
			gameObject.transform.position = Vector2.zero;
			gameObject.GetComponent<BlueprintSlotManager>().SetPlayerBlueprint(playerBlueprint);
		}
	}

	// Token: 0x060005FD RID: 1533 RVA: 0x0001F00C File Offset: 0x0001D20C
	public void SetSelectedBlueprint(PlayerBlueprint playerBlueprint)
	{
		this.selectedBlueprint = playerBlueprint;
		if (!this.selectedBlueprint.IsDefined)
		{
			return;
		}
		this.ClearMaterials();
		foreach (BlueprintMaterial material in playerBlueprint.Blueprint.Materials)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.materialPrefab);
			gameObject.transform.SetParent(this.materialsHolder.transform, false);
			gameObject.transform.position = Vector2.zero;
			gameObject.GetComponent<CraftMaterialManager>().SetMaterial(material);
		}
	}

	// Token: 0x060005FE RID: 1534 RVA: 0x0001F098 File Offset: 0x0001D298
	public void OnCraftButtonClick()
	{
		base.StartCoroutine(this.Craft(1));
	}

	// Token: 0x060005FF RID: 1535 RVA: 0x0001F0A8 File Offset: 0x0001D2A8
	public void On10TimeCraftButtonClick()
	{
		base.StartCoroutine(this.Craft(10));
	}

	// Token: 0x06000600 RID: 1536 RVA: 0x0001F0B9 File Offset: 0x0001D2B9
	public void On100TimeCraftButtonClick()
	{
		base.StartCoroutine(this.Craft(100));
	}

	// Token: 0x06000601 RID: 1537 RVA: 0x0001F0CA File Offset: 0x0001D2CA
	public void On1000TimeCraftButtonClick()
	{
		base.StartCoroutine(this.Craft(1000));
	}

	// Token: 0x06000602 RID: 1538 RVA: 0x0001F0DE File Offset: 0x0001D2DE
	private IEnumerator Craft(int times)
	{
		if (!this.HasAllMaterials(times))
		{
			yield break;
		}
		if (!this.HasFreeInventorySlots(1))
		{
			yield break;
		}
		this.craftButton.interactable = false;
		this.craft10TimesButton.interactable = false;
		this.craft100TimesButton.interactable = false;
		this.craft1000TimesButton.interactable = false;
		int num;
		for (int i = 0; i < times; i = num + 1)
		{
			do
			{
				float time = 0.05f;
				if (times > 1)
				{
					time = 0.03f;
				}
				if (times > 10)
				{
					time = 0.01f;
				}
				yield return new WaitForSecondsRealtime(time);
				this.progressFill.fillAmount += 0.1f;
			}
			while (this.progressFill.fillAmount < 1f);
			if (!this.HasFreeInventorySlots(1))
			{
				break;
			}
			try
			{
				this.progressFill.fillAmount = 0f;
				int desiredLevel;
				int.TryParse(this.levelInput.text, out desiredLevel);
				this.uiSystemModule.CraftModule.CmdCraftItem(this.selectedBlueprint.Blueprint.Id, this.ParseAndClampDesiredLevel(desiredLevel));
				this.uiSystemModule.EffectModule.PlaySoundEffect("magic_explosion", 1f, 0f, Vector3.zero);
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
			num = i;
		}
		this.craftButton.interactable = true;
		this.craft10TimesButton.interactable = true;
		this.craft100TimesButton.interactable = true;
		this.craft1000TimesButton.interactable = true;
		yield break;
	}

	// Token: 0x06000603 RID: 1539 RVA: 0x0001F0F4 File Offset: 0x0001D2F4
	private bool HasFreeInventorySlots(int times)
	{
		if (!this.uiSystemModule.InventoryModule.HasFreeSlots(this.selectedBlueprint.Blueprint.ProducesItem, this.selectedBlueprint.Blueprint.ProducesAmount * times))
		{
			this.uiSystemModule.EffectModule.ShowScreenMessage("inventory_full_message", 3, 3.5f, Array.Empty<string>());
			return false;
		}
		return true;
	}

	// Token: 0x06000604 RID: 1540 RVA: 0x0001F158 File Offset: 0x0001D358
	private bool HasAllMaterials(int times)
	{
		bool flag = true;
		foreach (BlueprintMaterial blueprintMaterial in this.selectedBlueprint.Blueprint.Materials)
		{
			if (this.uiSystemModule.InventoryModule.GetAmount(blueprintMaterial.MaterialItem.Id) < blueprintMaterial.MaterialAmount * times)
			{
				flag = false;
				break;
			}
		}
		if (!flag)
		{
			this.uiSystemModule.EffectModule.ShowScreenMessage("not_enough_blueprint_materials_message", 3, 3.5f, Array.Empty<string>());
			return false;
		}
		return true;
	}

	// Token: 0x06000605 RID: 1541 RVA: 0x0001F1E0 File Offset: 0x0001D3E0
	private void ClearMaterials()
	{
		for (int i = 0; i < this.materialsHolder.transform.childCount; i++)
		{
			Transform child = this.materialsHolder.transform.GetChild(i);
			if (child != null)
			{
				UnityEngine.Object.Destroy(child.gameObject);
			}
		}
	}

	// Token: 0x06000606 RID: 1542 RVA: 0x0001F230 File Offset: 0x0001D430
	public int ParseAndClampDesiredLevel(int desiredLevel)
	{
		desiredLevel = Mathf.Min(this.uiSystemModule.AttributeModule.ProfessionLevel, desiredLevel);
		desiredLevel = Mathf.Max(1, desiredLevel);
		if (this.selectedBlueprint.IsDefined)
		{
			desiredLevel = Mathf.Max(GlobalUtils.GetMinRequiredLevelForQuality(this.selectedBlueprint.Blueprint.ProducesItem.Quality), desiredLevel);
		}
		return desiredLevel;
	}

	// Token: 0x06000607 RID: 1543 RVA: 0x0001F290 File Offset: 0x0001D490
	public void OnLevelInputChanged()
	{
		int desiredLevel;
		int.TryParse(this.levelInput.text, out desiredLevel);
		this.levelInput.text = this.ParseAndClampDesiredLevel(desiredLevel).ToString();
	}

	// Token: 0x06000608 RID: 1544 RVA: 0x0001F2CC File Offset: 0x0001D4CC
	public void OnIncreaseLevelButtonClick()
	{
		int num;
		int.TryParse(this.levelInput.text, out num);
		num++;
		this.levelInput.text = this.ParseAndClampDesiredLevel(num).ToString();
	}

	// Token: 0x06000609 RID: 1545 RVA: 0x0001F30C File Offset: 0x0001D50C
	public void OnDecreaseLevelButtonClick()
	{
		int num;
		int.TryParse(this.levelInput.text, out num);
		num--;
		this.levelInput.text = this.ParseAndClampDesiredLevel(num).ToString();
	}

	// Token: 0x04000845 RID: 2117
	[SerializeField]
	private GameObject blueprintSlotPrefab;

	// Token: 0x04000846 RID: 2118
	[SerializeField]
	private GameObject blueprintsHolder;

	// Token: 0x04000847 RID: 2119
	[SerializeField]
	private GameObject materialPrefab;

	// Token: 0x04000848 RID: 2120
	[SerializeField]
	private GameObject materialsHolder;

	// Token: 0x04000849 RID: 2121
	[SerializeField]
	private Text professionLevelText;

	// Token: 0x0400084A RID: 2122
	[SerializeField]
	private Text professionExperienceText;

	// Token: 0x0400084B RID: 2123
	[SerializeField]
	private Image professionExperienceFill;

	// Token: 0x0400084C RID: 2124
	[SerializeField]
	private Image progressFill;

	// Token: 0x0400084D RID: 2125
	[SerializeField]
	private Button craftButton;

	// Token: 0x0400084E RID: 2126
	[SerializeField]
	private Button craft10TimesButton;

	// Token: 0x0400084F RID: 2127
	[SerializeField]
	private Button craft100TimesButton;

	// Token: 0x04000850 RID: 2128
	[SerializeField]
	private Button craft1000TimesButton;

	// Token: 0x04000851 RID: 2129
	[SerializeField]
	private InputField levelInput;

	// Token: 0x04000852 RID: 2130
	private UISystemModule uiSystemModule;

	// Token: 0x04000853 RID: 2131
	private PlayerBlueprint selectedBlueprint;

	// Token: 0x04000854 RID: 2132
	private float lastUpdateUITime;
}
