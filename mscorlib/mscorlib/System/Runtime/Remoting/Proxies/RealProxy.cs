﻿using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Threading;

namespace System.Runtime.Remoting.Proxies
{
	/// <summary>Provides base functionality for proxies.</summary>
	// Token: 0x02000580 RID: 1408
	[ComVisible(true)]
	[StructLayout(LayoutKind.Sequential)]
	public abstract class RealProxy
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.Proxies.RealProxy" /> class with default values.</summary>
		// Token: 0x06003722 RID: 14114 RVA: 0x000C6F4F File Offset: 0x000C514F
		protected RealProxy()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.Proxies.RealProxy" /> class that represents a remote object of the specified <see cref="T:System.Type" />.</summary>
		/// <param name="classToProxy">The <see cref="T:System.Type" /> of the remote object for which to create a proxy.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="classToProxy" /> is not an interface, and is not derived from <see cref="T:System.MarshalByRefObject" />.</exception>
		// Token: 0x06003723 RID: 14115 RVA: 0x000C6F5E File Offset: 0x000C515E
		protected RealProxy(Type classToProxy) : this(classToProxy, IntPtr.Zero, null)
		{
		}

		// Token: 0x06003724 RID: 14116 RVA: 0x000C6F6D File Offset: 0x000C516D
		internal RealProxy(Type classToProxy, ClientIdentity identity) : this(classToProxy, IntPtr.Zero, null)
		{
			this._objectIdentity = identity;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.Proxies.RealProxy" /> class.</summary>
		/// <param name="classToProxy">The <see cref="T:System.Type" /> of the remote object for which to create a proxy.</param>
		/// <param name="stub">A stub to associate with the new proxy instance.</param>
		/// <param name="stubData">The stub data to set for the specified stub and the new proxy instance.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="classToProxy" /> is not an interface, and is not derived from <see cref="T:System.MarshalByRefObject" />.</exception>
		// Token: 0x06003725 RID: 14117 RVA: 0x000C6F84 File Offset: 0x000C5184
		protected RealProxy(Type classToProxy, IntPtr stub, object stubData)
		{
			if (!classToProxy.IsMarshalByRef && !classToProxy.IsInterface)
			{
				throw new ArgumentException("object must be MarshalByRef");
			}
			this.class_to_proxy = classToProxy;
			if (stub != IntPtr.Zero)
			{
				throw new NotSupportedException("stub is not used in Mono");
			}
		}

		// Token: 0x06003726 RID: 14118
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Type InternalGetProxyType(object transparentProxy);

		/// <summary>Returns the <see cref="T:System.Type" /> of the object that the current instance of <see cref="T:System.Runtime.Remoting.Proxies.RealProxy" /> represents.</summary>
		/// <returns>The <see cref="T:System.Type" /> of the object that the current instance of <see cref="T:System.Runtime.Remoting.Proxies.RealProxy" /> represents.</returns>
		// Token: 0x06003727 RID: 14119 RVA: 0x000C6FD8 File Offset: 0x000C51D8
		public Type GetProxiedType()
		{
			if (this._objTP != null)
			{
				return RealProxy.InternalGetProxyType(this._objTP);
			}
			if (this.class_to_proxy.IsInterface)
			{
				return typeof(MarshalByRefObject);
			}
			return this.class_to_proxy;
		}

		/// <summary>Creates an <see cref="T:System.Runtime.Remoting.ObjRef" /> for the specified object type, and registers it with the remoting infrastructure as a client-activated object.</summary>
		/// <param name="requestedType">The object type that an <see cref="T:System.Runtime.Remoting.ObjRef" /> is created for.</param>
		/// <returns>A new instance of <see cref="T:System.Runtime.Remoting.ObjRef" /> that is created for the specified type.</returns>
		// Token: 0x06003728 RID: 14120 RVA: 0x000C700C File Offset: 0x000C520C
		public virtual ObjRef CreateObjRef(Type requestedType)
		{
			return RemotingServices.Marshal((MarshalByRefObject)this.GetTransparentProxy(), null, requestedType);
		}

		/// <summary>Adds the transparent proxy of the object represented by the current instance of <see cref="T:System.Runtime.Remoting.Proxies.RealProxy" /> to the specified <see cref="T:System.Runtime.Serialization.SerializationInfo" />.</summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> into which the transparent proxy is serialized.</param>
		/// <param name="context">The source and destination of the serialization.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> or <paramref name="context" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have SerializationFormatter permission.</exception>
		// Token: 0x06003729 RID: 14121 RVA: 0x000C7020 File Offset: 0x000C5220
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			RemotingServices.GetObjectData(this.GetTransparentProxy(), info, context);
		}

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x0600372A RID: 14122 RVA: 0x000C702F File Offset: 0x000C522F
		// (set) Token: 0x0600372B RID: 14123 RVA: 0x000C7037 File Offset: 0x000C5237
		internal Identity ObjectIdentity
		{
			get
			{
				return this._objectIdentity;
			}
			set
			{
				this._objectIdentity = value;
			}
		}

