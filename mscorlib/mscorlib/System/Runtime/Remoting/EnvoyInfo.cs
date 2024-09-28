using System;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting
{
	// Token: 0x0200055B RID: 1371
	[Serializable]
	internal class EnvoyInfo : IEnvoyInfo
	{
		// Token: 0x060035DD RID: 13789 RVA: 0x000C2381 File Offset: 0x000C0581
		public EnvoyInfo(IMessageSink sinks)
		{
			this.envoySinks = sinks;
		}

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x060035DE RID: 13790 RVA: 0x000C2390 File Offset: 0x000C0590
		// (set) Token: 0x060035DF RID: 13791 RVA: 0x000C2398 File Offset: 0x000C0598
		public IMessageSink EnvoySinks
		{
			get
			{
				return this.envoySinks;
			}
			set
			{
				this.envoySinks = value;
			}
		}

		// Token: 0x0400251A RID: 9498
		private IMessageSink envoySinks;
	}
}
