using System;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001F2 RID: 498
public class DefaultTooltipManager : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x06000631 RID: 1585 RVA: 0x0001FA68 File Offset: 0x0001DC68
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x06000632 RID: 1586 RVA: 0x0001FA8C File Offset: 0x0001DC8C
	private void Start()
	{
		if (NetworkServer.active)
		{
			return;
		}
		this.title = LanguageManager.Instance.GetText(this.title);
		this.description = LanguageManager.Instance.GetText(this.description);
	}

	// Token: 0x06000633 RID: 1587 RVA: 0x0001FAC2 File Offset: 0x0001DCC2
	private void LateUpdate()
	{
		if (this.isOver && Time.time - this.overTime > 0.7f)
		{
			this.uiSystemModule.ShowDefaultTooltip(Input.mousePosition, this.icon, this.title, this.description);
		}
	}

	// Token: 0x06000634 RID: 1588 RVA: 0x0001FB01 File Offset: 0x0001DD01
	private void OnEnabled()
	{
		this.isOver = false;
		this.overTime = Time.time;
		this.uiSystemModule.CloseDefaultTooltip();
	}

	// Token: 0x06000635 RID: 1589 RVA: 0x0001FB20 File Offset: 0x0001DD20
	private void OnDisable()
	{
		this.isOver = false;
		this.uiSystemModule.CloseDefaultTooltip();
	}

	// Token: 0x06000636 RID: 1590 RVA: 0x0001FB34 File Offset: 0x0001DD34
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.isOver = true;
		this.overTime = Time.time;
	}

	// Token: 0x06000637 RID: 1591 RVA: 0x0001FB20 File Offset: 0x0001DD20
	public void OnPointerExit(PointerEventData eventData)
	{
		this.isOver = false;
		this.uiSystemModule.CloseDefaultTooltip();
	}

	// Token: 0x04000877 RID: 2167
	[SerializeField]
	private Image icon;

	// Token: 0x04000878 RID: 2168
	[SerializeField]
	private string title;

	// Token: 0x04000879 RID: 2169
	[SerializeField]
	private string description;

	// Token: 0x0400087A RID: 2170
	private bool isOver;

	// Token: 0x0400087B RID: 2171
	private float overTime;

	// Token: 0x0400087C RID: 2172
	private UISystemModule uiSystemModule;
}
