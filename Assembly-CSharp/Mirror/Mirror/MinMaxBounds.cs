using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Mirror
{
	// Token: 0x0200001B RID: 27
	public struct MinMaxBounds : IEquatable<Bounds>
	{
		// Token: 0x0600003C RID: 60 RVA: 0x00002C4A File Offset: 0x00000E4A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Encapsulate(Vector3 point)
		{
			this.min = Vector3.Min(this.min, point);
			this.max = Vector3.Max(this.max, point);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002C70 File Offset: 0x00000E70
		public void Encapsulate(MinMaxBounds bounds)
		{
			this.Encapsulate(bounds.min);
			this.Encapsulate(bounds.max);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002C8A File Offset: 0x00000E8A
		public static bool operator ==(MinMaxBounds lhs, Bounds rhs)
		{
			return lhs.min == rhs.min && lhs.max == rhs.max;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002CB4 File Offset: 0x00000EB4
		public static bool operator !=(MinMaxBounds lhs, Bounds rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002CC0 File Offset: 0x00000EC0
		public override bool Equals(object obj)
		{
			if (obj is MinMaxBounds)
			{
				MinMaxBounds minMaxBounds = (MinMaxBounds)obj;
				if (this.min == minMaxBounds.min)
				{
					return this.max == minMaxBounds.max;
				}
			}
			return false;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002D02 File Offset: 0x00000F02
		public bool Equals(MinMaxBounds other)
		{
			return this.min.Equals(other.min) && this.max.Equals(other.max);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002D2A File Offset: 0x00000F2A
		public bool Equals(Bounds other)
		{
			return this.min.Equals(other.min) && this.max.Equals(other.max);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002D54 File Offset: 0x00000F54
		public override int GetHashCode()
		{
			return HashCode.Combine<Vector3, Vector3>(this.min, this.max);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002D67 File Offset: 0x00000F67
		public override string ToString()
		{
			return string.Format("({0}, {1})", this.min, this.max);
		}

		// Token: 0x04000023 RID: 35
		public Vector3 min;

		// Token: 0x04000024 RID: 36
		public Vector3 max;
	}
}
