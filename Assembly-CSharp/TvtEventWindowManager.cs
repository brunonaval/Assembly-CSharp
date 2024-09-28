using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000297 RID: 663
public class TvtEventWindowManager : MonoBehaviour
{
	// Token: 0x06000A66 RID: 2662 RVA: 0x0002F8E2 File Offset: 0x0002DAE2
	private void Start()
	{
		base.StartCoroutine(this.CountdownTimer());
	}

	// Token: 0x06000A67 RID: 2663 RVA: 0x0002F8F1 File Offset: 0x0002DAF1
	private IEnumerator CountdownTimer()
	{
		float counter = 600f;
		WaitForSeconds waitForCountdown = new WaitForSeconds(0.05f);
		do
		{
			counter -= 0.05f;
			float fillAmount = counter * 100f / 600f * 0.01f;
			this.timerFillImage.fillAmount = fillAmount;
			yield return waitForCountdown;
		}
		while (counter > 0f);
		this.Reject();
		yield break;
	}

	// Token: 0x06000A68 RID: 2664 RVA: 0x0002F900 File Offset: 0x0002DB00
	public void Participate()
	{
		this.uiSystemModule.MovementModule.CmdTeleport("tvt_arena_entrance", default(Effect));
		base.GetComponent<DragWindowModule>().CloseWindow();
	}

	// Token: 0x06000A69 RID: 2665 RVA: 0x0002C52C File Offset: 0x0002A72C
	public void Reject()
	{
		base.GetComponent<DragWindowModule>().CloseWindow();
	}

	// Token: 0x06000A6A RID: 2666 RVA: 0x0002F936 File Offset: 0x0002DB36
	public void LearnMore()
	{
		if (this.tvtEventDetailsWindow.activeInHierarchy)
		{
			return;
		}
		this.tvtEventDetailsWindow.SetActive(true);
	}

	// Token: 0x04000BE9 RID: 3049
	private const int DURATION = 600;

	// Token: 0x04000BEA RID: 3050
	private const float COUNTDOWN_SPEED = 0.05f;

	// Token: 0x04000BEB RID: 3051
	[SerializeField]
	private Image timerFillImage;

	// Token: 0x04000BEC RID: 3052
	[SerializeField]
	private GameObject tvtEventDetailsWindow;

	// Token: 0x04000BED RID: 3053
	[SerializeField]
	private UISystemModule uiSystemModule;
}
