using System;
using UnityEngine;

// Token: 0x02000565 RID: 1381
public class MoveToMousePos : MonoBehaviour
{
	// Token: 0x06001ECD RID: 7885 RVA: 0x00099C56 File Offset: 0x00097E56
	private void Start()
	{
		this.newPosition = base.transform.position;
	}

	// Token: 0x06001ECE RID: 7886 RVA: 0x00099C6C File Offset: 0x00097E6C
	private void Update()
	{
		RaycastHit raycastHit;
		if (Input.GetMouseButton(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit))
		{
			this.newPosition = raycastHit.point;
		}
		Vector3 vector = base.transform.InverseTransformPoint(this.newPosition);
		float num = Mathf.Atan2(vector.x, vector.y) * 57.29578f;
		base.transform.Rotate(0f, 0f, -num * Time.deltaTime * 50f);
		Quaternion rotation = Quaternion.LookRotation(Vector3.forward);
		this.spriteTransform.rotation = rotation;
		if (this.canMove && Vector2.Distance(base.transform.position, this.newPosition) < 1f)
		{
			this.canMove = false;
		}
		else if (!this.canMove && Vector2.Distance(base.transform.position, this.newPosition) > 3f)
		{
			this.canMove = true;
		}
		base.transform.position += base.transform.up * this.speed * Time.deltaTime * 50f * 0.1f;
		if (this.canMove)
		{
			this.speed = Mathf.Clamp01(this.speed + 0.05f * Time.deltaTime * 50f);
			return;
		}
		this.speed = Mathf.Clamp01(this.speed - 0.1f * Time.deltaTime * 50f);
	}

	// Token: 0x0400189D RID: 6301
	private Vector3 newPosition;

	// Token: 0x0400189E RID: 6302
	private bool canMove = true;

	// Token: 0x0400189F RID: 6303
	public Transform spriteTransform;

	// Token: 0x040018A0 RID: 6304
	private float speed;
}