		/// <summary>Requests an unmanaged reference to the object represented by the current proxy instance.</summary>
		/// <param name="fIsMarshalled">
		///   <see langword="true" /> if the object reference is requested for marshaling to a remote location; <see langword="false" /> if the object reference is requested for communication with unmanaged objects in the current process through COM.</param>
		/// <returns>A pointer to a COM Callable Wrapper if the object reference is requested for communication with unmanaged objects in the current process through COM, or a pointer to a cached or newly generated <see langword="IUnknown" /> COM interface if the object reference is requested for marshaling to a remote location.</returns>
		// Token: 0x0600372C RID: 14124 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public virtual IntPtr GetCOMIUnknown(bool fIsMarshalled)
		{
			throw new NotImplementedException();
		}

		/// <summary>Stores an unmanaged proxy of the object that is represented by the current instance.</summary>
		/// <param name="i">A pointer to the <see langword="IUnknown" /> interface for the object that is represented by the current proxy instance.</param>
		// Token: 0x0600372D RID: 14125 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public virtual void SetCOMIUnknown(IntPtr i)
		{
			throw new NotImplementedException();
		}

		/// <summary>Requests a COM interface with the specified ID.</summary>
		/// <param name="iid">A reference to the requested interface.</param>
		/// <returns>A pointer to the requested interface.</returns>
		// Token: 0x0600372E RID: 14126 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public virtual IntPtr SupportsInterface(ref Guid iid)
		{
			throw new NotImplementedException();
		}

		/// <summary>Retrieves stub data that is stored for the specified proxy.</summary>
		/// <param name="rp">The proxy for which stub data is requested.</param>
		/// <returns>Stub data for the specified proxy.</returns>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have UnmanagedCode permission.</exception>
		// Token: 0x0600372F RID: 14127 RVA: 0x000C7040 File Offset: 0x000C5240
		public static object GetStubData(RealProxy rp)
		{
			return rp._stubData;
		}

		/// <summary>Sets the stub data for the specified proxy.</summary>
		/// <param name="rp">The proxy for which to set stub data.</param>
		/// <param name="stubData">The new stub data.</param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have UnmanagedCode permission.</exception>
		// Token: 0x06003730 RID: 14128 RVA: 0x000C7048 File Offset: 0x000C5248
		public static void SetStubData(RealProxy rp, object stubData)
		{
			rp._stubData = stubData;
		}

		/// <summary>When overridden in a derived class, invokes the method that is specified in the provided <see cref="T:System.Runtime.Remoting.Messaging.IMessage" /> on the remote object that is represented by the current instance.</summary>
		/// <param name="msg">A <see cref="T:System.Runtime.Remoting.Messaging.IMessage" /> that contains a <see cref="T:System.Collections.IDictionary" /> of information about the method call.</param>
		/// <returns>The message returned by the invoked method, containing the return value and any <see langword="out" /> or <see langword="ref" /> parameters.</returns>
		// Token: 0x06003731 RID: 14129
		public abstract IMessage Invoke(IMessage msg);

