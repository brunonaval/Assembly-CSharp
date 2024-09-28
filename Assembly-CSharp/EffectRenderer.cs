using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004B8 RID: 1208
[RequireComponent(typeof(SpriteRenderer))]
public class EffectRenderer : MonoBehaviour
{
	// Token: 0x06001B2F RID: 6959 RVA: 0x0008AC2D File Offset: 0x00088E2D
	private void Awake()
	{
		base.TryGetComponent<SpriteRenderer>(out this.spriteRenderer);
	}

	// Token: 0x06001B30 RID: 6960 RVA: 0x0008AC3C File Offset: 0x00088E3C
	private void Start()
	{
		this.startTime = Time.time;
	}

	// Token: 0x06001B31 RID: 6961 RVA: 0x0008AC49 File Offset: 0x00088E49
	private void OnDisable()
	{
		base.transform.localScale = Vector3.one;
	}

	// Token: 0x06001B32 RID: 6962 RVA: 0x0008AC5B File Offset: 0x00088E5B
	private void Update()
	{
		if (this.loopDuration == 0f)
		{
			return;
		}
		if (Time.time - this.startTime >= this.loopDuration)
		{
			this.loopDuration = 0f;
			this.DisableAndRemoveParent();
		}
	}

	// Token: 0x06001B33 RID: 6963 RVA: 0x0008AC90 File Offset: 0x00088E90
	public void RunAnimation(string spriteSheet, float scaleModifier, float speedModifier, float loopTime)
	{
		if (string.IsNullOrEmpty(spriteSheet))
		{
			return;
		}
		this.sprites = AssetBundleManager.Instance.GetEffectSprites(spriteSheet);
		if (this.sprites == null)
		{
			return;
		}
		this.startTime = Time.time;
		this.loopDuration = loopTime;
		base.StartCoroutine(this.Run(scaleModifier, speedModifier));
	}

	// Token: 0x06001B34 RID: 6964 RVA: 0x0008ACE2 File Offset: 0x00088EE2
	private IEnumerator Run(float scaleModifier, float speedModifier)
	{
		scaleModifier = ((scaleModifier == 0f) ? 1f : scaleModifier);
		speedModifier = ((speedModifier == 0f) ? 1f : speedModifier);
		if (this.sprites != null)
		{
			base.transform.localScale = base.transform.localScale * scaleModifier;
			WaitForSeconds delay = new WaitForSeconds(0.1f * speedModifier);
			do
			{
				foreach (Sprite sprite in this.sprites)
				{
					this.spriteRenderer.sprite = sprite;
					yield return delay;
				}
				Sprite[] array = null;
			}
			while (this.loopDuration != 0f);
			delay = null;
		}
		this.DisableAndRemoveParent();
		yield break;
	}

	// Token: 0x06001B35 RID: 6965 RVA: 0x0008ACFF File Offset: 0x00088EFF
	private void DisableAndRemoveParent()
	{
		this.loopDuration = 0f;
		base.gameObject.transform.SetParent(null, false);
		base.gameObject.SetActive(false);
	}

	// Token: 0x040016A6 RID: 5798
	private Sprite[] sprites;

	// Token: 0x040016A7 RID: 5799
	private SpriteRenderer spriteRenderer;

	// Token: 0x040016A8 RID: 5800
	private float loopDuration;

	// Token: 0x040016A9 RID: 5801
	private float startTime;
}
