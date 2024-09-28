using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Tilemaps;

namespace UnityEngine
{
	// Token: 0x02000570 RID: 1392
	[CreateAssetMenu(fileName = "New Rule Override Tile", menuName = "Tiles/Rule Override Tile")]
	[Serializable]
	public class RuleOverrideTile : TileBase
	{
		// Token: 0x170002CC RID: 716
		public Sprite this[Sprite originalSprite]
		{
			get
			{
				foreach (RuleOverrideTile.TileSpritePair tileSpritePair in this.m_Sprites)
				{
					if (tileSpritePair.m_OriginalSprite == originalSprite)
					{
						return tileSpritePair.m_OverrideSprite;
					}
				}
				return null;
			}
			set
			{
				if (value == null)
				{
					this.m_Sprites = (from spritePair in this.m_Sprites
					where spritePair.m_OriginalSprite != originalSprite
					select spritePair).ToList<RuleOverrideTile.TileSpritePair>();
					return;
				}
				foreach (RuleOverrideTile.TileSpritePair tileSpritePair in this.m_Sprites)
				{
					if (tileSpritePair.m_OriginalSprite == originalSprite)
					{
						tileSpritePair.m_OverrideSprite = value;
						return;
					}
				}
				this.m_Sprites.Add(new RuleOverrideTile.TileSpritePair
				{
					m_OriginalSprite = originalSprite,
					m_OverrideSprite = value
				});
			}
		}

