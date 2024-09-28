using System;
using UnityEngine;
using UnityEngine.Tilemaps;

// Token: 0x0200001C RID: 28
[CreateAssetMenu(fileName = "New Replication Tile", menuName = "Tiles/Replication Tile")]
[Serializable]
public class ReplicationTile : Tile
{
	// Token: 0x0600006F RID: 111 RVA: 0x000033B4 File Offset: 0x000015B4
	public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
	{
		Tilemap component = tilemap.GetComponent<Tilemap>();
		if (!component.name.Equals(this.baseTilemap, StringComparison.InvariantCultureIgnoreCase))
		{
			return false;
		}
		foreach (ReplicatonConfig replicatonConfig in this.replicationConfig)
		{
			Transform transform = component.transform.parent.Find(replicatonConfig.TargetTilemapName);
			Tilemap tilemap2 = (transform != null) ? transform.GetComponent<Tilemap>() : null;
			if (tilemap2 != null)
			{
				Vector3Int position2 = new Vector3Int(position.x + replicatonConfig.PositionXOffset, position.y + replicatonConfig.PositionYOffset, position.z + replicatonConfig.PositionZOffset);
				tilemap2.SetTile(position2, replicatonConfig.Tile);
			}
		}
		return true;
	}

	// Token: 0x0400004A RID: 74
	[SerializeField]
	private string baseTilemap;

	// Token: 0x0400004B RID: 75
	[SerializeField]
	private ReplicatonConfig[] replicationConfig;
}
