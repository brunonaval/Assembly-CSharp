using System;
using System.Collections.Generic;
using DuloGames.UI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020002C4 RID: 708
public class AttributeWindowModule : MonoBehaviour
{
	// Token: 0x06000C46 RID: 3142 RVA: 0x00036F00 File Offset: 0x00035100
	private void OnEnable()
	{
		List<string> options = new List<string>
		{
			LanguageManager.Instance.GetText("training_mode_balanced"),
			LanguageManager.Instance.GetText("training_mode_axp_focused"),
			LanguageManager.Instance.GetText("training_mode_exp_focused")
		};
		this.trainingModeSlider.options = options;
		this.trainingModeSlider.value = (float)this.uiSystemModule.AttributeModule.TrainingMode;
	}

	// Token: 0x06000C47 RID: 3143 RVA: 0x00036F7A File Offset: 0x0003517A
	private void Update()
	{
		this.UpdatePowerUI();
		this.UpdateToughnessUI();
		this.UpdateAgilityUI();
		this.UpdatePrecisionUI();
		this.UpdateVitalityUI();
	}

	// Token: 0x06000C48 RID: 3144 RVA: 0x00036F9C File Offset: 0x0003519C
	public void UpdatePowerUI()
	{
		global::Attribute adjustedAttribute = this.uiSystemModule.AttributeModule.GetAdjustedAttribute(AttributeType.Power);
		long experience = adjustedAttribute.Experience;
		long experienceToLevel = adjustedAttribute.ExperienceToLevel;
		long experienceToCurrentLevel = adjustedAttribute.ExperienceToCurrentLevel;
		long num = experienceToLevel - experienceToCurrentLevel;
		long num2 = num - (experienceToLevel - experience);
		bool flag = adjustedAttribute.Level >= adjustedAttribute.MaxLevel;
		this.powerExperienceBar.fillAmount = (flag ? 1f : ((float)num2 / (float)num));
		this.powerExperienceText.text = (flag ? "MAX." : string.Format("{0}/{1}", num2, num));
		this.powerLevelText.text = this.FormatAttributeLevelText(adjustedAttribute);
		float attack = this.uiSystemModule.AttributeModule.Attack;
		float value = this.uiSystemModule.AttributeModule.CriticalDamage * 100f;
		this.powerDescriptionText.text = string.Format(LanguageManager.Instance.GetText("attribute_slot_power_description"), this.FormatAttributeText(attack, 2, ""), this.FormatAttributeText(value, 2, "%"));
	}

	// Token: 0x06000C49 RID: 3145 RVA: 0x000370B8 File Offset: 0x000352B8
	public void UpdateToughnessUI()
	{
		global::Attribute adjustedAttribute = this.uiSystemModule.AttributeModule.GetAdjustedAttribute(AttributeType.Toughness);
		long experience = adjustedAttribute.Experience;
		long experienceToLevel = adjustedAttribute.ExperienceToLevel;
		long experienceToCurrentLevel = adjustedAttribute.ExperienceToCurrentLevel;
		long num = experienceToLevel - experienceToCurrentLevel;
		long num2 = num - (experienceToLevel - experience);
		bool flag = adjustedAttribute.Level >= adjustedAttribute.MaxLevel;
		this.toughnessExperienceBar.fillAmount = (flag ? 1f : ((float)num2 / (float)num));
		this.toughnessExperienceText.text = (flag ? "MAX." : string.Format("{0}/{1}", num2, num));
		this.toughnessLevelText.text = this.FormatAttributeLevelText(adjustedAttribute);
		float defense = this.uiSystemModule.AttributeModule.Defense;
		float resistance = this.uiSystemModule.AttributeModule.Resistance;
		this.toughnessDescriptionText.text = string.Format(LanguageManager.Instance.GetText("attribute_slot_toughness_description"), this.FormatAttributeText(defense, 2, ""), this.FormatAttributeText(resistance, 2, ""));
	}

