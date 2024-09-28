using System;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200028F RID: 655
public class StorylineWindowManager : MonoBehaviour
{
	// Token: 0x06000A40 RID: 2624 RVA: 0x0002F376 File Offset: 0x0002D576
	private void OnEnable()
	{
		if (NetworkServer.active)
		{
			return;
		}
		this.storylineText.text = LanguageManager.Instance.GetText("storyline_initial");
	}

	// Token: 0x06000A41 RID: 2625 RVA: 0x0002F39A File Offset: 0x0002D59A
	public void SetStoryline(string text)
	{
		this.storylineText.text = text;
		this.scrollbar.value = 1f;
	}

	// Token: 0x04000BC3 RID: 3011
	[SerializeField]
	private Text storylineText;

	// Token: 0x04000BC4 RID: 3012
	[SerializeField]
	private Scrollbar scrollbar;
}
