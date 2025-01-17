﻿using System;
using System.Collections;
using System.Globalization;
using System.IO;

namespace System.Runtime.Serialization
{
	/// <summary>Provides base functionality for the common language runtime serialization formatters.</summary>
	// Token: 0x02000650 RID: 1616
	[CLSCompliant(false)]
	[Serializable]
	public abstract class Formatter : IFormatter
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Serialization.Formatter" /> class.</summary>
		// Token: 0x06003C69 RID: 15465 RVA: 0x000D160E File Offset: 0x000CF80E
		protected Formatter()
		{
			this.m_objectQueue = new Queue();
			this.m_idGenerator = new ObjectIDGenerator();
		}

		/// <summary>When overridden in a derived class, deserializes the stream attached to the formatter when it was created, creating a graph of objects identical to the graph originally serialized into that stream.</summary>
		/// <param name="serializationStream">The stream to deserialize.</param>
		/// <returns>The top object of the deserialized graph of objects.</returns>
		// Token: 0x06003C6A RID: 15466
		public abstract object Deserialize(Stream serializationStream);

		/// <summary>Returns the next object to serialize, from the formatter's internal work queue.</summary>
		/// <param name="objID">The ID assigned to the current object during serialization.</param>
		/// <returns>The next object to serialize.</returns>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The next object retrieved from the work queue did not have an assigned ID.</exception>
		// Token: 0x06003C6B RID: 15467 RVA: 0x000D162C File Offset: 0x000CF82C
		protected virtual object GetNext(out long objID)
		{
			if (this.m_objectQueue.Count == 0)
			{
				objID = 0L;
				return null;
			}
			object obj = this.m_objectQueue.Dequeue();
			bool flag;
			objID = this.m_idGenerator.HasId(obj, out flag);
			if (flag)
			{
				throw new SerializationException("Object has never been assigned an objectID");
			}
			return obj;
		}

		/// <summary>Schedules an object for later serialization.</summary>
		/// <param name="obj">The object to schedule for serialization.</param>
		/// <returns>The object ID assigned to the object.</returns>
		// Token: 0x06003C6C RID: 15468 RVA: 0x000D1678 File Offset: 0x000CF878
		protected virtual long Schedule(object obj)
		{
			if (obj == null)
			{
				return 0L;
			}
			bool flag;
			long id = this.m_idGenerator.GetId(obj, out flag);
			if (flag)
			{
				this.m_objectQueue.Enqueue(obj);
			}
			return id;
		}

		/// <summary>When overridden in a derived class, serializes the graph of objects with the specified root to the stream already attached to the formatter.</summary>
		/// <param name="serializationStream">The stream to which the objects are serialized.</param>
		/// <param name="graph">The object at the root of the graph to serialize.</param>
		// Token: 0x06003C6D RID: 15469
		public abstract void Serialize(Stream serializationStream, object graph);

		/// <summary>When overridden in a derived class, writes an array to the stream already attached to the formatter.</summary>
		/// <param name="obj">The array to write.</param>
		/// <param name="name">The name of the array.</param>
		/// <param name="memberType">The type of elements that the array holds.</param>
		// Token: 0x06003C6E RID: 15470
		protected abstract void WriteArray(object obj, string name, Type memberType);

		/// <summary>When overridden in a derived class, writes a Boolean value to the stream already attached to the formatter.</summary>
		/// <param name="val">The value to write.</param>
		/// <param name="name">The name of the member.</param>
		// Token: 0x06003C6F RID: 15471
		protected abstract void WriteBoolean(bool val, string name);

		/// <summary>When overridden in a derived class, writes an 8-bit unsigned integer to the stream already attached to the formatter.</summary>
		/// <param name="val">The value to write.</param>
		/// <param name="name">The name of the member.</param>
		// Token: 0x06003C70 RID: 15472
		protected abstract void WriteByte(byte val, string name);

		/// <summary>When overridden in a derived class, writes a Unicode character to the stream already attached to the formatter.</summary>
		/// <param name="val">The value to write.</param>
		/// <param name="name">The name of the member.</param>
		// Token: 0x06003C71 RID: 15473
		protected abstract void WriteChar(char val, string name);

