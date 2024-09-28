using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001D4 RID: 468
public class AntiAfkWindowManager : MonoBehaviour
{
	// Token: 0x0600056F RID: 1391 RVA: 0x0001D0C9 File Offset: 0x0001B2C9
	private void Start()
	{
		this.attempts = 0;
	}

	// Token: 0x06000570 RID: 1392 RVA: 0x0001D0D2 File Offset: 0x0001B2D2
	private void OnDisable()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x06000571 RID: 1393 RVA: 0x0001D0DA File Offset: 0x0001B2DA
	private void OnEnable()
	{
		this.StartCountdown();
	}

	// Token: 0x06000572 RID: 1394 RVA: 0x0001D0E4 File Offset: 0x0001B2E4
	private void StartCountdown()
	{
		this.validToken = this.GenerateRandomToken();
		if (this.attempts != 0)
		{
			this.message.text = string.Format(LanguageManager.Instance.GetText("anti_afk_try_again_message"), this.validToken);
		}
		else
		{
			this.message.text = string.Format(LanguageManager.Instance.GetText("anti_afk_message"), this.validToken);
		}
		int num = UnityEngine.Random.Range(0, this.optionButtons.Length);
		for (int i = 0; i < this.optionButtons.Length; i++)
		{
			this.optionButtons[i].onClick.RemoveAllListeners();
			if (i == num)
			{
				this.optionButtons[i].onClick.AddListener(new UnityAction(this.ValidButtonClick));
				Text componentInChildren = this.optionButtons[i].GetComponentInChildren<Text>();
				componentInChildren.text = this.validToken;
				componentInChildren.color = Color.white;
			}
			else
			{
				this.optionButtons[i].onClick.AddListener(new UnityAction(this.InvalidButtonClick));
				Text componentInChildren2 = this.optionButtons[i].GetComponentInChildren<Text>();
				componentInChildren2.text = this.GenerateRandomToken();
				componentInChildren2.color = Color.white;
			}
		}
		base.StartCoroutine(this.CountdownTimer());
	}

	// Token: 0x06000573 RID: 1395 RVA: 0x0001D220 File Offset: 0x0001B420
	private void ValidButtonClick()
	{
		this.attempts = 0;
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000574 RID: 1396 RVA: 0x0001D235 File Offset: 0x0001B435
	private void InvalidButtonClick()
	{
		this.attempts++;
		if (this.attempts <= 2)
		{
			base.StopAllCoroutines();
			this.StartCountdown();
			return;
		}
		this.DisconnectClient();
	}

	// Token: 0x06000575 RID: 1397 RVA: 0x0001D261 File Offset: 0x0001B461
	private void DisconnectClient()
	{
		GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManagerModule>().StopClient();
	}

	// Token: 0x06000576 RID: 1398 RVA: 0x0001D278 File Offset: 0x0001B478
	private string GenerateRandomToken()
	{
		return Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5).ToUpper();
	}

	// Token: 0x06000577 RID: 1399 RVA: 0x0001D2B3 File Offset: 0x0001B4B3
	private IEnumerator CountdownTimer()
	{
		float counter = this.duration;
		WaitForSecondsRealtime delay = new WaitForSecondsRealtime(this.countdownSpeed);
		do
		{
			counter -= this.countdownSpeed;
			float fillAmount = counter * 100f / this.duration * 0.01f;
			this.timerFillImage.fillAmount = fillAmount;
			yield return delay;
		}
		while (counter > 0f);
		this.DisconnectClient();
		yield break;
	}

	// Token: 0x040007E3 RID: 2019
	[SerializeField]
	private Image timerFillImage;

	// Token: 0x040007E4 RID: 2020
	[SerializeField]
	private Button[] optionButtons;

	// Token: 0x040007E5 RID: 2021
	[SerializeField]
	private Text message;

	// Token: 0x040007E6 RID: 2022
	private float countdownSpeed = 0.1f;

	// Token: 0x040007E7 RID: 2023
	private float duration = 180f;

	// Token: 0x040007E8 RID: 2024
	private string validToken;

	// Token: 0x040007E9 RID: 2025
	private int attempts;
}
