using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Token: 0x020004B6 RID: 1206
public class AnimatedTextRenderer : MonoBehaviour
{
	// Token: 0x06001B22 RID: 6946 RVA: 0x0008A9E4 File Offset: 0x00088BE4
	private void Awake()
	{
		this.duration = 0.5f;
		this.textMesh = base.GetComponent<TextMeshPro>();
		this.originalFontColor = this.textMesh.color;
		this.originalFontSize = this.textMesh.fontSize;
	}

	// Token: 0x06001B23 RID: 6947 RVA: 0x0008AA20 File Offset: 0x00088C20
	private void Update()
	{
		if (this.started & this.animate)
		{
			base.transform.position += Vector3.up * 0.02f;
			this.textMesh.fontSize += 0.001f;
			Color color = this.textMesh.color;
			color.a -= 1f * Time.deltaTime;
			this.textMesh.color = color;
			if (color.a == 0f)
			{
				this.StopRunning();
			}
		}
	}

	// Token: 0x06001B24 RID: 6948 RVA: 0x0008AAB9 File Offset: 0x00088CB9
	private void OnDisable()
	{
		this.textMesh.fontSize = this.originalFontSize;
		this.textMesh.color = this.originalFontColor;
	}

	// Token: 0x06001B25 RID: 6949 RVA: 0x0008AAE0 File Offset: 0x00088CE0
	public void StartRunning(Color color, string text, bool animate)
	{
		this.animate = animate;
		if (!animate)
		{
			this.duration = 4f;
			base.transform.position += new Vector3(0f, 0.48f);
			base.StartCoroutine(this.RunNonAnimated());
		}
		this.started = true;
		this.textMesh.text = GlobalUtils.SpliceWords(text, 25);
		this.textMesh.color = color;
		base.Invoke("StopRunning", this.duration);
	}

	// Token: 0x06001B26 RID: 6950 RVA: 0x0008AB6B File Offset: 0x00088D6B
	private IEnumerator RunNonAnimated()
	{
		WaitForSeconds waitForOneSecond = new WaitForSeconds(1f);
		do
		{
			base.transform.position += new Vector3(0f, 0.16f);
			yield return waitForOneSecond;
		}
		while (this.started);
		yield break;
	}

	// Token: 0x06001B27 RID: 6951 RVA: 0x0008AB7A File Offset: 0x00088D7A
	private void StopRunning()
	{
		this.started = false;
		base.gameObject.SetActive(false);
	}

	// Token: 0x0400169C RID: 5788
	[SerializeField]
	private float duration;

	// Token: 0x0400169D RID: 5789
	private bool animate;

	// Token: 0x0400169E RID: 5790
	private bool started;

	// Token: 0x0400169F RID: 5791
	private TextMeshPro textMesh;

	// Token: 0x040016A0 RID: 5792
	private Color originalFontColor;

	// Token: 0x040016A1 RID: 5793
	private float originalFontSize;
}
