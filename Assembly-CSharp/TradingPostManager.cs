using System;
using DuloGames.UI;
using UnityEngine;

// Token: 0x02000266 RID: 614
public class TradingPostManager : MonoBehaviour
{
	// Token: 0x06000951 RID: 2385 RVA: 0x0002B91C File Offset: 0x00029B1C
	public void ChangeActiveTab(string id)
	{
		string text = id.ToLower();
		uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
		if (num <= 1779026152U)
		{
			if (num <= 862676408U)
			{
				if (num != 30055014U)
				{
					if (num != 790612537U)
					{
						if (num != 862676408U)
						{
							goto IL_34B;
						}
						if (!(text == "skin"))
						{
							goto IL_34B;
						}
						goto IL_34B;
					}
					else
					{
						if (!(text == "skinstore"))
						{
							goto IL_34B;
						}
						goto IL_34B;
					}
				}
				else
				{
					if (!(text == "storage"))
					{
						goto IL_34B;
					}
					goto IL_270;
				}
			}
			else if (num != 1527558748U)
			{
				if (num != 1766558334U)
				{
					if (num != 1779026152U)
					{
						goto IL_34B;
					}
					if (!(text == "playermarket"))
					{
						goto IL_34B;
					}
					goto IL_2B9;
				}
				else
				{
					if (!(text == "pet"))
					{
						goto IL_34B;
					}
					goto IL_302;
				}
			}
			else if (!(text == "gem"))
			{
				goto IL_34B;
			}
		}
		else if (num <= 2895812186U)
		{
			if (num != 2291761999U)
			{
				if (num != 2572487085U)
				{
					if (num != 2895812186U)
					{
						goto IL_34B;
					}
					if (!(text == "buygem"))
					{
						goto IL_34B;
					}
					this.buyGemTab.isOn = true;
					this.marketTab.isOn = false;
					this.skinsStoreTab.isOn = false;
					this.storageTab.isOn = false;
					this.gemStoreTab.isOn = false;
					this.buyPetTab.isOn = false;
					return;
				}
				else if (!(text == "gemstore"))
				{
					goto IL_34B;
				}
			}
			else
			{
				if (!(text == "petstore"))
				{
					goto IL_34B;
				}
				goto IL_302;
			}
		}
		else if (num <= 3172023953U)
		{
			if (num != 3052940913U)
			{
				if (num != 3172023953U)
				{
					goto IL_34B;
				}
				if (!(text == "skins"))
				{
					goto IL_34B;
				}
				goto IL_34B;
			}
			else
			{
				if (!(text == "market"))
				{
					goto IL_34B;
				}
				goto IL_2B9;
			}
		}
		else if (num != 3376437914U)
		{
			if (num != 3471463031U)
			{
				goto IL_34B;
			}
			if (!(text == "pets"))
			{
				goto IL_34B;
			}
			goto IL_302;
		}
		else
		{
			if (!(text == "marketstorage"))
			{
				goto IL_34B;
			}
			goto IL_270;
		}
		this.buyGemTab.isOn = false;
		this.marketTab.isOn = false;
		this.skinsStoreTab.isOn = false;
		this.storageTab.isOn = false;
		this.gemStoreTab.isOn = true;
		this.buyPetTab.isOn = false;
		return;
		IL_270:
		this.buyGemTab.isOn = false;
		this.gemStoreTab.isOn = false;
		this.marketTab.isOn = false;
		this.skinsStoreTab.isOn = false;
		this.storageTab.isOn = true;
		this.buyPetTab.isOn = false;
		return;
		IL_2B9:
		this.buyGemTab.isOn = false;
		this.gemStoreTab.isOn = false;
		this.skinsStoreTab.isOn = false;
		this.storageTab.isOn = false;
		this.marketTab.isOn = true;
		this.buyPetTab.isOn = false;
		return;
		IL_302:
		this.buyGemTab.isOn = false;
		this.gemStoreTab.isOn = false;
		this.storageTab.isOn = false;
		this.marketTab.isOn = false;
		this.skinsStoreTab.isOn = false;
		this.buyPetTab.isOn = true;
		return;
		IL_34B:
		this.buyGemTab.isOn = true;
		this.gemStoreTab.isOn = false;
		this.storageTab.isOn = false;
		this.marketTab.isOn = false;
		this.skinsStoreTab.isOn = false;
		this.buyPetTab.isOn = false;
	}

	// Token: 0x04000AF7 RID: 2807
	[SerializeField]
	private UITab skinsStoreTab;

	// Token: 0x04000AF8 RID: 2808
	[SerializeField]
	private UITab gemStoreTab;

	// Token: 0x04000AF9 RID: 2809
	[SerializeField]
	private UITab marketTab;

	// Token: 0x04000AFA RID: 2810
	[SerializeField]
	private UITab storageTab;

	// Token: 0x04000AFB RID: 2811
	[SerializeField]
	private UITab buyGemTab;

	// Token: 0x04000AFC RID: 2812
	[SerializeField]
	private UITab buyPetTab;
}
