﻿using System;
using System.Collections;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Security;

namespace System.Runtime.Serialization
{
	// Token: 0x0200065F RID: 1631
	internal static class ObjectCloneHelper
	{
		// Token: 0x06003CE2 RID: 15586 RVA: 0x000D2868 File Offset: 0x000D0A68
		[SecurityCritical]
		internal static object GetObjectData(object serObj, out string typeName, out string assemName, out string[] fieldNames, out object[] fieldValues)
		{
			object obj = null;
			Type type;
			if (RemotingServices.IsTransparentProxy(serObj))
			{
				type = typeof(MarshalByRefObject);
			}
			else
			{
				type = serObj.GetType();
			}
			SerializationInfo serializationInfo = new SerializationInfo(type, ObjectCloneHelper.s_converter);
			if (serObj is ObjRef)
			{
				ObjectCloneHelper.s_ObjRefRemotingSurrogate.GetObjectData(serObj, serializationInfo, ObjectCloneHelper.s_cloneContext);
			}
			else if (RemotingServices.IsTransparentProxy(serObj) || serObj is MarshalByRefObject)
			{
				if (obj == null)
				{
					ObjectCloneHelper.s_RemotingSurrogate.GetObjectData(serObj, serializationInfo, ObjectCloneHelper.s_cloneContext);
				}
			}
			else
			{
				if (!(serObj is ISerializable))
				{
					throw new ArgumentException(Environment.GetResourceString("Serialization error."));
				}
				((ISerializable)serObj).GetObjectData(serializationInfo, ObjectCloneHelper.s_cloneContext);
			}
			if (obj == null)
			{
				typeName = serializationInfo.FullTypeName;
				assemName = serializationInfo.AssemblyName;
				fieldNames = serializationInfo.MemberNames;
				fieldValues = serializationInfo.MemberValues;
			}
			else
			{
				typeName = null;
				assemName = null;
				fieldNames = null;
				fieldValues = null;
			}
			return obj;
		}

		// Token: 0x06003CE3 RID: 15587 RVA: 0x000D2944 File Offset: 0x000D0B44
		[SecurityCritical]
		internal static SerializationInfo PrepareConstructorArgs(object serObj, string[] fieldNames, object[] fieldValues, out StreamingContext context)
		{
			SerializationInfo serializationInfo = null;
			if (serObj is ISerializable)
			{
				serializationInfo = new SerializationInfo(serObj.GetType(), ObjectCloneHelper.s_converter);
				for (int i = 0; i < fieldNames.Length; i++)
				{
					if (fieldNames[i] != null)
					{
						serializationInfo.AddValue(fieldNames[i], fieldValues[i]);
					}
				}
			}
			else
			{
				Hashtable hashtable = new Hashtable();
				int j = 0;
				int num = 0;
				while (j < fieldNames.Length)
				{
					if (fieldNames[j] != null)
					{
						hashtable[fieldNames[j]] = fieldValues[j];
						num++;
					}
					j++;
				}
				MemberInfo[] serializableMembers = FormatterServices.GetSerializableMembers(serObj.GetType());
				for (int k = 0; k < serializableMembers.Length; k++)
				{
					string name = serializableMembers[k].Name;
					if (!hashtable.Contains(name))
					{
						object[] customAttributes = serializableMembers[k].GetCustomAttributes(typeof(OptionalFieldAttribute), false);
						if (customAttributes == null || customAttributes.Length == 0)
						{
							throw new SerializationException(Environment.GetResourceString("Member '{0}' in class '{1}' is not present in the serialized stream and is not marked with {2}.", new object[]
							{
								serializableMembers[k],
								serObj.GetType(),
								typeof(OptionalFieldAttribute).FullName
							}));
						}
					}
					else
					{
						object value = hashtable[name];
						FormatterServices.SerializationSetValue(serializableMembers[k], serObj, value);
					}
				}
			}
			context = ObjectCloneHelper.s_cloneContext;
			return serializationInfo;
		}

		// Token: 0x04002738 RID: 10040
		private static readonly IFormatterConverter s_converter = new FormatterConverter();

		// Token: 0x04002739 RID: 10041
		private static readonly StreamingContext s_cloneContext = new StreamingContext(StreamingContextStates.CrossAppDomain);

		// Token: 0x0400273A RID: 10042
		private static readonly ISerializationSurrogate s_RemotingSurrogate = new RemotingSurrogate();

		// Token: 0x0400273B RID: 10043
		private static readonly ISerializationSurrogate s_ObjRefRemotingSurrogate = new ObjRefSurrogate();
	}
}
