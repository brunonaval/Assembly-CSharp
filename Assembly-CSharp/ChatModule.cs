using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

// Token: 0x020002CF RID: 719
public class ChatModule : NetworkBehaviour
{
	// Token: 0x14000004 RID: 4
	// (add) Token: 0x06000C7F RID: 3199 RVA: 0x00038464 File Offset: 0x00036664
	// (remove) Token: 0x06000C80 RID: 3200 RVA: 0x0003849C File Offset: 0x0003669C
	public event ChatModule.OnFriendAddedEventHandler OnFriendAdded;

	// Token: 0x14000005 RID: 5
	// (add) Token: 0x06000C81 RID: 3201 RVA: 0x000384D4 File Offset: 0x000366D4
	// (remove) Token: 0x06000C82 RID: 3202 RVA: 0x0003850C File Offset: 0x0003670C
	public event ChatModule.OnFriendRemovedEventHandler OnFriendRemoved;

	// Token: 0x14000006 RID: 6
	// (add) Token: 0x06000C83 RID: 3203 RVA: 0x00038544 File Offset: 0x00036744
	// (remove) Token: 0x06000C84 RID: 3204 RVA: 0x0003857C File Offset: 0x0003677C
	public event ChatModule.OnFriendBecomesOnlineEventHandler OnFriendBecomesOnline;

	// Token: 0x06000C85 RID: 3205 RVA: 0x000385B4 File Offset: 0x000367B4
	private void Awake()
	{
		this.syncInterval = 0.5f;
		this.localChatColliders = new Collider2D[50];
		base.TryGetComponent<TradeModule>(out this.tradeModule);
		base.TryGetComponent<PvpModule>(out this.pvpModule);
		this.partyModule = base.GetComponent<PartyModule>();
		this.walletModule = base.GetComponent<WalletModule>();
		this.combatModule = base.GetComponent<CombatModule>();
		this.effectModule = base.GetComponent<EffectModule>();
		this.playerModule = base.GetComponent<PlayerModule>();
		this.creatureModule = base.GetComponent<CreatureModule>();
		this.movementModule = base.GetComponent<MovementModule>();
		this.conditionModule = base.GetComponent<ConditionModule>();
		this.inventoryModule = base.GetComponent<InventoryModule>();
		this.attributeModule = base.GetComponent<AttributeModule>();
		GameObject gameObject = GameObject.FindGameObjectWithTag("GameEnvironment");
		this.gameEnvironmentModule = gameObject.GetComponent<GameEnvironmentModule>();
		if (NetworkServer.active)
		{
			GameObject gameObject2 = GameObject.FindGameObjectWithTag("DatabaseManager");
			this.npcDatabaseModule = gameObject2.GetComponent<NpcDatabaseModule>();
			this.itemDatabaseModule = gameObject2.GetComponent<ItemDatabaseModule>();
		}
		if (NetworkClient.active)
		{
			GameObject gameObject3 = GameObject.FindGameObjectWithTag("UISystem");
			this.uiSystemModule = gameObject3.GetComponent<UISystemModule>();
		}
	}

	// Token: 0x06000C86 RID: 3206 RVA: 0x000386CA File Offset: 0x000368CA
	public override void OnStartLocalPlayer()
	{
		this.Friends.Callback += this.OnFriendsUpdated;
	}

	// Token: 0x06000C87 RID: 3207 RVA: 0x000386E3 File Offset: 0x000368E3
	public override void OnStartServer()
	{
		base.InvokeRepeating("CheckIfFriendIsOnlineTimer", 2f, 2f);
	}

	// Token: 0x06000C88 RID: 3208 RVA: 0x000386FA File Offset: 0x000368FA
	private void OnFriendsUpdated(SyncList<AccountFriend>.Operation op, int itemIndex, AccountFriend oldItem, AccountFriend newItem)
	{
		this.playerModule.SetFriendListDirty(true);
	}

	// Token: 0x06000C89 RID: 3209 RVA: 0x00038708 File Offset: 0x00036908
	private void OnDestroy()
	{
		bool active = NetworkServer.active;
	}

	// Token: 0x06000C8A RID: 3210 RVA: 0x00038710 File Offset: 0x00036910
	[TargetRpc]
	public void TargetSendChatMessage(ChatMessage message)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		Mirror.GeneratedNetworkCode._Write_ChatMessage(writer, message);
		this.SendTargetRPCInternal(null, "System.Void ChatModule::TargetSendChatMessage(ChatMessage)", 1158255092, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000C8B RID: 3211 RVA: 0x0003874C File Offset: 0x0003694C
	[Command]
	public void CmdSendChatMessage(string channelName, bool isChannelGlobal, bool isChannelParty, string content)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(channelName);
		writer.WriteBool(isChannelGlobal);
		writer.WriteBool(isChannelParty);
		writer.WriteString(content);
		base.SendCommandInternal("System.Void ChatModule::CmdSendChatMessage(System.String,System.Boolean,System.Boolean,System.String)", 956503752, writer, 0, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000C8C RID: 3212 RVA: 0x000387A4 File Offset: 0x000369A4
	[Server]
	private void SendMessageToParty(string content, ChatContact senderContact)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ChatModule::SendMessageToParty(System.String,ChatContact)' called when server was not active");
			return;
		}
		ChatMessage message = ChatModule.BuildPartyChatMessage(content, senderContact);
		foreach (PartyMember partyMember in this.partyModule.PartyMembers)
		{
			if (partyMember.IsDefined)
			{
				partyMember.Member.GetComponent<ChatModule>().TargetSendChatMessage(message);
			}
		}
		this.TargetSendChatMessage(message);
	}

	// Token: 0x06000C8D RID: 3213 RVA: 0x00038834 File Offset: 0x00036A34
	private static ChatMessage BuildPartyChatMessage(string content, ChatContact senderContact)
	{
		AttributeModule attributeModule;
		senderContact.Player.TryGetComponent<AttributeModule>(out attributeModule);
		return new ChatMessage("chat_tab_party", senderContact.Name, string.Format("{0} [{1}]", senderContact.Name, attributeModule.BaseLevel), (double)DateTime.Now.Ticks, content, false, true, false, true, null, false);
	}

