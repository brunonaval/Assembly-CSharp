using System;
using System.Collections;
using System.Runtime.InteropServices;
using Mirror;
using UnityEngine;

// Token: 0x0200029F RID: 671
public class ActionTileModule : NetworkBehaviour
{
	// Token: 0x06000A7C RID: 2684 RVA: 0x0002FC51 File Offset: 0x0002DE51
	private void Awake()
	{
		this.NetworkTeleportPosition = Vector2.zero;
		if (NetworkServer.active)
		{
			UnityEngine.Object.Destroy(this.lightObject);
		}
	}

	// Token: 0x06000A7D RID: 2685 RVA: 0x0002FC70 File Offset: 0x0002DE70
	public override void OnStartClient()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("GameEnvironment");
		this.gameEnvironmentModule = gameObject.GetComponent<GameEnvironmentModule>();
		if (this.Type == "light")
		{
			this.ConfigureLight(this.lightRange, this.lightIntensity, this.lightColorId, this.turnOn);
			base.gameObject.layer = LayerMask.NameToLayer("Default");
			base.StartCoroutine(this.ConfigureTurnOn());
		}
	}

	// Token: 0x06000A7E RID: 2686 RVA: 0x0002FCE8 File Offset: 0x0002DEE8
	[Client]
	private IEnumerator ConfigureTurnOn()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Collections.IEnumerator ActionTileModule::ConfigureTurnOn()' called when client was not active");
			return null;
		}
		ActionTileModule.<ConfigureTurnOn>d__12 <ConfigureTurnOn>d__ = new ActionTileModule.<ConfigureTurnOn>d__12(0);
		<ConfigureTurnOn>d__.<>4__this = this;
		return <ConfigureTurnOn>d__;
	}

	// Token: 0x06000A7F RID: 2687 RVA: 0x0002FD23 File Offset: 0x0002DF23
	[Server]
	public void SetTeleportPosition(Vector2 teleportPosition)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ActionTileModule::SetTeleportPosition(UnityEngine.Vector2)' called when server was not active");
			return;
		}
		this.NetworkTeleportPosition = teleportPosition;
	}

	// Token: 0x06000A80 RID: 2688 RVA: 0x0002FD41 File Offset: 0x0002DF41
	[Server]
	public void SetActionTag(string actionTag)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ActionTileModule::SetActionTag(System.String)' called when server was not active");
			return;
		}
		this.NetworkActionTag = actionTag;
	}

	// Token: 0x06000A81 RID: 2689 RVA: 0x0002FD5F File Offset: 0x0002DF5F
	[Server]
	public void SetType(string type)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ActionTileModule::SetType(System.String)' called when server was not active");
			return;
		}
		this.NetworkType = type;
	}

	// Token: 0x06000A82 RID: 2690 RVA: 0x0002FD7D File Offset: 0x0002DF7D
	[Server]
	public void SetLight(float lightRange, float lightIntensity, int lightColorId, float turnOn)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ActionTileModule::SetLight(System.Single,System.Single,System.Int32,System.Single)' called when server was not active");
			return;
		}
		this.NetworklightRange = lightRange;
		this.NetworklightIntensity = lightIntensity;
		this.NetworklightColorId = lightColorId;
		this.NetworkturnOn = turnOn;
	}

	// Token: 0x06000A83 RID: 2691 RVA: 0x0002FDB4 File Offset: 0x0002DFB4
	[Client]
	private void ConfigureLight(float lightRange, float lightIntensity, int lightColorId, float turnOn)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void ActionTileModule::ConfigureLight(System.Single,System.Single,System.Int32,System.Single)' called when client was not active");
			return;
		}
		Transform transform = base.transform.Find("Light");
		if (transform != null & lightRange != 0f & lightIntensity != 0f)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
			Light light = transform.GetComponent<Light>();
			if (light == null)
			{
				light = transform.gameObject.AddComponent<Light>();
			}
			light.range = lightRange;
			light.intensity = lightIntensity;
			if (QualitySettings.GetQualityLevel() < 3)
			{
				light.renderMode = LightRenderMode.ForceVertex;
			}
			else
			{
				light.renderMode = LightRenderMode.ForcePixel;
			}
			light.type = LightType.Point;
			if (GlobalSettings.Colors.ContainsKey(lightColorId))
			{
				light.color = GlobalSettings.Colors[lightColorId];
			}
			if (turnOn != 0f && RenderSettings.ambientIntensity >= turnOn)
			{
				transform.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06000A85 RID: 2693 RVA: 0x0002FEC9 File Offset: 0x0002E0C9
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x1700010D RID: 269
	// (get) Token: 0x06000A86 RID: 2694 RVA: 0x0002FECC File Offset: 0x0002E0CC
	// (set) Token: 0x06000A87 RID: 2695 RVA: 0x0002FEDF File Offset: 0x0002E0DF
	public float NetworklightRange
	{
		get
		{
			return this.lightRange;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<float>(value, ref this.lightRange, 1UL, null);
		}
	}

	// Token: 0x1700010E RID: 270
	// (get) Token: 0x06000A88 RID: 2696 RVA: 0x0002FEFC File Offset: 0x0002E0FC
	// (set) Token: 0x06000A89 RID: 2697 RVA: 0x0002FF0F File Offset: 0x0002E10F
	public float NetworklightIntensity
	{
		get
		{
			return this.lightIntensity;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<float>(value, ref this.lightIntensity, 2UL, null);
		}
	}

	// Token: 0x1700010F RID: 271
	// (get) Token: 0x06000A8A RID: 2698 RVA: 0x0002FF2C File Offset: 0x0002E12C
	// (set) Token: 0x06000A8B RID: 2699 RVA: 0x0002FF3F File Offset: 0x0002E13F
	public float NetworkturnOn
	{
		get
		{
			return this.turnOn;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<float>(value, ref this.turnOn, 4UL, null);
		}
	}

	// Token: 0x17000110 RID: 272
	// (get) Token: 0x06000A8C RID: 2700 RVA: 0x0002FF5C File Offset: 0x0002E15C
	// (set) Token: 0x06000A8D RID: 2701 RVA: 0x0002FF6F File Offset: 0x0002E16F
	public int NetworklightColorId
	{
		get
		{
			return this.lightColorId;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.lightColorId, 8UL, null);
		}
	}

	// Token: 0x17000111 RID: 273
	// (get) Token: 0x06000A8E RID: 2702 RVA: 0x0002FF8C File Offset: 0x0002E18C
	// (set) Token: 0x06000A8F RID: 2703 RVA: 0x0002FF9F File Offset: 0x0002E19F
	public string NetworkType
	{
		get
		{
			return this.Type;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<string>(value, ref this.Type, 16UL, null);
		}
	}

	// Token: 0x17000112 RID: 274
	// (get) Token: 0x06000A90 RID: 2704 RVA: 0x0002FFBC File Offset: 0x0002E1BC
	// (set) Token: 0x06000A91 RID: 2705 RVA: 0x0002FFCF File Offset: 0x0002E1CF
	public Vector2 NetworkTeleportPosition
	{
		get
		{
			return this.TeleportPosition;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<Vector2>(value, ref this.TeleportPosition, 32UL, null);
		}
	}

	// Token: 0x17000113 RID: 275
	// (get) Token: 0x06000A92 RID: 2706 RVA: 0x0002FFEC File Offset: 0x0002E1EC
	// (set) Token: 0x06000A93 RID: 2707 RVA: 0x0002FFFF File Offset: 0x0002E1FF
	public string NetworkActionTag
	{
		get
		{
			return this.ActionTag;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<string>(value, ref this.ActionTag, 64UL, null);
		}
	}

	// Token: 0x06000A94 RID: 2708 RVA: 0x0003001C File Offset: 0x0002E21C
	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteFloat(this.lightRange);
			writer.WriteFloat(this.lightIntensity);
			writer.WriteFloat(this.turnOn);
			writer.WriteInt(this.lightColorId);
			writer.WriteString(this.Type);
			writer.WriteVector2(this.TeleportPosition);
			writer.WriteString(this.ActionTag);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1UL) != 0UL)
		{
			writer.WriteFloat(this.lightRange);
		}
		if ((base.syncVarDirtyBits & 2UL) != 0UL)
		{
			writer.WriteFloat(this.lightIntensity);
		}
		if ((base.syncVarDirtyBits & 4UL) != 0UL)
		{
			writer.WriteFloat(this.turnOn);
		}
		if ((base.syncVarDirtyBits & 8UL) != 0UL)
		{
			writer.WriteInt(this.lightColorId);
		}
		if ((base.syncVarDirtyBits & 16UL) != 0UL)
		{
			writer.WriteString(this.Type);
		}
		if ((base.syncVarDirtyBits & 32UL) != 0UL)
		{
			writer.WriteVector2(this.TeleportPosition);
		}
		if ((base.syncVarDirtyBits & 64UL) != 0UL)
		{
			writer.WriteString(this.ActionTag);
		}
	}

	// Token: 0x06000A95 RID: 2709 RVA: 0x00030188 File Offset: 0x0002E388
	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			base.GeneratedSyncVarDeserialize<float>(ref this.lightRange, null, reader.ReadFloat());
			base.GeneratedSyncVarDeserialize<float>(ref this.lightIntensity, null, reader.ReadFloat());
			base.GeneratedSyncVarDeserialize<float>(ref this.turnOn, null, reader.ReadFloat());
			base.GeneratedSyncVarDeserialize<int>(ref this.lightColorId, null, reader.ReadInt());
			base.GeneratedSyncVarDeserialize<string>(ref this.Type, null, reader.ReadString());
			base.GeneratedSyncVarDeserialize<Vector2>(ref this.TeleportPosition, null, reader.ReadVector2());
			base.GeneratedSyncVarDeserialize<string>(ref this.ActionTag, null, reader.ReadString());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 1L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<float>(ref this.lightRange, null, reader.ReadFloat());
		}
		if ((num & 2L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<float>(ref this.lightIntensity, null, reader.ReadFloat());
		}
		if ((num & 4L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<float>(ref this.turnOn, null, reader.ReadFloat());
		}
		if ((num & 8L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.lightColorId, null, reader.ReadInt());
		}
		if ((num & 16L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<string>(ref this.Type, null, reader.ReadString());
		}
		if ((num & 32L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<Vector2>(ref this.TeleportPosition, null, reader.ReadVector2());
		}
		if ((num & 64L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<string>(ref this.ActionTag, null, reader.ReadString());
		}
	}

	// Token: 0x04000C02 RID: 3074
	[SerializeField]
	private GameObject lightObject;

	// Token: 0x04000C03 RID: 3075
	private readonly float turnOnInterval = 5f;

	// Token: 0x04000C04 RID: 3076
	private GameEnvironmentModule gameEnvironmentModule;

	// Token: 0x04000C05 RID: 3077
	[SyncVar]
	private float lightRange;

	// Token: 0x04000C06 RID: 3078
	[SyncVar]
	private float lightIntensity;

	// Token: 0x04000C07 RID: 3079
	[SyncVar]
	private float turnOn;

	// Token: 0x04000C08 RID: 3080
	[SyncVar]
	private int lightColorId;

	// Token: 0x04000C09 RID: 3081
	[SyncVar]
	public string Type;

	// Token: 0x04000C0A RID: 3082
	[SyncVar]
	public Vector2 TeleportPosition;

	// Token: 0x04000C0B RID: 3083
	[SyncVar]
	public string ActionTag;
}
