using System;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001D9 RID: 473
public class AttributeWidgetManager : MonoBehaviour
{
	// Token: 0x0600058F RID: 1423 RVA: 0x0001D928 File Offset: 0x0001BB28
	private void Awake()
	{
		if (NetworkServer.active)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		GameObject.FindGameObjectWithTag("UISystem").TryGetComponent<UISystemModule>(out this.uiSystemModule);
	}

	// Token: 0x06000590 RID: 1424 RVA: 0x0001D954 File Offset: 0x0001BB54
	private void Update()
	{
		if (Time.time - this.lastUpdateTime > 0.5f)
		{
			this.UpdateAttributeUI(this.powerWidgetIcon, AttributeType.Power);
			this.UpdateAttributeUI(this.toughnessWidgetIcon, AttributeType.Toughness);
			this.UpdateAttributeUI(this.agilityWidgetIcon, AttributeType.Agility);
			this.UpdateAttributeUI(this.precisionWidgetIcon, AttributeType.Precision);
			this.UpdateAttributeUI(this.vitalityWidgetIcon, AttributeType.Vitality);
			this.lastUpdateTime = Time.time;
		}
	}

	// Token: 0x06000591 RID: 1425 RVA: 0x0001D9C0 File Offset: 0x0001BBC0
	private void UpdateAttributeUI(Image attributeWidgetIcon, AttributeType attributeType)
	{
		if (this.uiSystemModule == null)
		{
			return;
		}
		if (this.uiSystemModule.AttributeModule == null)
		{
			return;
		}
		global::Attribute adjustedAttribute = this.uiSystemModule.AttributeModule.GetAdjustedAttribute(attributeType);
		long experience = adjustedAttribute.Experience;
		long experienceToLevel = adjustedAttribute.ExperienceToLevel;
		long experienceToCurrentLevel = adjustedAttribute.ExperienceToCurrentLevel;
		long num = experienceToLevel - experienceToCurrentLevel;
		long num2 = num - (experienceToLevel - experience);
		attributeWidgetIcon.fillAmount = ((adjustedAttribute.Level >= adjustedAttribute.MaxLevel) ? 1f : ((float)num2 / (float)num));
	}

	// Token: 0x040007FA RID: 2042
	[SerializeField]
	private Image powerWidgetIcon;

	// Token: 0x040007FB RID: 2043
	[SerializeField]
	private Image toughnessWidgetIcon;

	// Token: 0x040007FC RID: 2044
	[SerializeField]
	private Image agilityWidgetIcon;

	// Token: 0x040007FD RID: 2045
	[SerializeField]
	private Image precisionWidgetIcon;

	// Token: 0x040007FE RID: 2046
	[SerializeField]
	private Image vitalityWidgetIcon;

	// Token: 0x040007FF RID: 2047
	private UISystemModule uiSystemModule;

	// Token: 0x04000800 RID: 2048
	private float lastUpdateTime;
}
