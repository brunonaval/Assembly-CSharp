using System;
using UnityEngine;

// Token: 0x0200001F RID: 31
[ExecuteInEditMode]
[AddComponentMenu("Break Prefab Connection")]
public class BreakPrefabConnection : MonoBehaviour
{
	// Token: 0x06000073 RID: 115 RVA: 0x00003485 File Offset: 0x00001685
	private void Start()
	{
		UnityEngine.Object.DestroyImmediate(this);
	}
}
