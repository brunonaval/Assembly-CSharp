using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
	// Token: 0x02000015 RID: 21
	[DisallowMultipleComponent]
	[HelpURL("https://mirror-networking.gitbook.io/docs/guides/interest-management")]
	public abstract class InterestManagement : InterestManagementBase
	{
		// Token: 0x06000020 RID: 32
		public abstract void OnRebuildObservers(NetworkIdentity identity, HashSet<NetworkConnectionToClient> newObservers);

		// Token: 0x06000021 RID: 33 RVA: 0x000024FC File Offset: 0x000006FC
		[ServerCallback]
		protected void RebuildAll()
		{
			if (!NetworkServer.active)
			{
				return;
			}
			foreach (NetworkIdentity identity in NetworkServer.spawned.Values)
			{
				NetworkServer.RebuildObservers(identity, false);
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x0000255C File Offset: 0x0000075C
		public override void Rebuild(NetworkIdentity identity, bool initialize)
		{
			this.newObservers.Clear();
			if (identity.visible != Visibility.ForceHidden)
			{
				this.OnRebuildObservers(identity, this.newObservers);
			}
			if (identity.connectionToClient != null)
			{
				this.newObservers.Add(identity.connectionToClient);
			}
			bool flag = false;
			foreach (NetworkConnectionToClient networkConnectionToClient in this.newObservers)
			{
				if (networkConnectionToClient != null && networkConnectionToClient.isReady && (initialize || !identity.observers.ContainsKey(networkConnectionToClient.connectionId)))
				{
					networkConnectionToClient.AddToObserving(identity);
					flag = true;
				}
			}
			foreach (NetworkConnectionToClient networkConnectionToClient2 in identity.observers.Values)
			{
				if (!this.newObservers.Contains(networkConnectionToClient2))
				{
					networkConnectionToClient2.RemoveFromObserving(identity, false);
					flag = true;
				}
			}
			if (flag)
			{
				identity.observers.Clear();
				foreach (NetworkConnectionToClient networkConnectionToClient3 in this.newObservers)
				{
					if (networkConnectionToClient3 != null && networkConnectionToClient3.isReady)
					{
						identity.observers.Add(networkConnectionToClient3.connectionId, networkConnectionToClient3);
					}
				}
			}
			if (initialize && !this.newObservers.Contains(NetworkServer.localConnection))
			{
				this.SetHostVisibility(identity, false);
			}
		}

		// Token: 0x04000019 RID: 25
		private readonly HashSet<NetworkConnectionToClient> newObservers = new HashSet<NetworkConnectionToClient>();
	}
}
