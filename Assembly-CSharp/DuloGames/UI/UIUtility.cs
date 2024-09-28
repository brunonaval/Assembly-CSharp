using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x0200066E RID: 1646
	public static class UIUtility
	{
		// Token: 0x06002485 RID: 9349 RVA: 0x000B2A35 File Offset: 0x000B0C35
		public static void BringToFront(GameObject go)
		{
			UIUtility.BringToFront(go, true);
		}

		// Token: 0x06002486 RID: 9350 RVA: 0x000B2A40 File Offset: 0x000B0C40
		public static void BringToFront(GameObject go, bool allowReparent)
		{
			Transform transform = null;
			UIScene uiscene = UIUtility.FindInParents<UIScene>(go);
			if (uiscene != null && uiscene.content != null)
			{
				transform = uiscene.content;
			}
			else
			{
				Canvas canvas = UIUtility.FindInParents<Canvas>(go);
				if (canvas != null)
				{
					transform = canvas.transform;
				}
			}
			if (allowReparent && transform != null)
			{
				go.transform.SetParent(transform, true);
			}
			go.transform.SetAsLastSibling();
			if (transform != null)
			{
				UIAlwaysOnTop[] componentsInChildren = transform.gameObject.GetComponentsInChildren<UIAlwaysOnTop>();
				if (componentsInChildren.Length != 0)
				{
					Array.Sort<UIAlwaysOnTop>(componentsInChildren);
					UIAlwaysOnTop[] array = componentsInChildren;
					for (int i = 0; i < array.Length; i++)
					{
						array[i].transform.SetAsLastSibling();
					}
				}
			}
		}

		// Token: 0x06002487 RID: 9351 RVA: 0x000B2AF8 File Offset: 0x000B0CF8
		public static T FindInParents<T>(GameObject go) where T : Component
		{
			if (go == null)
			{
				return default(T);
			}
			T component = go.GetComponent<T>();
			if (component != null)
			{
				return component;
			}
			Transform parent = go.transform.parent;
			while (parent != null && component == null)
			{
				component = parent.gameObject.GetComponent<T>();
				parent = parent.parent;
			}
			return component;
		}
	}
}
