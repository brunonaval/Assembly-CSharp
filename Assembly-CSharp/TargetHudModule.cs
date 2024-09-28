using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200042C RID: 1068
public class TargetHudModule : MonoBehaviour
{
	// Token: 0x06001746 RID: 5958 RVA: 0x000767F8 File Offset: 0x000749F8
	private void Update()
	{
		if (Time.time - this.updateTime > this.updateInterval)
		{
			this.updateTime = Time.time;
			if (this.selectedTarget == null)
			{
				base.gameObject.SetActive(false);
				return;
			}
			if (this.targetAttributeModule != null)
			{
				this.UpdatePlayerInfo();
				return;
			}
			if (this.nonPlayerTargetAttributeModule != null)
			{
				this.UpdateNonPlayerInfo();
			}
		}
	}

	// Token: 0x06001747 RID: 5959 RVA: 0x00076868 File Offset: 0x00074A68
	private void UpdateNonPlayerInfo()
	{
		this.partyButton.SetActive(false);
		this.messageButton.SetActive(false);
		this.tradeButton.SetActive(false);
		this.guildButton.SetActive(false);
		if (!this.targetCreatureModule.IsAlive)
		{
			this.uiSystemModule.HideTargetHud();
			return;
		}
		float num = (float)this.nonPlayerTargetAttributeModule.CurrentHealth * 100f / (float)this.nonPlayerTargetAttributeModule.MaxHealth;
		this.healthBar.fillAmount = num / 100f;
		this.enduranceBar.fillAmount = 1f;
		this.healthText.text = string.Format("{0}/{1}", this.nonPlayerTargetAttributeModule.CurrentHealth, this.nonPlayerTargetAttributeModule.MaxHealth);
		this.enduranceText.text = string.Empty;
	}

	// Token: 0x06001748 RID: 5960 RVA: 0x00076948 File Offset: 0x00074B48
	private void UpdatePlayerInfo()
	{
		this.partyButton.SetActive(true);
		this.messageButton.SetActive(true);
		this.tradeButton.SetActive(true);
		if (this.targetGuildModule.ActiveGuildId == 0 && this.uiSystemModule.GuildModule.MemberRank != GuildMemberRank.Member)
		{
			this.guildButton.SetActive(true);
		}
		this.levelText.text = this.targetAttributeModule.BaseLevel.ToString();
		float num = (float)this.targetAttributeModule.CurrentHealth * 100f / (float)this.targetAttributeModule.MaxHealth;
		this.healthBar.fillAmount = num / 100f;
		float num2 = (float)this.targetAttributeModule.CurrentHealth * 100f / (float)this.targetAttributeModule.MaxEndurance;
		this.enduranceBar.fillAmount = num2 / 100f;
		this.healthText.text = string.Format("{0}/{1}", this.targetAttributeModule.CurrentHealth, this.targetAttributeModule.MaxHealth);
		this.enduranceText.text = string.Format("{0}/{1}", this.targetAttributeModule.CurrentEndurance, this.targetAttributeModule.MaxEndurance);
	}

