﻿using System;

namespace Mono.Math.Prime
{
	// Token: 0x0200009D RID: 157
	internal sealed class PrimalityTests
	{
		// Token: 0x060003F2 RID: 1010 RVA: 0x0000259F File Offset: 0x0000079F
		private PrimalityTests()
		{
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00015558 File Offset: 0x00013758
		private static int GetSPPRounds(BigInteger bi, ConfidenceFactor confidence)
		{
			int num = bi.BitCount();
			int num2;
			if (num <= 100)
			{
				num2 = 27;
			}
			else if (num <= 150)
			{
				num2 = 18;
			}
			else if (num <= 200)
			{
				num2 = 15;
			}
			else if (num <= 250)
			{
				num2 = 12;
			}
			else if (num <= 300)
			{
				num2 = 9;
			}
			else if (num <= 350)
			{
				num2 = 8;
			}
			else if (num <= 400)
			{
				num2 = 7;
			}
			else if (num <= 500)
			{
				num2 = 6;
			}
			else if (num <= 600)
			{
				num2 = 5;
			}
			else if (num <= 800)
			{
				num2 = 4;
			}
			else if (num <= 1250)
			{
				num2 = 3;
			}
			else
			{
				num2 = 2;
			}
			switch (confidence)
			{
			case ConfidenceFactor.ExtraLow:
				num2 >>= 2;
				if (num2 == 0)
				{
					return 1;
				}
				return num2;
			case ConfidenceFactor.Low:
				num2 >>= 1;
				if (num2 == 0)
				{
					return 1;
				}
				return num2;
			case ConfidenceFactor.Medium:
				return num2;
			case ConfidenceFactor.High:
				return num2 << 1;
			case ConfidenceFactor.ExtraHigh:
				return num2 << 2;
			case ConfidenceFactor.Provable:
				throw new Exception("The Rabin-Miller test can not be executed in a way such that its results are provable");
			default:
				throw new ArgumentOutOfRangeException("confidence");
			}
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x00015649 File Offset: 0x00013849
		public static bool Test(BigInteger n, ConfidenceFactor confidence)
		{
			if (n.BitCount() < 33)
			{
				return PrimalityTests.SmallPrimeSppTest(n, confidence);
			}
			return PrimalityTests.RabinMillerTest(n, confidence);
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x00015664 File Offset: 0x00013864
		public static bool RabinMillerTest(BigInteger n, ConfidenceFactor confidence)
		{
			int num = n.BitCount();
			int spprounds = PrimalityTests.GetSPPRounds(num, confidence);
			BigInteger bigInteger = n - 1;
			int num2 = bigInteger.LowestSetBit();
			BigInteger bigInteger2 = bigInteger >> num2;
			BigInteger.ModulusRing modulusRing = new BigInteger.ModulusRing(n);
			BigInteger bigInteger3 = null;
			if (n.BitCount() > 100)
			{
				bigInteger3 = modulusRing.Pow(2U, bigInteger2);
			}
			for (int i = 0; i < spprounds; i++)
			{
				if (i > 0 || bigInteger3 == null)
				{
					BigInteger bigInteger4;
					do
					{
						bigInteger4 = BigInteger.GenerateRandom(num);
					}
					while (bigInteger4 <= 2 && bigInteger4 >= bigInteger);
					bigInteger3 = modulusRing.Pow(bigInteger4, bigInteger2);
				}
				if (!(bigInteger3 == 1U))
				{
					int num3 = 0;
					while (num3 < num2 && bigInteger3 != bigInteger)
					{
						bigInteger3 = modulusRing.Pow(bigInteger3, 2);
						if (bigInteger3 == 1U)
						{
							return false;
						}
						num3++;
					}
					if (bigInteger3 != bigInteger)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x00015768 File Offset: 0x00013968
		public static bool SmallPrimeSppTest(BigInteger bi, ConfidenceFactor confidence)
		{
			int spprounds = PrimalityTests.GetSPPRounds(bi, confidence);
			BigInteger bigInteger = bi - 1;
			int num = bigInteger.LowestSetBit();
			BigInteger exp = bigInteger >> num;
			BigInteger.ModulusRing modulusRing = new BigInteger.ModulusRing(bi);
			for (int i = 0; i < spprounds; i++)
			{
				BigInteger bigInteger2 = modulusRing.Pow(BigInteger.smallPrimes[i], exp);
				if (!(bigInteger2 == 1U))
				{
					bool flag = false;
					for (int j = 0; j < num; j++)
					{
						if (bigInteger2 == bigInteger)
						{
							flag = true;
							break;
						}
						bigInteger2 = bigInteger2 * bigInteger2 % bi;
					}
					if (!flag)
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
