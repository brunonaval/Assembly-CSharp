using System;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000230 RID: 560
public class PartyMemberManager : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	// Token: 0x060007D4 RID: 2004 RVA: 0x00025C30 File Offset: 0x00023E30
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x060007D5 RID: 2005 RVA: 0x00025C54 File Offset: 0x00023E54
	private void Update()
	{
		if (Time.time - this.updateTime < this.updateInterval)
		{
			return;
		}
		this.updateTime = Time.time;
		if (this.partyMember.IsDefined)
		{
			this.UpdateDefinedPlayerInfo();
			return;
		}
		this.UpdateUndefinedPlayerInfo();
	}

	// Token: 0x060007D6 RID: 2006 RVA: 0x00025C90 File Offset: 0x00023E90
	private void UpdateUndefinedPlayerInfo()
	{
		NetworkIdentity networkIdentity;
		if (NetworkClient.spawned.TryGetValue(this.partyMember.NetworkInstanceId, out networkIdentity) && networkIdentity != null && networkIdentity.gameObject != null)
		{
			this.SetPartyMember(new PartyMember(this.partyMember.Name, this.partyMember.NetworkInstanceId, networkIdentity.gameObject));
			return;
		}
		if (this.partyMemberNameText != null && string.IsNullOrEmpty(this.partyMemberNameText.text))
		{
			this.partyMemberNameText.text = this.partyMember.Name;
		}
		if (!this.vocationBadge.gameObject.activeInHierarchy)
		{
			return;
		}
		this.vocationBadge.gameObject.SetActive(false);
	}

	// Token: 0x060007D7 RID: 2007 RVA: 0x00025D50 File Offset: 0x00023F50
	private void UpdateDefinedPlayerInfo()
	{
		if (!this.vocationBadge.gameObject.activeInHierarchy)
		{
			this.vocationBadge.gameObject.SetActive(true);
		}
		if (this.partyMemberNameText != null)
		{
			this.partyMemberNameText.text = this.partyMember.Name;
		}
		this.levelText.text = this.partyMemberAttributeModule.BaseLevel.ToString();
		this.UpdateHealthBarInfo();
		this.UpdateEnduranceBarInfo();
	}

	// Token: 0x060007D8 RID: 2008 RVA: 0x00025DCC File Offset: 0x00023FCC
	private void UpdateEnduranceBarInfo()
	{
		if (GlobalSettings.IsMobilePlatform)
		{
			return;
		}
		int maxEndurance = this.partyMemberAttributeModule.MaxEndurance;
		int currentEndurance = this.partyMemberAttributeModule.CurrentEndurance;
		float num = (float)currentEndurance * 100f / (float)maxEndurance;
		this.enduranceBar.fillAmount = num / 100f;
		this.enduranceBarText.text = string.Format("{0}/{1}", currentEndurance, maxEndurance);
	}

	// Token: 0x060007D9 RID: 2009 RVA: 0x00025E38 File Offset: 0x00024038
	private void UpdateHealthBarInfo()
	{
		int maxHealth = this.partyMemberAttributeModule.MaxHealth;
		int currentHealth = this.partyMemberAttributeModule.CurrentHealth;
		float num = (float)currentHealth * 100f / (float)maxHealth;
		this.healthBar.fillAmount = num / 100f;
		if (this.healthBarText != null)
		{
			this.healthBarText.text = string.Format("{0}/{1}", currentHealth, maxHealth);
		}
	}

	// Token: 0x060007DA RID: 2010 RVA: 0x00025EAC File Offset: 0x000240AC
	public void SetPartyMember(PartyMember partyMember)
	{
		this.partyMember = partyMember;
		if (!partyMember.IsDefined)
		{
			return;
		}
		this.partyMemberAttributeModule = partyMember.Member.GetComponent<AttributeModule>();
		Vocation vocation = partyMember.Member.GetComponent<VocationModule>().Vocation;
		Sprite vocationIconSprite = ResourcesManager.Instance.GetVocationIconSprite(vocation.ToString().ToLower());
		this.vocationBadge.sprite = vocationIconSprite;
		if (this.backgroundVocationBadge != null)
		{
			this.backgroundVocationBadge.sprite = vocationIconSprite;
		}
	}

	// Token: 0x060007DB RID: 2011 RVA: 0x00025F2F File Offset: 0x0002412F
	public void OnPointerDown(PointerEventData eventData)
	{
		this.DetectLeftClickAndSelectPlayer(eventData);
		this.DetectRightClickAndSelectPlayersTarget(eventData);
	}

	// Token: 0x060007DC RID: 2012 RVA: 0x00025F40 File Offset: 0x00024140
	private void DetectRightClickAndSelectPlayersTarget(PointerEventData eventData)
	{
		if (!this.partyMember.IsDefined)
		{
			return;
		}
		if (eventData.button != PointerEventData.InputButton.Right)
		{
			return;
		}
		GameObject target = this.partyMember.Member.GetComponent<CombatModule>().Target;
		this.uiSystemModule.PartyModule.CmdSetTargetFromMember(this.partyMember.Name);
	}

	// Token: 0x060007DD RID: 2013 RVA: 0x00025F98 File Offset: 0x00024198
	private void DetectLeftClickAndSelectPlayer(PointerEventData eventData)
	{
		if (!this.partyMember.IsDefined)
		{
			return;
		}
		if (eventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		this.uiSystemModule.CombatModule.CmdRemoveTarget();
		this.uiSystemModule.CombatModule.CmdSetTarget(this.partyMember.Member, true);
	}

	// Token: 0x040009AF RID: 2479
	private PartyMember partyMember;

	// Token: 0x040009B0 RID: 2480
	[SerializeField]
	private Text partyMemberNameText;

	// Token: 0x040009B1 RID: 2481
	[SerializeField]
	private Text levelText;

	// Token: 0x040009B2 RID: 2482
	[SerializeField]
	private Image healthBar;

	// Token: 0x040009B3 RID: 2483
	[SerializeField]
	private Text healthBarText;

	// Token: 0x040009B4 RID: 2484
	[SerializeField]
	private Image enduranceBar;

	// Token: 0x040009B5 RID: 2485
	[SerializeField]
	private Text enduranceBarText;

	// Token: 0x040009B6 RID: 2486
	[SerializeField]
	private Image vocationBadge;

	// Token: 0x040009B7 RID: 2487
	[SerializeField]
	private Image backgroundVocationBadge;

	// Token: 0x040009B8 RID: 2488
	private UISystemModule uiSystemModule;

	// Token: 0x040009B9 RID: 2489
	private AttributeModule partyMemberAttributeModule;

	// Token: 0x040009BA RID: 2490
	private float updateTime;

	// Token: 0x040009BB RID: 2491
	private readonly float updateInterval = 0.2f;
}
