using System;
using System.Collections;
using System.Runtime.InteropServices;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

// Token: 0x02000386 RID: 902
public class MovementModule : NetworkBehaviour
{
	// Token: 0x170001F4 RID: 500
	// (get) Token: 0x06001220 RID: 4640 RVA: 0x00057329 File Offset: 0x00055529
	// (set) Token: 0x06001221 RID: 4641 RVA: 0x00057331 File Offset: 0x00055531
	public string SpawnPoint { get; private set; }

	// Token: 0x170001F5 RID: 501
	// (get) Token: 0x06001222 RID: 4642 RVA: 0x0005733A File Offset: 0x0005553A
	// (set) Token: 0x06001223 RID: 4643 RVA: 0x00057342 File Offset: 0x00055542
	public Vector3 SpawnPointLocation { get; private set; }

	// Token: 0x170001F6 RID: 502
	// (get) Token: 0x06001224 RID: 4644 RVA: 0x0005734B File Offset: 0x0005554B
	// (set) Token: 0x06001225 RID: 4645 RVA: 0x00057353 File Offset: 0x00055553
	public float LastMoveTime { get; private set; }

	// Token: 0x1400001C RID: 28
	// (add) Token: 0x06001226 RID: 4646 RVA: 0x0005735C File Offset: 0x0005555C
	// (remove) Token: 0x06001227 RID: 4647 RVA: 0x00057394 File Offset: 0x00055594
	public event MovementModule.OnSetSpawnPointEventHandler OnSetSpawnPoint;

	// Token: 0x06001228 RID: 4648 RVA: 0x000573CC File Offset: 0x000555CC
	private void Awake()
	{
		this.movementRigidbody = base.GetComponent<Rigidbody2D>();
		this.effectModule = base.GetComponent<EffectModule>();
		this.creatureModule = base.GetComponent<CreatureModule>();
		this.conditionModule = base.GetComponent<ConditionModule>();
		this.monsterModule = base.GetComponent<MonsterModule>();
		this.attributeModule = base.GetComponent<AttributeModule>();
		this.nonPlayerAttributeModule = base.GetComponent<NonPlayerAttributeModule>();
		if (NetworkClient.active)
		{
			this.trailRenderer = base.GetComponentInChildren<TrailRenderer>();
		}
	}

	// Token: 0x06001229 RID: 4649 RVA: 0x00057440 File Offset: 0x00055640
	private void Start()
	{
		this.LastMoveTime = Time.time;
	}

