using System;

namespace System.Buffers
{
	// Token: 0x02000AED RID: 2797
	public readonly struct StandardFormat : IEquatable<StandardFormat>
	{
		// Token: 0x17001196 RID: 4502
		// (get) Token: 0x0600636D RID: 25453 RVA: 0x0014CA17 File Offset: 0x0014AC17
		public char Symbol
		{
			get
			{
				return (char)this._format;
			}
		}

		// Token: 0x17001197 RID: 4503
		// (get) Token: 0x0600636E RID: 25454 RVA: 0x0014CA1F File Offset: 0x0014AC1F
		public byte Precision
		{
			get
			{
				return this._precision;
			}
		}

		// Token: 0x17001198 RID: 4504
		// (get) Token: 0x0600636F RID: 25455 RVA: 0x0014CA27 File Offset: 0x0014AC27
		public bool HasPrecision
		{
			get
			{
				return this._precision != byte.MaxValue;
			}
		}

		// Token: 0x17001199 RID: 4505
		// (get) Token: 0x06006370 RID: 25456 RVA: 0x0014CA39 File Offset: 0x0014AC39
		public bool IsDefault
		{
			get
			{
				return this._format == 0 && this._precision == 0;
			}
		}

		// Token: 0x06006371 RID: 25457 RVA: 0x0014CA4E File Offset: 0x0014AC4E
		public StandardFormat(char symbol, byte precision = 255)
		{
			if (precision != 255 && precision > 99)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException_PrecisionTooLarge();
			}
			if (symbol != (char)((byte)symbol))
			{
				ThrowHelper.ThrowArgumentOutOfRangeException_SymbolDoesNotFit();
			}
			this._format = (byte)symbol;
			this._precision = precision;
		}

		// Token: 0x06006372 RID: 25458 RVA: 0x0014CA7B File Offset: 0x0014AC7B
		public static implicit operator StandardFormat(char symbol)
		{
			return new StandardFormat(symbol, byte.MaxValue);
		}

		// Token: 0x06006373 RID: 25459 RVA: 0x0014CA88 File Offset: 0x0014AC88
		public static StandardFormat Parse(ReadOnlySpan<char> format)
		{
			StandardFormat result;
			StandardFormat.ParseHelper(format, out result, true);
			return result;
		}

		// Token: 0x06006374 RID: 25460 RVA: 0x0014CAA0 File Offset: 0x0014ACA0
		public static StandardFormat Parse(string format)
		{
			if (format != null)
			{
				return StandardFormat.Parse(format.AsSpan());
			}
			return default(StandardFormat);
		}

		// Token: 0x06006375 RID: 25461 RVA: 0x0014CAC5 File Offset: 0x0014ACC5
		public static bool TryParse(ReadOnlySpan<char> format, out StandardFormat result)
		{
			return StandardFormat.ParseHelper(format, out result, false);
		}

		// Token: 0x06006376 RID: 25462 RVA: 0x0014CAD0 File Offset: 0x0014ACD0
		private unsafe static bool ParseHelper(ReadOnlySpan<char> format, out StandardFormat standardFormat, bool throws = false)
		{
			standardFormat = default(StandardFormat);
			if (format.Length == 0)
			{
				return true;
			}
			char symbol = (char)(*format[0]);
			byte precision;
			if (format.Length == 1)
			{
				precision = byte.MaxValue;
			}
			else
			{
				uint num = 0U;
				int i = 1;
				while (i < format.Length)
				{
					uint num2 = (uint)(*format[i] - 48);
					if (num2 > 9U)
					{
						if (!throws)
						{
							return false;
						}
						throw new FormatException(SR.Format("Characters following the format symbol must be a number of {0} or less.", 99));
					}
					else
					{
						num = num * 10U + num2;
						if (num > 99U)
						{
							if (!throws)
							{
								return false;
							}
							throw new FormatException(SR.Format("Precision cannot be larger than {0}.", 99));
						}
						else
						{
							i++;
						}
					}
				}
				precision = (byte)num;
			}
			standardFormat = new StandardFormat(symbol, precision);
			return true;
		}

		// Token: 0x06006377 RID: 25463 RVA: 0x0014CB8C File Offset: 0x0014AD8C
		public override bool Equals(object obj)
		{
			if (obj is StandardFormat)
			{
				StandardFormat other = (StandardFormat)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x06006378 RID: 25464 RVA: 0x0014CBB1 File Offset: 0x0014ADB1
		public override int GetHashCode()
		{
			return this._format.GetHashCode() ^ this._precision.GetHashCode();
		}

		// Token: 0x06006379 RID: 25465 RVA: 0x0014CBCA File Offset: 0x0014ADCA
		public bool Equals(StandardFormat other)
		{
			return this._format == other._format && this._precision == other._precision;
		}

		// Token: 0x0600637A RID: 25466 RVA: 0x0014CBEC File Offset: 0x0014ADEC
		public unsafe override string ToString()
		{
			Span<char> destination = new Span<char>(stackalloc byte[(UIntPtr)6], 3);
			int length = this.Format(destination);
			return new string(destination.Slice(0, length));
		}

		// Token: 0x0600637B RID: 25467 RVA: 0x0014CC20 File Offset: 0x0014AE20
		internal unsafe int Format(Span<char> destination)
		{
			int num = 0;
			char symbol = this.Symbol;
			if (symbol != '\0' && destination.Length == 3)
			{
				*destination[0] = symbol;
				num = 1;
				uint precision = (uint)this.Precision;
				if (precision != 255U)
				{
					if (precision >= 10U)
					{
						uint num2 = Math.DivRem(precision, 10U, out precision);
						*destination[1] = (char)(48U + num2 % 10U);
						num = 2;
					}
					*destination[num] = (char)(48U + precision);
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600637C RID: 25468 RVA: 0x0014CC94 File Offset: 0x0014AE94
		public static bool operator ==(StandardFormat left, StandardFormat right)
		{
			return left.Equals(right);
		}

		// Token: 0x0600637D RID: 25469 RVA: 0x0014CC9E File Offset: 0x0014AE9E
		public static bool operator !=(StandardFormat left, StandardFormat right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04003A81 RID: 14977
		public const byte NoPrecision = 255;

		// Token: 0x04003A82 RID: 14978
		public const byte MaxPrecision = 99;

		// Token: 0x04003A83 RID: 14979
		private readonly byte _format;

		// Token: 0x04003A84 RID: 14980
		private readonly byte _precision;

		// Token: 0x04003A85 RID: 14981
		internal const int FormatStringLength = 3;
	}
}
