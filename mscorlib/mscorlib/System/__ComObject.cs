﻿using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Mono.Interop;

namespace System
{
	// Token: 0x02000274 RID: 628
	[StructLayout(LayoutKind.Sequential)]
	internal class __ComObject : MarshalByRefObject
	{
		// Token: 0x06001C74 RID: 7284
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern __ComObject CreateRCW(Type t);

		// Token: 0x06001C75 RID: 7285
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ReleaseInterfaces();

		// Token: 0x06001C76 RID: 7286 RVA: 0x0006A0F0 File Offset: 0x000682F0
		~__ComObject()
		{
			if (this.hash_table != IntPtr.Zero)
			{
				if (this.synchronization_context != null)
				{
					this.synchronization_context.Post(delegate(object state)
					{
						this.ReleaseInterfaces();
					}, this);
				}
				else
				{
					this.ReleaseInterfaces();
				}
			}
			this.proxy = null;
		}

		// Token: 0x06001C77 RID: 7287 RVA: 0x0006A158 File Offset: 0x00068358
		public __ComObject()
		{
			this.Initialize(base.GetType());
		}

		// Token: 0x06001C78 RID: 7288 RVA: 0x0006A16C File Offset: 0x0006836C
		internal __ComObject(Type t)
		{
			this.Initialize(t);
		}

		// Token: 0x06001C79 RID: 7289 RVA: 0x0006A17C File Offset: 0x0006837C
		internal __ComObject(IntPtr pItf, ComInteropProxy p)
		{
			this.proxy = p;
			this.InitializeApartmentDetails();
			Guid iid_IUnknown = __ComObject.IID_IUnknown;
			Marshal.ThrowExceptionForHR(Marshal.QueryInterface(pItf, ref iid_IUnknown, out this.iunknown));
		}

		// Token: 0x06001C7A RID: 7290 RVA: 0x0006A1B5 File Offset: 0x000683B5
		internal void Initialize(IntPtr pUnk, ComInteropProxy p)
		{
			this.proxy = p;
			this.InitializeApartmentDetails();
			this.iunknown = pUnk;
		}

		// Token: 0x06001C7B RID: 7291 RVA: 0x0006A1CB File Offset: 0x000683CB
		internal void Initialize(Type t)
		{
			this.InitializeApartmentDetails();
			if (this.iunknown != IntPtr.Zero)
			{
				return;
			}
			this.iunknown = __ComObject.CreateIUnknown(t);
		}

		// Token: 0x06001C7C RID: 7292 RVA: 0x0006A1F4 File Offset: 0x000683F4
		internal static IntPtr CreateIUnknown(Type t)
		{
			RuntimeHelpers.RunClassConstructor(t.TypeHandle);
			ObjectCreationDelegate objectCreationCallback = ExtensibleClassFactory.GetObjectCreationCallback(t);
			IntPtr intPtr;
			if (objectCreationCallback != null)
			{
				intPtr = objectCreationCallback(IntPtr.Zero);
				if (intPtr == IntPtr.Zero)
				{
					throw new COMException(string.Format("ObjectCreationDelegate for type {0} failed to return a valid COM object", t));
				}
			}
			else
			{
				Marshal.ThrowExceptionForHR(__ComObject.CoCreateInstance(__ComObject.GetCLSID(t), IntPtr.Zero, 21U, __ComObject.IID_IUnknown, out intPtr));
			}
			return intPtr;
		}

		// Token: 0x06001C7D RID: 7293 RVA: 0x0006A260 File Offset: 0x00068460
		private void InitializeApartmentDetails()
		{
			if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
			{
				return;
			}
			this.synchronization_context = SynchronizationContext.Current;
			if (this.synchronization_context != null && this.synchronization_context.GetType() == typeof(SynchronizationContext))
			{
				this.synchronization_context = null;
			}
		}

