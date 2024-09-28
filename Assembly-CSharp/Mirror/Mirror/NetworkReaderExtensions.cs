using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Mirror
{
	// Token: 0x02000057 RID: 87
	public static class NetworkReaderExtensions
	{
		// Token: 0x0600020D RID: 525 RVA: 0x0000922F File Offset: 0x0000742F
		public static byte ReadByte(this NetworkReader reader)
		{
			return reader.ReadBlittable<byte>();
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00009237 File Offset: 0x00007437
		public static byte? ReadByteNullable(this NetworkReader reader)
		{
			return reader.ReadBlittableNullable<byte>();
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000923F File Offset: 0x0000743F
		public static sbyte ReadSByte(this NetworkReader reader)
		{
			return reader.ReadBlittable<sbyte>();
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00009247 File Offset: 0x00007447
		public static sbyte? ReadSByteNullable(this NetworkReader reader)
		{
			return reader.ReadBlittableNullable<sbyte>();
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000924F File Offset: 0x0000744F
		public static char ReadChar(this NetworkReader reader)
		{
			return (char)reader.ReadBlittable<ushort>();
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00009258 File Offset: 0x00007458
		public static char? ReadCharNullable(this NetworkReader reader)
		{
			ushort? num = reader.ReadBlittableNullable<ushort>();
			if (num == null)
			{
				return null;
			}
			return new char?((char)num.GetValueOrDefault());
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000928B File Offset: 0x0000748B
		public static bool ReadBool(this NetworkReader reader)
		{
			return reader.ReadBlittable<byte>() > 0;
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00009298 File Offset: 0x00007498
		public static bool? ReadBoolNullable(this NetworkReader reader)
		{
			byte? b = reader.ReadBlittableNullable<byte>();
			if (b == null)
			{
				return null;
			}
			return new bool?(b.Value > 0);
		}

		// Token: 0x06000215 RID: 533 RVA: 0x000092CE File Offset: 0x000074CE
		public static short ReadShort(this NetworkReader reader)
		{
			return (short)reader.ReadUShort();
		}

		// Token: 0x06000216 RID: 534 RVA: 0x000092D7 File Offset: 0x000074D7
		public static short? ReadShortNullable(this NetworkReader reader)
		{
			return reader.ReadBlittableNullable<short>();
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000924F File Offset: 0x0000744F
		public static ushort ReadUShort(this NetworkReader reader)
		{
			return reader.ReadBlittable<ushort>();
		}

		// Token: 0x06000218 RID: 536 RVA: 0x000092DF File Offset: 0x000074DF
		public static ushort? ReadUShortNullable(this NetworkReader reader)
		{
			return reader.ReadBlittableNullable<ushort>();
		}

		// Token: 0x06000219 RID: 537 RVA: 0x000092E7 File Offset: 0x000074E7
		public static int ReadInt(this NetworkReader reader)
		{
			return reader.ReadBlittable<int>();
		}

		// Token: 0x0600021A RID: 538 RVA: 0x000092EF File Offset: 0x000074EF
		public static int? ReadIntNullable(this NetworkReader reader)
		{
			return reader.ReadBlittableNullable<int>();
		}

		// Token: 0x0600021B RID: 539 RVA: 0x000092F7 File Offset: 0x000074F7
		public static uint ReadUInt(this NetworkReader reader)
		{
			return reader.ReadBlittable<uint>();
		}

		// Token: 0x0600021C RID: 540 RVA: 0x000092FF File Offset: 0x000074FF
		public static uint? ReadUIntNullable(this NetworkReader reader)
		{
			return reader.ReadBlittableNullable<uint>();
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00009307 File Offset: 0x00007507
		public static long ReadLong(this NetworkReader reader)
		{
			return reader.ReadBlittable<long>();
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000930F File Offset: 0x0000750F
		public static long? ReadLongNullable(this NetworkReader reader)
		{
			return reader.ReadBlittableNullable<long>();
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00009317 File Offset: 0x00007517
		public static ulong ReadULong(this NetworkReader reader)
		{
			return reader.ReadBlittable<ulong>();
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000931F File Offset: 0x0000751F
		public static ulong? ReadULongNullable(this NetworkReader reader)
		{
			return reader.ReadBlittableNullable<ulong>();
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00009327 File Offset: 0x00007527
		public static float ReadFloat(this NetworkReader reader)
		{
			return reader.ReadBlittable<float>();
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000932F File Offset: 0x0000752F
		public static float? ReadFloatNullable(this NetworkReader reader)
		{
			return reader.ReadBlittableNullable<float>();
		}

		// Token: 0x06000223 RID: 547 RVA: 0x00009337 File Offset: 0x00007537
		public static double ReadDouble(this NetworkReader reader)
		{
			return reader.ReadBlittable<double>();
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000933F File Offset: 0x0000753F
		public static double? ReadDoubleNullable(this NetworkReader reader)
		{
			return reader.ReadBlittableNullable<double>();
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00009347 File Offset: 0x00007547
		public static decimal ReadDecimal(this NetworkReader reader)
		{
			return reader.ReadBlittable<decimal>();
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000934F File Offset: 0x0000754F
		public static decimal? ReadDecimalNullable(this NetworkReader reader)
		{
			return reader.ReadBlittableNullable<decimal>();
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00009358 File Offset: 0x00007558
		public static string ReadString(this NetworkReader reader)
		{
			ushort num = reader.ReadUShort();
			if (num == 0)
			{
				return null;
			}
			ushort num2 = num - 1;
			if (num2 > 65534)
			{
				throw new EndOfStreamException(string.Format("NetworkReader.ReadString - Value too long: {0} bytes. Limit is: {1} bytes", num2, 65534));
			}
			ArraySegment<byte> arraySegment = reader.ReadBytesSegment((int)num2);
			return reader.encoding.GetString(arraySegment.Array, arraySegment.Offset, arraySegment.Count);
		}

		// Token: 0x06000228 RID: 552 RVA: 0x000093C8 File Offset: 0x000075C8
		public static byte[] ReadBytesAndSize(this NetworkReader reader)
		{
			uint num = reader.ReadUInt();
			if (num != 0U)
			{
				return reader.ReadBytes(checked((int)(num - 1U)));
			}
			return null;
		}

		// Token: 0x06000229 RID: 553 RVA: 0x000093EC File Offset: 0x000075EC
		public static byte[] ReadBytes(this NetworkReader reader, int count)
		{
			if (count > 16777216)
			{
				throw new EndOfStreamException(string.Format("NetworkReader attempted to allocate {0} bytes, which is larger than the allowed limit of {1} bytes.", count, 16777216));
			}
			byte[] array = new byte[count];
			reader.ReadBytes(array, count);
			return array;
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00009434 File Offset: 0x00007634
		public static ArraySegment<byte> ReadBytesAndSizeSegment(this NetworkReader reader)
		{
			uint num = reader.ReadUInt();
			if (num != 0U)
			{
				return reader.ReadBytesSegment(checked((int)(num - 1U)));
			}
			return default(ArraySegment<byte>);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000945F File Offset: 0x0000765F
		public static Vector2 ReadVector2(this NetworkReader reader)
		{
			return reader.ReadBlittable<Vector2>();
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00009467 File Offset: 0x00007667
		public static Vector2? ReadVector2Nullable(this NetworkReader reader)
		{
			return reader.ReadBlittableNullable<Vector2>();
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000946F File Offset: 0x0000766F
		public static Vector3 ReadVector3(this NetworkReader reader)
		{
			return reader.ReadBlittable<Vector3>();
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00009477 File Offset: 0x00007677
		public static Vector3? ReadVector3Nullable(this NetworkReader reader)
		{
			return reader.ReadBlittableNullable<Vector3>();
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000947F File Offset: 0x0000767F
		public static Vector4 ReadVector4(this NetworkReader reader)
		{
			return reader.ReadBlittable<Vector4>();
		}

		// Token: 0x06000230 RID: 560 RVA: 0x00009487 File Offset: 0x00007687
		public static Vector4? ReadVector4Nullable(this NetworkReader reader)
		{
			return reader.ReadBlittableNullable<Vector4>();
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000948F File Offset: 0x0000768F
		public static Vector2Int ReadVector2Int(this NetworkReader reader)
		{
			return reader.ReadBlittable<Vector2Int>();
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00009497 File Offset: 0x00007697
		public static Vector2Int? ReadVector2IntNullable(this NetworkReader reader)
		{
			return reader.ReadBlittableNullable<Vector2Int>();
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000949F File Offset: 0x0000769F
		public static Vector3Int ReadVector3Int(this NetworkReader reader)
		{
			return reader.ReadBlittable<Vector3Int>();
		}

		// Token: 0x06000234 RID: 564 RVA: 0x000094A7 File Offset: 0x000076A7
		public static Vector3Int? ReadVector3IntNullable(this NetworkReader reader)
		{
			return reader.ReadBlittableNullable<Vector3Int>();
		}

		// Token: 0x06000235 RID: 565 RVA: 0x000094AF File Offset: 0x000076AF
		public static Color ReadColor(this NetworkReader reader)
		{
			return reader.ReadBlittable<Color>();
		}

		// Token: 0x06000236 RID: 566 RVA: 0x000094B7 File Offset: 0x000076B7
		public static Color? ReadColorNullable(this NetworkReader reader)
		{
			return reader.ReadBlittableNullable<Color>();
		}

		// Token: 0x06000237 RID: 567 RVA: 0x000094BF File Offset: 0x000076BF
		public static Color32 ReadColor32(this NetworkReader reader)
		{
			return reader.ReadBlittable<Color32>();
		}

		// Token: 0x06000238 RID: 568 RVA: 0x000094C7 File Offset: 0x000076C7
		public static Color32? ReadColor32Nullable(this NetworkReader reader)
		{
			return reader.ReadBlittableNullable<Color32>();
		}

		// Token: 0x06000239 RID: 569 RVA: 0x000094CF File Offset: 0x000076CF
		public static Quaternion ReadQuaternion(this NetworkReader reader)
		{
			return reader.ReadBlittable<Quaternion>();
		}

		// Token: 0x0600023A RID: 570 RVA: 0x000094D7 File Offset: 0x000076D7
		public static Quaternion? ReadQuaternionNullable(this NetworkReader reader)
		{
			return reader.ReadBlittableNullable<Quaternion>();
		}

		// Token: 0x0600023B RID: 571 RVA: 0x000094DF File Offset: 0x000076DF
		public static Rect ReadRect(this NetworkReader reader)
		{
			return new Rect(reader.ReadVector2(), reader.ReadVector2());
		}

		// Token: 0x0600023C RID: 572 RVA: 0x000094F4 File Offset: 0x000076F4
		public static Rect? ReadRectNullable(this NetworkReader reader)
		{
			if (!reader.ReadBool())
			{
				return null;
			}
			return new Rect?(reader.ReadRect());
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000951E File Offset: 0x0000771E
		public static Plane ReadPlane(this NetworkReader reader)
		{
			return new Plane(reader.ReadVector3(), reader.ReadFloat());
		}

		// Token: 0x0600023E RID: 574 RVA: 0x00009534 File Offset: 0x00007734
		public static Plane? ReadPlaneNullable(this NetworkReader reader)
		{
			if (!reader.ReadBool())
			{
				return null;
			}
			return new Plane?(reader.ReadPlane());
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000955E File Offset: 0x0000775E
		public static Ray ReadRay(this NetworkReader reader)
		{
			return new Ray(reader.ReadVector3(), reader.ReadVector3());
		}

		// Token: 0x06000240 RID: 576 RVA: 0x00009574 File Offset: 0x00007774
		public static Ray? ReadRayNullable(this NetworkReader reader)
		{
			if (!reader.ReadBool())
			{
				return null;
			}
			return new Ray?(reader.ReadRay());
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000959E File Offset: 0x0000779E
		public static Matrix4x4 ReadMatrix4x4(this NetworkReader reader)
		{
			return reader.ReadBlittable<Matrix4x4>();
		}

		// Token: 0x06000242 RID: 578 RVA: 0x000095A6 File Offset: 0x000077A6
		public static Matrix4x4? ReadMatrix4x4Nullable(this NetworkReader reader)
		{
			return reader.ReadBlittableNullable<Matrix4x4>();
		}

		// Token: 0x06000243 RID: 579 RVA: 0x000095B0 File Offset: 0x000077B0
		public static Guid ReadGuid(this NetworkReader reader)
		{
			if (reader.Remaining >= 16)
			{
				ReadOnlySpan<byte> b = new ReadOnlySpan<byte>(reader.buffer.Array, reader.buffer.Offset + reader.Position, 16);
				reader.Position += 16;
				return new Guid(b);
			}
			throw new EndOfStreamException(string.Format("ReadGuid out of range: {0}", reader));
		}

		// Token: 0x06000244 RID: 580 RVA: 0x00009610 File Offset: 0x00007810
		public static Guid? ReadGuidNullable(this NetworkReader reader)
		{
			if (!reader.ReadBool())
			{
				return null;
			}
			return new Guid?(reader.ReadGuid());
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000963C File Offset: 0x0000783C
		public static NetworkIdentity ReadNetworkIdentity(this NetworkReader reader)
		{
			uint num = reader.ReadUInt();
			if (num == 0U)
			{
				return null;
			}
			return Utils.GetSpawnedInServerOrClient(num);
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000965C File Offset: 0x0000785C
		public static NetworkBehaviour ReadNetworkBehaviour(this NetworkReader reader)
		{
			uint num = reader.ReadUInt();
			if (num == 0U)
			{
				return null;
			}
			byte b = reader.ReadByte();
			NetworkIdentity spawnedInServerOrClient = Utils.GetSpawnedInServerOrClient(num);
			if (!(spawnedInServerOrClient != null))
			{
				return null;
			}
			return spawnedInServerOrClient.NetworkBehaviours[(int)b];
		}

		// Token: 0x06000247 RID: 583 RVA: 0x00009696 File Offset: 0x00007896
		public static T ReadNetworkBehaviour<T>(this NetworkReader reader) where T : NetworkBehaviour
		{
			return reader.ReadNetworkBehaviour() as T;
		}

		// Token: 0x06000248 RID: 584 RVA: 0x000096A8 File Offset: 0x000078A8
		public static NetworkBehaviourSyncVar ReadNetworkBehaviourSyncVar(this NetworkReader reader)
		{
			uint num = reader.ReadUInt();
			byte componentIndex = 0;
			if (num != 0U)
			{
				componentIndex = reader.ReadByte();
			}
			return new NetworkBehaviourSyncVar(num, (int)componentIndex);
		}

		// Token: 0x06000249 RID: 585 RVA: 0x000096D0 File Offset: 0x000078D0
		public static Transform ReadTransform(this NetworkReader reader)
		{
			NetworkIdentity networkIdentity = reader.ReadNetworkIdentity();
			if (!(networkIdentity != null))
			{
				return null;
			}
			return networkIdentity.transform;
		}

		// Token: 0x0600024A RID: 586 RVA: 0x000096F8 File Offset: 0x000078F8
		public static GameObject ReadGameObject(this NetworkReader reader)
		{
			NetworkIdentity networkIdentity = reader.ReadNetworkIdentity();
			if (!(networkIdentity != null))
			{
				return null;
			}
			return networkIdentity.gameObject;
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00009720 File Offset: 0x00007920
		public static List<T> ReadList<T>(this NetworkReader reader)
		{
			int num = reader.ReadInt();
			if (num < 0)
			{
				return null;
			}
			if (num > 16777216)
			{
				throw new EndOfStreamException(string.Format("NetworkReader attempted to allocate a List<{0}> {1} elements, which is larger than the allowed limit of {2}.", typeof(T), num, 16777216));
			}
			List<T> list = new List<T>(num);
			for (int i = 0; i < num; i++)
			{
				list.Add(reader.Read<T>());
			}
			return list;
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000978C File Offset: 0x0000798C
		public static T[] ReadArray<T>(this NetworkReader reader)
		{
			int num = reader.ReadInt();
			if (num < 0)
			{
				return null;
			}
			if (num > 16777216)
			{
				throw new EndOfStreamException(string.Format("NetworkReader attempted to allocate an Array<{0}> with {1} elements, which is larger than the allowed limit of {2}.", typeof(T), num, 16777216));
			}
			T[] array = new T[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = reader.Read<T>();
			}
			return array;
		}

		// Token: 0x0600024D RID: 589 RVA: 0x000097FC File Offset: 0x000079FC
		public static Uri ReadUri(this NetworkReader reader)
		{
			string text = reader.ReadString();
			if (!string.IsNullOrWhiteSpace(text))
			{
				return new Uri(text);
			}
			return null;
		}

		// Token: 0x0600024E RID: 590 RVA: 0x00009820 File Offset: 0x00007A20
		public static Texture2D ReadTexture2D(this NetworkReader reader)
		{
			short num = reader.ReadShort();
			if (num == -1)
			{
				return null;
			}
			short num2 = reader.ReadShort();
			int num3 = (int)(num * num2);
			if (num3 > 16777216)
			{
				Debug.LogWarning(string.Format("NetworkReader attempted to allocate a Texture2D with total size (width * height) of {0}, which is larger than the allowed limit of {1}.", num3, 16777216));
				return null;
			}
			Texture2D texture2D = new Texture2D((int)num, (int)num2);
			Color32[] pixels = reader.ReadArray<Color32>();
			texture2D.SetPixels32(pixels);
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000988C File Offset: 0x00007A8C
		public static Sprite ReadSprite(this NetworkReader reader)
		{
			Texture2D texture2D = reader.ReadTexture2D();
			if (texture2D == null)
			{
				return null;
			}
			return Sprite.Create(texture2D, reader.ReadRect(), reader.ReadVector2());
		}

		// Token: 0x06000250 RID: 592 RVA: 0x000098BD File Offset: 0x00007ABD
		public static DateTime ReadDateTime(this NetworkReader reader)
		{
			return DateTime.FromOADate(reader.ReadDouble());
		}

		// Token: 0x06000251 RID: 593 RVA: 0x000098CC File Offset: 0x00007ACC
		public static DateTime? ReadDateTimeNullable(this NetworkReader reader)
		{
			if (!reader.ReadBool())
			{
				return null;
			}
			return new DateTime?(reader.ReadDateTime());
		}
	}
}
