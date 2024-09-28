﻿using System;
using System.Threading;

namespace System.Collections.Generic
{
	// Token: 0x02000ABF RID: 2751
	internal class GenericArraySortHelper<TKey, TValue> where TKey : IComparable<TKey>
	{
		// Token: 0x0600626A RID: 25194 RVA: 0x00149664 File Offset: 0x00147864
		public void Sort(TKey[] keys, TValue[] values, int index, int length, IComparer<TKey> comparer)
		{
			try
			{
				if (comparer == null || comparer == Comparer<TKey>.Default)
				{
					GenericArraySortHelper<TKey, TValue>.IntrospectiveSort(keys, values, index, length);
				}
				else
				{
					ArraySortHelper<TKey, TValue>.IntrospectiveSort(keys, values, index, length, comparer);
				}
			}
			catch (IndexOutOfRangeException)
			{
				IntrospectiveSortUtilities.ThrowOrIgnoreBadComparer(comparer);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception innerException)
			{
				throw new InvalidOperationException("Failed to compare two elements in the array.", innerException);
			}
		}

		// Token: 0x0600626B RID: 25195 RVA: 0x001496DC File Offset: 0x001478DC
		private static void SwapIfGreaterWithItems(TKey[] keys, TValue[] values, int a, int b)
		{
			if (a != b && keys[a] != null && keys[a].CompareTo(keys[b]) > 0)
			{
				TKey tkey = keys[a];
				keys[a] = keys[b];
				keys[b] = tkey;
				TValue tvalue = values[a];
				values[a] = values[b];
				values[b] = tvalue;
			}
		}

		// Token: 0x0600626C RID: 25196 RVA: 0x00149758 File Offset: 0x00147958
		private static void Swap(TKey[] keys, TValue[] values, int i, int j)
		{
			if (i != j)
			{
				TKey tkey = keys[i];
				keys[i] = keys[j];
				keys[j] = tkey;
				TValue tvalue = values[i];
				values[i] = values[j];
				values[j] = tvalue;
			}
		}

		// Token: 0x0600626D RID: 25197 RVA: 0x001497A5 File Offset: 0x001479A5
		internal static void IntrospectiveSort(TKey[] keys, TValue[] values, int left, int length)
		{
			if (length < 2)
			{
				return;
			}
			GenericArraySortHelper<TKey, TValue>.IntroSort(keys, values, left, length + left - 1, 2 * IntrospectiveSortUtilities.FloorLog2PlusOne(length));
		}

		// Token: 0x0600626E RID: 25198 RVA: 0x001497C4 File Offset: 0x001479C4
		private static void IntroSort(TKey[] keys, TValue[] values, int lo, int hi, int depthLimit)
		{
			while (hi > lo)
			{
				int num = hi - lo + 1;
				if (num <= 16)
				{
					if (num == 1)
					{
						return;
					}
					if (num == 2)
					{
						GenericArraySortHelper<TKey, TValue>.SwapIfGreaterWithItems(keys, values, lo, hi);
						return;
					}
					if (num == 3)
					{
						GenericArraySortHelper<TKey, TValue>.SwapIfGreaterWithItems(keys, values, lo, hi - 1);
						GenericArraySortHelper<TKey, TValue>.SwapIfGreaterWithItems(keys, values, lo, hi);
						GenericArraySortHelper<TKey, TValue>.SwapIfGreaterWithItems(keys, values, hi - 1, hi);
						return;
					}
					GenericArraySortHelper<TKey, TValue>.InsertionSort(keys, values, lo, hi);
					return;
				}
				else
				{
					if (depthLimit == 0)
					{
						GenericArraySortHelper<TKey, TValue>.Heapsort(keys, values, lo, hi);
						return;
					}
					depthLimit--;
					int num2 = GenericArraySortHelper<TKey, TValue>.PickPivotAndPartition(keys, values, lo, hi);
					GenericArraySortHelper<TKey, TValue>.IntroSort(keys, values, num2 + 1, hi, depthLimit);
					hi = num2 - 1;
				}
			}
		}

