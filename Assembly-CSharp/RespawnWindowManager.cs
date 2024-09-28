using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200028D RID: 653
public class RespawnWindowManager : MonoBehaviour
{
	// Token: 0x06000A35 RID: 2613 RVA: 0x0002F208 File Offset: 0x0002D408
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x06000A36 RID: 2614 RVA: 0x0002F22C File Offset: 0x0002D42C
	private void OnEnable()
	{
		base.StartCoroutine(this.CountdownTimer());
	}

	// Token: 0x06000A37 RID: 2615 RVA: 0x0002F23B File Offset: 0x0002D43B
	private IEnumerator CountdownTimer()
	{
		float counter = (float)this.duration;
		WaitForSeconds waitForCountdown = new WaitForSeconds(this.countdownSpeed);
		do
		{
			counter -= this.countdownSpeed;
			float fillAmount = counter * 100f / (float)this.duration * 0.01f;
			this.timerFillImage.fillAmount = fillAmount;
			yield return waitForCountdown;
		}
		while (counter > 0f & !this.uiSystemModule.CreatureModule.IsAlive);
		if (!this.uiSystemModule.CreatureModule.IsAlive)
		{
			this.Respawn();
		}
		this.uiSystemModule.HideRespawnWindow();
		yield break;
	}

	// Token: 0x06000A38 RID: 2616 RVA: 0x0002F24A File Offset: 0x0002D44A
	public void Respawn()
	{
		this.uiSystemModule.PlayerModule.CmdRespawn();
	}

	// Token: 0x04000BBA RID: 3002
	[SerializeField]
	private int duration = 60;

	// Token: 0x04000BBB RID: 3003
	[SerializeField]
	private float countdownSpeed = 0.1f;

	// Token: 0x04000BBC RID: 3004
	[SerializeField]
	private Image timerFillImage;

	// Token: 0x04000BBD RID: 3005
	private UISystemModule uiSystemModule;
}
