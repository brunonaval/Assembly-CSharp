using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x020005E2 RID: 1506
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[AddComponentMenu("UI/Switch Select Field", 58)]
	public class UISwitchSelect : MonoBehaviour
	{
		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06002130 RID: 8496 RVA: 0x000A6A72 File Offset: 0x000A4C72
		public List<string> options
		{
			get
			{
				return this.m_Options;
			}
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06002131 RID: 8497 RVA: 0x000A6A7A File Offset: 0x000A4C7A
		// (set) Token: 0x06002132 RID: 8498 RVA: 0x000A6A82 File Offset: 0x000A4C82
		public string value
		{
			get
			{
				return this.m_SelectedItem;
			}
			set
			{
				this.SelectOption(value);
			}
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06002133 RID: 8499 RVA: 0x000A6A8B File Offset: 0x000A4C8B
		public int selectedOptionIndex
		{
			get
			{
				return this.GetOptionIndex(this.m_SelectedItem);
			}
		}

		// Token: 0x06002134 RID: 8500 RVA: 0x000A6A9C File Offset: 0x000A4C9C
		protected void OnEnable()
		{
			if (this.m_PrevButton != null)
			{
				this.m_PrevButton.onClick.AddListener(new UnityAction(this.OnPrevButtonClick));
			}
			if (this.m_NextButton != null)
			{
				this.m_NextButton.onClick.AddListener(new UnityAction(this.OnNextButtonClick));
			}
		}

		// Token: 0x06002135 RID: 8501 RVA: 0x000A6B00 File Offset: 0x000A4D00
		protected void OnDisable()
		{
			if (this.m_PrevButton != null)
			{
				this.m_PrevButton.onClick.RemoveListener(new UnityAction(this.OnPrevButtonClick));
			}
			if (this.m_NextButton != null)
			{
				this.m_NextButton.onClick.RemoveListener(new UnityAction(this.OnNextButtonClick));
			}
		}

		// Token: 0x06002136 RID: 8502 RVA: 0x000A6B64 File Offset: 0x000A4D64
		protected void OnPrevButtonClick()
		{
			int num = this.selectedOptionIndex - 1;
			if (num < 0)
			{
				num = this.m_Options.Count - 1;
			}
			if (num >= this.m_Options.Count)
			{
				num = 0;
			}
			this.SelectOptionByIndex(num);
		}

		// Token: 0x06002137 RID: 8503 RVA: 0x000A6BA4 File Offset: 0x000A4DA4
		protected void OnNextButtonClick()
		{
			int num = this.selectedOptionIndex + 1;
			if (num < 0)
			{
				num = this.m_Options.Count - 1;
			}
			if (num >= this.m_Options.Count)
			{
				num = 0;
			}
			this.SelectOptionByIndex(num);
		}

		// Token: 0x06002138 RID: 8504 RVA: 0x000A6BE4 File Offset: 0x000A4DE4
		public int GetOptionIndex(string optionValue)
		{
			if (this.m_Options != null && this.m_Options.Count > 0 && !string.IsNullOrEmpty(optionValue))
			{
				for (int i = 0; i < this.m_Options.Count; i++)
				{
					if (optionValue.Equals(this.m_Options[i], StringComparison.OrdinalIgnoreCase))
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x06002139 RID: 8505 RVA: 0x000A6C40 File Offset: 0x000A4E40
		public void SelectOptionByIndex(int index)
		{
			if (index < 0 || index >= this.m_Options.Count)
			{
				return;
			}
			string text = this.m_Options[index];
			if (!text.Equals(this.m_SelectedItem))
			{
				this.m_SelectedItem = text;
				this.TriggerChangeEvent();
			}
		}

		// Token: 0x0600213A RID: 8506 RVA: 0x000A6C88 File Offset: 0x000A4E88
		public void SelectOption(string optionValue)
		{
			if (string.IsNullOrEmpty(optionValue))
			{
				return;
			}
			int optionIndex = this.GetOptionIndex(optionValue);
			if (optionIndex < 0 || optionIndex >= this.m_Options.Count)
			{
				return;
			}
			this.SelectOptionByIndex(optionIndex);
		}

		// Token: 0x0600213B RID: 8507 RVA: 0x000A6CC0 File Offset: 0x000A4EC0
		public void AddOption(string optionValue)
		{
			if (this.m_Options != null)
			{
				this.m_Options.Add(optionValue);
			}
		}

		// Token: 0x0600213C RID: 8508 RVA: 0x000A6CD6 File Offset: 0x000A4ED6
		public void AddOptionAtIndex(string optionValue, int index)
		{
			if (this.m_Options == null)
			{
				return;
			}
			if (index >= this.m_Options.Count)
			{
				this.m_Options.Add(optionValue);
				return;
			}
			this.m_Options.Insert(index, optionValue);
		}

		// Token: 0x0600213D RID: 8509 RVA: 0x000A6D09 File Offset: 0x000A4F09
		public void RemoveOption(string optionValue)
		{
			if (this.m_Options == null)
			{
				return;
			}
			if (this.m_Options.Contains(optionValue))
			{
				this.m_Options.Remove(optionValue);
				this.ValidateSelectedOption();
			}
		}

		// Token: 0x0600213E RID: 8510 RVA: 0x000A6D35 File Offset: 0x000A4F35
		public void RemoveOptionAtIndex(int index)
		{
			if (this.m_Options == null)
			{
				return;
			}
			if (index >= 0 && index < this.m_Options.Count)
			{
				this.m_Options.RemoveAt(index);
				this.ValidateSelectedOption();
			}
		}

		// Token: 0x0600213F RID: 8511 RVA: 0x000A6D64 File Offset: 0x000A4F64
		public void ValidateSelectedOption()
		{
			if (this.m_Options == null)
			{
				return;
			}
			if (!this.m_Options.Contains(this.m_SelectedItem))
			{
				this.SelectOptionByIndex(0);
			}
		}

		// Token: 0x06002140 RID: 8512 RVA: 0x000A6D89 File Offset: 0x000A4F89
		protected virtual void TriggerChangeEvent()
		{
			if (this.m_Text != null)
			{
				this.m_Text.text = this.m_SelectedItem;
			}
			if (this.onChange != null)
			{
				this.onChange.Invoke(this.selectedOptionIndex, this.m_SelectedItem);
			}
		}

		// Token: 0x04001B18 RID: 6936
		[SerializeField]
		private Text m_Text;

		// Token: 0x04001B19 RID: 6937
		[SerializeField]
		private Button m_PrevButton;

		// Token: 0x04001B1A RID: 6938
		[SerializeField]
		private Button m_NextButton;

		// Token: 0x04001B1B RID: 6939
		[HideInInspector]
		[SerializeField]
		private string m_SelectedItem;

		// Token: 0x04001B1C RID: 6940
		[SerializeField]
		private List<string> m_Options = new List<string>();

		// Token: 0x04001B1D RID: 6941
		public UISwitchSelect.ChangeEvent onChange = new UISwitchSelect.ChangeEvent();

		// Token: 0x020005E3 RID: 1507
		[Serializable]
		public class ChangeEvent : UnityEvent<int, string>
		{
		}
	}
}
