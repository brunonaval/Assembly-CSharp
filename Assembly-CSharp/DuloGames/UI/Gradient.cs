using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x020005F3 RID: 1523
	[AddComponentMenu("UI/Effects/Gradient")]
	public class Gradient : BaseMeshEffect
	{
		// Token: 0x0600217E RID: 8574 RVA: 0x000A78AC File Offset: 0x000A5AAC
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

		// Token: 0x0600217F RID: 8575 RVA: 0x000A78E4 File Offset: 0x000A5AE4
		public void ModifyVertices(List<UIVertex> vertexList)
		{
			if (!this.IsActive() || vertexList.Count == 0)
			{
				return;
			}
			int count = vertexList.Count;
			float num = vertexList[0].position.y;
			float num2 = vertexList[0].position.y;
			for (int i = 1; i < count; i++)
			{
				float y = vertexList[i].position.y;
				if (y > num2)
				{
					num2 = y;
				}
				else if (y < num)
				{
					num = y;
				}
			}
			float num3 = num2 - num;
			for (int j = 0; j < count; j++)
			{
				UIVertex uivertex = vertexList[j];
				uivertex.color *= Color.Lerp(this.bottomColor, this.topColor, (uivertex.position.y - num) / num3);
				vertexList[j] = uivertex;
			}
		}

		// Token: 0x04001B78 RID: 7032
		[SerializeField]
		private Color topColor = Color.white;

		// Token: 0x04001B79 RID: 7033
		[SerializeField]
		private Color bottomColor = Color.black;
	}
}
