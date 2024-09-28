using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x0200062C RID: 1580
	[ExecuteInEditMode]
	public class UIWorldCanvasScaleByCamera : MonoBehaviour
	{
		// Token: 0x060022C0 RID: 8896 RVA: 0x000AC358 File Offset: 0x000AA558
		protected void Update()
		{
			if (this.m_Camera == null || this.m_Canvas == null)
			{
				return;
			}
			float num = Vector3.Distance(this.m_Camera.transform.position, this.m_Canvas.transform.position);
			float num2;
			if (this.m_Camera.orthographic)
			{
				num2 = this.m_Camera.orthographicSize * 2f;
			}
			else
			{
				num2 = 2f * num * Mathf.Tan(this.m_Camera.fieldOfView * 0.5f * 0.017453292f);
			}
			float num3 = (float)Screen.height / (this.m_Canvas.transform as RectTransform).rect.height;
			float num4 = num2 / (float)Screen.height * num3;
			this.m_Canvas.transform.localScale = new Vector3(num4, num4, 1f);
		}

		// Token: 0x04001C3C RID: 7228
		[SerializeField]
		private Camera m_Camera;

		// Token: 0x04001C3D RID: 7229
		[SerializeField]
		private Canvas m_Canvas;
	}
}
