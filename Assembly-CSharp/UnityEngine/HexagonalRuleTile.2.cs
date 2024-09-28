using System;
using UnityEngine.Tilemaps;

namespace UnityEngine
{
	// Token: 0x0200056D RID: 1389
	[CreateAssetMenu(fileName = "New Hexagonal Rule Tile", menuName = "Tiles/Hexagonal Rule Tile")]
	[Serializable]
	public class HexagonalRuleTile : RuleTile
	{
		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06001EED RID: 7917 RVA: 0x0009B66E File Offset: 0x0009986E
		public override int neighborCount
		{
			get
			{
				return HexagonalRuleTile.NeighborCount;
			}
		}

		// Token: 0x06001EEE RID: 7918 RVA: 0x0009B678 File Offset: 0x00099878
		public override void RefreshTile(Vector3Int location, ITilemap tileMap)
		{
			if (this.m_TilingRules != null && this.m_TilingRules.Count > 0)
			{
				for (int i = 0; i < this.neighborCount; i++)
				{
					base.RefreshTile(location + this.GetOffsetPosition(location, i), tileMap);
				}
			}
			base.RefreshTile(location, tileMap);
		}

		// Token: 0x06001EEF RID: 7919 RVA: 0x0009B6CC File Offset: 0x000998CC
		protected override bool RuleMatches(RuleTile.TilingRule rule, ref TileBase[] neighboringTiles, ref Matrix4x4 transform)
		{
			for (int i = 0; i <= ((rule.m_RuleTransform == RuleTile.TilingRule.Transform.Rotated) ? 300 : 0); i += 60)
			{
				if (base.RuleMatches(rule, ref neighboringTiles, i))
				{
					transform = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, (float)(-(float)i)), Vector3.one);
					return true;
				}
			}
			if (rule.m_RuleTransform == RuleTile.TilingRule.Transform.MirrorX && base.RuleMatches(rule, ref neighboringTiles, true, false))
			{
				transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(-1f, 1f, 1f));
				return true;
			}
			if (rule.m_RuleTransform == RuleTile.TilingRule.Transform.MirrorY && base.RuleMatches(rule, ref neighboringTiles, false, true))
			{
				transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1f, -1f, 1f));
				return true;
			}
			return false;
		}

		// Token: 0x06001EF0 RID: 7920 RVA: 0x0009B7AC File Offset: 0x000999AC
		protected override Matrix4x4 ApplyRandomTransform(RuleTile.TilingRule.Transform type, Matrix4x4 original, float perlinScale, Vector3Int position)
		{
			float perlinValue = RuleTile.GetPerlinValue(position, perlinScale, 200000f);
			switch (type)
			{
			case RuleTile.TilingRule.Transform.Rotated:
			{
				int num = Mathf.Clamp(Mathf.FloorToInt(perlinValue * (float)this.neighborCount), 0, this.neighborCount - 1) * (360 / this.neighborCount);
				return Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, (float)(-(float)num)), Vector3.one);
			}
			case RuleTile.TilingRule.Transform.MirrorX:
				return original * Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(((double)perlinValue < 0.5) ? 1f : -1f, 1f, 1f));
			case RuleTile.TilingRule.Transform.MirrorY:
				return original * Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1f, ((double)perlinValue < 0.5) ? 1f : -1f, 1f));
			default:
				return original;
			}
		}

		// Token: 0x06001EF1 RID: 7921 RVA: 0x0009B8AC File Offset: 0x00099AAC
		protected override void GetMatchingNeighboringTiles(ITilemap tilemap, Vector3Int position, ref TileBase[] neighboringTiles)
		{
			if (neighboringTiles != null)
			{
				return;
			}
			if (this.m_CachedNeighboringTiles == null || this.m_CachedNeighboringTiles.Length < this.neighborCount)
			{
				this.m_CachedNeighboringTiles = new TileBase[this.neighborCount];
			}
			for (int i = 0; i < this.neighborCount; i++)
			{
				Vector3Int position2 = position + this.GetOffsetPosition(position, i);
				this.m_CachedNeighboringTiles[i] = tilemap.GetTile(position2);
			}
			neighboringTiles = this.m_CachedNeighboringTiles;
		}

		// Token: 0x06001EF2 RID: 7922 RVA: 0x0009B91E File Offset: 0x00099B1E
		protected override int GetRotatedIndex(int original, int rotation)
		{
			return (original + rotation / 60) % this.neighborCount;
		}

		// Token: 0x06001EF3 RID: 7923 RVA: 0x0009B930 File Offset: 0x00099B30
		protected override int GetMirroredIndex(int original, bool mirrorX, bool mirrorY)
		{
			if (mirrorX && mirrorY)
			{
				return HexagonalRuleTile.RotatedOrMirroredIndexes[2, original];
			}
			if (mirrorX)
			{
				return HexagonalRuleTile.RotatedOrMirroredIndexes[this.m_FlatTop ? 3 : 0, original];
			}
			if (mirrorY)
			{
				return HexagonalRuleTile.RotatedOrMirroredIndexes[this.m_FlatTop ? 4 : 1, original];
			}
			return original;
		}

		// Token: 0x06001EF4 RID: 7924 RVA: 0x0009B988 File Offset: 0x00099B88
		private Vector3Int GetOffsetPosition(Vector3Int location, int direction)
		{
			int num = location.y & 1;
			if (!this.m_FlatTop)
			{
				return HexagonalRuleTile.PointedTopNeighborOffsets[num, direction];
			}
			return HexagonalRuleTile.FlatTopNeighborOffsets[num, direction];
		}

		// Token: 0x06001EF6 RID: 7926 RVA: 0x0009B9C8 File Offset: 0x00099BC8
		// Note: this type is marked as 'beforefieldinit'.
		static HexagonalRuleTile()
		{
			Vector3Int[,] array = new Vector3Int[2, 6];
			array[0, 0] = new Vector3Int(1, 0, 0);
			array[0, 1] = new Vector3Int(0, -1, 0);
			array[0, 2] = new Vector3Int(-1, -1, 0);
			array[0, 3] = new Vector3Int(-1, 0, 0);
			array[0, 4] = new Vector3Int(-1, 1, 0);
			array[0, 5] = new Vector3Int(0, 1, 0);
			array[1, 0] = new Vector3Int(1, 0, 0);
			array[1, 1] = new Vector3Int(1, -1, 0);
			array[1, 2] = new Vector3Int(0, -1, 0);
			array[1, 3] = new Vector3Int(-1, 0, 0);
			array[1, 4] = new Vector3Int(0, 1, 0);
			array[1, 5] = new Vector3Int(1, 1, 0);
			HexagonalRuleTile.PointedTopNeighborOffsets = array;
			Vector3Int[,] array2 = new Vector3Int[2, 6];
			array2[0, 0] = new Vector3Int(1, 0, 0);
			array2[0, 1] = new Vector3Int(0, 1, 0);
			array2[0, 2] = new Vector3Int(-1, 1, 0);
			array2[0, 3] = new Vector3Int(-1, 0, 0);
			array2[0, 4] = new Vector3Int(-1, -1, 0);
			array2[0, 5] = new Vector3Int(0, -1, 0);
			array2[1, 0] = new Vector3Int(1, 0, 0);
			array2[1, 1] = new Vector3Int(1, 1, 0);
			array2[1, 2] = new Vector3Int(0, 1, 0);
			array2[1, 3] = new Vector3Int(-1, 0, 0);
			array2[1, 4] = new Vector3Int(0, -1, 0);
			array2[1, 5] = new Vector3Int(1, -1, 0);
			HexagonalRuleTile.FlatTopNeighborOffsets = array2;
			HexagonalRuleTile.NeighborCount = 6;
		}

		// Token: 0x040018F7 RID: 6391
		private static readonly int[,] RotatedOrMirroredIndexes = new int[,]
		{
			{
				3,
				2,
				1,
				0,
				5,
				4
			},
			{
				0,
				5,
				4,
				3,
				2,
				1
			},
			{
				3,
				4,
				5,
				0,
				1,
				2
			},
			{
				0,
				5,
				4,
				3,
				2,
				1
			},
			{
				3,
				2,
				1,
				0,
				5,
				4
			}
		};

		// Token: 0x040018F8 RID: 6392
		private static readonly Vector3Int[,] PointedTopNeighborOffsets;

		// Token: 0x040018F9 RID: 6393
		private static readonly Vector3Int[,] FlatTopNeighborOffsets;

		// Token: 0x040018FA RID: 6394
		private static readonly int NeighborCount;

		// Token: 0x040018FB RID: 6395
		public bool m_FlatTop;
	}
}