		// Token: 0x06003732 RID: 14130 RVA: 0x000C7054 File Offset: 0x000C5254
		internal static object PrivateInvoke(RealProxy rp, IMessage msg, out Exception exc, out object[] out_args)
		{
			MonoMethodMessage monoMethodMessage = (MonoMethodMessage)msg;
			monoMethodMessage.LogicalCallContext = Thread.CurrentThread.GetMutableExecutionContext().LogicalCallContext;
			CallType callType = monoMethodMessage.CallType;
			bool flag = rp is RemotingProxy;
			out_args = null;
			IMethodReturnMessage methodReturnMessage = null;
			if (callType == CallType.BeginInvoke)
			{
				monoMethodMessage.AsyncResult.CallMessage = monoMethodMessage;
			}
			if (callType == CallType.EndInvoke)
			{
				methodReturnMessage = (IMethodReturnMessage)monoMethodMessage.AsyncResult.EndInvoke();
			}
			if (monoMethodMessage.MethodBase.IsConstructor)
			{
				if (flag)
				{
					methodReturnMessage = (IMethodReturnMessage)(rp as RemotingProxy).ActivateRemoteObject((IMethodMessage)msg);
				}
				else
				{
					msg = new ConstructionCall(rp.GetProxiedType());
				}
			}
			if (methodReturnMessage == null)
			{
				bool flag2 = false;
				try
				{
					methodReturnMessage = (IMethodReturnMessage)rp.Invoke(msg);
				}
				catch (Exception e)
				{
					flag2 = true;
					if (callType != CallType.BeginInvoke)
					{
						throw;
					}
					monoMethodMessage.AsyncResult.SyncProcessMessage(new ReturnMessage(e, msg as IMethodCallMessage));
					methodReturnMessage = new ReturnMessage(null, null, 0, null, msg as IMethodCallMessage);
				}
				if (!flag && callType == CallType.BeginInvoke && !flag2)
				{
					object ret = monoMethodMessage.AsyncResult.SyncProcessMessage(methodReturnMessage);
					out_args = methodReturnMessage.OutArgs;
					methodReturnMessage = new ReturnMessage(ret, null, 0, null, methodReturnMessage as IMethodCallMessage);
				}
			}
			if (methodReturnMessage.LogicalCallContext != null && methodReturnMessage.LogicalCallContext.HasInfo)
			{
				Thread.CurrentThread.GetMutableExecutionContext().LogicalCallContext.Merge(methodReturnMessage.LogicalCallContext);
			}
			exc = methodReturnMessage.Exception;
			if (exc != null)
			{
				out_args = null;
				throw exc.FixRemotingException();
			}
			if (methodReturnMessage is IConstructionReturnMessage)
			{
				if (out_args == null)
				{
					out_args = methodReturnMessage.OutArgs;
				}
			}
			else if (monoMethodMessage.CallType != CallType.BeginInvoke)
			{
				if (monoMethodMessage.CallType == CallType.Sync)
				{
					out_args = RealProxy.ProcessResponse(methodReturnMessage, monoMethodMessage);
				}
				else if (monoMethodMessage.CallType == CallType.EndInvoke)
				{
					out_args = RealProxy.ProcessResponse(methodReturnMessage, monoMethodMessage.AsyncResult.CallMessage);
				}
				else if (out_args == null)
				{
					out_args = methodReturnMessage.OutArgs;
				}
			}
			return methodReturnMessage.ReturnValue;
		}

		// Token: 0x06003733 RID: 14131
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal virtual extern object InternalGetTransparentProxy(string className);

		/// <summary>Returns the transparent proxy for the current instance of <see cref="T:System.Runtime.Remoting.Proxies.RealProxy" />.</summary>
		/// <returns>The transparent proxy for the current proxy instance.</returns>
		// Token: 0x06003734 RID: 14132 RVA: 0x000C7224 File Offset: 0x000C5424
		public virtual object GetTransparentProxy()
		{
			if (this._objTP == null)
			{
				IRemotingTypeInfo remotingTypeInfo = this as IRemotingTypeInfo;
				string text;
				if (remotingTypeInfo != null)
				{
					text = remotingTypeInfo.TypeName;
					if (text == null || text == typeof(MarshalByRefObject).AssemblyQualifiedName)
					{
						text = this.class_to_proxy.AssemblyQualifiedName;
					}
				}
				else
				{
					text = this.class_to_proxy.AssemblyQualifiedName;
				}
				this._objTP = this.InternalGetTransparentProxy(text);
			}
			return this._objTP;
		}

		/// <summary>Initializes a new instance of the object <see cref="T:System.Type" /> of the remote object that the current instance of <see cref="T:System.Runtime.Remoting.Proxies.RealProxy" /> represents with the specified <see cref="T:System.Runtime.Remoting.Activation.IConstructionCallMessage" />.</summary>
		/// <param name="ctorMsg">A construction call message that contains the constructor parameters for the new instance of the remote object that is represented by the current <see cref="T:System.Runtime.Remoting.Proxies.RealProxy" />. Can be <see langword="null" />.</param>
		/// <returns>The result of the construction request.</returns>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have UnmanagedCode permission.</exception>
		// Token: 0x06003735 RID: 14133 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		[ComVisible(true)]
		public IConstructionReturnMessage InitializeServerObject(IConstructionCallMessage ctorMsg)
		{
			throw new NotImplementedException();
		}

