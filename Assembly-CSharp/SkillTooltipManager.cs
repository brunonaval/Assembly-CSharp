using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200023E RID: 574
public class SkillTooltipManager : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x0600082E RID: 2094 RVA: 0x00027588 File Offset: 0x00025788
	private void Awake()
	{
		GameObject.FindGameObjectWithTag("UISystem").TryGetComponent<UISystemModule>(out this.uiSystemModule);
	}

	// Token: 0x0600082F RID: 2095 RVA: 0x000275A0 File Offset: 0x000257A0
	private void OnEnable()
	{
		this.isOver = false;
		this.overTime = Time.time;
		if (this.skill.IsDefined)
		{
			this.uiSystemModule.CloseSkillTooltip();
		}
	}

	// Token: 0x06000830 RID: 2096 RVA: 0x000275CC File Offset: 0x000257CC
	private void OnDisable()
	{
		this.isOver = false;
		if (this.skill.IsDefined)
		{
			this.uiSystemModule.CloseSkillTooltip();
		}
	}

	// Token: 0x06000831 RID: 2097 RVA: 0x000275F0 File Offset: 0x000257F0
	private void LateUpdate()
	{
		if (!this.isOver)
		{
			return;
		}
		if (!this.skill.IsDefined)
		{
			return;
		}
		if (Time.time - this.overTime <= 0.7f)
		{
			return;
		}
		this.uiSystemModule.ShowSkillTooltip(Input.mousePosition, this.skill);
	}

	// Token: 0x06000832 RID: 2098 RVA: 0x0002763E File Offset: 0x0002583E
	public void SetSkill(Skill skill)
	{
		this.skill = skill;
	}

	// Token: 0x06000833 RID: 2099 RVA: 0x00027647 File Offset: 0x00025847
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.isOver = true;
		this.overTime = Time.time;
	}

	// Token: 0x06000834 RID: 2100 RVA: 0x000275CC File Offset: 0x000257CC
	public void OnPointerExit(PointerEventData eventData)
	{
		this.isOver = false;
		if (this.skill.IsDefined)
		{
			this.uiSystemModule.CloseSkillTooltip();
		}
	}

	// Token: 0x04000A11 RID: 2577
	private Skill skill;

	// Token: 0x04000A12 RID: 2578
	private bool isOver;

	// Token: 0x04000A13 RID: 2579
	private float overTime;

	// Token: 0x04000A14 RID: 2580
	private UISystemModule uiSystemModule;
}
