using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x020005AC RID: 1452
	public class Test_SelectAddOption : MonoBehaviour
	{
		// Token: 0x06002005 RID: 8197 RVA: 0x000A0B03 File Offset: 0x0009ED03
		[ContextMenu("Add Option")]
		public void AddOption()
		{
			if (this.m_SelectField != null)
			{
				this.m_SelectField.AddOption(this.m_Text);
			}
		}

		// Token: 0x040019D2 RID: 6610
		[SerializeField]
		private UISelectField m_SelectField;

		// Token: 0x040019D3 RID: 6611
		[SerializeField]
		private string m_Text;
	}
}
