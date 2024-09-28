using System;
using UnityEngine;

// Token: 0x0200010B RID: 267
public struct EffectConfig
{
	// Token: 0x17000037 RID: 55
	// (get) Token: 0x060002B0 RID: 688 RVA: 0x00012687 File Offset: 0x00010887
	public bool IsDefined
	{
		get
		{
			return !string.IsNullOrEmpty(this.EffectName) | !string.IsNullOrEmpty(this.SoundEffectName) | !string.IsNullOrEmpty(this.Text);
		}
	}

	// Token: 0x0400050D RID: 1293
	public string EffectName;

	// Token: 0x0400050E RID: 1294
	public float EffectScaleModifier;

	// Token: 0x0400050F RID: 1295
	public float EffectSpeedModifier;

	// Token: 0x04000510 RID: 1296
	public float EffectLoopDuration;

	// Token: 0x04000511 RID: 1297
	public string SoundEffectName;

	// Token: 0x04000512 RID: 1298
	public string Text;

	// Token: 0x04000513 RID: 1299
	public int TextColorId;

	// Token: 0x04000514 RID: 1300
	public Vector3 Position;
}
