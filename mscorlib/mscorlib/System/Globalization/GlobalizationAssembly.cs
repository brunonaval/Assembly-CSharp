using System;
using System.IO;
using System.Reflection;
using System.Security;

namespace System.Globalization
{
	// Token: 0x02000987 RID: 2439
	internal sealed class GlobalizationAssembly
	{
		// Token: 0x0600567B RID: 22139 RVA: 0x001244DC File Offset: 0x001226DC
		[SecurityCritical]
		internal unsafe static byte* GetGlobalizationResourceBytePtr(Assembly assembly, string tableName)
		{
			UnmanagedMemoryStream unmanagedMemoryStream = assembly.GetManifestResourceStream(tableName) as UnmanagedMemoryStream;
			if (unmanagedMemoryStream != null)
			{
				byte* positionPointer = unmanagedMemoryStream.PositionPointer;
				if (positionPointer != null)
				{
					return positionPointer;
				}
			}
			throw new InvalidOperationException();
		}
	}
}
