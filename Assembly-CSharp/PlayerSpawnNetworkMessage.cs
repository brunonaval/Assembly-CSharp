using System;
using Mirror;
using UnityEngine;

// Token: 0x0200029C RID: 668
public struct PlayerSpawnNetworkMessage : NetworkMessage
{
	// Token: 0x04000BF9 RID: 3065
	public int PlayerId;

	// Token: 0x04000BFA RID: 3066
	public int AccountId;

	// Token: 0x04000BFB RID: 3067
	public Vector3 Position;

	// Token: 0x04000BFC RID: 3068
	public PackageType PackageType;

	// Token: 0x04000BFD RID: 3069
	public string AccountUniqueId;

	// Token: 0x04000BFE RID: 3070
	public string ConnectionVersion;
}
