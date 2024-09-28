using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x02000632 RID: 1586
	[AddComponentMenu("UI/Raycast Filters/Ignore Raycast Filter")]
	public class UIIgnoreRaycast : MonoBehaviour, ICanvasRaycastFilter
	{
		// Token: 0x060022EA RID: 8938 RVA: 0x00002076 File Offset: 0x00000276
		public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
		{
			return false;
		}
	}
}
