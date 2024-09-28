﻿using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.IO
{
	// Token: 0x02000B64 RID: 2916
	internal static class MonoIO
	{
		// Token: 0x060069CB RID: 27083 RVA: 0x0016979A File Offset: 0x0016799A
		public static Exception GetException(MonoIOError error)
		{
			if (error == MonoIOError.ERROR_ACCESS_DENIED)
			{
				return new UnauthorizedAccessException("Access to the path is denied.");
			}
			if (error != MonoIOError.ERROR_FILE_EXISTS)
			{
				return MonoIO.GetException(string.Empty, error);
			}
			return new IOException("Cannot create a file that already exist.", -2147024816);
		}

		// Token: 0x060069CC RID: 27084 RVA: 0x001697D0 File Offset: 0x001679D0
		public static Exception GetException(string path, MonoIOError error)
		{
			if (error <= MonoIOError.ERROR_FILE_EXISTS)
			{
				if (error <= MonoIOError.ERROR_NOT_SAME_DEVICE)
				{
					switch (error)
					{
					case MonoIOError.ERROR_FILE_NOT_FOUND:
						return new FileNotFoundException(string.Format("Could not find file \"{0}\"", path), path);
					case MonoIOError.ERROR_PATH_NOT_FOUND:
						return new DirectoryNotFoundException(string.Format("Could not find a part of the path \"{0}\"", path));
					case MonoIOError.ERROR_TOO_MANY_OPEN_FILES:
						if (MonoIO.dump_handles)
						{
							MonoIO.DumpHandles();
						}
						return new IOException("Too many open files", (int)((MonoIOError)(-2147024896) | error));
					case MonoIOError.ERROR_ACCESS_DENIED:
						return new UnauthorizedAccessException(string.Format("Access to the path \"{0}\" is denied.", path));
					case MonoIOError.ERROR_INVALID_HANDLE:
						return new IOException(string.Format("Invalid handle to path \"{0}\"", path), (int)((MonoIOError)(-2147024896) | error));
					default:
						if (error == MonoIOError.ERROR_INVALID_DRIVE)
						{
							return new DriveNotFoundException(string.Format("Could not find the drive  '{0}'. The drive might not be ready or might not be mapped.", path));
						}
						if (error == MonoIOError.ERROR_NOT_SAME_DEVICE)
						{
							return new IOException("Source and destination are not on the same device", (int)((MonoIOError)(-2147024896) | error));
						}
						break;
					}
				}
				else
				{
					switch (error)
					{
					case MonoIOError.ERROR_WRITE_FAULT:
						return new IOException(string.Format("Write fault on path {0}", path), (int)((MonoIOError)(-2147024896) | error));
					case MonoIOError.ERROR_READ_FAULT:
					case MonoIOError.ERROR_GEN_FAILURE:
						break;
					case MonoIOError.ERROR_SHARING_VIOLATION:
						return new IOException(string.Format("Sharing violation on path {0}", path), (int)((MonoIOError)(-2147024896) | error));
					case MonoIOError.ERROR_LOCK_VIOLATION:
						return new IOException(string.Format("Lock violation on path {0}", path), (int)((MonoIOError)(-2147024896) | error));
					default:
						if (error == MonoIOError.ERROR_HANDLE_DISK_FULL)
						{
							return new IOException(string.Format("Disk full. Path {0}", path), (int)((MonoIOError)(-2147024896) | error));
						}
						if (error == MonoIOError.ERROR_FILE_EXISTS)
						{
							return new IOException(string.Format("Could not create file \"{0}\". File already exists.", path), (int)((MonoIOError)(-2147024896) | error));
						}
						break;
					}
				}
			}
			else if (error <= MonoIOError.ERROR_DIR_NOT_EMPTY)
			{
				if (error == MonoIOError.ERROR_CANNOT_MAKE)
				{
					return new IOException(string.Format("Path {0} is a directory", path), (int)((MonoIOError)(-2147024896) | error));
				}
				if (error == MonoIOError.ERROR_INVALID_PARAMETER)
				{
					return new IOException(string.Format("Invalid parameter", Array.Empty<object>()), (int)((MonoIOError)(-2147024896) | error));
				}
				if (error == MonoIOError.ERROR_DIR_NOT_EMPTY)
				{
					return new IOException(string.Format("Directory {0} is not empty", path), (int)((MonoIOError)(-2147024896) | error));
				}
			}
			else
			{
				if (error == MonoIOError.ERROR_FILENAME_EXCED_RANGE)
				{
					return new PathTooLongException(string.Format("Path is too long. Path: {0}", path));
				}
				if (error == MonoIOError.ERROR_DIRECTORY)
				{
					return new IOException("The directory name is invalid", (int)((MonoIOError)(-2147024896) | error));
				}
				if (error == MonoIOError.ERROR_ENCRYPTION_FAILED)
				{
					return new IOException("Encryption failed", (int)((MonoIOError)(-2147024896) | error));
				}
			}
			return new IOException(string.Format("Win32 IO returned {0}. Path: {1}", error, path), (int)((MonoIOError)(-2147024896) | error));
		}

		// Token: 0x060069CD RID: 27085
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern bool CreateDirectory(char* path, out MonoIOError error);

		// Token: 0x060069CE RID: 27086 RVA: 0x00169A3C File Offset: 0x00167C3C
		public unsafe static bool CreateDirectory(string path, out MonoIOError error)
		{
			char* ptr = path;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			return MonoIO.CreateDirectory(ptr, out error);
		}

		// Token: 0x060069CF RID: 27087
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern bool RemoveDirectory(char* path, out MonoIOError error);

		// Token: 0x060069D0 RID: 27088 RVA: 0x00169A60 File Offset: 0x00167C60
		public unsafe static bool RemoveDirectory(string path, out MonoIOError error)
		{
			char* ptr = path;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			return MonoIO.RemoveDirectory(ptr, out error);
		}

		// Token: 0x060069D1 RID: 27089
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string GetCurrentDirectory(out MonoIOError error);

		// Token: 0x060069D2 RID: 27090
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern bool SetCurrentDirectory(char* path, out MonoIOError error);

		// Token: 0x060069D3 RID: 27091 RVA: 0x00169A84 File Offset: 0x00167C84
		public unsafe static bool SetCurrentDirectory(string path, out MonoIOError error)
		{
			char* ptr = path;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			return MonoIO.SetCurrentDirectory(ptr, out error);
		}

		// Token: 0x060069D4 RID: 27092
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern bool MoveFile(char* path, char* dest, out MonoIOError error);

		// Token: 0x060069D5 RID: 27093 RVA: 0x00169AA8 File Offset: 0x00167CA8
		public unsafe static bool MoveFile(string path, string dest, out MonoIOError error)
		{
			char* ptr = path;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			char* ptr2 = dest;
			if (ptr2 != null)
			{
				ptr2 += RuntimeHelpers.OffsetToStringData / 2;
			}
			return MonoIO.MoveFile(ptr, ptr2, out error);
		}

		// Token: 0x060069D6 RID: 27094
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern bool CopyFile(char* path, char* dest, bool overwrite, out MonoIOError error);

		// Token: 0x060069D7 RID: 27095 RVA: 0x00169AE0 File Offset: 0x00167CE0
		public unsafe static bool CopyFile(string path, string dest, bool overwrite, out MonoIOError error)
		{
			char* ptr = path;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			char* ptr2 = dest;
			if (ptr2 != null)
			{
				ptr2 += RuntimeHelpers.OffsetToStringData / 2;
			}
			return MonoIO.CopyFile(ptr, ptr2, overwrite, out error);
		}

		// Token: 0x060069D8 RID: 27096
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern bool DeleteFile(char* path, out MonoIOError error);

		// Token: 0x060069D9 RID: 27097 RVA: 0x00169B18 File Offset: 0x00167D18
		public unsafe static bool DeleteFile(string path, out MonoIOError error)
		{
			char* ptr = path;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			return MonoIO.DeleteFile(ptr, out error);
		}

		// Token: 0x060069DA RID: 27098
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern bool ReplaceFile(char* sourceFileName, char* destinationFileName, char* destinationBackupFileName, bool ignoreMetadataErrors, out MonoIOError error);

		// Token: 0x060069DB RID: 27099 RVA: 0x00169B3C File Offset: 0x00167D3C
		public unsafe static bool ReplaceFile(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors, out MonoIOError error)
		{
			char* ptr = sourceFileName;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			char* ptr2 = destinationFileName;
			if (ptr2 != null)
			{
				ptr2 += RuntimeHelpers.OffsetToStringData / 2;
			}
			char* ptr3 = destinationBackupFileName;
			if (ptr3 != null)
			{
				ptr3 += RuntimeHelpers.OffsetToStringData / 2;
			}
			return MonoIO.ReplaceFile(ptr, ptr2, ptr3, ignoreMetadataErrors, out error);
		}

		// Token: 0x060069DC RID: 27100
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern FileAttributes GetFileAttributes(char* path, out MonoIOError error);

		// Token: 0x060069DD RID: 27101 RVA: 0x00169B88 File Offset: 0x00167D88
		public unsafe static FileAttributes GetFileAttributes(string path, out MonoIOError error)
		{
			char* ptr = path;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			return MonoIO.GetFileAttributes(ptr, out error);
		}

		// Token: 0x060069DE RID: 27102
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern bool SetFileAttributes(char* path, FileAttributes attrs, out MonoIOError error);

		// Token: 0x060069DF RID: 27103 RVA: 0x00169BAC File Offset: 0x00167DAC
		public unsafe static bool SetFileAttributes(string path, FileAttributes attrs, out MonoIOError error)
		{
			char* ptr = path;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			return MonoIO.SetFileAttributes(ptr, attrs, out error);
		}

		// Token: 0x060069E0 RID: 27104
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern MonoFileType GetFileType(IntPtr handle, out MonoIOError error);

		// Token: 0x060069E1 RID: 27105 RVA: 0x00169BD4 File Offset: 0x00167DD4
		public static MonoFileType GetFileType(SafeHandle safeHandle, out MonoIOError error)
		{
			bool flag = false;
			MonoFileType fileType;
			try
			{
				safeHandle.DangerousAddRef(ref flag);
				fileType = MonoIO.GetFileType(safeHandle.DangerousGetHandle(), out error);
			}
			finally
			{
				if (flag)
				{
					safeHandle.DangerousRelease();
				}
			}
			return fileType;
		}

		// Token: 0x060069E2 RID: 27106
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern IntPtr FindFirstFile(char* pathWithPattern, out string fileName, out int fileAttr, out int error);

		// Token: 0x060069E3 RID: 27107 RVA: 0x00169C18 File Offset: 0x00167E18
		public unsafe static IntPtr FindFirstFile(string pathWithPattern, out string fileName, out int fileAttr, out int error)
		{
			char* ptr = pathWithPattern;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			return MonoIO.FindFirstFile(ptr, out fileName, out fileAttr, out error);
		}

		// Token: 0x060069E4 RID: 27108
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool FindNextFile(IntPtr hnd, out string fileName, out int fileAttr, out int error);

		// Token: 0x060069E5 RID: 27109
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool FindCloseFile(IntPtr hnd);

		// Token: 0x060069E6 RID: 27110 RVA: 0x00169C3E File Offset: 0x00167E3E
		public static bool Exists(string path, out MonoIOError error)
		{
			return MonoIO.GetFileAttributes(path, out error) != (FileAttributes)(-1);
		}

		// Token: 0x060069E7 RID: 27111 RVA: 0x00169C50 File Offset: 0x00167E50
		public static bool ExistsFile(string path, out MonoIOError error)
		{
			FileAttributes fileAttributes = MonoIO.GetFileAttributes(path, out error);
			return fileAttributes != (FileAttributes)(-1) && (fileAttributes & FileAttributes.Directory) == (FileAttributes)0;
		}

		// Token: 0x060069E8 RID: 27112 RVA: 0x00169C74 File Offset: 0x00167E74
		public static bool ExistsDirectory(string path, out MonoIOError error)
		{
			FileAttributes fileAttributes = MonoIO.GetFileAttributes(path, out error);
			if (error == MonoIOError.ERROR_FILE_NOT_FOUND)
			{
				error = MonoIOError.ERROR_PATH_NOT_FOUND;
			}
			return fileAttributes != (FileAttributes)(-1) && (fileAttributes & FileAttributes.Directory) != (FileAttributes)0;
		}

		// Token: 0x060069E9 RID: 27113 RVA: 0x00169CA0 File Offset: 0x00167EA0
		public static bool ExistsSymlink(string path, out MonoIOError error)
		{
			FileAttributes fileAttributes = MonoIO.GetFileAttributes(path, out error);
			return fileAttributes != (FileAttributes)(-1) && (fileAttributes & FileAttributes.ReparsePoint) != (FileAttributes)0;
		}

		// Token: 0x060069EA RID: 27114
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern bool GetFileStat(char* path, out MonoIOStat stat, out MonoIOError error);

		// Token: 0x060069EB RID: 27115 RVA: 0x00169CC8 File Offset: 0x00167EC8
		public unsafe static bool GetFileStat(string path, out MonoIOStat stat, out MonoIOError error)
		{
			char* ptr = path;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			return MonoIO.GetFileStat(ptr, out stat, out error);
		}

		// Token: 0x060069EC RID: 27116
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern IntPtr Open(char* filename, FileMode mode, FileAccess access, FileShare share, FileOptions options, out MonoIOError error);

		// Token: 0x060069ED RID: 27117 RVA: 0x00169CF0 File Offset: 0x00167EF0
		public unsafe static IntPtr Open(string filename, FileMode mode, FileAccess access, FileShare share, FileOptions options, out MonoIOError error)
		{
			char* ptr = filename;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			return MonoIO.Open(ptr, mode, access, share, options, out error);
		}

		// Token: 0x060069EE RID: 27118
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Cancel_internal(IntPtr handle, out MonoIOError error);

		// Token: 0x060069EF RID: 27119 RVA: 0x00169D1C File Offset: 0x00167F1C
		internal static bool Cancel(SafeHandle safeHandle, out MonoIOError error)
		{
			bool flag = false;
			bool result;
			try
			{
				safeHandle.DangerousAddRef(ref flag);
				result = MonoIO.Cancel_internal(safeHandle.DangerousGetHandle(), out error);
			}
			finally
			{
				if (flag)
				{
					safeHandle.DangerousRelease();
				}
			}
			return result;
		}

		// Token: 0x060069F0 RID: 27120
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool Close(IntPtr handle, out MonoIOError error);

		// Token: 0x060069F1 RID: 27121
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Read(IntPtr handle, byte[] dest, int dest_offset, int count, out MonoIOError error);

		// Token: 0x060069F2 RID: 27122 RVA: 0x00169D60 File Offset: 0x00167F60
		public static int Read(SafeHandle safeHandle, byte[] dest, int dest_offset, int count, out MonoIOError error)
		{
			bool flag = false;
			int result;
			try
			{
				safeHandle.DangerousAddRef(ref flag);
				result = MonoIO.Read(safeHandle.DangerousGetHandle(), dest, dest_offset, count, out error);
			}
			finally
			{
				if (flag)
				{
					safeHandle.DangerousRelease();
				}
			}
			return result;
		}

		// Token: 0x060069F3 RID: 27123
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Write(IntPtr handle, [In] byte[] src, int src_offset, int count, out MonoIOError error);

		// Token: 0x060069F4 RID: 27124 RVA: 0x00169DA8 File Offset: 0x00167FA8
		public static int Write(SafeHandle safeHandle, byte[] src, int src_offset, int count, out MonoIOError error)
		{
			bool flag = false;
			int result;
			try
			{
				safeHandle.DangerousAddRef(ref flag);
				result = MonoIO.Write(safeHandle.DangerousGetHandle(), src, src_offset, count, out error);
			}
			finally
			{
				if (flag)
				{
					safeHandle.DangerousRelease();
				}
			}
			return result;
		}

		// Token: 0x060069F5 RID: 27125
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern long Seek(IntPtr handle, long offset, SeekOrigin origin, out MonoIOError error);

		// Token: 0x060069F6 RID: 27126 RVA: 0x00169DF0 File Offset: 0x00167FF0
		public static long Seek(SafeHandle safeHandle, long offset, SeekOrigin origin, out MonoIOError error)
		{
			bool flag = false;
			long result;
			try
			{
				safeHandle.DangerousAddRef(ref flag);
				result = MonoIO.Seek(safeHandle.DangerousGetHandle(), offset, origin, out error);
			}
			finally
			{
				if (flag)
				{
					safeHandle.DangerousRelease();
				}
			}
			return result;
		}

		// Token: 0x060069F7 RID: 27127
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Flush(IntPtr handle, out MonoIOError error);

		// Token: 0x060069F8 RID: 27128 RVA: 0x00169E34 File Offset: 0x00168034
		public static bool Flush(SafeHandle safeHandle, out MonoIOError error)
		{
			bool flag = false;
			bool result;
			try
			{
				safeHandle.DangerousAddRef(ref flag);
				result = MonoIO.Flush(safeHandle.DangerousGetHandle(), out error);
			}
			finally
			{
				if (flag)
				{
					safeHandle.DangerousRelease();
				}
			}
			return result;
		}

		// Token: 0x060069F9 RID: 27129
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern long GetLength(IntPtr handle, out MonoIOError error);

		// Token: 0x060069FA RID: 27130 RVA: 0x00169E78 File Offset: 0x00168078
		public static long GetLength(SafeHandle safeHandle, out MonoIOError error)
		{
			bool flag = false;
			long length;
			try
			{
				safeHandle.DangerousAddRef(ref flag);
				length = MonoIO.GetLength(safeHandle.DangerousGetHandle(), out error);
			}
			finally
			{
				if (flag)
				{
					safeHandle.DangerousRelease();
				}
			}
			return length;
		}

		// Token: 0x060069FB RID: 27131
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SetLength(IntPtr handle, long length, out MonoIOError error);

		// Token: 0x060069FC RID: 27132 RVA: 0x00169EBC File Offset: 0x001680BC
		public static bool SetLength(SafeHandle safeHandle, long length, out MonoIOError error)
		{
			bool flag = false;
			bool result;
			try
			{
				safeHandle.DangerousAddRef(ref flag);
				result = MonoIO.SetLength(safeHandle.DangerousGetHandle(), length, out error);
			}
			finally
			{
				if (flag)
				{
					safeHandle.DangerousRelease();
				}
			}
			return result;
		}

		// Token: 0x060069FD RID: 27133
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool SetFileTime(IntPtr handle, long creation_time, long last_access_time, long last_write_time, out MonoIOError error);

		// Token: 0x060069FE RID: 27134 RVA: 0x00169F00 File Offset: 0x00168100
		public static bool SetFileTime(SafeHandle safeHandle, long creation_time, long last_access_time, long last_write_time, out MonoIOError error)
		{
			bool flag = false;
			bool result;
			try
			{
				safeHandle.DangerousAddRef(ref flag);
				result = MonoIO.SetFileTime(safeHandle.DangerousGetHandle(), creation_time, last_access_time, last_write_time, out error);
			}
			finally
			{
				if (flag)
				{
					safeHandle.DangerousRelease();
				}
			}
			return result;
		}

		// Token: 0x060069FF RID: 27135 RVA: 0x00169F48 File Offset: 0x00168148
		public static bool SetFileTime(string path, long creation_time, long last_access_time, long last_write_time, out MonoIOError error)
		{
			return MonoIO.SetFileTime(path, 0, creation_time, last_access_time, last_write_time, DateTime.MinValue, out error);
		}

		// Token: 0x06006A00 RID: 27136 RVA: 0x00169F5B File Offset: 0x0016815B
		public static bool SetCreationTime(string path, DateTime dateTime, out MonoIOError error)
		{
			return MonoIO.SetFileTime(path, 1, -1L, -1L, -1L, dateTime, out error);
		}

		// Token: 0x06006A01 RID: 27137 RVA: 0x00169F6C File Offset: 0x0016816C
		public static bool SetLastAccessTime(string path, DateTime dateTime, out MonoIOError error)
		{
			return MonoIO.SetFileTime(path, 2, -1L, -1L, -1L, dateTime, out error);
		}

		// Token: 0x06006A02 RID: 27138 RVA: 0x00169F7D File Offset: 0x0016817D
		public static bool SetLastWriteTime(string path, DateTime dateTime, out MonoIOError error)
		{
			return MonoIO.SetFileTime(path, 3, -1L, -1L, -1L, dateTime, out error);
		}

		// Token: 0x06006A03 RID: 27139 RVA: 0x00169F90 File Offset: 0x00168190
		public static bool SetFileTime(string path, int type, long creation_time, long last_access_time, long last_write_time, DateTime dateTime, out MonoIOError error)
		{
			IntPtr intPtr = MonoIO.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite, FileOptions.None, out error);
			if (intPtr == MonoIO.InvalidHandle)
			{
				return false;
			}
			switch (type)
			{
			case 1:
				creation_time = dateTime.ToFileTime();
				break;
			case 2:
				last_access_time = dateTime.ToFileTime();
				break;
			case 3:
				last_write_time = dateTime.ToFileTime();
				break;
			}
			bool result = MonoIO.SetFileTime(new SafeFileHandle(intPtr, false), creation_time, last_access_time, last_write_time, out error);
			MonoIOError monoIOError;
			MonoIO.Close(intPtr, out monoIOError);
			return result;
		}

		// Token: 0x06006A04 RID: 27140
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Lock(IntPtr handle, long position, long length, out MonoIOError error);

		// Token: 0x06006A05 RID: 27141 RVA: 0x0016A00C File Offset: 0x0016820C
		public static void Lock(SafeHandle safeHandle, long position, long length, out MonoIOError error)
		{
			bool flag = false;
			try
			{
				safeHandle.DangerousAddRef(ref flag);
				MonoIO.Lock(safeHandle.DangerousGetHandle(), position, length, out error);
			}
			finally
			{
				if (flag)
				{
					safeHandle.DangerousRelease();
				}
			}
		}

		// Token: 0x06006A06 RID: 27142
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Unlock(IntPtr handle, long position, long length, out MonoIOError error);

		// Token: 0x06006A07 RID: 27143 RVA: 0x0016A050 File Offset: 0x00168250
		public static void Unlock(SafeHandle safeHandle, long position, long length, out MonoIOError error)
		{
			bool flag = false;
			try
			{
				safeHandle.DangerousAddRef(ref flag);
				MonoIO.Unlock(safeHandle.DangerousGetHandle(), position, length, out error);
			}
			finally
			{
				if (flag)
				{
					safeHandle.DangerousRelease();
				}
			}
		}

		// Token: 0x17001240 RID: 4672
		// (get) Token: 0x06006A08 RID: 27144
		public static extern IntPtr ConsoleOutput { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17001241 RID: 4673
		// (get) Token: 0x06006A09 RID: 27145
		public static extern IntPtr ConsoleInput { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17001242 RID: 4674
		// (get) Token: 0x06006A0A RID: 27146
		public static extern IntPtr ConsoleError { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x06006A0B RID: 27147
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool CreatePipe(out IntPtr read_handle, out IntPtr write_handle, out MonoIOError error);

		// Token: 0x06006A0C RID: 27148
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool DuplicateHandle(IntPtr source_process_handle, IntPtr source_handle, IntPtr target_process_handle, out IntPtr target_handle, int access, int inherit, int options, out MonoIOError error);

		// Token: 0x17001243 RID: 4675
		// (get) Token: 0x06006A0D RID: 27149
		public static extern char VolumeSeparatorChar { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17001244 RID: 4676
		// (get) Token: 0x06006A0E RID: 27150
		public static extern char DirectorySeparatorChar { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17001245 RID: 4677
		// (get) Token: 0x06006A0F RID: 27151
		public static extern char AltDirectorySeparatorChar { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x17001246 RID: 4678
		// (get) Token: 0x06006A10 RID: 27152
		public static extern char PathSeparator { [MethodImpl(MethodImplOptions.InternalCall)] get; }

		// Token: 0x06006A11 RID: 27153
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DumpHandles();

		// Token: 0x06006A12 RID: 27154
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool RemapPath(string path, out string newPath);

		// Token: 0x04003D54 RID: 15700
		public const int FileAlreadyExistsHResult = -2147024816;

		// Token: 0x04003D55 RID: 15701
		public const FileAttributes InvalidFileAttributes = (FileAttributes)(-1);

		// Token: 0x04003D56 RID: 15702
		public static readonly IntPtr InvalidHandle = (IntPtr)(-1L);

		// Token: 0x04003D57 RID: 15703
		private static bool dump_handles = Environment.GetEnvironmentVariable("MONO_DUMP_HANDLES_ON_ERROR_TOO_MANY_OPEN_FILES") != null;
	}
}
