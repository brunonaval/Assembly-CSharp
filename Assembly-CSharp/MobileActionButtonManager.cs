using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200020D RID: 525
public class MobileActionButtonManager : MonoBehaviour
{
	// Token: 0x0600070C RID: 1804 RVA: 0x00022B76 File Offset: 0x00020D76
	private void Start()
	{
		this.buttonIcon.sprite = this.noActionSprite;
	}

	// Token: 0x0600070D RID: 1805 RVA: 0x00022B89 File Offset: 0x00020D89
	private void Update()
	{
		this.HandleMobileNearby();
	}

	// Token: 0x0600070E RID: 1806 RVA: 0x00022B94 File Offset: 0x00020D94
	private void HandleMobileNearby()
	{
		if (!this.uiSystemModule.CreatureModule.IsAlive)
		{
			return;
		}
		int layerMask = 1 << LayerMask.NameToLayer("Action Tile");
		if (Physics2D.OverlapCircle(this.uiSystemModule.PlayerModule.gameObject.transform.position, 0.32f, layerMask) != null)
		{
			this.EnableButtonAndSetSprite(this.actionTileSprite);
			return;
		}
		int layerMask2 = 1 << LayerMask.NameToLayer("Ground Slot");
		if (Physics2D.OverlapCircle(this.uiSystemModule.PlayerModule.gameObject.transform.position, 0.48f, layerMask2) != null)
		{
			this.EnableButtonAndSetSprite(this.collectSprite);
			return;
		}
		int layerMask3 = 1 << LayerMask.NameToLayer("Npc") | 1 << LayerMask.NameToLayer("Combatant");
		if (Physics2D.OverlapCircle(this.uiSystemModule.PlayerModule.gameObject.transform.position, 0.96f, layerMask3) != null)
		{
			this.EnableButtonAndSetSprite(this.talkNpcSprite);
			return;
		}
		int layerMask4 = 1 << LayerMask.NameToLayer("Pet");
		if (Physics2D.OverlapCircle(this.uiSystemModule.PlayerModule.gameObject.transform.position, 0.48f, layerMask4) != null)
		{
			this.EnableButtonAndSetSprite(this.talkNpcSprite);
			return;
		}
		this.DisableButtonAndRemoveSprite();
	}

	// Token: 0x0600070F RID: 1807 RVA: 0x00022D06 File Offset: 0x00020F06
	public void OnClick()
	{
		if (this.CollectNearbyItem())
		{
			return;
		}
		if (this.ExecuteAction())
		{
			return;
		}
		if (this.InteractWithNpc())
		{
			return;
		}
		this.InteractWithPet();
	}

	// Token: 0x06000710 RID: 1808 RVA: 0x00022D2C File Offset: 0x00020F2C
	private bool ExecuteAction()
	{
		int layerMask = 1 << LayerMask.NameToLayer("Action Tile");
		if (Physics2D.OverlapCircle(this.uiSystemModule.PlayerModule.gameObject.transform.position, 0.48f, layerMask) == null)
		{
			return false;
		}
		this.uiSystemModule.PlayerModule.ExecuteMapAction();
		return true;
	}

	// Token: 0x06000711 RID: 1809 RVA: 0x00022D90 File Offset: 0x00020F90
	private bool InteractWithNpc()
	{
		int layerMask = 1 << LayerMask.NameToLayer("Npc") | 1 << LayerMask.NameToLayer("Combatant");
		Collider2D collider2D = Physics2D.OverlapCircle(this.uiSystemModule.PlayerModule.gameObject.transform.position, 0.96f, layerMask);
		if (collider2D == null)
		{
			return false;
		}
		this.uiSystemModule.PlayerModule.CmdNpcHandshake(collider2D.gameObject);
		return true;
	}

	// Token: 0x06000712 RID: 1810 RVA: 0x00022E0C File Offset: 0x0002100C
	private bool InteractWithPet()
	{
		int layerMask = 1 << LayerMask.NameToLayer("Pet");
		Collider2D collider2D = Physics2D.OverlapCircle(this.uiSystemModule.PlayerModule.gameObject.transform.position, 0.96f, layerMask);
		if (collider2D == null || !collider2D.CompareTag("Npc"))
		{
			return false;
		}
		this.uiSystemModule.PlayerModule.CmdNpcHandshake(collider2D.gameObject);
		return true;
	}

	// Token: 0x06000713 RID: 1811 RVA: 0x00022E84 File Offset: 0x00021084
	private bool CollectNearbyItem()
	{
		int layerMask = 1 << LayerMask.NameToLayer("Ground Slot");
		Collider2D collider2D = Physics2D.OverlapCircle(this.uiSystemModule.PlayerModule.gameObject.transform.position, 0.48f, layerMask);
		if (collider2D == null)
		{
			return false;
		}
		if (Time.time - this.collectTime < 0.3f)
		{
			return false;
		}
		this.collectTime = Time.time;
		this.CollectItemFromGround(collider2D.gameObject);
		return true;
	}

	// Token: 0x06000714 RID: 1812 RVA: 0x00022F04 File Offset: 0x00021104
	private void CollectItemFromGround(GameObject groundSlot)
	{
		this.uiSystemModule.InventoryModule.CmdCollectFromGround(groundSlot);
	}

	// Token: 0x06000715 RID: 1813 RVA: 0x00022F17 File Offset: 0x00021117
	private void EnableButtonAndSetSprite(Sprite sprite)
	{
		this.button.interactable = true;
		this.buttonIcon.sprite = sprite;
		this.buttonFrame.enabled = true;
		this.buttonIcon.enabled = true;
	}

	// Token: 0x06000716 RID: 1814 RVA: 0x00022F49 File Offset: 0x00021149
	private void DisableButtonAndRemoveSprite()
	{
		this.buttonIcon.sprite = this.noActionSprite;
		this.buttonFrame.enabled = false;
		this.buttonIcon.enabled = false;
		this.button.interactable = false;
	}

	// Token: 0x04000914 RID: 2324
	[SerializeField]
	private UISystemModule uiSystemModule;

	// Token: 0x04000915 RID: 2325
	[SerializeField]
	private Button button;

	// Token: 0x04000916 RID: 2326
	[SerializeField]
	private Image buttonIcon;

	// Token: 0x04000917 RID: 2327
	[SerializeField]
	private Image buttonFrame;

	// Token: 0x04000918 RID: 2328
	[SerializeField]
	private Sprite collectSprite;

	// Token: 0x04000919 RID: 2329
	[SerializeField]
	private Sprite talkNpcSprite;

	// Token: 0x0400091A RID: 2330
	[SerializeField]
	private Sprite actionTileSprite;

	// Token: 0x0400091B RID: 2331
	[SerializeField]
	private Sprite noActionSprite;

	// Token: 0x0400091C RID: 2332
	private float collectTime;
}
