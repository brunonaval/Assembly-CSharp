using System;
using UnityEngine;
using UnityEngine.UI;

namespace GGEZ
{
	// Token: 0x020006C2 RID: 1730
	public class ZoomDemo : MonoBehaviour
	{
		// Token: 0x060025CF RID: 9679 RVA: 0x000B543C File Offset: 0x000B363C
		public void SwitchModes()
		{
			switch (this.mode)
			{
			case ZoomDemoMode.Bubbling:
				this.mode = ZoomDemoMode.Marching;
				this.SwitchModesButtonText.text = "Switch to Shimmering";
				break;
			case ZoomDemoMode.Shimmering:
				this.mode = ZoomDemoMode.Bubbling;
				this.SwitchModesButtonText.text = "Switch to Marching";
				break;
			case ZoomDemoMode.Marching:
				this.mode = ZoomDemoMode.Shimmering;
				this.SwitchModesButtonText.text = "Switch to Bubbling";
				break;
			}
			this.TitleText.text = this.mode.ToString();
		}

		// Token: 0x060025D0 RID: 9680 RVA: 0x000B54C9 File Offset: 0x000B36C9
		private void Start()
		{
			this.SwitchModes();
		}

		// Token: 0x060025D1 RID: 9681 RVA: 0x000B54D4 File Offset: 0x000B36D4
		private void Update()
		{
			PerfectPixelCamera perfectPixelCamera = this.MainCamera.GetComponent(typeof(PerfectPixelCamera)) as PerfectPixelCamera;
			bool flag = perfectPixelCamera != null && perfectPixelCamera.isActiveAndEnabled;
			float orthographicSize = this.MainCamera.orthographicSize;
			float num = orthographicSize;
			if (flag)
			{
				float num2 = Mathf.Max(1f, Mathf.Ceil(1f * this.MainCamera.pixelRect.height / (this.MainCamera.orthographicSize * 2f * (float)perfectPixelCamera.TexturePixelsPerWorldUnit)));
				num = 1f * this.MainCamera.pixelRect.height / (num2 * 2f * (float)perfectPixelCamera.TexturePixelsPerWorldUnit);
			}
			switch (this.mode)
			{
			case ZoomDemoMode.Bubbling:
				this.MainCamera.transform.position = new Vector3(Mathf.Cos(Time.time * 0.5f) * 5f, Mathf.Sin(Time.time * 0.5f) * 5f, -10f);
				if (flag)
				{
					this.BodyText.text = "With the PerfectPixelCamera enabled, panning doesn't jiggle anymore!";
					return;
				}
				this.BodyText.text = "When panning, the image appears to jiggle and bubble. Add the PerfectPixelCamera component to the MainCamera object to check out the fix, or tap the button to see other errors.";
				return;
			case ZoomDemoMode.Shimmering:
				this.MainCamera.orthographicSize = this.TimeToOrthographicSize.Evaluate(Time.time);
				this.MainCamera.transform.position = Vector3.back * 10f;
				if (flag)
				{
					this.BodyText.text = string.Concat(new string[]
					{
						"The Perfect Pixel Camera snaps the orthographicSize of the camera component behind the scenes to fix zooming issues (",
						orthographicSize.ToString("0.0"),
						" snapped to ",
						num.ToString("0.0"),
						")"
					});
					return;
				}
				this.BodyText.text = "A miscalibrated camera shows a shimmer or wave pattern as it zooms. Add the PerfectPixelCamera component to the MainCamera object to check out the fix, or tap the button to see other errors.";
				return;
			case ZoomDemoMode.Marching:
				this.MainCamera.orthographicSize = (float)this.MainCamera.pixelHeight / 32f * 0.5f;
				this.MainCamera.transform.position = new Vector3((Mathf.PingPong(Time.time * 0.1f, 2f) - 1f) * 0.1f, (Mathf.PingPong(Time.time * 0.1f, 2f) - 1f) * 3f, -10f);
				if (flag)
				{
					this.BodyText.text = "With the PerfectPixelCamera enabled, the camera's projection matrix is automatically offset by less than a pixel and marching is fixed.";
					return;
				}
				this.BodyText.text = "Even with orthographicSize set correctly to " + this.MainCamera.orthographicSize.ToString("0.0") + ", an alignment issue is easily visible at the border at max zoom. Use the scale slider to zoom in. Add the PerfectPixelCamera component to the MainCamera object to check out the fix, or tap the button to see other errors.";
				return;
			default:
				return;
			}
		}

		// Token: 0x04001EFA RID: 7930
		public Camera MainCamera;

		// Token: 0x04001EFB RID: 7931
		public AnimationCurve TimeToOrthographicSize;

		// Token: 0x04001EFC RID: 7932
		public Text TitleText;

		// Token: 0x04001EFD RID: 7933
		public Text BodyText;

		// Token: 0x04001EFE RID: 7934
		public Text SwitchModesButtonText;

		// Token: 0x04001EFF RID: 7935
		private ZoomDemoMode mode = ZoomDemoMode.Marching;
	}
}
