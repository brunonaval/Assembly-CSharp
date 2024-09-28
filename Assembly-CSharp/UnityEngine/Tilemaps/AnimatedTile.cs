using System;

namespace UnityEngine.Tilemaps
{
	// Token: 0x02000580 RID: 1408
	[CreateAssetMenu(fileName = "New Animated Tile", menuName = "Tiles/Animated Tile")]
	[Serializable]
	public class AnimatedTile : TileBase
	{
		// Token: 0x06001F41 RID: 8001 RVA: 0x0009D4A4 File Offset: 0x0009B6A4
		public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData)
		{
			tileData.transform = Matrix4x4.identity;
			tileData.color = Color.white;
			if (this.m_AnimatedSprites != null && this.m_AnimatedSprites.Length != 0)
			{
				tileData.sprite = this.m_AnimatedSprites[this.m_AnimatedSprites.Length - 1];
				tileData.colliderType = this.m_TileColliderType;
			}
		}

		// Token: 0x06001F42 RID: 8002 RVA: 0x0009D4FB File Offset: 0x0009B6FB
		public override bool GetTileAnimationData(Vector3Int location, ITilemap tileMap, ref TileAnimationData tileAnimationData)
		{
			if (this.m_AnimatedSprites.Length != 0)
			{
				tileAnimationData.animatedSprites = this.m_AnimatedSprites;
				tileAnimationData.animationSpeed = Random.Range(this.m_MinSpeed, this.m_MaxSpeed);
				tileAnimationData.animationStartTime = this.m_AnimationStartTime;
				return true;
			}
			return false;
		}

		// Token: 0x04001941 RID: 6465
		public Sprite[] m_AnimatedSprites;

		// Token: 0x04001942 RID: 6466
		public float m_MinSpeed = 1f;

		// Token: 0x04001943 RID: 6467
		public float m_MaxSpeed = 1f;

		// Token: 0x04001944 RID: 6468
		public float m_AnimationStartTime;

		// Token: 0x04001945 RID: 6469
		public Tile.ColliderType m_TileColliderType;
	}
}
