using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x02000596 RID: 1430
	public class Demo_CharacterSelectList_Character : MonoBehaviour
	{
		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06001FA2 RID: 8098 RVA: 0x0009F142 File Offset: 0x0009D342
		public Demo_CharacterInfo characterInfo
		{
			get
			{
				return this.m_CharacterInfo;
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06001FA3 RID: 8099 RVA: 0x0009F14A File Offset: 0x0009D34A
		public bool isSelected
		{
			get
			{
				return this.m_Toggle != null && this.m_Toggle.isOn;
			}
		}

		// Token: 0x06001FA4 RID: 8100 RVA: 0x0009F167 File Offset: 0x0009D367
		protected void Awake()
		{
			this.m_OnCharacterSelected = new Demo_CharacterSelectList_Character.OnCharacterSelectEvent();
			this.m_OnCharacterDelete = new Demo_CharacterSelectList_Character.OnCharacterDeleteEvent();
		}

		// Token: 0x06001FA5 RID: 8101 RVA: 0x0009F180 File Offset: 0x0009D380
		protected void OnEnable()
		{
			if (this.m_Toggle != null)
			{
				this.m_Toggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnToggleValueChanged));
			}
			if (this.m_Delete != null)
			{
				this.m_Delete.onClick.AddListener(new UnityAction(this.OnDeleteClick));
			}
		}

		// Token: 0x06001FA6 RID: 8102 RVA: 0x0009F1E4 File Offset: 0x0009D3E4
		protected void OnDisable()
		{
			if (this.m_Toggle != null)
			{
				this.m_Toggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.OnToggleValueChanged));
			}
			if (this.m_Delete != null)
			{
				this.m_Delete.onClick.RemoveListener(new UnityAction(this.OnDeleteClick));
			}
		}

		// Token: 0x06001FA7 RID: 8103 RVA: 0x0009F248 File Offset: 0x0009D448
		public void SetCharacterInfo(Demo_CharacterInfo info)
		{
			if (info == null)
			{
				return;
			}
			if (this.m_NameText != null)
			{
				this.m_NameText.text = info.name.ToLower();
			}
			if (this.m_LevelText != null)
			{
				this.m_LevelText.text = info.level.ToString();
			}
			if (this.m_RaceText != null)
			{
				this.m_RaceText.text = info.raceString;
			}
			if (this.m_ClassText != null)
			{
				this.m_ClassText.text = info.classString;
			}
			this.m_CharacterInfo = info;
		}

		// Token: 0x06001FA8 RID: 8104 RVA: 0x0009F2E6 File Offset: 0x0009D4E6
		public void SetAvatar(Sprite sprite)
		{
			if (this.m_Avatar != null)
			{
				this.m_Avatar.sprite = sprite;
			}
		}

		// Token: 0x06001FA9 RID: 8105 RVA: 0x0009F302 File Offset: 0x0009D502
		public void SetToggleGroup(ToggleGroup group)
		{
			if (this.m_Toggle != null)
			{
				this.m_Toggle.group = group;
			}
		}

		// Token: 0x06001FAA RID: 8106 RVA: 0x0009F31E File Offset: 0x0009D51E
		public void SetSelected(bool selected)
		{
			if (this.m_Toggle != null)
			{
				this.m_Toggle.isOn = selected;
			}
		}

		// Token: 0x06001FAB RID: 8107 RVA: 0x0009F33A File Offset: 0x0009D53A
		private void OnToggleValueChanged(bool value)
		{
			if (value && this.m_OnCharacterSelected != null)
			{
				this.m_OnCharacterSelected.Invoke(this);
			}
		}

		// Token: 0x06001FAC RID: 8108 RVA: 0x0009F353 File Offset: 0x0009D553
		private void OnDeleteClick()
		{
			if (this.m_OnCharacterDelete != null)
			{
				this.m_OnCharacterDelete.Invoke(this);
			}
		}

		// Token: 0x06001FAD RID: 8109 RVA: 0x0009F369 File Offset: 0x0009D569
		public void AddOnSelectListener(UnityAction<Demo_CharacterSelectList_Character> call)
		{
			this.m_OnCharacterSelected.AddListener(call);
		}

		// Token: 0x06001FAE RID: 8110 RVA: 0x0009F377 File Offset: 0x0009D577
		public void RemoveOnSelectListener(UnityAction<Demo_CharacterSelectList_Character> call)
		{
			this.m_OnCharacterSelected.RemoveListener(call);
		}

		// Token: 0x06001FAF RID: 8111 RVA: 0x0009F385 File Offset: 0x0009D585
		public void AddOnDeleteListener(UnityAction<Demo_CharacterSelectList_Character> call)
		{
			this.m_OnCharacterDelete.AddListener(call);
		}

		// Token: 0x06001FB0 RID: 8112 RVA: 0x0009F393 File Offset: 0x0009D593
		public void RemoveOnDeleteListener(UnityAction<Demo_CharacterSelectList_Character> call)
		{
			this.m_OnCharacterDelete.RemoveListener(call);
		}

		// Token: 0x04001988 RID: 6536
		[SerializeField]
		private Toggle m_Toggle;

		// Token: 0x04001989 RID: 6537
		[SerializeField]
		private Button m_Delete;

		// Token: 0x0400198A RID: 6538
		[Header("Texts")]
		[SerializeField]
		private Text m_NameText;

		// Token: 0x0400198B RID: 6539
		[SerializeField]
		private Text m_LevelText;

		// Token: 0x0400198C RID: 6540
		[SerializeField]
		private Text m_RaceText;

		// Token: 0x0400198D RID: 6541
		[SerializeField]
		private Text m_ClassText;

		// Token: 0x0400198E RID: 6542
		[SerializeField]
		private Image m_Avatar;

		// Token: 0x0400198F RID: 6543
		private Demo_CharacterInfo m_CharacterInfo;

		// Token: 0x04001990 RID: 6544
		private Demo_CharacterSelectList_Character.OnCharacterSelectEvent m_OnCharacterSelected;

		// Token: 0x04001991 RID: 6545
		private Demo_CharacterSelectList_Character.OnCharacterDeleteEvent m_OnCharacterDelete;

		// Token: 0x02000597 RID: 1431
		[Serializable]
		public class OnCharacterSelectEvent : UnityEvent<Demo_CharacterSelectList_Character>
		{
		}

		// Token: 0x02000598 RID: 1432
		[Serializable]
		public class OnCharacterDeleteEvent : UnityEvent<Demo_CharacterSelectList_Character>
		{
		}
	}
}
