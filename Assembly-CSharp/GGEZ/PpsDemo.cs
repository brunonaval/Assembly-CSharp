using System;
using UnityEngine;
using UnityEngine.UI;

namespace GGEZ
{
	// Token: 0x020006BF RID: 1727
	public class PpsDemo : MonoBehaviour
	{
		// Token: 0x060025C9 RID: 9673 RVA: 0x000B5168 File Offset: 0x000B3368
		public void SwitchModes()
		{
			this.statementIndex = 0;
			PpsDemo.DemoMode demoMode = this.mode;
			if (demoMode == PpsDemo.DemoMode.Character)
			{
				this.mode = PpsDemo.DemoMode.Checkerboard;
				this.TitleText.text = "Checkerboard";
				this.SwitchModesButtonText.text = "View Character";
				return;
			}
			if (demoMode != PpsDemo.DemoMode.Checkerboard)
			{
				return;
			}
			this.mode = PpsDemo.DemoMode.Character;
			this.TitleText.text = "Multi-Part Character";
			this.SwitchModesButtonText.text = "View Checkerboard";
		}

		// Token: 0x060025CA RID: 9674 RVA: 0x000B51DA File Offset: 0x000B33DA
		private void Start()
		{
			this.SwitchModes();
		}

		// Token: 0x060025CB RID: 9675 RVA: 0x000B51E2 File Offset: 0x000B33E2
		public void ShowNextStatement()
		{
			this.statementIndex++;
		}

		// Token: 0x060025CC RID: 9676 RVA: 0x000B51F4 File Offset: 0x000B33F4
		private static void SetGroupActive(GameObject[] gameObjects, bool active)
		{
			for (int i = 0; i < gameObjects.Length; i++)
			{
				gameObjects[i].SetActive(active);
			}
		}

		// Token: 0x060025CD RID: 9677 RVA: 0x000B521C File Offset: 0x000B341C
		private void Update()
		{
			this.MainCamera.orthographicSize = (float)this.MainCamera.pixelHeight * 0.5f / 48f;
			this.MainCamera.transform.position = Vector3.back * 10f;
			PpsDemo.SetGroupActive(this.CheckerboardObjects, this.mode == PpsDemo.DemoMode.Checkerboard);
			PpsDemo.SetGroupActive(this.CharacterObjects, this.mode == PpsDemo.DemoMode.Character);
			Vector3 position = new Vector3(1f * Mathf.Cos(Time.time * 0.5f) + this.MainCamera.transform.position.x - 0.5f, 0.25f * Mathf.Sin(Time.time * 0.5f) + this.MainCamera.transform.position.y - 0.5f, 0f);
			Transform[] movingObjects = this.MovingObjects;
			for (int i = 0; i < movingObjects.Length; i++)
			{
				movingObjects[i].position = position;
			}
			PpsDemo.DemoMode demoMode = this.mode;
			if (demoMode != PpsDemo.DemoMode.Character)
			{
				if (demoMode == PpsDemo.DemoMode.Checkerboard)
				{
					switch (this.statementIndex % 8)
					{
					case 0:
						this.BodyText.text = "This demo reveals texturing issues with sprites that are off the screen's pixel grid. Perfect Pixel Sprite fixes these by aligning your sprite container to this grid. Press \"more\" to keep reading...";
						return;
					case 1:
						this.BodyText.text = "The checkerboard sprite is surrounded by pink to make edge bleeding obvious. The 'bilinear' row uses bilinear texture filtering and the 'point' row uses point texture filtering...";
						return;
					case 2:
						this.BodyText.text = "The 'sliding' column is how sprites look without any adjustments. The 'fixed' column shows sprites that are aligned by Perfect Pixel Sprite...";
						return;
					case 3:
						this.BodyText.text = "Watch the edges of the sliding-bilinear checkerboard (top left) as it circles. Do you notice how the texture seems disconnected from its edges? Compare that to how solid the fixed-bilinear version in the top right appears...";
						return;
					case 4:
						this.BodyText.text = "Normally, this effect is hard to notice. This demo is zoomed in 3x to reveal it easily. However, point filtering causes a glitch that can be very obvious even at normal scale...";
						return;
					case 5:
						this.BodyText.text = "Every once in a while, a pink line might appear on the border of the sliding-point checkerboard (bottom left). Not all platforms do this, but many do. Enable the <b>Pause If Pink</b> component on the Main Camera to catch the frame when it happens. Disable and unpause to continue...";
						return;
					case 6:
						this.BodyText.text = "You might be wondering, \"Doesn't Unity have a Pixel Snap feature?\" The short answer is that it doesn't help. You can give that a try by changing the material of the sliding column to Sprites-Default-PixelSnapOn.";
						return;
					case 7:
						this.BodyText.text = "(click View Character for the other part of this demo)";
						return;
					default:
						return;
					}
				}
			}
			else
			{
				switch (this.statementIndex % 4)
				{
				case 0:
					this.BodyText.text = "The king is made of 3 layers. As he moves, the texture issues seen only at the edges of a single sprite occur inside of the character...";
					return;
				case 1:
					this.BodyText.text = "To get pixel-perfect rendering also requires care in setting up your texture assets and game camera. Check out ggez.org for details!";
					return;
				case 2:
					this.BodyText.text = "(click View Checkerboard for the other part of this demo)";
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x04001EEA RID: 7914
		public Camera MainCamera;

		// Token: 0x04001EEB RID: 7915
		public Text TitleText;

		// Token: 0x04001EEC RID: 7916
		public Text BodyText;

		// Token: 0x04001EED RID: 7917
		public Text SwitchModesButtonText;

		// Token: 0x04001EEE RID: 7918
		private PpsDemo.DemoMode mode;

		// Token: 0x04001EEF RID: 7919
		public Transform[] MovingObjects;

		// Token: 0x04001EF0 RID: 7920
		public GameObject[] CheckerboardObjects;

		// Token: 0x04001EF1 RID: 7921
		public GameObject[] CharacterObjects;

		// Token: 0x04001EF2 RID: 7922
		private int statementIndex;

		// Token: 0x020006C0 RID: 1728
		private enum DemoMode
		{
			// Token: 0x04001EF4 RID: 7924
			Character,
			// Token: 0x04001EF5 RID: 7925
			Checkerboard
		}
	}
}
