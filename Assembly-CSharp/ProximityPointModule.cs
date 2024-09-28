using System;
using Mirror;
using UnityEngine;
using UnityEngine.Rendering.Universal;

// Token: 0x020003F1 RID: 1009
public class ProximityPointModule : MonoBehaviour
{
	// Token: 0x06001596 RID: 5526 RVA: 0x0006E48A File Offset: 0x0006C68A
	private void Awake()
	{
		this.networkIdentity = base.GetComponentInParent<NetworkIdentity>();
	}

	// Token: 0x06001597 RID: 5527 RVA: 0x0006E498 File Offset: 0x0006C698
	private void Start()
	{
		if (NetworkServer.active)
		{
			return;
		}
		if (!this.networkIdentity.isLocalPlayer)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (this.networkIdentity.isLocalPlayer)
		{
			this.proximityPointCollider.radius = 12f;
			return;
		}
	}

	// Token: 0x06001598 RID: 5528 RVA: 0x0006E4E4 File Offset: 0x0006C6E4
	[ClientCallback]
	private void OnTriggerStay2D(Collider2D other)
	{
		if (!NetworkClient.active)
		{
			return;
		}
		this.ProcessPlayerEnterTrigger(other);
	}

	// Token: 0x06001599 RID: 5529 RVA: 0x0006E4F8 File Offset: 0x0006C6F8
	private void OnTriggerEnter2D(Collider2D other)
	{
		this.ProcessCombatantEnterTrigger(other);
		this.ProcessPlayerEnterTrigger(other);
	}

	// Token: 0x0600159A RID: 5530 RVA: 0x0006E508 File Offset: 0x0006C708
	private void ProcessCombatantEnterTrigger(Collider2D other)
	{
		if (!base.transform.parent.CompareTag("Combatant") & !base.transform.parent.CompareTag("Npc"))
		{
			return;
		}
		CombatModule combatModule;
		if (other.TryGetComponent<CombatModule>(out combatModule))
		{
			combatModule.AddNearbyTarget(base.transform.parent.gameObject);
			this.combatModule.AddNearbyTarget(other.gameObject);
		}
	}

	// Token: 0x0600159B RID: 5531 RVA: 0x0006E57C File Offset: 0x0006C77C
	private void ProcessPlayerEnterTrigger(Collider2D other)
	{
		if (!base.transform.parent.CompareTag("Player"))
		{
			return;
		}
		if (other.CompareTag("BlockAll"))
		{
			return;
		}
		if (other.CompareTag("BlockCreature"))
		{
			return;
		}
		if (other.CompareTag("BlockProjectile"))
		{
			return;
		}
		if (!NetworkClient.active || !other.CompareTag("Light"))
		{
			CombatModule combatModule;
			if (other.TryGetComponent<CombatModule>(out combatModule))
			{
				combatModule.AddNearbyTarget(base.transform.parent.gameObject);
				this.combatModule.AddNearbyTarget(other.gameObject);
			}
			ComponentManagementModule componentManagementModule;
			if (other.TryGetComponent<ComponentManagementModule>(out componentManagementModule))
			{
				componentManagementModule.AddObserver(base.transform.parent.gameObject);
			}
			return;
		}
		if (!this.networkIdentity.isLocalPlayer)
		{
			return;
		}
		if (SettingsManager.Instance.DisableLightSystem)
		{
			return;
		}
		Light2D light2D;
		other.TryGetComponent<Light2D>(out light2D);
		light2D.enabled = true;
	}

	// Token: 0x0600159C RID: 5532 RVA: 0x0006E65C File Offset: 0x0006C85C
	private void OnTriggerExit2D(Collider2D other)
	{
		this.ProcessCombatantExitTrigger(other);
		this.ProcessPlayerExitTrigger(other);
	}

	// Token: 0x0600159D RID: 5533 RVA: 0x0006E66C File Offset: 0x0006C86C
	private void ProcessCombatantExitTrigger(Collider2D other)
	{
		if (!base.transform.parent.CompareTag("Combatant") & !base.transform.parent.CompareTag("Npc"))
		{
			return;
		}
		CombatModule combatModule;
		if (other.TryGetComponent<CombatModule>(out combatModule))
		{
			combatModule.RemoveNearbyTarget(base.transform.parent.gameObject);
			this.combatModule.RemoveNearbyTarget(other.gameObject);
			if (NetworkServer.active && this.combatModule.Target == other.gameObject)
			{
				this.combatModule.RemoveTarget();
			}
		}
	}

	// Token: 0x0600159E RID: 5534 RVA: 0x0006E708 File Offset: 0x0006C908
	private void ProcessPlayerExitTrigger(Collider2D other)
	{
		if (!base.transform.parent.CompareTag("Player"))
		{
			return;
		}
		if (!NetworkClient.active || !other.CompareTag("Light"))
		{
			CombatModule combatModule;
			if (other.TryGetComponent<CombatModule>(out combatModule))
			{
				combatModule.RemoveNearbyTarget(base.transform.parent.gameObject);
				this.combatModule.RemoveNearbyTarget(other.gameObject);
				if (NetworkServer.active && this.combatModule.Target == other.gameObject)
				{
					this.combatModule.RemoveTarget();
				}
			}
			ComponentManagementModule componentManagementModule;
			if (other.TryGetComponent<ComponentManagementModule>(out componentManagementModule))
			{
				componentManagementModule.RemoveObserver(base.transform.parent.gameObject);
			}
			return;
		}
		if (!this.networkIdentity.isLocalPlayer)
		{
			return;
		}
		Light2D light2D;
		other.TryGetComponent<Light2D>(out light2D);
		light2D.enabled = false;
	}

	// Token: 0x040013BE RID: 5054
	private NetworkIdentity networkIdentity;

	// Token: 0x040013BF RID: 5055
	[SerializeField]
	private CombatModule combatModule;

	// Token: 0x040013C0 RID: 5056
	[SerializeField]
	private CircleCollider2D proximityPointCollider;
}
