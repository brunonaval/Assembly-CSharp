using System;
using System.Diagnostics;
using System.Security;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020006A2 RID: 1698
	internal sealed class MemberPrimitiveUnTyped : IStreamable
	{
		// Token: 0x06003E63 RID: 15971 RVA: 0x0000259F File Offset: 0x0000079F
		internal MemberPrimitiveUnTyped()
		{
		}

		// Token: 0x06003E64 RID: 15972 RVA: 0x000D784F File Offset: 0x000D5A4F
		internal void Set(InternalPrimitiveTypeE typeInformation, object value)
		{
			this.typeInformation = typeInformation;
			this.value = value;
		}

		// Token: 0x06003E65 RID: 15973 RVA: 0x000D785F File Offset: 0x000D5A5F
		internal void Set(InternalPrimitiveTypeE typeInformation)
		{
			this.typeInformation = typeInformation;
		}

		// Token: 0x06003E66 RID: 15974 RVA: 0x000D7868 File Offset: 0x000D5A68
		public void Write(__BinaryWriter sout)
		{
			sout.WriteValue(this.typeInformation, this.value);
		}

		// Token: 0x06003E67 RID: 15975 RVA: 0x000D787C File Offset: 0x000D5A7C
		[SecurityCritical]
		public void Read(__BinaryParser input)
		{
			this.value = input.ReadValue(this.typeInformation);
		}

		// Token: 0x06003E68 RID: 15976 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public void Dump()
		{
		}

		// Token: 0x06003E69 RID: 15977 RVA: 0x000D7890 File Offset: 0x000D5A90
		[Conditional("_LOGGING")]
		private void DumpInternal()
		{
			if (BCLDebug.CheckEnabled("BINARY"))
			{
				Converter.ToComType(this.typeInformation);
			}
		}

		// Token: 0x0400287B RID: 10363
		internal InternalPrimitiveTypeE typeInformation;

		// Token: 0x0400287C RID: 10364
		internal object value;
	}
}
