using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x02000634 RID: 1588
	[AddComponentMenu("UI/Raycast Filters/Rectangular Raycast Filter")]
	[RequireComponent(typeof(RectTransform))]
	public class UIRectangularRaycastFilter : MonoBehaviour, ICanvasRaycastFilter
	{
		// Token: 0x1700035E RID: 862
		// (get) Token: 0x060022EF RID: 8943 RVA: 0x000ACE1B File Offset: 0x000AB01B
		// (set) Token: 0x060022F0 RID: 8944 RVA: 0x000ACE23 File Offset: 0x000AB023
		public Vector2 offset
		{
			get
			{
				return this.m_Offset;
			}
			set
			{
				this.m_Offset = value;
			}
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x060022F1 RID: 8945 RVA: 0x000ACE2C File Offset: 0x000AB02C
		// (set) Token: 0x060022F2 RID: 8946 RVA: 0x000ACE34 File Offset: 0x000AB034
		public RectOffset borders
		{
			get
			{
				return this.m_Borders;
			}
			set
			{
				this.m_Borders = value;
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x060022F3 RID: 8947 RVA: 0x000ACE3D File Offset: 0x000AB03D
		// (set) Token: 0x060022F4 RID: 8948 RVA: 0x000ACE45 File Offset: 0x000AB045
		public float scaleX
		{
			get
			{
				return this.m_ScaleX;
			}
			set
			{
				this.m_ScaleX = value;
			}
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x060022F5 RID: 8949 RVA: 0x000ACE4E File Offset: 0x000AB04E
		// (set) Token: 0x060022F6 RID: 8950 RVA: 0x000ACE56 File Offset: 0x000AB056
		public float scaleY
		{
			get
			{
				return this.m_ScaleY;
			}
			set
			{
				this.m_ScaleY = value;
			}
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x060022F7 RID: 8951 RVA: 0x000ACE60 File Offset: 0x000AB060
		public Rect scaledRect
		{
			get
			{
				RectTransform rectTransform = (RectTransform)base.transform;
				return new Rect(this.offset.x + (float)this.borders.left + (rectTransform.rect.x + (rectTransform.rect.width - rectTransform.rect.width * this.m_ScaleX) / 2f), this.offset.y + (float)this.borders.bottom + (rectTransform.rect.y + (rectTransform.rect.height - rectTransform.rect.height * this.m_ScaleY) / 2f), rectTransform.rect.width * this.m_ScaleX - (float)this.borders.left - (float)this.borders.right, rectTransform.rect.height * this.m_ScaleY - (float)this.borders.top - (float)this.borders.bottom);
			}
		}

		// Token: 0x060022F8 RID: 8952 RVA: 0x000ACF80 File Offset: 0x000AB180
		public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
		{
			if (!base.enabled)
			{
				return true;
			}
			Vector2 point;
			RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)base.transform, screenPoint, eventCamera, out point);
			return this.scaledRect.Contains(point);
		}

		// Token: 0x04001C5C RID: 7260
		[SerializeField]
		private Vector2 m_Offset = Vector2.zero;

		// Token: 0x04001C5D RID: 7261
		[SerializeField]
		private RectOffset m_Borders = new RectOffset();

		// Token: 0x04001C5E RID: 7262
		[Range(0f, 1f)]
		[SerializeField]
		private float m_ScaleX = 1f;

		// Token: 0x04001C5F RID: 7263
		[Range(0f, 1f)]
		[SerializeField]
		private float m_ScaleY = 1f;
	}
}
