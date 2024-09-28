using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
	// Token: 0x02000062 RID: 98
	public static class NetworkWriterExtensions
	{
		// Token: 0x060002CE RID: 718 RVA: 0x0000B7C2 File Offset: 0x000099C2
		public static void WriteByte(this NetworkWriter writer, byte value)
		{
			writer.WriteBlittable<byte>(value);
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000B7CB File Offset: 0x000099CB
		public static void WriteByteNullable(this NetworkWriter writer, byte? value)
		{
			writer.WriteBlittableNullable<byte>(value);
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000B7D4 File Offset: 0x000099D4
		public static void WriteSByte(this NetworkWriter writer, sbyte value)
		{
			writer.WriteBlittable<sbyte>(value);
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000B7DD File Offset: 0x000099DD
		public static void WriteSByteNullable(this NetworkWriter writer, sbyte? value)
		{
			writer.WriteBlittableNullable<sbyte>(value);
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000B7E6 File Offset: 0x000099E6
		public static void WriteChar(this NetworkWriter writer, char value)
		{
			writer.WriteBlittable<ushort>((ushort)value);
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000B7F0 File Offset: 0x000099F0
		public static void WriteCharNullable(this NetworkWriter writer, char? value)
		{
			char? c = value;
			writer.WriteBlittableNullable<ushort>((c != null) ? new ushort?((ushort)c.GetValueOrDefault()) : null);
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000B825 File Offset: 0x00009A25
		public static void WriteBool(this NetworkWriter writer, bool value)
		{
			writer.WriteBlittable<byte>(value ? 1 : 0);
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000B838 File Offset: 0x00009A38
		public static void WriteBoolNullable(this NetworkWriter writer, bool? value)
		{
			writer.WriteBlittableNullable<byte>((value != null) ? new byte?(value.Value ? 1 : 0) : null);
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000B872 File Offset: 0x00009A72
		public static void WriteShort(this NetworkWriter writer, short value)
		{
			writer.WriteBlittable<short>(value);
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000B87B File Offset: 0x00009A7B
		public static void WriteShortNullable(this NetworkWriter writer, short? value)
		{
			writer.WriteBlittableNullable<short>(value);
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000B7E6 File Offset: 0x000099E6
		public static void WriteUShort(this NetworkWriter writer, ushort value)
		{
			writer.WriteBlittable<ushort>(value);
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000B884 File Offset: 0x00009A84
		public static void WriteUShortNullable(this NetworkWriter writer, ushort? value)
		{
			writer.WriteBlittableNullable<ushort>(value);
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000B88D File Offset: 0x00009A8D
		public static void WriteInt(this NetworkWriter writer, int value)
		{
			writer.WriteBlittable<int>(value);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000B896 File Offset: 0x00009A96
		public static void WriteIntNullable(this NetworkWriter writer, int? value)
		{
			writer.WriteBlittableNullable<int>(value);
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000B89F File Offset: 0x00009A9F
		public static void WriteUInt(this NetworkWriter writer, uint value)
		{
			writer.WriteBlittable<uint>(value);
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000B8A8 File Offset: 0x00009AA8
		public static void WriteUIntNullable(this NetworkWriter writer, uint? value)
		{
			writer.WriteBlittableNullable<uint>(value);
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000B8B1 File Offset: 0x00009AB1
		public static void WriteLong(this NetworkWriter writer, long value)
		{
			writer.WriteBlittable<long>(value);
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000B8BA File Offset: 0x00009ABA
		public static void WriteLongNullable(this NetworkWriter writer, long? value)
		{
			writer.WriteBlittableNullable<long>(value);
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000B8C3 File Offset: 0x00009AC3
		public static void WriteULong(this NetworkWriter writer, ulong value)
		{
			writer.WriteBlittable<ulong>(value);
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000B8CC File Offset: 0x00009ACC
		public static void WriteULongNullable(this NetworkWriter writer, ulong? value)
		{
			writer.WriteBlittableNullable<ulong>(value);
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000B8D5 File Offset: 0x00009AD5
		public static void WriteFloat(this NetworkWriter writer, float value)
		{
			writer.WriteBlittable<float>(value);
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000B8DE File Offset: 0x00009ADE
		public static void WriteFloatNullable(this NetworkWriter writer, float? value)
		{
			writer.WriteBlittableNullable<float>(value);
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000B8E7 File Offset: 0x00009AE7
		public static void WriteDouble(this NetworkWriter writer, double value)
		{
			writer.WriteBlittable<double>(value);
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000B8F0 File Offset: 0x00009AF0
		public static void WriteDoubleNullable(this NetworkWriter writer, double? value)
		{
			writer.WriteBlittableNullable<double>(value);
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000B8F9 File Offset: 0x00009AF9
		public static void WriteDecimal(this NetworkWriter writer, decimal value)
		{
			writer.WriteBlittable<decimal>(value);
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000B902 File Offset: 0x00009B02
		public static void WriteDecimalNullable(this NetworkWriter writer, decimal? value)
		{
			writer.WriteBlittableNullable<decimal>(value);
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000B90C File Offset: 0x00009B0C
		public static void WriteString(this NetworkWriter writer, string value)
		{
			if (value == null)
			{
				writer.WriteUShort(0);
				return;
			}
			int maxByteCount = writer.encoding.GetMaxByteCount(value.Length);
			writer.EnsureCapacity(writer.Position + 2 + maxByteCount);
			int bytes = writer.encoding.GetBytes(value, 0, value.Length, writer.buffer, writer.Position + 2);
			if (bytes > 65534)
			{
				throw new IndexOutOfRangeException(string.Format("NetworkWriter.WriteString - Value too long: {0} bytes. Limit: {1} bytes", bytes, 65534));
			}
			writer.WriteUShort(checked((ushort)(bytes + 1)));
			writer.Position += bytes;
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000B9A8 File Offset: 0x00009BA8
		public static void WriteBytesAndSizeSegment(this NetworkWriter writer, ArraySegment<byte> buffer)
		{
			writer.WriteBytesAndSize(buffer.Array, buffer.Offset, buffer.Count);
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000B9C5 File Offset: 0x00009BC5
		public static void WriteBytesAndSize(this NetworkWriter writer, byte[] buffer)
		{
			writer.WriteBytesAndSize(buffer, 0, (buffer != null) ? buffer.Length : 0);
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000B9D8 File Offset: 0x00009BD8
		public static void WriteBytesAndSize(this NetworkWriter writer, byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				writer.WriteUInt(0U);
				return;
			}
			writer.WriteUInt(checked((uint)count) + 1U);
			writer.WriteBytes(buffer, offset, count);
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000B9F8 File Offset: 0x00009BF8
		public static void WriteArraySegment<T>(this NetworkWriter writer, ArraySegment<T> segment)
		{
			int count = segment.Count;
			writer.WriteInt(count);
			for (int i = 0; i < count; i++)
			{
				writer.Write<T>(segment.Array[segment.Offset + i]);
			}
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000BA3B File Offset: 0x00009C3B
		public static void WriteVector2(this NetworkWriter writer, Vector2 value)
		{
			writer.WriteBlittable<Vector2>(value);
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000BA44 File Offset: 0x00009C44
		public static void WriteVector2Nullable(this NetworkWriter writer, Vector2? value)
		{
			writer.WriteBlittableNullable<Vector2>(value);
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000BA4D File Offset: 0x00009C4D
		public static void WriteVector3(this NetworkWriter writer, Vector3 value)
		{
			writer.WriteBlittable<Vector3>(value);
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000BA56 File Offset: 0x00009C56
		public static void WriteVector3Nullable(this NetworkWriter writer, Vector3? value)
		{
			writer.WriteBlittableNullable<Vector3>(value);
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000BA5F File Offset: 0x00009C5F
		public static void WriteVector4(this NetworkWriter writer, Vector4 value)
		{
			writer.WriteBlittable<Vector4>(value);
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000BA68 File Offset: 0x00009C68
		public static void WriteVector4Nullable(this NetworkWriter writer, Vector4? value)
		{
			writer.WriteBlittableNullable<Vector4>(value);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000BA71 File Offset: 0x00009C71
		public static void WriteVector2Int(this NetworkWriter writer, Vector2Int value)
		{
			writer.WriteBlittable<Vector2Int>(value);
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000BA7A File Offset: 0x00009C7A
		public static void WriteVector2IntNullable(this NetworkWriter writer, Vector2Int? value)
		{
			writer.WriteBlittableNullable<Vector2Int>(value);
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000BA83 File Offset: 0x00009C83
		public static void WriteVector3Int(this NetworkWriter writer, Vector3Int value)
		{
			writer.WriteBlittable<Vector3Int>(value);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000BA8C File Offset: 0x00009C8C
		public static void WriteVector3IntNullable(this NetworkWriter writer, Vector3Int? value)
		{
			writer.WriteBlittableNullable<Vector3Int>(value);
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000BA95 File Offset: 0x00009C95
		public static void WriteColor(this NetworkWriter writer, Color value)
		{
			writer.WriteBlittable<Color>(value);
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000BA9E File Offset: 0x00009C9E
		public static void WriteColorNullable(this NetworkWriter writer, Color? value)
		{
			writer.WriteBlittableNullable<Color>(value);
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000BAA7 File Offset: 0x00009CA7
		public static void WriteColor32(this NetworkWriter writer, Color32 value)
		{
			writer.WriteBlittable<Color32>(value);
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000BAB0 File Offset: 0x00009CB0
		public static void WriteColor32Nullable(this NetworkWriter writer, Color32? value)
		{
			writer.WriteBlittableNullable<Color32>(value);
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000BAB9 File Offset: 0x00009CB9
		public static void WriteQuaternion(this NetworkWriter writer, Quaternion value)
		{
			writer.WriteBlittable<Quaternion>(value);
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000BAC2 File Offset: 0x00009CC2
		public static void WriteQuaternionNullable(this NetworkWriter writer, Quaternion? value)
		{
			writer.WriteBlittableNullable<Quaternion>(value);
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000BACB File Offset: 0x00009CCB
		public static void WriteRect(this NetworkWriter writer, Rect value)
		{
			writer.WriteVector2(value.position);
			writer.WriteVector2(value.size);
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000BAE7 File Offset: 0x00009CE7
		public static void WriteRectNullable(this NetworkWriter writer, Rect? value)
		{
			writer.WriteBool(value != null);
			if (value != null)
			{
				writer.WriteRect(value.Value);
			}
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000BB0C File Offset: 0x00009D0C
		public static void WritePlane(this NetworkWriter writer, Plane value)
		{
			writer.WriteVector3(value.normal);
			writer.WriteFloat(value.distance);
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000BB28 File Offset: 0x00009D28
		public static void WritePlaneNullable(this NetworkWriter writer, Plane? value)
		{
			writer.WriteBool(value != null);
			if (value != null)
			{
				writer.WritePlane(value.Value);
			}
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000BB4D File Offset: 0x00009D4D
		public static void WriteRay(this NetworkWriter writer, Ray value)
		{
			writer.WriteVector3(value.origin);
			writer.WriteVector3(value.direction);
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000BB69 File Offset: 0x00009D69
		public static void WriteRayNullable(this NetworkWriter writer, Ray? value)
		{
			writer.WriteBool(value != null);
			if (value != null)
			{
				writer.WriteRay(value.Value);
			}
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000BB8E File Offset: 0x00009D8E
		public static void WriteMatrix4x4(this NetworkWriter writer, Matrix4x4 value)
		{
			writer.WriteBlittable<Matrix4x4>(value);
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000BB97 File Offset: 0x00009D97
		public static void WriteMatrix4x4Nullable(this NetworkWriter writer, Matrix4x4? value)
		{
			writer.WriteBlittableNullable<Matrix4x4>(value);
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000BBA0 File Offset: 0x00009DA0
		public static void WriteGuid(this NetworkWriter writer, Guid value)
		{
			writer.EnsureCapacity(writer.Position + 16);
			value.TryWriteBytes(new Span<byte>(writer.buffer, writer.Position, 16));
			writer.Position += 16;
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000BBDB File Offset: 0x00009DDB
		public static void WriteGuidNullable(this NetworkWriter writer, Guid? value)
		{
			writer.WriteBool(value != null);
			if (value != null)
			{
				writer.WriteGuid(value.Value);
			}
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000BC00 File Offset: 0x00009E00
		public static void WriteNetworkIdentity(this NetworkWriter writer, NetworkIdentity value)
		{
			if (value == null)
			{
				writer.WriteUInt(0U);
				return;
			}
			if (value.netId == 0U)
			{
				Debug.LogWarning("Attempted to serialize unspawned GameObject: " + value.name + ". Prefabs and unspawned GameObjects would always be null on the other side. Please spawn it before using it in [SyncVar]s/Rpcs/Cmds/NetworkMessages etc.");
			}
			writer.WriteUInt(value.netId);
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000BC4C File Offset: 0x00009E4C
		public static void WriteNetworkBehaviour(this NetworkWriter writer, NetworkBehaviour value)
		{
			if (value == null)
			{
				writer.WriteUInt(0U);
				return;
			}
			if (value.netId == 0U)
			{
				Debug.LogWarning(string.Format("Attempted to serialize unspawned NetworkBehaviour: of type {0} on GameObject {1}. Prefabs and unspawned GameObjects would always be null on the other side. Please spawn it before using it in [SyncVar]s/Rpcs/Cmds/NetworkMessages etc.", value.GetType(), value.name));
				writer.WriteUInt(0U);
				return;
			}
			writer.WriteUInt(value.netId);
			writer.WriteByte(value.ComponentIndex);
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000BCB0 File Offset: 0x00009EB0
		public static void WriteTransform(this NetworkWriter writer, Transform value)
		{
			if (value == null)
			{
				writer.WriteUInt(0U);
				return;
			}
			NetworkIdentity networkIdentity;
			if (value.TryGetComponent<NetworkIdentity>(out networkIdentity))
			{
				writer.WriteUInt(networkIdentity.netId);
				return;
			}
			Debug.LogWarning(string.Format("NetworkWriter {0} has no NetworkIdentity", value));
			writer.WriteUInt(0U);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000BCFC File Offset: 0x00009EFC
		public static void WriteGameObject(this NetworkWriter writer, GameObject value)
		{
			if (value == null)
			{
				writer.WriteUInt(0U);
				return;
			}
			NetworkIdentity value2;
			if (!value.TryGetComponent<NetworkIdentity>(out value2))
			{
				Debug.LogWarning(string.Format("NetworkWriter {0} has no NetworkIdentity", value));
			}
			writer.WriteNetworkIdentity(value2);
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000BD3C File Offset: 0x00009F3C
		public static void WriteList<T>(this NetworkWriter writer, List<T> list)
		{
			if (list == null)
			{
				writer.WriteInt(-1);
				return;
			}
			if (list.Count > 16777216)
			{
				throw new IndexOutOfRangeException(string.Format("NetworkWriter.WriteList - List<{0}> too big: {1} elements. Limit: {2}", typeof(T), list.Count, 16777216));
			}
			writer.WriteInt(list.Count);
			for (int i = 0; i < list.Count; i++)
			{
				writer.Write<T>(list[i]);
			}
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000BDBC File Offset: 0x00009FBC
		public static void WriteArray<T>(this NetworkWriter writer, T[] array)
		{
			if (array == null)
			{
				writer.WriteInt(-1);
				return;
			}
			if (array.Length > 16777216)
			{
				throw new IndexOutOfRangeException(string.Format("NetworkWriter.WriteArray - Array<{0}> too big: {1} elements. Limit: {2}", typeof(T), array.Length, 16777216));
			}
			writer.WriteInt(array.Length);
			for (int i = 0; i < array.Length; i++)
			{
				writer.Write<T>(array[i]);
			}
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000BE2E File Offset: 0x0000A02E
		public static void WriteUri(this NetworkWriter writer, Uri uri)
		{
			writer.WriteString((uri != null) ? uri.ToString() : null);
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000BE44 File Offset: 0x0000A044
		public static void WriteTexture2D(this NetworkWriter writer, Texture2D texture2D)
		{
			if (texture2D == null)
			{
				writer.WriteShort(-1);
				return;
			}
			int num = texture2D.width * texture2D.height;
			if (num > 16777216)
			{
				throw new IndexOutOfRangeException(string.Format("NetworkWriter.WriteTexture2D - Texture2D total size (width*height) too big: {0}. Limit: {1}", num, 16777216));
			}
			writer.WriteShort((short)texture2D.width);
			writer.WriteShort((short)texture2D.height);
			writer.WriteArray(texture2D.GetPixels32());
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000BEBE File Offset: 0x0000A0BE
		public static void WriteSprite(this NetworkWriter writer, Sprite sprite)
		{
			if (sprite == null)
			{
				writer.WriteTexture2D(null);
				return;
			}
			writer.WriteTexture2D(sprite.texture);
			writer.WriteRect(sprite.rect);
			writer.WriteVector2(sprite.pivot);
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000BEF5 File Offset: 0x0000A0F5
		public static void WriteDateTime(this NetworkWriter writer, DateTime dateTime)
		{
			writer.WriteDouble(dateTime.ToOADate());
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000BF04 File Offset: 0x0000A104
		public static void WriteDateTimeNullable(this NetworkWriter writer, DateTime? dateTime)
		{
			writer.WriteBool(dateTime != null);
			if (dateTime != null)
			{
				writer.WriteDouble(dateTime.Value.ToOADate());
			}
		}
	}
}
