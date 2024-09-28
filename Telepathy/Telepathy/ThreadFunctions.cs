using System;
using System.Net.Sockets;
using System.Threading;

namespace Telepathy
{
	// Token: 0x0200000E RID: 14
	public static class ThreadFunctions
	{
		// Token: 0x06000032 RID: 50 RVA: 0x00003394 File Offset: 0x00001594
		public static bool SendMessagesBlocking(NetworkStream stream, byte[] payload, int packetSize)
		{
			bool result;
			try
			{
				stream.Write(payload, 0, packetSize);
				result = true;
			}
			catch (Exception ex)
			{
				Action<string> info = Log.Info;
				string str = "[Telepathy] Send: stream.Write exception: ";
				Exception ex2 = ex;
				info(str + ((ex2 != null) ? ex2.ToString() : null));
				result = false;
			}
			return result;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000033E8 File Offset: 0x000015E8
		public static bool ReadMessageBlocking(NetworkStream stream, int MaxMessageSize, byte[] headerBuffer, byte[] payloadBuffer, out int size)
		{
			size = 0;
			if (payloadBuffer.Length != 4 + MaxMessageSize)
			{
				Log.Error(string.Format("[Telepathy] ReadMessageBlocking: payloadBuffer needs to be of size 4 + MaxMessageSize = {0} instead of {1}", 4 + MaxMessageSize, payloadBuffer.Length));
				return false;
			}
			if (!stream.ReadExactly(headerBuffer, 4))
			{
				return false;
			}
			size = Utils.BytesToIntBigEndian(headerBuffer);
			if (size > 0 && size <= MaxMessageSize)
			{
				return stream.ReadExactly(payloadBuffer, size);
			}
			Log.Warning("[Telepathy] ReadMessageBlocking: possible header attack with a header of: " + size.ToString() + " bytes.");
			return false;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003474 File Offset: 0x00001674
		public static void ReceiveLoop(int connectionId, TcpClient client, int MaxMessageSize, MagnificentReceivePipe receivePipe, int QueueLimit)
		{
			NetworkStream stream = client.GetStream();
			byte[] array = new byte[4 + MaxMessageSize];
			byte[] headerBuffer = new byte[4];
			try
			{
				receivePipe.Enqueue(connectionId, EventType.Connected, default(ArraySegment<byte>));
				int count;
				while (ThreadFunctions.ReadMessageBlocking(stream, MaxMessageSize, headerBuffer, array, out count))
				{
					ArraySegment<byte> message = new ArraySegment<byte>(array, 0, count);
					receivePipe.Enqueue(connectionId, EventType.Data, message);
					if (receivePipe.Count(connectionId) >= QueueLimit)
					{
						Log.Warning(string.Format("[Telepathy] ReceivePipe reached limit of {0} for connectionId {1}. This can happen if network messages come in way faster than we manage to process them. Disconnecting this connection for load balancing.", QueueLimit, connectionId));
						break;
					}
				}
			}
			catch (Exception ex)
			{
				Action<string> info = Log.Info;
				string str = "[Telepathy] ReceiveLoop finished receive function for connectionId=";
				string str2 = connectionId.ToString();
				string str3 = " reason: ";
				Exception ex2 = ex;
				info(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
			}
			finally
			{
				stream.Close();
				client.Close();
				receivePipe.Enqueue(connectionId, EventType.Disconnected, default(ArraySegment<byte>));
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003568 File Offset: 0x00001768
		public static void SendLoop(int connectionId, TcpClient client, MagnificentSendPipe sendPipe, ManualResetEvent sendPending)
		{
			NetworkStream stream = client.GetStream();
			byte[] payload = null;
			try
			{
				while (client.Connected)
				{
					sendPending.Reset();
					int packetSize;
					if (sendPipe.DequeueAndSerializeAll(ref payload, out packetSize) && !ThreadFunctions.SendMessagesBlocking(stream, payload, packetSize))
					{
						break;
					}
					sendPending.WaitOne();
				}
			}
			catch (ThreadAbortException)
			{
			}
			catch (ThreadInterruptedException)
			{
			}
			catch (Exception ex)
			{
				Action<string> info = Log.Info;
				string str = "[Telepathy] SendLoop Exception: connectionId=";
				string str2 = connectionId.ToString();
				string str3 = " reason: ";
				Exception ex2 = ex;
				info(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
			}
			finally
			{
				stream.Close();
				client.Close();
			}
		}
	}
}
