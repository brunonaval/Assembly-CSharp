using System;
using DuloGames.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000320 RID: 800
public class DragWindowModule : MonoBehaviour, IDragHandler, IEventSystemHandler, IBeginDragHandler, IPointerDownHandler
{
	// Token: 0x06000F3E RID: 3902 RVA: 0x00047B07 File Offset: 0x00045D07
	private void Awake()
	{
		this.uiSystemModule = base.GetComponentInParent<UISystemModule>();
	}

	// Token: 0x06000F3F RID: 3903 RVA: 0x00047B15 File Offset: 0x00045D15
	private void Start()
	{
		this.system = EventSystem.current;
	}

	// Token: 0x06000F40 RID: 3904 RVA: 0x00047B24 File Offset: 0x00045D24
	private void Update()
	{
		if (this.closeWhenPlayerMoves && Time.time - this.lastCheckPlayerPositionTime > this.checkPlayerPositionInterval)
		{
			this.lastCheckPlayerPositionTime = Time.time;
			if (!GlobalUtils.IsClose(this.uiSystemModule.PlayerModule.transform.position, this.playerStartPosition, 3f))
			{
				base.gameObject.SetActive(false);
				return;
			}
		}
		this.ChangeInputWhenTabIsPressed();
	}

	// Token: 0x06000F41 RID: 3905 RVA: 0x00047B9C File Offset: 0x00045D9C
	private void ChangeInputWhenTabIsPressed()
	{
		if (GlobalSettings.IsMobilePlatform)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.Tab) & this.system.currentSelectedGameObject != null)
		{
			Selectable selectable = this.system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnRight();
			if (selectable != null)
			{
				InputField component = selectable.GetComponent<InputField>();
				if (component != null)
				{
					component.OnPointerClick(new PointerEventData(this.system));
				}
				this.system.SetSelectedGameObject(selectable.gameObject, new BaseEventData(this.system));
			}
		}
	}

	// Token: 0x06000F42 RID: 3906 RVA: 0x00047C28 File Offset: 0x00045E28
	private void OnEnable()
	{
		if (this.uiSystemModule == null)
		{
			return;
		}
		this.playerStartPosition = this.uiSystemModule.PlayerModule.transform.position;
		this.uiSystemModule.PlayerModule.ChatFocused = false;
		this.uiSystemModule.PlatformChatHolderManager.DeactivateChat();
		this.uiSystemModule.HasWindowsOpened = true;
		this.uiSystemModule.HideHint();
	}

	// Token: 0x06000F43 RID: 3907 RVA: 0x00047C98 File Offset: 0x00045E98
	private void OnDisable()
	{
		if (this.uiSystemModule == null)
		{
			return;
		}
		UISelectField_List[] componentsInChildren = this.uiSystemModule.GetComponentsInChildren<UISelectField_List>();
		if (componentsInChildren.Length != 0)
		{
			UISelectField_List[] array = componentsInChildren;
			for (int i = 0; i < array.Length; i++)
			{
				UnityEngine.Object.Destroy(array[i].gameObject);
			}
		}
		this.uiSystemModule.HasWindowsOpened = false;
	}

	// Token: 0x06000F44 RID: 3908 RVA: 0x00047CED File Offset: 0x00045EED
	public void OnBeginDrag(PointerEventData eventData)
	{
		if (this.allowDragging)
		{
			this.windowStartPosition = base.transform.position;
			this.mouseStartPosition = Input.mousePosition;
		}
	}

	// Token: 0x06000F45 RID: 3909 RVA: 0x00047D14 File Offset: 0x00045F14
	public void OnDrag(PointerEventData eventData)
	{
		if (this.allowDragging)
		{
			Vector3 b = Input.mousePosition - this.mouseStartPosition;
			Vector3 position = this.windowStartPosition + b;
			base.transform.position = position;
		}
	}

	// Token: 0x06000F46 RID: 3910 RVA: 0x000202BF File Offset: 0x0001E4BF
	public void CloseWindow()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x06000F47 RID: 3911 RVA: 0x00047D53 File Offset: 0x00045F53
	public void OnPointerDown(PointerEventData eventData)
	{
		base.transform.SetAsLastSibling();
	}

	// Token: 0x04000F14 RID: 3860
	private Vector3 windowStartPosition;

	// Token: 0x04000F15 RID: 3861
	private Vector3 mouseStartPosition;

	// Token: 0x04000F16 RID: 3862
	private Vector3 playerStartPosition;

	// Token: 0x04000F17 RID: 3863
	[SerializeField]
	private bool allowDragging;

	// Token: 0x04000F18 RID: 3864
	[SerializeField]
	private bool closeWhenPlayerMoves;

	// Token: 0x04000F19 RID: 3865
	private EventSystem system;

	// Token: 0x04000F1A RID: 3866
	private UISystemModule uiSystemModule;

	// Token: 0x04000F1B RID: 3867
	private readonly float checkPlayerPositionInterval = 1f;

	// Token: 0x04000F1C RID: 3868
	private float lastCheckPlayerPositionTime;
}
