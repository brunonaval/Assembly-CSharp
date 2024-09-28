using System;
using System.Security.Principal;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000606 RID: 1542
	[Serializable]
	internal class CallContextSecurityData : ICloneable
	{
		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x06003A6E RID: 14958 RVA: 0x000CCE6C File Offset: 0x000CB06C
		// (set) Token: 0x06003A6F RID: 14959 RVA: 0x000CCE74 File Offset: 0x000CB074
		internal IPrincipal Principal
		{
			get
			{
				return this._principal;
			}
			set
			{
				this._principal = value;
			}
		}

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x06003A70 RID: 14960 RVA: 0x000CCE7D File Offset: 0x000CB07D
		internal bool HasInfo
		{
			get
			{
				return this._principal != null;
			}
		}

		// Token: 0x06003A71 RID: 14961 RVA: 0x000CCE88 File Offset: 0x000CB088
		public object Clone()
		{
			return new CallContextSecurityData
			{
				_principal = this._principal
			};
		}

		// Token: 0x04002657 RID: 9815
		private IPrincipal _principal;
	}
}
