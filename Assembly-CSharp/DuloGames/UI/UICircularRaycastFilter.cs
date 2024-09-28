using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x02000631 RID: 1585
	[AddComponentMenu("UI/Raycast Filters/Circular Raycast Filter")]
	[RequireComponent(typeof(RectTransform))]
	public class UICircularRaycastFilter : MonoBehaviour, ICanvasRaycastFilter
	{
		// Token: 0x17000358 RID: 856
		// (get) Token: 0x060022DF RID: 8927 RVA: 0x000AC94C File Offset: 0x000AAB4C
		// (set) Token: 0x060022E0 RID: 8928 RVA: 0x000AC954 File Offset: 0x000AAB54
		public bool radiusInPercents
		{
			get
			{
				return this.m_RadiusInPercents;
			}
			set
			{
				this.m_RadiusInPercents = value;
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x060022E1 RID: 8929 RVA: 0x000AC95D File Offset: 0x000AAB5D
		// (set) Token: 0x060022E2 RID: 8930 RVA: 0x000AC965 File Offset: 0x000AAB65
		public float radius
		{
			get
			{
				return this.m_Radius;
			}
			set
			{
				this.m_Radius = value;
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x060022E3 RID: 8931 RVA: 0x000AC96E File Offset: 0x000AAB6E
		// (set) Token: 0x060022E4 RID: 8932 RVA: 0x000AC976 File Offset: 0x000AAB76
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

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x060022E5 RID: 8933 RVA: 0x000AC980 File Offset: 0x000AAB80
		public Vector2 center
		{
			get
			{
				RectTransform rectTransform = (RectTransform)base.transform;
				return new Vector2(rectTransform.rect.width / 2f, rectTransform.rect.height / 2f);
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x060022E6 RID: 8934 RVA: 0x000AC9C6 File Offset: 0x000AABC6
		private float rectMaxRadius
		{
			get
			{
				return Mathf.Sqrt(this.center.x * this.center.x + this.center.y * this.center.y);
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x060022E7 RID: 8935 RVA: 0x000AC9FC File Offset: 0x000AABFC
		public float operationalRadius
		{
			get
			{
				if (!this.m_RadiusInPercents)
				{
					return this.m_Radius;
				}
				return this.rectMaxRadius * (this.m_Radius / 100f);
			}
		}

		// Token: 0x060022E8 RID: 8936 RVA: 0x000ACA20 File Offset: 0x000AAC20
		public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
		{
			if (!base.enabled)
			{
				return true;
			}
			if (this.m_Radius == 0f)
			{
				return false;
			}
			RectTransform rectTransform = (RectTransform)base.transform;
			Vector2 vector;
			RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)base.transform, screenPoint, eventCamera, out vector);
			Vector2 a = new Vector2(vector.x + rectTransform.pivot.x * rectTransform.rect.width, vector.y + rectTransform.pivot.y * rectTransform.rect.height);
			Vector2 b = this.offset + this.center;
			return Vector2.Distance(a, b) <= this.operationalRadius;
		}

		// Token: 0x04001C56 RID: 7254
		[SerializeField]
		private bool m_RadiusInPercents = true;

		// Token: 0x04001C57 RID: 7255
		[Range(0f, 100f)]
		[SerializeField]
		private float m_Radius = 70f;

		// Token: 0x04001C58 RID: 7256
		[SerializeField]
		private Vector2 m_Offset = Vector2.zero;
	}
}