	// Token: 0x06000C4A RID: 3146 RVA: 0x000371CC File Offset: 0x000353CC
	public void UpdateAgilityUI()
	{
		global::Attribute adjustedAttribute = this.uiSystemModule.AttributeModule.GetAdjustedAttribute(AttributeType.Agility);
		long experience = adjustedAttribute.Experience;
		long experienceToLevel = adjustedAttribute.ExperienceToLevel;
		long experienceToCurrentLevel = adjustedAttribute.ExperienceToCurrentLevel;
		long num = experienceToLevel - experienceToCurrentLevel;
		long num2 = num - (experienceToLevel - experience);
		bool flag = adjustedAttribute.Level >= adjustedAttribute.MaxLevel;
		this.agilityExperienceBar.fillAmount = (flag ? 1f : ((float)num2 / (float)num));
		this.agilityExperienceText.text = (flag ? "MAX." : string.Format("{0}/{1}", num2, num));
		this.agilityLevelText.text = this.FormatAttributeLevelText(adjustedAttribute);
		float speed = this.uiSystemModule.AttributeModule.Speed;
		int maxEndurance = this.uiSystemModule.AttributeModule.MaxEndurance;
		this.agilityDescriptionText.text = string.Format(LanguageManager.Instance.GetText("attribute_slot_agility_description"), this.FormatAttributeText(speed, 2, ""), this.FormatAttributeText((float)maxEndurance, 0, ""));
	}

	// Token: 0x06000C4B RID: 3147 RVA: 0x000372E0 File Offset: 0x000354E0
	public void UpdatePrecisionUI()
	{
		global::Attribute adjustedAttribute = this.uiSystemModule.AttributeModule.GetAdjustedAttribute(AttributeType.Precision);
		long experience = adjustedAttribute.Experience;
		long experienceToLevel = adjustedAttribute.ExperienceToLevel;
		long experienceToCurrentLevel = adjustedAttribute.ExperienceToCurrentLevel;
		long num = experienceToLevel - experienceToCurrentLevel;
		long num2 = num - (experienceToLevel - experience);
		bool flag = adjustedAttribute.Level >= adjustedAttribute.MaxLevel;
		this.precisionExperienceBar.fillAmount = (flag ? 1f : ((float)num2 / (float)num));
		this.precisionExperienceText.text = (flag ? "MAX." : string.Format("{0}/{1}", num2, num));
		this.precisionLevelText.text = this.FormatAttributeLevelText(adjustedAttribute);
		float value = this.uiSystemModule.AttributeModule.CriticalChance * 100f;
		this.precisionDescriptionText.text = string.Format(LanguageManager.Instance.GetText("attribute_slot_precision_description"), this.FormatAttributeText(value, 2, "%"));
	}

	// Token: 0x06000C4C RID: 3148 RVA: 0x000373DC File Offset: 0x000355DC
	public void UpdateVitalityUI()
	{
		global::Attribute adjustedAttribute = this.uiSystemModule.AttributeModule.GetAdjustedAttribute(AttributeType.Vitality);
		long experience = adjustedAttribute.Experience;
		long experienceToLevel = adjustedAttribute.ExperienceToLevel;
		long experienceToCurrentLevel = adjustedAttribute.ExperienceToCurrentLevel;
		long num = experienceToLevel - experienceToCurrentLevel;
		long num2 = num - (experienceToLevel - experience);
		bool flag = adjustedAttribute.Level >= adjustedAttribute.MaxLevel;
		this.vitalityExperienceBar.fillAmount = (flag ? 1f : ((float)num2 / (float)num));
		this.vitalityExperienceText.text = (flag ? "MAX." : string.Format("{0}/{1}", num2, num));
		this.vitalityLevelText.text = this.FormatAttributeLevelText(adjustedAttribute);
		int maxHealth = this.uiSystemModule.AttributeModule.MaxHealth;
		int regenerationAmount = this.uiSystemModule.AttributeModule.RegenerationAmount;
		float regenerationInterval = this.uiSystemModule.AttributeModule.RegenerationInterval;
		this.vitalityDescriptionText.text = string.Format(LanguageManager.Instance.GetText("attribute_slot_vitality_description"), this.FormatAttributeText((float)maxHealth, 0, ""), this.FormatAttributeText((float)regenerationAmount, 0, ""), this.FormatAttributeText(regenerationInterval, 2, "s"));
	}

