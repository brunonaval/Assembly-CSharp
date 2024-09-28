using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

// Token: 0x020003A5 RID: 933
public class ObjectPoolModule : MonoBehaviour
{
	// Token: 0x06001357 RID: 4951 RVA: 0x0005F68A File Offset: 0x0005D88A
	private void Awake()
	{
		if (ObjectPoolModule.Instance != null)
		{
			return;
		}
		this.InitializeServerPools();
		this.InitializeClientPools();
		if (NetworkClient.active)
		{
			base.InvokeRepeating("CleanClientPools", 30f, 15f);
		}
		ObjectPoolModule.Instance = this;
	}

	// Token: 0x06001358 RID: 4952 RVA: 0x0005F6C8 File Offset: 0x0005D8C8
	private void InitializeClientPools()
	{
		if (!NetworkClient.active)
		{
			return;
		}
		this.soundEffectPool = new List<PoolObject>();
		this.visualEffectPool = new List<PoolObject>();
		this.animatedTextPool = new List<PoolObject>();
		this.screenMessageTextPool = new List<PoolObject>();
	}

	// Token: 0x06001359 RID: 4953 RVA: 0x0005F6FE File Offset: 0x0005D8FE
	private void CleanClientPools()
	{
		this.CleanSoundEffectPool();
		this.CleanVisualEffectPool();
		this.CleanAnimatedTextPool();
		this.CleanScreenMessageTextPool();
	}

	// Token: 0x0600135A RID: 4954 RVA: 0x0005F718 File Offset: 0x0005D918
	private void CleanScreenMessageTextPool()
	{
		for (int i = this.screenMessageTextPool.Count - 1; i >= 0; i--)
		{
			if (this.screenMessageTextPool[i].Object == null || (Time.time - this.screenMessageTextPool[i].SpawnTime > this.maxSpawnTime && !this.screenMessageTextPool[i].Object.activeInHierarchy))
			{
				if (this.screenMessageTextPool[i].Object != null)
				{
					UnityEngine.Object.Destroy(this.screenMessageTextPool[i].Object);
				}
				this.screenMessageTextPool.RemoveAt(i);
			}
		}
	}

	// Token: 0x0600135B RID: 4955 RVA: 0x0005F7D0 File Offset: 0x0005D9D0
	private void CleanSoundEffectPool()
	{
		for (int i = this.soundEffectPool.Count - 1; i >= 0; i--)
		{
			if (this.soundEffectPool[i].Object == null || (Time.time - this.soundEffectPool[i].SpawnTime > this.maxSpawnTime && !this.soundEffectPool[i].Object.activeInHierarchy))
			{
				if (this.soundEffectPool[i].Object != null)
				{
					UnityEngine.Object.Destroy(this.soundEffectPool[i].Object);
				}
				this.soundEffectPool.RemoveAt(i);
			}
		}
	}

	// Token: 0x0600135C RID: 4956 RVA: 0x0005F888 File Offset: 0x0005DA88
	private void CleanVisualEffectPool()
	{
		for (int i = this.visualEffectPool.Count - 1; i >= 0; i--)
		{
			if (this.visualEffectPool[i].Object == null || (Time.time - this.visualEffectPool[i].SpawnTime > this.maxSpawnTime && !this.visualEffectPool[i].Object.activeInHierarchy))
			{
				if (this.visualEffectPool[i].Object != null)
				{
					UnityEngine.Object.Destroy(this.visualEffectPool[i].Object);
				}
				this.visualEffectPool.RemoveAt(i);
			}
		}
	}

	// Token: 0x0600135D RID: 4957 RVA: 0x0005F940 File Offset: 0x0005DB40
	private void CleanAnimatedTextPool()
	{
		for (int i = this.animatedTextPool.Count - 1; i >= 0; i--)
		{
			if (this.animatedTextPool[i].Object == null || (Time.time - this.animatedTextPool[i].SpawnTime > this.maxSpawnTime && !this.animatedTextPool[i].Object.activeInHierarchy))
			{
				if (this.animatedTextPool[i].Object != null)
				{
					UnityEngine.Object.Destroy(this.animatedTextPool[i].Object);
				}
				this.animatedTextPool.RemoveAt(i);
			}
		}
	}

