using System;
using UnityEngine;

// Token: 0x02000223 RID: 547
public class MobileWindowManager : MonoBehaviour
{
	// Token: 0x06000774 RID: 1908 RVA: 0x000240DC File Offset: 0x000222DC
	private void Start()
	{
		this.GrowWindowRectOnMobile();
		this.GowVerticalScrollbarOnMobile();
		this.ScaleWindowRectOnMobile();
	}

	// Token: 0x06000775 RID: 1909 RVA: 0x000240F0 File Offset: 0x000222F0
	private void ScaleWindowRectOnMobile()
	{
		if (!GlobalSettings.IsMobilePlatform)
		{
			return;
		}
		if (this.windowRectTransform == null)
		{
			return;
		}
		this.windowRectTransform.localScale = new Vector2(this.windowScale, this.windowScale);
	}

	// Token: 0x06000776 RID: 1910 RVA: 0x0002412C File Offset: 0x0002232C
	private void GowVerticalScrollbarOnMobile()
	{
		if (!GlobalSettings.IsMobilePlatform)
		{
			return;
		}
		if (this.verticalScrollbarRectTransform == null)
		{
			return;
		}
		this.verticalScrollbarRectTransform.sizeDelta = new Vector2(this.verticalScrollbarGrowth - this.verticalScrollbarRectTransform.sizeDelta.x, this.verticalScrollbarRectTransform.sizeDelta.y);
	}

	// Token: 0x06000777 RID: 1911 RVA: 0x00024188 File Offset: 0x00022388
	private void GrowWindowRectOnMobile()
	{
		if (!GlobalSettings.IsMobilePlatform)
		{
			return;
		}
		if (this.windowRectTransform == null)
		{
			return;
		}
		this.windowRectTransform.sizeDelta = new Vector2(this.windowRectTransform.sizeDelta.x + this.windowGrowth, this.windowRectTransform.sizeDelta.y);
	}

	// Token: 0x04000958 RID: 2392
	[SerializeField]
	private RectTransform windowRectTransform;

	// Token: 0x04000959 RID: 2393
	[SerializeField]
	private RectTransform verticalScrollbarRectTransform;

	// Token: 0x0400095A RID: 2394
	[SerializeField]
	private float windowGrowth;

	// Token: 0x0400095B RID: 2395
	[SerializeField]
	private float verticalScrollbarGrowth;

	// Token: 0x0400095C RID: 2396
	[SerializeField]
	private float windowScale = 1f;
}
