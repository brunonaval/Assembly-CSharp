using System;
using UnityEngine;

namespace Mirror
{
	// Token: 0x0200008D RID: 141
	public abstract class Transport : MonoBehaviour
	{
		// Token: 0x0600040C RID: 1036
		public abstract bool Available();

		// Token: 0x0600040D RID: 1037
		public abstract bool ClientConnected();

		// Token: 0x0600040E RID: 1038
		public abstract void ClientConnect(string address);

		// Token: 0x0600040F RID: 1039 RVA: 0x0000EBFD File Offset: 0x0000CDFD
		public virtual void ClientConnect(Uri uri)
		{
			this.ClientConnect(uri.Host);
		}

		// Token: 0x06000410 RID: 1040
		public abstract void ClientSend(ArraySegment<byte> segment, int channelId = 0);

		// Token: 0x06000411 RID: 1041
		public abstract void ClientDisconnect();

		// Token: 0x06000412 RID: 1042
		public abstract Uri ServerUri();

		// Token: 0x06000413 RID: 1043
		public abstract bool ServerActive();

		// Token: 0x06000414 RID: 1044
		public abstract void ServerStart();

		// Token: 0x06000415 RID: 1045
		public abstract void ServerSend(int connectionId, ArraySegment<byte> segment, int channelId = 0);

		// Token: 0x06000416 RID: 1046
		public abstract void ServerDisconnect(int connectionId);

		// Token: 0x06000417 RID: 1047
		public abstract string ServerGetClientAddress(int connectionId);

		// Token: 0x06000418 RID: 1048
		public abstract void ServerStop();

		// Token: 0x06000419 RID: 1049
		public abstract int GetMaxPacketSize(int channelId = 0);

		// Token: 0x0600041A RID: 1050 RVA: 0x0000EC0B File Offset: 0x0000CE0B
		public virtual int GetBatchThreshold(int channelId = 0)
		{
			return this.GetMaxPacketSize(channelId);
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x000024F9 File Offset: 0x000006F9
		public void Update()
		{
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x000024F9 File Offset: 0x000006F9
		public void LateUpdate()
		{
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual void ClientEarlyUpdate()
		{
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual void ServerEarlyUpdate()
		{
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual void ClientLateUpdate()
		{
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual void ServerLateUpdate()
		{
		}

		// Token: 0x06000421 RID: 1057
		public abstract void Shutdown();

		// Token: 0x06000422 RID: 1058 RVA: 0x0000EC14 File Offset: 0x0000CE14
		public virtual void OnApplicationQuit()
		{
			this.Shutdown();
		}

		// Token: 0x0400018B RID: 395
		public static Transport active;

		// Token: 0x0400018C RID: 396
		public Action OnClientConnected;

		// Token: 0x0400018D RID: 397
		public Action<ArraySegment<byte>, int> OnClientDataReceived;

		// Token: 0x0400018E RID: 398
		public Action<ArraySegment<byte>, int> OnClientDataSent;

		// Token: 0x0400018F RID: 399
		public Action<TransportError, string> OnClientError;

		// Token: 0x04000190 RID: 400
		public Action OnClientDisconnected;

		// Token: 0x04000191 RID: 401
		public Action<int> OnServerConnected;

		// Token: 0x04000192 RID: 402
		public Action<int, ArraySegment<byte>, int> OnServerDataReceived;

		// Token: 0x04000193 RID: 403
		public Action<int, ArraySegment<byte>, int> OnServerDataSent;

		// Token: 0x04000194 RID: 404
		public Action<int, TransportError, string> OnServerError;

		// Token: 0x04000195 RID: 405
		public Action<int> OnServerDisconnected;
	}
}
