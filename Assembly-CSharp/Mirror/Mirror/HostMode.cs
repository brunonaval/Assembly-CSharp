using System;

namespace Mirror
{
	// Token: 0x02000014 RID: 20
	public static class HostMode
	{
		// Token: 0x0600001D RID: 29 RVA: 0x000024BC File Offset: 0x000006BC
		internal static void SetupConnections()
		{
			LocalConnectionToClient localConnection;
			LocalConnectionToServer connection;
			Utils.CreateLocalConnections(out localConnection, out connection);
			NetworkClient.connection = connection;
			NetworkServer.SetLocalConnection(localConnection);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000024DE File Offset: 0x000006DE
		public static void InvokeOnConnected()
		{
			NetworkServer.OnConnected(NetworkServer.localConnection);
			((LocalConnectionToServer)NetworkClient.connection).QueueConnectedEvent();
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000024F9 File Offset: 0x000006F9
		[Obsolete("ActivateHostScene did nothing, since identities all had .isClient set in NetworkServer.SpawnObjects.")]
		public static void ActivateHostScene()
		{
		}
	}
}
