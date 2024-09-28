using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200020C RID: 524
public class MinimapPointManager : MonoBehaviour
{
	// Token: 0x06000703 RID: 1795 RVA: 0x00022628 File Offset: 0x00020828
	private void Awake()
	{
		this.pvpModule = base.GetComponentInParent<PvpModule>();
		this.combatModule = base.GetComponentInParent<CombatModule>();
		this.playerModule = base.GetComponentInParent<PlayerModule>();
		this.monsterModule = base.GetComponentInParent<MonsterModule>();
		this.conditionModule = base.GetComponentInParent<ConditionModule>();
		if (NetworkClient.active)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
			this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
		}
	}

	// Token: 0x06000704 RID: 1796 RVA: 0x00002E81 File Offset: 0x00001081
	private void Start()
	{
		if (NetworkServer.active)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06000705 RID: 1797 RVA: 0x00022690 File Offset: 0x00020890
	private void LateUpdate()
	{
		if (NetworkClient.active)
		{
			bool flag = this.conditionModule.isLocalPlayer || !this.conditionModule.HasActiveCondition(ConditionCategory.Invisibility);
			if (this.spriteRenderer.enabled && !flag)
			{
				this.spriteRenderer.enabled = false;
			}
			else if (!this.spriteRenderer.enabled && flag)
			{
				this.spriteRenderer.enabled = true;
			}
		}
		if (Time.time - this.lastColorUpdate > 1f)
		{
			this.lastColorUpdate = Time.time;
			this.DefineColor();
			this.DefineHealthColor();
			this.DefinePlayerOrbColor();
		}
	}

	// Token: 0x06000706 RID: 1798 RVA: 0x00022730 File Offset: 0x00020930
	private void DefineHealthColor()
	{
		bool flag2;
		bool flag = this.IsPartyMember(out flag2);
		if (flag && flag2)
		{
			this.healthbarImage.color = Color.cyan;
			return;
		}
		if (flag & !flag2)
		{
			this.healthbarImage.color = Color.yellow;
			return;
		}
		if (this.playerModule != null && this.playerModule.isLocalPlayer)
		{
			this.healthbarImage.color = Color.green;
			return;
		}
		this.healthbarImage.color = new Color(0.4823529f, 0.145098f, 0.1019608f, 1f);
	}

	// Token: 0x06000707 RID: 1799 RVA: 0x000227C8 File Offset: 0x000209C8
	private void DefinePlayerOrbColor()
	{
		if (this.pvpModule == null)
		{
			return;
		}
		if (this.pvpModule.PvpStatus == PvpStatus.Neutral)
		{
			this.pvpFrame.SetActive(false);
			return;
		}
		this.pvpFrame.SetActive(true);
		if (this.pvpModule.PvpStatus == PvpStatus.InCombat)
		{
			this.glowOrb.SetActive(false);
			this.orbImage.color = new Color(0.8509804f, 0.4078431f, 0.1333333f, 1f);
			return;
		}
		if (this.pvpModule.PvpStatus == PvpStatus.PlayerKiller)
		{
			this.glowOrb.SetActive(false);
			this.orbImage.color = new Color(0.772549f, 0.1843137f, 0.1058824f, 1f);
			return;
		}
		if (this.pvpModule.PvpStatus == PvpStatus.Outlaw)
		{
			this.glowOrb.SetActive(true);
			this.orbImage.color = new Color(0f, 0f, 0f, 1f);
		}
	}

	// Token: 0x06000708 RID: 1800 RVA: 0x000228C8 File Offset: 0x00020AC8
	private void DefineColor()
	{
		if (this.monsterModule != null)
		{
			if (this.combatModule.IsPreemptive)
			{
				this.spriteRenderer.color = Color.red;
				return;
			}
			if (!this.combatModule.IsPreemptive)
			{
				this.spriteRenderer.color = Color.white;
				return;
			}
		}
		if (!(this.playerModule != null))
		{
			this.spriteRenderer.color = Color.white;
			return;
		}
		if (this.pvpModule.PvpStatus == PvpStatus.PlayerKiller | this.pvpModule.PvpStatus == PvpStatus.Outlaw)
		{
			this.spriteRenderer.color = Color.red;
			return;
		}
		if (this.pvpModule.PvpStatus == PvpStatus.InCombat)
		{
			this.spriteRenderer.color = GlobalSettings.Colors[2];
			return;
		}
		if (this.pvpModule.PvpStatus == PvpStatus.Neutral)
		{
			bool flag;
			if (this.IsPartyMember(out flag))
			{
				if (flag)
				{
					this.spriteRenderer.color = Color.green;
					return;
				}
				this.spriteRenderer.color = Color.yellow;
				return;
			}
			else
			{
				this.spriteRenderer.color = Color.cyan;
			}
		}
	}

	// Token: 0x06000709 RID: 1801 RVA: 0x000229E4 File Offset: 0x00020BE4
	private bool IsPartyMember(out bool canShareRewards)
	{
		if (this.playerModule == null)
		{
			canShareRewards = false;
			return false;
		}
		if (this.uiSystemModule == null || this.uiSystemModule.PartyModule == null)
		{
			canShareRewards = false;
			return false;
		}
		List<PartyMember> list = new List<PartyMember>(this.uiSystemModule.PartyModule.PartyMembers);
		list.Add(new PartyMember(this.uiSystemModule.CreatureModule.CreatureName, this.uiSystemModule.CreatureModule.netId, this.uiSystemModule.CreatureModule.gameObject));
		if (list.Count < 2)
		{
			canShareRewards = false;
			return false;
		}
		int highestMemberLevel = this.uiSystemModule.PartyModule.GetHighestMemberLevel();
		foreach (PartyMember partyMember in list)
		{
			if (NetworkClient.spawned.ContainsKey(partyMember.NetworkInstanceId) && NetworkClient.spawned[partyMember.NetworkInstanceId].netId == this.playerModule.netId)
			{
				AttributeModule attributeModule;
				this.playerModule.TryGetComponent<AttributeModule>(out attributeModule);
				canShareRewards = GlobalUtils.CanShareRewards(highestMemberLevel, attributeModule.BaseLevel, attributeModule.MasteryLevel);
				Debug.Log(string.Format("Name: {0}/HPM: {1}/BL: {2}", partyMember.Name, highestMemberLevel, attributeModule.BaseLevel));
				return true;
			}
		}
		canShareRewards = false;
		return false;
	}

	// Token: 0x0600070A RID: 1802 RVA: 0x00022B68 File Offset: 0x00020D68
	public void ChangeColor(Color color)
	{
		this.spriteRenderer.color = color;
	}

	// Token: 0x04000908 RID: 2312
	private PvpModule pvpModule;

	// Token: 0x04000909 RID: 2313
	private CombatModule combatModule;

	// Token: 0x0400090A RID: 2314
	private PlayerModule playerModule;

	// Token: 0x0400090B RID: 2315
	private MonsterModule monsterModule;

	// Token: 0x0400090C RID: 2316
	private UISystemModule uiSystemModule;

	// Token: 0x0400090D RID: 2317
	private ConditionModule conditionModule;

	// Token: 0x0400090E RID: 2318
	[SerializeField]
	private SpriteRenderer spriteRenderer;

	// Token: 0x0400090F RID: 2319
	[SerializeField]
	private Image healthbarImage;

	// Token: 0x04000910 RID: 2320
	[SerializeField]
	private Image orbImage;

	// Token: 0x04000911 RID: 2321
	[SerializeField]
	private GameObject glowOrb;

	// Token: 0x04000912 RID: 2322
	[SerializeField]
	private GameObject pvpFrame;

	// Token: 0x04000913 RID: 2323
	private float lastColorUpdate;
}
