using System;
using Mirror;
using UnityEngine;
using UnityEngine.Tilemaps;

// Token: 0x02000012 RID: 18
public class MapManager : MonoBehaviour
{
	// Token: 0x0600004D RID: 77 RVA: 0x00002E98 File Offset: 0x00001098
	private void Awake()
	{
		if (NetworkServer.active)
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				GameObject gameObject = base.transform.GetChild(i).gameObject;
				TilemapRenderer tilemapRenderer;
				TilemapCollider2D tilemapCollider2D;
				if (gameObject.TryGetComponent<TilemapRenderer>(out tilemapRenderer) && (!gameObject.activeInHierarchy || !gameObject.TryGetComponent<TilemapCollider2D>(out tilemapCollider2D)))
				{
					UnityEngine.Object.Destroy(gameObject);
				}
			}
			GameObject[] array = this.objectsToEnableOnServer;
			for (int j = 0; j < array.Length; j++)
			{
				array[j].SetActive(true);
			}
		}
	}

	// Token: 0x0400002B RID: 43
	[SerializeField]
	private GameObject[] objectsToEnableOnServer;
}
