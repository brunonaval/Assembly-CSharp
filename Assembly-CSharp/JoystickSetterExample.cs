using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000006 RID: 6
public class JoystickSetterExample : MonoBehaviour
{
	// Token: 0x06000010 RID: 16 RVA: 0x000022BB File Offset: 0x000004BB
	public void ModeChanged(int index)
	{
		switch (index)
		{
		case 0:
			this.variableJoystick.SetMode(JoystickType.Fixed);
			return;
		case 1:
			this.variableJoystick.SetMode(JoystickType.Floating);
			return;
		case 2:
			this.variableJoystick.SetMode(JoystickType.Dynamic);
			return;
		default:
			return;
		}
	}

	// Token: 0x06000011 RID: 17 RVA: 0x000022F8 File Offset: 0x000004F8
	public void AxisChanged(int index)
	{
		switch (index)
		{
		case 0:
			this.variableJoystick.AxisOptions = AxisOptions.Both;
			this.background.sprite = this.axisSprites[index];
			return;
		case 1:
			this.variableJoystick.AxisOptions = AxisOptions.Horizontal;
			this.background.sprite = this.axisSprites[index];
			return;
		case 2:
			this.variableJoystick.AxisOptions = AxisOptions.Vertical;
			this.background.sprite = this.axisSprites[index];
			return;
		default:
			return;
		}
	}

	// Token: 0x06000012 RID: 18 RVA: 0x00002377 File Offset: 0x00000577
	public void SnapX(bool value)
	{
		this.variableJoystick.SnapX = value;
	}

	// Token: 0x06000013 RID: 19 RVA: 0x00002385 File Offset: 0x00000585
	public void SnapY(bool value)
	{
		this.variableJoystick.SnapY = value;
	}

	// Token: 0x06000014 RID: 20 RVA: 0x00002394 File Offset: 0x00000594
	private void Update()
	{
		this.valueText.text = "Current Value: " + this.variableJoystick.Direction.ToString();
	}

	// Token: 0x04000007 RID: 7
	public VariableJoystick variableJoystick;

	// Token: 0x04000008 RID: 8
	public Text valueText;

	// Token: 0x04000009 RID: 9
	public Image background;

	// Token: 0x0400000A RID: 10
	public Sprite[] axisSprites;
}
