using System;
using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x0200061D RID: 1565
	[ExecuteInEditMode]
	public class UIDrawVertices : MonoBehaviour, IMeshModifier
	{
		// Token: 0x06002289 RID: 8841 RVA: 0x000AB40D File Offset: 0x000A960D
		public void ModifyMesh(VertexHelper vertexHelper)
		{
			if (this.mesh == null)
			{
				this.mesh = new Mesh();
			}
			vertexHelper.FillMesh(this.mesh);
		}

		// Token: 0x0600228A RID: 8842 RVA: 0x0000219A File Offset: 0x0000039A
		public void ModifyMesh(Mesh mesh)
		{
		}

		// Token: 0x0600228B RID: 8843 RVA: 0x000AB434 File Offset: 0x000A9634
		public void OnDrawGizmos()
		{
			if (this.mesh == null)
			{
				return;
			}
			Gizmos.color = this.color;
			Gizmos.DrawWireMesh(this.mesh, base.transform.position);
		}

		// Token: 0x04001C0F RID: 7183
		[SerializeField]
		private Color color = Color.green;

		// Token: 0x04001C10 RID: 7184
		private Mesh mesh;
	}
}
