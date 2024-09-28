using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

// Token: 0x020002ED RID: 749
[RequireComponent(typeof(NetworkIdentity))]
public class ComponentManagementModule : MonoBehaviour
{
	// Token: 0x06000D8D RID: 3469 RVA: 0x0003FFA4 File Offset: 0x0003E1A4
	private void Start()
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		this.spawned = true;
		this.tempObservers.Clear();
		base.TryGetComponent<NpcModule>(out this.npcModule);
		base.TryGetComponent<CombatModule>(out this.combatModule);
		base.TryGetComponent<MonsterModule>(out this.monsterModule);
		base.TryGetComponent<MovementModule>(out this.movementModule);
		base.TryGetComponent<NetworkIdentity>(out this.networkIdentity);
		base.TryGetComponent<NetworkTransformUnreliable>(out this.networkTransform);
		base.TryGetComponent<NonPlayerAIModule>(out this.nonPlayerAIModule);
		base.TryGetComponent<AnimationControllerModule>(out this.animationControllerModule);
	}

	// Token: 0x06000D8E RID: 3470 RVA: 0x00040039 File Offset: 0x0003E239
	private void Update()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		if (this.networkIdentity.serverOnly)
		{
			return;
		}
		if (Time.time - this.lastRebuildTime > this.updateInterval)
		{
			this.RebuildObservers();
			this.lastRebuildTime = Time.time;
		}
	}

	// Token: 0x06000D8F RID: 3471 RVA: 0x00040078 File Offset: 0x0003E278
	public void AddObserver(GameObject observer)
	{
		NetworkIdentity component = observer.GetComponent<NetworkIdentity>();
		if (component == null)
		{
			return;
		}
		if (observer != null && observer.activeInHierarchy && !this.tempObservers.Contains(component))
		{
			this.tempObservers.Add(component);
			if (base.CompareTag("Projectile") | base.CompareTag("GroundSlot") | base.CompareTag("Npc"))
			{
				this.RebuildObservers();
				return;
			}
			this.EnableScripts();
		}
	}

	// Token: 0x06000D90 RID: 3472 RVA: 0x000400F4 File Offset: 0x0003E2F4
	public void RemoveObserver(GameObject observer)
	{
		NetworkIdentity component = observer.GetComponent<NetworkIdentity>();
		if (component == null)
		{
			return;
		}
		this.tempObservers.Remove(component);
	}

	// Token: 0x06000D91 RID: 3473 RVA: 0x00040120 File Offset: 0x0003E320
	private void RebuildObservers()
	{
		if (this.tempObservers.Count == 0)
		{
			this.DisableScripts();
			return;
		}
		this.EnableScripts();
		for (int i = this.tempObservers.Count - 1; i >= 0; i--)
		{
			if (this.tempObservers[i] == null)
			{
				this.tempObservers.RemoveAt(i);
			}
			else if ((this.tempObservers[i].transform.position - base.transform.position).sqrMagnitude >= (float)(this.visibilityRange * this.visibilityRange))
			{
				this.tempObservers.RemoveAt(i);
			}
		}
	}

	// Token: 0x06000D92 RID: 3474 RVA: 0x000401CC File Offset: 0x0003E3CC
	private void DisableScripts()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		if (base.CompareTag("Monster") | base.CompareTag("DeadMonster"))
		{
			base.enabled = false;
			this.combatModule.enabled = false;
			this.monsterModule.enabled = false;
			this.networkTransform.enabled = false;
			this.nonPlayerAIModule.enabled = false;
			this.animationControllerModule.enabled = false;
			if (this.movementModule.SpawnPointLocation != Vector3.zero)
			{
				this.movementModule.Teleport(this.movementModule.SpawnPointLocation, default(Effect));
				this.movementModule.SetDirection(Direction.South);
			}
			this.AutoUnSpawn();
		}
		if ((base.CompareTag("Npc") || base.CompareTag("Combatant")) && this.npcModule.Owner == null)
		{
			base.enabled = false;
			this.npcModule.enabled = false;
			this.combatModule.enabled = false;
			this.networkTransform.enabled = false;
			this.nonPlayerAIModule.enabled = false;
			this.animationControllerModule.enabled = false;
			if (this.movementModule.SpawnPointLocation != Vector3.zero)
			{
				this.movementModule.Teleport(this.movementModule.SpawnPointLocation, default(Effect));
				this.movementModule.SetDirection(Direction.South);
			}
			this.AutoUnSpawn();
		}
	}

	// Token: 0x06000D93 RID: 3475 RVA: 0x00040348 File Offset: 0x0003E548
	private void EnableScripts()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		if (base.enabled)
		{
			return;
		}
		if (base.CompareTag("Monster") | base.CompareTag("DeadMonster"))
		{
			this.AutoSpawn();
			base.enabled = true;
			this.combatModule.enabled = true;
			this.monsterModule.enabled = true;
			this.networkTransform.enabled = true;
			this.nonPlayerAIModule.enabled = true;
			this.animationControllerModule.enabled = true;
		}
		if (base.CompareTag("Npc") || base.CompareTag("Combatant"))
		{
			this.AutoSpawn();
			base.enabled = true;
			this.npcModule.enabled = true;
			this.combatModule.enabled = true;
			this.networkTransform.enabled = true;
			this.nonPlayerAIModule.enabled = true;
			this.animationControllerModule.enabled = true;
		}
	}

	// Token: 0x06000D94 RID: 3476 RVA: 0x0004042B File Offset: 0x0003E62B
	private void AutoUnSpawn()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		if (!this.spawned)
		{
			return;
		}
		this.spawned = false;
		NetworkServer.UnSpawn(base.gameObject);
	}

	// Token: 0x06000D95 RID: 3477 RVA: 0x00040450 File Offset: 0x0003E650
	private void AutoSpawn()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		if (this.spawned)
		{
			return;
		}
		this.spawned = true;
		NetworkServer.Spawn(base.gameObject, null);
	}

	// Token: 0x04000E03 RID: 3587
	[SerializeField]
	private int visibilityRange = 5;

	// Token: 0x04000E04 RID: 3588
	[SerializeField]
	private float updateInterval = 1f;

	// Token: 0x04000E05 RID: 3589
	private bool spawned;

	// Token: 0x04000E06 RID: 3590
	private float lastRebuildTime;

	// Token: 0x04000E07 RID: 3591
	private NetworkIdentity networkIdentity;

	// Token: 0x04000E08 RID: 3592
	private readonly List<NetworkIdentity> tempObservers = new List<NetworkIdentity>();

	// Token: 0x04000E09 RID: 3593
	private NpcModule npcModule;

	// Token: 0x04000E0A RID: 3594
	private CombatModule combatModule;

	// Token: 0x04000E0B RID: 3595
	private MonsterModule monsterModule;

	// Token: 0x04000E0C RID: 3596
	private MovementModule movementModule;

	// Token: 0x04000E0D RID: 3597
	private NetworkTransformUnreliable networkTransform;

	// Token: 0x04000E0E RID: 3598
	private NonPlayerAIModule nonPlayerAIModule;

	// Token: 0x04000E0F RID: 3599
	private AnimationControllerModule animationControllerModule;
}
