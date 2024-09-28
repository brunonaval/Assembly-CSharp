﻿using System;
using System.Runtime.Remoting.Activation;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000636 RID: 1590
	internal class ServerContextTerminatorSink : IMessageSink
	{
		// Token: 0x06003C0E RID: 15374 RVA: 0x000D0D87 File Offset: 0x000CEF87
		public IMessage SyncProcessMessage(IMessage msg)
		{
			if (msg is IConstructionCallMessage)
			{
				return ActivationServices.CreateInstanceFromMessage((IConstructionCallMessage)msg);
			}
			return ((ServerIdentity)RemotingServices.GetMessageTargetIdentity(msg)).SyncObjectProcessMessage(msg);
		}

		// Token: 0x06003C0F RID: 15375 RVA: 0x000D0DAE File Offset: 0x000CEFAE
		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			return ((ServerIdentity)RemotingServices.GetMessageTargetIdentity(msg)).AsyncObjectProcessMessage(msg, replySink);
		}

		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x06003C10 RID: 15376 RVA: 0x0000AF5E File Offset: 0x0000915E
		public IMessageSink NextSink
		{
			get
			{
				return null;
			}
		}
	}
}
