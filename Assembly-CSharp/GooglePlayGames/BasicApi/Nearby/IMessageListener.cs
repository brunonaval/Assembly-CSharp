using System;

namespace GooglePlayGames.BasicApi.Nearby
{
	// Token: 0x020006B8 RID: 1720
	public interface IMessageListener
	{
		// Token: 0x060025B5 RID: 9653
		void OnMessageReceived(string remoteEndpointId, byte[] data, bool isReliableMessage);

		// Token: 0x060025B6 RID: 9654
		void OnRemoteEndpointDisconnected(string remoteEndpointId);
	}
}
