using System;
using UnityEngine;
using UnityEngine.Tilemaps;

// Token: 0x02000563 RID: 1379
[ExecuteInEditMode]
public class TintTextureGenerator : MonoBehaviour
{
	// Token: 0x06001EBE RID: 7870 RVA: 0x0009992D File Offset: 0x00097B2D
	public void Start()
	{
		this.Refresh(base.GetComponent<Grid>());
	}

	// Token: 0x170002C8 RID: 712
	// (get) Token: 0x06001EBF RID: 7871 RVA: 0x0009993C File Offset: 0x00097B3C
	private Texture2D tintTexture
	{
		get
		{
			if (this.m_TintTexture == null)
			{
				this.m_TintTexture = new Texture2D(this.k_TintMapSize, this.k_TintMapSize, TextureFormat.ARGB32, false);
				this.m_TintTexture.hideFlags = HideFlags.HideAndDontSave;
				this.m_TintTexture.wrapMode = TextureWrapMode.Clamp;
				this.m_TintTexture.filterMode = FilterMode.Bilinear;
				this.RefreshGlobalShaderValues();
			}
			return this.m_TintTexture;
		}
	}

	// Token: 0x06001EC0 RID: 7872 RVA: 0x000999A4 File Offset: 0x00097BA4
	public void Refresh(Grid grid)
	{
		if (grid == null)
		{
			return;
		}
		int width = this.tintTexture.width;
		int height = this.tintTexture.height;
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				Vector3Int position = this.TextureToWorld(new Vector3Int(j, i, 0));
				this.tintTexture.SetPixel(j, i, this.GetGridInformation(grid).GetPositionProperty(position, "Tint", Color.white));
			}
		}
		this.tintTexture.Apply();
	}

	// Token: 0x06001EC1 RID: 7873 RVA: 0x00099A2C File Offset: 0x00097C2C
	public void Refresh(Grid grid, Vector3Int position)
	{
		if (grid == null)
		{
			return;
		}
		this.RefreshGlobalShaderValues();
		Vector3Int vector3Int = this.WorldToTexture(position);
		this.tintTexture.SetPixel(vector3Int.x, vector3Int.y, this.GetGridInformation(grid).GetPositionProperty(position, "Tint", Color.white));
		this.tintTexture.Apply();
	}

	// Token: 0x06001EC2 RID: 7874 RVA: 0x00099A8C File Offset: 0x00097C8C
	public Color GetColor(Grid grid, Vector3Int position)
	{
		if (grid == null)
		{
			return Color.white;
		}
		return this.GetGridInformation(grid).GetPositionProperty(position, "Tint", Color.white);
	}

	// Token: 0x06001EC3 RID: 7875 RVA: 0x00099AB4 File Offset: 0x00097CB4
	public void SetColor(Grid grid, Vector3Int position, Color color)
	{
		if (grid == null)
		{
			return;
		}
		this.GetGridInformation(grid).SetPositionProperty(position, "Tint", color);
		this.Refresh(grid, position);
	}

	// Token: 0x06001EC4 RID: 7876 RVA: 0x00099ADC File Offset: 0x00097CDC
	private Vector3Int WorldToTexture(Vector3Int world)
	{
		return new Vector3Int(world.x + this.tintTexture.width / 2, world.y + this.tintTexture.height / 2, 0);
	}

	// Token: 0x06001EC5 RID: 7877 RVA: 0x00099B0E File Offset: 0x00097D0E
	private Vector3Int TextureToWorld(Vector3Int texpos)
	{
		return new Vector3Int(texpos.x - this.tintTexture.width / 2, texpos.y - this.tintTexture.height / 2, 0);
	}

	// Token: 0x06001EC6 RID: 7878 RVA: 0x00099B40 File Offset: 0x00097D40
	private GridInformation GetGridInformation(Grid grid)
	{
		GridInformation gridInformation = grid.GetComponent<GridInformation>();
		if (gridInformation == null)
		{
			gridInformation = grid.gameObject.AddComponent<GridInformation>();
		}
		return gridInformation;
	}

	// Token: 0x06001EC7 RID: 7879 RVA: 0x00099B6A File Offset: 0x00097D6A
	private void RefreshGlobalShaderValues()
	{
		Shader.SetGlobalTexture("_TintMap", this.m_TintTexture);
		Shader.SetGlobalFloat("_TintMapSize", (float)this.k_TintMapSize);
	}

	// Token: 0x0400189A RID: 6298
	public int k_TintMapSize = 256;

	// Token: 0x0400189B RID: 6299
	private Texture2D m_TintTexture;
}
