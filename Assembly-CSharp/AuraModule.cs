using System;
using Mirror;
using UnityEngine;

// Token: 0x020002C5 RID: 709
public class AuraModule : MonoBehaviour
{
	// Token: 0x06000C51 RID: 3153 RVA: 0x00002E81 File Offset: 0x00001081
	private void Start()
	{
		if (NetworkServer.active)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000C52 RID: 3154 RVA: 0x00037660 File Offset: 0x00035860
	private void Update()
	{
		if (this.sprites != null && this.spriteRenderer.enabled)
		{
			int num = (int)(Time.timeSinceLevelLoad * this.framesPerSecond);
			num %= this.sprites.Length;
			this.spriteRenderer.sprite = this.sprites[num];
		}
	}

	// Token: 0x04000D33 RID: 3379
	[SerializeField]
	private Sprite[] sprites;

	// Token: 0x04000D34 RID: 3380
	[SerializeField]
	private SpriteRenderer spriteRenderer;

	// Token: 0x04000D35 RID: 3381
	[SerializeField]
	private float framesPerSecond = 60f;
}
