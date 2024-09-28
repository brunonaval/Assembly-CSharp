﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>Represents a value tuple with 5 components.</summary>
	/// <typeparam name="T1">The type of the value tuple's first element.</typeparam>
	/// <typeparam name="T2">The type of the value tuple's second element.</typeparam>
	/// <typeparam name="T3">The type of the value tuple's third element.</typeparam>
	/// <typeparam name="T4">The type of the value tuple's fourth element.</typeparam>
	/// <typeparam name="T5">The type of the value tuple's fifth element.</typeparam>
	// Token: 0x020001B2 RID: 434
	[Serializable]
	[StructLayout(LayoutKind.Auto)]
	public struct ValueTuple<T1, T2, T3, T4, T5> : IEquatable<ValueTuple<T1, T2, T3, T4, T5>>, IStructuralEquatable, IStructuralComparable, IComparable, IComparable<ValueTuple<T1, T2, T3, T4, T5>>, IValueTupleInternal, ITuple
	{
		/// <summary>Initializes a new <see cref="T:System.ValueTuple`5" /> instance.</summary>
		/// <param name="item1">The value tuple's first element.</param>
		/// <param name="item2">The value tuple's second element.</param>
		/// <param name="item3">The value tuple's third element.</param>
		/// <param name="item4">The value tuple's fourth element.</param>
		/// <param name="item5">The value tuple's fifth element.</param>
		// Token: 0x060012C4 RID: 4804 RVA: 0x000498C2 File Offset: 0x00047AC2
		public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
		{
			this.Item1 = item1;
			this.Item2 = item2;
			this.Item3 = item3;
			this.Item4 = item4;
			this.Item5 = item5;
		}

		/// <summary>Returns a value that indicates whether the current <see cref="T:System.ValueTuple`5" /> instance is equal to a specified object.</summary>
		/// <param name="obj">The object to compare with this instance.</param>
		/// <returns>
		///   <see langword="true" /> if the current instance is equal to the specified object; otherwise, <see langword="false" />.</returns>
		// Token: 0x060012C5 RID: 4805 RVA: 0x000498E9 File Offset: 0x00047AE9
		public override bool Equals(object obj)
		{
			return obj is ValueTuple<T1, T2, T3, T4, T5> && this.Equals((ValueTuple<T1, T2, T3, T4, T5>)obj);
		}

		/// <summary>Returns a value that indicates whether the current <see cref="T:System.ValueTuple`5" /> instance is equal to a specified <see cref="T:System.ValueTuple`5" /> instance.</summary>
		/// <param name="other">The value tuple to compare with this instance.</param>
		/// <returns>
		///   <see langword="true" /> if the current instance is equal to the specified tuple; otherwise, <see langword="false" />.</returns>
		// Token: 0x060012C6 RID: 4806 RVA: 0x00049904 File Offset: 0x00047B04
		public bool Equals(ValueTuple<T1, T2, T3, T4, T5> other)
		{
			return EqualityComparer<T1>.Default.Equals(this.Item1, other.Item1) && EqualityComparer<T2>.Default.Equals(this.Item2, other.Item2) && EqualityComparer<T3>.Default.Equals(this.Item3, other.Item3) && EqualityComparer<T4>.Default.Equals(this.Item4, other.Item4) && EqualityComparer<T5>.Default.Equals(this.Item5, other.Item5);
		}

		/// <summary>Returns a value that indicates whether the current <see cref="T:System.ValueTuple`5" /> instance is equal to a specified object based on a specified comparison method.</summary>
		/// <param name="other">The object to compare with this instance.</param>
		/// <param name="comparer">An object that defines the method to use to evaluate whether the two objects are equal.</param>
		/// <returns>
		///   <see langword="true" /> if the current instance is equal to the specified objects; otherwise, <see langword="false" />.</returns>
		// Token: 0x060012C7 RID: 4807 RVA: 0x0004998C File Offset: 0x00047B8C
		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (other == null || !(other is ValueTuple<T1, T2, T3, T4, T5>))
			{
				return false;
			}
			ValueTuple<T1, T2, T3, T4, T5> valueTuple = (ValueTuple<T1, T2, T3, T4, T5>)other;
			return comparer.Equals(this.Item1, valueTuple.Item1) && comparer.Equals(this.Item2, valueTuple.Item2) && comparer.Equals(this.Item3, valueTuple.Item3) && comparer.Equals(this.Item4, valueTuple.Item4) && comparer.Equals(this.Item5, valueTuple.Item5);
		}

		/// <summary>Compares the current <see cref="T:System.ValueTuple`5" /> instance to a specified object by using a specified comparer and returns an integer that indicates whether the current object is before, after, or in the same position as the specified object in the sort order.</summary>
		/// <param name="other">The object to compare with the current instance.</param>
		/// <returns>A signed integer that indicates the relative position of this instance and <paramref name="obj" /> in the sort order, as shown in the following table.  
		///   Value  
		///
		///   Description  
		///
		///   A negative integer  
		///
		///   This instance precedes <paramref name="other" />.  
		///
		///   Zero  
		///
		///   This instance and <paramref name="other" /> have the same position in the sort order.  
		///
		///   A positive integer  
		///
		///   This instance follows <paramref name="other" />.</returns>
		// Token: 0x060012C8 RID: 4808 RVA: 0x00049A44 File Offset: 0x00047C44
		int IComparable.CompareTo(object other)
		{
			if (other == null)
			{
				return 1;
			}
			if (!(other is ValueTuple<T1, T2, T3, T4, T5>))
			{
				throw new ArgumentException(SR.Format("Argument must be of type {0}.", base.GetType().ToString()), "other");
			}
			return this.CompareTo((ValueTuple<T1, T2, T3, T4, T5>)other);
		}

		/// <summary>Compares the current <see cref="T:System.ValueTuple`5" /> instance to a specified <see cref="T:System.ValueTuple`5" /> instance.</summary>
		/// <param name="other">The tuple to compare with this instance.</param>
		/// <returns>A signed integer that indicates the relative position of this instance and <paramref name="other" /> in the sort order, as shown in the following table.  
		///   Value  
		///
		///   Description  
		///
		///   A negative integer  
		///
		///   This instance precedes <paramref name="other" />.  
		///
		///   Zero  
		///
		///   This instance and <paramref name="other" /> have the same position in the sort order.  
		///
		///   A positive integer  
		///
		///   This instance follows <paramref name="other" />.</returns>
		// Token: 0x060012C9 RID: 4809 RVA: 0x00049A94 File Offset: 0x00047C94
		public int CompareTo(ValueTuple<T1, T2, T3, T4, T5> other)
		{
			int num = Comparer<T1>.Default.Compare(this.Item1, other.Item1);
			if (num != 0)
			{
				return num;
			}
			num = Comparer<T2>.Default.Compare(this.Item2, other.Item2);
			if (num != 0)
			{
				return num;
			}
			num = Comparer<T3>.Default.Compare(this.Item3, other.Item3);
			if (num != 0)
			{
				return num;
			}
			num = Comparer<T4>.Default.Compare(this.Item4, other.Item4);
			if (num != 0)
			{
				return num;
			}
			return Comparer<T5>.Default.Compare(this.Item5, other.Item5);
		}

		/// <summary>Compares the current <see cref="T:System.ValueTuple`5" /> instance to a specified object by using a specified comparer and returns an integer that indicates whether the current object is before, after, or in the same position as the specified object in the sort order.</summary>
		/// <param name="other">The object to compare with the current instance.</param>
		/// <param name="comparer">An object that provides custom rules for comparison.</param>
		/// <returns>A signed integer that indicates the relative position of this instance and <paramref name="other" /> in the sort order, as shown in the following table.  
		///   Value  
		///
		///   Description  
		///
		///   A negative integer  
		///
		///   This instance precedes <paramref name="other" />.  
		///
		///   Zero  
		///
		///   This instance and <paramref name="other" /> have the same position in the sort order.  
		///
		///   A positive integer  
		///
		///   This instance follows <paramref name="other" />.</returns>
		// Token: 0x060012CA RID: 4810 RVA: 0x00049B28 File Offset: 0x00047D28
		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
			{
				return 1;
			}
			if (!(other is ValueTuple<T1, T2, T3, T4, T5>))
			{
				throw new ArgumentException(SR.Format("Argument must be of type {0}.", base.GetType().ToString()), "other");
			}
			ValueTuple<T1, T2, T3, T4, T5> valueTuple = (ValueTuple<T1, T2, T3, T4, T5>)other;
			int num = comparer.Compare(this.Item1, valueTuple.Item1);
			if (num != 0)
			{
				return num;
			}
			num = comparer.Compare(this.Item2, valueTuple.Item2);
			if (num != 0)
			{
				return num;
			}
			num = comparer.Compare(this.Item3, valueTuple.Item3);
			if (num != 0)
			{
				return num;
			}
			num = comparer.Compare(this.Item4, valueTuple.Item4);
			if (num != 0)
			{
				return num;
			}
			return comparer.Compare(this.Item5, valueTuple.Item5);
		}

		/// <summary>Calculates the hash code for the current <see cref="T:System.ValueTuple`5" /> instance.</summary>
		/// <returns>The hash code for the current <see cref="T:System.ValueTuple`5" /> instance.</returns>
		// Token: 0x060012CB RID: 4811 RVA: 0x00049C18 File Offset: 0x00047E18
		public override int GetHashCode()
		{
			ref T1 ptr = ref this.Item1;
			T1 t = default(T1);
			int h;
			if (t == null)
			{
				t = this.Item1;
				ptr = ref t;
				if (t == null)
				{
					h = 0;
					goto IL_35;
				}
			}
			h = ptr.GetHashCode();
			IL_35:
			ref T2 ptr2 = ref this.Item2;
			T2 t2 = default(T2);
			int h2;
			if (t2 == null)
			{
				t2 = this.Item2;
				ptr2 = ref t2;
				if (t2 == null)
				{
					h2 = 0;
					goto IL_6A;
				}
			}
			h2 = ptr2.GetHashCode();
			IL_6A:
			ref T3 ptr3 = ref this.Item3;
			T3 t3 = default(T3);
			int h3;
			if (t3 == null)
			{
				t3 = this.Item3;
				ptr3 = ref t3;
				if (t3 == null)
				{
					h3 = 0;
					goto IL_9F;
				}
			}
			h3 = ptr3.GetHashCode();
			IL_9F:
			ref T4 ptr4 = ref this.Item4;
			T4 t4 = default(T4);
			int h4;
			if (t4 == null)
			{
				t4 = this.Item4;
				ptr4 = ref t4;
				if (t4 == null)
				{
					h4 = 0;
					goto IL_D4;
				}
			}
			h4 = ptr4.GetHashCode();
			IL_D4:
			ref T5 ptr5 = ref this.Item5;
			T5 t5 = default(T5);
			int h5;
			if (t5 == null)
			{
				t5 = this.Item5;
				ptr5 = ref t5;
				if (t5 == null)
				{
					h5 = 0;
					goto IL_10C;
				}
			}
			h5 = ptr5.GetHashCode();
			IL_10C:
			return ValueTuple.CombineHashCodes(h, h2, h3, h4, h5);
		}

		/// <summary>Calculates the hash code for the current <see cref="T:System.ValueTuple`5" /> instance by using a specified computation method.</summary>
		/// <param name="comparer">An object whose <see cref="M:System.Collections.IEqualityComparer.GetHashCode(System.Object)" /> method calculates the hash code of the current <see cref="T:System.ValueTuple`5" /> instance.</param>
		/// <returns>A 32-bit signed integer hash code.</returns>
		// Token: 0x060012CC RID: 4812 RVA: 0x00049D36 File Offset: 0x00047F36
		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return this.GetHashCodeCore(comparer);
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x00049D40 File Offset: 0x00047F40
		private int GetHashCodeCore(IEqualityComparer comparer)
		{
			return ValueTuple.CombineHashCodes(comparer.GetHashCode(this.Item1), comparer.GetHashCode(this.Item2), comparer.GetHashCode(this.Item3), comparer.GetHashCode(this.Item4), comparer.GetHashCode(this.Item5));
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x00049D36 File Offset: 0x00047F36
		int IValueTupleInternal.GetHashCode(IEqualityComparer comparer)
		{
			return this.GetHashCodeCore(comparer);
		}

		/// <summary>Returns a string that represents the value of this <see cref="T:System.ValueTuple`5" /> instance.</summary>
		/// <returns>The string representation of this <see cref="T:System.ValueTuple`5" /> instance.</returns>
		// Token: 0x060012CF RID: 4815 RVA: 0x00049DA8 File Offset: 0x00047FA8
		public override string ToString()
		{
			string[] array = new string[11];
			array[0] = "(";
			int num = 1;
			ref T1 ptr = ref this.Item1;
			T1 t = default(T1);
			string text;
			if (t == null)
			{
				t = this.Item1;
				ptr = ref t;
				if (t == null)
				{
					text = null;
					goto IL_46;
				}
			}
			text = ptr.ToString();
			IL_46:
			array[num] = text;
			array[2] = ", ";
			int num2 = 3;
			ref T2 ptr2 = ref this.Item2;
			T2 t2 = default(T2);
			string text2;
			if (t2 == null)
			{
				t2 = this.Item2;
				ptr2 = ref t2;
				if (t2 == null)
				{
					text2 = null;
					goto IL_86;
				}
			}
			text2 = ptr2.ToString();
			IL_86:
			array[num2] = text2;
			array[4] = ", ";
			int num3 = 5;
			ref T3 ptr3 = ref this.Item3;
			T3 t3 = default(T3);
			string text3;
			if (t3 == null)
			{
				t3 = this.Item3;
				ptr3 = ref t3;
				if (t3 == null)
				{
					text3 = null;
					goto IL_C6;
				}
			}
			text3 = ptr3.ToString();
			IL_C6:
			array[num3] = text3;
			array[6] = ", ";
			int num4 = 7;
			ref T4 ptr4 = ref this.Item4;
			T4 t4 = default(T4);
			string text4;
			if (t4 == null)
			{
				t4 = this.Item4;
				ptr4 = ref t4;
				if (t4 == null)
				{
					text4 = null;
					goto IL_106;
				}
			}
			text4 = ptr4.ToString();
			IL_106:
			array[num4] = text4;
			array[8] = ", ";
			int num5 = 9;
			ref T5 ptr5 = ref this.Item5;
			T5 t5 = default(T5);
			string text5;
			if (t5 == null)
			{
				t5 = this.Item5;
				ptr5 = ref t5;
				if (t5 == null)
				{
					text5 = null;
					goto IL_14A;
				}
			}
			text5 = ptr5.ToString();
			IL_14A:
			array[num5] = text5;
			array[10] = ")";
			return string.Concat(array);
		}

		// Token: 0x060012D0 RID: 4816 RVA: 0x00049F10 File Offset: 0x00048110
		string IValueTupleInternal.ToStringEnd()
		{
			string[] array = new string[10];
			int num = 0;
			ref T1 ptr = ref this.Item1;
			T1 t = default(T1);
			string text;
			if (t == null)
			{
				t = this.Item1;
				ptr = ref t;
				if (t == null)
				{
					text = null;
					goto IL_3E;
				}
			}
			text = ptr.ToString();
			IL_3E:
			array[num] = text;
			array[1] = ", ";
			int num2 = 2;
			ref T2 ptr2 = ref this.Item2;
			T2 t2 = default(T2);
			string text2;
			if (t2 == null)
			{
				t2 = this.Item2;
				ptr2 = ref t2;
				if (t2 == null)
				{
					text2 = null;
					goto IL_7E;
				}
			}
			text2 = ptr2.ToString();
			IL_7E:
			array[num2] = text2;
			array[3] = ", ";
			int num3 = 4;
			ref T3 ptr3 = ref this.Item3;
			T3 t3 = default(T3);
			string text3;
			if (t3 == null)
			{
				t3 = this.Item3;
				ptr3 = ref t3;
				if (t3 == null)
				{
					text3 = null;
					goto IL_BE;
				}
			}
			text3 = ptr3.ToString();
			IL_BE:
			array[num3] = text3;
			array[5] = ", ";
			int num4 = 6;
			ref T4 ptr4 = ref this.Item4;
			T4 t4 = default(T4);
			string text4;
			if (t4 == null)
			{
				t4 = this.Item4;
				ptr4 = ref t4;
				if (t4 == null)
				{
					text4 = null;
					goto IL_FE;
				}
			}
			text4 = ptr4.ToString();
			IL_FE:
			array[num4] = text4;
			array[7] = ", ";
			int num5 = 8;
			ref T5 ptr5 = ref this.Item5;
			T5 t5 = default(T5);
			string text5;
			if (t5 == null)
			{
				t5 = this.Item5;
				ptr5 = ref t5;
				if (t5 == null)
				{
					text5 = null;
					goto IL_141;
				}
			}
			text5 = ptr5.ToString();
			IL_141:
			array[num5] = text5;
			array[9] = ")";
			return string.Concat(array);
		}

		/// <summary>Gets the number of elements in the <see langword="ValueTuple" />.</summary>
		/// <returns>5, the number of elements in a <see cref="T:System.ValueTuple`5" /> object.</returns>
		// Token: 0x170001BC RID: 444
		// (get) Token: 0x060012D1 RID: 4817 RVA: 0x0003CDA4 File Offset: 0x0003AFA4
		int ITuple.Length
		{
			get
			{
				return 5;
			}
		}

		/// <summary>Gets the value of the specified <see langword="ValueTuple" /> element.</summary>
		/// <param name="index">The index of the specified <see langword="ValueTuple" /> element. <paramref name="index" /> can range from 0 to 4.</param>
		/// <returns>The value of the <see langword="ValueTuple" /> element at the specified position.</returns>
		/// <exception cref="T:System.IndexOutOfRangeException">
		///   <paramref name="index" /> is less than 0 or greater than 4.</exception>
		// Token: 0x170001BD RID: 445
		object ITuple.this[int index]
		{
			get
			{
				switch (index)
				{
				case 0:
					return this.Item1;
				case 1:
					return this.Item2;
				case 2:
					return this.Item3;
				case 3:
					return this.Item4;
				case 4:
					return this.Item5;
				default:
					throw new IndexOutOfRangeException();
				}
			}
		}

		/// <summary>Gets the value of the current <see cref="T:System.ValueTuple`5" /> instance's first element.</summary>
		// Token: 0x04001370 RID: 4976
		public T1 Item1;

		/// <summary>Gets the value of the current <see cref="T:System.ValueTuple`5" /> instance's second element.</summary>
		// Token: 0x04001371 RID: 4977
		public T2 Item2;

		/// <summary>Gets the value of the current <see cref="T:System.ValueTuple`5" /> instance's third element.</summary>
		// Token: 0x04001372 RID: 4978
		public T3 Item3;

		/// <summary>Gets the value of the current <see cref="T:System.ValueTuple`5" /> instance's fourth element.</summary>
		// Token: 0x04001373 RID: 4979
		public T4 Item4;

		/// <summary>Gets the value of the current <see cref="T:System.ValueTuple`5" /> instance's fifth element.</summary>
		// Token: 0x04001374 RID: 4980
		public T5 Item5;
	}
}