	// Token: 0x06001749 RID: 5961 RVA: 0x00076A90 File Offset: 0x00074C90
	public void SetTarget(GameObject target)
	{
		if (target == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		this.selectedTarget = target;
		target.TryGetComponent<GuildModule>(out this.targetGuildModule);
		target.TryGetComponent<CreatureModule>(out this.targetCreatureModule);
		ConditionModule conditionModule;
		target.TryGetComponent<ConditionModule>(out conditionModule);
		conditionModule.BuildTargetConditions();
		if (target.CompareTag("Player"))
		{
			this.SetPlayerInfo(target);
			this.UpdatePlayerInfo();
			return;
		}
		this.SetNonPlayerInfo(target);
		this.UpdateNonPlayerInfo();
	}

	// Token: 0x0600174A RID: 5962 RVA: 0x00076B0C File Offset: 0x00074D0C
	private void SetNonPlayerInfo(GameObject target)
	{
		this.skullObject.SetActive(true);
		this.levelText.gameObject.SetActive(false);
		this.targetAttributeModule = null;
		this.nonPlayerTargetAttributeModule = target.GetComponent<NonPlayerAttributeModule>();
		Rank rank = this.targetCreatureModule.Rank;
		string text = rank.ToString().ToLower();
		this.avatarImage.sprite = AssetBundleManager.Instance.GetRankIconSprite(text);
		this.targetNameText.text = LanguageManager.Instance.GetText(this.targetCreatureModule.CreatureName);
		if (rank > Rank.None & rank != Rank.Normal)
		{
			Text text2 = this.targetNameText;
			text2.text = text2.text + " - " + LanguageManager.Instance.GetText("rank_" + text);
		}
	}

	// Token: 0x0600174B RID: 5963 RVA: 0x00076BDC File Offset: 0x00074DDC
	private void SetPlayerInfo(GameObject target)
	{
		this.nonPlayerTargetAttributeModule = null;
		this.targetAttributeModule = target.GetComponent<AttributeModule>();
		this.skullObject.SetActive(false);
		this.levelText.gameObject.SetActive(true);
		Vocation vocation = target.GetComponent<VocationModule>().Vocation;
		this.avatarImage.sprite = ResourcesManager.Instance.GetVocationIconSprite(vocation.ToString().ToLower());
		this.targetNameText.text = this.targetCreatureModule.CreatureName;
	}

	// Token: 0x0600174C RID: 5964 RVA: 0x00076C62 File Offset: 0x00074E62
	public void PartyButtonClicked()
	{
		this.uiSystemModule.PartyModule.CmdBeginInviteOnTarget();
	}

	// Token: 0x0600174D RID: 5965 RVA: 0x00076C74 File Offset: 0x00074E74
	public void GuildButtonClicked()
	{
		this.uiSystemModule.GuildModule.CmdBeginInviteOnTarget();
	}

	// Token: 0x0600174E RID: 5966 RVA: 0x00076C88 File Offset: 0x00074E88
	public void MessageButtonClicked()
	{
		if (this.selectedTarget == null)
		{
			return;
		}
		CreatureModule creatureModule;
		if (!this.selectedTarget.TryGetComponent<CreatureModule>(out creatureModule))
		{
			return;
		}
		if (GlobalSettings.IsMobilePlatform)
		{
			this.uiSystemModule.OpenMobileChatHolder();
		}
		this.uiSystemModule.PlayerModule.ChatFocused = true;
		this.uiSystemModule.PlatformChatHolderManager.OpenChannel(creatureModule.OriginalCreatureName, true, false, false, false);
		this.uiSystemModule.PlatformChatHolderManager.SetSendMessageInputFocus(true, null);
	}

	// Token: 0x0600174F RID: 5967 RVA: 0x00076D04 File Offset: 0x00074F04
	public void TradeButtonClicked()
	{
		if (this.selectedTarget == null)
		{
			return;
		}
		this.uiSystemModule.TradeModule.CmdBeginHandshake();
	}

	// Token: 0x040014B8 RID: 5304
	[SerializeField]
	private Text targetNameText;

	// Token: 0x040014B9 RID: 5305
	[SerializeField]
	private Text levelText;

	// Token: 0x040014BA RID: 5306
	[SerializeField]
	private Image healthBar;

	// Token: 0x040014BB RID: 5307
	[SerializeField]
	private Text healthText;

	// Token: 0x040014BC RID: 5308
	[SerializeField]
	private Image enduranceBar;

	// Token: 0x040014BD RID: 5309
	[SerializeField]
	private Text enduranceText;

	// Token: 0x040014BE RID: 5310
	[SerializeField]
	private Image avatarImage;

	// Token: 0x040014BF RID: 5311
	[SerializeField]
	private GameObject skullObject;

	// Token: 0x040014C0 RID: 5312
	[SerializeField]
	private GameObject partyButton;

	// Token: 0x040014C1 RID: 5313
	[SerializeField]
	private GameObject messageButton;

	// Token: 0x040014C2 RID: 5314
	[SerializeField]
	private GameObject tradeButton;

	// Token: 0x040014C3 RID: 5315
	[SerializeField]
	private GameObject guildButton;

	// Token: 0x040014C4 RID: 5316
	[SerializeField]
	private UISystemModule uiSystemModule;

	// Token: 0x040014C5 RID: 5317
	private GameObject selectedTarget;

	// Token: 0x040014C6 RID: 5318
	private GuildModule targetGuildModule;

	// Token: 0x040014C7 RID: 5319
	private CreatureModule targetCreatureModule;

	// Token: 0x040014C8 RID: 5320
	private AttributeModule targetAttributeModule;

	// Token: 0x040014C9 RID: 5321
	private NonPlayerAttributeModule nonPlayerTargetAttributeModule;

	// Token: 0x040014CA RID: 5322
	private float updateTime;

	// Token: 0x040014CB RID: 5323
	private readonly float updateInterval = 0.2f;
}
