using System;
using UnityEngine;

namespace Mirror
{
	// Token: 0x02000016 RID: 22
	[DisallowMultipleComponent]
	[HelpURL("https://mirror-networking.gitbook.io/docs/guides/interest-management")]
	public abstract class InterestManagementBase : MonoBehaviour
	{
		// Token: 0x06000024 RID: 36 RVA: 0x00002708 File Offset: 0x00000908
		protected virtual void OnEnable()
		{
			if (NetworkServer.aoi == null)
			{
				NetworkServer.aoi = this;
			}
			else
			{
				Debug.LogError(string.Format("Only one InterestManagement component allowed. {0} has been set up already.", NetworkServer.aoi.GetType()));
			}
			if (NetworkClient.aoi == null)
			{
				NetworkClient.aoi = this;
				return;
			}
			Debug.LogError(string.Format("Only one InterestManagement component allowed. {0} has been set up already.", NetworkClient.aoi.GetType()));
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002770 File Offset: 0x00000970
		[ServerCallback]
		public virtual void Reset()
		{
			if (!NetworkServer.active)
			{
				return;
			}
		}

		// Token: 0x06000026 RID: 38
		public abstract bool OnCheckObserver(NetworkIdentity identity, NetworkConnectionToClient newObserver);

		// Token: 0x06000027 RID: 39 RVA: 0x00002780 File Offset: 0x00000980
		[ServerCallback]
		public virtual void SetHostVisibility(NetworkIdentity identity, bool visible)
		{
			if (!NetworkServer.active)
			{
				return;
			}
			Renderer[] componentsInChildren = identity.GetComponentsInChildren<Renderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = visible;
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002770 File Offset: 0x00000970
		[ServerCallback]
		public virtual void OnSpawned(NetworkIdentity identity)
		{
			if (!NetworkServer.active)
			{
				return;
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002770 File Offset: 0x00000970
		[ServerCallback]
		public virtual void OnDestroyed(NetworkIdentity identity)
		{
			if (!NetworkServer.active)
			{
				return;
			}
		}

		// Token: 0x0600002A RID: 42
		public abstract void Rebuild(NetworkIdentity identity, bool initialize);

		// Token: 0x0600002B RID: 43 RVA: 0x000027B6 File Offset: 0x000009B6
		protected void AddObserver(NetworkConnectionToClient connection, NetworkIdentity identity)
		{
			connection.AddToObserving(identity);
			identity.observers.Add(connection.connectionId, connection);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000027D1 File Offset: 0x000009D1
		protected void RemoveObserver(NetworkConnectionToClient connection, NetworkIdentity identity)
		{
			connection.RemoveFromObserving(identity, false);
			identity.observers.Remove(connection.connectionId);
		}
	}
}
