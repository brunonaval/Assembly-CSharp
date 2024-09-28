using System;
using UnityEngine;

// Token: 0x02000224 RID: 548
public class SafeAreaManager : MonoBehaviour
{
	// Token: 0x06000779 RID: 1913 RVA: 0x000241F6 File Offset: 0x000223F6
	private void Start()
	{
		this.parentRectTransform = base.GetComponentInParent<RectTransform>();
	}

	// Token: 0x0600077A RID: 1914 RVA: 0x00024204 File Offset: 0x00022404
	private void Update()
	{
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			return;
		}
		if (this.lastSafeArea == Screen.safeArea & this.lastOrientation == Screen.orientation)
		{
			return;
		}
		this.ApplySafeArea();
	}

	// Token: 0x0600077B RID: 1915 RVA: 0x00024238 File Offset: 0x00022438
	private void ApplySafeArea()
	{
		Rect safeArea = Screen.safeArea;
		float num = this.parentRectTransform.rect.width / (float)Screen.width;
		float x = safeArea.xMin * num;
		float x2 = -((float)Screen.width - safeArea.xMax) * num;
		float yMin = safeArea.yMin;
		int height = Screen.height;
		float yMax = safeArea.yMax;
		RectTransform component = base.GetComponent<RectTransform>();
		if (Screen.orientation == ScreenOrientation.LandscapeLeft)
		{
			component.offsetMin = new Vector2(x, 0f);
			component.offsetMax = Vector2.zero;
		}
		if (Screen.orientation == ScreenOrientation.LandscapeRight)
		{
			component.offsetMin = Vector2.zero;
			component.offsetMax = new Vector2(x2, 0f);
		}
		this.lastSafeArea = Screen.safeArea;
		this.lastOrientation = Screen.orientation;
	}

	// Token: 0x0400095D RID: 2397
	private ScreenOrientation lastOrientation;

	// Token: 0x0400095E RID: 2398
	private Rect lastSafeArea;

	// Token: 0x0400095F RID: 2399
	private RectTransform parentRectTransform;
}
