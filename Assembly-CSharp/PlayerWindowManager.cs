using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000288 RID: 648
public class PlayerWindowManager : MonoBehaviour
{
	// Token: 0x06000A0B RID: 2571 RVA: 0x0002E479 File Offset: 0x0002C679
	private void OnEnable()
	{
		this.UpdateAllUIs();
		this.RefreshTitles();
	}

	// Token: 0x06000A0C RID: 2572 RVA: 0x0002E487 File Offset: 0x0002C687
	private void Update()
	{
		if (Time.time - this.lastUIUpdateTime < 0.5f)
		{
			return;
		}
		this.UpdateAllUIs();
	}

	// Token: 0x06000A0D RID: 2573 RVA: 0x0002E4A4 File Offset: 0x0002C6A4
	private void RefreshTitles()
	{
		for (int i = 0; i < this.titlesHolder.transform.childCount; i++)
		{
			UnityEngine.Object.Destroy(this.titlesHolder.transform.GetChild(i).gameObject);
		}
		foreach (Title title in this.uiSystemModule.TitleModule.Titles)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.titleSlotPrefab);
			gameObject.transform.SetParent(this.titlesHolder.transform, false);
			gameObject.transform.position = Vector2.zero;
			gameObject.GetComponent<PlayerTitleSlotManager>().SetTitle(title);
			Toggle component = gameObject.GetComponent<Toggle>();
			component.group = this.titleToggleGroup;
			if (title.Name.Equals(this.uiSystemModule.CreatureModule.CreatureTitle))
			{
				component.isOn = true;
			}
		}
	}

	// Token: 0x06000A0E RID: 2574 RVA: 0x0002E5B0 File Offset: 0x0002C7B0
	private void UpdateAllUIs()
	{
		this.UpdatePremiumDaysUI();
		this.UpdateBaseInfoUI();
		this.UpdateCombatInfoUI();
		this.UpdatePowerUI();
		this.UpdateToughnessUI();
		this.UpdatePrecisionUI();
		this.UpdateAgilityUI();
		this.UpdateVitalityUI();
		this.UpdateProfessionInfoUI();
		this.lastUIUpdateTime = Time.time;
	}

	// Token: 0x06000A0F RID: 2575 RVA: 0x0002E600 File Offset: 0x0002C800
	private void UpdateCombatInfoUI()
	{
		this.pvpPointsText.text = this.uiSystemModule.PvpModule.CombatScore.ToString();
		this.pkPointsText.text = this.uiSystemModule.PvpModule.KillScore.ToString();
		this.karmaPointsText.text = this.uiSystemModule.PvpModule.KarmaPoints.ToString();
		this.dropChanceText.text = string.Format("{0:0.00}%", this.uiSystemModule.PvpModule.EquipmentDropChance * 100f);
	}

	// Token: 0x06000A10 RID: 2576 RVA: 0x0002E6A0 File Offset: 0x0002C8A0
	private void UpdatePowerUI()
	{
		float num = this.uiSystemModule.AttributeModule.Attack + (float)this.uiSystemModule.CombatModule.GetWeaponAttack() + (float)this.uiSystemModule.EquipmentModule.GetItem(SlotType.Ammo).Attack;
		this.attackText.text = string.Format("{0:0.00}", num);
		this.criticalDamageText.text = string.Format("{0:0}%", this.uiSystemModule.AttributeModule.CriticalDamage * 100f);
	}

	// Token: 0x06000A11 RID: 2577 RVA: 0x0002E738 File Offset: 0x0002C938
	private void UpdatePremiumDaysUI()
	{
		int premiumDays = this.uiSystemModule.PlayerModule.PremiumDays;
		if (premiumDays == 0)
		{
			this.premiumDaysText.text = string.Format("<color=red>{0}</color>", premiumDays);
			return;
		}
		if (premiumDays <= 7)
		{
			this.premiumDaysText.text = string.Format("<color=orange>{0}</color>", premiumDays);
			return;
		}
		this.premiumDaysText.text = string.Format("<color=green>{0}</color>", premiumDays);
	}

	// Token: 0x06000A12 RID: 2578 RVA: 0x0002E7B0 File Offset: 0x0002C9B0
	private void UpdateBaseInfoUI()
	{
		this.playerNameText.text = this.uiSystemModule.CreatureModule.CreatureName;
		string str = this.uiSystemModule.VocationModule.Vocation.ToString().ToLower();
		string str2 = this.uiSystemModule.CreatureModule.Gender.ToString().ToLower();
		this.playerImage.sprite = ResourcesManager.Instance.GetVocationSprite(str + "_" + str2 + "_full");
		if (!string.IsNullOrEmpty(this.uiSystemModule.CreatureModule.CreatureTitle))
		{
			this.playerTitleText.text = LanguageManager.Instance.GetText(this.uiSystemModule.CreatureModule.CreatureTitle);
		}
		else
		{
			this.playerTitleText.text = string.Empty;
		}
		this.levelText.text = this.uiSystemModule.AttributeModule.BaseLevel.ToString();
		this.experienceText.text = this.uiSystemModule.AttributeModule.BaseExperience.ToString();
		this.masteryLevelText.text = this.uiSystemModule.AttributeModule.MasteryLevel.ToString();
	}

	// Token: 0x06000A13 RID: 2579 RVA: 0x0002E8F0 File Offset: 0x0002CAF0
	private void UpdateVitalityUI()
	{
		this.maxHealthText.text = this.uiSystemModule.AttributeModule.MaxHealth.ToString();
		this.regenText.text = this.uiSystemModule.AttributeModule.RegenerationAmount.ToString();
		this.regenIntervalText.text = string.Format("{0:0.00}s", this.uiSystemModule.AttributeModule.RegenerationInterval);
	}

	// Token: 0x06000A14 RID: 2580 RVA: 0x0002E96C File Offset: 0x0002CB6C
	private void UpdateAgilityUI()
	{
		this.speedText.text = string.Format("{0:0.00}", this.uiSystemModule.AttributeModule.Speed);
		this.maxEnduranceText.text = string.Format("{0:0}", this.uiSystemModule.AttributeModule.MaxEndurance);
	}

	// Token: 0x06000A15 RID: 2581 RVA: 0x0002E9CD File Offset: 0x0002CBCD
	private void UpdatePrecisionUI()
	{
		this.criticalChanceText.text = string.Format("{0:0.00}%", this.uiSystemModule.AttributeModule.CriticalChance * 100f);
	}

	// Token: 0x06000A16 RID: 2582 RVA: 0x0002EA00 File Offset: 0x0002CC00
	private void UpdateToughnessUI()
	{
		float num = this.uiSystemModule.AttributeModule.Defense + (float)this.uiSystemModule.CombatModule.GetWeaponDefense();
		this.defenseText.text = string.Format("{0:0.00}", num);
		float num2 = this.uiSystemModule.AttributeModule.Resistance + (float)this.uiSystemModule.CombatModule.GetArmorDefense();
		this.resistanceText.text = string.Format("{0:0.00}", num2);
	}

	// Token: 0x06000A17 RID: 2583 RVA: 0x0002EA8C File Offset: 0x0002CC8C
	private void UpdateProfessionInfoUI()
	{
		this.professionText.text = LanguageManager.Instance.GetText(this.uiSystemModule.AttributeModule.Profession.ToString().ToLower());
		this.professionLevelText.text = this.uiSystemModule.AttributeModule.ProfessionLevel.ToString();
	}

	// Token: 0x04000B88 RID: 2952
	[SerializeField]
	private UISystemModule uiSystemModule;

	// Token: 0x04000B89 RID: 2953
	[SerializeField]
	private Text playerNameText;

	// Token: 0x04000B8A RID: 2954
	[SerializeField]
	private Text playerTitleText;

	// Token: 0x04000B8B RID: 2955
	[SerializeField]
	private Image playerImage;

	// Token: 0x04000B8C RID: 2956
	[SerializeField]
	private Text premiumDaysText;

	// Token: 0x04000B8D RID: 2957
	[SerializeField]
	private Text maxHealthText;

	// Token: 0x04000B8E RID: 2958
	[SerializeField]
	private Text maxEnduranceText;

	// Token: 0x04000B8F RID: 2959
	[SerializeField]
	private Text attackText;

	// Token: 0x04000B90 RID: 2960
	[SerializeField]
	private Text criticalDamageText;

	// Token: 0x04000B91 RID: 2961
	[SerializeField]
	private Text regenText;

	// Token: 0x04000B92 RID: 2962
	[SerializeField]
	private Text regenIntervalText;

	// Token: 0x04000B93 RID: 2963
	[SerializeField]
	private Text speedText;

	// Token: 0x04000B94 RID: 2964
	[SerializeField]
	private Text criticalChanceText;

	// Token: 0x04000B95 RID: 2965
	[SerializeField]
	private Text defenseText;

	// Token: 0x04000B96 RID: 2966
	[SerializeField]
	private Text resistanceText;

	// Token: 0x04000B97 RID: 2967
	[SerializeField]
	private Text pvpPointsText;

	// Token: 0x04000B98 RID: 2968
	[SerializeField]
	private Text pkPointsText;

	// Token: 0x04000B99 RID: 2969
	[SerializeField]
	private Text karmaPointsText;

	// Token: 0x04000B9A RID: 2970
	[SerializeField]
	private Text dropChanceText;

	// Token: 0x04000B9B RID: 2971
	[SerializeField]
	private Text levelText;

	// Token: 0x04000B9C RID: 2972
	[SerializeField]
	private Text experienceText;

	// Token: 0x04000B9D RID: 2973
	[SerializeField]
	private Text masteryLevelText;

	// Token: 0x04000B9E RID: 2974
	[SerializeField]
	private Text professionText;

	// Token: 0x04000B9F RID: 2975
	[SerializeField]
	private Text professionLevelText;

	// Token: 0x04000BA0 RID: 2976
	[SerializeField]
	private GameObject titlesHolder;

	// Token: 0x04000BA1 RID: 2977
	[SerializeField]
	private GameObject titleSlotPrefab;

	// Token: 0x04000BA2 RID: 2978
	[SerializeField]
	private ToggleGroup titleToggleGroup;

	// Token: 0x04000BA3 RID: 2979
	private float lastUIUpdateTime;
}
