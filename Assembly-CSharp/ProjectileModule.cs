using System;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

// Token: 0x020003F0 RID: 1008
[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileModule : NetworkBehaviour
{
	// Token: 0x06001566 RID: 5478 RVA: 0x0006D51C File Offset: 0x0006B71C
	private void Awake()
	{
		this.effectModule = base.GetComponent<EffectModule>();
		this.projectileBody2D = base.GetComponent<Rigidbody2D>();
		this.spriteRenderer = base.GetComponent<SpriteRenderer>();
		if (NetworkServer.active)
		{
			UnityEngine.Object.Destroy(this.spriteRenderer);
			this.spriteRenderer = null;
			this.sprites = null;
		}
	}

	// Token: 0x06001567 RID: 5479 RVA: 0x0006D56D File Offset: 0x0006B76D
	private void Update()
	{
		if (NetworkClient.active)
		{
			this.UpdateSprite();
		}
		if (NetworkServer.active)
		{
			this.DestroyIfFarAway();
			this.DestroyIfAttackerIsDead();
			this.DestroyIfTargetIsDead();
		}
	}

	// Token: 0x06001568 RID: 5480 RVA: 0x0006D595 File Offset: 0x0006B795
	private void FixedUpdate()
	{
		this.ChaseTarget();
	}

	// Token: 0x06001569 RID: 5481 RVA: 0x0006D59D File Offset: 0x0006B79D
	private void OnEnable()
	{
		if (!NetworkClient.active)
		{
			return;
		}
		base.Invoke("DestroyIfTimeOut", 2f);
	}

	// Token: 0x0600156A RID: 5482 RVA: 0x0006D5B7 File Offset: 0x0006B7B7
	private void OnDisable()
	{
		if (!NetworkClient.active)
		{
			return;
		}
		base.CancelInvoke("DestroyIfTimeOut");
	}

	// Token: 0x0600156B RID: 5483 RVA: 0x0006D5CC File Offset: 0x0006B7CC
	public override void OnStartClient()
	{
		if (this.Networkattacker == null || this.Networkattacker.transform == null)
		{
			return;
		}
		if (this.useAttackerAsStartingPoint)
		{
			this.NetworkstartPoint = this.Networkattacker.transform.position + this.shootPivot;
		}
		this.effectModule.PlaySoundEffect(this.shootSoundEffectName, 1f, 0f, this.startPoint);
		base.transform.position = this.startPoint;
		this.projectileBody2D.velocity = this.velocity;
	}

	// Token: 0x0600156C RID: 5484 RVA: 0x0006D676 File Offset: 0x0006B876
	[ServerCallback]
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		if (other.CompareTag("ProximityPoint"))
		{
			return;
		}
		this.ProcessCollision(other.gameObject);
	}

	// Token: 0x0600156D RID: 5485 RVA: 0x0006D69D File Offset: 0x0006B89D
	private void DestroyIfTimeOut()
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		this.UnSpawnAndResetProjectile();
	}

	// Token: 0x0600156E RID: 5486 RVA: 0x0006D6C6 File Offset: 0x0006B8C6
	private void DestroyIfTargetIsDead()
	{
		if (this.Networktarget == null)
		{
			return;
		}
		if (!this.Networktarget.CompareTag("DeadMonster"))
		{
			return;
		}
		this.UnSpawnAndResetProjectile();
	}

	// Token: 0x0600156F RID: 5487 RVA: 0x0006D6F0 File Offset: 0x0006B8F0
	private void DestroyIfAttackerIsDead()
	{
		if (this.Networkattacker != null && !this.Networkattacker.CompareTag("DeadMonster"))
		{
			return;
		}
		this.UnSpawnAndResetProjectile();
	}

	// Token: 0x06001570 RID: 5488 RVA: 0x0006D71C File Offset: 0x0006B91C
	private void ChaseTarget()
	{
		if (this.Networktarget == null)
		{
			return;
		}
		if (this.Networktarget.CompareTag("DeadMonster"))
		{
			return;
		}
		if (GlobalUtils.IsClose(this.Networktarget.transform.position, base.transform.position, 0.32f))
		{
			this.ProcessCollision(this.Networktarget);
			return;
		}
		Vector3 vector = this.Networktarget.transform.position - base.transform.position;
		vector.Normalize();
		if (vector == Vector3.zero)
		{
			return;
		}
		float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
		base.transform.rotation = Quaternion.Euler(0f, 0f, num + this.angle);
		this.projectileBody2D.velocity = vector * this.projectileSpeed;
	}

	// Token: 0x06001571 RID: 5489 RVA: 0x0006D815 File Offset: 0x0006BA15
	[Server]
	public void Config(ProjectileConfig projectileConfig, Direction direction)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ProjectileModule::Config(ProjectileConfig,Direction)' called when server was not active");
			return;
		}
		this.SetConfigVariables(projectileConfig);
		base.transform.rotation = GlobalUtils.DirectionToRotation(direction, this.angle);
	}

	// Token: 0x06001572 RID: 5490 RVA: 0x0006D84A File Offset: 0x0006BA4A
	[Server]
	public void Config(ProjectileConfig projectileConfig, float targetAngle)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ProjectileModule::Config(ProjectileConfig,System.Single)' called when server was not active");
			return;
		}
		this.SetConfigVariables(projectileConfig);
		base.transform.rotation = Quaternion.Euler(0f, 0f, targetAngle + this.angle);
	}

	// Token: 0x06001573 RID: 5491 RVA: 0x0006D88C File Offset: 0x0006BA8C
	[Server]
	private void SetConfigVariables(ProjectileConfig projectileConfig)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ProjectileModule::SetConfigVariables(ProjectileConfig)' called when server was not active");
			return;
		}
		this.skill = projectileConfig.Skill;
		this.ammo = projectileConfig.Ammo;
		this.NetworkcanCauseDamage = this.skill.CanCauseDamage;
		this.NetworkstartPoint = projectileConfig.StartPoint;
		this.Networkattacker = projectileConfig.Attacker;
		this.skillCondition = projectileConfig.Condition;
		this.skillConditionChance = projectileConfig.ConditionChance;
		this.Networkvelocity = projectileConfig.Velocity;
		this.NetworkshootPivot = projectileConfig.ShootPivot;
		this.NetworkuseAttackerAsStartingPoint = projectileConfig.UseAttackerAsStartPoint;
		this.NetworkshootSoundEffectName = this.ammo.ProjectileShootSoundEffectName;
		this.NetworkprojectileSpeed = this.ammo.Projectile.Speed;
		this.NetworkmaxRange = this.skill.Range;
		this.DefineTarget(projectileConfig);
	}

	// Token: 0x06001574 RID: 5492 RVA: 0x0006D96A File Offset: 0x0006BB6A
	public void UnSpawnAndResetProjectile()
	{
		NetworkServer.UnSpawn(base.gameObject);
		this.ResetProjectile();
	}

	// Token: 0x06001575 RID: 5493 RVA: 0x0006D980 File Offset: 0x0006BB80
	private void ResetProjectile()
	{
		base.gameObject.SetActive(false);
		this.triggered = false;
		this.skill = default(Skill);
		this.ammo = default(Item);
		this.NetworkcanCauseDamage = false;
		this.NetworkstartPoint = Vector3.zero;
		this.Networkattacker = null;
		this.skillCondition = default(Condition);
		this.Networkvelocity = Vector3.zero;
		this.NetworkshootPivot = Vector3.zero;
		this.NetworkuseAttackerAsStartingPoint = false;
		this.NetworkshootSoundEffectName = null;
		this.NetworkprojectileSpeed = 0f;
		this.NetworkmaxRange = 0f;
		this.Networktarget = null;
		base.transform.rotation = Quaternion.identity;
	}

	// Token: 0x06001576 RID: 5494 RVA: 0x0006DA38 File Offset: 0x0006BC38
	private void DefineTarget(ProjectileConfig projectileConfig)
	{
		if (projectileConfig.Target == null)
		{
			return;
		}
		if (projectileConfig.Target.CompareTag("DeadMonster"))
		{
			return;
		}
		if (projectileConfig.Target.CompareTag("Projectile"))
		{
			return;
		}
		NetworkIdentity networkIdentity;
		if (!projectileConfig.Target.TryGetComponent<NetworkIdentity>(out networkIdentity))
		{
			return;
		}
		if (networkIdentity.netId == 0U)
		{
			return;
		}
		this.Networktarget = projectileConfig.Target;
	}

	// Token: 0x06001577 RID: 5495 RVA: 0x0006DAA0 File Offset: 0x0006BCA0
	[Client]
	private void UpdateSprite()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void ProjectileModule::UpdateSprite()' called when client was not active");
			return;
		}
		float num = Vector2.Distance(this.startPoint, base.transform.position);
		if (this.triggered | num > this.maxRange)
		{
			this.spriteRenderer.enabled = false;
			return;
		}
		if (this.sprites != null)
		{
			this.spriteRenderer.enabled = (num > this.startDistance);
			if (this.spriteRenderer.enabled)
			{
				float num2 = (this.framesPerSecond > 0f) ? this.framesPerSecond : 13f;
				int num3 = (int)(Time.timeSinceLevelLoad * num2);
				num3 %= this.sprites.Length;
				this.spriteRenderer.sprite = this.sprites[num3];
			}
		}
	}

	// Token: 0x06001578 RID: 5496 RVA: 0x0006DB68 File Offset: 0x0006BD68
	[ServerCallback]
	private void ProcessCollision(GameObject blockingObject)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		if (!this.ValidateBlockingTags(blockingObject))
		{
			return;
		}
		if (!this.IsValidBlock(blockingObject))
		{
			return;
		}
		this.ExplodeProjectile(blockingObject);
	}

	// Token: 0x06001579 RID: 5497 RVA: 0x0006DB90 File Offset: 0x0006BD90
	private bool ValidateBlockingTags(GameObject blockingObject)
	{
		if (this.triggered)
		{
			return false;
		}
		Transform parent = blockingObject.transform.parent;
		if (parent != null)
		{
			for (int i = 0; i < GlobalSettings.BlockableTags.Length; i++)
			{
				if (parent.CompareTag(GlobalSettings.BlockableTags[i]))
				{
					blockingObject = parent.gameObject;
					return true;
				}
			}
		}
		for (int j = 0; j < GlobalSettings.BlockableTags.Length; j++)
		{
			if (blockingObject.CompareTag(GlobalSettings.BlockableTags[j]))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600157A RID: 5498 RVA: 0x0006DC0C File Offset: 0x0006BE0C
	private void ExplodeProjectile(GameObject blockingObject)
	{
		this.triggered = true;
		try
		{
			this.Networktarget = blockingObject;
			ProjectileBase projectileBase = this.CreateProjectileBehaviour(this.ammo.Projectile.Script);
			ConditionConfig conditionConfig = new ConditionConfig(this.skillConditionChance, this.skillCondition);
			ProjectileBaseConfig projectileBaseConfig = new ProjectileBaseConfig(this.skill, this.ammo, this.Networktarget, this.Networkattacker, base.gameObject, conditionConfig);
			projectileBase.ExplodeAndDestroy(projectileBaseConfig);
			this.Networktarget = null;
		}
		catch (Exception ex)
		{
			Debug.Log("Error: " + ex.Message + " Details: " + ex.StackTrace);
		}
	}

	// Token: 0x0600157B RID: 5499 RVA: 0x0006DCB8 File Offset: 0x0006BEB8
	private ProjectileBase CreateProjectileBehaviour(string className)
	{
		return ClassFactory.GetFromCache<ProjectileBase>(className, Array.Empty<object>());
	}

	// Token: 0x0600157C RID: 5500 RVA: 0x0006DCC5 File Offset: 0x0006BEC5
	private void DestroyIfFarAway()
	{
		if (Vector2.Distance(this.startPoint, base.transform.position) > this.maxRange)
		{
			this.UnSpawnAndResetProjectile();
		}
	}

	// Token: 0x0600157D RID: 5501 RVA: 0x0006DCF0 File Offset: 0x0006BEF0
	private bool IsValidBlock(GameObject target)
	{
		if (this.Networkattacker == null || !this.Networkattacker.activeInHierarchy)
		{
			return false;
		}
		if (target == null || !target.activeInHierarchy)
		{
			return false;
		}
		if (this.Networkattacker == target)
		{
			return false;
		}
		if (target.CompareTag("BlockAll") | target.CompareTag("BlockProjectile"))
		{
			return true;
		}
		CreatureModule component = target.GetComponent<CreatureModule>();
		if (component != null && !component.IsAlive)
		{
			return false;
		}
		bool flag = this.Networkattacker.CompareTag("Player");
		bool flag2 = this.Networkattacker.CompareTag("Monster");
		bool flag3 = this.Networkattacker.CompareTag("Combatant");
		bool flag4 = target.CompareTag("Player");
		bool flag5 = target.CompareTag("Monster");
		bool flag6 = target.CompareTag("Combatant");
		if (flag)
		{
			if (flag5 & !this.skill.CanCauseDamage)
			{
				return false;
			}
			bool pvpEnabled = this.Networkattacker.GetComponent<PvpModule>().PvpEnabled;
			return !(flag4 & this.skill.CanCauseDamage & !pvpEnabled) && !(flag6 & this.skill.CanCauseDamage & !pvpEnabled);
		}
		else
		{
			if (!flag3)
			{
				return flag2 && (flag4 || flag6);
			}
			if (flag4)
			{
				return target.GetComponent<PvpModule>().HasPlayerKillerLimitations();
			}
			return flag5;
		}
	}

	// Token: 0x0600157F RID: 5503 RVA: 0x0002FEC9 File Offset: 0x0002E0C9
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x17000234 RID: 564
	// (get) Token: 0x06001580 RID: 5504 RVA: 0x0006DE44 File Offset: 0x0006C044
	// (set) Token: 0x06001581 RID: 5505 RVA: 0x0006DE57 File Offset: 0x0006C057
	public Vector2 Networkvelocity
	{
		get
		{
			return this.velocity;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<Vector2>(value, ref this.velocity, 1UL, null);
		}
	}

	// Token: 0x17000235 RID: 565
	// (get) Token: 0x06001582 RID: 5506 RVA: 0x0006DE74 File Offset: 0x0006C074
	// (set) Token: 0x06001583 RID: 5507 RVA: 0x0006DE87 File Offset: 0x0006C087
	public Vector3 NetworkshootPivot
	{
		get
		{
			return this.shootPivot;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<Vector3>(value, ref this.shootPivot, 2UL, null);
		}
	}

	// Token: 0x17000236 RID: 566
	// (get) Token: 0x06001584 RID: 5508 RVA: 0x0006DEA4 File Offset: 0x0006C0A4
	// (set) Token: 0x06001585 RID: 5509 RVA: 0x0006DEC3 File Offset: 0x0006C0C3
	public GameObject Networkattacker
	{
		get
		{
			return base.GetSyncVarGameObject(this.___attackerNetId, ref this.attacker);
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter_GameObject(value, ref this.attacker, 4UL, null, ref this.___attackerNetId);
		}
	}

	// Token: 0x17000237 RID: 567
	// (get) Token: 0x06001586 RID: 5510 RVA: 0x0006DEE4 File Offset: 0x0006C0E4
	// (set) Token: 0x06001587 RID: 5511 RVA: 0x0006DF03 File Offset: 0x0006C103
	public GameObject Networktarget
	{
		get
		{
			return base.GetSyncVarGameObject(this.___targetNetId, ref this.target);
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter_GameObject(value, ref this.target, 8UL, null, ref this.___targetNetId);
		}
	}

	// Token: 0x17000238 RID: 568
	// (get) Token: 0x06001588 RID: 5512 RVA: 0x0006DF24 File Offset: 0x0006C124
	// (set) Token: 0x06001589 RID: 5513 RVA: 0x0006DF37 File Offset: 0x0006C137
	public float NetworkmaxRange
	{
		get
		{
			return this.maxRange;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<float>(value, ref this.maxRange, 16UL, null);
		}
	}

	// Token: 0x17000239 RID: 569
	// (get) Token: 0x0600158A RID: 5514 RVA: 0x0006DF54 File Offset: 0x0006C154
	// (set) Token: 0x0600158B RID: 5515 RVA: 0x0006DF67 File Offset: 0x0006C167
	public Vector2 NetworkstartPoint
	{
		get
		{
			return this.startPoint;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<Vector2>(value, ref this.startPoint, 32UL, null);
		}
	}

	// Token: 0x1700023A RID: 570
	// (get) Token: 0x0600158C RID: 5516 RVA: 0x0006DF84 File Offset: 0x0006C184
	// (set) Token: 0x0600158D RID: 5517 RVA: 0x0006DF97 File Offset: 0x0006C197
	public bool NetworkuseAttackerAsStartingPoint
	{
		get
		{
			return this.useAttackerAsStartingPoint;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<bool>(value, ref this.useAttackerAsStartingPoint, 64UL, null);
		}
	}

	// Token: 0x1700023B RID: 571
	// (get) Token: 0x0600158E RID: 5518 RVA: 0x0006DFB4 File Offset: 0x0006C1B4
	// (set) Token: 0x0600158F RID: 5519 RVA: 0x0006DFC7 File Offset: 0x0006C1C7
	public string NetworkshootSoundEffectName
	{
		get
		{
			return this.shootSoundEffectName;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<string>(value, ref this.shootSoundEffectName, 128UL, null);
		}
	}

	// Token: 0x1700023C RID: 572
	// (get) Token: 0x06001590 RID: 5520 RVA: 0x0006DFE4 File Offset: 0x0006C1E4
	// (set) Token: 0x06001591 RID: 5521 RVA: 0x0006DFF7 File Offset: 0x0006C1F7
	public bool NetworkcanCauseDamage
	{
		get
		{
			return this.canCauseDamage;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<bool>(value, ref this.canCauseDamage, 256UL, null);
		}
	}

	// Token: 0x1700023D RID: 573
	// (get) Token: 0x06001592 RID: 5522 RVA: 0x0006E014 File Offset: 0x0006C214
	// (set) Token: 0x06001593 RID: 5523 RVA: 0x0006E027 File Offset: 0x0006C227
	public float NetworkprojectileSpeed
	{
		get
		{
			return this.projectileSpeed;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<float>(value, ref this.projectileSpeed, 512UL, null);
		}
	}

	// Token: 0x06001594 RID: 5524 RVA: 0x0006E044 File Offset: 0x0006C244
	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteVector2(this.velocity);
			writer.WriteVector3(this.shootPivot);
			writer.WriteGameObject(this.Networkattacker);
			writer.WriteGameObject(this.Networktarget);
			writer.WriteFloat(this.maxRange);
			writer.WriteVector2(this.startPoint);
			writer.WriteBool(this.useAttackerAsStartingPoint);
			writer.WriteString(this.shootSoundEffectName);
			writer.WriteBool(this.canCauseDamage);
			writer.WriteFloat(this.projectileSpeed);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1UL) != 0UL)
		{
			writer.WriteVector2(this.velocity);
		}
		if ((base.syncVarDirtyBits & 2UL) != 0UL)
		{
			writer.WriteVector3(this.shootPivot);
		}
		if ((base.syncVarDirtyBits & 4UL) != 0UL)
		{
			writer.WriteGameObject(this.Networkattacker);
		}
		if ((base.syncVarDirtyBits & 8UL) != 0UL)
		{
			writer.WriteGameObject(this.Networktarget);
		}
		if ((base.syncVarDirtyBits & 16UL) != 0UL)
		{
			writer.WriteFloat(this.maxRange);
		}
		if ((base.syncVarDirtyBits & 32UL) != 0UL)
		{
			writer.WriteVector2(this.startPoint);
		}
		if ((base.syncVarDirtyBits & 64UL) != 0UL)
		{
			writer.WriteBool(this.useAttackerAsStartingPoint);
		}
		if ((base.syncVarDirtyBits & 128UL) != 0UL)
		{
			writer.WriteString(this.shootSoundEffectName);
		}
		if ((base.syncVarDirtyBits & 256UL) != 0UL)
		{
			writer.WriteBool(this.canCauseDamage);
		}
		if ((base.syncVarDirtyBits & 512UL) != 0UL)
		{
			writer.WriteFloat(this.projectileSpeed);
		}
	}

	// Token: 0x06001595 RID: 5525 RVA: 0x0006E23C File Offset: 0x0006C43C
	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			base.GeneratedSyncVarDeserialize<Vector2>(ref this.velocity, null, reader.ReadVector2());
			base.GeneratedSyncVarDeserialize<Vector3>(ref this.shootPivot, null, reader.ReadVector3());
			base.GeneratedSyncVarDeserialize_GameObject(ref this.attacker, null, reader, ref this.___attackerNetId);
			base.GeneratedSyncVarDeserialize_GameObject(ref this.target, null, reader, ref this.___targetNetId);
			base.GeneratedSyncVarDeserialize<float>(ref this.maxRange, null, reader.ReadFloat());
			base.GeneratedSyncVarDeserialize<Vector2>(ref this.startPoint, null, reader.ReadVector2());
			base.GeneratedSyncVarDeserialize<bool>(ref this.useAttackerAsStartingPoint, null, reader.ReadBool());
			base.GeneratedSyncVarDeserialize<string>(ref this.shootSoundEffectName, null, reader.ReadString());
			base.GeneratedSyncVarDeserialize<bool>(ref this.canCauseDamage, null, reader.ReadBool());
			base.GeneratedSyncVarDeserialize<float>(ref this.projectileSpeed, null, reader.ReadFloat());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 1L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<Vector2>(ref this.velocity, null, reader.ReadVector2());
		}
		if ((num & 2L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<Vector3>(ref this.shootPivot, null, reader.ReadVector3());
		}
		if ((num & 4L) != 0L)
		{
			base.GeneratedSyncVarDeserialize_GameObject(ref this.attacker, null, reader, ref this.___attackerNetId);
		}
		if ((num & 8L) != 0L)
		{
			base.GeneratedSyncVarDeserialize_GameObject(ref this.target, null, reader, ref this.___targetNetId);
		}
		if ((num & 16L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<float>(ref this.maxRange, null, reader.ReadFloat());
		}
		if ((num & 32L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<Vector2>(ref this.startPoint, null, reader.ReadVector2());
		}
		if ((num & 64L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<bool>(ref this.useAttackerAsStartingPoint, null, reader.ReadBool());
		}
		if ((num & 128L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<string>(ref this.shootSoundEffectName, null, reader.ReadString());
		}
		if ((num & 256L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<bool>(ref this.canCauseDamage, null, reader.ReadBool());
		}
		if ((num & 512L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<float>(ref this.projectileSpeed, null, reader.ReadFloat());
		}
	}

	// Token: 0x040013A6 RID: 5030
	[SerializeField]
	private float startDistance;

	// Token: 0x040013A7 RID: 5031
	[SerializeField]
	private float angle;

	// Token: 0x040013A8 RID: 5032
	[SerializeField]
	private float framesPerSecond;

	// Token: 0x040013A9 RID: 5033
	[SerializeField]
	private Sprite[] sprites;

	// Token: 0x040013AA RID: 5034
	[SyncVar]
	private Vector2 velocity;

	// Token: 0x040013AB RID: 5035
	[SyncVar]
	private Vector3 shootPivot;

	// Token: 0x040013AC RID: 5036
	[SyncVar]
	private GameObject attacker;

	// Token: 0x040013AD RID: 5037
	[SyncVar]
	private GameObject target;

	// Token: 0x040013AE RID: 5038
	[SyncVar]
	private float maxRange;

	// Token: 0x040013AF RID: 5039
	[SyncVar]
	private Vector2 startPoint;

	// Token: 0x040013B0 RID: 5040
	[SyncVar]
	private bool useAttackerAsStartingPoint;

	// Token: 0x040013B1 RID: 5041
	[SyncVar]
	private string shootSoundEffectName;

	// Token: 0x040013B2 RID: 5042
	[SyncVar]
	private bool canCauseDamage;

	// Token: 0x040013B3 RID: 5043
	[SyncVar]
	private float projectileSpeed;

	// Token: 0x040013B4 RID: 5044
	private bool triggered;

	// Token: 0x040013B5 RID: 5045
	private float skillConditionChance;

	// Token: 0x040013B6 RID: 5046
	private Item ammo;

	// Token: 0x040013B7 RID: 5047
	private Skill skill;

	// Token: 0x040013B8 RID: 5048
	private Condition skillCondition;

	// Token: 0x040013B9 RID: 5049
	private Rigidbody2D projectileBody2D;

	// Token: 0x040013BA RID: 5050
	private SpriteRenderer spriteRenderer;

	// Token: 0x040013BB RID: 5051
	private EffectModule effectModule;

	// Token: 0x040013BC RID: 5052
	protected uint ___attackerNetId;

	// Token: 0x040013BD RID: 5053
	protected uint ___targetNetId;
}
