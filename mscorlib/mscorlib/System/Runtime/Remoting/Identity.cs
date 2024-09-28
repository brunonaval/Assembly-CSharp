using System;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting
{
	// Token: 0x02000560 RID: 1376
	internal abstract class Identity
	{
		// Token: 0x060035E8 RID: 13800 RVA: 0x000C23A1 File Offset: 0x000C05A1
		public Identity(string objectUri)
		{
			this._objectUri = objectUri;
		}

		// Token: 0x060035E9 RID: 13801
		public abstract ObjRef CreateObjRef(Type requestedType);

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x060035EA RID: 13802 RVA: 0x000C23B0 File Offset: 0x000C05B0
		public bool IsFromThisAppDomain
		{
			get
			{
				return this._channelSink == null;
			}
		}

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x060035EB RID: 13803 RVA: 0x000C23BB File Offset: 0x000C05BB
		// (set) Token: 0x060035EC RID: 13804 RVA: 0x000C23C3 File Offset: 0x000C05C3
		public IMessageSink ChannelSink
		{
			get
			{
				return this._channelSink;
			}
			set
			{
				this._channelSink = value;
			}
		}

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x060035ED RID: 13805 RVA: 0x000C23CC File Offset: 0x000C05CC
		public IMessageSink EnvoySink
		{
			get
			{
				return this._envoySink;
			}
		}

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x060035EE RID: 13806 RVA: 0x000C23D4 File Offset: 0x000C05D4
		// (set) Token: 0x060035EF RID: 13807 RVA: 0x000C23DC File Offset: 0x000C05DC
		public string ObjectUri
		{
			get
			{
				return this._objectUri;
			}
			set
			{
				this._objectUri = value;
			}
		}

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x060035F0 RID: 13808 RVA: 0x000C23E5 File Offset: 0x000C05E5
		public bool IsConnected
		{
			get
			{
				return this._objectUri != null;
			}
		}

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x060035F1 RID: 13809 RVA: 0x000C23F0 File Offset: 0x000C05F0
		// (set) Token: 0x060035F2 RID: 13810 RVA: 0x000C23F8 File Offset: 0x000C05F8
		public bool Disposed
		{
			get
			{
				return this._disposed;
			}
			set
			{
				this._disposed = value;
			}
		}

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x060035F3 RID: 13811 RVA: 0x000C2401 File Offset: 0x000C0601
		public DynamicPropertyCollection ClientDynamicProperties
		{
			get
			{
				if (this._clientDynamicProperties == null)
				{
					this._clientDynamicProperties = new DynamicPropertyCollection();
				}
				return this._clientDynamicProperties;
			}
		}

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x060035F4 RID: 13812 RVA: 0x000C241C File Offset: 0x000C061C
		public DynamicPropertyCollection ServerDynamicProperties
		{
			get
			{
				if (this._serverDynamicProperties == null)
				{
					this._serverDynamicProperties = new DynamicPropertyCollection();
				}
				return this._serverDynamicProperties;
			}
		}

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x060035F5 RID: 13813 RVA: 0x000C2437 File Offset: 0x000C0637
		public bool HasClientDynamicSinks
		{
			get
			{
				return this._clientDynamicProperties != null && this._clientDynamicProperties.HasProperties;
			}
		}

		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x060035F6 RID: 13814 RVA: 0x000C244E File Offset: 0x000C064E
		public bool HasServerDynamicSinks
		{
			get
			{
				return this._serverDynamicProperties != null && this._serverDynamicProperties.HasProperties;
			}
		}

		// Token: 0x060035F7 RID: 13815 RVA: 0x000C2465 File Offset: 0x000C0665
		public void NotifyClientDynamicSinks(bool start, IMessage req_msg, bool client_site, bool async)
		{
			if (this._clientDynamicProperties != null && this._clientDynamicProperties.HasProperties)
			{
				this._clientDynamicProperties.NotifyMessage(start, req_msg, client_site, async);
			}
		}

		// Token: 0x060035F8 RID: 13816 RVA: 0x000C248C File Offset: 0x000C068C
		public void NotifyServerDynamicSinks(bool start, IMessage req_msg, bool client_site, bool async)
		{
			if (this._serverDynamicProperties != null && this._serverDynamicProperties.HasProperties)
			{
				this._serverDynamicProperties.NotifyMessage(start, req_msg, client_site, async);
			}
		}

		// Token: 0x0400251B RID: 9499
		protected string _objectUri;

		// Token: 0x0400251C RID: 9500
		protected IMessageSink _channelSink;

		// Token: 0x0400251D RID: 9501
		protected IMessageSink _envoySink;

		// Token: 0x0400251E RID: 9502
		private DynamicPropertyCollection _clientDynamicProperties;

		// Token: 0x0400251F RID: 9503
		private DynamicPropertyCollection _serverDynamicProperties;

		// Token: 0x04002520 RID: 9504
		protected ObjRef _objRef;

		// Token: 0x04002521 RID: 9505
		private bool _disposed;
	}
}
