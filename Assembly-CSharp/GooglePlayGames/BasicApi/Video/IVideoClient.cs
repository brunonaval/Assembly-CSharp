using System;

namespace GooglePlayGames.BasicApi.Video
{
	// Token: 0x020006A6 RID: 1702
	public interface IVideoClient
	{
		// Token: 0x06002567 RID: 9575
		void GetCaptureCapabilities(Action<ResponseStatus, VideoCapabilities> callback);

		// Token: 0x06002568 RID: 9576
		void ShowCaptureOverlay();

		// Token: 0x06002569 RID: 9577
		void GetCaptureState(Action<ResponseStatus, VideoCaptureState> callback);

		// Token: 0x0600256A RID: 9578
		void IsCaptureAvailable(VideoCaptureMode captureMode, Action<ResponseStatus, bool> callback);

		// Token: 0x0600256B RID: 9579
		bool IsCaptureSupported();

		// Token: 0x0600256C RID: 9580
		void RegisterCaptureOverlayStateChangedListener(CaptureOverlayStateListener listener);

		// Token: 0x0600256D RID: 9581
		void UnregisterCaptureOverlayStateChangedListener();
	}
}
