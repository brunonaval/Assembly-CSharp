﻿using System;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting
{
	// Token: 0x02000571 RID: 1393
	internal class SingletonIdentity : ServerIdentity
	{
		// Token: 0x060036C3 RID: 14019 RVA: 0x000C5BD3 File Offset: 0x000C3DD3
		public SingletonIdentity(string objectUri, Context context, Type objectType) : base(objectUri, context, objectType)
		{
		}

		// Token: 0x060036C4 RID: 14020 RVA: 0x000C5BE0 File Offset: 0x000C3DE0
		public MarshalByRefObject GetServerObject()
		{
			if (this._serverObject != null)
			{
				return this._serverObject;
			}
			lock (this)
			{
				if (this._serverObject == null)
				{
					MarshalByRefObject marshalByRefObject = (MarshalByRefObject)Activator.CreateInstance(this._objectType, true);
					base.AttachServerObject(marshalByRefObject, Context.DefaultContext);
					base.StartTrackingLifetime((ILease)marshalByRefObject.InitializeLifetimeService());
				}
			}
			return this._serverObject;
		}

		// Token: 0x060036C5 RID: 14021 RVA: 0x000C5C64 File Offset: 0x000C3E64
		public override IMessage SyncObjectProcessMessage(IMessage msg)
		{
			MarshalByRefObject serverObject = this.GetServerObject();
			if (this._serverSink == null)
			{
				this._serverSink = this._context.CreateServerObjectSinkChain(serverObject, false);
			}
			return this._serverSink.SyncProcessMessage(msg);
		}

		// Token: 0x060036C6 RID: 14022 RVA: 0x000C5CA0 File Offset: 0x000C3EA0
		public override IMessageCtrl AsyncObjectProcessMessage(IMessage msg, IMessageSink replySink)
		{
			MarshalByRefObject serverObject = this.GetServerObject();
			if (this._serverSink == null)
			{
				this._serverSink = this._context.CreateServerObjectSinkChain(serverObject, false);
			}
			return this._serverSink.AsyncProcessMessage(msg, replySink);
		}
	}
}