		// Token: 0x0600626F RID: 25199 RVA: 0x00149854 File Offset: 0x00147A54
		private static int PickPivotAndPartition(TKey[] keys, TValue[] values, int lo, int hi)
		{
			int num = lo + (hi - lo) / 2;
			GenericArraySortHelper<TKey, TValue>.SwapIfGreaterWithItems(keys, values, lo, num);
			GenericArraySortHelper<TKey, TValue>.SwapIfGreaterWithItems(keys, values, lo, hi);
			GenericArraySortHelper<TKey, TValue>.SwapIfGreaterWithItems(keys, values, num, hi);
			TKey tkey = keys[num];
			GenericArraySortHelper<TKey, TValue>.Swap(keys, values, num, hi - 1);
			int i = lo;
			int j = hi - 1;
			while (i < j)
			{
				if (tkey == null)
				{
					while (i < hi - 1 && keys[++i] == null)
					{
					}
					while (j > lo)
					{
						if (keys[--j] == null)
						{
							break;
						}
					}
				}
				else
				{
					while (tkey.CompareTo(keys[++i]) > 0)
					{
					}
					while (tkey.CompareTo(keys[--j]) < 0)
					{
					}
				}
				if (i >= j)
				{
					break;
				}
				GenericArraySortHelper<TKey, TValue>.Swap(keys, values, i, j);
			}
			GenericArraySortHelper<TKey, TValue>.Swap(keys, values, i, hi - 1);
			return i;
		}

		// Token: 0x06006270 RID: 25200 RVA: 0x0014992C File Offset: 0x00147B2C
		private static void Heapsort(TKey[] keys, TValue[] values, int lo, int hi)
		{
			int num = hi - lo + 1;
			for (int i = num / 2; i >= 1; i--)
			{
				GenericArraySortHelper<TKey, TValue>.DownHeap(keys, values, i, num, lo);
			}
			for (int j = num; j > 1; j--)
			{
				GenericArraySortHelper<TKey, TValue>.Swap(keys, values, lo, lo + j - 1);
				GenericArraySortHelper<TKey, TValue>.DownHeap(keys, values, 1, j - 1, lo);
			}
		}

		// Token: 0x06006271 RID: 25201 RVA: 0x0014997C File Offset: 0x00147B7C
		private static void DownHeap(TKey[] keys, TValue[] values, int i, int n, int lo)
		{
			TKey tkey = keys[lo + i - 1];
			TValue tvalue = values[lo + i - 1];
			while (i <= n / 2)
			{
				int num = 2 * i;
				if (num < n && (keys[lo + num - 1] == null || keys[lo + num - 1].CompareTo(keys[lo + num]) < 0))
				{
					num++;
				}
				if (keys[lo + num - 1] == null || keys[lo + num - 1].CompareTo(tkey) < 0)
				{
					break;
				}
				keys[lo + i - 1] = keys[lo + num - 1];
				values[lo + i - 1] = values[lo + num - 1];
				i = num;
			}
			keys[lo + i - 1] = tkey;
			values[lo + i - 1] = tvalue;
		}

		// Token: 0x06006272 RID: 25202 RVA: 0x00149A74 File Offset: 0x00147C74
		private static void InsertionSort(TKey[] keys, TValue[] values, int lo, int hi)
		{
			for (int i = lo; i < hi; i++)
			{
				int num = i;
				TKey tkey = keys[i + 1];
				TValue tvalue = values[i + 1];
				while (num >= lo && (tkey == null || tkey.CompareTo(keys[num]) < 0))
				{
					keys[num + 1] = keys[num];
					values[num + 1] = values[num];
					num--;
				}
				keys[num + 1] = tkey;
				values[num + 1] = tvalue;
			}
		}
	}
}