		// Token: 0x06001C7E RID: 7294 RVA: 0x0006A2B0 File Offset: 0x000684B0
		private static Guid GetCLSID(Type t)
		{
			if (t.IsImport)
			{
				return t.GUID;
			}
			Type baseType = t.BaseType;
			while (baseType != typeof(object))
			{
				if (baseType.IsImport)
				{
					return baseType.GUID;
				}
				baseType = baseType.BaseType;
			}
			throw new COMException("Could not find base COM type for type " + t.ToString());
		}

		// Token: 0x06001C7F RID: 7295
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern IntPtr GetInterfaceInternal(Type t, bool throwException);

		// Token: 0x06001C80 RID: 7296 RVA: 0x0006A312 File Offset: 0x00068512
		internal IntPtr GetInterface(Type t, bool throwException)
		{
			this.CheckIUnknown();
			return this.GetInterfaceInternal(t, throwException);
		}

		// Token: 0x06001C81 RID: 7297 RVA: 0x0006A322 File Offset: 0x00068522
		internal IntPtr GetInterface(Type t)
		{
			return this.GetInterface(t, true);
		}

		// Token: 0x06001C82 RID: 7298 RVA: 0x0006A32C File Offset: 0x0006852C
		private void CheckIUnknown()
		{
			if (this.iunknown == IntPtr.Zero)
			{
				throw new InvalidComObjectException("COM object that has been separated from its underlying RCW cannot be used.");
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06001C83 RID: 7299 RVA: 0x0006A34B File Offset: 0x0006854B
		internal IntPtr IUnknown
		{
			get
			{
				if (this.iunknown == IntPtr.Zero)
				{
					throw new InvalidComObjectException("COM object that has been separated from its underlying RCW cannot be used.");
				}
				return this.iunknown;
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06001C84 RID: 7300 RVA: 0x0006A370 File Offset: 0x00068570
		internal IntPtr IDispatch
		{
			get
			{
				IntPtr @interface = this.GetInterface(typeof(IDispatch));
				if (@interface == IntPtr.Zero)
				{
					throw new InvalidComObjectException("COM object that has been separated from its underlying RCW cannot be used.");
				}
				return @interface;
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06001C85 RID: 7301 RVA: 0x0006A39A File Offset: 0x0006859A
		internal static Guid IID_IUnknown
		{
			get
			{
				return new Guid("00000000-0000-0000-C000-000000000046");
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06001C86 RID: 7302 RVA: 0x0006A3A6 File Offset: 0x000685A6
		internal static Guid IID_IDispatch
		{
			get
			{
				return new Guid("00020400-0000-0000-C000-000000000046");
			}
		}

		// Token: 0x06001C87 RID: 7303 RVA: 0x0006A3B4 File Offset: 0x000685B4
		public override bool Equals(object obj)
		{
			this.CheckIUnknown();
			if (obj == null)
			{
				return false;
			}
			__ComObject _ComObject = obj as __ComObject;
			return _ComObject != null && this.iunknown == _ComObject.IUnknown;
		}

		// Token: 0x06001C88 RID: 7304 RVA: 0x0006A3E9 File Offset: 0x000685E9
		public override int GetHashCode()
		{
			this.CheckIUnknown();
			return this.iunknown.ToInt32();
		}

		// Token: 0x06001C89 RID: 7305
		[DllImport("ole32.dll", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
		private static extern int CoCreateInstance([MarshalAs(UnmanagedType.LPStruct)] [In] Guid rclsid, IntPtr pUnkOuter, uint dwClsContext, [MarshalAs(UnmanagedType.LPStruct)] [In] Guid riid, out IntPtr pUnk);

		// Token: 0x040019DF RID: 6623
		private IntPtr iunknown;

		// Token: 0x040019E0 RID: 6624
		private IntPtr hash_table;

		// Token: 0x040019E1 RID: 6625
		private SynchronizationContext synchronization_context;

		// Token: 0x040019E2 RID: 6626
		private ComInteropProxy proxy;
	}
}
