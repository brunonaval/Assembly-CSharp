﻿using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	/// <summary>A platform-specific type that is used to represent a pointer or a handle.</summary>
	// Token: 0x02000239 RID: 569
	[ComVisible(true)]
	[Serializable]
	public readonly struct IntPtr : ISerializable, IEquatable<IntPtr>
	{
		/// <summary>Initializes a new instance of <see cref="T:System.IntPtr" /> using the specified 32-bit pointer or handle.</summary>
		/// <param name="value">A pointer or handle contained in a 32-bit signed integer.</param>
		// Token: 0x060019F8 RID: 6648 RVA: 0x0005FCDC File Offset: 0x0005DEDC
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public IntPtr(int value)
		{
			this.m_value = value;
		}

		/// <summary>Initializes a new instance of <see cref="T:System.IntPtr" /> using the specified 64-bit pointer.</summary>
		/// <param name="value">A pointer or handle contained in a 64-bit signed integer.</param>
		/// <exception cref="T:System.OverflowException">On a 32-bit platform, <paramref name="value" /> is too large or too small to represent as an <see cref="T:System.IntPtr" />.</exception>
		// Token: 0x060019F9 RID: 6649 RVA: 0x0005FCE6 File Offset: 0x0005DEE6
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public IntPtr(long value)
		{
			this.m_value = value;
		}

		/// <summary>Initializes a new instance of <see cref="T:System.IntPtr" /> using the specified pointer to an unspecified type.</summary>
		/// <param name="value">A pointer to an unspecified type.</param>
		// Token: 0x060019FA RID: 6650 RVA: 0x0005FCF0 File Offset: 0x0005DEF0
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		[CLSCompliant(false)]
		public unsafe IntPtr(void* value)
		{
			this.m_value = value;
		}

		// Token: 0x060019FB RID: 6651 RVA: 0x0005FCFC File Offset: 0x0005DEFC
		private IntPtr(SerializationInfo info, StreamingContext context)
		{
			long @int = info.GetInt64("value");
			this.m_value = @int;
		}

		/// <summary>Gets the size of this instance.</summary>
		/// <returns>The size of a pointer or handle in this process, measured in bytes. The value of this property is 4 in a 32-bit process, and 8 in a 64-bit process. You can define the process type by setting the <see langword="/platform" /> switch when you compile your code with the C# and Visual Basic compilers.</returns>
		// Token: 0x170002ED RID: 749
		// (get) Token: 0x060019FC RID: 6652 RVA: 0x0005FD1D File Offset: 0x0005DF1D
		public unsafe static int Size
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return sizeof(void*);
			}
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with the data needed to serialize the current <see cref="T:System.IntPtr" /> object.</summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object to populate with data.</param>
		/// <param name="context">The destination for this serialization. (This parameter is not used; specify <see langword="null" />.)</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="info" /> is <see langword="null" />.</exception>
		// Token: 0x060019FD RID: 6653 RVA: 0x0005FD25 File Offset: 0x0005DF25
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("value", this.ToInt64());
		}

		/// <summary>Returns a value indicating whether this instance is equal to a specified object.</summary>
		/// <param name="obj">An object to compare with this instance or <see langword="null" />.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="obj" /> is an instance of <see cref="T:System.IntPtr" /> and equals the value of this instance; otherwise, <see langword="false" />.</returns>
		// Token: 0x060019FE RID: 6654 RVA: 0x0005FD46 File Offset: 0x0005DF46
		public override bool Equals(object obj)
		{
			return obj is IntPtr && ((IntPtr)obj).m_value == this.m_value;
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		// Token: 0x060019FF RID: 6655 RVA: 0x0005FD65 File Offset: 0x0005DF65
		public override int GetHashCode()
		{
			return this.m_value;
		}

		/// <summary>Converts the value of this instance to a 32-bit signed integer.</summary>
		/// <returns>A 32-bit signed integer equal to the value of this instance.</returns>
		/// <exception cref="T:System.OverflowException">On a 64-bit platform, the value of this instance is too large or too small to represent as a 32-bit signed integer.</exception>
		// Token: 0x06001A00 RID: 6656 RVA: 0x0005FD65 File Offset: 0x0005DF65
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public int ToInt32()
		{
			return this.m_value;
		}

		/// <summary>Converts the value of this instance to a 64-bit signed integer.</summary>
		/// <returns>A 64-bit signed integer equal to the value of this instance.</returns>
		// Token: 0x06001A01 RID: 6657 RVA: 0x0005FD6E File Offset: 0x0005DF6E
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public long ToInt64()
		{
			if (IntPtr.Size == 4)
			{
				return (long)this.m_value;
			}
			return this.m_value;
		}

		/// <summary>Converts the value of this instance to a pointer to an unspecified type.</summary>
		/// <returns>A pointer to <see cref="T:System.Void" />; that is, a pointer to memory containing data of an unspecified type.</returns>
		// Token: 0x06001A02 RID: 6658 RVA: 0x0005FD88 File Offset: 0x0005DF88
		[CLSCompliant(false)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public unsafe void* ToPointer()
		{
			return this.m_value;
		}

		/// <summary>Converts the numeric value of the current <see cref="T:System.IntPtr" /> object to its equivalent string representation.</summary>
		/// <returns>The string representation of the value of this instance.</returns>
		// Token: 0x06001A03 RID: 6659 RVA: 0x0005FD90 File Offset: 0x0005DF90
		public override string ToString()
		{
			return this.ToString(null);
		}

		/// <summary>Converts the numeric value of the current <see cref="T:System.IntPtr" /> object to its equivalent string representation.</summary>
		/// <param name="format">A format specification that governs how the current <see cref="T:System.IntPtr" /> object is converted.</param>
		/// <returns>The string representation of the value of the current <see cref="T:System.IntPtr" /> object.</returns>
		// Token: 0x06001A04 RID: 6660 RVA: 0x0005FD9C File Offset: 0x0005DF9C
		public string ToString(string format)
		{
			if (IntPtr.Size == 4)
			{
				return this.m_value.ToString(format, null);
			}
			return this.m_value.ToString(format, null);
		}

		/// <summary>Determines whether two specified instances of <see cref="T:System.IntPtr" /> are equal.</summary>
		/// <param name="value1">The first pointer or handle to compare.</param>
		/// <param name="value2">The second pointer or handle to compare.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="value1" /> equals <paramref name="value2" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001A05 RID: 6661 RVA: 0x0005FDD4 File Offset: 0x0005DFD4
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static bool operator ==(IntPtr value1, IntPtr value2)
		{
			return value1.m_value == value2.m_value;
		}

		/// <summary>Determines whether two specified instances of <see cref="T:System.IntPtr" /> are not equal.</summary>
		/// <param name="value1">The first pointer or handle to compare.</param>
		/// <param name="value2">The second pointer or handle to compare.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="value1" /> does not equal <paramref name="value2" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001A06 RID: 6662 RVA: 0x0005FDE6 File Offset: 0x0005DFE6
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public static bool operator !=(IntPtr value1, IntPtr value2)
		{
			return value1.m_value != value2.m_value;
		}

		/// <summary>Converts the value of a 32-bit signed integer to an <see cref="T:System.IntPtr" />.</summary>
		/// <param name="value">A 32-bit signed integer.</param>
		/// <returns>A new instance of <see cref="T:System.IntPtr" /> initialized to <paramref name="value" />.</returns>
		// Token: 0x06001A07 RID: 6663 RVA: 0x0005FDFB File Offset: 0x0005DFFB
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public static explicit operator IntPtr(int value)
		{
			return new IntPtr(value);
		}

		/// <summary>Converts the value of a 64-bit signed integer to an <see cref="T:System.IntPtr" />.</summary>
		/// <param name="value">A 64-bit signed integer.</param>
		/// <returns>A new instance of <see cref="T:System.IntPtr" /> initialized to <paramref name="value" />.</returns>
		/// <exception cref="T:System.OverflowException">On a 32-bit platform, <paramref name="value" /> is too large to represent as an <see cref="T:System.IntPtr" />.</exception>
		// Token: 0x06001A08 RID: 6664 RVA: 0x0005FE03 File Offset: 0x0005E003
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public static explicit operator IntPtr(long value)
		{
			return new IntPtr(value);
		}

		/// <summary>Converts the specified pointer to an unspecified type to an <see cref="T:System.IntPtr" />.  
		///  This API is not CLS-compliant.</summary>
		/// <param name="value">A pointer to an unspecified type.</param>
		/// <returns>A new instance of <see cref="T:System.IntPtr" /> initialized to <paramref name="value" />.</returns>
		// Token: 0x06001A09 RID: 6665 RVA: 0x0005FE0B File Offset: 0x0005E00B
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		[CLSCompliant(false)]
		public unsafe static explicit operator IntPtr(void* value)
		{
			return new IntPtr(value);
		}

		/// <summary>Converts the value of the specified <see cref="T:System.IntPtr" /> to a 32-bit signed integer.</summary>
		/// <param name="value">The pointer or handle to convert.</param>
		/// <returns>The contents of <paramref name="value" />.</returns>
		/// <exception cref="T:System.OverflowException">On a 64-bit platform, the value of <paramref name="value" /> is too large to represent as a 32-bit signed integer.</exception>
		// Token: 0x06001A0A RID: 6666 RVA: 0x0005FE13 File Offset: 0x0005E013
		public static explicit operator int(IntPtr value)
		{
			return value.m_value;
		}

		/// <summary>Converts the value of the specified <see cref="T:System.IntPtr" /> to a 64-bit signed integer.</summary>
		/// <param name="value">The pointer or handle to convert.</param>
		/// <returns>The contents of <paramref name="value" />.</returns>
		// Token: 0x06001A0B RID: 6667 RVA: 0x0005FE1D File Offset: 0x0005E01D
		public static explicit operator long(IntPtr value)
		{
			return value.ToInt64();
		}

		/// <summary>Converts the value of the specified <see cref="T:System.IntPtr" /> to a pointer to an unspecified type.  
		///  This API is not CLS-compliant.</summary>
		/// <param name="value">The pointer or handle to convert.</param>
		/// <returns>The contents of <paramref name="value" />.</returns>
		// Token: 0x06001A0C RID: 6668 RVA: 0x0005FE26 File Offset: 0x0005E026
		[CLSCompliant(false)]
		public unsafe static explicit operator void*(IntPtr value)
		{
			return value.m_value;
		}

		/// <summary>Adds an offset to the value of a pointer.</summary>
		/// <param name="pointer">The pointer to add the offset to.</param>
		/// <param name="offset">The offset to add.</param>
		/// <returns>A new pointer that reflects the addition of <paramref name="offset" /> to <paramref name="pointer" />.</returns>
		// Token: 0x06001A0D RID: 6669 RVA: 0x0005FE2F File Offset: 0x0005E02F
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public unsafe static IntPtr Add(IntPtr pointer, int offset)
		{
			return (IntPtr)((void*)((byte*)((void*)pointer) + offset));
		}

		/// <summary>Subtracts an offset from the value of a pointer.</summary>
		/// <param name="pointer">The pointer to subtract the offset from.</param>
		/// <param name="offset">The offset to subtract.</param>
		/// <returns>A new pointer that reflects the subtraction of <paramref name="offset" /> from <paramref name="pointer" />.</returns>
		// Token: 0x06001A0E RID: 6670 RVA: 0x0005FE3E File Offset: 0x0005E03E
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public unsafe static IntPtr Subtract(IntPtr pointer, int offset)
		{
			return (IntPtr)((void*)((byte*)((void*)pointer) - offset));
		}

		/// <summary>Adds an offset to the value of a pointer.</summary>
		/// <param name="pointer">The pointer to add the offset to.</param>
		/// <param name="offset">The offset to add.</param>
		/// <returns>A new pointer that reflects the addition of <paramref name="offset" /> to <paramref name="pointer" />.</returns>
		// Token: 0x06001A0F RID: 6671 RVA: 0x0005FE2F File Offset: 0x0005E02F
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public unsafe static IntPtr operator +(IntPtr pointer, int offset)
		{
			return (IntPtr)((void*)((byte*)((void*)pointer) + offset));
		}

		/// <summary>Subtracts an offset from the value of a pointer.</summary>
		/// <param name="pointer">The pointer to subtract the offset from.</param>
		/// <param name="offset">The offset to subtract.</param>
		/// <returns>A new pointer that reflects the subtraction of <paramref name="offset" /> from <paramref name="pointer" />.</returns>
		// Token: 0x06001A10 RID: 6672 RVA: 0x0005FE3E File Offset: 0x0005E03E
		[ReliabilityContract(Consistency.MayCorruptInstance, Cer.MayFail)]
		public unsafe static IntPtr operator -(IntPtr pointer, int offset)
		{
			return (IntPtr)((void*)((byte*)((void*)pointer) - offset));
		}

		// Token: 0x06001A11 RID: 6673 RVA: 0x0005FE4D File Offset: 0x0005E04D
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal bool IsNull()
		{
			return this.m_value == null;
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x0005FE59 File Offset: 0x0005E059
		bool IEquatable<IntPtr>.Equals(IntPtr other)
		{
			return this.m_value == other.m_value;
		}

		// Token: 0x0400171C RID: 5916
		private unsafe readonly void* m_value;

		/// <summary>A read-only field that represents a pointer or handle that has been initialized to zero.</summary>
		// Token: 0x0400171D RID: 5917
		public static readonly IntPtr Zero;
	}
}
