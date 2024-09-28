using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.RemoteCalls
{
	// Token: 0x02000093 RID: 147
	public static class RemoteProcedureCalls
	{
		// Token: 0x0600042B RID: 1067 RVA: 0x0000EC44 File Offset: 0x0000CE44
		private static bool CheckIfDelegateExists(Type componentType, RemoteCallType remoteCallType, RemoteCallDelegate func, ushort functionHash)
		{
			if (RemoteProcedureCalls.remoteCallDelegates.ContainsKey(functionHash))
			{
				Invoker invoker = RemoteProcedureCalls.remoteCallDelegates[functionHash];
				if (invoker.AreEqual(componentType, remoteCallType, func))
				{
					return true;
				}
				Debug.LogError(string.Format("Function {0}.{1} and {2}.{3} have the same hash. Please rename one of them. To save bandwidth, we only use 2 bytes for the hash, which has a small chance of collisions.", new object[]
				{
					invoker.componentType,
					invoker.function.GetMethodName(),
					componentType,
					func.GetMethodName()
				}));
			}
			return false;
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0000ECB4 File Offset: 0x0000CEB4
		internal static ushort RegisterDelegate(Type componentType, string functionFullName, RemoteCallType remoteCallType, RemoteCallDelegate func, bool cmdRequiresAuthority = true)
		{
			ushort num = (ushort)(functionFullName.GetStableHashCode() & 65535);
			if (RemoteProcedureCalls.CheckIfDelegateExists(componentType, remoteCallType, func, num))
			{
				return num;
			}
			RemoteProcedureCalls.remoteCallDelegates[num] = new Invoker
			{
				callType = remoteCallType,
				componentType = componentType,
				function = func,
				cmdRequiresAuthority = cmdRequiresAuthority
			};
			return num;
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0000ED0A File Offset: 0x0000CF0A
		public static void RegisterCommand(Type componentType, string functionFullName, RemoteCallDelegate func, bool requiresAuthority)
		{
			RemoteProcedureCalls.RegisterDelegate(componentType, functionFullName, RemoteCallType.Command, func, requiresAuthority);
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0000ED17 File Offset: 0x0000CF17
		public static void RegisterRpc(Type componentType, string functionFullName, RemoteCallDelegate func)
		{
			RemoteProcedureCalls.RegisterDelegate(componentType, functionFullName, RemoteCallType.ClientRpc, func, true);
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0000ED24 File Offset: 0x0000CF24
		internal static void RemoveDelegate(ushort hash)
		{
			RemoteProcedureCalls.remoteCallDelegates.Remove(hash);
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0000ED32 File Offset: 0x0000CF32
		private static bool GetInvokerForHash(ushort functionHash, RemoteCallType remoteCallType, out Invoker invoker)
		{
			return RemoteProcedureCalls.remoteCallDelegates.TryGetValue(functionHash, out invoker) && invoker != null && invoker.callType == remoteCallType;
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0000ED54 File Offset: 0x0000CF54
		internal static bool Invoke(ushort functionHash, RemoteCallType remoteCallType, NetworkReader reader, NetworkBehaviour component, NetworkConnectionToClient senderConnection = null)
		{
			Invoker invoker;
			if (RemoteProcedureCalls.GetInvokerForHash(functionHash, remoteCallType, out invoker) && invoker.componentType.IsInstanceOfType(component))
			{
				invoker.function(component, reader, senderConnection);
				return true;
			}
			return false;
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0000ED8C File Offset: 0x0000CF8C
		internal static bool CommandRequiresAuthority(ushort cmdHash)
		{
			Invoker invoker;
			return RemoteProcedureCalls.GetInvokerForHash(cmdHash, RemoteCallType.Command, out invoker) && invoker.cmdRequiresAuthority;
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0000EDAC File Offset: 0x0000CFAC
		public static RemoteCallDelegate GetDelegate(ushort functionHash)
		{
			Invoker invoker;
			if (!RemoteProcedureCalls.remoteCallDelegates.TryGetValue(functionHash, out invoker))
			{
				return null;
			}
			return invoker.function;
		}

		// Token: 0x040001A6 RID: 422
		private static readonly Dictionary<ushort, Invoker> remoteCallDelegates = new Dictionary<ushort, Invoker>();
	}
}
