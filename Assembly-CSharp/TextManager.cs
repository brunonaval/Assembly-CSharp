using System;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000248 RID: 584
public class TextManager : MonoBehaviour
{
	// Token: 0x0600086A RID: 2154 RVA: 0x00028363 File Offset: 0x00026563
	private void Awake()
	{
		if (NetworkServer.active)
		{
			return;
		}
		this.textComponent = base.GetComponent<Text>();
		this.textMeshProComponent = base.GetComponent<TextMeshPro>();
	}

	// Token: 0x0600086B RID: 2155 RVA: 0x00028388 File Offset: 0x00026588
	private void Start()
	{
		if (NetworkServer.active)
		{
			base.enabled = false;
			return;
		}
		string text = LanguageManager.Instance.GetText(this.key);
		if (this.lowerCase)
		{
			text = ((text != null) ? text.ToLower() : null);
		}
		if (this.upperCase)
		{
			text = ((text != null) ? text.ToUpper() : null);
		}
		if (this.textComponent != null)
		{
			this.textComponent.text = text;
		}
		if (this.textMeshProComponent != null)
		{
			this.textMeshProComponent.text = text;
		}
	}

	// Token: 0x04000A38 RID: 2616
	[SerializeField]
	private string key;

	// Token: 0x04000A39 RID: 2617
	[SerializeField]
	private bool lowerCase;

	// Token: 0x04000A3A RID: 2618
	[SerializeField]
	private bool upperCase;

	// Token: 0x04000A3B RID: 2619
	private Text textComponent;

	// Token: 0x04000A3C RID: 2620
	private TextMeshPro textMeshProComponent;
}
