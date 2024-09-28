using System;

namespace Mirror
{
	// Token: 0x02000035 RID: 53
	public struct NetworkBehaviourSyncVar : IEquatable<NetworkBehaviourSyncVar>
	{
		// Token: 0x060000B0 RID: 176 RVA: 0x000040A8 File Offset: 0x000022A8
		public NetworkBehaviourSyncVar(uint netId, int componentIndex)
		{
			this = default(NetworkBehaviourSyncVar);
			this.netId = netId;
			this.componentIndex = (byte)componentIndex;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000040C0 File Offset: 0x000022C0
		public bool Equals(NetworkBehaviourSyncVar other)
		{
			return other.netId == this.netId && other.componentIndex == this.componentIndex;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000040E0 File Offset: 0x000022E0
		public bool Equals(uint netId, int componentIndex)
		{
			return this.netId == netId && (int)this.componentIndex == componentIndex;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000040F6 File Offset: 0x000022F6
		public override string ToString()
		{
			return string.Format("[netId:{0} compIndex:{1}]", this.netId, this.componentIndex);
		}

		// Token: 0x0400005F RID: 95
		public uint netId;

		// Token: 0x04000060 RID: 96
		public byte componentIndex;
	}
}
