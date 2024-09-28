﻿using System;
using System.Collections.Generic;

namespace System
{
	/// <summary>Supports a value type that can be assigned <see langword="null" />. This class cannot be inherited.</summary>
	// Token: 0x02000164 RID: 356
	public static class Nullable
	{
		/// <summary>Compares the relative values of two <see cref="T:System.Nullable`1" /> objects.</summary>
		/// <param name="n1">A <see cref="T:System.Nullable`1" /> object.</param>
		/// <param name="n2">A <see cref="T:System.Nullable`1" /> object.</param>
		/// <typeparam name="T">The underlying value type of the <paramref name="n1" /> and <paramref name="n2" /> parameters.</typeparam>
		/// <returns>An integer that indicates the relative values of the <paramref name="n1" /> and <paramref name="n2" /> parameters.  
		///   Return Value  
		///
		///   Description  
		///
		///   Less than zero  
		///
		///   The <see cref="P:System.Nullable`1.HasValue" /> property for <paramref name="n1" /> is <see langword="false" />, and the <see cref="P:System.Nullable`1.HasValue" /> property for <paramref name="n2" /> is <see langword="true" />.  
		///
		///  -or-  
		///
		///  The <see cref="P:System.Nullable`1.HasValue" /> properties for <paramref name="n1" /> and <paramref name="n2" /> are <see langword="true" />, and the value of the <see cref="P:System.Nullable`1.Value" /> property for <paramref name="n1" /> is less than the value of the <see cref="P:System.Nullable`1.Value" /> property for <paramref name="n2" />.  
		///
		///   Zero  
		///
		///   The <see cref="P:System.Nullable`1.HasValue" /> properties for <paramref name="n1" /> and <paramref name="n2" /> are <see langword="false" />.  
		///
		///  -or-  
		///
		///  The <see cref="P:System.Nullable`1.HasValue" /> properties for <paramref name="n1" /> and <paramref name="n2" /> are <see langword="true" />, and the value of the <see cref="P:System.Nullable`1.Value" /> property for <paramref name="n1" /> is equal to the value of the <see cref="P:System.Nullable`1.Value" /> property for <paramref name="n2" />.  
		///
		///   Greater than zero  
		///
		///   The <see cref="P:System.Nullable`1.HasValue" /> property for <paramref name="n1" /> is <see langword="true" />, and the <see cref="P:System.Nullable`1.HasValue" /> property for <paramref name="n2" /> is <see langword="false" />.  
		///
		///  -or-  
		///
		///  The <see cref="P:System.Nullable`1.HasValue" /> properties for <paramref name="n1" /> and <paramref name="n2" /> are <see langword="true" />, and the value of the <see cref="P:System.Nullable`1.Value" /> property for <paramref name="n1" /> is greater than the value of the <see cref="P:System.Nullable`1.Value" /> property for <paramref name="n2" />.</returns>
		// Token: 0x06000DF5 RID: 3573 RVA: 0x00036156 File Offset: 0x00034356
		public static int Compare<T>(T? n1, T? n2) where T : struct
		{
			if (n1 != null)
			{
				if (n2 != null)
				{
					return Comparer<T>.Default.Compare(n1.value, n2.value);
				}
				return 1;
			}
			else
			{
				if (n2 != null)
				{
					return -1;
				}
				return 0;
			}
		}

		/// <summary>Indicates whether two specified <see cref="T:System.Nullable`1" /> objects are equal.</summary>
		/// <param name="n1">A <see cref="T:System.Nullable`1" /> object.</param>
		/// <param name="n2">A <see cref="T:System.Nullable`1" /> object.</param>
		/// <typeparam name="T">The underlying value type of the <paramref name="n1" /> and <paramref name="n2" /> parameters.</typeparam>
		/// <returns>
		///   <see langword="true" /> if the <paramref name="n1" /> parameter is equal to the <paramref name="n2" /> parameter; otherwise, <see langword="false" />.  
		/// The return value depends on the <see cref="P:System.Nullable`1.HasValue" /> and <see cref="P:System.Nullable`1.Value" /> properties of the two parameters that are compared.  
		///  Return Value  
		///
		///  Description  
		///
		/// <see langword="true" /> The <see cref="P:System.Nullable`1.HasValue" /> properties for <paramref name="n1" /> and <paramref name="n2" /> are <see langword="false" />.  
		///
		/// -or-  
		///
		/// The <see cref="P:System.Nullable`1.HasValue" /> properties for <paramref name="n1" /> and <paramref name="n2" /> are <see langword="true" />, and the <see cref="P:System.Nullable`1.Value" /> properties of the parameters are equal.  
		///
		/// <see langword="false" /> The <see cref="P:System.Nullable`1.HasValue" /> property is <see langword="true" /> for one parameter and <see langword="false" /> for the other parameter.  
		///
		/// -or-  
		///
		/// The <see cref="P:System.Nullable`1.HasValue" /> properties for <paramref name="n1" /> and <paramref name="n2" /> are <see langword="true" />, and the <see cref="P:System.Nullable`1.Value" /> properties of the parameters are unequal.</returns>
		// Token: 0x06000DF6 RID: 3574 RVA: 0x0003618F File Offset: 0x0003438F
		public static bool Equals<T>(T? n1, T? n2) where T : struct
		{
			if (n1 != null)
			{
				return n2 != null && EqualityComparer<T>.Default.Equals(n1.value, n2.value);
			}
			return n2 == null;
		}

		/// <summary>Returns the underlying type argument of the specified nullable type.</summary>
		/// <param name="nullableType">A <see cref="T:System.Type" /> object that describes a closed generic nullable type.</param>
		/// <returns>The type argument of the <paramref name="nullableType" /> parameter, if the <paramref name="nullableType" /> parameter is a closed generic nullable type; otherwise, <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="nullableType" /> is <see langword="null" />.</exception>
		// Token: 0x06000DF7 RID: 3575 RVA: 0x000361C8 File Offset: 0x000343C8
		public static Type GetUnderlyingType(Type nullableType)
		{
			if (nullableType == null)
			{
				throw new ArgumentNullException("nullableType");
			}
			if (nullableType.IsGenericType && !nullableType.IsGenericTypeDefinition && nullableType.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				return nullableType.GetGenericArguments()[0];
			}
			return null;
		}
	}
}
