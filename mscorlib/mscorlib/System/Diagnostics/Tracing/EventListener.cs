﻿using System;
using System.Collections.Generic;

namespace System.Diagnostics.Tracing
{
	/// <summary>Provides methods for enabling and disabling events from event sources.</summary>
	// Token: 0x020009F5 RID: 2549
	public class EventListener : IDisposable
	{
		/// <summary>Gets a small non-negative number that represents the specified event source.</summary>
		/// <param name="eventSource">The event source to find the index for.</param>
		/// <returns>A small non-negative number that represents the specified event source.</returns>
		// Token: 0x06005AC6 RID: 23238 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public static int EventSourceIndex(EventSource eventSource)
		{
			return 0;
		}

		/// <summary>Enables events for the specified event source that has the specified verbosity level or lower.</summary>
		/// <param name="eventSource">The event source to enable events for.</param>
		/// <param name="level">The level of events to enable.</param>
		// Token: 0x06005AC7 RID: 23239 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public void EnableEvents(EventSource eventSource, EventLevel level)
		{
		}

		/// <summary>Enables events for the specified event source that has the specified verbosity level or lower, and matching keyword flags.</summary>
		/// <param name="eventSource">The event source to enable events for.</param>
		/// <param name="level">The level of events to enable.</param>
		/// <param name="matchAnyKeyword">The keyword flags necessary to enable the events.</param>
		// Token: 0x06005AC8 RID: 23240 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public void EnableEvents(EventSource eventSource, EventLevel level, EventKeywords matchAnyKeyword)
		{
		}

		/// <summary>Enables events for the specified event source that has the specified verbosity level or lower, matching event keyword flag, and matching arguments.</summary>
		/// <param name="eventSource">The event source to enable events for.</param>
		/// <param name="level">The level of events to enable.</param>
		/// <param name="matchAnyKeyword">The keyword flags necessary to enable the events.</param>
		/// <param name="arguments">The arguments to be matched to enable the events.</param>
		// Token: 0x06005AC9 RID: 23241 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public void EnableEvents(EventSource eventSource, EventLevel level, EventKeywords matchAnyKeyword, IDictionary<string, string> arguments)
		{
		}

		/// <summary>Disables all events for the specified event source.</summary>
		/// <param name="eventSource">The event source to disable events for.</param>
		// Token: 0x06005ACA RID: 23242 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public void DisableEvents(EventSource eventSource)
		{
		}

		/// <summary>Called for all existing event sources when the event listener is created and when a new event source is attached to the listener.</summary>
		/// <param name="eventSource">The event source.</param>
		// Token: 0x06005ACB RID: 23243 RVA: 0x00004BF9 File Offset: 0x00002DF9
		protected internal virtual void OnEventSourceCreated(EventSource eventSource)
		{
		}

		/// <summary>Called whenever an event has been written by an event source for which the event listener has enabled events.</summary>
		/// <param name="eventData">The event arguments that describe the event.</param>
		// Token: 0x06005ACC RID: 23244 RVA: 0x00004BF9 File Offset: 0x00002DF9
		protected internal virtual void OnEventWritten(EventWrittenEventArgs eventData)
		{
		}

		/// <summary>Releases the resources used by the current instance of the <see cref="T:System.Diagnostics.Tracing.EventListener" /> class.</summary>
		// Token: 0x06005ACD RID: 23245 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public virtual void Dispose()
		{
		}

		/// <summary>Occurs when an event source (<see cref="T:System.Diagnostics.Tracing.EventSource" /> object) is attached to the dispatcher.</summary>
		// Token: 0x14000020 RID: 32
		// (add) Token: 0x06005ACE RID: 23246 RVA: 0x00134430 File Offset: 0x00132630
		// (remove) Token: 0x06005ACF RID: 23247 RVA: 0x00134468 File Offset: 0x00132668
		public event EventHandler<EventSourceCreatedEventArgs> EventSourceCreated;

		/// <summary>Occurs when an event has been written by an event source (<see cref="T:System.Diagnostics.Tracing.EventSource" /> object) for which the event listener has enabled events.</summary>
		// Token: 0x14000021 RID: 33
		// (add) Token: 0x06005AD0 RID: 23248 RVA: 0x001344A0 File Offset: 0x001326A0
		// (remove) Token: 0x06005AD1 RID: 23249 RVA: 0x001344D8 File Offset: 0x001326D8
		public event EventHandler<EventWrittenEventArgs> EventWritten;
	}
}
