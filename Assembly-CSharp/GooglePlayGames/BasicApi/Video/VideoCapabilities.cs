using System;
using System.Linq;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.BasicApi.Video
{
	// Token: 0x020006A7 RID: 1703
	public class VideoCapabilities
	{
		// Token: 0x0600256E RID: 9582 RVA: 0x000B4C95 File Offset: 0x000B2E95
		internal VideoCapabilities(bool isCameraSupported, bool isMicSupported, bool isWriteStorageSupported, bool[] captureModesSupported, bool[] qualityLevelsSupported)
		{
			this.mIsCameraSupported = isCameraSupported;
			this.mIsMicSupported = isMicSupported;
			this.mIsWriteStorageSupported = isWriteStorageSupported;
			this.mCaptureModesSupported = captureModesSupported;
			this.mQualityLevelsSupported = qualityLevelsSupported;
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x0600256F RID: 9583 RVA: 0x000B4CC2 File Offset: 0x000B2EC2
		public bool IsCameraSupported
		{
			get
			{
				return this.mIsCameraSupported;
			}
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06002570 RID: 9584 RVA: 0x000B4CCA File Offset: 0x000B2ECA
		public bool IsMicSupported
		{
			get
			{
				return this.mIsMicSupported;
			}
		}

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06002571 RID: 9585 RVA: 0x000B4CD2 File Offset: 0x000B2ED2
		public bool IsWriteStorageSupported
		{
			get
			{
				return this.mIsWriteStorageSupported;
			}
		}

		// Token: 0x06002572 RID: 9586 RVA: 0x000B4CDA File Offset: 0x000B2EDA
		public bool SupportsCaptureMode(VideoCaptureMode captureMode)
		{
			if (captureMode != VideoCaptureMode.Unknown)
			{
				return this.mCaptureModesSupported[(int)captureMode];
			}
			Logger.w("SupportsCaptureMode called with an unknown captureMode.");
			return false;
		}

		// Token: 0x06002573 RID: 9587 RVA: 0x000B4CF4 File Offset: 0x000B2EF4
		public bool SupportsQualityLevel(VideoQualityLevel qualityLevel)
		{
			if (qualityLevel != VideoQualityLevel.Unknown)
			{
				return this.mQualityLevelsSupported[(int)qualityLevel];
			}
			Logger.w("SupportsCaptureMode called with an unknown qualityLevel.");
			return false;
		}

		// Token: 0x06002574 RID: 9588 RVA: 0x000B4D10 File Offset: 0x000B2F10
		public override string ToString()
		{
			string format = "[VideoCapabilities: mIsCameraSupported={0}, mIsMicSupported={1}, mIsWriteStorageSupported={2}, mCaptureModesSupported={3}, mQualityLevelsSupported={4}]";
			object[] array = new object[5];
			array[0] = this.mIsCameraSupported;
			array[1] = this.mIsMicSupported;
			array[2] = this.mIsWriteStorageSupported;
			array[3] = string.Join(",", (from p in this.mCaptureModesSupported
			select p.ToString()).ToArray<string>());
			array[4] = string.Join(",", (from p in this.mQualityLevelsSupported
			select p.ToString()).ToArray<string>());
			return string.Format(format, array);
		}

		// Token: 0x04001E9A RID: 7834
		private bool mIsCameraSupported;

		// Token: 0x04001E9B RID: 7835
		private bool mIsMicSupported;

		// Token: 0x04001E9C RID: 7836
		private bool mIsWriteStorageSupported;

		// Token: 0x04001E9D RID: 7837
		private bool[] mCaptureModesSupported;

		// Token: 0x04001E9E RID: 7838
		private bool[] mQualityLevelsSupported;
	}
}
