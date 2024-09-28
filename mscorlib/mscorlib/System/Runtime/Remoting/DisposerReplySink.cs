using System;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting
{
	// Token: 0x02000573 RID: 1395
	internal class DisposerReplySink : IMessageSink
	{
		// Token: 0x060036CA RID: 14026 RVA: 0x000C5D79 File Offset: 0x000C3F79
		public DisposerReplySink(IMessageSink next, IDisposable disposable)
		{
			this._next = next;
			this._disposable = disposable;
		}

		// Token: 0x060036CB RID: 14027 RVA: 0x000C5D8F File Offset: 0x000C3F8F
		public IMessage SyncProcessMessage(IMessage msg)
		{
			this._disposable.Dispose();
			return this._next.SyncProcessMessage(msg);
		}

		// Token: 0x060036CC RID: 14028 RVA: 0x000472CC File Offset: 0x000454CC
		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			throw new NotSupportedException();
		}

		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x060036CD RID: 14029 RVA: 0x000C5DA8 File Offset: 0x000C3FA8
		public IMessageSink NextSink
		{
			get
			{
				return this._next;
			}
		}

		// Token: 0x0400255F RID: 9567
		private IMessageSink _next;

		// Token: 0x04002560 RID: 9568
		private IDisposable _disposable;
	}
}
