using System;
using UnityEngine;

namespace GGEZ
{
	// Token: 0x020006C3 RID: 1731
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[HelpURL("http://ggez.org/posts/perfect-pixel-camera/")]
	[DisallowMultipleComponent]
	[AddComponentMenu("GGEZ/Camera/Perfect Pixel Camera")]
	public class PerfectPixelCamera : MonoBehaviour
	{
		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x060025D3 RID: 9683 RVA: 0x000B5788 File Offset: 0x000B3988
		// (set) Token: 0x060025D4 RID: 9684 RVA: 0x000B5790 File Offset: 0x000B3990
		public float SnapSizeWorldUnits { get; private set; }

		// Token: 0x060025D5 RID: 9685 RVA: 0x000B5799 File Offset: 0x000B3999
		private void OnEnable()
		{
			this.cameraComponent = (Camera)base.GetComponent(typeof(Camera));
			this.LateUpdate();
		}

		// Token: 0x060025D6 RID: 9686 RVA: 0x000B57BC File Offset: 0x000B39BC
		private void OnDisable()
		{
			if (this.cameraComponent == null)
			{
				return;
			}
			this.cameraComponent.ResetProjectionMatrix();
			this.cameraComponent = null;
		}

		// Token: 0x060025D7 RID: 9687 RVA: 0x000B57E0 File Offset: 0x000B39E0
		private void LateUpdate()
		{
			Camera camera = this.cameraComponent;
			camera.transparencySortMode = TransparencySortMode.Orthographic;
			camera.orthographic = true;
			camera.transform.rotation = Quaternion.identity;
			camera.orthographicSize = Mathf.Max(camera.orthographicSize, 1E-05f);
			Rect pixelRect = camera.pixelRect;
			float num = (float)this.TexturePixelsPerWorldUnit;
			float num2 = Mathf.Max(1f, Mathf.Ceil(1f * pixelRect.height / (camera.orthographicSize * 2f * num)));
			float num3 = 1f * pixelRect.width / (num2 * 2f * num);
			float num4 = 1f * pixelRect.height / (num2 * 2f * num);
			float num5 = 1f / (num2 * num);
			float num6 = 0f * num5;
			float num7 = num6 - Mathf.Repeat(num5 + Mathf.Repeat(camera.transform.position.x, num5), num5);
			float num8 = num6 - Mathf.Repeat(num5 + Mathf.Repeat(camera.transform.position.y, num5), num5);
			this.SnapSizeWorldUnits = num5;
			camera.projectionMatrix = Matrix4x4.Ortho(-num3 + num7, num3 + num7, -num4 + num8, num4 + num8, camera.nearClipPlane, camera.farClipPlane);
		}

		// Token: 0x04001F00 RID: 7936
		[Tooltip("The number of texture pixels that fit in 1.0 world units. Common values are 8, 16, 32 and 64. If you're making a tile-based game, this is your tile size.")]
		[Range(1f, 64f)]
		public int TexturePixelsPerWorldUnit = 16;

		// Token: 0x04001F01 RID: 7937
		private Camera cameraComponent;

		// Token: 0x04001F02 RID: 7938
		private const float halfPixelOffsetIfNeededForD3D = 0f;
	}
}
