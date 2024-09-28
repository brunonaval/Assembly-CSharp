using System;
using UnityEngine;

// Token: 0x02000272 RID: 626
public class DailyRewardWindowManager : MonoBehaviour
{
	// Token: 0x0600098B RID: 2443 RVA: 0x0002C8C0 File Offset: 0x0002AAC0
	private void OnEnable()
	{
		for (int i = 0; i < this.dailyRewardHolder.transform.childCount; i++)
		{
			Transform child = this.dailyRewardHolder.transform.GetChild(i);
			if (child != null)
			{
				UnityEngine.Object.Destroy(child.gameObject);
			}
		}
		foreach (DailyReward dailyReward in this.uiSystemModule.DailyRewardModule.Rewards)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.dailyRewardSlotPrefab);
			DailyRewardSlotManager component = gameObject.GetComponent<DailyRewardSlotManager>();
			if (component != null)
			{
				component.SetDailyReward(dailyReward, this.uiSystemModule.DailyRewardModule.DailyRewardId);
				gameObject.transform.SetParent(this.dailyRewardHolder.transform, false);
			}
		}
	}

	// Token: 0x0600098C RID: 2444 RVA: 0x0002C9A8 File Offset: 0x0002ABA8
	public void OnTakeRewardButtonClick()
	{
		this.uiSystemModule.DailyRewardModule.CmdTakeReward();
		this.uiSystemModule.ToggleDailyRewardWindow();
	}

	// Token: 0x04000B28 RID: 2856
	[SerializeField]
	private GameObject dailyRewardSlotPrefab;

	// Token: 0x04000B29 RID: 2857
	[SerializeField]
	private GameObject dailyRewardHolder;

	// Token: 0x04000B2A RID: 2858
	[SerializeField]
	private UISystemModule uiSystemModule;
}
