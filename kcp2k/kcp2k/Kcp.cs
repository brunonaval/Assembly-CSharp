using System;
using System.Collections.Generic;

namespace kcp2k
{
	// Token: 0x02000010 RID: 16
	public class Kcp
	{
		// Token: 0x06000048 RID: 72 RVA: 0x00003938 File Offset: 0x00001B38
		public Kcp(uint conv, Action<byte[], int> output)
		{
			this.conv = conv;
			this.output = output;
			this.snd_wnd = 32U;
			this.rcv_wnd = 128U;
			this.rmt_wnd = 128U;
			this.mtu = 1200U;
			this.mss = this.mtu - 24U;
			this.rx_rto = 200;
			this.rx_minrto = 100;
			this.interval = 100U;
			this.ts_flush = 100U;
			this.ssthresh = 2U;
			this.fastlimit = 5;
			this.dead_link = 20U;
			this.buffer = new byte[(this.mtu + 24U) * 3U];
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003A6C File Offset: 0x00001C6C
		private Segment SegmentNew()
		{
			return this.SegmentPool.Take();
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003A79 File Offset: 0x00001C79
		private void SegmentDelete(Segment seg)
		{
			this.SegmentPool.Return(seg);
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00003A87 File Offset: 0x00001C87
		public int WaitSnd
		{
			get
			{
				return this.snd_buf.Count + this.snd_queue.Count;
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003AA0 File Offset: 0x00001CA0
		internal uint WndUnused()
		{
			if ((long)this.rcv_queue.Count < (long)((ulong)this.rcv_wnd))
			{
				return this.rcv_wnd - (uint)this.rcv_queue.Count;
			}
			return 0U;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003ACC File Offset: 0x00001CCC
		public int Receive(byte[] buffer, int len)
		{
			if (len < 0)
			{
				throw new NotSupportedException("Receive ispeek for negative len is not supported!");
			}
			if (this.rcv_queue.Count == 0)
			{
				return -1;
			}
			if (len < 0)
			{
				len = -len;
			}
			int num = this.PeekSize();
			if (num < 0)
			{
				return -2;
			}
			if (num > len)
			{
				return -3;
			}
			bool flag = (long)this.rcv_queue.Count >= (long)((ulong)this.rcv_wnd);
			int num2 = 0;
			len = 0;
			while (this.rcv_queue.Count > 0)
			{
				Segment segment = this.rcv_queue.Dequeue();
				Buffer.BlockCopy(segment.data.GetBuffer(), 0, buffer, num2, (int)segment.data.Position);
				num2 += (int)segment.data.Position;
				len += (int)segment.data.Position;
				bool frg = segment.frg != 0U;
				this.SegmentDelete(segment);
				if (!frg)
				{
					break;
				}
			}
			int num3 = 0;
			foreach (Segment segment2 in this.rcv_buf)
			{
				if (segment2.sn != this.rcv_nxt || (long)this.rcv_queue.Count >= (long)((ulong)this.rcv_wnd))
				{
					break;
				}
				num3++;
				this.rcv_queue.Enqueue(segment2);
				this.rcv_nxt += 1U;
			}
			this.rcv_buf.RemoveRange(0, num3);
			if ((long)this.rcv_queue.Count < (long)((ulong)this.rcv_wnd) && flag)
			{
				this.probe |= 2U;
			}
			return len;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003C60 File Offset: 0x00001E60
		public int PeekSize()
		{
			int num = 0;
			if (this.rcv_queue.Count == 0)
			{
				return -1;
			}
			Segment segment = this.rcv_queue.Peek();
			if (segment.frg == 0U)
			{
				return (int)segment.data.Position;
			}
			if ((long)this.rcv_queue.Count < (long)((ulong)(segment.frg + 1U)))
			{
				return -1;
			}
			foreach (Segment segment2 in this.rcv_queue)
			{
				num += (int)segment2.data.Position;
				if (segment2.frg == 0U)
				{
					break;
				}
			}
			return num;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003D14 File Offset: 0x00001F14
		public int Send(byte[] buffer, int offset, int len)
		{
			if (len < 0)
			{
				return -1;
			}
			int num;
			if ((long)len <= (long)((ulong)this.mss))
			{
				num = 1;
			}
			else
			{
				num = (int)(((long)len + (long)((ulong)this.mss) - 1L) / (long)((ulong)this.mss));
			}
			if (num > 255)
			{
				throw new Exception(string.Format("Send len={0} requires {1} fragments, but kcp can only handle up to {2} fragments.", len, num, 255));
			}
			if ((long)num >= (long)((ulong)this.rcv_wnd))
			{
				return -2;
			}
			if (num == 0)
			{
				num = 1;
			}
			for (int i = 0; i < num; i++)
			{
				int num2 = (int)((len > (int)this.mss) ? this.mss : ((uint)len));
				Segment segment = this.SegmentNew();
				if (len > 0)
				{
					segment.data.Write(buffer, offset, num2);
				}
				segment.frg = (uint)(num - i - 1);
				this.snd_queue.Enqueue(segment);
				offset += num2;
				len -= num2;
			}
			return 0;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003DE8 File Offset: 0x00001FE8
		private void UpdateAck(int rtt)
		{
			if (this.rx_srtt == 0)
			{
				this.rx_srtt = rtt;
				this.rx_rttval = rtt / 2;
			}
			else
			{
				int num = rtt - this.rx_srtt;
				if (num < 0)
				{
					num = -num;
				}
				this.rx_rttval = (3 * this.rx_rttval + num) / 4;
				this.rx_srtt = (7 * this.rx_srtt + rtt) / 8;
				if (this.rx_srtt < 1)
				{
					this.rx_srtt = 1;
				}
			}
			int value = this.rx_srtt + Math.Max((int)this.interval, 4 * this.rx_rttval);
			this.rx_rto = Utils.Clamp(value, this.rx_minrto, 60000);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003E88 File Offset: 0x00002088
		internal void ShrinkBuf()
		{
			if (this.snd_buf.Count > 0)
			{
				Segment segment = this.snd_buf[0];
				this.snd_una = segment.sn;
				return;
			}
			this.snd_una = this.snd_nxt;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003ECC File Offset: 0x000020CC
		internal void ParseAck(uint sn)
		{
			if (Utils.TimeDiff(sn, this.snd_una) < 0 || Utils.TimeDiff(sn, this.snd_nxt) >= 0)
			{
				return;
			}
			for (int i = 0; i < this.snd_buf.Count; i++)
			{
				Segment segment = this.snd_buf[i];
				if (sn == segment.sn)
				{
					this.snd_buf.RemoveAt(i);
					this.SegmentDelete(segment);
					return;
				}
				if (Utils.TimeDiff(sn, segment.sn) < 0)
				{
					break;
				}
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003F48 File Offset: 0x00002148
		internal void ParseUna(uint una)
		{
			int num = 0;
			foreach (Segment segment in this.snd_buf)
			{
				if (segment.sn >= una)
				{
					break;
				}
				num++;
				this.SegmentDelete(segment);
			}
			this.snd_buf.RemoveRange(0, num);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003FB8 File Offset: 0x000021B8
		internal void ParseFastack(uint sn, uint ts)
		{
			if (sn < this.snd_una)
			{
				return;
			}
			if (sn >= this.snd_nxt)
			{
				return;
			}
			foreach (Segment segment in this.snd_buf)
			{
				if (sn < segment.sn)
				{
					break;
				}
				if (sn != segment.sn)
				{
					segment.fastack += 1U;
				}
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x0000403C File Offset: 0x0000223C
		private void AckPush(uint sn, uint ts)
		{
			this.acklist.Add(new AckItem
			{
				serialNumber = sn,
				timestamp = ts
			});
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00004070 File Offset: 0x00002270
		private void ParseData(Segment newseg)
		{
			uint sn = newseg.sn;
			if (Utils.TimeDiff(sn, this.rcv_nxt + this.rcv_wnd) >= 0 || Utils.TimeDiff(sn, this.rcv_nxt) < 0)
			{
				this.SegmentDelete(newseg);
				return;
			}
			this.InsertSegmentInReceiveBuffer(newseg);
			this.MoveReceiveBufferReadySegmentsToQueue();
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000040C0 File Offset: 0x000022C0
		internal void InsertSegmentInReceiveBuffer(Segment newseg)
		{
			bool flag = false;
			int i;
			for (i = this.rcv_buf.Count - 1; i >= 0; i--)
			{
				Segment segment = this.rcv_buf[i];
				if (segment.sn == newseg.sn)
				{
					flag = true;
					break;
				}
				if (Utils.TimeDiff(newseg.sn, segment.sn) > 0)
				{
					break;
				}
			}
			if (!flag)
			{
				this.rcv_buf.Insert(i + 1, newseg);
				return;
			}
			this.SegmentDelete(newseg);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00004134 File Offset: 0x00002334
		private void MoveReceiveBufferReadySegmentsToQueue()
		{
			int num = 0;
			foreach (Segment segment in this.rcv_buf)
			{
				if (segment.sn != this.rcv_nxt || (long)this.rcv_queue.Count >= (long)((ulong)this.rcv_wnd))
				{
					break;
				}
				num++;
				this.rcv_queue.Enqueue(segment);
				this.rcv_nxt += 1U;
			}
			this.rcv_buf.RemoveRange(0, num);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000041D0 File Offset: 0x000023D0
		public int Input(byte[] data, int offset, int size)
		{
			uint earlier = this.snd_una;
			uint num = 0U;
			uint ts = 0U;
			int num2 = 0;
			if (data == null || size < 24)
			{
				return -1;
			}
			while (size >= 24)
			{
				uint num3;
				offset += Utils.Decode32U(data, offset, out num3);
				if (num3 != this.conv)
				{
					return -1;
				}
				byte b;
				offset += Utils.Decode8u(data, offset, out b);
				byte frg;
				offset += Utils.Decode8u(data, offset, out frg);
				ushort wnd;
				offset += Utils.Decode16U(data, offset, out wnd);
				uint num4;
				offset += Utils.Decode32U(data, offset, out num4);
				uint num5;
				offset += Utils.Decode32U(data, offset, out num5);
				uint una;
				offset += Utils.Decode32U(data, offset, out una);
				uint num6;
				offset += Utils.Decode32U(data, offset, out num6);
				size -= 24;
				if ((long)size < (long)((ulong)num6) || num6 < 0U)
				{
					return -2;
				}
				if (b != 81 && b != 82 && b != 83 && b != 84)
				{
					return -3;
				}
				this.rmt_wnd = (uint)wnd;
				this.ParseUna(una);
				this.ShrinkBuf();
				if (b == 82)
				{
					if (Utils.TimeDiff(this.current, num4) >= 0)
					{
						this.UpdateAck(Utils.TimeDiff(this.current, num4));
					}
					this.ParseAck(num5);
					this.ShrinkBuf();
					if (num2 == 0)
					{
						num2 = 1;
						num = num5;
						ts = num4;
					}
					else if (Utils.TimeDiff(num5, num) > 0)
					{
						num = num5;
						ts = num4;
					}
				}
				else if (b == 81)
				{
					if (Utils.TimeDiff(num5, this.rcv_nxt + this.rcv_wnd) < 0)
					{
						this.AckPush(num5, num4);
						if (Utils.TimeDiff(num5, this.rcv_nxt) >= 0)
						{
							Segment segment = this.SegmentNew();
							segment.conv = num3;
							segment.cmd = (uint)b;
							segment.frg = (uint)frg;
							segment.wnd = (uint)wnd;
							segment.ts = num4;
							segment.sn = num5;
							segment.una = una;
							if (num6 > 0U)
							{
								segment.data.Write(data, offset, (int)num6);
							}
							this.ParseData(segment);
						}
					}
				}
				else if (b == 83)
				{
					this.probe |= 2U;
				}
				else if (b != 84)
				{
					return -3;
				}
				offset += (int)num6;
				size -= (int)num6;
			}
			if (num2 != 0)
			{
				this.ParseFastack(num, ts);
			}
			if (Utils.TimeDiff(this.snd_una, earlier) > 0 && this.cwnd < this.rmt_wnd)
			{
				if (this.cwnd < this.ssthresh)
				{
					this.cwnd += 1U;
					this.incr += this.mss;
				}
				else
				{
					if (this.incr < this.mss)
					{
						this.incr = this.mss;
					}
					this.incr += this.mss * this.mss / this.incr + this.mss / 16U;
					if ((this.cwnd + 1U) * this.mss <= this.incr)
					{
						this.cwnd = (this.incr + this.mss - 1U) / ((this.mss > 0U) ? this.mss : 1U);
					}
				}
				if (this.cwnd > this.rmt_wnd)
				{
					this.cwnd = this.rmt_wnd;
					this.incr = this.rmt_wnd * this.mss;
				}
			}
			return 0;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00004500 File Offset: 0x00002700
		private void MakeSpace(ref int size, int space)
		{
			if ((long)(size + space) > (long)((ulong)this.mtu))
			{
				this.output(this.buffer, size);
				size = 0;
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00004526 File Offset: 0x00002726
		private void FlushBuffer(int size)
		{
			if (size > 0)
			{
				this.output(this.buffer, size);
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00004540 File Offset: 0x00002740
		public void Flush()
		{
			int num = 0;
			bool flag = false;
			if (!this.updated)
			{
				return;
			}
			Segment segment = this.SegmentNew();
			segment.conv = this.conv;
			segment.cmd = 82U;
			segment.wnd = this.WndUnused();
			segment.una = this.rcv_nxt;
			foreach (AckItem ackItem in this.acklist)
			{
				this.MakeSpace(ref num, 24);
				segment.sn = ackItem.serialNumber;
				segment.ts = ackItem.timestamp;
				num += segment.Encode(this.buffer, num);
			}
			this.acklist.Clear();
			if (this.rmt_wnd == 0U)
			{
				if (this.probe_wait == 0U)
				{
					this.probe_wait = 7000U;
					this.ts_probe = this.current + this.probe_wait;
				}
				else if (Utils.TimeDiff(this.current, this.ts_probe) >= 0)
				{
					if (this.probe_wait < 7000U)
					{
						this.probe_wait = 7000U;
					}
					this.probe_wait += this.probe_wait / 2U;
					if (this.probe_wait > 120000U)
					{
						this.probe_wait = 120000U;
					}
					this.ts_probe = this.current + this.probe_wait;
					this.probe |= 1U;
				}
			}
			else
			{
				this.ts_probe = 0U;
				this.probe_wait = 0U;
			}
			if ((this.probe & 1U) != 0U)
			{
				segment.cmd = 83U;
				this.MakeSpace(ref num, 24);
				num += segment.Encode(this.buffer, num);
			}
			if ((this.probe & 2U) != 0U)
			{
				segment.cmd = 84U;
				this.MakeSpace(ref num, 24);
				num += segment.Encode(this.buffer, num);
			}
			this.probe = 0U;
			uint num2 = Math.Min(this.snd_wnd, this.rmt_wnd);
			if (!this.nocwnd)
			{
				num2 = Math.Min(this.cwnd, num2);
			}
			while (Utils.TimeDiff(this.snd_nxt, this.snd_una + num2) < 0 && this.snd_queue.Count != 0)
			{
				Segment segment2 = this.snd_queue.Dequeue();
				segment2.conv = this.conv;
				segment2.cmd = 81U;
				segment2.wnd = segment.wnd;
				segment2.ts = this.current;
				segment2.sn = this.snd_nxt;
				this.snd_nxt += 1U;
				segment2.una = this.rcv_nxt;
				segment2.resendts = this.current;
				segment2.rto = this.rx_rto;
				segment2.fastack = 0U;
				segment2.xmit = 0U;
				this.snd_buf.Add(segment2);
			}
			uint num3 = (uint)((this.fastresend > 0) ? this.fastresend : -1);
			uint num4 = (this.nodelay == 0U) ? ((uint)this.rx_rto >> 3) : 0U;
			int num5 = 0;
			foreach (Segment segment3 in this.snd_buf)
			{
				bool flag2 = false;
				if (segment3.xmit == 0U)
				{
					flag2 = true;
					segment3.xmit += 1U;
					segment3.rto = this.rx_rto;
					segment3.resendts = this.current + (uint)segment3.rto + num4;
				}
				else if (Utils.TimeDiff(this.current, segment3.resendts) >= 0)
				{
					flag2 = true;
					segment3.xmit += 1U;
					this.xmit += 1U;
					if (this.nodelay == 0U)
					{
						segment3.rto += Math.Max(segment3.rto, this.rx_rto);
					}
					else
					{
						int num6 = (this.nodelay < 2U) ? segment3.rto : this.rx_rto;
						segment3.rto += num6 / 2;
					}
					segment3.resendts = this.current + (uint)segment3.rto;
					flag = true;
				}
				else if (segment3.fastack >= num3 && ((ulong)segment3.xmit <= (ulong)((long)this.fastlimit) || this.fastlimit <= 0))
				{
					flag2 = true;
					segment3.xmit += 1U;
					segment3.fastack = 0U;
					segment3.resendts = this.current + (uint)segment3.rto;
					num5++;
				}
				if (flag2)
				{
					segment3.ts = this.current;
					segment3.wnd = segment.wnd;
					segment3.una = this.rcv_nxt;
					int space = 24 + (int)segment3.data.Position;
					this.MakeSpace(ref num, space);
					num += segment3.Encode(this.buffer, num);
					if (segment3.data.Position > 0L)
					{
						Buffer.BlockCopy(segment3.data.GetBuffer(), 0, this.buffer, num, (int)segment3.data.Position);
						num += (int)segment3.data.Position;
					}
					if (segment3.xmit >= this.dead_link)
					{
						this.state = -1;
					}
				}
			}
			this.SegmentDelete(segment);
			this.FlushBuffer(num);
			if (num5 > 0)
			{
				uint num7 = this.snd_nxt - this.snd_una;
				this.ssthresh = num7 / 2U;
				if (this.ssthresh < 2U)
				{
					this.ssthresh = 2U;
				}
				this.cwnd = this.ssthresh + num3;
				this.incr = this.cwnd * this.mss;
			}
			if (flag)
			{
				this.ssthresh = num2 / 2U;
				if (this.ssthresh < 2U)
				{
					this.ssthresh = 2U;
				}
				this.cwnd = 1U;
				this.incr = this.mss;
			}
			if (this.cwnd < 1U)
			{
				this.cwnd = 1U;
				this.incr = this.mss;
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00004B50 File Offset: 0x00002D50
		public void Update(uint currentTimeMilliSeconds)
		{
			this.current = currentTimeMilliSeconds;
			if (!this.updated)
			{
				this.updated = true;
				this.ts_flush = this.current;
			}
			int num = Utils.TimeDiff(this.current, this.ts_flush);
			if (num >= 10000 || num < -10000)
			{
				this.ts_flush = this.current;
				num = 0;
			}
			if (num >= 0)
			{
				this.ts_flush += this.interval;
				if (this.current >= this.ts_flush)
				{
					this.ts_flush = this.current + this.interval;
				}
				this.Flush();
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00004BF0 File Offset: 0x00002DF0
		public uint Check(uint current_)
		{
			uint num = this.ts_flush;
			int num2 = int.MaxValue;
			if (!this.updated)
			{
				return current_;
			}
			if (Utils.TimeDiff(current_, num) >= 10000 || Utils.TimeDiff(current_, num) < -10000)
			{
				num = current_;
			}
			if (Utils.TimeDiff(current_, num) >= 0)
			{
				return current_;
			}
			int num3 = Utils.TimeDiff(num, current_);
			foreach (Segment segment in this.snd_buf)
			{
				int num4 = Utils.TimeDiff(segment.resendts, current_);
				if (num4 <= 0)
				{
					return current_;
				}
				if (num4 < num2)
				{
					num2 = num4;
				}
			}
			uint num5 = (uint)((num2 < num3) ? num2 : num3);
			if (num5 >= this.interval)
			{
				num5 = this.interval;
			}
			return current_ + num5;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00004CC4 File Offset: 0x00002EC4
		public void SetMtu(uint mtu)
		{
			if (mtu < 50U || mtu < 24U)
			{
				throw new ArgumentException("MTU must be higher than 50 and higher than OVERHEAD");
			}
			this.buffer = new byte[(mtu + 24U) * 3U];
			this.mtu = mtu;
			this.mss = mtu - 24U;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00004CFD File Offset: 0x00002EFD
		public void SetInterval(uint interval)
		{
			if (interval > 5000U)
			{
				interval = 5000U;
			}
			else if (interval < 10U)
			{
				interval = 10U;
			}
			this.interval = interval;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00004D20 File Offset: 0x00002F20
		public void SetNoDelay(uint nodelay, uint interval = 100U, int resend = 0, bool nocwnd = false)
		{
			this.nodelay = nodelay;
			if (nodelay != 0U)
			{
				this.rx_minrto = 30;
			}
			else
			{
				this.rx_minrto = 100;
			}
			if (interval >= 0U)
			{
				if (interval > 5000U)
				{
					interval = 5000U;
				}
				else if (interval < 10U)
				{
					interval = 10U;
				}
				this.interval = interval;
			}
			if (resend >= 0)
			{
				this.fastresend = resend;
			}
			this.nocwnd = nocwnd;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00004D81 File Offset: 0x00002F81
		public void SetWindowSize(uint sendWindow, uint receiveWindow)
		{
			if (sendWindow > 0U)
			{
				this.snd_wnd = sendWindow;
			}
			if (receiveWindow > 0U)
			{
				this.rcv_wnd = Math.Max(receiveWindow, 128U);
			}
		}

		// Token: 0x04000056 RID: 86
		public const int RTO_NDL = 30;

		// Token: 0x04000057 RID: 87
		public const int RTO_MIN = 100;

		// Token: 0x04000058 RID: 88
		public const int RTO_DEF = 200;

		// Token: 0x04000059 RID: 89
		public const int RTO_MAX = 60000;

		// Token: 0x0400005A RID: 90
		public const int CMD_PUSH = 81;

		// Token: 0x0400005B RID: 91
		public const int CMD_ACK = 82;

		// Token: 0x0400005C RID: 92
		public const int CMD_WASK = 83;

		// Token: 0x0400005D RID: 93
		public const int CMD_WINS = 84;

		// Token: 0x0400005E RID: 94
		public const int ASK_SEND = 1;

		// Token: 0x0400005F RID: 95
		public const int ASK_TELL = 2;

		// Token: 0x04000060 RID: 96
		public const int WND_SND = 32;

		// Token: 0x04000061 RID: 97
		public const int WND_RCV = 128;

		// Token: 0x04000062 RID: 98
		public const int MTU_DEF = 1200;

		// Token: 0x04000063 RID: 99
		public const int ACK_FAST = 3;

		// Token: 0x04000064 RID: 100
		public const int INTERVAL = 100;

		// Token: 0x04000065 RID: 101
		public const int OVERHEAD = 24;

		// Token: 0x04000066 RID: 102
		public const int FRG_MAX = 255;

		// Token: 0x04000067 RID: 103
		public const int DEADLINK = 20;

		// Token: 0x04000068 RID: 104
		public const int THRESH_INIT = 2;

		// Token: 0x04000069 RID: 105
		public const int THRESH_MIN = 2;

		// Token: 0x0400006A RID: 106
		public const int PROBE_INIT = 7000;

		// Token: 0x0400006B RID: 107
		public const int PROBE_LIMIT = 120000;

		// Token: 0x0400006C RID: 108
		public const int FASTACK_LIMIT = 5;

		// Token: 0x0400006D RID: 109
		internal int state;

		// Token: 0x0400006E RID: 110
		private readonly uint conv;

		// Token: 0x0400006F RID: 111
		internal uint mtu;

		// Token: 0x04000070 RID: 112
		internal uint mss;

		// Token: 0x04000071 RID: 113
		internal uint snd_una;

		// Token: 0x04000072 RID: 114
		internal uint snd_nxt;

		// Token: 0x04000073 RID: 115
		internal uint rcv_nxt;

		// Token: 0x04000074 RID: 116
		internal uint ssthresh;

		// Token: 0x04000075 RID: 117
		internal int rx_rttval;

		// Token: 0x04000076 RID: 118
		internal int rx_srtt;

		// Token: 0x04000077 RID: 119
		internal int rx_rto;

		// Token: 0x04000078 RID: 120
		internal int rx_minrto;

		// Token: 0x04000079 RID: 121
		internal uint snd_wnd;

		// Token: 0x0400007A RID: 122
		internal uint rcv_wnd;

		// Token: 0x0400007B RID: 123
		internal uint rmt_wnd;

		// Token: 0x0400007C RID: 124
		internal uint cwnd;

		// Token: 0x0400007D RID: 125
		internal uint probe;

		// Token: 0x0400007E RID: 126
		internal uint interval;

		// Token: 0x0400007F RID: 127
		internal uint ts_flush;

		// Token: 0x04000080 RID: 128
		internal uint xmit;

		// Token: 0x04000081 RID: 129
		internal uint nodelay;

		// Token: 0x04000082 RID: 130
		internal bool updated;

		// Token: 0x04000083 RID: 131
		internal uint ts_probe;

		// Token: 0x04000084 RID: 132
		internal uint probe_wait;

		// Token: 0x04000085 RID: 133
		internal uint dead_link;

		// Token: 0x04000086 RID: 134
		internal uint incr;

		// Token: 0x04000087 RID: 135
		internal uint current;

		// Token: 0x04000088 RID: 136
		internal int fastresend;

		// Token: 0x04000089 RID: 137
		internal int fastlimit;

		// Token: 0x0400008A RID: 138
		internal bool nocwnd;

		// Token: 0x0400008B RID: 139
		internal readonly Queue<Segment> snd_queue = new Queue<Segment>(16);

		// Token: 0x0400008C RID: 140
		internal readonly Queue<Segment> rcv_queue = new Queue<Segment>(16);

		// Token: 0x0400008D RID: 141
		internal readonly List<Segment> snd_buf = new List<Segment>(16);

		// Token: 0x0400008E RID: 142
		internal readonly List<Segment> rcv_buf = new List<Segment>(16);

		// Token: 0x0400008F RID: 143
		internal readonly List<AckItem> acklist = new List<AckItem>(16);

		// Token: 0x04000090 RID: 144
		internal byte[] buffer;

		// Token: 0x04000091 RID: 145
		private readonly Action<byte[], int> output;

		// Token: 0x04000092 RID: 146
		private readonly Pool<Segment> SegmentPool = new Pool<Segment>(() => new Segment(), delegate(Segment segment)
		{
			segment.Reset();
		}, 32);
	}
}
