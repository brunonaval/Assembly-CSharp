using System;
using System.Collections;
using Mirror;
using UnityEngine;

// Token: 0x02000017 RID: 23
public class NpcSpawnBrushManager : MonoBehaviour
{
	// Token: 0x0600005F RID: 95 RVA: 0x00003180 File Offset: 0x00001380
	private IEnumerator Start()
	{
		if (this.mapIcon != null & NetworkClient.active)
		{
			SpriteRenderer spriteRenderer;
			base.TryGetComponent<SpriteRenderer>(out spriteRenderer);
			spriteRenderer.transform.localScale = new Vector3(1f, 1f, 1f);
			spriteRenderer.sprite = this.mapIcon;
			yield break;
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("DatabaseManager");
		QuestDatabaseModule questDatabaseModule;
		gameObject.TryGetComponent<QuestDatabaseModule>(out questDatabaseModule);
		NpcDatabaseModule npcDatabaseModule;
		gameObject.TryGetComponent<NpcDatabaseModule>(out npcDatabaseModule);
		ItemDatabaseModule itemDatabaseModule;
		gameObject.TryGetComponent<ItemDatabaseModule>(out itemDatabaseModule);
		yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("GameEnvironment") != null);
		yield return new WaitUntil(() => questDatabaseModule.IsLoaded);
		yield return new WaitUntil(() => npcDatabaseModule.IsLoaded);
		yield return new WaitUntil(() => itemDatabaseModule.IsLoaded);
		if (NetworkServer.active && this.npcId != 0)
		{
			NetworkServer.Spawn(GlobalUtils.SpawnNpc(this.npcPrefab, npcDatabaseModule, this.npcId, base.transform.position), null);
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04000039 RID: 57
	[SerializeField]
	private int npcId;

	// Token: 0x0400003A RID: 58
	[SerializeField]
	private GameObject npcPrefab;

	// Token: 0x0400003B RID: 59
	[SerializeField]
	private Sprite mapIcon;
}
