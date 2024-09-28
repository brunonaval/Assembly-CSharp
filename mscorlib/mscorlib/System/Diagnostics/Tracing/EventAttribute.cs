using System;

namespace System.Diagnostics.Tracing
{
	/// <summary>Specifies additional event schema information for an event.</summary>
	// Token: 0x020009EC RID: 2540
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class EventAttribute : Attribute
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Diagnostics.Tracing.EventAttribute" /> class with the specified event identifier.</summary>
		/// <param name="eventId">The event identifier for the event.</param>
		// Token: 0x06005A9F RID: 23199 RVA: 0x00134363 File Offset: 0x00132563
		public EventAttribute(int eventId)
		{
			this.EventId = eventId;
		}

		/// <summary>Gets or sets the identifier for the event.</summary>
		/// <returns>The event identifier. This value should be between 0 and 65535.</returns>
		// Token: 0x17000F7F RID: 3967
		// (get) Token: 0x06005AA0 RID: 23200 RVA: 0x00134372 File Offset: 0x00132572
		// (set) Token: 0x06005AA1 RID: 23201 RVA: 0x0013437A File Offset: 0x0013257A
		public int EventId { get; private set; }

		/// <summary>Specifies the behavior of the start and stop events of an activity. An activity is the region of time in an app between the start and the stop.</summary>
		/// <returns>Returns <see cref="T:System.Diagnostics.Tracing.EventActivityOptions" />.</returns>
		// Token: 0x17000F80 RID: 3968
		// (get) Token: 0x06005AA2 RID: 23202 RVA: 0x00134383 File Offset: 0x00132583
		// (set) Token: 0x06005AA3 RID: 23203 RVA: 0x0013438B File Offset: 0x0013258B
		public EventActivityOptions ActivityOptions { get; set; }

		/// <summary>Gets or sets the level for the event.</summary>
		/// <returns>One of the enumeration values that specifies the level for the event.</returns>
		// Token: 0x17000F81 RID: 3969
		// (get) Token: 0x06005AA4 RID: 23204 RVA: 0x00134394 File Offset: 0x00132594
		// (set) Token: 0x06005AA5 RID: 23205 RVA: 0x0013439C File Offset: 0x0013259C
		public EventLevel Level { get; set; }

		/// <summary>Gets or sets the keywords for the event.</summary>
		/// <returns>A bitwise combination of the enumeration values.</returns>
		// Token: 0x17000F82 RID: 3970
		// (get) Token: 0x06005AA6 RID: 23206 RVA: 0x001343A5 File Offset: 0x001325A5
		// (set) Token: 0x06005AA7 RID: 23207 RVA: 0x001343AD File Offset: 0x001325AD
		public EventKeywords Keywords { get; set; }

		/// <summary>Gets or sets the operation code for the event.</summary>
		/// <returns>One of the enumeration values that specifies the operation code.</returns>
		// Token: 0x17000F83 RID: 3971
		// (get) Token: 0x06005AA8 RID: 23208 RVA: 0x001343B6 File Offset: 0x001325B6
		// (set) Token: 0x06005AA9 RID: 23209 RVA: 0x001343BE File Offset: 0x001325BE
		public EventOpcode Opcode { get; set; }

		/// <summary>Gets or sets an additional event log where the event should be written.</summary>
		/// <returns>An additional event log where the event should be written.</returns>
		// Token: 0x17000F84 RID: 3972
		// (get) Token: 0x06005AAA RID: 23210 RVA: 0x001343C7 File Offset: 0x001325C7
		// (set) Token: 0x06005AAB RID: 23211 RVA: 0x001343CF File Offset: 0x001325CF
		public EventChannel Channel { get; set; }

		/// <summary>Gets or sets the message for the event.</summary>
		/// <returns>The message for the event.</returns>
		// Token: 0x17000F85 RID: 3973
		// (get) Token: 0x06005AAC RID: 23212 RVA: 0x001343D8 File Offset: 0x001325D8
		// (set) Token: 0x06005AAD RID: 23213 RVA: 0x001343E0 File Offset: 0x001325E0
		public string Message { get; set; }

		/// <summary>Gets or sets the task for the event.</summary>
		/// <returns>The task for the event.</returns>
		// Token: 0x17000F86 RID: 3974
		// (get) Token: 0x06005AAE RID: 23214 RVA: 0x001343E9 File Offset: 0x001325E9
		// (set) Token: 0x06005AAF RID: 23215 RVA: 0x001343F1 File Offset: 0x001325F1
		public EventTask Task { get; set; }

		/// <summary>Gets or sets the <see cref="T:System.Diagnostics.Tracing.EventTags" /> value for this <see cref="T:System.Diagnostics.Tracing.EventAttribute" /> object. An event tag is a user-defined value that is passed through when the event is logged.</summary>
		/// <returns>The <see cref="T:System.Diagnostics.Tracing.EventTags" /> value for this <see cref="T:System.Diagnostics.Tracing.EventAttribute" /> object. An event tag is a user-defined value that is passed through when the event is logged.</returns>
		// Token: 0x17000F87 RID: 3975
		// (get) Token: 0x06005AB0 RID: 23216 RVA: 0x001343FA File Offset: 0x001325FA
		// (set) Token: 0x06005AB1 RID: 23217 RVA: 0x00134402 File Offset: 0x00132602
		public EventTags Tags { get; set; }

		/// <summary>Gets or sets the version of the event.</summary>
		/// <returns>The version of the event.</returns>
		// Token: 0x17000F88 RID: 3976
		// (get) Token: 0x06005AB2 RID: 23218 RVA: 0x0013440B File Offset: 0x0013260B
		// (set) Token: 0x06005AB3 RID: 23219 RVA: 0x00134413 File Offset: 0x00132613
		public byte Version { get; set; }
	}
}
