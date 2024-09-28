using System;
using UnityEngine;
using UnityEngine.Events;

namespace Mirror
{
	// Token: 0x02000030 RID: 48
	[HelpURL("https://mirror-networking.gitbook.io/docs/components/network-authenticators")]
	public abstract class NetworkAuthenticator : MonoBehaviour
	{
		// Token: 0x06000057 RID: 87 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual void OnStartServer()
		{
		}

		// Token: 0x06000058 RID: 88 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual void OnStopServer()
		{
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual void OnServerAuthenticate(NetworkConnectionToClient conn)
		{
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002FA8 File Offset: 0x000011A8
		protected void ServerAccept(NetworkConnectionToClient conn)
		{
			this.OnServerAuthenticated.Invoke(conn);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002FB6 File Offset: 0x000011B6
		protected void ServerReject(NetworkConnectionToClient conn)
		{
			conn.Disconnect();
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual void OnStartClient()
		{
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual void OnStopClient()
		{
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual void OnClientAuthenticate()
		{
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002FBE File Offset: 0x000011BE
		protected void ClientAccept()
		{
			this.OnClientAuthenticated.Invoke();
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002FCB File Offset: 0x000011CB
		protected void ClientReject()
		{
			NetworkClient.connection.isAuthenticated = false;
			NetworkClient.connection.Disconnect();
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000024F9 File Offset: 0x000006F9
		private void Reset()
		{
		}

		// Token: 0x0400004B RID: 75
		[Header("Event Listeners (optional)")]
		[Tooltip("Mirror has an internal subscriber to this event. You can add your own here.")]
		public UnityEventNetworkConnection OnServerAuthenticated = new UnityEventNetworkConnection();

		// Token: 0x0400004C RID: 76
		[Tooltip("Mirror has an internal subscriber to this event. You can add your own here.")]
		public UnityEvent OnClientAuthenticated = new UnityEvent();
	}
}
