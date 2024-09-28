using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200020E RID: 526
public class MobileAttackButtonManager : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler
{
	// Token: 0x06000718 RID: 1816 RVA: 0x00022F80 File Offset: 0x00021180
	private void Update()
	{
		if (this.buttonPressed)
		{
			this.OnClick();
		}
	}

	// Token: 0x06000719 RID: 1817 RVA: 0x00022F90 File Offset: 0x00021190
	private void OnApplicationFocus(bool focus)
	{
		if (!focus)
		{
			this.buttonPressed = false;
		}
	}

	// Token: 0x0600071A RID: 1818 RVA: 0x00022F9C File Offset: 0x0002119C
	public void OnClick()
	{
		Skill skill = this.uiSystemModule.SkillModule.SkillBook.FirstOrDefault((Skill s) => s.IsDefaultSkill);
		if (skill.CooldownTimer(NetworkTime.time) > 0.0)
		{
			return;
		}
		List<GameObject> nearbyTagets = this.uiSystemModule.CombatModule.GetNearbyTagets();
		if (nearbyTagets.Count == 0)
		{
			return;
		}
		bool flag = false;
		foreach (GameObject gameObject in nearbyTagets)
		{
			CreatureModule creatureModule;
			if (gameObject.TryGetComponent<CreatureModule>(out creatureModule) && creatureModule.IsAlive && this.uiSystemModule.CombatModule.TargetFinder.IsTargetValid(skill.Range, true, true, true, false, this.uiSystemModule.PlayerModule.transform.position, this.uiSystemModule.PlayerModule.gameObject, gameObject))
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return;
		}
		if (!this.uiSystemModule.SkillModule.IsCasting)
		{
			this.uiSystemModule.SkillModule.CmdCast(skill.Id);
		}
	}

	// Token: 0x0600071B RID: 1819 RVA: 0x000230DC File Offset: 0x000212DC
	public void OnPointerDown(PointerEventData eventData)
	{
		this.buttonPressed = true;
	}

	// Token: 0x0600071C RID: 1820 RVA: 0x000230E5 File Offset: 0x000212E5
	public void OnPointerUp(PointerEventData eventData)
	{
		this.buttonPressed = false;
	}

	// Token: 0x0400091D RID: 2333
	[SerializeField]
	private UISystemModule uiSystemModule;

	// Token: 0x0400091E RID: 2334
	private bool buttonPressed;
}
