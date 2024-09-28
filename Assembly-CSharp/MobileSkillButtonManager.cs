using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200021E RID: 542
public class MobileSkillButtonManager : MonoBehaviour, IDropHandler, IEventSystemHandler, IEndDragHandler, IDragHandler
{
	// Token: 0x170000C9 RID: 201
	// (get) Token: 0x06000756 RID: 1878 RVA: 0x00023A10 File Offset: 0x00021C10
	// (set) Token: 0x06000757 RID: 1879 RVA: 0x00023A18 File Offset: 0x00021C18
	public float DragTime { get; private set; }

	// Token: 0x170000CA RID: 202
	// (get) Token: 0x06000758 RID: 1880 RVA: 0x00023A21 File Offset: 0x00021C21
	public Skill Skill
	{
		get
		{
			return this.skill;
		}
	}

	// Token: 0x170000CB RID: 203
	// (get) Token: 0x06000759 RID: 1881 RVA: 0x00023A29 File Offset: 0x00021C29
	private string PrefKey
	{
		get
		{
			return string.Format("{0}_{1}", this.uiSystemModule.PlayerModule.PlayerId, base.gameObject.name);
		}
	}

	// Token: 0x0600075A RID: 1882 RVA: 0x00023A55 File Offset: 0x00021C55
	private IEnumerator Start()
	{
		yield return new WaitUntil(() => this.uiSystemModule.PlayerModule.PlayerId != 0);
		if (PlayerPrefs.HasKey(this.PrefKey))
		{
			int skillId = PlayerPrefs.GetInt(this.PrefKey);
			Skill skill = this.uiSystemModule.SkillModule.SkillBook.FirstOrDefault((Skill s) => s.Id == skillId);
			this.SetSkill(skill);
		}
		yield break;
	}

