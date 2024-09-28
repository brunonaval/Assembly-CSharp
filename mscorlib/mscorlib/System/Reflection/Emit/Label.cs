﻿using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	/// <summary>Represents a label in the instruction stream. <see langword="Label" /> is used in conjunction with the <see cref="T:System.Reflection.Emit.ILGenerator" /> class.</summary>
	// Token: 0x02000932 RID: 2354
	[ComVisible(true)]
	[Serializable]
	public readonly struct Label : IEquatable<Label>
	{
		// Token: 0x060050EC RID: 20716 RVA: 0x000FD738 File Offset: 0x000FB938
		internal Label(int val)
		{
			this.label = val;
		}

		/// <summary>Checks if the given object is an instance of <see langword="Label" /> and is equal to this instance.</summary>
		/// <param name="obj">The object to compare with this <see langword="Label" /> instance.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="obj" /> is an instance of <see langword="Label" /> and is equal to this object; otherwise, <see langword="false" />.</returns>
		// Token: 0x060050ED RID: 20717 RVA: 0x000FD744 File Offset: 0x000FB944
		public override bool Equals(object obj)
		{
			bool flag = obj is Label;
			if (flag)
			{
				Label label = (Label)obj;
				flag = (this.label == label.label);
			}
			return flag;
		}

		/// <summary>Indicates whether the current instance is equal to the specified <see cref="T:System.Reflection.Emit.Label" />.</summary>
		/// <param name="obj">The <see cref="T:System.Reflection.Emit.Label" /> to compare to the current instance.</param>
		/// <returns>
		///   <see langword="true" /> if the value of <paramref name="obj" /> is equal to the value of the current instance; otherwise, <see langword="false" />.</returns>
		// Token: 0x060050EE RID: 20718 RVA: 0x000FD775 File Offset: 0x000FB975
		public bool Equals(Label obj)
		{
			return this.label == obj.label;
		}

		/// <summary>Indicates whether two <see cref="T:System.Reflection.Emit.Label" /> structures are equal.</summary>
		/// <param name="a">The <see cref="T:System.Reflection.Emit.Label" /> to compare to <paramref name="b" />.</param>
		/// <param name="b">The <see cref="T:System.Reflection.Emit.Label" /> to compare to <paramref name="a" />.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="a" /> is equal to <paramref name="b" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060050EF RID: 20719 RVA: 0x000FD785 File Offset: 0x000FB985
		public static bool operator ==(Label a, Label b)
		{
			return a.Equals(b);
		}

		/// <summary>Indicates whether two <see cref="T:System.Reflection.Emit.Label" /> structures are not equal.</summary>
		/// <param name="a">The <see cref="T:System.Reflection.Emit.Label" /> to compare to <paramref name="b" />.</param>
		/// <param name="b">The <see cref="T:System.Reflection.Emit.Label" /> to compare to <paramref name="a" />.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="a" /> is not equal to <paramref name="b" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060050F0 RID: 20720 RVA: 0x000FD78F File Offset: 0x000FB98F
		public static bool operator !=(Label a, Label b)
		{
			return !(a == b);
		}

		/// <summary>Generates a hash code for this instance.</summary>
		/// <returns>A hash code for this instance.</returns>
		// Token: 0x060050F1 RID: 20721 RVA: 0x000FD79B File Offset: 0x000FB99B
		public override int GetHashCode()
		{
			return this.label.GetHashCode();
		}

		// Token: 0x040031B2 RID: 12722
		internal readonly int label;
	}
}
