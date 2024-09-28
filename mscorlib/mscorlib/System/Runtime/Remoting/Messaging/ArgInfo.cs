using System;
using System.Reflection;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000609 RID: 1545
	internal class ArgInfo
	{
		// Token: 0x06003A78 RID: 14968 RVA: 0x000CCECC File Offset: 0x000CB0CC
		public ArgInfo(MethodBase method, ArgInfoType type)
		{
			this._method = method;
			ParameterInfo[] parameters = this._method.GetParameters();
			this._paramMap = new int[parameters.Length];
			this._inoutArgCount = 0;
			if (type == ArgInfoType.In)
			{
				for (int i = 0; i < parameters.Length; i++)
				{
					if (!parameters[i].ParameterType.IsByRef)
					{
						int[] paramMap = this._paramMap;
						int inoutArgCount = this._inoutArgCount;
						this._inoutArgCount = inoutArgCount + 1;
						paramMap[inoutArgCount] = i;
					}
				}
				return;
			}
			for (int j = 0; j < parameters.Length; j++)
			{
				if (parameters[j].ParameterType.IsByRef || parameters[j].IsOut)
				{
					int[] paramMap2 = this._paramMap;
					int inoutArgCount = this._inoutArgCount;
					this._inoutArgCount = inoutArgCount + 1;
					paramMap2[inoutArgCount] = j;
				}
			}
		}

		// Token: 0x06003A79 RID: 14969 RVA: 0x000CCF81 File Offset: 0x000CB181
		public int GetInOutArgIndex(int inoutArgNum)
		{
			return this._paramMap[inoutArgNum];
		}

		// Token: 0x06003A7A RID: 14970 RVA: 0x000CCF8B File Offset: 0x000CB18B
		public virtual string GetInOutArgName(int index)
		{
			return this._method.GetParameters()[this._paramMap[index]].Name;
		}

		// Token: 0x06003A7B RID: 14971 RVA: 0x000CCFA6 File Offset: 0x000CB1A6
		public int GetInOutArgCount()
		{
			return this._inoutArgCount;
		}

		// Token: 0x06003A7C RID: 14972 RVA: 0x000CCFB0 File Offset: 0x000CB1B0
		public object[] GetInOutArgs(object[] args)
		{
			object[] array = new object[this._inoutArgCount];
			for (int i = 0; i < this._inoutArgCount; i++)
			{
				array[i] = args[this._paramMap[i]];
			}
			return array;
		}

		// Token: 0x0400265C RID: 9820
		private int[] _paramMap;

		// Token: 0x0400265D RID: 9821
		private int _inoutArgCount;

		// Token: 0x0400265E RID: 9822
		private MethodBase _method;
	}
}
