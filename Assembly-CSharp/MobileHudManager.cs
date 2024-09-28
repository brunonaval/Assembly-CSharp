using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000215 RID: 533
public class MobileHudManager : MonoBehaviour
{
	// Token: 0x06000732 RID: 1842 RVA: 0x00023254 File Offset: 0x00021454
	private void Start()
	{
		Sprite vocationIconSprite = ResourcesManager.Instance.GetVocationIconSprite(this.uiSystemModule.VocationModule.Vocation.ToString().ToLower());
		this.vocationIcon.sprite = vocationIconSprite;
	}

	// Token: 0x06000733 RID: 1843 RVA: 0x00023298 File Offset: 0x00021498
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
		if (Time.time - this.updateTime < 0.5f)
		{
			return;
		}
		this.UpdateJoystickPosition();
		this.updateTime = Time.time;
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
		if (MobileExpBarManager.ShowExpPerMinute)
		{
			this.experienceBarText.text = string.Format("({0} EXP/m | ({1} AXP/m)", this.uiSystemModule.AttributeModule.ExperiencePerMinute, this.uiSystemModule.AttributeModule.AttributeExperiencePerMinute);
		}
		else
		{
			this.experienceBarText.text = string.Format("{0}/{1}", num2, num);
		}
		this.enduranceBarText.text = currentEndurance.ToString() + "/" + maxEndurance.ToString();
		float num3 = (float)currentHealth * 100f / (float)maxHealth;
		this.lifeBar.fillAmount = num3 / 100f;
		float num4 = (float)num2 * 100f / (float)num;
		this.experienceBar.fillAmount = num4 / 100f;
		float num5 = (float)currentEndurance * 100f / (float)maxEndurance;
		this.enduranceBar.fillAmount = num5 / 100f;
	}

	// Token: 0x06000734 RID: 1844 RVA: 0x00023500 File Offset: 0x00021700
	private void UpdateJoystickPosition()
	{
		if (SettingsManager.Instance.FixedJoystick)
		{
			this.floatingJoystick.SetActive(false);
			this.fixedJoystick.SetActive(true);
			Joystick movementJoystick;
			this.fixedJoystick.TryGetComponent<Joystick>(out movementJoystick);
			this.uiSystemModule.MovementJoystick = movementJoystick;
			return;
		}
		this.fixedJoystick.SetActive(false);
		this.floatingJoystick.SetActive(true);
		Joystick movementJoystick2;
		this.floatingJoystick.TryGetComponent<Joystick>(out movementJoystick2);
		this.uiSystemModule.MovementJoystick = movementJoystick2;
	}

	// Token: 0x0400092A RID: 2346
	[SerializeField]
	private UISystemModule uiSystemModule;

	// Token: 0x0400092B RID: 2347
	[SerializeField]
	private Text lifeBarText;

	// Token: 0x0400092C RID: 2348
	[SerializeField]
	private Image lifeBar;

	// Token: 0x0400092D RID: 2349
	[SerializeField]
	private Text enduranceBarText;

	// Token: 0x0400092E RID: 2350
	[SerializeField]
	private Image enduranceBar;

	// Token: 0x0400092F RID: 2351
	[SerializeField]
	private Text baseLevelText;

	// Token: 0x04000930 RID: 2352
	[SerializeField]
	private Image vocationIcon;

	// Token: 0x04000931 RID: 2353
	[SerializeField]
	private Text goldCoinsText;

	// Token: 0x04000932 RID: 2354
	[SerializeField]
	private Text gemsText;

	// Token: 0x04000933 RID: 2355
	[SerializeField]
	private Text experienceBarText;

	// Token: 0x04000934 RID: 2356
	[SerializeField]
	private Image experienceBar;

	// Token: 0x04000935 RID: 2357
	[SerializeField]
	private GameObject floatingJoystick;

	// Token: 0x04000936 RID: 2358
	[SerializeField]
	private GameObject fixedJoystick;

	// Token: 0x04000937 RID: 2359
	private float updateTime;
}
