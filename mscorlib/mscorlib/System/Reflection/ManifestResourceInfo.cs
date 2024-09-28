﻿using System;

namespace System.Reflection
{
	/// <summary>Provides access to manifest resources, which are XML files that describe application dependencies.</summary>
	// Token: 0x020008A8 RID: 2216
	public class ManifestResourceInfo
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Reflection.ManifestResourceInfo" /> class for a resource that is contained by the specified assembly and file, and that has the specified location.</summary>
		/// <param name="containingAssembly">The assembly that contains the manifest resource.</param>
		/// <param name="containingFileName">The name of the file that contains the manifest resource, if the file is not the same as the manifest file.</param>
		/// <param name="resourceLocation">A bitwise combination of enumeration values that provides information about the location of the manifest resource.</param>
		// Token: 0x06004908 RID: 18696 RVA: 0x000EE8F8 File Offset: 0x000ECAF8
		public ManifestResourceInfo(Assembly containingAssembly, string containingFileName, ResourceLocation resourceLocation)
		{
			this.ReferencedAssembly = containingAssembly;
			this.FileName = containingFileName;
			this.ResourceLocation = resourceLocation;
		}

		/// <summary>Gets the containing assembly for the manifest resource.</summary>
		/// <returns>The manifest resource's containing assembly.</returns>
		// Token: 0x17000B54 RID: 2900
		// (get) Token: 0x06004909 RID: 18697 RVA: 0x000EE915 File Offset: 0x000ECB15
		public virtual Assembly ReferencedAssembly { get; }

		/// <summary>Gets the name of the file that contains the manifest resource, if it is not the same as the manifest file.</summary>
		/// <returns>The manifest resource's file name.</returns>
		// Token: 0x17000B55 RID: 2901
		// (get) Token: 0x0600490A RID: 18698 RVA: 0x000EE91D File Offset: 0x000ECB1D
		public virtual string FileName { get; }

		/// <summary>Gets the manifest resource's location.</summary>
		/// <returns>A bitwise combination of <see cref="T:System.Reflection.ResourceLocation" /> flags that indicates the location of the manifest resource.</returns>
		// Token: 0x17000B56 RID: 2902
		// (get) Token: 0x0600490B RID: 18699 RVA: 0x000EE925 File Offset: 0x000ECB25
		public virtual ResourceLocation ResourceLocation { get; }
	}
}
