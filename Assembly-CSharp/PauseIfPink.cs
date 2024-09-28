using System;
using UnityEngine;

// Token: 0x02000004 RID: 4
public class PauseIfPink : MonoBehaviour
{
	// Token: 0x0600000B RID: 11 RVA: 0x0000219A File Offset: 0x0000039A
	private void Start()
	{
	}

	// Token: 0x0600000C RID: 12 RVA: 0x0000219C File Offset: 0x0000039C
	private void OnPostRender()
	{
		Texture2D texture2D = new Texture2D(this.renderTexture.width, this.renderTexture.height, TextureFormat.RGB24, false);
		texture2D.ReadPixels(new Rect(0f, 0f, (float)this.renderTexture.width, (float)this.renderTexture.height), 0, 0);
		texture2D.Apply();
		foreach (Color color in texture2D.GetPixels())
		{
			if (Mathf.Approximately(1f, color.r) && Mathf.Approximately(0f, color.g) && Mathf.Approximately(1f, color.b))
			{
				Debug.Break();
			}
		}
	}

	// Token: 0x04000003 RID: 3
	public RenderTexture renderTexture;
}
