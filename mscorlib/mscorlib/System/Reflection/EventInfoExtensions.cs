using System;

namespace System.Reflection
{
	// Token: 0x020008D6 RID: 2262
	public static class EventInfoExtensions
	{
		// Token: 0x06004B70 RID: 19312 RVA: 0x000F0721 File Offset: 0x000EE921
		public static MethodInfo GetAddMethod(EventInfo eventInfo)
		{
			Requires.NotNull(eventInfo, "eventInfo");
			return eventInfo.GetAddMethod();
		}

		// Token: 0x06004B71 RID: 19313 RVA: 0x000F0734 File Offset: 0x000EE934
		public static MethodInfo GetAddMethod(EventInfo eventInfo, bool nonPublic)
		{
			Requires.NotNull(eventInfo, "eventInfo");
			return eventInfo.GetAddMethod(nonPublic);
		}

		// Token: 0x06004B72 RID: 19314 RVA: 0x000F0748 File Offset: 0x000EE948
		public static MethodInfo GetRaiseMethod(EventInfo eventInfo)
		{
			Requires.NotNull(eventInfo, "eventInfo");
			return eventInfo.GetRaiseMethod();
		}

		// Token: 0x06004B73 RID: 19315 RVA: 0x000F075B File Offset: 0x000EE95B
		public static MethodInfo GetRaiseMethod(EventInfo eventInfo, bool nonPublic)
		{
			Requires.NotNull(eventInfo, "eventInfo");
			return eventInfo.GetRaiseMethod(nonPublic);
		}

		// Token: 0x06004B74 RID: 19316 RVA: 0x000F076F File Offset: 0x000EE96F
		public static MethodInfo GetRemoveMethod(EventInfo eventInfo)
		{
			Requires.NotNull(eventInfo, "eventInfo");
			return eventInfo.GetRemoveMethod();
		}

		// Token: 0x06004B75 RID: 19317 RVA: 0x000F0782 File Offset: 0x000EE982
		public static MethodInfo GetRemoveMethod(EventInfo eventInfo, bool nonPublic)
		{
			Requires.NotNull(eventInfo, "eventInfo");
			return eventInfo.GetRemoveMethod(nonPublic);
		}
	}
}
