using System;

namespace UnityEngine.Tilemaps
{
	// Token: 0x02000583 RID: 1411
	[CreateAssetMenu(fileName = "New Terrain Tile", menuName = "Tiles/Terrain Tile")]
	[Serializable]
	public class TerrainTile : TileBase
	{
		// Token: 0x06001F4D RID: 8013 RVA: 0x0009D888 File Offset: 0x0009BA88
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

		// Token: 0x06001F4E RID: 8014 RVA: 0x0009D8DE File Offset: 0x0009BADE
		public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData)
		{
			this.UpdateTile(location, tileMap, ref tileData);
		}

		// Token: 0x06001F4F RID: 8015 RVA: 0x0009D8EC File Offset: 0x0009BAEC
		private void UpdateTile(Vector3Int location, ITilemap tileMap, ref TileData tileData)
		{
			tileData.transform = Matrix4x4.identity;
			tileData.color = Color.white;
			int num = this.TileValue(tileMap, location + new Vector3Int(0, 1, 0)) ? 1 : 0;
			num += (this.TileValue(tileMap, location + new Vector3Int(1, 1, 0)) ? 2 : 0);
			num += (this.TileValue(tileMap, location + new Vector3Int(1, 0, 0)) ? 4 : 0);
			num += (this.TileValue(tileMap, location + new Vector3Int(1, -1, 0)) ? 8 : 0);
			num += (this.TileValue(tileMap, location + new Vector3Int(0, -1, 0)) ? 16 : 0);
			num += (this.TileValue(tileMap, location + new Vector3Int(-1, -1, 0)) ? 32 : 0);
			num += (this.TileValue(tileMap, location + new Vector3Int(-1, 0, 0)) ? 64 : 0);
			num += (this.TileValue(tileMap, location + new Vector3Int(-1, 1, 0)) ? 128 : 0);
			byte b = (byte)num;
			if ((b | 254) < 255)
			{
				num &= 125;
			}
			if ((b | 251) < 255)
			{
				num &= 245;
			}
			if ((b | 239) < 255)
			{
				num &= 215;
			}
			if ((b | 191) < 255)
			{
				num &= 95;
			}
			int index = this.GetIndex((byte)num);
			if (index >= 0 && index < this.m_Sprites.Length && this.TileValue(tileMap, location))
			{
				tileData.sprite = this.m_Sprites[index];
				tileData.transform = this.GetTransform((byte)num);
				tileData.color = Color.white;
				tileData.flags = TileFlags.LockAll;
				tileData.colliderType = Tile.ColliderType.Sprite;
			}
		}

		// Token: 0x06001F50 RID: 8016 RVA: 0x0009DAB0 File Offset: 0x0009BCB0
		private bool TileValue(ITilemap tileMap, Vector3Int position)
		{
			TileBase tile = tileMap.GetTile(position);
			return tile != null && tile == this;
		}

