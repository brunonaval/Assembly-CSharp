using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000211 RID: 529
public class MobileChangeSkillbarButtonManager : MonoBehaviour
{
	// Token: 0x06000724 RID: 1828 RVA: 0x00023127 File Offset: 0x00021327
	private void Awake()
	{
		this.currentIndex = 0;
		this.EnableSkillbarByIndex(this.currentIndex);
	}

	// Token: 0x06000725 RID: 1829 RVA: 0x0002313C File Offset: 0x0002133C
	public void OnClick()
	{
		this.currentIndex++;
		if (this.currentIndex >= this.skillbars.Length)
		{
			this.currentIndex = 0;
		}
		this.EnableSkillbarByIndex(this.currentIndex);
	}

	// Token: 0x06000726 RID: 1830 RVA: 0x00023170 File Offset: 0x00021370
	private void EnableSkillbarByIndex(int skillbarIndex)
	{
		this.skillbarIndexText.text = (skillbarIndex + 1).ToString();
		for (int i = 0; i < this.skillbars.Length; i++)
		{
			if (i == skillbarIndex)
			{
				this.skillbars[i].SetActive(true);
			}
			else
			{
				this.skillbars[i].SetActive(false);
			}
		}
	}

	// Token: 0x04000923 RID: 2339
	[SerializeField]
	private GameObject[] skillbars;

	// Token: 0x04000924 RID: 2340
	[SerializeField]
	private Text skillbarIndexText;

	// Token: 0x04000925 RID: 2341
	private int currentIndex;
}
