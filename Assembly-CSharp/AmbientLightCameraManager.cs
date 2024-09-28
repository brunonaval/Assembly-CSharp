using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Token: 0x020001D0 RID: 464
public class AmbientLightCameraManager : MonoBehaviour
{
	// Token: 0x0600055F RID: 1375 RVA: 0x0001CE43 File Offset: 0x0001B043
	private void Start()
	{
		RenderPipelineManager.beginCameraRendering += this.RenderPipelineManager_beginCameraRendering;
		RenderPipelineManager.endCameraRendering += this.RenderPipelineManager_endCameraRendering;
	}

	// Token: 0x06000560 RID: 1376 RVA: 0x0001CE67 File Offset: 0x0001B067
	private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext arg1, Camera arg2)
	{
		if (!this.IgnoreAmbientLight | !arg2.CompareTag("MainCamera"))
		{
			return;
		}
		this.globalLight.color = this.ambientLight;
	}

	// Token: 0x06000561 RID: 1377 RVA: 0x0001CE98 File Offset: 0x0001B098
	private void RenderPipelineManager_beginCameraRendering(ScriptableRenderContext arg1, Camera arg2)
	{
		if (!arg2.CompareTag("MainCamera"))
		{
			this.globalLight.color = new Color(1f, 1f, 1f);
			return;
		}
		if (!this.IgnoreAmbientLight)
		{
			return;
		}
		this.ambientLight = this.globalLight.color;
		this.globalLight.color = new Color(1f, 1f, 1f);
	}

	// Token: 0x040007D7 RID: 2007
	private Color ambientLight;

	// Token: 0x040007D8 RID: 2008
	public bool IgnoreAmbientLight;

	// Token: 0x040007D9 RID: 2009
	[SerializeField]
	private Light2D globalLight;
}
