using System;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

namespace UnityEngine
{
	// Token: 0x02000575 RID: 1397
	[CreateAssetMenu(fileName = "New Rule Tile", menuName = "Tiles/Rule Tile")]
	[Serializable]
	public class RuleTile : TileBase
	{
		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06001F12 RID: 7954 RVA: 0x0009C41C File Offset: 0x0009A61C
		public virtual Type m_NeighborType
		{
			get
			{
				return typeof(RuleTile.TilingRule.Neighbor);
			}
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06001F13 RID: 7955 RVA: 0x0009C428 File Offset: 0x0009A628
		public virtual int neighborCount
		{
			get
			{
				return RuleTile.NeighborCount;
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06001F14 RID: 7956 RVA: 0x0009C42F File Offset: 0x0009A62F
		// (set) Token: 0x06001F15 RID: 7957 RVA: 0x0009C446 File Offset: 0x0009A646
		public TileBase m_Self
		{
			get
			{
				if (!this.m_OverrideSelf)
				{
					return this;
				}
				return this.m_OverrideSelf;
			}
			set
			{
				this.m_OverrideSelf = value;
			}
		}

		// Token: 0x06001F16 RID: 7958 RVA: 0x0009C450 File Offset: 0x0009A650
		public override bool StartUp(Vector3Int location, ITilemap tilemap, GameObject instantiateedGameObject)
		{
			if (instantiateedGameObject != null)
			{
				Tilemap component = tilemap.GetComponent<Tilemap>();
				instantiateedGameObject.transform.position = component.LocalToWorld(component.CellToLocalInterpolated(location + component.tileAnchor));
				instantiateedGameObject.transform.rotation = this.m_GameObjectQuaternion;
			}
			return true;
		}

		// Token: 0x06001F17 RID: 7959 RVA: 0x0009C4A8 File Offset: 0x0009A6A8
		public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
		{
			TileBase[] array = null;
			this.GetMatchingNeighboringTiles(tilemap, position, ref array);
			Matrix4x4 identity = Matrix4x4.identity;
			tileData.sprite = this.m_DefaultSprite;
			tileData.gameObject = this.m_DefaultGameObject;
			tileData.colliderType = this.m_DefaultColliderType;
			tileData.flags = TileFlags.LockTransform;
			tileData.transform = identity;
			foreach (RuleTile.TilingRule tilingRule in this.m_TilingRules)
			{
				Matrix4x4 matrix4x = identity;
				if (this.RuleMatches(tilingRule, ref array, ref matrix4x))
				{
					switch (tilingRule.m_Output)
					{
					case RuleTile.TilingRule.OutputSprite.Single:
					case RuleTile.TilingRule.OutputSprite.Animation:
						tileData.sprite = tilingRule.m_Sprites[0];
						break;
					case RuleTile.TilingRule.OutputSprite.Random:
					{
						int num = Mathf.Clamp(Mathf.FloorToInt(RuleTile.GetPerlinValue(position, tilingRule.m_PerlinScale, 100000f) * (float)tilingRule.m_Sprites.Length), 0, tilingRule.m_Sprites.Length - 1);
						tileData.sprite = tilingRule.m_Sprites[num];
						if (tilingRule.m_RandomTransform != RuleTile.TilingRule.Transform.Fixed)
						{
							matrix4x = this.ApplyRandomTransform(tilingRule.m_RandomTransform, matrix4x, tilingRule.m_PerlinScale, position);
						}
						break;
					}
					}
					tileData.transform = matrix4x;
					tileData.gameObject = tilingRule.m_GameObject;
					tileData.colliderType = tilingRule.m_ColliderType;
					this.m_GameObjectQuaternion = Quaternion.LookRotation(new Vector3(matrix4x.m02, matrix4x.m12, matrix4x.m22), new Vector3(matrix4x.m01, matrix4x.m11, matrix4x.m21));
					break;
				}
			}
		}

		// Token: 0x06001F18 RID: 7960 RVA: 0x0009C64C File Offset: 0x0009A84C
		protected static float GetPerlinValue(Vector3Int position, float scale, float offset)
		{
			return Mathf.PerlinNoise(((float)position.x + offset) * scale, ((float)position.y + offset) * scale);
		}

		// Token: 0x06001F19 RID: 7961 RVA: 0x0009C66C File Offset: 0x0009A86C
		public override bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
		{
			TileBase[] array = null;
			Matrix4x4 identity = Matrix4x4.identity;
			foreach (RuleTile.TilingRule tilingRule in this.m_TilingRules)
			{
				if (tilingRule.m_Output == RuleTile.TilingRule.OutputSprite.Animation)
				{
					Matrix4x4 matrix4x = identity;
					this.GetMatchingNeighboringTiles(tilemap, position, ref array);
					if (this.RuleMatches(tilingRule, ref array, ref matrix4x))
					{
						tileAnimationData.animatedSprites = tilingRule.m_Sprites;
						tileAnimationData.animationSpeed = tilingRule.m_AnimationSpeed;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001F1A RID: 7962 RVA: 0x0009C704 File Offset: 0x0009A904
		public override void RefreshTile(Vector3Int location, ITilemap tileMap)
		{
			if (this.m_TilingRules != null && this.m_TilingRules.Count > 0)
			{
				for (int i = -1; i <= 1; i++)
				{
					for (int j = -1; j <= 1; j++)
					{
						base.RefreshTile(location + new Vector3Int(j, i, 0), tileMap);
					}
				}
				return;
			}
			base.RefreshTile(location, tileMap);
		}

		// Token: 0x06001F1B RID: 7963 RVA: 0x0009C760 File Offset: 0x0009A960
		protected virtual bool RuleMatches(RuleTile.TilingRule rule, ref TileBase[] neighboringTiles, ref Matrix4x4 transform)
		{
			for (int i = 0; i <= ((rule.m_RuleTransform == RuleTile.TilingRule.Transform.Rotated) ? 270 : 0); i += 90)
			{
				if (this.RuleMatches(rule, ref neighboringTiles, i))
				{
					transform = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, (float)(-(float)i)), Vector3.one);
					return true;
				}
			}
			if (rule.m_RuleTransform == RuleTile.TilingRule.Transform.MirrorX && this.RuleMatches(rule, ref neighboringTiles, true, false))
			{
				transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(-1f, 1f, 1f));
				return true;
			}
			if (rule.m_RuleTransform == RuleTile.TilingRule.Transform.MirrorY && this.RuleMatches(rule, ref neighboringTiles, false, true))
			{
				transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1f, -1f, 1f));
				return true;
			}
			return false;
		}

		// Token: 0x06001F1C RID: 7964 RVA: 0x0009C840 File Offset: 0x0009AA40
		protected virtual Matrix4x4 ApplyRandomTransform(RuleTile.TilingRule.Transform type, Matrix4x4 original, float perlinScale, Vector3Int position)
		{
			float perlinValue = RuleTile.GetPerlinValue(position, perlinScale, 200000f);
			switch (type)
			{
			case RuleTile.TilingRule.Transform.Rotated:
			{
				int num = Mathf.Clamp(Mathf.FloorToInt(perlinValue * 4f), 0, 3) * 90;
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

		// Token: 0x06001F1D RID: 7965 RVA: 0x0009C92C File Offset: 0x0009AB2C
		public virtual bool RuleMatch(int neighbor, TileBase tile)
		{
			if (tile is RuleOverrideTile)
			{
				tile = (tile as RuleOverrideTile).runtimeTile.m_Self;
			}
			else if (tile is RuleTile)
			{
				tile = (tile as RuleTile).m_Self;
			}
			if (neighbor != 1)
			{
				return neighbor != 2 || tile != this.m_Self;
			}
			return tile == this.m_Self;
		}

		// Token: 0x06001F1E RID: 7966 RVA: 0x0009C990 File Offset: 0x0009AB90
		protected bool RuleMatches(RuleTile.TilingRule rule, ref TileBase[] neighboringTiles, int angle)
		{
			for (int i = 0; i < this.neighborCount; i++)
			{
				int rotatedIndex = this.GetRotatedIndex(i, angle);
				TileBase tile = neighboringTiles[rotatedIndex];
				if (!this.RuleMatch(rule.m_Neighbors[i], tile))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001F1F RID: 7967 RVA: 0x0009C9D0 File Offset: 0x0009ABD0
		protected bool RuleMatches(RuleTile.TilingRule rule, ref TileBase[] neighboringTiles, bool mirrorX, bool mirrorY)
		{
			for (int i = 0; i < this.neighborCount; i++)
			{
				int mirroredIndex = this.GetMirroredIndex(i, mirrorX, mirrorY);
				TileBase tile = neighboringTiles[mirroredIndex];
				if (!this.RuleMatch(rule.m_Neighbors[i], tile))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001F20 RID: 7968 RVA: 0x0009CA14 File Offset: 0x0009AC14
		protected virtual void GetMatchingNeighboringTiles(ITilemap tilemap, Vector3Int position, ref TileBase[] neighboringTiles)
		{
			if (neighboringTiles != null)
			{
				return;
			}
			if (this.m_CachedNeighboringTiles == null || this.m_CachedNeighboringTiles.Length < this.neighborCount)
			{
				this.m_CachedNeighboringTiles = new TileBase[this.neighborCount];
			}
			int num = 0;
			for (int i = 1; i >= -1; i--)
			{
				for (int j = -1; j <= 1; j++)
				{
					if (j != 0 || i != 0)
					{
						Vector3Int position2 = new Vector3Int(position.x + j, position.y + i, position.z);
						this.m_CachedNeighboringTiles[num++] = tilemap.GetTile(position2);
					}
				}
			}
			neighboringTiles = this.m_CachedNeighboringTiles;
		}

		// Token: 0x06001F21 RID: 7969 RVA: 0x0009CAAC File Offset: 0x0009ACAC
		protected virtual int GetRotatedIndex(int original, int rotation)
		{
			if (rotation <= 90)
			{
				if (rotation == 0)
				{
					return original;
				}
				if (rotation == 90)
				{
					return RuleTile.RotatedOrMirroredIndexes[0, original];
				}
			}
			else
			{
				if (rotation == 180)
				{
					return RuleTile.RotatedOrMirroredIndexes[1, original];
				}
				if (rotation == 270)
				{
					return RuleTile.RotatedOrMirroredIndexes[2, original];
				}
			}
			return original;
		}

		// Token: 0x06001F22 RID: 7970 RVA: 0x0009CB04 File Offset: 0x0009AD04
		protected virtual int GetMirroredIndex(int original, bool mirrorX, bool mirrorY)
		{
			if (mirrorX && mirrorY)
			{
				return RuleTile.RotatedOrMirroredIndexes[1, original];
			}
			if (mirrorX)
			{
				return RuleTile.RotatedOrMirroredIndexes[3, original];
			}
			if (mirrorY)
			{
				return RuleTile.RotatedOrMirroredIndexes[4, original];
			}
			return original;
		}

		// Token: 0x04001908 RID: 6408
		private static readonly int[,] RotatedOrMirroredIndexes = new int[,]
		{
			{
				2,
				4,
				7,
				1,
				6,
				0,
				3,
				5
			},
			{
				7,
				6,
				5,
				4,
				3,
				2,
				1,
				0
			},
			{
				5,
				3,
				0,
				6,
				1,
				7,
				4,
				2
			},
			{
				2,
				1,
				0,
				4,
				3,
				7,
				6,
				5
			},
			{
				5,
				6,
				7,
				3,
				4,
				0,
				1,
				2
			}
		};

		// Token: 0x04001909 RID: 6409
		private static readonly int NeighborCount = 8;

		// Token: 0x0400190A RID: 6410
		public Sprite m_DefaultSprite;

		// Token: 0x0400190B RID: 6411
		public GameObject m_DefaultGameObject;

		// Token: 0x0400190C RID: 6412
		public Tile.ColliderType m_DefaultColliderType = Tile.ColliderType.Sprite;

		// Token: 0x0400190D RID: 6413
		protected TileBase[] m_CachedNeighboringTiles = new TileBase[RuleTile.NeighborCount];

		// Token: 0x0400190E RID: 6414
		private TileBase m_OverrideSelf;

		// Token: 0x0400190F RID: 6415
		private Quaternion m_GameObjectQuaternion;

		// Token: 0x04001910 RID: 6416
		[HideInInspector]
		public List<RuleTile.TilingRule> m_TilingRules;

		// Token: 0x02000576 RID: 1398
		[Serializable]
		public class TilingRule
		{
			// Token: 0x06001F25 RID: 7973 RVA: 0x0009CB78 File Offset: 0x0009AD78
			public TilingRule()
			{
				this.m_Output = RuleTile.TilingRule.OutputSprite.Single;
				this.m_Neighbors = new int[RuleTile.NeighborCount];
				this.m_Sprites = new Sprite[1];
				this.m_GameObject = null;
				this.m_AnimationSpeed = 1f;
				this.m_PerlinScale = 0.5f;
				this.m_ColliderType = Tile.ColliderType.Sprite;
				for (int i = 0; i < this.m_Neighbors.Length; i++)
				{
					this.m_Neighbors[i] = 0;
				}
			}

			// Token: 0x04001911 RID: 6417
			public int[] m_Neighbors;

			// Token: 0x04001912 RID: 6418
			public Sprite[] m_Sprites;

			// Token: 0x04001913 RID: 6419
			public GameObject m_GameObject;

			// Token: 0x04001914 RID: 6420
			public float m_AnimationSpeed;

			// Token: 0x04001915 RID: 6421
			public float m_PerlinScale;

			// Token: 0x04001916 RID: 6422
			public RuleTile.TilingRule.Transform m_RuleTransform;

			// Token: 0x04001917 RID: 6423
			public RuleTile.TilingRule.OutputSprite m_Output;

			// Token: 0x04001918 RID: 6424
			public Tile.ColliderType m_ColliderType;

			// Token: 0x04001919 RID: 6425
			public RuleTile.TilingRule.Transform m_RandomTransform;

			// Token: 0x02000577 RID: 1399
			public class Neighbor
			{
				// Token: 0x0400191A RID: 6426
				public const int DontCare = 0;

				// Token: 0x0400191B RID: 6427
				public const int This = 1;

				// Token: 0x0400191C RID: 6428
				public const int NotThis = 2;
			}

			// Token: 0x02000578 RID: 1400
			public enum Transform
			{
				// Token: 0x0400191E RID: 6430
				Fixed,
				// Token: 0x0400191F RID: 6431
				Rotated,
				// Token: 0x04001920 RID: 6432
				MirrorX,
				// Token: 0x04001921 RID: 6433
				MirrorY
			}

			// Token: 0x02000579 RID: 1401
			public enum OutputSprite
			{
				// Token: 0x04001923 RID: 6435
				Single,
				// Token: 0x04001924 RID: 6436
				Random,
				// Token: 0x04001925 RID: 6437
				Animation
			}
		}
	}
}
