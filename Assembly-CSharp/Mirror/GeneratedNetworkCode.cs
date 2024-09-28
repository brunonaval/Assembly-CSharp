using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Mirror
{
	// Token: 0x020006D4 RID: 1748
	[StructLayout(LayoutKind.Auto, CharSet = CharSet.Auto)]
	public static class GeneratedNetworkCode
	{
		// Token: 0x060025DF RID: 9695 RVA: 0x000B5A70 File Offset: 0x000B3C70
		public static TimeSnapshotMessage TimeSnapshotMessage(NetworkReader reader)
		{
			return default(TimeSnapshotMessage);
		}

		// Token: 0x060025E0 RID: 9696 RVA: 0x000B5A88 File Offset: 0x000B3C88
		public static void TimeSnapshotMessage(NetworkWriter writer, TimeSnapshotMessage value)
		{
		}

		// Token: 0x060025E1 RID: 9697 RVA: 0x000B5A98 File Offset: 0x000B3C98
		public static ReadyMessage ReadyMessage(NetworkReader reader)
		{
			return default(ReadyMessage);
		}

		// Token: 0x060025E2 RID: 9698 RVA: 0x000B5AB0 File Offset: 0x000B3CB0
		public static void ReadyMessage(NetworkWriter writer, ReadyMessage value)
		{
		}

		// Token: 0x060025E3 RID: 9699 RVA: 0x000B5AC0 File Offset: 0x000B3CC0
		public static NotReadyMessage NotReadyMessage(NetworkReader reader)
		{
			return default(NotReadyMessage);
		}

		// Token: 0x060025E4 RID: 9700 RVA: 0x000B5AD8 File Offset: 0x000B3CD8
		public static void NotReadyMessage(NetworkWriter writer, NotReadyMessage value)
		{
		}

		// Token: 0x060025E5 RID: 9701 RVA: 0x000B5AE8 File Offset: 0x000B3CE8
		public static AddPlayerMessage AddPlayerMessage(NetworkReader reader)
		{
			return default(AddPlayerMessage);
		}

		// Token: 0x060025E6 RID: 9702 RVA: 0x000B5B00 File Offset: 0x000B3D00
		public static void AddPlayerMessage(NetworkWriter writer, AddPlayerMessage value)
		{
		}

		// Token: 0x060025E7 RID: 9703 RVA: 0x000B5B10 File Offset: 0x000B3D10
		public static SceneMessage SceneMessage(NetworkReader reader)
		{
			return new SceneMessage
			{
				sceneName = reader.ReadString(),
				sceneOperation = GeneratedNetworkCode._Read_Mirror.SceneOperation(reader),
				customHandling = reader.ReadBool()
			};
		}

		// Token: 0x060025E8 RID: 9704 RVA: 0x000B5B58 File Offset: 0x000B3D58
		public static SceneOperation SceneOperation(NetworkReader reader)
		{
			return (SceneOperation)reader.ReadByte();
		}

		// Token: 0x060025E9 RID: 9705 RVA: 0x000B5B6C File Offset: 0x000B3D6C
		public static void SceneMessage(NetworkWriter writer, SceneMessage value)
		{
			writer.WriteString(value.sceneName);
			GeneratedNetworkCode._Write_Mirror.SceneOperation(writer, value.sceneOperation);
			writer.WriteBool(value.customHandling);
		}

		// Token: 0x060025EA RID: 9706 RVA: 0x000B5BA0 File Offset: 0x000B3DA0
		public static void SceneOperation(NetworkWriter writer, SceneOperation value)
		{
			writer.WriteByte((byte)value);
		}

		// Token: 0x060025EB RID: 9707 RVA: 0x000B5BB4 File Offset: 0x000B3DB4
		public static CommandMessage CommandMessage(NetworkReader reader)
		{
			return new CommandMessage
			{
				netId = reader.ReadUInt(),
				componentIndex = reader.ReadByte(),
				functionHash = reader.ReadUShort(),
				payload = reader.ReadBytesAndSizeSegment()
			};
		}

		// Token: 0x060025EC RID: 9708 RVA: 0x000B5C08 File Offset: 0x000B3E08
		public static void CommandMessage(NetworkWriter writer, CommandMessage value)
		{
			writer.WriteUInt(value.netId);
			writer.WriteByte(value.componentIndex);
			writer.WriteUShort(value.functionHash);
			writer.WriteBytesAndSizeSegment(value.payload);
		}

		// Token: 0x060025ED RID: 9709 RVA: 0x000B5C48 File Offset: 0x000B3E48
		public static RpcMessage RpcMessage(NetworkReader reader)
		{
			return new RpcMessage
			{
				netId = reader.ReadUInt(),
				componentIndex = reader.ReadByte(),
				functionHash = reader.ReadUShort(),
				payload = reader.ReadBytesAndSizeSegment()
			};
		}

		// Token: 0x060025EE RID: 9710 RVA: 0x000B5C9C File Offset: 0x000B3E9C
		public static void RpcMessage(NetworkWriter writer, RpcMessage value)
		{
			writer.WriteUInt(value.netId);
			writer.WriteByte(value.componentIndex);
			writer.WriteUShort(value.functionHash);
			writer.WriteBytesAndSizeSegment(value.payload);
		}

		// Token: 0x060025EF RID: 9711 RVA: 0x000B5CDC File Offset: 0x000B3EDC
		public static SpawnMessage SpawnMessage(NetworkReader reader)
		{
			return new SpawnMessage
			{
				netId = reader.ReadUInt(),
				isLocalPlayer = reader.ReadBool(),
				isOwner = reader.ReadBool(),
				sceneId = reader.ReadULong(),
				assetId = reader.ReadUInt(),
				position = reader.ReadVector3(),
				rotation = reader.ReadQuaternion(),
				scale = reader.ReadVector3(),
				payload = reader.ReadBytesAndSizeSegment()
			};
		}

		// Token: 0x060025F0 RID: 9712 RVA: 0x000B5D7C File Offset: 0x000B3F7C
		public static void SpawnMessage(NetworkWriter writer, SpawnMessage value)
		{
			writer.WriteUInt(value.netId);
			writer.WriteBool(value.isLocalPlayer);
			writer.WriteBool(value.isOwner);
			writer.WriteULong(value.sceneId);
			writer.WriteUInt(value.assetId);
			writer.WriteVector3(value.position);
			writer.WriteQuaternion(value.rotation);
			writer.WriteVector3(value.scale);
			writer.WriteBytesAndSizeSegment(value.payload);
		}

		// Token: 0x060025F1 RID: 9713 RVA: 0x000B5DF8 File Offset: 0x000B3FF8
		public static ChangeOwnerMessage ChangeOwnerMessage(NetworkReader reader)
		{
			return new ChangeOwnerMessage
			{
				netId = reader.ReadUInt(),
				isOwner = reader.ReadBool(),
				isLocalPlayer = reader.ReadBool()
			};
		}

		// Token: 0x060025F2 RID: 9714 RVA: 0x000B5E40 File Offset: 0x000B4040
		public static void ChangeOwnerMessage(NetworkWriter writer, ChangeOwnerMessage value)
		{
			writer.WriteUInt(value.netId);
			writer.WriteBool(value.isOwner);
			writer.WriteBool(value.isLocalPlayer);
		}

		// Token: 0x060025F3 RID: 9715 RVA: 0x000B5E74 File Offset: 0x000B4074
		public static ObjectSpawnStartedMessage ObjectSpawnStartedMessage(NetworkReader reader)
		{
			return default(ObjectSpawnStartedMessage);
		}

		// Token: 0x060025F4 RID: 9716 RVA: 0x000B5E8C File Offset: 0x000B408C
		public static void ObjectSpawnStartedMessage(NetworkWriter writer, ObjectSpawnStartedMessage value)
		{
		}

		// Token: 0x060025F5 RID: 9717 RVA: 0x000B5E9C File Offset: 0x000B409C
		public static ObjectSpawnFinishedMessage ObjectSpawnFinishedMessage(NetworkReader reader)
		{
			return default(ObjectSpawnFinishedMessage);
		}

		// Token: 0x060025F6 RID: 9718 RVA: 0x000B5EB4 File Offset: 0x000B40B4
		public static void ObjectSpawnFinishedMessage(NetworkWriter writer, ObjectSpawnFinishedMessage value)
		{
		}

		// Token: 0x060025F7 RID: 9719 RVA: 0x000B5EC4 File Offset: 0x000B40C4
		public static ObjectDestroyMessage ObjectDestroyMessage(NetworkReader reader)
		{
			return new ObjectDestroyMessage
			{
				netId = reader.ReadUInt()
			};
		}

		// Token: 0x060025F8 RID: 9720 RVA: 0x000B5EEC File Offset: 0x000B40EC
		public static void ObjectDestroyMessage(NetworkWriter writer, ObjectDestroyMessage value)
		{
			writer.WriteUInt(value.netId);
		}

		// Token: 0x060025F9 RID: 9721 RVA: 0x000B5F08 File Offset: 0x000B4108
		public static ObjectHideMessage ObjectHideMessage(NetworkReader reader)
		{
			return new ObjectHideMessage
			{
				netId = reader.ReadUInt()
			};
		}

		// Token: 0x060025FA RID: 9722 RVA: 0x000B5F30 File Offset: 0x000B4130
		public static void ObjectHideMessage(NetworkWriter writer, ObjectHideMessage value)
		{
			writer.WriteUInt(value.netId);
		}

		// Token: 0x060025FB RID: 9723 RVA: 0x000B5F4C File Offset: 0x000B414C
		public static EntityStateMessage EntityStateMessage(NetworkReader reader)
		{
			return new EntityStateMessage
			{
				netId = reader.ReadUInt(),
				payload = reader.ReadBytesAndSizeSegment()
			};
		}

		// Token: 0x060025FC RID: 9724 RVA: 0x000B5F84 File Offset: 0x000B4184
		public static void EntityStateMessage(NetworkWriter writer, EntityStateMessage value)
		{
			writer.WriteUInt(value.netId);
			writer.WriteBytesAndSizeSegment(value.payload);
		}

		// Token: 0x060025FD RID: 9725 RVA: 0x000B5FAC File Offset: 0x000B41AC
		public static NetworkPingMessage NetworkPingMessage(NetworkReader reader)
		{
			return new NetworkPingMessage
			{
				localTime = reader.ReadDouble()
			};
		}

		// Token: 0x060025FE RID: 9726 RVA: 0x000B5FD4 File Offset: 0x000B41D4
		public static void NetworkPingMessage(NetworkWriter writer, NetworkPingMessage value)
		{
			writer.WriteDouble(value.localTime);
		}

		// Token: 0x060025FF RID: 9727 RVA: 0x000B5FF0 File Offset: 0x000B41F0
		public static NetworkPongMessage NetworkPongMessage(NetworkReader reader)
		{
			return new NetworkPongMessage
			{
				localTime = reader.ReadDouble()
			};
		}

		// Token: 0x06002600 RID: 9728 RVA: 0x000B6018 File Offset: 0x000B4218
		public static void NetworkPongMessage(NetworkWriter writer, NetworkPongMessage value)
		{
			writer.WriteDouble(value.localTime);
		}

		// Token: 0x06002601 RID: 9729 RVA: 0x000B6034 File Offset: 0x000B4234
		public static ConnectionFailureMessage _Read_ConnectionFailureMessage(NetworkReader reader)
		{
			return new ConnectionFailureMessage
			{
				ErrorMessage = reader.ReadString()
			};
		}

		// Token: 0x06002602 RID: 9730 RVA: 0x000B605C File Offset: 0x000B425C
		public static void _Write_ConnectionFailureMessage(NetworkWriter writer, ConnectionFailureMessage value)
		{
			writer.WriteString(value.ErrorMessage);
		}

		// Token: 0x06002603 RID: 9731 RVA: 0x000B6078 File Offset: 0x000B4278
		public static PlayerSpawnNetworkMessage _Read_PlayerSpawnNetworkMessage(NetworkReader reader)
		{
			return new PlayerSpawnNetworkMessage
			{
				PlayerId = reader.ReadInt(),
				AccountId = reader.ReadInt(),
				Position = reader.ReadVector3(),
				PackageType = GeneratedNetworkCode._Read_PackageType(reader),
				AccountUniqueId = reader.ReadString(),
				ConnectionVersion = reader.ReadString()
			};
		}

		// Token: 0x06002604 RID: 9732 RVA: 0x000B60EC File Offset: 0x000B42EC
		public static PackageType _Read_PackageType(NetworkReader reader)
		{
			return (PackageType)reader.ReadInt();
		}

		// Token: 0x06002605 RID: 9733 RVA: 0x000B6100 File Offset: 0x000B4300
		public static void _Write_PlayerSpawnNetworkMessage(NetworkWriter writer, PlayerSpawnNetworkMessage value)
		{
			writer.WriteInt(value.PlayerId);
			writer.WriteInt(value.AccountId);
			writer.WriteVector3(value.Position);
			GeneratedNetworkCode._Write_PackageType(writer, value.PackageType);
			writer.WriteString(value.AccountUniqueId);
			writer.WriteString(value.ConnectionVersion);
		}

		// Token: 0x06002606 RID: 9734 RVA: 0x000B6158 File Offset: 0x000B4358
		public static void _Write_PackageType(NetworkWriter writer, PackageType value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x06002607 RID: 9735 RVA: 0x000B616C File Offset: 0x000B436C
		public static BossEventMessage _Read_BossEventMessage(NetworkReader reader)
		{
			return new BossEventMessage
			{
				BossLevel = reader.ReadInt(),
				BossName = reader.ReadString(),
				BossPosition = reader.ReadVector2()
			};
		}

		// Token: 0x06002608 RID: 9736 RVA: 0x000B61B4 File Offset: 0x000B43B4
		public static void _Write_BossEventMessage(NetworkWriter writer, BossEventMessage value)
		{
			writer.WriteInt(value.BossLevel);
			writer.WriteString(value.BossName);
			writer.WriteVector2(value.BossPosition);
		}

		// Token: 0x06002609 RID: 9737 RVA: 0x000B61E8 File Offset: 0x000B43E8
		public static TvtEventMessage _Read_TvtEventMessage(NetworkReader reader)
		{
			return default(TvtEventMessage);
		}

		// Token: 0x0600260A RID: 9738 RVA: 0x000B6200 File Offset: 0x000B4400
		public static void _Write_TvtEventMessage(NetworkWriter writer, TvtEventMessage value)
		{
		}

		// Token: 0x0600260B RID: 9739 RVA: 0x000B6210 File Offset: 0x000B4410
		public static void _Write_AnimationType(NetworkWriter writer, AnimationType value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x0600260C RID: 9740 RVA: 0x000B6224 File Offset: 0x000B4424
		public static void _Write_AnimationConfig(NetworkWriter writer, AnimationConfig value)
		{
			writer.WriteFloat(value.FrameSpeed);
			writer.WriteInt(value.FreezeFrame);
			writer.WriteFloat(value.FreezeInterval);
			writer.WriteBool(value.ForceDirection);
			GeneratedNetworkCode._Write_Direction(writer, value.Direction);
		}

		// Token: 0x0600260D RID: 9741 RVA: 0x000B6270 File Offset: 0x000B4470
		public static void _Write_Direction(NetworkWriter writer, Direction value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x0600260E RID: 9742 RVA: 0x000B6284 File Offset: 0x000B4484
		public static AnimationType _Read_AnimationType(NetworkReader reader)
		{
			return (AnimationType)reader.ReadInt();
		}

		// Token: 0x0600260F RID: 9743 RVA: 0x000B6298 File Offset: 0x000B4498
		public static AnimationConfig _Read_AnimationConfig(NetworkReader reader)
		{
			return new AnimationConfig
			{
				FrameSpeed = reader.ReadFloat(),
				FreezeFrame = reader.ReadInt(),
				FreezeInterval = reader.ReadFloat(),
				ForceDirection = reader.ReadBool(),
				Direction = GeneratedNetworkCode._Read_Direction(reader)
			};
		}

		// Token: 0x06002610 RID: 9744 RVA: 0x000B62FC File Offset: 0x000B44FC
		public static Direction _Read_Direction(NetworkReader reader)
		{
			return (Direction)reader.ReadInt();
		}

		// Token: 0x06002611 RID: 9745 RVA: 0x000B6310 File Offset: 0x000B4510
		public static global::Attribute _Read_Attribute(NetworkReader reader)
		{
			return new global::Attribute
			{
				Level = reader.ReadInt(),
				BaseLevel = reader.ReadInt(),
				Experience = reader.ReadLong(),
				Type = GeneratedNetworkCode._Read_AttributeType(reader),
				LevelBlessingModifier = reader.ReadInt(),
				LevelCurseModifier = reader.ReadInt(),
				CurseModifier = reader.ReadFloat(),
				BlessingModifier = reader.ReadFloat()
			};
		}

		// Token: 0x06002612 RID: 9746 RVA: 0x000B63A0 File Offset: 0x000B45A0
		public static AttributeType _Read_AttributeType(NetworkReader reader)
		{
			return (AttributeType)reader.ReadInt();
		}

		// Token: 0x06002613 RID: 9747 RVA: 0x000B63B4 File Offset: 0x000B45B4
		public static void _Write_Attribute(NetworkWriter writer, global::Attribute value)
		{
			writer.WriteInt(value.Level);
			writer.WriteInt(value.BaseLevel);
			writer.WriteLong(value.Experience);
			GeneratedNetworkCode._Write_AttributeType(writer, value.Type);
			writer.WriteInt(value.LevelBlessingModifier);
			writer.WriteInt(value.LevelCurseModifier);
			writer.WriteFloat(value.CurseModifier);
			writer.WriteFloat(value.BlessingModifier);
		}

		// Token: 0x06002614 RID: 9748 RVA: 0x000B6424 File Offset: 0x000B4624
		public static void _Write_AttributeType(NetworkWriter writer, AttributeType value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x06002615 RID: 9749 RVA: 0x000B6438 File Offset: 0x000B4638
		public static void _Write_TrainingMode(NetworkWriter writer, TrainingMode value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x06002616 RID: 9750 RVA: 0x000B644C File Offset: 0x000B464C
		public static TrainingMode _Read_TrainingMode(NetworkReader reader)
		{
			return (TrainingMode)reader.ReadInt();
		}

		// Token: 0x06002617 RID: 9751 RVA: 0x000B6460 File Offset: 0x000B4660
		public static void _Write_PlayerProfession(NetworkWriter writer, PlayerProfession value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x06002618 RID: 9752 RVA: 0x000B6474 File Offset: 0x000B4674
		public static void _Write_AccessLevel(NetworkWriter writer, AccessLevel value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x06002619 RID: 9753 RVA: 0x000B6488 File Offset: 0x000B4688
		public static PlayerProfession _Read_PlayerProfession(NetworkReader reader)
		{
			return (PlayerProfession)reader.ReadInt();
		}

		// Token: 0x0600261A RID: 9754 RVA: 0x000B649C File Offset: 0x000B469C
		public static AccessLevel _Read_AccessLevel(NetworkReader reader)
		{
			return (AccessLevel)reader.ReadInt();
		}

		// Token: 0x0600261B RID: 9755 RVA: 0x000B64B0 File Offset: 0x000B46B0
		public static AccountFriend _Read_AccountFriend(NetworkReader reader)
		{
			return new AccountFriend
			{
				IsOnline = reader.ReadBool(),
				FriendName = reader.ReadString(),
				FriendPlayerId = reader.ReadInt(),
				Muted = reader.ReadBool()
			};
		}

		// Token: 0x0600261C RID: 9756 RVA: 0x000B6504 File Offset: 0x000B4704
		public static void _Write_AccountFriend(NetworkWriter writer, AccountFriend value)
		{
			writer.WriteBool(value.IsOnline);
			writer.WriteString(value.FriendName);
			writer.WriteInt(value.FriendPlayerId);
			writer.WriteBool(value.Muted);
		}

		// Token: 0x0600261D RID: 9757 RVA: 0x000B6544 File Offset: 0x000B4744
		public static void _Write_ChatMessage(NetworkWriter writer, ChatMessage value)
		{
			writer.WriteString(value.Color);
			writer.WriteString(value.SenderName);
			writer.WriteString(value.SenderDisplay);
			writer.WriteString(value.Channel);
			writer.WriteString(value.Content);
			writer.WriteDouble(value.Timestamp);
			writer.WriteBool(value.IsBroadcast);
			writer.WriteBool(value.IsChannelFixed);
			writer.WriteBool(value.IsChannelParty);
			writer.WriteBool(value.IsChannelGlobal);
			writer.WriteBool(value.HideFromChatOverlay);
		}

		// Token: 0x0600261E RID: 9758 RVA: 0x000B65D8 File Offset: 0x000B47D8
		public static ChatMessage _Read_ChatMessage(NetworkReader reader)
		{
			return new ChatMessage
			{
				Color = reader.ReadString(),
				SenderName = reader.ReadString(),
				SenderDisplay = reader.ReadString(),
				Channel = reader.ReadString(),
				Content = reader.ReadString(),
				Timestamp = reader.ReadDouble(),
				IsBroadcast = reader.ReadBool(),
				IsChannelFixed = reader.ReadBool(),
				IsChannelParty = reader.ReadBool(),
				IsChannelGlobal = reader.ReadBool(),
				HideFromChatOverlay = reader.ReadBool()
			};
		}

		// Token: 0x0600261F RID: 9759 RVA: 0x000B6698 File Offset: 0x000B4898
		public static void String[](NetworkWriter writer, string[] value)
		{
			writer.WriteArray(value);
		}

		// Token: 0x06002620 RID: 9760 RVA: 0x000B66AC File Offset: 0x000B48AC
		public static string[] String[](NetworkReader reader)
		{
			return reader.ReadArray<string>();
		}

		// Token: 0x06002621 RID: 9761 RVA: 0x000B66C0 File Offset: 0x000B48C0
		public static void _Write_Rank(NetworkWriter writer, Rank value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x06002622 RID: 9762 RVA: 0x000B66D4 File Offset: 0x000B48D4
		public static Rank _Read_Rank(NetworkReader reader)
		{
			return (Rank)reader.ReadInt();
		}

		// Token: 0x06002623 RID: 9763 RVA: 0x000B66E8 File Offset: 0x000B48E8
		public static Condition _Read_Condition(NetworkReader reader)
		{
			return new Condition
			{
				Power = reader.ReadFloat(),
				Interval = reader.ReadFloat(),
				Duration = reader.ReadFloat(),
				LastUseTime = reader.ReadDouble(),
				Type = GeneratedNetworkCode._Read_ConditionType(reader),
				Category = GeneratedNetworkCode._Read_ConditionCategory(reader)
			};
		}

		// Token: 0x06002624 RID: 9764 RVA: 0x000B675C File Offset: 0x000B495C
		public static ConditionType _Read_ConditionType(NetworkReader reader)
		{
			return (ConditionType)reader.ReadInt();
		}

		// Token: 0x06002625 RID: 9765 RVA: 0x000B6770 File Offset: 0x000B4970
		public static ConditionCategory _Read_ConditionCategory(NetworkReader reader)
		{
			return (ConditionCategory)reader.ReadInt();
		}

		// Token: 0x06002626 RID: 9766 RVA: 0x000B6784 File Offset: 0x000B4984
		public static void _Write_Condition(NetworkWriter writer, Condition value)
		{
			writer.WriteFloat(value.Power);
			writer.WriteFloat(value.Interval);
			writer.WriteFloat(value.Duration);
			writer.WriteDouble(value.LastUseTime);
			GeneratedNetworkCode._Write_ConditionType(writer, value.Type);
			GeneratedNetworkCode._Write_ConditionCategory(writer, value.Category);
		}

		// Token: 0x06002627 RID: 9767 RVA: 0x000B67DC File Offset: 0x000B49DC
		public static void _Write_ConditionType(NetworkWriter writer, ConditionType value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x06002628 RID: 9768 RVA: 0x000B67F0 File Offset: 0x000B49F0
		public static void _Write_ConditionCategory(NetworkWriter writer, ConditionCategory value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x06002629 RID: 9769 RVA: 0x000B6804 File Offset: 0x000B4A04
		public static PlayerBlueprint _Read_PlayerBlueprint(NetworkReader reader)
		{
			return new PlayerBlueprint
			{
				PlayerId = reader.ReadInt(),
				CreatedAmount = reader.ReadInt(),
				Blueprint = GeneratedNetworkCode._Read_Blueprint(reader)
			};
		}

		// Token: 0x0600262A RID: 9770 RVA: 0x000B684C File Offset: 0x000B4A4C
		public static Blueprint _Read_Blueprint(NetworkReader reader)
		{
			return new Blueprint
			{
				Id = reader.ReadInt(),
				ItemName = reader.ReadString(),
				RequiredProfession = GeneratedNetworkCode._Read_PlayerProfession(reader),
				RequiredProfessionLevel = reader.ReadInt(),
				Experience = reader.ReadInt(),
				ProducesItem = GeneratedNetworkCode._Read_Item(reader),
				ProducesAmount = reader.ReadInt(),
				Materials = GeneratedNetworkCode._Read_BlueprintMaterial[](reader)
			};
		}

		// Token: 0x0600262B RID: 9771 RVA: 0x000B68DC File Offset: 0x000B4ADC
		public static Item _Read_Item(NetworkReader reader)
		{
			return new Item
			{
				_value = reader.ReadInt(),
				_attack = reader.ReadShort(),
				_defense = reader.ReadShort(),
				_metaName = reader.ReadString(),
				_rarity = GeneratedNetworkCode._Read_Rarity(reader),
				_requiredLevel = reader.ReadInt(),
				Id = reader.ReadInt(),
				SkillId = reader.ReadInt(),
				Name = reader.ReadString(),
				Type = GeneratedNetworkCode._Read_ItemType(reader),
				Soulbind = reader.ReadBool(),
				Sellable = reader.ReadBool(),
				TwoHanded = reader.ReadBool(),
				BoostLevel = reader.ReadInt(),
				Stackable = reader.ReadBool(),
				BlueprintId = reader.ReadInt(),
				OwnerName = reader.ReadString(),
				SlotType = GeneratedNetworkCode._Read_SlotType(reader),
				Description = reader.ReadString(),
				Quality = GeneratedNetworkCode._Read_ItemQuality(reader),
				Category = GeneratedNetworkCode._Read_ItemCategory(reader),
				RequiredVocation = GeneratedNetworkCode._Read_Vocation(reader),
				IgnoreQualityRestrictions = reader.ReadBool(),
				BlueprintRequiredProfessionLevel = reader.ReadInt(),
				BlueprintRequiredProfession = GeneratedNetworkCode._Read_PlayerProfession(reader),
				Amount = reader.ReadInt(),
				SlotPosition = reader.ReadInt(),
				OwnerId = reader.ReadInt()
			};
		}

		// Token: 0x0600262C RID: 9772 RVA: 0x000B6A98 File Offset: 0x000B4C98
		public static Rarity _Read_Rarity(NetworkReader reader)
		{
			return (Rarity)reader.ReadInt();
		}

		// Token: 0x0600262D RID: 9773 RVA: 0x000B6AAC File Offset: 0x000B4CAC
		public static ItemType _Read_ItemType(NetworkReader reader)
		{
			return (ItemType)reader.ReadInt();
		}

		// Token: 0x0600262E RID: 9774 RVA: 0x000B6AC0 File Offset: 0x000B4CC0
		public static SlotType _Read_SlotType(NetworkReader reader)
		{
			return (SlotType)reader.ReadInt();
		}

		// Token: 0x0600262F RID: 9775 RVA: 0x000B6AD4 File Offset: 0x000B4CD4
		public static ItemQuality _Read_ItemQuality(NetworkReader reader)
		{
			return (ItemQuality)reader.ReadInt();
		}

		// Token: 0x06002630 RID: 9776 RVA: 0x000B6AE8 File Offset: 0x000B4CE8
		public static ItemCategory _Read_ItemCategory(NetworkReader reader)
		{
			return (ItemCategory)reader.ReadInt();
		}

		// Token: 0x06002631 RID: 9777 RVA: 0x000B6AFC File Offset: 0x000B4CFC
		public static Vocation _Read_Vocation(NetworkReader reader)
		{
			return (Vocation)reader.ReadInt();
		}

		// Token: 0x06002632 RID: 9778 RVA: 0x000B6B10 File Offset: 0x000B4D10
		public static BlueprintMaterial[] _Read_BlueprintMaterial[](NetworkReader reader)
		{
			return reader.ReadArray<BlueprintMaterial>();
		}

		// Token: 0x06002633 RID: 9779 RVA: 0x000B6B24 File Offset: 0x000B4D24
		public static BlueprintMaterial _Read_BlueprintMaterial(NetworkReader reader)
		{
			return new BlueprintMaterial
			{
				MaterialItem = GeneratedNetworkCode._Read_Item(reader),
				MaterialAmount = reader.ReadInt()
			};
		}

		// Token: 0x06002634 RID: 9780 RVA: 0x000B6B5C File Offset: 0x000B4D5C
		public static void _Write_PlayerBlueprint(NetworkWriter writer, PlayerBlueprint value)
		{
			writer.WriteInt(value.PlayerId);
			writer.WriteInt(value.CreatedAmount);
			GeneratedNetworkCode._Write_Blueprint(writer, value.Blueprint);
		}

		// Token: 0x06002635 RID: 9781 RVA: 0x000B6B90 File Offset: 0x000B4D90
		public static void _Write_Blueprint(NetworkWriter writer, Blueprint value)
		{
			writer.WriteInt(value.Id);
			writer.WriteString(value.ItemName);
			GeneratedNetworkCode._Write_PlayerProfession(writer, value.RequiredProfession);
			writer.WriteInt(value.RequiredProfessionLevel);
			writer.WriteInt(value.Experience);
			GeneratedNetworkCode._Write_Item(writer, value.ProducesItem);
			writer.WriteInt(value.ProducesAmount);
			GeneratedNetworkCode._Write_BlueprintMaterial[](writer, value.Materials);
		}

		// Token: 0x06002636 RID: 9782 RVA: 0x000B6C00 File Offset: 0x000B4E00
		public static void _Write_Item(NetworkWriter writer, Item value)
		{
			writer.WriteInt(value._value);
			writer.WriteShort(value._attack);
			writer.WriteShort(value._defense);
			writer.WriteString(value._metaName);
			GeneratedNetworkCode._Write_Rarity(writer, value._rarity);
			writer.WriteInt(value._requiredLevel);
			writer.WriteInt(value.Id);
			writer.WriteInt(value.SkillId);
			writer.WriteString(value.Name);
			GeneratedNetworkCode._Write_ItemType(writer, value.Type);
			writer.WriteBool(value.Soulbind);
			writer.WriteBool(value.Sellable);
			writer.WriteBool(value.TwoHanded);
			writer.WriteInt(value.BoostLevel);
			writer.WriteBool(value.Stackable);
			writer.WriteInt(value.BlueprintId);
			writer.WriteString(value.OwnerName);
			GeneratedNetworkCode._Write_SlotType(writer, value.SlotType);
			writer.WriteString(value.Description);
			GeneratedNetworkCode._Write_ItemQuality(writer, value.Quality);
			GeneratedNetworkCode._Write_ItemCategory(writer, value.Category);
			GeneratedNetworkCode._Write_Vocation(writer, value.RequiredVocation);
			writer.WriteBool(value.IgnoreQualityRestrictions);
			writer.WriteInt(value.BlueprintRequiredProfessionLevel);
			GeneratedNetworkCode._Write_PlayerProfession(writer, value.BlueprintRequiredProfession);
			writer.WriteInt(value.Amount);
			writer.WriteInt(value.SlotPosition);
			writer.WriteInt(value.OwnerId);
		}

		// Token: 0x06002637 RID: 9783 RVA: 0x000B6D60 File Offset: 0x000B4F60
		public static void _Write_Rarity(NetworkWriter writer, Rarity value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x06002638 RID: 9784 RVA: 0x000B6D74 File Offset: 0x000B4F74
		public static void _Write_ItemType(NetworkWriter writer, ItemType value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x06002639 RID: 9785 RVA: 0x000B6D88 File Offset: 0x000B4F88
		public static void _Write_SlotType(NetworkWriter writer, SlotType value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x0600263A RID: 9786 RVA: 0x000B6D9C File Offset: 0x000B4F9C
		public static void _Write_ItemQuality(NetworkWriter writer, ItemQuality value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x0600263B RID: 9787 RVA: 0x000B6DB0 File Offset: 0x000B4FB0
		public static void _Write_ItemCategory(NetworkWriter writer, ItemCategory value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x0600263C RID: 9788 RVA: 0x000B6DC4 File Offset: 0x000B4FC4
		public static void _Write_Vocation(NetworkWriter writer, Vocation value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x0600263D RID: 9789 RVA: 0x000B6DD8 File Offset: 0x000B4FD8
		public static void _Write_BlueprintMaterial[](NetworkWriter writer, BlueprintMaterial[] value)
		{
			writer.WriteArray(value);
		}

		// Token: 0x0600263E RID: 9790 RVA: 0x000B6DEC File Offset: 0x000B4FEC
		public static void _Write_BlueprintMaterial(NetworkWriter writer, BlueprintMaterial value)
		{
			GeneratedNetworkCode._Write_Item(writer, value.MaterialItem);
			writer.WriteInt(value.MaterialAmount);
		}

		// Token: 0x0600263F RID: 9791 RVA: 0x000B6E14 File Offset: 0x000B5014
		public static void _Write_CreatureGender(NetworkWriter writer, CreatureGender value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x06002640 RID: 9792 RVA: 0x000B6E28 File Offset: 0x000B5028
		public static CreatureGender _Read_CreatureGender(NetworkReader reader)
		{
			return (CreatureGender)reader.ReadInt();
		}

		// Token: 0x06002641 RID: 9793 RVA: 0x000B6E3C File Offset: 0x000B503C
		public static DailyReward _Read_DailyReward(NetworkReader reader)
		{
			return new DailyReward
			{
				Id = reader.ReadInt(),
				Item = GeneratedNetworkCode._Read_Item(reader),
				Amount = reader.ReadInt()
			};
		}

		// Token: 0x06002642 RID: 9794 RVA: 0x000B6E84 File Offset: 0x000B5084
		public static void _Write_DailyReward(NetworkWriter writer, DailyReward value)
		{
			writer.WriteInt(value.Id);
			GeneratedNetworkCode._Write_Item(writer, value.Item);
			writer.WriteInt(value.Amount);
		}

		// Token: 0x06002643 RID: 9795 RVA: 0x000B6EB8 File Offset: 0x000B50B8
		public static void _Write_EffectConfig(NetworkWriter writer, EffectConfig value)
		{
			writer.WriteString(value.EffectName);
			writer.WriteFloat(value.EffectScaleModifier);
			writer.WriteFloat(value.EffectSpeedModifier);
			writer.WriteFloat(value.EffectLoopDuration);
			writer.WriteString(value.SoundEffectName);
			writer.WriteString(value.Text);
			writer.WriteInt(value.TextColorId);
			writer.WriteVector3(value.Position);
		}

		// Token: 0x06002644 RID: 9796 RVA: 0x000B6F28 File Offset: 0x000B5128
		public static EffectConfig _Read_EffectConfig(NetworkReader reader)
		{
			return new EffectConfig
			{
				EffectName = reader.ReadString(),
				EffectScaleModifier = reader.ReadFloat(),
				EffectSpeedModifier = reader.ReadFloat(),
				EffectLoopDuration = reader.ReadFloat(),
				SoundEffectName = reader.ReadString(),
				Text = reader.ReadString(),
				TextColorId = reader.ReadInt(),
				Position = reader.ReadVector3()
			};
		}

		// Token: 0x06002645 RID: 9797 RVA: 0x000B6FB8 File Offset: 0x000B51B8
		public static void _Write_GuildMemberRank(NetworkWriter writer, GuildMemberRank value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x06002646 RID: 9798 RVA: 0x000B6FCC File Offset: 0x000B51CC
		public static GuildMemberRank _Read_GuildMemberRank(NetworkReader reader)
		{
			return (GuildMemberRank)reader.ReadInt();
		}

		// Token: 0x06002647 RID: 9799 RVA: 0x000B6FE0 File Offset: 0x000B51E0
		public static void _Write_Effect(NetworkWriter writer, Effect value)
		{
			writer.WriteString(value.Name);
			writer.WriteFloat(value.ScaleModifier);
			writer.WriteFloat(value.SpeedModifier);
		}

		// Token: 0x06002648 RID: 9800 RVA: 0x000B7014 File Offset: 0x000B5214
		public static Effect _Read_Effect(NetworkReader reader)
		{
			return new Effect
			{
				Name = reader.ReadString(),
				ScaleModifier = reader.ReadFloat(),
				SpeedModifier = reader.ReadFloat()
			};
		}

		// Token: 0x06002649 RID: 9801 RVA: 0x000B705C File Offset: 0x000B525C
		public static Quest _Read_Quest(NetworkReader reader)
		{
			return new Quest
			{
				Id = reader.ReadInt(),
				Name = reader.ReadString(),
				RequiredLevel = reader.ReadInt(),
				Description = reader.ReadString(),
				Rewards = GeneratedNetworkCode._Read_QuestReward[](reader),
				Objectives = GeneratedNetworkCode._Read_QuestObjective[](reader)
			};
		}

		// Token: 0x0600264A RID: 9802 RVA: 0x000B70D0 File Offset: 0x000B52D0
		public static QuestReward[] _Read_QuestReward[](NetworkReader reader)
		{
			return reader.ReadArray<QuestReward>();
		}

		// Token: 0x0600264B RID: 9803 RVA: 0x000B70E4 File Offset: 0x000B52E4
		public static QuestReward _Read_QuestReward(NetworkReader reader)
		{
			return new QuestReward
			{
				Id = reader.ReadInt(),
				QuestId = reader.ReadInt(),
				RewardType = GeneratedNetworkCode._Read_RewardType(reader),
				Vocation = GeneratedNetworkCode._Read_Vocation(reader),
				ItemId = reader.ReadInt(),
				ItemName = reader.ReadString(),
				TitleId = reader.ReadInt(),
				TitleName = reader.ReadString(),
				Amount = reader.ReadInt(),
				RequiredLevel = reader.ReadInt(),
				Rarity = GeneratedNetworkCode._Read_Rarity(reader)
			};
		}

		// Token: 0x0600264C RID: 9804 RVA: 0x000B71A4 File Offset: 0x000B53A4
		public static RewardType _Read_RewardType(NetworkReader reader)
		{
			return (RewardType)reader.ReadInt();
		}

		// Token: 0x0600264D RID: 9805 RVA: 0x000B71B8 File Offset: 0x000B53B8
		public static QuestObjective[] _Read_QuestObjective[](NetworkReader reader)
		{
			return reader.ReadArray<QuestObjective>();
		}

		// Token: 0x0600264E RID: 9806 RVA: 0x000B71CC File Offset: 0x000B53CC
		public static QuestObjective _Read_QuestObjective(NetworkReader reader)
		{
			return new QuestObjective
			{
				Id = reader.ReadInt(),
				QuestId = reader.ReadInt(),
				ObjectiveId = reader.ReadInt(),
				ObjectiveRank = GeneratedNetworkCode._Read_Rank(reader),
				ObjectiveAmount = reader.ReadInt(),
				ObjectiveName = reader.ReadString(),
				ObjectivePluralName = reader.ReadString(),
				ObjectiveType = GeneratedNetworkCode._Read_ObjectiveType(reader)
			};
		}

		// Token: 0x0600264F RID: 9807 RVA: 0x000B725C File Offset: 0x000B545C
		public static ObjectiveType _Read_ObjectiveType(NetworkReader reader)
		{
			return (ObjectiveType)reader.ReadInt();
		}

		// Token: 0x06002650 RID: 9808 RVA: 0x000B7270 File Offset: 0x000B5470
		public static void _Write_Quest(NetworkWriter writer, Quest value)
		{
			writer.WriteInt(value.Id);
			writer.WriteString(value.Name);
			writer.WriteInt(value.RequiredLevel);
			writer.WriteString(value.Description);
			GeneratedNetworkCode._Write_QuestReward[](writer, value.Rewards);
			GeneratedNetworkCode._Write_QuestObjective[](writer, value.Objectives);
		}

		// Token: 0x06002651 RID: 9809 RVA: 0x000B72C8 File Offset: 0x000B54C8
		public static void _Write_QuestReward[](NetworkWriter writer, QuestReward[] value)
		{
			writer.WriteArray(value);
		}

		// Token: 0x06002652 RID: 9810 RVA: 0x000B72DC File Offset: 0x000B54DC
		public static void _Write_QuestReward(NetworkWriter writer, QuestReward value)
		{
			writer.WriteInt(value.Id);
			writer.WriteInt(value.QuestId);
			GeneratedNetworkCode._Write_RewardType(writer, value.RewardType);
			GeneratedNetworkCode._Write_Vocation(writer, value.Vocation);
			writer.WriteInt(value.ItemId);
			writer.WriteString(value.ItemName);
			writer.WriteInt(value.TitleId);
			writer.WriteString(value.TitleName);
			writer.WriteInt(value.Amount);
			writer.WriteInt(value.RequiredLevel);
			GeneratedNetworkCode._Write_Rarity(writer, value.Rarity);
		}

		// Token: 0x06002653 RID: 9811 RVA: 0x000B7370 File Offset: 0x000B5570
		public static void _Write_RewardType(NetworkWriter writer, RewardType value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x06002654 RID: 9812 RVA: 0x000B7384 File Offset: 0x000B5584
		public static void _Write_QuestObjective[](NetworkWriter writer, QuestObjective[] value)
		{
			writer.WriteArray(value);
		}

		// Token: 0x06002655 RID: 9813 RVA: 0x000B7398 File Offset: 0x000B5598
		public static void _Write_QuestObjective(NetworkWriter writer, QuestObjective value)
		{
			writer.WriteInt(value.Id);
			writer.WriteInt(value.QuestId);
			writer.WriteInt(value.ObjectiveId);
			GeneratedNetworkCode._Write_Rank(writer, value.ObjectiveRank);
			writer.WriteInt(value.ObjectiveAmount);
			writer.WriteString(value.ObjectiveName);
			writer.WriteString(value.ObjectivePluralName);
			GeneratedNetworkCode._Write_ObjectiveType(writer, value.ObjectiveType);
		}

		// Token: 0x06002656 RID: 9814 RVA: 0x000B7408 File Offset: 0x000B5608
		public static void _Write_ObjectiveType(NetworkWriter writer, ObjectiveType value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x06002657 RID: 9815 RVA: 0x000B741C File Offset: 0x000B561C
		public static PartyMember _Read_PartyMember(NetworkReader reader)
		{
			return new PartyMember
			{
				Name = reader.ReadString(),
				NetworkInstanceId = reader.ReadUInt(),
				Member = reader.ReadGameObject()
			};
		}

		// Token: 0x06002658 RID: 9816 RVA: 0x000B7464 File Offset: 0x000B5664
		public static void _Write_PartyMember(NetworkWriter writer, PartyMember value)
		{
			writer.WriteString(value.Name);
			writer.WriteUInt(value.NetworkInstanceId);
			writer.WriteGameObject(value.Member);
		}

		// Token: 0x06002659 RID: 9817 RVA: 0x000B7498 File Offset: 0x000B5698
		public static void _Write_MarketOrder(NetworkWriter writer, MarketOrder value)
		{
			writer.WriteInt(value.Id);
			GeneratedNetworkCode._Write_Item(writer, value.Item);
			writer.WriteInt(value.Amount);
			writer.WriteInt(value.PlayerId);
			writer.WriteInt(value.UnitValue);
			writer.WriteString(value.ItemName);
			writer.WriteString(value.SellerName);
		}

		// Token: 0x0600265A RID: 9818 RVA: 0x000B74FC File Offset: 0x000B56FC
		public static MarketOrder _Read_MarketOrder(NetworkReader reader)
		{
			return new MarketOrder
			{
				Id = reader.ReadInt(),
				Item = GeneratedNetworkCode._Read_Item(reader),
				Amount = reader.ReadInt(),
				PlayerId = reader.ReadInt(),
				UnitValue = reader.ReadInt(),
				ItemName = reader.ReadString(),
				SellerName = reader.ReadString()
			};
		}

		// Token: 0x0600265B RID: 9819 RVA: 0x000B7580 File Offset: 0x000B5780
		public static void _Write_NpcDialog(NetworkWriter writer, NpcDialog value)
		{
			writer.WriteString(value.Name);
			writer.WriteString(value.Display);
			GeneratedNetworkCode._Write_NpcChoice[](writer, value.Choices);
			GeneratedNetworkCode._Write_StoreItemConfig[](writer, value.StoreItems);
			GeneratedNetworkCode._Write_Quest(writer, value.Quest);
			GeneratedNetworkCode._Write_StoreAction(writer, value.StoreAction);
		}

		// Token: 0x0600265C RID: 9820 RVA: 0x000B75D8 File Offset: 0x000B57D8
		public static void _Write_NpcChoice[](NetworkWriter writer, NpcChoice[] value)
		{
			writer.WriteArray(value);
		}

		// Token: 0x0600265D RID: 9821 RVA: 0x000B75EC File Offset: 0x000B57EC
		public static void _Write_NpcChoice(NetworkWriter writer, NpcChoice value)
		{
			writer.WriteInt(value.ActionId);
			writer.WriteString(value.Display);
			writer.WriteString(value.Param1);
			writer.WriteString(value.Param2);
			writer.WriteString(value.Param3);
		}

		// Token: 0x0600265E RID: 9822 RVA: 0x000B7638 File Offset: 0x000B5838
		public static void _Write_StoreItemConfig[](NetworkWriter writer, StoreItemConfig[] value)
		{
			writer.WriteArray(value);
		}

		// Token: 0x0600265F RID: 9823 RVA: 0x000B764C File Offset: 0x000B584C
		public static void _Write_StoreItemConfig(NetworkWriter writer, StoreItemConfig value)
		{
			GeneratedNetworkCode._Write_Item(writer, value.Item);
			writer.WriteInt(value.ItemValue);
			writer.WriteInt(value.NpcId);
		}

		// Token: 0x06002660 RID: 9824 RVA: 0x000B7680 File Offset: 0x000B5880
		public static void _Write_StoreAction(NetworkWriter writer, StoreAction value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x06002661 RID: 9825 RVA: 0x000B7694 File Offset: 0x000B5894
		public static NpcDialog _Read_NpcDialog(NetworkReader reader)
		{
			return new NpcDialog
			{
				Name = reader.ReadString(),
				Display = reader.ReadString(),
				Choices = GeneratedNetworkCode._Read_NpcChoice[](reader),
				StoreItems = GeneratedNetworkCode._Read_StoreItemConfig[](reader),
				Quest = GeneratedNetworkCode._Read_Quest(reader),
				StoreAction = GeneratedNetworkCode._Read_StoreAction(reader)
			};
		}

		// Token: 0x06002662 RID: 9826 RVA: 0x000B7708 File Offset: 0x000B5908
		public static NpcChoice[] _Read_NpcChoice[](NetworkReader reader)
		{
			return reader.ReadArray<NpcChoice>();
		}

		// Token: 0x06002663 RID: 9827 RVA: 0x000B771C File Offset: 0x000B591C
		public static NpcChoice _Read_NpcChoice(NetworkReader reader)
		{
			return new NpcChoice
			{
				ActionId = reader.ReadInt(),
				Display = reader.ReadString(),
				Param1 = reader.ReadString(),
				Param2 = reader.ReadString(),
				Param3 = reader.ReadString()
			};
		}

		// Token: 0x06002664 RID: 9828 RVA: 0x000B7780 File Offset: 0x000B5980
		public static StoreItemConfig[] _Read_StoreItemConfig[](NetworkReader reader)
		{
			return reader.ReadArray<StoreItemConfig>();
		}

		// Token: 0x06002665 RID: 9829 RVA: 0x000B7794 File Offset: 0x000B5994
		public static StoreItemConfig _Read_StoreItemConfig(NetworkReader reader)
		{
			return new StoreItemConfig
			{
				Item = GeneratedNetworkCode._Read_Item(reader),
				ItemValue = reader.ReadInt(),
				NpcId = reader.ReadInt()
			};
		}

		// Token: 0x06002666 RID: 9830 RVA: 0x000B77DC File Offset: 0x000B59DC
		public static StoreAction _Read_StoreAction(NetworkReader reader)
		{
			return (StoreAction)reader.ReadInt();
		}

		// Token: 0x06002667 RID: 9831 RVA: 0x000B77F0 File Offset: 0x000B59F0
		public static void _Write_PvpStatus(NetworkWriter writer, PvpStatus value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x06002668 RID: 9832 RVA: 0x000B7804 File Offset: 0x000B5A04
		public static void _Write_TvtTeam(NetworkWriter writer, TvtTeam value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x06002669 RID: 9833 RVA: 0x000B7818 File Offset: 0x000B5A18
		public static PvpStatus _Read_PvpStatus(NetworkReader reader)
		{
			return (PvpStatus)reader.ReadInt();
		}

		// Token: 0x0600266A RID: 9834 RVA: 0x000B782C File Offset: 0x000B5A2C
		public static TvtTeam _Read_TvtTeam(NetworkReader reader)
		{
			return (TvtTeam)reader.ReadInt();
		}

		// Token: 0x0600266B RID: 9835 RVA: 0x000B7840 File Offset: 0x000B5A40
		public static PlayerQuest _Read_PlayerQuest(NetworkReader reader)
		{
			return new PlayerQuest
			{
				Name = reader.ReadString(),
				QuestId = reader.ReadInt(),
				PlayerId = reader.ReadInt(),
				Started = reader.ReadString(),
				Completed = reader.ReadString(),
				RequiredLevel = reader.ReadInt(),
				Description = reader.ReadString(),
				ShowOverlay = reader.ReadBool(),
				IsDailyTask = reader.ReadBool(),
				Rewards = GeneratedNetworkCode._Read_PlayerQuestReward[](reader),
				Objectives = GeneratedNetworkCode._Read_PlayerQuestObjective[](reader)
			};
		}

		// Token: 0x0600266C RID: 9836 RVA: 0x000B7900 File Offset: 0x000B5B00
		public static PlayerQuestReward[] _Read_PlayerQuestReward[](NetworkReader reader)
		{
			return reader.ReadArray<PlayerQuestReward>();
		}

		// Token: 0x0600266D RID: 9837 RVA: 0x000B7914 File Offset: 0x000B5B14
		public static PlayerQuestReward _Read_PlayerQuestReward(NetworkReader reader)
		{
			return new PlayerQuestReward
			{
				Id = reader.ReadInt(),
				PlayerQuestId = reader.ReadInt(),
				RewardType = GeneratedNetworkCode._Read_RewardType(reader),
				Vocation = GeneratedNetworkCode._Read_Vocation(reader),
				ItemId = reader.ReadInt(),
				ItemName = reader.ReadString(),
				TitleId = reader.ReadInt(),
				TitleName = reader.ReadString(),
				Amount = reader.ReadInt(),
				RequiredLevel = reader.ReadInt(),
				Rarity = GeneratedNetworkCode._Read_Rarity(reader)
			};
		}

		// Token: 0x0600266E RID: 9838 RVA: 0x000B79D4 File Offset: 0x000B5BD4
		public static PlayerQuestObjective[] _Read_PlayerQuestObjective[](NetworkReader reader)
		{
			return reader.ReadArray<PlayerQuestObjective>();
		}

		// Token: 0x0600266F RID: 9839 RVA: 0x000B79E8 File Offset: 0x000B5BE8
		public static PlayerQuestObjective _Read_PlayerQuestObjective(NetworkReader reader)
		{
			return new PlayerQuestObjective
			{
				Id = reader.ReadInt(),
				ObjectiveId = reader.ReadInt(),
				PlayerQuestId = reader.ReadInt(),
				ObjectiveRank = GeneratedNetworkCode._Read_Rank(reader),
				ObjectiveAmount = reader.ReadInt(),
				ObjectiveName = reader.ReadString(),
				ObjectivePluralName = reader.ReadString(),
				ObjectiveProgress = reader.ReadInt(),
				ObjectiveType = GeneratedNetworkCode._Read_ObjectiveType(reader)
			};
		}

		// Token: 0x06002670 RID: 9840 RVA: 0x000B7A88 File Offset: 0x000B5C88
		public static void _Write_PlayerQuest(NetworkWriter writer, PlayerQuest value)
		{
			writer.WriteString(value.Name);
			writer.WriteInt(value.QuestId);
			writer.WriteInt(value.PlayerId);
			writer.WriteString(value.Started);
			writer.WriteString(value.Completed);
			writer.WriteInt(value.RequiredLevel);
			writer.WriteString(value.Description);
			writer.WriteBool(value.ShowOverlay);
			writer.WriteBool(value.IsDailyTask);
			GeneratedNetworkCode._Write_PlayerQuestReward[](writer, value.Rewards);
			GeneratedNetworkCode._Write_PlayerQuestObjective[](writer, value.Objectives);
		}

		// Token: 0x06002671 RID: 9841 RVA: 0x000B7B1C File Offset: 0x000B5D1C
		public static void _Write_PlayerQuestReward[](NetworkWriter writer, PlayerQuestReward[] value)
		{
			writer.WriteArray(value);
		}

		// Token: 0x06002672 RID: 9842 RVA: 0x000B7B30 File Offset: 0x000B5D30
		public static void _Write_PlayerQuestReward(NetworkWriter writer, PlayerQuestReward value)
		{
			writer.WriteInt(value.Id);
			writer.WriteInt(value.PlayerQuestId);
			GeneratedNetworkCode._Write_RewardType(writer, value.RewardType);
			GeneratedNetworkCode._Write_Vocation(writer, value.Vocation);
			writer.WriteInt(value.ItemId);
			writer.WriteString(value.ItemName);
			writer.WriteInt(value.TitleId);
			writer.WriteString(value.TitleName);
			writer.WriteInt(value.Amount);
			writer.WriteInt(value.RequiredLevel);
			GeneratedNetworkCode._Write_Rarity(writer, value.Rarity);
		}

		// Token: 0x06002673 RID: 9843 RVA: 0x000B7BC4 File Offset: 0x000B5DC4
		public static void _Write_PlayerQuestObjective[](NetworkWriter writer, PlayerQuestObjective[] value)
		{
			writer.WriteArray(value);
		}

		// Token: 0x06002674 RID: 9844 RVA: 0x000B7BD8 File Offset: 0x000B5DD8
		public static void _Write_PlayerQuestObjective(NetworkWriter writer, PlayerQuestObjective value)
		{
			writer.WriteInt(value.Id);
			writer.WriteInt(value.ObjectiveId);
			writer.WriteInt(value.PlayerQuestId);
			GeneratedNetworkCode._Write_Rank(writer, value.ObjectiveRank);
			writer.WriteInt(value.ObjectiveAmount);
			writer.WriteString(value.ObjectiveName);
			writer.WriteString(value.ObjectivePluralName);
			writer.WriteInt(value.ObjectiveProgress);
			GeneratedNetworkCode._Write_ObjectiveType(writer, value.ObjectiveType);
		}

		// Token: 0x06002675 RID: 9845 RVA: 0x000B7C54 File Offset: 0x000B5E54
		public static Skill _Read_Skill(NetworkReader reader)
		{
			return new Skill
			{
				Id = reader.ReadInt(),
				Name = reader.ReadString(),
				Range = reader.ReadFloat(),
				Learned = reader.ReadBool(),
				_cooldown = reader.ReadFloat(),
				SkillPower = reader.ReadFloat(),
				Type = GeneratedNetworkCode._Read_SkillType(reader),
				CastAmount = reader.ReadInt(),
				MaxTargets = reader.ReadInt(),
				NeedTarget = reader.ReadBool(),
				EnchantLevel = reader.ReadInt(),
				RequiredLevel = reader.ReadInt(),
				Description = reader.ReadString(),
				IsDefaultSkill = reader.ReadBool(),
				WeaponType = GeneratedNetworkCode._Read_ItemType(reader),
				Category = GeneratedNetworkCode._Read_SkillCategory(reader),
				SlotPosition = reader.ReadInt(),
				SkillBarId = reader.ReadInt(),
				LastUseTime = reader.ReadDouble()
			};
		}

		// Token: 0x06002676 RID: 9846 RVA: 0x000B7D8C File Offset: 0x000B5F8C
		public static SkillType _Read_SkillType(NetworkReader reader)
		{
			return (SkillType)reader.ReadInt();
		}

		// Token: 0x06002677 RID: 9847 RVA: 0x000B7DA0 File Offset: 0x000B5FA0
		public static SkillCategory _Read_SkillCategory(NetworkReader reader)
		{
			return (SkillCategory)reader.ReadInt();
		}

		// Token: 0x06002678 RID: 9848 RVA: 0x000B7DB4 File Offset: 0x000B5FB4
		public static void _Write_Skill(NetworkWriter writer, Skill value)
		{
			writer.WriteInt(value.Id);
			writer.WriteString(value.Name);
			writer.WriteFloat(value.Range);
			writer.WriteBool(value.Learned);
			writer.WriteFloat(value._cooldown);
			writer.WriteFloat(value.SkillPower);
			GeneratedNetworkCode._Write_SkillType(writer, value.Type);
			writer.WriteInt(value.CastAmount);
			writer.WriteInt(value.MaxTargets);
			writer.WriteBool(value.NeedTarget);
			writer.WriteInt(value.EnchantLevel);
			writer.WriteInt(value.RequiredLevel);
			writer.WriteString(value.Description);
			writer.WriteBool(value.IsDefaultSkill);
			GeneratedNetworkCode._Write_ItemType(writer, value.WeaponType);
			GeneratedNetworkCode._Write_SkillCategory(writer, value.Category);
			writer.WriteInt(value.SlotPosition);
			writer.WriteInt(value.SkillBarId);
			writer.WriteDouble(value.LastUseTime);
		}

		// Token: 0x06002679 RID: 9849 RVA: 0x000B7EA8 File Offset: 0x000B60A8
		public static void _Write_SkillType(NetworkWriter writer, SkillType value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x0600267A RID: 9850 RVA: 0x000B7EBC File Offset: 0x000B60BC
		public static void _Write_SkillCategory(NetworkWriter writer, SkillCategory value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x0600267B RID: 9851 RVA: 0x000B7ED0 File Offset: 0x000B60D0
		public static TalentIdentifier _Read_TalentIdentifier(NetworkReader reader)
		{
			return (TalentIdentifier)reader.ReadInt();
		}

		// Token: 0x0600267C RID: 9852 RVA: 0x000B7EE4 File Offset: 0x000B60E4
		public static void _Write_TalentIdentifier(NetworkWriter writer, TalentIdentifier value)
		{
			writer.WriteInt((int)value);
		}

		// Token: 0x0600267D RID: 9853 RVA: 0x000B7EF8 File Offset: 0x000B60F8
		public static Title _Read_Title(NetworkReader reader)
		{
			return new Title
			{
				Id = reader.ReadInt(),
				Name = reader.ReadString(),
				Description = reader.ReadString()
			};
		}

		// Token: 0x0600267E RID: 9854 RVA: 0x000B7F40 File Offset: 0x000B6140
		public static void _Write_Title(NetworkWriter writer, Title value)
		{
			writer.WriteInt(value.Id);
			writer.WriteString(value.Name);
			writer.WriteString(value.Description);
		}

		// Token: 0x0600267F RID: 9855 RVA: 0x000B7F74 File Offset: 0x000B6174
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void InitReadWriters()
		{
			Writer<byte>.write = new Action<NetworkWriter, byte>(NetworkWriterExtensions.WriteByte);
			Writer<byte?>.write = new Action<NetworkWriter, byte?>(NetworkWriterExtensions.WriteByteNullable);
			Writer<sbyte>.write = new Action<NetworkWriter, sbyte>(NetworkWriterExtensions.WriteSByte);
			Writer<sbyte?>.write = new Action<NetworkWriter, sbyte?>(NetworkWriterExtensions.WriteSByteNullable);
			Writer<char>.write = new Action<NetworkWriter, char>(NetworkWriterExtensions.WriteChar);
			Writer<char?>.write = new Action<NetworkWriter, char?>(NetworkWriterExtensions.WriteCharNullable);
			Writer<bool>.write = new Action<NetworkWriter, bool>(NetworkWriterExtensions.WriteBool);
			Writer<bool?>.write = new Action<NetworkWriter, bool?>(NetworkWriterExtensions.WriteBoolNullable);
			Writer<short>.write = new Action<NetworkWriter, short>(NetworkWriterExtensions.WriteShort);
			Writer<short?>.write = new Action<NetworkWriter, short?>(NetworkWriterExtensions.WriteShortNullable);
			Writer<ushort>.write = new Action<NetworkWriter, ushort>(NetworkWriterExtensions.WriteUShort);
			Writer<ushort?>.write = new Action<NetworkWriter, ushort?>(NetworkWriterExtensions.WriteUShortNullable);
			Writer<int>.write = new Action<NetworkWriter, int>(NetworkWriterExtensions.WriteInt);
			Writer<int?>.write = new Action<NetworkWriter, int?>(NetworkWriterExtensions.WriteIntNullable);
			Writer<uint>.write = new Action<NetworkWriter, uint>(NetworkWriterExtensions.WriteUInt);
			Writer<uint?>.write = new Action<NetworkWriter, uint?>(NetworkWriterExtensions.WriteUIntNullable);
			Writer<long>.write = new Action<NetworkWriter, long>(NetworkWriterExtensions.WriteLong);
			Writer<long?>.write = new Action<NetworkWriter, long?>(NetworkWriterExtensions.WriteLongNullable);
			Writer<ulong>.write = new Action<NetworkWriter, ulong>(NetworkWriterExtensions.WriteULong);
			Writer<ulong?>.write = new Action<NetworkWriter, ulong?>(NetworkWriterExtensions.WriteULongNullable);
			Writer<float>.write = new Action<NetworkWriter, float>(NetworkWriterExtensions.WriteFloat);
			Writer<float?>.write = new Action<NetworkWriter, float?>(NetworkWriterExtensions.WriteFloatNullable);
			Writer<double>.write = new Action<NetworkWriter, double>(NetworkWriterExtensions.WriteDouble);
			Writer<double?>.write = new Action<NetworkWriter, double?>(NetworkWriterExtensions.WriteDoubleNullable);
			Writer<decimal>.write = new Action<NetworkWriter, decimal>(NetworkWriterExtensions.WriteDecimal);
			Writer<decimal?>.write = new Action<NetworkWriter, decimal?>(NetworkWriterExtensions.WriteDecimalNullable);
			Writer<string>.write = new Action<NetworkWriter, string>(NetworkWriterExtensions.WriteString);
			Writer<ArraySegment<byte>>.write = new Action<NetworkWriter, ArraySegment<byte>>(NetworkWriterExtensions.WriteBytesAndSizeSegment);
			Writer<byte[]>.write = new Action<NetworkWriter, byte[]>(NetworkWriterExtensions.WriteBytesAndSize);
			Writer<Vector2>.write = new Action<NetworkWriter, Vector2>(NetworkWriterExtensions.WriteVector2);
			Writer<Vector2?>.write = new Action<NetworkWriter, Vector2?>(NetworkWriterExtensions.WriteVector2Nullable);
			Writer<Vector3>.write = new Action<NetworkWriter, Vector3>(NetworkWriterExtensions.WriteVector3);
			Writer<Vector3?>.write = new Action<NetworkWriter, Vector3?>(NetworkWriterExtensions.WriteVector3Nullable);
			Writer<Vector4>.write = new Action<NetworkWriter, Vector4>(NetworkWriterExtensions.WriteVector4);
			Writer<Vector4?>.write = new Action<NetworkWriter, Vector4?>(NetworkWriterExtensions.WriteVector4Nullable);
			Writer<Vector2Int>.write = new Action<NetworkWriter, Vector2Int>(NetworkWriterExtensions.WriteVector2Int);
			Writer<Vector2Int?>.write = new Action<NetworkWriter, Vector2Int?>(NetworkWriterExtensions.WriteVector2IntNullable);
			Writer<Vector3Int>.write = new Action<NetworkWriter, Vector3Int>(NetworkWriterExtensions.WriteVector3Int);
			Writer<Vector3Int?>.write = new Action<NetworkWriter, Vector3Int?>(NetworkWriterExtensions.WriteVector3IntNullable);
			Writer<Color>.write = new Action<NetworkWriter, Color>(NetworkWriterExtensions.WriteColor);
			Writer<Color?>.write = new Action<NetworkWriter, Color?>(NetworkWriterExtensions.WriteColorNullable);
			Writer<Color32>.write = new Action<NetworkWriter, Color32>(NetworkWriterExtensions.WriteColor32);
			Writer<Color32?>.write = new Action<NetworkWriter, Color32?>(NetworkWriterExtensions.WriteColor32Nullable);
			Writer<Quaternion>.write = new Action<NetworkWriter, Quaternion>(NetworkWriterExtensions.WriteQuaternion);
			Writer<Quaternion?>.write = new Action<NetworkWriter, Quaternion?>(NetworkWriterExtensions.WriteQuaternionNullable);
			Writer<Rect>.write = new Action<NetworkWriter, Rect>(NetworkWriterExtensions.WriteRect);
			Writer<Rect?>.write = new Action<NetworkWriter, Rect?>(NetworkWriterExtensions.WriteRectNullable);
			Writer<Plane>.write = new Action<NetworkWriter, Plane>(NetworkWriterExtensions.WritePlane);
			Writer<Plane?>.write = new Action<NetworkWriter, Plane?>(NetworkWriterExtensions.WritePlaneNullable);
			Writer<Ray>.write = new Action<NetworkWriter, Ray>(NetworkWriterExtensions.WriteRay);
			Writer<Ray?>.write = new Action<NetworkWriter, Ray?>(NetworkWriterExtensions.WriteRayNullable);
			Writer<Matrix4x4>.write = new Action<NetworkWriter, Matrix4x4>(NetworkWriterExtensions.WriteMatrix4x4);
			Writer<Matrix4x4?>.write = new Action<NetworkWriter, Matrix4x4?>(NetworkWriterExtensions.WriteMatrix4x4Nullable);
			Writer<Guid>.write = new Action<NetworkWriter, Guid>(NetworkWriterExtensions.WriteGuid);
			Writer<Guid?>.write = new Action<NetworkWriter, Guid?>(NetworkWriterExtensions.WriteGuidNullable);
			Writer<NetworkIdentity>.write = new Action<NetworkWriter, NetworkIdentity>(NetworkWriterExtensions.WriteNetworkIdentity);
			Writer<NetworkBehaviour>.write = new Action<NetworkWriter, NetworkBehaviour>(NetworkWriterExtensions.WriteNetworkBehaviour);
			Writer<Transform>.write = new Action<NetworkWriter, Transform>(NetworkWriterExtensions.WriteTransform);
			Writer<GameObject>.write = new Action<NetworkWriter, GameObject>(NetworkWriterExtensions.WriteGameObject);
			Writer<Uri>.write = new Action<NetworkWriter, Uri>(NetworkWriterExtensions.WriteUri);
			Writer<Texture2D>.write = new Action<NetworkWriter, Texture2D>(NetworkWriterExtensions.WriteTexture2D);
			Writer<Sprite>.write = new Action<NetworkWriter, Sprite>(NetworkWriterExtensions.WriteSprite);
			Writer<DateTime>.write = new Action<NetworkWriter, DateTime>(NetworkWriterExtensions.WriteDateTime);
			Writer<DateTime?>.write = new Action<NetworkWriter, DateTime?>(NetworkWriterExtensions.WriteDateTimeNullable);
			Writer<TimeSnapshotMessage>.write = new Action<NetworkWriter, TimeSnapshotMessage>(GeneratedNetworkCode._Write_Mirror.TimeSnapshotMessage);
			Writer<ReadyMessage>.write = new Action<NetworkWriter, ReadyMessage>(GeneratedNetworkCode._Write_Mirror.ReadyMessage);
			Writer<NotReadyMessage>.write = new Action<NetworkWriter, NotReadyMessage>(GeneratedNetworkCode._Write_Mirror.NotReadyMessage);
			Writer<AddPlayerMessage>.write = new Action<NetworkWriter, AddPlayerMessage>(GeneratedNetworkCode._Write_Mirror.AddPlayerMessage);
			Writer<SceneMessage>.write = new Action<NetworkWriter, SceneMessage>(GeneratedNetworkCode._Write_Mirror.SceneMessage);
			Writer<SceneOperation>.write = new Action<NetworkWriter, SceneOperation>(GeneratedNetworkCode._Write_Mirror.SceneOperation);
			Writer<CommandMessage>.write = new Action<NetworkWriter, CommandMessage>(GeneratedNetworkCode._Write_Mirror.CommandMessage);
			Writer<RpcMessage>.write = new Action<NetworkWriter, RpcMessage>(GeneratedNetworkCode._Write_Mirror.RpcMessage);
			Writer<SpawnMessage>.write = new Action<NetworkWriter, SpawnMessage>(GeneratedNetworkCode._Write_Mirror.SpawnMessage);
			Writer<ChangeOwnerMessage>.write = new Action<NetworkWriter, ChangeOwnerMessage>(GeneratedNetworkCode._Write_Mirror.ChangeOwnerMessage);
			Writer<ObjectSpawnStartedMessage>.write = new Action<NetworkWriter, ObjectSpawnStartedMessage>(GeneratedNetworkCode._Write_Mirror.ObjectSpawnStartedMessage);
			Writer<ObjectSpawnFinishedMessage>.write = new Action<NetworkWriter, ObjectSpawnFinishedMessage>(GeneratedNetworkCode._Write_Mirror.ObjectSpawnFinishedMessage);
			Writer<ObjectDestroyMessage>.write = new Action<NetworkWriter, ObjectDestroyMessage>(GeneratedNetworkCode._Write_Mirror.ObjectDestroyMessage);
			Writer<ObjectHideMessage>.write = new Action<NetworkWriter, ObjectHideMessage>(GeneratedNetworkCode._Write_Mirror.ObjectHideMessage);
			Writer<EntityStateMessage>.write = new Action<NetworkWriter, EntityStateMessage>(GeneratedNetworkCode._Write_Mirror.EntityStateMessage);
			Writer<NetworkPingMessage>.write = new Action<NetworkWriter, NetworkPingMessage>(GeneratedNetworkCode._Write_Mirror.NetworkPingMessage);
			Writer<NetworkPongMessage>.write = new Action<NetworkWriter, NetworkPongMessage>(GeneratedNetworkCode._Write_Mirror.NetworkPongMessage);
			Writer<ConnectionFailureMessage>.write = new Action<NetworkWriter, ConnectionFailureMessage>(GeneratedNetworkCode._Write_ConnectionFailureMessage);
			Writer<PlayerSpawnNetworkMessage>.write = new Action<NetworkWriter, PlayerSpawnNetworkMessage>(GeneratedNetworkCode._Write_PlayerSpawnNetworkMessage);
			Writer<PackageType>.write = new Action<NetworkWriter, PackageType>(GeneratedNetworkCode._Write_PackageType);
			Writer<BossEventMessage>.write = new Action<NetworkWriter, BossEventMessage>(GeneratedNetworkCode._Write_BossEventMessage);
			Writer<TvtEventMessage>.write = new Action<NetworkWriter, TvtEventMessage>(GeneratedNetworkCode._Write_TvtEventMessage);
			Writer<AnimationType>.write = new Action<NetworkWriter, AnimationType>(GeneratedNetworkCode._Write_AnimationType);
			Writer<AnimationConfig>.write = new Action<NetworkWriter, AnimationConfig>(GeneratedNetworkCode._Write_AnimationConfig);
			Writer<Direction>.write = new Action<NetworkWriter, Direction>(GeneratedNetworkCode._Write_Direction);
			Writer<global::Attribute>.write = new Action<NetworkWriter, global::Attribute>(GeneratedNetworkCode._Write_Attribute);
			Writer<AttributeType>.write = new Action<NetworkWriter, AttributeType>(GeneratedNetworkCode._Write_AttributeType);
			Writer<TrainingMode>.write = new Action<NetworkWriter, TrainingMode>(GeneratedNetworkCode._Write_TrainingMode);
			Writer<PlayerProfession>.write = new Action<NetworkWriter, PlayerProfession>(GeneratedNetworkCode._Write_PlayerProfession);
			Writer<AccessLevel>.write = new Action<NetworkWriter, AccessLevel>(GeneratedNetworkCode._Write_AccessLevel);
			Writer<AccountFriend>.write = new Action<NetworkWriter, AccountFriend>(GeneratedNetworkCode._Write_AccountFriend);
			Writer<ChatMessage>.write = new Action<NetworkWriter, ChatMessage>(GeneratedNetworkCode._Write_ChatMessage);
			Writer<string[]>.write = new Action<NetworkWriter, string[]>(GeneratedNetworkCode._Write_System.String[]);
			Writer<Rank>.write = new Action<NetworkWriter, Rank>(GeneratedNetworkCode._Write_Rank);
			Writer<Condition>.write = new Action<NetworkWriter, Condition>(GeneratedNetworkCode._Write_Condition);
			Writer<ConditionType>.write = new Action<NetworkWriter, ConditionType>(GeneratedNetworkCode._Write_ConditionType);
			Writer<ConditionCategory>.write = new Action<NetworkWriter, ConditionCategory>(GeneratedNetworkCode._Write_ConditionCategory);
			Writer<PlayerBlueprint>.write = new Action<NetworkWriter, PlayerBlueprint>(GeneratedNetworkCode._Write_PlayerBlueprint);
			Writer<Blueprint>.write = new Action<NetworkWriter, Blueprint>(GeneratedNetworkCode._Write_Blueprint);
			Writer<Item>.write = new Action<NetworkWriter, Item>(GeneratedNetworkCode._Write_Item);
			Writer<Rarity>.write = new Action<NetworkWriter, Rarity>(GeneratedNetworkCode._Write_Rarity);
			Writer<ItemType>.write = new Action<NetworkWriter, ItemType>(GeneratedNetworkCode._Write_ItemType);
			Writer<SlotType>.write = new Action<NetworkWriter, SlotType>(GeneratedNetworkCode._Write_SlotType);
			Writer<ItemQuality>.write = new Action<NetworkWriter, ItemQuality>(GeneratedNetworkCode._Write_ItemQuality);
			Writer<ItemCategory>.write = new Action<NetworkWriter, ItemCategory>(GeneratedNetworkCode._Write_ItemCategory);
			Writer<Vocation>.write = new Action<NetworkWriter, Vocation>(GeneratedNetworkCode._Write_Vocation);
			Writer<BlueprintMaterial[]>.write = new Action<NetworkWriter, BlueprintMaterial[]>(GeneratedNetworkCode._Write_BlueprintMaterial[]);
			Writer<BlueprintMaterial>.write = new Action<NetworkWriter, BlueprintMaterial>(GeneratedNetworkCode._Write_BlueprintMaterial);
			Writer<CreatureGender>.write = new Action<NetworkWriter, CreatureGender>(GeneratedNetworkCode._Write_CreatureGender);
			Writer<DailyReward>.write = new Action<NetworkWriter, DailyReward>(GeneratedNetworkCode._Write_DailyReward);
			Writer<EffectConfig>.write = new Action<NetworkWriter, EffectConfig>(GeneratedNetworkCode._Write_EffectConfig);
			Writer<GuildMemberRank>.write = new Action<NetworkWriter, GuildMemberRank>(GeneratedNetworkCode._Write_GuildMemberRank);
			Writer<Effect>.write = new Action<NetworkWriter, Effect>(GeneratedNetworkCode._Write_Effect);
			Writer<Quest>.write = new Action<NetworkWriter, Quest>(GeneratedNetworkCode._Write_Quest);
			Writer<QuestReward[]>.write = new Action<NetworkWriter, QuestReward[]>(GeneratedNetworkCode._Write_QuestReward[]);
			Writer<QuestReward>.write = new Action<NetworkWriter, QuestReward>(GeneratedNetworkCode._Write_QuestReward);
			Writer<RewardType>.write = new Action<NetworkWriter, RewardType>(GeneratedNetworkCode._Write_RewardType);
			Writer<QuestObjective[]>.write = new Action<NetworkWriter, QuestObjective[]>(GeneratedNetworkCode._Write_QuestObjective[]);
			Writer<QuestObjective>.write = new Action<NetworkWriter, QuestObjective>(GeneratedNetworkCode._Write_QuestObjective);
			Writer<ObjectiveType>.write = new Action<NetworkWriter, ObjectiveType>(GeneratedNetworkCode._Write_ObjectiveType);
			Writer<PartyMember>.write = new Action<NetworkWriter, PartyMember>(GeneratedNetworkCode._Write_PartyMember);
			Writer<MarketOrder>.write = new Action<NetworkWriter, MarketOrder>(GeneratedNetworkCode._Write_MarketOrder);
			Writer<NpcDialog>.write = new Action<NetworkWriter, NpcDialog>(GeneratedNetworkCode._Write_NpcDialog);
			Writer<NpcChoice[]>.write = new Action<NetworkWriter, NpcChoice[]>(GeneratedNetworkCode._Write_NpcChoice[]);
			Writer<NpcChoice>.write = new Action<NetworkWriter, NpcChoice>(GeneratedNetworkCode._Write_NpcChoice);
			Writer<StoreItemConfig[]>.write = new Action<NetworkWriter, StoreItemConfig[]>(GeneratedNetworkCode._Write_StoreItemConfig[]);
			Writer<StoreItemConfig>.write = new Action<NetworkWriter, StoreItemConfig>(GeneratedNetworkCode._Write_StoreItemConfig);
			Writer<StoreAction>.write = new Action<NetworkWriter, StoreAction>(GeneratedNetworkCode._Write_StoreAction);
			Writer<PvpStatus>.write = new Action<NetworkWriter, PvpStatus>(GeneratedNetworkCode._Write_PvpStatus);
			Writer<TvtTeam>.write = new Action<NetworkWriter, TvtTeam>(GeneratedNetworkCode._Write_TvtTeam);
			Writer<PlayerQuest>.write = new Action<NetworkWriter, PlayerQuest>(GeneratedNetworkCode._Write_PlayerQuest);
			Writer<PlayerQuestReward[]>.write = new Action<NetworkWriter, PlayerQuestReward[]>(GeneratedNetworkCode._Write_PlayerQuestReward[]);
			Writer<PlayerQuestReward>.write = new Action<NetworkWriter, PlayerQuestReward>(GeneratedNetworkCode._Write_PlayerQuestReward);
			Writer<PlayerQuestObjective[]>.write = new Action<NetworkWriter, PlayerQuestObjective[]>(GeneratedNetworkCode._Write_PlayerQuestObjective[]);
			Writer<PlayerQuestObjective>.write = new Action<NetworkWriter, PlayerQuestObjective>(GeneratedNetworkCode._Write_PlayerQuestObjective);
			Writer<Skill>.write = new Action<NetworkWriter, Skill>(GeneratedNetworkCode._Write_Skill);
			Writer<SkillType>.write = new Action<NetworkWriter, SkillType>(GeneratedNetworkCode._Write_SkillType);
			Writer<SkillCategory>.write = new Action<NetworkWriter, SkillCategory>(GeneratedNetworkCode._Write_SkillCategory);
			Writer<TalentIdentifier>.write = new Action<NetworkWriter, TalentIdentifier>(GeneratedNetworkCode._Write_TalentIdentifier);
			Writer<Title>.write = new Action<NetworkWriter, Title>(GeneratedNetworkCode._Write_Title);
			Reader<byte>.read = new Func<NetworkReader, byte>(NetworkReaderExtensions.ReadByte);
			Reader<byte?>.read = new Func<NetworkReader, byte?>(NetworkReaderExtensions.ReadByteNullable);
			Reader<sbyte>.read = new Func<NetworkReader, sbyte>(NetworkReaderExtensions.ReadSByte);
			Reader<sbyte?>.read = new Func<NetworkReader, sbyte?>(NetworkReaderExtensions.ReadSByteNullable);
			Reader<char>.read = new Func<NetworkReader, char>(NetworkReaderExtensions.ReadChar);
			Reader<char?>.read = new Func<NetworkReader, char?>(NetworkReaderExtensions.ReadCharNullable);
			Reader<bool>.read = new Func<NetworkReader, bool>(NetworkReaderExtensions.ReadBool);
			Reader<bool?>.read = new Func<NetworkReader, bool?>(NetworkReaderExtensions.ReadBoolNullable);
			Reader<short>.read = new Func<NetworkReader, short>(NetworkReaderExtensions.ReadShort);
			Reader<short?>.read = new Func<NetworkReader, short?>(NetworkReaderExtensions.ReadShortNullable);
			Reader<ushort>.read = new Func<NetworkReader, ushort>(NetworkReaderExtensions.ReadUShort);
			Reader<ushort?>.read = new Func<NetworkReader, ushort?>(NetworkReaderExtensions.ReadUShortNullable);
			Reader<int>.read = new Func<NetworkReader, int>(NetworkReaderExtensions.ReadInt);
			Reader<int?>.read = new Func<NetworkReader, int?>(NetworkReaderExtensions.ReadIntNullable);
			Reader<uint>.read = new Func<NetworkReader, uint>(NetworkReaderExtensions.ReadUInt);
			Reader<uint?>.read = new Func<NetworkReader, uint?>(NetworkReaderExtensions.ReadUIntNullable);
			Reader<long>.read = new Func<NetworkReader, long>(NetworkReaderExtensions.ReadLong);
			Reader<long?>.read = new Func<NetworkReader, long?>(NetworkReaderExtensions.ReadLongNullable);
			Reader<ulong>.read = new Func<NetworkReader, ulong>(NetworkReaderExtensions.ReadULong);
			Reader<ulong?>.read = new Func<NetworkReader, ulong?>(NetworkReaderExtensions.ReadULongNullable);
			Reader<float>.read = new Func<NetworkReader, float>(NetworkReaderExtensions.ReadFloat);
			Reader<float?>.read = new Func<NetworkReader, float?>(NetworkReaderExtensions.ReadFloatNullable);
			Reader<double>.read = new Func<NetworkReader, double>(NetworkReaderExtensions.ReadDouble);
			Reader<double?>.read = new Func<NetworkReader, double?>(NetworkReaderExtensions.ReadDoubleNullable);
			Reader<decimal>.read = new Func<NetworkReader, decimal>(NetworkReaderExtensions.ReadDecimal);
			Reader<decimal?>.read = new Func<NetworkReader, decimal?>(NetworkReaderExtensions.ReadDecimalNullable);
			Reader<string>.read = new Func<NetworkReader, string>(NetworkReaderExtensions.ReadString);
			Reader<byte[]>.read = new Func<NetworkReader, byte[]>(NetworkReaderExtensions.ReadBytesAndSize);
			Reader<ArraySegment<byte>>.read = new Func<NetworkReader, ArraySegment<byte>>(NetworkReaderExtensions.ReadBytesAndSizeSegment);
			Reader<Vector2>.read = new Func<NetworkReader, Vector2>(NetworkReaderExtensions.ReadVector2);
			Reader<Vector2?>.read = new Func<NetworkReader, Vector2?>(NetworkReaderExtensions.ReadVector2Nullable);
			Reader<Vector3>.read = new Func<NetworkReader, Vector3>(NetworkReaderExtensions.ReadVector3);
			Reader<Vector3?>.read = new Func<NetworkReader, Vector3?>(NetworkReaderExtensions.ReadVector3Nullable);
			Reader<Vector4>.read = new Func<NetworkReader, Vector4>(NetworkReaderExtensions.ReadVector4);
			Reader<Vector4?>.read = new Func<NetworkReader, Vector4?>(NetworkReaderExtensions.ReadVector4Nullable);
			Reader<Vector2Int>.read = new Func<NetworkReader, Vector2Int>(NetworkReaderExtensions.ReadVector2Int);
			Reader<Vector2Int?>.read = new Func<NetworkReader, Vector2Int?>(NetworkReaderExtensions.ReadVector2IntNullable);
			Reader<Vector3Int>.read = new Func<NetworkReader, Vector3Int>(NetworkReaderExtensions.ReadVector3Int);
			Reader<Vector3Int?>.read = new Func<NetworkReader, Vector3Int?>(NetworkReaderExtensions.ReadVector3IntNullable);
			Reader<Color>.read = new Func<NetworkReader, Color>(NetworkReaderExtensions.ReadColor);
			Reader<Color?>.read = new Func<NetworkReader, Color?>(NetworkReaderExtensions.ReadColorNullable);
			Reader<Color32>.read = new Func<NetworkReader, Color32>(NetworkReaderExtensions.ReadColor32);
			Reader<Color32?>.read = new Func<NetworkReader, Color32?>(NetworkReaderExtensions.ReadColor32Nullable);
			Reader<Quaternion>.read = new Func<NetworkReader, Quaternion>(NetworkReaderExtensions.ReadQuaternion);
			Reader<Quaternion?>.read = new Func<NetworkReader, Quaternion?>(NetworkReaderExtensions.ReadQuaternionNullable);
			Reader<Rect>.read = new Func<NetworkReader, Rect>(NetworkReaderExtensions.ReadRect);
			Reader<Rect?>.read = new Func<NetworkReader, Rect?>(NetworkReaderExtensions.ReadRectNullable);
			Reader<Plane>.read = new Func<NetworkReader, Plane>(NetworkReaderExtensions.ReadPlane);
			Reader<Plane?>.read = new Func<NetworkReader, Plane?>(NetworkReaderExtensions.ReadPlaneNullable);
			Reader<Ray>.read = new Func<NetworkReader, Ray>(NetworkReaderExtensions.ReadRay);
			Reader<Ray?>.read = new Func<NetworkReader, Ray?>(NetworkReaderExtensions.ReadRayNullable);
			Reader<Matrix4x4>.read = new Func<NetworkReader, Matrix4x4>(NetworkReaderExtensions.ReadMatrix4x4);
			Reader<Matrix4x4?>.read = new Func<NetworkReader, Matrix4x4?>(NetworkReaderExtensions.ReadMatrix4x4Nullable);
			Reader<Guid>.read = new Func<NetworkReader, Guid>(NetworkReaderExtensions.ReadGuid);
			Reader<Guid?>.read = new Func<NetworkReader, Guid?>(NetworkReaderExtensions.ReadGuidNullable);
			Reader<NetworkIdentity>.read = new Func<NetworkReader, NetworkIdentity>(NetworkReaderExtensions.ReadNetworkIdentity);
			Reader<NetworkBehaviour>.read = new Func<NetworkReader, NetworkBehaviour>(NetworkReaderExtensions.ReadNetworkBehaviour);
			Reader<NetworkBehaviourSyncVar>.read = new Func<NetworkReader, NetworkBehaviourSyncVar>(NetworkReaderExtensions.ReadNetworkBehaviourSyncVar);
			Reader<Transform>.read = new Func<NetworkReader, Transform>(NetworkReaderExtensions.ReadTransform);
			Reader<GameObject>.read = new Func<NetworkReader, GameObject>(NetworkReaderExtensions.ReadGameObject);
			Reader<Uri>.read = new Func<NetworkReader, Uri>(NetworkReaderExtensions.ReadUri);
			Reader<Texture2D>.read = new Func<NetworkReader, Texture2D>(NetworkReaderExtensions.ReadTexture2D);
			Reader<Sprite>.read = new Func<NetworkReader, Sprite>(NetworkReaderExtensions.ReadSprite);
			Reader<DateTime>.read = new Func<NetworkReader, DateTime>(NetworkReaderExtensions.ReadDateTime);
			Reader<DateTime?>.read = new Func<NetworkReader, DateTime?>(NetworkReaderExtensions.ReadDateTimeNullable);
			Reader<TimeSnapshotMessage>.read = new Func<NetworkReader, TimeSnapshotMessage>(GeneratedNetworkCode._Read_Mirror.TimeSnapshotMessage);
			Reader<ReadyMessage>.read = new Func<NetworkReader, ReadyMessage>(GeneratedNetworkCode._Read_Mirror.ReadyMessage);
			Reader<NotReadyMessage>.read = new Func<NetworkReader, NotReadyMessage>(GeneratedNetworkCode._Read_Mirror.NotReadyMessage);
			Reader<AddPlayerMessage>.read = new Func<NetworkReader, AddPlayerMessage>(GeneratedNetworkCode._Read_Mirror.AddPlayerMessage);
			Reader<SceneMessage>.read = new Func<NetworkReader, SceneMessage>(GeneratedNetworkCode._Read_Mirror.SceneMessage);
			Reader<SceneOperation>.read = new Func<NetworkReader, SceneOperation>(GeneratedNetworkCode._Read_Mirror.SceneOperation);
			Reader<CommandMessage>.read = new Func<NetworkReader, CommandMessage>(GeneratedNetworkCode._Read_Mirror.CommandMessage);
			Reader<RpcMessage>.read = new Func<NetworkReader, RpcMessage>(GeneratedNetworkCode._Read_Mirror.RpcMessage);
			Reader<SpawnMessage>.read = new Func<NetworkReader, SpawnMessage>(GeneratedNetworkCode._Read_Mirror.SpawnMessage);
			Reader<ChangeOwnerMessage>.read = new Func<NetworkReader, ChangeOwnerMessage>(GeneratedNetworkCode._Read_Mirror.ChangeOwnerMessage);
			Reader<ObjectSpawnStartedMessage>.read = new Func<NetworkReader, ObjectSpawnStartedMessage>(GeneratedNetworkCode._Read_Mirror.ObjectSpawnStartedMessage);
			Reader<ObjectSpawnFinishedMessage>.read = new Func<NetworkReader, ObjectSpawnFinishedMessage>(GeneratedNetworkCode._Read_Mirror.ObjectSpawnFinishedMessage);
			Reader<ObjectDestroyMessage>.read = new Func<NetworkReader, ObjectDestroyMessage>(GeneratedNetworkCode._Read_Mirror.ObjectDestroyMessage);
			Reader<ObjectHideMessage>.read = new Func<NetworkReader, ObjectHideMessage>(GeneratedNetworkCode._Read_Mirror.ObjectHideMessage);
			Reader<EntityStateMessage>.read = new Func<NetworkReader, EntityStateMessage>(GeneratedNetworkCode._Read_Mirror.EntityStateMessage);
			Reader<NetworkPingMessage>.read = new Func<NetworkReader, NetworkPingMessage>(GeneratedNetworkCode._Read_Mirror.NetworkPingMessage);
			Reader<NetworkPongMessage>.read = new Func<NetworkReader, NetworkPongMessage>(GeneratedNetworkCode._Read_Mirror.NetworkPongMessage);
			Reader<ConnectionFailureMessage>.read = new Func<NetworkReader, ConnectionFailureMessage>(GeneratedNetworkCode._Read_ConnectionFailureMessage);
			Reader<PlayerSpawnNetworkMessage>.read = new Func<NetworkReader, PlayerSpawnNetworkMessage>(GeneratedNetworkCode._Read_PlayerSpawnNetworkMessage);
			Reader<PackageType>.read = new Func<NetworkReader, PackageType>(GeneratedNetworkCode._Read_PackageType);
			Reader<BossEventMessage>.read = new Func<NetworkReader, BossEventMessage>(GeneratedNetworkCode._Read_BossEventMessage);
			Reader<TvtEventMessage>.read = new Func<NetworkReader, TvtEventMessage>(GeneratedNetworkCode._Read_TvtEventMessage);
			Reader<AnimationType>.read = new Func<NetworkReader, AnimationType>(GeneratedNetworkCode._Read_AnimationType);
			Reader<AnimationConfig>.read = new Func<NetworkReader, AnimationConfig>(GeneratedNetworkCode._Read_AnimationConfig);
			Reader<Direction>.read = new Func<NetworkReader, Direction>(GeneratedNetworkCode._Read_Direction);
			Reader<global::Attribute>.read = new Func<NetworkReader, global::Attribute>(GeneratedNetworkCode._Read_Attribute);
			Reader<AttributeType>.read = new Func<NetworkReader, AttributeType>(GeneratedNetworkCode._Read_AttributeType);
			Reader<TrainingMode>.read = new Func<NetworkReader, TrainingMode>(GeneratedNetworkCode._Read_TrainingMode);
			Reader<PlayerProfession>.read = new Func<NetworkReader, PlayerProfession>(GeneratedNetworkCode._Read_PlayerProfession);
			Reader<AccessLevel>.read = new Func<NetworkReader, AccessLevel>(GeneratedNetworkCode._Read_AccessLevel);
			Reader<AccountFriend>.read = new Func<NetworkReader, AccountFriend>(GeneratedNetworkCode._Read_AccountFriend);
			Reader<ChatMessage>.read = new Func<NetworkReader, ChatMessage>(GeneratedNetworkCode._Read_ChatMessage);
			Reader<string[]>.read = new Func<NetworkReader, string[]>(GeneratedNetworkCode._Read_System.String[]);
			Reader<Rank>.read = new Func<NetworkReader, Rank>(GeneratedNetworkCode._Read_Rank);
			Reader<Condition>.read = new Func<NetworkReader, Condition>(GeneratedNetworkCode._Read_Condition);
			Reader<ConditionType>.read = new Func<NetworkReader, ConditionType>(GeneratedNetworkCode._Read_ConditionType);
			Reader<ConditionCategory>.read = new Func<NetworkReader, ConditionCategory>(GeneratedNetworkCode._Read_ConditionCategory);
			Reader<PlayerBlueprint>.read = new Func<NetworkReader, PlayerBlueprint>(GeneratedNetworkCode._Read_PlayerBlueprint);
			Reader<Blueprint>.read = new Func<NetworkReader, Blueprint>(GeneratedNetworkCode._Read_Blueprint);
			Reader<Item>.read = new Func<NetworkReader, Item>(GeneratedNetworkCode._Read_Item);
			Reader<Rarity>.read = new Func<NetworkReader, Rarity>(GeneratedNetworkCode._Read_Rarity);
			Reader<ItemType>.read = new Func<NetworkReader, ItemType>(GeneratedNetworkCode._Read_ItemType);
			Reader<SlotType>.read = new Func<NetworkReader, SlotType>(GeneratedNetworkCode._Read_SlotType);
			Reader<ItemQuality>.read = new Func<NetworkReader, ItemQuality>(GeneratedNetworkCode._Read_ItemQuality);
			Reader<ItemCategory>.read = new Func<NetworkReader, ItemCategory>(GeneratedNetworkCode._Read_ItemCategory);
			Reader<Vocation>.read = new Func<NetworkReader, Vocation>(GeneratedNetworkCode._Read_Vocation);
			Reader<BlueprintMaterial[]>.read = new Func<NetworkReader, BlueprintMaterial[]>(GeneratedNetworkCode._Read_BlueprintMaterial[]);
			Reader<BlueprintMaterial>.read = new Func<NetworkReader, BlueprintMaterial>(GeneratedNetworkCode._Read_BlueprintMaterial);
			Reader<CreatureGender>.read = new Func<NetworkReader, CreatureGender>(GeneratedNetworkCode._Read_CreatureGender);
			Reader<DailyReward>.read = new Func<NetworkReader, DailyReward>(GeneratedNetworkCode._Read_DailyReward);
			Reader<EffectConfig>.read = new Func<NetworkReader, EffectConfig>(GeneratedNetworkCode._Read_EffectConfig);
			Reader<GuildMemberRank>.read = new Func<NetworkReader, GuildMemberRank>(GeneratedNetworkCode._Read_GuildMemberRank);
			Reader<Effect>.read = new Func<NetworkReader, Effect>(GeneratedNetworkCode._Read_Effect);
			Reader<Quest>.read = new Func<NetworkReader, Quest>(GeneratedNetworkCode._Read_Quest);
			Reader<QuestReward[]>.read = new Func<NetworkReader, QuestReward[]>(GeneratedNetworkCode._Read_QuestReward[]);
			Reader<QuestReward>.read = new Func<NetworkReader, QuestReward>(GeneratedNetworkCode._Read_QuestReward);
			Reader<RewardType>.read = new Func<NetworkReader, RewardType>(GeneratedNetworkCode._Read_RewardType);
			Reader<QuestObjective[]>.read = new Func<NetworkReader, QuestObjective[]>(GeneratedNetworkCode._Read_QuestObjective[]);
			Reader<QuestObjective>.read = new Func<NetworkReader, QuestObjective>(GeneratedNetworkCode._Read_QuestObjective);
			Reader<ObjectiveType>.read = new Func<NetworkReader, ObjectiveType>(GeneratedNetworkCode._Read_ObjectiveType);
			Reader<PartyMember>.read = new Func<NetworkReader, PartyMember>(GeneratedNetworkCode._Read_PartyMember);
			Reader<MarketOrder>.read = new Func<NetworkReader, MarketOrder>(GeneratedNetworkCode._Read_MarketOrder);
			Reader<NpcDialog>.read = new Func<NetworkReader, NpcDialog>(GeneratedNetworkCode._Read_NpcDialog);
			Reader<NpcChoice[]>.read = new Func<NetworkReader, NpcChoice[]>(GeneratedNetworkCode._Read_NpcChoice[]);
			Reader<NpcChoice>.read = new Func<NetworkReader, NpcChoice>(GeneratedNetworkCode._Read_NpcChoice);
			Reader<StoreItemConfig[]>.read = new Func<NetworkReader, StoreItemConfig[]>(GeneratedNetworkCode._Read_StoreItemConfig[]);
			Reader<StoreItemConfig>.read = new Func<NetworkReader, StoreItemConfig>(GeneratedNetworkCode._Read_StoreItemConfig);
			Reader<StoreAction>.read = new Func<NetworkReader, StoreAction>(GeneratedNetworkCode._Read_StoreAction);
			Reader<PvpStatus>.read = new Func<NetworkReader, PvpStatus>(GeneratedNetworkCode._Read_PvpStatus);
			Reader<TvtTeam>.read = new Func<NetworkReader, TvtTeam>(GeneratedNetworkCode._Read_TvtTeam);
			Reader<PlayerQuest>.read = new Func<NetworkReader, PlayerQuest>(GeneratedNetworkCode._Read_PlayerQuest);
			Reader<PlayerQuestReward[]>.read = new Func<NetworkReader, PlayerQuestReward[]>(GeneratedNetworkCode._Read_PlayerQuestReward[]);
			Reader<PlayerQuestReward>.read = new Func<NetworkReader, PlayerQuestReward>(GeneratedNetworkCode._Read_PlayerQuestReward);
			Reader<PlayerQuestObjective[]>.read = new Func<NetworkReader, PlayerQuestObjective[]>(GeneratedNetworkCode._Read_PlayerQuestObjective[]);
			Reader<PlayerQuestObjective>.read = new Func<NetworkReader, PlayerQuestObjective>(GeneratedNetworkCode._Read_PlayerQuestObjective);
			Reader<Skill>.read = new Func<NetworkReader, Skill>(GeneratedNetworkCode._Read_Skill);
			Reader<SkillType>.read = new Func<NetworkReader, SkillType>(GeneratedNetworkCode._Read_SkillType);
			Reader<SkillCategory>.read = new Func<NetworkReader, SkillCategory>(GeneratedNetworkCode._Read_SkillCategory);
			Reader<TalentIdentifier>.read = new Func<NetworkReader, TalentIdentifier>(GeneratedNetworkCode._Read_TalentIdentifier);
			Reader<Title>.read = new Func<NetworkReader, Title>(GeneratedNetworkCode._Read_Title);
		}
	}
}
