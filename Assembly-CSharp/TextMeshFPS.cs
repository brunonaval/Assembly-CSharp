using System;
using UnityEngine;

// Token: 0x02000567 RID: 1383
public class TextMeshFPS : MonoBehaviour
{
	// Token: 0x06001ED2 RID: 7890 RVA: 0x00099E60 File Offset: 0x00098060
	private void OnApplicationQuit()
	{
		TextMeshFPS.instance = null;
	}

	// Token: 0x06001ED3 RID: 7891 RVA: 0x00099E68 File Offset: 0x00098068
	private void Start()
	{
		if (TextMeshFPS.instance)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			TextMeshFPS.instance = this;
		}
		this.timeleft = this.updateInterval;
		this._textMesh = base.transform.GetComponent<TextMesh>();
	}

	// Token: 0x06001ED4 RID: 7892 RVA: 0x00099EA8 File Offset: 0x000980A8
	private void Update()
	{
		this.timeleft -= Time.deltaTime;
		this.accum += Time.timeScale / Time.deltaTime;
		this.frames += 1f;
		if (this.timeleft <= 0f)
		{
			this._textMesh.text = ((this.accum / this.frames).ToString("f2") ?? "");
			this.timeleft = this.updateInterval;
			this.accum = 0f;
			this.frames = 0f;
		}
	}

	// Token: 0x040018A2 RID: 6306
	private TextMesh _textMesh;

	// Token: 0x040018A3 RID: 6307
	private float updateInterval = 0.5f;

	// Token: 0x040018A4 RID: 6308
	private float accum;

	// Token: 0x040018A5 RID: 6309
	private float frames;

	// Token: 0x040018A6 RID: 6310
	private float timeleft;

	// Token: 0x040018A7 RID: 6311
	public static TextMeshFPS instance;
}