		/// <summary>Attaches the current proxy instance to the specified remote <see cref="T:System.MarshalByRefObject" />.</summary>
		/// <param name="s">The <see cref="T:System.MarshalByRefObject" /> that the current proxy instance represents.</param>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have UnmanagedCode permission.</exception>
		// Token: 0x06003736 RID: 14134 RVA: 0x000C7291 File Offset: 0x000C5491
		protected void AttachServer(MarshalByRefObject s)
		{
			this._server = s;
		}

		/// <summary>Detaches the current proxy instance from the remote server object that it represents.</summary>
		/// <returns>The detached server object.</returns>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have UnmanagedCode permission.</exception>
		// Token: 0x06003737 RID: 14135 RVA: 0x000C729A File Offset: 0x000C549A
		protected MarshalByRefObject DetachServer()
		{
			MarshalByRefObject server = this._server;
			this._server = null;
			return server;
		}

		/// <summary>Returns the server object that is represented by the current proxy instance.</summary>
		/// <returns>The server object that is represented by the current proxy instance.</returns>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have UnmanagedCode permission.</exception>
		// Token: 0x06003738 RID: 14136 RVA: 0x000C72A9 File Offset: 0x000C54A9
		protected MarshalByRefObject GetUnwrappedServer()
		{
			return this._server;
		}

		// Token: 0x06003739 RID: 14137 RVA: 0x000C72B1 File Offset: 0x000C54B1
		internal void SetTargetDomain(int domainId)
		{
			this._targetDomainId = domainId;
		}

		// Token: 0x0600373A RID: 14138 RVA: 0x000C72BC File Offset: 0x000C54BC
		internal object GetAppDomainTarget()
		{
			if (this._server == null)
			{
				ClientActivatedIdentity clientActivatedIdentity = RemotingServices.GetIdentityForUri(this._targetUri) as ClientActivatedIdentity;
				if (clientActivatedIdentity == null)
				{
					throw new RemotingException("Server for uri '" + this._targetUri + "' not found");
				}
				this._server = clientActivatedIdentity.GetServerObject();
			}
			return this._server;
		}

		// Token: 0x0600373B RID: 14139 RVA: 0x000C7314 File Offset: 0x000C5514
		private static object[] ProcessResponse(IMethodReturnMessage mrm, MonoMethodMessage call)
		{
			MethodInfo methodInfo = (MethodInfo)call.MethodBase;
			if (mrm.ReturnValue != null && !methodInfo.ReturnType.IsInstanceOfType(mrm.ReturnValue))
			{
				throw new InvalidCastException("Return value has an invalid type");
			}
			int num;
			if (call.NeedsOutProcessing(out num))
			{
				ParameterInfo[] parameters = methodInfo.GetParameters();
				object[] array = new object[num];
				int num2 = 0;
				foreach (ParameterInfo parameterInfo in parameters)
				{
					if (parameterInfo.IsOut && !parameterInfo.ParameterType.IsByRef)
					{
						object obj = (parameterInfo.Position < mrm.ArgCount) ? mrm.GetArg(parameterInfo.Position) : null;
						if (obj != null)
						{
							object arg = call.GetArg(parameterInfo.Position);
							if (arg == null)
							{
								throw new RemotingException("Unexpected null value in local out parameter '" + parameterInfo.Name + "'");
							}
							RemotingServices.UpdateOutArgObject(parameterInfo, arg, obj);
						}
					}
					else if (parameterInfo.ParameterType.IsByRef)
					{
						object obj2 = (parameterInfo.Position < mrm.ArgCount) ? mrm.GetArg(parameterInfo.Position) : null;
						if (obj2 != null && !parameterInfo.ParameterType.GetElementType().IsInstanceOfType(obj2))
						{
							throw new InvalidCastException("Return argument '" + parameterInfo.Name + "' has an invalid type");
						}
						array[num2++] = obj2;
					}
				}
				return array;
			}
			return new object[0];
		}

		// Token: 0x0400257A RID: 9594
		private Type class_to_proxy;

		// Token: 0x0400257B RID: 9595
		internal Context _targetContext;

		// Token: 0x0400257C RID: 9596
		internal MarshalByRefObject _server;

		// Token: 0x0400257D RID: 9597
		private int _targetDomainId = -1;

		// Token: 0x0400257E RID: 9598
		internal string _targetUri;

		// Token: 0x0400257F RID: 9599
		internal Identity _objectIdentity;

		// Token: 0x04002580 RID: 9600
		private object _objTP;

		// Token: 0x04002581 RID: 9601
		private object _stubData;
	}
}
