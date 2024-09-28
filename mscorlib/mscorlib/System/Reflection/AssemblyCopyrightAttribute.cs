using System;

namespace System.Reflection
{
	/// <summary>Defines a copyright custom attribute for an assembly manifest.</summary>
	// Token: 0x02000881 RID: 2177
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyCopyrightAttribute : Attribute
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Reflection.AssemblyCopyrightAttribute" /> class.</summary>
		/// <param name="copyright">The copyright information.</param>
		// Token: 0x06004855 RID: 18517 RVA: 0x000EDFFA File Offset: 0x000EC1FA
		public AssemblyCopyrightAttribute(string copyright)
		{
			this.Copyright = copyright;
		}

		/// <summary>Gets copyright information.</summary>
		/// <returns>A string containing the copyright information.</returns>
		// Token: 0x17000B23 RID: 2851
		// (get) Token: 0x06004856 RID: 18518 RVA: 0x000EE009 File Offset: 0x000EC209
		public string Copyright { get; }
	}
}
