using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x0200061E RID: 1566
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(Graphic))]
	[DisallowMultipleComponent]
	[AddComponentMenu("UI/Flippable", 8)]
	public class UIFlippable : MonoBehaviour, IMeshModifier
	{
		// Token: 0x17000352 RID: 850
		// (get) Token: 0x0600228D RID: 8845 RVA: 0x000AB479 File Offset: 0x000A9679
		// (set) Token: 0x0600228E RID: 8846 RVA: 0x000AB481 File Offset: 0x000A9681
		public bool horizontal
		{
			get
			{
				return this.m_Horizontal;
			}
			set
			{
				this.m_Horizontal = value;
				base.GetComponent<Graphic>().SetVerticesDirty();
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x0600228F RID: 8847 RVA: 0x000AB495 File Offset: 0x000A9695
		// (set) Token: 0x06002290 RID: 8848 RVA: 0x000AB49D File Offset: 0x000A969D
		public bool vertical
		{
			get
			{
				return this.m_Veritical;
			}
			set
			{
				this.m_Veritical = value;
				base.GetComponent<Graphic>().SetVerticesDirty();
			}
		}

		// Token: 0x06002291 RID: 8849 RVA: 0x000AB4B4 File Offset: 0x000A96B4
		public void ModifyMesh(VertexHelper vertexHelper)
		{
			if (!base.enabled)
			{
				return;
			}
			List<UIVertex> list = new List<UIVertex>();
			vertexHelper.GetUIVertexStream(list);
			this.ModifyVertices(list);
			vertexHelper.Clear();
			vertexHelper.AddUIVertexTriangleStream(list);
		}

		// Token: 0x06002292 RID: 8850 RVA: 0x000AB4EC File Offset: 0x000A96EC
		public void ModifyMesh(Mesh mesh)
		{
			if (!base.enabled)
			{
				return;
			}
			List<UIVertex> list = new List<UIVertex>();
			using (VertexHelper vertexHelper = new VertexHelper(mesh))
			{
				vertexHelper.GetUIVertexStream(list);
			}
			this.ModifyVertices(list);
			using (VertexHelper vertexHelper2 = new VertexHelper())
			{
				vertexHelper2.AddUIVertexTriangleStream(list);
				vertexHelper2.FillMesh(mesh);
			}
		}

		// Token: 0x06002293 RID: 8851 RVA: 0x000AB568 File Offset: 0x000A9768
		public void ModifyVertices(List<UIVertex> verts)
		{
			if (!base.enabled)
			{
				return;
			}
			RectTransform rectTransform = base.transform as RectTransform;
			for (int i = 0; i < verts.Count; i++)
			{
				UIVertex uivertex = verts[i];
				uivertex.position = new Vector3(this.m_Horizontal ? (uivertex.position.x + (rectTransform.rect.center.x - uivertex.position.x) * 2f) : uivertex.position.x, this.m_Veritical ? (uivertex.position.y + (rectTransform.rect.center.y - uivertex.position.y) * 2f) : uivertex.position.y, uivertex.position.z);
				verts[i] = uivertex;
			}
		}

		// Token: 0x04001C11 RID: 7185
		[SerializeField]
		private bool m_Horizontal;

		// Token: 0x04001C12 RID: 7186
		[SerializeField]
		private bool m_Veritical;
	}
}
