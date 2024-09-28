using System;
using System.Collections;
using System.Linq;
using Mirror;
using Mirror.RemoteCalls;
using UnityEngine;

// Token: 0x02000321 RID: 801
public class EffectModule : NetworkBehaviour
{
	// Token: 0x06000F49 RID: 3913 RVA: 0x00047D74 File Offset: 0x00045F74
	private void Awake()
	{
		if (NetworkClient.active)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
			this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
		}
		this.creatureModule = base.GetComponent<CreatureModule>();
		this.conditionModule = base.GetComponent<ConditionModule>();
	}

	// Token: 0x06000F4A RID: 3914 RVA: 0x00047DB8 File Offset: 0x00045FB8
	[ClientRpc(channel = 1)]
	private void RpcShowEffects(EffectConfig effectConfig)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		Mirror.GeneratedNetworkCode._Write_EffectConfig(writer, effectConfig);
		this.SendRPCInternal("System.Void EffectModule::RpcShowEffects(EffectConfig)", -1311247761, writer, 1, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000F4B RID: 3915 RVA: 0x00047DF4 File Offset: 0x00045FF4
	[TargetRpc(channel = 1)]
	public void TargetShowEffects(EffectConfig effectConfig)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		Mirror.GeneratedNetworkCode._Write_EffectConfig(writer, effectConfig);
		this.SendTargetRPCInternal(null, "System.Void EffectModule::TargetShowEffects(EffectConfig)", -618044895, writer, 1);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000F4C RID: 3916 RVA: 0x00047E30 File Offset: 0x00046030
	public void ShowEffects(EffectConfig effectConfig)
	{
		if (!effectConfig.IsDefined)
		{
			return;
		}
		if (NetworkClient.active && SettingsManager.Instance.DisableVisualEffects)
		{
			return;
		}
		if (NetworkServer.active)
		{
			if (!this.CanCallRpc())
			{
				return;
			}
			this.RpcShowEffects(effectConfig);
			return;
		}
		else
		{
			if (!base.isLocalPlayer && this.conditionModule != null && this.conditionModule.HasActiveCondition(ConditionCategory.Invisibility))
			{
				return;
			}
			if (this.creatureModule != null)
			{
				float num = GlobalUtils.RankToPercentScale(this.creatureModule.Rank);
				effectConfig.EffectScaleModifier += effectConfig.EffectScaleModifier * num;
			}
			this.ShowVisualEffect(effectConfig.EffectName, effectConfig.EffectScaleModifier, effectConfig.EffectSpeedModifier, effectConfig.EffectLoopDuration, effectConfig.Position);
			this.PlaySoundEffect(effectConfig.SoundEffectName, 1f, effectConfig.EffectLoopDuration, effectConfig.Position);
			this.ShowAnimatedText(effectConfig.Text, effectConfig.TextColorId, true, effectConfig.Position);
			return;
		}
	}

	// Token: 0x06000F4D RID: 3917 RVA: 0x00047F24 File Offset: 0x00046124
	[ClientRpc(channel = 1)]
	private void RpcShowEffectsRandomly(int amount, float range, float interval, EffectConfig effectConfig)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteInt(amount);
		writer.WriteFloat(range);
		writer.WriteFloat(interval);
		Mirror.GeneratedNetworkCode._Write_EffectConfig(writer, effectConfig);
		this.SendRPCInternal("System.Void EffectModule::RpcShowEffectsRandomly(System.Int32,System.Single,System.Single,EffectConfig)", -1281452858, writer, 1, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000F4E RID: 3918 RVA: 0x00047F7C File Offset: 0x0004617C
	public void ShowEffectsRandomly(int amount, float range, float interval, EffectConfig effectConfig)
	{
		if (!effectConfig.IsDefined)
		{
			return;
		}
		if (!NetworkServer.active)
		{
			base.StartCoroutine(this.StartEffectsRandomly(amount, range, interval, effectConfig));
			return;
		}
		if (!this.CanCallRpc())
		{
			return;
		}
		this.RpcShowEffectsRandomly(amount, range, interval, effectConfig);
	}

	// Token: 0x06000F4F RID: 3919 RVA: 0x00047FB6 File Offset: 0x000461B6
	private IEnumerator StartEffectsRandomly(int amount, float range, float interval, EffectConfig effectConfig)
	{
		WaitForSeconds delay = new WaitForSeconds(interval);
		Vector3 centerPosition = effectConfig.Position;
		int num;
		for (int i = 0; i < amount; i = num + 1)
		{
			float radius = UnityEngine.Random.Range(0f, range);
			effectConfig.Position = GlobalUtils.RandomCircle(centerPosition, radius);
			this.ShowEffects(effectConfig);
			yield return delay;
			num = i;
		}
		yield break;
	}

	// Token: 0x06000F50 RID: 3920 RVA: 0x00047FE4 File Offset: 0x000461E4
	[ClientRpc(channel = 1)]
	private void RpcShowVisualEffect(string name, float scaleModifier, float speedModifier, float loopDuration, Vector3 position)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(name);
		writer.WriteFloat(scaleModifier);
		writer.WriteFloat(speedModifier);
		writer.WriteFloat(loopDuration);
		writer.WriteVector3(position);
		this.SendRPCInternal("System.Void EffectModule::RpcShowVisualEffect(System.String,System.Single,System.Single,System.Single,UnityEngine.Vector3)", 828283761, writer, 1, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000F51 RID: 3921 RVA: 0x00048048 File Offset: 0x00046248
	public void ShowVisualEffect(string name, float scaleModifier, float speedModifier, float loopDuration, Vector3 position)
	{
		if (string.IsNullOrEmpty(name))
		{
			return;
		}
		if (!NetworkServer.active)
		{
			bool flag = position == Vector3.zero;
			GameObject visualEffectFromPool = ObjectPoolModule.Instance.GetVisualEffectFromPool(position);
			if (Application.isEditor)
			{
				visualEffectFromPool.name = name;
			}
			if (flag)
			{
				visualEffectFromPool.transform.SetParent(base.transform, false);
			}
			EffectRenderer effectRenderer;
			if (visualEffectFromPool.TryGetComponent<EffectRenderer>(out effectRenderer))
			{
				effectRenderer.RunAnimation(name, scaleModifier, speedModifier, loopDuration);
			}
			return;
		}
		if (!this.CanCallRpc())
		{
			return;
		}
		this.RpcShowVisualEffect(name, scaleModifier, speedModifier, loopDuration, position);
	}

	// Token: 0x06000F52 RID: 3922 RVA: 0x000480CC File Offset: 0x000462CC
	[ClientRpc(channel = 1)]
	private void RpcPlaySoundEffect(string name, float volume, float loopDuration, Vector3 position)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(name);
		writer.WriteFloat(volume);
		writer.WriteFloat(loopDuration);
		writer.WriteVector3(position);
		this.SendRPCInternal("System.Void EffectModule::RpcPlaySoundEffect(System.String,System.Single,System.Single,UnityEngine.Vector3)", -1086387530, writer, 1, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000F53 RID: 3923 RVA: 0x00048124 File Offset: 0x00046324
	public void PlaySoundEffect(string name, float volume, float loopDuration, Vector3 position)
	{
		if (string.IsNullOrEmpty(name))
		{
			return;
		}
		if (NetworkServer.active)
		{
			if (!this.CanCallRpc())
			{
				return;
			}
			this.RpcPlaySoundEffect(name, volume, loopDuration, position);
			return;
		}
		else
		{
			if (SettingsManager.Instance.Mute || volume == 0f)
			{
				return;
			}
			volume *= SettingsManager.Instance.SfxVolume;
			if (!string.IsNullOrEmpty(name))
			{
				bool flag = position == Vector3.zero;
				GameObject soundEffectFromPool = ObjectPoolModule.Instance.GetSoundEffectFromPool(position);
				if (Application.isEditor)
				{
					soundEffectFromPool.name = name;
				}
				if (flag)
				{
					soundEffectFromPool.transform.SetParent(base.transform, false);
				}
				SoundEffectPlayer component = soundEffectFromPool.GetComponent<SoundEffectPlayer>();
				if (loopDuration == 0f)
				{
					component.PlayOneShot(name, volume);
					return;
				}
				component.PlayInLoop(name, volume, loopDuration);
			}
			return;
		}
	}

	// Token: 0x06000F54 RID: 3924 RVA: 0x000481E0 File Offset: 0x000463E0
	[TargetRpc(channel = 1)]
	public void TargetPlaySoundEffect(string name, float volume, float loopDuration, Vector3 position)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(name);
		writer.WriteFloat(volume);
		writer.WriteFloat(loopDuration);
		writer.WriteVector3(position);
		this.SendTargetRPCInternal(null, "System.Void EffectModule::TargetPlaySoundEffect(System.String,System.Single,System.Single,UnityEngine.Vector3)", 1502807620, writer, 1);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000F55 RID: 3925 RVA: 0x00048238 File Offset: 0x00046438
	[ClientRpc(channel = 1)]
	private void RpcShowAnimatedText(string text, int colorId, bool animate, Vector3 position)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(text);
		writer.WriteInt(colorId);
		writer.WriteBool(animate);
		writer.WriteVector3(position);
		this.SendRPCInternal("System.Void EffectModule::RpcShowAnimatedText(System.String,System.Int32,System.Boolean,UnityEngine.Vector3)", 533238569, writer, 1, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000F56 RID: 3926 RVA: 0x00048290 File Offset: 0x00046490
	[TargetRpc(channel = 1)]
	public void TargetShowAnimatedText(string text, int colorId, bool animate, Vector3 position)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(text);
		writer.WriteInt(colorId);
		writer.WriteBool(animate);
		writer.WriteVector3(position);
		this.SendTargetRPCInternal(null, "System.Void EffectModule::TargetShowAnimatedText(System.String,System.Int32,System.Boolean,UnityEngine.Vector3)", -806090405, writer, 1);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000F57 RID: 3927 RVA: 0x000482E8 File Offset: 0x000464E8
	public void ShowAnimatedText(string text, int colorId, bool animate, Vector3 position)
	{
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		if (!NetworkServer.active)
		{
			text = LanguageManager.Instance.GetText(text);
			if (position == Vector3.zero)
			{
				position = base.transform.position;
			}
			AnimatedTextRenderer component = ObjectPoolModule.Instance.GetAnimatedTextFromPool(position).GetComponent<AnimatedTextRenderer>();
			if (component != null && GlobalSettings.Colors.ContainsKey(colorId))
			{
				component.StartRunning(GlobalSettings.Colors[colorId], text, animate);
			}
			return;
		}
		if (!this.CanCallRpc())
		{
			return;
		}
		this.RpcShowAnimatedText(text, colorId, animate, position);
	}

	// Token: 0x06000F58 RID: 3928 RVA: 0x00048380 File Offset: 0x00046580
	[TargetRpc(channel = 1)]
	private void TargetShowScreenMessage(NetworkConnection target, string text, int colorId, float duration, params string[] args)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(text);
		writer.WriteInt(colorId);
		writer.WriteFloat(duration);
		Mirror.GeneratedNetworkCode._Write_System.String[](writer, args);
		this.SendTargetRPCInternal(target, "System.Void EffectModule::TargetShowScreenMessage(Mirror.NetworkConnection,System.String,System.Int32,System.Single,System.String[])", 1169202474, writer, 1);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000F59 RID: 3929 RVA: 0x000483D8 File Offset: 0x000465D8
	[ClientRpc(channel = 1)]
	public void RpcShowScreenMessage(string text, int colorId, float duration, params string[] args)
	{
		NetworkWriterPooled writer = NetworkWriterPool.Get();
		writer.WriteString(text);
		writer.WriteInt(colorId);
		writer.WriteFloat(duration);
		Mirror.GeneratedNetworkCode._Write_System.String[](writer, args);
		this.SendRPCInternal("System.Void EffectModule::RpcShowScreenMessage(System.String,System.Int32,System.Single,System.String[])", -2044283921, writer, 1, true);
		NetworkWriterPool.Return(writer);
	}

	// Token: 0x06000F5A RID: 3930 RVA: 0x00048430 File Offset: 0x00046630
	public void ShowScreenMessage(string text, int colorId, float duration, params string[] args)
	{
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		if (!NetworkServer.active)
		{
			text = LanguageManager.Instance.GetText(text);
			args = (from arg in args
			select LanguageManager.Instance.GetText(arg)).ToArray<string>();
			if (args != null && args.Length != 0)
			{
				string format = text;
				object[] args2 = args;
				text = string.Format(format, args2);
			}
			this.uiSystemModule.ShowMessageText(text, colorId, duration);
			if (SettingsManager.Instance.ShowInfoOnChat)
			{
				string colorTag = GlobalUtils.ColorIdToColorTag(colorId);
				if (this.uiSystemModule.ChatModule != null)
				{
					this.uiSystemModule.ChatModule.SendSystemTranslatedMessage(text, colorTag, false, Array.Empty<string>());
				}
			}
			return;
		}
		if (!this.CanCallRpc())
		{
			return;
		}
		this.TargetShowScreenMessage(base.connectionToClient, text, colorId, duration, args);
	}

	// Token: 0x06000F5B RID: 3931 RVA: 0x00048504 File Offset: 0x00046704
	private bool CanCallRpc()
	{
		if (base.CompareTag("Player"))
		{
			NetworkConnectionToClient connectionToClient = base.connectionToClient;
			return connectionToClient != null && connectionToClient.isReady;
		}
		return !(base.CompareTag("Monster") | base.CompareTag("DeadMonster") | base.CompareTag("Combatant")) || (base.isClient | base.isServer);
	}

	// Token: 0x06000F5D RID: 3933 RVA: 0x0002FEC9 File Offset: 0x0002E0C9
	public override bool Weaved()
	{
		return true;
	}

	// Token: 0x06000F5E RID: 3934 RVA: 0x00048564 File Offset: 0x00046764
	protected void UserCode_RpcShowEffects__EffectConfig(EffectConfig effectConfig)
	{
		this.ShowEffects(effectConfig);
	}

	// Token: 0x06000F5F RID: 3935 RVA: 0x0004856D File Offset: 0x0004676D
	protected static void InvokeUserCode_RpcShowEffects__EffectConfig(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcShowEffects called on server.");
			return;
		}
		((EffectModule)obj).UserCode_RpcShowEffects__EffectConfig(Mirror.GeneratedNetworkCode._Read_EffectConfig(reader));
	}

	// Token: 0x06000F60 RID: 3936 RVA: 0x00048564 File Offset: 0x00046764
	protected void UserCode_TargetShowEffects__EffectConfig(EffectConfig effectConfig)
	{
		this.ShowEffects(effectConfig);
	}

	// Token: 0x06000F61 RID: 3937 RVA: 0x00048596 File Offset: 0x00046796
	protected static void InvokeUserCode_TargetShowEffects__EffectConfig(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetShowEffects called on server.");
			return;
		}
		((EffectModule)obj).UserCode_TargetShowEffects__EffectConfig(Mirror.GeneratedNetworkCode._Read_EffectConfig(reader));
	}

	// Token: 0x06000F62 RID: 3938 RVA: 0x000485BF File Offset: 0x000467BF
	protected void UserCode_RpcShowEffectsRandomly__Int32__Single__Single__EffectConfig(int amount, float range, float interval, EffectConfig effectConfig)
	{
		this.ShowEffectsRandomly(amount, range, interval, effectConfig);
	}

	// Token: 0x06000F63 RID: 3939 RVA: 0x000485CC File Offset: 0x000467CC
	protected static void InvokeUserCode_RpcShowEffectsRandomly__Int32__Single__Single__EffectConfig(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcShowEffectsRandomly called on server.");
			return;
		}
		((EffectModule)obj).UserCode_RpcShowEffectsRandomly__Int32__Single__Single__EffectConfig(reader.ReadInt(), reader.ReadFloat(), reader.ReadFloat(), Mirror.GeneratedNetworkCode._Read_EffectConfig(reader));
	}

	// Token: 0x06000F64 RID: 3940 RVA: 0x00048609 File Offset: 0x00046809
	protected void UserCode_RpcShowVisualEffect__String__Single__Single__Single__Vector3(string name, float scaleModifier, float speedModifier, float loopDuration, Vector3 position)
	{
		this.ShowVisualEffect(name, scaleModifier, speedModifier, loopDuration, position);
	}

	// Token: 0x06000F65 RID: 3941 RVA: 0x00048618 File Offset: 0x00046818
	protected static void InvokeUserCode_RpcShowVisualEffect__String__Single__Single__Single__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcShowVisualEffect called on server.");
			return;
		}
		((EffectModule)obj).UserCode_RpcShowVisualEffect__String__Single__Single__Single__Vector3(reader.ReadString(), reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat(), reader.ReadVector3());
	}

	// Token: 0x06000F66 RID: 3942 RVA: 0x00048667 File Offset: 0x00046867
	protected void UserCode_RpcPlaySoundEffect__String__Single__Single__Vector3(string name, float volume, float loopDuration, Vector3 position)
	{
		this.PlaySoundEffect(name, volume, loopDuration, position);
	}

	// Token: 0x06000F67 RID: 3943 RVA: 0x00048674 File Offset: 0x00046874
	protected static void InvokeUserCode_RpcPlaySoundEffect__String__Single__Single__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcPlaySoundEffect called on server.");
			return;
		}
		((EffectModule)obj).UserCode_RpcPlaySoundEffect__String__Single__Single__Vector3(reader.ReadString(), reader.ReadFloat(), reader.ReadFloat(), reader.ReadVector3());
	}

	// Token: 0x06000F68 RID: 3944 RVA: 0x00048667 File Offset: 0x00046867
	protected void UserCode_TargetPlaySoundEffect__String__Single__Single__Vector3(string name, float volume, float loopDuration, Vector3 position)
	{
		this.PlaySoundEffect(name, volume, loopDuration, position);
	}

	// Token: 0x06000F69 RID: 3945 RVA: 0x000486B1 File Offset: 0x000468B1
	protected static void InvokeUserCode_TargetPlaySoundEffect__String__Single__Single__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetPlaySoundEffect called on server.");
			return;
		}
		((EffectModule)obj).UserCode_TargetPlaySoundEffect__String__Single__Single__Vector3(reader.ReadString(), reader.ReadFloat(), reader.ReadFloat(), reader.ReadVector3());
	}

	// Token: 0x06000F6A RID: 3946 RVA: 0x000486EE File Offset: 0x000468EE
	protected void UserCode_RpcShowAnimatedText__String__Int32__Boolean__Vector3(string text, int colorId, bool animate, Vector3 position)
	{
		this.ShowAnimatedText(text, colorId, animate, position);
	}

	// Token: 0x06000F6B RID: 3947 RVA: 0x000486FB File Offset: 0x000468FB
	protected static void InvokeUserCode_RpcShowAnimatedText__String__Int32__Boolean__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcShowAnimatedText called on server.");
			return;
		}
		((EffectModule)obj).UserCode_RpcShowAnimatedText__String__Int32__Boolean__Vector3(reader.ReadString(), reader.ReadInt(), reader.ReadBool(), reader.ReadVector3());
	}

	// Token: 0x06000F6C RID: 3948 RVA: 0x000486EE File Offset: 0x000468EE
	protected void UserCode_TargetShowAnimatedText__String__Int32__Boolean__Vector3(string text, int colorId, bool animate, Vector3 position)
	{
		this.ShowAnimatedText(text, colorId, animate, position);
	}

	// Token: 0x06000F6D RID: 3949 RVA: 0x00048736 File Offset: 0x00046936
	protected static void InvokeUserCode_TargetShowAnimatedText__String__Int32__Boolean__Vector3(NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetShowAnimatedText called on server.");
			return;
		}
		((EffectModule)obj).UserCode_TargetShowAnimatedText__String__Int32__Boolean__Vector3(reader.ReadString(), reader.ReadInt(), reader.ReadBool(), reader.ReadVector3());
	}

	// Token: 0x06000F6E RID: 3950 RVA: 0x00048771 File Offset: 0x00046971
	protected void UserCode_TargetShowScreenMessage__NetworkConnection__String__Int32__Single__String[](NetworkConnection target, string text, int colorId, float duration, string[] args)
	{
		this.ShowScreenMessage(text, colorId, duration, args);
	}

	// Token: 0x06000F6F RID: 3951 RVA: 0x0004877F File Offset: 0x0004697F
	protected static void InvokeUserCode_TargetShowScreenMessage__NetworkConnection__String__Int32__Single__String[](NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("TargetRPC TargetShowScreenMessage called on server.");
			return;
		}
		((EffectModule)obj).UserCode_TargetShowScreenMessage__NetworkConnection__String__Int32__Single__String[](null, reader.ReadString(), reader.ReadInt(), reader.ReadFloat(), Mirror.GeneratedNetworkCode._Read_System.String[](reader));
	}

	// Token: 0x06000F70 RID: 3952 RVA: 0x000487BC File Offset: 0x000469BC
	protected void UserCode_RpcShowScreenMessage__String__Int32__Single__String[](string text, int colorId, float duration, string[] args)
	{
		this.ShowScreenMessage(text, colorId, duration, args);
	}

	// Token: 0x06000F71 RID: 3953 RVA: 0x000487C9 File Offset: 0x000469C9
	protected static void InvokeUserCode_RpcShowScreenMessage__String__Int32__Single__String[](NetworkBehaviour obj, NetworkReader reader, NetworkConnectionToClient senderConnection)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcShowScreenMessage called on server.");
			return;
		}
		((EffectModule)obj).UserCode_RpcShowScreenMessage__String__Int32__Single__String[](reader.ReadString(), reader.ReadInt(), reader.ReadFloat(), Mirror.GeneratedNetworkCode._Read_System.String[](reader));
	}

	// Token: 0x06000F72 RID: 3954 RVA: 0x00048808 File Offset: 0x00046A08
	static EffectModule()
	{
		RemoteProcedureCalls.RegisterRpc(typeof(EffectModule), "System.Void EffectModule::RpcShowEffects(EffectConfig)", new RemoteCallDelegate(EffectModule.InvokeUserCode_RpcShowEffects__EffectConfig));
		RemoteProcedureCalls.RegisterRpc(typeof(EffectModule), "System.Void EffectModule::RpcShowEffectsRandomly(System.Int32,System.Single,System.Single,EffectConfig)", new RemoteCallDelegate(EffectModule.InvokeUserCode_RpcShowEffectsRandomly__Int32__Single__Single__EffectConfig));
		RemoteProcedureCalls.RegisterRpc(typeof(EffectModule), "System.Void EffectModule::RpcShowVisualEffect(System.String,System.Single,System.Single,System.Single,UnityEngine.Vector3)", new RemoteCallDelegate(EffectModule.InvokeUserCode_RpcShowVisualEffect__String__Single__Single__Single__Vector3));
		RemoteProcedureCalls.RegisterRpc(typeof(EffectModule), "System.Void EffectModule::RpcPlaySoundEffect(System.String,System.Single,System.Single,UnityEngine.Vector3)", new RemoteCallDelegate(EffectModule.InvokeUserCode_RpcPlaySoundEffect__String__Single__Single__Vector3));
		RemoteProcedureCalls.RegisterRpc(typeof(EffectModule), "System.Void EffectModule::RpcShowAnimatedText(System.String,System.Int32,System.Boolean,UnityEngine.Vector3)", new RemoteCallDelegate(EffectModule.InvokeUserCode_RpcShowAnimatedText__String__Int32__Boolean__Vector3));
		RemoteProcedureCalls.RegisterRpc(typeof(EffectModule), "System.Void EffectModule::RpcShowScreenMessage(System.String,System.Int32,System.Single,System.String[])", new RemoteCallDelegate(EffectModule.InvokeUserCode_RpcShowScreenMessage__String__Int32__Single__String[]));
		RemoteProcedureCalls.RegisterRpc(typeof(EffectModule), "System.Void EffectModule::TargetShowEffects(EffectConfig)", new RemoteCallDelegate(EffectModule.InvokeUserCode_TargetShowEffects__EffectConfig));
		RemoteProcedureCalls.RegisterRpc(typeof(EffectModule), "System.Void EffectModule::TargetPlaySoundEffect(System.String,System.Single,System.Single,UnityEngine.Vector3)", new RemoteCallDelegate(EffectModule.InvokeUserCode_TargetPlaySoundEffect__String__Single__Single__Vector3));
		RemoteProcedureCalls.RegisterRpc(typeof(EffectModule), "System.Void EffectModule::TargetShowAnimatedText(System.String,System.Int32,System.Boolean,UnityEngine.Vector3)", new RemoteCallDelegate(EffectModule.InvokeUserCode_TargetShowAnimatedText__String__Int32__Boolean__Vector3));
		RemoteProcedureCalls.RegisterRpc(typeof(EffectModule), "System.Void EffectModule::TargetShowScreenMessage(Mirror.NetworkConnection,System.String,System.Int32,System.Single,System.String[])", new RemoteCallDelegate(EffectModule.InvokeUserCode_TargetShowScreenMessage__NetworkConnection__String__Int32__Single__String[]));
	}

	// Token: 0x04000F1D RID: 3869
	private CreatureModule creatureModule;

	// Token: 0x04000F1E RID: 3870
	private UISystemModule uiSystemModule;

	// Token: 0x04000F1F RID: 3871
	private ConditionModule conditionModule;
}