	// Token: 0x0600075B RID: 1883 RVA: 0x00023A64 File Offset: 0x00021C64
	private void OnDisable()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("UIIcon");
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].transform.parent.CompareTag("MobileHUD"))
			{
				UnityEngine.Object.Destroy(array[i]);
			}
		}
	}

	// Token: 0x0600075C RID: 1884 RVA: 0x00023AAB File Offset: 0x00021CAB
	private void Update()
	{
		this.UpdateCooldownText();
	}

	// Token: 0x0600075D RID: 1885 RVA: 0x00023AB4 File Offset: 0x00021CB4
	public void OnClick()
	{
		if (!this.skill.IsDefined)
		{
			return;
		}
		this.RefreshSkillUsingSkillBook();
		if (this.skill.CooldownTimer(NetworkTime.time) > 0.0)
		{
			return;
		}
		if (this.skill.IsDefaultSkill | this.skill.CanCauseDamage)
		{
			List<GameObject> nearbyTagets = this.uiSystemModule.CombatModule.GetNearbyTagets();
			if (nearbyTagets.Count == 0)
			{
				return;
			}
			bool flag = false;
			foreach (GameObject gameObject in nearbyTagets)
			{
				CreatureModule creatureModule;
				if (gameObject.TryGetComponent<CreatureModule>(out creatureModule) && creatureModule.IsAlive && this.uiSystemModule.CombatModule.TargetFinder.IsTargetValid(this.skill.Range, true, true, true, false, this.uiSystemModule.PlayerModule.transform.position, this.uiSystemModule.PlayerModule.gameObject, gameObject))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return;
			}
		}
		this.uiSystemModule.SkillModule.CmdCast(this.skill.Id);
	}

	// Token: 0x0600075E RID: 1886 RVA: 0x00023BE8 File Offset: 0x00021DE8
	public void SetSkill(Skill skill)
	{
		Skill item = this.uiSystemModule.SkillModule.SkillBook.FirstOrDefault((Skill s) => s.Id == skill.Id);
		this.skillBookIndex = this.uiSystemModule.SkillModule.SkillBook.IndexOf(item);
		if (!item.IsDefined)
		{
			return;
		}
		this.skill = item;
		this.iconImage.sprite = this.skill.Icon;
		this.overlayImage.sprite = this.skill.Icon;
		PlayerPrefs.SetInt(this.PrefKey, skill.Id);
	}

	// Token: 0x0600075F RID: 1887 RVA: 0x00023C94 File Offset: 0x00021E94
	private void RemoveSkill()
	{
		this.skill = default(Skill);
		Sprite sprite = Resources.Load<Sprite>("Icons/Slots/default_skill_slot");
		this.iconImage.sprite = sprite;
		this.overlayImage.sprite = sprite;
		this.overlayImage.fillAmount = 1f;
		this.cooldownText.text = string.Empty;
		PlayerPrefs.DeleteKey(this.PrefKey);
	}

	// Token: 0x06000760 RID: 1888 RVA: 0x00023CFC File Offset: 0x00021EFC
	private void UpdateCooldownText()
	{
		if (!this.skill.IsDefined)
		{
			return;
		}
		Skill skill = this.uiSystemModule.SkillModule.SkillBook[this.skillBookIndex];
		if (!skill.IsDefined)
		{
			return;
		}
		float num = (float)skill.CooldownTimer(NetworkTime.time);
		if (num > 0f)
		{
			this.overlayImage.fillAmount = num / skill.Cooldown;
			if (num < 10f)
			{
				this.cooldownText.text = string.Format("{0:0.0}", num);
				return;
			}
			this.cooldownText.text = string.Format("{0:0}", num);
			return;
		}
		else
		{
			this.cooldownText.text = string.Empty;
			this.overlayImage.fillAmount = 0f;
			if (this.uiSystemModule.AttributeModule.BaseLevel < this.skill.RequiredLevel)
			{
				this.iconImage.color = Color.gray;
				return;
			}
			this.iconImage.color = Color.white;
			return;
		}
	}

	// Token: 0x06000761 RID: 1889 RVA: 0x00023E08 File Offset: 0x00022008
	private void RefreshSkillUsingSkillBook()
	{
		for (int i = 0; i < this.uiSystemModule.SkillModule.SkillBook.Count; i++)
		{
			if (this.uiSystemModule.SkillModule.SkillBook[i].Id == this.skill.Id)
			{
				this.skill = this.uiSystemModule.SkillModule.SkillBook[i];
				return;
			}
		}
	}

	// Token: 0x06000762 RID: 1890 RVA: 0x00023E7C File Offset: 0x0002207C
	public void OnDrop(PointerEventData eventData)
	{
		if (!eventData.pointerDrag.CompareTag("UISkillBookSlot"))
		{
			if (eventData.pointerDrag.CompareTag("UISkillBarSlot"))
			{
				MobileSkillButtonManager componentInParent = eventData.pointerDrag.GetComponentInParent<MobileSkillButtonManager>();
				if (Time.time - componentInParent.DragTime <= 0.2f)
				{
					return;
				}
				Skill skill = this.skill;
				Skill skill2 = componentInParent.Skill;
				this.RemoveSkill();
				componentInParent.RemoveSkill();
				if (skill2.IsDefined)
				{
					this.SetSkill(skill2);
					if (skill.IsDefined)
					{
						componentInParent.SetSkill(skill);
					}
				}
				if (!skill2.IsDefined && skill.IsDefined)
				{
					componentInParent.SetSkill(skill);
				}
			}
			return;
		}
		SkillBookSlotModule componentInParent2 = eventData.pointerDrag.GetComponentInParent<SkillBookSlotModule>();
		if (componentInParent2 == null)
		{
			return;
		}
		if (!componentInParent2.Skill.Learned)
		{
			return;
		}
		if (this.uiSystemModule.AttributeModule.BaseLevel < componentInParent2.Skill.RequiredLevel)
		{
			return;
		}
		this.SetSkill(componentInParent2.Skill);
	}

	// Token: 0x06000763 RID: 1891 RVA: 0x00023F70 File Offset: 0x00022170
	public void OnEndDrag(PointerEventData eventData)
	{
		if (Time.time - this.DragTime <= 0.2f)
		{
			return;
		}
		if (eventData.hovered.Count != 0)
		{
			if (!eventData.hovered.Any((GameObject h) => h.CompareTag("Joystick")))
			{
				return;
			}
		}
		this.RemoveSkill();
	}

	// Token: 0x06000764 RID: 1892 RVA: 0x00023FD0 File Offset: 0x000221D0
	public void OnDrag(PointerEventData eventData)
	{
		this.DragTime = Time.time;
	}

	// Token: 0x0400094A RID: 2378
	[SerializeField]
	private UISystemModule uiSystemModule;

	// Token: 0x0400094B RID: 2379
	[SerializeField]
	private Image iconImage;

	// Token: 0x0400094C RID: 2380
	[SerializeField]
	private Image overlayImage;

	// Token: 0x0400094D RID: 2381
	[SerializeField]
	private Text cooldownText;

	// Token: 0x0400094E RID: 2382
	private Skill skill;

	// Token: 0x0400094F RID: 2383
	private int skillBookIndex;
}
