﻿using System;
using System.Globalization;

namespace Mono.Globalization.Unicode
{
	// Token: 0x02000074 RID: 116
	internal class SimpleCollator : ISimpleCollator
	{
		// Token: 0x060001B7 RID: 439 RVA: 0x00006D0C File Offset: 0x00004F0C
		public SimpleCollator(CultureInfo culture)
		{
			this.lcid = culture.LCID;
			this.textInfo = culture.TextInfo;
			this.SetCJKTable(culture, ref this.cjkIndexer, ref this.cjkCatTable, ref this.cjkLv1Table, ref this.cjkLv2Indexer, ref this.cjkLv2Table);
			TailoringInfo tailoringInfo = null;
			CultureInfo cultureInfo = culture;
			while (cultureInfo.LCID != 127)
			{
				tailoringInfo = MSCompatUnicodeTable.GetTailoringInfo(cultureInfo.LCID);
				if (tailoringInfo != null)
				{
					break;
				}
				cultureInfo = cultureInfo.Parent;
			}
			if (tailoringInfo == null)
			{
				tailoringInfo = MSCompatUnicodeTable.GetTailoringInfo(127);
			}
			this.frenchSort = tailoringInfo.FrenchSort;
			MSCompatUnicodeTable.BuildTailoringTables(culture, tailoringInfo, ref this.contractions, ref this.level2Maps);
			this.unsafeFlags = new byte[96];
			foreach (Contraction contraction in this.contractions)
			{
				if (contraction.Source.Length > 1)
				{
					foreach (char c in contraction.Source)
					{
						byte[] array2 = this.unsafeFlags;
						char c2 = c / '\b';
						array2[(int)c2] = (array2[(int)c2] | (byte)(1 << (int)(c & '\a')));
					}
				}
			}
			if (this.lcid != 127)
			{
				foreach (Contraction contraction2 in SimpleCollator.invariant.contractions)
				{
					if (contraction2.Source.Length > 1)
					{
						foreach (char c3 in contraction2.Source)
						{
							byte[] array3 = this.unsafeFlags;
							char c4 = c3 / '\b';
							array3[(int)c4] = (array3[(int)c4] | (byte)(1 << (int)(c3 & '\a')));
						}
					}
				}
			}
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00006E95 File Offset: 0x00005095
		private unsafe void SetCJKTable(CultureInfo culture, ref CodePointIndexer cjkIndexer, ref byte* catTable, ref byte* lv1Table, ref CodePointIndexer lv2Indexer, ref byte* lv2Table)
		{
			MSCompatUnicodeTable.FillCJK(SimpleCollator.GetNeutralCulture(culture).Name, ref cjkIndexer, ref catTable, ref lv1Table, ref lv2Indexer, ref lv2Table);
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00006EB0 File Offset: 0x000050B0
		private static CultureInfo GetNeutralCulture(CultureInfo info)
		{
			CultureInfo cultureInfo = info;
			while (cultureInfo.Parent != null && cultureInfo.Parent.LCID != 127)
			{
				cultureInfo = cultureInfo.Parent;
			}
			return cultureInfo;
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00006EE0 File Offset: 0x000050E0
		private unsafe byte Category(int cp)
		{
			if (cp < 12288 || this.cjkCatTable == null)
			{
				return MSCompatUnicodeTable.Category(cp);
			}
			int num = this.cjkIndexer.ToIndex(cp);
			if (num >= 0)
			{
				return this.cjkCatTable[num];
			}
			return MSCompatUnicodeTable.Category(cp);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00006F28 File Offset: 0x00005128
		private unsafe byte Level1(int cp)
		{
			if (cp < 12288 || this.cjkLv1Table == null)
			{
				return MSCompatUnicodeTable.Level1(cp);
			}
			int num = this.cjkIndexer.ToIndex(cp);
			if (num >= 0)
			{
				return this.cjkLv1Table[num];
			}
			return MSCompatUnicodeTable.Level1(cp);
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00006F70 File Offset: 0x00005170
		private unsafe byte Level2(int cp, SimpleCollator.ExtenderType ext)
		{
			if (ext == SimpleCollator.ExtenderType.Buggy)
			{
				return 5;
			}
			if (ext == SimpleCollator.ExtenderType.Conditional)
			{
				return 0;
			}
			if (cp < 12288 || this.cjkLv2Table == null)
			{
				return MSCompatUnicodeTable.Level2(cp);
			}
			int num = this.cjkLv2Indexer.ToIndex(cp);
			byte b = (num < 0) ? 0 : this.cjkLv2Table[num];
			if (b != 0)
			{
				return b;
			}
			b = MSCompatUnicodeTable.Level2(cp);
			if (this.level2Maps.Length == 0)
			{
				return b;
			}
			for (int i = 0; i < this.level2Maps.Length; i++)
			{
				if (this.level2Maps[i].Source == b)
				{
					return this.level2Maps[i].Replace;
				}
				if (this.level2Maps[i].Source > b)
				{
					break;
				}
			}
			return b;
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00007019 File Offset: 0x00005219
		private static bool IsHalfKana(int cp, CompareOptions opt)
		{
			return (opt & CompareOptions.IgnoreWidth) != CompareOptions.None || MSCompatUnicodeTable.IsHalfWidthKana((char)cp);
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000702C File Offset: 0x0000522C
		private Contraction GetContraction(string s, int start, int end)
		{
			Contraction contraction = this.GetContraction(s, start, end, this.contractions);
			if (contraction != null || this.lcid == 127)
			{
				return contraction;
			}
			return this.GetContraction(s, start, end, SimpleCollator.invariant.contractions);
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000706C File Offset: 0x0000526C
		private Contraction GetContraction(string s, int start, int end, Contraction[] clist)
		{
			foreach (Contraction contraction in clist)
			{
				int num = (int)(contraction.Source[0] - s[start]);
				if (num > 0)
				{
					return null;
				}
				if (num >= 0)
				{
					char[] source = contraction.Source;
					if (end - start >= source.Length)
					{
						bool flag = true;
						for (int j = 0; j < source.Length; j++)
						{
							if (s[start + j] != source[j])
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							return contraction;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x000070E8 File Offset: 0x000052E8
		private Contraction GetTailContraction(string s, int start, int end)
		{
			Contraction tailContraction = this.GetTailContraction(s, start, end, this.contractions);
			if (tailContraction != null || this.lcid == 127)
			{
				return tailContraction;
			}
			return this.GetTailContraction(s, start, end, SimpleCollator.invariant.contractions);
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00007128 File Offset: 0x00005328
		private Contraction GetTailContraction(string s, int start, int end, Contraction[] clist)
		{
			if (start == end || end < -1 || start >= s.Length || s.Length <= end + 1)
			{
				throw new SystemException(string.Format("MONO internal error. Failed to get TailContraction. start = {0} end = {1} string = '{2}'", start, end, s));
			}
			foreach (Contraction contraction in clist)
			{
				char[] source = contraction.Source;
				if (source.Length <= start - end && source[source.Length - 1] == s[start])
				{
					bool flag = true;
					int j = 0;
					int num = start - source.Length + 1;
					while (j < source.Length)
					{
						if (s[num] != source[j])
						{
							flag = false;
							break;
						}
						j++;
						num++;
					}
					if (flag)
					{
						return contraction;
					}
				}
			}
			return null;
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x000071DC File Offset: 0x000053DC
		private Contraction GetContraction(char c)
		{
			Contraction contraction = this.GetContraction(c, this.contractions);
			if (contraction != null || this.lcid == 127)
			{
				return contraction;
			}
			return this.GetContraction(c, SimpleCollator.invariant.contractions);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00007218 File Offset: 0x00005418
		private Contraction GetContraction(char c, Contraction[] clist)
		{
			foreach (Contraction contraction in clist)
			{
				if (contraction.Source[0] > c)
				{
					return null;
				}
				if (contraction.Source[0] == c && contraction.Source.Length == 1)
				{
					return contraction;
				}
			}
			return null;
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00007260 File Offset: 0x00005460
		private int FilterOptions(int i, CompareOptions opt)
		{
			if ((opt & CompareOptions.IgnoreWidth) != CompareOptions.None)
			{
				int num = MSCompatUnicodeTable.ToWidthCompat(i);
				if (num != 0)
				{
					i = num;
				}
			}
			if ((opt & CompareOptions.OrdinalIgnoreCase) != CompareOptions.None)
			{
				i = (int)this.textInfo.ToLower((char)i);
			}
			if ((opt & CompareOptions.IgnoreCase) != CompareOptions.None)
			{
				i = (int)this.textInfo.ToLower((char)i);
			}
			if ((opt & CompareOptions.IgnoreKanaType) != CompareOptions.None)
			{
				i = MSCompatUnicodeTable.ToKanaTypeInsensitive(i);
			}
			return i;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x000072BC File Offset: 0x000054BC
		private SimpleCollator.ExtenderType GetExtenderType(int i)
		{
			if (i == 8213)
			{
				if (this.lcid != 16)
				{
					return SimpleCollator.ExtenderType.None;
				}
				return SimpleCollator.ExtenderType.Conditional;
			}
			else
			{
				if (i < 12293 || i > 65392)
				{
					return SimpleCollator.ExtenderType.None;
				}
				if (i >= 65148)
				{
					if (i - 65148 <= 1)
					{
						return SimpleCollator.ExtenderType.Simple;
					}
					if (i == 65392)
					{
						return SimpleCollator.ExtenderType.Conditional;
					}
					if (i - 65438 <= 1)
					{
						return SimpleCollator.ExtenderType.Voiced;
					}
				}
				if (i > 12542)
				{
					return SimpleCollator.ExtenderType.None;
				}
				if (i <= 12338)
				{
					if (i == 12293)
					{
						return SimpleCollator.ExtenderType.Buggy;
					}
					if (i - 12337 > 1)
					{
						return SimpleCollator.ExtenderType.None;
					}
				}
				else if (i != 12445)
				{
					if (i != 12446)
					{
						switch (i)
						{
						case 12540:
							return SimpleCollator.ExtenderType.Conditional;
						case 12541:
							return SimpleCollator.ExtenderType.Simple;
						case 12542:
							break;
						default:
							return SimpleCollator.ExtenderType.None;
						}
					}
					return SimpleCollator.ExtenderType.Voiced;
				}
				return SimpleCollator.ExtenderType.Simple;
			}
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00007376 File Offset: 0x00005576
		private static byte ToDashTypeValue(SimpleCollator.ExtenderType ext, CompareOptions opt)
		{
			if ((opt & CompareOptions.IgnoreNonSpace) != CompareOptions.None)
			{
				return 3;
			}
			if (ext == SimpleCollator.ExtenderType.None)
			{
				return 3;
			}
			if (ext != SimpleCollator.ExtenderType.Conditional)
			{
				return 4;
			}
			return 5;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00007390 File Offset: 0x00005590
		private int FilterExtender(int i, SimpleCollator.ExtenderType ext, CompareOptions opt)
		{
			if (ext == SimpleCollator.ExtenderType.Conditional && MSCompatUnicodeTable.HasSpecialWeight((char)i))
			{
				bool flag = SimpleCollator.IsHalfKana((int)((ushort)i), opt);
				bool flag2 = !MSCompatUnicodeTable.IsHiragana((char)i);
				switch (this.Level1(i) & 7)
				{
				case 2:
					if (flag)
					{
						return 65393;
					}
					if (!flag2)
					{
						return 12354;
					}
					return 12450;
				case 3:
					if (flag)
					{
						return 65394;
					}
					if (!flag2)
					{
						return 12356;
					}
					return 12452;
				case 4:
					if (flag)
					{
						return 65395;
					}
					if (!flag2)
					{
						return 12358;
					}
					return 12454;
				case 5:
					if (flag)
					{
						return 65396;
					}
					if (!flag2)
					{
						return 12360;
					}
					return 12456;
				case 6:
					if (flag)
					{
						return 65397;
					}
					if (!flag2)
					{
						return 12362;
					}
					return 12458;
				}
			}
			return i;
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00007465 File Offset: 0x00005665
		private static bool IsIgnorable(int i, CompareOptions opt)
		{
			return MSCompatUnicodeTable.IsIgnorable(i, (byte)((((opt & (CompareOptions.OrdinalIgnoreCase | CompareOptions.Ordinal)) == CompareOptions.None) ? 1 : 0) + (((opt & CompareOptions.IgnoreSymbols) != CompareOptions.None) ? 2 : 0) + (((opt & CompareOptions.IgnoreNonSpace) != CompareOptions.None) ? 4 : 0)));
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000748F File Offset: 0x0000568F
		private bool IsSafe(int i)
		{
			return i / 8 >= this.unsafeFlags.Length || ((int)this.unsafeFlags[i / 8] & 1 << i % 8) == 0;
		}

		// Token: 0x060001CA RID: 458 RVA: 0x000074B6 File Offset: 0x000056B6
		public SortKey GetSortKey(string s)
		{
			return this.GetSortKey(s, CompareOptions.None);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x000074C0 File Offset: 0x000056C0
		public SortKey GetSortKey(string s, CompareOptions options)
		{
			return this.GetSortKey(s, 0, s.Length, options);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x000074D4 File Offset: 0x000056D4
		public SortKey GetSortKey(string s, int start, int length, CompareOptions options)
		{
			SortKeyBuffer sortKeyBuffer = new SortKeyBuffer(this.lcid);
			sortKeyBuffer.Initialize(options, this.lcid, s, this.frenchSort);
			int end = start + length;
			this.GetSortKey(s, start, end, sortKeyBuffer, options);
			return sortKeyBuffer.GetResultAndReset();
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00007518 File Offset: 0x00005718
		private unsafe void GetSortKey(string s, int start, int end, SortKeyBuffer buf, CompareOptions opt)
		{
			byte* ptr = stackalloc byte[(UIntPtr)4];
			this.ClearBuffer(ptr, 4);
			SimpleCollator.Context context = new SimpleCollator.Context(opt, null, null, null, null, ptr);
			for (int i = start; i < end; i++)
			{
				int num = (int)s[i];
				SimpleCollator.ExtenderType extenderType = this.GetExtenderType(num);
				if (extenderType != SimpleCollator.ExtenderType.None)
				{
					num = this.FilterExtender(context.PrevCode, extenderType, opt);
					if (num >= 0)
					{
						this.FillSortKeyRaw(num, extenderType, buf, opt);
					}
					else if (context.PrevSortKey != null)
					{
						byte* prevSortKey = context.PrevSortKey;
						buf.AppendNormal(*prevSortKey, prevSortKey[1], (prevSortKey[2] != 1) ? prevSortKey[2] : this.Level2(num, extenderType), (prevSortKey[3] != 1) ? prevSortKey[3] : MSCompatUnicodeTable.Level3(num));
					}
				}
				else if (!SimpleCollator.IsIgnorable(num, opt))
				{
					num = this.FilterOptions(num, opt);
					Contraction contraction = this.GetContraction(s, i, end);
					if (contraction != null)
					{
						if (contraction.Replacement != null)
						{
							this.GetSortKey(contraction.Replacement, 0, contraction.Replacement.Length, buf, opt);
						}
						else
						{
							byte* prevSortKey2 = context.PrevSortKey;
							for (int j = 0; j < contraction.SortKey.Length; j++)
							{
								prevSortKey2[j] = contraction.SortKey[j];
							}
							buf.AppendNormal(*prevSortKey2, prevSortKey2[1], (prevSortKey2[2] != 1) ? prevSortKey2[2] : this.Level2(num, extenderType), (prevSortKey2[3] != 1) ? prevSortKey2[3] : MSCompatUnicodeTable.Level3(num));
							context.PrevCode = -1;
						}
						i += contraction.Source.Length - 1;
					}
					else
					{
						if (!MSCompatUnicodeTable.IsIgnorableNonSpacing(num))
						{
							context.PrevCode = num;
						}
						this.FillSortKeyRaw(num, SimpleCollator.ExtenderType.None, buf, opt);
					}
				}
			}
		}

		// Token: 0x060001CE RID: 462 RVA: 0x000076DC File Offset: 0x000058DC
		private void FillSortKeyRaw(int i, SimpleCollator.ExtenderType ext, SortKeyBuffer buf, CompareOptions opt)
		{
			if (13312 <= i && i <= 19893)
			{
				int num = i - 13312;
				buf.AppendCJKExtension((byte)(16 + num / 254), (byte)(num % 254 + 2));
				return;
			}
			UnicodeCategory unicodeCategory = char.GetUnicodeCategory((char)i);
			if (unicodeCategory == UnicodeCategory.Surrogate)
			{
				this.FillSurrogateSortKeyRaw(i, buf);
				return;
			}
			if (unicodeCategory == UnicodeCategory.PrivateUse)
			{
				int num2 = i - 57344;
				buf.AppendNormal((byte)(229 + num2 / 254), (byte)(num2 % 254 + 2), 0, 0);
				return;
			}
			byte lv = this.Level2(i, ext);
			if (MSCompatUnicodeTable.HasSpecialWeight((char)i))
			{
				byte lv2 = this.Level1(i);
				buf.AppendKana(this.Category(i), lv2, lv, MSCompatUnicodeTable.Level3(i), MSCompatUnicodeTable.IsJapaneseSmallLetter((char)i), SimpleCollator.ToDashTypeValue(ext, opt), !MSCompatUnicodeTable.IsHiragana((char)i), SimpleCollator.IsHalfKana((int)((ushort)i), opt));
				if ((opt & CompareOptions.IgnoreNonSpace) == CompareOptions.None && ext == SimpleCollator.ExtenderType.Voiced)
				{
					buf.AppendNormal(1, 1, 1, 0);
					return;
				}
			}
			else
			{
				buf.AppendNormal(this.Category(i), this.Level1(i), lv, MSCompatUnicodeTable.Level3(i));
			}
		}

		// Token: 0x060001CF RID: 463 RVA: 0x000077E4 File Offset: 0x000059E4
		private void FillSurrogateSortKeyRaw(int i, SortKeyBuffer buf)
		{
			int num;
			int num2;
			byte b;
			if (i < 55360)
			{
				num = 55296;
				num2 = 65;
				b = ((i == 55296) ? 62 : 63);
			}
			else if (55360 <= i && i < 55424)
			{
				num = 55360;
				num2 = 242;
				b = 62;
			}
			else if (56192 <= i && i < 56320)
			{
				num = 56128;
				num2 = 254;
				b = 62;
			}
			else
			{
				num = 56074;
				num2 = 65;
				b = 63;
			}
			int num3 = i - num;
			buf.AppendNormal((byte)(num2 + num3 / 254), (byte)(num3 % 254 + 2), b, b);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00007888 File Offset: 0x00005A88
		public int Compare(string s1, string s2)
		{
			return this.Compare(s1, 0, s1.Length, s2, 0, s2.Length, CompareOptions.None);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x000078A1 File Offset: 0x00005AA1
		int ISimpleCollator.Compare(string s1, int idx1, int len1, string s2, int idx2, int len2, CompareOptions options)
		{
			return this.Compare(s1, idx1, len1, s2, idx2, len2, options);
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x000078B4 File Offset: 0x00005AB4
		internal unsafe int Compare(string s1, int idx1, int len1, string s2, int idx2, int len2, CompareOptions options)
		{
			byte* ptr = stackalloc byte[(UIntPtr)4];
			byte* ptr2 = stackalloc byte[(UIntPtr)4];
			this.ClearBuffer(ptr, 4);
			this.ClearBuffer(ptr2, 4);
			SimpleCollator.Context context = new SimpleCollator.Context(options, null, null, ptr, ptr2, null);
			bool flag;
			bool flag2;
			int num = this.CompareInternal(s1, idx1, len1, s2, idx2, len2, out flag, out flag2, true, false, ref context);
			if (num == 0)
			{
				return 0;
			}
			if (num >= 0)
			{
				return 1;
			}
			return -1;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00007914 File Offset: 0x00005B14
		private unsafe void ClearBuffer(byte* buffer, int size)
		{
			for (int i = 0; i < size; i++)
			{
				buffer[i] = 0;
			}
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00007934 File Offset: 0x00005B34
		private unsafe int CompareInternal(string s1, int idx1, int len1, string s2, int idx2, int len2, out bool targetConsumed, out bool sourceConsumed, bool skipHeadingExtenders, bool immediateBreakup, ref SimpleCollator.Context ctx)
		{
			CompareOptions option = ctx.Option;
			int num = idx1;
			int num2 = idx2;
			int num3 = idx1 + len1;
			int num4 = idx2 + len2;
			targetConsumed = false;
			sourceConsumed = false;
			SimpleCollator.PreviousInfo previousInfo = new SimpleCollator.PreviousInfo(false);
			int num5 = 0;
			int num6 = 5;
			int num7 = -1;
			int num8 = -1;
			int num9 = 0;
			int num10 = 0;
			if (skipHeadingExtenders)
			{
				while (idx1 < num3)
				{
					if (this.GetExtenderType((int)s1[idx1]) == SimpleCollator.ExtenderType.None)
					{
						IL_71:
						while (idx2 < num4 && this.GetExtenderType((int)s2[idx2]) != SimpleCollator.ExtenderType.None)
						{
							idx2++;
						}
						goto IL_77;
					}
					idx1++;
				}
				goto IL_71;
			}
			IL_77:
			SimpleCollator.ExtenderType extenderType = SimpleCollator.ExtenderType.None;
			SimpleCollator.ExtenderType extenderType2 = SimpleCollator.ExtenderType.None;
			int num11 = idx1;
			int num12 = idx2;
			bool flag = (option & CompareOptions.StringSort) > CompareOptions.None;
			bool flag2 = (option & CompareOptions.IgnoreNonSpace) > CompareOptions.None;
			SimpleCollator.Escape escape = default(SimpleCollator.Escape);
			SimpleCollator.Escape escape2 = default(SimpleCollator.Escape);
			int num20;
			for (;;)
			{
				if (idx1 < num3)
				{
					if (SimpleCollator.IsIgnorable((int)s1[idx1], option))
					{
						idx1++;
						continue;
					}
				}
				while (idx2 < num4 && SimpleCollator.IsIgnorable((int)s2[idx2], option))
				{
					idx2++;
				}
				if (idx1 >= num3)
				{
					if (escape.Source == null)
					{
						goto IL_882;
					}
					s1 = escape.Source;
					num = escape.Start;
					idx1 = escape.Index;
					num3 = escape.End;
					num11 = escape.Optional;
					escape.Source = null;
				}
				else if (idx2 >= num4)
				{
					if (escape2.Source == null)
					{
						goto IL_882;
					}
					s2 = escape2.Source;
					num2 = escape2.Start;
					idx2 = escape2.Index;
					num4 = escape2.End;
					num12 = escape2.Optional;
					escape2.Source = null;
				}
				else
				{
					if (num11 < idx1 && num12 < idx2)
					{
						while (idx1 < num3 && idx2 < num4 && s1[idx1] == s2[idx2])
						{
							idx1++;
							idx2++;
						}
						if (idx1 != num3 && idx2 != num4)
						{
							int num13 = num11;
							int num14 = num12;
							num11 = idx1;
							num12 = idx2;
							idx1--;
							idx2--;
							while (idx1 > num13)
							{
								if (this.Category((int)s1[idx1]) != 1)
								{
									IL_20B:
									while (idx2 > num14)
									{
										if (this.Category((int)s2[idx2]) != 1)
										{
											IL_227:
											while (idx1 > num13)
											{
												if (this.IsSafe((int)s1[idx1]))
												{
													IL_245:
													while (idx2 > num14 && !this.IsSafe((int)s2[idx2]))
													{
														idx2--;
													}
													goto IL_24B;
												}
												idx1--;
											}
											goto IL_245;
										}
										idx2--;
									}
									goto IL_227;
								}
								idx1--;
							}
							goto IL_20B;
						}
						continue;
					}
					IL_24B:
					int num15 = idx1;
					int num16 = idx2;
					byte* ptr = null;
					byte* ptr2 = null;
					int num17 = this.FilterOptions((int)s1[idx1], option);
					int num18 = this.FilterOptions((int)s2[idx2], option);
					bool flag3 = false;
					bool flag4 = false;
					extenderType = this.GetExtenderType(num17);
					if (extenderType != SimpleCollator.ExtenderType.None)
					{
						if (ctx.PrevCode < 0)
						{
							if (ctx.PrevSortKey == null)
							{
								idx1++;
								continue;
							}
							ptr = ctx.PrevSortKey;
						}
						else
						{
							num17 = this.FilterExtender(ctx.PrevCode, extenderType, option);
						}
					}
					extenderType2 = this.GetExtenderType(num18);
					if (extenderType2 != SimpleCollator.ExtenderType.None)
					{
						if (previousInfo.Code < 0)
						{
							if (previousInfo.SortKey == null)
							{
								idx2++;
								continue;
							}
							ptr2 = previousInfo.SortKey;
						}
						else
						{
							num18 = this.FilterExtender(previousInfo.Code, extenderType2, option);
						}
					}
					byte b = this.Category(num17);
					byte b2 = this.Category(num18);
					if (b == 6)
					{
						if (!flag && num6 == 5)
						{
							num7 = ((escape.Source != null) ? (escape.Index - escape.Start) : (num15 - num));
							num9 = (int)this.Level1(num17) << (int)(8 + MSCompatUnicodeTable.Level3(num17));
						}
						ctx.PrevCode = num17;
						idx1++;
					}
					if (b2 == 6)
					{
						if (!flag && num6 == 5)
						{
							num8 = ((escape2.Source != null) ? (escape2.Index - escape2.Start) : (num16 - num2));
							num10 = (int)this.Level1(num18) << (int)(8 + MSCompatUnicodeTable.Level3(num18));
						}
						previousInfo.Code = num18;
						idx2++;
					}
					if (b == 6 || b2 == 6)
					{
						if (num6 == 5)
						{
							if (num9 == num10)
							{
								num8 = (num7 = -1);
								num10 = (num9 = 0);
							}
							else
							{
								num6 = 4;
							}
						}
					}
					else
					{
						Contraction contraction = null;
						if (extenderType == SimpleCollator.ExtenderType.None)
						{
							contraction = this.GetContraction(s1, idx1, num3);
						}
						int num19 = 1;
						if (ptr != null)
						{
							num19 = 1;
						}
						else if (contraction != null)
						{
							num19 = contraction.Source.Length;
							if (contraction.SortKey != null)
							{
								ptr = ctx.Buffer1;
								for (int i = 0; i < contraction.SortKey.Length; i++)
								{
									ptr[i] = contraction.SortKey[i];
								}
								ctx.PrevCode = -1;
								ctx.PrevSortKey = ptr;
							}
							else if (escape.Source == null)
							{
								escape.Source = s1;
								escape.Start = num;
								escape.Index = num15 + contraction.Source.Length;
								escape.End = num3;
								escape.Optional = num11;
								s1 = contraction.Replacement;
								idx1 = 0;
								num = 0;
								num3 = s1.Length;
								num11 = 0;
								continue;
							}
						}
						else
						{
							ptr = ctx.Buffer1;
							*ptr = b;
							ptr[1] = this.Level1(num17);
							if (!flag2 && num6 > 1)
							{
								ptr[2] = this.Level2(num17, extenderType);
							}
							if (num6 > 2)
							{
								ptr[3] = MSCompatUnicodeTable.Level3(num17);
							}
							if (num6 > 3)
							{
								flag3 = MSCompatUnicodeTable.HasSpecialWeight((char)num17);
							}
							if (b > 1)
							{
								ctx.PrevCode = num17;
							}
						}
						Contraction contraction2 = null;
						if (extenderType2 == SimpleCollator.ExtenderType.None)
						{
							contraction2 = this.GetContraction(s2, idx2, num4);
						}
						if (ptr2 != null)
						{
							idx2++;
						}
						else if (contraction2 != null)
						{
							idx2 += contraction2.Source.Length;
							if (contraction2.SortKey != null)
							{
								ptr2 = ctx.Buffer2;
								for (int j = 0; j < contraction2.SortKey.Length; j++)
								{
									ptr2[j] = contraction2.SortKey[j];
								}
								previousInfo.Code = -1;
								previousInfo.SortKey = ptr2;
							}
							else if (escape2.Source == null)
							{
								escape2.Source = s2;
								escape2.Start = num2;
								escape2.Index = num16 + contraction2.Source.Length;
								escape2.End = num4;
								escape2.Optional = num12;
								s2 = contraction2.Replacement;
								idx2 = 0;
								num2 = 0;
								num4 = s2.Length;
								num12 = 0;
								continue;
							}
						}
						else
						{
							ptr2 = ctx.Buffer2;
							*ptr2 = b2;
							ptr2[1] = this.Level1(num18);
							if (!flag2 && num6 > 1)
							{
								ptr2[2] = this.Level2(num18, extenderType2);
							}
							if (num6 > 2)
							{
								ptr2[3] = MSCompatUnicodeTable.Level3(num18);
							}
							if (num6 > 3)
							{
								flag4 = MSCompatUnicodeTable.HasSpecialWeight((char)num18);
							}
							if (b2 > 1)
							{
								previousInfo.Code = num18;
							}
							idx2++;
						}
						idx1 += num19;
						if (!flag2)
						{
							while (idx1 < num3)
							{
								if (this.Category((int)s1[idx1]) != 1)
								{
									IL_72B:
									while (idx2 < num4 && this.Category((int)s2[idx2]) == 1)
									{
										if (ptr2[2] == 0)
										{
											ptr2[2] = 2;
										}
										ptr2[2] = ptr2[2] + this.Level2((int)s2[idx2], SimpleCollator.ExtenderType.None);
										idx2++;
									}
									goto IL_731;
								}
								if (ptr[2] == 0)
								{
									ptr[2] = 2;
								}
								ptr[2] = ptr[2] + this.Level2((int)s1[idx1], SimpleCollator.ExtenderType.None);
								idx1++;
							}
							goto IL_72B;
						}
						IL_731:
						num20 = (int)(*ptr - *ptr2);
						num20 = ((num20 != 0) ? num20 : ((int)(ptr[1] - ptr2[1])));
						if (num20 != 0)
						{
							break;
						}
						if (num6 != 1)
						{
							if (!flag2)
							{
								num20 = (int)(ptr[2] - ptr2[2]);
								if (num20 != 0)
								{
									num5 = num20;
									if (immediateBreakup)
									{
										return -1;
									}
									num6 = (this.frenchSort ? 2 : 1);
									continue;
								}
							}
							if (num6 != 2)
							{
								num20 = (int)(ptr[3] - ptr2[3]);
								if (num20 != 0)
								{
									num5 = num20;
									if (immediateBreakup)
									{
										return -1;
									}
									num6 = 2;
								}
								else if (num6 != 3)
								{
									if (flag3 != flag4)
									{
										if (immediateBreakup)
										{
											return -1;
										}
										num5 = (flag3 ? 1 : -1);
										num6 = 3;
									}
									else if (flag3)
									{
										num20 = this.CompareFlagPair(!MSCompatUnicodeTable.IsJapaneseSmallLetter((char)num17), !MSCompatUnicodeTable.IsJapaneseSmallLetter((char)num18));
										num20 = ((num20 != 0) ? num20 : ((int)(SimpleCollator.ToDashTypeValue(extenderType, option) - SimpleCollator.ToDashTypeValue(extenderType2, option))));
										num20 = ((num20 != 0) ? num20 : this.CompareFlagPair(MSCompatUnicodeTable.IsHiragana((char)num17), MSCompatUnicodeTable.IsHiragana((char)num18)));
										num20 = ((num20 != 0) ? num20 : this.CompareFlagPair(!SimpleCollator.IsHalfKana((int)((ushort)num17), option), !SimpleCollator.IsHalfKana((int)((ushort)num18), option)));
										if (num20 != 0)
										{
											if (immediateBreakup)
											{
												return -1;
											}
											num5 = num20;
											num6 = 3;
										}
									}
								}
							}
						}
					}
				}
			}
			return num20;
			IL_882:
			if (!flag2 && num5 != 0 && num6 > 2)
			{
				while (idx1 < num3 && idx2 < num4 && MSCompatUnicodeTable.IsIgnorableNonSpacing((int)s1[idx1]) && MSCompatUnicodeTable.IsIgnorableNonSpacing((int)s2[idx2]))
				{
					num5 = (int)(this.Level2(this.FilterOptions((int)s1[idx1], option), extenderType) - this.Level2(this.FilterOptions((int)s2[idx2], option), extenderType2));
					if (num5 != 0)
					{
						break;
					}
					idx1++;
					idx2++;
					extenderType = SimpleCollator.ExtenderType.None;
					extenderType2 = SimpleCollator.ExtenderType.None;
				}
			}
			if (num6 == 1 && num5 != 0)
			{
				while (idx1 < num3)
				{
					if (!MSCompatUnicodeTable.IsIgnorableNonSpacing((int)s1[idx1]))
					{
						IL_939:
						while (idx2 < num4 && MSCompatUnicodeTable.IsIgnorableNonSpacing((int)s2[idx2]))
						{
							idx2++;
						}
						goto IL_93F;
					}
					idx1++;
				}
				goto IL_939;
			}
			IL_93F:
			if (num5 == 0)
			{
				if (num7 < 0 && num8 >= 0)
				{
					num5 = -1;
				}
				else if (num8 < 0 && num7 >= 0)
				{
					num5 = 1;
				}
				else
				{
					num5 = num7 - num8;
					if (num5 == 0)
					{
						num5 = num9 - num10;
					}
				}
			}
			if (num5 == 0)
			{
				if (idx2 == num4)
				{
					targetConsumed = true;
				}
				if (idx1 == num3)
				{
					sourceConsumed = true;
				}
			}
			if (idx1 != num3)
			{
				return 1;
			}
			if (idx2 != num4)
			{
				return -1;
			}
			return num5;
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x000082DA File Offset: 0x000064DA
		private int CompareFlagPair(bool b1, bool b2)
		{
			if (b1 == b2)
			{
				return 0;
			}
			if (!b1)
			{
				return -1;
			}
			return 1;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x000082E8 File Offset: 0x000064E8
		public bool IsPrefix(string src, string target, CompareOptions opt)
		{
			return this.IsPrefix(src, target, 0, src.Length, opt);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x000082FC File Offset: 0x000064FC
		public unsafe bool IsPrefix(string s, string target, int start, int length, CompareOptions opt)
		{
			if (target.Length == 0)
			{
				return true;
			}
			byte* ptr = stackalloc byte[(UIntPtr)4];
			byte* ptr2 = stackalloc byte[(UIntPtr)4];
			this.ClearBuffer(ptr, 4);
			this.ClearBuffer(ptr2, 4);
			SimpleCollator.Context context = new SimpleCollator.Context(opt, null, null, ptr, ptr2, null);
			return this.IsPrefix(s, target, start, length, true, ref context);
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000834C File Offset: 0x0000654C
		private bool IsPrefix(string s, string target, int start, int length, bool skipHeadingExtenders, ref SimpleCollator.Context ctx)
		{
			bool result;
			bool flag;
			this.CompareInternal(s, start, length, target, 0, target.Length, out result, out flag, skipHeadingExtenders, true, ref ctx);
			return result;
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00008376 File Offset: 0x00006576
		public bool IsSuffix(string src, string target, CompareOptions opt)
		{
			return this.IsSuffix(src, target, src.Length - 1, src.Length, opt);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00008390 File Offset: 0x00006590
		public bool IsSuffix(string s, string target, int start, int length, CompareOptions opt)
		{
			if (target.Length == 0)
			{
				return true;
			}
			int num = this.LastIndexOf(s, target, start, length, opt);
			return num >= 0 && this.Compare(s, num, s.Length - num, target, 0, target.Length, opt) == 0;
		}

		// Token: 0x060001DB RID: 475 RVA: 0x000083D8 File Offset: 0x000065D8
		public int IndexOf(string s, string target, CompareOptions opt)
		{
			return this.IndexOf(s, target, 0, s.Length, opt);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x000083EC File Offset: 0x000065EC
		private int QuickIndexOf(string s, string target, int start, int length, out bool testWasUnable)
		{
			int num = -1;
			int num2 = -1;
			testWasUnable = true;
			if (target.Length == 0)
			{
				return 0;
			}
			if (target.Length > length)
			{
				return -1;
			}
			testWasUnable = false;
			int num3 = start + length - target.Length + 1;
			for (int i = start; i < num3; i++)
			{
				bool flag = false;
				for (int j = 0; j < target.Length; j++)
				{
					if (num2 < j)
					{
						char c = target[j];
						if (c == '\0' || c >= '\u0080')
						{
							testWasUnable = true;
							return -1;
						}
						num2 = j;
					}
					if (num < i + j)
					{
						char c2 = s[i + j];
						if (c2 == '\0' || c2 >= '\u0080')
						{
							testWasUnable = true;
							return -1;
						}
						num = i + j;
					}
					if (s[i + j] != target[j])
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060001DD RID: 477 RVA: 0x000084C4 File Offset: 0x000066C4
		public unsafe int IndexOf(string s, string target, int start, int length, CompareOptions opt)
		{
			if (opt == CompareOptions.Ordinal)
			{
				throw new NotSupportedException("Should not be reached");
			}
			if (opt == CompareOptions.OrdinalIgnoreCase)
			{
				throw new NotSupportedException("Should not be reached");
			}
			if (opt == CompareOptions.None)
			{
				bool flag;
				int result = this.QuickIndexOf(s, target, start, length, out flag);
				if (!flag)
				{
					return result;
				}
			}
			byte* ptr = stackalloc byte[(UIntPtr)16];
			byte* ptr2 = stackalloc byte[(UIntPtr)16];
			byte* ptr3 = stackalloc byte[(UIntPtr)4];
			byte* ptr4 = stackalloc byte[(UIntPtr)4];
			byte* ptr5 = stackalloc byte[(UIntPtr)4];
			this.ClearBuffer(ptr, 16);
			this.ClearBuffer(ptr2, 16);
			this.ClearBuffer(ptr3, 4);
			this.ClearBuffer(ptr4, 4);
			this.ClearBuffer(ptr5, 4);
			SimpleCollator.Context context = new SimpleCollator.Context(opt, ptr, ptr2, ptr4, ptr5, null);
			return this.IndexOf(s, target, start, length, ptr3, ref context);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00008578 File Offset: 0x00006778
		private int IndexOfOrdinal(string s, string target, int start, int length)
		{
			if (target.Length == 0)
			{
				return 0;
			}
			if (target.Length > length)
			{
				return -1;
			}
			int num = start + length - target.Length + 1;
			for (int i = start; i < num; i++)
			{
				bool flag = false;
				for (int j = 0; j < target.Length; j++)
				{
					if (s[i + j] != target[j])
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x000085E4 File Offset: 0x000067E4
		public int IndexOf(string s, char target, CompareOptions opt)
		{
			return this.IndexOf(s, target, 0, s.Length, opt);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x000085F8 File Offset: 0x000067F8
		public unsafe int IndexOf(string s, char target, int start, int length, CompareOptions opt)
		{
			if (opt == CompareOptions.Ordinal)
			{
				throw new NotSupportedException("Should not be reached");
			}
			if (opt == CompareOptions.OrdinalIgnoreCase)
			{
				throw new NotSupportedException("Should not be reached");
			}
			byte* ptr = stackalloc byte[(UIntPtr)16];
			byte* ptr2 = stackalloc byte[(UIntPtr)16];
			byte* ptr3 = stackalloc byte[(UIntPtr)4];
			byte* ptr4 = stackalloc byte[(UIntPtr)4];
			byte* ptr5 = stackalloc byte[(UIntPtr)4];
			this.ClearBuffer(ptr, 16);
			this.ClearBuffer(ptr2, 16);
			this.ClearBuffer(ptr3, 4);
			this.ClearBuffer(ptr4, 4);
			this.ClearBuffer(ptr5, 4);
			SimpleCollator.Context context = new SimpleCollator.Context(opt, ptr, ptr2, ptr4, ptr5, null);
			Contraction contraction = this.GetContraction(target);
			if (contraction == null)
			{
				int num = this.FilterOptions((int)target, opt);
				*ptr3 = this.Category(num);
				ptr3[1] = this.Level1(num);
				if ((opt & CompareOptions.IgnoreNonSpace) == CompareOptions.None)
				{
					ptr3[2] = this.Level2(num, SimpleCollator.ExtenderType.None);
				}
				ptr3[3] = MSCompatUnicodeTable.Level3(num);
				return this.IndexOfSortKey(s, start, length, ptr3, target, num, !MSCompatUnicodeTable.HasSpecialWeight((char)num), ref context);
			}
			if (contraction.Replacement != null)
			{
				return this.IndexOf(s, contraction.Replacement, start, length, ptr3, ref context);
			}
			for (int i = 0; i < contraction.SortKey.Length; i++)
			{
				ptr5[i] = contraction.SortKey[i];
			}
			return this.IndexOfSortKey(s, start, length, ptr5, '\0', -1, true, ref context);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00008744 File Offset: 0x00006944
		private int IndexOfOrdinal(string s, char target, int start, int length)
		{
			int num = start + length;
			for (int i = start; i < num; i++)
			{
				if (s[i] == target)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00008770 File Offset: 0x00006970
		private unsafe int IndexOfSortKey(string s, int start, int length, byte* sortkey, char target, int ti, bool noLv4, ref SimpleCollator.Context ctx)
		{
			int num = start + length;
			int i = start;
			while (i < num)
			{
				int result = i;
				if (this.MatchesForward(s, ref i, num, ti, sortkey, noLv4, ref ctx))
				{
					return result;
				}
			}
			return -1;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x000087A4 File Offset: 0x000069A4
		private unsafe int IndexOf(string s, string target, int start, int length, byte* targetSortKey, ref SimpleCollator.Context ctx)
		{
			CompareOptions option = ctx.Option;
			int num = 0;
			while (num < target.Length && SimpleCollator.IsIgnorable((int)target[num], option))
			{
				num++;
			}
			if (num != target.Length)
			{
				Contraction contraction = this.GetContraction(target, num, target.Length - num);
				string text = (contraction != null) ? contraction.Replacement : null;
				byte* ptr = (text == null) ? targetSortKey : null;
				bool noLv = true;
				char target2 = '\0';
				int num2 = -1;
				if (contraction != null && ptr != null)
				{
					for (int i = 0; i < contraction.SortKey.Length; i++)
					{
						ptr[i] = contraction.SortKey[i];
					}
				}
				else if (ptr != null)
				{
					target2 = target[num];
					num2 = this.FilterOptions((int)target[num], option);
					*ptr = this.Category(num2);
					ptr[1] = this.Level1(num2);
					if ((option & CompareOptions.IgnoreNonSpace) == CompareOptions.None)
					{
						ptr[2] = this.Level2(num2, SimpleCollator.ExtenderType.None);
					}
					ptr[3] = MSCompatUnicodeTable.Level3(num2);
					noLv = !MSCompatUnicodeTable.HasSpecialWeight((char)num2);
				}
				if (ptr != null)
				{
					num++;
					while (num < target.Length && this.Category((int)target[num]) == 1)
					{
						if (ptr[2] == 0)
						{
							ptr[2] = 2;
						}
						ptr[2] = ptr[2] + this.Level2((int)target[num], SimpleCollator.ExtenderType.None);
						num++;
					}
				}
				for (;;)
				{
					int num3;
					if (text != null)
					{
						num3 = this.IndexOf(s, text, start, length, targetSortKey, ref ctx);
					}
					else
					{
						num3 = this.IndexOfSortKey(s, start, length, ptr, target2, num2, noLv, ref ctx);
					}
					if (num3 < 0)
					{
						break;
					}
					length -= num3 - start;
					start = num3;
					if (this.IsPrefix(s, target, start, length, false, ref ctx))
					{
						return num3;
					}
					Contraction contraction2 = this.GetContraction(s, start, length);
					if (contraction2 != null)
					{
						start += contraction2.Source.Length;
						length -= contraction2.Source.Length;
					}
					else
					{
						start++;
						length--;
					}
					if (length <= 0)
					{
						return -1;
					}
				}
				return -1;
			}
			if (this.IndexOfOrdinal(target, '\0', 0, target.Length) < 0)
			{
				return start;
			}
			return this.IndexOfOrdinal(s, target, start, length);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x000089AF File Offset: 0x00006BAF
		public int LastIndexOf(string s, string target, CompareOptions opt)
		{
			return this.LastIndexOf(s, target, s.Length - 1, s.Length, opt);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x000089C8 File Offset: 0x00006BC8
		public unsafe int LastIndexOf(string s, string target, int start, int length, CompareOptions opt)
		{
			if (opt == CompareOptions.Ordinal)
			{
				throw new NotSupportedException("Should not be reached");
			}
			if (opt == CompareOptions.OrdinalIgnoreCase)
			{
				throw new NotSupportedException("Should not be reached");
			}
			byte* ptr = stackalloc byte[(UIntPtr)16];
			byte* ptr2 = stackalloc byte[(UIntPtr)16];
			byte* ptr3 = stackalloc byte[(UIntPtr)4];
			byte* ptr4 = stackalloc byte[(UIntPtr)4];
			byte* ptr5 = stackalloc byte[(UIntPtr)4];
			this.ClearBuffer(ptr, 16);
			this.ClearBuffer(ptr2, 16);
			this.ClearBuffer(ptr3, 4);
			this.ClearBuffer(ptr4, 4);
			this.ClearBuffer(ptr5, 4);
			SimpleCollator.Context context = new SimpleCollator.Context(opt, ptr, ptr2, ptr4, ptr5, null);
			return this.LastIndexOf(s, target, start, length, ptr3, ref context);
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00008A64 File Offset: 0x00006C64
		private int LastIndexOfOrdinal(string s, string target, int start, int length)
		{
			if (target.Length == 0)
			{
				return start;
			}
			if (s.Length < target.Length || target.Length > length)
			{
				return -1;
			}
			int num = start - length + target.Length - 1;
			char c = target[target.Length - 1];
			int i = start;
			while (i > num)
			{
				if (s[i] != c)
				{
					i--;
				}
				else
				{
					int num2 = i - target.Length + 1;
					i--;
					bool flag = false;
					for (int j = target.Length - 2; j >= 0; j--)
					{
						if (s[num2 + j] != target[j])
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return num2;
					}
				}
			}
			return -1;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00008B13 File Offset: 0x00006D13
		public int LastIndexOf(string s, char target, CompareOptions opt)
		{
			return this.LastIndexOf(s, target, s.Length - 1, s.Length, opt);
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00008B2C File Offset: 0x00006D2C
		public unsafe int LastIndexOf(string s, char target, int start, int length, CompareOptions opt)
		{
			if (opt == CompareOptions.Ordinal)
			{
				throw new NotSupportedException();
			}
			if (opt == CompareOptions.OrdinalIgnoreCase)
			{
				throw new NotSupportedException();
			}
			byte* ptr = stackalloc byte[(UIntPtr)16];
			byte* ptr2 = stackalloc byte[(UIntPtr)16];
			byte* ptr3 = stackalloc byte[(UIntPtr)4];
			byte* ptr4 = stackalloc byte[(UIntPtr)4];
			byte* ptr5 = stackalloc byte[(UIntPtr)4];
			this.ClearBuffer(ptr, 16);
			this.ClearBuffer(ptr2, 16);
			this.ClearBuffer(ptr3, 4);
			this.ClearBuffer(ptr4, 4);
			this.ClearBuffer(ptr5, 4);
			SimpleCollator.Context context = new SimpleCollator.Context(opt, ptr, ptr2, ptr4, ptr5, null);
			Contraction contraction = this.GetContraction(target);
			if (contraction == null)
			{
				int num = this.FilterOptions((int)target, opt);
				*ptr3 = this.Category(num);
				ptr3[1] = this.Level1(num);
				if ((opt & CompareOptions.IgnoreNonSpace) == CompareOptions.None)
				{
					ptr3[2] = this.Level2(num, SimpleCollator.ExtenderType.None);
				}
				ptr3[3] = MSCompatUnicodeTable.Level3(num);
				return this.LastIndexOfSortKey(s, start, start, length, ptr3, num, !MSCompatUnicodeTable.HasSpecialWeight((char)num), ref context);
			}
			if (contraction.Replacement != null)
			{
				return this.LastIndexOf(s, contraction.Replacement, start, length, ptr3, ref context);
			}
			for (int i = 0; i < contraction.SortKey.Length; i++)
			{
				ptr5[i] = contraction.SortKey[i];
			}
			return this.LastIndexOfSortKey(s, start, start, length, ptr5, -1, true, ref context);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00008C70 File Offset: 0x00006E70
		private unsafe int LastIndexOfSortKey(string s, int start, int orgStart, int length, byte* sortkey, int ti, bool noLv4, ref SimpleCollator.Context ctx)
		{
			int num = start - length;
			int i = start;
			while (i > num)
			{
				int result = i;
				if (this.MatchesBackward(s, ref i, num, orgStart, ti, sortkey, noLv4, ref ctx))
				{
					return result;
				}
			}
			return -1;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00008CA4 File Offset: 0x00006EA4
		private unsafe int LastIndexOf(string s, string target, int start, int length, byte* targetSortKey, ref SimpleCollator.Context ctx)
		{
			CompareOptions option = ctx.Option;
			int num = start;
			int num2 = 0;
			while (num2 < target.Length && SimpleCollator.IsIgnorable((int)target[num2], option))
			{
				num2++;
			}
			if (num2 != target.Length)
			{
				Contraction contraction = this.GetContraction(target, num2, target.Length - num2);
				string text = (contraction != null) ? contraction.Replacement : null;
				byte* ptr = (text == null) ? targetSortKey : null;
				bool noLv = true;
				int num3 = -1;
				if (contraction != null && ptr != null)
				{
					for (int i = 0; i < contraction.SortKey.Length; i++)
					{
						ptr[i] = contraction.SortKey[i];
					}
				}
				else if (ptr != null)
				{
					num3 = this.FilterOptions((int)target[num2], option);
					*ptr = this.Category(num3);
					ptr[1] = this.Level1(num3);
					if ((option & CompareOptions.IgnoreNonSpace) == CompareOptions.None)
					{
						ptr[2] = this.Level2(num3, SimpleCollator.ExtenderType.None);
					}
					ptr[3] = MSCompatUnicodeTable.Level3(num3);
					noLv = !MSCompatUnicodeTable.HasSpecialWeight((char)num3);
				}
				if (ptr != null)
				{
					num2++;
					while (num2 < target.Length && this.Category((int)target[num2]) == 1)
					{
						if (ptr[2] == 0)
						{
							ptr[2] = 2;
						}
						ptr[2] = ptr[2] + this.Level2((int)target[num2], SimpleCollator.ExtenderType.None);
						num2++;
					}
				}
				int num4;
				for (;;)
				{
					if (text != null)
					{
						num4 = this.LastIndexOf(s, text, start, length, targetSortKey, ref ctx);
					}
					else
					{
						num4 = this.LastIndexOfSortKey(s, start, num, length, ptr, num3, noLv, ref ctx);
					}
					if (num4 < 0)
					{
						break;
					}
					length -= start - num4;
					start = num4;
					if (this.IsPrefix(s, target, num4, num - num4 + 1, false, ref ctx))
					{
						goto Block_16;
					}
					Contraction contraction2 = this.GetContraction(s, num4, num - num4 + 1);
					if (contraction2 != null)
					{
						start -= contraction2.Source.Length;
						length -= contraction2.Source.Length;
					}
					else
					{
						start--;
						length--;
					}
					if (length <= 0)
					{
						return -1;
					}
				}
				return -1;
				Block_16:
				while (num4 < num && SimpleCollator.IsIgnorable((int)s[num4], option))
				{
					num4++;
				}
				return num4;
			}
			if (this.IndexOfOrdinal(target, '\0', 0, target.Length) < 0)
			{
				return start;
			}
			return this.LastIndexOfOrdinal(s, target, start, length);
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00008ED0 File Offset: 0x000070D0
		private unsafe bool MatchesForward(string s, ref int idx, int end, int ti, byte* sortkey, bool noLv4, ref SimpleCollator.Context ctx)
		{
			int num = (int)s[idx];
			if (ctx.AlwaysMatchFlags != null && num < 128 && ((int)ctx.AlwaysMatchFlags[num / 8] & 1 << num % 8) != 0)
			{
				return true;
			}
			if (ctx.NeverMatchFlags != null && num < 128 && ((int)ctx.NeverMatchFlags[num / 8] & 1 << num % 8) != 0)
			{
				idx++;
				return false;
			}
			SimpleCollator.ExtenderType extenderType = this.GetExtenderType((int)s[idx]);
			Contraction contraction = null;
			if (this.MatchesForwardCore(s, ref idx, end, ti, sortkey, noLv4, extenderType, ref contraction, ref ctx))
			{
				if (ctx.AlwaysMatchFlags != null && contraction == null && extenderType == SimpleCollator.ExtenderType.None && num < 128)
				{
					byte* ptr = ctx.AlwaysMatchFlags + num / 8;
					*ptr |= (byte)(1 << num % 8);
				}
				return true;
			}
			if (ctx.NeverMatchFlags != null && contraction == null && extenderType == SimpleCollator.ExtenderType.None && num < 128)
			{
				byte* ptr2 = ctx.NeverMatchFlags + num / 8;
				*ptr2 |= (byte)(1 << num % 8);
			}
			return false;
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00008FD4 File Offset: 0x000071D4
		private unsafe bool MatchesForwardCore(string s, ref int idx, int end, int ti, byte* sortkey, bool noLv4, SimpleCollator.ExtenderType ext, ref Contraction ct, ref SimpleCollator.Context ctx)
		{
			CompareOptions option = ctx.Option;
			byte* ptr = ctx.Buffer1;
			bool flag = (option & CompareOptions.IgnoreNonSpace) > CompareOptions.None;
			int num = -1;
			if (ext == SimpleCollator.ExtenderType.None)
			{
				ct = this.GetContraction(s, idx, end);
			}
			else if (ctx.PrevCode < 0)
			{
				if (ctx.PrevSortKey == null)
				{
					idx++;
					return false;
				}
				ptr = ctx.PrevSortKey;
			}
			else
			{
				num = this.FilterExtender(ctx.PrevCode, ext, option);
			}
			if (ct != null)
			{
				idx += ct.Source.Length;
				if (!noLv4)
				{
					return false;
				}
				if (ct.SortKey == null)
				{
					int num2 = 0;
					return this.MatchesForward(ct.Replacement, ref num2, ct.Replacement.Length, ti, sortkey, noLv4, ref ctx);
				}
				for (int i = 0; i < 4; i++)
				{
					ptr[i] = sortkey[i];
				}
				ctx.PrevCode = -1;
				ctx.PrevSortKey = ptr;
			}
			else
			{
				if (num < 0)
				{
					num = this.FilterOptions((int)s[idx], option);
				}
				idx++;
				*ptr = this.Category(num);
				bool flag2 = false;
				if (*sortkey == *ptr)
				{
					ptr[1] = this.Level1(num);
				}
				else
				{
					flag2 = true;
				}
				if (!flag && sortkey[1] == ptr[1])
				{
					ptr[2] = this.Level2(num, ext);
				}
				else if (!flag)
				{
					flag2 = true;
				}
				if (flag2)
				{
					while (idx < end && this.Category((int)s[idx]) == 1)
					{
						idx++;
					}
					return false;
				}
				ptr[3] = MSCompatUnicodeTable.Level3(num);
				if (*ptr != 1)
				{
					ctx.PrevCode = num;
				}
			}
			while (idx < end && this.Category((int)s[idx]) == 1)
			{
				if (!flag)
				{
					if (ptr[2] == 0)
					{
						ptr[2] = 2;
					}
					ptr[2] = ptr[2] + this.Level2((int)s[idx], SimpleCollator.ExtenderType.None);
				}
				idx++;
			}
			return this.MatchesPrimitive(option, ptr, num, ext, sortkey, ti, noLv4);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x000091B4 File Offset: 0x000073B4
		private unsafe bool MatchesPrimitive(CompareOptions opt, byte* source, int si, SimpleCollator.ExtenderType ext, byte* target, int ti, bool noLv4)
		{
			bool flag = (opt & CompareOptions.IgnoreNonSpace) > CompareOptions.None;
			return *source == *target && source[1] == target[1] && (flag || source[2] == target[2]) && source[3] == target[3] && ((noLv4 && (si < 0 || !MSCompatUnicodeTable.HasSpecialWeight((char)si))) || (!noLv4 && (flag || ext != SimpleCollator.ExtenderType.Conditional) && MSCompatUnicodeTable.IsJapaneseSmallLetter((char)si) == MSCompatUnicodeTable.IsJapaneseSmallLetter((char)ti) && SimpleCollator.ToDashTypeValue(ext, opt) == SimpleCollator.ToDashTypeValue(SimpleCollator.ExtenderType.None, opt) && !MSCompatUnicodeTable.IsHiragana((char)si) == !MSCompatUnicodeTable.IsHiragana((char)ti) && SimpleCollator.IsHalfKana((int)((ushort)si), opt) == SimpleCollator.IsHalfKana((int)((ushort)ti), opt)));
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00009268 File Offset: 0x00007468
		private unsafe bool MatchesBackward(string s, ref int idx, int end, int orgStart, int ti, byte* sortkey, bool noLv4, ref SimpleCollator.Context ctx)
		{
			int num = (int)s[idx];
			if (ctx.AlwaysMatchFlags != null && num < 128 && ((int)ctx.AlwaysMatchFlags[num / 8] & 1 << num % 8) != 0)
			{
				return true;
			}
			if (ctx.NeverMatchFlags != null && num < 128 && ((int)ctx.NeverMatchFlags[num / 8] & 1 << num % 8) != 0)
			{
				idx--;
				return false;
			}
			SimpleCollator.ExtenderType extenderType = this.GetExtenderType((int)s[idx]);
			Contraction contraction = null;
			if (this.MatchesBackwardCore(s, ref idx, end, orgStart, ti, sortkey, noLv4, extenderType, ref contraction, ref ctx))
			{
				if (ctx.AlwaysMatchFlags != null && contraction == null && extenderType == SimpleCollator.ExtenderType.None && num < 128)
				{
					byte* ptr = ctx.AlwaysMatchFlags + num / 8;
					*ptr |= (byte)(1 << num % 8);
				}
				return true;
			}
			if (ctx.NeverMatchFlags != null && contraction == null && extenderType == SimpleCollator.ExtenderType.None && num < 128)
			{
				byte* ptr2 = ctx.NeverMatchFlags + num / 8;
				*ptr2 |= (byte)(1 << num % 8);
			}
			return false;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000936C File Offset: 0x0000756C
		private unsafe bool MatchesBackwardCore(string s, ref int idx, int end, int orgStart, int ti, byte* sortkey, bool noLv4, SimpleCollator.ExtenderType ext, ref Contraction ct, ref SimpleCollator.Context ctx)
		{
			CompareOptions option = ctx.Option;
			byte* buffer = ctx.Buffer1;
			bool flag = (option & CompareOptions.IgnoreNonSpace) > CompareOptions.None;
			int num = idx;
			int num2 = -1;
			if (ext != SimpleCollator.ExtenderType.None)
			{
				byte b = 0;
				for (int i = idx; i >= 0; i--)
				{
					if (!SimpleCollator.IsIgnorable((int)s[i], option))
					{
						int num3 = this.FilterOptions((int)s[i], option);
						byte b2 = this.Category(num3);
						if (b2 != 1)
						{
							num2 = this.FilterExtender(num3, ext, option);
							*buffer = b2;
							buffer[1] = this.Level1(num2);
							if (!flag)
							{
								buffer[2] = this.Level2(num2, ext);
							}
							buffer[3] = MSCompatUnicodeTable.Level3(num2);
							if (ext != SimpleCollator.ExtenderType.Conditional && b != 0)
							{
								buffer[2] = ((buffer[2] == 0) ? (b + 2) : b);
							}
							idx--;
							goto IL_DA;
						}
						b = this.Level2(num3, SimpleCollator.ExtenderType.None);
					}
				}
				return false;
			}
			IL_DA:
			if (ext == SimpleCollator.ExtenderType.None)
			{
				ct = this.GetTailContraction(s, idx, end);
			}
			if (ct != null)
			{
				idx -= ct.Source.Length;
				if (!noLv4)
				{
					return false;
				}
				if (ct.SortKey == null)
				{
					int num4 = ct.Replacement.Length - 1;
					return 0 <= this.LastIndexOfSortKey(ct.Replacement, num4, num4, ct.Replacement.Length, sortkey, ti, noLv4, ref ctx);
				}
				for (int j = 0; j < 4; j++)
				{
					buffer[j] = sortkey[j];
				}
				ctx.PrevCode = -1;
				ctx.PrevSortKey = buffer;
			}
			else if (ext == SimpleCollator.ExtenderType.None)
			{
				if (num2 < 0)
				{
					num2 = this.FilterOptions((int)s[idx], option);
				}
				idx--;
				bool flag2 = false;
				*buffer = this.Category(num2);
				if (*buffer == *sortkey)
				{
					buffer[1] = this.Level1(num2);
				}
				else
				{
					flag2 = true;
				}
				if (!flag && buffer[1] == sortkey[1])
				{
					buffer[2] = this.Level2(num2, ext);
				}
				else if (!flag)
				{
					flag2 = true;
				}
				if (flag2)
				{
					return false;
				}
				buffer[3] = MSCompatUnicodeTable.Level3(num2);
				if (*buffer != 1)
				{
					ctx.PrevCode = num2;
				}
			}
			if (ext == SimpleCollator.ExtenderType.None)
			{
				int num5 = num + 1;
				while (num5 < orgStart && this.Category((int)s[num5]) == 1)
				{
					if (!flag)
					{
						if (buffer[2] == 0)
						{
							buffer[2] = 2;
						}
						buffer[2] = buffer[2] + this.Level2((int)s[num5], SimpleCollator.ExtenderType.None);
					}
					num5++;
				}
			}
			return this.MatchesPrimitive(option, buffer, num2, ext, sortkey, ti, noLv4);
		}

		// Token: 0x04000E5F RID: 3679
		private static SimpleCollator invariant = new SimpleCollator(CultureInfo.InvariantCulture);

		// Token: 0x04000E60 RID: 3680
		private readonly TextInfo textInfo;

		// Token: 0x04000E61 RID: 3681
		private readonly CodePointIndexer cjkIndexer;

		// Token: 0x04000E62 RID: 3682
		private readonly Contraction[] contractions;

		// Token: 0x04000E63 RID: 3683
		private readonly Level2Map[] level2Maps;

		// Token: 0x04000E64 RID: 3684
		private readonly byte[] unsafeFlags;

		// Token: 0x04000E65 RID: 3685
		private unsafe readonly byte* cjkCatTable;

		// Token: 0x04000E66 RID: 3686
		private unsafe readonly byte* cjkLv1Table;

		// Token: 0x04000E67 RID: 3687
		private unsafe readonly byte* cjkLv2Table;

		// Token: 0x04000E68 RID: 3688
		private readonly CodePointIndexer cjkLv2Indexer;

		// Token: 0x04000E69 RID: 3689
		private readonly int lcid;

		// Token: 0x04000E6A RID: 3690
		private readonly bool frenchSort;

		// Token: 0x04000E6B RID: 3691
		private const int UnsafeFlagLength = 96;

		// Token: 0x02000075 RID: 117
		internal struct Context
		{
			// Token: 0x060001F1 RID: 497 RVA: 0x000095F7 File Offset: 0x000077F7
			public unsafe Context(CompareOptions opt, byte* alwaysMatchFlags, byte* neverMatchFlags, byte* buffer1, byte* buffer2, byte* prev1)
			{
				this.Option = opt;
				this.AlwaysMatchFlags = alwaysMatchFlags;
				this.NeverMatchFlags = neverMatchFlags;
				this.Buffer1 = buffer1;
				this.Buffer2 = buffer2;
				this.PrevSortKey = prev1;
				this.PrevCode = -1;
			}

			// Token: 0x060001F2 RID: 498 RVA: 0x0000962D File Offset: 0x0000782D
			public void ClearPrevInfo()
			{
				this.PrevCode = -1;
				this.PrevSortKey = null;
			}

			// Token: 0x04000E6C RID: 3692
			public readonly CompareOptions Option;

			// Token: 0x04000E6D RID: 3693
			public unsafe readonly byte* NeverMatchFlags;

			// Token: 0x04000E6E RID: 3694
			public unsafe readonly byte* AlwaysMatchFlags;

			// Token: 0x04000E6F RID: 3695
			public unsafe byte* Buffer1;

			// Token: 0x04000E70 RID: 3696
			public unsafe byte* Buffer2;

			// Token: 0x04000E71 RID: 3697
			public int PrevCode;

			// Token: 0x04000E72 RID: 3698
			public unsafe byte* PrevSortKey;
		}

		// Token: 0x02000076 RID: 118
		private struct PreviousInfo
		{
			// Token: 0x060001F3 RID: 499 RVA: 0x0000963E File Offset: 0x0000783E
			public PreviousInfo(bool dummy)
			{
				this.Code = -1;
				this.SortKey = null;
			}

			// Token: 0x04000E73 RID: 3699
			public int Code;

			// Token: 0x04000E74 RID: 3700
			public unsafe byte* SortKey;
		}

		// Token: 0x02000077 RID: 119
		private struct Escape
		{
			// Token: 0x04000E75 RID: 3701
			public string Source;

			// Token: 0x04000E76 RID: 3702
			public int Index;

			// Token: 0x04000E77 RID: 3703
			public int Start;

			// Token: 0x04000E78 RID: 3704
			public int End;

			// Token: 0x04000E79 RID: 3705
			public int Optional;
		}

		// Token: 0x02000078 RID: 120
		private enum ExtenderType
		{
			// Token: 0x04000E7B RID: 3707
			None,
			// Token: 0x04000E7C RID: 3708
			Simple,
			// Token: 0x04000E7D RID: 3709
			Voiced,
			// Token: 0x04000E7E RID: 3710
			Conditional,
			// Token: 0x04000E7F RID: 3711
			Buggy
		}
	}
}
