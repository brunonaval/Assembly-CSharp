﻿using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security;

namespace System.Collections
{
	/// <summary>Compares two objects for equivalence, where string comparisons are case-sensitive.</summary>
	// Token: 0x02000A0F RID: 2575
	[Serializable]
	public sealed class Comparer : IComparer, ISerializable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Collections.Comparer" /> class using the specified <see cref="T:System.Globalization.CultureInfo" />.</summary>
		/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> to use for the new <see cref="T:System.Collections.Comparer" />.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="culture" /> is <see langword="null" />.</exception>
		// Token: 0x06005B6E RID: 23406 RVA: 0x00134A46 File Offset: 0x00132C46
		public Comparer(CultureInfo culture)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			this._compareInfo = culture.CompareInfo;
		}

		// Token: 0x06005B6F RID: 23407 RVA: 0x00134A68 File Offset: 0x00132C68
		private Comparer(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this._compareInfo = (CompareInfo)info.GetValue("CompareInfo", typeof(CompareInfo));
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with the data required for serialization.</summary>
		/// <param name="info">The object to populate with data.</param>
		/// <param name="context">The context information about the source or destination of the serialization.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="info" /> is <see langword="null" />.</exception>
		// Token: 0x06005B70 RID: 23408 RVA: 0x00134A9E File Offset: 0x00132C9E
		[SecurityCritical]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("CompareInfo", this._compareInfo);
		}

		/// <summary>Performs a case-sensitive comparison of two objects of the same type and returns a value indicating whether one is less than, equal to, or greater than the other.</summary>
		/// <param name="a">The first object to compare.</param>
		/// <param name="b">The second object to compare.</param>
		/// <returns>A signed integer that indicates the relative values of <paramref name="a" /> and <paramref name="b" />, as shown in the following table.  
		///   Value  
		///
		///   Meaning  
		///
		///   Less than zero  
		///
		///  <paramref name="a" /> is less than <paramref name="b" />.  
		///
		///   Zero  
		///
		///  <paramref name="a" /> equals <paramref name="b" />.  
		///
		///   Greater than zero  
		///
		///  <paramref name="a" /> is greater than <paramref name="b" />.</returns>
		/// <exception cref="T:System.ArgumentException">Neither <paramref name="a" /> nor <paramref name="b" /> implements the <see cref="T:System.IComparable" /> interface.  
		///  -or-  
		///  <paramref name="a" /> and <paramref name="b" /> are of different types and neither one can handle comparisons with the other.</exception>
		// Token: 0x06005B71 RID: 23409 RVA: 0x00134AC0 File Offset: 0x00132CC0
		public int Compare(object a, object b)
		{
			if (a == b)
			{
				return 0;
			}
			if (a == null)
			{
				return -1;
			}
			if (b == null)
			{
				return 1;
			}
			string text = a as string;
			string text2 = b as string;
			if (text != null && text2 != null)
			{
				return this._compareInfo.Compare(text, text2);
			}
			IComparable comparable = a as IComparable;
			if (comparable != null)
			{
				return comparable.CompareTo(b);
			}
			IComparable comparable2 = b as IComparable;
			if (comparable2 != null)
			{
				return -comparable2.CompareTo(a);
			}
			throw new ArgumentException("At least one object must implement IComparable.");
		}

		// Token: 0x04003864 RID: 14436
		private CompareInfo _compareInfo;

		/// <summary>Represents an instance of <see cref="T:System.Collections.Comparer" /> that is associated with the <see cref="P:System.Threading.Thread.CurrentCulture" /> of the current thread. This field is read-only.</summary>
		// Token: 0x04003865 RID: 14437
		public static readonly Comparer Default = new Comparer(CultureInfo.CurrentCulture);

		/// <summary>Represents an instance of <see cref="T:System.Collections.Comparer" /> that is associated with <see cref="P:System.Globalization.CultureInfo.InvariantCulture" />. This field is read-only.</summary>
		// Token: 0x04003866 RID: 14438
		public static readonly Comparer DefaultInvariant = new Comparer(CultureInfo.InvariantCulture);
	}
}
