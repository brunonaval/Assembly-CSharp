using System;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001E6 RID: 486
public class ConditionBarSlotManager : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x060005E3 RID: 1507 RVA: 0x0001E930 File Offset: 0x0001CB30
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x060005E4 RID: 1508 RVA: 0x0001E954 File Offset: 0x0001CB54
	private void OnDisable()
	{
		this.isOver = false;
		if (this.Condition.IsDefined)
		{
			this.uiSystemModule.CloseConditionTooltip();
		}
	}

	// Token: 0x060005E5 RID: 1509 RVA: 0x0001E978 File Offset: 0x0001CB78
	private void Update()
	{
		if (this.Condition.IsDefined)
		{
			this.skillIcon.enabled = true;
			this.skillIconOverlay.enabled = true;
			this.skillIcon.sprite = this.Condition.Icon;
			this.skillIconOverlay.sprite = this.Condition.Icon;
			double num = this.Condition.CooldownTimer(NetworkTime.time);
			double num2 = 1.0 - num * 100.0 / (double)this.Condition.Duration / 100.0;
			this.skillIconOverlay.type = Image.Type.Filled;
			this.skillIconOverlay.fillAmount = (float)num2;
			if (num2 >= 1.0)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			if (this.isTarget & this.uiSystemModule.CombatModule.Target == null)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
		}
		else
		{
			this.skillIcon.enabled = false;
			this.skillIconOverlay.enabled = false;
		}
		if (this.isOver && Time.time - this.overTime > 0.7f && this.Condition.IsDefined)
		{
			this.uiSystemModule.ShowConditionTooltip(Input.mousePosition, this.Condition);
		}
	}

	// Token: 0x060005E6 RID: 1510 RVA: 0x0001EAC9 File Offset: 0x0001CCC9
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.isOver = true;
		this.overTime = Time.time;
	}

	// Token: 0x060005E7 RID: 1511 RVA: 0x0001EADD File Offset: 0x0001CCDD
	public void OnPointerExit(PointerEventData eventData)
	{
		this.isOver = false;
		if (this.Condition.IsDefined)
		{
			this.uiSystemModule.CloseSkillTooltip();
		}
	}

	// Token: 0x060005E8 RID: 1512 RVA: 0x0001EAFE File Offset: 0x0001CCFE
	public void SetCondition(bool isTarget, Condition condition)
	{
		this.Condition = condition;
		this.isTarget = isTarget;
	}

	// Token: 0x0400082E RID: 2094
	public Condition Condition;

	// Token: 0x0400082F RID: 2095
	[SerializeField]
	private Image skillIcon;

	// Token: 0x04000830 RID: 2096
	[SerializeField]
	private Image skillIconOverlay;

	// Token: 0x04000831 RID: 2097
	private bool isOver;

	// Token: 0x04000832 RID: 2098
	private bool isTarget;

	// Token: 0x04000833 RID: 2099
	private float overTime;

	// Token: 0x04000834 RID: 2100
	private UISystemModule uiSystemModule;
}
