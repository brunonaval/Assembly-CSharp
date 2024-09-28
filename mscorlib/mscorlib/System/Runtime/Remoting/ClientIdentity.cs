using System;

namespace System.Runtime.Remoting
{
	// Token: 0x02000561 RID: 1377
	internal class ClientIdentity : Identity
	{
		// Token: 0x060035F9 RID: 13817 RVA: 0x000C24B3 File Offset: 0x000C06B3
		public ClientIdentity(string objectUri, ObjRef objRef) : base(objectUri)
		{
			this._objRef = objRef;
			this._envoySink = ((this._objRef.EnvoyInfo != null) ? this._objRef.EnvoyInfo.EnvoySinks : null);
		}

		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x060035FA RID: 13818 RVA: 0x000C24E9 File Offset: 0x000C06E9
		// (set) Token: 0x060035FB RID: 13819 RVA: 0x000C2502 File Offset: 0x000C0702
		public MarshalByRefObject ClientProxy
		{
			get
			{
				WeakReference proxyReference = this._proxyReference;
				return (MarshalByRefObject)((proxyReference != null) ? proxyReference.Target : null);
			}
			set
			{
				this._proxyReference = new WeakReference(value);
			}
		}

		// Token: 0x060035FC RID: 13820 RVA: 0x000C2510 File Offset: 0x000C0710
		public override ObjRef CreateObjRef(Type requestedType)
		{
			return this._objRef;
		}

		// Token: 0x1700078E RID: 1934
		// (get) Token: 0x060035FD RID: 13821 RVA: 0x000C2518 File Offset: 0x000C0718
		public string TargetUri
		{
			get
			{
				return this._objRef.URI;
			}
		}

		// Token: 0x04002522 RID: 9506
		private WeakReference _proxyReference;
	}
}
