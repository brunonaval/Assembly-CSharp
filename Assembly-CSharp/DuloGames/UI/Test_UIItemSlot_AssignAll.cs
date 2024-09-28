using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x020005B1 RID: 1457
	public class Test_UIItemSlot_AssignAll : MonoBehaviour
	{
		// Token: 0x06002014 RID: 8212 RVA: 0x000A0E20 File Offset: 0x0009F020
		private void Start()
		{
			if (this.m_Container == null || UIItemDatabase.Instance == null)
			{
				this.Destruct();
				return;
			}
			UIItemSlot[] componentsInChildren = this.m_Container.gameObject.GetComponentsInChildren<UIItemSlot>();
			UIItemInfo[] items = UIItemDatabase.Instance.items;
			if (componentsInChildren.Length != 0 && items.Length != 0)
			{
				UIItemSlot[] array = componentsInChildren;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].Assign(items[UnityEngine.Random.Range(0, items.Length)]);
				}
			}
			this.Destruct();
		}

		// Token: 0x06002015 RID: 8213 RVA: 0x00003485 File Offset: 0x00001685
		private void Destruct()
		{
			UnityEngine.Object.DestroyImmediate(this);
		}

		// Token: 0x040019E3 RID: 6627
		[SerializeField]
		private Transform m_Container;
	}
}
