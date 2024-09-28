using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x020005F4 RID: 1524
	[AddComponentMenu("UI/Effects/Letter Spacing", 14)]
	[RequireComponent(typeof(Text))]
	public class LetterSpacing : BaseMeshEffect, ILayoutElement
	{
		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06002181 RID: 8577 RVA: 0x000A79E7 File Offset: 0x000A5BE7
		// (set) Token: 0x06002182 RID: 8578 RVA: 0x000A79EF File Offset: 0x000A5BEF
		public float spacing
		{
			get
			{
				return this.m_spacing;
			}
			set
			{
				if (this.m_spacing == value)
				{
					return;
				}
				this.m_spacing = value;
				if (base.graphic != null)
				{
					base.graphic.SetVerticesDirty();
				}
				LayoutRebuilder.MarkLayoutForRebuild((RectTransform)base.transform);
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06002183 RID: 8579 RVA: 0x000A7A2B File Offset: 0x000A5C2B
		private Text text
		{
			get
			{
				return base.gameObject.GetComponent<Text>();
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06002184 RID: 8580 RVA: 0x000A7A38 File Offset: 0x000A5C38
		public float minWidth
		{
			get
			{
				return this.text.minWidth;
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06002185 RID: 8581 RVA: 0x000A7A45 File Offset: 0x000A5C45
		public float preferredWidth
		{
			get
			{
				return this.spacing * (float)this.text.fontSize / 100f * (float)(this.text.text.Length - 1) + this.text.preferredWidth;
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06002186 RID: 8582 RVA: 0x000A7A80 File Offset: 0x000A5C80
		public float flexibleWidth
		{
			get
			{
				return this.text.flexibleWidth;
			}
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06002187 RID: 8583 RVA: 0x000A7A8D File Offset: 0x000A5C8D
		public float minHeight
		{
			get
			{
				return this.text.minHeight;
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06002188 RID: 8584 RVA: 0x000A7A9A File Offset: 0x000A5C9A
		public float preferredHeight
		{
			get
			{
				return this.text.preferredHeight;
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06002189 RID: 8585 RVA: 0x000A7AA7 File Offset: 0x000A5CA7
		public float flexibleHeight
		{
			get
			{
				return this.text.flexibleHeight;
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x0600218A RID: 8586 RVA: 0x000A7AB4 File Offset: 0x000A5CB4
		public int layoutPriority
		{
			get
			{
				return this.text.layoutPriority;
			}
		}

		// Token: 0x0600218B RID: 8587 RVA: 0x000A7AC1 File Offset: 0x000A5CC1
		protected LetterSpacing()
		{
		}

		// Token: 0x0600218C RID: 8588 RVA: 0x0000219A File Offset: 0x0000039A
		public void CalculateLayoutInputHorizontal()
		{
		}

		// Token: 0x0600218D RID: 8589 RVA: 0x0000219A File Offset: 0x0000039A
		public void CalculateLayoutInputVertical()
		{
		}

		// Token: 0x0600218E RID: 8590 RVA: 0x000A7ACC File Offset: 0x000A5CCC
		private string[] GetLines()
		{
			IList<UILineInfo> lines = this.text.cachedTextGenerator.lines;
			string[] array = new string[lines.Count];
			for (int i = 0; i < lines.Count; i++)
			{
				if (i < lines.Count - 1)
				{
					ref UILineInfo ptr = lines[i + 1];
					int startCharIdx = lines[i].startCharIdx;
					int length = ptr.startCharIdx - 1 - startCharIdx;
					array[i] = this.text.text.Substring(lines[i].startCharIdx, length);
				}
				else
				{
					array[i] = this.text.text.Substring(lines[i].startCharIdx);
				}
			}
			return array;
		}

		// Token: 0x0600218F RID: 8591 RVA: 0x000A7B78 File Offset: 0x000A5D78
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

		// Token: 0x06002190 RID: 8592 RVA: 0x000A7BB0 File Offset: 0x000A5DB0
		public void ModifyVertices(List<UIVertex> verts)
		{
			if (!this.IsActive() || verts.Count == 0)
			{
				return;
			}
			string[] lines = this.GetLines();
			float num = this.spacing * (float)this.text.fontSize / 100f;
			float num2 = 0f;
			int num3 = 0;
			switch (this.text.alignment)
			{
			case TextAnchor.UpperLeft:
			case TextAnchor.MiddleLeft:
			case TextAnchor.LowerLeft:
				num2 = 0f;
				break;
			case TextAnchor.UpperCenter:
			case TextAnchor.MiddleCenter:
			case TextAnchor.LowerCenter:
				num2 = 0.5f;
				break;
			case TextAnchor.UpperRight:
			case TextAnchor.MiddleRight:
			case TextAnchor.LowerRight:
				num2 = 1f;
				break;
			}
			foreach (string text in lines)
			{
				float num4 = (float)(text.Length - 1) * num * num2;
				for (int j = 0; j < text.Length; j++)
				{
					int index = num3 * 6;
					int index2 = num3 * 6 + 1;
					int index3 = num3 * 6 + 2;
					int index4 = num3 * 6 + 3;
					int index5 = num3 * 6 + 4;
					int num5 = num3 * 6 + 5;
					if (num5 > verts.Count - 1)
					{
						return;
					}
					UIVertex value = verts[index];
					UIVertex value2 = verts[index2];
					UIVertex value3 = verts[index3];
					UIVertex value4 = verts[index4];
					UIVertex value5 = verts[index5];
					UIVertex value6 = verts[num5];
					Vector3 b = Vector3.right * (num * (float)j - num4);
					value.position += b;
					value2.position += b;
					value3.position += b;
					value4.position += b;
					value5.position += b;
					value6.position += b;
					verts[index] = value;
					verts[index2] = value2;
					verts[index3] = value3;
					verts[index4] = value4;
					verts[index5] = value5;
					verts[num5] = value6;
					num3++;
				}
				num3++;
			}
		}

		// Token: 0x04001B7A RID: 7034
		[SerializeField]
		private float m_spacing;
	}
}
