using System;

namespace Mirror
{
	// Token: 0x02000022 RID: 34
	public struct SceneMessage : NetworkMessage
	{
		// Token: 0x0400002A RID: 42
		public string sceneName;

		// Token: 0x0400002B RID: 43
		public SceneOperation sceneOperation;

		// Token: 0x0400002C RID: 44
		public bool customHandling;
	}
}
