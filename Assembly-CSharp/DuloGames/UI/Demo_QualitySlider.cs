using System;
using System.Collections.Generic;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x020005A1 RID: 1441
	public class Demo_QualitySlider : MonoBehaviour
	{
		// Token: 0x06001FD0 RID: 8144 RVA: 0x0009FF64 File Offset: 0x0009E164
		private void Start()
		{
			List<string> list = new List<string>(QualitySettings.names.Length);
			foreach (string item in QualitySettings.names)
			{
				list.Add(item);
			}
			this.m_Slider.options = list;
			this.m_Slider.value = (float)QualitySettings.GetQualityLevel();
		}

		// Token: 0x040019B1 RID: 6577
		[SerializeField]
		private UISliderExtended m_Slider;
	}
}
