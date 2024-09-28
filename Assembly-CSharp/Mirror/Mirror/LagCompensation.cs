using System;
using System.Collections.Generic;

namespace Mirror
{
	// Token: 0x02000019 RID: 25
	public static class LagCompensation
	{
		// Token: 0x06000037 RID: 55 RVA: 0x000029F3 File Offset: 0x00000BF3
		public static void Insert<T>(Queue<KeyValuePair<double, T>> history, int historyLimit, double timestamp, T capture) where T : Capture
		{
			if (history.Count >= historyLimit)
			{
				history.Dequeue();
			}
			history.Enqueue(new KeyValuePair<double, T>(timestamp, capture));
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002A14 File Offset: 0x00000C14
		public static bool Sample<T>(Queue<KeyValuePair<double, T>> history, double timestamp, double interval, out T before, out T after, out double t) where T : Capture
		{
			before = default(T);
			after = default(T);
			t = 0.0;
			if (history.Count < 2)
			{
				return false;
			}
			if (timestamp < history.Peek().Key)
			{
				return false;
			}
			KeyValuePair<double, T> keyValuePair = default(KeyValuePair<double, T>);
			KeyValuePair<double, T> keyValuePair2 = default(KeyValuePair<double, T>);
			foreach (KeyValuePair<double, T> keyValuePair3 in history)
			{
				if (timestamp == keyValuePair3.Key)
				{
					before = keyValuePair3.Value;
					after = keyValuePair3.Value;
					t = Mathd.InverseLerp(before.timestamp, after.timestamp, timestamp);
					return true;
				}
				if (keyValuePair3.Key > timestamp)
				{
					before = keyValuePair.Value;
					after = keyValuePair3.Value;
					t = Mathd.InverseLerp(before.timestamp, after.timestamp, timestamp);
					return true;
				}
				keyValuePair2 = keyValuePair;
				keyValuePair = keyValuePair3;
			}
			if (keyValuePair.Key < timestamp && timestamp <= keyValuePair.Key + interval)
			{
				before = keyValuePair2.Value;
				after = keyValuePair.Value;
				t = 1.0 + Mathd.InverseLerp(after.timestamp, after.timestamp + interval, timestamp);
				return true;
			}
			return false;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002BB4 File Offset: 0x00000DB4
		public static double EstimateTime(double serverTime, double rtt, double bufferTime)
		{
			double num = rtt / 2.0;
			return serverTime - num - bufferTime;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002BD4 File Offset: 0x00000DD4
		public static void DrawGizmos<T>(Queue<KeyValuePair<double, T>> history) where T : Capture
		{
			foreach (KeyValuePair<double, T> keyValuePair in history)
			{
				T value = keyValuePair.Value;
				value.DrawGizmo();
			}
		}
	}
}