		/// <summary>When overridden in a derived class, writes a <see cref="T:System.DateTime" /> value to the stream already attached to the formatter.</summary>
		/// <param name="val">The value to write.</param>
		/// <param name="name">The name of the member.</param>
		// Token: 0x06003C72 RID: 15474
		protected abstract void WriteDateTime(DateTime val, string name);

		/// <summary>When overridden in a derived class, writes a <see cref="T:System.Decimal" /> value to the stream already attached to the formatter.</summary>
		/// <param name="val">The value to write.</param>
		/// <param name="name">The name of the member.</param>
		// Token: 0x06003C73 RID: 15475
		protected abstract void WriteDecimal(decimal val, string name);

		/// <summary>When overridden in a derived class, writes a double-precision floating-point number to the stream already attached to the formatter.</summary>
		/// <param name="val">The value to write.</param>
		/// <param name="name">The name of the member.</param>
		// Token: 0x06003C74 RID: 15476
		protected abstract void WriteDouble(double val, string name);

		/// <summary>When overridden in a derived class, writes a 16-bit signed integer to the stream already attached to the formatter.</summary>
		/// <param name="val">The value to write.</param>
		/// <param name="name">The name of the member.</param>
		// Token: 0x06003C75 RID: 15477
		protected abstract void WriteInt16(short val, string name);

		/// <summary>When overridden in a derived class, writes a 32-bit signed integer to the stream.</summary>
		/// <param name="val">The value to write.</param>
		/// <param name="name">The name of the member.</param>
		// Token: 0x06003C76 RID: 15478
		protected abstract void WriteInt32(int val, string name);

		/// <summary>When overridden in a derived class, writes a 64-bit signed integer to the stream.</summary>
		/// <param name="val">The value to write.</param>
		/// <param name="name">The name of the member.</param>
		// Token: 0x06003C77 RID: 15479
		protected abstract void WriteInt64(long val, string name);

		/// <summary>When overridden in a derived class, writes an object reference to the stream already attached to the formatter.</summary>
		/// <param name="obj">The object reference to write.</param>
		/// <param name="name">The name of the member.</param>
		/// <param name="memberType">The type of object the reference points to.</param>
		// Token: 0x06003C78 RID: 15480
		protected abstract void WriteObjectRef(object obj, string name, Type memberType);

