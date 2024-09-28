using System;
using Mirror;
using UnityEngine;

// Token: 0x02000395 RID: 917
[RequireComponent(typeof(MovementModule))]
public class NonPlayerAIModule : MonoBehaviour
{
	// Token: 0x060012B2 RID: 4786 RVA: 0x0005C3C8 File Offset: 0x0005A5C8
	private void Awake()
	{
		base.TryGetComponent<CombatModule>(out this.combatModule);
		base.TryGetComponent<CreatureModule>(out this.creatureModule);
		base.TryGetComponent<MovementModule>(out this.movementModule);
		base.TryGetComponent<ConditionModule>(out this.conditionModule);
		if (!NetworkServer.active)
		{
			return;
		}
		if (this.conditionModule != null)
		{
			this.conditionModule.OnConditionCreated += this.ConditionModule_OnConditionCreated;
			this.conditionModule.OnConditionRemoved += this.ConditionModule_OnConditionRemoved;
		}
	}

	// Token: 0x060012B3 RID: 4787 RVA: 0x0005C450 File Offset: 0x0005A650
	private void OnDestroy()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		if (this.conditionModule != null)
		{
			this.conditionModule.OnConditionCreated -= this.ConditionModule_OnConditionCreated;
			this.conditionModule.OnConditionRemoved -= this.ConditionModule_OnConditionRemoved;
		}
	}

	// Token: 0x060012B4 RID: 4788 RVA: 0x0005C4A1 File Offset: 0x0005A6A1
	private void Start()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		if (base.CompareTag("Npc") | base.CompareTag("Pet"))
		{
			this.chaseDestinationDistance = 0.64f;
		}
		this.canMove = true;
	}

	// Token: 0x060012B5 RID: 4789 RVA: 0x0005C4D6 File Offset: 0x0005A6D6
	private void ConditionModule_OnConditionRemoved(Condition condition)
	{
		this.canMove = this.movementModule.CanMove();
		if (!this.canMove)
		{
			this.RemoveDestination();
		}
	}

	// Token: 0x060012B6 RID: 4790 RVA: 0x0005C4D6 File Offset: 0x0005A6D6
	private void ConditionModule_OnConditionCreated(Condition condition)
	{
		this.canMove = this.movementModule.CanMove();
		if (!this.canMove)
		{
			this.RemoveDestination();
		}
	}

	// Token: 0x060012B7 RID: 4791 RVA: 0x0005C4F8 File Offset: 0x0005A6F8
	private void FixedUpdate()
	{
		if (!base.enabled)
		{
			return;
		}
		if (!NetworkServer.active)
		{
			return;
		}
		if (!this.canMove)
		{
			return;
		}
		if (!this.creatureModule.IsAlive)
		{
			return;
		}
		if (!this.movementModule.Movable)
		{
			return;
		}
		if (!this.ChaseTarget())
		{
			this.ChaseDestination();
		}
	}

	// Token: 0x060012B8 RID: 4792 RVA: 0x0005C54C File Offset: 0x0005A74C
	private bool ChaseTarget()
	{
		if (!this.combatModule.HasTarget || this.combatModule.Target == null)
		{
			return false;
		}
		if (GlobalUtils.IsClose(base.transform.position, this.combatModule.Target.transform.position, 0.4f))
		{
			return false;
		}
		Vector3 vector = this.combatModule.Target.transform.position - base.transform.position;
		vector.Normalize();
		this.movementModule.PhyMoveToPosition(vector);
		Direction direction = GlobalUtils.AngleToDirection(Mathf.Atan2(vector.y, vector.x) * 57.29578f);
		if (this.movementModule.Direction != direction)
		{
			this.movementModule.SetDirection(direction);
		}
		this.RemoveDestination();
		return true;
	}

	// Token: 0x060012B9 RID: 4793 RVA: 0x0005C630 File Offset: 0x0005A830
	private void ChaseDestination()
	{
		if (this.destination.Equals(this.vector3Zero))
		{
			return;
		}
		if (GlobalUtils.IsClose(base.transform.position, this.destination, this.chaseDestinationDistance))
		{
			this.RemoveDestination();
			this.movementModule.SetDirection(Direction.South);
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

	// Token: 0x060012BA RID: 4794 RVA: 0x0005C6F2 File Offset: 0x0005A8F2
	public void SetDestination(Vector3 newDestination)
	{
		if (this.movementModule.CanMove())
		{
			this.destination = newDestination;
			this.HasDestination = true;
		}
	}

	// Token: 0x060012BB RID: 4795 RVA: 0x0005C70F File Offset: 0x0005A90F
	public void RemoveDestination()
	{
		if (!this.destination.Equals(this.vector3Zero) | this.HasDestination)
		{
			this.destination = this.vector3Zero;
			this.HasDestination = false;
		}
	}

	// Token: 0x060012BC RID: 4796 RVA: 0x0005C741 File Offset: 0x0005A941
	public void SetCanMove(bool canMove)
	{
		this.canMove = canMove;
	}

	// Token: 0x04001180 RID: 4480
	public bool HasDestination;

	// Token: 0x04001181 RID: 4481
	private bool canMove;

	// Token: 0x04001182 RID: 4482
	private Vector3 vector3Zero = Vector3.zero;

	// Token: 0x04001183 RID: 4483
	private Vector3 destination;

	// Token: 0x04001184 RID: 4484
	private CombatModule combatModule;

	// Token: 0x04001185 RID: 4485
	private MovementModule movementModule;

	// Token: 0x04001186 RID: 4486
	private CreatureModule creatureModule;

	// Token: 0x04001187 RID: 4487
	private ConditionModule conditionModule;

	// Token: 0x04001188 RID: 4488
	private float chaseDestinationDistance = 0.08f;
}