		// Token: 0x170002CD RID: 717
		public RuleTile.TilingRule this[RuleTile.TilingRule originalRule]
		{
			get
			{
				if (!this.m_Tile)
				{
					return null;
				}
				int num = this.m_Tile.m_TilingRules.IndexOf(originalRule);
				if (num == -1)
				{
					return null;
				}
				if (this.m_OverrideTilingRules.Count < num + 1)
				{
					return null;
				}
				if (!this.m_OverrideTilingRules[num].m_Enabled)
				{
					return null;
				}
				return this.m_OverrideTilingRules[num].m_TilingRule;
			}
			set
			{
				if (!this.m_Tile)
				{
					return;
				}
				int num = this.m_Tile.m_TilingRules.IndexOf(originalRule);
				if (num == -1)
				{
					return;
				}
				if (value == null)
				{
					if (this.m_OverrideTilingRules.Count < num + 1)
					{
						return;
					}
					this.m_OverrideTilingRules[num].m_Enabled = false;
					while (this.m_OverrideTilingRules.Count > 0)
					{
						if (this.m_OverrideTilingRules[this.m_OverrideTilingRules.Count - 1].m_Enabled)
						{
							return;
						}
						this.m_OverrideTilingRules.RemoveAt(this.m_OverrideTilingRules.Count - 1);
					}
				}
				else
				{
					while (this.m_OverrideTilingRules.Count < num + 1)
					{
						this.m_OverrideTilingRules.Add(new RuleOverrideTile.OverrideTilingRule());
					}
					this.m_OverrideTilingRules[num].m_Enabled = true;
					this.m_OverrideTilingRules[num].m_TilingRule = this.CloneTilingRule(value);
					this.m_OverrideTilingRules[num].m_TilingRule.m_Neighbors = null;
				}
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06001EFE RID: 7934 RVA: 0x0009BE30 File Offset: 0x0009A030
		public RuleTile.TilingRule m_OriginalDefault
		{
			get
			{
				return new RuleTile.TilingRule
				{
					m_Sprites = new Sprite[]
					{
						(this.m_Tile != null) ? this.m_Tile.m_DefaultSprite : null
					},
					m_ColliderType = ((this.m_Tile != null) ? this.m_Tile.m_DefaultColliderType : Tile.ColliderType.None)
				};
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06001EFF RID: 7935 RVA: 0x0009BE91 File Offset: 0x0009A091
		public RuleTile runtimeTile
		{
			get
			{
				if (!this.m_RuntimeTile)
				{
					this.Override();
				}
				return this.m_RuntimeTile;
			}
		}

		// Token: 0x06001F00 RID: 7936 RVA: 0x0009BEAC File Offset: 0x0009A0AC
		public override bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
		{
			return this.runtimeTile.GetTileAnimationData(position, tilemap, ref tileAnimationData);
		}

		// Token: 0x06001F01 RID: 7937 RVA: 0x0009BEBC File Offset: 0x0009A0BC
		public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
		{
			this.runtimeTile.GetTileData(position, tilemap, ref tileData);
		}

		// Token: 0x06001F02 RID: 7938 RVA: 0x0009BECC File Offset: 0x0009A0CC
		public override void RefreshTile(Vector3Int position, ITilemap tilemap)
		{
			this.runtimeTile.RefreshTile(position, tilemap);
		}

		// Token: 0x06001F03 RID: 7939 RVA: 0x0009BEDB File Offset: 0x0009A0DB
		public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
		{
			return this.runtimeTile.StartUp(position, tilemap, go);
		}

		// Token: 0x06001F04 RID: 7940 RVA: 0x0009BEEC File Offset: 0x0009A0EC
		public void ApplyOverrides(IList<KeyValuePair<Sprite, Sprite>> overrides)
		{
			if (overrides == null)
			{
				throw new ArgumentNullException("overrides");
			}
			for (int i = 0; i < overrides.Count; i++)
			{
				this[overrides[i].Key] = overrides[i].Value;
			}
		}

		// Token: 0x06001F05 RID: 7941 RVA: 0x0009BF3C File Offset: 0x0009A13C
		public void GetOverrides(List<KeyValuePair<Sprite, Sprite>> overrides)
		{
			if (overrides == null)
			{
				throw new ArgumentNullException("overrides");
			}
			overrides.Clear();
			if (!this.m_Tile)
			{
				return;
			}
			List<Sprite> list = new List<Sprite>();
			if (this.m_Tile.m_DefaultSprite)
			{
				list.Add(this.m_Tile.m_DefaultSprite);
			}
			foreach (RuleTile.TilingRule tilingRule in this.m_Tile.m_TilingRules)
			{
				foreach (Sprite sprite in tilingRule.m_Sprites)
				{
					if (sprite && !list.Contains(sprite))
					{
						list.Add(sprite);
					}
				}
			}
			foreach (Sprite sprite2 in list)
			{
				overrides.Add(new KeyValuePair<Sprite, Sprite>(sprite2, this[sprite2]));
			}
		}

		// Token: 0x06001F06 RID: 7942 RVA: 0x0009C058 File Offset: 0x0009A258
		public void ApplyOverrides(IList<KeyValuePair<RuleTile.TilingRule, RuleTile.TilingRule>> overrides)
		{
			if (overrides == null)
			{
				throw new ArgumentNullException("overrides");
			}
			for (int i = 0; i < overrides.Count; i++)
			{
				this[overrides[i].Key] = overrides[i].Value;
			}
		}

		// Token: 0x06001F07 RID: 7943 RVA: 0x0009C0A8 File Offset: 0x0009A2A8
		public void GetOverrides(List<KeyValuePair<RuleTile.TilingRule, RuleTile.TilingRule>> overrides)
		{
			if (overrides == null)
			{
				throw new ArgumentNullException("overrides");
			}
			overrides.Clear();
			if (!this.m_Tile)
			{
				return;
			}
			foreach (RuleTile.TilingRule tilingRule in this.m_Tile.m_TilingRules)
			{
				RuleTile.TilingRule value = this[tilingRule];
				overrides.Add(new KeyValuePair<RuleTile.TilingRule, RuleTile.TilingRule>(tilingRule, value));
			}
			overrides.Add(new KeyValuePair<RuleTile.TilingRule, RuleTile.TilingRule>(this.m_OriginalDefault, this.m_OverrideDefault.m_TilingRule));
		}

		// Token: 0x06001F08 RID: 7944 RVA: 0x0009C14C File Offset: 0x0009A34C
		public void Override()
		{
			this.m_RuntimeTile = (this.m_Tile ? Object.Instantiate<RuleTile>(this.m_Tile) : new RuleTile());
			this.m_RuntimeTile.m_Self = (this.m_OverrideSelf ? this : this.m_Tile);
			if (!this.m_Advanced)
			{
				if (this.m_RuntimeTile.m_DefaultSprite)
				{
					this.m_RuntimeTile.m_DefaultSprite = this[this.m_RuntimeTile.m_DefaultSprite];
				}
				if (this.m_RuntimeTile.m_TilingRules == null)
				{
					return;
				}
				using (List<RuleTile.TilingRule>.Enumerator enumerator = this.m_RuntimeTile.m_TilingRules.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						RuleTile.TilingRule tilingRule = enumerator.Current;
						for (int i = 0; i < tilingRule.m_Sprites.Length; i++)
						{
							if (tilingRule.m_Sprites[i])
							{
								tilingRule.m_Sprites[i] = this[tilingRule.m_Sprites[i]];
							}
						}
					}
					return;
				}
			}
			if (this.m_OverrideDefault.m_Enabled)
			{
				this.m_RuntimeTile.m_DefaultSprite = ((this.m_OverrideDefault.m_TilingRule.m_Sprites.Length != 0) ? this.m_OverrideDefault.m_TilingRule.m_Sprites[0] : null);
				this.m_RuntimeTile.m_DefaultColliderType = this.m_OverrideDefault.m_TilingRule.m_ColliderType;
			}
			if (this.m_RuntimeTile.m_TilingRules != null)
			{
				for (int j = 0; j < this.m_RuntimeTile.m_TilingRules.Count; j++)
				{
					RuleTile.TilingRule to = this.m_RuntimeTile.m_TilingRules[j];
					RuleTile.TilingRule tilingRule2 = this[this.m_Tile.m_TilingRules[j]];
					if (tilingRule2 != null)
					{
						this.CopyTilingRule(tilingRule2, to, false);
					}
				}
			}
		}

		// Token: 0x06001F09 RID: 7945 RVA: 0x0009C320 File Offset: 0x0009A520
		public RuleTile.TilingRule CloneTilingRule(RuleTile.TilingRule from)
		{
			RuleTile.TilingRule tilingRule = new RuleTile.TilingRule();
			this.CopyTilingRule(from, tilingRule, true);
			return tilingRule;
		}

		// Token: 0x06001F0A RID: 7946 RVA: 0x0009C340 File Offset: 0x0009A540
		public void CopyTilingRule(RuleTile.TilingRule from, RuleTile.TilingRule to, bool copyRule)
		{
			if (copyRule)
			{
				to.m_Neighbors = from.m_Neighbors;
				to.m_RuleTransform = from.m_RuleTransform;
			}
			to.m_Sprites = (from.m_Sprites.Clone() as Sprite[]);
			to.m_GameObject = from.m_GameObject;
			to.m_AnimationSpeed = from.m_AnimationSpeed;
			to.m_PerlinScale = from.m_PerlinScale;
			to.m_Output = from.m_Output;
			to.m_ColliderType = from.m_ColliderType;
			to.m_RandomTransform = from.m_RandomTransform;
		}

		// Token: 0x040018FC RID: 6396
		public RuleTile m_Tile;

		// Token: 0x040018FD RID: 6397
		public bool m_OverrideSelf = true;

		// Token: 0x040018FE RID: 6398
		public bool m_Advanced;

		// Token: 0x040018FF RID: 6399
		public List<RuleOverrideTile.TileSpritePair> m_Sprites = new List<RuleOverrideTile.TileSpritePair>();

		// Token: 0x04001900 RID: 6400
		public List<RuleOverrideTile.OverrideTilingRule> m_OverrideTilingRules = new List<RuleOverrideTile.OverrideTilingRule>();

		// Token: 0x04001901 RID: 6401
		public RuleOverrideTile.OverrideTilingRule m_OverrideDefault = new RuleOverrideTile.OverrideTilingRule();

		// Token: 0x04001902 RID: 6402
		private RuleTile m_RuntimeTile;

		// Token: 0x02000571 RID: 1393
		[Serializable]
		public class TileSpritePair
		{
			// Token: 0x04001903 RID: 6403
			public Sprite m_OriginalSprite;

			// Token: 0x04001904 RID: 6404
			public Sprite m_OverrideSprite;
		}

		// Token: 0x02000572 RID: 1394
		[Serializable]
		public class OverrideTilingRule
		{
			// Token: 0x04001905 RID: 6405
			public bool m_Enabled;

			// Token: 0x04001906 RID: 6406
			public RuleTile.TilingRule m_TilingRule = new RuleTile.TilingRule();
		}
	}
}
