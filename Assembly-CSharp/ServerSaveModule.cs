using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Mirror;
using UnityEngine;

// Token: 0x0200040E RID: 1038
public class ServerSaveModule : MonoBehaviour
{
	// Token: 0x1700024F RID: 591
	// (get) Token: 0x06001671 RID: 5745 RVA: 0x00072B33 File Offset: 0x00070D33
	// (set) Token: 0x06001672 RID: 5746 RVA: 0x00072B3A File Offset: 0x00070D3A
	public static bool Restarting { get; set; }

	// Token: 0x17000250 RID: 592
	// (get) Token: 0x06001673 RID: 5747 RVA: 0x00072B42 File Offset: 0x00070D42
	// (set) Token: 0x06001674 RID: 5748 RVA: 0x00072B49 File Offset: 0x00070D49
	public static bool ForceRestarting { get; set; }

	// Token: 0x06001675 RID: 5749 RVA: 0x00072B51 File Offset: 0x00070D51
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06001676 RID: 5750 RVA: 0x00072B5E File Offset: 0x00070D5E
	private void Start()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		base.InvokeRepeating("ServerSaveTimer", 60f, 60f);
	}

	// Token: 0x06001677 RID: 5751 RVA: 0x00072B80 File Offset: 0x00070D80
	private void ServerSaveTimer()
	{
		if (Time.timeSinceLevelLoad < 3600f & !ServerSaveModule.ForceRestarting)
		{
			return;
		}
		if (!ServerSaveModule.Restarting & (DateTime.UtcNow.Hour == 7 | ServerSaveModule.ForceRestarting))
		{
			base.StartCoroutine(this.RestartServer());
			return;
		}
	}

	// Token: 0x06001678 RID: 5752 RVA: 0x00072BD4 File Offset: 0x00070DD4
	private IEnumerator RestartServer()
	{
		ServerSaveModule.Restarting = true;
		this.gameEnvironmentModule.BroadcastChatMessage(null, "first_server_saving_message");
		yield return new WaitForSecondsRealtime(120f);
		this.gameEnvironmentModule.BroadcastChatMessage(null, "second_server_saving_message");
		yield return new WaitForSecondsRealtime(120f);
		this.gameEnvironmentModule.BroadcastChatMessage(null, "third_server_saving_message");
		yield return new WaitForSecondsRealtime(60f);
		GameObject[] connectedPlayers = (from cp in NetworkManagerModule.ConnectedPlayers
		select cp.Value).ToArray<GameObject>();
		int i = 0;
		while (i < connectedPlayers.Length)
		{
			try
			{
				if (connectedPlayers[i] == null)
				{
					goto IL_1B5;
				}
				if (!connectedPlayers[i].activeInHierarchy)
				{
					goto IL_1B5;
				}
				NetworkIdentity component = connectedPlayers[i].GetComponent<NetworkIdentity>();
				if (component == null)
				{
					goto IL_1B5;
				}
				if (component.connectionToClient == null)
				{
					goto IL_1B5;
				}
				component.connectionToClient.Disconnect();
			}
			catch (Exception arg)
			{
				UnityEngine.Debug.Log(string.Format("Failed disconnecting player: {0}", arg));
			}
			goto IL_18B;
			IL_1B5:
			int num = i;
			i = num + 1;
			continue;
			IL_18B:
			if (i % 3 == 0)
			{
				yield return new WaitForSecondsRealtime(3f);
				goto IL_1B5;
			}
			goto IL_1B5;
		}
		yield return new WaitForSecondsRealtime(30f);
		NetworkServer.Shutdown();
		yield return new WaitForSecondsRealtime(10f);
		Process.GetCurrentProcess().Kill();
		yield break;
	}

	// Token: 0x0400143D RID: 5181
	[SerializeField]
	private GameEnvironmentModule gameEnvironmentModule;
}