		/// <summary>Inspects the type of data received, and calls the appropriate <see langword="Write" /> method to perform the write to the stream already attached to the formatter.</summary>
		/// <param name="memberName">The name of the member to serialize.</param>
		/// <param name="data">The object to write to the stream attached to the formatter.</param>
		// Token: 0x06003C79 RID: 15481 RVA: 0x000D16A8 File Offset: 0x000CF8A8
		protected virtual void WriteMember(string memberName, object data)
		{
			if (data == null)
			{
				this.WriteObjectRef(data, memberName, typeof(object));
				return;
			}
			Type type = data.GetType();
			if (type == typeof(bool))
			{
				this.WriteBoolean(Convert.ToBoolean(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(char))
			{
				this.WriteChar(Convert.ToChar(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(sbyte))
			{
				this.WriteSByte(Convert.ToSByte(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(byte))
			{
				this.WriteByte(Convert.ToByte(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(short))
			{
				this.WriteInt16(Convert.ToInt16(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(int))
			{
				this.WriteInt32(Convert.ToInt32(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(long))
			{
				this.WriteInt64(Convert.ToInt64(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(float))
			{
				this.WriteSingle(Convert.ToSingle(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(double))
			{
				this.WriteDouble(Convert.ToDouble(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(DateTime))
			{
				this.WriteDateTime(Convert.ToDateTime(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(decimal))
			{
				this.WriteDecimal(Convert.ToDecimal(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(ushort))
			{
				this.WriteUInt16(Convert.ToUInt16(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(uint))
			{
				this.WriteUInt32(Convert.ToUInt32(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type == typeof(ulong))
			{
				this.WriteUInt64(Convert.ToUInt64(data, CultureInfo.InvariantCulture), memberName);
				return;
			}
			if (type.IsArray)
			{
				this.WriteArray(data, memberName, type);
				return;
			}
			if (type.IsValueType)
			{
				this.WriteValueType(data, memberName, type);
				return;
			}
			this.WriteObjectRef(data, memberName, type);
		}

		/// <summary>When overridden in a derived class, writes an 8-bit signed integer to the stream already attached to the formatter.</summary>
		/// <param name="val">The value to write.</param>
		/// <param name="name">The name of the member.</param>
		// Token: 0x06003C7A RID: 15482
		[CLSCompliant(false)]
		protected abstract void WriteSByte(sbyte val, string name);

		/// <summary>When overridden in a derived class, writes a single-precision floating-point number to the stream already attached to the formatter.</summary>
		/// <param name="val">The value to write.</param>
		/// <param name="name">The name of the member.</param>
		// Token: 0x06003C7B RID: 15483
		protected abstract void WriteSingle(float val, string name);

		/// <summary>When overridden in a derived class, writes a <see cref="T:System.TimeSpan" /> value to the stream already attached to the formatter.</summary>
		/// <param name="val">The value to write.</param>
		/// <param name="name">The name of the member.</param>
		// Token: 0x06003C7C RID: 15484
		protected abstract void WriteTimeSpan(TimeSpan val, string name);

		/// <summary>When overridden in a derived class, writes a 16-bit unsigned integer to the stream already attached to the formatter.</summary>
		/// <param name="val">The value to write.</param>
		/// <param name="name">The name of the member.</param>
		// Token: 0x06003C7D RID: 15485
		[CLSCompliant(false)]
		protected abstract void WriteUInt16(ushort val, string name);

		/// <summary>When overridden in a derived class, writes a 32-bit unsigned integer to the stream already attached to the formatter.</summary>
		/// <param name="val">The value to write.</param>
		/// <param name="name">The name of the member.</param>
		// Token: 0x06003C7E RID: 15486
		[CLSCompliant(false)]
		protected abstract void WriteUInt32(uint val, string name);

		/// <summary>When overridden in a derived class, writes a 64-bit unsigned integer to the stream already attached to the formatter.</summary>
		/// <param name="val">The value to write.</param>
		/// <param name="name">The name of the member.</param>
		// Token: 0x06003C7F RID: 15487
		[CLSCompliant(false)]
		protected abstract void WriteUInt64(ulong val, string name);

		/// <summary>When overridden in a derived class, writes a value of the given type to the stream already attached to the formatter.</summary>
		/// <param name="obj">The object representing the value type.</param>
		/// <param name="name">The name of the member.</param>
		/// <param name="memberType">The <see cref="T:System.Type" /> of the value type.</param>
		// Token: 0x06003C80 RID: 15488
		protected abstract void WriteValueType(object obj, string name, Type memberType);

		/// <summary>When overridden in a derived class, gets or sets the <see cref="T:System.Runtime.Serialization.ISurrogateSelector" /> used with the current formatter.</summary>
		/// <returns>The <see cref="T:System.Runtime.Serialization.ISurrogateSelector" /> used with the current formatter.</returns>
		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x06003C81 RID: 15489
		// (set) Token: 0x06003C82 RID: 15490
		public abstract ISurrogateSelector SurrogateSelector { get; set; }

		/// <summary>When overridden in a derived class, gets or sets the <see cref="T:System.Runtime.Serialization.SerializationBinder" /> used with the current formatter.</summary>
		/// <returns>The <see cref="T:System.Runtime.Serialization.SerializationBinder" /> used with the current formatter.</returns>
		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x06003C83 RID: 15491
		// (set) Token: 0x06003C84 RID: 15492
		public abstract SerializationBinder Binder { get; set; }

		/// <summary>When overridden in a derived class, gets or sets the <see cref="T:System.Runtime.Serialization.StreamingContext" /> used for the current serialization.</summary>
		/// <returns>The <see cref="T:System.Runtime.Serialization.StreamingContext" /> used for the current serialization.</returns>
		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x06003C85 RID: 15493
		// (set) Token: 0x06003C86 RID: 15494
		public abstract StreamingContext Context { get; set; }

		/// <summary>Contains the <see cref="T:System.Runtime.Serialization.ObjectIDGenerator" /> used with the current formatter.</summary>
		// Token: 0x0400271F RID: 10015
		protected ObjectIDGenerator m_idGenerator;

		/// <summary>Contains a <see cref="T:System.Collections.Queue" /> of the objects left to serialize.</summary>
		// Token: 0x04002720 RID: 10016
		protected Queue m_objectQueue;
	}
}
