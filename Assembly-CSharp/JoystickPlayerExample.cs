using System;
using UnityEngine;

// Token: 0x02000005 RID: 5
public class JoystickPlayerExample : MonoBehaviour
{
	// Token: 0x0600000E RID: 14 RVA: 0x0000225C File Offset: 0x0000045C
	public void FixedUpdate()
	{
		Vector3 a = Vector3.forward * this.variableJoystick.Vertical + Vector3.right * this.variableJoystick.Horizontal;
		this.rb.AddForce(a * this.speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
	}

	// Token: 0x04000004 RID: 4
	public float speed;

	// Token: 0x04000005 RID: 5
	public VariableJoystick variableJoystick;

	// Token: 0x04000006 RID: 6
	public Rigidbody rb;
}