	// Token: 0x06000C8E RID: 3214 RVA: 0x00038890 File Offset: 0x00036A90
	[Server]
	private void SendLocalMessage(string content, ChatContact senderContact)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ChatModule::SendLocalMessage(System.String,ChatContact)' called when server was not active");
			return;
		}
		ChatMessage message = this.BuildLocalChatMessage(content, senderContact);
		int layerMask = 1 << LayerMask.NameToLayer("Player");
		int num = Physics2D.OverlapCircleNonAlloc(base.transform.position, 6f, this.localChatColliders, layerMask);
		for (int i = 0; i < num; i++)
		{
			this.localChatColliders[i].GetComponent<ChatModule>().TargetSendChatMessage(message);
		}
		content = string.Format("[{0}]: {1}", senderContact.Name, content);
		this.effectModule.ShowAnimatedText(content, 5, false, base.transform.position);
	}

	// Token: 0x06000C8F RID: 3215 RVA: 0x00038938 File Offset: 0x00036B38
	private ChatMessage BuildLocalChatMessage(string content, ChatContact senderContact)
	{
		AttributeModule attributeModule;
		senderContact.Player.TryGetComponent<AttributeModule>(out attributeModule);
		return new ChatMessage("chat_tab_default", senderContact.Name, string.Format("{0} [{1}]", senderContact.Name, attributeModule.BaseLevel), (double)DateTime.Now.Ticks, content, false, true, false, false, null, false);
	}

	// Token: 0x06000C90 RID: 3216 RVA: 0x00038994 File Offset: 0x00036B94
	[Server]
	private void SendPrivateMessage(string channelName, string content, ChatContact senderContact)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ChatModule::SendPrivateMessage(System.String,System.String,ChatContact)' called when server was not active");
			return;
		}
		ChatContact chatContact = this.gameEnvironmentModule.FindContact(channelName);
		if (!this.ValidatePrivateMessage(channelName, chatContact))
		{
			return;
		}
		ChatMessage message = ChatModule.BuildPrivateChatMessage(content, senderContact);
		chatContact.Player.GetComponent<ChatModule>().TargetSendChatMessage(message);
		message.Channel = chatContact.Name;
		this.TargetSendChatMessage(message);
	}

	// Token: 0x06000C91 RID: 3217 RVA: 0x000389FC File Offset: 0x00036BFC
	private static ChatMessage BuildPrivateChatMessage(string content, ChatContact senderContact)
	{
		AttributeModule attributeModule;
		senderContact.Player.TryGetComponent<AttributeModule>(out attributeModule);
		CreatureModule creatureModule;
		senderContact.Player.TryGetComponent<CreatureModule>(out creatureModule);
		return new ChatMessage(creatureModule.OriginalCreatureName, creatureModule.OriginalCreatureName, string.Format("{0} [{1}]", senderContact.Name, attributeModule.BaseLevel), (double)DateTime.Now.Ticks, content, false, false, false, false, null, false);
	}

	// Token: 0x06000C92 RID: 3218 RVA: 0x00038A68 File Offset: 0x00036C68
	[Server]
	private bool ValidatePrivateMessage(string channelName, ChatContact receiverContact)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean ChatModule::ValidatePrivateMessage(System.String,ChatContact)' called when server was not active");
			return default(bool);
		}
		if (receiverContact.ConnectionId == base.connectionToClient.connectionId)
		{
			this.effectModule.ShowScreenMessage("chat_cant_send_yourself_message", 0, 3.5f, Array.Empty<string>());
			return false;
		}
		if (!receiverContact.IsDefined)
		{
			this.effectModule.ShowScreenMessage("chat_player_not_online_message", 0, 3.5f, new string[]
			{
				channelName
			});
			return false;
		}
		return true;
	}

	// Token: 0x06000C93 RID: 3219 RVA: 0x00038AF4 File Offset: 0x00036CF4
	[Server]
	private void SendHelpMessage(string content, ChatContact senderContact)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ChatModule::SendHelpMessage(System.String,ChatContact)' called when server was not active");
			return;
		}
		ChatMessage message = this.BuildHelpChatMessage(content, senderContact);
		for (int i = 0; i < this.gameEnvironmentModule.Contacts.Count; i++)
		{
			ChatContact chatContact = this.gameEnvironmentModule.Contacts[i];
			if (chatContact.Player != null)
			{
				chatContact.Player.GetComponent<ChatModule>().TargetSendChatMessage(message);
			}
		}
	}

	// Token: 0x06000C94 RID: 3220 RVA: 0x00038B6C File Offset: 0x00036D6C
	[Server]
	private void SendGlobalMessage(string content, ChatContact senderContact)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ChatModule::SendGlobalMessage(System.String,ChatContact)' called when server was not active");
			return;
		}
		ChatMessage message = this.BuildGlobalChatMessage(content, senderContact);
		for (int i = 0; i < this.gameEnvironmentModule.Contacts.Count; i++)
		{
			ChatContact chatContact = this.gameEnvironmentModule.Contacts[i];
			if (chatContact.Player != null)
			{
				chatContact.Player.GetComponent<ChatModule>().TargetSendChatMessage(message);
			}
		}
	}

	// Token: 0x06000C95 RID: 3221 RVA: 0x00038BE4 File Offset: 0x00036DE4
	private ChatMessage BuildHelpChatMessage(string content, ChatContact senderContact)
	{
		AttributeModule attributeModule;
		senderContact.Player.TryGetComponent<AttributeModule>(out attributeModule);
		return new ChatMessage("chat_tab_help", senderContact.Name, string.Format("{0} [{1}]", senderContact.Name, attributeModule.BaseLevel), (double)DateTime.Now.Ticks, content, false, true, true, false, null, false);
	}

	// Token: 0x06000C96 RID: 3222 RVA: 0x00038C40 File Offset: 0x00036E40
	private ChatMessage BuildGlobalChatMessage(string content, ChatContact senderContact)
	{
		AttributeModule attributeModule;
		senderContact.Player.TryGetComponent<AttributeModule>(out attributeModule);
		return new ChatMessage("chat_tab_global", senderContact.Name, string.Format("{0} [{1}]", senderContact.Name, attributeModule.BaseLevel), (double)DateTime.Now.Ticks, content, false, true, true, false, null, false);
	}

	// Token: 0x06000C97 RID: 3223 RVA: 0x00038C9C File Offset: 0x00036E9C
	[Server]
	private Task<bool> RunChatCommandAsync(string command)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Threading.Tasks.Task`1<System.Boolean> ChatModule::RunChatCommandAsync(System.String)' called when server was not active");
			return null;
		}
		ChatModule.<RunChatCommandAsync>d__51 <RunChatCommandAsync>d__;
		<RunChatCommandAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<RunChatCommandAsync>d__.<>4__this = this;
		<RunChatCommandAsync>d__.command = command;
		<RunChatCommandAsync>d__.<>1__state = -1;
		<RunChatCommandAsync>d__.<>t__builder.Start<ChatModule.<RunChatCommandAsync>d__51>(ref <RunChatCommandAsync>d__);
		return <RunChatCommandAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000C98 RID: 3224 RVA: 0x00038D08 File Offset: 0x00036F08
	[Server]
	private IEnumerator TeleportAfterDelay()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Collections.IEnumerator ChatModule::TeleportAfterDelay()' called when server was not active");
			return null;
		}
		ChatModule.<TeleportAfterDelay>d__52 <TeleportAfterDelay>d__ = new ChatModule.<TeleportAfterDelay>d__52(0);
		<TeleportAfterDelay>d__.<>4__this = this;
		return <TeleportAfterDelay>d__;
	}

	// Token: 0x06000C99 RID: 3225 RVA: 0x00038D44 File Offset: 0x00036F44
	[Server]
	private bool ChatCommandUnstuck()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean ChatModule::ChatCommandUnstuck()' called when server was not active");
			return default(bool);
		}
		base.StartCoroutine(this.TeleportAfterDelay());
		return true;
	}

	// Token: 0x06000C9A RID: 3226 RVA: 0x00038D80 File Offset: 0x00036F80
	[Server]
	private bool ChatCommandHelp()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean ChatModule::ChatCommandHelp()' called when server was not active");
			return default(bool);
		}
		this.TargetShowHelpWindow();
		return true;
	}

	// Token: 0x06000C9B RID: 3227 RVA: 0x00038DB8 File Offset: 0x00036FB8
	[TargetRpc]
	private void TargetShowHelpWindow()
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		this.SendTargetRPCInternal(null, "System.Void ChatModule::TargetShowHelpWindow()", 321142568, writer, 0);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000C9C RID: 3228 RVA: 0x00038DE8 File Offset: 0x00036FE8
	[Server]
	private Task<bool> ChatCommandTransferGoldAsync(string command)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Threading.Tasks.Task`1<System.Boolean> ChatModule::ChatCommandTransferGoldAsync(System.String)' called when server was not active");
			return null;
		}
		ChatModule.<ChatCommandTransferGoldAsync>d__56 <ChatCommandTransferGoldAsync>d__;
		<ChatCommandTransferGoldAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<ChatCommandTransferGoldAsync>d__.<>4__this = this;
		<ChatCommandTransferGoldAsync>d__.command = command;
		<ChatCommandTransferGoldAsync>d__.<>1__state = -1;
		<ChatCommandTransferGoldAsync>d__.<>t__builder.Start<ChatModule.<ChatCommandTransferGoldAsync>d__56>(ref <ChatCommandTransferGoldAsync>d__);
		return <ChatCommandTransferGoldAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000C9D RID: 3229 RVA: 0x00038E54 File Offset: 0x00037054
	[Server]
	private Task<bool> ChatCommandAddFriendAsync(string command)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Threading.Tasks.Task`1<System.Boolean> ChatModule::ChatCommandAddFriendAsync(System.String)' called when server was not active");
			return null;
		}
		ChatModule.<ChatCommandAddFriendAsync>d__57 <ChatCommandAddFriendAsync>d__;
		<ChatCommandAddFriendAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<ChatCommandAddFriendAsync>d__.<>4__this = this;
		<ChatCommandAddFriendAsync>d__.command = command;
		<ChatCommandAddFriendAsync>d__.<>1__state = -1;
		<ChatCommandAddFriendAsync>d__.<>t__builder.Start<ChatModule.<ChatCommandAddFriendAsync>d__57>(ref <ChatCommandAddFriendAsync>d__);
		return <ChatCommandAddFriendAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000C9E RID: 3230 RVA: 0x00038EC0 File Offset: 0x000370C0
	[Server]
	private bool ChatCommandRemoveFriend(string command)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean ChatModule::ChatCommandRemoveFriend(System.String)' called when server was not active");
			return default(bool);
		}
		string text = command.Substring(command.IndexOf(" ")).Trim();
		if (!string.IsNullOrEmpty(text))
		{
			this.RemoveFriend(text);
			return true;
		}
		return false;
	}

	// Token: 0x06000C9F RID: 3231 RVA: 0x00038F18 File Offset: 0x00037118
	[Server]
	private bool ChatCommandTrade(string command)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean ChatModule::ChatCommandTrade(System.String)' called when server was not active");
			return default(bool);
		}
		if (!this.combatModule.HasTarget)
		{
			return true;
		}
		this.tradeModule.BeginHandshake(this.combatModule.Target);
		return true;
	}

	// Token: 0x06000CA0 RID: 3232 RVA: 0x00038F6C File Offset: 0x0003716C
	[Server]
	private bool ChatCommandInviteToParty(string command)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean ChatModule::ChatCommandInviteToParty(System.String)' called when server was not active");
			return default(bool);
		}
		int num = command.IndexOf(" ");
		if (num >= command.Length)
		{
			return false;
		}
		if (num < 0)
		{
			this.partyModule.BeginInvite(this.combatModule.Target);
			return true;
		}
		string name = command.Substring(num).Trim();
		ChatContact chatContact = this.gameEnvironmentModule.FindContact(name);
		if (chatContact.IsDefined)
		{
			this.partyModule.BeginInvite(chatContact.Player);
		}
		else if (this.combatModule.Target != null)
		{
			this.partyModule.BeginInvite(this.combatModule.Target);
		}
		return true;
	}

	// Token: 0x06000CA1 RID: 3233 RVA: 0x00039030 File Offset: 0x00037230
	[Server]
	private bool ChatCommandLeaveParty(string command)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean ChatModule::ChatCommandLeaveParty(System.String)' called when server was not active");
			return default(bool);
		}
		this.partyModule.LeaveParty();
		return true;
	}

	// Token: 0x06000CA2 RID: 3234 RVA: 0x0003906C File Offset: 0x0003726C
	[Server]
	private bool ChatCommandKickFromParty(string command)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean ChatModule::ChatCommandKickFromParty(System.String)' called when server was not active");
			return default(bool);
		}
		if (!command.Contains(" "))
		{
			return false;
		}
		string text = command.Substring(command.IndexOf(" ")).Trim();
		if (!string.IsNullOrEmpty(text))
		{
			this.partyModule.KickFromParty(text);
			return true;
		}
		return false;
	}

	// Token: 0x06000CA3 RID: 3235 RVA: 0x000390D8 File Offset: 0x000372D8
	[Server]
	private Task<bool> ChatCommandMuteAsync(string command)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Threading.Tasks.Task`1<System.Boolean> ChatModule::ChatCommandMuteAsync(System.String)' called when server was not active");
			return null;
		}
		ChatModule.<ChatCommandMuteAsync>d__63 <ChatCommandMuteAsync>d__;
		<ChatCommandMuteAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<ChatCommandMuteAsync>d__.<>4__this = this;
		<ChatCommandMuteAsync>d__.command = command;
		<ChatCommandMuteAsync>d__.<>1__state = -1;
		<ChatCommandMuteAsync>d__.<>t__builder.Start<ChatModule.<ChatCommandMuteAsync>d__63>(ref <ChatCommandMuteAsync>d__);
		return <ChatCommandMuteAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000CA4 RID: 3236 RVA: 0x00039144 File Offset: 0x00037344
	[Server]
	private Task<bool> ChatCommandWarningAsync(string command)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Threading.Tasks.Task`1<System.Boolean> ChatModule::ChatCommandWarningAsync(System.String)' called when server was not active");
			return null;
		}
		ChatModule.<ChatCommandWarningAsync>d__64 <ChatCommandWarningAsync>d__;
		<ChatCommandWarningAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<ChatCommandWarningAsync>d__.<>4__this = this;
		<ChatCommandWarningAsync>d__.command = command;
		<ChatCommandWarningAsync>d__.<>1__state = -1;
		<ChatCommandWarningAsync>d__.<>t__builder.Start<ChatModule.<ChatCommandWarningAsync>d__64>(ref <ChatCommandWarningAsync>d__);
		return <ChatCommandWarningAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000CA5 RID: 3237 RVA: 0x000391B0 File Offset: 0x000373B0
	[Server]
	private bool ChatCommandBroadcast(string command)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean ChatModule::ChatCommandBroadcast(System.String)' called when server was not active");
			return default(bool);
		}
		if (this.attributeModule.AccessLevel < AccessLevel.GameMaster)
		{
			return false;
		}
		string content = command.Substring(command.IndexOf(" ")).Trim();
		this.gameEnvironmentModule.BroadcastChatMessage(null, content);
		return true;
	}

	// Token: 0x06000CA6 RID: 3238 RVA: 0x00039214 File Offset: 0x00037414
	[Server]
	private bool ChatCommandGlobalEventOn(string command)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean ChatModule::ChatCommandGlobalEventOn(System.String)' called when server was not active");
			return default(bool);
		}
		if (this.attributeModule.AccessLevel < AccessLevel.CommunityManager)
		{
			return false;
		}
		int num = command.IndexOf(" ", StringComparison.Ordinal);
		string[] array = command.Substring(num, command.Length - num).Trim().Split(new string[]
		{
			";"
		}, StringSplitOptions.RemoveEmptyEntries);
		if (array.Length != 3)
		{
			return false;
		}
		int num2;
		if (!int.TryParse(array[0], out num2))
		{
			return false;
		}
		float num3;
		if (!float.TryParse(array[1].Replace("%", ""), out num3))
		{
			return false;
		}
		int num4;
		if (!int.TryParse(array[2].Replace("lv", ""), out num4))
		{
			return false;
		}
		GlobalEventModule.Singleton.AddEvent(num2, num3, num4);
		this.effectModule.ShowScreenMessage(string.Format("[GLOBAL EVENT] Activated for ITEM_ID {0}, DROP_CHANCE: {1} and MIN_LEVEL: {2}", num2, num3, num4), 1, 3.5f, Array.Empty<string>());
		return true;
	}

	// Token: 0x06000CA7 RID: 3239 RVA: 0x00039320 File Offset: 0x00037520
	[Server]
	private bool ChatCommandGlobalEventOff(string command)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean ChatModule::ChatCommandGlobalEventOff(System.String)' called when server was not active");
			return default(bool);
		}
		if (this.attributeModule.AccessLevel < AccessLevel.CommunityManager)
		{
			return false;
		}
		int num = command.IndexOf(" ", StringComparison.Ordinal);
		string[] array = command.Substring(num, command.Length - num).Trim().Split(new string[]
		{
			";"
		}, StringSplitOptions.RemoveEmptyEntries);
		if (array.Length != 1)
		{
			return false;
		}
		int num2;
		if (!int.TryParse(array[0], out num2))
		{
			return false;
		}
		GlobalEventModule.Singleton.RemoveEvent(num2);
		this.effectModule.ShowScreenMessage(string.Format("[GLOBAL EVENT] Disabled for ITEM_ID {0}", num2), 2, 3.5f, Array.Empty<string>());
		return true;
	}

	// Token: 0x06000CA8 RID: 3240 RVA: 0x000393E0 File Offset: 0x000375E0
	[Server]
	private bool ChatCommandEventOn(string command)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean ChatModule::ChatCommandEventOn(System.String)' called when server was not active");
			return default(bool);
		}
		if (this.attributeModule.AccessLevel < AccessLevel.CommunityManager)
		{
			return false;
		}
		string text = command.Substring(command.IndexOf(" ")).Trim();
		if (string.IsNullOrEmpty(text))
		{
			return false;
		}
		string a = text.Trim().ToLower();
		if (a == "copper")
		{
			PlayerPrefs.SetInt("copper-event", 1);
			this.effectModule.ShowScreenMessage("You activated Copper Event and now Copper Tokens will drop from monsters.", 2, 3.5f, Array.Empty<string>());
			ServerSettingsManager.CopperEventActive = true;
			return true;
		}
		if (a == "silver")
		{
			PlayerPrefs.SetInt("silver-event", 1);
			this.effectModule.ShowScreenMessage("You activated Silver Event and now Silver Tokens will drop from monsters.", 2, 3.5f, Array.Empty<string>());
			ServerSettingsManager.SilverEventActive = true;
			return true;
		}
		if (!(a == "gold"))
		{
			return false;
		}
		PlayerPrefs.SetInt("gold-event", 1);
		this.effectModule.ShowScreenMessage("You activated Gold Event and now Gold Tokens will drop from monsters.", 2, 3.5f, Array.Empty<string>());
		ServerSettingsManager.GoldEventActive = true;
		return true;
	}

	// Token: 0x06000CA9 RID: 3241 RVA: 0x00039504 File Offset: 0x00037704
	[Server]
	private bool ChatCommandEventStatus()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean ChatModule::ChatCommandEventStatus()' called when server was not active");
			return default(bool);
		}
		if (this.attributeModule.AccessLevel < AccessLevel.CommunityManager)
		{
			return false;
		}
		string text = "Copper Event: " + (ServerSettingsManager.CopperEventActive ? "on" : "off");
		text = text + " | Silver Event: " + (ServerSettingsManager.SilverEventActive ? "on" : "off");
		text = text + " | Gold Event: " + (ServerSettingsManager.GoldEventActive ? "on" : "off");
		this.effectModule.ShowScreenMessage(text, 2, 3.5f, Array.Empty<string>());
		return true;
	}

	// Token: 0x06000CAA RID: 3242 RVA: 0x000395B8 File Offset: 0x000377B8
	[Server]
	private bool ChatCommandEventOff(string command)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean ChatModule::ChatCommandEventOff(System.String)' called when server was not active");
			return default(bool);
		}
		if (this.attributeModule.AccessLevel < AccessLevel.CommunityManager)
		{
			return false;
		}
		string text = command.Substring(command.IndexOf(" ")).Trim();
		if (string.IsNullOrEmpty(text))
		{
			return false;
		}
		string a = text.Trim().ToLower();
		if (a == "copper")
		{
			PlayerPrefs.SetInt("copper-event", 0);
			this.effectModule.ShowScreenMessage("You deactivated Copper Event and now Copper Tokens will NOT drop from monsters.", 2, 3.5f, Array.Empty<string>());
			ServerSettingsManager.CopperEventActive = false;
			return true;
		}
		if (a == "silver")
		{
			PlayerPrefs.SetInt("silver-event", 0);
			this.effectModule.ShowScreenMessage("You deactivated Silver Event and now Silver Tokens will NOT drop from monsters.", 2, 3.5f, Array.Empty<string>());
			ServerSettingsManager.SilverEventActive = false;
			return true;
		}
		if (!(a == "gold"))
		{
			return false;
		}
		PlayerPrefs.SetInt("gold-event", 0);
		this.effectModule.ShowScreenMessage("You deactivated Gold Event and now Gold Tokens will NOT drop from monsters.", 2, 3.5f, Array.Empty<string>());
		ServerSettingsManager.GoldEventActive = false;
		return true;
	}

	// Token: 0x06000CAB RID: 3243 RVA: 0x000396DC File Offset: 0x000378DC
	[Server]
	private Task<bool> ChatCommandSetExpRateAsync(string command)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Threading.Tasks.Task`1<System.Boolean> ChatModule::ChatCommandSetExpRateAsync(System.String)' called when server was not active");
			return null;
		}
		ChatModule.<ChatCommandSetExpRateAsync>d__71 <ChatCommandSetExpRateAsync>d__;
		<ChatCommandSetExpRateAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<ChatCommandSetExpRateAsync>d__.<>4__this = this;
		<ChatCommandSetExpRateAsync>d__.command = command;
		<ChatCommandSetExpRateAsync>d__.<>1__state = -1;
		<ChatCommandSetExpRateAsync>d__.<>t__builder.Start<ChatModule.<ChatCommandSetExpRateAsync>d__71>(ref <ChatCommandSetExpRateAsync>d__);
		return <ChatCommandSetExpRateAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000CAC RID: 3244 RVA: 0x00039748 File Offset: 0x00037948
	[Server]
	private Task<bool> ChatCommandSetCraftExpRateAsync(string command)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Threading.Tasks.Task`1<System.Boolean> ChatModule::ChatCommandSetCraftExpRateAsync(System.String)' called when server was not active");
			return null;
		}
		ChatModule.<ChatCommandSetCraftExpRateAsync>d__72 <ChatCommandSetCraftExpRateAsync>d__;
		<ChatCommandSetCraftExpRateAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<ChatCommandSetCraftExpRateAsync>d__.<>4__this = this;
		<ChatCommandSetCraftExpRateAsync>d__.command = command;
		<ChatCommandSetCraftExpRateAsync>d__.<>1__state = -1;
		<ChatCommandSetCraftExpRateAsync>d__.<>t__builder.Start<ChatModule.<ChatCommandSetCraftExpRateAsync>d__72>(ref <ChatCommandSetCraftExpRateAsync>d__);
		return <ChatCommandSetCraftExpRateAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000CAD RID: 3245 RVA: 0x000397B4 File Offset: 0x000379B4
	[Server]
	private bool ChatCommandGotoPlayer(string command)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean ChatModule::ChatCommandGotoPlayer(System.String)' called when server was not active");
			return default(bool);
		}
		if (this.attributeModule.AccessLevel < AccessLevel.CommunityManager)
		{
			return false;
		}
		string text = command.Substring(command.IndexOf(" ")).Trim();
		if (!string.IsNullOrEmpty(text))
		{
			ChatContact chatContact = this.gameEnvironmentModule.FindContact(text);
			if (chatContact.IsDefined & chatContact.Player != null)
			{
				this.movementModule.TargetTeleport(base.connectionToClient, chatContact.Player.transform.position, new Effect("TeleporterHit", 0.5f, 0.25f));
			}
			else
			{
				this.effectModule.ShowScreenMessage("chat_player_not_online_message", 0, 3.5f, new string[]
				{
					text
				});
			}
			return true;
		}
		return false;
	}

	// Token: 0x06000CAE RID: 3246 RVA: 0x00039890 File Offset: 0x00037A90
	[Server]
	private bool ChatCommandDisconnectPlayer(string command)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean ChatModule::ChatCommandDisconnectPlayer(System.String)' called when server was not active");
			return default(bool);
		}
		if (this.attributeModule.AccessLevel < AccessLevel.CommunityManager)
		{
			return false;
		}
		int num = command.IndexOf(" ");
		if (num >= command.Length)
		{
			return false;
		}
		string name = command.Substring(num).Trim();
		ChatContact chatContact = this.gameEnvironmentModule.FindContact(name);
		if (chatContact.Player == null)
		{
			return true;
		}
		chatContact.Player.GetComponent<NetworkIdentity>().connectionToClient.Disconnect();
		return true;
	}

	// Token: 0x06000CAF RID: 3247 RVA: 0x00039928 File Offset: 0x00037B28
	[Server]
	private Task<bool> ChatCommandBanAsync(string command)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Threading.Tasks.Task`1<System.Boolean> ChatModule::ChatCommandBanAsync(System.String)' called when server was not active");
			return null;
		}
		ChatModule.<ChatCommandBanAsync>d__75 <ChatCommandBanAsync>d__;
		<ChatCommandBanAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<ChatCommandBanAsync>d__.<>4__this = this;
		<ChatCommandBanAsync>d__.command = command;
		<ChatCommandBanAsync>d__.<>1__state = -1;
		<ChatCommandBanAsync>d__.<>t__builder.Start<ChatModule.<ChatCommandBanAsync>d__75>(ref <ChatCommandBanAsync>d__);
		return <ChatCommandBanAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000CB0 RID: 3248 RVA: 0x00039994 File Offset: 0x00037B94
	[Server]
	private Task<bool> ChatCommandUnbanAsync(string command)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Threading.Tasks.Task`1<System.Boolean> ChatModule::ChatCommandUnbanAsync(System.String)' called when server was not active");
			return null;
		}
		ChatModule.<ChatCommandUnbanAsync>d__76 <ChatCommandUnbanAsync>d__;
		<ChatCommandUnbanAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<ChatCommandUnbanAsync>d__.<>4__this = this;
		<ChatCommandUnbanAsync>d__.command = command;
		<ChatCommandUnbanAsync>d__.<>1__state = -1;
		<ChatCommandUnbanAsync>d__.<>t__builder.Start<ChatModule.<ChatCommandUnbanAsync>d__76>(ref <ChatCommandUnbanAsync>d__);
		return <ChatCommandUnbanAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000CB1 RID: 3249 RVA: 0x00039A00 File Offset: 0x00037C00
	[Server]
	private bool ChatCommandInvisible(string command)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean ChatModule::ChatCommandInvisible(System.String)' called when server was not active");
			return default(bool);
		}
		if (this.attributeModule.AccessLevel < AccessLevel.CommunityManager)
		{
			return false;
		}
		Condition condition = new Condition(ConditionCategory.Invisibility, ConditionType.Invisible, 86400f, 5f, 0f, default(Effect), 0, 0f, "");
		this.conditionModule.StartCondition(condition, base.gameObject, true);
		return true;
	}

	// Token: 0x06000CB2 RID: 3250 RVA: 0x00039A80 File Offset: 0x00037C80
	[Server]
	private bool ChatCommandVisible(string command)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean ChatModule::ChatCommandVisible(System.String)' called when server was not active");
			return default(bool);
		}
		if (this.attributeModule.AccessLevel < AccessLevel.CommunityManager)
		{
			return false;
		}
		this.conditionModule.RemoveCondition(ConditionType.Invisible);
		return true;
	}

	// Token: 0x06000CB3 RID: 3251 RVA: 0x00039ACC File Offset: 0x00037CCC
	[Server]
	private bool ChatCommandGoTo(string command)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean ChatModule::ChatCommandGoTo(System.String)' called when server was not active");
			return default(bool);
		}
		if (this.attributeModule.AccessLevel < AccessLevel.CommunityManager)
		{
			return false;
		}
		string[] array = command.Substring(command.IndexOf(" ")).Trim().Split(new string[]
		{
			";"
		}, StringSplitOptions.RemoveEmptyEntries);
		if (array.Length == 1)
		{
			Vector3 locationFromSpawnPoint = GlobalUtils.GetLocationFromSpawnPoint(array[0]);
			if (locationFromSpawnPoint == Vector3.zero)
			{
				return false;
			}
			this.movementModule.TargetTeleport(base.connectionToClient, locationFromSpawnPoint, new Effect("TeleporterHit", 0.5f, 0.25f));
			return true;
		}
		else
		{
			if (array.Length == 2)
			{
				string s = array[0];
				string s2 = array[1];
				float x;
				float.TryParse(s, out x);
				float y;
				float.TryParse(s2, out y);
				Vector3 position = new Vector3(x, y);
				this.movementModule.TargetTeleport(base.connectionToClient, position, new Effect("TeleporterHit", 0.5f, 0.25f));
				return true;
			}
			return false;
		}
	}

	// Token: 0x06000CB4 RID: 3252 RVA: 0x00039BD0 File Offset: 0x00037DD0
	[Server]
	private bool ChatCommandPullPlayer(string command)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean ChatModule::ChatCommandPullPlayer(System.String)' called when server was not active");
			return default(bool);
		}
		if (this.attributeModule.AccessLevel < AccessLevel.CommunityManager)
		{
			return false;
		}
		string text = command.Substring(command.IndexOf(" ")).Trim();
		if (!string.IsNullOrEmpty(text))
		{
			ChatContact chatContact = this.gameEnvironmentModule.FindContact(text);
			if (chatContact.IsDefined & chatContact.Player != null)
			{
				MovementModule component = chatContact.Player.GetComponent<MovementModule>();
				component.TargetTeleport(component.connectionToClient, base.transform.position, new Effect("TeleporterHit", 0.5f, 0.25f));
			}
			else
			{
				this.effectModule.ShowScreenMessage("chat_player_not_online_message", 0, 3.5f, new string[]
				{
					text
				});
			}
			return true;
		}
		return false;
	}

	// Token: 0x06000CB5 RID: 3253 RVA: 0x00039CAC File Offset: 0x00037EAC
	[Server]
	private bool ChatCommandMonster(string command)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean ChatModule::ChatCommandMonster(System.String)' called when server was not active");
			return default(bool);
		}
		if (this.attributeModule.AccessLevel < AccessLevel.CommunityManager)
		{
			return false;
		}
		string[] array = command.Substring(command.IndexOf(" ")).Trim().Split(new string[]
		{
			";"
		}, StringSplitOptions.RemoveEmptyEntries);
		if (array.Length == 1)
		{
			string s = array[0];
			GlobalUtils.SpawnMonster(AssetBundleManager.Instance.MonsterPrefab, int.Parse(s), base.transform.position, Rank.Normal, false, false);
			return true;
		}
		if (array.Length == 2)
		{
			string s2 = array[0];
			string s3 = array[1];
			GlobalUtils.SpawnMonster(AssetBundleManager.Instance.MonsterPrefab, int.Parse(s2), base.transform.position, (Rank)int.Parse(s3), false, false);
			return true;
		}
		if (array.Length == 3)
		{
			string s4 = array[0];
			string s5 = array[1];
			string text = array[2];
			GlobalUtils.SpawnMonster(AssetBundleManager.Instance.MonsterPrefab, int.Parse(s4), base.transform.position, (Rank)int.Parse(s5), text.Equals("true", StringComparison.InvariantCultureIgnoreCase), false);
			return true;
		}
		return false;
	}

	// Token: 0x06000CB6 RID: 3254 RVA: 0x00039DD0 File Offset: 0x00037FD0
	[Server]
	private bool ChatCommandRunEffect(string command)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean ChatModule::ChatCommandRunEffect(System.String)' called when server was not active");
			return default(bool);
		}
		if (this.attributeModule.AccessLevel < AccessLevel.Administrator)
		{
			return false;
		}
		string[] array = command.Substring(command.IndexOf(" ")).Trim().Split(new string[]
		{
			";"
		}, StringSplitOptions.RemoveEmptyEntries);
		if (array.Length == 1)
		{
			this.effectModule.ShowEffects(new EffectConfig
			{
				EffectName = array[0],
				EffectScaleModifier = 1f,
				EffectSpeedModifier = 1f
			});
			return true;
		}
		if (array.Length == 2)
		{
			this.effectModule.ShowEffects(new EffectConfig
			{
				EffectName = array[0],
				EffectScaleModifier = float.Parse(array[1]),
				EffectSpeedModifier = 1f
			});
			return true;
		}
		if (array.Length == 3)
		{
			this.effectModule.ShowEffects(new EffectConfig
			{
				EffectName = array[0],
				EffectScaleModifier = float.Parse(array[1]),
				EffectSpeedModifier = float.Parse(array[2])
			});
			return true;
		}
		return false;
	}

	// Token: 0x06000CB7 RID: 3255 RVA: 0x00039F00 File Offset: 0x00038100
	[Server]
	private bool ChatRestartServer(string command)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean ChatModule::ChatRestartServer(System.String)' called when server was not active");
			return default(bool);
		}
		if (this.attributeModule.AccessLevel < AccessLevel.Administrator)
		{
			return false;
		}
		ServerSaveModule.ForceRestarting = true;
		return true;
	}

	// Token: 0x06000CB8 RID: 3256 RVA: 0x00039F48 File Offset: 0x00038148
	[Server]
	private bool ChatReleaseMemory(string command)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean ChatModule::ChatReleaseMemory(System.String)' called when server was not active");
			return default(bool);
		}
		if (this.attributeModule.AccessLevel < AccessLevel.Administrator)
		{
			return false;
		}
		Resources.UnloadUnusedAssets();
		return true;
	}

	// Token: 0x06000CB9 RID: 3257 RVA: 0x00039F90 File Offset: 0x00038190
	[Server]
	private bool ChatApplyDamage(string command)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean ChatModule::ChatApplyDamage(System.String)' called when server was not active");
			return default(bool);
		}
		if (this.attributeModule.AccessLevel < AccessLevel.Administrator)
		{
			return false;
		}
		if (this.combatModule.Target == null)
		{
			return true;
		}
		if (!this.combatModule.Target.CompareTag("Monster"))
		{
			return true;
		}
		CombatModule combatModule;
		this.combatModule.Target.TryGetComponent<CombatModule>(out combatModule);
		NonPlayerAttributeModule nonPlayerAttributeModule;
		this.combatModule.Target.TryGetComponent<NonPlayerAttributeModule>(out nonPlayerAttributeModule);
		int num = Mathf.RoundToInt((float)nonPlayerAttributeModule.MaxHealth * 0.9f);
		EffectConfig damageEffectConfig = new EffectConfig
		{
			EffectName = "DarkBlast",
			EffectScaleModifier = 0.7f,
			EffectSpeedModifier = 0.25f,
			SoundEffectName = "dark_blast",
			TextColorId = 7,
			Text = num.ToString()
		};
		combatModule.ApplyDamage(num, damageEffectConfig, null, false, Array.Empty<ConditionConfig>());
		nonPlayerAttributeModule.AddHealth(-num);
		return true;
	}

	// Token: 0x06000CBA RID: 3258 RVA: 0x0003A0A0 File Offset: 0x000382A0
	[Server]
	private bool ChatCommandKill(string command)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean ChatModule::ChatCommandKill(System.String)' called when server was not active");
			return default(bool);
		}
		if (this.attributeModule.AccessLevel < AccessLevel.Administrator)
		{
			return false;
		}
		if (this.combatModule.Target == null)
		{
			return true;
		}
		if (!this.combatModule.Target.CompareTag("Monster"))
		{
			return true;
		}
		CreatureModule creatureModule;
		this.combatModule.Target.TryGetComponent<CreatureModule>(out creatureModule);
		NonPlayerAttributeModule nonPlayerAttributeModule;
		this.combatModule.Target.TryGetComponent<NonPlayerAttributeModule>(out nonPlayerAttributeModule);
		EffectConfig effectConfig = default(EffectConfig);
		effectConfig.EffectName = "DarkBlast";
		effectConfig.EffectScaleModifier = 0.7f;
		effectConfig.EffectSpeedModifier = 0.25f;
		effectConfig.SoundEffectName = "dark_blast";
		effectConfig.TextColorId = 7;
		effectConfig.Text = nonPlayerAttributeModule.MaxHealth.ToString();
		creatureModule.Kill(base.gameObject, new List<Attacker>());
		return true;
	}

	// Token: 0x06000CBB RID: 3259 RVA: 0x0003A198 File Offset: 0x00038398
	[Server]
	private bool ChatCommandKillPlayer(string command)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean ChatModule::ChatCommandKillPlayer(System.String)' called when server was not active");
			return default(bool);
		}
		if (this.attributeModule.AccessLevel < AccessLevel.Administrator)
		{
			return false;
		}
		int num = command.IndexOf(" ");
		if (num >= command.Length)
		{
			return false;
		}
		string name = command.Substring(num).Trim();
		ChatContact chatContact = this.gameEnvironmentModule.FindContact(name);
		if (chatContact.Player == null)
		{
			return true;
		}
		AttributeModule component = chatContact.Player.GetComponent<AttributeModule>();
		CombatModule component2 = chatContact.Player.GetComponent<CombatModule>();
		EffectConfig damageEffectConfig = new EffectConfig
		{
			EffectName = "DarkBlast",
			EffectScaleModifier = 0.7f,
			EffectSpeedModifier = 0.25f,
			SoundEffectName = "dark_blast",
			TextColorId = 7,
			Text = component.MaxHealth.ToString()
		};
		component2.ApplyDamage(component.MaxHealth, damageEffectConfig, null, false, Array.Empty<ConditionConfig>());
		return true;
	}

	// Token: 0x06000CBC RID: 3260 RVA: 0x0003A29C File Offset: 0x0003849C
	[Server]
	private bool ChatCommandDisconnectAll(string command)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean ChatModule::ChatCommandDisconnectAll(System.String)' called when server was not active");
			return default(bool);
		}
		if (this.attributeModule.AccessLevel < AccessLevel.Administrator)
		{
			return false;
		}
		foreach (GameObject gameObject in (from cp in NetworkManagerModule.ConnectedPlayers
		select cp.Value).ToArray<GameObject>())
		{
			if (gameObject.GetComponent<AttributeModule>().AccessLevel == AccessLevel.Player)
			{
				gameObject.GetComponent<NetworkIdentity>().connectionToClient.Disconnect();
			}
		}
		return true;
	}

	// Token: 0x06000CBD RID: 3261 RVA: 0x0003A33C File Offset: 0x0003853C
	[Server]
	private bool ChatCommandNpc(string command)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean ChatModule::ChatCommandNpc(System.String)' called when server was not active");
			return default(bool);
		}
		if (this.attributeModule.AccessLevel < AccessLevel.Administrator)
		{
			return false;
		}
		string text = command.Substring(command.IndexOf(" ")).Trim();
		if (!string.IsNullOrEmpty(text))
		{
			NetworkServer.Spawn(GlobalUtils.SpawnNpc(AssetBundleManager.Instance.NpcPrefab, this.npcDatabaseModule, int.Parse(text), base.transform.position), null);
			return true;
		}
		return false;
	}

	// Token: 0x06000CBE RID: 3262 RVA: 0x0003A3C8 File Offset: 0x000385C8
	[Server]
	private bool ChatCommandItem(string command)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean ChatModule::ChatCommandItem(System.String)' called when server was not active");
			return default(bool);
		}
		if (this.attributeModule.AccessLevel < AccessLevel.Administrator)
		{
			return false;
		}
		string[] array = command.Substring(command.IndexOf(" ")).Trim().Split(new string[]
		{
			";"
		}, StringSplitOptions.RemoveEmptyEntries);
		if (array.Length == 3)
		{
			int itemId = int.Parse(array[0]);
			Rarity rarity = (Rarity)int.Parse(array[1]);
			int requiredLevel = int.Parse(array[2]);
			Item item = this.itemDatabaseModule.GetItem(itemId);
			item.Amount = 1;
			item.Rarity = rarity;
			item.RequiredLevel = requiredLevel;
			this.inventoryModule.AddItem(item, true);
			return true;
		}
		if (array.Length == 4)
		{
			int itemId2 = int.Parse(array[0]);
			Rarity rarity2 = (Rarity)int.Parse(array[1]);
			int requiredLevel2 = int.Parse(array[2]);
			int num = int.Parse(array[3]);
			Item item2 = this.itemDatabaseModule.GetItem(itemId2);
			num = ((num < 1) ? 1 : num);
			num = ((num > 750) ? 750 : num);
			item2.Rarity = rarity2;
			item2.Amount = num;
			item2.RequiredLevel = requiredLevel2;
			this.inventoryModule.AddItem(item2, true);
			return true;
		}
		return false;
	}

	// Token: 0x06000CBF RID: 3263 RVA: 0x0003A514 File Offset: 0x00038714
	[Server]
	private bool ChatCommandCheckpoint()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean ChatModule::ChatCommandCheckpoint()' called when server was not active");
			return default(bool);
		}
		if (this.attributeModule.AccessLevel < AccessLevel.Administrator)
		{
			return false;
		}
		NetworkManagerModule.CreateCheckpointLog();
		return true;
	}

	// Token: 0x06000CC0 RID: 3264 RVA: 0x0003A558 File Offset: 0x00038758
	[Server]
	private bool ChatCommandClearCache()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Boolean ChatModule::ChatCommandClearCache()' called when server was not active");
			return default(bool);
		}
		if (this.attributeModule.AccessLevel < AccessLevel.Administrator)
		{
			return false;
		}
		ClassFactory.Clear();
		ObjectPoolModule.Instance.ClearServerPools();
		Debug.Log("Cache and Pool Cleanup.");
		return true;
	}

	// Token: 0x06000CC1 RID: 3265 RVA: 0x0003A5B0 File Offset: 0x000387B0
	[Command(channel = 1)]
	public void CmdSetShowInfoOnChat(bool value)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteBool(value);
		base.SendCommandInternal("System.Void ChatModule::CmdSetShowInfoOnChat(System.Boolean)", -637878998, writer, 1, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000CC2 RID: 3266 RVA: 0x0003A5EC File Offset: 0x000387EC
	public void SendSystemTranslatedMessage(string content, string colorTag, bool hideFromChatOverlay, params string[] args)
	{
		if (!base.isServer)
		{
			content = LanguageManager.Instance.GetText(content);
			if (args != null && args.Length != 0)
			{
				List<string> list = new List<string>();
				foreach (string key in args)
				{
					string text = LanguageManager.Instance.GetText(key);
					list.Add(text);
				}
				string format = content;
				object[] args2 = list.ToArray();
				content = string.Format(format, args2);
			}
			if (!string.IsNullOrEmpty(colorTag))
			{
				content = string.Format("<color={0}>{1}</color>", colorTag, content);
			}
			ChatMessage message = new ChatMessage("chat_tab_system", null, null, (double)DateTime.Now.Ticks, content, false, true, false, false, null, hideFromChatOverlay);
			this.uiSystemModule.PlatformChatHolderManager.AddMessageToChannel(message, true);
			return;
		}
		if (!this.ShowInfoOnChat)
		{
			return;
		}
		this.TargetSendSystemTranslatedMessage(base.connectionToClient, content, colorTag, hideFromChatOverlay, args);
	}

	// Token: 0x06000CC3 RID: 3267 RVA: 0x0003A6C4 File Offset: 0x000388C4
	[TargetRpc(channel = 1)]
	private void TargetSendSystemTranslatedMessage(NetworkConnection target, string content, string colorTag, bool hideFromChatOverlay, params string[] args)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(content);
		writer.WriteString(colorTag);
		writer.WriteBool(hideFromChatOverlay);
		Mirror.GeneratedNetworkCode._Write_System.String[](writer, args);
		this.SendTargetRPCInternal(target, "System.Void ChatModule::TargetSendSystemTranslatedMessage(Mirror.NetworkConnection,System.String,System.String,System.Boolean,System.String[])", -1303418694, writer, 1);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000CC4 RID: 3268 RVA: 0x0003A71C File Offset: 0x0003891C
	[TargetRpc(channel = 1)]
	public void TargetSendWelcomeMessage(NetworkConnection target)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		this.SendTargetRPCInternal(target, "System.Void ChatModule::TargetSendWelcomeMessage(Mirror.NetworkConnection)", -1329182774, writer, 1);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000CC5 RID: 3269 RVA: 0x0003A74C File Offset: 0x0003894C
	[TargetRpc(channel = 1)]
	private void TargetSendSystemApplyDamageMessage(NetworkConnection target, string creatureName, bool creatureAllowRankNamePrefix, Rank creatureRank, int damage)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(creatureName);
		writer.WriteBool(creatureAllowRankNamePrefix);
		Mirror.GeneratedNetworkCode._Write_Rank(writer, creatureRank);
		writer.WriteInt(damage);
		this.SendTargetRPCInternal(target, "System.Void ChatModule::TargetSendSystemApplyDamageMessage(Mirror.NetworkConnection,System.String,System.Boolean,Rank,System.Int32)", 1865819392, writer, 1);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000CC6 RID: 3270 RVA: 0x0003A7A4 File Offset: 0x000389A4
	[Server]
	public void SendSystemApplyDamageMessage(string creatureName, bool creatureAllowRankNamePrefix, Rank creatureRank, int damage)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ChatModule::SendSystemApplyDamageMessage(System.String,System.Boolean,Rank,System.Int32)' called when server was not active");
			return;
		}
		if (!this.ShowInfoOnChat)
		{
			return;
		}
		this.TargetSendSystemApplyDamageMessage(base.connectionToClient, creatureName, creatureAllowRankNamePrefix, creatureRank, damage);
	}

	// Token: 0x06000CC7 RID: 3271 RVA: 0x0003A7D8 File Offset: 0x000389D8
	[TargetRpc(channel = 1)]
	private void TargetSendSystemReceiveDamageMessage(NetworkConnection target, string creatureName, bool creatureAllowRankNamePrefix, Rank creatureRank, int damage)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(creatureName);
		writer.WriteBool(creatureAllowRankNamePrefix);
		Mirror.GeneratedNetworkCode._Write_Rank(writer, creatureRank);
		writer.WriteInt(damage);
		this.SendTargetRPCInternal(target, "System.Void ChatModule::TargetSendSystemReceiveDamageMessage(Mirror.NetworkConnection,System.String,System.Boolean,Rank,System.Int32)", 1304310453, writer, 1);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000CC8 RID: 3272 RVA: 0x0003A830 File Offset: 0x00038A30
	[Server]
	public void SendSystemReceiveDamageMessage(string creatureName, bool creatureAllowRankNamePrefix, Rank creatureRank, int damage)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ChatModule::SendSystemReceiveDamageMessage(System.String,System.Boolean,Rank,System.Int32)' called when server was not active");
			return;
		}
		if (!this.ShowInfoOnChat)
		{
			return;
		}
		this.TargetSendSystemReceiveDamageMessage(base.connectionToClient, creatureName, creatureAllowRankNamePrefix, creatureRank, damage);
	}

	// Token: 0x06000CC9 RID: 3273 RVA: 0x0003A864 File Offset: 0x00038A64
	[TargetRpc(channel = 1)]
	public void TargetSendSystemQuestUpdateKillMessage(NetworkConnection target, string playerQuestName, int left, string creatureRankName, string creatureName, string creaturePluralName)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(playerQuestName);
		writer.WriteInt(left);
		writer.WriteString(creatureRankName);
		writer.WriteString(creatureName);
		writer.WriteString(creaturePluralName);
		this.SendTargetRPCInternal(target, "System.Void ChatModule::TargetSendSystemQuestUpdateKillMessage(Mirror.NetworkConnection,System.String,System.Int32,System.String,System.String,System.String)", -589166339, writer, 1);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000CCA RID: 3274 RVA: 0x0003A8C8 File Offset: 0x00038AC8
	[TargetRpc(channel = 1)]
	public void TargetSendSystemQuestUpdateCollectMessage(NetworkConnection target, string playerQuestName, int left, string itemName, string itemPluralName)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(playerQuestName);
		writer.WriteInt(left);
		writer.WriteString(itemName);
		writer.WriteString(itemPluralName);
		this.SendTargetRPCInternal(target, "System.Void ChatModule::TargetSendSystemQuestUpdateCollectMessage(Mirror.NetworkConnection,System.String,System.Int32,System.String,System.String)", 1421985911, writer, 1);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000CCB RID: 3275 RVA: 0x0003A920 File Offset: 0x00038B20
	[TargetRpc(channel = 1)]
	public void TargetSendSystemQuestReadyMessage(NetworkConnection target)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		this.SendTargetRPCInternal(target, "System.Void ChatModule::TargetSendSystemQuestReadyMessage(Mirror.NetworkConnection)", -1153631270, writer, 1);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000CCC RID: 3276 RVA: 0x0003A950 File Offset: 0x00038B50
	[Server]
	public Task LoadFriendListAsync(int accountId)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Threading.Tasks.Task ChatModule::LoadFriendListAsync(System.Int32)' called when server was not active");
			return null;
		}
		ChatModule.<LoadFriendListAsync>d__104 <LoadFriendListAsync>d__;
		<LoadFriendListAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<LoadFriendListAsync>d__.<>4__this = this;
		<LoadFriendListAsync>d__.accountId = accountId;
		<LoadFriendListAsync>d__.<>1__state = -1;
		<LoadFriendListAsync>d__.<>t__builder.Start<ChatModule.<LoadFriendListAsync>d__104>(ref <LoadFriendListAsync>d__);
		return <LoadFriendListAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000CCD RID: 3277 RVA: 0x0003A9BC File Offset: 0x00038BBC
	[Server]
	public void CheckIfFriendIsOnlineTimer()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ChatModule::CheckIfFriendIsOnlineTimer()' called when server was not active");
			return;
		}
		for (int i = 0; i < this.gameEnvironmentModule.Contacts.Count; i++)
		{
			if (this.IsFriend(this.gameEnvironmentModule.Contacts[i].Name))
			{
				this.SetOnline(this.gameEnvironmentModule.Contacts[i].Name, true);
			}
		}
	}

	// Token: 0x06000CCE RID: 3278 RVA: 0x0003AA34 File Offset: 0x00038C34
	[Server]
	public void SetOnline(string friendName, bool isOnline)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ChatModule::SetOnline(System.String,System.Boolean)' called when server was not active");
			return;
		}
		int i = 0;
		while (i < this.Friends.Count)
		{
			if (string.Equals(this.Friends[i].FriendName, friendName, StringComparison.OrdinalIgnoreCase))
			{
				AccountFriend accountFriend = this.Friends[i];
				int isOnline2 = accountFriend.IsOnline ? 1 : 0;
				accountFriend.IsOnline = isOnline;
				this.Friends[i] = accountFriend;
				if (isOnline2 == 0 && isOnline && this.OnFriendBecomesOnline != null)
				{
					this.OnFriendBecomesOnline(accountFriend);
					return;
				}
				break;
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x06000CCF RID: 3279 RVA: 0x0003AACC File Offset: 0x00038CCC
	[Command(channel = 1)]
	public void CmdAddFriend(string friendName)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(friendName);
		base.SendCommandInternal("System.Void ChatModule::CmdAddFriend(System.String)", 1882612468, writer, 1, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000CD0 RID: 3280 RVA: 0x0003AB08 File Offset: 0x00038D08
	[Server]
	private Task AddFriendAsync(string friendName)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Threading.Tasks.Task ChatModule::AddFriendAsync(System.String)' called when server was not active");
			return null;
		}
		ChatModule.<AddFriendAsync>d__108 <AddFriendAsync>d__;
		<AddFriendAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<AddFriendAsync>d__.<>4__this = this;
		<AddFriendAsync>d__.friendName = friendName;
		<AddFriendAsync>d__.<>1__state = -1;
		<AddFriendAsync>d__.<>t__builder.Start<ChatModule.<AddFriendAsync>d__108>(ref <AddFriendAsync>d__);
		return <AddFriendAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000CD1 RID: 3281 RVA: 0x0003AB74 File Offset: 0x00038D74
	[Command(channel = 1)]
	public void CmdRemoveFriend(string friendName)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(friendName);
		base.SendCommandInternal("System.Void ChatModule::CmdRemoveFriend(System.String)", 984043021, writer, 1, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000CD2 RID: 3282 RVA: 0x0003ABB0 File Offset: 0x00038DB0
	[Command(channel = 1)]
	public void CmdMuteFriend(string friendName)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(friendName);
		base.SendCommandInternal("System.Void ChatModule::CmdMuteFriend(System.String)", 1916409624, writer, 1, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000CD3 RID: 3283 RVA: 0x0003ABEC File Offset: 0x00038DEC
	[Command(channel = 1)]
	public void CmdUnMuteFriend(string friendName)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(friendName);
		base.SendCommandInternal("System.Void ChatModule::CmdUnMuteFriend(System.String)", -992283905, writer, 1, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000CD4 RID: 3284 RVA: 0x0003AC28 File Offset: 0x00038E28
	[Server]
	private void RemoveFriend(string friendName)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ChatModule::RemoveFriend(System.String)' called when server was not active");
			return;
		}
		bool flag = false;
		for (int i = 0; i < this.Friends.Count; i++)
		{
			if (string.Equals(this.Friends[i].FriendName, friendName, StringComparison.OrdinalIgnoreCase))
			{
				AccountFriend friend = this.Friends[i];
				this.Friends.RemoveAt(i);
				ChatModule.OnFriendRemovedEventHandler onFriendRemoved = this.OnFriendRemoved;
				if (onFriendRemoved != null)
				{
					onFriendRemoved(friend);
				}
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			this.effectModule.ShowScreenMessage("chat_player_not_friend_message", 0, 3.5f, new string[]
			{
				friendName
			});
		}
	}

	// Token: 0x06000CD5 RID: 3285 RVA: 0x0003ACD0 File Offset: 0x00038ED0
	public bool IsFriend(string friendName)
	{
		for (int i = 0; i < this.Friends.Count; i++)
		{
			if (string.Equals(this.Friends[i].FriendName, friendName, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000CD6 RID: 3286 RVA: 0x0003AD10 File Offset: 0x00038F10
	public ChatModule()
	{
		base.InitSyncObject(this.Friends);
	}

	// Token: 0x06000CD7 RID: 3287 RVA: 0x0002FEC9 File Offset: 0x0002E0C9
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x06000CD8 RID: 3288 RVA: 0x0003AD3C File Offset: 0x00038F3C
	protected void UserCode_TargetSendChatMessage__ChatMessage(ChatMessage message)
	{
		if (message.IsBroadcast & !string.IsNullOrEmpty(message.SenderDisplay))
		{
			this.effectModule.ShowScreenMessage(string.Format("[{0}] {1}", message.SenderDisplay, message.Content), 2, 7f, Array.Empty<string>());
			return;
		}
		if (message.IsBroadcast)
		{
			this.effectModule.ShowScreenMessage(message.Content, 2, 7f, Array.Empty<string>());
			return;
		}
		if (this.Friends.Any((AccountFriend f) => f.FriendName == message.SenderName & f.Muted))
		{
			return;
		}
		ChatChannel chatChannel = this.uiSystemModule.PlatformChatHolderManager.FindActiveChannel();
		bool isActive = chatChannel != null && chatChannel.Name == message.Channel;
		this.uiSystemModule.PlatformChatHolderManager.AddMessageToChannel(message, isActive);
		if (message.IsPrivate && (chatChannel == null || chatChannel.Name != message.Channel))
		{
			this.effectModule.ShowScreenMessage(message.Channel + ": " + message.Content, 6, 7f, Array.Empty<string>());
		}
	}

	// Token: 0x06000CD9 RID: 3289 RVA: 0x0003AE9A File Offset: 0x0003909A
	protected static void InvokeUserCode_TargetSendChatMessage__ChatMessage(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetSendChatMessage called on server.");
			return;
		}
		((ChatModule)obj).UserCode_TargetSendChatMessage__ChatMessage(Mirror.GeneratedNetworkCode._Read_ChatMessage(reader));
	}

	// Token: 0x06000CDA RID: 3290 RVA: 0x0003AEC4 File Offset: 0x000390C4
	protected void UserCode_CmdSendChatMessage__String__Boolean__Boolean__String(string channelName, bool isChannelGlobal, bool isChannelParty, string content)
	{
		ChatModule.<CmdSendChatMessage>d__39 <CmdSendChatMessage>d__;
		<CmdSendChatMessage>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CmdSendChatMessage>d__.<>4__this = this;
		<CmdSendChatMessage>d__.channelName = channelName;
		<CmdSendChatMessage>d__.isChannelGlobal = isChannelGlobal;
		<CmdSendChatMessage>d__.isChannelParty = isChannelParty;
		<CmdSendChatMessage>d__.content = content;
		<CmdSendChatMessage>d__.<>1__state = -1;
		<CmdSendChatMessage>d__.<>t__builder.Start<ChatModule.<CmdSendChatMessage>d__39>(ref <CmdSendChatMessage>d__);
	}

	// Token: 0x06000CDB RID: 3291 RVA: 0x0003AF1C File Offset: 0x0003911C
	protected static void InvokeUserCode_CmdSendChatMessage__String__Boolean__Boolean__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSendChatMessage called on client.");
			return;
		}
		((ChatModule)obj).UserCode_CmdSendChatMessage__String__Boolean__Boolean__String(reader.ReadString(), reader.ReadBool(), reader.ReadBool(), reader.ReadString());
	}

	// Token: 0x06000CDC RID: 3292 RVA: 0x0003AF57 File Offset: 0x00039157
	protected void UserCode_TargetShowHelpWindow()
	{
		this.uiSystemModule.ShowHelpWindow();
	}

	// Token: 0x06000CDD RID: 3293 RVA: 0x0003AF64 File Offset: 0x00039164
	protected static void InvokeUserCode_TargetShowHelpWindow(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetShowHelpWindow called on server.");
			return;
		}
		((ChatModule)obj).UserCode_TargetShowHelpWindow();
	}

	// Token: 0x06000CDE RID: 3294 RVA: 0x0003AF87 File Offset: 0x00039187
	protected void UserCode_CmdSetShowInfoOnChat__Boolean(bool value)
	{
		this.ShowInfoOnChat = value;
	}

	// Token: 0x06000CDF RID: 3295 RVA: 0x0003AF90 File Offset: 0x00039190
	protected static void InvokeUserCode_CmdSetShowInfoOnChat__Boolean(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdSetShowInfoOnChat called on client.");
			return;
		}
		((ChatModule)obj).UserCode_CmdSetShowInfoOnChat__Boolean(reader.ReadBool());
	}

	// Token: 0x06000CE0 RID: 3296 RVA: 0x0003AFB9 File Offset: 0x000391B9
	protected void UserCode_TargetSendSystemTranslatedMessage__NetworkConnection__String__String__Boolean__String[](NetworkConnection target, string content, string colorTag, bool hideFromChatOverlay, string[] args)
	{
		this.SendSystemTranslatedMessage(content, colorTag, hideFromChatOverlay, args);
	}

	// Token: 0x06000CE1 RID: 3297 RVA: 0x0003AFC7 File Offset: 0x000391C7
	protected static void InvokeUserCode_TargetSendSystemTranslatedMessage__NetworkConnection__String__String__Boolean__String[](NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetSendSystemTranslatedMessage called on server.");
			return;
		}
		((ChatModule)obj).UserCode_TargetSendSystemTranslatedMessage__NetworkConnection__String__String__Boolean__String[](null, reader.ReadString(), reader.ReadString(), reader.ReadBool(), Mirror.GeneratedNetworkCode._Read_System.String[](reader));
	}

	// Token: 0x06000CE2 RID: 3298 RVA: 0x0003B004 File Offset: 0x00039204
	protected void UserCode_TargetSendWelcomeMessage__NetworkConnection(NetworkConnection target)
	{
		string text = LanguageManager.Instance.GetText("welcome_message");
		ChatMessage message = new ChatMessage("chat_tab_default", null, null, (double)DateTime.Now.Ticks, text, false, true, false, false, null, false);
		this.uiSystemModule.PlatformChatHolderManager.AddMessageToChannel(message, true);
	}

	// Token: 0x06000CE3 RID: 3299 RVA: 0x0003B056 File Offset: 0x00039256
	protected static void InvokeUserCode_TargetSendWelcomeMessage__NetworkConnection(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetSendWelcomeMessage called on server.");
			return;
		}
		((ChatModule)obj).UserCode_TargetSendWelcomeMessage__NetworkConnection(null);
	}

	// Token: 0x06000CE4 RID: 3300 RVA: 0x0003B07C File Offset: 0x0003927C
	protected void UserCode_TargetSendSystemApplyDamageMessage__NetworkConnection__String__Boolean__Rank__Int32(NetworkConnection target, string creatureName, bool creatureAllowRankNamePrefix, Rank creatureRank, int damage)
	{
		creatureName = LanguageManager.Instance.GetText(creatureName);
		string text = LanguageManager.Instance.GetText(GlobalUtils.RankToString(creatureRank));
		if (creatureAllowRankNamePrefix & !string.IsNullOrEmpty(text))
		{
			creatureName = string.Format("{0} ({1})", creatureName, text);
		}
		string text2 = LanguageManager.Instance.GetText("damage_dealt_message");
		text2 = string.Format(text2, damage, LanguageManager.Instance.GetText(creatureName));
		ChatMessage message = new ChatMessage("chat_tab_system", null, null, (double)DateTime.Now.Ticks, text2, false, true, false, false, null, true);
		this.uiSystemModule.PlatformChatHolderManager.AddMessageToChannel(message, true);
	}

	// Token: 0x06000CE5 RID: 3301 RVA: 0x0003B121 File Offset: 0x00039321
	protected static void InvokeUserCode_TargetSendSystemApplyDamageMessage__NetworkConnection__String__Boolean__Rank__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetSendSystemApplyDamageMessage called on server.");
			return;
		}
		((ChatModule)obj).UserCode_TargetSendSystemApplyDamageMessage__NetworkConnection__String__Boolean__Rank__Int32(null, reader.ReadString(), reader.ReadBool(), Mirror.GeneratedNetworkCode._Read_Rank(reader), reader.ReadInt());
	}

	// Token: 0x06000CE6 RID: 3302 RVA: 0x0003B160 File Offset: 0x00039360
	protected void UserCode_TargetSendSystemReceiveDamageMessage__NetworkConnection__String__Boolean__Rank__Int32(NetworkConnection target, string creatureName, bool creatureAllowRankNamePrefix, Rank creatureRank, int damage)
	{
		creatureName = LanguageManager.Instance.GetText(creatureName);
		string text = LanguageManager.Instance.GetText(GlobalUtils.RankToString(creatureRank));
		if (creatureAllowRankNamePrefix & !string.IsNullOrEmpty(text))
		{
			creatureName = string.Format("{0} ({1})", creatureName, text);
		}
		string text2 = LanguageManager.Instance.GetText("damage_received_message");
		text2 = string.Format(text2, LanguageManager.Instance.GetText(creatureName), damage);
		ChatMessage message = new ChatMessage("chat_tab_system", null, null, (double)DateTime.Now.Ticks, text2, false, true, false, false, null, true);
		this.uiSystemModule.PlatformChatHolderManager.AddMessageToChannel(message, true);
	}

	// Token: 0x06000CE7 RID: 3303 RVA: 0x0003B205 File Offset: 0x00039405
	protected static void InvokeUserCode_TargetSendSystemReceiveDamageMessage__NetworkConnection__String__Boolean__Rank__Int32(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetSendSystemReceiveDamageMessage called on server.");
			return;
		}
		((ChatModule)obj).UserCode_TargetSendSystemReceiveDamageMessage__NetworkConnection__String__Boolean__Rank__Int32(null, reader.ReadString(), reader.ReadBool(), Mirror.GeneratedNetworkCode._Read_Rank(reader), reader.ReadInt());
	}

	// Token: 0x06000CE8 RID: 3304 RVA: 0x0003B244 File Offset: 0x00039444
	protected void UserCode_TargetSendSystemQuestUpdateKillMessage__NetworkConnection__String__Int32__String__String__String(NetworkConnection target, string playerQuestName, int left, string creatureRankName, string creatureName, string creaturePluralName)
	{
		playerQuestName = LanguageManager.Instance.GetText(playerQuestName);
		creatureName = LanguageManager.Instance.GetText(creatureName);
		creaturePluralName = LanguageManager.Instance.GetText(creaturePluralName);
		creatureRankName = LanguageManager.Instance.GetText(creatureRankName);
		creatureRankName = (string.IsNullOrEmpty(creatureRankName) ? "" : (" (" + creatureRankName + ")"));
		string text;
		if (left < 1)
		{
			text = string.Format(LanguageManager.Instance.GetText("quest_update_kill_ready_message"), playerQuestName, creaturePluralName, creatureRankName);
		}
		else
		{
			creatureName = ((left > 1) ? creaturePluralName : creatureName);
			text = string.Format(LanguageManager.Instance.GetText("quest_update_kill_message"), new object[]
			{
				playerQuestName,
				left,
				creatureName,
				creatureRankName
			});
		}
		text = string.Format("<color=#6897BB>{0}</color>", text);
		ChatMessage chatMessage = new ChatMessage("chat_tab_system", null, null, (double)DateTime.Now.Ticks, text, false, true, false, false, null, false);
		if (GlobalSettings.IsMobilePlatform)
		{
			this.effectModule.ShowScreenMessage(chatMessage.Content, 0, 3.5f, Array.Empty<string>());
			return;
		}
		this.uiSystemModule.PlatformChatHolderManager.AddMessageToChannel(chatMessage, true);
	}

	// Token: 0x06000CE9 RID: 3305 RVA: 0x0003B370 File Offset: 0x00039570
	protected static void InvokeUserCode_TargetSendSystemQuestUpdateKillMessage__NetworkConnection__String__Int32__String__String__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetSendSystemQuestUpdateKillMessage called on server.");
			return;
		}
		((ChatModule)obj).UserCode_TargetSendSystemQuestUpdateKillMessage__NetworkConnection__String__Int32__String__String__String(null, reader.ReadString(), reader.ReadInt(), reader.ReadString(), reader.ReadString(), reader.ReadString());
	}

	// Token: 0x06000CEA RID: 3306 RVA: 0x0003B3C0 File Offset: 0x000395C0
	protected void UserCode_TargetSendSystemQuestUpdateCollectMessage__NetworkConnection__String__Int32__String__String(NetworkConnection target, string playerQuestName, int left, string itemName, string itemPluralName)
	{
		playerQuestName = LanguageManager.Instance.GetText(playerQuestName);
		itemName = LanguageManager.Instance.GetText(itemName);
		itemPluralName = LanguageManager.Instance.GetText(itemPluralName);
		string text;
		if (left < 1)
		{
			text = string.Format(LanguageManager.Instance.GetText("quest_update_collect_ready_message"), playerQuestName, itemPluralName);
		}
		else
		{
			itemName = ((left > 1) ? itemPluralName : itemName);
			text = string.Format(LanguageManager.Instance.GetText("quest_update_collect_message"), playerQuestName, left, itemName);
		}
		text = string.Format("<color=#6897BB>{0}</color>", text);
		ChatMessage chatMessage = new ChatMessage("chat_tab_system", null, null, (double)DateTime.Now.Ticks, text, false, true, false, false, null, false);
		if (GlobalSettings.IsMobilePlatform)
		{
			this.effectModule.ShowScreenMessage(chatMessage.Content, 0, 3.5f, Array.Empty<string>());
			return;
		}
		this.uiSystemModule.PlatformChatHolderManager.AddMessageToChannel(chatMessage, true);
	}

	// Token: 0x06000CEB RID: 3307 RVA: 0x0003B4A3 File Offset: 0x000396A3
	protected static void InvokeUserCode_TargetSendSystemQuestUpdateCollectMessage__NetworkConnection__String__Int32__String__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetSendSystemQuestUpdateCollectMessage called on server.");
			return;
		}
		((ChatModule)obj).UserCode_TargetSendSystemQuestUpdateCollectMessage__NetworkConnection__String__Int32__String__String(null, reader.ReadString(), reader.ReadInt(), reader.ReadString(), reader.ReadString());
	}

	// Token: 0x06000CEC RID: 3308 RVA: 0x0003B4E0 File Offset: 0x000396E0
	protected void UserCode_TargetSendSystemQuestReadyMessage__NetworkConnection(NetworkConnection target)
	{
		string text = LanguageManager.Instance.GetText("quest_all_ready_message");
		text = string.Format("<color=#007F3C>{0}</color>", text);
		ChatMessage chatMessage = new ChatMessage("chat_tab_system", null, null, (double)DateTime.Now.Ticks, text, false, true, false, false, null, false);
		if (GlobalSettings.IsMobilePlatform)
		{
			this.effectModule.ShowScreenMessage(chatMessage.Content, 0, 3.5f, Array.Empty<string>());
			return;
		}
		this.uiSystemModule.PlatformChatHolderManager.AddMessageToChannel(chatMessage, true);
	}

	// Token: 0x06000CED RID: 3309 RVA: 0x0003B562 File Offset: 0x00039762
	protected static void InvokeUserCode_TargetSendSystemQuestReadyMessage__NetworkConnection(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetSendSystemQuestReadyMessage called on server.");
			return;
		}
		((ChatModule)obj).UserCode_TargetSendSystemQuestReadyMessage__NetworkConnection(null);
	}

	// Token: 0x06000CEE RID: 3310 RVA: 0x0003B588 File Offset: 0x00039788
	protected void UserCode_CmdAddFriend__String(string friendName)
	{
		ChatModule.<CmdAddFriend>d__107 <CmdAddFriend>d__;
		<CmdAddFriend>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CmdAddFriend>d__.<>4__this = this;
		<CmdAddFriend>d__.friendName = friendName;
		<CmdAddFriend>d__.<>1__state = -1;
		<CmdAddFriend>d__.<>t__builder.Start<ChatModule.<CmdAddFriend>d__107>(ref <CmdAddFriend>d__);
	}

	// Token: 0x06000CEF RID: 3311 RVA: 0x0003B5C7 File Offset: 0x000397C7
	protected static void InvokeUserCode_CmdAddFriend__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdAddFriend called on client.");
			return;
		}
		((ChatModule)obj).UserCode_CmdAddFriend__String(reader.ReadString());
	}

	// Token: 0x06000CF0 RID: 3312 RVA: 0x0003B5F0 File Offset: 0x000397F0
	protected void UserCode_CmdRemoveFriend__String(string friendName)
	{
		this.RemoveFriend(friendName);
	}

	// Token: 0x06000CF1 RID: 3313 RVA: 0x0003B5F9 File Offset: 0x000397F9
	protected static void InvokeUserCode_CmdRemoveFriend__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdRemoveFriend called on client.");
			return;
		}
		((ChatModule)obj).UserCode_CmdRemoveFriend__String(reader.ReadString());
	}

	// Token: 0x06000CF2 RID: 3314 RVA: 0x0003B624 File Offset: 0x00039824
	protected void UserCode_CmdMuteFriend__String(string friendName)
	{
		ChatModule.<CmdMuteFriend>d__110 <CmdMuteFriend>d__;
		<CmdMuteFriend>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CmdMuteFriend>d__.<>4__this = this;
		<CmdMuteFriend>d__.friendName = friendName;
		<CmdMuteFriend>d__.<>1__state = -1;
		<CmdMuteFriend>d__.<>t__builder.Start<ChatModule.<CmdMuteFriend>d__110>(ref <CmdMuteFriend>d__);
	}

	// Token: 0x06000CF3 RID: 3315 RVA: 0x0003B663 File Offset: 0x00039863
	protected static void InvokeUserCode_CmdMuteFriend__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdMuteFriend called on client.");
			return;
		}
		((ChatModule)obj).UserCode_CmdMuteFriend__String(reader.ReadString());
	}

	// Token: 0x06000CF4 RID: 3316 RVA: 0x0003B68C File Offset: 0x0003988C
	protected void UserCode_CmdUnMuteFriend__String(string friendName)
	{
		ChatModule.<CmdUnMuteFriend>d__111 <CmdUnMuteFriend>d__;
		<CmdUnMuteFriend>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<CmdUnMuteFriend>d__.<>4__this = this;
		<CmdUnMuteFriend>d__.friendName = friendName;
		<CmdUnMuteFriend>d__.<>1__state = -1;
		<CmdUnMuteFriend>d__.<>t__builder.Start<ChatModule.<CmdUnMuteFriend>d__111>(ref <CmdUnMuteFriend>d__);
	}

	// Token: 0x06000CF5 RID: 3317 RVA: 0x0003B6CB File Offset: 0x000398CB
	protected static void InvokeUserCode_CmdUnMuteFriend__String(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdUnMuteFriend called on client.");
			return;
		}
		((ChatModule)obj).UserCode_CmdUnMuteFriend__String(reader.ReadString());
	}

	// Token: 0x06000CF6 RID: 3318 RVA: 0x0003B6F4 File Offset: 0x000398F4
	static ChatModule()
	{
		RemoteProcedureCalls.RegisterCommand(typeof(ChatModule), "System.Void ChatModule::CmdSendChatMessage(System.String,System.Boolean,System.Boolean,System.String)", new RemoteCallDelegate(ChatModule.InvokeUserCode_CmdSendChatMessage__String__Boolean__Boolean__String), true);
		RemoteProcedureCalls.RegisterCommand(typeof(ChatModule), "System.Void ChatModule::CmdSetShowInfoOnChat(System.Boolean)", new RemoteCallDelegate(ChatModule.InvokeUserCode_CmdSetShowInfoOnChat__Boolean), true);
		RemoteProcedureCalls.RegisterCommand(typeof(ChatModule), "System.Void ChatModule::CmdAddFriend(System.String)", new RemoteCallDelegate(ChatModule.InvokeUserCode_CmdAddFriend__String), true);
		RemoteProcedureCalls.RegisterCommand(typeof(ChatModule), "System.Void ChatModule::CmdRemoveFriend(System.String)", new RemoteCallDelegate(ChatModule.InvokeUserCode_CmdRemoveFriend__String), true);
		RemoteProcedureCalls.RegisterCommand(typeof(ChatModule), "System.Void ChatModule::CmdMuteFriend(System.String)", new RemoteCallDelegate(ChatModule.InvokeUserCode_CmdMuteFriend__String), true);
		RemoteProcedureCalls.RegisterCommand(typeof(ChatModule), "System.Void ChatModule::CmdUnMuteFriend(System.String)", new RemoteCallDelegate(ChatModule.InvokeUserCode_CmdUnMuteFriend__String), true);
		RemoteProcedureCalls.RegisterRpc(typeof(ChatModule), "System.Void ChatModule::TargetSendChatMessage(ChatMessage)", new RemoteCallDelegate(ChatModule.InvokeUserCode_TargetSendChatMessage__ChatMessage));
		RemoteProcedureCalls.RegisterRpc(typeof(ChatModule), "System.Void ChatModule::TargetShowHelpWindow()", new RemoteCallDelegate(ChatModule.InvokeUserCode_TargetShowHelpWindow));
		RemoteProcedureCalls.RegisterRpc(typeof(ChatModule), "System.Void ChatModule::TargetSendSystemTranslatedMessage(Mirror.NetworkConnection,System.String,System.String,System.Boolean,System.String[])", new RemoteCallDelegate(ChatModule.InvokeUserCode_TargetSendSystemTranslatedMessage__NetworkConnection__String__String__Boolean__String[]));
		RemoteProcedureCalls.RegisterRpc(typeof(ChatModule), "System.Void ChatModule::TargetSendWelcomeMessage(Mirror.NetworkConnection)", new RemoteCallDelegate(ChatModule.InvokeUserCode_TargetSendWelcomeMessage__NetworkConnection));
		RemoteProcedureCalls.RegisterRpc(typeof(ChatModule), "System.Void ChatModule::TargetSendSystemApplyDamageMessage(Mirror.NetworkConnection,System.String,System.Boolean,Rank,System.Int32)", new RemoteCallDelegate(ChatModule.InvokeUserCode_TargetSendSystemApplyDamageMessage__NetworkConnection__String__Boolean__Rank__Int32));
		RemoteProcedureCalls.RegisterRpc(typeof(ChatModule), "System.Void ChatModule::TargetSendSystemReceiveDamageMessage(Mirror.NetworkConnection,System.String,System.Boolean,Rank,System.Int32)", new RemoteCallDelegate(ChatModule.InvokeUserCode_TargetSendSystemReceiveDamageMessage__NetworkConnection__String__Boolean__Rank__Int32));
		RemoteProcedureCalls.RegisterRpc(typeof(ChatModule), "System.Void ChatModule::TargetSendSystemQuestUpdateKillMessage(Mirror.NetworkConnection,System.String,System.Int32,System.String,System.String,System.String)", new RemoteCallDelegate(ChatModule.InvokeUserCode_TargetSendSystemQuestUpdateKillMessage__NetworkConnection__String__Int32__String__String__String));
		RemoteProcedureCalls.RegisterRpc(typeof(ChatModule), "System.Void ChatModule::TargetSendSystemQuestUpdateCollectMessage(Mirror.NetworkConnection,System.String,System.Int32,System.String,System.String)", new RemoteCallDelegate(ChatModule.InvokeUserCode_TargetSendSystemQuestUpdateCollectMessage__NetworkConnection__String__Int32__String__String));
		RemoteProcedureCalls.RegisterRpc(typeof(ChatModule), "System.Void ChatModule::TargetSendSystemQuestReadyMessage(Mirror.NetworkConnection)", new RemoteCallDelegate(ChatModule.InvokeUserCode_TargetSendSystemQuestReadyMessage__NetworkConnection));
	}

	// Token: 0x04000D63 RID: 3427
	public DateTime MuteEndDate;

	// Token: 0x04000D64 RID: 3428
	private PvpModule pvpModule;

	// Token: 0x04000D65 RID: 3429
	private TradeModule tradeModule;

	// Token: 0x04000D66 RID: 3430
	private PartyModule partyModule;

	// Token: 0x04000D67 RID: 3431
	private WalletModule walletModule;

	// Token: 0x04000D68 RID: 3432
	private CombatModule combatModule;

	// Token: 0x04000D69 RID: 3433
	private EffectModule effectModule;

	// Token: 0x04000D6A RID: 3434
	private PlayerModule playerModule;

	// Token: 0x04000D6B RID: 3435
	private CreatureModule creatureModule;

	// Token: 0x04000D6C RID: 3436
	private MovementModule movementModule;

	// Token: 0x04000D6D RID: 3437
	private UISystemModule uiSystemModule;

	// Token: 0x04000D6E RID: 3438
	private ConditionModule conditionModule;

	// Token: 0x04000D6F RID: 3439
	private InventoryModule inventoryModule;

	// Token: 0x04000D70 RID: 3440
	private AttributeModule attributeModule;

	// Token: 0x04000D71 RID: 3441
	private NpcDatabaseModule npcDatabaseModule;

	// Token: 0x04000D72 RID: 3442
	private ItemDatabaseModule itemDatabaseModule;

	// Token: 0x04000D73 RID: 3443
	private GameEnvironmentModule gameEnvironmentModule;

	// Token: 0x04000D74 RID: 3444
	private Collider2D[] localChatColliders;

	// Token: 0x04000D75 RID: 3445
	public bool ShowInfoOnChat;

	// Token: 0x04000D76 RID: 3446
	public readonly SyncListAccountFriend Friends = new SyncListAccountFriend();

	// Token: 0x04000D7A RID: 3450
	private Dictionary<int, float> accountCommandTimes = new Dictionary<int, float>();

	// Token: 0x020002D0 RID: 720
	// (Invoke) Token: 0x06000CF8 RID: 3320
	public delegate void OnFriendAddedEventHandler(AccountFriend friend);

	// Token: 0x020002D1 RID: 721
	// (Invoke) Token: 0x06000CFC RID: 3324
	public delegate void OnFriendRemovedEventHandler(AccountFriend friend);

	// Token: 0x020002D2 RID: 722
	// (Invoke) Token: 0x06000D00 RID: 3328
	public delegate void OnFriendBecomesOnlineEventHandler(AccountFriend friend);
}
