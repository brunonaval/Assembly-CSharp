using System;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000638 RID: 1592
	internal class ServerObjectReplySink : IMessageSink
	{
		// Token: 0x06003C16 RID: 15382 RVA: 0x000D0E69 File Offset: 0x000CF069
		public ServerObjectReplySink(ServerIdentity identity, IMessageSink replySink)
		{
			this._replySink = replySink;
			this._identity = identity;
		}

		// Token: 0x06003C17 RID: 15383 RVA: 0x000D0E7F File Offset: 0x000CF07F
		public IMessage SyncProcessMessage(IMessage msg)
		{
			this._identity.NotifyServerDynamicSinks(false, msg, true, true);
			return this._replySink.SyncProcessMessage(msg);
		}

		// Token: 0x06003C18 RID: 15384 RVA: 0x000472CC File Offset: 0x000454CC
		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x06003C19 RID: 15385 RVA: 0x000D0E9C File Offset: 0x000CF09C
		public IMessageSink NextSink
		{
			get
			{
				return this._replySink;
			}
		}

		// Token: 0x040026EA RID: 9962
		private IMessageSink _replySink;

		// Token: 0x040026EB RID: 9963
		private ServerIdentity _identity;
	}
}
