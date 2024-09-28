using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Mirror;
using UnityEngine;
using UnityEngine.Rendering.Universal;

// Token: 0x0200032A RID: 810
public class GameEnvironmentModule : NetworkBehaviour
{
	// Token: 0x06000FB1 RID: 4017 RVA: 0x000497E0 File Offset: 0x000479E0
	private void Awake()
	{
		Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us");
		this.globalLight = base.GetComponent<Light2D>();
		this.globalLight.color = new Color(1f, 1f, 1f, 1f);
		this.lastAmbientColor = new Color(1f, 1f, 1f, 1f);
		if (NetworkClient.active)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
			this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
			base.InvokeRepeating("UpdateAmbientTimeAndLightTimer", 0f, 1f);
		}
		if (NetworkServer.active)
		{
			base.InvokeRepeating("UpdateGameTimeTimer", 0f, 30f);
		}
	}

	// Token: 0x06000FB2 RID: 4018 RVA: 0x000498A0 File Offset: 0x00047AA0
	private void Start()
	{
		if (NetworkServer.active)
		{
			UnityEngine.Object.Destroy(GameObject.Find("EventSystem"));
			this.NetworkGameHours = DateTime.Now.Hour;
		}
	}

	// Token: 0x06000FB3 RID: 4019 RVA: 0x000498D8 File Offset: 0x00047AD8
	[Server]
	public void BroadcastChatMessage(string sender, string content)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void GameEnvironmentModule::BroadcastChatMessage(System.String,System.String)' called when server was not active");
			return;
		}
		ChatMessage message = new ChatMessage("chat_tab_default", null, sender, (double)DateTime.Now.Ticks, content, true, true, false, false, null, false);
		for (int i = 0; i < this.Contacts.Count; i++)
		{
			if (this.Contacts[i].Player != null)
			{
				this.Contacts[i].Player.GetComponent<ChatModule>().TargetSendChatMessage(message);
			}
		}
	}

	// Token: 0x06000FB4 RID: 4020 RVA: 0x00049968 File Offset: 0x00047B68
	[Server]
	public void BroadcastScreenMessage(string text, int colorId, float duration, params string[] args)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void GameEnvironmentModule::BroadcastScreenMessage(System.String,System.Int32,System.Single,System.String[])' called when server was not active");
			return;
		}
		for (int i = 0; i < this.Contacts.Count; i++)
		{
			if (this.Contacts[i].Player != null)
			{
				this.Contacts[i].Player.GetComponent<EffectModule>().ShowScreenMessage(text, colorId, duration, args);
			}
		}
	}

	// Token: 0x06000FB5 RID: 4021 RVA: 0x000499DC File Offset: 0x00047BDC
	[Server]
	public void AddContact(GameObject gameObject, string name, int connectionId)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void GameEnvironmentModule::AddContact(UnityEngine.GameObject,System.String,System.Int32)' called when server was not active");
			return;
		}
		ChatContact contact = new ChatContact(gameObject, name, connectionId);
		if (!this.Contacts.Any((ChatContact a) => a.Name.Equals(contact.Name, StringComparison.InvariantCultureIgnoreCase) | a.ConnectionId == contact.ConnectionId))
		{
			this.Contacts.Add(contact);
		}
	}

	// Token: 0x06000FB6 RID: 4022 RVA: 0x00049A3C File Offset: 0x00047C3C
	[Server]
	public void UpdateContact(int connectionId, GameObject playerObject, string name)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void GameEnvironmentModule::UpdateContact(System.Int32,UnityEngine.GameObject,System.String)' called when server was not active");
			return;
		}
		for (int i = 0; i < this.Contacts.Count; i++)
		{
			if (this.Contacts[i].ConnectionId == connectionId)
			{
				ChatContact value = this.Contacts[i];
				value.Player = playerObject;
				value.Name = name;
				this.Contacts[i] = value;
				return;
			}
		}
	}

	// Token: 0x06000FB7 RID: 4023 RVA: 0x00049AB4 File Offset: 0x00047CB4
	[Server]
	public void RemoveContact(int connectionId)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void GameEnvironmentModule::RemoveContact(System.Int32)' called when server was not active");
			return;
		}
		for (int i = 0; i < this.Contacts.Count; i++)
		{
			if (this.Contacts[i].ConnectionId == connectionId)
			{
				this.Contacts.RemoveAt(i);
			}
		}
	}

	// Token: 0x06000FB8 RID: 4024 RVA: 0x00049B0C File Offset: 0x00047D0C
	[Server]
	public ChatContact FindContact(string name)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'ChatContact GameEnvironmentModule::FindContact(System.String)' called when server was not active");
			return default(ChatContact);
		}
		for (int i = 0; i < this.Contacts.Count; i++)
		{
			if (this.Contacts[i].Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) & this.Contacts[i].IsDefined)
			{
				return this.Contacts[i];
			}
		}
		return default(ChatContact);
	}

	// Token: 0x06000FB9 RID: 4025 RVA: 0x00049B95 File Offset: 0x00047D95
	[ClientCallback]
	private void UpdateAmbientTimeAndLightTimer()
	{
		if (!NetworkClient.active)
		{
			return;
		}
		if (!NetworkClient.active)
		{
			return;
		}
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		this.ambientUpdateTime = Time.time;
		base.StartCoroutine(this.ChangeAmbientLight());
	}

	// Token: 0x06000FBA RID: 4026 RVA: 0x00049BD0 File Offset: 0x00047DD0
	[Server]
	private void UpdateGameTimeTimer()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void GameEnvironmentModule::UpdateGameTimeTimer()' called when server was not active");
			return;
		}
		float num = Time.time - this.gameTime;
		this.gameSeconds += (double)(num * this.ambientTimeMultiplier);
		if (this.gameSeconds >= 60.0)
		{
			this.NetworkGameMinutes = this.GameMinutes + 1;
			this.gameSeconds = 0.0;
		}
		if (this.GameMinutes >= 60)
		{
			this.NetworkGameHours = this.GameHours + 1;
			this.NetworkGameMinutes = 0;
		}
		if (this.GameHours >= 24)
		{
			this.NetworkGameHours = 0;
		}
		if (this.GameHours >= 20 | this.GameHours <= 5)
		{
			this.NetworkIsNight = true;
		}
		else
		{
			this.NetworkIsNight = false;
		}
		this.gameTime = Time.time;
	}

	// Token: 0x06000FBB RID: 4027 RVA: 0x00049CAC File Offset: 0x00047EAC
	public float GetAmbientIntensity()
	{
		float num = 0f;
		if (this.GameHours >= 0 & this.GameHours <= 3)
		{
			num = 0.3f;
		}
		else if (this.GameHours == 4)
		{
			num = 0.4f;
		}
		else if (this.GameHours == 5)
		{
			num = 0.5f;
		}
		else if (this.GameHours == 6)
		{
			num = 0.6f;
		}
		else if (this.GameHours == 7)
		{
			num = 0.7f;
		}
		else if (this.GameHours == 8)
		{
			num = 0.8f;
		}
		else if (this.GameHours == 9)
		{
			num = 0.9f;
		}
		else if (this.GameHours >= 10 & this.GameHours < 19)
		{
			num = 1f;
		}
		else if (this.GameHours == 19)
		{
			num = 0.9f;
		}
		else if (this.GameHours == 20)
		{
			num = 0.6f;
		}
		else if (this.GameHours >= 21 & this.GameHours <= 23)
		{
			num = 0.3f;
		}
		if (this.uiSystemModule.AreaModule != null)
		{
			num = Mathf.Min(this.uiSystemModule.AreaModule.CurrentAmbientLight, num);
		}
		return num;
	}

	// Token: 0x06000FBC RID: 4028 RVA: 0x00049DEC File Offset: 0x00047FEC
	public void SetIgnoreAmbientLight(bool ignore)
	{
		if (SettingsManager.Instance.DisableLightSystem)
		{
			return;
		}
		this.ignoreAmbientLight = ignore;
		if (ignore)
		{
			this.lastAmbientColor = this.globalLight.color;
			this.globalLight.color = new Color(1f, 1f, 1f, 1f);
			return;
		}
		this.globalLight.color = this.lastAmbientColor;
	}

	// Token: 0x06000FBD RID: 4029 RVA: 0x00049E58 File Offset: 0x00048058
	[Client]
	public void SetAmbientLightToMax()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void GameEnvironmentModule::SetAmbientLightToMax()' called when client was not active");
			return;
		}
		this.ignoreAmbientLight = false;
		this.lastAmbientColor = this.globalLight.color;
		this.globalLight.color = new Color(1f, 1f, 1f, 1f);
	}

	// Token: 0x06000FBE RID: 4030 RVA: 0x00049EB8 File Offset: 0x000480B8
	[Client]
	private IEnumerator ChangeAmbientLight()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Collections.IEnumerator GameEnvironmentModule::ChangeAmbientLight()' called when client was not active");
			return null;
		}
		GameEnvironmentModule.<ChangeAmbientLight>d__31 <ChangeAmbientLight>d__ = new GameEnvironmentModule.<ChangeAmbientLight>d__31(0);
		<ChangeAmbientLight>d__.<>4__this = this;
		return <ChangeAmbientLight>d__;
	}

	// Token: 0x06000FC1 RID: 4033 RVA: 0x0002FEC9 File Offset: 0x0002E0C9
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x170001C6 RID: 454
	// (get) Token: 0x06000FC2 RID: 4034 RVA: 0x00049F34 File Offset: 0x00048134
	// (set) Token: 0x06000FC3 RID: 4035 RVA: 0x00049F47 File Offset: 0x00048147
	public int NetworkGameMinutes
	{
		get
		{
			return this.GameMinutes;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.GameMinutes, 1UL, null);
		}
	}

	// Token: 0x170001C7 RID: 455
	// (get) Token: 0x06000FC4 RID: 4036 RVA: 0x00049F64 File Offset: 0x00048164
	// (set) Token: 0x06000FC5 RID: 4037 RVA: 0x00049F77 File Offset: 0x00048177
	public int NetworkGameHours
	{
		get
		{
			return this.GameHours;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<int>(value, ref this.GameHours, 2UL, null);
		}
	}

	// Token: 0x170001C8 RID: 456
	// (get) Token: 0x06000FC6 RID: 4038 RVA: 0x00049F94 File Offset: 0x00048194
	// (set) Token: 0x06000FC7 RID: 4039 RVA: 0x00049FA7 File Offset: 0x000481A7
	public bool NetworkIsNight
	{
		get
		{
			return this.IsNight;
		}
		[param: In]
		set
		{
			base.GeneratedSyncVarSetter<bool>(value, ref this.IsNight, 4UL, null);
		}
	}

	// Token: 0x06000FC8 RID: 4040 RVA: 0x00049FC4 File Offset: 0x000481C4
	public override void SerializeSyncVars(NetworkWriter writer, bool forceAll)
	{
		base.SerializeSyncVars(writer, forceAll);
		if (forceAll)
		{
			writer.WriteInt(this.GameMinutes);
			writer.WriteInt(this.GameHours);
			writer.WriteBool(this.IsNight);
			return;
		}
		writer.WriteULong(base.syncVarDirtyBits);
		if ((base.syncVarDirtyBits & 1UL) != 0UL)
		{
			writer.WriteInt(this.GameMinutes);
		}
		if ((base.syncVarDirtyBits & 2UL) != 0UL)
		{
			writer.WriteInt(this.GameHours);
		}
		if ((base.syncVarDirtyBits & 4UL) != 0UL)
		{
			writer.WriteBool(this.IsNight);
		}
	}

	// Token: 0x06000FC9 RID: 4041 RVA: 0x0004A078 File Offset: 0x00048278
	public override void DeserializeSyncVars(NetworkReader reader, bool initialState)
	{
		base.DeserializeSyncVars(reader, initialState);
		if (initialState)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.GameMinutes, null, reader.ReadInt());
			base.GeneratedSyncVarDeserialize<int>(ref this.GameHours, null, reader.ReadInt());
			base.GeneratedSyncVarDeserialize<bool>(ref this.IsNight, null, reader.ReadBool());
			return;
		}
		long num = (long)reader.ReadULong();
		if ((num & 1L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.GameMinutes, null, reader.ReadInt());
		}
		if ((num & 2L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<int>(ref this.GameHours, null, reader.ReadInt());
		}
		if ((num & 4L) != 0L)
		{
			base.GeneratedSyncVarDeserialize<bool>(ref this.IsNight, null, reader.ReadBool());
		}
	}

	// Token: 0x04000F3D RID: 3901
	[SyncVar]
	public int GameMinutes;

	// Token: 0x04000F3E RID: 3902
	[SyncVar]
	public int GameHours;

	// Token: 0x04000F3F RID: 3903
	[SyncVar]
	public bool IsNight;

	// Token: 0x04000F40 RID: 3904
	[SerializeField]
	private WorldCreatorModule worldCreatorModule;

	// Token: 0x04000F41 RID: 3905
	private float gameTime;

	// Token: 0x04000F42 RID: 3906
	private bool updatingLight;

	// Token: 0x04000F43 RID: 3907
	private double gameSeconds;

	// Token: 0x04000F44 RID: 3908
	private Light2D globalLight;

	// Token: 0x04000F45 RID: 3909
	private float ambientUpdateTime;

	// Token: 0x04000F46 RID: 3910
	private float lastGarbageCollectorTime;

	// Token: 0x04000F47 RID: 3911
	private UISystemModule uiSystemModule;

	// Token: 0x04000F48 RID: 3912
	private float ambientTimeMultiplier = 2.5f;

	// Token: 0x04000F49 RID: 3913
	public float AmbientLight = 1f;

	// Token: 0x04000F4A RID: 3914
	public bool ignoreAmbientLight;

	// Token: 0x04000F4B RID: 3915
	public Color lastAmbientColor;

	// Token: 0x04000F4C RID: 3916
	public List<ChatContact> Contacts = new List<ChatContact>();

	// Token: 0x04000F4D RID: 3917
	public static List<NpcSpawnPoint> NpcSpawnPoints = new List<NpcSpawnPoint>();

	// Token: 0x04000F4E RID: 3918
	public static List<MonsterSpawnPoint> MonsterSpawnPoints = new List<MonsterSpawnPoint>();
}
