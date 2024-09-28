﻿using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Identifies the type of class interface that is generated for a class.</summary>
	// Token: 0x020006E9 RID: 1769
	[ComVisible(true)]
	[Serializable]
	public enum ClassInterfaceType
	{
		/// <summary>Indicates that no class interface is generated for the class. If no interfaces are implemented explicitly, the class can only provide late-bound access through the <see langword="IDispatch" /> interface. This is the recommended setting for <see cref="T:System.Runtime.InteropServices.ClassInterfaceAttribute" />. Using <see langword="ClassInterfaceType.None" /> is the only way to expose functionality through interfaces implemented explicitly by the class.</summary>
		// Token: 0x04002A36 RID: 10806
		None,
		/// <summary>Indicates that the class only supports late binding for COM clients. A <see langword="dispinterface" /> for the class is automatically exposed to COM clients on request. The type library produced by Tlbexp.exe (Type Library Exporter) does not contain type information for the <see langword="dispinterface" /> in order to prevent clients from caching the DISPIDs of the interface. The <see langword="dispinterface" /> does not exhibit the versioning problems described in <see cref="T:System.Runtime.InteropServices.ClassInterfaceAttribute" /> because clients can only late-bind to the interface.</summary>
		// Token: 0x04002A37 RID: 10807
		AutoDispatch,
		/// <summary>Indicates that a dual class interface is automatically generated for the class and exposed to COM. Type information is produced for the class interface and published in the type library. Using <see langword="AutoDual" /> is strongly discouraged because of the versioning limitations described in <see cref="T:System.Runtime.InteropServices.ClassInterfaceAttribute" />.</summary>
		// Token: 0x04002A38 RID: 10808
		AutoDual
	}
}
