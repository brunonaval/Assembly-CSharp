﻿using System;

namespace System.Reflection
{
	/// <summary>Defines additional version information for an assembly manifest.</summary>
	// Token: 0x02000888 RID: 2184
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class AssemblyInformationalVersionAttribute : Attribute
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Reflection.AssemblyInformationalVersionAttribute" /> class.</summary>
		/// <param name="informationalVersion">The assembly version information.</param>
		// Token: 0x06004866 RID: 18534 RVA: 0x000EE0A9 File Offset: 0x000EC2A9
		public AssemblyInformationalVersionAttribute(string informationalVersion)
		{
			this.InformationalVersion = informationalVersion;
		}

		/// <summary>Gets version information.</summary>
		/// <returns>A string containing the version information.</returns>
		// Token: 0x17000B2B RID: 2859
		// (get) Token: 0x06004867 RID: 18535 RVA: 0x000EE0B8 File Offset: 0x000EC2B8
		public string InformationalVersion { get; }
	}
}
