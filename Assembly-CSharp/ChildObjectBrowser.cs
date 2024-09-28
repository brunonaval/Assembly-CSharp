using System;
using UnityEngine;

// Token: 0x02000564 RID: 1380
public class ChildObjectBrowser : MonoBehaviour
{
	// Token: 0x06001EC9 RID: 7881 RVA: 0x00099BA0 File Offset: 0x00097DA0
	public void NextChild(int next)
	{
		this.DisableAll();
		this.childIndex += next;
		if (this.childIndex < 0)
		{
			this.childIndex = base.transform.childCount - 1;
		}
		else
		{
			this.childIndex %= base.transform.childCount;
		}
		this.EnableChild(this.childIndex);
	}

	// Token: 0x06001ECA RID: 7882 RVA: 0x00099C03 File Offset: 0x00097E03
	private void EnableChild(int index)
	{
		base.transform.GetChild(index).gameObject.SetActive(true);
	}

	// Token: 0x06001ECB RID: 7883 RVA: 0x00099C1C File Offset: 0x00097E1C
	private void DisableAll()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			base.transform.GetChild(i).gameObject.SetActive(false);
		}
	}

	// Token: 0x0400189C RID: 6300
	private int childIndex;
}