	// Token: 0x06000C4D RID: 3149 RVA: 0x00037514 File Offset: 0x00035714
	private string FormatAttributeLevelText(global::Attribute attribute)
	{
		int num = attribute.LevelBlessingModifier - attribute.LevelCurseModifier;
		if (num > 0)
		{
			return string.Format(LanguageManager.Instance.GetText("attribute_level_label"), attribute.BaseLevel, attribute.Level, string.Format("{0}<color=green> (+{1})</color>", attribute.AdjustedLevel, num));
		}
		if (num < 0)
		{
			return string.Format(LanguageManager.Instance.GetText("attribute_level_label"), attribute.BaseLevel, attribute.Level, string.Format("{0}<color=red> ({1})</color>", attribute.AdjustedLevel, num));
		}
		return string.Format(LanguageManager.Instance.GetText("attribute_level_label"), attribute.BaseLevel, attribute.Level, attribute.AdjustedLevel);
	}

	// Token: 0x06000C4E RID: 3150 RVA: 0x000375FC File Offset: 0x000357FC
	private string FormatAttributeText(float value, int valueDecimals, string suffix = "")
	{
		string str = (valueDecimals <= 1) ? string.Empty : ".".PadRight(valueDecimals + 1, '0');
		return string.Format("{0:0" + str + "}{1}", value, suffix);
	}

	// Token: 0x06000C4F RID: 3151 RVA: 0x00037640 File Offset: 0x00035840
	public void OnTrainingModeChanged()
	{
		this.uiSystemModule.AttributeModule.CmdSetTrainingMode((TrainingMode)this.trainingModeSlider.value);
	}

	// Token: 0x04000D1D RID: 3357
	[SerializeField]
	private UISystemModule uiSystemModule;

	// Token: 0x04000D1E RID: 3358
	[SerializeField]
	private UISliderExtended trainingModeSlider;

	// Token: 0x04000D1F RID: 3359
	[SerializeField]
	private Text powerLevelText;

	// Token: 0x04000D20 RID: 3360
	[SerializeField]
	private Text powerDescriptionText;

	// Token: 0x04000D21 RID: 3361
	[SerializeField]
	private Image powerExperienceBar;

	// Token: 0x04000D22 RID: 3362
	[SerializeField]
	private Text powerExperienceText;

	// Token: 0x04000D23 RID: 3363
	[SerializeField]
	private Text toughnessLevelText;

	// Token: 0x04000D24 RID: 3364
	[SerializeField]
	private Text toughnessDescriptionText;

	// Token: 0x04000D25 RID: 3365
	[SerializeField]
	private Image toughnessExperienceBar;

	// Token: 0x04000D26 RID: 3366
	[SerializeField]
	private Text toughnessExperienceText;

	// Token: 0x04000D27 RID: 3367
	[SerializeField]
	private Text agilityLevelText;

	// Token: 0x04000D28 RID: 3368
	[SerializeField]
	private Text agilityDescriptionText;

	// Token: 0x04000D29 RID: 3369
	[SerializeField]
	private Image agilityExperienceBar;

	// Token: 0x04000D2A RID: 3370
	[SerializeField]
	private Text agilityExperienceText;

	// Token: 0x04000D2B RID: 3371
	[SerializeField]
	private Text precisionLevelText;

	// Token: 0x04000D2C RID: 3372
	[SerializeField]
	private Text precisionDescriptionText;

	// Token: 0x04000D2D RID: 3373
	[SerializeField]
	private Image precisionExperienceBar;

	// Token: 0x04000D2E RID: 3374
	[SerializeField]
	private Text precisionExperienceText;

	// Token: 0x04000D2F RID: 3375
	[SerializeField]
	private Text vitalityLevelText;

	// Token: 0x04000D30 RID: 3376
	[SerializeField]
	private Text vitalityDescriptionText;

	// Token: 0x04000D31 RID: 3377
	[SerializeField]
	private Image vitalityExperienceBar;

	// Token: 0x04000D32 RID: 3378
	[SerializeField]
	private Text vitalityExperienceText;
}
