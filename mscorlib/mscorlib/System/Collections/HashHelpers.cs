﻿using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading;

namespace System.Collections
{
	// Token: 0x02000A11 RID: 2577
	internal static class HashHelpers
	{
		// Token: 0x06005B79 RID: 23417 RVA: 0x00134B94 File Offset: 0x00132D94
		public static bool IsPrime(int candidate)
		{
			if ((candidate & 1) != 0)
			{
				int num = (int)Math.Sqrt((double)candidate);
				for (int i = 3; i <= num; i += 2)
				{
					if (candidate % i == 0)
					{
						return false;
					}
				}
				return true;
			}
			return candidate == 2;
		}

		// Token: 0x06005B7A RID: 23418 RVA: 0x00134BC8 File Offset: 0x00132DC8
		public static int GetPrime(int min)
		{
			if (min < 0)
			{
				throw new ArgumentException("Hashtable's capacity overflowed and went negative. Check load factor, capacity and the current size of the table.");
			}
			for (int i = 0; i < HashHelpers.primes.Length; i++)
			{
				int num = HashHelpers.primes[i];
				if (num >= min)
				{
					return num;
				}
			}
			for (int j = min | 1; j < 2147483647; j += 2)
			{
				if (HashHelpers.IsPrime(j) && (j - 1) % 101 != 0)
				{
					return j;
				}
			}
			return min;
		}

		// Token: 0x06005B7B RID: 23419 RVA: 0x00134C2C File Offset: 0x00132E2C
		public static int ExpandPrime(int oldSize)
		{
			int num = 2 * oldSize;
			if (num > 2146435069 && 2146435069 > oldSize)
			{
				return 2146435069;
			}
			return HashHelpers.GetPrime(num);
		}

		// Token: 0x17000FBE RID: 4030
		// (get) Token: 0x06005B7C RID: 23420 RVA: 0x00134C59 File Offset: 0x00132E59
		internal static ConditionalWeakTable<object, SerializationInfo> SerializationInfoTable
		{
			get
			{
				if (HashHelpers.s_serializationInfoTable == null)
				{
					Interlocked.CompareExchange<ConditionalWeakTable<object, SerializationInfo>>(ref HashHelpers.s_serializationInfoTable, new ConditionalWeakTable<object, SerializationInfo>(), null);
				}
				return HashHelpers.s_serializationInfoTable;
			}
		}

		// Token: 0x04003869 RID: 14441
		public const int HashCollisionThreshold = 100;

		// Token: 0x0400386A RID: 14442
		public const int HashPrime = 101;

		// Token: 0x0400386B RID: 14443
		public static readonly int[] primes = new int[]
		{
			3,
			7,
			11,
			17,
			23,
			29,
			37,
			47,
			59,
			71,
			89,
			107,
			131,
			163,
			197,
			239,
			293,
			353,
			431,
			521,
			631,
			761,
			919,
			1103,
			1327,
			1597,
			1931,
			2333,
			2801,
			3371,
			4049,
			4861,
			5839,
			7013,
			8419,
			10103,
			12143,
			14591,
			17519,
			21023,
			25229,
			30293,
			36353,
			43627,
			52361,
			62851,
			75431,
			90523,
			108631,
			130363,
			156437,
			187751,
			225307,
			270371,
			324449,
			389357,
			467237,
			560689,
			672827,
			807403,
			968897,
			1162687,
			1395263,
			1674319,
			2009191,
			2411033,
			2893249,
			3471899,
			4166287,
			4999559,
			5999471,
			7199369
		};

		// Token: 0x0400386C RID: 14444
		public const int MaxPrimeArrayLength = 2146435069;

		// Token: 0x0400386D RID: 14445
		private static ConditionalWeakTable<object, SerializationInfo> s_serializationInfoTable;
	}
}
