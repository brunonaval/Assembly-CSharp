using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Mirror
{
	// Token: 0x02000069 RID: 105
	public static class SnapshotInterpolation
	{
		// Token: 0x06000321 RID: 801 RVA: 0x0000BFC9 File Offset: 0x0000A1C9
		public static double Timescale(double drift, double catchupSpeed, double slowdownSpeed, double absoluteCatchupNegativeThreshold, double absoluteCatchupPositiveThreshold)
		{
			if (drift > absoluteCatchupPositiveThreshold)
			{
				return 1.0 + catchupSpeed;
			}
			if (drift < absoluteCatchupNegativeThreshold)
			{
				return 1.0 - slowdownSpeed;
			}
			return 1.0;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000BFF5 File Offset: 0x0000A1F5
		public static double DynamicAdjustment(double sendInterval, double jitterStandardDeviation, double dynamicAdjustmentTolerance)
		{
			return (sendInterval + jitterStandardDeviation) / sendInterval + dynamicAdjustmentTolerance;
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000C000 File Offset: 0x0000A200
		public static bool InsertIfNotExists<T>(SortedList<double, T> buffer, int bufferLimit, T snapshot) where T : Snapshot
		{
			if (buffer.Count >= bufferLimit)
			{
				return false;
			}
			int count = buffer.Count;
			buffer[snapshot.remoteTime] = snapshot;
			return buffer.Count > count;
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000C03C File Offset: 0x0000A23C
		public static double TimelineClamp(double localTimeline, double bufferTime, double latestRemoteTime)
		{
			double num = latestRemoteTime - bufferTime;
			double min = num - bufferTime;
			double max = num + bufferTime;
			return Mathd.Clamp(localTimeline, min, max);
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000C05C File Offset: 0x0000A25C
		public static void InsertAndAdjust<T>(SortedList<double, T> buffer, int bufferLimit, T snapshot, ref double localTimeline, ref double localTimescale, float sendInterval, double bufferTime, double catchupSpeed, double slowdownSpeed, ref ExponentialMovingAverage driftEma, float catchupNegativeThreshold, float catchupPositiveThreshold, ref ExponentialMovingAverage deliveryTimeEma) where T : Snapshot
		{
			if (buffer.Count == 0)
			{
				localTimeline = snapshot.remoteTime - bufferTime;
			}
			if (SnapshotInterpolation.InsertIfNotExists<T>(buffer, bufferLimit, snapshot))
			{
				if (buffer.Count >= 2)
				{
					T t = buffer.Values[buffer.Count - 2];
					double localTime = t.localTime;
					t = buffer.Values[buffer.Count - 1];
					double newValue = t.localTime - localTime;
					deliveryTimeEma.Add(newValue);
				}
				double remoteTime = snapshot.remoteTime;
				localTimeline = SnapshotInterpolation.TimelineClamp(localTimeline, bufferTime, remoteTime);
				double newValue2 = remoteTime - localTimeline;
				driftEma.Add(newValue2);
				double drift = driftEma.Value - bufferTime;
				double absoluteCatchupNegativeThreshold = (double)(sendInterval * catchupNegativeThreshold);
				double absoluteCatchupPositiveThreshold = (double)(sendInterval * catchupPositiveThreshold);
				localTimescale = SnapshotInterpolation.Timescale(drift, catchupSpeed, slowdownSpeed, absoluteCatchupNegativeThreshold, absoluteCatchupPositiveThreshold);
			}
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000C140 File Offset: 0x0000A340
		public static void Sample<T>(SortedList<double, T> buffer, double localTimeline, out int from, out int to, out double t) where T : Snapshot
		{
			from = -1;
			to = -1;
			t = 0.0;
			for (int i = 0; i < buffer.Count - 1; i++)
			{
				T t2 = buffer.Values[i];
				T t3 = buffer.Values[i + 1];
				if (localTimeline >= t2.remoteTime && localTimeline <= t3.remoteTime)
				{
					from = i;
					to = i + 1;
					t = Mathd.InverseLerp(t2.remoteTime, t3.remoteTime, localTimeline);
					return;
				}
			}
			T t4 = buffer.Values[0];
			if (t4.remoteTime > localTimeline)
			{
				from = (to = 0);
				t = 0.0;
				return;
			}
			from = (to = buffer.Count - 1);
			t = 0.0;
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000C22B File Offset: 0x0000A42B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void StepTime(double deltaTime, ref double localTimeline, double localTimescale)
		{
			localTimeline += deltaTime * localTimescale;
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000C238 File Offset: 0x0000A438
		public static void StepInterpolation<T>(SortedList<double, T> buffer, double localTimeline, out T fromSnapshot, out T toSnapshot, out double t) where T : Snapshot
		{
			int num;
			int index;
			SnapshotInterpolation.Sample<T>(buffer, localTimeline, out num, out index, out t);
			fromSnapshot = buffer.Values[num];
			toSnapshot = buffer.Values[index];
			buffer.RemoveRange(num);
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000C27D File Offset: 0x0000A47D
		public static void Step<T>(SortedList<double, T> buffer, double deltaTime, ref double localTimeline, double localTimescale, out T fromSnapshot, out T toSnapshot, out double t) where T : Snapshot
		{
			SnapshotInterpolation.StepTime(deltaTime, ref localTimeline, localTimescale);
			SnapshotInterpolation.StepInterpolation<T>(buffer, localTimeline, out fromSnapshot, out toSnapshot, out t);
		}
	}
}
