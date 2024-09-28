using System;
using UnityEngine;

namespace GooglePlayGames.OurUtils
{
	// Token: 0x0200068D RID: 1677
	public class Logger
	{
		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06002536 RID: 9526 RVA: 0x000B458B File Offset: 0x000B278B
		// (set) Token: 0x06002537 RID: 9527 RVA: 0x000B4592 File Offset: 0x000B2792
		public static bool DebugLogEnabled
		{
			get
			{
				return Logger.debugLogEnabled;
			}
			set
			{
				Logger.debugLogEnabled = value;
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06002538 RID: 9528 RVA: 0x000B459A File Offset: 0x000B279A
		// (set) Token: 0x06002539 RID: 9529 RVA: 0x000B45A1 File Offset: 0x000B27A1
		public static bool WarningLogEnabled
		{
			get
			{
				return Logger.warningLogEnabled;
			}
			set
			{
				Logger.warningLogEnabled = value;
			}
		}

		// Token: 0x0600253A RID: 9530 RVA: 0x000B45AC File Offset: 0x000B27AC
		public static void d(string msg)
		{
			if (Logger.debugLogEnabled)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					Debug.Log(Logger.ToLogMessage(string.Empty, "DEBUG", msg));
				});
			}
		}

		// Token: 0x0600253B RID: 9531 RVA: 0x000B45E0 File Offset: 0x000B27E0
		public static void w(string msg)
		{
			if (Logger.warningLogEnabled)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					Debug.LogWarning(Logger.ToLogMessage("!!!", "WARNING", msg));
				});
			}
		}

		// Token: 0x0600253C RID: 9532 RVA: 0x000B4614 File Offset: 0x000B2814
		public static void e(string msg)
		{
			if (Logger.warningLogEnabled)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					Debug.LogWarning(Logger.ToLogMessage("***", "ERROR", msg));
				});
			}
		}

		// Token: 0x0600253D RID: 9533 RVA: 0x000B4648 File Offset: 0x000B2848
		public static string describe(byte[] b)
		{
			if (b != null)
			{
				return "byte[" + b.Length.ToString() + "]";
			}
			return "(null)";
		}

		// Token: 0x0600253E RID: 9534 RVA: 0x000B4678 File Offset: 0x000B2878
		private static string ToLogMessage(string prefix, string logType, string msg)
		{
			string text = null;
			try
			{
				text = DateTime.Now.ToString("MM/dd/yy H:mm:ss zzz");
			}
			catch (Exception)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					Debug.LogWarning("*** [Play Games Plugin 0.10.14] ERROR: Failed to format DateTime.Now");
				});
				text = string.Empty;
			}
			return string.Format("{0} [Play Games Plugin 0.10.14] {1} {2}: {3}", new object[]
			{
				prefix,
				text,
				logType,
				msg
			});
		}

		// Token: 0x04001E36 RID: 7734
		private static bool debugLogEnabled = false;

		// Token: 0x04001E37 RID: 7735
		private static bool warningLogEnabled = true;
	}
}
