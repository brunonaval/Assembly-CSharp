using System;
using UnityEngine;

// Token: 0x02000566 RID: 1382
public class Rotate : MonoBehaviour
{
	// Token: 0x06001ED0 RID: 7888 RVA: 0x00099E26 File Offset: 0x00098026
	private void FixedUpdate()
	{
		base.transform.Rotate(Vector3.forward * this.speed * Time.deltaTime);
	}

	// Token: 0x040018A1 RID: 6305
	public float speed = 100f;
}
