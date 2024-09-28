﻿using System;

namespace System.Diagnostics.Tracing
{
	/// <summary>Specifies a property should be ignored when writing an event type with the <see cref="M:System.Diagnostics.Tracing.EventSource.Write``1(System.String,System.Diagnostics.Tracing.EventSourceOptions@,``0@)" /> method.</summary>
	// Token: 0x020009F4 RID: 2548
	[AttributeUsage(AttributeTargets.Property)]
	public class EventIgnoreAttribute : Attribute
	{
	}
}
