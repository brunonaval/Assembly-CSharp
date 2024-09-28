using System;
using System.Runtime.CompilerServices;

namespace Mirror
{
	// Token: 0x0200008C RID: 140
	public struct Vector3Long
	{
		// Token: 0x060003FD RID: 1021 RVA: 0x0000E96D File Offset: 0x0000CB6D
		public Vector3Long(long x, long y, long z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0000E984 File Offset: 0x0000CB84
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3Long operator +(Vector3Long a, Vector3Long b)
		{
			return new Vector3Long(a.x + b.x, a.y + b.y, a.z + b.z);
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x0000E9B2 File Offset: 0x0000CBB2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3Long operator -(Vector3Long a, Vector3Long b)
		{
			return new Vector3Long(a.x - b.x, a.y - b.y, a.z - b.z);
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0000E9E0 File Offset: 0x0000CBE0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3Long operator -(Vector3Long v)
		{
			return new Vector3Long(-v.x, -v.y, -v.z);
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0000E9FC File Offset: 0x0000CBFC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3Long operator *(Vector3Long a, long n)
		{
			return new Vector3Long(a.x * n, a.y * n, a.z * n);
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0000EA1B File Offset: 0x0000CC1B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3Long operator *(long n, Vector3Long a)
		{
			return new Vector3Long(a.x * n, a.y * n, a.z * n);
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0000EA3A File Offset: 0x0000CC3A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Vector3Long a, Vector3Long b)
		{
			return a.x == b.x && a.y == b.y && a.z == b.z;
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0000EA68 File Offset: 0x0000CC68
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Vector3Long a, Vector3Long b)
		{
			return !(a == b);
		}

		// Token: 0x17000065 RID: 101
		public long this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				switch (index)
				{
				case 0:
					return this.x;
				case 1:
					return this.y;
				case 2:
					return this.z;
				default:
					throw new IndexOutOfRangeException(string.Format("Vector3Long[{0}] out of range.", index));
				}
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				switch (index)
				{
				case 0:
					this.x = value;
					return;
				case 1:
					this.y = value;
					return;
				case 2:
					this.z = value;
					return;
				default:
					throw new IndexOutOfRangeException(string.Format("Vector3Long[{0}] out of range.", index));
				}
			}
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0000EB02 File Offset: 0x0000CD02
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			return string.Format("({0} {1} {2})", this.x, this.y, this.z);
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0000EA3A File Offset: 0x0000CC3A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Vector3Long other)
		{
			return this.x == other.x && this.y == other.y && this.z == other.z;
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0000EB30 File Offset: 0x0000CD30
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object other)
		{
			if (other is Vector3Long)
			{
				Vector3Long other2 = (Vector3Long)other;
				return this.Equals(other2);
			}
			return false;
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0000EB55 File Offset: 0x0000CD55
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode()
		{
			return HashCode.Combine<long, long, long>(this.x, this.y, this.z);
		}

		// Token: 0x04000180 RID: 384
		public long x;

		// Token: 0x04000181 RID: 385
		public long y;

		// Token: 0x04000182 RID: 386
		public long z;

		// Token: 0x04000183 RID: 387
		public static readonly Vector3Long zero = new Vector3Long(0L, 0L, 0L);

		// Token: 0x04000184 RID: 388
		public static readonly Vector3Long one = new Vector3Long(1L, 1L, 1L);

		// Token: 0x04000185 RID: 389
		public static readonly Vector3Long forward = new Vector3Long(0L, 0L, 1L);

		// Token: 0x04000186 RID: 390
		public static readonly Vector3Long back = new Vector3Long(0L, 0L, -1L);

		// Token: 0x04000187 RID: 391
		public static readonly Vector3Long left = new Vector3Long(-1L, 0L, 0L);

		// Token: 0x04000188 RID: 392
		public static readonly Vector3Long right = new Vector3Long(1L, 0L, 0L);

		// Token: 0x04000189 RID: 393
		public static readonly Vector3Long up = new Vector3Long(0L, 1L, 0L);

		// Token: 0x0400018A RID: 394
		public static readonly Vector3Long down = new Vector3Long(0L, -1L, 0L);
	}
}
