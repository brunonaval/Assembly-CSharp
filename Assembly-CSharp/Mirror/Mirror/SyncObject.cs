using System;

namespace Mirror
{
	// Token: 0x02000076 RID: 118
	public abstract class SyncObject
	{
		// Token: 0x06000383 RID: 899
		public abstract void ClearChanges();

		// Token: 0x06000384 RID: 900
		public abstract void OnSerializeAll(NetworkWriter writer);

		// Token: 0x06000385 RID: 901
		public abstract void OnSerializeDelta(NetworkWriter writer);

		// Token: 0x06000386 RID: 902
		public abstract void OnDeserializeAll(NetworkReader reader);

		// Token: 0x06000387 RID: 903
		public abstract void OnDeserializeDelta(NetworkReader reader);

		// Token: 0x06000388 RID: 904
		public abstract void Reset();

		// Token: 0x04000160 RID: 352
		public Action OnDirty;

		// Token: 0x04000161 RID: 353
		public Func<bool> IsRecording = () => true;

		// Token: 0x04000162 RID: 354
		public Func<bool> IsWritable = () => true;
	}
}
