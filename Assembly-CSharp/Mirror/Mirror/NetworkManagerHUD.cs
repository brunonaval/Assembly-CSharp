using System;
using UnityEngine;

namespace Mirror
{
	// Token: 0x0200004F RID: 79
	[DisallowMultipleComponent]
	[AddComponentMenu("Network/Network Manager HUD")]
	[RequireComponent(typeof(NetworkManager))]
	[HelpURL("https://mirror-networking.gitbook.io/docs/components/network-manager-hud")]
	public class NetworkManagerHUD : MonoBehaviour
	{
		// Token: 0x060001EE RID: 494 RVA: 0x00008A3F File Offset: 0x00006C3F
		private void Awake()
		{
			this.manager = base.GetComponent<NetworkManager>();
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00008A50 File Offset: 0x00006C50
		private void OnGUI()
		{
			GUILayout.BeginArea(new Rect((float)(10 + this.offsetX), (float)(40 + this.offsetY), 250f, 9999f));
			if (!NetworkClient.isConnected && !NetworkServer.active)
			{
				this.StartButtons();
			}
			else
			{
				this.StatusLabels();
			}
			if (NetworkClient.isConnected && !NetworkClient.ready && GUILayout.Button("Client Ready", Array.Empty<GUILayoutOption>()))
			{
				NetworkClient.Ready();
				if (NetworkClient.localPlayer == null)
				{
					NetworkClient.AddPlayer();
				}
			}
			this.StopButtons();
			GUILayout.EndArea();
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00008AE4 File Offset: 0x00006CE4
		private void StartButtons()
		{
			if (!NetworkClient.active)
			{
				if (Application.platform != RuntimePlatform.WebGLPlayer && GUILayout.Button("Host (Server + Client)", Array.Empty<GUILayoutOption>()))
				{
					this.manager.StartHost();
				}
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Client", Array.Empty<GUILayoutOption>()))
				{
					this.manager.StartClient();
				}
				this.manager.networkAddress = GUILayout.TextField(this.manager.networkAddress, Array.Empty<GUILayoutOption>());
				GUILayout.EndHorizontal();
				if (Application.platform == RuntimePlatform.WebGLPlayer)
				{
					GUILayout.Box("(  WebGL cannot be server  )", Array.Empty<GUILayoutOption>());
					return;
				}
				if (GUILayout.Button("Server Only", Array.Empty<GUILayoutOption>()))
				{
					this.manager.StartServer();
					return;
				}
			}
			else
			{
				GUILayout.Label("Connecting to " + this.manager.networkAddress + "..", Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Cancel Connection Attempt", Array.Empty<GUILayoutOption>()))
				{
					this.manager.StopClient();
				}
			}
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00008BE4 File Offset: 0x00006DE4
		private void StatusLabels()
		{
			if (NetworkServer.active && NetworkClient.active)
			{
				GUILayout.Label(string.Format("<b>Host</b>: running via {0}", Transport.active), Array.Empty<GUILayoutOption>());
				return;
			}
			if (NetworkServer.active)
			{
				GUILayout.Label(string.Format("<b>Server</b>: running via {0}", Transport.active), Array.Empty<GUILayoutOption>());
				return;
			}
			if (NetworkClient.isConnected)
			{
				GUILayout.Label(string.Format("<b>Client</b>: connected to {0} via {1}", this.manager.networkAddress, Transport.active), Array.Empty<GUILayoutOption>());
			}
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00008C68 File Offset: 0x00006E68
		private void StopButtons()
		{
			if (NetworkServer.active && NetworkClient.isConnected)
			{
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				if (GUILayout.Button("Stop Host", Array.Empty<GUILayoutOption>()))
				{
					this.manager.StopHost();
				}
				if (GUILayout.Button("Stop Client", Array.Empty<GUILayoutOption>()))
				{
					this.manager.StopClient();
				}
				GUILayout.EndHorizontal();
				return;
			}
			if (NetworkClient.isConnected)
			{
				if (GUILayout.Button("Stop Client", Array.Empty<GUILayoutOption>()))
				{
					this.manager.StopClient();
					return;
				}
			}
			else if (NetworkServer.active && GUILayout.Button("Stop Server", Array.Empty<GUILayoutOption>()))
			{
				this.manager.StopServer();
			}
		}

		// Token: 0x040000FF RID: 255
		private NetworkManager manager;

		// Token: 0x04000100 RID: 256
		public int offsetX;

		// Token: 0x04000101 RID: 257
		public int offsetY;
	}
}
