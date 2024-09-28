using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200026E RID: 622
public class BossEventWindowManager : MonoBehaviour
{
	// Token: 0x06000976 RID: 2422 RVA: 0x0002C478 File Offset: 0x0002A678
	public void Initialize(string bossName, int bossLevel, Vector2 bossLocation)
	{
		this.bossLevel = bossLevel;
		this.bossName = LanguageManager.Instance.GetText(bossName);
		this.bossLocation = bossLocation;
		this.descriptionText.text = string.Format(LanguageManager.Instance.GetText("boss_event_invite_message"), this.bossName, this.bossLevel);
		base.StartCoroutine(this.CountdownTimer());
	}

	// Token: 0x06000977 RID: 2423 RVA: 0x0002C4E1 File Offset: 0x0002A6E1
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

	// Token: 0x06000978 RID: 2424 RVA: 0x0002C4F0 File Offset: 0x0002A6F0
	public void Accept()
	{
		this.uiSystemModule.MovementModule.CmdTeleport(this.bossLocation, default(Effect));
		base.GetComponent<DragWindowModule>().CloseWindow();
	}

	// Token: 0x06000979 RID: 2425 RVA: 0x0002C52C File Offset: 0x0002A72C
	public void Reject()
	{
		base.GetComponent<DragWindowModule>().CloseWindow();
	}

	// Token: 0x04000B14 RID: 2836
	private const int DURATION = 600;

	// Token: 0x04000B15 RID: 2837
	private const float COUNTDOWN_SPEED = 0.05f;

	// Token: 0x04000B16 RID: 2838
	[SerializeField]
	private Image timerFillImage;

	// Token: 0x04000B17 RID: 2839
	[SerializeField]
	private UISystemModule uiSystemModule;

	// Token: 0x04000B18 RID: 2840
	[SerializeField]
	private Text descriptionText;

	// Token: 0x04000B19 RID: 2841
	private int bossLevel;

	// Token: 0x04000B1A RID: 2842
	private string bossName;

	// Token: 0x04000B1B RID: 2843
	private Vector2 bossLocation;
}
