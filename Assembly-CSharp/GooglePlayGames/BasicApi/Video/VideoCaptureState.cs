using System;

namespace GooglePlayGames.BasicApi.Video
{
	// Token: 0x020006A9 RID: 1705
	public class VideoCaptureState
	{
		// Token: 0x06002579 RID: 9593 RVA: 0x000B4DE4 File Offset: 0x000B2FE4
		internal VideoCaptureState(bool isCapturing, VideoCaptureMode captureMode, VideoQualityLevel qualityLevel, bool isOverlayVisible, bool isPaused)
		{
			this.mIsCapturing = isCapturing;
			this.mCaptureMode = captureMode;
			this.mQualityLevel = qualityLevel;
			this.mIsOverlayVisible = isOverlayVisible;
			this.mIsPaused = isPaused;
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x0600257A RID: 9594 RVA: 0x000B4E11 File Offset: 0x000B3011
		public bool IsCapturing
		{
			get
			{
				return this.mIsCapturing;
			}
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x0600257B RID: 9595 RVA: 0x000B4E19 File Offset: 0x000B3019
		public VideoCaptureMode CaptureMode
		{
			get
			{
				return this.mCaptureMode;
			}
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x0600257C RID: 9596 RVA: 0x000B4E21 File Offset: 0x000B3021
		public VideoQualityLevel QualityLevel
		{
			get
			{
				return this.mQualityLevel;
			}
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x0600257D RID: 9597 RVA: 0x000B4E29 File Offset: 0x000B3029
		public bool IsOverlayVisible
		{
			get
			{
				return this.mIsOverlayVisible;
			}
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x0600257E RID: 9598 RVA: 0x000B4E31 File Offset: 0x000B3031
		public bool IsPaused
		{
			get
			{
				return this.mIsPaused;
			}
		}

		// Token: 0x0600257F RID: 9599 RVA: 0x000B4E3C File Offset: 0x000B303C
		public override string ToString()
		{
			return string.Format("[VideoCaptureState: mIsCapturing={0}, mCaptureMode={1}, mQualityLevel={2}, mIsOverlayVisible={3}, mIsPaused={4}]", new object[]
			{
				this.mIsCapturing,
				this.mCaptureMode.ToString(),
				this.mQualityLevel.ToString(),
				this.mIsOverlayVisible,
				this.mIsPaused
			});
		}

		// Token: 0x04001EA2 RID: 7842
		private bool mIsCapturing;

		// Token: 0x04001EA3 RID: 7843
		private VideoCaptureMode mCaptureMode;

		// Token: 0x04001EA4 RID: 7844
		private VideoQualityLevel mQualityLevel;

		// Token: 0x04001EA5 RID: 7845
		private bool mIsOverlayVisible;

		// Token: 0x04001EA6 RID: 7846
		private bool mIsPaused;
	}
}
