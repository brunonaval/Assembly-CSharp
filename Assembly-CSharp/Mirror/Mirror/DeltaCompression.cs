using System;
using System.Runtime.CompilerServices;

namespace Mirror
{
	// Token: 0x02000080 RID: 128
	public static class DeltaCompression
	{
		// Token: 0x060003CB RID: 971 RVA: 0x0000E1F5 File Offset: 0x0000C3F5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Compress(NetworkWriter writer, long last, long current)
		{
			Compression.CompressVarInt(writer, current - last);
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0000E200 File Offset: 0x0000C400
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long Decompress(NetworkReader reader, long last)
		{
			return last + Compression.DecompressVarInt(reader);
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0000E20A File Offset: 0x0000C40A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Compress(NetworkWriter writer, Vector3Long last, Vector3Long current)
		{
			DeltaCompression.Compress(writer, last.x, current.x);
			DeltaCompression.Compress(writer, last.y, current.y);
			DeltaCompression.Compress(writer, last.z, current.z);
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0000E244 File Offset: 0x0000C444
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3Long Decompress(NetworkReader reader, Vector3Long last)
		{
			long x = DeltaCompression.Decompress(reader, last.x);
			long y = DeltaCompression.Decompress(reader, last.y);
			long z = DeltaCompression.Decompress(reader, last.z);
			return new Vector3Long(x, y, z);
		}
	}
}
