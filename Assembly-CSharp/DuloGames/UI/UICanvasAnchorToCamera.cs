using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x0200061F RID: 1567
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	public class UICanvasAnchorToCamera : MonoBehaviour
	{
		// Token: 0x06002295 RID: 8853 RVA: 0x000AB654 File Offset: 0x000A9854
		protected void Awake()
		{
			this.m_RectTransform = (base.transform as RectTransform);
		}

		// Token: 0x06002296 RID: 8854 RVA: 0x000AB668 File Offset: 0x000A9868
		private void Update()
		{
			if (this.m_Camera == null)
			{
				return;
			}
			Vector3 position = this.m_Camera.ViewportToWorldPoint(new Vector3(this.m_Horizontal, this.m_Vertical, this.m_Camera.farClipPlane));
			position.z = this.m_RectTransform.position.z;
			this.m_RectTransform.position = position;
		}

		// Token: 0x04001C13 RID: 7187
		[SerializeField]
		private Camera m_Camera;

		// Token: 0x04001C14 RID: 7188
		[SerializeField]
		[Range(0f, 1f)]
		private float m_Vertical;

		// Token: 0x04001C15 RID: 7189
		[SerializeField]
		[Range(0f, 1f)]
		private float m_Horizontal;

		// Token: 0x04001C16 RID: 7190
		private RectTransform m_RectTransform;
	}
}
