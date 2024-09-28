using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020006B2 RID: 1714
	internal sealed class TypeInformation
	{
		// Token: 0x170009AB RID: 2475
		// (get) Token: 0x06003F26 RID: 16166 RVA: 0x000DA8B2 File Offset: 0x000D8AB2
		internal string FullTypeName
		{
			get
			{
				return this.fullTypeName;
			}
		}

		// Token: 0x170009AC RID: 2476
		// (get) Token: 0x06003F27 RID: 16167 RVA: 0x000DA8BA File Offset: 0x000D8ABA
		internal string AssemblyString
		{
			get
			{
				return this.assemblyString;
			}
		}

		// Token: 0x170009AD RID: 2477
		// (get) Token: 0x06003F28 RID: 16168 RVA: 0x000DA8C2 File Offset: 0x000D8AC2
		internal bool HasTypeForwardedFrom
		{
			get
			{
				return this.hasTypeForwardedFrom;
			}
		}

		// Token: 0x06003F29 RID: 16169 RVA: 0x000DA8CA File Offset: 0x000D8ACA
		internal TypeInformation(string fullTypeName, string assemblyString, bool hasTypeForwardedFrom)
		{
			this.fullTypeName = fullTypeName;
			this.assemblyString = assemblyString;
			this.hasTypeForwardedFrom = hasTypeForwardedFrom;
		}

		// Token: 0x0400292A RID: 10538
		private string fullTypeName;

		// Token: 0x0400292B RID: 10539
		private string assemblyString;

		// Token: 0x0400292C RID: 10540
		private bool hasTypeForwardedFrom;
	}
}
