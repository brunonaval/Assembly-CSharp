using System;

namespace UnityEngine.Tilemaps
{
	// Token: 0x02000581 RID: 1409
	[CreateAssetMenu(fileName = "New Pipeline Tile", menuName = "Tiles/Pipeline Tile")]
	[Serializable]
	public class PipelineTile : TileBase
	{
		// Token: 0x06001F44 RID: 8004 RVA: 0x0009D558 File Offset: 0x0009B758
		public override void RefreshTile(Vector3Int location, ITilemap tileMap)
		{
			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
				{
					Vector3Int position = new Vector3Int(location.x + j, location.y + i, location.z);
					if (this.TileValue(tileMap, position))
					{
						tileMap.RefreshTile(position);
					}
				}
			}
		}

		// Token: 0x06001F45 RID: 8005 RVA: 0x0009D5AE File Offset: 0x0009B7AE
		public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData)
		{
			this.UpdateTile(location, tileMap, ref tileData);
		}

		// Token: 0x06001F46 RID: 8006 RVA: 0x0009D5BC File Offset: 0x0009B7BC
		private void UpdateTile(Vector3Int location, ITilemap tileMap, ref TileData tileData)
		{
			tileData.transform = Matrix4x4.identity;
			tileData.color = Color.white;
			int num = this.TileValue(tileMap, location + new Vector3Int(0, 1, 0)) ? 1 : 0;
			num += (this.TileValue(tileMap, location + new Vector3Int(1, 0, 0)) ? 2 : 0);
			num += (this.TileValue(tileMap, location + new Vector3Int(0, -1, 0)) ? 4 : 0);
			num += (this.TileValue(tileMap, location + new Vector3Int(-1, 0, 0)) ? 8 : 0);
			int index = this.GetIndex((byte)num);
			if (index >= 0 && index < this.m_Sprites.Length && this.TileValue(tileMap, location))
			{
				tileData.sprite = this.m_Sprites[index];
				tileData.transform = this.GetTransform((byte)num);
				tileData.flags = TileFlags.LockAll;
				tileData.colliderType = Tile.ColliderType.Sprite;
			}
		}

		// Token: 0x06001F47 RID: 8007 RVA: 0x0009D6A4 File Offset: 0x0009B8A4
		private bool TileValue(ITilemap tileMap, Vector3Int position)
		{
			TileBase tile = tileMap.GetTile(position);
			return tile != null && tile == this;
		}

		// Token: 0x06001F48 RID: 8008 RVA: 0x0009D6CC File Offset: 0x0009B8CC
		private int GetIndex(byte mask)
		{
			switch (mask)
			{
			case 0:
				return 0;
			case 1:
			case 2:
			case 4:
			case 5:
			case 8:
			case 10:
				return 2;
			case 3:
			case 6:
			case 9:
			case 12:
				return 1;
			case 7:
			case 11:
			case 13:
			case 14:
				return 3;
			case 15:
				return 4;
			default:
				return -1;
			}
		}

		// Token: 0x06001F49 RID: 8009 RVA: 0x0009D72C File Offset: 0x0009B92C
		private Matrix4x4 GetTransform(byte mask)
		{
			switch (mask)
			{
			case 2:
			case 7:
			case 8:
			case 9:
			case 10:
				return Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, -90f), Vector3.one);
			case 3:
			case 14:
				return Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, -180f), Vector3.one);
			case 6:
			case 13:
				return Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, -270f), Vector3.one);
			}
			return Matrix4x4.identity;
		}

		// Token: 0x04001946 RID: 6470
		[SerializeField]
		public Sprite[] m_Sprites;
	}
}
