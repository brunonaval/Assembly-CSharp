﻿using System;
using System.Runtime.InteropServices;

namespace System.Security.AccessControl
{
	/// <summary>Represents the access control and audit security for a file. This class cannot be inherited.</summary>
	// Token: 0x02000527 RID: 1319
	public sealed class FileSecurity : FileSystemSecurity
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Security.AccessControl.FileSecurity" /> class.</summary>
		/// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows 2000 or later.</exception>
		// Token: 0x06003436 RID: 13366 RVA: 0x000BEA30 File Offset: 0x000BCC30
		public FileSecurity() : base(false)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.AccessControl.FileSecurity" /> class from a specified file using the specified values of the <see cref="T:System.Security.AccessControl.AccessControlSections" /> enumeration.</summary>
		/// <param name="fileName">The location of a file to create a <see cref="T:System.Security.AccessControl.FileSecurity" /> object from.</param>
		/// <param name="includeSections">One of the <see cref="T:System.Security.AccessControl.AccessControlSections" /> values that specifies the type of access control list (ACL) information to retrieve.</param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="fileName" /> parameter is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, (for example, it is on an unmapped drive).</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file specified in the <paramref name="fileName" /> parameter was not found.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
		/// <exception cref="T:System.NotSupportedException">
		///   <paramref name="path" /> is in an invalid format.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.SEHException">The <paramref name="fileName" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows 2000 or later.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.Security.AccessControl.PrivilegeNotHeldException">The current system account does not have administrative privileges.</exception>
		/// <exception cref="T:System.SystemException">The file could not be found.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The <paramref name="fileName" /> parameter specified a file that is read-only.  
		///  -or-  
		///  This operation is not supported on the current platform.  
		///  -or-  
		///  The <paramref name="fileName" /> parameter specified a directory.  
		///  -or-  
		///  The caller does not have the required permission.</exception>
		// Token: 0x06003437 RID: 13367 RVA: 0x000BEA39 File Offset: 0x000BCC39
		public FileSecurity(string fileName, AccessControlSections includeSections) : base(false, fileName, includeSections)
		{
		}

		// Token: 0x06003438 RID: 13368 RVA: 0x000BEA44 File Offset: 0x000BCC44
		internal FileSecurity(SafeHandle handle, AccessControlSections includeSections) : base(false, handle, includeSections)
		{
		}
	}
}
