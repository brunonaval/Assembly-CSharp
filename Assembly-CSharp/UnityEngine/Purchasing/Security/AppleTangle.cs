using System;

namespace UnityEngine.Purchasing.Security
{
	// Token: 0x02000586 RID: 1414
	public class AppleTangle
	{
		// Token: 0x06001F56 RID: 8022 RVA: 0x0009DFA4 File Offset: 0x0009C1A4
		public static byte[] Data()
		{
			if (!AppleTangle.IsPopulated)
			{
				return null;
			}
			return Obfuscator.DeObfuscate(AppleTangle.data, AppleTangle.order, AppleTangle.key);
		}

		// Token: 0x0400194C RID: 6476
		private static byte[] data = Convert.FromBase64String("w2/J5zZQZl6ze2nCOegqF7w/9GNJUhNU/kceg3n2u6qf11uNJx4vEN/XBeYzJyG121s1x4yPlwS5ktc4cXR39nV7dET2dX529nV1dJDl3X1HQi5EFkV/RH1ydyFwcmd2ISdFZxgRVD0aF1pFUkRQcnchcH9naTUEWFQXEQYAHRIdFxUAEVQEGxgdFw0aEFQXGxoQHQAdGxoHVBsSVAEHEcFO2YB7enTmf8VVYloAoUh5rxZiVBsSVAAcEVQAHBEaVBUEBBgdFxVE9nDPRPZ319R3dnV2dnV2RHlyfUFGRUBER0IuY3lHQURGRE1GRUBEcnchaXpwYnBgX6QdM+ACfYqAH/lEZXJ3IXB+Z341BAQYEVQ9GhdaRQ1UFQcHARkRB1QVFxcRBAAVGhcR9nV0cn1e8jzygxcQcXVE9YZEXnJUNzVE9nVWRHlyfV7yPPKDeXV1dWJEYHJ3IXB3Z3k1BAQYEVQmGxsAJhEYHRUaFxFUGxpUABwdB1QXEQb/bf2qjT8YgXPfVkR2nGxKjCR9p17yPPKDeXV1cXF0RBZFf0R9cnchLdNxfQhjNCJlagCnw/9XTzPXoRvcqApWQb5Roa17oh+g1lBXZYPV2AAdEh0XFQARVBYNVBUaDVQEFQYAeXJ9XvI88oN5dXVxcXR39nV1dChbRPW3cnxfcnVxcXN2dkT1wm71x2vlr2ozJJ9xmSoN8FmfQtYjOCGY4eoOeNAz/y+gYkNHv7B7ObpgHaV8X3J1cXFzdnViahwAAAQHTltbA7QXRwODTnNYIp+ue1V6rs4HbTvBrUILtfMhrdPtzUY2j6yhBeoK1SYEGBFUJhsbAFQ3NURqY3lEQkRARsqAB++aphB7vw07QKzWSo0Mix+8BhUXAB0XEVQHABUAERkRGgAHWkQAHBsGHQANRWJEYHJ3IXB3Z3k1BPRgX6QdM+ACfYqAH/laNNKDMzkLHRIdFxUAHRsaVDUBABwbBh0ADUW9bQaBKXqhCyvvhlF3ziH7OSl5hRBBV2E/YS1px+CDgujquyTOtSwkUkRQcnchcH9naTUEBBgRVDcRBgBr8ffxb+1JM0OG3e80+ligxeRmrHvpSYdfPVxuvIq6wc16rSpoor9JxUQsmC5wRvgcx/tpqhEHixMqEcgEGBFUNxEGAB0SHRcVAB0bGlQ1AULtOFkMw5n476iHA++GAqYDRDu1fCpE9nVlcnchaVRw9nV8RPZ1cEQxCms4HyTiNf2wABZ/ZPc180f+9RYYEVQHABUaEBUGEFQAEQYZB1QVcHJndiEnRWdEZXJ3IXB+Z341BARaNNKDMzkLfCpEa3J3IWlXcGxEYhP7fMBUg7/YWFQbBMJLdUT4wze7c5gJTff/J1SnTLDFy+47fh+LX4g9rALrR2AR1QPgvVl2d3V0ddf2dQ5E9nUCRHpydyFpe3V1i3Bwd3Z1ckR7cnchaWd1dYtwcUR3dXWLRGn7B/UUsm8vfVvmxowwPIQUTOphgVCWn6XDBKt7MZVTvoUZDJmTwWNjAwNaFQQEGBFaFxsZWxUEBBgRFxVUFRoQVBcRBgAdEh0XFQAdGxpUBAs13OyNpb4S6FAfZaTXz5BvXrdrJN7+oa6QiKR9c0PEAQFV");

		// Token: 0x0400194D RID: 6477
		private static int[] order = new int[]
		{
			59,
			56,
			24,
			14,
			8,
			14,
			44,
			43,
			23,
			41,
			59,
			43,
			30,
			41,
			40,
			34,
			41,
			30,
			38,
			41,
			56,
			52,
			54,
			39,
			47,
			37,
			34,
			38,
			35,
			57,
			58,
			44,
			50,
			46,
			40,
			50,
			43,
			41,
			56,
			59,
			49,
			54,
			49,
			59,
			48,
			46,
			54,
			49,
			49,
			49,
			52,
			51,
			56,
			56,
			57,
			55,
			56,
			59,
			59,
			59,
			60
		};

		// Token: 0x0400194E RID: 6478
		private static int key = 116;

		// Token: 0x0400194F RID: 6479
		public static readonly bool IsPopulated = true;
	}
}
