using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x020005F5 RID: 1525
	[AddComponentMenu("UI/Effects/Nicer Outline")]
	public class NicerOutline : BaseMeshEffect
	{
		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06002191 RID: 8593 RVA: 0x000A7DF4 File Offset: 0x000A5FF4
		// (set) Token: 0x06002192 RID: 8594 RVA: 0x000A7DFC File Offset: 0x000A5FFC
		public Color effectColor
		{
			get
			{
				return this.m_EffectColor;
			}
			set
			{
				this.m_EffectColor = value;
				if (base.graphic != null)
				{
					base.graphic.SetVerticesDirty();
				}
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06002193 RID: 8595 RVA: 0x000A7E1E File Offset: 0x000A601E
		// (set) Token: 0x06002194 RID: 8596 RVA: 0x000A7E28 File Offset: 0x000A6028
		public Vector2 effectDistance
		{
			get
			{
				return this.m_EffectDistance;
			}
			set
			{
				if (value.x > 600f)
				{
					value.x = 600f;
				}
				if (value.x < -600f)
				{
					value.x = -600f;
				}
				if (value.y > 600f)
				{
					value.y = 600f;
				}
				if (value.y < -600f)
				{
					value.y = -600f;
				}
				if (this.m_EffectDistance == value)
				{
					return;
				}
				this.m_EffectDistance = value;
				if (base.graphic != null)
				{
					base.graphic.SetVerticesDirty();
				}
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06002195 RID: 8597 RVA: 0x000A7EC8 File Offset: 0x000A60C8
		// (set) Token: 0x06002196 RID: 8598 RVA: 0x000A7ED0 File Offset: 0x000A60D0
		public bool useGraphicAlpha
		{
			get
			{
				return this.m_UseGraphicAlpha;
			}
			set
			{
				this.m_UseGraphicAlpha = value;
				if (base.graphic != null)
				{
					base.graphic.SetVerticesDirty();
				}
			}
		}

		// Token: 0x06002197 RID: 8599 RVA: 0x000A7EF4 File Offset: 0x000A60F4
		protected void ApplyShadow(List<UIVertex> verts, Color32 color, int start, int end, float x, float y)
		{
			int num = verts.Count * 2;
			if (verts.Capacity < num)
			{
				verts.Capacity = num;
			}
			for (int i = start; i < end; i++)
			{
				UIVertex uivertex = verts[i];
				verts.Add(uivertex);
				Vector3 position = uivertex.position;
				position.x += x;
				position.y += y;
				uivertex.position = position;
				Color32 color2 = color;
				if (this.m_UseGraphicAlpha)
				{
					color2.a = color2.a * verts[i].color.a / byte.MaxValue;
				}
				uivertex.color = color2;
				verts[i] = uivertex;
			}
		}

		// Token: 0x06002198 RID: 8600 RVA: 0x000A7FA8 File Offset: 0x000A61A8
		public override void ModifyMesh(VertexHelper vertexHelper)
		{
			if (!this.IsActive())
			{
				return;
			}
			List<UIVertex> list = new List<UIVertex>();
			vertexHelper.GetUIVertexStream(list);
			this.ModifyVertices(list);
			vertexHelper.Clear();
			vertexHelper.AddUIVertexTriangleStream(list);
		}

		// Token: 0x06002199 RID: 8601 RVA: 0x000A7FE0 File Offset: 0x000A61E0
		public void ModifyVertices(List<UIVertex> verts)
		{
			if (!this.IsActive() || verts.Count == 0)
			{
				return;
			}
			Text component = base.GetComponent<Text>();
			float num = 1f;
			if (component && component.resizeTextForBestFit)
			{
				num = (float)component.cachedTextGenerator.fontSizeUsedForBestFit / (float)(component.resizeTextMaxSize - 1);
			}
			float num2 = this.effectDistance.x * num;
			float num3 = this.effectDistance.y * num;
			int start = 0;
			int count = verts.Count;
			this.ApplyShadow(verts, this.effectColor, start, verts.Count, num2, num3);
			start = count;
			int count2 = verts.Count;
			this.ApplyShadow(verts, this.effectColor, start, verts.Count, num2, -num3);
			start = count2;
			int count3 = verts.Count;
			this.ApplyShadow(verts, this.effectColor, start, verts.Count, -num2, num3);
			start = count3;
			int count4 = verts.Count;
			this.ApplyShadow(verts, this.effectColor, start, verts.Count, -num2, -num3);
			start = count4;
			int count5 = verts.Count;
			this.ApplyShadow(verts, this.effectColor, start, verts.Count, num2, 0f);
			start = count5;
			int count6 = verts.Count;
			this.ApplyShadow(verts, this.effectColor, start, verts.Count, -num2, 0f);
			start = count6;
			int count7 = verts.Count;
			this.ApplyShadow(verts, this.effectColor, start, verts.Count, 0f, num3);
			start = count7;
			int count8 = verts.Count;
			this.ApplyShadow(verts, this.effectColor, start, verts.Count, 0f, -num3);
		}

		// Token: 0x04001B7B RID: 7035
		[SerializeField]
		private Color m_EffectColor = new Color(0f, 0f, 0f, 0.5f);

		// Token: 0x04001B7C RID: 7036
		[SerializeField]
		private Vector2 m_EffectDistance = new Vector2(1f, -1f);

		// Token: 0x04001B7D RID: 7037
		[SerializeField]
		private bool m_UseGraphicAlpha = true;
	}
}
