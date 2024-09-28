using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000344 RID: 836
public class HudModule : MonoBehaviour
{
	// Token: 0x0600105C RID: 4188 RVA: 0x0004CE98 File Offset: 0x0004B098
	private void Start()
	{
		Sprite vocationIconSprite = ResourcesManager.Instance.GetVocationIconSprite(this.uiSystemModule.VocationModule.Vocation.ToString().ToLower());
		this.vocationIcon.sprite = vocationIconSprite;
	}

	// Token: 0x0600105D RID: 4189 RVA: 0x0004CEDC File Offset: 0x0004B0DC
	private void Update()
	{
		if (this.uiSystemModule == null)
		{
			return;
		}
		if (this.uiSystemModule.AttributeModule == null)
		{
			return;
		}
		int baseLevel = this.uiSystemModule.AttributeModule.BaseLevel;
		int maxHealth = this.uiSystemModule.AttributeModule.MaxHealth;
		int currentHealth = this.uiSystemModule.AttributeModule.CurrentHealth;
		int maxEndurance = this.uiSystemModule.AttributeModule.MaxEndurance;
		int currentEndurance = this.uiSystemModule.AttributeModule.CurrentEndurance;
		long baseExperienceToLevel = this.uiSystemModule.AttributeModule.BaseExperienceToLevel;
		long baseExperienceToCurrentLevel = this.uiSystemModule.AttributeModule.BaseExperienceToCurrentLevel;
		long baseExperience = this.uiSystemModule.AttributeModule.BaseExperience;
		this.goldCoinsText.text = GlobalUtils.FormatLongNumber(this.uiSystemModule.WalletModule.GoldCoins, LanguageManager.Instance.GetText("api_culture"));
		this.gemsText.text = this.uiSystemModule.WalletModule.Gems.ToString();
		long num = baseExperienceToLevel - baseExperienceToCurrentLevel;
		long num2 = num - (baseExperienceToLevel - baseExperience);
		this.baseLevelText.text = baseLevel.ToString();
		this.lifeBarText.text = currentHealth.ToString() + "/" + maxHealth.ToString();
		this.experienceBarText.text = string.Format("{0}/{1} ({2} EXP/m | ({3} AXP/m)", new object[]
		{
			num2,
			num,
			this.uiSystemModule.AttributeModule.ExperiencePerMinute,
			this.uiSystemModule.AttributeModule.AttributeExperiencePerMinute
		});
		this.enduranceBarText.text = currentEndurance.ToString() + "/" + maxEndurance.ToString();
		float num3 = (float)currentHealth * 100f / (float)maxHealth;
		this.lifeBar.fillAmount = num3 / 100f;
		this.healthGlobe.fillAmount = this.lifeBar.fillAmount;
		float num4 = (float)num2 * 100f / (float)num;
		this.experienceBar.fillAmount = num4 / 100f;
		float num5 = (float)currentEndurance * 100f / (float)maxEndurance;
		this.enduranceBar.fillAmount = num5 / 100f;
		this.enduranceGlobe.fillAmount = this.enduranceBar.fillAmount;
	}

	// Token: 0x04000FD5 RID: 4053
	[SerializeField]
	private UISystemModule uiSystemModule;

	// Token: 0x04000FD6 RID: 4054
	[SerializeField]
	private Text lifeBarText;

	// Token: 0x04000FD7 RID: 4055
	[SerializeField]
	private Image lifeBar;

	// Token: 0x04000FD8 RID: 4056
	[SerializeField]
	private Image healthGlobe;

	// Token: 0x04000FD9 RID: 4057
	[SerializeField]
	private Image enduranceGlobe;

	// Token: 0x04000FDA RID: 4058
	[SerializeField]
	private Text experienceBarText;

	// Token: 0x04000FDB RID: 4059
	[SerializeField]
	private Image experienceBar;

	// Token: 0x04000FDC RID: 4060
	[SerializeField]
	private Text enduranceBarText;

	// Token: 0x04000FDD RID: 4061
	[SerializeField]
	private Image enduranceBar;

	// Token: 0x04000FDE RID: 4062
	[SerializeField]
	private Text baseLevelText;

	// Token: 0x04000FDF RID: 4063
	[SerializeField]
	private Image vocationIcon;

	// Token: 0x04000FE0 RID: 4064
	[SerializeField]
	private Text goldCoinsText;

	// Token: 0x04000FE1 RID: 4065
	[SerializeField]
	private Text gemsText;
}
