using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001F1 RID: 497
public class DailyRewardSlotManager : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x06000629 RID: 1577 RVA: 0x0001F864 File Offset: 0x0001DA64
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x0600062A RID: 1578 RVA: 0x0001F888 File Offset: 0x0001DA88
	private void OnEnable()
	{
		this.isOver = false;
		this.overTime = Time.time;
		if (this.dailyReward.Item.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x0600062B RID: 1579 RVA: 0x0001F8B9 File Offset: 0x0001DAB9
	private void OnDisable()
	{
		this.isOver = false;
		if (this.dailyReward.Item.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x0600062C RID: 1580 RVA: 0x0001F8E0 File Offset: 0x0001DAE0
	private void Update()
	{
		if (this.isOver && Time.time - this.overTime > 0.7f && this.dailyReward.Item.IsDefined)
		{
			this.uiSystemModule.ShowItemTooltip(Input.mousePosition, this.dailyReward.Item);
		}
	}

	// Token: 0x0600062D RID: 1581 RVA: 0x0001F938 File Offset: 0x0001DB38
	public void SetDailyReward(DailyReward dailyReward, int currentRewardId)
	{
		this.dailyReward = dailyReward;
		this.itemIcon.sprite = dailyReward.Item.Icon;
		this.itemAmount.text = dailyReward.Amount.ToString();
		if (dailyReward.Id < currentRewardId)
		{
			this.itemFrame.color = new Color(0f, 0.5f, 0f, 1f);
			this.itemIcon.color = new Color(0.3f, 0.3f, 0.3f, 1f);
			return;
		}
		if (dailyReward.Id == currentRewardId)
		{
			this.itemFrame.color = new Color(0f, 0.65f, 0.89f, 1f);
			this.itemIcon.color = Color.white;
			return;
		}
		this.itemFrame.color = new Color(0.5f, 0.5f, 0.5f, 1f);
		this.itemIcon.color = new Color(0.3f, 0.3f, 0.3f, 1f);
	}

	// Token: 0x0600062E RID: 1582 RVA: 0x0001FA52 File Offset: 0x0001DC52
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.isOver = true;
		this.overTime = Time.time;
	}

	// Token: 0x0600062F RID: 1583 RVA: 0x0001F8B9 File Offset: 0x0001DAB9
	public void OnPointerExit(PointerEventData eventData)
	{
		this.isOver = false;
		if (this.dailyReward.Item.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x04000870 RID: 2160
	private DailyReward dailyReward;

	// Token: 0x04000871 RID: 2161
	[SerializeField]
	private Image itemIcon;

	// Token: 0x04000872 RID: 2162
	[SerializeField]
	private Image itemFrame;

	// Token: 0x04000873 RID: 2163
	[SerializeField]
	private Text itemAmount;

	// Token: 0x04000874 RID: 2164
	private UISystemModule uiSystemModule;

	// Token: 0x04000875 RID: 2165
	private bool isOver;

	// Token: 0x04000876 RID: 2166
	private float overTime;
}
