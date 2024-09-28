using System;
using System.Collections;
using System.Security;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000602 RID: 1538
	internal class IllogicalCallContext
	{
		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x06003A41 RID: 14913 RVA: 0x000CC6E8 File Offset: 0x000CA8E8
		private Hashtable Datastore
		{
			get
			{
				if (this.m_Datastore == null)
				{
					this.m_Datastore = new Hashtable();
				}
				return this.m_Datastore;
			}
		}

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06003A42 RID: 14914 RVA: 0x000CC703 File Offset: 0x000CA903
		// (set) Token: 0x06003A43 RID: 14915 RVA: 0x000CC70B File Offset: 0x000CA90B
		internal object HostContext
		{
			get
			{
				return this.m_HostContext;
			}
			set
			{
				this.m_HostContext = value;
			}
		}

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x06003A44 RID: 14916 RVA: 0x000CC714 File Offset: 0x000CA914
		internal bool HasUserData
		{
			get
			{
				return this.m_Datastore != null && this.m_Datastore.Count > 0;
			}
		}

		// Token: 0x06003A45 RID: 14917 RVA: 0x000CC72E File Offset: 0x000CA92E
		public void FreeNamedDataSlot(string name)
		{
			this.Datastore.Remove(name);
		}

		// Token: 0x06003A46 RID: 14918 RVA: 0x000CC73C File Offset: 0x000CA93C
		public object GetData(string name)
		{
			return this.Datastore[name];
		}

		// Token: 0x06003A47 RID: 14919 RVA: 0x000CC74A File Offset: 0x000CA94A
		public void SetData(string name, object data)
		{
			this.Datastore[name] = data;
		}

		// Token: 0x06003A48 RID: 14920 RVA: 0x000CC75C File Offset: 0x000CA95C
		public IllogicalCallContext CreateCopy()
		{
			IllogicalCallContext illogicalCallContext = new IllogicalCallContext();
			illogicalCallContext.HostContext = this.HostContext;
			if (this.HasUserData)
			{
				IDictionaryEnumerator enumerator = this.m_Datastore.GetEnumerator();
				while (enumerator.MoveNext())
				{
					illogicalCallContext.Datastore[(string)enumerator.Key] = enumerator.Value;
				}
			}
			return illogicalCallContext;
		}

		// Token: 0x0400264A RID: 9802
		private Hashtable m_Datastore;

		// Token: 0x0400264B RID: 9803
		private object m_HostContext;

		// Token: 0x02000603 RID: 1539
		internal struct Reader
		{
			// Token: 0x06003A4A RID: 14922 RVA: 0x000CC7B6 File Offset: 0x000CA9B6
			public Reader(IllogicalCallContext ctx)
			{
				this.m_ctx = ctx;
			}

			// Token: 0x1700087C RID: 2172
			// (get) Token: 0x06003A4B RID: 14923 RVA: 0x000CC7BF File Offset: 0x000CA9BF
			public bool IsNull
			{
				get
				{
					return this.m_ctx == null;
				}
			}

			// Token: 0x06003A4C RID: 14924 RVA: 0x000CC7CA File Offset: 0x000CA9CA
			[SecurityCritical]
			public object GetData(string name)
			{
				if (!this.IsNull)
				{
					return this.m_ctx.GetData(name);
				}
				return null;
			}

			// Token: 0x1700087D RID: 2173
			// (get) Token: 0x06003A4D RID: 14925 RVA: 0x000CC7E2 File Offset: 0x000CA9E2
			public object HostContext
			{
				get
				{
					if (!this.IsNull)
					{
						return this.m_ctx.HostContext;
					}
					return null;
				}
			}

			// Token: 0x0400264C RID: 9804
			private IllogicalCallContext m_ctx;
		}
	}
}
