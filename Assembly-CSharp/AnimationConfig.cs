using System;

// Token: 0x020000ED RID: 237
public struct AnimationConfig
{
	// Token: 0x06000259 RID: 601 RVA: 0x00011768 File Offset: 0x0000F968
	public AnimationConfig(float frameSpeed)
	{
		this.FrameSpeed = frameSpeed;
		this.FreezeFrame = 0;
		this.FreezeInterval = 0f;
		this.Direction = Direction.North;
		this.ForceDirection = false;
	}

	// Token: 0x0600025A RID: 602 RVA: 0x00011791 File Offset: 0x0000F991
	public AnimationConfig(int freezeFrame, float freezeInterval)
	{
		this.FrameSpeed = 0f;
		this.FreezeFrame = freezeFrame;
		this.FreezeInterval = freezeInterval;
		this.Direction = Direction.North;
		this.ForceDirection = false;
	}

	// Token: 0x04000450 RID: 1104
	public float FrameSpeed;

	// Token: 0x04000451 RID: 1105
	public int FreezeFrame;

	// Token: 0x04000452 RID: 1106
	public float FreezeInterval;

	// Token: 0x04000453 RID: 1107
	public bool ForceDirection;

	// Token: 0x04000454 RID: 1108
	public Direction Direction;
}
