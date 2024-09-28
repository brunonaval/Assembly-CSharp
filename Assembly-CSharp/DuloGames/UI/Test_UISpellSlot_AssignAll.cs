using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x020005B6 RID: 1462
	public class Test_UISpellSlot_AssignAll : MonoBehaviour
	{
		// Token: 0x06002022 RID: 8226 RVA: 0x000A1198 File Offset: 0x0009F398
		private void Start()
		{
			if (this.m_Container == null || UISpellDatabase.Instance == null)
			{
				this.Destruct();
				return;
			}
			UISpellSlot[] componentsInChildren = this.m_Container.gameObject.GetComponentsInChildren<UISpellSlot>();
			UISpellInfo[] spells = UISpellDatabase.Instance.spells;
			if (componentsInChildren.Length != 0 && spells.Length != 0)
			{
				UISpellSlot[] array = componentsInChildren;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].Assign(spells[UnityEngine.Random.Range(0, spells.Length)]);
				}
			}
			this.Destruct();
		}

		// Token: 0x06002023 RID: 8227 RVA: 0x00003485 File Offset: 0x00001685
		private void Destruct()
		{
			UnityEngine.Object.DestroyImmediate(this);
		}

		// Token: 0x040019F5 RID: 6645
		[SerializeField]
		private Transform m_Container;
	}
}
