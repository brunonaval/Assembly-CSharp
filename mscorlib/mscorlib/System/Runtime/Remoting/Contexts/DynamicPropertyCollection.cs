﻿using System;
using System.Collections;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Contexts
{
	// Token: 0x0200058C RID: 1420
	internal class DynamicPropertyCollection
	{
		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x060037AE RID: 14254 RVA: 0x000C87E0 File Offset: 0x000C69E0
		public bool HasProperties
		{
			get
			{
				return this._properties.Count > 0;
			}
		}

		// Token: 0x060037AF RID: 14255 RVA: 0x000C87F0 File Offset: 0x000C69F0
		public bool RegisterDynamicProperty(IDynamicProperty prop)
		{
			bool result;
			lock (this)
			{
				if (this.FindProperty(prop.Name) != -1)
				{
					throw new InvalidOperationException("Another property by this name already exists");
				}
				ArrayList arrayList = new ArrayList(this._properties);
				DynamicPropertyCollection.DynamicPropertyReg dynamicPropertyReg = new DynamicPropertyCollection.DynamicPropertyReg();
				dynamicPropertyReg.Property = prop;
				IContributeDynamicSink contributeDynamicSink = prop as IContributeDynamicSink;
				if (contributeDynamicSink != null)
				{
					dynamicPropertyReg.Sink = contributeDynamicSink.GetDynamicSink();
				}
				arrayList.Add(dynamicPropertyReg);
				this._properties = arrayList;
				result = true;
			}
			return result;
		}

		// Token: 0x060037B0 RID: 14256 RVA: 0x000C8888 File Offset: 0x000C6A88
		public bool UnregisterDynamicProperty(string name)
		{
			bool result;
			lock (this)
			{
				int num = this.FindProperty(name);
				if (num == -1)
				{
					throw new RemotingException("A property with the name " + name + " was not found");
				}
				this._properties.RemoveAt(num);
				result = true;
			}
			return result;
		}

		// Token: 0x060037B1 RID: 14257 RVA: 0x000C88F0 File Offset: 0x000C6AF0
		public void NotifyMessage(bool start, IMessage msg, bool client_site, bool async)
		{
			ArrayList properties = this._properties;
			if (start)
			{
				using (IEnumerator enumerator = properties.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						DynamicPropertyCollection.DynamicPropertyReg dynamicPropertyReg = (DynamicPropertyCollection.DynamicPropertyReg)obj;
						if (dynamicPropertyReg.Sink != null)
						{
							dynamicPropertyReg.Sink.ProcessMessageStart(msg, client_site, async);
						}
					}
					return;
				}
			}
			foreach (object obj2 in properties)
			{
				DynamicPropertyCollection.DynamicPropertyReg dynamicPropertyReg2 = (DynamicPropertyCollection.DynamicPropertyReg)obj2;
				if (dynamicPropertyReg2.Sink != null)
				{
					dynamicPropertyReg2.Sink.ProcessMessageFinish(msg, client_site, async);
				}
			}
		}

		// Token: 0x060037B2 RID: 14258 RVA: 0x000C89B4 File Offset: 0x000C6BB4
		private int FindProperty(string name)
		{
			for (int i = 0; i < this._properties.Count; i++)
			{
				if (((DynamicPropertyCollection.DynamicPropertyReg)this._properties[i]).Property.Name == name)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x040025AE RID: 9646
		private ArrayList _properties = new ArrayList();

		// Token: 0x0200058D RID: 1421
		private class DynamicPropertyReg
		{
			// Token: 0x040025AF RID: 9647
			public IDynamicProperty Property;

			// Token: 0x040025B0 RID: 9648
			public IDynamicMessageSink Sink;
		}
	}
}
