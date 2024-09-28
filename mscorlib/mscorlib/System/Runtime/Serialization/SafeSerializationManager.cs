﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security;

namespace System.Runtime.Serialization
{
	// Token: 0x0200066B RID: 1643
	[Serializable]
	internal sealed class SafeSerializationManager : IObjectReference, ISerializable
	{
		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06003D5A RID: 15706 RVA: 0x000D4784 File Offset: 0x000D2984
		// (remove) Token: 0x06003D5B RID: 15707 RVA: 0x000D47BC File Offset: 0x000D29BC
		internal event EventHandler<SafeSerializationEventArgs> SerializeObjectState;

		// Token: 0x06003D5C RID: 15708 RVA: 0x0000259F File Offset: 0x0000079F
		internal SafeSerializationManager()
		{
		}

		// Token: 0x06003D5D RID: 15709 RVA: 0x000D47F4 File Offset: 0x000D29F4
		[SecurityCritical]
		private SafeSerializationManager(SerializationInfo info, StreamingContext context)
		{
			RuntimeType runtimeType = info.GetValueNoThrow("CLR_SafeSerializationManager_RealType", typeof(RuntimeType)) as RuntimeType;
			if (runtimeType == null)
			{
				this.m_serializedStates = (info.GetValue("m_serializedStates", typeof(List<object>)) as List<object>);
				return;
			}
			this.m_realType = runtimeType;
			this.m_savedSerializationInfo = info;
		}

		// Token: 0x1700096D RID: 2413
		// (get) Token: 0x06003D5E RID: 15710 RVA: 0x000D485A File Offset: 0x000D2A5A
		internal bool IsActive
		{
			get
			{
				return this.SerializeObjectState != null;
			}
		}

		// Token: 0x06003D5F RID: 15711 RVA: 0x000D4868 File Offset: 0x000D2A68
		[SecurityCritical]
		internal void CompleteSerialization(object serializedObject, SerializationInfo info, StreamingContext context)
		{
			this.m_serializedStates = null;
			EventHandler<SafeSerializationEventArgs> serializeObjectState = this.SerializeObjectState;
			if (serializeObjectState != null)
			{
				SafeSerializationEventArgs safeSerializationEventArgs = new SafeSerializationEventArgs(context);
				serializeObjectState(serializedObject, safeSerializationEventArgs);
				this.m_serializedStates = safeSerializationEventArgs.SerializedStates;
				info.AddValue("CLR_SafeSerializationManager_RealType", serializedObject.GetType(), typeof(RuntimeType));
				info.SetType(typeof(SafeSerializationManager));
			}
		}

		// Token: 0x06003D60 RID: 15712 RVA: 0x000D48CC File Offset: 0x000D2ACC
		internal void CompleteDeserialization(object deserializedObject)
		{
			if (this.m_serializedStates != null)
			{
				foreach (object obj in this.m_serializedStates)
				{
					((ISafeSerializationData)obj).CompleteDeserialization(deserializedObject);
				}
			}
		}

		// Token: 0x06003D61 RID: 15713 RVA: 0x000D4924 File Offset: 0x000D2B24
		[SecurityCritical]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("m_serializedStates", this.m_serializedStates, typeof(List<IDeserializationCallback>));
		}

		// Token: 0x06003D62 RID: 15714 RVA: 0x000D4944 File Offset: 0x000D2B44
		[SecurityCritical]
		object IObjectReference.GetRealObject(StreamingContext context)
		{
			if (this.m_realObject != null)
			{
				return this.m_realObject;
			}
			if (this.m_realType == null)
			{
				return this;
			}
			Stack stack = new Stack();
			RuntimeType runtimeType = this.m_realType;
			do
			{
				stack.Push(runtimeType);
				runtimeType = (runtimeType.BaseType as RuntimeType);
			}
			while (runtimeType != typeof(object));
			RuntimeType t;
			RuntimeConstructorInfo runtimeConstructorInfo;
			do
			{
				t = runtimeType;
				runtimeType = (stack.Pop() as RuntimeType);
				runtimeConstructorInfo = runtimeType.GetSerializationCtor();
			}
			while (runtimeConstructorInfo != null && runtimeConstructorInfo.IsSecurityCritical);
			runtimeConstructorInfo = ObjectManager.GetConstructor(t);
			object uninitializedObject = FormatterServices.GetUninitializedObject(this.m_realType);
			runtimeConstructorInfo.SerializationInvoke(uninitializedObject, this.m_savedSerializationInfo, context);
			this.m_savedSerializationInfo = null;
			this.m_realType = null;
			this.m_realObject = uninitializedObject;
			return uninitializedObject;
		}

		// Token: 0x06003D63 RID: 15715 RVA: 0x000D4A07 File Offset: 0x000D2C07
		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			if (this.m_realObject != null)
			{
				SerializationEventsCache.GetSerializationEventsForType(this.m_realObject.GetType()).InvokeOnDeserialized(this.m_realObject, context);
				this.m_realObject = null;
			}
		}

		// Token: 0x0400277B RID: 10107
		private IList<object> m_serializedStates;

		// Token: 0x0400277C RID: 10108
		private SerializationInfo m_savedSerializationInfo;

		// Token: 0x0400277D RID: 10109
		private object m_realObject;

		// Token: 0x0400277E RID: 10110
		private RuntimeType m_realType;

		// Token: 0x04002780 RID: 10112
		private const string RealTypeSerializationName = "CLR_SafeSerializationManager_RealType";
	}
}
