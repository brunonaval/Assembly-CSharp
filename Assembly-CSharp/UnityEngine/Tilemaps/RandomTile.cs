using System;

namespace UnityEngine.Tilemaps
{
	// Token: 0x02000582 RID: 1410
	[CreateAssetMenu(fileName = "New Random Tile", menuName = "Tiles/Random Tile")]
	[Serializable]
	public class RandomTile : Tile
	{
		// Token: 0x06001F4B RID: 8011 RVA: 0x0009D7F0 File Offset: 0x0009B9F0
		public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData)
		{
			base.GetTileData(location, tileMap, ref tileData);
			if (this.m_Sprites != null && this.m_Sprites.Length != 0)
			{
				long num = (long)location.x;
				num = num + (long)((ulong)-1412623820) + (num << 15);
				num = (num + 159903659L ^ num >> 11);
				num ^= (long)location.y;
				num = num + 1185682173L + (num << 7);
				num = (num + (long)((ulong)-1097387857) ^ num << 11);
				Random.InitState((int)num);
				tileData.sprite = this.m_Sprites[(int)((float)this.m_Sprites.Length * Random.value)];
			}
		}

		// Token: 0x04001947 RID: 6471
		[SerializeField]
		public Sprite[] m_Sprites;
	}
}
