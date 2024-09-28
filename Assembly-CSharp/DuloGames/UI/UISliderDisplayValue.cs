using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x02000624 RID: 1572
	public class UISliderDisplayValue : MonoBehaviour
	{
		// Token: 0x060022A5 RID: 8869 RVA: 0x000AB8ED File Offset: 0x000A9AED
		protected void Awake()
		{
			if (this.m_slider == null)
			{
				this.m_slider = base.gameObject.GetComponent<Slider>();
			}
		}

		// Token: 0x060022A6 RID: 8870 RVA: 0x000AB90E File Offset: 0x000A9B0E
		protected void OnEnable()
		{
			if (this.m_slider != null)
			{
				this.m_slider.onValueChanged.AddListener(new UnityAction<float>(this.SetValue));
				this.SetValue(this.m_slider.value);
			}
		}

		// Token: 0x060022A7 RID: 8871 RVA: 0x000AB94B File Offset: 0x000A9B4B
		protected void OnDisable()
		{
			if (this.m_slider != null)
			{
				this.m_slider.onValueChanged.RemoveListener(new UnityAction<float>(this.SetValue));
			}
		}

		// Token: 0x060022A8 RID: 8872 RVA: 0x000AB978 File Offset: 0x000A9B78
		public void SetValue(float value)
		{
			if (this.m_Text != null)
			{
				if (this.m_Display == UISliderDisplayValue.DisplayValue.Percentage)
				{
					this.m_Text.text = (value * 100f).ToString(this.m_Format) + this.m_Append;
					return;
				}
				this.m_Text.text = value.ToString(this.m_Format) + this.m_Append;
			}
		}

		// Token: 0x04001C1F RID: 7199
		[SerializeField]
		private Slider m_slider;

		// Token: 0x04001C20 RID: 7200
		[SerializeField]
		private Text m_Text;

		// Token: 0x04001C21 RID: 7201
		[SerializeField]
		private UISliderDisplayValue.DisplayValue m_Display = UISliderDisplayValue.DisplayValue.Percentage;

		// Token: 0x04001C22 RID: 7202
		[SerializeField]
		private string m_Format = "0";

		// Token: 0x04001C23 RID: 7203
		[SerializeField]
		private string m_Append = "%";

		// Token: 0x02000625 RID: 1573
		public enum DisplayValue
		{
			// Token: 0x04001C25 RID: 7205
			Raw,
			// Token: 0x04001C26 RID: 7206
			Percentage
		}
	}
}
