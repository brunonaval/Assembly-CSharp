using System;

namespace UnityEngine.Tilemaps
{
	// Token: 0x02000585 RID: 1413
	[CreateAssetMenu(fileName = "New Weighted Random Tile", menuName = "Tiles/Weighted Random Tile")]
	[Serializable]
	public class WeightedRandomTile : Tile
	{
		// Token: 0x06001F54 RID: 8020 RVA: 0x0009DEB4 File Offset: 0x0009C0B4
		public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData)
		{
			base.GetTileData(location, tileMap, ref tileData);
			if (this.Sprites == null || this.Sprites.Length == 0)
			{
				return;
			}
			long num = (long)location.x;
			num = num + (long)((ulong)-1412623820) + (num << 15);
			num = (num + 159903659L ^ num >> 11);
			num ^= (long)location.y;
			num = num + 1185682173L + (num << 7);
			num = (num + (long)((ulong)-1097387857) ^ num << 11);
			Random.InitState((int)num);
			int num2 = 0;
			foreach (WeightedSprite weightedSprite in this.Sprites)
			{
				num2 += weightedSprite.Weight;
			}
			int num3 = Random.Range(0, num2);
			foreach (WeightedSprite weightedSprite2 in this.Sprites)
			{
				num3 -= weightedSprite2.Weight;
				if (num3 < 0)
				{
					tileData.sprite = weightedSprite2.Sprite;
					return;
				}
			}
		}

		// Token: 0x0400194B RID: 6475
		[SerializeField]
		public WeightedSprite[] Sprites;
	}
}
