using System;

namespace System.Reflection
{
	/// <summary>Instructs obfuscation tools to take the specified actions for an assembly, type, or member.</summary>
	// Token: 0x020008B4 RID: 2228
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Parameter | AttributeTargets.Delegate, AllowMultiple = true, Inherited = false)]
	public sealed class ObfuscationAttribute : Attribute
	{
		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value indicating whether the obfuscation tool should remove this attribute after processing.</summary>
		/// <returns>
		///   <see langword="true" /> if an obfuscation tool should remove the attribute after processing; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000B87 RID: 2951
		// (get) Token: 0x060049B7 RID: 18871 RVA: 0x000EF1A3 File Offset: 0x000ED3A3
		// (set) Token: 0x060049B8 RID: 18872 RVA: 0x000EF1AB File Offset: 0x000ED3AB
		public bool StripAfterObfuscation { get; set; } = true;

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value indicating whether the obfuscation tool should exclude the type or member from obfuscation.</summary>
		/// <returns>
		///   <see langword="true" /> if the type or member to which this attribute is applied should be excluded from obfuscation; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000B88 RID: 2952
		// (get) Token: 0x060049B9 RID: 18873 RVA: 0x000EF1B4 File Offset: 0x000ED3B4
		// (set) Token: 0x060049BA RID: 18874 RVA: 0x000EF1BC File Offset: 0x000ED3BC
		public bool Exclude { get; set; } = true;

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value indicating whether the attribute of a type is to apply to the members of the type.</summary>
		/// <returns>
		///   <see langword="true" /> if the attribute is to apply to the members of the type; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000B89 RID: 2953
		// (get) Token: 0x060049BB RID: 18875 RVA: 0x000EF1C5 File Offset: 0x000ED3C5
		// (set) Token: 0x060049BC RID: 18876 RVA: 0x000EF1CD File Offset: 0x000ED3CD
		public bool ApplyToMembers { get; set; } = true;

		/// <summary>Gets or sets a string value that is recognized by the obfuscation tool, and which specifies processing options.</summary>
		/// <returns>A string value that is recognized by the obfuscation tool, and which specifies processing options. The default is "all".</returns>
		// Token: 0x17000B8A RID: 2954
		// (get) Token: 0x060049BD RID: 18877 RVA: 0x000EF1D6 File Offset: 0x000ED3D6
		// (set) Token: 0x060049BE RID: 18878 RVA: 0x000EF1DE File Offset: 0x000ED3DE
		public string Feature { get; set; } = "all";
	}
}
