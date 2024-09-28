﻿using System;
using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Runtime.InteropServices.WindowsRuntime
{
	/// <summary>Provides data for the <see cref="E:System.Runtime.InteropServices.WindowsRuntime.WindowsRuntimeMetadata.ReflectionOnlyNamespaceResolve" /> event.</summary>
	// Token: 0x02000798 RID: 1944
	[ComVisible(false)]
	public class NamespaceResolveEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.InteropServices.WindowsRuntime.NamespaceResolveEventArgs" /> class, specifying the namespace to resolve and the assembly whose dependency is being resolved.</summary>
		/// <param name="namespaceName">The namespace to resolve.</param>
		/// <param name="requestingAssembly">The assembly whose dependency is being resolved.</param>
		// Token: 0x060044DD RID: 17629 RVA: 0x000E4F04 File Offset: 0x000E3104
		public NamespaceResolveEventArgs(string namespaceName, Assembly requestingAssembly)
		{
			this.NamespaceName = namespaceName;
			this.RequestingAssembly = requestingAssembly;
			this.ResolvedAssemblies = new Collection<Assembly>();
		}

		/// <summary>Gets the name of the namespace to resolve.</summary>
		/// <returns>The name of the namespace to resolve.</returns>
		// Token: 0x17000AAA RID: 2730
		// (get) Token: 0x060044DE RID: 17630 RVA: 0x000E4F25 File Offset: 0x000E3125
		// (set) Token: 0x060044DF RID: 17631 RVA: 0x000E4F2D File Offset: 0x000E312D
		public string NamespaceName { get; private set; }

		/// <summary>Gets the name of the assembly whose dependency is being resolved.</summary>
		/// <returns>The name of the assembly whose dependency is being resolved.</returns>
		// Token: 0x17000AAB RID: 2731
		// (get) Token: 0x060044E0 RID: 17632 RVA: 0x000E4F36 File Offset: 0x000E3136
		// (set) Token: 0x060044E1 RID: 17633 RVA: 0x000E4F3E File Offset: 0x000E313E
		public Assembly RequestingAssembly { get; private set; }

		/// <summary>Gets a collection of assemblies; when the event handler for the <see cref="E:System.Runtime.InteropServices.WindowsRuntime.WindowsRuntimeMetadata.ReflectionOnlyNamespaceResolve" /> event is invoked, the collection is empty, and the event handler is responsible for adding the necessary assemblies.</summary>
		/// <returns>A collection of assemblies that define the requested namespace.</returns>
		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x060044E2 RID: 17634 RVA: 0x000E4F47 File Offset: 0x000E3147
		// (set) Token: 0x060044E3 RID: 17635 RVA: 0x000E4F4F File Offset: 0x000E314F
		public Collection<Assembly> ResolvedAssemblies { get; private set; }
	}
}
