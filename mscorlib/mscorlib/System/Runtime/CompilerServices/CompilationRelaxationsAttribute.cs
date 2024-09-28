﻿using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	/// <summary>Controls the strictness of the code generated by the common language runtime's just-in-time (JIT) compiler.</summary>
	// Token: 0x02000831 RID: 2097
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Method)]
	[Serializable]
	public class CompilationRelaxationsAttribute : Attribute
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.CompilerServices.CompilationRelaxationsAttribute" /> class with the specified compilation relaxations.</summary>
		/// <param name="relaxations">The compilation relaxations.</param>
		// Token: 0x060046B1 RID: 18097 RVA: 0x000E70E3 File Offset: 0x000E52E3
		public CompilationRelaxationsAttribute(int relaxations)
		{
			this.m_relaxations = relaxations;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.CompilerServices.CompilationRelaxationsAttribute" /> class with the specified <see cref="T:System.Runtime.CompilerServices.CompilationRelaxations" /> value.</summary>
		/// <param name="relaxations">One of the <see cref="T:System.Runtime.CompilerServices.CompilationRelaxations" /> values.</param>
		// Token: 0x060046B2 RID: 18098 RVA: 0x000E70E3 File Offset: 0x000E52E3
		public CompilationRelaxationsAttribute(CompilationRelaxations relaxations)
		{
			this.m_relaxations = (int)relaxations;
		}

		/// <summary>Gets the compilation relaxations specified when the current object was constructed.</summary>
		/// <returns>The compilation relaxations specified when the current object was constructed.  
		///  Use the <see cref="T:System.Runtime.CompilerServices.CompilationRelaxations" /> enumeration with the <see cref="P:System.Runtime.CompilerServices.CompilationRelaxationsAttribute.CompilationRelaxations" /> property.</returns>
		// Token: 0x17000ADE RID: 2782
		// (get) Token: 0x060046B3 RID: 18099 RVA: 0x000E70F2 File Offset: 0x000E52F2
		public int CompilationRelaxations
		{
			get
			{
				return this.m_relaxations;
			}
		}

		// Token: 0x04002D7B RID: 11643
		private int m_relaxations;
	}
}