	// Token: 0x0600135E RID: 4958 RVA: 0x0005F9F8 File Offset: 0x0005DBF8
	private void InitializeServerPools()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		this.groundSlotPool = new List<GameObject>();
		this.spawnableProjectilesPool = new Dictionary<string, List<GameObject>>();
		this.serverOnlyProjectilesPool = new Dictionary<string, List<GameObject>>();
		string[] projectilePrefabNames = AssetBundleManager.Instance.GetProjectilePrefabNames();
		for (int i = 0; i < projectilePrefabNames.Length; i++)
		{
			this.spawnableProjectilesPool.Add(projectilePrefabNames[i], new List<GameObject>());
			this.serverOnlyProjectilesPool.Add(projectilePrefabNames[i], new List<GameObject>());
		}
	}

	// Token: 0x0600135F RID: 4959 RVA: 0x0005FA70 File Offset: 0x0005DC70
	public string GetServerPoolMonitorData()
	{
		string text = "[Pool Monitor]\r\n" + string.Format("GroundSlot Pool: {0} objects.\r\n", this.groundSlotPool.Count);
		foreach (KeyValuePair<string, List<GameObject>> keyValuePair in this.spawnableProjectilesPool)
		{
			text += string.Format("{0} Pool: {1} objects.\r\n", keyValuePair.Key, keyValuePair.Value.Count);
		}
		foreach (KeyValuePair<string, List<GameObject>> keyValuePair2 in this.serverOnlyProjectilesPool)
		{
			text += string.Format("{0} Pool: {1} server-only objects.\r\n", keyValuePair2.Key, keyValuePair2.Value.Count);
		}
		return text;
	}

	// Token: 0x06001360 RID: 4960 RVA: 0x0005FB70 File Offset: 0x0005DD70
	public GameObject GetScreenMessageTextFromPool(Vector3 position)
	{
		for (int i = 0; i < this.screenMessageTextPool.Count; i++)
		{
			if (this.screenMessageTextPool[i].Object == null)
			{
				this.screenMessageTextPool.RemoveAt(i);
			}
			else if (!this.screenMessageTextPool[i].Object.activeInHierarchy)
			{
				GameObject @object = this.screenMessageTextPool[i].Object;
				this.screenMessageTextPool[i].SpawnTime = Time.time;
				@object.transform.position = position;
				@object.SetActive(true);
				return @object;
			}
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.screenMessageTextPrefab, position, Quaternion.identity);
		this.screenMessageTextPool.Add(new PoolObject
		{
			Object = gameObject,
			SpawnTime = Time.time
		});
		return gameObject;
	}

	// Token: 0x06001361 RID: 4961 RVA: 0x0005FC48 File Offset: 0x0005DE48
	public GameObject GetAnimatedTextFromPool(Vector3 position)
	{
		for (int i = this.animatedTextPool.Count - 1; i >= 0; i--)
		{
			if (this.animatedTextPool[i].Object == null)
			{
				this.animatedTextPool.RemoveAt(i);
			}
			else if (!this.animatedTextPool[i].Object.activeInHierarchy)
			{
				GameObject @object = this.animatedTextPool[i].Object;
				this.animatedTextPool[i].SpawnTime = Time.time;
				@object.transform.position = position;
				@object.SetActive(true);
				return @object;
			}
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.animatedTextPrefab, position, Quaternion.identity);
		this.animatedTextPool.Add(new PoolObject
		{
			Object = gameObject,
			SpawnTime = Time.time
		});
		return gameObject;
	}

	// Token: 0x06001362 RID: 4962 RVA: 0x0005FD20 File Offset: 0x0005DF20
	public GameObject GetVisualEffectFromPool(Vector3 position)
	{
		for (int i = this.visualEffectPool.Count - 1; i >= 0; i--)
		{
			if (this.visualEffectPool[i].Object == null)
			{
				this.visualEffectPool.RemoveAt(i);
			}
			else if (!this.visualEffectPool[i].Object.activeInHierarchy)
			{
				GameObject @object = this.visualEffectPool[i].Object;
				this.visualEffectPool[i].SpawnTime = Time.time;
				@object.transform.position = position;
				@object.SetActive(true);
				return @object;
			}
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.visualEffectPrefab, position, Quaternion.identity);
		this.visualEffectPool.Add(new PoolObject
		{
			Object = gameObject,
			SpawnTime = Time.time
		});
		return gameObject;
	}

	// Token: 0x06001363 RID: 4963 RVA: 0x0005FDF8 File Offset: 0x0005DFF8
	public GameObject GetSoundEffectFromPool(Vector3 position)
	{
		position.z = -10f;
		for (int i = this.soundEffectPool.Count - 1; i >= 0; i--)
		{
			if (this.soundEffectPool[i].Object == null)
			{
				this.soundEffectPool.RemoveAt(i);
			}
			else if (!this.soundEffectPool[i].Object.activeInHierarchy)
			{
				GameObject @object = this.soundEffectPool[i].Object;
				this.soundEffectPool[i].SpawnTime = Time.time;
				@object.transform.position = position;
				@object.SetActive(true);
				return @object;
			}
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.soundEffectPrefab, position, Quaternion.identity);
		this.soundEffectPool.Add(new PoolObject
		{
			Object = gameObject,
			SpawnTime = Time.time
		});
		return gameObject;
	}

	// Token: 0x06001364 RID: 4964 RVA: 0x0005FEDC File Offset: 0x0005E0DC
	public GameObject GetGroundSlotFromPool(Vector3 position)
	{
		position.z = 0f;
		for (int i = 0; i < this.groundSlotPool.Count; i++)
		{
			if (!this.groundSlotPool[i].activeInHierarchy)
			{
				GameObject gameObject = this.groundSlotPool[i];
				gameObject.transform.position = position;
				gameObject.SetActive(true);
				return gameObject;
			}
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.groundSlotPrefab, position, Quaternion.identity);
		gameObject2.GetComponent<SpriteRenderer>().sortingOrder = 10;
		this.groundSlotPool.Add(gameObject2);
		return gameObject2;
	}

	// Token: 0x06001365 RID: 4965 RVA: 0x0005FF6C File Offset: 0x0005E16C
	public GameObject GetProjectileFromPool(Vector3 position, string projectilePrefabName)
	{
		if (string.IsNullOrEmpty(projectilePrefabName))
		{
			return null;
		}
		List<GameObject> list = this.spawnableProjectilesPool[projectilePrefabName];
		for (int i = 0; i < list.Count; i++)
		{
			if (!list[i].activeInHierarchy)
			{
				GameObject gameObject = list[i];
				gameObject.transform.position = position;
				gameObject.SetActive(true);
				return gameObject;
			}
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(AssetBundleManager.Instance.GetProjectilePrefab(projectilePrefabName), position, Quaternion.identity);
		this.spawnableProjectilesPool[projectilePrefabName].Add(gameObject2);
		return gameObject2;
	}

	// Token: 0x06001366 RID: 4966 RVA: 0x0005FFF4 File Offset: 0x0005E1F4
	public GameObject GetServerOnlyProjectileFromPool(Vector3 position, string projectilePrefabName)
	{
		if (string.IsNullOrEmpty(projectilePrefabName))
		{
			return null;
		}
		List<GameObject> list = this.serverOnlyProjectilesPool[projectilePrefabName];
		for (int i = 0; i < list.Count; i++)
		{
			if (!list[i].activeInHierarchy)
			{
				GameObject gameObject = list[i];
				gameObject.transform.position = position;
				gameObject.SetActive(true);
				return gameObject;
			}
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(AssetBundleManager.Instance.GetProjectilePrefab(projectilePrefabName), position, Quaternion.identity);
		NetworkIdentity networkIdentity;
		gameObject2.TryGetComponent<NetworkIdentity>(out networkIdentity);
		networkIdentity.serverOnly = true;
		this.serverOnlyProjectilesPool[projectilePrefabName].Add(gameObject2);
		return gameObject2;
	}

	// Token: 0x06001367 RID: 4967 RVA: 0x0006008C File Offset: 0x0005E28C
	public void ClearServerPools()
	{
		foreach (KeyValuePair<string, List<GameObject>> keyValuePair in this.spawnableProjectilesPool)
		{
			foreach (GameObject obj in keyValuePair.Value)
			{
				NetworkServer.Destroy(obj);
			}
			keyValuePair.Value.Clear();
		}
		foreach (KeyValuePair<string, List<GameObject>> keyValuePair2 in this.serverOnlyProjectilesPool)
		{
			foreach (GameObject obj2 in keyValuePair2.Value)
			{
				NetworkServer.Destroy(obj2);
			}
			keyValuePair2.Value.Clear();
		}
		foreach (GameObject obj3 in this.groundSlotPool)
		{
			NetworkServer.Destroy(obj3);
		}
		this.groundSlotPool.Clear();
	}

	// Token: 0x040011F1 RID: 4593
	[SerializeField]
	private GameObject groundSlotPrefab;

	// Token: 0x040011F2 RID: 4594
	[SerializeField]
	private GameObject visualEffectPrefab;

	// Token: 0x040011F3 RID: 4595
	[SerializeField]
	private GameObject soundEffectPrefab;

	// Token: 0x040011F4 RID: 4596
	[SerializeField]
	private GameObject animatedTextPrefab;

	// Token: 0x040011F5 RID: 4597
	[SerializeField]
	private GameObject screenMessageTextPrefab;

	// Token: 0x040011F6 RID: 4598
	private List<PoolObject> soundEffectPool;

	// Token: 0x040011F7 RID: 4599
	private List<PoolObject> visualEffectPool;

	// Token: 0x040011F8 RID: 4600
	private List<PoolObject> animatedTextPool;

	// Token: 0x040011F9 RID: 4601
	private List<PoolObject> screenMessageTextPool;

	// Token: 0x040011FA RID: 4602
	private Dictionary<string, List<GameObject>> spawnableProjectilesPool;

	// Token: 0x040011FB RID: 4603
	private Dictionary<string, List<GameObject>> serverOnlyProjectilesPool;

	// Token: 0x040011FC RID: 4604
	private List<GameObject> groundSlotPool;

	// Token: 0x040011FD RID: 4605
	public static ObjectPoolModule Instance;

	// Token: 0x040011FE RID: 4606
	private readonly float maxSpawnTime = 30f;
}
