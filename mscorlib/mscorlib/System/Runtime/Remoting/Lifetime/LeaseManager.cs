﻿using System;
using System.Collections;
using System.Threading;

namespace System.Runtime.Remoting.Lifetime
{
	// Token: 0x02000587 RID: 1415
	internal class LeaseManager
	{
		// Token: 0x06003772 RID: 14194 RVA: 0x000C7D88 File Offset: 0x000C5F88
		public void SetPollTime(TimeSpan timeSpan)
		{
			object syncRoot = this._objects.SyncRoot;
			lock (syncRoot)
			{
				if (this._timer != null)
				{
					this._timer.Change(timeSpan, timeSpan);
				}
			}
		}

		// Token: 0x06003773 RID: 14195 RVA: 0x000C7DE0 File Offset: 0x000C5FE0
		public void TrackLifetime(ServerIdentity identity)
		{
			object syncRoot = this._objects.SyncRoot;
			lock (syncRoot)
			{
				identity.Lease.Activate();
				this._objects.Add(identity);
				if (this._timer == null)
				{
					this.StartManager();
				}
			}
		}

		// Token: 0x06003774 RID: 14196 RVA: 0x000C7E48 File Offset: 0x000C6048
		public void StopTrackingLifetime(ServerIdentity identity)
		{
			object syncRoot = this._objects.SyncRoot;
			lock (syncRoot)
			{
				this._objects.Remove(identity);
			}
		}

		// Token: 0x06003775 RID: 14197 RVA: 0x000C7E94 File Offset: 0x000C6094
		public void StartManager()
		{
			this._timer = new Timer(new TimerCallback(this.ManageLeases), null, LifetimeServices.LeaseManagerPollTime, LifetimeServices.LeaseManagerPollTime);
		}

		// Token: 0x06003776 RID: 14198 RVA: 0x000C7EB8 File Offset: 0x000C60B8
		public void StopManager()
		{
			Timer timer = this._timer;
			this._timer = null;
			if (timer != null)
			{
				timer.Dispose();
			}
		}

		// Token: 0x06003777 RID: 14199 RVA: 0x000C7EDC File Offset: 0x000C60DC
		public void ManageLeases(object state)
		{
			object syncRoot = this._objects.SyncRoot;
			lock (syncRoot)
			{
				int i = 0;
				while (i < this._objects.Count)
				{
					ServerIdentity serverIdentity = (ServerIdentity)this._objects[i];
					serverIdentity.Lease.UpdateState();
					if (serverIdentity.Lease.CurrentState == LeaseState.Expired)
					{
						this._objects.RemoveAt(i);
						serverIdentity.OnLifetimeExpired();
					}
					else
					{
						i++;
					}
				}
				if (this._objects.Count == 0)
				{
					this.StopManager();
				}
			}
		}

		// Token: 0x04002591 RID: 9617
		private ArrayList _objects = new ArrayList();

		// Token: 0x04002592 RID: 9618
		private Timer _timer;
	}
}
