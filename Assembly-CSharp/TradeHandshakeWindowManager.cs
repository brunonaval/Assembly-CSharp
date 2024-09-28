using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000295 RID: 661
public class TradeHandshakeWindowManager : MonoBehaviour
{
	// Token: 0x06000A5A RID: 2650 RVA: 0x0002F72E File Offset: 0x0002D92E
	private IEnumerator CountdownTimer()
	{
		float duration = 15f;
		float counter = duration;
		float countdownSpeed = 0.05f;
		WaitForSeconds waitForCountdown = new WaitForSeconds(countdownSpeed);
		do
		{
			counter -= countdownSpeed;
			float fillAmount = counter * 100f / duration * 0.01f;
			this.timerFillImage.fillAmount = fillAmount;
			this.UpdateTradeMessage(this.trader);
			yield return waitForCountdown;
		}
		while (counter > 0f);
		this.Reject();
		yield break;
	}

	// Token: 0x06000A5B RID: 2651 RVA: 0x0002F73D File Offset: 0x0002D93D
	public void Accept()
	{
		this.uiSystemModule.TradeModule.CmdAcceptTrade(this.trader);
		this.trader = null;
		this.uiSystemModule.HideTradeHandshakeWindow();
	}

	// Token: 0x06000A5C RID: 2652 RVA: 0x0002F767 File Offset: 0x0002D967
	public void Reject()
	{
		this.uiSystemModule.TradeModule.CmdRejectTrade(this.trader);
		this.trader = null;
		this.uiSystemModule.HideTradeHandshakeWindow();
	}

	// Token: 0x06000A5D RID: 2653 RVA: 0x0002F791 File Offset: 0x0002D991
	public void SetTrader(GameObject trader)
	{
		if (trader == null)
		{
			return;
		}
		this.trader = trader;
		this.UpdateTradeMessage(trader);
		base.StartCoroutine(this.CountdownTimer());
	}

	// Token: 0x06000A5E RID: 2654 RVA: 0x0002F7B8 File Offset: 0x0002D9B8
	private void UpdateTradeMessage(GameObject trader)
	{
		CreatureModule creatureModule;
		trader.TryGetComponent<CreatureModule>(out creatureModule);
		this.invitationMessage.text = string.Format(LanguageManager.Instance.GetText("trade_handshake_message"), creatureModule.CreatureName);
	}

	// Token: 0x04000BDE RID: 3038
	[SerializeField]
	private Image timerFillImage;

	// Token: 0x04000BDF RID: 3039
	[SerializeField]
	private Text invitationMessage;

	// Token: 0x04000BE0 RID: 3040
	private GameObject trader;

	// Token: 0x04000BE1 RID: 3041
	[SerializeField]
	private UISystemModule uiSystemModule;
}
