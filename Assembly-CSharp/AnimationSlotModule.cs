using System;
using Mirror;
using UnityEngine;

// Token: 0x020002A9 RID: 681
public class AnimationSlotModule : MonoBehaviour
{
	// Token: 0x06000AFC RID: 2812 RVA: 0x000325E8 File Offset: 0x000307E8
	private void Awake()
	{
		if (NetworkServer.active)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		this.vocationModule = base.GetComponentInParent<VocationModule>();
		this.equipmentModule = base.GetComponentInParent<EquipmentModule>();
		this.animationControllerModule = base.GetComponentInParent<AnimationControllerModule>();
	}

	// Token: 0x06000AFD RID: 2813 RVA: 0x00032621 File Offset: 0x00030821
	private void Start()
	{
		this.AddVocationSkins();
	}

	// Token: 0x06000AFE RID: 2814 RVA: 0x0003262C File Offset: 0x0003082C
	private void Update()
	{
		if (this.equipmentModule == null)
		{
			return;
		}
		if (!this.equipmentModule.isClient)
		{
			return;
		}
		if (this.equipmentModule.EquippedItems.Count - 1 < (int)this.SlotType)
		{
			return;
		}
		if (Time.time - this.lastUpdateCheckTime <= 0.5f)
		{
			return;
		}
		this.lastUpdateCheckTime = Time.time;
		if (this.SlotType == SlotType.Body)
		{
			bool flag = this.equipmentModule.IsEquipped(SlotType.Skin, ItemType.BodySkin);
			if (flag)
			{
				this.RemoveAnimation();
				return;
			}
			if (base.transform.childCount < 1 & !flag)
			{
				this.AddAnimation(this.GetDefaultVocationBody());
				return;
			}
		}
		Item item = this.equipmentModule.GetItem(this.SlotType);
		if (item.Id == this.equippedItem.Id)
		{
			return;
		}
		this.equippedItem = item;
		if (item.IsDefined)
		{
			this.AddAnimation(item.MetaName);
			return;
		}
		this.AddVocationSkins();
	}

	// Token: 0x06000AFF RID: 2815 RVA: 0x00032720 File Offset: 0x00030920
	private void AddVocationSkins()
	{
		if (this.SlotType == SlotType.Body)
		{
			this.AddAnimation(this.GetDefaultVocationBody());
			return;
		}
		if (this.SlotType == SlotType.Skin)
		{
			if (this.animationControllerModule.UsePremiumSkins)
			{
				this.AddAnimation(this.GetEliteVocationSkin());
				return;
			}
			this.AddAnimation(this.GetDefaultVocationSkin());
			return;
		}
		else
		{
			if (this.SlotType == SlotType.RightHandSkin)
			{
				this.AddAnimation(this.GetDefaultVocationRightHandSkin());
				return;
			}
			if (this.SlotType == SlotType.LeftHandSkin)
			{
				this.AddAnimation(this.GetDefaultVocationLeftHandSkin());
				return;
			}
			return;
		}
	}

	// Token: 0x06000B00 RID: 2816 RVA: 0x000327A4 File Offset: 0x000309A4
	private void AddAnimation(string metaName)
	{
		if (string.IsNullOrEmpty(metaName))
		{
			return;
		}
		GameObject animationPrefab = AssetBundleManager.Instance.GetAnimationPrefab(metaName);
		if (animationPrefab == null)
		{
			return;
		}
		this.RemoveAnimation();
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(animationPrefab);
		gameObject.transform.SetParent(base.gameObject.transform);
		gameObject.transform.position = Vector3.zero;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		gameObject.layer = LayerMask.NameToLayer("AnimationSlot");
	}

	// Token: 0x06000B01 RID: 2817 RVA: 0x00032834 File Offset: 0x00030A34
	private string GetDefaultVocationRightHandSkin()
	{
		switch (this.vocationModule.Vocation)
		{
		case Vocation.Sentinel:
			return "default_bow_skin";
		case Vocation.Warrior:
			return "default_spear_skin";
		case Vocation.Elementor:
			return "default_staff_skin";
		case Vocation.Protector:
			return "default_sword_skin";
		case Vocation.Lyrus:
			return "default_staff_skin";
		default:
			return string.Empty;
		}
	}

	// Token: 0x06000B02 RID: 2818 RVA: 0x0003288E File Offset: 0x00030A8E
	private string GetDefaultVocationLeftHandSkin()
	{
		if (this.vocationModule.Vocation == Vocation.Protector)
		{
			return "default_shield_skin";
		}
		return string.Empty;
	}

	// Token: 0x06000B03 RID: 2819 RVA: 0x000328AC File Offset: 0x00030AAC
	private string GetDefaultVocationSkin()
	{
		switch (this.vocationModule.Vocation)
		{
		case Vocation.Sentinel:
			return "default_sentinel_skin";
		case Vocation.Warrior:
			return "default_warrior_skin";
		case Vocation.Elementor:
			return "default_elementor_skin";
		case Vocation.Protector:
			return "default_protector_skin";
		case Vocation.Lyrus:
			return "default_lyrus_skin";
		default:
			return string.Empty;
		}
	}

	// Token: 0x06000B04 RID: 2820 RVA: 0x00032908 File Offset: 0x00030B08
	private string GetEliteVocationSkin()
	{
		switch (this.vocationModule.Vocation)
		{
		case Vocation.Sentinel:
			return "elite_sentinel_skin";
		case Vocation.Warrior:
			return "elite_warrior_skin";
		case Vocation.Elementor:
			return "elite_elementor_skin";
		case Vocation.Protector:
			return "elite_protector_skin";
		case Vocation.Lyrus:
			return "elite_lyrus_skin";
		default:
			return string.Empty;
		}
	}

	// Token: 0x06000B05 RID: 2821 RVA: 0x00032964 File Offset: 0x00030B64
	private string GetDefaultVocationBody()
	{
		switch (this.vocationModule.Vocation)
		{
		case Vocation.Sentinel:
		case Vocation.Protector:
		case Vocation.Lyrus:
			return "white_body_skin";
		case Vocation.Warrior:
			return "black_body_skin";
		case Vocation.Elementor:
			return "pale_blue_body_skin";
		default:
			return string.Empty;
		}
	}

	// Token: 0x06000B06 RID: 2822 RVA: 0x000329B4 File Offset: 0x00030BB4
	private void RemoveAnimation()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			UnityEngine.Object.Destroy(base.transform.GetChild(i).gameObject);
		}
	}

	// Token: 0x04000C7C RID: 3196
	public SlotType SlotType;

	// Token: 0x04000C7D RID: 3197
	private VocationModule vocationModule;

	// Token: 0x04000C7E RID: 3198
	private EquipmentModule equipmentModule;

	// Token: 0x04000C7F RID: 3199
	private AnimationControllerModule animationControllerModule;

	// Token: 0x04000C80 RID: 3200
	private Item equippedItem;

	// Token: 0x04000C81 RID: 3201
	private float lastUpdateCheckTime;
}