		// Token: 0x06001F51 RID: 8017 RVA: 0x0009DAD8 File Offset: 0x0009BCD8
		private int GetIndex(byte mask)
		{
			if (mask <= 197)
			{
				if (mask <= 87)
				{
					if (mask <= 23)
					{
						switch (mask)
						{
						case 0:
							return 0;
						case 1:
						case 4:
							break;
						case 2:
						case 3:
						case 6:
							return -1;
						case 5:
							return 2;
						case 7:
							return 3;
						default:
							switch (mask)
							{
							case 16:
								break;
							case 17:
								return 4;
							case 18:
							case 19:
							case 22:
								return -1;
							case 20:
								return 2;
							case 21:
								return 5;
							case 23:
								return 6;
							default:
								return -1;
							}
							break;
						}
					}
					else
					{
						switch (mask)
						{
						case 28:
							return 3;
						case 29:
							return 7;
						case 30:
							return -1;
						case 31:
							return 8;
						default:
							switch (mask)
							{
							case 64:
								break;
							case 65:
								return 2;
							case 66:
							case 67:
							case 70:
								return -1;
							case 68:
								return 4;
							case 69:
								return 5;
							case 71:
								return 7;
							default:
								switch (mask)
								{
								case 80:
									return 2;
								case 81:
								case 84:
									return 5;
								case 82:
								case 83:
								case 86:
									return -1;
								case 85:
									return 9;
								case 87:
									return 10;
								default:
									return -1;
								}
								break;
							}
							break;
						}
					}
					return 1;
				}
				if (mask <= 119)
				{
					switch (mask)
					{
					case 92:
						return 6;
					case 93:
						return 10;
					case 94:
						return -1;
					case 95:
						return 11;
					default:
						switch (mask)
						{
						case 112:
							break;
						case 113:
							return 6;
						case 114:
						case 115:
						case 118:
							return -1;
						case 116:
							return 7;
						case 117:
							return 10;
						case 119:
							return 12;
						default:
							return -1;
						}
						break;
					}
				}
				else
				{
					switch (mask)
					{
					case 124:
						return 8;
					case 125:
						return 11;
					case 126:
						return -1;
					case 127:
						return 13;
					default:
						if (mask != 193)
						{
							if (mask != 197)
							{
								return -1;
							}
							return 6;
						}
						break;
					}
				}
				return 3;
			}
			if (mask <= 221)
			{
				if (mask <= 209)
				{
					if (mask == 199)
					{
						return 8;
					}
					if (mask != 209)
					{
						return -1;
					}
				}
				else
				{
					if (mask == 213)
					{
						return 10;
					}
					if (mask == 215)
					{
						return 11;
					}
					if (mask != 221)
					{
						return -1;
					}
					return 12;
				}
			}
			else if (mask <= 245)
			{
				if (mask == 223)
				{
					return 13;
				}
				if (mask == 241)
				{
					return 8;
				}
				if (mask != 245)
				{
					return -1;
				}
				return 11;
			}
			else
			{
				if (mask == 247 || mask == 253)
				{
					return 13;
				}
				if (mask != 255)
				{
					return -1;
				}
				return 14;
			}
			return 7;
		}

		// Token: 0x06001F52 RID: 8018 RVA: 0x0009DCE0 File Offset: 0x0009BEE0
		private Matrix4x4 GetTransform(byte mask)
		{
			if (mask <= 193)
			{
				if (mask <= 71)
				{
					if (mask <= 16)
					{
						if (mask != 4)
						{
							if (mask != 16)
							{
								goto IL_1BF;
							}
							goto IL_177;
						}
					}
					else if (mask != 20 && mask != 28)
					{
						switch (mask)
						{
						case 64:
						case 65:
						case 69:
						case 71:
							goto IL_19B;
						case 66:
						case 67:
						case 70:
							goto IL_1BF;
						case 68:
							break;
						default:
							goto IL_1BF;
						}
					}
				}
				else if (mask <= 93)
				{
					if (mask - 80 <= 1)
					{
						goto IL_177;
					}
					if (mask != 84 && mask - 92 > 1)
					{
						goto IL_1BF;
					}
				}
				else
				{
					switch (mask)
					{
					case 112:
					case 113:
					case 117:
						goto IL_177;
					case 114:
					case 115:
						goto IL_1BF;
					case 116:
						break;
					default:
						if (mask - 124 > 1)
						{
							if (mask != 193)
							{
								goto IL_1BF;
							}
							goto IL_19B;
						}
						break;
					}
				}
			}
			else if (mask <= 215)
			{
				if (mask <= 199)
				{
					if (mask != 197 && mask != 199)
					{
						goto IL_1BF;
					}
					goto IL_19B;
				}
				else
				{
					if (mask == 209)
					{
						goto IL_177;
					}
					if (mask != 213 && mask != 215)
					{
						goto IL_1BF;
					}
					goto IL_19B;
				}
			}
			else if (mask <= 241)
			{
				if (mask != 221)
				{
					if (mask == 223)
					{
						goto IL_19B;
					}
					if (mask != 241)
					{
						goto IL_1BF;
					}
					goto IL_177;
				}
			}
			else
			{
				if (mask == 245 || mask == 247)
				{
					goto IL_177;
				}
				if (mask != 253)
				{
					goto IL_1BF;
				}
			}
			return Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, -90f), Vector3.one);
			IL_177:
			return Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, -180f), Vector3.one);
			IL_19B:
			return Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, -270f), Vector3.one);
			IL_1BF:
			return Matrix4x4.identity;
		}

		// Token: 0x04001948 RID: 6472
		[SerializeField]
		public Sprite[] m_Sprites;
	}
}
