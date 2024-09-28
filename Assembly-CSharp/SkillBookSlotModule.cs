using System;
using DuloGames.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000413 RID: 1043
public class SkillBookSlotModule : MonoBehaviour, IDragHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
	// Token: 0x06001694 RID: 5780 RVA: 0x0007349C File Offset: 0x0007169C
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x06001695 RID: 5781 RVA: 0x000734C0 File Offset: 0x000716C0
	private void OnEnable()
	{
		this.isOver = false;
		this.overTime = Time.time;
		if (this.Skill.IsDefined)
		{
			this.uiSystemModule.CloseSkillTooltip();
		}
	}

	// Token: 0x06001696 RID: 5782 RVA: 0x000734EC File Offset: 0x000716EC
	private void OnDisable()
	{
		this.isOver = false;
		if (this.Skill.IsDefined)
		{
			this.uiSystemModule.CloseSkillTooltip();
		}
	}

	// Token: 0x06001697 RID: 5783 RVA: 0x0007350D File Offset: 0x0007170D
	private void Update()
	{
		if (!this.Skill.IsDefined)
		{
			this.nameText.enabled = false;
			this.skillIcon.enabled = false;
			this.skillPowerText.enabled = false;
			return;
		}
		this.UpdateSkillInfo();
		this.UpdateTooltipInfo();
	}

	// Token: 0x06001698 RID: 5784 RVA: 0x0007354D File Offset: 0x0007174D
	private void UpdateTooltipInfo()
	{
		if (this.isOver && Time.time - this.overTime > 0.7f && this.Skill.IsDefined)
		{
			this.uiSystemModule.ShowSkillTooltip(Input.mousePosition, this.Skill);
		}
	}

	// Token: 0x06001699 RID: 5785 RVA: 0x00073590 File Offset: 0x00071790
	private void UpdateSkillInfo()
	{
		this.nameText.enabled = true;
		this.skillIcon.enabled = true;
		this.skillPowerText.enabled = true;
		this.skillIcon.sprite = this.Skill.Icon;
		this.nameText.text = this.Skill.DisplayName.ToLower();
		this.requiredLevelText.text = string.Format(LanguageManager.Instance.GetText("skill_required_level"), this.Skill.RequiredLevel.ToString());
		this.nameText.color = GlobalUtils.SkillCategoryToFrameColor(this.Skill.Category);
		if (this.Skill.Learned && this.uiSystemModule.AttributeModule.BaseLevel >= this.Skill.RequiredLevel)
		{
			this.slotBase.dragAndDropEnabled = true;
			this.skillIcon.color = Color.white;
			this.requiredLevelText.color = Color.green;
			this.blockedIcon.SetActive(false);
		}
		else
		{
			this.slotBase.dragAndDropEnabled = false;
			this.skillIcon.color = Color.gray;
			this.requiredLevelText.color = Color.red;
			this.blockedIcon.SetActive(true);
		}
		this.descriptionText.text = LanguageManager.Instance.GetText(this.Skill.Description);
		if (this.Skill.SkillPower > 1f)
		{
			this.UpdateIntegerSkillPowerInfo();
			return;
		}
		this.UpdatePercentageSkillPowerInfo();
	}

	// Token: 0x0600169A RID: 5786 RVA: 0x00073718 File Offset: 0x00071918
	private void UpdatePercentageSkillPowerInfo()
	{
		if (this.Skill.CastAmount > 1)
		{
			this.skillPowerText.text = string.Format("{0}% (x{1})", this.Skill.SkillPower * 100f, this.Skill.CastAmount);
			return;
		}
		this.skillPowerText.text = string.Format("{0}%", this.Skill.SkillPower * 100f);
	}

	// Token: 0x0600169B RID: 5787 RVA: 0x0007379C File Offset: 0x0007199C
	private void UpdateIntegerSkillPowerInfo()
	{
		if (this.Skill.CastAmount > 1)
		{
			this.skillPowerText.text = string.Format("{0} (x{1})", this.Skill.SkillPower, this.Skill.CastAmount);
			return;
		}
		this.skillPowerText.text = string.Format("{0}", this.Skill.SkillPower);
	}

	// Token: 0x0600169C RID: 5788 RVA: 0x00073812 File Offset: 0x00071A12
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.isOver = true;
		this.overTime = Time.time;
	}

	// Token: 0x0600169D RID: 5789 RVA: 0x000734EC File Offset: 0x000716EC
	public void OnPointerExit(PointerEventData eventData)
	{
		this.isOver = false;
		if (this.Skill.IsDefined)
		{
			this.uiSystemModule.CloseSkillTooltip();
		}
	}

	// Token: 0x0600169E RID: 5790 RVA: 0x00073826 File Offset: 0x00071A26
	public void OnDrag(PointerEventData eventData)
	{
		if (this.Skill.IsDefined)
		{
			this.Skill.SlotPosition = this.SlotPosition;
		}
	}

	// Token: 0x04001462 RID: 5218
	public Skill Skill;

	// Token: 0x04001463 RID: 5219
	public int SlotPosition;

	// Token: 0x04001464 RID: 5220
	[SerializeField]
	public Text requiredLevelText;

	// Token: 0x04001465 RID: 5221
	[SerializeField]
	private GameObject blockedIcon;

	// Token: 0x04001466 RID: 5222
	[SerializeField]
	private Image skillIcon;

	// Token: 0x04001467 RID: 5223
	[SerializeField]
	private Text nameText;

	// Token: 0x04001468 RID: 5224
	[SerializeField]
	private Text skillPowerText;

	// Token: 0x04001469 RID: 5225
	[SerializeField]
	private Text descriptionText;

	// Token: 0x0400146A RID: 5226
	[SerializeField]
	private UISlotBase slotBase;

	// Token: 0x0400146B RID: 5227
	private bool isOver;

	// Token: 0x0400146C RID: 5228
	private float overTime;

	// Token: 0x0400146D RID: 5229
	private UISystemModule uiSystemModule;
}
