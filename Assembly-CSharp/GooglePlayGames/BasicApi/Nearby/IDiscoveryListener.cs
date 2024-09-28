using System;

namespace GooglePlayGames.BasicApi.Nearby
{
	// Token: 0x020006B9 RID: 1721
	public interface IDiscoveryListener
	{
		// Token: 0x060025B7 RID: 9655
		void OnEndpointFound(EndpointDetails discoveredEndpoint);

		// Token: 0x060025B8 RID: 9656
		void OnEndpointLost(string lostEndpointId);
	}
}
