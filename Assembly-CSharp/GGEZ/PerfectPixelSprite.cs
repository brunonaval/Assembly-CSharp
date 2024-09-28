using System;
using UnityEngine;

namespace GGEZ
{
	// Token: 0x020006C4 RID: 1732
	[HelpURL("http://ggez.org/posts/perfect-pixel-sprite/")]
	[DisallowMultipleComponent]
	[AddComponentMenu("GGEZ/Sprite/Perfect Pixel Sprite")]
	public class PerfectPixelSprite : MonoBehaviour
	{
		// Token: 0x060025D9 RID: 9689 RVA: 0x000B5936 File Offset: 0x000B3B36
		private void OnEnable()
		{
			if (base.transform.parent == null)
			{
				base.enabled = false;
			}
		}

		// Token: 0x060025DA RID: 9690 RVA: 0x000B5952 File Offset: 0x000B3B52
		private void OnDisable()
		{
			base.transform.localPosition = Vector3.zero;
		}

		// Token: 0x060025DB RID: 9691 RVA: 0x000B5964 File Offset: 0x000B3B64
		private void LateUpdate()
		{
			Camera main = Camera.main;
			float num = (float)this.TexturePixelsPerWorldUnit;
			float num2 = 1f / (Mathf.Max(1f, Mathf.Ceil(1f * main.pixelRect.height / (main.orthographicSize * 2f * num))) * num);
			Vector3 position = base.transform.parent.position;
			base.transform.localPosition = new Vector3(-PerfectPixelSprite.repeatUniform(position.x, num2), num2 * 0.5f - PerfectPixelSprite.repeatUniform(position.y, num2), 0f);
		}

		// Token: 0x060025DC RID: 9692 RVA: 0x000B5A04 File Offset: 0x000B3C04
		private static float repeatUniform(float number, float range)
		{
			float num = number - (float)((int)(number / range)) * range;
			if (num < 0f)
			{
				num += range;
			}
			return num;
		}

		// Token: 0x04001F04 RID: 7940
		[Tooltip("The number of texture pixels that fit in 1.0 world units. Common values are 8, 16, 32 and 64. If you're making a tile-based game, this is your tile size.")]
		[Range(1f, 64f)]
		public int TexturePixelsPerWorldUnit = 16;
	}
}
