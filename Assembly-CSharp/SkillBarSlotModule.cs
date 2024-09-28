using System;
using DuloGames.UI;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000412 RID: 1042
public class SkillBarSlotModule : MonoBehaviour, IDragHandler, IEventSystemHandler, IEndDragHandler, IDropHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	// Token: 0x17000253 RID: 595
	// (get) Token: 0x06001684 RID: 5764 RVA: 0x00072EE0 File Offset: 0x000710E0
	// (set) Token: 0x06001685 RID: 5765 RVA: 0x00072EE8 File Offset: 0x000710E8
	public Skill Skill
	{
		get
		{
			return this._skill;
		}
		set
		{
			this._skill = value;
			this.UpdateShortcutText();
			this.skillIcon.sprite = null;
			if (this._skill.IsDefined)
			{
				this.skillIcon.sprite = this._skill.Icon;
				this.cooldownText.enabled = true;
				this.skillIcon.color = (this._skill.Learned ? Color.white : Color.gray);
				this.blockedIcon.SetActive(!this._skill.Learned);
				return;
			}
			this.skillIcon.color = Color.gray;
			this.skillIcon.sprite = this.GetDefaultIcon();
			this.cooldownText.enabled = false;
			this.blockedIcon.SetActive(false);
		}
	}

	// Token: 0x06001686 RID: 5766 RVA: 0x00072FB4 File Offset: 0x000711B4
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
		base.InvokeRepeating("UpdateShortcutText", 10f, 2f);
	}

	// Token: 0x06001687 RID: 5767 RVA: 0x00072FF0 File Offset: 0x000711F0
	private void OnEnable()
	{
		this.isOver = false;
		this.overTime = Time.time;
		if (this.Skill.IsDefined)
		{
			this.uiSystemModule.CloseSkillTooltip();
		}
	}

	// Token: 0x06001688 RID: 5768 RVA: 0x0007302C File Offset: 0x0007122C
	private void UpdateShortcutText()
	{
		KeyMap keyMap = GameInputModule.GetKeyMap(this.BuildKeyMapName());
		if (keyMap.KeyCode != KeyCode.None)
		{
			this.shortcut.text = LanguageManager.Instance.GetText(GlobalUtils.KeyCodeToString(keyMap.KeyCode));
			return;
		}
		if (keyMap.AltKeyCode != KeyCode.None)
		{
			this.shortcut.text = LanguageManager.Instance.GetText(GlobalUtils.KeyCodeToString(keyMap.AltKeyCode));
			return;
		}
		this.shortcut.text = "";
	}

	// Token: 0x06001689 RID: 5769 RVA: 0x000730A8 File Offset: 0x000712A8
	private string BuildKeyMapName()
	{
		string text;
		if (this.SlotPosition == 9)
		{
			text = "Skill0";
		}
		else
		{
			text = string.Format("Skill{0}", this.SlotPosition + 1);
		}
		if (this.SkillBarId != 0)
		{
			text = "Second " + text;
		}
		return text;
	}

	// Token: 0x0600168A RID: 5770 RVA: 0x000730FC File Offset: 0x000712FC
	private void OnDisable()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("UIIcon");
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].transform.parent.CompareTag("UIHud"))
			{
				UnityEngine.Object.Destroy(array[i]);
			}
		}
		this.isOver = false;
		if (this.Skill.IsDefined)
		{
			this.uiSystemModule.CloseSkillTooltip();
		}
	}

	// Token: 0x0600168B RID: 5771 RVA: 0x00073168 File Offset: 0x00071368
	private void Update()
	{
		if (!this._skill.IsDefined)
		{
			this.skillIcon.sprite = this.defaultIcon;
			this.slotBase.dragAndDropEnabled = false;
			return;
		}
		this.skillIcon.sprite = this._skill.Icon;
		this.slotBase.dragAndDropEnabled = true;
		double num = this._skill.CooldownTimer(NetworkTime.time);
		if (num > 0.0)
		{
			this.skillIcon.color = Color.gray;
			if (num < 10.0)
			{
				this.cooldownText.text = string.Format("{0:0.0}", num);
			}
			else
			{
				this.cooldownText.text = string.Format("{0:0}", num);
			}
		}
		else
		{
			this.cooldownText.text = string.Empty;
			if (!this._skill.Learned || this.uiSystemModule.AttributeModule.BaseLevel < this._skill.RequiredLevel)
			{
				this.skillIcon.color = Color.gray;
				this.blockedIcon.SetActive(true);
			}
			else
			{
				this.skillIcon.color = Color.white;
				this.blockedIcon.SetActive(false);
			}
		}
		if (this.isOver && Time.time - this.overTime > 0.7f)
		{
			this.uiSystemModule.ShowSkillTooltip(Input.mousePosition, this._skill);
		}
	}

	// Token: 0x0600168C RID: 5772 RVA: 0x000732DD File Offset: 0x000714DD
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.isOver = true;
		this.overTime = Time.time;
	}

	// Token: 0x0600168D RID: 5773 RVA: 0x000732F4 File Offset: 0x000714F4
	public void OnPointerExit(PointerEventData eventData)
	{
		this.isOver = false;
		if (this.Skill.IsDefined)
		{
			this.uiSystemModule.CloseSkillTooltip();
		}
	}

	// Token: 0x0600168E RID: 5774 RVA: 0x00073323 File Offset: 0x00071523
	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			this.uiSystemModule.PlayerModule.CastSkill(this.Skill.Id);
		}
	}

	// Token: 0x0600168F RID: 5775 RVA: 0x00073348 File Offset: 0x00071548
	public void OnDrag(PointerEventData eventData)
	{
		if (this._skill.IsDefined)
		{
			this._skill.SlotPosition = this.SlotPosition;
		}
	}

	// Token: 0x06001690 RID: 5776 RVA: 0x00073368 File Offset: 0x00071568
	public void OnEndDrag(PointerEventData eventData)
	{
		if (eventData.hovered.Count == 0)
		{
			this.uiSystemModule.SkillModule.CmdRemoveFromSkillBar(this.SlotPosition, this.SkillBarId, true);
		}
	}

	// Token: 0x06001691 RID: 5777 RVA: 0x00073394 File Offset: 0x00071594
	public void OnDrop(PointerEventData eventData)
	{
		if (!eventData.pointerDrag.CompareTag("UISkillBookSlot"))
		{
			if (eventData.pointerDrag.CompareTag("UISkillBarSlot"))
			{
				SkillBarSlotModule skillBarSlotModule;
				if (!eventData.pointerDrag.TryGetComponent<SkillBarSlotModule>(out skillBarSlotModule))
				{
					return;
				}
				if (!skillBarSlotModule.Skill.IsDefined)
				{
					return;
				}
				this.uiSystemModule.SkillModule.CmdSwapSkillBarSlot(this.SlotPosition, skillBarSlotModule.SlotPosition, this.SkillBarId, true);
			}
			return;
		}
		SkillBookSlotModule componentInParent = eventData.pointerDrag.GetComponentInParent<SkillBookSlotModule>();
		if (componentInParent == null)
		{
			return;
		}
		if (!componentInParent.Skill.Learned)
		{
			return;
		}
		if (this.uiSystemModule.AttributeModule.BaseLevel < componentInParent.Skill.RequiredLevel)
		{
			return;
		}
		this.uiSystemModule.SkillModule.CmdAddToSkillBar(componentInParent.SlotPosition, this.SlotPosition, this.SkillBarId, true);
	}

	// Token: 0x06001692 RID: 5778 RVA: 0x0007346F File Offset: 0x0007166F
	private Sprite GetDefaultIcon()
	{
		if (this.defaultIcon != null)
		{
			return this.defaultIcon;
		}
		this.defaultIcon = Resources.Load<Sprite>("Icons/Slots/default_skill_slot");
		return this.defaultIcon;
	}

	// Token: 0x04001456 RID: 5206
	[SerializeField]
	private Image skillIcon;

	// Token: 0x04001457 RID: 5207
	[SerializeField]
	private Text cooldownText;

	// Token: 0x04001458 RID: 5208
	[SerializeField]
	private Text shortcut;

	// Token: 0x04001459 RID: 5209
	[SerializeField]
	private UISlotBase slotBase;

	// Token: 0x0400145A RID: 5210
	[SerializeField]
	private GameObject blockedIcon;

	// Token: 0x0400145B RID: 5211
	private bool isOver;

	// Token: 0x0400145C RID: 5212
	private float overTime;

	// Token: 0x0400145D RID: 5213
	private Sprite defaultIcon;

	// Token: 0x0400145E RID: 5214
	private UISystemModule uiSystemModule;

	// Token: 0x0400145F RID: 5215
	private Skill _skill;

	// Token: 0x04001460 RID: 5216
	public int SlotPosition;

	// Token: 0x04001461 RID: 5217
	public int SkillBarId;
}
