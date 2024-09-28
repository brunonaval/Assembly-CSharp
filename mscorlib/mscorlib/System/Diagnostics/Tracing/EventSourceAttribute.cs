using System;

namespace System.Diagnostics.Tracing
{
	/// <summary>Allows the event tracing for Windows (ETW) name to be defined independently of the name of the event source class.</summary>
	// Token: 0x020009F9 RID: 2553
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class EventSourceAttribute : Attribute
	{
		/// <summary>Gets or sets the event source identifier.</summary>
		/// <returns>The event source identifier.</returns>
		// Token: 0x17000F96 RID: 3990
		// (get) Token: 0x06005B15 RID: 23317 RVA: 0x001347BE File Offset: 0x001329BE
		// (set) Token: 0x06005B16 RID: 23318 RVA: 0x001347C6 File Offset: 0x001329C6
		public string Guid { get; set; }

		/// <summary>Gets or sets the name of the localization resource file.</summary>
		/// <returns>The name of the localization resource file, or <see langword="null" /> if the localization resource file does not exist.</returns>
		// Token: 0x17000F97 RID: 3991
		// (get) Token: 0x06005B17 RID: 23319 RVA: 0x001347CF File Offset: 0x001329CF
		// (set) Token: 0x06005B18 RID: 23320 RVA: 0x001347D7 File Offset: 0x001329D7
		public string LocalizationResources { get; set; }

		/// <summary>Gets or sets the name of the event source.</summary>
		/// <returns>The name of the event source.</returns>
		// Token: 0x17000F98 RID: 3992
		// (get) Token: 0x06005B19 RID: 23321 RVA: 0x001347E0 File Offset: 0x001329E0
		// (set) Token: 0x06005B1A RID: 23322 RVA: 0x001347E8 File Offset: 0x001329E8
		public string Name { get; set; }
	}
}
