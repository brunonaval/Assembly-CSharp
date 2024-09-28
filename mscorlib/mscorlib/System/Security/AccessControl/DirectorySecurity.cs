﻿using System;

namespace System.Security.AccessControl
{
	/// <summary>Represents the access control and audit security for a directory. This class cannot be inherited.</summary>
	// Token: 0x02000521 RID: 1313
	public sealed class DirectorySecurity : FileSystemSecurity
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Security.AccessControl.DirectorySecurity" /> class.</summary>
		/// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows 2000 or later.</exception>
		// Token: 0x06003409 RID: 13321 RVA: 0x000BE72C File Offset: 0x000BC92C
		public DirectorySecurity() : base(true)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.AccessControl.DirectorySecurity" /> class from a specified directory using the specified values of the <see cref="T:System.Security.AccessControl.AccessControlSections" /> enumeration.</summary>
		/// <param name="name">The location of a directory to create a <see cref="T:System.Security.AccessControl.DirectorySecurity" /> object from.</param>
		/// <param name="includeSections">One of the <see cref="T:System.Security.AccessControl.AccessControlSections" /> values that specifies the type of access control list (ACL) information to retrieve.</param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="name" /> parameter is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, (for example, it is on an unmapped drive).</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file specified in the <paramref name="name" /> parameter was not found.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the directory.</exception>
		/// <exception cref="T:System.NotSupportedException">The <paramref name="name" /> parameter is in an invalid format.</exception>
		/// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows 2000 or later.</exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length.</exception>
		/// <exception cref="T:System.Security.AccessControl.PrivilegeNotHeldException">The current system account does not have administrative privileges.</exception>
		/// <exception cref="T:System.SystemException">The directory could not be found.</exception>
		/// <exception cref="T:System.UnauthorizedAccessException">The <paramref name="name" /> parameter specified a directory that is read-only.  
		///  -or-  
		///  This operation is not supported on the current platform.  
		///  -or-  
		///  The caller does not have the required permission.</exception>
		// Token: 0x0600340A RID: 13322 RVA: 0x000BE735 File Offset: 0x000BC935
		public DirectorySecurity(string name, AccessControlSections includeSections) : base(true, name, includeSections)
		{
		}
	}
}
