using System;
using UnityEngine;
using UnityEngine.Events;

namespace DuloGames.UI
{
	// Token: 0x020005A4 RID: 1444
	public class Demo_ResolutionSelect : MonoBehaviour
	{
		// Token: 0x06001FDA RID: 8154 RVA: 0x000A0198 File Offset: 0x0009E398
		protected void Start()
		{
			if (this.m_SelectField == null)
			{
				return;
			}
			this.m_SelectField.ClearOptions();
			foreach (Resolution resolution in Screen.resolutions)
			{
				this.m_SelectField.AddOption(string.Concat(new string[]
				{
					resolution.width.ToString(),
					"x",
					resolution.height.ToString(),
					" @ ",
					resolution.refreshRateRatio.ToString(),
					"Hz"
				}));
			}
			Resolution currentResolution = Screen.currentResolution;
			this.m_SelectField.SelectOption(string.Concat(new string[]
			{
				currentResolution.width.ToString(),
				"x",
				currentResolution.height.ToString(),
				" @ ",
				currentResolution.refreshRateRatio.ToString(),
				"Hz"
			}));
		}

		// Token: 0x06001FDB RID: 8155 RVA: 0x000A02B9 File Offset: 0x0009E4B9
		protected void OnEnable()
		{
			if (this.m_SelectField == null)
			{
				return;
			}
			this.m_SelectField.onChange.AddListener(new UnityAction<int, string>(this.OnSelectedOption));
		}

		// Token: 0x06001FDC RID: 8156 RVA: 0x000A02E6 File Offset: 0x0009E4E6
		protected void OnDisable()
		{
			if (this.m_SelectField == null)
			{
				return;
			}
			this.m_SelectField.onChange.RemoveListener(new UnityAction<int, string>(this.OnSelectedOption));
		}

		// Token: 0x06001FDD RID: 8157 RVA: 0x000A0314 File Offset: 0x0009E514
		protected void OnSelectedOption(int index, string option)
		{
			Resolution resolution = Screen.resolutions[index];
			if (resolution.Equals(Screen.currentResolution))
			{
				return;
			}
			Screen.SetResolution(resolution.width, resolution.height, FullScreenMode.FullScreenWindow, resolution.refreshRateRatio);
		}

		// Token: 0x040019B8 RID: 6584
		[SerializeField]
		private UISelectField m_SelectField;
	}
}
