using System;

namespace System.Reflection
{
	// Token: 0x020008DF RID: 2271
	public sealed class MissingMetadataException : TypeAccessException
	{
		// Token: 0x06004BA6 RID: 19366 RVA: 0x000F10A0 File Offset: 0x000EF2A0
		public MissingMetadataException()
		{
		}

		// Token: 0x06004BA7 RID: 19367 RVA: 0x000F10A8 File Offset: 0x000EF2A8
		public MissingMetadataException(string message) : base(message)
		{
		}
	}
}