	// Token: 0x0600122A RID: 4650 RVA: 0x0005744D File Offset: 0x0005564D
	[Server]
	public void SetSpawnPointLocation(Vector3 spawnPointLocation, bool invokeEvents)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void MovementModule::SetSpawnPointLocation(UnityEngine.Vector3,System.Boolean)' called when server was not active");
			return;
		}
		this.SpawnPointLocation = spawnPointLocation;
		if (invokeEvents & this.OnSetSpawnPoint != null)
		{
			this.OnSetSpawnPoint(spawnPointLocation, string.Empty);
		}
	}

	// Token: 0x0600122B RID: 4651 RVA: 0x0005748C File Offset: 0x0005568C
	[Server]
	public void SetSpawnPoint(string spawnPoint, bool invokeEvents)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void MovementModule::SetSpawnPoint(System.String,System.Boolean)' called when server was not active");
			return;
		}
		Vector3 locationFromSpawnPoint = GlobalUtils.GetLocationFromSpawnPoint(spawnPoint);
		if (locationFromSpawnPoint == Vector3.zero)
		{
			Debug.LogError("SpawnPoint não encontrado " + spawnPoint);
			return;
		}
		this.SpawnPoint = spawnPoint;
		this.SpawnPointLocation = locationFromSpawnPoint;
		if (invokeEvents & this.OnSetSpawnPoint != null)
		{
			this.OnSetSpawnPoint(this.SpawnPointLocation, spawnPoint);
		}
	}

	// Token: 0x0600122C RID: 4652 RVA: 0x00057500 File Offset: 0x00055700
	public void Teleport(Vector3 position, Effect teleportEffect)
	{
		base.transform.position = position;
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = teleportEffect.Name,
			EffectScaleModifier = teleportEffect.ScaleModifier,
			EffectSpeedModifier = teleportEffect.SpeedModifier,
			Position = position
		};
		this.effectModule.ShowEffects(effectConfig);
	}

	// Token: 0x0600122D RID: 4653 RVA: 0x00057560 File Offset: 0x00055760
	[TargetRpc]
	public void TargetTeleport(NetworkConnection target, Vector3 position, Effect teleportEffect)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteVector3(position);
		Mirror.GeneratedNetworkCode._Write_Effect(writer, teleportEffect);
		this.SendTargetRPCInternal(target, "System.Void MovementModule::TargetTeleport(Mirror.NetworkConnection,UnityEngine.Vector3,Effect)", -69787453, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600122E RID: 4654 RVA: 0x000575A4 File Offset: 0x000557A4
	[ClientRpc]
	public void RpcTeleport(Vector3 position, Effect teleportEffect)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteVector3(position);
		Mirror.GeneratedNetworkCode._Write_Effect(writer, teleportEffect);
		this.SendRPCInternal("System.Void MovementModule::RpcTeleport(UnityEngine.Vector3,Effect)", -542929042, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600122F RID: 4655 RVA: 0x000575E8 File Offset: 0x000557E8
	[Command]
	public void CmdTeleport(Vector3 position, Effect teleportEffect)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteVector3(position);
		Mirror.GeneratedNetworkCode._Write_Effect(writer, teleportEffect);
		base.SendCommandInternal("System.Void MovementModule::CmdTeleport(UnityEngine.Vector3,Effect)", 138743875, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001230 RID: 4656 RVA: 0x0005762C File Offset: 0x0005582C
	[Command]
	public void CmdTeleport(string locationName, Effect teleportEffect)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(locationName);
		Mirror.GeneratedNetworkCode._Write_Effect(writer, teleportEffect);
		base.SendCommandInternal("System.Void MovementModule::CmdTeleport(System.String,Effect)", 1907458618, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001231 RID: 4657 RVA: 0x00057670 File Offset: 0x00055870
	private void OnDirection(Direction oldValue, Direction newValue)
	{
		if (base.isLocalPlayer)
		{
			return;
		}
		this.NetworkDirection = newValue;
	}

	// Token: 0x06001232 RID: 4658 RVA: 0x00057682 File Offset: 0x00055882
	public void SetDirection(Direction direction)
	{
		this.NetworkDirection = direction;
		if (base.isLocalPlayer)
		{
			this.CmdSetDirection(direction);
		}
	}

	// Token: 0x06001233 RID: 4659 RVA: 0x0005769C File Offset: 0x0005589C
	[Command]
	public void CmdSetDirection(Direction direction)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		Mirror.GeneratedNetworkCode._Write_Direction(writer, direction);
		base.SendCommandInternal("System.Void MovementModule::CmdSetDirection(Direction)", 1627885818, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001234 RID: 4660 RVA: 0x000576D8 File Offset: 0x000558D8
	[ClientRpc]
	public void RpcSetDirection(Direction direction)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		Mirror.GeneratedNetworkCode._Write_Direction(writer, direction);
		this.SendRPCInternal("System.Void MovementModule::RpcSetDirection(Direction)", 223528943, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001235 RID: 4661 RVA: 0x00057714 File Offset: 0x00055914
	[TargetRpc]
	public void TargetSetDirection(NetworkConnection target, Direction direction)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		Mirror.GeneratedNetworkCode._Write_Direction(writer, direction);
		this.SendTargetRPCInternal(target, "System.Void MovementModule::TargetSetDirection(Mirror.NetworkConnection,Direction)", -1370237602, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001236 RID: 4662 RVA: 0x00057750 File Offset: 0x00055950
	[Command]
	public void CmdSetMovable(bool value)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteBool(value);
		base.SendCommandInternal("System.Void MovementModule::CmdSetMovable(System.Boolean)", 1905225407, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001237 RID: 4663 RVA: 0x0005778A File Offset: 0x0005598A
	[Server]
	public void SetMovable(bool movable)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void MovementModule::SetMovable(System.Boolean)' called when server was not active");
			return;
		}
		this.NetworkMovable = movable;
	}

	// Token: 0x06001238 RID: 4664 RVA: 0x000577A8 File Offset: 0x000559A8
	public void PhyStop()
	{
		this.movementRigidbody.velocity = Vector2.zero;
	}

	// Token: 0x06001239 RID: 4665 RVA: 0x000577BC File Offset: 0x000559BC
	public void PhyMoveNorth(bool changeDirection)
	{
		this.LastMoveTime = Time.time;
		if (base.isOwned & !this.CanMove())
		{
			return;
		}
		if (this.attributeModule != null)
		{
			Vector2 force = Vector2.up * this.attributeModule.Speed;
			this.movementRigidbody.AddForce(force);
		}
		else if (this.nonPlayerAttributeModule != null)
		{
			Vector2 force2 = Vector2.up * this.nonPlayerAttributeModule.Speed;
			this.movementRigidbody.AddForce(force2);
		}
		if (changeDirection)
		{
			this.SetDirection(Direction.North);
		}
	}

	// Token: 0x0600123A RID: 4666 RVA: 0x00057854 File Offset: 0x00055A54
	public void PhyMoveEast(bool changeDirection)
	{
		this.LastMoveTime = Time.time;
		if (base.isOwned & !this.CanMove())
		{
			return;
		}
		if (this.attributeModule != null)
		{
			Vector2 force = Vector2.right * this.attributeModule.Speed;
			this.movementRigidbody.AddForce(force);
		}
		else if (this.nonPlayerAttributeModule != null)
		{
			Vector2 force2 = Vector2.right * this.nonPlayerAttributeModule.Speed;
			this.movementRigidbody.AddForce(force2);
		}
		if (changeDirection)
		{
			this.SetDirection(Direction.East);
		}
	}

	// Token: 0x0600123B RID: 4667 RVA: 0x000578EC File Offset: 0x00055AEC
	public void PhyMoveSouth(bool changeDirection)
	{
		this.LastMoveTime = Time.time;
		if (base.isOwned & !this.CanMove())
		{
			return;
		}
		if (this.attributeModule != null)
		{
			Vector2 force = Vector2.down * this.attributeModule.Speed;
			this.movementRigidbody.AddForce(force);
		}
		else if (this.nonPlayerAttributeModule != null)
		{
			Vector2 force2 = Vector2.down * this.nonPlayerAttributeModule.Speed;
			this.movementRigidbody.AddForce(force2);
		}
		if (changeDirection)
		{
			this.SetDirection(Direction.South);
		}
	}

	// Token: 0x0600123C RID: 4668 RVA: 0x00057984 File Offset: 0x00055B84
	public void PhyMoveWest(bool changeDirection)
	{
		this.LastMoveTime = Time.time;
		if (base.isOwned & !this.CanMove())
		{
			return;
		}
		if (this.attributeModule != null)
		{
			Vector2 force = Vector2.left * this.attributeModule.Speed;
			this.movementRigidbody.AddForce(force);
		}
		else if (this.nonPlayerAttributeModule != null)
		{
			Vector2 force2 = Vector2.left * this.nonPlayerAttributeModule.Speed;
			this.movementRigidbody.AddForce(force2);
		}
		if (changeDirection)
		{
			this.SetDirection(Direction.West);
		}
	}

	// Token: 0x0600123D RID: 4669 RVA: 0x00057A1C File Offset: 0x00055C1C
	public void PhyMoveToPosition(Vector2 position)
	{
		this.LastMoveTime = Time.time;
		if (this.movementRigidbody == null)
		{
			return;
		}
		if (base.isServer)
		{
			if (this.attributeModule != null)
			{
				Vector2 force = position * this.attributeModule.Speed;
				this.movementRigidbody.AddForce(force);
			}
			else if (this.nonPlayerAttributeModule != null)
			{
				Vector2 force2 = position * this.nonPlayerAttributeModule.Speed;
				this.movementRigidbody.AddForce(force2);
			}
			if (base.CompareTag("Player"))
			{
				this.TargetPhyMoveToPosition(base.connectionToClient, position);
				return;
			}
		}
		else if (base.isLocalPlayer)
		{
			Vector2 force3 = position * this.attributeModule.Speed;
			this.movementRigidbody.AddForce(force3);
		}
	}

	// Token: 0x0600123E RID: 4670 RVA: 0x00057AE8 File Offset: 0x00055CE8
	[TargetRpc]
	public void TargetPhyMoveToPosition(NetworkConnection target, Vector2 position)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteVector2(position);
		this.SendTargetRPCInternal(target, "System.Void MovementModule::TargetPhyMoveToPosition(Mirror.NetworkConnection,UnityEngine.Vector2)", 315467358, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600123F RID: 4671 RVA: 0x00057B24 File Offset: 0x00055D24
	public void KnockbackToPosition(Vector3 position, float speed)
	{
		if (base.CompareTag("Monster") && this.monsterModule.IsBoss)
		{
			return;
		}
		this.movementRigidbody.AddRelativeForce(position * speed, ForceMode2D.Impulse);
		if (base.isServer)
		{
			this.RpcKnockbackToPosition(position, speed);
		}
	}

	// Token: 0x06001240 RID: 4672 RVA: 0x00057B74 File Offset: 0x00055D74
	[ClientRpc]
	private void RpcKnockbackToPosition(Vector3 position, float speed)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteVector3(position);
		writer.WriteFloat(speed);
		this.SendRPCInternal("System.Void MovementModule::RpcKnockbackToPosition(UnityEngine.Vector3,System.Single)", -544873180, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001241 RID: 4673 RVA: 0x00057BB8 File Offset: 0x00055DB8
	public void Knockback(Direction direction, float speed)
	{
		if (base.CompareTag("Monster") && this.monsterModule.IsBoss)
		{
			return;
		}
		this.movementRigidbody.AddRelativeForce(GlobalUtils.DirectionToPosition(direction) * speed, ForceMode2D.Impulse);
		if (base.isServer)
		{
			this.RpcKnockback(direction, speed);
		}
	}

	// Token: 0x06001242 RID: 4674 RVA: 0x00057C10 File Offset: 0x00055E10
	[ClientRpc]
	private void RpcKnockback(Direction direction, float speed)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		Mirror.GeneratedNetworkCode._Write_Direction(writer, direction);
		writer.WriteFloat(speed);
		this.SendRPCInternal("System.Void MovementModule::RpcKnockback(Direction,System.Single)", -703558622, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001243 RID: 4675 RVA: 0x00057C54 File Offset: 0x00055E54
	[Server]
	public void DashToPosition(Vector3 position, float speed, bool consumeEndurance)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void MovementModule::DashToPosition(UnityEngine.Vector3,System.Single,System.Boolean)' called when server was not active");
			return;
		}
		if (consumeEndurance && this.attributeModule != null && this.attributeModule.CurrentEndurance < 50)
		{
			this.effectModule.TargetShowAnimatedText("movement_not_enough_endurance_message", 3, true, Vector3.zero);
			return;
		}
		if (this.CanDash())
		{
			this.movementRigidbody.AddRelativeForce(position * speed, ForceMode2D.Impulse);
			if (consumeEndurance && this.attributeModule != null)
			{
				this.attributeModule.AddEndurance(-50);
			}
			this.RpcDashToPosition(position, speed);
		}
	}

	// Token: 0x06001244 RID: 4676 RVA: 0x00057CF4 File Offset: 0x00055EF4
	[Server]
	public void Dash(Direction direction, float speed, bool consumeEndurance)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void MovementModule::Dash(Direction,System.Single,System.Boolean)' called when server was not active");
			return;
		}
		if (this.IsDashing)
		{
			return;
		}
		if (consumeEndurance & this.attributeModule.CurrentEndurance < 50)
		{
			this.effectModule.TargetShowAnimatedText("movement_not_enough_endurance_message", 3, true, Vector3.zero);
			return;
		}
		if (this.CanDash())
		{
			this.movementRigidbody.AddRelativeForce(GlobalUtils.DirectionToPosition(direction) * speed, ForceMode2D.Impulse);
			if (consumeEndurance)
			{
				this.attributeModule.AddEndurance(-50);
			}
			this.IsDashing = true;
			this.RpcDash(direction, speed);
		}
	}

	// Token: 0x06001245 RID: 4677 RVA: 0x00057D90 File Offset: 0x00055F90
	[Command]
	public void CmdDash(Direction direction)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		Mirror.GeneratedNetworkCode._Write_Direction(writer, direction);
		base.SendCommandInternal("System.Void MovementModule::CmdDash(Direction)", 620648581, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001246 RID: 4678 RVA: 0x00057DCC File Offset: 0x00055FCC
	[ClientRpc]
	private void RpcDash(Direction direction, float speed)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		Mirror.GeneratedNetworkCode._Write_Direction(writer, direction);
		writer.WriteFloat(speed);
		this.SendRPCInternal("System.Void MovementModule::RpcDash(Direction,System.Single)", -497122849, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001247 RID: 4679 RVA: 0x00057E10 File Offset: 0x00056010
	[ClientRpc]
	private void RpcDashToPosition(Vector3 position, float speed)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteVector3(position);
		writer.WriteFloat(speed);
		this.SendRPCInternal("System.Void MovementModule::RpcDashToPosition(UnityEngine.Vector3,System.Single)", 1932827233, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06001248 RID: 4680 RVA: 0x00057E54 File Offset: 0x00056054
	[Client]
	private void StopDash()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void MovementModule::StopDash()' called when client was not active");
			return;
		}
		this.movementRigidbody.velocity = Vector3.zero;
		if (this.trailRenderer != null)
		{
			this.trailRenderer.Clear();
			this.trailRenderer.time = 0f;
			this.trailRenderer.enabled = false;
		}
		if (base.isLocalPlayer)
		{
			this.CmdStopDash();
		}
	}

	// Token: 0x06001249 RID: 4681 RVA: 0x00057ED0 File Offset: 0x000560D0
	[Command]
	private void CmdStopDash()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		base.SendCommandInternal("System.Void MovementModule::CmdStopDash()", 706453264, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600124A RID: 4682 RVA: 0x00057F00 File Offset: 0x00056100
	[ClientRpc]
	private void RpcStopDash()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		this.SendRPCInternal("System.Void MovementModule::RpcStopDash()", -1510392325, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x0600124B RID: 4683 RVA: 0x00057F30 File Offset: 0x00056130
	[Client]
	private IEnumerator StopDashAfterTime(float time)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Collections.IEnumerator MovementModule::StopDashAfterTime(System.Single)' called when client was not active");
			return null;
		}
		MovementModule.<StopDashAfterTime>d__62 <StopDashAfterTime>d__ = new MovementModule.<StopDashAfterTime>d__62(0);
		<StopDashAfterTime>d__.<>4__this = this;
		<StopDashAfterTime>d__.time = time;
		return <StopDashAfterTime>d__;
	}

	// Token: 0x0600124C RID: 4684 RVA: 0x00057F74 File Offset: 0x00056174
	public bool CanMove()
	{
		return this.Movable && !(this.creatureModule != null & !this.creatureModule.IsAlive) && (this.conditionModule == null || (!this.conditionModule.HasActiveCondition(ConditionCategory.Confusion) && !this.conditionModule.HasActiveCondition(ConditionCategory.Paralyzing)));
	}

	// Token: 0x0600124D RID: 4685 RVA: 0x00057FDB File Offset: 0x000561DB
	public bool CanDash()
	{
		return !this.conditionModule.HasActiveCondition(ConditionType.Dash) && this.CanMove();
	}

	// Token: 0x0600124F RID: 4687 RVA: 0x0002FEC9 File Offset: 0x0002E0C9
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x170001F7 RID: 503
	// (get) Token: 0x06001250 RID: 4688 RVA: 0x00057FFC File Offset: 0x000561FC
	// (set) Token: 0x06001251 RID: 4689 RVA: 0x0005800F File Offset: 0x0005620F
	public bool NetworkMovable
	{
		get
		{
			return this.Movable;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<bool>(value, ref this.Movable, 1UL, null);
		}
	}

	// Token: 0x170001F8 RID: 504
	// (get) Token: 0x06001252 RID: 4690 RVA: 0x0005802C File Offset: 0x0005622C
	// (set) Token: 0x06001253 RID: 4691 RVA: 0x0005803F File Offset: 0x0005623F
	public Direction NetworkDirection
	{
		get
		{
			return this.Direction;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<Direction>(value, ref this.Direction, 2UL, new Action<Direction, Direction>(this.OnDirection));
		}
	}

	// Token: 0x06001254 RID: 4692 RVA: 0x00058064 File Offset: 0x00056264
	protected void UserCode_TargetTeleport__NetworkConnection__Vector3__Effect(NetworkConnection target, Vector3 position, Effect teleportEffect)
	{
		if (Physics2D.OverlapPoint(position, 1 << LayerMask.NameToLayer("WorldArea")) == null)
		{
			base.GetComponent<PlayerModule>().IsSceneLoaded = false;
		}
		this.Teleport(position, teleportEffect);
	}

	// Token: 0x06001255 RID: 4693 RVA: 0x0005809C File Offset: 0x0005629C
	protected static void InvokeUserCode_TargetTeleport__NetworkConnection__Vector3__Effect(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetTeleport called on server.");
			return;
		}
		((MovementModule)obj).UserCode_TargetTeleport__NetworkConnection__Vector3__Effect(null, reader.ReadVector3(), Mirror.GeneratedNetworkCode._Read_Effect(reader));
	}

	// Token: 0x06001256 RID: 4694 RVA: 0x000580CC File Offset: 0x000562CC
	protected void UserCode_RpcTeleport__Vector3__Effect(Vector3 position, Effect teleportEffect)
	{
		this.Teleport(position, teleportEffect);
	}

	// Token: 0x06001257 RID: 4695 RVA: 0x000580D6 File Offset: 0x000562D6
	protected static void InvokeUserCode_RpcTeleport__Vector3__Effect(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcTeleport called on server.");
			return;
		}
		((MovementModule)obj).UserCode_RpcTeleport__Vector3__Effect(reader.ReadVector3(), Mirror.GeneratedNetworkCode._Read_Effect(reader));
	}

	// Token: 0x06001258 RID: 4696 RVA: 0x00058105 File Offset: 0x00056305
	protected void UserCode_CmdTeleport__Vector3__Effect(Vector3 position, Effect teleportEffect)
	{
		this.Teleport(position, teleportEffect);
		this.RpcTeleport(position, teleportEffect);
	}

	// Token: 0x06001259 RID: 4697 RVA: 0x00058117 File Offset: 0x00056317
	protected static void InvokeUserCode_CmdTeleport__Vector3__Effect(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdTeleport called on client.");
			return;
		}
		((MovementModule)obj).UserCode_CmdTeleport__Vector3__Effect(reader.ReadVector3(), Mirror.GeneratedNetworkCode._Read_Effect(reader));
	}

	// Token: 0x0600125A RID: 4698 RVA: 0x00058148 File Offset: 0x00056348
	protected void UserCode_CmdTeleport__String__Effect(string locationName, Effect teleportEffect)
	{
		Vector3 locationFromSpawnPoint = GlobalUtils.GetLocationFromSpawnPoint(locationName);
		this.Teleport(locationFromSpawnPoint, teleportEffect);
		this.RpcTeleport(locationFromSpawnPoint, teleportEffect);
	}

	// Token: 0x0600125B RID: 4699 RVA: 0x0005816C File Offset: 0x0005636C
	protected static void InvokeUserCode_CmdTeleport__String__Effect(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdTeleport called on client.");
			return;
		}
		((MovementModule)obj).UserCode_CmdTeleport__String__Effect(reader.ReadString(), Mirror.GeneratedNetworkCode._Read_Effect(reader));
	}

	// Token: 0x0600125C RID: 4700 RVA: 0x0005819B File Offset: 0x0005639B
	protected void UserCode_CmdSetDirection__Direction(Direction direction)
	{
		this.NetworkDirection = direction;
		this.RpcSetDirection(direction);
	}

	// Token: 0x0600125D RID: 4701 RVA: 0x000581AB File Offset: 0x000563AB
	protected static void InvokeUserCode_CmdSetDirection__Direction(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSetDirection called on client.");
			return;
		}
		((MovementModule)obj).UserCode_CmdSetDirection__Direction(Mirror.GeneratedNetworkCode._Read_Direction(reader));
	}

	// Token: 0x0600125E RID: 4702 RVA: 0x000581D4 File Offset: 0x000563D4
	protected void UserCode_RpcSetDirection__Direction(Direction direction)
	{
		if (!base.isLocalPlayer)
		{
			this.NetworkDirection = direction;
		}
	}

	// Token: 0x0600125F RID: 4703 RVA: 0x000581E5 File Offset: 0x000563E5
	protected static void InvokeUserCode_RpcSetDirection__Direction(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcSetDirection called on server.");
			return;
		}
		((MovementModule)obj).UserCode_RpcSetDirection__Direction(Mirror.GeneratedNetworkCode._Read_Direction(reader));
	}

	// Token: 0x06001260 RID: 4704 RVA: 0x0005820E File Offset: 0x0005640E
	protected void UserCode_TargetSetDirection__NetworkConnection__Direction(NetworkConnection target, Direction direction)
	{
		this.NetworkDirection = direction;
	}

	// Token: 0x06001261 RID: 4705 RVA: 0x00058217 File Offset: 0x00056417
	protected static void InvokeUserCode_TargetSetDirection__NetworkConnection__Direction(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetSetDirection called on server.");
			return;
		}
		((MovementModule)obj).UserCode_TargetSetDirection__NetworkConnection__Direction(null, Mirror.GeneratedNetworkCode._Read_Direction(reader));
	}

	// Token: 0x06001262 RID: 4706 RVA: 0x00058241 File Offset: 0x00056441
	protected void UserCode_CmdSetMovable__Boolean(bool value)
	{
		this.SetMovable(value);
	}

	// Token: 0x06001263 RID: 4707 RVA: 0x0005824A File Offset: 0x0005644A
	protected static void InvokeUserCode_CmdSetMovable__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSetMovable called on client.");
			return;
		}
		((MovementModule)obj).UserCode_CmdSetMovable__Boolean(reader.ReadBool());
	}

	// Token: 0x06001264 RID: 4708 RVA: 0x00058274 File Offset: 0x00056474
	protected void UserCode_TargetPhyMoveToPosition__NetworkConnection__Vector2(NetworkConnection target, Vector2 position)
	{
		if (base.isOwned & !this.CanMove())
		{
			return;
		}
		Vector2 force = position * this.attributeModule.Speed;
		this.movementRigidbody.AddForce(force);
	}

	// Token: 0x06001265 RID: 4709 RVA: 0x000582B2 File Offset: 0x000564B2
	protected static void InvokeUserCode_TargetPhyMoveToPosition__NetworkConnection__Vector2(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetPhyMoveToPosition called on server.");
			return;
		}
		((MovementModule)obj).UserCode_TargetPhyMoveToPosition__NetworkConnection__Vector2(null, reader.ReadVector2());
	}

	// Token: 0x06001266 RID: 4710 RVA: 0x000582DC File Offset: 0x000564DC
	protected void UserCode_RpcKnockbackToPosition__Vector3__Single(Vector3 position, float speed)
	{
		this.movementRigidbody.AddRelativeForce(position * speed, ForceMode2D.Impulse);
	}

	// Token: 0x06001267 RID: 4711 RVA: 0x000582F6 File Offset: 0x000564F6
	protected static void InvokeUserCode_RpcKnockbackToPosition__Vector3__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcKnockbackToPosition called on server.");
			return;
		}
		((MovementModule)obj).UserCode_RpcKnockbackToPosition__Vector3__Single(reader.ReadVector3(), reader.ReadFloat());
	}

	// Token: 0x06001268 RID: 4712 RVA: 0x00058326 File Offset: 0x00056526
	protected void UserCode_RpcKnockback__Direction__Single(Direction direction, float speed)
	{
		this.movementRigidbody.AddRelativeForce(GlobalUtils.DirectionToPosition(direction) * speed, ForceMode2D.Impulse);
	}

	// Token: 0x06001269 RID: 4713 RVA: 0x00058345 File Offset: 0x00056545
	protected static void InvokeUserCode_RpcKnockback__Direction__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcKnockback called on server.");
			return;
		}
		((MovementModule)obj).UserCode_RpcKnockback__Direction__Single(Mirror.GeneratedNetworkCode._Read_Direction(reader), reader.ReadFloat());
	}

	// Token: 0x0600126A RID: 4714 RVA: 0x00058375 File Offset: 0x00056575
	protected void UserCode_CmdDash__Direction(Direction direction)
	{
		this.Dash(direction, 3f, true);
	}

	// Token: 0x0600126B RID: 4715 RVA: 0x00058384 File Offset: 0x00056584
	protected static void InvokeUserCode_CmdDash__Direction(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdDash called on client.");
			return;
		}
		((MovementModule)obj).UserCode_CmdDash__Direction(Mirror.GeneratedNetworkCode._Read_Direction(reader));
	}

	// Token: 0x0600126C RID: 4716 RVA: 0x000583B0 File Offset: 0x000565B0
	protected void UserCode_RpcDash__Direction__Single(Direction direction, float speed)
	{
		this.trailRenderer.enabled = true;
		this.trailRenderer.time = 0.2f;
		this.effectModule.PlaySoundEffect("rush", 1f, 0f, Vector3.zero);
		if (base.isLocalPlayer)
		{
			this.movementRigidbody.AddRelativeForce(GlobalUtils.DirectionToPosition(direction) * speed, ForceMode2D.Impulse);
			base.StartCoroutine(this.StopDashAfterTime(0.15f));
		}
	}

	// Token: 0x0600126D RID: 4717 RVA: 0x0005842F File Offset: 0x0005662F
	protected static void InvokeUserCode_RpcDash__Direction__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcDash called on server.");
			return;
		}
		((MovementModule)obj).UserCode_RpcDash__Direction__Single(Mirror.GeneratedNetworkCode._Read_Direction(reader), reader.ReadFloat());
	}

	// Token: 0x0600126E RID: 4718 RVA: 0x00058460 File Offset: 0x00056660
	protected void UserCode_RpcDashToPosition__Vector3__Single(Vector3 position, float speed)
	{
		this.trailRenderer.enabled = true;
		this.trailRenderer.time = 0.2f;
		this.effectModule.PlaySoundEffect("rush", 1f, 0f, Vector3.zero);
		if (base.isLocalPlayer)
		{
			this.movementRigidbody.AddRelativeForce(position * speed, ForceMode2D.Impulse);
		}
		if (base.isLocalPlayer || base.CompareTag("Monster"))
		{
			base.StartCoroutine(this.StopDashAfterTime(0.15f));
		}
	}

	// Token: 0x0600126F RID: 4719 RVA: 0x000584EF File Offset: 0x000566EF
	protected static void InvokeUserCode_RpcDashToPosition__Vector3__Single(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcDashToPosition called on server.");
			return;
		}
		((MovementModule)obj).UserCode_RpcDashToPosition__Vector3__Single(reader.ReadVector3(), reader.ReadFloat());
	}

	// Token: 0x06001270 RID: 4720 RVA: 0x0005851F File Offset: 0x0005671F
	protected void UserCode_CmdStopDash()
	{
		this.IsDashing = false;
		this.movementRigidbody.velocity = Vector3.zero;
		this.RpcStopDash();
	}

	// Token: 0x06001271 RID: 4721 RVA: 0x00058543 File Offset: 0x00056743
	protected static void InvokeUserCode_CmdStopDash(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdStopDash called on client.");
			return;
		}
		((MovementModule)obj).UserCode_CmdStopDash();
	}

	// Token: 0x06001272 RID: 4722 RVA: 0x00058568 File Offset: 0x00056768
	protected void UserCode_RpcStopDash()
	{
		if (!base.isLocalPlayer)
		{
			this.movementRigidbody.velocity = Vector3.zero;
			if (this.trailRenderer != null)
			{
				this.trailRenderer.Clear();
				this.trailRenderer.time = 0f;
				this.trailRenderer.enabled = false;
			}
		}
	}

	// Token: 0x06001273 RID: 4723 RVA: 0x000585C7 File Offset: 0x000567C7
	protected static void InvokeUserCode_RpcStopDash(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcStopDash called on server.");
			return;
		}
		((MovementModule)obj).UserCode_RpcStopDash();
	}

	// Token: 0x06001274 RID: 4724 RVA: 0x000585EC File Offset: 0x000567EC
	static MovementModule()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(MovementModule), "System.Void MovementModule::CmdTeleport(UnityEngine.Vector3,Effect)", new RemoteCallDelegate(MovementModule.InvokeUserCode_CmdTeleport__Vector3__Effect), true);
		RemoteProcedureCalls.RegisterCommand(typeof(MovementModule), "System.Void MovementModule::CmdTeleport(System.String,Effect)", new RemoteCallDelegate(MovementModule.InvokeUserCode_CmdTeleport__String__Effect), true);
		RemoteProcedureCalls.RegisterCommand(typeof(MovementModule), "System.Void MovementModule::CmdSetDirection(Direction)", new RemoteCallDelegate(MovementModule.InvokeUserCode_CmdSetDirection__Direction), true);
		RemoteProcedureCalls.RegisterCommand(typeof(MovementModule), "System.Void MovementModule::CmdSetMovable(System.Boolean)", new RemoteCallDelegate(MovementModule.InvokeUserCode_CmdSetMovable__Boolean), true);
		RemoteProcedureCalls.RegisterCommand(typeof(MovementModule), "System.Void MovementModule::CmdDash(Direction)", new RemoteCallDelegate(MovementModule.InvokeUserCode_CmdDash__Direction), true);
		RemoteProcedureCalls.RegisterCommand(typeof(MovementModule), "System.Void MovementModule::CmdStopDash()", new RemoteCallDelegate(MovementModule.InvokeUserCode_CmdStopDash), true);
		RemoteProcedureCalls.RegisterRpc(typeof(MovementModule), "System.Void MovementModule::RpcTeleport(UnityEngine.Vector3,Effect)", new RemoteCallDelegate(MovementModule.InvokeUserCode_RpcTeleport__Vector3__Effect));
		RemoteProcedureCalls.RegisterRpc(typeof(MovementModule), "System.Void MovementModule::RpcSetDirection(Direction)", new RemoteCallDelegate(MovementModule.InvokeUserCode_RpcSetDirection__Direction));
		RemoteProcedureCalls.RegisterRpc(typeof(MovementModule), "System.Void MovementModule::RpcKnockbackToPosition(UnityEngine.Vector3,System.Single)", new RemoteCallDelegate(MovementModule.InvokeUserCode_RpcKnockbackToPosition__Vector3__Single));
		RemoteProcedureCalls.RegisterRpc(typeof(MovementModule), "System.Void MovementModule::RpcKnockback(Direction,System.Single)", new RemoteCallDelegate(MovementModule.InvokeUserCode_RpcKnockback__Direction__Single));
		RemoteProcedureCalls.RegisterRpc(typeof(MovementModule), "System.Void MovementModule::RpcDash(Direction,System.Single)", new RemoteCallDelegate(MovementModule.InvokeUserCode_RpcDash__Direction__Single));
		RemoteProcedureCalls.RegisterRpc(typeof(MovementModule), "System.Void MovementModule::RpcDashToPosition(UnityEngine.Vector3,System.Single)", new RemoteCallDelegate(MovementModule.InvokeUserCode_RpcDashToPosition__Vector3__Single));
		RemoteProcedureCalls.RegisterRpc(typeof(MovementModule), "System.Void MovementModule::RpcStopDash()", new RemoteCallDelegate(MovementModule.InvokeUserCode_RpcStopDash));
		RemoteProcedureCalls.RegisterRpc(typeof(MovementModule), "System.Void MovementModule::TargetTeleport(Mirror.NetworkConnection,UnityEngine.Vector3,Effect)", new RemoteCallDelegate(MovementModule.InvokeUserCode_TargetTeleport__NetworkConnection__Vector3__Effect));
		RemoteProcedureCalls.RegisterRpc(typeof(MovementModule), "System.Void MovementModule::TargetSetDirection(Mirror.NetworkConnection,Direction)", new RemoteCallDelegate(MovementModule.InvokeUserCode_TargetSetDirection__NetworkConnection__Direction));
		RemoteProcedureCalls.RegisterRpc(typeof(MovementModule), "System.Void MovementModule::TargetPhyMoveToPosition(Mirror.NetworkConnection,UnityEngine.Vector2)", new RemoteCallDelegate(MovementModule.InvokeUserCode_TargetPhyMoveToPosition__NetworkConnection__Vector2));
	}

	// Token: 0x06001275 RID: 4725 RVA: 0x00058800 File Offset: 0x00056A00
	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteBool(this.Movable);
			Mirror.GeneratedNetworkCode._Write_Direction(writer, this.Direction);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1UL) != 0UL)
		{
			writer.WriteBool(this.Movable);
		}
		if ((base.syncVarDirtyBits & 2UL) != 0UL)
		{
			Mirror.GeneratedNetworkCode._Write_Direction(writer, this.Direction);
		}
	}

	// Token: 0x06001276 RID: 4726 RVA: 0x00058888 File Offset: 0x00056A88
	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			base.GeneratedSyncVarDeserialize<bool>(ref this.Movable, null, reader.ReadBool());
			base.GeneratedSyncVarDeserialize<Direction>(ref this.Direction, new Action<Direction, Direction>(this.OnDirection), Mirror.GeneratedNetworkCode._Read_Direction(reader));
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 1L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<bool>(ref this.Movable, null, reader.ReadBool());
		}
		if ((num & 2L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<Direction>(ref this.Direction, new Action<Direction, Direction>(this.OnDirection), Mirror.GeneratedNetworkCode._Read_Direction(reader));
		}
	}

	// Token: 0x04001115 RID: 4373
	[SyncVar]
	public bool Movable;

	// Token: 0x04001116 RID: 4374
	[SyncVar(hook = "OnDirection")]
	public Direction Direction;

	// Token: 0x04001117 RID: 4375
	public bool IsDashing;

	// Token: 0x0400111B RID: 4379
	private Rigidbody2D movementRigidbody;

	// Token: 0x0400111C RID: 4380
	private EffectModule effectModule;

	// Token: 0x0400111D RID: 4381
	private CreatureModule creatureModule;

	// Token: 0x0400111E RID: 4382
	private ConditionModule conditionModule;

	// Token: 0x0400111F RID: 4383
	private MonsterModule monsterModule;

	// Token: 0x04001120 RID: 4384
	private TrailRenderer trailRenderer;

	// Token: 0x04001121 RID: 4385
	private AttributeModule attributeModule;

	// Token: 0x04001122 RID: 4386
	private NonPlayerAttributeModule nonPlayerAttributeModule;

	// Token: 0x02000387 RID: 903
	// (Invoke) Token: 0x06001278 RID: 4728
	public delegate void OnSetSpawnPointEventHandler(Vector3 spawnPoint, string spawnPointName);
}
