using System;
using Mirror;
using UnityEngine;

// Token: 0x020002A1 RID: 673
public class AIModule : MonoBehaviour
{
	// Token: 0x06000A9C RID: 2716 RVA: 0x0003046A File Offset: 0x0002E66A
	private void Awake()
	{
		this.movementModule = base.GetComponent<MovementModule>();
		this.networkIdentity = base.GetComponent<NetworkIdentity>();
	}

	// Token: 0x06000A9D RID: 2717 RVA: 0x00030484 File Offset: 0x0002E684
	private void FixedUpdate()
	{
		if (NetworkServer.active)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		if (!this.networkIdentity.isLocalPlayer)
		{
			return;
		}
		if (this.destination == Vector3.zero)
		{
			return;
		}
		if (!this.movementModule.CanMove())
		{
			return;
		}
		this.GoToDestination();
	}

	// Token: 0x06000A9E RID: 2718 RVA: 0x000304D4 File Offset: 0x0002E6D4
	private void GoToDestination()
	{
		if (!this.HasDestination)
		{
			return;
		}
		if (GlobalUtils.IsClose(base.transform.position, this.destination, 0.34f))
		{
			this.RemoveDestination();
			return;
		}
		Vector3 vector = this.destination - base.transform.position;
		vector.Normalize();
		this.movementModule.PhyMoveToPosition(vector);
		Direction direction = GlobalUtils.AngleToDirection(Mathf.Atan2(vector.y, vector.x) * 57.29578f);
		if (this.movementModule.Direction != direction)
		{
			this.movementModule.SetDirection(direction);
		}
	}

	// Token: 0x06000A9F RID: 2719 RVA: 0x0003057E File Offset: 0x0002E77E
	public void SetDestination(Vector2 newDestination)
	{
		if (this.movementModule.CanMove())
		{
			this.destination = newDestination;
			this.HasDestination = true;
		}
	}

	// Token: 0x06000AA0 RID: 2720 RVA: 0x000305A0 File Offset: 0x0002E7A0
	public void RemoveDestination()
	{
		if (this.destination != Vector3.zero | this.HasDestination)
		{
			this.destination = Vector3.zero;
			this.HasDestination = false;
		}
	}

	// Token: 0x04000C11 RID: 3089
	private Vector3 destination;

	// Token: 0x04000C12 RID: 3090
	private MovementModule movementModule;

	// Token: 0x04000C13 RID: 3091
	private NetworkIdentity networkIdentity;

	// Token: 0x04000C14 RID: 3092
	public bool HasDestination;
}
